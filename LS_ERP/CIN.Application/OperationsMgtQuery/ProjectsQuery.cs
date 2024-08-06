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
using System.IO;

namespace CIN.Application.OperationsMgtQuery
{





    #region GetProjectsPagedList

    public class GetProjectsPagedList : IRequest<PaginatedList<OP_HRM_TEMP_Project_PaginationDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetProjectsPagedListHandler : IRequestHandler<GetProjectsPagedList, PaginatedList<OP_HRM_TEMP_Project_PaginationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetProjectsPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<OP_HRM_TEMP_Project_PaginationDto>> Handle(GetProjectsPagedList request, CancellationToken cancellationToken)
        {
            try
            {
                bool isAdmin = _context.SystemLogins.AsNoTracking().FirstOrDefault(e => e.Id == request.User.UserId).LoginId == "Admin" ? true : false;

                Log.Info("----Info GetProjectsPagedList method start----");
                var oprAuths = _context.TblOpAuthoritiesList.AsNoTracking();
                var oprApprvls = _context.TblOprTrnApprovalsList.AsNoTracking();
                var search = request.Input.Query;
                var Sites = _context.TblOpProjectSites.AsNoTracking();
                var AuthSites = Sites.Join(oprAuths, s => s.BranchCode, a => a.BranchCode, (s, a) => new
                {
                    s.ProjectCode,
                    s.CustomerCode,
                    s.BranchCode,
                    a.AppAuth,
                    a.CanApprovePvReq,
                    s.SiteCode,
                    a.CanAddSurveyorToEnquiry,
                    a.CanApproveContract,
                    a.CanApproveEnquiry,
                    a.CanApproveEstimation,
                    a.CanApproveProposal,
                    a.CanApproveSurvey,
                    a.CanConvertEnqToProject,
                    a.CanConvertEstimationToProposal,
                    a.CanConvertProposalToContract,
                    a.CanCreateRoaster,
                    a.CanEditEnquiry,
                    a.CanEditPvReq,
                    a.CanModifyEstimation,
                    a.CanEditRoaster,
                    a.IsFinalAuthority

                }).AsNoTracking();




                var list = await _context.OP_HRM_TEMP_Projects.AsNoTracking()
                   .OrderBy(request.Input.OrderBy).Select(d => new OP_HRM_TEMP_Project_PaginationDto
                   {
                       Id = d.Id,
                       ProjectCode = d.ProjectCode,
                       CustomerCode = d.CustomerCode,
                       ProjectNameEng = d.ProjectNameEng,
                       ProjectNameArb = d.ProjectNameArb,
                       ModifiedBy = d.ModifiedBy,
                       CreatedBy = d.CreatedBy,
                       StartDate = d.StartDate,
                       EndDate = d.EndDate,
                       IsResourcesAssigned = !Sites.Any(e => e.ProjectCode == d.ProjectCode && e.CustomerCode == d.CustomerCode && !e.IsResourcesAssigned && !e.IsAdendum),
                       IsMaterialAssigned = !Sites.Any(e => e.ProjectCode == d.ProjectCode && e.CustomerCode == d.CustomerCode && !e.IsMaterialAssigned && !e.IsAdendum),
                       IsLogisticsAssigned = !Sites.Any(e => e.ProjectCode == d.ProjectCode && e.CustomerCode == d.CustomerCode && !e.IsLogisticsAssigned && !e.IsAdendum),
                       IsShiftsAssigned = !Sites.Any(e => e.ProjectCode == d.ProjectCode && e.CustomerCode == d.CustomerCode && !e.IsShiftsAssigned && !e.IsAdendum),
                       IsExpenceOverheadsAssigned = !Sites.Any(e => e.ProjectCode == d.ProjectCode && e.CustomerCode == d.CustomerCode && !e.IsExpenceOverheadsAssigned && !e.IsAdendum),
                       IsEstimationCompleted = !Sites.Any(e => e.ProjectCode == d.ProjectCode && e.CustomerCode == d.CustomerCode && !e.IsEstimationCompleted && !e.IsAdendum),
                       IsSkillSetsMapped = !Sites.Any(e => e.ProjectCode == d.ProjectCode && e.CustomerCode == d.CustomerCode && !e.IsSkillSetsMapped && !e.IsAdendum),
                       IsConvertedToProposal = !Sites.Any(e => e.ProjectCode == d.ProjectCode && e.CustomerCode == d.CustomerCode && !e.IsConvertedToProposal && !e.IsAdendum),
                       IsConvrtedToContract = !Sites.Any(e => e.ProjectCode == d.ProjectCode && e.CustomerCode == d.CustomerCode && !e.IsConvrtedToContract && !e.IsAdendum),
                       BranchCode = d.BranchCode,
                       IsAdmin=isAdmin,


                       HasAuthority = AuthSites.Any(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode) || isAdmin,
                       ApprovedUser = oprApprvls.Any(e => e.AppAuth == request.User.UserId && e.ServiceType == "EST" && e.IsApproved && e.ServiceCode == d.ProjectCode),
                      // IsApproved = oprAuths.Where(e => e.BranchCode == d.BranchCode && e.CanApproveEstimation == true).Select(e => new { AppAuth = e.AppAuth }).GroupBy(e => e.AppAuth).Count() <= oprApprvls.Where(e => e.ServiceType == "EST" && e.ServiceCode == d.ProjectCode && e.BranchCode == d.BranchCode && e.IsApproved).Count(),
                       IsApproved = oprApprvls.Where(e => e.ServiceType == "EST" && e.ServiceCode == d.ProjectCode && /*e.BranchCode == d.BranchCode &&*/ e.IsApproved).Count()>0,
                    
                       FileUrl=d.FileUrl,
                       FileUploadBy=d.FileUploadBy,

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

                       } :
                       AuthSites.Any(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode)
                       ?
                       new TblOpAuthorities
                       {
                           CanApproveEnquiry = AuthSites.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode).CanApproveEnquiry,
                           CanApproveSurvey = AuthSites.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode).CanApproveSurvey,
                           CanConvertEnqToProject = AuthSites.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode).CanConvertEnqToProject,
                           CanCreateRoaster = AuthSites.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode).CanCreateRoaster,
                           CanEditRoaster = AuthSites.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode).CanEditRoaster,
                           CanApproveProposal = AuthSites.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode).CanApproveProposal,
                           CanModifyEstimation = AuthSites.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode).CanModifyEstimation,
                           CanApproveContract = AuthSites.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode).CanApproveContract,
                           CanApproveEstimation = AuthSites.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode).CanApproveEstimation,
                           IsFinalAuthority = AuthSites.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode).IsFinalAuthority

                       }
                       : new TblOpAuthorities
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
                   }).Where(e => //e.CompanyId == request.CompanyId &&
                                (e.ProjectCode.Contains(search) ||
                                e.ProjectNameEng.Contains(search) ||
                                e.ProjectNameArb.Contains(search) ||
                                 e.CustomerCode.Contains(search) ||
                                 e.BranchCode.Contains(search) ||
                                 search == "" || search == null
                                 ) && e.HasAuthority)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);





                return list;
            }

            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateProject Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }
    #endregion

    #region CreateUpdateProject
    public class CreateProject : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public OP_HRM_TEMP_ProjectDto ProjectDto { get; set; }
    }

    public class CreateProjectHandler : IRequestHandler<CreateProject, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateProjectHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateProject request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateProject method start----");



                var obj = request.ProjectDto;


                OP_HRM_TEMP_Project Project = new();

                if (obj.Id > 0)
                    Project = await _context.OP_HRM_TEMP_Projects.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);
                else
                {
                    if (_context.OP_HRM_TEMP_Projects.Any(x => x.ProjectCode == obj.ProjectCode))
                    {
                        return -1;
                    }
                    Project.ProjectCode = obj.ProjectCode.ToUpper();
                }

                Project.IsActive = obj.IsActive;
                Project.ProjectNameEng = obj.ProjectNameEng;
                Project.ProjectNameArb = obj.ProjectNameArb;
                Project.IsEstimationCompleted = false;
                Project.IsExpenceOverheadsAssigned = false;
                Project.IsLogisticsAssigned = false;
                Project.IsResourcesAssigned = false;
                Project.IsMaterialAssigned = false;
                Project.IsConvertedToProposal = false;
                Project.IsShiftsAssigned = false;
                Project.IsSkillSetsMapped = false;
                Project.IsConvrtedToContract = false;
                Project.BranchCode = request.ProjectDto.BranchCode;

                if (obj.Id > 0)
                {
                    Project.ProjectCode = obj.ProjectCode;
                    Project.ModifiedOn = DateTime.Now;
                    Project.ModifiedBy = request.User.UserId;
                    _context.OP_HRM_TEMP_Projects.Update(Project);
                }
                else
                {
                    
                    Project.CreatedOn = DateTime.Now;
                    Project.CreatedBy = request.User.UserId;
                    await _context.OP_HRM_TEMP_Projects.AddAsync(Project);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateProject method Exit----");
                return Project.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateProject Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetProjectByProjectCode
    public class GetProjectByProjectCode : IRequest<OP_HRM_TEMP_ProjectDto>
    {
        public UserIdentityDto User { get; set; }
        public string ProjectCode { get; set; }
    }

    public class GetProjectByProjectCodeHandler : IRequestHandler<GetProjectByProjectCode, OP_HRM_TEMP_ProjectDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetProjectByProjectCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<OP_HRM_TEMP_ProjectDto> Handle(GetProjectByProjectCode request, CancellationToken cancellationToken)
        {
        
            var Project = await _context.OP_HRM_TEMP_Projects.AsNoTracking().ProjectTo<OP_HRM_TEMP_ProjectDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.ProjectCode == request.ProjectCode);
          
            return Project;
        }
    }

    #endregion

    #region GetProjectById
    public class GetProjectById : IRequest<OP_HRM_TEMP_ProjectDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetProjectByIdHandler : IRequestHandler<GetProjectById, OP_HRM_TEMP_ProjectDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetProjectByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<OP_HRM_TEMP_ProjectDto> Handle(GetProjectById request, CancellationToken cancellationToken)
        {
         
            var Project = await _context.OP_HRM_TEMP_Projects.AsNoTracking().ProjectTo<OP_HRM_TEMP_ProjectDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            

            return Project;
        }
    }

    #endregion

    #region GetSelectProjectList

    public class GetSelectProjectList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectProjectListHandler : IRequestHandler<GetSelectProjectList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectProjectListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectProjectList request, CancellationToken cancellationToken)
        {

            var list = await _context.OP_HRM_TEMP_Projects.AsNoTracking()
              .Select(e => new CustomSelectListItem { Text = e.ProjectNameEng, Value = e.ProjectCode, TextTwo = e.ProjectNameArb })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region GetSelectProjectList2  //text contains all data

    public class GetSelectProjectList2 : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectProjectList2Handler : IRequestHandler<GetSelectProjectList2, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectProjectList2Handler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectProjectList2 request, CancellationToken cancellationToken)
        {

            var list = await _context.OP_HRM_TEMP_Projects.AsNoTracking()
              .Select(e => new CustomSelectListItem { Text = e.ProjectCode+"/"+e.ProjectNameEng+"/"+ e.ProjectNameArb, Value = e.ProjectCode, TextTwo = e.ProjectNameArb })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region GetSelectProjectListByCustomerCode

    public class GetSelectProjectListByCustomerCode : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string  CustomerCode { get; set; }
    }

    public class GetSelectProjectListByCustomerCodeHandler : IRequestHandler<GetSelectProjectListByCustomerCode, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectProjectListByCustomerCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectProjectListByCustomerCode request, CancellationToken cancellationToken)
        {

            var list = await _context.OP_HRM_TEMP_Projects.AsNoTracking().Where(s =>s.CustomerCode==request.CustomerCode)
              .Select(e => new CustomSelectListItem { Text = e.ProjectNameEng, Value = e.ProjectCode, TextTwo = e.ProjectNameArb })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region DeleteProject
    public class DeleteProject : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteProjectQueryHandler : IRequestHandler<DeleteProject, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteProjectQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteProject request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteProject start----");

                if (request.Id > 0)
                {
                    var Project = await _context.OP_HRM_TEMP_Projects.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(Project);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteProject");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion


    #region ConvertEnquiryToProject
    public class ConvertEnquiryToProject : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public ConvertCustToProjectDto ProjectDto { get; set; }
    }

    public class ConvertEnquiryToProjectHandler : IRequestHandler<ConvertEnquiryToProject, int>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public ConvertEnquiryToProjectHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<int> Handle(ConvertEnquiryToProject request, CancellationToken cancellationToken)
        {

            bool isAdendum = await _context.OP_HRM_TEMP_Projects.AsNoTracking().AnyAsync(e => e.ProjectCode == request.ProjectDto.ProjectCode);
            var enquiries = _context.OprEnquiries.AsNoTracking().Where(e => e.EnquiryNumber == request.ProjectDto.EnquiryNumber).ToList().GroupBy(s => s.SiteCode);

            //using (var transaction2 = await _contextDMC.Database.BeginTransactionAsync())
            //{

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {

                        foreach (var group in enquiries)
                        {

                            if (!(await _context.TblOpProjectSites.AsNoTracking().AnyAsync(e => e.ProjectCode == request.ProjectDto.EnquiryNumber && e.SiteCode == group.Key)))
                            {
                        
                            var siteData=_context.OprSites.AsNoTracking().FirstOrDefault(e => e.SiteCode == group.Key);

                            TblOpProjectSites projectSite = new()
                                {
                                    ProjectCode = request.ProjectDto.ProjectCode.ToUpper(),
                                    SiteCode = group.Key,
                                    StartDate = Convert.ToDateTime(request.ProjectDto.StartDate, CultureInfo.InvariantCulture),
                                    ProjectNameArb = siteData.SiteName,
                                    ProjectNameEng = siteData.SiteArbName,
                                    EndDate = Convert.ToDateTime(request.ProjectDto.EndDate, CultureInfo.InvariantCulture),
                                    CustomerCode = request.ProjectDto.CustomerCode,
                                    BranchCode = siteData.SiteCityCode, //request.ProjectDto.BranchCode, new asignment is cityCode of site is branch code in HRM 
                                    IsEstimationCompleted = false,
                                    IsExpenceOverheadsAssigned = false,
                                    IsLogisticsAssigned = false,
                                    IsResourcesAssigned = false,
                                    IsMaterialAssigned = false,
                                    IsConvertedToProposal = false,
                                    IsShiftsAssigned = false,
                                    IsSkillSetsMapped = false,
                                    IsConvrtedToContract = false,
                                    IsAdendum = isAdendum,
                                    IsInActive=true,
                                    IsInProgress=true,
                                    IsCancelled=false,
                                    IsClosed=false,
                                    IsSuspended=false,
                                    CreatedBy=request.User.UserId,
                                    CreatedOn=DateTime.UtcNow,
                                    IsActive=true,
                                   
                                };
                                await _context.TblOpProjectSites.AddAsync(projectSite);
                                await _context.SaveChangesAsync();
                            }

                        }






                        TblSndDefServiceEnquiryHeader enquiryHead = new();


                        Log.Info("----Info ConvertEnquiryToProject method start----");

                        enquiryHead = await _context.OprEnquiryHeaders.AsNoTracking().FirstOrDefaultAsync(e => e.EnquiryNumber == request.ProjectDto.EnquiryNumber);
                        enquiryHead.StusEnquiryHead = "Converted_To_Project";
                        enquiryHead.IsConvertedToProject = true;
                        _context.Update(enquiryHead);
                        await _context.SaveChangesAsync();


                        if (!isAdendum)
                        {

                            OP_HRM_TEMP_Project Project = new();

                            Project.ProjectCode = request.ProjectDto.ProjectCode.ToUpper();
                            Project.StartDate = Convert.ToDateTime(request.ProjectDto.StartDate, CultureInfo.InvariantCulture);
                            Project.ProjectNameArb = request.ProjectDto.ProjectNameArb;
                            Project.ProjectNameEng = request.ProjectDto.ProjectNameEng;
                            Project.EndDate = Convert.ToDateTime(request.ProjectDto.EndDate, CultureInfo.InvariantCulture);
                            Project.CustomerCode = request.ProjectDto.CustomerCode;
                            Project.BranchCode = request.ProjectDto.BranchCode;
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
                            Project.CreatedBy = request.User.UserId;
                        Project.CreatedOn = DateTime.UtcNow;
                            await _context.OP_HRM_TEMP_Projects.AddAsync(Project);
                            await _context.SaveChangesAsync();
                        }





                     //   await transaction2.CommitAsync();
                        Log.Info("----Info ConvertEnquiryToProject method Exit----");

                        await transaction.CommitAsync();
                        return 1;
                    }

                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                      //  await transaction2.RollbackAsync();
                        Log.Error("Error in ConvertEnquiryToProject Method");
                        Log.Error("Error occured time : " + DateTime.UtcNow);
                        Log.Error("Error message : " + ex.Message);
                        Log.Error("Error StackTrace : " + ex.StackTrace);
                        return 0;
                    }
               // }
            }
        }
    
        }




    #endregion


    #region UploadContractForm
    public class UploadContractForm : IRequest<CreateUpdateResultDto>
    {
        public UserIdentityDto User { get; set; }
        public InputUploadContractFormForProjectSite dto { get; set; }
    }

    public class UploadContractFormHandler : IRequestHandler<UploadContractForm, CreateUpdateResultDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public UploadContractFormHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreateUpdateResultDto> Handle(UploadContractForm request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {

                try
                {
                    Log.Info("----Info UploadContractForm method start----");
                    var obj = request.dto;


                    string FileName = "CF"+obj.CustomerCode+"_"+obj.ProjectCode;
                    if (!string.IsNullOrEmpty(obj.FileName))
                    {
                        if (obj.FileIForm != null && obj.FileIForm.Length > 0)
                        {
                            var guid = Guid.NewGuid().ToString();
                            guid = $"{guid}_{FileName}{ Path.GetExtension(obj.FileIForm.FileName)}";
                            obj.FileName += guid;
                            var filePath = Path.Combine(obj.WebRoot, guid);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                obj.FileIForm.CopyTo(stream);
                            }



                            if (obj.IsAdendum)
                            {
                                var ProjectSite = await _context.TblOpProjectSites.AsNoTracking().SingleOrDefaultAsync(e => e.Id == obj.Id);
                                if (ProjectSite is null)
                                {
                                    return new() { IsSuccess = false, ErrorMsg = "Invalid request Number" };
                                }
                                ProjectSite.FileUrl = obj.FileName;
                                ProjectSite.FileUploadBy = request.User.UserId;

                                _context.TblOpProjectSites.Update(ProjectSite);
                                await _context.SaveChangesAsync();


                            }
                            else
                            {
                                var Project = await _context.OP_HRM_TEMP_Projects.AsNoTracking().SingleOrDefaultAsync(e => e.Id == obj.Id);
                                if (Project is null)
                                {
                                    return new() { IsSuccess = false, ErrorMsg = "Invalid request Number" };
                                }
                                Project.FileUrl = obj.FileName;
                                Project.FileUploadBy = request.User.UserId;

                                _context.OP_HRM_TEMP_Projects.Update(Project);
                                await _context.SaveChangesAsync();
                            }


                        }

                    }
                    else
                    {

                        return new() { IsSuccess = false, ErrorMsg = "File is Empty" };
                    }

                    await transaction.CommitAsync();
                    return new() { IsSuccess = true };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in UploadContractForm Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return new() { IsSuccess = false, ErrorId = 0, ErrorMsg = "Something went wrong" };
                }
            }
        }
    }

    #endregion





    #region GetProjectsPagedListWithFilter

    public class GetProjectsPagedListWithFilter : IRequest<PaginatedList<OP_HRM_TEMP_Project_PaginationDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
        public OprFilter FilterInput { get; set; }
    }

    public class GetProjectsPagedListWithFilterHandler : IRequestHandler<GetProjectsPagedListWithFilter, PaginatedList<OP_HRM_TEMP_Project_PaginationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetProjectsPagedListWithFilterHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<OP_HRM_TEMP_Project_PaginationDto>> Handle(GetProjectsPagedListWithFilter request, CancellationToken cancellationToken)
        {
            try
            {
                bool isAdmin = _context.SystemLogins.AsNoTracking().FirstOrDefault(e => e.Id == request.User.UserId).LoginId == "Admin" ? true : false;

                Log.Info("----Info GetProjectsPagedListWithFilter method start----");
                var oprAuths = _context.TblOpAuthoritiesList.AsNoTracking();
                var oprApprvls = _context.TblOprTrnApprovalsList.AsNoTracking();
                var search = request.Input.Query;
                var Sites = _context.TblOpProjectSites.AsNoTracking();
                var AuthSites = Sites.Join(oprAuths, s => s.BranchCode, a => a.BranchCode, (s, a) => new
                {
                    s.ProjectCode,
                    s.CustomerCode,
                    s.BranchCode,
                    a.AppAuth,
                    a.CanApprovePvReq,
                    s.SiteCode,
                    a.CanAddSurveyorToEnquiry,
                    a.CanApproveContract,
                    a.CanApproveEnquiry,
                    a.CanApproveEstimation,
                    a.CanApproveProposal,
                    a.CanApproveSurvey,
                    a.CanConvertEnqToProject,
                    a.CanConvertEstimationToProposal,
                    a.CanConvertProposalToContract,
                    a.CanCreateRoaster,
                    a.CanEditEnquiry,
                    a.CanEditPvReq,
                    a.CanModifyEstimation,
                    a.CanEditRoaster,
                    a.IsFinalAuthority

                }).AsNoTracking();




                var list =  _context.OP_HRM_TEMP_Projects
                   .OrderBy(request.Input.OrderBy).Select(d => new OP_HRM_TEMP_Project_PaginationDto
                   {
                       Id = d.Id,
                       ProjectCode = d.ProjectCode,
                       CustomerCode = d.CustomerCode,
                       ProjectNameEng = d.ProjectNameEng,
                       ProjectNameArb = d.ProjectNameArb,
                       ModifiedBy = d.ModifiedBy,
                       CreatedBy = d.CreatedBy,
                       StartDate = d.StartDate,
                       EndDate = d.EndDate,
                       IsResourcesAssigned = !Sites.Any(e => e.ProjectCode == d.ProjectCode && e.CustomerCode == d.CustomerCode && !e.IsResourcesAssigned && !e.IsAdendum),
                       IsMaterialAssigned = !Sites.Any(e => e.ProjectCode == d.ProjectCode && e.CustomerCode == d.CustomerCode && !e.IsMaterialAssigned && !e.IsAdendum),
                       IsLogisticsAssigned = !Sites.Any(e => e.ProjectCode == d.ProjectCode && e.CustomerCode == d.CustomerCode && !e.IsLogisticsAssigned && !e.IsAdendum),
                       IsShiftsAssigned = !Sites.Any(e => e.ProjectCode == d.ProjectCode && e.CustomerCode == d.CustomerCode && !e.IsShiftsAssigned && !e.IsAdendum),
                       IsExpenceOverheadsAssigned = !Sites.Any(e => e.ProjectCode == d.ProjectCode && e.CustomerCode == d.CustomerCode && !e.IsExpenceOverheadsAssigned && !e.IsAdendum),
                       IsEstimationCompleted = !Sites.Any(e => e.ProjectCode == d.ProjectCode && e.CustomerCode == d.CustomerCode && !e.IsEstimationCompleted && !e.IsAdendum),
                       IsSkillSetsMapped = !Sites.Any(e => e.ProjectCode == d.ProjectCode && e.CustomerCode == d.CustomerCode && !e.IsSkillSetsMapped && !e.IsAdendum),
                       IsConvertedToProposal = !Sites.Any(e => e.ProjectCode == d.ProjectCode && e.CustomerCode == d.CustomerCode && !e.IsConvertedToProposal && !e.IsAdendum),
                       IsConvrtedToContract = !Sites.Any(e => e.ProjectCode == d.ProjectCode && e.CustomerCode == d.CustomerCode && !e.IsConvrtedToContract && !e.IsAdendum),
                       BranchCode = d.BranchCode,
                       IsAdmin = isAdmin,


                       HasAuthority = AuthSites.Any(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode) || isAdmin,
                       ApprovedUser = oprApprvls.Any(e => e.AppAuth == request.User.UserId && e.ServiceType == "EST" && e.IsApproved && e.ServiceCode == d.ProjectCode),
                       // IsApproved = oprAuths.Where(e => e.BranchCode == d.BranchCode && e.CanApproveEstimation == true).Select(e => new { AppAuth = e.AppAuth }).GroupBy(e => e.AppAuth).Count() <= oprApprvls.Where(e => e.ServiceType == "EST" && e.ServiceCode == d.ProjectCode && e.BranchCode == d.BranchCode && e.IsApproved).Count(),
                       IsApproved = oprApprvls.Where(e => e.ServiceType == "EST" && e.ServiceCode == d.ProjectCode && /*e.BranchCode == d.BranchCode &&*/ e.IsApproved).Count() > 0,

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

                       } :
                       AuthSites.Any(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode)
                       ?
                       new TblOpAuthorities
                       {
                           CanApproveEnquiry = AuthSites.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode).CanApproveEnquiry,
                           CanApproveSurvey = AuthSites.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode).CanApproveSurvey,
                           CanConvertEnqToProject = AuthSites.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode).CanConvertEnqToProject,
                           CanCreateRoaster = AuthSites.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode).CanCreateRoaster,
                           CanEditRoaster = AuthSites.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode).CanEditRoaster,
                           CanApproveProposal = AuthSites.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode).CanApproveProposal,
                           CanModifyEstimation = AuthSites.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode).CanModifyEstimation,
                           CanApproveContract = AuthSites.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode).CanApproveContract,
                           CanApproveEstimation = AuthSites.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode).CanApproveEstimation,
                           IsFinalAuthority = AuthSites.FirstOrDefault(e => e.AppAuth == request.User.UserId && e.BranchCode == d.BranchCode && d.ProjectCode == e.ProjectCode).IsFinalAuthority

                       }
                       : new TblOpAuthorities
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
                   }).Where(e => //e.CompanyId == request.CompanyId &&
                                (e.ProjectCode.Contains(search) ||
                                e.ProjectNameEng.Contains(search) ||
                                e.ProjectNameArb.Contains(search) ||
                                 e.CustomerCode.Contains(search) ||
                                 e.BranchCode.Contains(search) ||
                                 search == "" || search == null
                                 ) && e.HasAuthority);

                //  .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                if (!string.IsNullOrEmpty(request.FilterInput.CustomerCode))
                {
                    list = list.Where(e => e.CustomerCode == request.FilterInput.CustomerCode);
                }
                 if (!string.IsNullOrEmpty(request.FilterInput.BranchCode))
                {
                    list = list.Where(e => e.BranchCode == request.FilterInput.BranchCode);
                }
                  if (request.FilterInput.StartDateFrom.HasValue&&request.FilterInput.StartDateTo.HasValue)
                {
                    var fd = request.FilterInput.StartDateFrom.Value;
                    var td = request.FilterInput.StartDateTo.Value;
                   list = list.Where(e => e.StartDate >=fd && e.StartDate <=td);
                }

                  if (request.FilterInput.EndDateFrom.HasValue&&request.FilterInput.EndDateTo.HasValue)
                {
                    var fd = request.FilterInput.EndDateFrom.Value;
                    var td = request.FilterInput.EndDateTo.Value;
                  
                    list = list.Where(e => e.EndDate >= fd && e.EndDate <= td);
                }


                var nreports = await list.OrderBy(request.Input.OrderBy).PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                return nreports;
            }

            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateProject Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }
    #endregion
}
