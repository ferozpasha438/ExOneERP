using AutoMapper;
using CIN.Application.MobileMgt.Dtos;
using CIN.DB;
using CIN.Domain.SystemSetup;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIN.Application.SystemQuery
{


    public class CheckLogin : IRequest<(int, string, string)>
    {
        public CheckCINServerMetaDataDto Input { get; set; }
    }

    public class CheckLoginHandler : IRequestHandler<CheckLogin, (int, string, string)>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CheckLoginHandler(CINDBOneContext context)
        {
            _context = context;
        }
        public async Task<(int, string, string)> Handle(CheckLogin request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.SystemLogins.FirstOrDefaultAsync(e => e.UserName == request.Input.UserName);

                if (user is not null)
                {
                    //if (SecurePasswordHasher.Verify(request.Input.Password, user.Password))
                    if (SecurePasswordHasher.IsPasswordDecoded(user.Password, request.Input.Password))
                    {
                        return (user.Id, user.PrimaryBranch, user.LoginType);
                    }
                    else
                        return (0, string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return (0, string.Empty, string.Empty);
            //return await _context.Users.AnyAsync(e => e.UserName == request.Input.UserName && e.Password == request.Input.Password && e.CINNumber == request.Input.CINNumber);
        }
    }


    #region Mobile Login

    public class CheckMobileLogin : IRequest<MobileLoginResponseDto>
    {
        public CheckMobileCINServerMetaDataDto Input { get; set; }
    }

    public class CheckMobileLoginHandler : IRequestHandler<CheckMobileLogin, MobileLoginResponseDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CheckMobileLoginHandler(CINDBOneContext context)
        {
            _context = context;
        }
        public async Task<MobileLoginResponseDto> Handle(CheckMobileLogin request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.SystemLogins.FirstOrDefaultAsync(e => e.UserName == request.Input.UserName && e.IsLoginAllow);

                if (user is not null)
                {
                    var obj = request.Input;
                    if (!user.IsSigned)
                    {
                        //if (SecurePasswordHasher.Verify(request.Input.Password, user.Password))
                        if (SecurePasswordHasher.IsPasswordDecoded(user.Password, obj.Password))
                        {
                            var mappings = _context.UserSiteMappings.Where(e => e.LoginId == user.Id.ToString());

                            if (await mappings.AnyAsync())
                            {
                                var sites = await _context.OprSites.Where(e => mappings.Any(sc => sc.SiteCode == e.SiteCode))
                                    .Select(e => new { e.SiteGeoLatitude, e.SiteGeoLongitude, e.SiteGeoGain, e.SiteName, e.SiteArbName, e.SiteCode }).ToListAsync();
                                string siteName = string.Empty, siteNameAr = string.Empty, siteCode = string.Empty;
                                decimal? SiteGeoLatitude = 0, SiteGeoLongitude = 0, SiteGeoGain = 0;
                                long? employeeId = 0;
                                foreach (var site in sites)
                                {
                                    var meters = Extensions.GetMeters((double)site.SiteGeoLatitude, (double)site.SiteGeoLongitude, (double)obj.SiteGeoLatitude, (double)obj.SiteGeoLongitude);

                                    //if (obj.SiteLocationNvMeter <= meters && meters <= obj.SiteLocationPvMeter)
                                    //{

                                    //}

                                    siteName = site.SiteName;
                                    siteNameAr = site.SiteArbName;
                                    siteCode = site.SiteCode;
                                    SiteGeoLatitude = site.SiteGeoLatitude;
                                    SiteGeoLongitude = site.SiteGeoLongitude;
                                    SiteGeoGain = site.SiteGeoGain;

                                    var mapping = await mappings.Where(e => e.SiteCode == site.SiteCode).FirstOrDefaultAsync();
                                    employeeId = mapping.EmployeeID;
                                    break;

                                    //    return new() { Message = "GreenZone", Status = true, Id = 0 };
                                    //else if (obj.SiteLocationMeter < meters && meters <= obj.SiteLocationExtraMeter)
                                    //    return new() { Message = "OrangeZone", Status = true, Id = 0 };
                                    //else
                                    //    return new() { Message = "RedZone", Status = true, Id = 0 };

                                }

                                if (siteCode.HasValue())
                                {
                                    return new()
                                    {
                                        Id = user.Id,
                                        BranchCode = user.PrimaryBranch,
                                        SiteName = siteName,
                                        SiteNameAR = siteNameAr,
                                        SiteCode = siteCode,
                                        InnerRadius = obj.SiteLocationPvMeter,
                                        OuterRadius = obj.SiteLocationExtraMeter,
                                        SiteGeoLatitude = SiteGeoLatitude,
                                        SiteGeoLongitude = SiteGeoLongitude,
                                        SiteGeoGain = SiteGeoGain,
                                        EmployeeId = employeeId,
                                        IsLoginAllow = user.IsLoginAllow
                                    };
                                }
                                return new() { Id = -3 };
                            }

                            return new() { Id = -2 };
                        }

                        return new() { Id = 0 };
                    }
                    return new() { Id = -1 };

                }
            }
            catch (Exception ex)
            {
                return new() { Id = 0 };
            }
            return new() { Id = 0 };
            //return await _context.Users.AnyAsync(e => e.UserName == request.Input.UserName && e.Password == request.Input.Password && e.CINNumber == request.Input.CINNumber);
        }
    }

    #endregion

    #region Mobile Login By Employee

    public class CheckEmployeeMobileLogin : IRequest<EmployeeMobileLoginResponseDto>
    {
        public MobileLoginInput Input { get; set; }
    }

    public class CheckEmployeeMobileLoginHandler : IRequestHandler<CheckEmployeeMobileLogin, EmployeeMobileLoginResponseDto>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public CheckEmployeeMobileLoginHandler(CINDBOneContext context, DMCContext contextDMC)
        {
            _context = context;
            _contextDMC = contextDMC;
        }
        public async Task<EmployeeMobileLoginResponseDto> Handle(CheckEmployeeMobileLogin request, CancellationToken cancellationToken)
        {
           
                EmployeeMobileLoginResponseDto res = new() { MobileLoginId = 0,ShiftsList=new() };
                var shiftMaster = await _contextDMC.HRM_DEF_EmployeeShiftMasters.AsNoTracking().ToListAsync();
                var siteMaster = await _context.OprSites.AsNoTracking().ToListAsync();
                string EmployeeNumber = request.Input.UserName;
                var skillsets = await _context.TblOpSkillsets.AsNoTracking().ToListAsync();
                var user = await _context.ErpSysEmployeeLogin.FirstOrDefaultAsync(e => e.EmployeeNumber == request.Input.UserName);
                if (user is null || (!SecurePasswordHasher.IsPasswordDecoded(user.Password, request.Input.Password)))
                {
                    return new() { MobileLoginId = -1, ErrorMessage = "Invalid Username & Password" };
                }
                res.MobileLoginId = user.Id;
                var Employee = await _contextDMC.HRM_TRAN_Employees.FirstOrDefaultAsync(e=>e.EmployeeNumber== EmployeeNumber);
            if (Employee is null)
            {
                return new() { MobileLoginId = -2, ErrorMessage = "Invalid Employee Number" };
            }
                    res.EmployeeNumber = Employee.EmployeeNumber;
                    res.EmployeeNameAr = Employee.EmployeeName_AR;
                    res.EmployeeNameEn = Employee.EmployeeName;
                
                return res;

           

        }
    }

    #endregion


}
