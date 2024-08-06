using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.MobileMgt.Dtos;
using CIN.Application.OperationsMgtDtos;
using CIN.DB;
using CIN.Domain.MobileMgt;
using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.MobileMgt.Queries
{


    #region EnterEmployeeAttendanceThroughMobileQuery

    public class EnterEmployeeAttendanceThroughMobileQuery : IRequest<(bool, string)>
    {
        public UserMobileIdentityDto User { get; set; }
        public InputEnterEmployeeAttendanceThroughMobileDto Input { get; set; }
        public int? UserId { get; set; }
    }

    public class EnterEmployeeAttendanceThroughMobileQueryHandler : IRequestHandler<EnterEmployeeAttendanceThroughMobileQuery, (bool, string)>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public EnterEmployeeAttendanceThroughMobileQueryHandler(CINDBOneContext context, DMCContext contextDMC)
        {
            _context = context;
            _contextDMC = contextDMC;
        }
        public async Task<(bool, string)> Handle(EnterEmployeeAttendanceThroughMobileQuery request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {



                    long attendanceId = 0;

                    request.Input.AttnDate = Convert.ToDateTime(request.Input.AttnDate, CultureInfo.InvariantCulture);

                    var shift = await _contextDMC.HRM_DEF_EmployeeShiftMasters.FirstOrDefaultAsync(e => e.ShiftCode == request.Input.ShiftCode);


                    var existAttendance = await _context.EmployeeAttendance.FirstOrDefaultAsync(e => e.EmployeeNumber == request.Input.EmployeeNumber &&
                    e.ProjectCode == request.Input.ProjectCode
                    && e.SiteCode == request.Input.SiteCode
                    && e.ShiftCode == request.Input.ShiftCode
                    && e.AttnDate == request.Input.AttnDate
                    );
                    var obj = request.Input;
                    string Intime = obj.InTime.Split(':')[0].PadLeft(2, '0') + ":" + obj.InTime.Split(':')[1].PadLeft(2, '0');



                    if (existAttendance is null)
                    {
                        TblOpEmployeeAttendance attendance = new()
                        {
                            EmployeeNumber = obj.EmployeeNumber,
                            ProjectCode = obj.ProjectCode,
                            SiteCode = obj.SiteCode,
                            ShiftCode = obj.ShiftCode,
                            SkillsetCode = obj.SkillsetCode,
                            InTime = TimeSpan.Parse(Intime),
                            OutTime = TimeSpan.Parse(Intime),
                            AltEmployeeNumber = "",
                            AltShiftCode = "",
                            Attendance = "P",
                            isDefaultEmployee = true,
                            IsActive = true,
                            CreatedBy = request.UserId ?? 0,
                            Created = DateTime.UtcNow,
                            AttnDate = obj.AttnDate,
                            IsOnBreak = false,
                            isPosted = false,
                            isDefShiftOff = false,
                            IsGeofenseOut = false,
                            IsLogoutFromShift = false,
                            IsLoginToShift = true,
                            isPrimarySite = true,
                            GeofenseOutCount = 0,
                            Modified = DateTime.UtcNow,
                            ModifiedBy = request.UserId ?? 0,
                            RefIdForAlt = 0,
                            ShiftNumber = 1,
                            Id = 0,
                            IsLate = shift.InTime.Value.Add(shift.InGrace.Value) < TimeSpan.Parse(request.Input.InTime)

                        };

                        await _context.EmployeeAttendance.AddAsync(attendance);
                        await _context.SaveChangesAsync();
                        attendanceId = attendance.Id;

                    }
                    else if (existAttendance.isPosted)
                    {
                        await transaction.RollbackAsync();
                        return (false, "Attendance_Already_Posted");
                    }
                    else if (!(existAttendance.IsLoginToShift ?? false))
                    {


                        attendanceId = existAttendance.Id;

                        existAttendance.IsLoginToShift = true;
                        existAttendance.InTime = TimeSpan.Parse(Intime); //TimeSpan.Parse(obj.InTime);
                        existAttendance.OutTime = TimeSpan.Parse(Intime);//TimeSpan.Parse(obj.InTime);
                        existAttendance.IsLate = shift.InTime.Value.Add(shift.InGrace.Value) < TimeSpan.Parse(request.Input.InTime);
                        existAttendance.Modified = DateTime.UtcNow;
                        existAttendance.ModifiedBy = request.UserId ?? 0;
                        _context.EmployeeAttendance.Update(existAttendance);
                        await _context.SaveChangesAsync();


                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return (false, "Employee_Already_Logged_In");
                    }

                    if (attendanceId > 0)
                    {
                        string FileName = attendanceId.ToString()+"_login";
                        bool isImageUploaded = await _context.MobMgtAttendanceImages.AnyAsync(e => e.AttendanceId == attendanceId);
                        if (obj.Base64Image == null || obj.Base64Image.Length == 0)
                        {
                            await transaction.RollbackAsync();
                            return (false, "Image_Not_Found");
                        }
                        else if (!isImageUploaded)
                        {
                            string fileName = string.Empty;

                            fileName = $"{FileName}{".jpeg"}";
                            var filePath = Path.Combine(obj.WebRootForAttendanceImages, fileName);
                            Regex regex = new Regex(@"^[\w/\:.-]+;base64,");
                            var base64File = regex.Replace(request.Input.Base64Image, string.Empty);
                            byte[] imageBytes = Convert.FromBase64String(base64File);
                            MemoryStream ipstream = new MemoryStream(imageBytes, 0, imageBytes.Length);


                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                ipstream.CopyTo(stream);
                            }


                            TblMobMgtAttendanceImages attendanceImage = new()
                            {
                                EmployeeNumber = obj.EmployeeNumber,
                                AttendanceId = attendanceId,
                                CreatedOn = DateTime.Now,
                                LoginImagePath = fileName

                            };

                            await _context.MobMgtAttendanceImages.AddAsync(attendanceImage);
                            await _context.SaveChangesAsync();

                        }

                    }

                    await transaction.CommitAsync();
                    return (true, "Success");
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    return (false, "Exception:" + e.Message);
                }
            }
        }
    }

    #endregion
    #region UpdateEmployeeGeoLocationLogQuery

    public class UpdateEmployeeGeoLocationLogQuery : IRequest<(long, string)>
    {
        public UserMobileIdentityDto User { get; set; }
        public TblMobMgtGeoLocationLogsDto Input { get; set; }
        public int? UserId { get; set; }
    }

    public class UpdateEmployeeGeoLocationLogQueryHandler : IRequestHandler<UpdateEmployeeGeoLocationLogQuery, (long, string)>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public UpdateEmployeeGeoLocationLogQueryHandler(CINDBOneContext context, DMCContext contextDMC)
        {
            _context = context;
            _contextDMC = contextDMC;
        }
        public async Task<(long, string)> Handle(UpdateEmployeeGeoLocationLogQuery request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var attendance = await _context.EmployeeAttendance.FirstOrDefaultAsync(e => e.Id == request.Input.AttnId);
                    if (attendance is null)
                    {
                        return (-1, "Attendance_Not_Found");
                    }
                    else
                    {
                        if ((attendance.IsGeofenseOut.Value) != request.Input.IsGeofenceOut || !(await _context.MobMgtGeoLocationLogs.AnyAsync(e => e.AttnId == request.Input.AttnId)))
                        {
                            if (request.Input.IsGeofenceOut)
                            {
                                attendance.GeofenseOutCount++;
                            }

                            attendance.IsGeofenseOut = request.Input.IsGeofenceOut;
                            _context.Update(attendance);
                            await _context.SaveChangesAsync();

                            TblMobMgtGeoLocationLogs geolocationLog = new()
                            {
                                AttnId = request.Input.AttnId,
                                IsGeofenceOut = request.Input.IsGeofenceOut,
                                UpdatedTime = DateTime.UtcNow,
                                Remarks = "",
                                Id = 0,
                            };
                            await _context.MobMgtGeoLocationLogs.AddAsync(geolocationLog);
                            await _context.SaveChangesAsync();
                        }

                    }
                    await transaction.CommitAsync();
                    return (request.Input.AttnId, "Success");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return (-1, ex.Message);
                }
            }
        }
    }

    #endregion



    #region GetEmployeeTodaysShifts

    public class GetEmployeeTodaysShiftsQuery : IRequest<List<MobEmployeeScheduledSchiftsDto>>
    {
        public string EmployeeNumber { get; set; }
        public DateTime? TodayDate { get; set; }
    }

    public class GetEmployeeTodaysShiftsQueryHandler : IRequestHandler<GetEmployeeTodaysShiftsQuery, List<MobEmployeeScheduledSchiftsDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public GetEmployeeTodaysShiftsQueryHandler(CINDBOneContext context, DMCContext contextDMC)
        {
            _context = context;
            _contextDMC = contextDMC;
        }
        public async Task<List<MobEmployeeScheduledSchiftsDto>> Handle(GetEmployeeTodaysShiftsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<MobEmployeeScheduledSchiftsDto> res = new();

                var shiftMaster = await _contextDMC.HRM_DEF_EmployeeShiftMasters.AsNoTracking().ToListAsync();
                var siteMaster = await _context.OprSites.AsNoTracking().ToListAsync();

                var skillsets = await _context.TblOpSkillsets.AsNoTracking().ToListAsync();

                DateTime todayDate = request.TodayDate is null ? Convert.ToDateTime(DateTime.UtcNow, CultureInfo.InvariantCulture) : Convert.ToDateTime(request.TodayDate, CultureInfo.InvariantCulture);
                // DateTime todayDate = Convert.ToDateTime(DateTime.UtcNow, CultureInfo.InvariantCulture);
                // DateTime todayDate = Convert.ToDateTime("2022-11-01", CultureInfo.InvariantCulture);
                int day = todayDate.Day;

                var attendanceForEmployee = await _context.EmployeeAttendance.Where(e => e.EmployeeNumber == request.EmployeeNumber && e.AltEmployeeNumber == "" && e.AttnDate == todayDate).ToListAsync();

                var roasters = await _context.TblOpMonthlyRoasterForSites.Where(e => e.EmployeeNumber == request.EmployeeNumber && e.Month == todayDate.Month && e.Year == todayDate.Year).ToListAsync();
                if (roasters.Count > 0)
                {
                    switch (day)
                    {
                        case 1:
                            roasters = roasters.Where(e => e.S1 != "" && e.S1 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S1);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S1,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;

                        case 2:
                            roasters = roasters.Where(e => e.S2 != "" && e.S2 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S2);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S2,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,

                                    });

                                });
                            }
                            break;
                        case 3:
                            roasters = roasters.Where(e => e.S3 != "" && e.S3 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S3);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S3,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,

                                    });

                                });
                            }
                            break;
                        case 4:
                            roasters = roasters.Where(e => e.S4 != "" && e.S4 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S4);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S4,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;
                        case 5:
                            roasters = roasters.Where(e => e.S5 != "" && e.S5 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S5);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S5,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;
                        case 6:
                            roasters = roasters.Where(e => e.S6 != "" && e.S6 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S6);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S6,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;
                        case 7:
                            roasters = roasters.Where(e => e.S7 != "" && e.S7 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S7);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S7,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;
                        case 8:
                            roasters = roasters.Where(e => e.S8 != "" && e.S8 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S8);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S8,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;
                        case 9:
                            roasters = roasters.Where(e => e.S9 != "" && e.S9 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S9);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S9,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;
                        case 10:
                            roasters = roasters.Where(e => e.S10 != "" && e.S10 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S10);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S10,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;
                        case 11:
                            roasters = roasters.Where(e => e.S11 != "" && e.S11 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S11);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S11,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;
                        case 12:
                            roasters = roasters.Where(e => e.S12 != "" && e.S12 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S12);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S12,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;
                        case 13:
                            roasters = roasters.Where(e => e.S13 != "" && e.S13 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S13);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S13,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;
                        case 14:
                            roasters = roasters.Where(e => e.S14 != "" && e.S14 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S14);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S14,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;
                        case 15:
                            roasters = roasters.Where(e => e.S15 != "" && e.S15 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S15);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S15,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;
                        case 16:
                            roasters = roasters.Where(e => e.S16 != "" && e.S16 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S16);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S16,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;

                        case 17:
                            roasters = roasters.Where(e => e.S17 != "" && e.S17 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S17);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S17,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;
                        case 18:
                            roasters = roasters.Where(e => e.S18 != "" && e.S18 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S18);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S18,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;
                        case 19:
                            roasters = roasters.Where(e => e.S19 != "" && e.S19 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S19);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S19,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;
                        case 20:
                            roasters = roasters.Where(e => e.S20 != "" && e.S20 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S20);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S20,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;

                        case 21:
                            roasters = roasters.Where(e => e.S21 != "" && e.S21 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S21);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S21,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;

                        case 22:
                            roasters = roasters.Where(e => e.S22 != "" && e.S22 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S22);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S22,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;

                        case 23:
                            roasters = roasters.Where(e => e.S23 != "" && e.S23 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S23);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S23,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;

                        case 24:
                            roasters = roasters.Where(e => e.S24 != "" && e.S24 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S24);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S24,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;

                        case 25:
                            roasters = roasters.Where(e => e.S25 != "" && e.S25 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S25);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S25,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;

                        case 26:
                            roasters = roasters.Where(e => e.S26 != "" && e.S26 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S26);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S26,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;

                        case 27:
                            roasters = roasters.Where(e => e.S27 != "" && e.S27 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S27);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S27,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;

                        case 28:
                            roasters = roasters.Where(e => e.S28 != "" && e.S28 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S28);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S28,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;

                        case 29:
                            roasters = roasters.Where(e => e.S29 != "" && e.S29 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S29);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S29,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;

                        case 30:
                            roasters = roasters.Where(e => e.S30 != "" && e.S30 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S30);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S30,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;

                        case 31:
                            roasters = roasters.Where(e => e.S31 != "" && e.S31 != "x").ToList();
                            if (roasters.Count > 0)
                            {
                                roasters.ForEach(e =>
                                {
                                    var shift = shiftMaster.FirstOrDefault(s => s.ShiftCode == e.S31);
                                    var site = siteMaster.FirstOrDefault(s => s.SiteCode == e.SiteCode);
                                    res.Add(new()
                                    {
                                        ShiftCode = e.S31,
                                        ProjectCode = e.ProjectCode,
                                        SiteCode = e.SiteCode,
                                        SiteNameEn = site?.SiteName,
                                        SiteNameAr = site?.SiteArbName,
                                        ShiftInTime = shift?.InTime,
                                        ShiftOutTime = shift?.OutTime,
                                        SiteGeoGain = site.SiteGeoGain,
                                        SiteGeoLatitude = site.SiteGeoLatitude,
                                        SiteGeoLongitude = site.SiteGeoLongitude,
                                        StandardDeviation = site.StandardDeviation,
                                        SkillsetCode = e.SkillsetCode,
                                        SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInArabic,
                                        SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == e.SkillsetCode).NameInEnglish,
                                    });

                                });
                            }
                            break;

                    }
                }

                res = res.OrderBy(e => e.ShiftInTime).ToList();

                res.ForEach(sh =>
                {
                    var attn = attendanceForEmployee.FirstOrDefault(a => a.ShiftCode == sh.ShiftCode && sh.ProjectCode == a.ProjectCode && sh.SiteCode == a.SiteCode);
                    sh.IsLoginToShift = attn is not null ? attn?.IsLoginToShift ?? false : false;
                    sh.IsLogoutFromShift = attn is not null ? attn.IsLogoutFromShift ?? false : false;
                    sh.LogInTime = attn is not null ? attn?.InTime : null;
                    sh.LogOutTime = attn is not null ? attn?.OutTime : null;
                    sh.AttId = attn is not null ? attn.Id : 0;
                });

                foreach (var att in attendanceForEmployee.Where(e => e.ShiftCode != "O"))
                {
                    if (string.IsNullOrEmpty(att.SkillsetCode))
                    {
                        att.SkillsetCode = "SST000002";
                    }

                    if (!res.Any(e => e.ShiftCode == att.ShiftCode && e.ProjectCode == att.ProjectCode && e.SiteCode == att.SiteCode))
                    {
                        var site = siteMaster.FirstOrDefault(s => s.SiteCode == att.SiteCode);
                        res.Add(new()
                        {
                            ShiftCode = att.ShiftCode,
                            ProjectCode = att.ProjectCode,
                            SiteCode = att.SiteCode,
                            ShiftInTime = shiftMaster.FirstOrDefault(s => s.ShiftCode == att.ShiftCode).InTime,
                            ShiftOutTime = shiftMaster.FirstOrDefault(s => s.ShiftCode == att.ShiftCode).OutTime,
                            LogInTime = att.InTime,
                            LogOutTime = att.OutTime,
                            IsLoginToShift = att.IsLoginToShift ?? false,
                            IsLogoutFromShift = att.IsLogoutFromShift ?? false,

                            SiteGeoLatitude = site.SiteGeoLatitude,
                            SiteGeoLongitude = site.SiteGeoLongitude,
                            StandardDeviation = site.StandardDeviation,
                            SkillsetCode = att.SkillsetCode,
                            SkillsetNameArb = skillsets.FirstOrDefault(s => s.SkillSetCode == att.SkillsetCode).NameInArabic,
                            SkillsetNameEng = skillsets.FirstOrDefault(s => s.SkillSetCode == att.SkillsetCode).NameInEnglish,
                            SiteNameEn = site?.SiteName,
                            SiteNameAr = site?.SiteArbName,
                            SiteGeoGain = site.SiteGeoGain,
                            AttId = att.Id

                        });
                    }
                }

                return res;

            }

            catch (Exception ex)
            {
                return new();
            }

            //return await _context.Users.AnyAsync(e => e.UserName == request.Input.UserName && e.Password == request.Input.Password && e.CINNumber == request.Input.CINNumber);
        }
    }

    #endregion


    #region LogoutFromShiftByAttnIdQuery
    public class LogoutFromShiftByAttnIdQuery : IRequest<(bool,string)>
    {
        public InputEmployeeLogoutDto Input { get; set; }
    }

    public class LogoutFromShiftByAttnIdQueryHandler : IRequestHandler<LogoutFromShiftByAttnIdQuery, (bool, string)>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public LogoutFromShiftByAttnIdQueryHandler(CINDBOneContext context, DMCContext contextDMC)
        {
            _context = context;
            _contextDMC = contextDMC;
        }
        public async Task<(bool, string)> Handle(LogoutFromShiftByAttnIdQuery request, CancellationToken cancellationToken)
        {

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    var obj = request.Input;
                    var Attedance = await _context.EmployeeAttendance.FirstOrDefaultAsync(e => e.Id == request.Input.AttnId);
                    if (Attedance is null)
                    {
                        return (false, "InvalidAttendace Id");
                    }
                    if (Attedance.IsLoginToShift.Value == false)
                    {
                        return (false, "Shift login time not found");
                    }
                    if (Attedance.IsLogoutFromShift.Value == true)
                    {
                        return (false, "Shift Already Logged Out");
                    }
                    string FileName = obj.AttnId.ToString() + "_logout";
                    bool isImageUploaded = await _context.MobMgtAttendanceImages.AnyAsync(e => e.AttendanceId == obj.AttnId && !string.IsNullOrEmpty(e.LogoutImagePath));
                    if (obj.Base64Image == null || obj.Base64Image.Length == 0)
                    {
                        return (false, "Image_Not_Found");
                    }
                    else if (!isImageUploaded)
                    {
                        string fileName = string.Empty;

                        fileName = $"{FileName}{".jpeg"}";
                        var filePath = Path.Combine(obj.WebRootForAttendanceImages, fileName);
                        Regex regex = new Regex(@"^[\w/\:.-]+;base64,");
                        var base64File = regex.Replace(request.Input.Base64Image, string.Empty);
                        byte[] imageBytes = Convert.FromBase64String(base64File);
                        MemoryStream ipstream = new MemoryStream(imageBytes, 0, imageBytes.Length);


                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            ipstream.CopyTo(stream);
                        }

                        TblMobMgtAttendanceImages attendanceImage = await _context.MobMgtAttendanceImages.FirstOrDefaultAsync(e => e.AttendanceId == obj.AttnId);
                        if (attendanceImage is null)
                        {
                            attendanceImage = new()
                            {
                                LogoutImagePath = fileName,
                                AttendanceId = obj.AttnId,
                                CreatedOn = DateTime.Now,
                                EmployeeNumber = Attedance.EmployeeNumber
                            };
                            await _context.MobMgtAttendanceImages.AddAsync(attendanceImage);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            attendanceImage.LogoutImagePath = fileName;
                            _context.MobMgtAttendanceImages.Update(attendanceImage);
                            await _context.SaveChangesAsync();

                        }



                    }
                    Attedance.IsLogoutFromShift = true;
                    Attedance.OutTime = TimeSpan.Parse((request.Input.LogoutTime.Split(':')[0].PadLeft(2, '0') + ":" + request.Input.LogoutTime.Split(':')[1].PadLeft(2, '0')));
                    _context.Update(Attedance);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return (true, "Success");
                }



                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return (false, "Error");
                    throw;
                }
            }
        }
    }
}
#endregion