using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDto;
using CIN.Application.SchoolMgtDtos;
using CIN.Application.SystemSetupDtos;
using CIN.DB;
using CIN.Domain.SchoolMgt;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.SchoolMgtQuery
{
    #region GetSchoolAccountBranchList

    public class GetSchoolAccountBranchList : IRequest<PaginatedList<SchoolBranchesDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetSchoolAccountBranchListHandler : IRequestHandler<GetSchoolAccountBranchList, PaginatedList<SchoolBranchesDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolAccountBranchListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<SchoolBranchesDto>> Handle(GetSchoolAccountBranchList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.SchoolBranches.AsNoTracking()
              .Where(e => e.BranchCode.Contains(search))
               .OrderByDescending(x => x.Id)
               .Select(x => new SchoolBranchesDto()
               {
                   Address = x.Address,
                   BranchCode = x.BranchCode,
                   FinBranch = x.FinBranch,
                   BranchName = x.BranchName,
                   BranchNameAr = x.BranchNameAr,
                   BranchPrefix = x.BranchPrefix,
                   City = x.City,
                   CurrencyCode = x.CurrencyCode,
                   Email = x.Email,
                   EndAcademicDate = x.EndAcademicDate,
                   GeoLat = x.GeoLat,
                   GeoLong = x.GeoLong,
                   Id = x.Id,
                   Mobile = x.Mobile,
                   NextFeeVoucherNum = x.NextFeeVoucherNum,
                   NextStuNum = x.NextStuNum,
                   NumberOfWeekDays = x.NumberOfWeekDays,
                   Phone = x.Phone,
                   PrivacyPolicyUrl = x.PrivacyPolicyUrl,
                   StartAcademicDate = x.StartAcademicDate,
                   StartWeekDay = x.StartWeekDay,
                   Website = x.Website,
                   WeekOff1 = x.WeekOff1,
                   WeekOff2 = x.WeekOff2
               })
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }

    #endregion

    #region GetSchoolBranchDetails

    public class GetSchoolBranchDetails : IRequest<SchoolBranchesDto>
    {
        public UserIdentityDto User { get; set; }
        public string BranchCode { get; set; }
    }

    public class GetSchoolBranchDetailsHandler : IRequestHandler<GetSchoolBranchDetails, SchoolBranchesDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolBranchDetailsHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SchoolBranchesDto> Handle(GetSchoolBranchDetails request, CancellationToken cancellationToken)
        {
            SchoolBranchesDto obj = new();
            obj = await _context.SchoolBranches.AsNoTracking()
                        .Select(x => new SchoolBranchesDto()
                        {
                            Address = x.Address,
                            BranchCode = x.BranchCode,
                            FinBranch = x.FinBranch,
                            BranchName = x.BranchName,
                            BranchNameAr = x.BranchNameAr,
                            BranchPrefix = x.BranchPrefix,
                            City = x.City,
                            CurrencyCode = x.CurrencyCode,
                            Email = x.Email,
                            EndAcademicDate = x.EndAcademicDate,
                            GeoLat = x.GeoLat,
                            GeoLong = x.GeoLong,
                            Id = x.Id,
                            Mobile = x.Mobile,
                            NextFeeVoucherNum = x.NextFeeVoucherNum,
                            NextStuNum = x.NextStuNum,
                            NumberOfWeekDays = x.NumberOfWeekDays,
                            Phone = x.Phone,
                            PrivacyPolicyUrl = x.PrivacyPolicyUrl,
                            StartAcademicDate = x.StartAcademicDate,
                            StartWeekDay = x.StartWeekDay,
                            Website = x.Website,
                            WeekOff1 = x.WeekOff1,
                            WeekOff2 = x.WeekOff2
                        })
                       .FirstOrDefaultAsync(e => e.BranchCode == request.BranchCode);
            if (obj is not null)
            {
                var authList = await _context.SysSchoolBranchesAuthority.AsNoTracking()
                    .Where(e => e.BranchCode == obj.BranchCode)
                    .Select(e => new SchoolBranchesAuthorityDto
                    {
                        BranchCode = e.BranchCode,
                        Id = e.Id,
                        IsApproveDisciPlinaryAction = e.IsApproveDisciPlinaryAction,
                        IsApproveEvent = e.IsApproveEvent,
                        IsApproveLeave = e.IsApproveLeave,
                        IsApproveNotification = e.IsApproveNotification,
                        Level = e.Level,
                        TeacherCode = e.TeacherCode
                    }).ToListAsync();
                obj.SchoolBranchesAuthorityList = authList;
            }
            else
                obj = new();
            return obj;
        }
    }

    #endregion


    #region CreateSchoolBranch

    public class CreateSchoolBranch : IRequest<AppCtrollerDto>
    {
        public UserIdentityDto User { get; set; }
        public SchoolBranchesDto Input { get; set; }
    }

    public class CreateSchoolBranchHandler : IRequestHandler<CreateSchoolBranch, AppCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public CreateSchoolBranchHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppCtrollerDto> Handle(CreateSchoolBranch request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateSchoolBranch method start----");
                    var obj = request.Input;
                    TblSysSchoolBranches branchInfo = new();
                    if (obj.Id > 0)
                    {
                        branchInfo = await _context.SchoolBranches.FirstOrDefaultAsync(e => e.Id == obj.Id);
                    }
                    else
                    {
                        branchInfo = await _context.SchoolBranches.FirstOrDefaultAsync(e => e.BranchCode == obj.BranchCode);
                        if (branchInfo is not null)
                        {
                            return ApiMessageInfo.Status(0);
                        }
                        branchInfo = new();
                        branchInfo.BranchCode = obj.BranchCode;
                        branchInfo.FinBranch = obj.BranchCode;
                        branchInfo.CreatedBy = Convert.ToString(request.User.UserId);
                        branchInfo.CreatedOn = DateTime.Now;
                        branchInfo.NextFeeVoucherNum = 1;
                        branchInfo.NextStuNum = 1;

                    }
                    branchInfo.BranchName = obj.BranchName;
                    branchInfo.BranchNameAr = obj.BranchNameAr;
                    branchInfo.BranchPrefix = obj.BranchPrefix;
                    branchInfo.PrivacyPolicyUrl = obj.PrivacyPolicyUrl;
                    branchInfo.Address = obj.Address;
                    branchInfo.EndAcademicDate = obj.EndAcademicDate;
                    branchInfo.StartAcademicDate = obj.StartAcademicDate;
                    branchInfo.NumberOfWeekDays = (obj.WeekOff1 == obj.WeekOff2 && obj.WeekOff1 != string.Empty ? 6 :
                        obj.WeekOff1 == string.Empty ? 7 : obj.WeekOff2 == string.Empty ? 6 : 5).ToString();
                    branchInfo.CurrencyCode = obj.CurrencyCode;
                    branchInfo.StartWeekDay = obj.StartWeekDay;
                    branchInfo.WeekOff2 = obj.WeekOff2;
                    branchInfo.WeekOff1 = obj.WeekOff1;
                    branchInfo.Website = obj.Website;
                    branchInfo.FinBranch = obj.BranchCode;
                    branchInfo.GeoLat = obj.GeoLat;
                    branchInfo.GeoLong = obj.GeoLong;
                    branchInfo.Phone = obj.Phone;
                    branchInfo.Mobile = obj.Mobile;
                    branchInfo.Email = obj.Email;
                    branchInfo.City = obj.City;
                    if (obj.Id > 0)
                    {
                        _context.SchoolBranches.Update(branchInfo);
                    }
                    else
                    {
                        _context.SchoolBranches.Add(branchInfo);
                    }
                    await _context.SaveChangesAsync();

                    var oldAuthList = await _context.SysSchoolBranchesAuthority.Where(e => e.BranchCode == obj.BranchCode).ToListAsync();
                    if (oldAuthList.Count() > 0)
                    {
                        _context.SysSchoolBranchesAuthority.RemoveRange(oldAuthList);
                        await _context.SaveChangesAsync();
                    }

                    if (request.Input.SchoolBranchesAuthorityList.Count() > 0
                        && request.Input.SchoolBranchesAuthorityList.Where(x => x.TeacherCode != string.Empty && x.Level != null && x.Level > 0).Count() > 0)
                    {
                        List<TblSysSchoolBranchesAuthority> authList = new();
                        foreach (var auth in request.Input.SchoolBranchesAuthorityList)
                        {
                            TblSysSchoolBranchesAuthority authItem = new()
                            {
                                BranchCode = obj.BranchCode.ToUpper(),
                                IsApproveDisciPlinaryAction = auth.IsApproveDisciPlinaryAction,
                                IsApproveEvent = auth.IsApproveEvent,
                                IsApproveLeave = auth.IsApproveLeave,
                                IsApproveNotification = auth.IsApproveNotification,
                                Level = Convert.ToInt32(auth.Level),
                                TeacherCode = auth.TeacherCode
                            };
                            authList.Add(authItem);
                        }
                        await _context.SysSchoolBranchesAuthority.AddRangeAsync(authList);
                        await _context.SaveChangesAsync();
                    }
                    Log.Info("----Info CreateSchoolBranch method Exit----");
                    await transaction.CommitAsync();
                    return ApiMessageInfo.Status(1, 1);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateSchoolBranch Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return ApiMessageInfo.Status(0);
                }
            }
        }
    }
    #endregion

    #region DeleteSchoolBranches
    public class DeleteSchoolBranches : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeleteSchoolBranchesHandler : IRequestHandler<DeleteSchoolBranches, int>
    {
        //private readonly ICurrentUserService _currentUserService;
        //protected string UserId => _currentUserService.UserId;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public DeleteSchoolBranchesHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteSchoolBranches request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info DeleteSchoolBranches method start----");

                    if (request.Id > 0)
                    {
                        var branch = await _context.SchoolBranches.FirstOrDefaultAsync(e => e.Id == request.Id);

                        if (branch is not null)
                        {
                            var oldAuthList = await _context.SysSchoolBranchesAuthority.Where(e => e.BranchCode == branch.BranchCode).ToListAsync();
                            _context.SysSchoolBranchesAuthority.RemoveRange(oldAuthList);
                            _context.Remove(branch);
                        }
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return request.Id;
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in DeleteSchoolBranches Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }

    #endregion

    #region GetSelectSysBranchList

    public class GetSelectSysBranchList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string Search { get; set; }
    }

    public class GetSelectSysBranchListHandler : IRequestHandler<GetSelectSysBranchList, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSelectSysBranchListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectSysBranchList request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetBranchByBranchCode method start----");
            var items = await _context.CompanyBranches.AsNoTracking()
                .Where(e => e.BranchCode.Contains(request.Search) ||
                request.Search == null ||
                request.Search == string.Empty
                )
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.BranchName, Value = e.BranchCode, TextTwo = e.BranchAddress })
                  .ToListAsync(cancellationToken);
            Log.Info("----Info GetBranchByBranchCode method Ends----");
            return items;
        }
    }

    #endregion
    #region GetBranchByBranchCode

    public class GetBranchByBranchCode : IRequest<CustomSelectListItem>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetBranchByBranchCodeHandler : IRequestHandler<GetBranchByBranchCode, CustomSelectListItem>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBranchByBranchCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CustomSelectListItem> Handle(GetBranchByBranchCode request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetBranchByBranchCode method start----");
            var item = await _context.CompanyBranches.AsNoTracking()
                .Where(e => e.BranchCode == request.Input)
                .OrderByDescending(e => e.Id)
               .Select(e => new CustomSelectListItem { Text = e.BranchName, Value = e.BranchAddress, TextTwo = e.BranchAddressAr })
                  .FirstOrDefaultAsync(cancellationToken);
            Log.Info("----Info GetBranchByBranchCode method Ends----");
            return item;
        }
    }

    #endregion


    #region GetBranchDataByBranchCode

    public class GetBranchDataByBranchCode : IRequest<TblErpSysCompanyBranchDto>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetBranchDataByBranchCodeHandler : IRequestHandler<GetBranchDataByBranchCode, TblErpSysCompanyBranchDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBranchDataByBranchCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblErpSysCompanyBranchDto> Handle(GetBranchDataByBranchCode request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetBranchDataByBranchCode method start----");
            var item = await _context.CompanyBranches.ProjectTo<TblErpSysCompanyBranchDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.BranchCode == request.Input);
            Log.Info("----Info GetBranchDataByBranchCode method Ends----");
            return item;
        }
    }

    #endregion


    #region GetSchoolBranchDataByBranchCode

    public class GetSchoolBranchDataByBranchCode : IRequest<TblSysSchoolBranchesDto>
    {
        public UserIdentityDto User { get; set; }
        public string Input { get; set; }
    }

    public class GetSchoolBranchByBranchCodeHandler : IRequestHandler<GetSchoolBranchDataByBranchCode, TblSysSchoolBranchesDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetSchoolBranchByBranchCodeHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblSysSchoolBranchesDto> Handle(GetSchoolBranchDataByBranchCode request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetSchoolBranchDataByBranchCode method start----");
            var item = await _context.SchoolBranches.ProjectTo<TblSysSchoolBranchesDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.BranchCode == request.Input);
            Log.Info("----Info GetSchoolBranchDataByBranchCode method Ends----");
            return item;
        }
    }

    #endregion







    #region GetBranchHolidays

    public class GetBranchHolidays : IRequest<List<TblSysSchoolHolidayCalanderStudentDto>>
    {
        public UserIdentityDto User { get; set; }
        public string BranchCode { get; set; }
    }

    public class GetBranchHolidaysHandler : IRequestHandler<GetBranchHolidays, List<TblSysSchoolHolidayCalanderStudentDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBranchHolidaysHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblSysSchoolHolidayCalanderStudentDto>> Handle(GetBranchHolidays request, CancellationToken cancellationToken)
        {
            Log.Info("----Info GetBranchHolidays method start----");
            var item = await _context.StudentHolidayClaender.AsNoTracking()
                                    .ProjectTo<TblSysSchoolHolidayCalanderStudentDto>(_mapper.ConfigurationProvider)
                                    .Where(e => e.BranchCode == request.BranchCode).ToListAsync();
            Log.Info("----Info GetBranchHolidays method Ends----");
            return item;
        }
    }

    #endregion


    #region AddUpdateHolidays
    public class AddUpdateHolidays : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblSysSchoolHolidayCalanderStudentDto Input { get; set; }
    }

    public class AddUpdateHolidaysHandler : IRequestHandler<AddUpdateHolidays, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public AddUpdateHolidaysHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddUpdateHolidays request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info AddUpdateHolidays method start----");
                var obj = request.Input;
                TblSysSchoolHolidayCalanderStudent holidayData = new();
                if (obj.Id > 0)
                    holidayData = await _context.StudentHolidayClaender.SingleOrDefaultAsync(e => e.Id == obj.Id);
                else
                    holidayData = new();
                holidayData.Id = obj.Id;
                holidayData.BranchCode = obj.BranchCode;
                holidayData.HName = obj.HName;
                holidayData.HName2 = obj.HName2;
                holidayData.HDate = obj.HDate;
                if (obj.Id > 0)
                    _context.StudentHolidayClaender.Update(holidayData);
                else
                    await _context.StudentHolidayClaender.AddAsync(holidayData);
                await _context.SaveChangesAsync();
                Log.Info("----Info AddUpdateHolidays method Exit----");
                return holidayData.Id;
            }
            catch (Exception ex)
            {
                Log.Error("Error in AddUpdateHolidays Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
            }
            return 0;
        }
    }
    #endregion

    #region GetBranchEventsHolidaysData
    public class GetBranchEventsHolidaysData : IRequest<BranchEventsHolidaysDataDto>
    {
        public UserIdentityDto User { get; set; }
        public string BranchCode { get; set; }
    }

    public class GetBranchEventsHolidaysDataHandler : IRequestHandler<GetBranchEventsHolidaysData, BranchEventsHolidaysDataDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBranchEventsHolidaysDataHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BranchEventsHolidaysDataDto> Handle(GetBranchEventsHolidaysData request, CancellationToken cancellationToken)
        {
            BranchEventsHolidaysDataDto result = new();
            try
            {
                Log.Info("----Info GetBranchEventsHolidaysData method start----");
                var branchData = await _context.SchoolBranches.ProjectTo<TblSysSchoolBranchesDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.BranchCode == request.BranchCode);
                if (branchData != null)
                {
                    result.BranchCode = branchData.BranchCode;
                    result.StartDate = branchData.StartAcademicDate ?? DateTime.Now;
                    result.EndDate = branchData.EndAcademicDate ?? DateTime.Now;
                    List<EventsHolidaysData> holidayData = new();
                    for (DateTime sDate = result.StartDate; sDate <= result.EndDate; sDate.AddDays(1))
                    {
                        if (branchData != null)
                        {
                            if (!string.IsNullOrEmpty(branchData.WeekOff1))
                            {
                                if (sDate.DayOfWeek == GetWeekDayName(branchData.WeekOff1))
                                {
                                    holidayData.Add(new EventsHolidaysData()
                                    {
                                        EventDate = sDate,
                                        EventName = "Weekend",
                                        EventType = 1
                                    });
                                }
                            }
                            if (!string.IsNullOrEmpty(branchData.WeekOff2))
                            {
                                if (sDate.DayOfWeek == GetWeekDayName(branchData.WeekOff2))
                                {
                                    holidayData.Add(new EventsHolidaysData()
                                    {
                                        EventDate = sDate,
                                        EventName = "Weekend",
                                        EventType = 1
                                    });
                                }
                            }
                        }
                        var holiday = await _context.StudentHolidayClaender.ProjectTo<TblSysSchoolHolidayCalanderStudentDto>(_mapper.ConfigurationProvider)
                                           .FirstOrDefaultAsync(e => e.BranchCode == request.BranchCode && e.HDate.Date == sDate.Date);
                        if (holiday != null)
                        {
                            holidayData.Add(new EventsHolidaysData()
                            {
                                EventDate = sDate,
                                EventName = holiday.HName,
                                EventNameAr = holiday.HName2,
                                EventType = 2
                            });
                        }
                        var eventData = await _context.SchooScheduleEvents.ProjectTo<TblSysSchooScheduleEventsDto>(_mapper.ConfigurationProvider)
                                           .FirstOrDefaultAsync(e => e.BranchCode == request.BranchCode && e.HDate.Date == sDate.Date && e.IsActive == true && e.IsApproved == true);
                        if (eventData != null)
                        {
                            holidayData.Add(new EventsHolidaysData()
                            {
                                EventDate = sDate,
                                EventName = eventData.EventName,
                                EventNameAr = eventData.EventNameAr,
                                EventType = 3
                            });
                        }
                        sDate = sDate.AddDays(1);
                    }
                    result.EventsHolidaysDataList = holidayData;
                }

                Log.Info("----Info GetBranchEventsHolidaysData method Exit----");
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetBranchEventsHolidaysData Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
            }
            return result;
        }

        public DayOfWeek GetWeekDayName(string name)
        {
            DayOfWeek dayOfWeek = DayOfWeek.Friday;
            switch (name)
            {
                case "Sun":
                    dayOfWeek = DayOfWeek.Sunday;
                    break;
                case "Mon":
                    dayOfWeek = DayOfWeek.Monday;
                    break;
                case "Tue":
                    dayOfWeek = DayOfWeek.Tuesday;
                    break;
                case "Wed":
                    dayOfWeek = DayOfWeek.Wednesday;
                    break;
                case "Thu":
                    dayOfWeek = DayOfWeek.Thursday;
                    break;
                case "Fri":
                    dayOfWeek = DayOfWeek.Friday;
                    break;
                case "Sat":
                    dayOfWeek = DayOfWeek.Saturday;
                    break;
                default:
                    return DayOfWeek.Sunday;

            }
            return dayOfWeek;
        }
    }

    #endregion


    #region GetBranchDashBoard

    public class GetBranchDashBoard : IRequest<SchoolDashboardDto>
    {
        public UserIdentityDto User { get; set; }
        public string BranchCode { get; set; }
    }

    public class GetBranchDashBoardHandler : IRequestHandler<GetBranchDashBoard, SchoolDashboardDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetBranchDashBoardHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SchoolDashboardDto> Handle(GetBranchDashBoard request, CancellationToken cancellationToken)
        {

            Log.Info("----Info GetBranchDashBoard method start----");
            SchoolDashboardDto result = new();
            DateTime nowDate = DateTime.Now;
            var loginDetails = await _context.SystemLogins.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.User.UserId);
            var academicDetails = await _context.SysSchoolAcademicBatches.AsNoTracking().FirstOrDefaultAsync();
            if (loginDetails != null && academicDetails != null)
            {
                result.BranchCode = loginDetails.PrimaryBranch;
                if (!string.IsNullOrEmpty(result.BranchCode))
                {
                    #region DashboardCounts
                    result.TotalStudents = await _context.DefSchoolStudentMaster
                                                               .AsNoTracking().Where(x => x.BranchCode == result.BranchCode && x.IsActive == true)
                                                               .CountAsync();
                    result.StudentsOnLeave = await _context.StudentApplyLeave
                                                  .Join(_context.DefSchoolStudentMaster, SAL => SAL.StuAdmNum, SFH => SFH.StuAdmNum, (SAL, SFH) => new
                                                  {
                                                      SFH.StuAdmNum,
                                                      SFH.BranchCode,
                                                      SAL.IsApproved,
                                                      SAL.LeaveStartDate,
                                                      SAL.LeaveEndDate,
                                                      SAL.AcademicYear
                                                  })
                                                  .AsNoTracking()
                                                  .Where(e => e.BranchCode == request.BranchCode && e.IsApproved == true
                                                     && e.AcademicYear == academicDetails.AcademicYear
                                                     && e.LeaveStartDate.Date <= nowDate.Date && e.LeaveEndDate.Date >= nowDate.Date)
                                                  .CountAsync();
                    result.FeeDueStudents = await _context.DefStudentFeeHeader
                                             .Join(_context.DefSchoolStudentMaster, SFH => SFH.StuAdmNum, SSM => SSM.StuAdmNum, (SAF, SSM) => new
                                             {
                                                 SSM.StuAdmNum,
                                                 SSM.BranchCode,
                                                 SAF.AcademicYear,
                                                 SAF.IsPaid,
                                                 SAF.TermCode
                                             })
                                             .Join(_context.SysSchoolFeeTerms, SMFH => SMFH.TermCode, SFT => SFT.TermCode, (SMFH, SFT) => new
                                             {
                                                 SMFH.StuAdmNum,
                                                 SMFH.BranchCode,
                                                 SMFH.AcademicYear,
                                                 SMFH.IsPaid,
                                                 SMFH.TermCode,
                                                 SFT.FeeDueDate
                                             })
                                              .AsNoTracking()
                                              .Where(e => e.BranchCode == result.BranchCode && e.IsPaid == false
                                                 && e.FeeDueDate.Date <= nowDate.Date)
                                              .Select(x => x.StuAdmNum)
                                              .Distinct()
                                              .CountAsync();


                    result.TotalTeachers = await _context.DefSchoolTeacherMaster.AsNoTracking().Where(x => x.PrimaryBranchCode == result.BranchCode).CountAsync();

                    result.NewRegistrations = await _context.WebStudentRegistration.AsNoTracking().Where(x => x.CreatedOn.Date == nowDate.Date).CountAsync();

                    result.FeeDueTotal = await _context.DefStudentFeeHeader
                                             .Join(_context.DefSchoolStudentMaster, SAF => SAF.StuAdmNum, SSM => SSM.StuAdmNum, (SAF, SSM) => new
                                             {
                                                 SSM.StuAdmNum,
                                                 SSM.BranchCode,
                                                 SAF.AcademicYear,
                                                 SAF.IsPaid,
                                                 SAF.TermCode,
                                                 SAF.NetFeeAmount
                                             })
                                             .Join(_context.SysSchoolFeeTerms, SMFH => SMFH.TermCode, SFT => SFT.TermCode, (SMFH, SFT) => new
                                             {
                                                 SMFH.StuAdmNum,
                                                 SMFH.BranchCode,
                                                 SMFH.AcademicYear,
                                                 SMFH.IsPaid,
                                                 SMFH.TermCode,
                                                 SMFH.NetFeeAmount,
                                                 SFT.FeeDueDate
                                             })
                                              .AsNoTracking()
                                              .Where(e => e.BranchCode == result.BranchCode && e.IsPaid == false
                                                 && e.FeeDueDate.Date <= nowDate.Date)
                                              .SumAsync(x => x.NetFeeAmount);
                    #endregion
                    #region Events
                    result.DashboardEvents = await _context.SchooScheduleEvents.AsNoTracking()
                                                 .Where(x => x.BranchCode == result.BranchCode
                                                 && x.IsApproved == true && x.IsActive == true
                                                   && x.HDate.Date >= nowDate.Date)
                                                 .AsNoTracking()
                                                 .Select(x =>
                                                   new DashboardEventDto
                                                   {
                                                       DateTime = x.HDate,
                                                       EventName = x.EventName,
                                                       EventName2 = x.EventNameAr
                                                   })
                                                 .ToListAsync();
                    #endregion
                    #region TopStudents
                    var allGrades = await _context.SchoolAcedemicClassGrade.AsNoTracking()
                                                 .Where(x => x.IsActive == true)
                                                 .AsNoTracking()
                                                 .ToListAsync();
                    foreach (var gradeData in allGrades)
                    {
                        var topExamGradeResult = await _context.SchoolStudentResultHeader
                                                    .Where(x => x.GradeCode == gradeData.GradeCode)
                                                     .AsNoTracking()
                                                     .OrderByDescending(x => x.CreatedDate)
                                                     .FirstOrDefaultAsync();
                        if (topExamGradeResult != null)
                        {
                            var gradeTopStudent = await _context.SchoolStudentResultDetails
                                             .Join(_context.DefSchoolStudentMaster, SRD => SRD.StudentAdmNumber, SRH => SRH.StuAdmNum, (SRD, SRH) => new
                                             {
                                                 SRD.StudentAdmNumber,
                                                 SRD.StudentResultHeaderId,
                                                 SRD.MarksObtained,
                                                 SRH.StuName,
                                                 SRH.StuName2,
                                                 SRH.GradeCode,
                                                 SRH.BranchCode
                                             })
                                             .Join(_context.SchoolAcedemicClassGrade, SMFH => SMFH.GradeCode, SFT => SFT.GradeCode, (SMFH, SFT) => new
                                             {
                                                 SMFH.StudentAdmNumber,
                                                 SMFH.StudentResultHeaderId,
                                                 SMFH.MarksObtained,
                                                 SMFH.StuName,
                                                 SMFH.StuName2,
                                                 SMFH.GradeCode,
                                                 SFT.GradeName,
                                                 SFT.GradeName2,
                                                 SMFH.BranchCode
                                             })
                                              .AsNoTracking()
                                              .Where(e => e.StudentResultHeaderId == topExamGradeResult.Id
                                              && e.BranchCode == result.BranchCode)
                                              .OrderByDescending(x => x.MarksObtained)
                                              .FirstOrDefaultAsync();
                            if (gradeTopStudent != null)
                            {
                                result.DashboardStudents.Add(new DashboardStudentDto()
                                {
                                    Grade = gradeTopStudent.GradeName,
                                    Grade2 = gradeTopStudent.GradeName2,
                                    GradeCode = gradeTopStudent.GradeCode,
                                    StuAdmNum = gradeTopStudent.StudentAdmNumber,
                                    StudentName = gradeTopStudent.StuName,
                                    StudentName2 = gradeTopStudent.StuName2,
                                }
                                              );
                            }
                        }
                    }
                    #endregion

                    #region TodayAttandance
                    var presentTodayAttCount = await _context.StudentAttendance
                                           .Join(_context.DefSchoolStudentMaster, SRD => SRD.StuAdmNum, SRH => SRH.StuAdmNum, (SRD, SRH) => new
                                           {
                                               SRD.StuAdmNum,
                                               SRD.AtnDate,
                                               SRD.AtnTimeIn,
                                               SRD.AtnTimeOut,
                                               SRD.AtnFlag,
                                               SRD.IsLeave,
                                               SRD.LeaveCode,
                                               SRD.AcademicYear,
                                               SRH.BranchCode
                                           })
                                           .AsNoTracking()
                                           .Where(x => x.BranchCode == result.BranchCode
                                              && x.AtnDate.Date == nowDate.Date && x.AtnFlag == "P")
                                           .CountAsync();
                    //var leaveTodayAttCount = await _context.StudentApplyLeave
                    //                     .Join(_context.DefSchoolStudentMaster, SRD => SRD.StuAdmNum, SRH => SRH.StuAdmNum, (SRD, SRH) => new
                    //                     {
                    //                         SRD.StuAdmNum,
                    //                         SRD.LeaveCode,
                    //                         SRD.LeaveStartDate,
                    //                         SRD.LeaveEndDate,
                    //                         SRD.IsApproved,
                    //                         SRD.ApprovedDate,
                    //                         SRH.BranchCode
                    //                     })
                    //                     .AsNoTracking()
                    //                     .Where(x => x.BranchCode == result.BranchCode
                    //                        && x.LeaveStartDate.Date >= nowDate.Date
                    //                        && x.LeaveEndDate.Date <= nowDate.Date)
                    //                     .CountAsync();
                    var absentTodayAttData = result.TotalStudents - presentTodayAttCount;
                    result.ChartAttandanceData.Add(new ChartAttandanceDto() { TypeID = 1, TotalStudents = result.TotalStudents, PresentStudents = presentTodayAttCount, AbsentStudents = absentTodayAttData });
                    #endregion
                    #region MonthlyAttandance
                    var monthStartDate = new DateTime(nowDate.Year, nowDate.Month, 1);
                    var totalMonthDays = (nowDate - monthStartDate).Days + 1;
                    var presentMonthlyAttCount = await _context.StudentAttendance
                                         .Join(_context.DefSchoolStudentMaster, SRD => SRD.StuAdmNum, SRH => SRH.StuAdmNum, (SRD, SRH) => new
                                         {
                                             SRD.StuAdmNum,
                                             SRD.AtnDate,
                                             SRD.AtnTimeIn,
                                             SRD.AtnTimeOut,
                                             SRD.AtnFlag,
                                             SRD.IsLeave,
                                             SRD.LeaveCode,
                                             SRD.AcademicYear,
                                             SRH.BranchCode
                                         })
                                         .AsNoTracking()
                                         .Where(x => x.BranchCode == result.BranchCode
                                            && x.AtnDate.Date >= monthStartDate.Date && x.AtnFlag == "P")
                                         .CountAsync();
                    var absentMonthlyAttData = (totalMonthDays * result.TotalStudents) - presentMonthlyAttCount;
                    result.ChartAttandanceData.Add(new ChartAttandanceDto() { TypeID = 2, TotalStudents = (totalMonthDays * result.TotalStudents), PresentStudents = presentMonthlyAttCount, AbsentStudents = absentMonthlyAttData });
                    #endregion
                    #region YearlyAttandance
                    var yearStartDate = academicDetails.AcademicStartDate;
                    var totalYearDays = (nowDate - monthStartDate).Days + 1;
                    var presentYearAttCount = await _context.StudentAttendance
                                         .Join(_context.DefSchoolStudentMaster, SRD => SRD.StuAdmNum, SRH => SRH.StuAdmNum, (SRD, SRH) => new
                                         {
                                             SRD.StuAdmNum,
                                             SRD.AtnDate,
                                             SRD.AtnTimeIn,
                                             SRD.AtnTimeOut,
                                             SRD.AtnFlag,
                                             SRD.IsLeave,
                                             SRD.LeaveCode,
                                             SRD.AcademicYear,
                                             SRH.BranchCode
                                         })
                                         .AsNoTracking()
                                         .Where(x => x.BranchCode == result.BranchCode
                                            && x.AtnDate.Date >= yearStartDate.Date && x.AtnFlag == "P")
                                         .CountAsync();
                    var absentYearAttData = (totalYearDays * result.TotalStudents) - presentYearAttCount;
                    result.ChartAttandanceData.Add(new ChartAttandanceDto() { TypeID = 3, TotalStudents = (totalYearDays * result.TotalStudents), PresentStudents = presentYearAttCount, AbsentStudents = absentYearAttData });
                    #endregion
                }
            }
            Log.Info("----Info GetBranchDashBoard method Ends----");
            return result;
        }
    }

    #endregion
}
