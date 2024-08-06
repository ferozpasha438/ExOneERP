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








    #region GetOpeartionsDashboard
    public class GetOpeartionsDashboard : IRequest<OperationsDashboardDto>
    {
        public UserIdentityDto User { get; set; }
       
    }

    public class GetOpeartionsDashboardHandler : IRequestHandler<GetOpeartionsDashboard, OperationsDashboardDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetOpeartionsDashboardHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<OperationsDashboardDto> Handle(GetOpeartionsDashboard request, CancellationToken cancellationToken)
        {
            bool isArabic = request.User.Culture.IsArab();
            OperationsDashboardDto dashboardData = new OperationsDashboardDto();
            dashboardData.NoOfCustomers = await _context.OprCustomers.AsNoTracking().CountAsync();
            dashboardData.NoOfSites = await _context.OprSites.AsNoTracking().CountAsync();
            dashboardData.NoOfUnAppSur = await _context.OprEnquiries.AsNoTracking().Where(e=>e.IsSurveyCompleted && !e.IsApproved).CountAsync();
            dashboardData.NoOfAppSur = await _context.OprEnquiries.AsNoTracking().Where(e=>e.IsApproved).CountAsync();
            dashboardData.NoOfUnAppEst = await _context.OP_HRM_TEMP_Projects.AsNoTracking().Where(e=>e.IsEstimationCompleted && !e.IsConvertedToProposal).CountAsync();
            dashboardData.NoOfAppEst = await _context.OP_HRM_TEMP_Projects.AsNoTracking().Where(e=>e.IsConvertedToProposal).CountAsync();
            dashboardData.NoOfContracts = await _context.OP_HRM_TEMP_Projects.AsNoTracking().Where(e=>e.IsConvrtedToContract).CountAsync();
            dashboardData.EnquiriesCurentMonth = await _context.OprEnquiryHeaders.AsNoTracking().Where(e=>e.EstimateClosingDate.Month==DateTime.UtcNow.Month&& e.EstimateClosingDate.Year == DateTime.UtcNow.Year).CountAsync();
            dashboardData.EnquiriesCurentYear = await _context.OprEnquiryHeaders.AsNoTracking().Where(e=>e.EstimateClosingDate.Year==DateTime.UtcNow.Year).CountAsync();






           
            return dashboardData;
        }
    }
    #endregion




    #region GetOpeartionsDashboardByFilter
    public class GetOpeartionsDashboardByFilter : IRequest<OperationsDashboardOpDto>
    {
      //  public UserIdentityDto User { get; set; }
        public OperationsDashboardIpDto Input { get; set; }

    }

    public class GetOpeartionsDashboardByFilterHandler : IRequestHandler<GetOpeartionsDashboardByFilter, OperationsDashboardOpDto>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public GetOpeartionsDashboardByFilterHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<OperationsDashboardOpDto> Handle(GetOpeartionsDashboardByFilter request, CancellationToken cancellationToken)
        {
            OperationsDashboardOpDto dashboardData = new OperationsDashboardOpDto();

            try
            {
                bool isArabic = false;//request.User.Culture.IsArab();
                request.Input.Date = request.Input.Date is null ? Convert.ToDateTime(DateTime.Now, CultureInfo.InvariantCulture) : Convert.ToDateTime(request.Input.Date.Value, CultureInfo.InvariantCulture);
              
                //request.Input.BranchCode = string.IsNullOrEmpty(request.Input.BranchCode) ? request.User.BranchCode : request.Input.BranchCode;
              
                var ProjectSitesUnderBranch = _context.TblOpProjectSites.Where(e =>string.IsNullOrEmpty(request.Input.BranchCode) || e.BranchCode == request.Input.BranchCode).Select(ps => new { ps.ProjectCode, ps.SiteCode, ps.ProjectNameEng, ps.ProjectNameArb }).ToList();

                if (ProjectSitesUnderBranch.Count == 0)
                {
                    return dashboardData;
                }
                var employees = await _contextDMC.HRM_TRAN_Employees.AsNoTracking().ToListAsync();
                var projects = await _context.OP_HRM_TEMP_Projects.AsNoTracking().ToListAsync();//.Where(e => UniqRows.Select(s => s.ProjectCode).Contains(e.ProjectCode)).AsNoTracking().ToListAsync();
                var sites = await _context.OprSites.AsNoTracking().ToListAsync();//.Where(e => UniqRows.Select(s => s.ProjectCode).Contains(e.SiteCode)).AsNoTracking().ToListAsync();



                ProjectSitesUnderBranch.Select(e => e.ProjectCode).Distinct().ToList().ForEach(e => {
                dashboardData.ProjectsSelectionList.Add(new(){
                Value=e,
                Text=isArabic?projects.FirstOrDefault(p=>p.ProjectCode==e).ProjectNameArb:projects.FirstOrDefault(p=>p.ProjectCode==e).ProjectNameEng,
                TextTwo= e +"-"+ (isArabic? projects.FirstOrDefault(p=>p.ProjectCode==e).ProjectNameArb:projects.FirstOrDefault(p=>p.ProjectCode==e).ProjectNameEng),
                });
                });
                ProjectSitesUnderBranch.Select(e => e.SiteCode).Distinct().ToList().ForEach(e => {
                dashboardData.SitesSelectionList.Add(new(){
                Value=e,
                Text=isArabic?sites.FirstOrDefault(p=>p.SiteCode==e).SiteArbName:sites.FirstOrDefault(p=>p.SiteCode==e).SiteName,
                TextTwo=e+"-"+(isArabic?sites.FirstOrDefault(p=>p.SiteCode==e).SiteArbName:sites.FirstOrDefault(p=>p.SiteCode==e).SiteName),
                });
                });

                var UniqRows = ProjectSitesUnderBranch.Select(e => new { e.ProjectCode, e.SiteCode }).Distinct().ToList();


                var totalRoastersList =await _context.TblOpMonthlyRoasterForSites.Where(e =>e.Month==request.Input.Date.Value.Month  && e.Year==request.Input.Date.Value.Year && UniqRows.Select(s => s.ProjectCode).Contains(e.ProjectCode) && UniqRows.Select(s => s.SiteCode).Contains(e.SiteCode) && e.IsPrimaryResource/* && (e.SkillsetCode==request.Input.SkillsetCode)*/).ToListAsync();   // default skillset= SST000002

                if (totalRoastersList.Count == 0)
                {
                    return dashboardData;
                }
                var totalAttendanceReport = await _context.EmployeeAttendance.ProjectTo<DashboardEmployeeAttendanceDto>(_mapper.ConfigurationProvider).Where(e => (e.AttnDate == request.Input.Date) && (e.IsLate.HasValue) && UniqRows.Select(s => s.ProjectCode).Contains(e.ProjectCode) && UniqRows.Select(s => s.SiteCode).Contains(e.SiteCode) && (e.SkillsetCode == request.Input.SkillsetCode || string.IsNullOrEmpty(e.SkillsetCode))).ToListAsync();
                var totalLeavesList = await _context.EmployeeLeaves.Where(e =>e.AttnDate == request.Input.Date).ToListAsync();

                switch (request.Input.Date.Value.Day)
                {
                    case 1:
                        totalRoastersList = totalRoastersList.Where(e => e.S1 == "" || e.S1 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0 ).ToList().ForEach(e=> {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r=>r.ProjectCode==e.ProjectCode && r.SiteCode==e.SiteCode && r.EmployeeNumber==e.EmployeeNumber && r.S1==e.ShiftCode).SkillsetCode;
                        });



                        totalRoastersList.ForEach(e=> {
                             if (!(totalAttendanceReport.Any(t=>t.ProjectCode==e.ProjectCode&&t.SiteCode==e.SiteCode && e.EmployeeNumber==t.EmployeeNumber && e.S1==t.ShiftCode)))
                                totalAttendanceReport.Add(new() {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S1,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S1!="" &&e.S1!="O"),
                                   
                                });
                        
                        });
                        break;
                    case 2:
                        totalRoastersList = totalRoastersList.Where(e => e.S2 == "" || e.S2 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S2 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S2 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S2,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S2!="" &&e.S2!="O"),
                                });

                        });
                        break;

                    case 3:
                        totalRoastersList = totalRoastersList.Where(e => e.S3 == "" || e.S3 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S3 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S3 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S3,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S3!="" &&e.S3!="O"),
                                });

                        });
                        break;
                    case 4:
                        totalRoastersList = totalRoastersList.Where(e => e.S4 == "" || e.S4 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S4 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S4 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S4,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S4!="" &&e.S4!="O"),
                                });

                        });
                        break;
                    case 5:
                        totalRoastersList = totalRoastersList.Where(e => e.S5 == "" || e.S5 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S5 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S5 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S5,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S5!="" &&e.S5!="O"),
                                });

                        });
                        break;

                    case 6:
                        totalRoastersList = totalRoastersList.Where(e => e.S6 == "" || e.S6 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S6 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S6 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S6,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S6!="" &&e.S6!="O"),
                                });

                        });
                        break;

                    case 7:
                        totalRoastersList = totalRoastersList.Where(e => e.S7 == "" || e.S7 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S7 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S7 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S7,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S7!="" &&e.S7!="O"),
                                });

                        });
                        break;

                    case 8:
                        totalRoastersList = totalRoastersList.Where(e => e.S8 == "" || e.S8 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S8 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S8 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S8,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S8!="" &&e.S8!="O"),
                                });

                        });
                        break;

                    case 9:
                        totalRoastersList = totalRoastersList.Where(e => e.S9 == "" || e.S9 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S9 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S9 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S9,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S9!="" &&e.S9!="O"),
                                });

                        });
                        break;

                    case 10:
                        totalRoastersList = totalRoastersList.Where(e => e.S10 == "" || e.S10 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S10 == e.ShiftCode).SkillsetCode;
                        });

                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S10 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S10,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S10!="" &&e.S10!="O"),
                                });

                        });
                        break;

                    case 11:
                        totalRoastersList = totalRoastersList.Where(e => e.S11 == "" || e.S11 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S11 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S11 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S11,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S11!="" &&e.S11!="O"),
                                });

                        });
                        break;

                    case 12:
                        totalRoastersList = totalRoastersList.Where(e => e.S12 == "" || e.S12 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S12 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S12 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S12,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S12!="" &&e.S12!="O"),
                                });

                        });
                        break; 

                    case 13:
                        totalRoastersList = totalRoastersList.Where(e => e.S13 == "" || e.S13 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S13 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S13 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S13,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S13!="" &&e.S13!="O"),
                                });

                        });
                        break;

                    case 14:
                        totalRoastersList = totalRoastersList.Where(e => e.S14 == "" || e.S14 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S14 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S14 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S14,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S14!="" &&e.S14!="O"),
                                });

                        });
                        break;
                    case 15:
                        totalRoastersList = totalRoastersList.Where(e => e.S15 == "" || e.S15 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S15 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S15 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S15,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S15!="" &&e.S15!="O"),
                                });

                        });
                        break;
                    case 16:
                        totalRoastersList = totalRoastersList.Where(e => e.S16 == "" || e.S16 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S16 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S16 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S16,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S16!="" &&e.S16!="O"),
                                });

                        });
                        break;
                    case 17:
                        totalRoastersList = totalRoastersList.Where(e => e.S17 == "" || e.S17 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S17 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S17 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S17,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S17!="" &&e.S17!="O"),
                                });

                        });
                        break;
                    case 18:
                        totalRoastersList = totalRoastersList.Where(e => e.S18 == "" || e.S18 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S18 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S18 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S18,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S18!="" &&e.S18!="O"),
                                });

                        });
                        break;
                    case 19:
                        totalRoastersList = totalRoastersList.Where(e => e.S19 == "" || e.S19 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S19 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S19 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S19,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S19!="" &&e.S19!="O"),
                                });

                        });
                        break;
                    case 20:
                        totalRoastersList = totalRoastersList.Where(e => e.S20 == "" || e.S20 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S20 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S20 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S20,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S20!="" &&e.S20!="O"),
                                });

                        });
                        break;
                    case 21:
                        totalRoastersList = totalRoastersList.Where(e => e.S21 == "" || e.S21 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S21 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S21 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S21,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S21!="" &&e.S21!="O"),
                                });

                        });
                        break;
                    case 22:
                        totalRoastersList = totalRoastersList.Where(e => e.S22 == "" || e.S22 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S22 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S22 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S22,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S22!="" &&e.S22!="O"),
                                });

                        });
                        break;
                    case 23:
                        totalRoastersList = totalRoastersList.Where(e => e.S23 == "" || e.S23 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S23 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S23 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S23,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S23!="" &&e.S23!="O"),
                                });

                        });
                        break;
                    case 24:
                        totalRoastersList = totalRoastersList.Where(e => e.S24 == "" || e.S24 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S24 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S24 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S24,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S24!="" &&e.S24!="O"),
                                });

                        });
                        break;
                    case 25:
                        totalRoastersList = totalRoastersList.Where(e => e.S25 == "" || e.S25 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S25 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S25 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S25,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S25!="" &&e.S25!="O"),
                                });

                        });
                        break;
                    case 26:
                        totalRoastersList = totalRoastersList.Where(e => e.S26 == "" || e.S26 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S26 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S26 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S26,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S26!="" &&e.S26!="O"),
                                });

                        });
                        break;
                    case 27:
                        totalRoastersList = totalRoastersList.Where(e => e.S27 == "" || e.S27 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S27 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S27 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S27,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S27!="" &&e.S27!="O"),
                                });

                        });
                        break;
                    case 28:
                        totalRoastersList = totalRoastersList.Where(e => e.S28 == "" || e.S28 != "x" ).ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S28 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S28 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S28,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S28!="" &&e.S28!="O"),
                                });

                        });
                        break;
                    case 29:
                        totalRoastersList = totalRoastersList.Where(e => e.S29 == "" || e.S29 != "x" ).ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S29 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S29 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S29,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S29!="" &&e.S29!="O"),
                                });

                        });
                        break;
                    case 30:
                        totalRoastersList = totalRoastersList.Where(e => e.S30 == "" || e.S30 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S30 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S30 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S30,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S30!="" &&e.S30!="O"),
                                });

                        });
                        break;
                    case 31:
                        totalRoastersList = totalRoastersList.Where(e => e.S31 == "" || e.S31 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt==0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S31 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S31 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S31,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",SkillsetCode=e.SkillsetCode,IsOnLeave=totalLeavesList.Any(l=>l.EmployeeNumber==e.EmployeeNumber),
                                   OutTime = "",AttnDate=request.Input.Date.Value,Attendance="",IsShiftAssigned=(e.S31!="" &&e.S31!="O"),
                                });

                        });
                        break;

                    default: break;
                }
                totalAttendanceReport.Where(e => string.IsNullOrEmpty(e.SkillsetCode)).ToList().ForEach(t=> {
                    t.ShiftCode = totalAttendanceReport.FirstOrDefault(a=>a.Id==t.RefIdForAlt).SkillsetCode;
                });


                totalAttendanceReport = totalAttendanceReport.Where(e => e.SkillsetCode == request.Input.SkillsetCode &&(e.Attendance=="P"||e.Attendance=="OT"||e.Attendance=="")).ToList();
                totalRoastersList = totalRoastersList.Where(e => e.SkillsetCode == request.Input.SkillsetCode).ToList();

                if (!string.IsNullOrEmpty(request.Input.ProjectCode))
                {
                    totalAttendanceReport = totalAttendanceReport.Where(e => e.ProjectCode == request.Input.ProjectCode).ToList();
                    totalRoastersList = totalRoastersList.Where(e => e.ProjectCode == request.Input.ProjectCode).ToList();
                    UniqRows = UniqRows.Where(e => e.ProjectCode == request.Input.ProjectCode).ToList();
                }
                if (!string.IsNullOrEmpty(request.Input.SiteCode))
                {
                    totalAttendanceReport = totalAttendanceReport.Where(e => e.SiteCode == request.Input.SiteCode).ToList();
                    totalRoastersList = totalRoastersList.Where(e => e.SiteCode == request.Input.SiteCode).ToList();
                    UniqRows = UniqRows.Where(e => e.SiteCode == request.Input.SiteCode).ToList();

                }



               


                totalAttendanceReport.ForEach(e => {
                    e.EmployeeName = isArabic ? employees.FirstOrDefault(emp => emp.EmployeeNumber == e.EmployeeNumber).EmployeeName_AR : employees.FirstOrDefault(emp => emp.EmployeeNumber == e.EmployeeNumber).EmployeeName;
                    e.ProjectName = isArabic ? projects.FirstOrDefault(p => p.ProjectCode == e.ProjectCode).ProjectNameArb : projects.FirstOrDefault(p => p.ProjectCode == e.ProjectCode).ProjectNameEng;
                    e.SiteName = isArabic ? sites.FirstOrDefault(p => p.SiteCode == e.SiteCode).SiteArbName : sites.FirstOrDefault(p => p.SiteCode == e.SiteCode).SiteName;
                });

                totalAttendanceReport.Select(e => e.EmployeeNumber).Distinct().ToList().ForEach(e => {
                    dashboardData.EmployeesSelectionList.Add(new()
                    {
                        Value = e,
                        Text = isArabic ? employees.FirstOrDefault(emp => emp.EmployeeNumber == e).EmployeeName_AR : employees.FirstOrDefault(emp => emp.EmployeeNumber == e).EmployeeName,
                        TextTwo = e + "-" + (isArabic ? employees.FirstOrDefault(emp => emp.EmployeeNumber == e).EmployeeName_AR : employees.FirstOrDefault(emp => emp.EmployeeNumber == e).EmployeeName)
                    });
                });


                if (!string.IsNullOrEmpty(request.Input.EmployeeNumber))
                {
                    totalAttendanceReport = totalAttendanceReport.Where(e=>e.EmployeeNumber==request.Input.EmployeeNumber).ToList();
                }
                if (!string.IsNullOrEmpty(request.Input.SortingOrder))
                {
                    string orderby = request.Input.SortingOrder;

                    totalAttendanceReport = orderby switch
                    {
                        "projectCode asc" => totalAttendanceReport.OrderBy(e => e.ProjectCode).ToList(),
                        "projectCode desc" => totalAttendanceReport.OrderByDescending(e => e.ProjectCode).ToList(),
                        "projectName asc" => totalAttendanceReport.OrderBy(e => e.ProjectName).ToList(),
                        "projectName desc" => totalAttendanceReport.OrderByDescending(e => e.ProjectName).ToList(),
                        "siteCode asc" => totalAttendanceReport.OrderBy(e => e.SiteCode).ToList(),
                        "siteCode desc" => totalAttendanceReport.OrderByDescending(e => e.SiteCode).ToList(),
                        "siteName asc" => totalAttendanceReport.OrderBy(e => e.SiteName).ToList(),
                        "siteName desc" => totalAttendanceReport.OrderByDescending(e => e.SiteName).ToList(),
                        "employeeName asc" => totalAttendanceReport.OrderBy(e => e.EmployeeName).ToList(),
                        "employeeName desc" => totalAttendanceReport.OrderByDescending(e => e.EmployeeName).ToList(),
                        "employeeNumber asc" => totalAttendanceReport.OrderBy(e => e.EmployeeNumber).ToList(),
                        "employeeNumber desc" => totalAttendanceReport.OrderByDescending(e => e.EmployeeNumber).ToList(),
                        "geofenseOutCount asc" => totalAttendanceReport.OrderBy(e => e.GeofenseOutCount).ToList(),
                        "geofenseOutCount desc" => totalAttendanceReport.OrderByDescending(e => e.GeofenseOutCount).ToList(),
                         "inTime asc" => totalAttendanceReport.OrderBy(e => e.InTime).ToList(),
                        "inTime desc" => totalAttendanceReport.OrderByDescending(e => e.InTime).ToList(),
                        "outTime asc" => totalAttendanceReport.OrderBy(e => e.OutTime).ToList(),
                        "outTime desc" => totalAttendanceReport.OrderByDescending(e => e.OutTime).ToList(),
                        "isGeofenseOut asc" => totalAttendanceReport.OrderBy(e => e.IsGeofenseOut).ToList(),
                        "isGeofenseOut desc" => totalAttendanceReport.OrderByDescending(e => e.IsGeofenseOut).ToList(),
                        "isOnBreak asc" => totalAttendanceReport.OrderBy(e => e.IsOnBreak).ToList(),
                        "isOnBreak desc" => totalAttendanceReport.OrderByDescending(e => e.IsOnBreak).ToList(),
                       "overtime asc" => totalAttendanceReport.OrderBy(e => e.Attendance).ToList(),
                        "overtime desc" => totalAttendanceReport.OrderByDescending(e => e.Attendance).ToList(),
                        "late asc" => totalAttendanceReport.OrderBy(e => e.IsLate).ToList(),
                        "late desc" => totalAttendanceReport.OrderByDescending(e => e.IsLate).ToList(),
                         "shiftCode asc" => totalAttendanceReport.OrderBy(e => e.ShiftCode).ToList(),
                        "shiftCode desc" => totalAttendanceReport.OrderByDescending(e => e.ShiftCode).ToList(),

                        _ => totalAttendanceReport
                    };
                }
                dashboardData.TotalEmpCount = totalRoastersList.Count;
                dashboardData.NotReportedEmpCount = totalAttendanceReport.Count(e => !e.IsReported && e.IsShiftAssigned && !e.IsOnLeave);
                dashboardData.ShiftsNotAssignedCount = totalAttendanceReport.Count(e => !e.IsShiftAssigned);
                dashboardData.ReportedEmpCount = totalAttendanceReport.Count(e => e.IsReported && e.Attendance == "P" || e.Attendance == "OT");
                dashboardData.LeavesCount = totalAttendanceReport.Count(e => e.IsOnLeave && e.IsShiftAssigned);
                totalAttendanceReport = totalAttendanceReport.Where(e => e.Attendance != "S" && e.Attendance != "O").ToList();

                dashboardData.LateArrivalsCount = totalAttendanceReport.Count(e => e.IsLate.Value );


                if (request.Input.FilterOptions.Any(e=>e.IsSelected))
                {
                    request.Input.FilterOptions.Where(e => e.IsSelected).ToList().ForEach(e =>
                    {
                        totalAttendanceReport = e.Key.Trim() switch
                        {
                            "late" => totalAttendanceReport.Where(e=>e.IsLate.Value).ToList(),
                            "arrive" => totalAttendanceReport.Where(e=>e.IsLate.Value!=true).ToList(),
                            "break" => totalAttendanceReport.Where(e=>e.IsOnBreak.Value).ToList(),
                            "out Of geofence" => totalAttendanceReport.Where(e=>e.IsGeofenseOut.Value).ToList(),
                            "out of geofence count>0" => totalAttendanceReport.Where(e=>e.GeofenseOutCount>0).ToList(),
                            "overtime" => totalAttendanceReport.Where(e=>e.Attendance=="OT").ToList(),
                            "logged out" => totalAttendanceReport.Where(e=>e.IsLogoutFromShift.Value).ToList(),
                            "on duty" => totalAttendanceReport.Where(e=>!e.IsLogoutFromShift.Value && e.IsReported).ToList(),
                            "not reported" => totalAttendanceReport.Where(e=>!e.IsReported && e.IsShiftAssigned && !e.IsOnLeave).ToList(),
                            "reported" => totalAttendanceReport.Where(e=>e.IsReported).ToList(),
                            "shift not assigned" => totalAttendanceReport.Where(e=>!e.IsShiftAssigned).ToList(),
                            "employee on leave" => totalAttendanceReport.Where(e=>e.IsOnLeave && e.IsShiftAssigned).ToList(),
                            _ => totalAttendanceReport.ToList()

                        };

                    });

                }
                
                

               
                
              
                dashboardData.TotalItemsCount = totalAttendanceReport.Count;
                dashboardData.EmployeeAttendance = totalAttendanceReport.Skip(request.Input.PageNumber*request.Input.PageSize).Take(request.Input.PageSize).ToList();
                
                return dashboardData;
            }
            catch(Exception e)
            {
                return dashboardData;
            }
        }
    }
    #endregion

    #region GetOpeartionsManagementDashboardByFilter
    public class GetOpeartionsManagementDashboardByFilter : IRequest<OperationsManagementDashboardOpDto>
    {
        public UserIdentityDto User { get; set; }
        public OperationsDashboardIpDto Input { get; set; }

    }

    public class GetOpeartionsManagementDashboardByFilterHandler : IRequestHandler<GetOpeartionsManagementDashboardByFilter, OperationsManagementDashboardOpDto>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public GetOpeartionsManagementDashboardByFilterHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<OperationsManagementDashboardOpDto> Handle(GetOpeartionsManagementDashboardByFilter request, CancellationToken cancellationToken)
        {
            OperationsManagementDashboardOpDto dashboardData = new OperationsManagementDashboardOpDto();

            try
            {
                bool isArabic = request.User.Culture.IsArab();
                request.Input.Date = request.Input.Date is null ? Convert.ToDateTime(DateTime.Now, CultureInfo.InvariantCulture) : Convert.ToDateTime(request.Input.Date.Value, CultureInfo.InvariantCulture);

                //request.Input.BranchCode = string.IsNullOrEmpty(request.Input.BranchCode) ? request.User.BranchCode : request.Input.BranchCode;

                var ProjectSitesUnderBranch = _context.TblOpProjectSites.Where(e => string.IsNullOrEmpty(request.Input.BranchCode) || e.BranchCode == request.Input.BranchCode).Select(ps => new { ps.ProjectCode, ps.SiteCode, ps.ProjectNameEng, ps.ProjectNameArb }).ToList();

                if (ProjectSitesUnderBranch.Count == 0)
                {
                    return dashboardData;
                }
                var employees = await _contextDMC.HRM_TRAN_Employees.AsNoTracking().ToListAsync();
                var projects = await _context.OP_HRM_TEMP_Projects.AsNoTracking().ToListAsync();//.Where(e => UniqRows.Select(s => s.ProjectCode).Contains(e.ProjectCode)).AsNoTracking().ToListAsync();
                var sites = await _context.OprSites.AsNoTracking().ToListAsync();//.Where(e => UniqRows.Select(s => s.ProjectCode).Contains(e.SiteCode)).AsNoTracking().ToListAsync();
                var shifts = await _contextDMC.HRM_DEF_EmployeeShiftMasters.AsNoTracking().ToListAsync();




                ProjectSitesUnderBranch.Select(e => e.ProjectCode).Distinct().ToList().ForEach(e => {
                    dashboardData.ProjectsSelectionList.Add(new()
                    {
                        Value = e,
                        Text = isArabic ? projects.FirstOrDefault(p => p.ProjectCode == e).ProjectNameArb : projects.FirstOrDefault(p => p.ProjectCode == e).ProjectNameEng,
                        TextTwo = e + "-" + (isArabic ? projects.FirstOrDefault(p => p.ProjectCode == e).ProjectNameArb : projects.FirstOrDefault(p => p.ProjectCode == e).ProjectNameEng),
                    });
                });
                ProjectSitesUnderBranch.Select(e => e.SiteCode).Distinct().ToList().ForEach(e => {
                    dashboardData.SitesSelectionList.Add(new()
                    {
                        Value = e,
                        Text = isArabic ? sites.FirstOrDefault(p => p.SiteCode == e).SiteArbName : sites.FirstOrDefault(p => p.SiteCode == e).SiteName,
                        TextTwo = e + "-" + (isArabic ? sites.FirstOrDefault(p => p.SiteCode == e).SiteArbName : sites.FirstOrDefault(p => p.SiteCode == e).SiteName),
                    });
                });

                var UniqProjectSites = ProjectSitesUnderBranch.Select(e => new { e.ProjectCode, e.SiteCode }).Distinct().ToList();

                UniqProjectSites.Select(e => e.ProjectCode).Distinct().ToList().ForEach(e => {
                    dashboardData.ProjectsSelectionList.Add(new()
                    {
                        Value = e,
                        Text = isArabic ? projects.FirstOrDefault(p => p.ProjectCode == e).ProjectNameArb : projects.FirstOrDefault(p => p.ProjectCode == e).ProjectNameEng,
                        TextTwo = e + "-" + (isArabic ? projects.FirstOrDefault(p => p.ProjectCode == e).ProjectNameArb : projects.FirstOrDefault(p => p.ProjectCode == e).ProjectNameEng),
                    });
                });
                UniqProjectSites.Select(e => e.SiteCode).Distinct().ToList().ForEach(e => {
                    dashboardData.SitesSelectionList.Add(new()
                    {
                        Value = e,
                        Text = isArabic ? sites.FirstOrDefault(p => p.SiteCode == e).SiteArbName : sites.FirstOrDefault(p => p.SiteCode == e).SiteName,
                        TextTwo = e + "-" + (isArabic ? sites.FirstOrDefault(p => p.SiteCode == e).SiteArbName : sites.FirstOrDefault(p => p.SiteCode == e).SiteName),
                    });
                });




                var totalRoastersList = await _context.TblOpMonthlyRoasterForSites.Where(e => e.Month == request.Input.Date.Value.Month && e.Year == request.Input.Date.Value.Year && UniqProjectSites.Select(s => s.ProjectCode).Contains(e.ProjectCode) && UniqProjectSites.Select(s => s.SiteCode).Contains(e.SiteCode) && e.IsPrimaryResource).ToListAsync(); 

                if (totalRoastersList.Count == 0)
                {
                    return dashboardData;
                }
                var totalAttendanceReport = await _context.EmployeeAttendance.ProjectTo<DashboardEmployeeAttendanceDto>(_mapper.ConfigurationProvider).Where(e => (e.AttnDate == request.Input.Date) && (e.IsLate.HasValue) && UniqProjectSites.Select(s => s.ProjectCode).Contains(e.ProjectCode) && UniqProjectSites.Select(s => s.SiteCode).Contains(e.SiteCode) && (e.SkillsetCode == request.Input.SkillsetCode || string.IsNullOrEmpty(e.SkillsetCode))).ToListAsync();
                var totalLeavesList = await _context.EmployeeLeaves.Where(e => e.AttnDate == request.Input.Date).ToListAsync();

                switch (request.Input.Date.Value.Day)
                {
                    case 1:
                        totalRoastersList = totalRoastersList.Where(e => e.S1 == "" || e.S1 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S1 == e.ShiftCode).SkillsetCode;
                        });



                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S1 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S1,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S1 != "" && e.S1 != "O"),

                                });

                        });
                        break;
                    case 2:
                        totalRoastersList = totalRoastersList.Where(e => e.S2 == "" || e.S2 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S2 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S2 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S2,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S2 != "" && e.S2 != "O"),
                                });

                        });
                        break;

                    case 3:
                        totalRoastersList = totalRoastersList.Where(e => e.S3 == "" || e.S3 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S3 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S3 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S3,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S3 != "" && e.S3 != "O"),
                                });

                        });
                        break;
                    case 4:
                        totalRoastersList = totalRoastersList.Where(e => e.S4 == "" || e.S4 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S4 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S4 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S4,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S4 != "" && e.S4 != "O"),
                                });

                        });
                        break;
                    case 5:
                        totalRoastersList = totalRoastersList.Where(e => e.S5 == "" || e.S5 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S5 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S5 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S5,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S5 != "" && e.S5 != "O"),
                                });

                        });
                        break;

                    case 6:
                        totalRoastersList = totalRoastersList.Where(e => e.S6 == "" || e.S6 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S6 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S6 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S6,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S6 != "" && e.S6 != "O"),
                                });

                        });
                        break;

                    case 7:
                        totalRoastersList = totalRoastersList.Where(e => e.S7 == "" || e.S7 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S7 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S7 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S7,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S7 != "" && e.S7 != "O"),
                                });

                        });
                        break;

                    case 8:
                        totalRoastersList = totalRoastersList.Where(e => e.S8 == "" || e.S8 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S8 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S8 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S8,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S8 != "" && e.S8 != "O"),
                                });

                        });
                        break;

                    case 9:
                        totalRoastersList = totalRoastersList.Where(e => e.S9 == "" || e.S9 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S9 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S9 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S9,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S9 != "" && e.S9 != "O"),
                                });

                        });
                        break;

                    case 10:
                        totalRoastersList = totalRoastersList.Where(e => e.S10 == "" || e.S10 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S10 == e.ShiftCode).SkillsetCode;
                        });

                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S10 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S10,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S10 != "" && e.S10 != "O"),
                                });

                        });
                        break;

                    case 11:
                        totalRoastersList = totalRoastersList.Where(e => e.S11 == "" || e.S11 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S11 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S11 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S11,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S11 != "" && e.S11 != "O"),
                                });

                        });
                        break;

                    case 12:
                        totalRoastersList = totalRoastersList.Where(e => e.S12 == "" || e.S12 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S12 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S12 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S12,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S12 != "" && e.S12 != "O"),
                                });

                        });
                        break;

                    case 13:
                        totalRoastersList = totalRoastersList.Where(e => e.S13 == "" || e.S13 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S13 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S13 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S13,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S13 != "" && e.S13 != "O"),
                                });

                        });
                        break;

                    case 14:
                        totalRoastersList = totalRoastersList.Where(e => e.S14 == "" || e.S14 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S14 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S14 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S14,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S14 != "" && e.S14 != "O"),
                                });

                        });
                        break;
                    case 15:
                        totalRoastersList = totalRoastersList.Where(e => e.S15 == "" || e.S15 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S15 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S15 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S15,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S15 != "" && e.S15 != "O"),
                                });

                        });
                        break;
                    case 16:
                        totalRoastersList = totalRoastersList.Where(e => e.S16 == "" || e.S16 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S16 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S16 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S16,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S16 != "" && e.S16 != "O"),
                                });

                        });
                        break;
                    case 17:
                        totalRoastersList = totalRoastersList.Where(e => e.S17 == "" || e.S17 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S17 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S17 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S17,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S17 != "" && e.S17 != "O"),
                                });

                        });
                        break;
                    case 18:
                        totalRoastersList = totalRoastersList.Where(e => e.S18 == "" || e.S18 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S18 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S18 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S18,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S18 != "" && e.S18 != "O"),
                                });

                        });
                        break;
                    case 19:
                        totalRoastersList = totalRoastersList.Where(e => e.S19 == "" || e.S19 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S19 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S19 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S19,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S19 != "" && e.S19 != "O"),
                                });

                        });
                        break;
                    case 20:
                        totalRoastersList = totalRoastersList.Where(e => e.S20 == "" || e.S20 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S20 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S20 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S20,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S20 != "" && e.S20 != "O"),
                                });

                        });
                        break;
                    case 21:
                        totalRoastersList = totalRoastersList.Where(e => e.S21 == "" || e.S21 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S21 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S21 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S21,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S21 != "" && e.S21 != "O"),
                                });

                        });
                        break;
                    case 22:
                        totalRoastersList = totalRoastersList.Where(e => e.S22 == "" || e.S22 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S22 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S22 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S22,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S22 != "" && e.S22 != "O"),
                                });

                        });
                        break;
                    case 23:
                        totalRoastersList = totalRoastersList.Where(e => e.S23 == "" || e.S23 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S23 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S23 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S23,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S23 != "" && e.S23 != "O"),
                                });

                        });
                        break;
                    case 24:
                        totalRoastersList = totalRoastersList.Where(e => e.S24 == "" || e.S24 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S24 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S24 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S24,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S24 != "" && e.S24 != "O"),
                                });

                        });
                        break;
                    case 25:
                        totalRoastersList = totalRoastersList.Where(e => e.S25 == "" || e.S25 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S25 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S25 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S25,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S25 != "" && e.S25 != "O"),
                                });

                        });
                        break;
                    case 26:
                        totalRoastersList = totalRoastersList.Where(e => e.S26 == "" || e.S26 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S26 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S26 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S26,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S26 != "" && e.S26 != "O"),
                                });

                        });
                        break;
                    case 27:
                        totalRoastersList = totalRoastersList.Where(e => e.S27 == "" || e.S27 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S27 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S27 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S27,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S27 != "" && e.S27 != "O"),
                                });

                        });
                        break;
                    case 28:
                        totalRoastersList = totalRoastersList.Where(e => e.S28 == "" || e.S28 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S28 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S28 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S28,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S28 != "" && e.S28 != "O"),
                                });

                        });
                        break;
                    case 29:
                        totalRoastersList = totalRoastersList.Where(e => e.S29 == "" || e.S29 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S29 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S29 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S29,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S29 != "" && e.S29 != "O"),
                                });

                        });
                        break;
                    case 30:
                        totalRoastersList = totalRoastersList.Where(e => e.S30 == "" || e.S30 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S30 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S30 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S30,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S30 != "" && e.S30 != "O"),
                                });

                        });
                        break;
                    case 31:
                        totalRoastersList = totalRoastersList.Where(e => e.S31 == "" || e.S31 != "x").ToList();
                        totalAttendanceReport.Where(t => string.IsNullOrEmpty(t.SkillsetCode) && t.RefIdForAlt == 0).ToList().ForEach(e => {
                            e.SkillsetCode = totalRoastersList.FirstOrDefault(r => r.ProjectCode == e.ProjectCode && r.SiteCode == e.SiteCode && r.EmployeeNumber == e.EmployeeNumber && r.S31 == e.ShiftCode).SkillsetCode;
                        });
                        totalRoastersList.ForEach(e => {
                            if (!(totalAttendanceReport.Any(t => t.ProjectCode == e.ProjectCode && t.SiteCode == e.SiteCode && e.EmployeeNumber == t.EmployeeNumber && e.S31 == t.ShiftCode)))
                                totalAttendanceReport.Add(new()
                                {
                                    ProjectCode = e.ProjectCode,
                                    SiteCode = e.SiteCode,
                                    EmployeeNumber = e.EmployeeNumber,
                                    ShiftCode = e.S31,
                                    IsReported = false,
                                    IsOnBreak = false,
                                    IsLate = false,
                                    GeofenseOutCount = 0,
                                    IsGeofenseOut = false,
                                    IsLogoutFromShift = false,
                                    InTime = "",
                                    SkillsetCode = e.SkillsetCode,
                                    IsOnLeave = totalLeavesList.Any(l => l.EmployeeNumber == e.EmployeeNumber),
                                    OutTime = "",
                                    AttnDate = request.Input.Date.Value,
                                    Attendance = "",
                                    IsShiftAssigned = (e.S31 != "" && e.S31 != "O"),
                                });

                        });
                        break;

                    default: break;
                }
                totalAttendanceReport.Where(e => string.IsNullOrEmpty(e.SkillsetCode)).ToList().ForEach(t => {
                    t.ShiftCode = totalAttendanceReport.FirstOrDefault(a => a.Id == t.RefIdForAlt).SkillsetCode;
                });


                totalAttendanceReport = totalAttendanceReport.Where(e => e.SkillsetCode == request.Input.SkillsetCode && (e.Attendance == "P" || e.Attendance == "OT" || e.Attendance == "")).ToList();
                totalRoastersList = totalRoastersList.Where(e => e.SkillsetCode == request.Input.SkillsetCode).ToList();

                if (!string.IsNullOrEmpty(request.Input.ProjectCode))
                {
                    totalAttendanceReport = totalAttendanceReport.Where(e => e.ProjectCode == request.Input.ProjectCode).ToList();
                    totalRoastersList = totalRoastersList.Where(e => e.ProjectCode == request.Input.ProjectCode).ToList();
                    UniqProjectSites = UniqProjectSites.Where(e => e.ProjectCode == request.Input.ProjectCode).ToList();
                }
                if (!string.IsNullOrEmpty(request.Input.SiteCode))
                {
                    totalAttendanceReport = totalAttendanceReport.Where(e => e.SiteCode == request.Input.SiteCode).ToList();
                    totalRoastersList = totalRoastersList.Where(e => e.SiteCode == request.Input.SiteCode).ToList();
                    UniqProjectSites = UniqProjectSites.Where(e => e.SiteCode == request.Input.SiteCode).ToList();

                }






                totalAttendanceReport.ForEach(e => {
                    e.EmployeeName = isArabic ? employees.FirstOrDefault(emp => emp.EmployeeNumber == e.EmployeeNumber).EmployeeName_AR : employees.FirstOrDefault(emp => emp.EmployeeNumber == e.EmployeeNumber).EmployeeName;
                    e.ProjectName = isArabic ? projects.FirstOrDefault(p => p.ProjectCode == e.ProjectCode).ProjectNameArb : projects.FirstOrDefault(p => p.ProjectCode == e.ProjectCode).ProjectNameEng;
                    e.SiteName = isArabic ? sites.FirstOrDefault(p => p.SiteCode == e.SiteCode).SiteArbName : sites.FirstOrDefault(p => p.SiteCode == e.SiteCode).SiteName;
                });
                var uniqRows = totalAttendanceReport.Select(e => new { e.ProjectCode,e.SiteCode,e.ShiftCode}).Distinct().ToList();

                uniqRows.ForEach(e => {
                    

                    dashboardData.Rows.Add(new() { 
                     ProjectCode=e.ProjectCode,
                     ProjectName= isArabic ? projects.FirstOrDefault(p => p.ProjectCode == e.ProjectCode).ProjectNameArb : projects.FirstOrDefault(p => p.ProjectCode == e.ProjectCode).ProjectNameEng,
                    SiteCode =e.SiteCode,
                    SiteName= isArabic ? sites.FirstOrDefault(p => p.SiteCode == e.SiteCode).SiteArbName : sites.FirstOrDefault(p => p.SiteCode == e.SiteCode).SiteName,
                    LeavesCount =totalLeavesList.Count(l=>l.ProjectCode==e.ProjectCode && l.SiteCode==e.SiteCode && l.ShiftCode==e.ShiftCode),
                    TotalContracted = totalAttendanceReport.Count(l=>(l.ProjectCode==e.ProjectCode && l.SiteCode==e.SiteCode)&& l.AltEmployeeNumber=="" && (l.ShiftCode==e.ShiftCode || l.AltShiftCode==e.ShiftCode)),
                    Shortage = totalAttendanceReport.Count(l => (l.ProjectCode == e.ProjectCode && l.SiteCode == e.SiteCode) && !l.IsReported && l.IsShiftAssigned && !l.IsOnLeave && l.ShiftCode==e.ShiftCode),
                    ShiftsNotAssignedCount = totalAttendanceReport.Count(l => (l.ProjectCode == e.ProjectCode && l.SiteCode == e.SiteCode) && !l.IsShiftAssigned),
                    StaffPresent = totalAttendanceReport.Count(l => (l.ProjectCode == e.ProjectCode && l.SiteCode == e.SiteCode) && (l.ProjectCode == e.ProjectCode && l.SiteCode == e.SiteCode) && (l.IsReported && l.Attendance == "P" || l.Attendance == "OT")&& l.ShiftCode==e.ShiftCode),
                    LateStaff = totalAttendanceReport.Count(l => (l.ProjectCode == e.ProjectCode && l.SiteCode == e.SiteCode) && l.IsLate.Value && l.ShiftCode==e.ShiftCode),

                    SupportGaurds=0,
                    ShiftCode=e.ShiftCode,
                    ShiftInTime=shifts.FirstOrDefault(s=>s.ShiftCode==e.ShiftCode)?.InTime.Value.ToString(),
                    ShiftOutTime=shifts.FirstOrDefault(s=>s.ShiftCode==e.ShiftCode)?.OutTime.Value.ToString(),
                    ProjectStatus = totalAttendanceReport.Count(l => !l.IsReported && l.IsShiftAssigned && !l.IsOnLeave && l.ShiftCode == e.ShiftCode)>0,
                    });
                
                });



                if (!string.IsNullOrEmpty(request.Input.SortingOrder))
                {
                    string orderby = request.Input.SortingOrder;

                    dashboardData.Rows = orderby switch
                    {
                        "projectCode asc" => dashboardData.Rows.OrderBy(e => e.ProjectCode).ToList(),
                        "projectCode desc" => dashboardData.Rows.OrderByDescending(e => e.ProjectCode).ToList(),
                        "projectName asc" => dashboardData.Rows.OrderBy(e => e.ProjectName).ToList(),
                        "projectName desc" => dashboardData.Rows.OrderByDescending(e => e.ProjectName).ToList(),
                        "siteCode asc" => dashboardData.Rows.OrderBy(e => e.SiteCode).ToList(),
                        "siteCode desc" => dashboardData.Rows.OrderByDescending(e => e.SiteCode).ToList(),
                        "siteName asc" => dashboardData.Rows.OrderBy(e => e.SiteName).ToList(),
                        "siteName desc" => dashboardData.Rows.OrderByDescending(e => e.SiteName).ToList(),
                        "shiftCode asc" => dashboardData.Rows.OrderBy(e => e.ShiftCode).ToList(),
                        "shiftCode desc" => dashboardData.Rows.OrderByDescending(e => e.ShiftCode).ToList(),

                        _ => dashboardData.Rows
                    };
                }
                dashboardData.TotalContracted = totalRoastersList.Count;
                dashboardData.Shortage = totalAttendanceReport.Count(e => !e.IsReported && e.IsShiftAssigned && !e.IsOnLeave);
                dashboardData.ShiftsNotAssignedCount = totalAttendanceReport.Count(e => !e.IsShiftAssigned);
                dashboardData.StaffPresent = totalAttendanceReport.Count(e => e.IsReported && e.Attendance == "P" || e.Attendance == "OT");
                dashboardData.LeavesCount = totalAttendanceReport.Count(e => e.IsOnLeave && e.IsShiftAssigned);
                totalAttendanceReport = totalAttendanceReport.Where(e => e.Attendance != "S" && e.Attendance != "O").ToList();

                dashboardData.LateStaff = totalAttendanceReport.Count(e => e.IsLate.Value);

                dashboardData.TotalItemsCount = dashboardData.Rows.Count;
                dashboardData.Rows = dashboardData.Rows.Skip(request.Input.PageNumber * request.Input.PageSize).Take(request.Input.PageSize).ToList();
             
                return dashboardData;
            }
            catch (Exception e)
            {
                return dashboardData;
            }
        }
    }
    #endregion
}