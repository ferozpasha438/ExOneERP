using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.HumanResource.EmployeeMgmt.HRMgmtDtos;
using CIN.Application.TimeAndAttendance.Management.TNAMgmtDtos;
using CIN.DB;
using CIN.Domain.HumanResource.EmployeeMgt;
using CIN.Domain.TimeAndAttendance.Management;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.EmployeeMgmt.HRMgmtQuery
{
    #region GetEmployeeShiftById
    public class GetEmployeeShiftById : IRequest<TblHRMTrnEmployeeShiftInfoDto>
    {
        public UserIdentityDto User { get; set; }
        public int EmployeeID { get; set; }
    }

    public class GetEmployeeShiftByIdHandler : IRequestHandler<GetEmployeeShiftById, TblHRMTrnEmployeeShiftInfoDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeShiftByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblHRMTrnEmployeeShiftInfoDto> Handle(GetEmployeeShiftById request, CancellationToken cancellationToken)
        {


            Log.Info("----Info GetEmployeeShiftById method start----");
            try
            {
                var employeeShifts = await _context.EmployeeShifts.AsNoTracking()
                    .ProjectTo<TblHRMTrnEmployeeShiftInfoDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(e => e.EmployeeID == request.EmployeeID);
                Log.Info("----Info GetEmployeeShiftById method end----");

                if (employeeShifts is not null)
                    return employeeShifts;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeShiftById Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }
    #endregion

    #region CreateUpdateEmployeeShift
    public class CreateUpdateEmployeeShift : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public TblHRMTrnEmployeeShiftInfoDto Input { get; set; }
    }
    public class CreateUpdateEmployeeShiftHandler : IRequestHandler<CreateUpdateEmployeeShift, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateEmployeeShiftHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateEmployeeShift request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateEmployeeShift method start----");
                    var obj = request.Input;
                    TblHRMTrnEmployeeShiftInfo employeeShift = new();
                    if (request.Input.Id > 0)
                    {
                        employeeShift = await _context.EmployeeShifts
                            .FirstOrDefaultAsync(e => e.Id == obj.Id && e.EmployeeID == obj.EmployeeID);
                        employeeShift.MondayShiftCode = obj.MondayShiftCode;
                        employeeShift.TuesdayShiftCode = obj.TuesdayShiftCode;
                        employeeShift.WednesdayShiftCode = obj.WednesdayShiftCode;
                        employeeShift.ThursdayShiftCode = obj.ThursdayShiftCode;
                        employeeShift.FridayShiftCode = obj.FridayShiftCode;
                        employeeShift.SaturdayShiftCode = obj.SaturdayShiftCode;
                        employeeShift.SundayShiftCode = obj.SundayShiftCode;
                        employeeShift.IsActive = obj.IsActive;
                        employeeShift.ModifiedBy = request.User.UserId;
                        employeeShift.Modified = DateTime.Now;

                        _context.EmployeeShifts.Update(employeeShift);

                    }
                    else
                    {
                        employeeShift = new()
                        {
                            EmployeeID = obj.EmployeeID,
                            MondayShiftCode = obj.MondayShiftCode,
                            TuesdayShiftCode = obj.TuesdayShiftCode,
                            WednesdayShiftCode = obj.WednesdayShiftCode,
                            ThursdayShiftCode = obj.ThursdayShiftCode,
                            FridayShiftCode = obj.FridayShiftCode,
                            SaturdayShiftCode = obj.SaturdayShiftCode,
                            SundayShiftCode = obj.SundayShiftCode,
                            IsActive = obj.IsActive,
                            CreatedBy = request.User.UserId,
                            Created = DateTime.Now,
                        };
                        await _context.EmployeeShifts.AddAsync(employeeShift);
                    }
                    await _context.SaveChangesAsync();

                    //Retrieve Employee's PayrollGroupCode from Contract Info.
                    var employeeContractInfo = await _context.EmployeeContracts.AsNoTracking()
                        .FirstOrDefaultAsync(e => e.EmployeeID == obj.EmployeeID);

                    //Retrieve Employee's PayrollGroupCode from Contract Info.
                    var employeeControls = await _context.EmployeeControls.AsNoTracking()
                        .FirstOrDefaultAsync(e => e.EmployeeID == obj.EmployeeID);

                    if (employeeContractInfo is not null && employeeControls.IsRoasterApplicable)
                    {
                        //Retrieve PayrollGroup details.
                        var payrollGroupDetails = await _context.PayrollGroups.AsNoTracking()
                            .FirstOrDefaultAsync(e => e.PayrollGroupCode == employeeContractInfo.PayrollGroupCode);

                        if (payrollGroupDetails is not null)
                        {
                            DateTime dateTime = payrollGroupDetails.PayrollGroupStartDate;
                            while (Extensions.IsBetween<DateTime>(dateTime, payrollGroupDetails.PayrollGroupStartDate, payrollGroupDetails.PayrollGroupEndDate))
                            {
                                TblTNATrnEmployeeRoaster employeeRoaster = new();
                                employeeRoaster = await _context.EmployeeRoasters
                                    .FirstOrDefaultAsync(e => e.EmployeeID == obj.EmployeeID && e.Date == dateTime);

                                var shiftCode = GetShiftCodeByDayOfWeek(dateTime, obj);

                                //If employee roaster exists
                                if (employeeRoaster is not null && employeeRoaster.Id > 0)
                                {
                                    //If Shift code has changed, then update.
                                    if (employeeRoaster.ShiftCode != shiftCode)
                                    {
                                        employeeRoaster.ShiftCode = shiftCode;
                                        employeeRoaster.ModifiedBy = request.User.UserId;
                                        employeeRoaster.Modified = DateTime.Now;

                                        _context.EmployeeRoasters.Update(employeeRoaster);
                                    }
                                }
                                else
                                {
                                    //If Roaster does not exists.
                                    employeeRoaster = new()
                                    {
                                        EmployeeID = obj.EmployeeID,
                                        BranchCode = employeeContractInfo.BranchCode,
                                        PayrollGroupCode = employeeContractInfo.PayrollGroupCode,
                                        Date = dateTime,
                                        ShiftCode = shiftCode,
                                        IsActive = true,
                                        CreatedBy = request.User.UserId,
                                        Created = DateTime.Now,
                                    };
                                    await _context.EmployeeRoasters.AddAsync(employeeRoaster);
                                }
                                dateTime = dateTime.AddDays(1);
                            }
                            await _context.SaveChangesAsync();
                        }
                    }
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateEmployeeShift method Exit----");
                    return ApiMessageInfo.Status(1, employeeShift.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateEmployeeShift Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }

        private string GetShiftCodeByDayOfWeek(DateTime dateTime, TblHRMTrnEmployeeShiftInfoDto employeeShiftInfo)
        {
            string shiftCode = string.Empty;
            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    shiftCode = employeeShiftInfo.SundayShiftCode;
                    break;
                case DayOfWeek.Monday:
                    shiftCode = employeeShiftInfo.MondayShiftCode;
                    break;
                case DayOfWeek.Tuesday:
                    shiftCode = employeeShiftInfo.TuesdayShiftCode;
                    break;
                case DayOfWeek.Wednesday:
                    shiftCode = employeeShiftInfo.WednesdayShiftCode;
                    break;
                case DayOfWeek.Thursday:
                    shiftCode = employeeShiftInfo.ThursdayShiftCode;
                    break;
                case DayOfWeek.Friday:
                    shiftCode = employeeShiftInfo.FridayShiftCode;
                    break;
                case DayOfWeek.Saturday:
                    shiftCode = employeeShiftInfo.SaturdayShiftCode;
                    break;
            }
            return shiftCode;
        }
    }

    #endregion

    #region DeleteEmployeeShift
    public class DeleteEmployeeShift : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }
    public class DeleteEmployeeShiftHandler : IRequestHandler<DeleteEmployeeShift, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteEmployeeShiftHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteEmployeeShift request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info DeleteEmployeeShift method start----");
                if (request.Id > 0)
                {
                    var employeeShift = await _context.EmployeeShifts
                        .FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.Remove(employeeShift);
                    await _context.SaveChangesAsync();
                    Log.Info("----Info DeleteEmployeeShift method end----");
                    return request.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error in DeleteEmployeeShift Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }
    #endregion

    #region GetEmployeeShiftsSelectListItem
    public class GetEmployeeShiftsSelectListItem : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }
    public class GetEmployeeShiftsSelectListItemHandler : IRequestHandler<GetEmployeeShiftsSelectListItem, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeShiftsSelectListItemHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetEmployeeShiftsSelectListItem request, CancellationToken cancellationToken)
        {
            try
            {
                bool isArab = request.User.Culture.IsArab();

                var list = await (from employeeShifts in _context.EmployeeShifts
                                  join employee in _context.PersonalInformation on employeeShifts.EmployeeID equals employee.Id
                                  select new TblHRMTrnEmployeeShiftInfoDto
                                  {
                                      EmployeeID = employeeShifts.EmployeeID,
                                      EmployeeNameEn = string.Concat(employee.FirstNameEn, " ", employee.LastNameEn),
                                      EmployeeNameAr = string.Concat(employee.FirstNameAr, " ", employee.LastNameAr)
                                  })
                                  .AsNoTracking()
                                  .OrderByDescending(e => e.EmployeeID)
                                  .Select(e => new CustomSelectListItem { Text = !isArab ? e.EmployeeNameEn : e.EmployeeNameAr, Value = e.EmployeeID.ToString() })
                                  .ToListAsync(cancellationToken);
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeShiftsSelectListItem Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }
    #endregion

}
