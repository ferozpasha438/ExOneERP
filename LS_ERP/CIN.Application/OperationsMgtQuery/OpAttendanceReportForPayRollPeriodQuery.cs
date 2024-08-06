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







    #region AttendanceReportForPayRollPeriodQuery

    public class AttendanceReportForPayRollPeriodQuery : IRequest<Output_OpAttendanceReportForPayRollPeriodDto>
    {
        public UserIdentityDto User { get; set; }
        public Input_OpAttendanceReportForPayRollPeriodDto Input { get; set; }
    }

    public class AttendanceReportForPayRollPeriodQueryHandler : IRequestHandler<AttendanceReportForPayRollPeriodQuery, Output_OpAttendanceReportForPayRollPeriodDto>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public AttendanceReportForPayRollPeriodQueryHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<Output_OpAttendanceReportForPayRollPeriodDto> Handle(AttendanceReportForPayRollPeriodQuery request, CancellationToken cancellationToken)
        {
            try
            {

                bool isArabic = request.User.Culture.IsArab();

                var ProjectSite = await _context.TblOpProjectSites.FirstOrDefaultAsync(e => e.ProjectCode == request.Input.ProjectCode && e.SiteCode == request.Input.SiteCode);
                int SiteWorkingTime = ProjectSite is null ? 0 : ProjectSite.SiteWorkingHours.Value * 60; //mins

                DateTime startDate = Convert.ToDateTime(request.Input.PayrollStartDate, CultureInfo.InvariantCulture);
                bool IsSingleMonth = request.Input.PayrollStartDate.Day == 1;
                if (startDate.Day > 28)
                {
                    return new() { IsValidReq = false, ErrorMsg = "Invalid Date (Date Should Between 1-25)" };
                }
                DateTime endDateTemp = IsSingleMonth ? new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month)) : new DateTime(startDate.Month < 12 ? startDate.Year : startDate.Year + 1, startDate.Month < 12 ? startDate.Month + 1 : 1, startDate.Day - 1);
                DateTime endDate = Convert.ToDateTime(endDateTemp, CultureInfo.InvariantCulture);

                Output_OpAttendanceReportForPayRollPeriodDto Res = new()
                {
                    SiteWorkingTime = SiteWorkingTime,
                    Columns = new(),
                    Rows = new(),

                    IsSingleMonth = IsSingleMonth,
                    IsValidReq = false,
                    DaysInMonth1 = IsSingleMonth ? DateTime.DaysInMonth(startDate.Year, startDate.Month) : DateTime.DaysInMonth(startDate.Year, startDate.Month) - startDate.Day + 1,
                    DaysInMonth2 = IsSingleMonth ? 0 : endDate.Day,
                    ErrorMsg = "Processing Data Failed",
                    Month1Text = startDate.ToString("MMM"),
                    Month2Text = endDate.ToString("MMM"),
                    Year1Text = startDate.Year.ToString(),//.ToString("yyyy"),
                    Year2Text = endDate.Year.ToString(),//.ToString("yyyy"),


                };

                var Holidays = await _contextDMC.HRM_DEF_Holidays.Where(e => e.HolidayDate >= startDate && e.HolidayDate <= endDate).ToListAsync();



                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    Column_OpAttendanceReportForPayRollPeriodDto column = new()
                    {
                        AttnDate = date,
                        Day = date.Day,
                        Month = date.Month,
                        Year = date.Year,
                        DayText = date.ToString().Substring(0, 1).ToUpper(),
                        IsHoliday = Holidays.Any(e => e.HolidayDate.Value == date),
                        HolidayInf = Holidays.FirstOrDefault(e => e.HolidayDate.Value == date) ?? new(),
                    };
                    Res.Columns.Add(column);

                }


                var AttendanceList = await _context.EmployeeAttendance.Where(e => e.ProjectCode == request.Input.ProjectCode
                && e.SiteCode == request.Input.SiteCode
                && e.AttnDate >= startDate && e.AttnDate <= endDate
                /*&& e.ShiftCode != "O"*/
                && (e.EmployeeNumber == request.Input.EmployeeNumber || request.Input.EmployeeNumber == "")

                ).ToListAsync();


                var AltAttendanceList = await _context.EmployeeAttendance.Where(e => e.ProjectCode == request.Input.ProjectCode
                && e.SiteCode == request.Input.SiteCode
                && e.AttnDate >= startDate && e.AttnDate <= endDate
                /*&& e.ShiftCode != "O"*/
                && (e.RefIdForAlt > 0)

                ).ToListAsync();



                var Employees = AttendanceList.OrderBy(e => e.EmployeeNumber).ThenBy(e => e.ShiftCode).GroupBy(e => e.EmployeeNumber).ToList();

                var Shifts = AttendanceList.OrderBy(e => e.AttnDate).GroupBy(e => e.ShiftCode).ToList();


                foreach (var emp in Employees)
                {
                    foreach (var shift in Shifts)
                    {
                        if ((!Res.Rows.Any(e => e.EmployeeNumber == emp.Key && e.ShiftCode == shift.Key)) && AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.ShiftCode == shift.Key))
                        {
                            var employee = await _contextDMC.HRM_TRAN_Employees.FirstOrDefaultAsync(e => e.EmployeeNumber == emp.Key);

                            Row_OpAttendanceReportForPayRollPeriodDto row = new()
                            {
                                ShiftCode = shift.Key,
                                EmployeeNumber = emp.Key,
                                EmployeeName = employee is not null ? employee.EmployeeName : "NA",
                                EmployeeNameAr = employee is not null ? employee.EmployeeName_AR : "NA",
                                UniqueShiftsCount = 0,
                                Position= "SST000026",
                                PositionName = isArabic? "بديل راحات " : "Reliever",
                              
                        };
                            Res.Rows.Add(row);

                        }
                    }
                }

                var Roasters = await _context.TblOpMonthlyRoasterForSites.Where(e =>
                (e.ProjectCode == request.Input.ProjectCode &&
              e.SiteCode == request.Input.SiteCode
            && e.IsPrimaryResource
              )
              && ((e.Month == startDate.Month && e.Year == startDate.Year) || (e.Month == endDate.Month && e.Year == endDate.Year))

              && (e.EmployeeNumber == request.Input.EmployeeNumber || request.Input.EmployeeNumber == "")
              ).ToListAsync();


                


                if (Roasters.Count == 0 )
                {
                    return new() { IsValidReq = true, ErrorMsg = "No Roasters Found", Rows = new(), Columns = new() };
                }

                if (Roasters.Any(r =>
                 r.S1 == "" || r.S2 == "" || r.S3 == "" || r.S4 == "" || r.S5 == "" || r.S6 == "" || r.S7 == "" || r.S8 == "" || r.S9 == "" || r.S10 == "" ||
                 r.S11 == "" || r.S12 == "" || r.S13 == "" || r.S14 == "" || r.S15 == "" || r.S16 == "" || r.S17 == "" || r.S18 == "" || r.S19 == "" || r.S20 == "" ||
                 r.S21 == "" || r.S22 == "" || r.S23 == "" || r.S24 == "" || r.S25 == "" || r.S26 == "" || r.S27 == "" || r.S28 == "" || r.S29 == "" || r.S30 == "" || r.S31 == ""
                ))
                {
                    var incomproastr = Roasters.FirstOrDefault(r =>
                   r.S1 == "" || r.S2 == "" || r.S3 == "" || r.S4 == "" || r.S5 == "" || r.S6 == "" || r.S7 == "" || r.S8 == "" || r.S9 == "" || r.S10 == "" ||
                   r.S11 == "" || r.S12 == "" || r.S13 == "" || r.S14 == "" || r.S15 == "" || r.S16 == "" || r.S17 == "" || r.S18 == "" || r.S19 == "" || r.S20 == "" ||
                   r.S21 == "" || r.S22 == "" || r.S23 == "" || r.S24 == "" || r.S25 == "" || r.S26 == "" || r.S27 == "" || r.S28 == "" || r.S29 == "" || r.S30 == "" || r.S31 == ""
                  );
                    Res.IsExistEmptyShifts = true;
                }

                if (Employees.Count > 0)
                {

                    var Relivers = Employees.Where(e => !Roasters.Select(r => r.EmployeeNumber).Contains(e.Key)).ToList();


                    foreach (var emp in Relivers)
                    {
                        TblOpMonthlyRoasterForSite AltRoaster = new();

                        AltRoaster.EmployeeNumber = emp.Key;
                        AltRoaster.EmployeeID = 0;
                        AltRoaster.MonthEndDate = startDate;
                        AltRoaster.MonthStartDate = endDate;
                        AltRoaster.S1  ="x";
                        AltRoaster.S2  ="x";
                        AltRoaster.S3  ="x";
                        AltRoaster.S4  ="x";
                        AltRoaster.S5  ="x";
                        AltRoaster.S6  ="x";
                        AltRoaster.S7  ="x";
                        AltRoaster.S8  ="x";
                        AltRoaster.S9  ="x";
                        AltRoaster.S10 ="x";
                        AltRoaster.S11 ="x";
                        AltRoaster.S12 ="x";
                        AltRoaster.S13 ="x";
                        AltRoaster.S14 ="x";
                        AltRoaster.S15 ="x";
                        AltRoaster.S16 ="x";
                        AltRoaster.S17 ="x";
                        AltRoaster.S18 ="x";
                        AltRoaster.S19 ="x";
                        AltRoaster.S20 ="x";
                        AltRoaster.S21 ="x";
                        AltRoaster.S22 ="x";
                        AltRoaster.S23 ="x";
                        AltRoaster.S24 ="x";
                        AltRoaster.S25 ="x";
                        AltRoaster.S26 ="x";
                        AltRoaster.S27 ="x";
                        AltRoaster.S28 ="x";
                        AltRoaster.S29 ="x";
                        AltRoaster.S30 ="x";
                        AltRoaster.S31 = "x";
                        // AltRoaster.S1 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1).ShiftCode : "x";
                        //AltRoaster.S2 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 2).ShiftCode : "x";
                        //AltRoaster.S3 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 3).ShiftCode : "x";
                        //AltRoaster.S4 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 4).ShiftCode : "x";
                        //AltRoaster.S5 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 5).ShiftCode : "x";
                        //AltRoaster.S6 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 6).ShiftCode : "x";
                        //AltRoaster.S7 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 7).ShiftCode : "x";
                        //AltRoaster.S8 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 8).ShiftCode : "x";
                        //AltRoaster.S9 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 9).ShiftCode : "x";
                        //AltRoaster.S10 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 10).ShiftCode : "x";
                        //AltRoaster.S11 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 11).ShiftCode : "x";
                        //AltRoaster.S12 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 12).ShiftCode : "x";
                        //AltRoaster.S13 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 13).ShiftCode : "x";
                        //AltRoaster.S14 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 14).ShiftCode : "x";
                        //AltRoaster.S15 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 15).ShiftCode : "x";
                        //AltRoaster.S16 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 16).ShiftCode : "x";
                        //AltRoaster.S17 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 17).ShiftCode : "x";
                        //AltRoaster.S18 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 18).ShiftCode : "x";
                        //AltRoaster.S19 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 19).ShiftCode : "x";
                        //AltRoaster.S20 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 20).ShiftCode : "x";
                        //AltRoaster.S21 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 21).ShiftCode : "x";
                        //AltRoaster.S22 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 22).ShiftCode : "x";
                        //AltRoaster.S23 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 23).ShiftCode : "x";
                        //AltRoaster.S24 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 24).ShiftCode : "x";
                        //AltRoaster.S25 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 25).ShiftCode : "x";
                        //AltRoaster.S26 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 26).ShiftCode : "x";
                        //AltRoaster.S27 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 27).ShiftCode : "x";
                        //AltRoaster.S28 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 28).ShiftCode : "x";
                        //AltRoaster.S29 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 29).ShiftCode : "x";
                        //AltRoaster.S30 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 30).ShiftCode : "x";
                        //AltRoaster.S31 = AttendanceList.Any(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 1) ? AttendanceList.FirstOrDefault(e => e.EmployeeNumber == emp.Key && e.AttnDate.Value.Day == 31).ShiftCode : "x";
                        //AltRoaster.ProjectCode = request.Input.ProjectCode;
                        AltRoaster.SiteCode = request.Input.SiteCode;
                        AltRoaster.IsPrimaryResource = false;
                        AltRoaster.MapId = 0;
                        AltRoaster.Month = (short)startDate.Month;
                        AltRoaster.Year = (short)startDate.Year;
                        AltRoaster.SkillsetCode = "SST000026";
                        AltRoaster.SkillsetName = isArabic? "بديل راحات " : "Reliever";
                        AltRoaster.CustomerCode = "";
                        AltRoaster.Id = 0;
                        Roasters.Add(AltRoaster);
                    }
                }




                var HrmEmployees = await _contextDMC.HRM_TRAN_Employees.Where(e => Roasters.Select(r => r.EmployeeNumber).Contains(e.EmployeeNumber)).ToListAsync();


                foreach (var r in Roasters)
                {
                    for (var i = 1; i <= 31; i++)
                    {

                        string ShiftCode = "";


                        switch (i)
                        {
                            case 1: ShiftCode = r.S1; break;
                            case 2: ShiftCode = r.S2; break;
                            case 3: ShiftCode = r.S3; break;
                            case 4: ShiftCode = r.S4; break;
                            case 5: ShiftCode = r.S5; break;
                            case 6: ShiftCode = r.S6; break;
                            case 7: ShiftCode = r.S7; break;
                            case 8: ShiftCode = r.S8; break;
                            case 9: ShiftCode = r.S9; break;
                            case 10: ShiftCode = r.S10; break;
                            case 11: ShiftCode = r.S11; break;
                            case 12: ShiftCode = r.S12; break;
                            case 13: ShiftCode = r.S13; break;
                            case 14: ShiftCode = r.S14; break;
                            case 15: ShiftCode = r.S15; break;
                            case 16: ShiftCode = r.S16; break;
                            case 17: ShiftCode = r.S17; break;
                            case 18: ShiftCode = r.S18; break;
                            case 19: ShiftCode = r.S19; break;
                            case 20: ShiftCode = r.S20; break;
                            case 21: ShiftCode = r.S21; break;
                            case 22: ShiftCode = r.S22; break;
                            case 23: ShiftCode = r.S23; break;
                            case 24: ShiftCode = r.S24; break;
                            case 25: ShiftCode = r.S25; break;
                            case 26: ShiftCode = r.S26; break;
                            case 27: ShiftCode = r.S27; break;
                            case 28: ShiftCode = r.S28; break;
                            case 29: ShiftCode = r.S29; break;
                            case 30: ShiftCode = r.S30; break;
                            case 31: ShiftCode = r.S31; break;

                        }

                        if (!(Res.Rows.Any(e => e.EmployeeNumber == r.EmployeeNumber && e.ShiftCode == ShiftCode)) && ShiftCode != "" && ShiftCode != string.Empty && ShiftCode != "x" /*&& ShiftCode!="O"*/)
                        {
                            DateTime date = new DateTime(r.Year, r.Month, i);

                            if (date >= startDate && date <= endDate)
                            {
                                //var employee =await _contextDMC.HRM_TRAN_Employees.FirstOrDefaultAsync(e => e.EmployeeNumber == r.EmployeeNumber);
                                var employee = HrmEmployees.FirstOrDefault(e => e.EmployeeNumber == r.EmployeeNumber);
                                Row_OpAttendanceReportForPayRollPeriodDto row = new()
                                {
                                    ShiftCode = ShiftCode,
                                    EmployeeNumber = r.EmployeeNumber,
                                    EmployeeName = employee is not null ? employee.EmployeeName : "NA",
                                    EmployeeNameAr = employee is not null ? employee.EmployeeName_AR : "NA",
                                    UniqueShiftsCount = 0,
                                    Position= "SST000026",
                                    PositionName= "SST000026",
                                };
                                Res.Rows.Add(row);
                            }
                        }
                    }
                }



                Res.Rows = Res.Rows.OrderBy(rr => rr.EmployeeNumber).ThenBy(rr => rr.ShiftCode).ToList();



                Res.Columns = Res.Columns.OrderBy(dd => dd.AttnDate).ToList();


                var DistinctEmployees = Res.Rows.Select(e => e.EmployeeNumber).Distinct();


                Res.TotalEmployeesCount = DistinctEmployees.Count();
                DistinctEmployees = DistinctEmployees.Skip(request.Input.PageNumber * request.Input.PageSize).Take(request.Input.PageSize).ToList();
                Res.Rows = Res.Rows.Where(e => DistinctEmployees.Contains(e.EmployeeNumber)).ToList();
                Res.Rows.ForEach(r =>
                {
                    r.IsNewHire = !(_context.EmployeeAttendance.AsNoTracking().Any(e => e.EmployeeNumber == r.EmployeeNumber && e.AttnDate < startDate && e.isPosted));
                    r.Position = Roasters.FirstOrDefault(e => e.EmployeeNumber == r.EmployeeNumber).SkillsetCode;
                    r.PositionName = isArabic ? _context.TblOpSkillsets.FirstOrDefault(e => e.SkillSetCode == r.Position).NameInArabic : _context.TblOpSkillsets.FirstOrDefault(e => e.SkillSetCode == r.Position).NameInEnglish;
                });


                var TransResignTerminations = await _context.EmployeeTransResign.Where(e => DistinctEmployees.Contains(e.EmployeeNumber) &&
             e.ProjectCode == request.Input.ProjectCode &&
                        e.SiteCode == request.Input.SiteCode && e.AttnDate.Value.AddDays(31)>=startDate && e.AttnDate<=endDate
                        ).ToListAsync();



                var ShiftMaster = await _contextDMC.HRM_DEF_EmployeeShiftMasters.ToListAsync();

                var EmployeeLeaves = await _context.EmployeeLeaves.Where(e => DistinctEmployees.Contains(e.EmployeeNumber)
                && e.AttnDate <= endDate && e.AttnDate >= startDate).ToListAsync();

                var EmployeeMaster = await _contextDMC.HRM_TRAN_Employees.Where(e => DistinctEmployees.Contains(e.EmployeeNumber)).ToListAsync();

                var EmployeeOffs = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e => EmployeeMaster.Select(e => e.EmployeeID).Contains(e.EmployeeId)
                  && e.Date <= endDate && e.Date >= startDate).ToListAsync();

                foreach (var row in Res.Rows)
                {


                    var Shift = await _contextDMC.HRM_DEF_EmployeeShiftMasters.FirstOrDefaultAsync(e => e.ShiftCode == row.ShiftCode);

                    row.AttendanceMatrix = new();
                    foreach (var col in Res.Columns)
                    {

                        bool isAttendanceexist = AttendanceList.Any(e => e.EmployeeNumber == row.EmployeeNumber && e.ShiftCode == row.ShiftCode && e.AttnDate == col.AttnDate);
                        AttendanceReportCellData attendance = new();

                       
                        attendance.IsTransfered = TransResignTerminations.Any(e => e.ProjectCode == request.Input.ProjectCode &&
                        e.SiteCode == request.Input.SiteCode && e.EmployeeNumber == row.EmployeeNumber && e.AttnDate <= col.AttnDate 
                        && e.TR
                        );
                     
                        attendance.IsResigned = TransResignTerminations.Any(e => 
                         //e.ProjectCode == request.Input.ProjectCode &&
                        //e.SiteCode == request.Input.SiteCode && 
                        e.EmployeeNumber == row.EmployeeNumber && e.AttnDate <= col.AttnDate 
                        && e.R
                        );
                        attendance.IsTerminated = TransResignTerminations.Any(e => 
                        //e.ProjectCode == request.Input.ProjectCode &&
                        //e.SiteCode == request.Input.SiteCode && 
                        e.EmployeeNumber == row.EmployeeNumber && e.AttnDate <= col.AttnDate
                        && (!e.R && !e.TR)
                        );

                        if (isAttendanceexist)
                        {


                            var attnd = AttendanceList.FirstOrDefault(e => e.EmployeeNumber == row.EmployeeNumber && e.ShiftCode == row.ShiftCode && e.AttnDate == col.AttnDate);
                            attendance.AttendedTime = attnd.Attendance == "P" || attnd.Attendance == "OT" ? attnd.InTime < attnd.OutTime ? (int)(attnd.OutTime - attnd.InTime).TotalMinutes : (int)(24 * 60 - (attnd.InTime - attnd.OutTime).TotalMinutes) : 0;
                            attendance.AttnFlag = attnd.Attendance;
                            attendance.AttndId = attnd.Id;
                            attendance.RefIdForAlt = attnd.RefIdForAlt ?? 0;

                            attendance.IsPosted = attnd.isPosted;

                            var LeaveData = EmployeeLeaves.FirstOrDefault(e => e.AttnDate == col.AttnDate && e.EmployeeNumber == row.EmployeeNumber);
                            if (LeaveData is not null)
                            {
                                attendance.IsOnLeave = true;
                                attendance.AttnFlag = LeaveData.AL ? "AL" : LeaveData.EL ? "EL" : LeaveData.SL ? "SL" : LeaveData.STL ? "STL" : LeaveData.UL ? "UL" : LeaveData.W ? "W" : "L";

                            }


                            //var AltAtt= await _context.EmployeeAttendance.FirstOrDefaultAsync(e => e.RefIdForAlt == attnd.Id);
                            //var AltAtt = AttendanceList.FirstOrDefault(e => e.RefIdForAlt == attnd.Id);
                            var AltAtt = AltAttendanceList.FirstOrDefault(e => e.RefIdForAlt == attnd.Id);

                            if (AltAtt is not null)
                            {
                                attendance.AltAttId = AltAtt.Id;
                                attendance.AltEmployeeNumber = AltAtt.EmployeeNumber;
                                attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes;
                                attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes;
                            }
                            else if (attendance.RefIdForAlt > 0)
                            {
                                attendance.AllottedShiftTime = 0;
                                attendance.ContractualShiftTime = 0;
                            }
                            else
                            {
                                attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes;
                                attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes;
                            }
                        }
                        else
                        {
                            attendance.IsPosted = false;
                            attendance.AttendedTime = 0;
                            var roasters = Roasters.Where(e => e.EmployeeNumber == row.EmployeeNumber && (e.Month == col.AttnDate.Month && e.Year == col.AttnDate.Year)).ToList();
                            if (roasters.Count == 0)
                            {
                                attendance.AttnFlag = "";
                            }
                            else
                            {
                                switch (col.AttnDate.Day)
                                {
                                    case 1: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S1 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 2: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S2 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 3: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S3 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 4: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S4 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 5: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S5 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 6: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S6 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 7: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S7 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 8: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S8 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 9: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S9 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 10: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S10 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 11: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S11 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 12: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S12 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 13: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S13 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 14: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S14 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 15: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S15 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 16: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S16 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 17: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S17 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 18: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S18 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 19: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S19 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 20: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S20 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 21: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S21 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 22: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S22 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 23: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S23 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 24: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S24 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 25: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S25 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 26: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S26 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 27: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S27 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 28: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S28 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 29: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S29 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 30: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S30 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                    case 31: attendance.AttnFlag = roasters.FirstOrDefault(e => e.S31 == row.ShiftCode) is null ? "" : "-"; if (attendance.AttnFlag == "-") { attendance.AllottedShiftTime = (int)Shift.WorkingTime.Value.TotalMinutes; attendance.ContractualShiftTime = row.ShiftCode == "O" ? SiteWorkingTime : (int)Shift.WorkingTime.Value.TotalMinutes; } break;
                                }

                                if (attendance.AttnFlag == "-")
                                {

                                    //   var LeaveData = await _context.EmployeeLeaves.FirstOrDefaultAsync(e => e.AttnDate == col.AttnDate && e.EmployeeNumber == row.EmployeeNumber);
                                    var LeaveData = EmployeeLeaves.FirstOrDefault(e => e.AttnDate == col.AttnDate && e.EmployeeNumber == row.EmployeeNumber);
                                    if (LeaveData is not null)
                                    {
                                        attendance.IsOnLeave = true;
                                        attendance.AttnFlag = LeaveData.AL ? "AL" : LeaveData.EL ? "EL" : LeaveData.SL ? "SL" : LeaveData.STL ? "STL" : LeaveData.UL ? "UL" : LeaveData.W ? "W" : "L";

                                    }



                                }
                                else
                                {
                                    //var emp = await _contextDMC.HRM_TRAN_Employees.FirstOrDefaultAsync(e => e.EmployeeNumber == row.EmployeeNumber);
                                    var emp = HrmEmployees.FirstOrDefault(e => e.EmployeeNumber == row.EmployeeNumber);
                                    //var Off = await _contextDMC.HRM_DEF_EmployeeOffs.FirstOrDefaultAsync(e => e.EmployeeId == emp.EmployeeID && e.Date == col.AttnDate);
                                    var Off = EmployeeOffs.FirstOrDefault(e => e.EmployeeId == emp.EmployeeID && e.Date == col.AttnDate);
                                    if (Off is not null)
                                    {
                                        attendance.AttnFlag = "";
                                        attendance.AllottedShiftTime = 0;
                                        attendance.AttendedTime = 0;

                                    }
                                }
                            }




                        }

                        row.AttendanceMatrix.Add(attendance);
                    }


                }
                foreach (var emp in DistinctEmployees)
                {
                    var rows = Res.Rows.Where(e => e.EmployeeNumber == emp).ToList();
                    rows.FirstOrDefault().UniqueShiftsCount = rows.Count();
                }
                Res.IsValidReq = true;

                return Res;
            }
            catch (Exception ex)
            {

                Log.Error("Error in AttendanceReportForPayRollPeriodQuery Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return new() { IsValidReq = false, ErrorMsg = ex.Message };

            }
        }
    }


    #endregion



    #region RoasterReportForPayRollPeriodQuery

    public class RoasterReportForPayRollPeriodQuery : IRequest<Output_OpRoasterReportForPayRollPeriodDto>
    {
        public UserIdentityDto User { get; set; }
        public Input_OpAttendanceReportForPayRollPeriodDto Input { get; set; }
    }

    public class RoasterReportForPayRollPeriodQueryHandler : IRequestHandler<RoasterReportForPayRollPeriodQuery, Output_OpRoasterReportForPayRollPeriodDto>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public RoasterReportForPayRollPeriodQueryHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<Output_OpRoasterReportForPayRollPeriodDto> Handle(RoasterReportForPayRollPeriodQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var isArab = request.User.Culture.IsArab();

                var ProjectSite = await _context.TblOpProjectSites.FirstOrDefaultAsync(e => e.ProjectCode == request.Input.ProjectCode && e.SiteCode == request.Input.SiteCode);
                int SiteWorkingTime = ProjectSite is null ? 0 : ProjectSite.SiteWorkingHours.Value * 60; //mins

                DateTime startDate = Convert.ToDateTime(request.Input.PayrollStartDate, CultureInfo.InvariantCulture);
                bool IsSingleMonth = request.Input.PayrollStartDate.Day == 1;
                if (startDate.Day > 28)
                {
                    return new() { };
                }
                DateTime endDateTemp = IsSingleMonth ? new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month)) : new DateTime(startDate.Month < 12 ? startDate.Year : startDate.Year + 1, startDate.Month < 12 ? startDate.Month + 1 : 1, startDate.Day - 1);
                DateTime endDate = Convert.ToDateTime(endDateTemp, CultureInfo.InvariantCulture);
                double noOfDays = (endDate - startDate).TotalDays;
                Output_OpRoasterReportForPayRollPeriodDto Res = new();


                var Roasters = await _context.TblOpMonthlyRoasterForSites.Where(e => (e.ProjectCode == request.Input.ProjectCode &&
                  e.SiteCode == request.Input.SiteCode && e.IsPrimaryResource)
                  && ((e.Month == startDate.Month && e.Year == startDate.Year) || (e.Month == endDate.Month && e.Year == endDate.Year))
                  ).ToListAsync();

                var SkillSets = await _context.TblOpSkillsets.ToListAsync();
                if (Roasters.Count == 0)
                {
                    return new() { IsValidReq = false, ErrorMsg = "No Roasters Found" };
                }
                var KeyRows = Roasters.GroupBy(e => new { e.EmployeeNumber, e.SkillsetCode, e.SkillsetName }).Select(s => new { EmployeeNumber = s.Key.EmployeeNumber, Postion = s.Key.SkillsetCode }).ToList();

                for (int i = 0; i < KeyRows.Count; i++)
                {
                    Output_OpRoasterReportForPayRollPeriodDto row = new()
                    {
                        EmployeeNumber = KeyRows[i].EmployeeNumber,
                        Position = KeyRows[i].Postion,
                        PositionName = isArab ? SkillSets.FirstOrDefault(e => e.SkillSetCode == KeyRows[i].Postion).NameInArabic : SkillSets.FirstOrDefault(e => e.SkillSetCode == KeyRows[i].Postion).NameInEnglish,
                        DailyHrs = SiteWorkingTime / 60,
                        MonthlyHrs = 0,
                        MonthlyOffs = 0,
                        MonthlyShifts = 0,
                        Qty = Roasters.Count(e => e.EmployeeNumber == KeyRows[i].EmployeeNumber && e.MonthStartDate.Value.Month == startDate.Month && e.MonthStartDate.Value.Year == startDate.Year),
                    };

                    for (DateTime dt = startDate; dt <= endDate; dt = dt.AddDays(1))
                    {
                        var rstrs = Roasters.Where(e => e.EmployeeNumber == KeyRows[i].EmployeeNumber && e.SkillsetCode == KeyRows[i].Postion
                        && e.Month==dt.Month && e.Year==dt.Year).ToList();

                        switch (dt.Day)
                        {
                            case 1:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S1 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S1 != "" && s.S1 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S1 != "" && s.S1 != "x") * SiteWorkingTime / 60);

                                break;
                            case 2:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S2 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S2 != "" && s.S2 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S2 != "" && s.S2 != "x") * SiteWorkingTime / 60);

                                break;
                            case 3:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S3 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S3 != "" && s.S3 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S3 != "" && s.S3 != "x") * SiteWorkingTime / 60);

                                break;
                            case 4:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S4 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S4 != "" && s.S4 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S4 != "" && s.S4 != "x") * SiteWorkingTime / 60);

                                break;
                            case 5:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S5 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S5 != "" && s.S5 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S5 != "" && s.S5 != "x") * SiteWorkingTime / 60);

                                break;
                            case 6:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S6 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S6 != "" && s.S6 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S6 != "" && s.S6 != "x") * SiteWorkingTime / 60);

                                break;
                            case 7:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S7 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S7 != "" && s.S7 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S7 != "" && s.S7 != "x") * SiteWorkingTime / 60);

                                break;
                            case 8:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S8 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S8 != "" && s.S8 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S8 != "" && s.S8 != "x") * SiteWorkingTime / 60);

                                break;
                            case 9:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S9 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S9 != "" && s.S9 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S9 != "" && s.S9 != "x") * SiteWorkingTime / 60);

                                break;
                            case 10:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S10 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S10 != "" && s.S10 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S10 != "" && s.S10 != "x") * SiteWorkingTime / 60);

                                break;
                            case 11:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S11 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S11 != "" && s.S11 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S11 != "" && s.S11 != "x") * SiteWorkingTime / 60);

                                break;
                            case 12:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S12 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S12 != "" && s.S12 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S12 != "" && s.S12 != "x") * SiteWorkingTime / 60);

                                break;
                            case 13:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S13 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S13 != "" && s.S13 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S13 != "" && s.S13 != "x") * SiteWorkingTime / 60);

                                break;
                            case 14:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S14 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S14 != "" && s.S14 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S14 != "" && s.S14 != "x") * SiteWorkingTime / 60);

                                break;
                            case 15:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S15 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S15 != "" && s.S15 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S15 != "" && s.S15 != "x") * SiteWorkingTime / 60);

                                break;
                            case 16:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S16 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S16 != "" && s.S16 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S16 != "" && s.S16 != "x") * SiteWorkingTime / 60);

                                break;
                            case 17:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S17 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S17 != "" && s.S17 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S17 != "" && s.S17 != "x") * SiteWorkingTime / 60);

                                break;
                            case 18:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S18 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S18 != "" && s.S18 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S18 != "" && s.S18 != "x") * SiteWorkingTime / 60);

                                break;
                            case 19:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S19 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S19 != "" && s.S19 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S19 != "" && s.S19 != "x") * SiteWorkingTime / 60);

                                break;
                            case 20:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S20 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S20 != "" && s.S20 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S20 != "" && s.S20 != "x") * SiteWorkingTime / 60);

                                break;
                            case 21:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S21 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S21 != "" && s.S21 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S21 != "" && s.S21 != "x") * SiteWorkingTime / 60);

                                break;
                            case 22:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S22 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S22 != "" && s.S22 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S22 != "" && s.S22 != "x") * SiteWorkingTime / 60);

                                break;
                            case 23:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S23 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S23 != "" && s.S23 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S23 != "" && s.S23 != "x") * SiteWorkingTime / 60);

                                break;
                            case 24:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S24 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S24 != "" && s.S24 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S24 != "" && s.S24 != "x") * SiteWorkingTime / 60);

                                break;
                            case 25:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S25 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S25 != "" && s.S25 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S25 != "" && s.S25 != "x") * SiteWorkingTime / 60);

                                break;
                            case 26:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S26 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S26 != "" && s.S26 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S26 != "" && s.S26 != "x") * SiteWorkingTime / 60);

                                break;
                            case 27:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S27 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S27 != "" && s.S27 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S27 != "" && s.S27 != "x") * SiteWorkingTime / 60);

                                break;
                            case 28:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S28 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S3 != "" && s.S3 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S3 != "" && s.S3 != "x") * SiteWorkingTime / 60);

                                break;
                            case 29:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S29 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S29 != "" && s.S29 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S29 != "" && s.S29 != "x") * SiteWorkingTime / 60);

                                break;
                            case 30:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S30 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S30 != "" && s.S30 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S30 != "" && s.S30 != "x") * SiteWorkingTime / 60);

                                break;
                            case 31:
                                row.MonthlyOffs += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S31 == "O");
                                row.MonthlyShifts += rstrs.Count == 0 ? 0 : rstrs.Count(s => s.S31 != "" && s.S31 != "x");
                                row.MonthlyHrs += rstrs.Count == 0 ? 0 : (rstrs.Count(s => s.S31 != "" && s.S31 != "x") * SiteWorkingTime / 60);

                                break;

                        }


                    }
                    Res.Rows.Add(row);
                }
                if (Res.Rows.Count == 0)
                    return new() { IsValidReq = false, ErrorMsg = "No Roasters Found" };

                var rowsByPosition = Res.Rows.GroupBy(e => e.Position).Select(g => new { Position = g.Key, Group = g.ToList() }).ToList();
                rowsByPosition.ForEach(g => {
                    int qty = Roasters.Count(e => e.SkillsetCode == g.Position && e.Month == startDate.Month && e.Year == startDate.Year);
                    Res.RowsByPositions.Add(new()
                    {
                        Qty = qty,
                        MonthlyHrs = g.Group.Sum(m => m.MonthlyHrs),
                        DailyHrs = qty * SiteWorkingTime / 60,
                        MonthlyOffs = g.Group.Sum(o => o.MonthlyOffs),
                        MonthlyShifts = g.Group.Sum(sh => sh.MonthlyShifts),
                        PositionName = g.Group.First().PositionName,
                    });
                    Res.Qty = Roasters.Count(e => e.Month == startDate.Month && e.Year == startDate.Year);
                    Res.MonthlyHrs = Res.Rows.Sum(m => m.MonthlyHrs);
                    Res.DailyHrs = Res.Qty * SiteWorkingTime / 60;
                    Res.MonthlyOffs = Res.Rows.Sum(o => o.MonthlyOffs);
                    Res.MonthlyShifts = Res.Rows.Sum(sh => sh.MonthlyShifts);
                    Res.PositionName = "total";
                });
                Res.IsValidReq = true;
                return Res;
            }
            catch (Exception ex)
            {

                Log.Error("Error in RoasterReportForPayRollPeriodQuery Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return new() { IsValidReq = false, ErrorMsg = "Some thing went Wrong" };

            }
        }
    }


    #endregion



}
