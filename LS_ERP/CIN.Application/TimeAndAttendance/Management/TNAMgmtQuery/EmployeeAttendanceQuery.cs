using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.HumanResource.SetUp.HRMSetUpDtos;
using CIN.Application.TimeAndAttendance.Management.TNAMgmtDtos;
using CIN.DB;
using CIN.Domain.TimeAndAttendance.Management;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.TimeAndAttendance.Management.TNAMgmtQuery
{
    #region GetEmployeeAttendanceList

    public class GetEmployeeAttendanceList : IRequest<BaseEmployeeAttendanceDto>
    {
        public UserIdentityDto User { get; set; }
        public EmployeeAttendanceFilter Input { get; set; }
    }

    public class GetEmployeeAttendanceListHandler : IRequestHandler<GetEmployeeAttendanceList, BaseEmployeeAttendanceDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeAttendanceListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseEmployeeAttendanceDto> Handle(GetEmployeeAttendanceList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetEmployeeAttendanceList method start----");

                bool isArab = request.User.Culture.IsArab();
                BaseEmployeeAttendanceDto objBaseEmployeeAttendance = new BaseEmployeeAttendanceDto();

                //Retrieve PayrollGroup details.
                var payrollGroupDetails = await _context.PayrollGroups.AsNoTracking()
                    .FirstOrDefaultAsync(e => e.PayrollGroupCode == request.Input.PayrollGroupCode);

                payrollGroupDetails.PayrollGroupEndDate = payrollGroupDetails.PayrollGroupEndDate.AddDays(1).AddSeconds(-1);

                //Retrieve all Employees based on PayrollGroup and Branch and has attendance in the current payroll period.
                var employeesWithAttendance = await (from attendanceEmployee in _context.TNAEmployeeAttendance
                                                     join contract in _context.EmployeeContracts on attendanceEmployee.EmployeeID equals contract.EmployeeID
                                                     select new Employee
                                                     {
                                                         EmployeeID = attendanceEmployee.EmployeeID,
                                                         BranchCode = contract.BranchCode,
                                                         PayrollGroupCode = contract.PayrollGroupCode,
                                                         Date = attendanceEmployee.Date
                                                     })
                                       .AsNoTracking()
                                       .Where(e => (e.PayrollGroupCode.Contains(request.Input.PayrollGroupCode) && e.BranchCode.Contains(request.Input.BranchCode)
                                       && e.Date.CompareTo(payrollGroupDetails.PayrollGroupStartDate) >= 0 && e.Date.CompareTo(payrollGroupDetails.PayrollGroupEndDate) <= 0))
                                       .OrderByDescending(x => x.EmployeeID)
                                       .Distinct()
                                       .ToListAsync();

                if (employeesWithAttendance is not null)
                {
                    //Retrieve Employees based on PayrollGroup and Branch from Contract Info and consider only if roaster exists.
                    var contractEmployees = await (from contract in _context.EmployeeContracts
                                                   join personalInformation in _context.PersonalInformation on contract.EmployeeID equals personalInformation.Id
                                                   join employeeControls in _context.EmployeeControls on contract.EmployeeID equals employeeControls.EmployeeID
                                                   join payrollGroup in _context.PayrollGroups on contract.PayrollGroupCode equals payrollGroup.PayrollGroupCode
                                                   join companyBranches in _context.CompanyBranches on contract.BranchCode equals companyBranches.BranchCode
                                                   select new Employee
                                                   {
                                                       EmployeeID = contract.EmployeeID,
                                                       EmployeeName = isArab ? string.Concat(personalInformation.FirstNameAr, " ", personalInformation.LastNameAr) : string.Concat(personalInformation.FirstNameEn, " ", personalInformation.LastNameEn),
                                                       BranchCode = contract.BranchCode,
                                                       BranchName = companyBranches.BranchName,
                                                       PayrollGroupCode = contract.PayrollGroupCode,
                                                       PayrollGroupName = isArab ? payrollGroup.PayrollGroupNameAr : payrollGroup.PayrollGroupNameEn,
                                                       HolidayCalendarCode = contract.HolidayCalendarCode,
                                                       IsRoasterApplicable = employeeControls.IsRoasterApplicable
                                                   })
                                                   .AsNoTracking()
                                                   .Where(e => (e.PayrollGroupCode.Contains(request.Input.PayrollGroupCode) &&
                                                   e.BranchCode.Contains(request.Input.BranchCode)
                                                   ))
                                                   .OrderBy(x => x.EmployeeID).ToListAsync();

                    contractEmployees = contractEmployees.Where(x => employeesWithAttendance.Exists(p => p.EmployeeID == x.EmployeeID)).ToList();

                    if (contractEmployees.Count() > 0)
                    {
                        objBaseEmployeeAttendance.EmployeeList.AddRange(contractEmployees);

                        if (payrollGroupDetails is not null)
                        {
                            List<AttendanceColumn> objAttendanceColumns = new List<AttendanceColumn>();
                            DateTime dateTime = payrollGroupDetails.PayrollGroupStartDate;

                            //Add Attendance Columns
                            while (dateTime <= payrollGroupDetails.PayrollGroupEndDate)
                            {
                                objAttendanceColumns.Add(new AttendanceColumn
                                {
                                    AttendanceDate = dateTime.ToString("dd/MM/yyyy"),
                                    AttendanceDay = dateTime.DayOfWeek.ToString()
                                }); ;
                                dateTime = dateTime.AddDays(1);
                            }

                            if (objAttendanceColumns.Count() > 0)
                                objBaseEmployeeAttendance.AttendanceColumns.AddRange(objAttendanceColumns);

                            //Loop through each employee who has attendance with in PayrollGroupStartDate and PayrollGroupEndDate.
                            foreach (Employee e in objBaseEmployeeAttendance.EmployeeList.ToList())
                            {
                                //objBaseEmployeeAttendance.EmployeeList.ForEach(async (e) =>
                                //{
                                List<TblTNATrnEmployeeAttendanceDto> AttendanceRows = new List<TblTNATrnEmployeeAttendanceDto>();

                                //Retrieve Employee attendance for the PayrollGroup StartDate and EndDate from Primary site only(i.e., ShiftNumber is 1).
                                var empAttendanceList = await _context.TNAEmployeeAttendance.AsNoTracking()
                                .ProjectTo<TblTNATrnEmployeeAttendanceDto>(_mapper.ConfigurationProvider)
                                .Where(x => (x.EmployeeID == e.EmployeeID && x.ShiftNumber == 1 && !x.IsApproved &&
                                x.Date.CompareTo(payrollGroupDetails.PayrollGroupStartDate) >= 0 &&
                                x.Date.CompareTo(payrollGroupDetails.PayrollGroupEndDate) <= 0))
                                .OrderBy(x => x.Date)
                                .ToListAsync(cancellationToken);

                                if (empAttendanceList is not null && empAttendanceList.Count() > 0)
                                {
                                    //Retrieve holiday applicable for the employee based on HolidayCalendarCode
                                    var employeeCalendarMappings = await (from holidayCalendarMappings in _context.HolidayCalendarMappings
                                                                          join holidays in _context.Holidays on holidayCalendarMappings.HolidayCode equals holidays.HolidayCode
                                                                          select new
                                                                          {
                                                                              HolidayCalendarCode = holidayCalendarMappings.HolidayCalendarCode,
                                                                              HolidayCode = holidayCalendarMappings.HolidayCode,
                                                                              Date = holidays.Date
                                                                          })
                                                                  .AsNoTracking()
                                                                  .Where(p => p.HolidayCalendarCode == e.HolidayCalendarCode &&
                                                                  p.Date.CompareTo(payrollGroupDetails.PayrollGroupStartDate) >= 0 && p.Date.CompareTo(payrollGroupDetails.PayrollGroupEndDate) <= 0)
                                                                  .OrderByDescending(p => p.Date)
                                                                  .ToListAsync(cancellationToken);

                                    //Retrieve Employee Roaster for the PayrollGroup StartDate and EndDate.
                                    var employeeWeeklyOffs = await _context.EmployeeWeeklyOffs.AsNoTracking()
                                    .Where(x => (x.EmployeeID == e.EmployeeID && x.Date.CompareTo(payrollGroupDetails.PayrollGroupStartDate) >= 0 && x.Date.CompareTo(payrollGroupDetails.PayrollGroupEndDate) <= 0))
                                    .OrderBy(x => x.Date)
                                    .ToListAsync(cancellationToken);

                                    //If Roaster is not applicable for the employee.
                                    if (!e.IsRoasterApplicable)
                                    {
                                        //Loop through each day from PayrollGroup StartDate to EndDate.
                                        var payrollGroupStartDate = payrollGroupDetails.PayrollGroupStartDate;

                                        while (payrollGroupStartDate <= payrollGroupDetails.PayrollGroupEndDate)
                                        {
                                            //Retrieve employee attendance for the payrollGroupStartDate.
                                            var empAttendance = empAttendanceList.Where(e => e.Date.CompareTo(payrollGroupStartDate) == 0).FirstOrDefault();

                                            //if employee has no attendance then Check if it is Holiday/WeekOff.
                                            if (empAttendance is null)
                                            {
                                                //Retrieve top 1 employee attendance with ShiftNumber as '1'(i.e., Primary site).
                                                var AttendanceRow = empAttendanceList.Where(e => e.ShiftNumber == 1).FirstOrDefault();

                                                //Check if it is Holiday for the employee and Insert the attendance with the AttnFlag as 'H'.
                                                if (employeeCalendarMappings.Where(x => x.Date.CompareTo(payrollGroupStartDate) == 0).Count() > 0)
                                                {
                                                    AttendanceRows.Add(new TblTNATrnEmployeeAttendanceDto
                                                    {
                                                        EmployeeID = AttendanceRow.EmployeeID,
                                                        Date = payrollGroupStartDate,
                                                        AttnFlag = "H",
                                                        ShiftNumber = AttendanceRow.ShiftNumber
                                                    });
                                                }
                                                //Check if it is WeeklyOff for the employee and update the AttnFlag.
                                                else if (employeeWeeklyOffs.Where(x => x.Date.CompareTo(payrollGroupStartDate) == 0).Count() > 0)
                                                {
                                                    AttendanceRows.Add(new TblTNATrnEmployeeAttendanceDto
                                                    {
                                                        EmployeeID = AttendanceRow.EmployeeID,
                                                        Date = payrollGroupStartDate,
                                                        AttnFlag = "O",
                                                        ShiftNumber = AttendanceRow.ShiftNumber
                                                    });
                                                }
                                                //Neither a holiday nor an WeeklyOff, then Employee is Absent
                                                else
                                                {
                                                    AttendanceRows.Add(new TblTNATrnEmployeeAttendanceDto
                                                    {
                                                        EmployeeID = AttendanceRow.EmployeeID,
                                                        Date = payrollGroupStartDate,
                                                        AttnFlag = "A",
                                                        ShiftNumber = AttendanceRow.ShiftNumber
                                                    });
                                                }
                                            }
                                            //Check employee has attendance then update the AttnFlag with appropriate color coding.
                                            else
                                            {
                                                //Check if it is Holiday for the employee and Insert the attendance with the AttnFlag as 'H'.
                                                if (employeeCalendarMappings.Where(x => x.Date.CompareTo(payrollGroupStartDate) == 0).Count() > 0)
                                                    empAttendance.AttnFlag = "PH";
                                                //Check if it is WeeklyOff for the employee and update the AttnFlag.
                                                else if (employeeWeeklyOffs.Where(x => x.Date.CompareTo(payrollGroupStartDate) == 0).Count() > 0)
                                                    empAttendance.AttnFlag = "PO";

                                                AttendanceRows.Add(empAttendance);
                                            }
                                            payrollGroupStartDate = payrollGroupStartDate.AddDays(1);
                                        }
                                    }
                                    else
                                    {
                                        //Retrieve Employee Roaster for the PayrollGroup StartDate and EndDate.
                                        var empRoasterList = await _context.EmployeeRoasters.AsNoTracking()
                                        .Where(x => (x.EmployeeID == e.EmployeeID && x.Date.CompareTo(payrollGroupDetails.PayrollGroupStartDate) >= 0 && x.Date.CompareTo(payrollGroupDetails.PayrollGroupEndDate) <= 0))
                                        .OrderBy(x => x.Date)
                                        .ToListAsync(cancellationToken);

                                        if (empRoasterList is not null)
                                        {
                                            //Loop through each day from PayGroup StartDate to EndDate.
                                            var payrollGroupStartDate = payrollGroupDetails.PayrollGroupStartDate;

                                            while (payrollGroupStartDate <= payrollGroupDetails.PayrollGroupEndDate)
                                            {
                                                //Retrieve employee Roaster for the payrollGroupStartDate.
                                                var empRoaster = empRoasterList.Where(x => (x.Date.CompareTo(payrollGroupStartDate) == 0)).FirstOrDefault();

                                                //Retrieve employee attendance for the payrollGroupStartDate.
                                                var empAttendance = empAttendanceList.Where(e => e.Date.CompareTo(payrollGroupStartDate) == 0).FirstOrDefault();

                                                if (empRoaster is not null)
                                                {
                                                    //If employee is supposed to be working on payrollGroupStartDate as per roaster.
                                                    if (!empRoaster.ShiftCode.ToUpper().Equals("O"))
                                                    {
                                                        //if employee has attendance then AttnFlag will be 'P' else Check if it is Holiday/WeekOff.
                                                        if (empAttendance is null)
                                                        {
                                                            //Retrieve first employee attendance with ShiftNumber as '1'(i.e., Primary site).
                                                            var AttendanceRow = empAttendanceList.Where(e => e.ShiftNumber == 1).FirstOrDefault();

                                                            //Check if it is Holiday for the employee and Insert the attendance with the AttnFlag as 'H'.
                                                            if (employeeCalendarMappings.Where(x => x.Date.CompareTo(payrollGroupStartDate) == 0).Count() > 0)
                                                            {
                                                                AttendanceRows.Add(new TblTNATrnEmployeeAttendanceDto
                                                                {
                                                                    EmployeeID = AttendanceRow.EmployeeID,
                                                                    Date = payrollGroupStartDate,
                                                                    AttnFlag = "H",
                                                                    ShiftNumber = AttendanceRow.ShiftNumber
                                                                });
                                                            }
                                                            //Neither a holiday nor an WeeklyOff, then Employee is Absent
                                                            else
                                                            {
                                                                AttendanceRows.Add(new TblTNATrnEmployeeAttendanceDto
                                                                {
                                                                    EmployeeID = AttendanceRow.EmployeeID,
                                                                    Date = payrollGroupStartDate,
                                                                    AttnFlag = "A",
                                                                    ShiftNumber = AttendanceRow.ShiftNumber
                                                                });
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //Check if it is Holiday for the employee and Insert the attendance with the AttnFlag as 'H'.
                                                            if (employeeCalendarMappings.Where(x => x.Date.CompareTo(payrollGroupStartDate) == 0).Count() > 0)
                                                                empAttendance.AttnFlag = "PH";
                                                            //Check if it is WeeklyOff for the employee and update the AttnFlag.
                                                            else if (employeeWeeklyOffs.Where(x => x.Date.CompareTo(payrollGroupStartDate) == 0).Count() > 0)
                                                                empAttendance.AttnFlag = "PO";

                                                            AttendanceRows.Add(empAttendance);
                                                        }
                                                    }
                                                    //If it is Week off for the employee on payrollGroupStartDate as per roaster.
                                                    else
                                                    {
                                                        //if employee has no attendance then Check if it is Holiday/WeekOff.
                                                        if (empAttendance is null)
                                                        {
                                                            //Retrieve top 1 employee attendance with ShiftNumber as '1'(i.e., Primary site).
                                                            var AttendanceRow = empAttendanceList.Where(e => e.ShiftNumber == 1).FirstOrDefault();

                                                            //Check if it is Holiday for the employee and Insert the attendance with the AttnFlag as 'H'.
                                                            if (employeeCalendarMappings.Where(x => x.Date.CompareTo(payrollGroupStartDate) == 0).Count() > 0)
                                                            {
                                                                AttendanceRows.Add(new TblTNATrnEmployeeAttendanceDto
                                                                {
                                                                    EmployeeID = AttendanceRow.EmployeeID,
                                                                    Date = payrollGroupStartDate,
                                                                    AttnFlag = "H",
                                                                    ShiftNumber = AttendanceRow.ShiftNumber
                                                                });
                                                            }
                                                            //Check if it is WeeklyOff for the employee and update the AttnFlag.
                                                            else if (employeeWeeklyOffs.Where(x => x.Date.CompareTo(payrollGroupStartDate) == 0).Count() > 0)
                                                            {
                                                                AttendanceRows.Add(new TblTNATrnEmployeeAttendanceDto
                                                                {
                                                                    EmployeeID = AttendanceRow.EmployeeID,
                                                                    Date = payrollGroupStartDate,
                                                                    AttnFlag = "O",
                                                                    ShiftNumber = AttendanceRow.ShiftNumber
                                                                });
                                                            }
                                                            //Neither a holiday nor an WeeklyOff, then Employee is Absent
                                                            else
                                                            {
                                                                AttendanceRows.Add(new TblTNATrnEmployeeAttendanceDto
                                                                {
                                                                    EmployeeID = AttendanceRow.EmployeeID,
                                                                    Date = payrollGroupStartDate,
                                                                    AttnFlag = "A",
                                                                    ShiftNumber = AttendanceRow.ShiftNumber
                                                                });
                                                            }
                                                        }
                                                        //Check employee has attendance then update the AttnFlag with appropriate color coding.
                                                        else
                                                        {
                                                            //Check if it is Holiday for the employee and Insert the attendance with the AttnFlag as 'H'.
                                                            if (employeeCalendarMappings.Where(x => x.Date.CompareTo(payrollGroupStartDate) == 0).Count() > 0)
                                                                empAttendance.AttnFlag = "PH";
                                                            //Check if it is WeeklyOff for the employee and update the AttnFlag.
                                                            else if (employeeWeeklyOffs.Where(x => x.Date.CompareTo(payrollGroupStartDate) == 0).Count() > 0)
                                                                empAttendance.AttnFlag = "PO";

                                                            AttendanceRows.Add(empAttendance);
                                                        }
                                                    }
                                                }
                                                payrollGroupStartDate = payrollGroupStartDate.AddDays(1);
                                            }
                                        }
                                    }

                                    if (AttendanceRows is not null)
                                        e.AttendanceRows.AddRange(AttendanceRows);
                                    else
                                        //Remove employee if attendance does not exist in the payroll period.
                                        objBaseEmployeeAttendance.EmployeeList.Remove(e);
                                }
                                else
                                    //Remove employee if attendance does not exist in the payroll period.
                                    objBaseEmployeeAttendance.EmployeeList.Remove(e);
                            }
                        }
                    }
                }

                Log.Info("----Info GetEmployeeAttendanceList method end----");
                return objBaseEmployeeAttendance;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetEmployeeAttendanceList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }
        }
    }

    #endregion    

    #region CreateUpdateEmployeeAttendance

    public class CreateUpdateEmployeeAttendance : UserIdentityDto, IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public List<TblTNATrnEmployeeAttendanceDto> Input { get; set; }
    }
    public class CreateUpdateEmployeeAttendanceHandler : IRequestHandler<CreateUpdateEmployeeAttendance, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateEmployeeAttendanceHandler(IMapper mapper, CINDBOneContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<AppCtrollerDto> Handle(CreateUpdateEmployeeAttendance request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateEmployeeAttendance method start----");
                    var obj = request.Input;
                    int i = 0;
                    TblTNATrnEmployeeAttendance employeeAttendance = new();

                    foreach (var p in obj)
                    {
                        i++;
                        await Task.Run(async () =>
                        {
                            if (p.Id > 0)
                            {
                                employeeAttendance = await _context.TNAEmployeeAttendance.FirstOrDefaultAsync(e => e.Id == p.Id);
                                employeeAttendance.EmployeeID = p.EmployeeID;
                                employeeAttendance.Date = p.Date;
                                employeeAttendance.InTime = p.InTime;
                                employeeAttendance.OutTime = p.OutTime;
                                employeeAttendance.AttnFlag = char.Parse(p.AttnFlag);
                                employeeAttendance.ShiftNumber = p.ShiftNumber;
                                employeeAttendance.ShiftCode = p.ShiftCode;
                                employeeAttendance.ModifiedBy = request.User.UserId;
                                employeeAttendance.Modified = DateTime.Now;
                                _context.TNAEmployeeAttendance.Update(employeeAttendance);
                            }
                            else
                            {
                                //Retrieve existing employee attendance.
                                var employeeAttendanceList = await _context.TNAEmployeeAttendance.Where(e => e.EmployeeID == p.EmployeeID && e.Date.CompareTo(p.Date) == 0).ToListAsync();

                                //Delete existing attendance
                                if (employeeAttendanceList is not null)
                                    _context.TNAEmployeeAttendance.RemoveRange(employeeAttendanceList);

                                employeeAttendance = new()
                                {
                                    EmployeeID = p.EmployeeID,
                                    Date = p.Date,
                                    InTime = p.InTime,
                                    OutTime = p.OutTime,
                                    AttnFlag = char.Parse(p.AttnFlag),
                                    ShiftNumber = p.ShiftNumber,
                                    ShiftCode = p.ShiftCode,
                                    CreatedBy = request.User.UserId,
                                    Created = DateTime.Now,
                                };
                                await _context.TNAEmployeeAttendance.AddAsync(employeeAttendance);
                            }
                        }).ConfigureAwait(false);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    Log.Info("----Info CreateUpdateEmployeeAttendance method Exit----");
                    return ApiMessageInfo.Status(1, i);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdateEmployeeAttendance Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region ApproveEmployeeAttendance

    public class ApproveEmployeeAttendance : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public EmployeeAttendanceFilter Input { get; set; }
    }

    public class ApproveEmployeeAttendanceHandler : IRequestHandler<ApproveEmployeeAttendance, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public ApproveEmployeeAttendanceHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(ApproveEmployeeAttendance request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info ApproveEmployeeAttendance method start----");
                    int i = 0;

                    //Retrieve PayrollGroup details.
                    var payrollGroupDetails = await _context.PayrollGroups.AsNoTracking()
                        .FirstOrDefaultAsync(e => e.PayrollGroupCode == request.Input.PayrollGroupCode);

                    payrollGroupDetails.PayrollGroupEndDate = payrollGroupDetails.PayrollGroupEndDate.AddDays(1).AddSeconds(-1);

                    //Retrieve Employees having attendance in the current payroll period based on PayrollGroup and Branch.
                    var employees = await (from attendanceEmployee in _context.TNAEmployeeAttendance
                                           join contract in _context.EmployeeContracts on attendanceEmployee.EmployeeID equals contract.EmployeeID
                                           select new Employee
                                           {
                                               EmployeeID = attendanceEmployee.EmployeeID,
                                               BranchCode = contract.BranchCode,
                                               PayrollGroupCode = contract.PayrollGroupCode,
                                               Date = attendanceEmployee.Date,
                                               HolidayCalendarCode = contract.HolidayCalendarCode
                                           })
                                           .AsNoTracking()
                                           .Where(e => (e.PayrollGroupCode.Contains(request.Input.PayrollGroupCode) && e.BranchCode.Contains(request.Input.BranchCode)
                                           && e.Date.CompareTo(payrollGroupDetails.PayrollGroupStartDate) >= 0 && e.Date.CompareTo(payrollGroupDetails.PayrollGroupEndDate) <= 0))
                                           .OrderByDescending(x => x.EmployeeID)
                                           .Distinct()
                                           .ToListAsync();

                    //Retrieve distinct Employees based on PayrollGroup and Branch from Contract Info.
                    var contractEmployees = await (from contract in _context.EmployeeContracts
                                                   select new Employee
                                                   {
                                                       EmployeeID = contract.EmployeeID,
                                                       BranchCode = contract.BranchCode,
                                                       PayrollGroupCode = contract.PayrollGroupCode,
                                                       HolidayCalendarCode = contract.HolidayCalendarCode
                                                   })
                                                   .AsNoTracking()
                                                   .Where(e => (e.PayrollGroupCode.Contains(request.Input.PayrollGroupCode)
                                                   && e.BranchCode.Contains(request.Input.BranchCode)))
                                                   .OrderByDescending(x => x.EmployeeID)
                                                   .ToListAsync();

                    contractEmployees = contractEmployees.Where(x => employees.Exists(p => p.EmployeeID == x.EmployeeID)).ToList();

                    if (contractEmployees is not null)
                    {
                        //Loop through each employee and mark his attendance as approved.
                        foreach (var employee in contractEmployees)
                        {
                            i++;
                            DateTime dateTime = payrollGroupDetails.PayrollGroupStartDate;

                            //Retrieve holiday applicable for the employee based on HolidayCalendarCode
                            var employeeCalendarMappings = await (from holidayCalendarMappings in _context.HolidayCalendarMappings
                                                                  join holidays in _context.Holidays on holidayCalendarMappings.HolidayCode equals holidays.HolidayCode
                                                                  select new
                                                                  {
                                                                      HolidayCalendarCode = holidayCalendarMappings.HolidayCalendarCode,
                                                                      HolidayCode = holidayCalendarMappings.HolidayCode,
                                                                      Date = holidays.Date
                                                                  })
                                                              .AsNoTracking()
                                                              .Where(p => p.HolidayCalendarCode == employee.HolidayCalendarCode)
                                                              .OrderByDescending(p => p.Date)
                                                              .ToListAsync(cancellationToken);

                            //Retrieve Employee weekly offs between PayrollGroup StartDate and EndDate.
                            var employeeWeeklyOffs = await _context.EmployeeWeeklyOffs.AsNoTracking()
                            .Where(x => (x.EmployeeID == employee.EmployeeID && x.Date.CompareTo(payrollGroupDetails.PayrollGroupStartDate) >= 0 && x.Date.CompareTo(payrollGroupDetails.PayrollGroupEndDate) <= 0))
                            .OrderBy(x => x.Date)
                            .ToListAsync(cancellationToken);

                            //Loop through each day between PayrollGroupStartDate and PayrollGroupEndDate
                            while (dateTime <= payrollGroupDetails.PayrollGroupEndDate)
                            {
                                //Retrieve employee attendance for the current dateTime.
                                var attendance = await _context.TNAEmployeeAttendance
                                    .Where(e => e.EmployeeID == employee.EmployeeID && e.ShiftNumber == 1 && !e.IsApproved && e.Date.CompareTo(dateTime) == 0)
                                    .FirstOrDefaultAsync();

                                //if employee has attendance on given date.
                                if (attendance is not null)
                                {
                                    var shift = await _context.Shifts
                                        .AsNoTracking().ProjectTo<TblHRMSysShiftDto>(_mapper.ConfigurationProvider)
                                        .FirstOrDefaultAsync(e => e.ShiftCode == attendance.ShiftCode);

                                    if (shift is not null)
                                    {
                                        attendance.EstimatedInTime = TimeSpan.Parse(shift.InTime);
                                        attendance.EstimatedOutTime = TimeSpan.Parse(shift.OutTime);

                                        DateTime attendanceInTime = dateTime + attendance.InTime,
                                            attendanceOutTime = dateTime + attendance.OutTime,
                                            attendanceEstimatedInTime = dateTime + attendance.EstimatedInTime,
                                            attendanceEstimatedOutTime = dateTime + attendance.EstimatedOutTime;

                                        //Update flag and Network Time if OutTime is punched Next day.
                                        if (DateTime.Compare(attendanceInTime, attendanceOutTime) > 0)
                                        {
                                            attendance.IsPunchedOutNextDay = true;
                                            attendance.NetWorkingTime = (attendanceOutTime.AddDays(1) - attendanceInTime).Ticks;
                                        }
                                        else
                                            attendance.NetWorkingTime = (attendanceOutTime - attendanceInTime).Ticks;

                                        //Update Late Hours and IsLate flag if InTime crosses in grace time.
                                        if (DateTime.Compare(attendanceInTime, (attendanceEstimatedInTime + TimeSpan.Parse(shift.InGrace))) > 0)
                                        {
                                            attendance.IsLate = true;
                                            attendance.LateHours = (attendanceEstimatedInTime - attendanceInTime).Ticks;
                                        }

                                        //Update Over Time Hours if InTime is before Estimated InTime. 
                                        if (DateTime.Compare(attendanceInTime, attendanceEstimatedInTime) < 0)
                                            attendance.OverTimeHours = (attendanceInTime - attendanceEstimatedInTime).Ticks;

                                        //Update Over Time Hours if OutTime is after Estimated OutTime if outTime punched on same day. 
                                        if (!attendance.IsPunchedOutNextDay)
                                        {
                                            if (DateTime.Compare(attendanceOutTime, (attendanceEstimatedOutTime + TimeSpan.Parse(shift.OutGrace))) > 0)
                                                attendance.OverTimeHours = attendance.OverTimeHours + (attendanceOutTime - attendanceEstimatedOutTime).Ticks;
                                        }
                                        else
                                        {
                                            if (DateTime.Compare(attendanceOutTime.AddDays(1), (attendanceEstimatedOutTime.AddDays(1) + TimeSpan.Parse(shift.OutGrace))) > 0)
                                                attendance.OverTimeHours = attendance.OverTimeHours + (attendanceOutTime.AddDays(1) - attendanceEstimatedOutTime.AddDays(1)).Ticks;
                                        }
                                    }

                                    //Check if it is Holiday for the employee then mark it as a special day.
                                    if (employeeCalendarMappings.Where(x => x.Date.CompareTo(dateTime) == 0).Count() > 0)
                                        attendance.IsSpecialDay = true;

                                    //Check if it is WeeklyOff for the employee and update the AttnFlag.
                                    else if (employeeWeeklyOffs.Where(x => x.Date.CompareTo(dateTime) == 0).Count() > 0)
                                        attendance.IsSpecialDay = true;

                                    attendance.IsApproved = true;
                                    _context.TNAEmployeeAttendance.Update(attendance);
                                }

                                dateTime = dateTime.AddDays(1);
                            }
                        }

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }

                    Log.Info("----Info ApproveEmployeeAttendance method end----");
                    return ApiMessageInfo.Status(1, i);
                }
                catch (Exception ex)
                {
                    Log.Error("Error in ApproveEmployeeAttendance Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }

    #endregion

    #region ConsolidateEmployeeAttendance

    public class ConsolidateEmployeeAttendance : IRequest<List<TblTNATrnConsolidatedEmployeeAttendanceDto>>
    {
        public UserIdentityDto User { get; set; }
        public EmployeeAttendanceFilter Input { get; set; }
    }

    public class ConsolidateEmployeeAttendanceHandler : IRequestHandler<ConsolidateEmployeeAttendance, List<TblTNATrnConsolidatedEmployeeAttendanceDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public ConsolidateEmployeeAttendanceHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TblTNATrnConsolidatedEmployeeAttendanceDto>> Handle(ConsolidateEmployeeAttendance request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info ConsolidateEmployeeAttendance method start----");
                    bool isArab = request.User.Culture.IsArab();
                    List<TblTNATrnConsolidatedEmployeeAttendance> objConsolidatedEmployeeAttendanceList = new();
                    List<TblTNATrnConsolidatedEmployeeAttendanceDto> dToConsolidatedEmployeeAttendanceList = new();

                    //Retrieve PayrollGroup details.
                    var payrollGroupDetails = await _context.PayrollGroups.AsNoTracking()
                        .FirstOrDefaultAsync(e => e.PayrollGroupCode == request.Input.PayrollGroupCode);

                    payrollGroupDetails.PayrollGroupEndDate = payrollGroupDetails.PayrollGroupEndDate.AddDays(1).AddSeconds(-1);

                    //Retrieve distinct Employees having attendance in the current payroll period based on PayrollGroup and Branch.
                    var employees = await (from attendanceEmployee in _context.TNAEmployeeAttendance
                                           join contract in _context.EmployeeContracts on attendanceEmployee.EmployeeID equals contract.EmployeeID
                                           select new Employee
                                           {
                                               EmployeeID = attendanceEmployee.EmployeeID,
                                               BranchCode = contract.BranchCode,
                                               PayrollGroupCode = contract.PayrollGroupCode,
                                               Date = attendanceEmployee.Date,
                                               HolidayCalendarCode = contract.HolidayCalendarCode
                                           })
                                           .AsNoTracking()
                                           .Where(e => (e.PayrollGroupCode.Contains(request.Input.PayrollGroupCode) && e.BranchCode.Contains(request.Input.BranchCode)
                                           && e.Date.CompareTo(payrollGroupDetails.PayrollGroupStartDate) >= 0 && e.Date.CompareTo(payrollGroupDetails.PayrollGroupEndDate) <= 0))
                                           .OrderByDescending(x => x.EmployeeID)
                                           .Distinct()
                                           .ToListAsync();

                    //Retrieve distinct Employees based on PayrollGroup and Branch from Contract Info.
                    var contractEmployees = await (from contract in _context.EmployeeContracts
                                                   join personalInformation in _context.PersonalInformation on contract.EmployeeID equals personalInformation.Id
                                                   join companyBranches in _context.CompanyBranches on contract.BranchCode equals companyBranches.BranchCode
                                                   select new Employee
                                                   {
                                                       EmployeeID = contract.EmployeeID,
                                                       EmployeeName = isArab ? string.Concat(personalInformation.FirstNameAr, " ", personalInformation.LastNameAr) : string.Concat(personalInformation.FirstNameEn, " ", personalInformation.LastNameEn),
                                                       BranchCode = contract.BranchCode,
                                                       BranchName = companyBranches.BranchName,
                                                       PayrollGroupCode = contract.PayrollGroupCode,
                                                       HolidayCalendarCode = contract.HolidayCalendarCode
                                                   })
                                                   .AsNoTracking()
                                                   .Where(e => (e.PayrollGroupCode.Contains(request.Input.PayrollGroupCode)
                                                   && e.BranchCode.Contains(request.Input.BranchCode)))
                                                   .OrderByDescending(x => x.EmployeeID)
                                                   .ToListAsync();

                    if (request.Input.EmployeeName is not null)
                        contractEmployees = contractEmployees.Where(x => x.EmployeeName.Contains(request.Input.EmployeeName)).ToList();

                    contractEmployees = contractEmployees.Where(x => employees.Exists(p => p.EmployeeID == x.EmployeeID)).ToList();

                    if (contractEmployees is not null)
                    {
                        objConsolidatedEmployeeAttendanceList = new List<TblTNATrnConsolidatedEmployeeAttendance>();
                        dToConsolidatedEmployeeAttendanceList = new List<TblTNATrnConsolidatedEmployeeAttendanceDto>();

                        //Loop through each employee and consolidate his attendance.
                        foreach (var employee in contractEmployees)
                        {
                            //Retrieve employee's attendance in the current payroll period.
                            var employeeAttendance = await _context.TNAEmployeeAttendance
                                .AsNoTracking()
                                .ProjectTo<TblTNATrnEmployeeAttendanceDto>(_mapper.ConfigurationProvider)
                                .Where(x => (x.EmployeeID == employee.EmployeeID && x.ShiftNumber == 1 && x.IsApproved &&
                                x.Date.CompareTo(payrollGroupDetails.PayrollGroupStartDate) >= 0 &&
                                x.Date.CompareTo(payrollGroupDetails.PayrollGroupEndDate) <= 0))
                                .OrderBy(x => x.Date).ToListAsync(cancellationToken);

                            //Retrieve holiday applicable for the employee based on HolidayCalendarCode
                            var employeeCalendarMappings = await (from holidayCalendarMappings in _context.HolidayCalendarMappings
                                                                  join holidays in _context.Holidays on holidayCalendarMappings.HolidayCode equals holidays.HolidayCode
                                                                  select new
                                                                  {
                                                                      HolidayCalendarCode = holidayCalendarMappings.HolidayCalendarCode,
                                                                      HolidayCode = holidayCalendarMappings.HolidayCode,
                                                                      Date = holidays.Date
                                                                  })
                                                              .AsNoTracking()
                                                              .Where(p => p.HolidayCalendarCode == employee.HolidayCalendarCode && p.Date.CompareTo(payrollGroupDetails.PayrollGroupStartDate) >= 0 && p.Date.CompareTo(payrollGroupDetails.PayrollGroupEndDate) <= 0)
                                                              .OrderByDescending(p => p.Date)
                                                              .ToListAsync(cancellationToken);

                            //Retrieve Employee weekly offs between PayrollGroup StartDate and EndDate.
                            var employeeWeeklyOffs = await _context.EmployeeWeeklyOffs.AsNoTracking()
                            .Where(x => (x.EmployeeID == employee.EmployeeID && x.Date.CompareTo(payrollGroupDetails.PayrollGroupStartDate) >= 0 && x.Date.CompareTo(payrollGroupDetails.PayrollGroupEndDate) <= 0))
                            .OrderBy(x => x.Date)
                            .ToListAsync(cancellationToken);

                            if (employeeAttendance is not null)
                            {
                                var objConsolidatedEmployeeAttendance = new TblTNATrnConsolidatedEmployeeAttendance();
                                objConsolidatedEmployeeAttendance.EmployeeID = employee.EmployeeID;
                                objConsolidatedEmployeeAttendance.PayrollPeriodCode = payrollGroupDetails.PayrollGroupEndDate.ToString("MMMM yyyy");
                                objConsolidatedEmployeeAttendance.TotalDays = (payrollGroupDetails.PayrollGroupEndDate - payrollGroupDetails.PayrollGroupStartDate).Days + 1;
                                objConsolidatedEmployeeAttendance.TotalOffDays = employeeWeeklyOffs.ToList().Count();
                                objConsolidatedEmployeeAttendance.TotalHolidays = employeeCalendarMappings.ToList().Count();
                                objConsolidatedEmployeeAttendance.TotalPresentDays = employeeAttendance.Where(e => e.AttnFlag == "P").Count();
                                objConsolidatedEmployeeAttendance.TotalAbsents = (objConsolidatedEmployeeAttendance.TotalDays - (objConsolidatedEmployeeAttendance.TotalPresentDays + objConsolidatedEmployeeAttendance.TotalOffDays + objConsolidatedEmployeeAttendance.TotalHolidays));
                                objConsolidatedEmployeeAttendance.NetWorkingDays = employeeAttendance.Where(e => e.AttnFlag == "P").Count();
                                objConsolidatedEmployeeAttendance.TotalLateDays = employeeAttendance.Where(e => e.IsLate).ToList().Count();
                                objConsolidatedEmployeeAttendance.TotalLateHours = employeeAttendance.Where(e => e.AttnFlag == "P").Sum(e => e.LateHours);
                                objConsolidatedEmployeeAttendance.NormalOTHours = employeeAttendance.Where(e => !e.IsSpecialDay).Sum(e => e.OverTimeHours);
                                objConsolidatedEmployeeAttendance.SpecialOTHours = employeeAttendance.Where(e => e.IsSpecialDay).Sum(e => e.OverTimeHours);
                                objConsolidatedEmployeeAttendance.ShiftNumber = 1;
                                objConsolidatedEmployeeAttendanceList.Add(objConsolidatedEmployeeAttendance);
                            }
                        }

                        if (objConsolidatedEmployeeAttendanceList.Count() > 0)
                        {
                            //Retrieve employee's consolidated attendance.
                            var consolidatedEmployeeAttendance = await _context.ConsolidatedEmployeeAttendance
                                .Where(e => (e.PayrollPeriodCode == payrollGroupDetails.PayrollGroupEndDate.ToString("MMMM yyyy")))
                                .ToListAsync();

                            //Delete employee's consolidated attendance
                            if (consolidatedEmployeeAttendance is not null)
                                _context.ConsolidatedEmployeeAttendance.RemoveRange(consolidatedEmployeeAttendance);

                            await _context.ConsolidatedEmployeeAttendance.AddRangeAsync(objConsolidatedEmployeeAttendanceList);

                            //Convert entities to DataType Objects.
                            dToConsolidatedEmployeeAttendanceList.AddRange(_mapper.Map<List<TblTNATrnConsolidatedEmployeeAttendanceDto>>(objConsolidatedEmployeeAttendanceList));

                            //Loop through each employee to update Name, BranchCode and Branch Name.
                            dToConsolidatedEmployeeAttendanceList.ForEach(e =>
                            {
                                var employee = contractEmployees.Where(p => p.EmployeeID == e.EmployeeID).ToList().FirstOrDefault();
                                if (employee is not null)
                                {
                                    e.EmployeeName = employee.EmployeeName;
                                    e.BranchCode = employee.BranchCode;
                                    e.BranchName = employee.BranchName;
                                }
                            });
                        }

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }

                    Log.Info("----Info ConsolidateEmployeeAttendance method end----");
                    return dToConsolidatedEmployeeAttendanceList;
                }
                catch (Exception ex)
                {
                    Log.Error("Error in ConsolidateEmployeeAttendance Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    throw;
                }
            }
        }
    }

    #endregion
}
