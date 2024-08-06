using AutoMapper;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
using CIN.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using System.Linq.Dynamic.Core;
using CIN.Domain.OpeartionsMgt;
using System.Globalization;

namespace CIN.Application.OperationsMgtQuery
{



    #region CreateContract                      //For Project
    public class CreateContract : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public ContractForProjectDto ContractDto { get; set; }
    }

    public class CreateContractHandler : IRequestHandler<CreateContract, int>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public CreateContractHandler(CINDBOneContext context,DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateContract request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                using (var transaction2 = await _contextDMC.Database.BeginTransactionAsync())
                {
                    try
                {
                    Log.Info("----Info CreateContract method start----");



                    var OldPaymentTerms = await _context.TblOpPaymentTermsMapToProjectList.AsNoTracking().Where(e => e.ProjectCode == request.ContractDto.ProjectCode && e.BranchCode == request.ContractDto.BranchCode).ToListAsync();
                    var OldContractTerms = await _context.TblOpContractTermsMapToProjectList.AsNoTracking().Where(e => e.ProjectCode == request.ContractDto.ProjectCode && e.BranchCode == request.ContractDto.BranchCode).ToListAsync();
                    if (OldContractTerms.Count > 0)
                    {
                        _context.RemoveRange(OldContractTerms);
                    }
                    if (OldPaymentTerms.Count > 0)
                    {

                        _context.RemoveRange(OldPaymentTerms);
                    }

                    List<TblOpContractTermsMapToProject> newContractTerms = new List<TblOpContractTermsMapToProject>();
                    foreach (var ct in request.ContractDto.ContractTerms)
                    {
                        TblOpContractTermsMapToProject contTerm = new();

                        contTerm.BranchCode = ct.BranchCode;
                        contTerm.CustomerCode = ct.CustomerCode;
                        contTerm.ProjectCode = ct.ProjectCode;
                        contTerm.ContractTerm = ct.ContractTerm;
                        contTerm.isLiabilityAndInsurance = ct.isLiabilityAndInsurance;
                        contTerm.isTerminationClause = ct.isTerminationClause;

                        contTerm.Created = DateTime.UtcNow;
                        contTerm.CreatedBy = request.User.UserId;
                        newContractTerms.Add(contTerm);
                    }
                    await _context.TblOpContractTermsMapToProjectList.AddRangeAsync(newContractTerms);
                    await _context.SaveChangesAsync();
                    List<TblOpPaymentTermsMapToProject> newPaymentTerms = new List<TblOpPaymentTermsMapToProject>();
                    foreach (var pt in request.ContractDto.PaymentTerms)
                    {
                        TblOpPaymentTermsMapToProject payTerm = new();

                        payTerm.BranchCode = pt.BranchCode;
                        payTerm.CustomerCode = pt.CustomerCode;
                        payTerm.ProjectCode = pt.ProjectCode;
                        payTerm.InstDate = Convert.ToDateTime(pt.InstDate, CultureInfo.InvariantCulture);
                        payTerm.Particular = pt.Particular;

                        //payTerm.MonthStartDate = pt.MonthStartDate;
                        //payTerm.MonthEndDate = pt.MonthEndDate;
                        payTerm.Amount = pt.Amount;
                        payTerm.Created = DateTime.UtcNow;
                        payTerm.CreatedBy = request.User.UserId;
                        newPaymentTerms.Add(payTerm);
                    }
                    await _context.TblOpPaymentTermsMapToProjectList.AddRangeAsync(newPaymentTerms);
                    await _context.SaveChangesAsync();


                    var project = _context.OP_HRM_TEMP_Projects.AsNoTracking().FirstOrDefault(e => e.ProjectCode == request.ContractDto.ProjectCode && e.CustomerCode == request.ContractDto.CustomerCode);
                    project.IsConvrtedToContract = true;
                    _context.OP_HRM_TEMP_Projects.Update(project);
                 _context.SaveChanges();

                        var isProjectExistInDMC = _contextDMC.HRM_DEF_Projects.AsNoTracking().Any(p => p.ProjectCode == project.CustomerCode);
                        if (!isProjectExistInDMC)
                        {
                            HRM_DEF_Project projectDMC = new();
                            // projectDMC.ProjectID = project.Id;
                            projectDMC.ProjectCode = project.CustomerCode;
                            projectDMC.CustomerCode = project.CustomerCode;
                            projectDMC.ProjectName_EN = project.ProjectNameEng;
                            projectDMC.ProjectName_AR = project.ProjectNameArb;
                            projectDMC.ProjectDescription = "Form Operations";
                            projectDMC.IsActive = true;
                            projectDMC.IsSystem = 0;
                            projectDMC.CreatedBy = request.User.UserId;
                            projectDMC.CreatedDate = DateTime.UtcNow;
                            projectDMC.ModifiedBy = request.User.UserId;
                            projectDMC.ProjectSiteID = 0;
                            projectDMC.SiteCode = "N/A";
                            _contextDMC.HRM_DEF_Projects.Add(projectDMC);
                            _contextDMC.SaveChanges();
                        }

                        var enquiries = _context.OprEnquiries.AsNoTracking().Where(e => e.EnquiryNumber == project.ProjectCode).ToList();

                        var enqHead = _context.OprEnquiryHeaders.AsNoTracking().FirstOrDefault(e => e.EnquiryNumber == project.ProjectCode);
                        foreach (var enq in enquiries)
                        {
                                var isSitetExistInDMC = _contextDMC.HRM_DEF_Sites.AsNoTracking().Any(s => s.ProjectCode == project.ProjectCode && s.SiteCode == enq.SiteCode);

                            if (!isSitetExistInDMC)
                            {
                                var HrmProject = _contextDMC.HRM_DEF_Projects.AsNoTracking().FirstOrDefault(p => p.ProjectCode == project.CustomerCode);

                               // var OprSite = _context.OprSites.AsNoTracking().FirstOrDefault(s => s.SiteCode == enq.SiteCode);
                                bool isBranchExist = _contextDMC.HRM_DEF_Branches.AsNoTracking().Any(b => b.BranchCode.Trim() == enqHead.BranchCode); //here added trim() if u get any issue this may be the cause 


                                if (!isBranchExist)
                                {
                                    var oprBranch = _context.CityCodes.FirstOrDefault(e => e.CityCode == enqHead.BranchCode);  //if we take branch i couldn't find arabic name
                                    HRM_DEF_Branch newBranch = new()
                                    {
                                        BranchCode = enqHead.BranchCode,
                                        BranchDescription = "From Operations",
                                        BranchName_AR = oprBranch.CityNameAr,//"N/A",
                                        BranchName_EN = oprBranch.CityName,//"N/A",
                                        IsActive = true,
                                        IsSystem = 0
                                    };

                                    _contextDMC.HRM_DEF_Branches.Add(newBranch);
                                    _contextDMC.SaveChanges();


                                }

                                var HrmBranch = _contextDMC.HRM_DEF_Branches.AsNoTracking().FirstOrDefault(b => b.BranchCode.Trim() == enqHead.BranchCode); //here added trim() if u get any issue this may be the cause 

                                HRM_DEF_Site siteDMC = new();
                                siteDMC.IsSystem = false;
                                siteDMC.SiteCode = enq.SiteCode;
                                // siteDMC.SiteID = _context.OprSites.FirstOrDefault(s=>s.SiteCode==enquiry.SiteCode).Id;
                                siteDMC.ProjectCode = project.CustomerCode;
                                siteDMC.ProjectID = HrmProject.ProjectID;
                                siteDMC.SiteName_EN = _context.OprSites.FirstOrDefault(s => s.SiteCode == enq.SiteCode).SiteName;
                                siteDMC.SiteName_AR = _context.OprSites.FirstOrDefault(s => s.SiteCode == enq.SiteCode).SiteArbName;
                                siteDMC.BranchID = HrmBranch.BranchID;
                                siteDMC.CityCode = HrmBranch.BranchCode;
                                siteDMC.IsActive = true;
                                siteDMC.CreatedBy = request.User.UserId;
                                siteDMC.CreatedDate = DateTime.UtcNow;
                                siteDMC.SiteDescription = "From Operations";
                                _contextDMC.HRM_DEF_Sites.Add(siteDMC);
                                _contextDMC.SaveChanges();

                            }
                        }




                        var projectSites = await _context.TblOpProjectSites.AsNoTracking().Where(e=>e.ProjectCode==request.ContractDto.ProjectCode).ToListAsync();

                        projectSites.ForEach(e=> {
                            e.IsConvrtedToContract = true;
                        
                        });
                        _context.TblOpProjectSites.UpdateRange(projectSites);
                       await _context.SaveChangesAsync();




                        await transaction.CommitAsync();
                        await transaction2.CommitAsync();
                    Log.Info("----Info CreateUpdateUnit method Exit----");
                    return 1;

                }


                catch (Exception ex)
                {
                    Log.Error("Error in CreateContract Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    await transaction.RollbackAsync();
                    await transaction2.RollbackAsync();
                    return 0;
                }
            }
        }
        }
    }

    #endregion


    #region GetContractByProjectCode
    public class GetContractByProjectCode : IRequest<ContractForProjectDto>
    {
        public UserIdentityDto User { get; set; }
        public string ProjectCode { get; set; }
    }

    public class GetContractByProjectCodeHandler : IRequestHandler<GetContractByProjectCode, ContractForProjectDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetContractByProjectCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ContractForProjectDto> Handle(GetContractByProjectCode request, CancellationToken cancellationToken)
        {
            ContractForProjectDto obj = new();
            List<TblOpContractTermsMapToProjectDto>contTerms = await _context.TblOpContractTermsMapToProjectList.AsNoTracking().ProjectTo<TblOpContractTermsMapToProjectDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectCode == request.ProjectCode && e.SiteCode == null).ToListAsync();
            List<TblOpPaymentTermsMapToProjectDto> payTerms = await _context.TblOpPaymentTermsMapToProjectList.AsNoTracking().ProjectTo<TblOpPaymentTermsMapToProjectDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectCode == request.ProjectCode && e.SiteCode == null).ToListAsync();

            obj.ContractTerms = contTerms;
            obj.PaymentTerms = payTerms;
            obj.ProjectCode = request.ProjectCode;




            return obj;
        }
    }

    #endregion


    #region GetContractByProjectAndSiteCode
    public class GetContractByProjectAndSiteCode : IRequest<ContractForProjectDto>
    {
        public UserIdentityDto User { get; set; }
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
    }

    public class GetContractByProjectAndSiteCodeHandler : IRequestHandler<GetContractByProjectAndSiteCode, ContractForProjectDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetContractByProjectAndSiteCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ContractForProjectDto> Handle(GetContractByProjectAndSiteCode request, CancellationToken cancellationToken)
        {
            ContractForProjectDto obj = new();
            List<TblOpContractTermsMapToProjectDto> contTerms = await _context.TblOpContractTermsMapToProjectList.AsNoTracking().ProjectTo<TblOpContractTermsMapToProjectDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectCode == request.ProjectCode && e.SiteCode == request.SiteCode).ToListAsync();
            List<TblOpPaymentTermsMapToProjectDto> payTerms = await _context.TblOpPaymentTermsMapToProjectList.AsNoTracking().ProjectTo<TblOpPaymentTermsMapToProjectDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectCode == request.ProjectCode && e.SiteCode == request.SiteCode).ToListAsync();

            obj.ContractTerms = contTerms;
            obj.PaymentTerms = payTerms;
            obj.ProjectCode = request.ProjectCode;
            obj.SiteCode = request.SiteCode;
            return obj;
        }
    }

    #endregion
    #region CreateSiteContract          //Adendum
    public class CreateSiteContract : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public ContractForProjectDto ContractDto { get; set; }
    }

    public class CreateSiteContractHandler : IRequestHandler<CreateSiteContract, int>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public CreateSiteContractHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateSiteContract request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                using (var transaction2 = await _contextDMC.Database.BeginTransactionAsync())
                {
                    try
                    {
                        Log.Info("----Info CreateContract method start----");



                        var OldPaymentTerms =  _context.TblOpPaymentTermsMapToProjectList.AsNoTracking().Where(e => e.ProjectCode == request.ContractDto.ProjectCode && e.BranchCode == request.ContractDto.BranchCode && e.SiteCode==request.ContractDto.SiteCode).ToList();
                        var OldContractTerms =  _context.TblOpContractTermsMapToProjectList.AsNoTracking().Where(e => e.ProjectCode == request.ContractDto.ProjectCode && e.BranchCode == request.ContractDto.BranchCode && e.SiteCode == request.ContractDto.SiteCode).ToList();
                        if (OldContractTerms.Count > 0)
                        {
                            _context.RemoveRange(OldContractTerms);
                            _context.SaveChanges();
                        }
                        if (OldPaymentTerms.Count > 0)
                        {

                            _context.RemoveRange(OldPaymentTerms);
                            _context.SaveChanges();
                        }

                        List<TblOpContractTermsMapToProject> newContractTerms = new List<TblOpContractTermsMapToProject>();
                        foreach (var ct in request.ContractDto.ContractTerms)
                        {
                            TblOpContractTermsMapToProject contTerm = new();

                            contTerm.BranchCode = ct.BranchCode;
                            contTerm.CustomerCode = ct.CustomerCode;
                            contTerm.ProjectCode = ct.ProjectCode;
                            contTerm.SiteCode = request.ContractDto.SiteCode;
                            contTerm.ContractTerm = ct.ContractTerm;
                            contTerm.isLiabilityAndInsurance = ct.isLiabilityAndInsurance;
                            contTerm.isTerminationClause = ct.isTerminationClause;

                            contTerm.Created = DateTime.UtcNow;
                            contTerm.CreatedBy = request.User.UserId;
                            newContractTerms.Add(contTerm);
                        }
                         _context.TblOpContractTermsMapToProjectList.AddRange(newContractTerms);
                         _context.SaveChanges();
                        List<TblOpPaymentTermsMapToProject> newPaymentTerms = new List<TblOpPaymentTermsMapToProject>();
                        foreach (var pt in request.ContractDto.PaymentTerms)
                        {
                            TblOpPaymentTermsMapToProject payTerm = new();

                            payTerm.BranchCode = pt.BranchCode;
                            payTerm.CustomerCode = pt.CustomerCode;
                            payTerm.ProjectCode = pt.ProjectCode;
                            payTerm.SiteCode = request.ContractDto.SiteCode;
                            payTerm.InstDate = Convert.ToDateTime(pt.InstDate, CultureInfo.InvariantCulture);
                            payTerm.Particular = pt.Particular;

                            //payTerm.MonthStartDate = pt.MonthStartDate;
                            //payTerm.MonthEndDate = pt.MonthEndDate;
                            payTerm.Amount = pt.Amount;
                            payTerm.Created = DateTime.UtcNow;
                            payTerm.CreatedBy = request.User.UserId;
                            newPaymentTerms.Add(payTerm);
                        }
                         _context.TblOpPaymentTermsMapToProjectList.AddRange(newPaymentTerms);
                         _context.SaveChanges();

                       
                        var projectSite = _context.TblOpProjectSites.AsNoTracking().FirstOrDefault(e => e.ProjectCode == request.ContractDto.ProjectCode && e.CustomerCode == request.ContractDto.CustomerCode && e.SiteCode==request.ContractDto.SiteCode);
                        projectSite.IsConvrtedToContract = true;

                        if (projectSite.Id == 0)
                        {
                            transaction.Rollback();
                            transaction2.Rollback();
                            return 0;
                        
                        }
                            _context.TblOpProjectSites.Update(projectSite);
                        _context.SaveChanges();

                        var isProjectExistInDMC = _contextDMC.HRM_DEF_Projects.AsNoTracking().Any(p=>p.CustomerCode == request.ContractDto.CustomerCode || p.ProjectCode == request.ContractDto.CustomerCode);
                        if (!isProjectExistInDMC)
                        {
                            HRM_DEF_Project projectDMC = new();

                            var CustomerData = _context.OprCustomers.AsNoTracking().FirstOrDefault(e=>e.CustCode== projectSite.CustomerCode);
                            if (CustomerData is null)
                            {
                                transaction.Rollback();
                                transaction2.Rollback();
                                return 0;
                            }

                            // projectDMC.ProjectID = project.Id;
                            projectDMC.ProjectCode = projectSite.CustomerCode;
                            projectDMC.CustomerCode = projectSite.CustomerCode;
                            projectDMC.ProjectName_EN = CustomerData.CustName;
                            projectDMC.ProjectName_AR = CustomerData.CustArbName;
                            projectDMC.ProjectDescription = "Form Operations";
                            projectDMC.IsActive = true;
                            projectDMC.IsSystem = 0;
                            projectDMC.CreatedBy = request.User.UserId;
                            projectDMC.CreatedDate = DateTime.UtcNow;
                            projectDMC.ModifiedBy = request.User.UserId;
                            projectDMC.ProjectSiteID = 0;
                            projectDMC.SiteCode = "N/A";
                            _contextDMC.HRM_DEF_Projects.Add(projectDMC);
                            _contextDMC.SaveChanges();
                        }

                        
                      




                            var isSitetExistInDMC = _contextDMC.HRM_DEF_Sites.AsNoTracking().Any(s => s.ProjectCode == projectSite.CustomerCode && s.SiteCode == projectSite.SiteCode);

                            if (!isSitetExistInDMC)
                            {
                                var HrmProject = _contextDMC.HRM_DEF_Projects.AsNoTracking().FirstOrDefault(p => p.ProjectCode == projectSite.CustomerCode);

                                // var OprSite = _context.OprSites.AsNoTracking().FirstOrDefault(s => s.SiteCode == enq.SiteCode);
                                bool isBranchExist = _contextDMC.HRM_DEF_Branches.AsNoTracking().Any(b => b.BranchCode.Trim() == projectSite.BranchCode);  //here added trim() if u get any issue this may be the cause 


                            if (!isBranchExist)
                                {
                                var oprBranch = _context.CityCodes.FirstOrDefault(e => e.CityCode == projectSite.BranchCode);  //if we take branch i couldn't find arabic name

                                HRM_DEF_Branch newBranch = new()
                                    {
                                        BranchCode = projectSite.BranchCode,
                                        BranchDescription = "From Operations",
                                        BranchName_AR = oprBranch.CityNameAr,//"N/A",
                                        BranchName_EN = oprBranch.CityName,//"N/A",
                                        IsActive = true,
                                        CreatedDate=DateTime.UtcNow,
                                        IsSystem = 0
                                    };

                                    _contextDMC.HRM_DEF_Branches.Add(newBranch);
                                    _contextDMC.SaveChanges();


                                }

                                var HrmBranch = _contextDMC.HRM_DEF_Branches.AsNoTracking().FirstOrDefault(b => b.BranchCode.Trim() == projectSite.BranchCode); //here added trim() if u get any issue this may be the cause 

                            HRM_DEF_Site siteDMC = new();
                                siteDMC.IsSystem = false;
                                siteDMC.SiteCode = projectSite.SiteCode;
                                // siteDMC.SiteID = _context.OprSites.FirstOrDefault(s=>s.SiteCode==enquiry.SiteCode).Id;
                                siteDMC.ProjectCode = projectSite.CustomerCode;
                                siteDMC.ProjectID = HrmProject.ProjectID;
                                siteDMC.SiteName_EN = _context.OprSites.FirstOrDefault(s => s.SiteCode == projectSite.SiteCode).SiteName;
                                siteDMC.SiteName_AR = _context.OprSites.FirstOrDefault(s => s.SiteCode == projectSite.SiteCode).SiteArbName;
                                siteDMC.BranchID = HrmBranch.BranchID;
                                siteDMC.CityCode = HrmBranch.BranchCode;
                                siteDMC.IsActive = true;
                                siteDMC.SiteDescription = "From Operations";
                                _contextDMC.HRM_DEF_Sites.Add(siteDMC);
                                _contextDMC.SaveChanges();

                            }
                


                        await transaction2.CommitAsync();
                        await transaction.CommitAsync();
                        Log.Info("----Info CreateUpdateUnit method Exit----");
                        return 1;

                    }


                    catch (Exception ex)
                    {
                        Log.Error("Error in CreateContract Method");
                        Log.Error("Error occured time : " + DateTime.UtcNow);
                        Log.Error("Error message : " + ex.Message);
                        Log.Error("Error StackTrace : " + ex.StackTrace);
                        await transaction.RollbackAsync();
                        await transaction2.RollbackAsync();
                        return 0;
                    }
                }
            }
        }
    }

    #endregion



    #region ApproveContractForm

    public class ApproveContractForm : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public InputApproveContractFormDto Input { get; set; }
    }

    public class ApproveContractFormHandler : IRequestHandler<ApproveContractForm, int>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public ApproveContractFormHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<int> Handle(ApproveContractForm request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                using (var transaction2 = await _contextDMC.Database.BeginTransactionAsync())
                {
                    try
                    {
                        Log.Info("----Info ApproveContractForm method start----");

                        var project = _context.OP_HRM_TEMP_Projects.AsNoTracking().FirstOrDefault(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode);
                        project.IsConvrtedToContract = true;
                        project.ModifiedBy = request.User.UserId;
                        project.ModifiedOn = DateTime.UtcNow;
                        _context.OP_HRM_TEMP_Projects.Update(project);
                        _context.SaveChanges();

                        var isProjectExistInDMC = _contextDMC.HRM_DEF_Projects.AsNoTracking().Any(p => p.ProjectCode == project.CustomerCode);
                        if (!isProjectExistInDMC)
                        {
                            HRM_DEF_Project projectDMC = new();
                            // projectDMC.ProjectID = project.Id;
                            projectDMC.ProjectCode = project.CustomerCode;
                            projectDMC.CustomerCode = project.CustomerCode;
                            projectDMC.ProjectName_EN = project.ProjectNameEng;
                            projectDMC.ProjectName_AR = project.ProjectNameArb;
                            projectDMC.ProjectDescription = "Form Operations";
                            projectDMC.IsActive = true;
                            projectDMC.IsSystem = 0;
                            projectDMC.CreatedBy = request.User.UserId;
                            projectDMC.CreatedDate = DateTime.UtcNow;
                            projectDMC.ModifiedBy = request.User.UserId;
                            projectDMC.ProjectSiteID = 0;
                            projectDMC.SiteCode = "N/A";
                            _contextDMC.HRM_DEF_Projects.Add(projectDMC);
                            _contextDMC.SaveChanges();
                        }

                        //var enquiries = _context.OprEnquiries.AsNoTracking().Where(e => e.EnquiryNumber == project.ProjectCode).ToList();

                        //var enqHead = _context.OprEnquiryHeaders.AsNoTracking().FirstOrDefault(e => e.EnquiryNumber == project.ProjectCode);


                        var projectSites = request.Input.SiteCode is null ? await _context.TblOpProjectSites.Where(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode).ToListAsync() :
                            await _context.TblOpProjectSites.Where(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode && e.SiteCode == request.Input.SiteCode).ToListAsync();

                        foreach (var site in projectSites)
                        {
                            var isSitetExistInDMC = _contextDMC.HRM_DEF_Sites.AsNoTracking().Any(s => s.ProjectCode == project.CustomerCode && s.SiteCode == site.SiteCode);

                            if (!isSitetExistInDMC)
                            {
                                var HrmProject = _contextDMC.HRM_DEF_Projects.AsNoTracking().FirstOrDefault(p => p.ProjectCode == project.CustomerCode);

                                // var OprSite = _context.OprSites.AsNoTracking().FirstOrDefault(s => s.SiteCode == enq.SiteCode);
                                bool isBranchExist = _contextDMC.HRM_DEF_Branches.AsNoTracking().Any(b => b.BranchCode.Trim() == site.BranchCode);//here added trim() if u get any issue this may be the cause 


                                if (!isBranchExist)
                                {
                                    var oprBranch = _context.CityCodes.FirstOrDefault(e => e.CityCode == site.BranchCode);  //if we take branch i couldn't find arabic name

                                    HRM_DEF_Branch newBranch = new()
                                    {
                                        BranchCode = site.BranchCode,
                                        BranchDescription = "From Operations",
                                        BranchName_AR =oprBranch.CityNameAr,// "N/A",
                                        BranchName_EN = oprBranch.CityName,//"N/A",
                                        IsActive = true,
                                        IsSystem = 0
                                    };

                                    _contextDMC.HRM_DEF_Branches.Add(newBranch);
                                    _contextDMC.SaveChanges();


                                }

                                var HrmBranch = _contextDMC.HRM_DEF_Branches.AsNoTracking().FirstOrDefault(b => b.BranchCode.Trim() == site.BranchCode); //here added trim() if u get any issue this may be the cause 

                                HRM_DEF_Site siteDMC = new();
                                siteDMC.IsSystem = false;
                                siteDMC.SiteCode = site.SiteCode;
                                // siteDMC.SiteID = _context.OprSites.FirstOrDefault(s=>s.SiteCode==enquiry.SiteCode).Id;
                                siteDMC.ProjectCode = project.CustomerCode;
                                siteDMC.ProjectID = HrmProject.ProjectID;
                                siteDMC.SiteName_EN = _context.OprSites.FirstOrDefault(s => s.SiteCode == site.SiteCode).SiteName;
                                siteDMC.SiteName_AR = _context.OprSites.FirstOrDefault(s => s.SiteCode == site.SiteCode).SiteArbName;
                                siteDMC.BranchID = HrmBranch.BranchID;
                                siteDMC.CityCode = HrmBranch.BranchCode;
                                siteDMC.IsActive = true;
                                siteDMC.CreatedBy = request.User.UserId;
                                siteDMC.CreatedDate = DateTime.UtcNow;
                                siteDMC.SiteDescription = "From Operations";
                                _contextDMC.HRM_DEF_Sites.Add(siteDMC);
                                _contextDMC.SaveChanges();

                            }
                        }




                       // var projectSites = await _context.TblOpProjectSites.AsNoTracking().Where(e => e.ProjectCode == request.Input.ProjectCode).ToListAsync();

                        projectSites.ForEach(e => {
                            e.IsConvrtedToContract = true;
                            e.ModifiedBy = request.User.UserId;
                            e.ModifiedOn = DateTime.UtcNow;
                        });
                        _context.TblOpProjectSites.UpdateRange(projectSites);
                        await _context.SaveChangesAsync();




                        var contractFormHead = await _context.TblOpContractFormHeads.FirstOrDefaultAsync(e=>e.Id==request.Input.ContractFormHeadId);
                        if (contractFormHead is null)
                        {
                            await transaction.RollbackAsync();
                            await transaction2.RollbackAsync();
                            return -1;
                        }

                        contractFormHead.IsApproved = true;
                        contractFormHead.ApprovedBy = request.User.UserId;
                        _context.TblOpContractFormHeads.Update(contractFormHead);
                        await _context.SaveChangesAsync();


                        await transaction.CommitAsync();
                        await transaction2.CommitAsync();
                        Log.Info("----Info CreateUpdateUnit method Exit----");
                        return 1;

                    }


                    catch (Exception ex)
                    {
                        Log.Error("Error in ApproveContractForm Method");
                        Log.Error("Error occured time : " + DateTime.UtcNow);
                        Log.Error("Error message : " + ex.Message);
                        Log.Error("Error StackTrace : " + ex.StackTrace);
                        await transaction.RollbackAsync();
                        await transaction2.RollbackAsync();
                        return 0;
                    }
                }
            }
        }
    }


    #endregion
}
