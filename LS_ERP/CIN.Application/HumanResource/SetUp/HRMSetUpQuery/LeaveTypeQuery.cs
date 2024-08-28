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
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.SetUp.HRMSetUpQuery
{

    #region GetPagedList

    public class GetLeaveTypeList : IRequest<PaginatedList<TblHRMSysLeaveTypeDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetLeaveTypeListHandler : IRequestHandler<GetLeaveTypeList, PaginatedList<TblHRMSysLeaveTypeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetLeaveTypeListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TblHRMSysLeaveTypeDto>> Handle(GetLeaveTypeList request, CancellationToken cancellationToken)
        {
            try
            {
                //async Task<List<TblHRMSysLeaveTypeDto>> GetList()
                //{
                //    await Task.Delay(0);
                //    return new()
                //{
                //    new(){Id=1, LeaveCode = "Code_1",LeaveTypeEn = "Type_EN_1", LeaveTypeAr = "Type_AR_1", Type = 1, Gender = 1, IsActive = true  },
                //    new(){Id=2, LeaveCode = "Code_2",LeaveTypeEn = "Type_EN_2", LeaveTypeAr = "Type_AR_2", Type = 2, Gender = 2, IsActive = true  },
                //    new(){ Id=3,LeaveCode = "Code_3",LeaveTypeEn = "Type_EN_3", LeaveTypeAr = "Type_AR_3", Type = 3, Gender = 3, IsActive = true  },
                //};
                //}

                // var list = await (await GetList()).AsQueryable().PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
                Log.Info("----Info GetLeaveTypeList method start----");
                var search = request.Input.Query;
                var list = await _context.LeaveTypes.AsNoTracking()
                     .Where(e => (e.LeaveTypeCode.Contains(search) || e.LeaveTypeEn.Contains(search)) || e.LeaveTypeAr.Contains(search))
                   .OrderByDescending(x => x.Id)
                    .ProjectTo<TblHRMSysLeaveTypeDto>(_mapper.ConfigurationProvider)
                     .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

                Log.Info("----Info GetLeaveTypeList method end----");
                return list;// new(await GetList(), 3, 1, 10);
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetLeaveTypeList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region GetLeaveTypeById

    public class GetLeaveTypeById : IRequest<TblHRMSysLeaveTypeDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetLeaveTypeByIdHandler : IRequestHandler<GetLeaveTypeById, TblHRMSysLeaveTypeDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetLeaveTypeByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMSysLeaveTypeDto> Handle(GetLeaveTypeById request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetLeaveTypeById method start----");
            try
            {
                //async Task<List<TblHRMSysLeaveTypeDto>> GetList()
                //{
                //    await Task.Delay(0);
                //    return new()
                //{
                //    new(){Id=1, LeaveCode = "Code_1",LeaveTypeEn = "Type_EN_1", LeaveTypeAr = "Type_AR_1", Type = 1, Gender = 1, IsActive = true , IsUsedForVacation = true },
                //    new(){Id=2, LeaveCode = "Code_2",LeaveTypeEn = "Type_EN_2", LeaveTypeAr = "Type_AR_2", Type = 2, Gender = 2, IsActive = true ,IsUsedForVacation=true,IsSalaryPaid = true },
                //    new(){ Id=3,LeaveCode = "Code_3",LeaveTypeEn = "Type_EN_3", LeaveTypeAr = "Type_AR_3", Type = 3, Gender = 3, IsActive = true , IsUsedForVacation=true },
                //};
                //}
                //var list = await GetList();
                //return list.Where(e => e.Id == request.Id).FirstOrDefault();

                var leaveType = await _context.LeaveTypes.AsNoTracking()
                    .ProjectTo<TblHRMSysLeaveTypeDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                Log.Info("----Info GetLeaveTypeById method end----");

                if (leaveType is not null)
                    return leaveType;
                else
                    return new();
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetLeaveTypeById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateLeaveType

    public class CreateUpdateLeaveType : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMSysLeaveTypeDto Input { get; set; }
    }
    public class CreateUpdateLeaveTypeHandler : IRequestHandler<CreateUpdateLeaveType, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateLeaveTypeHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateLeaveType request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info CreateUpdateLeaveType method start----");
                var obj = request.Input;
                TblHRMSysLeaveType leaveType = await _context.LeaveTypes.FirstOrDefaultAsync(e => e.Id == request.Input.Id) ?? new();
                leaveType.LeaveTypeEn = obj.LeaveTypeEn;
                leaveType.LeaveTypeAr = obj.LeaveTypeAr;
                leaveType.IsCarryForward = obj.IsCarryForward;
                leaveType.MaxLeaveDays = obj.MaxLeaveDays;
                leaveType.IsSalaryPaid = obj.IsSalaryPaid;
                leaveType.Type = obj.Type;
                leaveType.MonthlyProRataLeaves = obj.MonthlyProRataLeaves;
                leaveType.SalaryPercentage = obj.IsSalaryPaid ? obj.SalaryPercentage : 0;
                leaveType.IsUsedForVacation = obj.IsUsedForVacation;
                leaveType.MaxAccumulatedLeave = obj.IsUsedForVacation ? obj.MaxAccumulatedLeave : 0;
                leaveType.IsTravelRequired = obj.IsTravelRequired;
                leaveType.IsDocumentRequired = obj.IsDocumentRequired;
                leaveType.IsExitAndreEntryRequired = obj.IsExitAndreEntryRequired;
                leaveType.IsFinanceApprovalRequired = obj.IsFinanceApprovalRequired;
                leaveType.IsLeaveEncashment = obj.IsLeaveEncashment;
                leaveType.IsLocalColumnRequired = obj.IsLocalColumnRequired;
                leaveType.IsReduceFromVacation = obj.IsReduceFromVacation;
                leaveType.Gender = obj.Gender;
                leaveType.NationalityId = obj.NationalityId;
                leaveType.IsActive = obj.IsActive;

                if (leaveType.Id > 0)
                {
                    leaveType.ModifiedBy = request.User.UserId;
                    leaveType.Modified = DateTime.Now;
                    _context.LeaveTypes.Update(leaveType);
                }
                else
                {
                    leaveType.LeaveTypeCode = obj.LeaveTypeCode.ToUpper();
                    leaveType.CreatedBy = request.User.UserId;
                    leaveType.Created = DateTime.Now;
                    await _context.LeaveTypes.AddAsync(leaveType);
                }

                await _context.SaveChangesAsync();

                Log.Info("----Info CreateUpdateLeaveType method Exit----");
                return ApiMessageInfo.Status(1, leaveType.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CreateUpdateLeaveType Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(ex.Message);
            }
        }
    }

    #endregion

    #region DeleteLeaveType
    public class DeleteLeaveType : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteLeaveTypeHandler : IRequestHandler<DeleteLeaveType, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteLeaveTypeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteLeaveType request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteLeaveType method start----");
                if (request.Id > 0)
                {
                    var gender = await _context.LeaveTypes.FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.LeaveTypes.Remove(gender);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteLeaveType method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteLeaveType Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion

    #region GetLeaveTypeSelectListItem
    public class GetLeaveTypeSelectListItem : IRequest<List<VacationRequestLeaveTypeDto>>
    {
        public UserIdentityDto User { get; set; }
        public int EmployeeId { get; set; }
        public string RequestType { get; set; }
    }

    public class GetLeaveTypeSelectListItemHandler : IRequestHandler<GetLeaveTypeSelectListItem, List<VacationRequestLeaveTypeDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetLeaveTypeSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<VacationRequestLeaveTypeDto>> Handle(GetLeaveTypeSelectListItem request, CancellationToken cancellationToken)
        {

            //bool isArab = request.User.Culture.IsArab();
            //var list = await _context.LeaveTypes.AsNoTracking().OrderByDescending(e => e.Id)
            //   .Select(e => new VacationRequestLeaveTypeDto { Text = isArab ? e.LeaveTypeAr : e.LeaveTypeEn, Value = e.LeaveTypeCode })
            //      .ToListAsync(cancellationToken);

            //foreach (var item in list)
            //{
            //    var empInfo = _context.EmployeeLeaveInformations.Where(e => e.EmployeeID == request.EmployeeId);
            //    var leaveInfo = await empInfo.Where(e => e.LeaveTypeCode == item.Value).FirstOrDefaultAsync();
            //    item.Availed = leaveInfo?.Availed ?? 0;
            //    item.Assigned = leaveInfo?.Assigned ?? 0;
            //}


            try
            {
                bool isArab = request.User.Culture.IsArab(), IsLeaveRequest = false;
                var list = (await _context.EmployeeLeaveInformations.AsNoTracking()
                    .Where(e => e.EmployeeID == request.EmployeeId).ToListAsync())
                    .GroupBy(e => e.LeaveTypeCode)
                    .ToList();

                List<VacationRequestLeaveTypeDto> vacationList = new();
                var leaveTypes = _context.LeaveTypes.AsNoTracking();
                if (request.RequestType.HasValue() && request.RequestType == "leaveRequest")
                {
                    IsLeaveRequest = true;
                }

                foreach (var item in list)
                {
                    TblHRMSysLeaveType leaveType = IsLeaveRequest ?
                                                   await leaveTypes.Where(e => e.LeaveTypeCode == item.Key && e.IsUsedForVacation == false).FirstOrDefaultAsync() :
                                                   await leaveTypes.Where(e => e.LeaveTypeCode == item.Key).FirstOrDefaultAsync();

                    if (leaveType is not null)
                    {
                        vacationList.Add(new VacationRequestLeaveTypeDto
                        {
                            Text = isArab ? leaveType.LeaveTypeAr : leaveType.LeaveTypeEn,
                            Value = item.Key,
                            Assigned = item.Sum(e => e.Assigned),
                            Availed = item.Sum(e => e.Availed),
                        });
                    }
                }

                return vacationList;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
    #endregion
}
