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









    #region GetSingleEmployeeAttendance

    public class GetSingleEmployeeAttendance : IRequest<EmployeeTimeSheetDto>
    {
        public UserIdentityDto User { get; set; }
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
        public string EmployeeNumber { get; set; }
        public string AttnDate { get; set; }
        public string ShiftCode { get; set; }


    }

    public class GetSingleEmployeeAttendanceHandler : IRequestHandler<GetSingleEmployeeAttendance, EmployeeTimeSheetDto>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public GetSingleEmployeeAttendanceHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<EmployeeTimeSheetDto> Handle(GetSingleEmployeeAttendance request, CancellationToken cancellationToken)
        {
            try
            {

                Log.Info($"EmployeesAttendance Handle request : {request}");

                var SiteWorkingHours = _context.TblOpProjectSites.FirstOrDefault(e=>e.ProjectCode==request.ProjectCode&&e.SiteCode==request.SiteCode).SiteWorkingHours??0;  
                TimeSpan siteWorkingHours = new TimeSpan(SiteWorkingHours, 0, 0);
                EmployeeTimeSheetDto attendance = new();
                List<EmployeeTimeSheetDto> timeSheet = new();
                TblOpEmployeeLeavesDto leavesData = new();
                TblOpEmployeeTransResignDto transORresignData = new();
                DateTime InputDate = Convert.ToDateTime(request.AttnDate, CultureInfo.InvariantCulture);
                bool IsPrimaryResource = _context.TblOpMonthlyRoasterForSites.Any(e =>
                e.ProjectCode == request.ProjectCode &&
                e.SiteCode == request.SiteCode &&
                e.Month == InputDate.Month &&
                e.Year == InputDate.Year &&
                e.EmployeeNumber == request.EmployeeNumber &&
                e.IsPrimaryResource);

                bool isShiftExist =await _contextDMC.HRM_DEF_EmployeeShiftMasters.AnyAsync(e => e.ShiftCode == request.ShiftCode);
                Log.Info($"EmployeesAttendance Handle isShiftExist : {isShiftExist}");
                if (request.ShiftCode == "x" || request.ShiftCode == "" || !isShiftExist || request.ShiftCode == null)
                {


                    attendance.Id = request.ShiftCode == "x" ? -1 : -2;


                    leavesData = new();
                    leavesData.AL = false;
                    leavesData.UL = false;
                    leavesData.SL = false;
                    leavesData.STL = false;
                    leavesData.EL = false;
                    leavesData.W = false;


                    transORresignData = new();
                    transORresignData.TR = false;
                    transORresignData.R = false;


                    attendance.isDefShiftOff = false;

                    attendance.isPrimarySite = IsPrimaryResource;
                    attendance.isDefaultEmployee = true;


                    attendance.NWTime = TimeSpan.Parse("00:00");
                    attendance.LateHrs = TimeSpan.Parse("00:00");
                    attendance.LateDays = 0;




                    attendance.IsWorkedOnday = false;
                    attendance.IsAbsent = false;
                    attendance.ShiftsCount = 0;
                    attendance.OtCount = 0;

                    attendance.LeavesData = leavesData;
                    attendance.TransORresignData = transORresignData;



                    attendance.IsOnLeave = false;
                    attendance.IsWithDrawn = false;
                    attendance.IsTransfered = false;
                    attendance.IsResigned = false;
                    Log.Info($"EmployeesAttendance Handle attendance : {attendance}");
                    return attendance;

                }





                //else
                //{

                leavesData = _context.EmployeeLeaves.AsNoTracking().ProjectTo<TblOpEmployeeLeavesDto>(_mapper.ConfigurationProvider).FirstOrDefault(e => e.AttnDate == Convert.ToDateTime(request.AttnDate, CultureInfo.InvariantCulture) && e.EmployeeNumber == request.EmployeeNumber && (e.EL || e.UL || e.SL || e.AL || e.STL || e.W));
                transORresignData = _context.EmployeeTransResign.AsNoTracking().ProjectTo<TblOpEmployeeTransResignDto>(_mapper.ConfigurationProvider).FirstOrDefault(e => e.AttnDate <= Convert.ToDateTime(request.AttnDate, CultureInfo.InvariantCulture) && e.ProjectCode == request.ProjectCode && e.SiteCode == request.SiteCode && e.EmployeeNumber == request.EmployeeNumber/* && (e.TR || e.R)*/);

                Log.Info($"EmployeesAttendance Handle leavesData : {leavesData}");
                if (leavesData is null)
                {
                    leavesData = new();
                    leavesData.AL = false;
                    leavesData.UL = false;
                    leavesData.SL = false;
                    leavesData.STL = false;
                    leavesData.EL = false;
                    leavesData.W = false;

                }

                if (transORresignData is null)
                {
                    transORresignData = new();
                    transORresignData.TR = false;
                    transORresignData.R = false;
                }

                attendance = _context.EmployeeAttendance.AsNoTracking().ProjectTo<EmployeeTimeSheetDto>(_mapper.ConfigurationProvider).FirstOrDefault(e =>
                 e.ProjectCode == request.ProjectCode &&
                 e.SiteCode == request.SiteCode &&
                 e.ShiftCode == request.ShiftCode &&
                 e.AttnDate == Convert.ToDateTime(request.AttnDate, CultureInfo.InvariantCulture) &&
                 e.EmployeeNumber == request.EmployeeNumber
                );
                Log.Info($"EmployeesAttendance Handle attendance : {attendance}");
                if (attendance is null)
                {
                    var shift = _contextDMC.HRM_DEF_EmployeeShiftMasters.FirstOrDefault(e => e.ShiftCode == request.ShiftCode);
                    attendance = new();

                    attendance.isDefShiftOff = shift is null ? false : shift.IsOff.Value;
                    attendance.isPrimarySite = IsPrimaryResource;
                    attendance.isDefaultEmployee = true;
                    //attendance.NWTime = TimeSpan.Parse("00:00");
                    //attendance.OverTime = TimeSpan.Parse("00:00");
                    //attendance.LateHrs = TimeSpan.Parse("00:00");
                    //attendance.LateDays = 0;
                    //attendance.ShiftsCount = 0;
                    //attendance.IsWorkedOnday = false;
                    //attendance.IsAbsent = false;


                    attendance.Id = shift is null ? request.ShiftCode == "x" ? -1 : request.ShiftCode == "y" ? -2 : 0 : 0;
                }
                attendance.LeavesData = leavesData;
                attendance.TransORresignData = transORresignData;
                timeSheet = _context.EmployeeAttendance.AsNoTracking().ProjectTo<EmployeeTimeSheetDto>(_mapper.ConfigurationProvider).Where(e =>
                 e.ProjectCode == request.ProjectCode &&
                 e.SiteCode == request.SiteCode &&
                 e.AttnDate == Convert.ToDateTime(request.AttnDate, CultureInfo.InvariantCulture) &&
                 e.EmployeeNumber == request.EmployeeNumber &&
                 e.AltEmployeeNumber == "" &&
                 (e.Attendance == "P" || e.Attendance == "OT")

                ).ToList();

                attendance.NWTime = TimeSpan.Parse("00:00");
                attendance.LateHrs = TimeSpan.Parse("00:00");
                TimeSpan lh = TimeSpan.Parse("00:00");
                TimeSpan ot = TimeSpan.Parse("00:00");

                int lateDays = 0;
                if (timeSheet.Count != 0)
                {



                    foreach (EmployeeTimeSheetDto ts in timeSheet)
                    {
                        TimeSpan nwt = TimeSpan.Parse(ts.OutTime) > TimeSpan.Parse(ts.InTime) ? (TimeSpan.Parse(ts.OutTime) - TimeSpan.Parse(ts.InTime)) : TimeSpan.Parse(ts.OutTime) + (TimeSpan.Parse("23:59") - TimeSpan.Parse(ts.InTime) + TimeSpan.Parse("00:01"));
                        attendance.NWTime = TimeSpan.Parse(ts.OutTime) > TimeSpan.Parse(ts.InTime) ? attendance.NWTime + (TimeSpan.Parse(ts.OutTime) - TimeSpan.Parse(ts.InTime)) : attendance.NWTime + TimeSpan.Parse(ts.OutTime) + (TimeSpan.Parse("23:59") - TimeSpan.Parse(ts.InTime) + TimeSpan.Parse("00:01"));
                       
                        
                       // lh = nwt < TimeSpan.Parse("08:00") ? lh + (TimeSpan.Parse("08:00") - nwt) : lh;
                        lh = nwt < siteWorkingHours ? lh + (siteWorkingHours - nwt) : lh;
                        lateDays = nwt < siteWorkingHours ? lateDays + 1 : lateDays;
                        //ot = ts.Attendance == "OT" ? ot + nwt : nwt > TimeSpan.Parse("08:00") ? ot + (nwt - TimeSpan.Parse("08:00")) : ot;
                        ot = attendance.isDefShiftOff ? ot + nwt : nwt > siteWorkingHours ? ot + (nwt - siteWorkingHours) : ot;
                    }
                    attendance.LateHrs = lh;
                    attendance.LateDays = lateDays;
                    //attendance.OverTime = timeSheet.Count==1?ot:attendance.NWTime> TimeSpan.Parse("08:00") ? attendance.NWTime - TimeSpan.Parse("08:00"): ot;
                    attendance.OverTime = attendance.isDefShiftOff ? ot : attendance.NWTime > siteWorkingHours ? attendance.NWTime - siteWorkingHours : ot;

                    attendance.IsWorkedOnday = attendance.NWTime > TimeSpan.Parse("00:00");
                    attendance.IsAbsent = false;
                    attendance.ShiftsCount = timeSheet.Count;
                    attendance.OtCount = attendance.isDefShiftOff ? timeSheet.Count : timeSheet.Count - 1;
                }
                else
                {

                    attendance.LateHrs = TimeSpan.Parse("00:00");
                    attendance.LateDays = 0;
                    attendance.OverTime = TimeSpan.Parse("00:00");
                    attendance.IsWorkedOnday = false;
                    attendance.IsAbsent = await _context.EmployeeAttendance.AsNoTracking().AnyAsync(e =>
                                                                                             e.ProjectCode == request.ProjectCode &&
                                                                                             e.SiteCode == request.SiteCode &&
                                                                                             e.AttnDate == Convert.ToDateTime(request.AttnDate, CultureInfo.InvariantCulture) &&
                                                                                             e.EmployeeNumber == request.EmployeeNumber &&
                                                                                             e.AltEmployeeNumber == "" &&
                                                                                             e.Attendance == "A");
                    attendance.ShiftsCount = 0;
                    attendance.OtCount = 0;

                    attendance.IsOnLeave = attendance.LeavesData.AL || attendance.LeavesData.UL || attendance.LeavesData.EL || attendance.LeavesData.SL || attendance.LeavesData.STL;
                    attendance.IsWithDrawn = await _context.EmployeeLeaves.AsNoTracking().AnyAsync(e => e.AttnDate == Convert.ToDateTime(request.AttnDate, CultureInfo.InvariantCulture) && e.EmployeeNumber == request.EmployeeNumber && e.W);
                    attendance.IsTransfered = await _context.EmployeeTransResign.AsNoTracking().AnyAsync(e => e.AttnDate <= Convert.ToDateTime(request.AttnDate, CultureInfo.InvariantCulture) && e.ProjectCode == request.ProjectCode && e.SiteCode == request.SiteCode && e.EmployeeNumber == request.EmployeeNumber && e.TR);
                    attendance.IsResigned = await _context.EmployeeTransResign.AsNoTracking().AnyAsync(e => e.AttnDate <= Convert.ToDateTime(request.AttnDate, CultureInfo.InvariantCulture) && e.EmployeeNumber == request.EmployeeNumber && e.ProjectCode == request.ProjectCode && e.SiteCode == request.SiteCode && e.R);
                    attendance.IsTerminated = await _context.EmployeeTransResign.AsNoTracking().AnyAsync(e => e.AttnDate <= Convert.ToDateTime(request.AttnDate, CultureInfo.InvariantCulture) && e.EmployeeNumber == request.EmployeeNumber && e.ProjectCode == request.ProjectCode && e.SiteCode == request.SiteCode && !e.R&& !e.TR);
                    if (attendance.Id == -1 || attendance.Id == -2)
                    {
                        attendance.isPrimarySite = IsPrimaryResource;
                    }
                }







                // }
                //if (attendance is null)
                //{

                //    attendance = new()
                //    {
                //        Id = -1,
                //     };
                //}


                return attendance;


            }


            catch (Exception ex)
            {
                Log.Error("Error in GetSingleEmployeeAttendance Method");
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }


        }
    }



    #endregion






    #region EnterAttendance

    public class EnterEmployeeAttendance : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public TblOpEmployeeAttendanceDto Input { get; set; }
    }

    public class EnterEmployeeAttendanceHandler : IRequestHandler<EnterEmployeeAttendance, long>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public EnterEmployeeAttendanceHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<long> Handle(EnterEmployeeAttendance request, CancellationToken cancellationToken)
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var obj = request.Input;
                    DateTime InputDate = Convert.ToDateTime(obj.AttnDate, CultureInfo.InvariantCulture);
                    var shifts = _contextDMC.HRM_DEF_EmployeeShiftMasters.AsNoTracking();
                    //var primaryEmps = _context.TblOpEmployeeToResourceMapList.AsNoTracking().Where(e => e.ProjectCode == request.Input.ProjectCode && e.SiteCode == request.Input.SiteCode && e.isPrimarySite);
                    bool isExistInompleteShiftLocks = await _context.TblOpMonthlyRoasterForSites.AnyAsync(p => (p.ProjectCode == request.Input.ProjectCode
                       && p.SiteCode == request.Input.SiteCode
                       && p.Month == InputDate.Month
                       && p.Year == InputDate.Year)
                       && (p.S1==""||p.S2==""||p.S3==""||p.S4==""||p.S5==""||p.S6==""||p.S7==""||p.S8==""||p.S9==""||p.S10==""||
                       p.S11 == "" || p.S12 == "" || p.S13 == "" || p.S14 == "" || p.S15 == "" || p.S16 == "" || p.S17 == "" || p.S18 == "" || p.S19 == "" || p.S20 == "" ||
                       p.S21 == "" || p.S22 == "" || p.S23 == "" || p.S24 == "" || p.S25 == "" || p.S26 == "" || p.S27 == "" || p.S28 == "" || p.S29 == "" || p.S30 == "" || p.S31 == "")
                       
                       );
                    if(isExistInompleteShiftLocks)
                    {
                        return -8;
                    }

                    var roasterData = _context.TblOpMonthlyRoasterForSites.AsNoTracking().FirstOrDefault(p => p.ProjectCode == request.Input.ProjectCode

                    && p.SiteCode == request.Input.SiteCode
                    && p.Month == InputDate.Month
                    && p.Year == InputDate.Year
                    && p.EmployeeNumber == request.Input.EmployeeNumber);

                   


                    TblOpEmployeeAttendance attendance = new();

                    if (obj.AltEmployeeNumber == "" || obj.AltEmployeeNumber == null)
                    {

                        TblOpEmployeeLeavesDto leaveData = _context.EmployeeLeaves.AsNoTracking().ProjectTo<TblOpEmployeeLeavesDto>(_mapper.ConfigurationProvider).FirstOrDefault(e => e.EmployeeNumber == obj.EmployeeNumber && e.AttnDate == InputDate);

                        TblOpEmployeeTransResignDto transResignData = _context.EmployeeTransResign.AsNoTracking().ProjectTo<TblOpEmployeeTransResignDto>(_mapper.ConfigurationProvider).FirstOrDefault(e => e.EmployeeNumber == obj.EmployeeNumber && e.AttnDate <= InputDate && e.ProjectCode==obj.ProjectCode &&e.SiteCode==obj.SiteCode);

                        if (leaveData is not null)
                        {

                            if (leaveData.W)
                            {
                                return -4;
                            }
                            else
                                return -3;

                        }
                        if (transResignData is not null)
                        {
                            if (transResignData.TR)
                            {
                                return -5;
                            }
                            else if(transResignData.R)
                            {
                                return -6;
                            }
                            else
                            {
                                return -7;
                            }
                            

                        }


                    }

                    if (obj.Id > 0)
                    {

                        attendance = _context.EmployeeAttendance.AsNoTracking().FirstOrDefault(e => e.Id == obj.Id);
                    }
                    else
                    {

                        string shiftCode = obj.ShiftCode;
                        var checkAtt = _context.EmployeeAttendance.Any(e => e.EmployeeNumber == obj.EmployeeNumber && e.AttnDate == InputDate && e.ShiftCode == shiftCode);
                        if (checkAtt)
                        {
                            await transaction.RollbackAsync();
                            return -1;

                        }
                    }
                    attendance.ProjectCode = obj.ProjectCode;
                    attendance.SiteCode = obj.SiteCode;
                    attendance.AttnDate = InputDate;
                    attendance.InTime = TimeSpan.Parse(obj.InTime);
                    attendance.OutTime = TimeSpan.Parse(obj.OutTime);
                    attendance.EmployeeNumber = obj.EmployeeNumber;
                    attendance.ShiftCode = obj.ShiftCode;
                    attendance.isDefaultEmployee = true;
                    attendance.IsActive = obj.IsActive;
                    attendance.isDefShiftOff = obj.isDefShiftOff;

                    attendance.isPosted = false;
                    attendance.Attendance = obj.isDefShiftOff ? obj.ShiftCode : obj.isDefaultEmployee ? "P" : "S";
                    attendance.AltEmployeeNumber = obj.AltEmployeeNumber;
                    attendance.AltShiftCode = obj.AltShiftCode;
                    attendance.RefIdForAlt = 0;
                    attendance.ShiftNumber = 1;

                    var shift = shifts.FirstOrDefault(e => e.ShiftCode == obj.ShiftCode);
                    if (string.IsNullOrEmpty(obj.AltEmployeeNumber))
                    {
                        attendance.IsLate = TimeSpan.Parse(obj.InTime) > (shift.InTime.Value/* + shift.InGrace*/);
                    }

                    if (obj.Id > 0)
                    {
                        attendance.Modified = DateTime.UtcNow;
                        attendance.ModifiedBy = request.User.UserId;
                        _context.EmployeeAttendance.Update(attendance);
                        _context.SaveChanges();

                    }
                    else
                    {

                        attendance.isPrimarySite = roasterData != null ? roasterData.IsPrimaryResource : false;

                        attendance.Created = DateTime.UtcNow;
                        attendance.CreatedBy = request.User.UserId;
                       
                       

                        _context.EmployeeAttendance.Add(attendance);
                        _context.SaveChanges();

                    }







                    if (obj.AltEmployeeNumber != "" && obj.AltEmployeeNumber != null)
                    {
                        TblOpEmployeeLeavesDto altLeaveData = _context.EmployeeLeaves.AsNoTracking().ProjectTo<TblOpEmployeeLeavesDto>(_mapper.ConfigurationProvider).FirstOrDefault(e => e.EmployeeNumber == obj.AltEmployeeNumber && e.AttnDate == InputDate);
                        TblOpEmployeeTransResignDto altTransResignData = _context.EmployeeTransResign.AsNoTracking().ProjectTo<TblOpEmployeeTransResignDto>(_mapper.ConfigurationProvider).FirstOrDefault(e => e.EmployeeNumber == obj.AltEmployeeNumber &&e.SiteCode==obj.SiteCode && e.AttnDate <= InputDate);
                        if (altLeaveData is not null)
                        {
                            if (altLeaveData.W)
                            {
                                await transaction.RollbackAsync();
                                return -4;
                            }
                            else
                            {
                                await transaction.RollbackAsync();
                                return -3;
                            }
                        }
                        if (altTransResignData is not null)
                        {
                            if (altTransResignData.TR)
                            {
                                await transaction.RollbackAsync();
                                return -5;
                            }
                            else if (altTransResignData.R)
                            {
                                await transaction.RollbackAsync();
                                return -6;

                            }
                            else
                            {
                                await transaction.RollbackAsync();
                                return -7;
                            }
                        }


                      

                        short month = short.Parse(InputDate.Month.ToString());
                        short year = short.Parse(InputDate.Year.ToString()); ;
                        short day = short.Parse(InputDate.Day.ToString());


                        var project = _context.OP_HRM_TEMP_Projects.AsNoTracking().FirstOrDefault(e => e.ProjectCode == request.Input.ProjectCode);
                        TblOpMonthlyRoasterForSite roaster = await _context.TblOpMonthlyRoasterForSites.AsNoTracking().FirstOrDefaultAsync(e => e.CustomerCode == project.CustomerCode && e.ProjectCode == project.ProjectCode && e.Month == month && e.Year == year && e.SiteCode == request.Input.SiteCode && e.EmployeeNumber == request.Input.AltEmployeeNumber);

                        

                        if (roaster is null)
                        {
                            
                    
                            // var Employee=await   _contextDMC.HRM_TRAN_Employees.FirstOrDefaultAsync(e => e.EmployeeNumber == request.Input.AltEmployeeNumber);
                            //    if (Employee==null)
                            //    {
                            //       await transaction.RollbackAsync();
                            //        return -8;
                                    
                            //    }
                            
                            //var skillset = await _context.TblOpSkillsets.AsNoTracking().FirstOrDefaultAsync(e => e.SkillSetCode == roasterData.SkillsetCode);


                            //var monthlyRoaster = _context.TblOpMonthlyRoasterForSites.AsNoTracking().FirstOrDefault(e => e.CustomerCode == project.CustomerCode && e.ProjectCode == project.ProjectCode && e.Month == month && e.Year == year && e.SiteCode == request.Input.SiteCode);
                            //roaster = new();

                            //roaster.CustomerCode = project.CustomerCode;
                            //roaster.SiteCode = request.Input.SiteCode;
                            //roaster.ProjectCode = request.Input.ProjectCode;
                            //roaster.Month = month;
                            //roaster.Year = year;
                            //roaster.MonthEndDate = monthlyRoaster.MonthEndDate;
                            //roaster.MonthStartDate = monthlyRoaster.MonthStartDate;
                            //roaster.SkillsetCode = roasterData.SkillsetCode;
                            //roaster.SkillsetName = skillset.NameInEnglish;
                            //roaster.S1 = day == 1 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 1 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S2 = day == 2 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 2 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S3 = day == 3 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 3 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S4 = day == 4 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 4 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S5 = day == 5 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 5 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S6 = day == 6 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 6 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S7 = day == 7 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 7 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S8 = day == 8 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 8 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S9 = day == 9 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 9 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S10 = day == 10 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 10 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S11 = day == 11 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 11 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S12 = day == 12 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 12 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S13 = day == 13 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 13 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S14 = day == 14 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 14 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S15 = day == 15 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 15 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S16 = day == 16 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 16 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S17 = day == 17 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 17 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S18 = day == 18 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 18 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S19 = day == 19 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 19 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S20 = day == 20 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 20 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S21 = day == 21 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 21 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S22 = day == 22 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 22 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S23 = day == 23 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 23 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S24 = day == 24 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 24 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S25 = day == 25 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 25 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S26 = day == 26 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 26 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S27 = day == 27 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 27 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S28 = day == 28 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 28 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S29 = day == 29 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 29 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S30 = day == 30 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 30 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";
                            //roaster.S31 = day == 31 && request.Input.isDefShiftOff ? request.Input.AltShiftCode : day == 31 && !request.Input.isDefShiftOff && request.Input.ShiftCode != "" ? request.Input.ShiftCode : "x";

                            //roaster.IsPrimaryResource = false;
                           
                            //roaster.EmployeeNumber = request.Input.AltEmployeeNumber;


                            //_context.TblOpMonthlyRoasterForSites.Add(roaster);
                            //_context.SaveChanges();
                        }
                        else
                        {
                            
                            string shiftcode = obj.isDefShiftOff ? obj.AltShiftCode : obj.ShiftCode;

                            var checkAltAtt = _context.EmployeeAttendance.Any(e => e.EmployeeNumber == obj.AltEmployeeNumber && e.AttnDate == InputDate && e.ShiftCode == shiftcode);
                            if (checkAltAtt && obj.Id == 0)
                            {
                                await transaction.RollbackAsync();
                                return -1;
                            }


                            switch (day)
                            {

                                case 1:

                                    if (roaster.S1 == "x")
                                        roaster.S1 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S1 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }


                                    break;
                                case 2:
                                    if (roaster.S2 == "x")

                                        roaster.S2 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S2 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }

                                    break;
                                case 3:
                                    if (roaster.S3 == "x")
                                        roaster.S3 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S3 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 4:
                                    if (roaster.S4 == "x")

                                        roaster.S4 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S4 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 5:
                                    if (roaster.S5 == "x")

                                        roaster.S5 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S5 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 6:
                                    if (roaster.S6 == "x")

                                        roaster.S6 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S6 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 7:
                                    if (roaster.S7 == "x")

                                        roaster.S7 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S7 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 8:
                                    if (roaster.S8 == "x")

                                        roaster.S8 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S8 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 9:
                                    if (roaster.S9 == "x")

                                        roaster.S9 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S9 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 10:
                                    if (roaster.S10 == "x")

                                        roaster.S10 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S10 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 11:
                                    if (roaster.S11 == "x")

                                        roaster.S11 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S11 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 12:
                                    if (roaster.S12 == "x")

                                        roaster.S12 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S12 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 13:
                                    if (roaster.S13 == "x")

                                        roaster.S13 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S13 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 14:
                                    if (roaster.S14 == "x")

                                        roaster.S14 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S14 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 15:
                                    if (roaster.S15 == "x")

                                        roaster.S15 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S15 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 16:
                                    if (roaster.S16 == "x")

                                        roaster.S16 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S16 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 17:
                                    if (roaster.S17 == "x")

                                        roaster.S17 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S17 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }

                                    break;
                                case 18:
                                    if (roaster.S18 == "x")

                                        roaster.S18 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S18 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 19:
                                    if (roaster.S19 == "x")

                                        roaster.S19 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S19 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 20:
                                    if (roaster.S20 == "x")

                                        roaster.S20 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S20 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 21:
                                    if (roaster.S21 == "x")

                                        roaster.S21 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S21 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 22:
                                    if (roaster.S22 == "x")

                                        roaster.S22 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S22 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }

                                    break;
                                case 23:
                                    if (roaster.S23 == "x")

                                        roaster.S23 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S23 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }

                                    break;
                                case 24:
                                    if (roaster.S24 == "x")

                                        roaster.S24 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S24 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 25:
                                    if (roaster.S25 == "x")

                                        roaster.S25 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S25 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 26:
                                    if (roaster.S26 == "x")
                                        roaster.S26 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S26 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }

                                    break;
                                case 27:
                                    if (roaster.S27 == "x")

                                        roaster.S27 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S27 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 28:
                                    if (roaster.S28 == "x")

                                        roaster.S28 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S28 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 29:
                                    if (roaster.S29 == "x")

                                        roaster.S29 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S29 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 30:
                                    if (roaster.S30 == "x")

                                        roaster.S30 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S30 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                case 31:
                                    if (roaster.S31 == "x")

                                        roaster.S31 = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                                    else if (roaster.S31 == shiftcode && roaster.IsPrimaryResource)
                                    {
                                        transaction.Rollback();
                                        return -2;
                                    }
                                    break;
                                default: break;
                            }












                            //_context.TblOpMonthlyRoasterForSites.Update(roaster);
                            //_context.SaveChanges();
                        }

                        string shiftCode = obj.isDefShiftOff ? obj.AltShiftCode : obj.ShiftCode;
                        var checkAtt = _context.EmployeeAttendance.Any(e => e.EmployeeNumber == obj.AltEmployeeNumber && e.AttnDate == InputDate && e.ShiftCode == shiftCode);
                        if (checkAtt && obj.Id == 0)
                        {
                            transaction.Rollback();
                            return -1;

                        }
                        //var refAtt = _context.EmployeeAttendance.AsNoTracking().FirstOrDefault(e => e.EmployeeNumber == obj.EmployeeNumber && e.ProjectCode == obj.ProjectCode && e.ShiftCode == obj.ShiftCode && e.SiteCode == obj.SiteCode && !(e.isDefaultEmployee) && e.AttnDate == InputDate && (e.Attendance == "A" || e.Attendance == "O"));
                        TblOpEmployeeAttendance defAttendance = new TblOpEmployeeAttendance();
                        TblOpEmployeeAttendance altAttendance = new TblOpEmployeeAttendance();

                        var altRoaster = _context.TblOpMonthlyRoasterForSites.AsNoTracking().FirstOrDefault(ar =>
                        ar.ProjectCode == request.Input.ProjectCode &&
                        ar.SiteCode == request.Input.SiteCode &&
                        ar.Month == short.Parse(InputDate.Month.ToString()) &&
                        ar.Year == short.Parse(InputDate.Year.ToString()) &&
                        ar.EmployeeNumber == request.Input.AltEmployeeNumber);
                        if (obj.Id > 0)
                        {
                            defAttendance = _context.EmployeeAttendance.AsNoTracking().FirstOrDefault(e => e.Id == obj.Id);
                        }
                        else
                        {

                            defAttendance = _context.EmployeeAttendance.AsNoTracking().FirstOrDefault(e => e.EmployeeNumber == obj.EmployeeNumber && e.AttnDate == InputDate && e.ShiftCode == obj.ShiftCode);
                        }

                        altAttendance = _context.EmployeeAttendance.AsNoTracking().FirstOrDefault(e => e.RefIdForAlt == defAttendance.Id);
                        bool isAltEmpPrimaryresource = _context.TblOpMonthlyRoasterForSites.AsNoTracking().Any(a =>
                        a.ProjectCode == request.Input.ProjectCode &&
                        a.SiteCode == request.Input.SiteCode &&
                        a.EmployeeNumber == request.Input.AltEmployeeNumber &&
                        a.Month == InputDate.Month &&
                        a.Year == InputDate.Year &&
                        a.IsPrimaryResource

                        );
                        if (altAttendance is null)
                            altAttendance = altAttendance = new TblOpEmployeeAttendance() { Id = 0 };
                        altAttendance.ProjectCode = obj.ProjectCode;
                        altAttendance.SiteCode = obj.SiteCode;
                        altAttendance.AttnDate = InputDate;
                        altAttendance.InTime = TimeSpan.Parse(obj.InTime);
                        altAttendance.OutTime = TimeSpan.Parse(obj.OutTime);
                        altAttendance.EmployeeNumber = obj.AltEmployeeNumber;
                        altAttendance.ShiftCode = request.Input.isDefShiftOff ? request.Input.AltShiftCode : request.Input.ShiftCode;
                        altAttendance.isDefaultEmployee = false;
                        altAttendance.IsActive = obj.IsActive;
                        altAttendance.isDefShiftOff = false;

                        altAttendance.isPosted = false;
                        altAttendance.Attendance = isAltEmpPrimaryresource ? "OT" : "P";

                        altAttendance.AltEmployeeNumber = "";
                        altAttendance.AltShiftCode = "";
                        altAttendance.RefIdForAlt = defAttendance.Id;

                        


                        if (altAttendance.Id > 0)
                        {

                            altAttendance.Modified = DateTime.UtcNow;
                            altAttendance.ModifiedBy = request.User.UserId;
                            _context.EmployeeAttendance.Update(altAttendance);
                            _context.SaveChanges();

                        }
                        else
                        {

                            
                            altAttendance.isPrimarySite = altRoaster is not null ? altRoaster.IsPrimaryResource : false;
                            altAttendance.ShiftNumber = Convert.ToInt16(2);     //   altAttendance.ShiftNumber = altAttendance.isPrimarySite?Convert.ToInt16(2):Convert.ToInt16(3);   //By default alternative attendance shift number giving as 2 but if required we can give 3 for relievers

                            altAttendance.Created = DateTime.UtcNow;
                            altAttendance.CreatedBy = request.User.UserId;


                            altAttendance.IsLate = TimeSpan.Parse(obj.InTime) > (shift.InTime.Value/* + shift.InGrace*/);
                            
                            _context.EmployeeAttendance.Add(altAttendance);
                            _context.SaveChanges();

                        }

                       
                    }

                    else if (attendance.Id > 0)
                    {
                        var altAtt = _context.EmployeeAttendance.FirstOrDefault(e => e.RefIdForAlt == attendance.Id);
                        if (altAtt is not null)
                        {
                            _context.EmployeeAttendance.Remove(altAtt);
                            _context.SaveChanges();
                        }

                    }






                   await transaction.CommitAsync();

                    Log.Info("----Info EnterEmployeeAttendance method Exit----");

                    return attendance.Id;
                }
                catch (Exception ex)
                {

                    transaction.Rollback();
                    Log.Error("Error in EnterEmployeeAttendance Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;

                }

            }
        }
    }

    #endregion

    #region EnterAbsent

    public class EnterEmployeeAbsent : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public TblOpEmployeeAttendanceDto Input { get; set; }
    }

    public class EnterEmployeeAbsentHandler : IRequestHandler<EnterEmployeeAbsent, long>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public EnterEmployeeAbsentHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<long> Handle(EnterEmployeeAbsent request, CancellationToken cancellationToken)
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //  var primaryEmps = _context.TblOpEmployeeToResourceMapList.AsNoTracking().Where(e => e.ProjectCode == request.Input.ProjectCode && e.SiteCode == request.Input.SiteCode && e.isPrimarySite);


                    var obj = request.Input;
                    DateTime InputDate = Convert.ToDateTime(obj.AttnDate, CultureInfo.InvariantCulture);
                    
                    bool isExistInompleteShiftLocks = await _context.TblOpMonthlyRoasterForSites.AnyAsync(p => (p.ProjectCode == request.Input.ProjectCode
                       && p.SiteCode == request.Input.SiteCode
                       && p.Month == InputDate.Month
                       && p.Year == InputDate.Year)
                       && (p.S1 == "" || p.S2 == "" || p.S3 == "" || p.S4 == "" || p.S5 == "" || p.S6 == "" || p.S7 == "" || p.S8 == "" || p.S9 == "" || p.S10 == "" ||
                       p.S11 == "" || p.S12 == "" || p.S13 == "" || p.S14 == "" || p.S15 == "" || p.S16 == "" || p.S17 == "" || p.S18 == "" || p.S19 == "" || p.S20 == "" ||
                       p.S21 == "" || p.S22 == "" || p.S23 == "" || p.S24 == "" || p.S25 == "" || p.S26 == "" || p.S27 == "" || p.S28 == "" || p.S29 == "" || p.S30 == "" || p.S31 == "")
                       );
                    if (isExistInompleteShiftLocks)
                    {
                        return -8;
                    }


                    var leaveData = _context.EmployeeLeaves.FirstOrDefault(e => e.AttnDate == InputDate && e.EmployeeNumber == obj.EmployeeNumber);
                    if (leaveData is not null)
                    {
                        if (leaveData.AL || leaveData.EL || leaveData.UL || leaveData.SL || leaveData.STL)
                            return -3;
                        else if (leaveData.W)
                        {
                            return -4;
                        }
                    }



                    var isAttendanceExist = _context.EmployeeAttendance.Any(e => e.AttnDate == InputDate && e.EmployeeNumber == obj.EmployeeNumber);
                    if (isAttendanceExist)
                    {
                        return -1;
                    }


                    var roaster = _context.TblOpMonthlyRoasterForSites.AsNoTracking().FirstOrDefault(r =>
                    r.ProjectCode == request.Input.ProjectCode &&
                    r.SiteCode == request.Input.SiteCode &&
                    r.Month == short.Parse(InputDate.Month.ToString()) &&
                    r.Year == short.Parse(InputDate.Year.ToString()) &&
                    r.EmployeeNumber == request.Input.EmployeeNumber &&
                    r.IsPrimaryResource);
                    if(roaster is null)
                    {
                        return -9;
                    }

                 //   TblOpEmployeeTransResignDto transResignData = _context.EmployeeTransResign.AsNoTracking().ProjectTo<TblOpEmployeeTransResignDto>(_mapper.ConfigurationProvider).FirstOrDefault(e => e.EmployeeNumber == obj.EmployeeNumber && e.AttnDate <= InputDate);

                    
                    //if (transResignData is not null)
                    //{
                    //    if (transResignData.TR)
                    //    {
                    //        return -5;
                    //    }
                    //    else
                    //        return -6;

                    //}
                    
                    TblOpEmployeeAttendance attendance = new();
                    attendance.Id = obj.Id;
                    attendance.ProjectCode = obj.ProjectCode;
                    attendance.SiteCode = obj.SiteCode;
                    attendance.AttnDate = InputDate;
                    attendance.InTime = TimeSpan.Parse(obj.InTime);
                    attendance.OutTime = TimeSpan.Parse(obj.OutTime);
                    attendance.EmployeeNumber = obj.EmployeeNumber;
                    attendance.ShiftCode = obj.ShiftCode;
                    attendance.isDefaultEmployee = obj.isDefaultEmployee;
                    attendance.IsActive = obj.IsActive;
                    attendance.isDefShiftOff = obj.isDefShiftOff;

                    attendance.isPosted = false;
                    attendance.Attendance = "A";
                    attendance.AltEmployeeNumber = "";
                    attendance.AltShiftCode = "";
                    attendance.RefIdForAlt = 0;
                    attendance.ShiftNumber = 0;
                    // attendance.isPrimarySite = primaryEmps.Any(e => e.EmployeeNumber == attendance.EmployeeNumber);
                    attendance.isPrimarySite = roaster is not null ? roaster.IsPrimaryResource : false;
                    if (obj.Id == 0)
                    {


                        attendance.Created = DateTime.UtcNow;
                        attendance.CreatedBy = request.User.UserId;
                        _context.EmployeeAttendance.Add(attendance);
                        _context.SaveChanges();
                    }
                    else if (obj.isDefaultEmployee)
                    {

                        attendance.Modified = DateTime.UtcNow;
                        attendance.ModifiedBy = request.User.UserId;
                        _context.EmployeeAttendance.Update(attendance);
                        _context.SaveChanges();
                    }

                    else

                    {
                        return 0;
                   
                    }

                    Log.Info("----Info EnterEmployeeAbsent method Exit----");
                    transaction.Commit();
                    return attendance.Id;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Log.Error("Error in EnterEmployeeAbsent Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;

                }
            }
        }
    }


    #endregion


    #region GetAttendanceById
    public class GetAttendanceById : IRequest<TblOpEmployeeAttendanceDto>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }

    public class GetAttendanceByIdHandler : IRequestHandler<GetAttendanceById, TblOpEmployeeAttendanceDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAttendanceByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpEmployeeAttendanceDto> Handle(GetAttendanceById request, CancellationToken cancellationToken)
        {
            TblOpEmployeeAttendanceDto obj = new();
            var attendance = await _context.EmployeeAttendance.AsNoTracking().ProjectTo<TblOpEmployeeAttendanceDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);
            return attendance;
        }
    }

    #endregion


    #region GetAltAttendanceById
    public class GetAltAttendanceById : IRequest<TblOpEmployeeAttendanceDto>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }

    public class GetAltAttendanceByIdHandler : IRequestHandler<GetAltAttendanceById, TblOpEmployeeAttendanceDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetAltAttendanceByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpEmployeeAttendanceDto> Handle(GetAltAttendanceById request, CancellationToken cancellationToken)
        {
            TblOpEmployeeAttendanceDto obj = new() { Id = -1 };
            var attendance = _context.EmployeeAttendance.AsNoTracking().ProjectTo<TblOpEmployeeAttendanceDto>(_mapper.ConfigurationProvider).FirstOrDefault(e => e.RefIdForAlt == request.Id);
            if (attendance is not null)
                return attendance;
            else return obj;
        }
    }

    #endregion



    


    #region ClearMonthlyAttendanceWithDate

    public class ClearMonthlyAttendanceWithDate : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public InputClearMonthlyAttendanceWithDate Input { get; set; }
    }

    public class ClearMonthlyAttendanceWithDateHandler : IRequestHandler<ClearMonthlyAttendanceWithDate, long>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public ClearMonthlyAttendanceWithDateHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<long> Handle(ClearMonthlyAttendanceWithDate request, CancellationToken cancellationToken)
        {

            var sites = _context.OprSites.AsNoTracking();
            var shifts = _contextDMC.HRM_DEF_EmployeeShiftMasters.AsNoTracking();
         

                try
                {
                    var CurrentDate = Convert.ToDateTime(DateTime.Today, CultureInfo.InvariantCulture);
                    var FromDate = Convert.ToDateTime(request.Input.Fromdate, CultureInfo.InvariantCulture);
                var Todate =request.Input.Todate is null? new DateTime(FromDate.Year, FromDate.Month, DateTime.DaysInMonth(FromDate.Year, FromDate.Month)): Convert.ToDateTime(request.Input.Todate,CultureInfo.InvariantCulture); // if todate is not given means need to clear attendance for complete month, otherwise payroll month
                var HolidaysList = _contextDMC.HRM_DEF_Holidays.AsNoTracking().Where(e => e.HolidayDate >= FromDate && e.HolidayDate <= Todate && (FromDate!=Todate)).Select(n => n.HolidayDate).ToList();
                //if selected for one day need not to consider holiday case    
                var attedanceList = await _context.EmployeeAttendance.Where(e => e.Attendance == "P" 
                    && e.RefIdForAlt==0
                    && !e.isPosted
                    && e.AttnDate >= FromDate
                    && e.AttnDate <= Todate
                    
                    && e.ProjectCode == request.Input.ProjectCode
                    && e.SiteCode == request.Input.SiteCode

                    ).ToListAsync();
                attedanceList = attedanceList.Where(e=>!HolidaysList.Any(h=>h.Value==e.AttnDate.Value)).ToList();

                    if (attedanceList.Count > 0)
                    {
                    _context.EmployeeAttendance.RemoveRange(attedanceList);
                    await _context.SaveChangesAsync();
                    return 1;
                }
                    else if (attedanceList.Count==0)
                {
                    return 0;
                }

                return 0;   
                    }


                    catch (Exception ex)
                    {
                        Log.Error("Error in ClearMonthlyAttendanceWithDate Method");
                        Log.Error("Error occured time : " + DateTime.UtcNow);
                        Log.Error("Error message : " + ex.Message);
                        Log.Error("Error StackTrace : " + ex.StackTrace);
                        return -1;

                    }
                

            }
        }
    

    #endregion

    #region CancelAttendance

    public class CancelAttendance : IRequest<TblOpEmployeeAttendance>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }
    public class CancelAttendanceQueryHandler : IRequestHandler<CancelAttendance, TblOpEmployeeAttendance>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CancelAttendanceQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TblOpEmployeeAttendance> Handle(CancelAttendance request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CancelAttendance method start----");
                    TblOpEmployeeAttendance Attendance =await  _context.EmployeeAttendance.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
                    TblOpEmployeeAttendance AltAttendance =await _context.EmployeeAttendance.AsNoTracking().FirstOrDefaultAsync(e => e.RefIdForAlt == request.Id);
                    TblOpEmployeeAttendance MainAttendance =await _context.EmployeeAttendance.AsNoTracking().FirstOrDefaultAsync(e => e.Id == Attendance.RefIdForAlt);
                    int res = 1;
                    bool isPrimaryAttn = true;
                    if (request.Id > 0)
                    {

                        if (MainAttendance is not null)
                        {
                            AltAttendance = Attendance;
                            Attendance = MainAttendance;
                            isPrimaryAttn = false;
                        }


                        if (Attendance.isPosted)
                        {
                            transaction.Rollback();
                        
                            return new() { Id = -1 };
                        }

                     
                        if (AltAttendance is not null)
                        {
                            DateTime InputDate = Convert.ToDateTime(AltAttendance.AttnDate, CultureInfo.InvariantCulture);
                            short day = short.Parse(InputDate.Day.ToString());
                            var roaster = await _context.TblOpMonthlyRoasterForSites.AsNoTracking().FirstOrDefaultAsync(e =>
                            e.ProjectCode == AltAttendance.ProjectCode &&
                            e.SiteCode == AltAttendance.SiteCode &&
                            e.EmployeeNumber == AltAttendance.EmployeeNumber &&
                            e.Month == InputDate.Month &&
                            e.Year == InputDate.Year
                            );
                            if(roaster is not null)
                            if (!roaster.IsPrimaryResource)
                            {
                                var existAtt = _context.EmployeeAttendance.AsNoTracking().FirstOrDefault(e =>
                                e.EmployeeNumber == AltAttendance.EmployeeNumber &&
                                e.AttnDate == InputDate &&
                                e.ProjectCode == AltAttendance.ProjectCode &&
                                e.SiteCode == AltAttendance.SiteCode &&
                                e.ShiftCode != AltAttendance.ShiftCode
                                );

                                switch (day)
                                {

                                    case 1:

                                        if (roaster.S1 == AltAttendance.ShiftCode)
                                        {
                                            roaster.S1 = existAtt is not null ? existAtt.ShiftCode : "x";
                                        }

                                        break;
                                    case 2:
                                        if (roaster.S2 == AltAttendance.ShiftCode)
                                            roaster.S2 = existAtt is not null ? existAtt.ShiftCode : "x";


                                        break;
                                    case 3:
                                        if (roaster.S3 == AltAttendance.ShiftCode)
                                            roaster.S3 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    case 4:
                                        if (roaster.S4 == AltAttendance.ShiftCode)
                                            roaster.S4 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    case 5:
                                        if (roaster.S5 == AltAttendance.ShiftCode)
                                            roaster.S5 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    case 6:
                                        if (roaster.S6 == AltAttendance.ShiftCode)
                                            roaster.S6 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    case 7:
                                        if (roaster.S7 == AltAttendance.ShiftCode)
                                            roaster.S7 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    case 8:
                                        if (roaster.S8 == AltAttendance.ShiftCode)
                                            roaster.S8 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    case 9:
                                        if (roaster.S9 == AltAttendance.ShiftCode)
                                            roaster.S9 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    case 10:
                                        if (roaster.S10 == AltAttendance.ShiftCode)
                                            roaster.S10 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    case 11:
                                        if (roaster.S11 == AltAttendance.ShiftCode)
                                            roaster.S11 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    case 12:
                                        if (roaster.S12 == AltAttendance.ShiftCode)
                                            roaster.S12 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    case 13:
                                        if (roaster.S13 == AltAttendance.ShiftCode)
                                            roaster.S13 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    case 14:
                                        if (roaster.S14 == AltAttendance.ShiftCode)
                                            roaster.S14 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    case 15:
                                        if (roaster.S15 == AltAttendance.ShiftCode)
                                            roaster.S15 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    case 16:
                                        if (roaster.S16 == AltAttendance.ShiftCode)
                                            roaster.S16 = "x";

                                        break;
                                    case 17:
                                        if (roaster.S17 == AltAttendance.ShiftCode)
                                            roaster.S17 = existAtt is not null ? existAtt.ShiftCode : "x";


                                        break;
                                    case 18:
                                        if (roaster.S18 == AltAttendance.ShiftCode)
                                            roaster.S18 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    case 19:
                                        if (roaster.S19 == AltAttendance.ShiftCode)
                                            roaster.S19 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    case 20:
                                        if (roaster.S20 == AltAttendance.ShiftCode)
                                            roaster.S2 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    case 21:
                                        if (roaster.S21 == AltAttendance.ShiftCode)
                                            roaster.S21 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    case 22:
                                        if (roaster.S22 == AltAttendance.ShiftCode)
                                            roaster.S22 = existAtt is not null ? existAtt.ShiftCode : "x";


                                        break;
                                    case 23:
                                        if (roaster.S23 == AltAttendance.ShiftCode)
                                            roaster.S23 = existAtt is not null ? existAtt.ShiftCode : "x";


                                        break;
                                    case 24:
                                        if (roaster.S24 == AltAttendance.ShiftCode)
                                            roaster.S24 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    case 25:
                                        if (roaster.S25 == AltAttendance.ShiftCode)
                                            roaster.S25 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    case 26:
                                        if (roaster.S26 == AltAttendance.ShiftCode)
                                            roaster.S26 = existAtt is not null ? existAtt.ShiftCode : "x";


                                        break;
                                    case 27:
                                        if (roaster.S27 == AltAttendance.ShiftCode)
                                            roaster.S27 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    case 28:
                                        if (roaster.S28 == AltAttendance.ShiftCode)
                                            roaster.S28 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    case 29:
                                        if (roaster.S29 == AltAttendance.ShiftCode)
                                            roaster.S29 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    case 30:
                                        if (roaster.S30 == AltAttendance.ShiftCode)
                                            roaster.S30 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    case 31:
                                        if (roaster.S31 == AltAttendance.ShiftCode)
                                            roaster.S31 = existAtt is not null ? existAtt.ShiftCode : "x";

                                        break;
                                    default: break;
                                }

                               
                                if ((roaster.S1 == "x" || roaster.S1 == "") &&
                                    (roaster.S2 == "x" || roaster.S2 == "") &&
                                    (roaster.S3 == "x" || roaster.S3 == "") &&
                                    (roaster.S4 == "x" || roaster.S4 == "") &&
                                    (roaster.S5 == "x" || roaster.S5 == "") &&
                                    (roaster.S6 == "x" || roaster.S6 == "") &&
                                    (roaster.S7 == "x" || roaster.S7 == "") &&
                                    (roaster.S8 == "x" || roaster.S8 == "") &&
                                    (roaster.S9 == "x" || roaster.S9 == "") &&
                                    (roaster.S10 == "x" || roaster.S10 == "") &&
                                    (roaster.S11 == "x" || roaster.S11 == "") &&
                                    (roaster.S12 == "x" || roaster.S12 == "") &&
                                    (roaster.S13 == "x" || roaster.S13 == "") &&
                                    (roaster.S14 == "x" || roaster.S14 == "") &&
                                    (roaster.S15 == "x" || roaster.S15 == "") &&
                                    (roaster.S16 == "x" || roaster.S16 == "") &&
                                    (roaster.S17 == "x" || roaster.S17 == "") &&
                                    (roaster.S18 == "x" || roaster.S18 == "") &&
                                    (roaster.S19 == "x" || roaster.S19 == "") &&
                                    (roaster.S20 == "x" || roaster.S20 == "") &&
                                    (roaster.S21 == "x" || roaster.S21 == "") &&
                                    (roaster.S22 == "x" || roaster.S22 == "") &&
                                    (roaster.S23 == "x" || roaster.S23 == "") &&
                                    (roaster.S24 == "x" || roaster.S24 == "") &&
                                    (roaster.S25 == "x" || roaster.S25 == "") &&
                                    (roaster.S26 == "x" || roaster.S26 == "") &&
                                    (roaster.S27 == "x" || roaster.S27 == "") &&
                                    (roaster.S28 == "x" || roaster.S28 == "") &&
                                    (roaster.S29 == "x" || roaster.S29 == "") &&
                                    (roaster.S30 == "x" || roaster.S30 == "") &&
                                    (roaster.S31 == "x" || roaster.S31 == ""))
                                {


                                    _context.TblOpMonthlyRoasterForSites.Remove(roaster);
                                    await _context.SaveChangesAsync();

                                    res = -100;                          //means Roaster deleted  

                                }
                                else
                                {

                                    _context.TblOpMonthlyRoasterForSites.Update(roaster);
                                    await _context.SaveChangesAsync();
                                  
                                }


                           }
                            
                            
                            if (AltAttendance.isPosted)
                            {
                                transaction.Rollback();
                               // AltAttendance.Id = -1;
                                return new(){Id=-1 };
                            }
                            _context.Remove(AltAttendance);
                           await _context.SaveChangesAsync();

                        }


                        _context.Remove(Attendance);
                        await _context.SaveChangesAsync();

                      await  transaction.CommitAsync();
                        return new() { Id= isPrimaryAttn?res:-101 };        //attendance column should be updated
                    }
                
                   
                    return new() { Id=0};
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CancelAttendance Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    TblOpEmployeeAttendance AltAttendance = new() { Id = 0 };
                    return AltAttendance;

                }
            }
        }
    }
    #endregion


    #region enterAutoAttendance

    public class EnterAutoAttendance : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public List<TblOpEmployeeAttendanceDto> Input { get; set; }
    }

    public class EnterAutoAttendanceHandler : IRequestHandler<EnterAutoAttendance, long>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public EnterAutoAttendanceHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<long> Handle(EnterAutoAttendance request, CancellationToken cancellationToken)
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int insertionCount = 0;
                    var CurrentDate = Convert.ToDateTime(DateTime.Today, CultureInfo.InvariantCulture);
                    var projectCode = request.Input[0].ProjectCode;
                    var siteCode = request.Input[0].SiteCode;
                    var HrmShifts = await _contextDMC.HRM_DEF_EmployeeShiftMasters.AsNoTracking().ToListAsync();
                    DateTime fromDate = request.Input.OrderBy(e => e.AttnDate).Select(e=>e.AttnDate).First();
                     fromDate = Convert.ToDateTime(fromDate, CultureInfo.InvariantCulture);
                   
                    DateTime toDate = request.Input.OrderByDescending(e => e.AttnDate).Select(e=>e.AttnDate).First();
                    toDate = Convert.ToDateTime(toDate, CultureInfo.InvariantCulture);
                   
                    var roasters= await _context.TblOpMonthlyRoasterForSites.Where(e =>
                   e.ProjectCode == projectCode &&
                   e.SiteCode == siteCode &&
                   ((e.Month==fromDate.Month && e.Year==fromDate.Year) || (e.Month==toDate.Month &&e.Year==toDate.Year))&&
                   e.IsPrimaryResource).ToListAsync();
                   
                    
                    bool isIncompleteShiftsLockExist = roasters.Any(e => e.S1 == "" || e.S2 == "" || e.S3 == "" || e.S4 == "" || e.S5 == "" || e.S6 == "" || e.S7 == "" || e.S8 == "" || e.S9 == "" || e.S10 == "" ||
                  e.S11 == "" || e.S12 == "" || e.S13 == "" || e.S14 == "" || e.S15 == "" || e.S16 == "" || e.S17 == "" || e.S18 == "" || e.S19 == "" || e.S20 == "" ||
                  e.S21 == "" || e.S22 == "" || e.S23 == "" || e.S24 == "" || e.S25 == "" || e.S26 == "" || e.S27 == "" || e.S28 == "" || e.S29 == "" || e.S30 == "" || e.S31 == "");
                    if(isIncompleteShiftsLockExist)
                    {
                        return -2;
                    }

                    var attendanceList =await _context.EmployeeAttendance.AsNoTracking().Where(a =>
                a.ProjectCode == projectCode &&
                   a.SiteCode == siteCode &&
                  a.AttnDate >= fromDate && a.AttnDate <= toDate
                  ).ToListAsync();

                    var Holidays =await _contextDMC.HRM_DEF_Holidays.Where(e => e.HolidayDate >= fromDate && e.HolidayDate <= toDate && fromDate!=toDate).ToListAsync();
                    var TransResignWithdrawals = await _context.EmployeeTransResign.Where(e=>e.AttnDate.Value.AddDays(31)>=fromDate && e.AttnDate<=toDate).ToListAsync();
                    for (var i = 0; i < request.Input.Count; i++)
                    {
                        TblOpEmployeeAttendanceDto attn = request.Input[i];
                        DateTime InputDate = Convert.ToDateTime(attn.AttnDate, CultureInfo.InvariantCulture);
                        bool isHoliday = Holidays.Any(e => e.HolidayDate == InputDate);
                        bool isTransResTerminated = TransResignWithdrawals.Any(e => (e.AttnDate <= InputDate.AddDays(31) && e.EmployeeNumber == attn.EmployeeNumber && ((e.TR && e.ProjectCode == attn.ProjectCode && e.SiteCode == attn.SiteCode) || e.R)) || (e.AttnDate == InputDate && !e.TR && !e.R));
                        
                            bool IsPrimaryResource = roasters.Any(e =>e.EmployeeNumber == attn.EmployeeNumber);
                        var isAttExistOnTheDay = attendanceList.Any(a =>attn.AttnDate == a.AttnDate);
                        if (!isTransResTerminated && !isHoliday&& !isAttExistOnTheDay && IsPrimaryResource && attn.ShiftCode!="O" && attn.ShiftCode!="" && attn.ShiftCode!="x")
                        {
                            insertionCount++;
                            TblOpEmployeeAttendance attendance = new();
                            attendance.ProjectCode = projectCode;
                            attendance.SiteCode = siteCode;
                            attendance.EmployeeNumber = attn.EmployeeNumber;
                            attendance.ShiftCode = attn.ShiftCode;
                            attendance.isDefaultEmployee = true;
                            attendance.isDefShiftOff = false;
                            attendance.AltEmployeeNumber = "";
                            attendance.AltShiftCode = "";
                            attendance.isPrimarySite = true;
                            attendance.isPosted = false;
                            attendance.Created = DateTime.UtcNow;
                            attendance.CreatedBy = request.User.UserId;
                            attendance.Attendance = "P";
                            attendance.AttnDate = attn.AttnDate;
                            attendance.IsActive = true;
                            attendance.RefIdForAlt = 0;
                            attendance.InTime = HrmShifts.FirstOrDefault(e => e.ShiftCode == attn.ShiftCode).InTime.Value;//TimeSpan.Parse(attn.InTime);
                            attendance.OutTime = HrmShifts.FirstOrDefault(e => e.ShiftCode == attn.ShiftCode).OutTime.Value;//TimeSpan.Parse(attn.OutTime);

                            attendance.ShiftNumber = 1;
                            _context.EmployeeAttendance.Add(attendance);
                            _context.SaveChanges();
                        }
                    }

                    transaction.Commit();
                    return insertionCount;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Log.Error("Error in EnterEmployeeAttendance Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return -1;

                }

            }
        }
    }

    #endregion

}


