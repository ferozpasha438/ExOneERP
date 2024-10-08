using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.HumanResource.SetUp.HRMSetUpDtos;
using CIN.DB;
using CIN.Domain.HumanResource.Setup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.SetUp.HRMSetUpQuery
{
    #region GetPagedList

    public class GetVacationPolicies : IRequest<PaginatedList<TblHRMSysVacationPolicyDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetVacationPoliciesHandler : IRequestHandler<GetVacationPolicies, PaginatedList<TblHRMSysVacationPolicyDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVacationPoliciesHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysVacationPolicyDto>> Handle(GetVacationPolicies request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetVacationPolicies method start----");
                var search = request.Input.Query;
                bool isArab = request.User.Culture.IsArab();

                var list = await (from vacationPolicy in _context.VacationPolicies
                                  join grade in _context.Grades on vacationPolicy.GradeCode equals grade.GradeCode into grades
                                  from g in grades.DefaultIfEmpty()
                                  join position in _context.Positions on vacationPolicy.PositionCode equals position.PositionCode into positions
                                  from p in positions.DefaultIfEmpty()
                                  select new TblHRMSysVacationPolicyDto
                                  {
                                      Id = vacationPolicy.Id,
                                      VacationPolicyCode = vacationPolicy.VacationPolicyCode,
                                      VacationPolicyNameEn = vacationPolicy.VacationPolicyNameEn,
                                      VacationPolicyNameAr = vacationPolicy.VacationPolicyNameAr,
                                      GradeCode = vacationPolicy.GradeCode,
                                      GradeName = isArab ? g.GradeNameAr : g.GradeNameEn,
                                      PositionCode = vacationPolicy.PositionCode,
                                      PositionName = isArab ? p.PositionNameAr : p.PositionNameEn,
                                      AnnualVacationDays = vacationPolicy.AnnualVacationDays,
                                      MaximumDaysAllowed = vacationPolicy.MaximumDaysAllowed,
                                      VacationDurationInMonths = vacationPolicy.VacationDurationInMonths,
                                      IsAirTicketAllowed = vacationPolicy.IsAirTicketAllowed,
                                      AirTicketDurationInMonths = vacationPolicy.AirTicketDurationInMonths,
                                      IsFamilyTicketAllowed = vacationPolicy.IsFamilyTicketAllowed,
                                      FamilyAirTicketDuration = vacationPolicy.FamilyAirTicketDuration,
                                      IsExitReEntryRequired = vacationPolicy.IsExitReEntryRequired,
                                      IsAdvanceVacationPayAllowed = vacationPolicy.IsAdvanceVacationPayAllowed,
                                      IsVacationExtensionAllowed = vacationPolicy.IsVacationExtensionAllowed,
                                      Created = vacationPolicy.Created,
                                      CreatedBy = vacationPolicy.CreatedBy,
                                      IsActive = vacationPolicy.IsActive,
                                      Modified = vacationPolicy.Modified,
                                      ModifiedBy = vacationPolicy.ModifiedBy,
                                      VacationPolicyName = isArab ? vacationPolicy.VacationPolicyNameAr : vacationPolicy.VacationPolicyNameEn
                                  })
                                  .AsNoTracking()
                                  .Where(e => (e.VacationPolicyCode.Contains(search) || e.VacationPolicyName.Contains(search)))
                                  .OrderByDescending(x => x.Id)
                                  .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                Log.Info("----Info GetVacationPolicies method end----");
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetVacationPolicies Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetVacationPolicyId

    public class GetVacationPolicyId : IRequest<TblHRMSysVacationPolicyDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetVacationPolicyIdHandler : IRequestHandler<GetVacationPolicyId, TblHRMSysVacationPolicyDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVacationPolicyIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysVacationPolicyDto> Handle(GetVacationPolicyId request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetVacationPolicyId method start----");
            try
            {
                bool isArab = request.User.Culture.IsArab();
                var VacationPolicy = await _context.VacationPolicies.AsNoTracking()
                    .ProjectTo<TblHRMSysVacationPolicyDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetPayrollPackageById method end----");

                return VacationPolicy;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetVacationPolicyId Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateVacationPolicy

    public class CreateUpdateVacationPolicy : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysVacationPolicyDto Input { get; set; }
    }
    public class CreateUpdateVacationPolicyHandler : IRequestHandler<CreateUpdateVacationPolicy, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateVacationPolicyHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateVacationPolicy request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateVacationPolicy method start----");
                    var obj = request.Input;
                    TblHRMSysVacationPolicy vacationPolicy = new();

                    vacationPolicy = await _context.VacationPolicies.FirstOrDefaultAsync(e => e.VacationPolicyCode == request.Input.VacationPolicyCode);

                    if (vacationPolicy is not null)
                    {
                        vacationPolicy.VacationPolicyNameEn = obj.VacationPolicyNameEn;
                        vacationPolicy.VacationPolicyNameAr = obj.VacationPolicyNameAr;
                        vacationPolicy.GradeCode = obj.GradeCode;
                        vacationPolicy.PositionCode = obj.PositionCode;
                        vacationPolicy.AnnualVacationDays = obj.AnnualVacationDays;
                        vacationPolicy.MaximumDaysAllowed = obj.MaximumDaysAllowed;
                        vacationPolicy.VacationDurationInMonths = obj.VacationDurationInMonths;
                        vacationPolicy.IsAirTicketAllowed = obj.IsAirTicketAllowed;
                        vacationPolicy.AirTicketDurationInMonths = obj.AirTicketDurationInMonths;
                        vacationPolicy.IsFamilyTicketAllowed = obj.IsFamilyTicketAllowed;
                        vacationPolicy.FamilyAirTicketDuration = obj.FamilyAirTicketDuration;
                        vacationPolicy.IsExitReEntryRequired = obj.IsExitReEntryRequired;
                        vacationPolicy.IsAdvanceVacationPayAllowed = obj.IsAdvanceVacationPayAllowed;
                        vacationPolicy.IsVacationExtensionAllowed = obj.IsVacationExtensionAllowed;
                        vacationPolicy.Id = obj.Id;
                        vacationPolicy.IsActive = obj.IsActive;
                        vacationPolicy.ModifiedBy = request.User.UserId;
                        vacationPolicy.Modified = DateTime.Now;

                        _context.VacationPolicies.Update(vacationPolicy);
                    }
                    else
                    {
                        vacationPolicy = new()
                        {
                            VacationPolicyCode = obj.VacationPolicyCode,
                            VacationPolicyNameEn = obj.VacationPolicyNameEn,
                            VacationPolicyNameAr = obj.VacationPolicyNameAr,
                            GradeCode = obj.GradeCode,
                            PositionCode = obj.PositionCode,
                            AnnualVacationDays = obj.AnnualVacationDays,
                            MaximumDaysAllowed = obj.MaximumDaysAllowed,
                            VacationDurationInMonths = obj.VacationDurationInMonths,
                            IsAirTicketAllowed = obj.IsAirTicketAllowed,
                            AirTicketDurationInMonths = obj.AirTicketDurationInMonths,
                            IsFamilyTicketAllowed = obj.IsFamilyTicketAllowed,
                            FamilyAirTicketDuration = obj.FamilyAirTicketDuration,
                            IsExitReEntryRequired = obj.IsExitReEntryRequired,
                            IsAdvanceVacationPayAllowed = obj.IsAdvanceVacationPayAllowed,
                            IsVacationExtensionAllowed = obj.IsVacationExtensionAllowed,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.VacationPolicies.AddAsync(vacationPolicy);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateVacationPolicy method Exit----");
                    return ApiMessageInfo.Status(1, vacationPolicy.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateVacationPolicy Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region DeleteVacationPolicy
    public class DeleteVacationPolicy : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteVacationPolicyHandler : IRequestHandler<DeleteVacationPolicy, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteVacationPolicyHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteVacationPolicy request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info DeleteVacationPolicy method start----");
                    if (request.Id > 0)
                    {
                        var vacationPolicy = await _context.VacationPolicies.FirstOrDefaultAsync(e => e.Id == request.Id);
                        _context.VacationPolicies.Remove(vacationPolicy);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        Log.Info("----Info DeleteVacationPolicy method end----");
                        return request.Id;
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    Log.Error("Error in DeleteVacationPolicy Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }

    #endregion

    #region GetVacationPolicySelectListItem
    public class GetVacationPolicySelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public VacationPolicyFilterDto Filter { get; set; }
    }

    public class GetVacationPolicySelectListItemHandler : IRequestHandler<GetVacationPolicySelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetVacationPolicySelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetVacationPolicySelectListItem request, CancellationToken cancellationToken)
        {
            bool isArab = request.User.Culture.IsArab();
            var filter = request.Filter;

            var list = await _context.VacationPolicies
                .AsNoTracking()
                .Where(e => (string.IsNullOrEmpty(filter.PositionCode) ? e.PositionCode == null : e.PositionCode == filter.PositionCode) &&
                (string.IsNullOrEmpty(filter.GradeCode) ? e.GradeCode == null : e.GradeCode == filter.GradeCode)).
                OrderByDescending(e => e.Id).
                Select(e => new CustomSelectListItem { Text = isArab ? e.VacationPolicyNameAr : e.VacationPolicyNameEn, Value = e.Id.ToString() }).
                ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion   
}
