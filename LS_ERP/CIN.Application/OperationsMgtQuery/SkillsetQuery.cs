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


namespace CIN.Application.OperationsMgtQuery
{

    #region GetSkillsetsPagedList

    public class GetSkillsetsPagedList : IRequest<PaginatedList<TblOpSkillsetDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetSkillsetsPagedListHandler : IRequestHandler<GetSkillsetsPagedList, PaginatedList<TblOpSkillsetDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSkillsetsPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblOpSkillsetDto>> Handle(GetSkillsetsPagedList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.TblOpSkillsets.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.SkillSetCode.Contains(search) ||
                            e.NameInArabic.Contains(search) ||
                            e.NameInEnglish.Contains(search)||
                            search == "" || search == null
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblOpSkillsetDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion

    #region CreateUpdateSkillset
    public class CreateSkillset : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblOpSkillsetDto SkillsetDto { get; set; }
    }

    public class CreateSkillsetHandler : IRequestHandler<CreateSkillset, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateSkillsetHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateSkillset request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateSkillset method start----");



                var obj = request.SkillsetDto;


                TblOpSkillset Skillset = new();
                if (obj.Id > 0)
                    Skillset = await _context.TblOpSkillsets.FirstOrDefaultAsync(e => e.Id == obj.Id);
                else
                {
                    var me = await _context.TblOpSkillsets.AsNoTracking().OrderByDescending(e => e.Id).FirstOrDefaultAsync();

                    if (me is not null)
                    {
                        Skillset.SkillSetCode = "SST" + (me.Id + 1).ToString().PadLeft(6, '0');
                    }
                    else
                        Skillset.SkillSetCode = "SST000001";

                    if (_context.TblOpSkillsets.Any(x => x.SkillSetCode == Skillset.SkillSetCode))
                    {
                        return -1;
                    }
                }
                Skillset.Id = obj.Id;
                Skillset.IsActive = obj.IsActive;
                Skillset.SkillType = obj.SkillType;
                Skillset.NameInEnglish = obj.NameInEnglish;
                Skillset.NameInArabic = obj.NameInArabic;
                Skillset.DetailsOfSkillSet = obj.DetailsOfSkillSet;
                Skillset.SkillSetType = obj.SkillSetType;
                Skillset.PrioryImportanceScale = obj.PrioryImportanceScale;
                Skillset.MinBufferResource = obj.MinBufferResource;
                Skillset.MonthlyCtc = obj.MonthlyCtc;
                Skillset.CostToCompanyDay = obj.CostToCompanyDay;
                Skillset.MonthlyOverheadCost = obj.MonthlyOverheadCost;
                Skillset.MonthlyOtherOverHeads = obj.MonthlyOtherOverHeads;
                Skillset.TotalSkillsetCost = obj.TotalSkillsetCost;
                Skillset.TotalSkillsetCostDay = obj.TotalSkillsetCostDay;
                Skillset.ServicePriceToCompany = obj.ServicePriceToCompany;
                Skillset.MinMarginRequired = obj.MinMarginRequired;
                Skillset.OverrideMarginIfRequired = obj.OverrideMarginIfRequired;
                Skillset.IsActive = obj.IsActive;
                Skillset.ResponsibilitiesRoles = obj.ResponsibilitiesRoles;


                if (obj.Id > 0)
                {
                    Skillset.SkillSetCode = obj.SkillSetCode;
                    Skillset.ModifiedOn = DateTime.Now;
                    _context.TblOpSkillsets.Update(Skillset);
                }
                else
                {

                    Skillset.CreatedOn = DateTime.Now;
                    await _context.TblOpSkillsets.AddAsync(Skillset);
                }
                await _context.SaveChangesAsync();
                Log.Info("----Info CreateUpdateSkillset method Exit----");
                return Skillset.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateSkillset Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region IsExistCode
    public class IsExistCode : IRequest<TblOpSkillsetDto>
    {
        public UserIdentityDto User { get; set; }
        public string SkillsetCode { get; set; }
    }

    public class IsExistCodeHandler : IRequestHandler<IsExistCode, TblOpSkillsetDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public IsExistCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpSkillsetDto> Handle(IsExistCode request, CancellationToken cancellationToken)
        {
            TblOpSkillsetDto obj = await _context.TblOpSkillsets.AsNoTracking().ProjectTo<TblOpSkillsetDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.SkillSetCode == request.SkillsetCode);

            return obj;
        }
    }

    #endregion

    #region GetSkillsetBySkillsetCode
        public class GetSkillsetBySkillsetCode : IRequest<TblOpSkillsetDto>
        {
            public UserIdentityDto User { get; set; }
            public string SkillsetCode { get; set; }
        }

        public class GetSkillsetBySkillsetCodeHandler : IRequestHandler<GetSkillsetBySkillsetCode, TblOpSkillsetDto>
        {
            private readonly CINDBOneContext _context;
            private readonly IMapper _mapper;
            public GetSkillsetBySkillsetCodeHandler(CINDBOneContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<TblOpSkillsetDto> Handle(GetSkillsetBySkillsetCode request, CancellationToken cancellationToken)
            {
                TblOpSkillsetDto obj = await _context.TblOpSkillsets.AsNoTracking().ProjectTo<TblOpSkillsetDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.SkillSetCode == request.SkillsetCode);

                return obj;
            }
        }

        #endregion

    #region GetSkillsetById
    public class GetSkillsetById : IRequest<TblOpSkillsetDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetSkillsetByIdHandler : IRequestHandler<GetSkillsetById, TblOpSkillsetDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSkillsetByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpSkillsetDto> Handle(GetSkillsetById request, CancellationToken cancellationToken)
        {

            TblOpSkillsetDto obj = await _context.TblOpSkillsets.AsNoTracking().ProjectTo<TblOpSkillsetDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return obj;
        }
    }

    #endregion

    #region GetSelectSkillsetList

    public class GetSelectSkillsetList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectSkillsetListHandler : IRequestHandler<GetSelectSkillsetList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectSkillsetListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectSkillsetList request, CancellationToken cancellationToken)
        {

            var list = await _context.TblOpSkillsets.AsNoTracking()
              .Select(e => new CustomSelectListItem { Text = e.NameInEnglish, Value = e.SkillSetCode, TextTwo = e.NameInArabic })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion


    #region GetAllSkillsetList

    public class GetAllSkillsetList : IRequest<List<TblOpSkillsetDto>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetAllSkillsetListHandler : IRequestHandler<GetAllSkillsetList, List<TblOpSkillsetDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAllSkillsetListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblOpSkillsetDto>> Handle(GetAllSkillsetList request, CancellationToken cancellationToken)
        {

            List<TblOpSkillsetDto> list = await _context.TblOpSkillsets.AsNoTracking().ProjectTo<TblOpSkillsetDto>(_mapper.ConfigurationProvider).ToListAsync();
            return list;
        }
    }

    #endregion














    #region GetSkillsetCodes

    public class GetSkillsetCodes : IRequest<List<string>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSkillsetCodesHandler : IRequestHandler<GetSkillsetCodes, List<string>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSkillsetCodesHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<string>> Handle(GetSkillsetCodes request, CancellationToken cancellationToken)
        {

            var list = await _context.TblOpSkillsets.AsNoTracking()
              .Select(e => e.SkillSetCode)
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    #region DeleteSkillset
    public class DeleteSkillset : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteSkillsetQueryHandler : IRequestHandler<DeleteSkillset, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteSkillsetQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteSkillset request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteSkillset start----");

                if (request.Id > 0)
                {
                    var Skillset = await _context.TblOpSkillsets.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(Skillset);

                    await _context.SaveChangesAsync();

                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {

                Log.Error("Error in DeleteSkillset");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }

        }
    }

    #endregion



    #region SkillsetPlanForProject

    #region CreateUpdateSkillsetPlanForProject
    public class CreateUpdateSkillsetPlanForProject : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public OpSkillsetPlanForProjectDto Skillset { get; set; }
    }

    public class CreateUpdateSkillsetPlanForProjectHandler : IRequestHandler<CreateUpdateSkillsetPlanForProject, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateSkillsetPlanForProjectHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateSkillsetPlanForProject request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateSkillsetPlanForProject method start----");
                    var obj = request.Skillset;

                    if (request.Skillset.SkillsetList.Count() > 0)
                    {
                        var isRoasterExist = await _context.TblOpMonthlyRoasterForSites.AsNoTracking().AnyAsync(m => m.ProjectCode == request.Skillset.ProjectCode
                          && m.SiteCode == request.Skillset.SiteCode);
                        if (isRoasterExist)
                        {
                            return -1;              //Roaster_Already_Exist_You_Cannot_Edit_Skillset_Plan
                        }

                        var oldSkillsetsList = await _context.TblOpSkillsetPlanForProjects.Where(e => e.SiteCode == request.Skillset.SiteCode &&e.ProjectCode==request.Skillset.ProjectCode).ToListAsync();
                        _context.TblOpSkillsetPlanForProjects.RemoveRange(oldSkillsetsList);
                       await _context.SaveChangesAsync();

                        var existingMappings = await _context.TblOpEmployeeToResourceMapList.AsNoTracking().Where(e => e.ProjectCode == request.Skillset.ProjectCode
                          && e.SiteCode == request.Skillset.SiteCode).ToListAsync();
                        if (existingMappings.Count>0)
                        {
                            _context.TblOpEmployeeToResourceMapList.RemoveRange(existingMappings);
                            await _context.SaveChangesAsync();
                        }

                        List<TblOpSkillsetPlanForProject> SkillsetList = new();
                        foreach (var skill in request.Skillset.SkillsetList)
                        {
                            TblOpSkillsetPlanForProject skillset = new()
                            {
                                ProjectCode=request.Skillset.ProjectCode,
                                SiteCode = request.Skillset.SiteCode,
                                SkillsetCode = skill.SkillsetCode,
                                Quantity = skill.Quantity
                                

                            };


                            SkillsetList.Add(skillset);
                        }
                        await _context.TblOpSkillsetPlanForProjects.AddRangeAsync(SkillsetList);
                        await _context.SaveChangesAsync();

                        int sitesWithSkillsets = _context.TblOpSkillsetPlanForProjects.AsNoTracking().Where(e => e.ProjectCode == request.Skillset.ProjectCode).GroupBy(e => e.SiteCode).Count();

                        int noOfSitesforEnquiry = _context.OprEnquiries.AsNoTracking().Where(e => e.EnquiryNumber == request.Skillset.ProjectCode).GroupBy(x => x.SiteCode).Count();

                        var projectSite = await _context.TblOpProjectSites.AsNoTracking().FirstOrDefaultAsync(e => e.ProjectCode == request.Skillset.ProjectCode && e.SiteCode == request.Skillset.SiteCode);

                        projectSite.IsSkillSetsMapped = true;
                        projectSite.IsConvertedToProposal = false;
                        projectSite.IsEstimationCompleted = false;
                        _context.TblOpProjectSites.Update(projectSite);
                        _context.SaveChanges();
                       
                        var project = _context.OP_HRM_TEMP_Projects.AsNoTracking().FirstOrDefault(e => e.ProjectCode == request.Skillset.ProjectCode);

                        if (!projectSite.IsAdendum)
                        {

                            project.IsSkillSetsMapped = !_context.TblOpProjectSites.AsNoTracking().Any(e => e.ProjectCode == request.Skillset.ProjectCode && e.SiteCode == request.Skillset.SiteCode && !e.IsAdendum && !e.IsSkillSetsMapped);
                            project.IsConvertedToProposal = false;
                            project.IsEstimationCompleted = false;
                            project.IsResourcesAssigned = !_context.TblOpProjectSites.AsNoTracking().Any(e => e.ProjectCode == request.Skillset.ProjectCode && e.SiteCode == request.Skillset.SiteCode && !e.IsAdendum && !e.IsResourcesAssigned);
                            _context.OP_HRM_TEMP_Projects.Update(project);
                            _context.SaveChanges();
                        }

                       
                        



                        var projectEstimation = _context.TblOpProjectBudgetEstimations.AsNoTracking().FirstOrDefault(e =>
                        e.ProjectCode == request.Skillset.ProjectCode);
                       
                        
                        if (projectEstimation is not null)
                        {
                            var projectBudgetCostings = _context.TblOpProjectBudgetCostings.AsNoTracking().Where(e =>
                            e.ProjectCode == request.Skillset.ProjectCode
                            && e.SiteCode == request.Skillset.SiteCode
                            && e.ServiceType == "SKILLSET").ToList();

                            if (projectBudgetCostings.Count!=0)
                            {
                                projectBudgetCostings.ForEach(pbc=> {
                                    var resourceCostings =  _context.TblOpProjectResourceCosting.AsNoTracking().Where(rc=>rc.ProjectBudgetCostingId==pbc.ProjectBudgetCostingId).ToList();
                                if (resourceCostings.Count != 0)
                                {
                                    resourceCostings.ForEach(rc=>{
                                        var resSubCostings = _context.TblOpProjectResourceSubCostings.AsNoTracking().Where(rsc => rsc.ResourceCostingId == rc.ResourceCostingId).ToList();
                                        if (resSubCostings.Count != 0)
                                        {
                                            _context.TblOpProjectResourceSubCostings.RemoveRange(resSubCostings);
                                            _context.SaveChanges();

                                        }
                                        



                                    });
                                        _context.TblOpProjectResourceCosting.RemoveRange(resourceCostings);
                                        _context.SaveChanges();
                                    }

                                });

                                _context.TblOpProjectBudgetCostings.RemoveRange(projectBudgetCostings);
                                _context.SaveChanges();
                            }

                        }

                     


                        await _context.SaveChangesAsync();


                    }
                    Log.Info("----Info CreateUpdateSkillsetPlanForProject method Exit----");

                    await transaction.CommitAsync();
                    return 1;

                }
                catch (Exception ex)
                {
                    Log.Error("Error in CreateUpdateSkillsetPlanForProject Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    transaction.Rollback();
                    return 0;

                }
            }
        }
    }

    #endregion


    #region GetSkillsetPlanForProjectBySiteCode
    public class GetSkillsetPlanForProjectBySiteCode : IRequest<List<OpSkillsetDto>>
    {
        public UserIdentityDto User { get; set; }
        public string SiteCode { get; set; }
    }

    public class GetSkillsetPlanForProjectBySiteCodeHandler : IRequestHandler<GetSkillsetPlanForProjectBySiteCode, List<OpSkillsetDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSkillsetPlanForProjectBySiteCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<OpSkillsetDto>> Handle(GetSkillsetPlanForProjectBySiteCode request, CancellationToken cancellationToken)
        {
            try
            {

                var skillsets = (from m in _context.TblOpSkillsetPlanForProjects
                              join s in _context.TblOpSkillsets
                              on m.SkillsetCode equals s.SkillSetCode
                              where m.SiteCode == request.SiteCode
                              select new OpSkillsetDto
                              {
                                  SkillSetCode = s.SkillSetCode,
                                  NameInEnglish = s.NameInEnglish,
                                  NameInArabic = s.NameInArabic,
                                  SkillSetType=s.SkillSetType,
                                  SkillType=s.SkillType,
                                  DetailsOfSkillSet=s.DetailsOfSkillSet,
                                  TotalSkillsetCostDay=s.TotalSkillsetCostDay,                  
                                  Quantity=m.Quantity
                              }).ToList();
                return skillsets;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetSkillsetPlanForProjectBySiteCode");
                return null;
            }
        }
    }

    #endregion

    #region GetAllSkillsetPlanForProjectBySiteCode
    public class GetAllSkillsetPlanForProjectBySiteCode : IRequest<List<OpSkillsetDto>>
    {
        public UserIdentityDto User { get; set; }
        public string SiteCode { get; set; }
    }

    public class GetAllSkillsetPlanForProjectBySiteCodeHandler : IRequestHandler<GetAllSkillsetPlanForProjectBySiteCode, List<OpSkillsetDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAllSkillsetPlanForProjectBySiteCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<OpSkillsetDto>> Handle(GetAllSkillsetPlanForProjectBySiteCode request, CancellationToken cancellationToken)
        {
            try
            {

                var ss = (from m in _context.TblOpSkillsetPlanForProjects
                              join s in _context.TblOpSkillsets
                              on m.SkillsetCode equals s.SkillSetCode
                              where m.SiteCode == request.SiteCode
                              select new OpSkillsetDto
                              {
                                  NameInArabic = s.NameInEnglish,
                                  NameInEnglish = s.NameInArabic,
                                   Quantity= m.Quantity,
                                   SkillSetCode=s.SkillSetCode
                              }).ToList();
                return ss;
            }
            catch (Exception ex)
            {
                Log.Error("Error in  GetAllSkillsetPlanForProjectBySiteCode method");
                return null;
            }
        }
    }

    #endregion
    #region GetSkillsetsByProjectCodeAndSiteCode
    public class GetSkillsetsByProjectCodeAndSiteCode : IRequest<List<OpSkillsetDto>>
    {
        public UserIdentityDto User { get; set; }
        public string SiteCode { get; set; }
        public string ProjectCode { get; set; }
    }

    public class GetSkillsetsByProjectCodeAndSiteCodeHandler : IRequestHandler<GetSkillsetsByProjectCodeAndSiteCode, List<OpSkillsetDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSkillsetsByProjectCodeAndSiteCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<OpSkillsetDto>> Handle(GetSkillsetsByProjectCodeAndSiteCode request, CancellationToken cancellationToken)
        {
            try
            {

                var shifts = (from m in _context.TblOpSkillsetPlanForProjects
                              join s in _context.TblOpSkillsets
                              on m.SkillsetCode equals s.SkillSetCode
                              where m.SiteCode == request.SiteCode && m.ProjectCode==request.ProjectCode
                              select new OpSkillsetDto
                              {
                                  NameInArabic = s.NameInArabic,
                                  NameInEnglish = s.NameInEnglish,
                                   Quantity= m.Quantity,
                                   SkillSetCode=s.SkillSetCode
                              }).ToList();
                return shifts;
            }
            catch (Exception ex)
            {
                Log.Error("Error in  GetSkillsetsByProjectCodeAndSiteCode method");
                return null;
            }
        }
    }

    #endregion




    #endregion


    #region EmployeeSkillsetForMonth
    public class EmployeeSkillsetForMonth : IRequest<TblOpSkillsetDto>
    {
        public UserIdentityDto User { get; set; }
        public InpuEmployeeSkillsetDto Input { get; set; }
    }

    public class EmployeeSkillsetForMonthHandler : IRequestHandler<EmployeeSkillsetForMonth, TblOpSkillsetDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public EmployeeSkillsetForMonthHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpSkillsetDto> Handle(EmployeeSkillsetForMonth request, CancellationToken cancellationToken)
        {
            try
            {



                var latestRoasterForEmployee = await _context.TblOpMonthlyRoasterForSites.OrderByDescending(e => e.MonthStartDate)
                    .FirstOrDefaultAsync(e => e.Month == request.Input.Date.Value.Month && e.Year == request.Input.Date.Value.Year
                && e.ProjectCode==request.Input.ProjectCode
                && e.SiteCode==request.Input.SiteCode
                && e.EmployeeNumber==request.Input.EmployeeNumber
                );

                TblOpSkillsetDto skillset = await _context.TblOpSkillsets.ProjectTo<TblOpSkillsetDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e=>e.SkillSetCode==latestRoasterForEmployee.SkillsetCode)??new();

                return skillset;
            }
            catch (Exception ex)
            {
                Log.Error("Error in EmployeeSkillsetForMonth");
                return null;
            }
        }
    }

    #endregion

}
