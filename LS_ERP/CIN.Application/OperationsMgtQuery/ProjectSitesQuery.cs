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





    #region GetProjectsPagedList

    public class GetProjectSitesPagedList : IRequest<PaginatedList<TblOpProjectSites_PaginationDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetProjectSitesPagedListHandler : IRequestHandler<GetProjectSitesPagedList, PaginatedList<TblOpProjectSites_PaginationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetProjectSitesPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblOpProjectSites_PaginationDto>> Handle(GetProjectSitesPagedList request, CancellationToken cancellationToken)
        {
            try
            {

                Log.Info("----Info GetProjectSitesPagedList method start----");
                bool isAdmin = _context.SystemLogins.AsNoTracking().FirstOrDefault(e => e.Id == request.User.UserId).LoginId == "Admin" ? true : false;
                var roasters =  _context.TblOpMonthlyRoasterForSites.AsNoTracking();
                var oprAuths = _context.TblOpAuthoritiesList.AsNoTracking();
                var oprApprvls = _context.TblOprTrnApprovalsList.AsNoTracking();
                var search = request.Input.Query;
                var list = await _context.TblOpProjectSites.AsNoTracking().Where(p => p.ProjectCode == search)
                   .OrderBy(request.Input.OrderBy).Select(d => new TblOpProjectSites_PaginationDto
                   {
                       Id = d.Id,
                       ProjectCode = d.ProjectCode,
                       SiteCode = d.SiteCode,
                       CustomerCode = d.CustomerCode,
                       ProjectNameEng = d.ProjectNameEng,
                       ProjectNameArb = d.ProjectNameArb,
                       ModifiedBy = d.ModifiedBy,
                       CreatedBy = d.CreatedBy,
                       StartDate = d.StartDate,
                       // StartDate = _context.TblOpMonthlyRoasterForSites.AsNoTracking().OrderBy(e=>e.MonthStartDate).FirstOrDefault(e=>e.SiteCode==d.SiteCode&&e.ProjectCode==d.ProjectCode).MonthStartDate??d.StartDate,
                     OldestRoasterStartDate = _context.TblOpMonthlyRoasterForSites.AsNoTracking().OrderByDescending(e=>e.MonthStartDate).FirstOrDefault(e=>e.SiteCode==d.SiteCode&&e.ProjectCode==d.ProjectCode).MonthStartDate??d.StartDate.Value,
                       EndDate = d.EndDate,
                       IsResourcesAssigned = d.IsResourcesAssigned,
                       IsMaterialAssigned = d.IsMaterialAssigned,
                       IsLogisticsAssigned = d.IsLogisticsAssigned,
                       IsShiftsAssigned = d.IsShiftsAssigned,
                       IsExpenceOverheadsAssigned = d.IsExpenceOverheadsAssigned,
                       IsEstimationCompleted = d.IsEstimationCompleted,
                       IsSkillSetsMapped = d.IsSkillSetsMapped,
                       IsConvertedToProposal = d.IsConvertedToProposal,
                       IsConvrtedToContract = d.IsConvrtedToContract,
                       BranchCode = d.BranchCode,




                       IsAdendum = d.IsAdendum,
                       IsCancelled = d.IsCancelled,
                       IsClosed = d.IsClosed,
                       IsInActive = d.IsInActive,
                       IsInProgress = DateTime.UtcNow <= d.EndDate,// d.IsInProgress,
                       CanExtendProject= DateTime.UtcNow <= d.EndDate.Value.AddDays(31) ,
                       IsSuspended = d.IsSuspended,
                       CreatedOn = d.CreatedOn,
                       ModifiedOn = d.ModifiedOn,



                       HasAuthority = oprAuths.Any(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode) || isAdmin,
                       ApprovedUser = oprApprvls.Any(e => e.AppAuth == request.User.UserId && e.ServiceType == "ADNDM" && e.IsApproved && e.ServiceCode == d.ProjectCode + "/" + d.SiteCode),
                      //IsAddendumEstimationApproved
                       IsApproved = oprApprvls.Where(e => e.ServiceType == "ADNDM" && e.ServiceCode == d.ProjectCode + "/" + d.SiteCode/* && e.BranchCode == request.User.BranchCode*/ && e.IsApproved).Count()>0,
                       IsEstimationForProjectApproved = oprApprvls.Where(e => e.ServiceType == "EST" && e.ServiceCode == d.ProjectCode /*&& e.BranchCode == d.BranchCode*/ && e.IsApproved).Count()>0,
                       IsAdmin=isAdmin,

                       FileUrl = d.FileUrl,
                       FileUploadBy = d.FileUploadBy,





                       Authorities = isAdmin ? new TblOpAuthorities
                       {
                           CanApproveEnquiry = true,
                           CanApproveSurvey = true,
                           CanConvertEnqToProject = true,
                           CanCreateRoaster = true,
                           CanEditRoaster = true,
                           CanApproveProposal = true,
                           CanModifyEstimation = true,
                           CanApproveContract = true,
                           CanApproveEstimation = true,
                           IsFinalAuthority = true

                       } : oprAuths.Any(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode)
                       ? oprAuths.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode) : new TblOpAuthorities
                       {
                           CanApproveEnquiry = false,
                           CanApproveSurvey = false,
                           CanConvertEnqToProject = false,
                           CanCreateRoaster = false,
                           CanEditRoaster = false,
                           CanApproveProposal = false,
                           CanModifyEstimation = false,
                           CanApproveContract = false,
                           CanApproveEstimation = false,
                           IsFinalAuthority = false

                       }
                   }).Where(e => e.ProjectCode == search && e.HasAuthority)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);





                return list;
            }

            catch (Exception ex)
            {
                Log.Error("Error in GetProjectSitesPagedList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }
    #endregion

    //#region CreateUpdateProject
    //public class CreateProject : IRequest<int>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public OP_HRM_TEMP_ProjectDto ProjectDto { get; set; }
    //}

    //public class CreateProjectHandler : IRequestHandler<CreateProject, int>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;

    //    public CreateProjectHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }

    //    public async Task<int> Handle(CreateProject request, CancellationToken cancellationToken)
    //    {
    //        try
    //        {
    //            Log.Info("----Info CreateUpdateProject method start----");



    //            var obj = request.ProjectDto;


    //            OP_HRM_TEMP_Project Project = new();

    //            if (obj.Id > 0)
    //                Project = await _context.OP_HRM_TEMP_Projects.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
    //            else
    //            {
    //                if (_context.OP_HRM_TEMP_Projects.Any(x => x.ProjectCode == obj.ProjectCode))
    //                {
    //                    return -1;
    //                }
    //                Project.ProjectCode = obj.ProjectCode.ToUpper();
    //            }

    //            Project.IsActive = obj.IsActive;
    //            Project.ProjectNameEng = obj.ProjectNameEng;
    //            Project.ProjectNameArb = obj.ProjectNameArb;
    //            Project.IsEstimationCompleted = false;
    //            Project.IsExpenceOverheadsAssigned = false;
    //            Project.IsLogisticsAssigned = false;
    //            Project.IsResourcesAssigned = false;
    //            Project.IsMaterialAssigned = false;
    //            Project.IsConvertedToProposal = false;
    //            Project.IsShiftsAssigned = false;
    //            Project.IsSkillSetsMapped = false;
    //            Project.IsConvrtedToContract = false;
    //            Project.BranchCode = request.ProjectDto.BranchCode;

    //            if (obj.Id > 0)
    //            {
    //                Project.ProjectCode = obj.ProjectCode;
    //                Project.ModifiedOn = DateTime.Now;
    //                Project.ModifiedBy = request.User.UserId;
    //                _context.OP_HRM_TEMP_Projects.Update(Project);
    //            }
    //            else
    //            {

    //                Project.CreatedOn = DateTime.Now;
    //                Project.CreatedBy = request.User.UserId;
    //                await _context.OP_HRM_TEMP_Projects.AddAsync(Project);
    //            }
    //            await _context.SaveChangesAsync();
    //            Log.Info("----Info CreateUpdateProject method Exit----");
    //            return Project.Id;
    //        }
    //        catch (Exception ex)
    //        {
    //            Log.Error("Error in CreateUpdateProject Method");
    //            Log.Error("Error occured time : " + DateTime.UtcNow);
    //            Log.Error("Error message : " + ex.Message);
    //            Log.Error("Error StackTrace : " + ex.StackTrace);
    //            return 0;
    //        }
    //    }
    //}

    //#endregion

    #region GetProjectSiteByProjectAndSiteCode
    public class GetProjectSiteByProjectAndSiteCode : IRequest<TblOpProjectSitesDto>
    {
        public UserIdentityDto User { get; set; }
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
    }

    public class GetProjectSiteByProjectAndSiteCodeHandler : IRequestHandler<GetProjectSiteByProjectAndSiteCode, TblOpProjectSitesDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetProjectSiteByProjectAndSiteCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpProjectSitesDto> Handle(GetProjectSiteByProjectAndSiteCode request, CancellationToken cancellationToken)
        {

            var Project = await _context.TblOpProjectSites.AsNoTracking().ProjectTo<TblOpProjectSitesDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.ProjectCode == request.ProjectCode && e.SiteCode == request.SiteCode);

            return Project is not null? Project:new() { Id=0};
        }
    }

    #endregion

    //#region GetProjectById
    //public class GetProjectById : IRequest<OP_HRM_TEMP_ProjectDto>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public int Id { get; set; }
    //}

    //public class GetProjectByIdHandler : IRequestHandler<GetProjectById, OP_HRM_TEMP_ProjectDto>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;
    //    public GetProjectByIdHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }
    //    public async Task<OP_HRM_TEMP_ProjectDto> Handle(GetProjectById request, CancellationToken cancellationToken)
    //    {

    //        var Project = await _context.OP_HRM_TEMP_Projects.AsNoTracking().ProjectTo<OP_HRM_TEMP_ProjectDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);


    //        return Project;
    //    }
    //}

    //#endregion

    #region GetSelectProjectSitesList

    public class GetSelectProjectSitesList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectProjectSitesListHandler : IRequestHandler<GetSelectProjectSitesList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectProjectSitesListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectProjectSitesList request, CancellationToken cancellationToken)
        {

            var list = await _context.TblOpProjectSites.AsNoTracking()
              .Select(e => new CustomSelectListItem { Text = e.ProjectNameEng, Value = e.ProjectCode, TextTwo = e.ProjectNameArb })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion
    #region GetSelectProjectSitesListByCustomerCode

    public class GetSelectProjectSitesListByCustomerCode : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string CustomerCode { get; set; }
    }

    public class GetSelectProjectSitesListByCustomerCodeHandler : IRequestHandler<GetSelectProjectSitesListByCustomerCode, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectProjectSitesListByCustomerCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectProjectSitesListByCustomerCode request, CancellationToken cancellationToken)
        {

            var list = await _context.TblOpProjectSites.AsNoTracking().Where(s => s.CustomerCode == request.CustomerCode)
              .Select(e => new CustomSelectListItem { Text = e.ProjectNameEng, Value = e.ProjectCode, TextTwo = e.ProjectNameArb })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    //#region DeleteProject
    //public class DeleteProject : IRequest<int>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public int Id { get; set; }
    //}

    //public class DeleteProjectQueryHandler : IRequestHandler<DeleteProject, int>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;

    //    public DeleteProjectQueryHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }

    //    public async Task<int> Handle(DeleteProject request, CancellationToken cancellationToken)
    //    {
    //        try
    //        {
    //            Log.Info("----Info DeleteProject start----");

    //            if (request.Id > 0)
    //            {
    //                var Project = await _context.OP_HRM_TEMP_Projects.FirstOrDefaultAsync(e => e.Id == request.Id);
    //                _context.Remove(Project);

    //                await _context.SaveChangesAsync();

    //                return request.Id;
    //            }
    //            return 0;
    //        }
    //        catch (Exception ex)
    //        {

    //            Log.Error("Error in DeleteProject");
    //            Log.Error("Error occured time : " + DateTime.UtcNow);
    //            Log.Error("Error message : " + ex.Message);
    //            Log.Error("Error StackTrace : " + ex.StackTrace);
    //            return 0;
    //        }

    //    }
    //}

    //#endregion


    #region ConvertEnquiryToProjectSites
    public class ConvertEnquiryToProjectSites : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public ConvertCustToProjectSitesDto ProjectSitesDto { get; set; }
    }

    public class ConvertEnquiryToProjectSitesHandler : IRequestHandler<ConvertEnquiryToProjectSites, int>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public ConvertEnquiryToProjectSitesHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<int> Handle(ConvertEnquiryToProjectSites request, CancellationToken cancellationToken)
        {
         
                
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
            {
                    if (request.ProjectSitesDto.ProjectSites.Count<=0)
                    {
                        return 0;
                    
                    }
                    DateTime Sd = Convert.ToDateTime(request.ProjectSitesDto.ProjectSites[0].StartDate,CultureInfo.InvariantCulture);
                    DateTime Ed = Convert.ToDateTime(request.ProjectSitesDto.ProjectSites[0].EndDate,CultureInfo.InvariantCulture);

                    foreach (var ps in request.ProjectSitesDto.ProjectSites)
                    {
                        DateTime sd = Convert.ToDateTime(ps.StartDate, CultureInfo.InvariantCulture);
                        DateTime ed = Convert.ToDateTime(ps.EndDate, CultureInfo.InvariantCulture);
                        if (sd < Sd)
                        {
                            Sd = sd;
                        }
                        if (ed > Ed)
                        {
                            Sd = ed;
                        }

                    
                    }



                    Log.Info("----Info ConvertEnquiryToProjectSites method start----");

                    bool isProjectExist = await _context.OP_HRM_TEMP_Projects.AsNoTracking().AnyAsync(e=>e.ProjectCode== request.ProjectSitesDto.ProjectSites[0].ProjectCode);
                   var customerData =_context.OprCustomers.FirstOrDefault(e => e.CustCode == request.ProjectSitesDto.ProjectSites[0].CustomerCode);
                    if (!isProjectExist)
                    {

                        string branch = _context.OprSites.AsNoTracking().FirstOrDefault(e => e.SiteCode == request.ProjectSitesDto.ProjectSites[0].SiteCode).SiteCityCode;

                        OP_HRM_TEMP_Project Project = new();
                        Project.ProjectCode = request.ProjectSitesDto.ProjectSites[0].ProjectCode.ToUpper();
                        Project.StartDate = Sd;                 //least start date
                        Project.ProjectNameArb = customerData.CustArbName;
                        Project.ProjectNameEng = customerData.CustName;
                        Project.EndDate = Ed;                   //highest end date
                        Project.CustomerCode = request.ProjectSitesDto.ProjectSites[0].CustomerCode;
                        Project.BranchCode = branch;   // request.ProjectSitesDto.ProjectSites[0].BranchCode;
                        Project.IsEstimationCompleted = false;
                        Project.IsExpenceOverheadsAssigned = false;
                        Project.IsLogisticsAssigned = false;
                        Project.IsResourcesAssigned = false;
                        Project.IsMaterialAssigned = false;
                        Project.IsConvertedToProposal = false;
                        Project.IsShiftsAssigned = false;
                        Project.IsSkillSetsMapped = false;
                        Project.IsConvrtedToContract = false;
                        Project.IsActive = true;
                        await _context.OP_HRM_TEMP_Projects.AddAsync(Project);
                        await _context.SaveChangesAsync();

                    }

                    for (int i=0;i<request.ProjectSitesDto.ProjectSites.Count;i++)
                    {
                        string branch = _context.OprSites.AsNoTracking().FirstOrDefault(e => e.SiteCode == request.ProjectSitesDto.ProjectSites[i].SiteCode).SiteCityCode;
                        var SiteData = _context.OprSites.AsNoTracking().FirstOrDefault(e => e.SiteCode == request.ProjectSitesDto.ProjectSites[i].SiteCode);
                        bool isProjectSiteExist = await _context.TblOpProjectSites.AsNoTracking().AnyAsync(e=>e.ProjectCode== request.ProjectSitesDto.ProjectSites[i].ProjectCode &&
                    e.SiteCode== request.ProjectSitesDto.ProjectSites[i].SiteCode);
                        if (!isProjectSiteExist)
                        {
                            TblOpProjectSites ProjectSite = new();
                            ProjectSite.ProjectCode = request.ProjectSitesDto.ProjectSites[i].ProjectCode.ToUpper();
                            ProjectSite.SiteCode = request.ProjectSitesDto.ProjectSites[i].SiteCode.ToUpper();
                            ProjectSite.StartDate = Convert.ToDateTime(request.ProjectSitesDto.ProjectSites[i].StartDate, CultureInfo.InvariantCulture);
                            ProjectSite.ProjectNameArb = SiteData.SiteArbName;
                            ProjectSite.ProjectNameEng = SiteData.SiteName;
                            ProjectSite.EndDate = Convert.ToDateTime(request.ProjectSitesDto.ProjectSites[i].EndDate, CultureInfo.InvariantCulture);
                            ProjectSite.CustomerCode = request.ProjectSitesDto.ProjectSites[i].CustomerCode;
                            ProjectSite.BranchCode = branch;//request.ProjectSitesDto.ProjectSites[i].BranchCode;
                            ProjectSite.IsEstimationCompleted = false;
                            ProjectSite.IsExpenceOverheadsAssigned = false;
                            ProjectSite.IsLogisticsAssigned = false;
                            ProjectSite.IsResourcesAssigned = false;
                            ProjectSite.IsMaterialAssigned = false;
                            ProjectSite.IsConvertedToProposal = false;
                            ProjectSite.IsShiftsAssigned = false;
                            ProjectSite.IsSkillSetsMapped = false;
                            ProjectSite.IsConvrtedToContract = false;
                            ProjectSite.IsActive = true;
                            ProjectSite.IsAdendum = isProjectExist;
                            ProjectSite.CreatedOn = DateTime.Now;
                            ProjectSite.CreatedBy = request.User.UserId;
                            ProjectSite.SiteWorkingHours = request.ProjectSitesDto.ProjectSites[i].SiteWorkingHours;
                            _context.TblOpProjectSites.Add(ProjectSite);
                             _context.SaveChanges();

                        }

                    }

                    TblSndDefServiceEnquiryHeader enquiryHead = new();
                    enquiryHead = await _context.OprEnquiryHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.EnquiryNumber == request.ProjectSitesDto.ProjectSites[0].ProjectCode);
                    enquiryHead.StusEnquiryHead = "Converted_To_Project";
                    enquiryHead.ModifiedOn =DateTime.UtcNow;
                    enquiryHead.IsConvertedToProject = true;
                    _context.Update(enquiryHead);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return 1;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in ConvertEnquiryToProjectSites Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }

        }
    }

    #endregion






    #region GetSelecSitesList

    public class GetSelectSitesList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectSitesListHandler : IRequestHandler<GetSelectSitesList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectSitesListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectSitesList request, CancellationToken cancellationToken)
        {

            var sites = await _context.TblOpProjectSites.AsNoTracking().ToListAsync();
            var sitesInMaster = await _context.OprSites.AsNoTracking().ToListAsync();
            var resList = sites.Join(sitesInMaster, m => m.SiteCode, s => s.SiteCode, (m, s) => new CustomSelectListItem
            {
                Text = s.SiteName,
                Value = m.SiteCode,
                TextTwo=m.ProjectNameArb
            }).ToList();

            return resList;
        }
    }

    #endregion
    #region GetSelectSitesListByCustomerCode

    public class GetSelectSitesListByCustomerCode : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string CustomerCode { get; set; }
    }

    public class GetSelectSitesListByCustomerCodeHandler : IRequestHandler<GetSelectSitesListByCustomerCode, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectSitesListByCustomerCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectSitesListByCustomerCode request, CancellationToken cancellationToken)
        {

            var projSites = await _context.TblOpProjectSites.AsNoTracking().Where(e=>e.CustomerCode==request.CustomerCode).ToListAsync();
            var sites = await _context.OprSites.AsNoTracking().Where(s => projSites.Contains(_context.TblOpProjectSites.FirstOrDefault(e => e.SiteCode == s.SiteCode)))
              .Select(e => new CustomSelectListItem
              {
                  Text = e.SiteName,
                  Value = e.SiteCode,
                  TextTwo = e.SiteArbName
              })
                 .ToListAsync();
            return sites;
        }
    }

    #endregion

    #region GetSelectSitesList2

    public class GetSelecSitesList2 : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelecSitesList2Handler : IRequestHandler<GetSelecSitesList2, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelecSitesList2Handler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelecSitesList2 request, CancellationToken cancellationToken)
        {

            var sites = await _context.TblOpProjectSites.AsNoTracking().ToListAsync();
            var sitesInMaster = await _context.OprSites.AsNoTracking().ToListAsync();
            var resList = sites.Join(sitesInMaster, p => p.SiteCode, s => s.SiteCode, (p, s) => new CustomSelectListItem
            {
               Text=p.SiteCode+"/"+s.SiteName+"/"+s.SiteArbName,
               Value=p.SiteCode
            }).ToList();

            return resList;
        }
    }

    #endregion
    
    #region GetSelectSitesListByProjectCode

    public class GetSelectSitesListByProjectCode : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string ProjectCode { get; set; }
    }

    public class GetSelectSitesListByProjectCodeHandler : IRequestHandler<GetSelectSitesListByProjectCode, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectSitesListByProjectCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectSitesListByProjectCode request, CancellationToken cancellationToken)
        {

            var sites = await _context.TblOpProjectSites.Where(e=>e.ProjectCode==request.ProjectCode).AsNoTracking().ToListAsync();
            var sitesInMaster = await _context.OprSites.AsNoTracking().ToListAsync();
            var resList = sites.Join(sitesInMaster, p => p.SiteCode, s => s.SiteCode, (p, s) => new CustomSelectListItem
            {
               Text=p.SiteCode+"/"+s.SiteName+"/"+s.SiteArbName,
               Value=p.SiteCode
            }).ToList();

            return resList;
        }
    }

    #endregion



    #region GetSelectSiteListWhicAreNotConvertedAsProjectByEnquiryNumber

    public class GetSelectSiteListWhicAreNotConvertedAsProjectByEnquiryNumber : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string EnquiryNumber { get; set; }
    }

    public class GetSelectSiteListWhicAreNotConvertedAsProjectByEnquiryNumberHandler : IRequestHandler<GetSelectSiteListWhicAreNotConvertedAsProjectByEnquiryNumber, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectSiteListWhicAreNotConvertedAsProjectByEnquiryNumberHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectSiteListWhicAreNotConvertedAsProjectByEnquiryNumber request, CancellationToken cancellationToken)
        {
            try {
                var CustomerCode =  _context.OprEnquiryHeaders.FirstOrDefault(e=>e.EnquiryNumber==request.EnquiryNumber).CustomerCode;

                var SitesForCustomer = await _context.OprSites.Where(e => e.CustomerCode == CustomerCode).ToListAsync();

                var projSites =await _context.TblOpProjectSites.Where(e => e.ProjectCode == request.EnquiryNumber).ToListAsync();
                var SitesNotConvertedAsProject = SitesForCustomer.Where(e => !projSites.Any(s => s.SiteCode == e.SiteCode)).ToList();

                List<CustomSelectListItem> sites = new();
                foreach (var e in SitesNotConvertedAsProject)
                {
                    var site = _context.OprSites.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                    sites.Add(new CustomSelectListItem
                    {
                        Text = site.SiteName,
                        TextTwo = site.SiteArbName,
                        Value = site.SiteCode,

                    });

                }
                return sites;

            }
            catch (Exception e) {
                return null;
            }
        
        }
    }

    #endregion

    #region GetRecentAttendanceDateForProjectSite

    public class GetRecentAttendanceDateForProjectSite : IRequest<DateTime>
    {
        public UserIdentityDto User { get; set; }
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
    }

    public class GetRecentAttendanceDateForProjectSiteHandler : IRequestHandler<GetRecentAttendanceDateForProjectSite, DateTime>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetRecentAttendanceDateForProjectSiteHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<DateTime> Handle(GetRecentAttendanceDateForProjectSite request, CancellationToken cancellationToken)
        {
            var obj = await _context.EmployeeAttendance.OrderByDescending(e => e.AttnDate).FirstOrDefaultAsync(e => e.ProjectCode == request.ProjectCode && e.SiteCode == request.SiteCode);
            return obj is null ? Convert.ToDateTime(new DateTime(), CultureInfo.InvariantCulture) : Convert.ToDateTime(obj.AttnDate, CultureInfo.InvariantCulture);
        }
    }

    #endregion


    






    #region GetProjectSiteCodes
    public class ProjectSiteCodesDto
    {
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
    }
    public class GetProjectSiteCodes : IRequest<List<ProjectSiteCodesDto>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetProjectSiteCodesHandler : IRequestHandler<GetProjectSiteCodes, List<ProjectSiteCodesDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetProjectSiteCodesHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<ProjectSiteCodesDto>> Handle(GetProjectSiteCodes request, CancellationToken cancellationToken)
        {
            var projectSites = _context.TblOpProjectSites.Select(e => new ProjectSiteCodesDto { ProjectCode = e.ProjectCode, SiteCode = e.SiteCode }).Distinct().ToList();
            return projectSites;
        }
    }

    #endregion
}
