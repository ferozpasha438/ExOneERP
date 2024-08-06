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
    

    


    #region IsValidSwapEmployeesRequest
    public class IsValidSwapEmployeesRequest : IRequest<ValidityCheckDto>
    {
        public UserIdentityDto User { get; set; }
        public TblOpPvSwapEmployeesReqDto InputDto { get; set; }
    }

    public class IsValidSwapEmployeesRequestHandler : IRequestHandler<IsValidSwapEmployeesRequest, ValidityCheckDto>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public IsValidSwapEmployeesRequestHandler(CINDBOneContext context, DMCContext contextDMC,  IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<ValidityCheckDto> Handle(IsValidSwapEmployeesRequest request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {

              

                    try
                    {
                        Log.Info("----Info IsValidSwapEmployeesRequest method start----");

                    if (request.InputDto.Id>0&& request.InputDto.IsApproved)
                    {
                        return new() { IsValidReq = false, ErrorId = -11, ErrorMsg = "Request Already Approved" };

                    }

                    if (!await _context.OP_HRM_TEMP_Projects.AnyAsync(e=>e.ProjectCode==request.InputDto.SrcProjectCode))
                    {
                        return new() { IsValidReq = false, ErrorId = -1,ErrorMsg= "Invalid Source ProjectCode" };     
                    }
                      if (!await _context.OP_HRM_TEMP_Projects.AnyAsync(e=>e.ProjectCode==request.InputDto.DestProjectCode))
                    {
                        return new() { IsValidReq = false, ErrorId = -2, ErrorMsg = "Invalid Destination ProjectCode" };      
                    }



                     if (!await _context.OprSites.AnyAsync(e=>e.SiteCode==request.InputDto.DestSiteCode))
                    {
                        return new() { IsValidReq = false, ErrorId = -3, ErrorMsg = "Invalid Destination SiteCode" };
                    }
                       if (!await _context.OprSites.AnyAsync(e=>e.SiteCode==request.InputDto.SrcSiteCode))
                    {
                        return new() { IsValidReq = false, ErrorId = -4, ErrorMsg = "Invalid Source SIteCode" };     
                    }

                    var srcProject = await _context.TblOpProjectSites.AsNoTracking().FirstAsync(e => e.ProjectCode == request.InputDto.SrcProjectCode && e.SiteCode == request.InputDto.SrcSiteCode);
                    var destProject = await _context.TblOpProjectSites.AsNoTracking().FirstAsync(e => e.ProjectCode == request.InputDto.DestProjectCode && e.SiteCode == request.InputDto.DestSiteCode);

                    if (srcProject.StartDate> request.InputDto.FromDate ||
                        destProject.StartDate > request.InputDto.FromDate ||
                        srcProject.EndDate < request.InputDto.FromDate||
                        destProject.EndDate < request.InputDto.FromDate
                        ) 
                    {
                        return new() { IsValidReq = false, ErrorId = -5, ErrorMsg = "Incompatible From Date" };
                    }

                    if (!(await _context.TblOpMonthlyRoasterForSites.AsNoTracking().AnyAsync(e=>e.EmployeeNumber==request.InputDto.SrcEmployeeNumber&& e.ProjectCode==request.InputDto.SrcProjectCode&& e.SiteCode==request.InputDto.SrcSiteCode&&e.MonthEndDate>=request.InputDto.FromDate))) {
                        return new() { IsValidReq = false, ErrorId = -6, ErrorMsg = "Source Employee Not Found in the Project" };
                    }

                     if (!(await _context.TblOpMonthlyRoasterForSites.AsNoTracking().AnyAsync(e=> e.EmployeeNumber == request.InputDto.DestEmployeeNumber && e.ProjectCode==request.InputDto.DestProjectCode&& e.SiteCode==request.InputDto.DestSiteCode && e.MonthEndDate>=request.InputDto.FromDate))) {
                        return new() { IsValidReq = false, ErrorId = -7, ErrorMsg = "Dest Employee Not Found in the Project" };
                    }

                    if (await _context.TblOpMonthlyRoasterForSites.AsNoTracking().AnyAsync(e=>e.ProjectCode==request.InputDto.DestProjectCode&& e.SiteCode==request.InputDto.DestSiteCode&&e.MonthEndDate>=request.InputDto.FromDate &&e.IsPrimaryResource && e.EmployeeNumber == request.InputDto.SrcEmployeeNumber)){
                        return new() { IsValidReq = false, ErrorId = -8, ErrorMsg = "Source Employee Already Exist in the Dest Project" };
                    }

                     if (await _context.TblOpMonthlyRoasterForSites.AsNoTracking().AnyAsync(e=>e.ProjectCode==request.InputDto.SrcProjectCode&& e.SiteCode==request.InputDto.SrcSiteCode&&e.MonthEndDate>=request.InputDto.FromDate &&e.IsPrimaryResource && e.EmployeeNumber == request.InputDto.DestEmployeeNumber)){
                        return new() { IsValidReq = false, ErrorId = -14, ErrorMsg = "Dest Employee Already Exist in the Src Project" };
                    }

                    if (await _context.EmployeeAttendance.AnyAsync(e => e.EmployeeNumber == request.InputDto.SrcEmployeeNumber && e.ProjectCode == request.InputDto.SrcProjectCode && e.SiteCode == request.InputDto.SrcSiteCode && e.AttnDate >= request.InputDto.FromDate)) {
                        var attendanceExist = await _context.EmployeeAttendance.OrderByDescending(e=>e.AttnDate).FirstOrDefaultAsync(e => e.EmployeeNumber == request.InputDto.SrcEmployeeNumber && e.ProjectCode == request.InputDto.SrcProjectCode && e.SiteCode == request.InputDto.SrcSiteCode && e.AttnDate >= request.InputDto.FromDate);
                        return new() { IsValidReq = false, ErrorId = -9, ErrorMsg = "Source Employee Attendance Exist:"+ attendanceExist.AttnDate.ToString() };
                    
                    }
                    if (await _context.EmployeeAttendance.AnyAsync(e => e.EmployeeNumber == request.InputDto.DestEmployeeNumber && e.ProjectCode == request.InputDto.DestProjectCode && e.SiteCode == request.InputDto.DestSiteCode && e.AttnDate >= request.InputDto.FromDate))
                    {
                        var attendanceExist = await _context.EmployeeAttendance.OrderByDescending(e => e.AttnDate).FirstOrDefaultAsync(e => e.EmployeeNumber == request.InputDto.DestEmployeeNumber && e.ProjectCode == request.InputDto.DestProjectCode && e.SiteCode == request.InputDto.DestSiteCode && e.AttnDate >= request.InputDto.FromDate);
                        return new() { IsValidReq = false, ErrorId = -10, ErrorMsg = "Dest Employee Attendance Exist:" + attendanceExist.AttnDate.ToString() };
                    }



                    if (await _context.EmployeeAttendance.AnyAsync(e => e.EmployeeNumber == request.InputDto.SrcEmployeeNumber && e.ProjectCode == request.InputDto.DestProjectCode && e.SiteCode == request.InputDto.DestSiteCode && e.AttnDate >= request.InputDto.FromDate)) {
                        var attendanceExist = await _context.EmployeeAttendance.OrderByDescending(e=>e.AttnDate).FirstOrDefaultAsync(e => e.EmployeeNumber == request.InputDto.SrcEmployeeNumber && e.ProjectCode == request.InputDto.DestProjectCode && e.SiteCode == request.InputDto.DestSiteCode && e.AttnDate >= request.InputDto.FromDate);
                        return new() { IsValidReq = false, ErrorId = -12, ErrorMsg = "Source Employee Attendance Exist in Destination Site:"+ attendanceExist.AttnDate.ToString() };
                    
                    }
                          if (await _context.EmployeeAttendance.AnyAsync(e => e.EmployeeNumber == request.InputDto.DestEmployeeNumber && e.ProjectCode == request.InputDto.SrcProjectCode && e.SiteCode == request.InputDto.SrcSiteCode && e.AttnDate >= request.InputDto.FromDate)) {
                        var attendanceExist = await _context.EmployeeAttendance.OrderByDescending(e=>e.AttnDate).FirstOrDefaultAsync(e => e.EmployeeNumber == request.InputDto.DestEmployeeNumber && e.ProjectCode == request.InputDto.SrcProjectCode && e.SiteCode == request.InputDto.SrcSiteCode && e.AttnDate >= request.InputDto.FromDate);
                        return new() { IsValidReq = false, ErrorId = -15, ErrorMsg = "Dest Employee Attendance Exist in Src Site:"+ attendanceExist.AttnDate.ToString() };
                    
                    }

                   
                        bool isSrcEmployeeTransferLogExist = await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AnyAsync(e => e.EmployeeNumber == request.InputDto.SrcEmployeeNumber
                        && e.TransferredDate <= request.InputDto.FromDate);

                    if (!isSrcEmployeeTransferLogExist)
                    { 
                        return new() { IsValidReq = false, ErrorId = -13, ErrorMsg = "Primary site log not exist for Employee:"+ request.InputDto.SrcEmployeeNumber };

                    }


                         bool isDestEmployeeTransferLogExist = await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AnyAsync(e => e.EmployeeNumber == request.InputDto.DestEmployeeNumber
                         && e.TransferredDate <= request.InputDto.FromDate);

                    if (!isDestEmployeeTransferLogExist)
                    { 
                        return new() { IsValidReq = false, ErrorId = -16, ErrorMsg = "Primary site log not exist for Employee:"+ request.InputDto.DestEmployeeNumber };

                    }




                    return new() { IsValidReq=true };


                    //latest count=-16

                    }
                    catch (Exception ex)
                    {

                        Log.Error("Error in IsValidSwapEmployeesRequestHandler Method");
                        Log.Error("Error occured time : " + DateTime.UtcNow);
                        Log.Error("Error message : " + ex.Message);
                        Log.Error("Error StackTrace : " + ex.StackTrace);
                        transaction.Rollback();
                        return new() { IsValidReq = false, ErrorId =0, ErrorMsg = "Some thing went wrong" };
                    }
                }
            
        }
    }

    #endregion






    #region CreateUpdatePvSwapEmployeesReq
    public class CreatePvSwapEmployeesReq : IRequest<CreateUpadteResultDto>
    {
        public UserIdentityDto User { get; set; }
        public TblOpPvSwapEmployeesReqDto InputDto { get; set; }
    }

    public class CreatePvSwapEmployeesReqHandler : IRequestHandler<CreatePvSwapEmployeesReq, CreateUpadteResultDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreatePvSwapEmployeesReqHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreateUpadteResultDto> Handle(CreatePvSwapEmployeesReq request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdatePvSwapEmployeesReq method start----");

                    var obj = request.InputDto;
                    TblOpPvSwapEmployeesReq ReqHead = new();
                    if (obj.Id > 0)

                        ReqHead = await _context.TblOpPvSwapEmployeesReqs.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);

                    ReqHead.IsActive = obj.IsActive;
                    ReqHead.SrcCustomerCode = obj.SrcCustomerCode;
                    ReqHead.SrcProjectCode = obj.SrcProjectCode;
                    ReqHead.SrcSiteCode = obj.SrcSiteCode;
                    ReqHead.DestCustomerCode = obj.DestCustomerCode;
                    ReqHead.DestProjectCode = obj.DestProjectCode;
                    ReqHead.DestSiteCode = obj.DestSiteCode;
                    ReqHead.SrcEmployeeNumber = obj.SrcEmployeeNumber;
                    ReqHead.DestEmployeeNumber = obj.DestEmployeeNumber;
                    ReqHead.FromDate = obj.FromDate;
                    ReqHead.IsApproved = false;
                    ReqHead.ApprovedBy = 0;
                    ReqHead.IsApproved = false;

                    ReqHead.IsMerged = false;
                    ReqHead.IsActive = true;
                 //   ReqHead.ApprovedBy = 0;



                    if (obj.Id > 0)
                    {
                        ReqHead.Modified = DateTime.Now;
                        ReqHead.ModifiedBy = request.User.UserId;
                        _context.TblOpPvSwapEmployeesReqs.Update(ReqHead);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ReqHead.CreatedBy = request.User.UserId;

                        ReqHead.Created = DateTime.Now;
                        await _context.TblOpPvSwapEmployeesReqs.AddAsync(ReqHead);
                        await _context.SaveChangesAsync();
                    }

                    Log.Info("----Info CreateUpdatePvSwapEmployeesReq method Exit----");

                    await transaction.CommitAsync();
                    return new() {IsSuccess=true ,};

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdatePvSwapEmployeesReq Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return new() { IsSuccess=false,ErrorMsg=ApiMessageInfo.Failed,ErrorId=0};

                }
            }
        }

    }

    #endregion






    #region GetPvSwapEmployeesReqsPagedList

    public class GetPvSwapEmployeesReqsPagedList : IRequest<PaginatedList<TblOpPvSwapEmployeesReqsPaginationDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetPvSwapEmployeesReqsPagedListHandler : IRequestHandler<GetPvSwapEmployeesReqsPagedList, PaginatedList<TblOpPvSwapEmployeesReqsPaginationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPvSwapEmployeesReqsPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblOpPvSwapEmployeesReqsPaginationDto>> Handle(GetPvSwapEmployeesReqsPagedList request, CancellationToken cancellationToken)
        {
            bool isAdmin = _context.SystemLogins.AsNoTracking().FirstOrDefault(e => e.Id == request.User.UserId).LoginId == "Admin" ? true : false;

            var oprAuths = _context.TblOpAuthoritiesList.AsNoTracking();
            var Projects = _context.OP_HRM_TEMP_Projects.AsNoTracking();

            var Sites = _context.OprSites.AsNoTracking();
            var search = request.Input.Query;
            var list = await _context.TblOpPvSwapEmployeesReqs.AsNoTracking()
               .OrderBy(request.Input.OrderBy).Select(d => new TblOpPvSwapEmployeesReqsPaginationDto
               {
                   Id = d.Id,
                   SrcCustomerCode = d.SrcCustomerCode,
                   DestCustomerCode = d.DestCustomerCode,
                   SrcSiteCode = d.SrcSiteCode,
                   DestSiteCode = d.DestSiteCode,
                   SrcSiteName = Sites.FirstOrDefault(s => s.SiteCode == d.SrcSiteCode).SiteName,
                   DestSiteName = Sites.FirstOrDefault(s => s.SiteCode == d.DestSiteCode).SiteName,
                   SrcSiteNameAr = Sites.FirstOrDefault(s => s.SiteCode == d.SrcSiteCode).SiteArbName,
                   DestSiteNameAr = Sites.FirstOrDefault(s => s.SiteCode == d.DestSiteCode).SiteArbName,
                   SrcProjectCode = d.SrcProjectCode,
                   DestProjectCode = d.DestProjectCode,
                   SrcProjectName = Projects.FirstOrDefault(p => p.ProjectCode == d.SrcProjectCode).ProjectNameEng,
                   DestProjectName = Projects.FirstOrDefault(p => p.ProjectCode == d.DestProjectCode).ProjectNameEng,
                   SrcProjectNameAr = Projects.FirstOrDefault(p => p.ProjectCode == d.SrcProjectCode).ProjectNameArb,
                   DestProjectNameAr = Projects.FirstOrDefault(p => p.ProjectCode == d.DestProjectCode).ProjectNameArb,
                   SrcEmployeeNumber = d.SrcEmployeeNumber,
                   DestEmployeeNumber = d.DestEmployeeNumber,
                   FromDate = d.FromDate,
                   IsActive = d.IsActive,
                   IsApproved = d.IsApproved,
                   CanEditReq = request.User.UserId == d.CreatedBy,
                   CanApproveReq = oprAuths.Any(a => a.AppAuth == request.User.UserId && a.CanApprovePvReq && a.BranchCode == Sites.FirstOrDefault(s => s.SiteCode == d.DestSiteCode).SiteCityCode) || isAdmin
                   ,IsAdmin=isAdmin,
                     ApprovedBy=d.ApprovedBy,
               })
              .Where(e =>
                            (e.SrcCustomerCode.Contains(search) ||
                            e.DestCustomerCode.Contains(search) ||
                            e.SrcSiteCode.Contains(search) ||
                            e.DestSiteCode.Contains(search) ||
                            e.SrcSiteName.Contains(search) ||
                            e.DestSiteName.Contains(search) ||
                            e.SrcSiteNameAr.Contains(search) ||
                            e.DestSiteNameAr.Contains(search) ||
                            e.SrcProjectCode.Contains(search) ||
                            e.DestProjectCode.Contains(search) ||
                            e.SrcProjectName.Contains(search) ||
                            e.DestProjectName.Contains(search) ||
                            e.SrcProjectNameAr.Contains(search) ||
                            e.DestProjectNameAr.Contains(search) ||
                            e.SrcEmployeeNumber.Contains(search) ||
                            e.DestEmployeeNumber.Contains(search) ||
                            search == "" || search == null
                             ) && (e.CanApproveReq || e.CanEditReq))

               .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion




    #region GetPvSwapEmployeesReqById
    public class GetPvSwapEmployeesReqById : IRequest<TblOpPvSwapEmployeesReqDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetPvSwapEmployeesReqByIdHandler : IRequestHandler<GetPvSwapEmployeesReqById, TblOpPvSwapEmployeesReqDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPvSwapEmployeesReqByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpPvSwapEmployeesReqDto> Handle(GetPvSwapEmployeesReqById request, CancellationToken cancellationToken)
        {
            try
            {

                var PvSwapEmployeesReq = await _context.TblOpPvSwapEmployeesReqs.AsNoTracking().ProjectTo<TblOpPvSwapEmployeesReqDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);


                return PvSwapEmployeesReq;





            }
            catch (Exception ex)
            {
                Log.Error("Error in GetPvSwapEmployeesReqByIdHandler Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion


    #region DeletePvSwapEmployeesReqById
    public class DeletePvSwapEmployeesReqById : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeletePvSwapEmployeesReqByIdHandler : IRequestHandler<DeletePvSwapEmployeesReqById, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public DeletePvSwapEmployeesReqByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<long> Handle(DeletePvSwapEmployeesReqById request, CancellationToken cancellationToken)
        {
            try
            {

                var PvSwapEmployeesReq = await _context.TblOpPvSwapEmployeesReqs.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
                _context.TblOpPvSwapEmployeesReqs.Remove(PvSwapEmployeesReq);

                _context.SaveChanges();
                return request.Id;

            }
            catch (Exception ex)
            {

                Log.Error("Error in DeletePvSwapEmployeesReqByIdHandler Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }
    #endregion

    #region ApproveReqPvSwapEmployeesReqById
    public class ApproveReqPvSwapEmployeesReqById : IRequest<CreateUpadteResultDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class ApproveReqPvSwapEmployeesReqByIdHandler : IRequestHandler<ApproveReqPvSwapEmployeesReqById, CreateUpadteResultDto>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public ApproveReqPvSwapEmployeesReqByIdHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<CreateUpadteResultDto> Handle(ApproveReqPvSwapEmployeesReqById request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                using (var transaction2 = await _contextDMC.Database.BeginTransactionAsync())
                {
                    try
                    {
                        Log.Info("----Info ApproveReqPvSwapEmployeesReqById method start----");

                        var Request = await _context.TblOpPvSwapEmployeesReqs.FirstOrDefaultAsync(e => e.Id == request.Id);


                   
                        var DestEmpRoasters = await _context.TblOpMonthlyRoasterForSites.OrderBy(e => e.MonthEndDate).AsNoTracking().Where(e => e.EmployeeNumber == Request.DestEmployeeNumber
                            && e.ProjectCode == Request.DestProjectCode
                            && e.SiteCode == Request.DestSiteCode
                            && e.IsPrimaryResource
                            && e.MonthEndDate >= Request.FromDate).ToListAsync();

                           var SrcEmpRoasters = await _context.TblOpMonthlyRoasterForSites.OrderBy(e => e.MonthEndDate).AsNoTracking().Where(e => e.EmployeeNumber == Request.SrcEmployeeNumber
                            && e.ProjectCode == Request.SrcProjectCode
                            && e.SiteCode == Request.SrcSiteCode
                            && e.IsPrimaryResource
                            && e.MonthEndDate >= Request.FromDate).ToListAsync();

                     


                        List<TblOpMonthlyRoasterForSite> RemainingDestRoasters = DestEmpRoasters;
                        List<TblOpMonthlyRoasterForSite> RemainingSrcRoasters = SrcEmpRoasters;


                        if (Request.FromDate.Value.Day != 1)    //from date not starting from 1st means its in the middle of the month=>there exist partial roaster
                        {
                            var partialDestRoaster = DestEmpRoasters.FirstOrDefault();
                            var partialSrcRoaster = SrcEmpRoasters.FirstOrDefault();
                            RemainingDestRoasters = DestEmpRoasters.Where(e => e.Id != partialDestRoaster.Id).ToList();
                            RemainingSrcRoasters = SrcEmpRoasters.Where(e => e.Id != partialSrcRoaster.Id).ToList();

                            DateTime FirstDayofMonth = new DateTime(Request.FromDate.Value.Year, Request.FromDate.Value.Month, 1);
                            TblOpMonthlyRoasterForSite newRoaster = new TblOpMonthlyRoasterForSite()
                                {
                                    CustomerCode = partialDestRoaster.CustomerCode,
                                    ProjectCode = partialDestRoaster.ProjectCode,
                                    SiteCode = partialDestRoaster.SiteCode,
                                    EmployeeNumber = Request.SrcEmployeeNumber,
                                    SkillsetCode = partialDestRoaster.SkillsetCode,
                                    SkillsetName = partialDestRoaster.SkillsetName,
                                    MonthStartDate = partialDestRoaster.MonthStartDate,
                                    MonthEndDate = partialDestRoaster.MonthEndDate,
                                    Month = partialDestRoaster.Month,
                                    Year = partialDestRoaster.Year,
                                    IsPrimaryResource = true,
                                    EmployeeID = _contextDMC.HRM_TRAN_Employees.FirstOrDefault(x => x.EmployeeNumber == Request.SrcEmployeeNumber).EmployeeID,
                                    MapId = 0,
                                    S1 = FirstDayofMonth.AddDays(0) < Request.FromDate.Value ? "x" : partialDestRoaster.S1,
                                    S2 = FirstDayofMonth.AddDays(1) < Request.FromDate.Value ? "x" : partialDestRoaster.S2,
                                    S3 = FirstDayofMonth.AddDays(2) < Request.FromDate.Value ? "x" : partialDestRoaster.S3,
                                    S4 = FirstDayofMonth.AddDays(3) < Request.FromDate.Value ? "x" : partialDestRoaster.S4,
                                    S5 = FirstDayofMonth.AddDays(4) < Request.FromDate.Value ? "x" : partialDestRoaster.S5,
                                    S6 = FirstDayofMonth.AddDays(5) < Request.FromDate.Value ? "x" : partialDestRoaster.S6,
                                    S7 = FirstDayofMonth.AddDays(6) < Request.FromDate.Value ? "x" : partialDestRoaster.S7,
                                    S8 = FirstDayofMonth.AddDays(7) < Request.FromDate.Value ? "x" : partialDestRoaster.S8,
                                    S9 = FirstDayofMonth.AddDays(8) < Request.FromDate.Value ? "x" : partialDestRoaster.S9,
                                    S10 = FirstDayofMonth.AddDays(9) < Request.FromDate.Value ? "x" : partialDestRoaster.S10,
                                    S11 = FirstDayofMonth.AddDays(10) < Request.FromDate.Value ? "x" : partialDestRoaster.S11,
                                    S12 = FirstDayofMonth.AddDays(11) < Request.FromDate.Value ? "x" : partialDestRoaster.S12,
                                    S13 = FirstDayofMonth.AddDays(12) < Request.FromDate.Value ? "x" : partialDestRoaster.S13,
                                    S14 = FirstDayofMonth.AddDays(13) < Request.FromDate.Value ? "x" : partialDestRoaster.S14,
                                    S15 = FirstDayofMonth.AddDays(14) < Request.FromDate.Value ? "x" : partialDestRoaster.S15,
                                    S16 = FirstDayofMonth.AddDays(15) < Request.FromDate.Value ? "x" : partialDestRoaster.S16,
                                    S17 = FirstDayofMonth.AddDays(16) < Request.FromDate.Value ? "x" : partialDestRoaster.S17,
                                    S18 = FirstDayofMonth.AddDays(17) < Request.FromDate.Value ? "x" : partialDestRoaster.S18,
                                    S19 = FirstDayofMonth.AddDays(18) < Request.FromDate.Value ? "x" : partialDestRoaster.S19,
                                    S20 = FirstDayofMonth.AddDays(19) < Request.FromDate.Value ? "x" : partialDestRoaster.S20,
                                    S21 = FirstDayofMonth.AddDays(20) < Request.FromDate.Value ? "x" : partialDestRoaster.S21,
                                    S22 = FirstDayofMonth.AddDays(21) < Request.FromDate.Value ? "x" : partialDestRoaster.S22,
                                    S23 = FirstDayofMonth.AddDays(22) < Request.FromDate.Value ? "x" : partialDestRoaster.S23,
                                    S24 = FirstDayofMonth.AddDays(23) < Request.FromDate.Value ? "x" : partialDestRoaster.S24,
                                    S25 = FirstDayofMonth.AddDays(24) < Request.FromDate.Value ? "x" : partialDestRoaster.S25,
                                    S26 = FirstDayofMonth.AddDays(25) < Request.FromDate.Value ? "x" : partialDestRoaster.S26,
                                    S27 = FirstDayofMonth.AddDays(26) < Request.FromDate.Value ? "x" : partialDestRoaster.S27,
                                    S28 = FirstDayofMonth.AddDays(27) < Request.FromDate.Value ? "x" : partialDestRoaster.S28,
                                    S29 = FirstDayofMonth.AddDays(28) < Request.FromDate.Value ? "x" : partialDestRoaster.S29,
                                    S30 = FirstDayofMonth.AddDays(29) < Request.FromDate.Value ? "x" : partialDestRoaster.S30,
                                    S31 = FirstDayofMonth.AddDays(30) < Request.FromDate.Value ? "x" : partialDestRoaster.S31,

                                };
                            await _context.TblOpMonthlyRoasterForSites.AddAsync(newRoaster);
                            await _context.SaveChangesAsync();
                            TblOpMonthlyRoasterForSite newRoaster2 = new TblOpMonthlyRoasterForSite()
                                {
                                    CustomerCode = partialSrcRoaster.CustomerCode,
                                    ProjectCode = partialSrcRoaster.ProjectCode,
                                    SiteCode = partialSrcRoaster.SiteCode,
                                    EmployeeNumber = Request.DestEmployeeNumber,
                                    SkillsetCode = partialSrcRoaster.SkillsetCode,
                                    SkillsetName = partialSrcRoaster.SkillsetName,
                                    MonthStartDate = partialSrcRoaster.MonthStartDate,
                                    MonthEndDate = partialSrcRoaster.MonthEndDate,
                                    Month = partialSrcRoaster.Month,
                                    Year = partialSrcRoaster.Year,
                                    IsPrimaryResource = true,
                                    EmployeeID = _contextDMC.HRM_TRAN_Employees.FirstOrDefault(x => x.EmployeeNumber == Request.DestEmployeeNumber).EmployeeID,
                                    MapId = 0,
                                    S1 = FirstDayofMonth.AddDays(0) < Request.FromDate.Value ? "x" : partialSrcRoaster.S1,
                                    S2 = FirstDayofMonth.AddDays(1) < Request.FromDate.Value ? "x" : partialSrcRoaster.S2,
                                    S3 = FirstDayofMonth.AddDays(2) < Request.FromDate.Value ? "x" : partialSrcRoaster.S3,
                                    S4 = FirstDayofMonth.AddDays(3) < Request.FromDate.Value ? "x" : partialSrcRoaster.S4,
                                    S5 = FirstDayofMonth.AddDays(4) < Request.FromDate.Value ? "x" : partialSrcRoaster.S5,
                                    S6 = FirstDayofMonth.AddDays(5) < Request.FromDate.Value ? "x" : partialSrcRoaster.S6,
                                    S7 = FirstDayofMonth.AddDays(6) < Request.FromDate.Value ? "x" : partialSrcRoaster.S7,
                                    S8 = FirstDayofMonth.AddDays(7) < Request.FromDate.Value ? "x" : partialSrcRoaster.S8,
                                    S9 = FirstDayofMonth.AddDays(8) < Request.FromDate.Value ? "x" : partialSrcRoaster.S9,
                                    S10 = FirstDayofMonth.AddDays(9) < Request.FromDate.Value ? "x" : partialSrcRoaster.S10,
                                    S11 = FirstDayofMonth.AddDays(10) < Request.FromDate.Value ? "x" : partialSrcRoaster.S11,
                                    S12 = FirstDayofMonth.AddDays(11) < Request.FromDate.Value ? "x" : partialSrcRoaster.S12,
                                    S13 = FirstDayofMonth.AddDays(12) < Request.FromDate.Value ? "x" : partialSrcRoaster.S13,
                                    S14 = FirstDayofMonth.AddDays(13) < Request.FromDate.Value ? "x" : partialSrcRoaster.S14,
                                    S15 = FirstDayofMonth.AddDays(14) < Request.FromDate.Value ? "x" : partialSrcRoaster.S15,
                                    S16 = FirstDayofMonth.AddDays(15) < Request.FromDate.Value ? "x" : partialSrcRoaster.S16,
                                    S17 = FirstDayofMonth.AddDays(16) < Request.FromDate.Value ? "x" : partialSrcRoaster.S17,
                                    S18 = FirstDayofMonth.AddDays(17) < Request.FromDate.Value ? "x" : partialSrcRoaster.S18,
                                    S19 = FirstDayofMonth.AddDays(18) < Request.FromDate.Value ? "x" : partialSrcRoaster.S19,
                                    S20 = FirstDayofMonth.AddDays(19) < Request.FromDate.Value ? "x" : partialSrcRoaster.S20,
                                    S21 = FirstDayofMonth.AddDays(20) < Request.FromDate.Value ? "x" : partialSrcRoaster.S21,
                                    S22 = FirstDayofMonth.AddDays(21) < Request.FromDate.Value ? "x" : partialSrcRoaster.S22,
                                    S23 = FirstDayofMonth.AddDays(22) < Request.FromDate.Value ? "x" : partialSrcRoaster.S23,
                                    S24 = FirstDayofMonth.AddDays(23) < Request.FromDate.Value ? "x" : partialSrcRoaster.S24,
                                    S25 = FirstDayofMonth.AddDays(24) < Request.FromDate.Value ? "x" : partialSrcRoaster.S25,
                                    S26 = FirstDayofMonth.AddDays(25) < Request.FromDate.Value ? "x" : partialSrcRoaster.S26,
                                    S27 = FirstDayofMonth.AddDays(26) < Request.FromDate.Value ? "x" : partialSrcRoaster.S27,
                                    S28 = FirstDayofMonth.AddDays(27) < Request.FromDate.Value ? "x" : partialSrcRoaster.S28,
                                    S29 = FirstDayofMonth.AddDays(28) < Request.FromDate.Value ? "x" : partialSrcRoaster.S29,
                                    S30 = FirstDayofMonth.AddDays(29) < Request.FromDate.Value ? "x" : partialSrcRoaster.S30,
                                    S31 = FirstDayofMonth.AddDays(30) < Request.FromDate.Value ? "x" : partialSrcRoaster.S31,

                                };
                            await _context.TblOpMonthlyRoasterForSites.AddAsync(newRoaster2);
                            await _context.SaveChangesAsync();

                            partialDestRoaster.S1 = FirstDayofMonth.AddDays(0) < Request.FromDate.Value ? partialDestRoaster.S1 : "x";
                            partialDestRoaster.S2 = FirstDayofMonth.AddDays(1) < Request.FromDate.Value ? partialDestRoaster.S2 : "x";
                            partialDestRoaster.S3 = FirstDayofMonth.AddDays(2) < Request.FromDate.Value ? partialDestRoaster.S3 : "x";
                            partialDestRoaster.S4 = FirstDayofMonth.AddDays(3) < Request.FromDate.Value ? partialDestRoaster.S4 : "x";
                            partialDestRoaster.S5 = FirstDayofMonth.AddDays(4) < Request.FromDate.Value ? partialDestRoaster.S5 : "x";
                            partialDestRoaster.S6 = FirstDayofMonth.AddDays(5) < Request.FromDate.Value ? partialDestRoaster.S6 : "x";
                            partialDestRoaster.S7 = FirstDayofMonth.AddDays(6) < Request.FromDate.Value ? partialDestRoaster.S7 : "x";
                            partialDestRoaster.S8 = FirstDayofMonth.AddDays(7) < Request.FromDate.Value ? partialDestRoaster.S8 : "x";
                            partialDestRoaster.S9 = FirstDayofMonth.AddDays(8) < Request.FromDate.Value ? partialDestRoaster.S9 : "x";
                            partialDestRoaster.S10 = FirstDayofMonth.AddDays(9) < Request.FromDate.Value ? partialDestRoaster.S10 : "x";
                            partialDestRoaster.S11 = FirstDayofMonth.AddDays(10) < Request.FromDate.Value ? partialDestRoaster.S11 : "x";
                            partialDestRoaster.S12 = FirstDayofMonth.AddDays(11) < Request.FromDate.Value ? partialDestRoaster.S12 : "x";
                            partialDestRoaster.S13 = FirstDayofMonth.AddDays(12) < Request.FromDate.Value ? partialDestRoaster.S13 : "x";
                            partialDestRoaster.S14 = FirstDayofMonth.AddDays(13) < Request.FromDate.Value ? partialDestRoaster.S14 : "x";
                            partialDestRoaster.S15 = FirstDayofMonth.AddDays(14) < Request.FromDate.Value ? partialDestRoaster.S15 : "x";
                            partialDestRoaster.S16 = FirstDayofMonth.AddDays(15) < Request.FromDate.Value ? partialDestRoaster.S16 : "x";
                            partialDestRoaster.S17 = FirstDayofMonth.AddDays(16) < Request.FromDate.Value ? partialDestRoaster.S17 : "x";
                            partialDestRoaster.S18 = FirstDayofMonth.AddDays(17) < Request.FromDate.Value ? partialDestRoaster.S18 : "x";
                            partialDestRoaster.S19 = FirstDayofMonth.AddDays(18) < Request.FromDate.Value ? partialDestRoaster.S19 : "x";
                            partialDestRoaster.S20 = FirstDayofMonth.AddDays(19) < Request.FromDate.Value ? partialDestRoaster.S20 : "x";
                            partialDestRoaster.S21 = FirstDayofMonth.AddDays(20) < Request.FromDate.Value ? partialDestRoaster.S21 : "x";
                            partialDestRoaster.S22 = FirstDayofMonth.AddDays(21) < Request.FromDate.Value ? partialDestRoaster.S22 : "x";
                            partialDestRoaster.S23 = FirstDayofMonth.AddDays(22) < Request.FromDate.Value ? partialDestRoaster.S23 : "x";
                            partialDestRoaster.S24 = FirstDayofMonth.AddDays(23) < Request.FromDate.Value ? partialDestRoaster.S24 : "x";
                            partialDestRoaster.S25 = FirstDayofMonth.AddDays(24) < Request.FromDate.Value ? partialDestRoaster.S25 : "x";
                            partialDestRoaster.S26 = FirstDayofMonth.AddDays(25) < Request.FromDate.Value ? partialDestRoaster.S26 : "x";
                            partialDestRoaster.S27 = FirstDayofMonth.AddDays(26) < Request.FromDate.Value ? partialDestRoaster.S27 : "x";
                            partialDestRoaster.S28 = FirstDayofMonth.AddDays(27) < Request.FromDate.Value ? partialDestRoaster.S28 : "x";
                            partialDestRoaster.S29 = FirstDayofMonth.AddDays(28) < Request.FromDate.Value ? partialDestRoaster.S29 : "x";
                            partialDestRoaster.S30 = FirstDayofMonth.AddDays(29) < Request.FromDate.Value ? partialDestRoaster.S30 : "x";
                            partialDestRoaster.S31 = FirstDayofMonth.AddDays(30) < Request.FromDate.Value ? partialDestRoaster.S31 : "x";

                            _context.TblOpMonthlyRoasterForSites.Update(partialDestRoaster);
                            await _context.SaveChangesAsync();
                            
                            
                            partialSrcRoaster.S1 = FirstDayofMonth.AddDays(0) < Request.FromDate.Value ? partialSrcRoaster.S1 : "x";
                            partialSrcRoaster.S2 = FirstDayofMonth.AddDays(1) < Request.FromDate.Value ? partialSrcRoaster.S2 : "x";
                            partialSrcRoaster.S3 = FirstDayofMonth.AddDays(2) < Request.FromDate.Value ? partialSrcRoaster.S3 : "x";
                            partialSrcRoaster.S4 = FirstDayofMonth.AddDays(3) < Request.FromDate.Value ? partialSrcRoaster.S4 : "x";
                            partialSrcRoaster.S5 = FirstDayofMonth.AddDays(4) < Request.FromDate.Value ? partialSrcRoaster.S5 : "x";
                            partialSrcRoaster.S6 = FirstDayofMonth.AddDays(5) < Request.FromDate.Value ? partialSrcRoaster.S6 : "x";
                            partialSrcRoaster.S7 = FirstDayofMonth.AddDays(6) < Request.FromDate.Value ? partialSrcRoaster.S7 : "x";
                            partialSrcRoaster.S8 = FirstDayofMonth.AddDays(7) < Request.FromDate.Value ? partialSrcRoaster.S8 : "x";
                            partialSrcRoaster.S9 = FirstDayofMonth.AddDays(8) < Request.FromDate.Value ? partialSrcRoaster.S9 : "x";
                            partialSrcRoaster.S10 = FirstDayofMonth.AddDays(9) < Request.FromDate.Value ? partialSrcRoaster.S10 : "x";
                            partialSrcRoaster.S11 = FirstDayofMonth.AddDays(10) < Request.FromDate.Value ? partialSrcRoaster.S11 : "x";
                            partialSrcRoaster.S12 = FirstDayofMonth.AddDays(11) < Request.FromDate.Value ? partialSrcRoaster.S12 : "x";
                            partialSrcRoaster.S13 = FirstDayofMonth.AddDays(12) < Request.FromDate.Value ? partialSrcRoaster.S13 : "x";
                            partialSrcRoaster.S14 = FirstDayofMonth.AddDays(13) < Request.FromDate.Value ? partialSrcRoaster.S14 : "x";
                            partialSrcRoaster.S15 = FirstDayofMonth.AddDays(14) < Request.FromDate.Value ? partialSrcRoaster.S15 : "x";
                            partialSrcRoaster.S16 = FirstDayofMonth.AddDays(15) < Request.FromDate.Value ? partialSrcRoaster.S16 : "x";
                            partialSrcRoaster.S17 = FirstDayofMonth.AddDays(16) < Request.FromDate.Value ? partialSrcRoaster.S17 : "x";
                            partialSrcRoaster.S18 = FirstDayofMonth.AddDays(17) < Request.FromDate.Value ? partialSrcRoaster.S18 : "x";
                            partialSrcRoaster.S19 = FirstDayofMonth.AddDays(18) < Request.FromDate.Value ? partialSrcRoaster.S19 : "x";
                            partialSrcRoaster.S20 = FirstDayofMonth.AddDays(19) < Request.FromDate.Value ? partialSrcRoaster.S20 : "x";
                            partialSrcRoaster.S21 = FirstDayofMonth.AddDays(20) < Request.FromDate.Value ? partialSrcRoaster.S21 : "x";
                            partialSrcRoaster.S22 = FirstDayofMonth.AddDays(21) < Request.FromDate.Value ? partialSrcRoaster.S22 : "x";
                            partialSrcRoaster.S23 = FirstDayofMonth.AddDays(22) < Request.FromDate.Value ? partialSrcRoaster.S23 : "x";
                            partialSrcRoaster.S24 = FirstDayofMonth.AddDays(23) < Request.FromDate.Value ? partialSrcRoaster.S24 : "x";
                            partialSrcRoaster.S25 = FirstDayofMonth.AddDays(24) < Request.FromDate.Value ? partialSrcRoaster.S25 : "x";
                            partialSrcRoaster.S26 = FirstDayofMonth.AddDays(25) < Request.FromDate.Value ? partialSrcRoaster.S26 : "x";
                            partialSrcRoaster.S27 = FirstDayofMonth.AddDays(26) < Request.FromDate.Value ? partialSrcRoaster.S27 : "x";
                            partialSrcRoaster.S28 = FirstDayofMonth.AddDays(27) < Request.FromDate.Value ? partialSrcRoaster.S28 : "x";
                            partialSrcRoaster.S29 = FirstDayofMonth.AddDays(28) < Request.FromDate.Value ? partialSrcRoaster.S29 : "x";
                            partialSrcRoaster.S30 = FirstDayofMonth.AddDays(29) < Request.FromDate.Value ? partialSrcRoaster.S30 : "x";
                            partialSrcRoaster.S31 = FirstDayofMonth.AddDays(30) < Request.FromDate.Value ? partialSrcRoaster.S31 : "x";

                            _context.TblOpMonthlyRoasterForSites.Update(partialSrcRoaster);
                            await _context.SaveChangesAsync();
                        }


                        if (RemainingDestRoasters.Count > 0)
                        {
                            foreach (var r in RemainingDestRoasters)
                            {
                                r.EmployeeNumber = Request.SrcEmployeeNumber;
                                r.EmployeeID = _contextDMC.HRM_TRAN_Employees.AsNoTracking().FirstOrDefault(e => e.EmployeeNumber == Request.SrcEmployeeNumber).EmployeeID;

                            }
                            _context.TblOpMonthlyRoasterForSites.UpdateRange(RemainingDestRoasters);
                            await _context.SaveChangesAsync();
                        }

                         if (RemainingSrcRoasters.Count > 0)
                        {
                            foreach (var r in RemainingSrcRoasters)
                            {
                                r.EmployeeNumber = Request.DestEmployeeNumber;
                                r.EmployeeID = _contextDMC.HRM_TRAN_Employees.AsNoTracking().FirstOrDefault(e => e.EmployeeNumber == Request.DestEmployeeNumber).EmployeeID;

                            }
                            _context.TblOpMonthlyRoasterForSites.UpdateRange(RemainingSrcRoasters);
                            await _context.SaveChangesAsync();
                        }


                        Request.IsApproved = true;
                        Request.ApprovedBy = request.User.UserId;
                        Request.Modified = DateTime.UtcNow;
                        Request.IsMerged = true;
                        _context.TblOpPvSwapEmployeesReqs.Update(Request);
                        await _context.SaveChangesAsync();



                        var SrcEmpMapInProjectSite = await _context.TblOpEmployeesToProjectSiteList.AsNoTracking().FirstOrDefaultAsync(e => e.EmployeeNumber == Request.SrcEmployeeNumber && e.SiteCode == Request.SrcSiteCode && e.ProjectCode == Request.SrcProjectCode);
                        var DestEmpMapInProjectSite = await _context.TblOpEmployeesToProjectSiteList.AsNoTracking().FirstOrDefaultAsync(e => e.EmployeeNumber == Request.DestEmployeeNumber && e.SiteCode == Request.DestSiteCode && e.ProjectCode == Request.DestProjectCode);

                        if (SrcEmpMapInProjectSite is not null)
                        {
                            SrcEmpMapInProjectSite.ProjectCode = Request.DestProjectCode;
                            SrcEmpMapInProjectSite.SiteCode = Request.DestSiteCode;
                            _context.TblOpEmployeesToProjectSiteList.Update(SrcEmpMapInProjectSite);
                            await _context.SaveChangesAsync();

                        }
                        if (DestEmpMapInProjectSite is not null)
                        {
                            DestEmpMapInProjectSite.ProjectCode = Request.SrcProjectCode;
                            DestEmpMapInProjectSite.SiteCode = Request.SrcSiteCode;
                            _context.TblOpEmployeesToProjectSiteList.Update(DestEmpMapInProjectSite);
                            await _context.SaveChangesAsync();

                        }


                       



                        bool isSrcEmployeeTransferLogExist = await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AnyAsync(e => e.EmployeeNumber == Request.SrcEmployeeNumber);

                        if (!isSrcEmployeeTransferLogExist)
                        {
                            transaction.Rollback();
                            transaction2.Rollback();
                            return new() { IsSuccess = false, ErrorId = -1, ErrorMsg = "Prmary Site Log Not Found For Employee:" + Request.SrcEmployeeNumber };
                        }
                        else
                        {
                            var primarySite = _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AsNoTracking().OrderByDescending(o => o.TransferredDate).FirstOrDefault(e => e.EmployeeNumber == Request.SrcEmployeeNumber
                           && e.TransferredDate < Request.FromDate);
                            bool isSourceSitePrimarySite = primarySite.SiteCode == Request.SrcSiteCode;
                            if (isSourceSitePrimarySite)
                            {
                                HRM_TRAN_EmployeePrimarySites_Log logData = new()
                                {
                                    CreatedBy = request.User.UserId,
                                    CreatedDate = DateTime.UtcNow,
                                    SiteCode = Request.DestSiteCode,
                                    EmployeeNumber = Request.SrcEmployeeNumber,
                                    TransferredDate = Request.FromDate


                                };
                                await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AddAsync(logData);
                                await _contextDMC.SaveChangesAsync();

                            }
                        }



                          bool isDestEmployeeTransferLogExist = await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AnyAsync(e => e.EmployeeNumber == Request.DestEmployeeNumber);

                        if (!isDestEmployeeTransferLogExist)
                        {
                            transaction.Rollback();
                            transaction2.Rollback();
                            return new() { IsSuccess = false, ErrorId = -1, ErrorMsg = "Prmary Site Log Not Found For Employee:" + Request.DestEmployeeNumber };
                        }
                        else
                        {
                            var primarySite = _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AsNoTracking().OrderByDescending(o => o.TransferredDate).FirstOrDefault(e => e.EmployeeNumber == Request.DestEmployeeNumber
                           && e.TransferredDate < Request.FromDate);
                            bool isDestSitePrimarySite = primarySite.SiteCode == Request.DestSiteCode;
                            if (isDestSitePrimarySite)
                            {
                                HRM_TRAN_EmployeePrimarySites_Log logData = new()
                                {
                                    CreatedBy = request.User.UserId,
                                    CreatedDate = DateTime.UtcNow,
                                    SiteCode = Request.SrcSiteCode,
                                    EmployeeNumber = Request.DestEmployeeNumber,
                                    TransferredDate = Request.FromDate


                                };
                                await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AddAsync(logData);
                                await _contextDMC.SaveChangesAsync();

                            }
                        }

                        //Removing Non Primary Roasters for replaced Employee
                        var NonPrimaryRoasters = await _context.TblOpMonthlyRoasterForSites.AsNoTracking().Where(e =>
                        ((e.ProjectCode == Request.SrcProjectCode && e.SiteCode == Request.SrcSiteCode  && e.EmployeeNumber == Request.DestEmployeeNumber)
                        ||
                        (e.ProjectCode == Request.DestProjectCode && e.SiteCode == Request.DestSiteCode && e.EmployeeNumber == Request.SrcEmployeeNumber))
                        && e.MonthEndDate >= Request.FromDate
                        && e.IsPrimaryResource == false).ToListAsync();

                        if (NonPrimaryRoasters.Count > 0)
                        {
                            _context.TblOpMonthlyRoasterForSites.RemoveRange(NonPrimaryRoasters);
                            await _context.SaveChangesAsync();
                        }

                        await transaction.CommitAsync();
                        await transaction2.CommitAsync();

                        return new() { IsSuccess = true };


                    }
                    catch (Exception ex)
                    {

                        Log.Error("Error in ApproveReqPvSwapEmployeesReqByIdHandler Method");
                        Log.Error("Error occured time : " + DateTime.UtcNow);
                        Log.Error("Error message : " + ex.Message);
                        Log.Error("Error StackTrace : " + ex.StackTrace);
                        transaction.Rollback();
                        transaction2.Rollback();
                        return new() { IsSuccess = false, ErrorMsg = ApiMessageInfo.Failed };
                    }
                }
            }
        }
    }

    #endregion
}






