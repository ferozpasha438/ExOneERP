using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.Common.Models;
using CIN.Application.HumanResource.EmployeeMgmt.HRMgmtDtos;
using CIN.Application.TimeAndAttendance.Management.TNAMgmtDtos;
using CIN.DB;
using CIN.Domain.TimeAndAttendance.Management;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.TimeAndAttendance.Management.TNAMgmtQuery
{

    #region GetPagedList

    public class GetEmployeeRoasterList : IRequest<BaseEmployeeRoasterDto>
    {
        public UserIdentityDto User { get; set; }
        public EmployeeRoasterFilter Input { get; set; }
    }

    public class GetEmployeeRoasterListHandler : IRequestHandler<GetEmployeeRoasterList, BaseEmployeeRoasterDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeRoasterListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseEmployeeRoasterDto> Handle(GetEmployeeRoasterList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetEmployeeRoasterList method start----");
                request.Input.PayrollGroupCode = request.Input.PayrollGroupCode.Trim();
                request.Input.BranchCode = request.Input.BranchCode.Trim();
                request.Input.PayrollPeriodCode = request.Input.PayrollPeriodCode.Trim();

                bool isArab = request.User.Culture.IsArab();
                BaseEmployeeRoasterDto objBaseEmployeeRoaster = new BaseEmployeeRoasterDto();

                //Retrieve distinct Employees based on PayrollGroup and Branch from Contract Info
                var roasterEmployees = await (from empRoaster in _context.EmployeeRoasters
                                              select new TblTNATrnEmployeeRoasterDto
                                              {
                                                  EmployeeID = empRoaster.EmployeeID,
                                                  BranchCode = empRoaster.BranchCode,
                                                  PayrollGroupCode = empRoaster.PayrollGroupCode,
                                              })
                    .AsNoTracking()
                    .Where(e => (e.PayrollGroupCode.Contains(request.Input.PayrollGroupCode) && e.BranchCode.Contains(request.Input.BranchCode)))
                    .OrderByDescending(x => x.EmployeeID)
                    .Distinct()
                    .ToListAsync();

                if (roasterEmployees is not null)
                {
                    //Retrieve Employees based on PayrollGroup and Branch from Contract Info and consider only if roaster exists.
                    var contractEmployees = await (from contract in _context.EmployeeContracts
                                                   join personalInformation in _context.PersonalInformation on contract.EmployeeID equals personalInformation.Id
                                                   join employeeControls in _context.EmployeeControls on contract.EmployeeID equals employeeControls.EmployeeID
                                                   join payrollGroup in _context.PayrollGroups on contract.PayrollGroupCode equals payrollGroup.PayrollGroupCode
                                                   join companyBranches in _context.CompanyBranches on contract.BranchCode equals companyBranches.BranchCode
                                                   select new TblTNATrnEmployeeRoasterDto
                                                   {
                                                       EmployeeID = contract.EmployeeID,
                                                       EmployeeName = isArab ? string.Concat(personalInformation.FirstNameAr, " ", personalInformation.LastNameAr) : string.Concat(personalInformation.FirstNameEn, " ", personalInformation.LastNameEn),
                                                       BranchCode = contract.BranchCode,
                                                       BranchName = companyBranches.BranchName,
                                                       PayrollGroupCode = contract.PayrollGroupCode,
                                                       PayrollGroupName = isArab ? payrollGroup.PayrollGroupNameAr : payrollGroup.PayrollGroupNameEn,
                                                       IsShiftApplicable = employeeControls.IsShiftApplicable,
                                                   })
                                                   .AsNoTracking()
                                                   .Where(e => (e.PayrollGroupCode.Contains(request.Input.PayrollGroupCode) &&
                                                   e.BranchCode.Contains(request.Input.BranchCode)
                                                   ))
                                                   .OrderByDescending(x => x.EmployeeID).ToListAsync();

                    contractEmployees = contractEmployees.Where(x => roasterEmployees.Exists(p => p.EmployeeID == x.EmployeeID)).ToList();

                    if (contractEmployees.Count() > 0)
                    {
                        objBaseEmployeeRoaster.EmployeeRoasters.AddRange(contractEmployees);
                        List<TblTNATrnEmployeeRoasterDto> employeeWithNoRoasters = new();

                        //Retrieve PayrollGroup details.
                        //var payrollGroupDetails = await _context.PayrollGroups.AsNoTracking()
                        //    .FirstOrDefaultAsync(e => e.PayrollGroupCode == request.Input.PayrollGroupCode);

                        //Retrieve Payroll Period details.
                        var payrollPeriodDetails = await _context.PayrollPeriods.AsNoTracking()
                            .FirstOrDefaultAsync(e => e.PayrollPeriodCode == request.Input.PayrollPeriodCode);

                        if (payrollPeriodDetails is not null)
                        {
                            List<RoasterColumn> objRoasterColumns = new List<RoasterColumn>();
                            DateTime dateTime = payrollPeriodDetails.PayrollPeriodStartDate;

                            while (dateTime <= payrollPeriodDetails.PayrollPeriodEndDate)
                            {
                                objRoasterColumns.Add(new RoasterColumn
                                {
                                    RoasterDate = dateTime.ToString("dd/MM/yyyy"),
                                    RoasterDay = dateTime.DayOfWeek.ToString()
                                }); ;
                                dateTime = dateTime.AddDays(1);
                            }

                            if (objRoasterColumns.Count() > 0)
                                objBaseEmployeeRoaster.RoasterColumns.AddRange(objRoasterColumns);

                            objBaseEmployeeRoaster.EmployeeRoasters.ForEach((e) =>
                           {
                               List<RoasterRow> objRoasterRows = new List<RoasterRow>();

                               var empRoasterList = _context.EmployeeRoasters.AsNoTracking()
                               .Where(x => (x.EmployeeID == e.EmployeeID
                               && x.Date.CompareTo(payrollPeriodDetails.PayrollPeriodStartDate) >= 0 && x.Date.CompareTo(payrollPeriodDetails.PayrollPeriodEndDate) <= 0)
                               ).OrderBy(x => x.Date)
                               .ToList();

                               if (empRoasterList is not null)
                               {
                                   //Adding Dynamic Properties.
                                   foreach (var empRoaster in empRoasterList)
                                       objRoasterRows.Add(new RoasterRow() { RoasterDate = empRoaster.Date.ToString("dd/MM/yyyy"), ShiftCode = !string.IsNullOrEmpty(empRoaster.ShiftCode) ? empRoaster.ShiftCode : "-" });
                               }
                               if (objRoasterRows.Count() > 0)
                                   e.RoasterRows.AddRange(objRoasterRows);
                               else
                                   employeeWithNoRoasters.Add(e);
                           });

                            //Remove Employees with no roasters in the selected PayrollPeriod.
                            if (employeeWithNoRoasters.Count() > 0)
                                employeeWithNoRoasters.ForEach(e => { objBaseEmployeeRoaster.EmployeeRoasters.Remove(e); });
                        }
                    }
                }
                Log.Info("----Info GetEmployeeRoasterList method end----");
                return objBaseEmployeeRoaster;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeRoasterList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion

    #region CreateUpdateEmployeeRoaster

    public class CreateUpdateEmployeeRoaster : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public BaseEmployeeRoasterDto Input { get; set; }
    }
    public class CreateUpdateEmployeeRoasterHandler : IRequestHandler<CreateUpdateEmployeeRoaster, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateEmployeeRoasterHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateEmployeeRoaster request, CancellationToken cancellationToken)
        {
            List<string> messages = new();

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateEmployeeRoaster method start----");
                    var objBaseEmployeeRoaster = request.Input;
                    if (objBaseEmployeeRoaster is not null)
                    {
                        messages = new List<string>();
                        if (objBaseEmployeeRoaster.EmployeeRoasters is not null && objBaseEmployeeRoaster.EmployeeRoasters.Count() > 0)
                        {
                            //for each employee.
                            for (int rowIndex = 0; rowIndex < objBaseEmployeeRoaster.EmployeeRoasters.Count(); rowIndex++)
                            {
                                var employee = objBaseEmployeeRoaster.EmployeeRoasters[rowIndex];
                                messages.Add(employee.EmployeeName);
                                if (employee.RoasterRows is not null)
                                {
                                    //for each employee Roaster
                                    for (int cellIndex = 0; cellIndex < employee.RoasterRows.Count(); cellIndex++)
                                    {
                                        var roaster = employee.RoasterRows[cellIndex];
                                        TblTNATrnEmployeeRoaster employeeRoaster = new();
                                        employeeRoaster = await _context.EmployeeRoasters
                                            .FirstOrDefaultAsync(e => e.EmployeeID == employee.EmployeeID
                                            && e.Date == DateTime.ParseExact(roaster.RoasterDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                                        //If employee roaster exists
                                        if (employeeRoaster.Id > 0)
                                        {
                                            //If Shift code has changed, then update.
                                            if (employeeRoaster.ShiftCode != roaster.ShiftCode)
                                            {
                                                employeeRoaster.ShiftCode = roaster.ShiftCode;
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
                                                EmployeeID = employee.EmployeeID,
                                                BranchCode = employee.BranchCode,
                                                PayrollGroupCode = employee.PayrollGroupCode,
                                                Date = Convert.ToDateTime(roaster.RoasterDate),
                                                ShiftCode = roaster.ShiftCode,
                                                IsActive = true,
                                                CreatedBy = request.User.UserId,
                                                Created = DateTime.Now,
                                            };
                                            await _context.EmployeeRoasters.AddAsync(employeeRoaster);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateEmployeeRoaster method Exit----");
                    return ApiMessageInfo.Status(string.Join(",", messages.ToArray()), 1);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateEmployeeRoaster Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(string.Join(",", messages.ToArray()), 0);
                }
            }
        }
    }

    #endregion

    #region GenerateEmployeesRoaster
    public class GenerateEmployeesRoaster : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public EmployeeRoasterFilter Input { get; set; }
    }
    public class GenerateEmployeesRoasterHandler : IRequestHandler<GenerateEmployeesRoaster, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GenerateEmployeesRoasterHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(GenerateEmployeesRoaster request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info GenerateEmployeesRoaster method start----");
                    request.Input.PayrollGroupCode = request.Input.PayrollGroupCode.Trim();
                    request.Input.BranchCode = request.Input.BranchCode.Trim();
                    request.Input.PayrollPeriodCode = request.Input.PayrollPeriodCode.Trim();
                    var obj = request.Input;

                    //Retrieve Contract Info of all Employees based on PayrollGroupCode and BranchCode.
                    var employees = await _context.EmployeeContracts
                        .AsNoTracking()
                        .Where(e => (e.PayrollGroupCode.Contains(obj.PayrollGroupCode) && e.BranchCode.Contains(obj.BranchCode)))
                        .ToListAsync();

                    if (employees.Count() > 0)
                    {
                        int i = 0;

                        //Retrieve Payroll Period details.
                        var payrollPeriodDetails = await _context.PayrollPeriods.AsNoTracking()
                            .FirstOrDefaultAsync(e => e.PayrollPeriodCode == obj.PayrollPeriodCode);

                        foreach (var employee in employees)
                        {
                            var employeeShiftInfo = await _context.EmployeeShifts.AsNoTracking()
                                .ProjectTo<TblHRMTrnEmployeeShiftInfoDto>(_mapper.ConfigurationProvider)
                                .FirstOrDefaultAsync(e => e.EmployeeID == employee.EmployeeID);

                            //Retrieve Employee's PayrollGroupCode from Contract Info.
                            var employeeControls = await _context.EmployeeControls.AsNoTracking()
                                .FirstOrDefaultAsync(e => e.EmployeeID == employee.EmployeeID);

                            if (employeeControls.IsRoasterApplicable)
                            {
                                if (payrollPeriodDetails is not null)
                                {
                                    DateTime dateTime = payrollPeriodDetails.PayrollPeriodStartDate;
                                    while (Extensions.IsBetween<DateTime>(dateTime, payrollPeriodDetails.PayrollPeriodStartDate, payrollPeriodDetails.PayrollPeriodEndDate))
                                    {
                                        TblTNATrnEmployeeRoaster employeeRoaster = new();
                                        employeeRoaster = await _context.EmployeeRoasters
                                            .FirstOrDefaultAsync(e => e.EmployeeID == employee.EmployeeID && e.Date == dateTime);

                                        var shiftCode = GetShiftCodeByDayOfWeek(dateTime, employeeShiftInfo);

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
                                                EmployeeID = employee.EmployeeID,
                                                BranchCode = employee.BranchCode,
                                                PayrollGroupCode = employee.PayrollGroupCode,
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
                                    i++;
                                }
                            }
                        }
                        if (i > 0)
                        {
                            await transaction.CommitAsync();
                            Log.Info("----Info GenerateEmployeesRoaster method Exit----");
                            return ApiMessageInfo.Status(1, 1);
                        }
                        else
                        {
                            Log.Info("----Info GenerateEmployeesRoaster method Exit with Error Message RoasterNotApplicableForEmployees----");
                            return ApiMessageInfo.Status("RoasterNotApplicableForEmployees");
                        }
                    }
                    else
                    {
                        Log.Info("----Info GenerateEmployeesRoaster method Exit with Error Message NoEmployeesInSearchCriteria----");
                        return ApiMessageInfo.Status("NoEmployeesInSearchCriteria");
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in GenerateEmployeesRoaster Method");
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

}
