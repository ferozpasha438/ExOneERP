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



    #region CreateExistingProject
    public class CreateExistingProject : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public ServicesEnquiryFormDto ServicesEnquiryFormDto { get; set; }
    }

    public class CreateExistingProjectHandler : IRequestHandler<CreateExistingProject, int>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public CreateExistingProjectHandler(CINDBOneContext context,DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateExistingProject request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateExistingProject method start----");




                    var obj = request.ServicesEnquiryFormDto;


                    TblSndDefServiceEnquiryHeader ServicesEnquiryHeader = new();
                    if (obj.Id > 0)
                        ServicesEnquiryHeader = await _context.OprEnquiryHeaders.FirstOrDefaultAsync(e => e.Id == obj.Id);

                    else
                    {
                        ServicesEnquiryHeader.BranchCode = request.User.BranchCode;
                      
                        var EnqForm = await _context.OprEnquiryHeaders.AsNoTracking().OrderByDescending(e => e.Id).FirstOrDefaultAsync();

                        if (EnqForm is not null)
                        {
                            ServicesEnquiryHeader.EnquiryNumber =(EnqForm.Id + 1).ToString().PadLeft(10,'0');
                        }
                        else
                            ServicesEnquiryHeader.EnquiryNumber = "0000000001";

                        if (_context.OprEnquiryHeaders.Any(x => x.EnquiryNumber == ServicesEnquiryHeader.EnquiryNumber))
                        {
                            return -1;
                        }


                        //  ServicesEnquiryHeader.EnquiryNumber = obj.EnquiryNumber.ToUpper();

                    }

                    //ServicesEnquiryForm.Id = obj.Id;
                    // ServicesEnquiryForm.ModifiedOn = obj.ModifiedOn;
                    // ServicesEnquiryForm.CreatedOn = obj.CreatedOn;
                    ServicesEnquiryHeader.IsActive = obj.IsActive;
                    // ServicesEnquiryForm.Id = obj.Id;

                    ServicesEnquiryHeader.CustomerCode = obj.CustomerCode;
                    //ServicesEnquiryHeader.DateOfEnquiry = obj.DateOfEnquiry;
                    ServicesEnquiryHeader.DateOfEnquiry = Convert.ToDateTime(obj.DateOfEnquiry, CultureInfo.InvariantCulture); 
                    ServicesEnquiryHeader.EstimateClosingDate = Convert.ToDateTime(obj.EstimateClosingDate, CultureInfo.InvariantCulture);
                    ServicesEnquiryHeader.UserName = obj.UserName;
                    ServicesEnquiryHeader.TotalEstPrice = obj.TotalEstPrice;
                    ServicesEnquiryHeader.Remarks = obj.Remarks;
                    ServicesEnquiryHeader.StusEnquiryHead = obj.StusEnquiryHead;
                    ServicesEnquiryHeader.BranchCode = obj.BranchCode;


                    ServicesEnquiryHeader.StusEnquiryHead = "Converted_To_Project";
                    ServicesEnquiryHeader.IsConvertedToProject = true;

                    if (obj.Id > 0)
                    {
                        ServicesEnquiryHeader.ModifiedOn = DateTime.Now;
                        _context.OprEnquiryHeaders.Update(ServicesEnquiryHeader);
                    }
                    else
                    {


                        ServicesEnquiryHeader.CreatedOn = DateTime.Now;
                        await _context.OprEnquiryHeaders.AddAsync(ServicesEnquiryHeader);
                    }
                    await _context.SaveChangesAsync();

                    
                    Log.Info("----Info CreateExistingProject method Exit----");
                   


                    OP_HRM_TEMP_Project Project = new();

                    TblSndDefServiceEnquiryHeader enquiryHead = new();
                    Log.Info("----Info ConvertEnquiryToProject method start----");

                    enquiryHead = _context.OprEnquiryHeaders.AsNoTracking().FirstOrDefault(e => e.EnquiryNumber == ServicesEnquiryHeader.EnquiryNumber);
                    Project.ProjectCode = ServicesEnquiryHeader.EnquiryNumber.ToUpper();
                    Project.StartDate = ServicesEnquiryHeader.DateOfEnquiry;
                    Project.ProjectNameArb = _context.OprCustomers.FirstOrDefault(p=>p.CustCode== ServicesEnquiryHeader.CustomerCode).CustArbName;
                    Project.ProjectNameEng = _context.OprCustomers.FirstOrDefault(p => p.CustCode == ServicesEnquiryHeader.CustomerCode).CustName;
                    Project.EndDate = ServicesEnquiryHeader.EstimateClosingDate;
                    Project.CustomerCode = ServicesEnquiryHeader.CustomerCode;
                    Project.BranchCode = ServicesEnquiryHeader.BranchCode;
                    Project.IsEstimationCompleted = false;
                    Project.IsExpenceOverheadsAssigned = false;
                    Project.IsLogisticsAssigned = false;
                    Project.IsResourcesAssigned = false;
                    Project.IsMaterialAssigned = false;
                    Project.IsConvertedToProposal = false;
                    Project.IsShiftsAssigned = false;
                    Project.IsSkillSetsMapped = false;
                    Project.IsConvrtedToContract = true;
                    Project.IsActive = true;
                     _context.Add(Project);
                    _context.SaveChanges();


                    if (request.ServicesEnquiryFormDto.EnquiriesList.Count() > 0)
                    {

                        using (var transaction2 = await _contextDMC.Database.BeginTransactionAsync())
                        {

                            var oldEnquiriesList = await _context.OprEnquiries.Where(e => e.EnquiryNumber == ServicesEnquiryHeader.EnquiryNumber).ToListAsync();
                            _context.OprEnquiries.RemoveRange(oldEnquiriesList);
                            var enqHead = _context.OprEnquiryHeaders.AsNoTracking().FirstOrDefault(e => e.EnquiryNumber == ServicesEnquiryHeader.EnquiryNumber);
                            List<TblSndDefServiceEnquiries> enquiriesList = new();

                            foreach (var enq in request.ServicesEnquiryFormDto.EnquiriesList)
                            {

                                TblSndDefServiceEnquiries enquiry = new()
                                {
                                    EnquiryID = enq.EnquiryID,
                                    EnquiryNumber = ServicesEnquiryHeader.EnquiryNumber,
                                    SiteCode = enq.SiteCode,
                                    ServiceCode = enq.ServiceCode,
                                    UnitCode = enq.UnitCode,
                                    ServiceQuantity = enq.ServiceQuantity,
                                    UnitQuantity = enq.UnitQuantity,
                                    EstimatedPrice = enq.EstimatedPrice,
                                    PricePerUnit = enq.PricePerUnit,
                                    StatusEnquiry = enq.StatusEnquiry,
                                    //ModifiedOn = enq.ModifiedOn,
                                    IsActive = true,
                                    SurveyorCode = "",
                                };
                                if (enquiry.EnquiryID > 0)
                                {
                                    enquiry.ModifiedOn = DateTime.Now;
                                    _context.OprEnquiries.Update(enquiry);
                                }
                                else
                                {

                                    enquiry.CreatedOn = DateTime.Now;
                                    await _context.OprEnquiries.AddAsync(enquiry);

                                    var isProjectExistInDMC = _contextDMC.HRM_DEF_Projects.AsNoTracking().Any(p => p.CustomerCode == Project.CustomerCode || p.ProjectCode==Project.CustomerCode);
                                    if (!isProjectExistInDMC)
                                    {
                                        HRM_DEF_Project projectDMC = new();
                                        //projectDMC.ProjectID = Project.Id;
                                        projectDMC.CustomerCode = Project.CustomerCode;
                                        projectDMC.ProjectCode = Project.CustomerCode;
                                        projectDMC.ProjectName_EN = Project.ProjectNameEng;
                                        projectDMC.ProjectName_AR = Project.ProjectNameArb;
                                        projectDMC.ProjectDescription = "From Operations";
                                        projectDMC.IsActive = true;
                                        projectDMC.IsSystem = 0;
                                        projectDMC.CreatedBy = request.User.UserId;
                                        projectDMC.CreatedDate = DateTime.UtcNow;
                                        projectDMC.ModifiedBy = request.User.UserId;
                                        projectDMC.ProjectSiteID = 0;
                                        projectDMC.SiteCode = enquiry.SiteCode;
                                        _contextDMC.HRM_DEF_Projects.Add(projectDMC);
                                        _contextDMC.SaveChanges();
                                    }
                                    var isSiteExistInDMC = _contextDMC.HRM_DEF_Sites.AsNoTracking().Any(s => s.ProjectCode == Project.CustomerCode && s.SiteCode== enquiry.SiteCode);

                                    if (!isSiteExistInDMC)
                                    {
                                        var HrmProject = _contextDMC.HRM_DEF_Projects.AsNoTracking().FirstOrDefault(p => p.ProjectCode == Project.CustomerCode || p.CustomerCode== Project.CustomerCode);

                                        //var OprSite = _context.OprSites.AsNoTracking().FirstOrDefault(s=>s.SiteCode==enquiry.SiteCode);

                                        bool isBranchExist =  _contextDMC.HRM_DEF_Branches.AsNoTracking().Any(b => b.BranchCode == ServicesEnquiryHeader.BranchCode);


                                        if (!isBranchExist)
                                        {
                                            HRM_DEF_Branch newBranch = new() {
                                                BranchCode = ServicesEnquiryHeader.BranchCode,
                                                BranchDescription = "From Operations",
                                                BranchName_AR = "N/A",
                                                BranchName_EN = "N/A",
                                                IsActive = true,
                                                IsSystem=0
                                            };

                                            _contextDMC.HRM_DEF_Branches.Add(newBranch);
                                            _contextDMC.SaveChanges();


                                        }

                                        var HrmBranch = _contextDMC.HRM_DEF_Branches.AsNoTracking().FirstOrDefault(b => b.BranchCode == ServicesEnquiryHeader.BranchCode);

                                        HRM_DEF_Site siteDMC = new();
                                        siteDMC.IsSystem = false;
                                        siteDMC.SiteCode= enquiry.SiteCode;
                                       // siteDMC.SiteID = _context.OprSites.FirstOrDefault(s=>s.SiteCode==enquiry.SiteCode).Id;
                                        siteDMC.ProjectCode = Project.CustomerCode;
                                        siteDMC.ProjectID = HrmProject.ProjectID;
                                        siteDMC.SiteName_EN = _context.OprSites.FirstOrDefault(s => s.SiteCode == enquiry.SiteCode).SiteName;
                                        siteDMC.SiteName_AR = _context.OprSites.FirstOrDefault(s => s.SiteCode == enquiry.SiteCode).SiteArbName;
                                        siteDMC.BranchID = HrmBranch.BranchID;
                                        siteDMC.CityCode = HrmBranch.BranchCode;
                                        siteDMC.IsActive = true;
                                        siteDMC.SiteDescription = "From Operations";
                                        _contextDMC.HRM_DEF_Sites.Add(siteDMC);
                                        _contextDMC.SaveChanges();

                                    }
                                }

                                enquiriesList.Add(enquiry);
                            }
                            await _context.OprEnquiries.AddRangeAsync(enquiriesList);
                            await _context.SaveChangesAsync();
                            await transaction2.CommitAsync();

                        }
                    }
                    else
                    {
                        await transaction.RollbackAsync();

                        return 0;

                    }
                    await transaction.CommitAsync();
                    
                    return Project.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    Log.Error("Error in CreateExistingProject Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;

                }
            }    
        }
        
    }

    #endregion


}
