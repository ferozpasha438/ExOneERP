using AutoMapper;
using AutoMapper.QueryableExtensions;
using CIN.Application.Common;
using CIN.Application.MobileMgt.Dtos;
using CIN.DB;
using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.MobileMgt.Queries
{

    #region SetSignInStatus

    public class SetSignInStatus : IRequest<bool>
    {
        public UserMobileIdentityDto User { get; set; }
        public bool Input { get; set; }
        public int? UserId { get; set; }
    }

    public class SetSignInStatusHandler : IRequestHandler<SetSignInStatus, bool>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public SetSignInStatusHandler(CINDBOneContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(SetSignInStatus request, CancellationToken cancellationToken)
        {
            int userId = request.UserId ?? request.User.UserId;
            var obj = await _context.SystemLogins.FirstOrDefaultAsync(e => e.Id == userId);

            obj.IsSigned = request.Input;
            _context.SystemLogins.Update(obj);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    #endregion



    #region CheckGeoLocation

    //public class CheckGeoLocation : IRequest<MobileCtrollerDto>
    //{
    //    public UserMobileIdentityDto User { get; set; }
    //    public CheckGeoLocationDto Input { get; set; }
    //}

    //public class CheckGeoLocationHandler : IRequestHandler<CheckGeoLocation, MobileCtrollerDto>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;

    //    public CheckGeoLocationHandler(CINDBOneContext context)
    //    {
    //        _context = context;
    //    }
    //    public async Task<MobileCtrollerDto> Handle(CheckGeoLocation request, CancellationToken cancellationToken)
    //    {
    //        try
    //        {
    //            var isSiteAssigned = await _context.SystemLogins.AnyAsync(e => e.Id == request.User.UserId && e.SiteCode == request.User.SiteCode);
    //            if (isSiteAssigned)
    //            {
    //                var obj = request.Input;
    //                var site = await _context.OprSites.FirstOrDefaultAsync(e =>
    //                e.SiteCode == request.User.SiteCode
    //                && e.SiteGeoLatitude == obj.SiteGeoLatitude
    //                && e.SiteGeoLongitude == obj.SiteGeoLatitude);

    //                if (site is not null)
    //                {
    //                    return new() { Message = ApiMessageInfo.Success, Status = true, Id = site.Id };
    //                }
    //            }
    //            return new() { Message = ApiMessageInfo.Failed, Status = false, Id = 0 };
    //        }
    //        catch (Exception ex)
    //        {
    //            return new() { Message = ex.Message, Status = false, Id = 0 };
    //        }
    //    }
    //}

    #endregion


    #region LocationTracking

    public class LocationTracking : IRequest<MobileCtrollerDto>
    {
        public UserMobileIdentityDto User { get; set; }
        public LocationTrackingDto Input { get; set; }
    }

    public class LocationTrackingHandler : IRequestHandler<LocationTracking, MobileCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public LocationTrackingHandler(CINDBOneContext context)
        {
            _context = context;
        }
        public async Task<MobileCtrollerDto> Handle(LocationTracking request, CancellationToken cancellationToken)
        {
            try
            {
                var obj = request.Input;
                var site = await _context.OprSites.AsNoTracking().FirstOrDefaultAsync(e => e.SiteCode == request.User.SiteCode);

                if (site is not null)
                {

                    var meters = Extensions.GetMeters((double)site.SiteGeoLatitude, (double)site.SiteGeoLongitude, (double)obj.SiteGeoLatitude, (double)obj.SiteGeoLongitude);

                        return new() { Message = "GreenZone", Status = true, Id = 0 };

                    //if (obj.SiteLocationNvMeter <= meters && meters <= obj.SiteLocationPvMeter)
                    //    return new() { Message = "GreenZone", Status = true, Id = 0 };
                    //else if (obj.SiteLocationPvMeter < meters && meters <= obj.SiteLocationExtraMeter)
                    //    return new() { Message = "OrangeZone", Status = true, Id = 0 };
                    //else
                    //    return new() { Message = "RedZone", Status = false, Id = 0 };



                    //if (obj.SiteGeoLatitude <= site.SiteGeoLatitude && obj.SiteGeoLongitude <= site.SiteGeoLongitude)
                    //    return new() { Message = "GreenZone", Status = true, Id = site.Id };
                    //else if (obj.SiteGeoLatitude <= (site.SiteGeoLatitude + obj.AppSiteGeoLatitude) &&
                    //    (obj.SiteGeoLongitude <= site.SiteGeoLongitude + obj.AppSiteGeoLongitude))
                    //    return new() { Message = "OrangeZone", Status = true, Id = site.Id };
                    //else
                    //    return new() { Message = "RedZone", Status = true, Id = site.Id };


                }
                return new() { Message = ApiMessageInfo.Failed, Status = false, Id = 0 };

            }
            catch (Exception ex)
            {
                return new() { Message = ex.Message, Status = false, Id = 0 };
            }
        }

    }

    #endregion


    #region IncidentReportQuery

    public class GetIncidentReportList : IRequest<PaginatedList<TblErpSysIncidentReportDto>>
    {
        public UserMobileIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetIncidentReportListHandler : IRequestHandler<GetIncidentReportList, PaginatedList<TblErpSysIncidentReportDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetIncidentReportListHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblErpSysIncidentReportDto>> Handle(GetIncidentReportList request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var list = await _context.IncidentReports.AsNoTracking()
              .Where(e => //e.CompanyId == request.CompanyId &&
                            (e.Title.Contains(search)))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<TblErpSysIncidentReportDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);
            return list;
        }
    }

    public class IncidentReportQuery : IRequest<MobileCtrollerDto>
    {
        public UserMobileIdentityDto User { get; set; }
        public TblErpSysIncidentReportDto Input { get; set; }
    }

    public class IncidentReportQueryHandler : IRequestHandler<IncidentReportQuery, MobileCtrollerDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public IncidentReportQueryHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<MobileCtrollerDto> Handle(IncidentReportQuery request, CancellationToken cancellationToken)
        {

            try
            {
                Log.Info("----Info IncidentReportQuery method start----");
                var obj = request.Input;
                TblErpSysIncidentReport report = new()
                {
                    Title = obj.Title,
                    Description = obj.Description,
                    SiteGeoLatitude = obj.SiteGeoLatitude,
                    SiteGeoLongitude = obj.SiteGeoLongitude,
                    ImagePath = obj.ImagePath,
                    CreatedOn = DateTime.Now,
                    CreatedBy = request.User.UserId
                };

                await _context.IncidentReports.AddAsync(report);
                await _context.SaveChangesAsync();

                return ApiMessageInfo.Status(1, true, report.Id);
            }
            catch (Exception ex)
            {
                Log.Error("Error in IncidentReportQuery Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0, false);
            }
        }
    }

    #endregion



    #region CheckOutEmployee

    public class CheckOutListQuery : IRequest<PaginatedList<HRM_TRAN_EmployeeTimeChartDto>>
    {
        public UserMobileIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class CheckOutListQueryHandler : IRequestHandler<CheckOutListQuery, PaginatedList<HRM_TRAN_EmployeeTimeChartDto>>
    {
        private readonly DMC2Context _dmcContext;
        private readonly IMapper _mapper;
        public CheckOutListQueryHandler(IMapper mapper, DMC2Context dmcContext)
        {
            _mapper = mapper;
            _dmcContext = dmcContext;
        }
        public async Task<PaginatedList<HRM_TRAN_EmployeeTimeChartDto>> Handle(CheckOutListQuery request, CancellationToken cancellationToken)
        {
            var search = request.Input.Query;
            var currentDate = DateTime.Now;
            var list = _dmcContext.EmployeePostedAttendance.AsNoTracking().Where(e => e.Date >= currentDate && e.EmployeeID == request.User.EmployeeId);

            var newList = await list.Where(e => e.EmployeeID.ToString().Contains(search))
             .OrderByDescending(e => e.ID)
            .ProjectTo<HRM_TRAN_EmployeeTimeChartDto>(_mapper.ConfigurationProvider)
               .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return newList;
        }
    }


    #endregion
    #region CheckInEmployee
    public class CheckInEmployeeQuery : IRequest<MobileCtrollerDto>
    {
        public UserMobileIdentityDto User { get; set; }
        public HRM_TRAN_EmployeeTimeChartDto Input { get; set; }
    }

    public class CheckInEmployeeQueryHandler : IRequestHandler<CheckInEmployeeQuery, MobileCtrollerDto>
    {
        private readonly DMC2Context _dmcContext;
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public CheckInEmployeeQueryHandler(IMapper mapper, DMC2Context dmcContext, CINDBOneContext context)
        {
            _mapper = mapper;
            _dmcContext = dmcContext;
            _context = context;
        }
        public async Task<MobileCtrollerDto> Handle(CheckInEmployeeQuery request, CancellationToken cancellationToken)
        {

            try
            {
                Log.Info("----Info CheckInEmployeeQuery method start----");
                var obj = request.Input;                
                var employee = await _dmcContext.HRM_TRAN_Employees
                    .Select(e => new { e.EmployeeID, e.EmployeeName }).FirstOrDefaultAsync(e => e.EmployeeID == request.User.EmployeeId);
                var userSite = await _context.UserSiteMappings.FirstOrDefaultAsync(e => e.LoginId == request.User.UserId.ToString());
                HRM_TRAN_EmployeeTimeChart report = new()
                {
                    EmployeeID = employee.EmployeeID,
                    Name = employee.EmployeeName,
                    Date = DateTime.Now.Date,
                    InTime = TimeSpan.Parse(obj.InTime),
                    OutTime = TimeSpan.Parse(obj.OutTime),
                    AttnFlag = 'P',
                    ShiftId = 1,
                    SiteCode = userSite.SiteCode,
                    ShiftNumber = 1
                };

                await _dmcContext.EmployeePostedAttendance.AddAsync(report);
                await _dmcContext.SaveChangesAsync();

                return ApiMessageInfo.Status(1, true, 1);
            }
            catch (Exception ex)
            {
                Log.Error("Error in CheckInEmployeeQuery Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return ApiMessageInfo.Status(0, false);
            }
        }
    }



    #endregion




}
