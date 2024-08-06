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
    

    


    #region IsValidTransferWithReplacementRequest
    public class IsValidTransferWithReplacementRequest : IRequest<ValidityCheckDto>
    {
        public UserIdentityDto User { get; set; }
        public TblOpPvTransferWithReplacementReqDto InputDto { get; set; }
    }

    public class IsValidTransferWithReplacementRequestHandler : IRequestHandler<IsValidTransferWithReplacementRequest, ValidityCheckDto>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public IsValidTransferWithReplacementRequestHandler(CINDBOneContext context, DMCContext contextDMC,  IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<ValidityCheckDto> Handle(IsValidTransferWithReplacementRequest request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {

              

                    try
                    {
                        Log.Info("----Info IsValidTransferWithReplacementRequest method start----");

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

                    if (!(await _context.TblOpMonthlyRoasterForSites.AsNoTracking().AnyAsync(e=>e.EmployeeNumber==request.InputDto.SrcEmployeeNumber&& e.ProjectCode==request.InputDto.SrcProjectCode&& e.SiteCode==request.InputDto.SrcSiteCode&&e.MonthEndDate>=request.InputDto.FromDate &&e.IsPrimaryResource))) {
                        return new() { IsValidReq = false, ErrorId = -6, ErrorMsg = "Source Employee Not Found in the Project" };
                    }

                     if (!(await _context.TblOpMonthlyRoasterForSites.AsNoTracking().AnyAsync(e=> e.EmployeeNumber == request.InputDto.DestEmployeeNumber && e.ProjectCode==request.InputDto.DestProjectCode&& e.SiteCode==request.InputDto.DestSiteCode && e.MonthEndDate>=request.InputDto.FromDate && e.IsPrimaryResource))) {
                        return new() { IsValidReq = false, ErrorId = -7, ErrorMsg = "Dest Employee Not Found in the Project" };
                    }

                    var LastRoasterForResignedEmployee = await _context.TblOpMonthlyRoasterForSites.AsNoTracking().OrderByDescending(e => e.MonthEndDate).FirstOrDefaultAsync(e => e.EmployeeNumber == request.InputDto.SrcEmployeeNumber && e.ProjectCode == request.InputDto.SrcProjectCode && e.SiteCode == request.InputDto.SrcSiteCode && e.MonthEndDate >= request.InputDto.FromDate && e.IsPrimaryResource);



                    if (await _context.TblOpMonthlyRoasterForSites.AsNoTracking().AnyAsync(e => e.EmployeeNumber == request.InputDto.DestEmployeeNumber && e.ProjectCode == request.InputDto.DestEmployeeNumber && e.SiteCode == request.InputDto.DestSiteCode && e.MonthEndDate >= request.InputDto.FromDate && e.IsPrimaryResource && e.MonthEndDate <= LastRoasterForResignedEmployee.MonthEndDate))
                    {
                        var LastRoasterForReplaceExist = await _context.TblOpMonthlyRoasterForSites.AsNoTracking().OrderByDescending(e => e.MonthEndDate).FirstOrDefaultAsync(e => e.EmployeeNumber == request.InputDto.DestEmployeeNumber && e.ProjectCode == request.InputDto.DestProjectCode && e.SiteCode == request.InputDto.DestSiteCode && e.MonthEndDate >= request.InputDto.FromDate && e.IsPrimaryResource && e.MonthEndDate <= LastRoasterForResignedEmployee.MonthEndDate);
                        return new() { IsValidReq = false, ErrorId = -8, ErrorMsg = "Employee:" + request.InputDto.DestEmployeeNumber + " Already Exist in the Project:" + request.InputDto.DestProjectCode + ", Year:" + LastRoasterForReplaceExist.Year + ",Month:" + LastRoasterForReplaceExist.Month };

                    }
                    if (await _context.EmployeeAttendance.AnyAsync(e => e.EmployeeNumber == request.InputDto.SrcEmployeeNumber && e.ProjectCode == request.InputDto.SrcProjectCode && e.SiteCode == request.InputDto.SrcSiteCode && e.AttnDate >= request.InputDto.FromDate)) {
                        var attendanceExist = await _context.EmployeeAttendance.OrderByDescending(e=>e.AttnDate).FirstOrDefaultAsync(e => e.EmployeeNumber == request.InputDto.SrcEmployeeNumber && e.ProjectCode == request.InputDto.SrcProjectCode && e.SiteCode == request.InputDto.SrcSiteCode && e.AttnDate >= request.InputDto.FromDate);
                        return new() { IsValidReq = false, ErrorId = -9, ErrorMsg = "Source Employee Attendance Exist:"+ attendanceExist.AttnDate.ToString() };
                    
                    }
                       if (await _context.EmployeeAttendance.AnyAsync(e => e.EmployeeNumber == request.InputDto.SrcEmployeeNumber && e.ProjectCode == request.InputDto.DestProjectCode && e.SiteCode == request.InputDto.DestSiteCode && e.AttnDate >= request.InputDto.FromDate)) {
                        var attendanceExist = await _context.EmployeeAttendance.OrderByDescending(e=>e.AttnDate).FirstOrDefaultAsync(e => e.EmployeeNumber == request.InputDto.SrcEmployeeNumber && e.ProjectCode == request.InputDto.DestProjectCode && e.SiteCode == request.InputDto.DestSiteCode && e.AttnDate >= request.InputDto.FromDate);
                        return new() { IsValidReq = false, ErrorId = -12, ErrorMsg = "Source Employee Attendance Exist in Destination Site:"+ attendanceExist.AttnDate.ToString() };
                    
                    }

                     if (await _context.EmployeeAttendance.AnyAsync(e => e.EmployeeNumber == request.InputDto.DestEmployeeNumber && e.ProjectCode == request.InputDto.DestProjectCode && e.SiteCode == request.InputDto.DestSiteCode && e.AttnDate >= request.InputDto.FromDate)) {
                        var attendanceExist = await _context.EmployeeAttendance.OrderByDescending(e=>e.AttnDate).FirstOrDefaultAsync(e => e.EmployeeNumber == request.InputDto.DestEmployeeNumber && e.ProjectCode == request.InputDto.DestProjectCode && e.SiteCode == request.InputDto.DestSiteCode && e.AttnDate >= request.InputDto.FromDate);
                        return new() { IsValidReq = false, ErrorId = -10, ErrorMsg = "Dest Employee Attendance Exist:"+ attendanceExist.AttnDate.ToString() };
                    }

                        bool isEmployeeTransferLogExist = await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AnyAsync(e => e.EmployeeNumber == request.InputDto.SrcEmployeeNumber && e.TransferredDate<=request.InputDto.FromDate);

                    if (!isEmployeeTransferLogExist)
                    { 
                        return new() { IsValidReq = false, ErrorId = -13, ErrorMsg = "Primary site log not exist for Employee"+ request.InputDto.SrcEmployeeNumber };

                    }

                      bool isEmployeeTransferLogExistForRepl = await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AnyAsync(e => e.EmployeeNumber == request.InputDto.DestEmployeeNumber && e.TransferredDate <= request.InputDto.FromDate);

                    if (!isEmployeeTransferLogExistForRepl)
                    { 
                        return new() { IsValidReq = false, ErrorId = -14, ErrorMsg = "Primary site log not exist for Employee"+ request.InputDto.DestEmployeeNumber };

                    }




                    return new() { IsValidReq=true };


                    //latest count=-13

                    }
                    catch (Exception ex)
                    {

                        Log.Error("Error in IsValidTransferWithReplacementRequestHandler Method");
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






    #region CreateUpdatePvTransferWithReplacementReq
    public class CreatePvTransferWithReplacementReq : IRequest<CreateUpadteResultDto>
    {
        public UserIdentityDto User { get; set; }
        public TblOpPvTransferWithReplacementReqDto InputDto { get; set; }
    }

    public class CreatePvTransferWithReplacementReqHandler : IRequestHandler<CreatePvTransferWithReplacementReq, CreateUpadteResultDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreatePvTransferWithReplacementReqHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreateUpadteResultDto> Handle(CreatePvTransferWithReplacementReq request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdatePvTransferWithReplacementReq method start----");

                    var obj = request.InputDto;
                    TblOpPvTransferWithReplacementReq ReqHead = new();
                    if (obj.Id > 0)

                        ReqHead = await _context.TblOpPvTransferWithReplacementReqs.AsNoTracking().FirstOrDefaultAsync(e => e.Id == obj.Id);

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
                        _context.TblOpPvTransferWithReplacementReqs.Update(ReqHead);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ReqHead.CreatedBy = request.User.UserId;

                        ReqHead.Created = DateTime.Now;
                        await _context.TblOpPvTransferWithReplacementReqs.AddAsync(ReqHead);
                        await _context.SaveChangesAsync();
                    }

                    Log.Info("----Info CreateUpdatePvTransferWithReplacementReq method Exit----");

                    await transaction.CommitAsync();
                    return new() {IsSuccess=true ,};

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdatePvTransferWithReplacementReq Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return new() { IsSuccess=false,ErrorMsg=ApiMessageInfo.Failed,ErrorId=0};

                }
            }
        }

    }

    #endregion






    #region GetPvTransferWithReplacementReqsPagedList

    public class GetPvTransferWithReplacementReqsPagedList : IRequest<PaginatedList<TblOpPvTransferWithReplacementReqsPaginationDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetPvTransferWithReplacementReqsPagedListHandler : IRequestHandler<GetPvTransferWithReplacementReqsPagedList, PaginatedList<TblOpPvTransferWithReplacementReqsPaginationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPvTransferWithReplacementReqsPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblOpPvTransferWithReplacementReqsPaginationDto>> Handle(GetPvTransferWithReplacementReqsPagedList request, CancellationToken cancellationToken)
        {
            bool isAdmin = _context.SystemLogins.AsNoTracking().FirstOrDefault(e => e.Id == request.User.UserId).LoginId == "Admin" ? true : false;

            var oprAuths = _context.TblOpAuthoritiesList.AsNoTracking();
            var Projects = _context.OP_HRM_TEMP_Projects.AsNoTracking();

            var Sites = _context.OprSites.AsNoTracking();
            var search = request.Input.Query;
            var list = await _context.TblOpPvTransferWithReplacementReqs.AsNoTracking()
               .OrderBy(request.Input.OrderBy).Select(d => new TblOpPvTransferWithReplacementReqsPaginationDto
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
                   ,IsAdmin=isAdmin
                   , ApprovedBy=d.ApprovedBy
                   
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




    #region GetPvTransferWithReplacementReqById
    public class GetPvTransferWithReplacementReqById : IRequest<TblOpPvTransferWithReplacementReqDto>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }

    public class GetPvTransferWithReplacementReqByIdHandler : IRequestHandler<GetPvTransferWithReplacementReqById, TblOpPvTransferWithReplacementReqDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPvTransferWithReplacementReqByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpPvTransferWithReplacementReqDto> Handle(GetPvTransferWithReplacementReqById request, CancellationToken cancellationToken)
        {
            try
            {

                var PvTransferWithReplacementReq = await _context.TblOpPvTransferWithReplacementReqs.ProjectTo<TblOpPvTransferWithReplacementReqDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);


                return PvTransferWithReplacementReq;





            }
            catch (Exception ex)
            {
                Log.Error("Error in GetPvTransferWithReplacementReqByIdHandler Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion


    #region DeletePvTransferWithReplacementReqById
    public class DeletePvTransferWithReplacementReqById : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeletePvTransferWithReplacementReqByIdHandler : IRequestHandler<DeletePvTransferWithReplacementReqById, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public DeletePvTransferWithReplacementReqByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<long> Handle(DeletePvTransferWithReplacementReqById request, CancellationToken cancellationToken)
        {
            try
            {

                var PvTransferWithReplacementReq = await _context.TblOpPvTransferWithReplacementReqs.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
                _context.TblOpPvTransferWithReplacementReqs.Remove(PvTransferWithReplacementReq);

                _context.SaveChanges();
                return request.Id;

            }
            catch (Exception ex)
            {

                Log.Error("Error in DeletePvTransferWithReplacementReqByIdHandler Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }
    #endregion

    #region ApproveReqPvTransferWithReplacementReqById
    public class ApproveReqPvTransferWithReplacementReqById : IRequest<CreateUpadteResultDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class ApproveReqPvTransferWithReplacementReqByIdHandler : IRequestHandler<ApproveReqPvTransferWithReplacementReqById, CreateUpadteResultDto>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public ApproveReqPvTransferWithReplacementReqByIdHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<CreateUpadteResultDto> Handle(ApproveReqPvTransferWithReplacementReqById request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                using (var transaction2 = await _contextDMC.Database.BeginTransactionAsync())
                {
                    try
                    {
                        Log.Info("----Info ApproveReqPvTransferWithReplacementReqById method start----");

                        var Request = await _context.TblOpPvTransferWithReplacementReqs.FirstOrDefaultAsync(e => e.Id == request.Id);


                        #region steps
                        //1.find Roasters For Dest Employee
                        //2.Find Partial And RemainingRoasters
                        //3.Check SrcEmployee Exist AS NotPrimaryResource
                        #endregion
                        var DestEmpRoasters = await _context.TblOpMonthlyRoasterForSites.OrderBy(e => e.MonthEndDate).AsNoTracking().Where(e => e.EmployeeNumber == Request.DestEmployeeNumber
                            && e.ProjectCode == Request.DestProjectCode
                            && e.SiteCode == Request.DestSiteCode
                            && e.IsPrimaryResource
                            && e.MonthEndDate >= Request.FromDate).ToListAsync();

                        var releiverRoaster = await _context.TblOpMonthlyRoasterForSites.FirstOrDefaultAsync(e => e.EmployeeNumber == Request.SrcEmployeeNumber
                             && e.ProjectCode == Request.DestProjectCode
                             && e.SiteCode == Request.DestSiteCode

                             && (!e.IsPrimaryResource)

                             && e.MonthEndDate >= Request.FromDate
                             && e.MonthStartDate <= Request.FromDate);

                        if (releiverRoaster is not null)
                        {
                            _context.TblOpMonthlyRoasterForSites.Remove(releiverRoaster);
                            await _context.SaveChangesAsync();
                        }



                        List<TblOpMonthlyRoasterForSite> RemainingRoasters = DestEmpRoasters;


                        if (Request.FromDate.Value.Day != 1)    //from date not starting from 1st means its in the middle of the month=>there exist partial roaster
                        {
                            var partialRoaster = DestEmpRoasters.FirstOrDefault();
                            RemainingRoasters = DestEmpRoasters.Where(e => e.Id != partialRoaster.Id).ToList();
                            DateTime FirstDayofMonth = new DateTime(Request.FromDate.Value.Year, Request.FromDate.Value.Month, 1);
                            TblOpMonthlyRoasterForSite newRoaster =

                                new TblOpMonthlyRoasterForSite()
                                {
                                    CustomerCode = partialRoaster.CustomerCode,
                                    ProjectCode = partialRoaster.ProjectCode,
                                    SiteCode = partialRoaster.SiteCode,
                                    EmployeeNumber = Request.SrcEmployeeNumber,
                                    SkillsetCode = partialRoaster.SkillsetCode,
                                    SkillsetName = partialRoaster.SkillsetName,
                                    MonthStartDate = partialRoaster.MonthStartDate,
                                    MonthEndDate = partialRoaster.MonthEndDate,
                                    Month = partialRoaster.Month,
                                    Year = partialRoaster.Year,
                                    IsPrimaryResource = true,
                                    EmployeeID = _contextDMC.HRM_TRAN_Employees.FirstOrDefault(x => x.EmployeeNumber == Request.SrcEmployeeNumber).EmployeeID,
                                    MapId = 0,
                                    S1 = FirstDayofMonth.AddDays(0) < Request.FromDate.Value ? "x" : partialRoaster.S1,
                                    S2 = FirstDayofMonth.AddDays(1) < Request.FromDate.Value ? "x" : partialRoaster.S2,
                                    S3 = FirstDayofMonth.AddDays(2) < Request.FromDate.Value ? "x" : partialRoaster.S3,
                                    S4 = FirstDayofMonth.AddDays(3) < Request.FromDate.Value ? "x" : partialRoaster.S4,
                                    S5 = FirstDayofMonth.AddDays(4) < Request.FromDate.Value ? "x" : partialRoaster.S5,
                                    S6 = FirstDayofMonth.AddDays(5) < Request.FromDate.Value ? "x" : partialRoaster.S6,
                                    S7 = FirstDayofMonth.AddDays(6) < Request.FromDate.Value ? "x" : partialRoaster.S7,
                                    S8 = FirstDayofMonth.AddDays(7) < Request.FromDate.Value ? "x" : partialRoaster.S8,
                                    S9 = FirstDayofMonth.AddDays(8) < Request.FromDate.Value ? "x" : partialRoaster.S9,
                                    S10 = FirstDayofMonth.AddDays(9) < Request.FromDate.Value ? "x" : partialRoaster.S10,
                                    S11 = FirstDayofMonth.AddDays(10) < Request.FromDate.Value ? "x" : partialRoaster.S11,
                                    S12 = FirstDayofMonth.AddDays(11) < Request.FromDate.Value ? "x" : partialRoaster.S12,
                                    S13 = FirstDayofMonth.AddDays(12) < Request.FromDate.Value ? "x" : partialRoaster.S13,
                                    S14 = FirstDayofMonth.AddDays(13) < Request.FromDate.Value ? "x" : partialRoaster.S14,
                                    S15 = FirstDayofMonth.AddDays(14) < Request.FromDate.Value ? "x" : partialRoaster.S15,
                                    S16 = FirstDayofMonth.AddDays(15) < Request.FromDate.Value ? "x" : partialRoaster.S16,
                                    S17 = FirstDayofMonth.AddDays(16) < Request.FromDate.Value ? "x" : partialRoaster.S17,
                                    S18 = FirstDayofMonth.AddDays(17) < Request.FromDate.Value ? "x" : partialRoaster.S18,
                                    S19 = FirstDayofMonth.AddDays(18) < Request.FromDate.Value ? "x" : partialRoaster.S19,
                                    S20 = FirstDayofMonth.AddDays(19) < Request.FromDate.Value ? "x" : partialRoaster.S20,
                                    S21 = FirstDayofMonth.AddDays(20) < Request.FromDate.Value ? "x" : partialRoaster.S21,
                                    S22 = FirstDayofMonth.AddDays(21) < Request.FromDate.Value ? "x" : partialRoaster.S22,
                                    S23 = FirstDayofMonth.AddDays(22) < Request.FromDate.Value ? "x" : partialRoaster.S23,
                                    S24 = FirstDayofMonth.AddDays(23) < Request.FromDate.Value ? "x" : partialRoaster.S24,
                                    S25 = FirstDayofMonth.AddDays(24) < Request.FromDate.Value ? "x" : partialRoaster.S25,
                                    S26 = FirstDayofMonth.AddDays(25) < Request.FromDate.Value ? "x" : partialRoaster.S26,
                                    S27 = FirstDayofMonth.AddDays(26) < Request.FromDate.Value ? "x" : partialRoaster.S27,
                                    S28 = FirstDayofMonth.AddDays(27) < Request.FromDate.Value ? "x" : partialRoaster.S28,
                                    S29 = FirstDayofMonth.AddDays(28) < Request.FromDate.Value ? "x" : partialRoaster.S29,
                                    S30 = FirstDayofMonth.AddDays(29) < Request.FromDate.Value ? "x" : partialRoaster.S30,
                                    S31 = FirstDayofMonth.AddDays(30) < Request.FromDate.Value ? "x" : partialRoaster.S31,

                                };
                            await _context.TblOpMonthlyRoasterForSites.AddAsync(newRoaster);
                            await _context.SaveChangesAsync();

                            partialRoaster.S1 = FirstDayofMonth.AddDays(0) < Request.FromDate.Value ? partialRoaster.S1 : "x";
                            partialRoaster.S2 = FirstDayofMonth.AddDays(1) < Request.FromDate.Value ? partialRoaster.S2 : "x";
                            partialRoaster.S3 = FirstDayofMonth.AddDays(2) < Request.FromDate.Value ? partialRoaster.S3 : "x";
                            partialRoaster.S4 = FirstDayofMonth.AddDays(3) < Request.FromDate.Value ? partialRoaster.S4 : "x";
                            partialRoaster.S5 = FirstDayofMonth.AddDays(4) < Request.FromDate.Value ? partialRoaster.S5 : "x";
                            partialRoaster.S6 = FirstDayofMonth.AddDays(5) < Request.FromDate.Value ? partialRoaster.S6 : "x";
                            partialRoaster.S7 = FirstDayofMonth.AddDays(6) < Request.FromDate.Value ? partialRoaster.S7 : "x";
                            partialRoaster.S8 = FirstDayofMonth.AddDays(7) < Request.FromDate.Value ? partialRoaster.S8 : "x";
                            partialRoaster.S9 = FirstDayofMonth.AddDays(8) < Request.FromDate.Value ? partialRoaster.S9 : "x";
                            partialRoaster.S10 = FirstDayofMonth.AddDays(9) < Request.FromDate.Value ? partialRoaster.S10 : "x";
                            partialRoaster.S11 = FirstDayofMonth.AddDays(10) < Request.FromDate.Value ? partialRoaster.S11 : "x";
                            partialRoaster.S12 = FirstDayofMonth.AddDays(11) < Request.FromDate.Value ? partialRoaster.S12 : "x";
                            partialRoaster.S13 = FirstDayofMonth.AddDays(12) < Request.FromDate.Value ? partialRoaster.S13 : "x";
                            partialRoaster.S14 = FirstDayofMonth.AddDays(13) < Request.FromDate.Value ? partialRoaster.S14 : "x";
                            partialRoaster.S15 = FirstDayofMonth.AddDays(14) < Request.FromDate.Value ? partialRoaster.S15 : "x";
                            partialRoaster.S16 = FirstDayofMonth.AddDays(15) < Request.FromDate.Value ? partialRoaster.S16 : "x";
                            partialRoaster.S17 = FirstDayofMonth.AddDays(16) < Request.FromDate.Value ? partialRoaster.S17 : "x";
                            partialRoaster.S18 = FirstDayofMonth.AddDays(17) < Request.FromDate.Value ? partialRoaster.S18 : "x";
                            partialRoaster.S19 = FirstDayofMonth.AddDays(18) < Request.FromDate.Value ? partialRoaster.S19 : "x";
                            partialRoaster.S20 = FirstDayofMonth.AddDays(19) < Request.FromDate.Value ? partialRoaster.S20 : "x";
                            partialRoaster.S21 = FirstDayofMonth.AddDays(20) < Request.FromDate.Value ? partialRoaster.S21 : "x";
                            partialRoaster.S22 = FirstDayofMonth.AddDays(21) < Request.FromDate.Value ? partialRoaster.S22 : "x";
                            partialRoaster.S23 = FirstDayofMonth.AddDays(22) < Request.FromDate.Value ? partialRoaster.S23 : "x";
                            partialRoaster.S24 = FirstDayofMonth.AddDays(23) < Request.FromDate.Value ? partialRoaster.S24 : "x";
                            partialRoaster.S25 = FirstDayofMonth.AddDays(24) < Request.FromDate.Value ? partialRoaster.S25 : "x";
                            partialRoaster.S26 = FirstDayofMonth.AddDays(25) < Request.FromDate.Value ? partialRoaster.S26 : "x";
                            partialRoaster.S27 = FirstDayofMonth.AddDays(26) < Request.FromDate.Value ? partialRoaster.S27 : "x";
                            partialRoaster.S28 = FirstDayofMonth.AddDays(27) < Request.FromDate.Value ? partialRoaster.S28 : "x";
                            partialRoaster.S29 = FirstDayofMonth.AddDays(28) < Request.FromDate.Value ? partialRoaster.S29 : "x";
                            partialRoaster.S30 = FirstDayofMonth.AddDays(29) < Request.FromDate.Value ? partialRoaster.S30 : "x";
                            partialRoaster.S31 = FirstDayofMonth.AddDays(30) < Request.FromDate.Value ? partialRoaster.S31 : "x";

                            _context.TblOpMonthlyRoasterForSites.Update(partialRoaster);
                            await _context.SaveChangesAsync();
                        }


                        if (RemainingRoasters.Count > 0)
                        {
                            foreach (var r in RemainingRoasters)
                            {
                                r.EmployeeNumber = Request.SrcEmployeeNumber;
                                r.EmployeeID = _contextDMC.HRM_TRAN_Employees.AsNoTracking().FirstOrDefault(e => e.EmployeeNumber == Request.SrcEmployeeNumber).EmployeeID;

                            }
                            _context.TblOpMonthlyRoasterForSites.UpdateRange(RemainingRoasters);
                            await _context.SaveChangesAsync();
                        }


                        Request.IsApproved = true;
                        Request.ApprovedBy = request.User.UserId;
                        Request.Modified = DateTime.UtcNow;
                        Request.IsMerged = true;
                        _context.TblOpPvTransferWithReplacementReqs.Update(Request);
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

                            _context.TblOpEmployeesToProjectSiteList.Remove(DestEmpMapInProjectSite);
                            await _context.SaveChangesAsync();

                        }



                        bool isEmployeeTransferLogExist = await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AnyAsync(e => e.EmployeeNumber == Request.SrcEmployeeNumber
                        && e.TransferredDate <=Request.FromDate);

                        if (!isEmployeeTransferLogExist)
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









                        await transaction.CommitAsync();
                        await transaction2.CommitAsync();

                        return new() { IsSuccess = true };


                    }
                    catch (Exception ex)
                    {

                        Log.Error("Error in ApproveReqPvTransferWithReplacementReqByIdHandler Method");
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






