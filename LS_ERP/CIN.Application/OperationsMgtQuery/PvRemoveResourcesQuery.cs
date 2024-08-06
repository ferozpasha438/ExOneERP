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



    #region CreateUpdatePvRemoveResourceReq
    public class CreatePvRemoveResourceReq : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public TblOpPvRemoveResourceReqDto PvRemoveResourceReqDto { get; set; }
    }

    public class CreatePvRemoveResourceReqHandler : IRequestHandler<CreatePvRemoveResourceReq, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreatePvRemoveResourceReqHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<long> Handle(CreatePvRemoveResourceReq request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdatePvRemoveResourceReq method start----");




                    var obj = request.PvRemoveResourceReqDto;


                    TblOpPvRemoveResourceReq ReqHead = new();
                    if (obj.Id > 0)

                        ReqHead = await _context.TblOpPvRemoveResourceReqs.FirstOrDefaultAsync(e => e.Id == obj.Id);
                 
                        ReqHead.IsActive = obj.IsActive;
                        ReqHead.CustomerCode = obj.CustomerCode;
                        ReqHead.ProjectCode = obj.ProjectCode;
                        ReqHead.SiteCode = obj.SiteCode;
                        ReqHead.EmployeeNumber = obj.EmployeeNumber;
                        ReqHead.FromDate = obj.FromDate;
                    ReqHead.IsApproved = false;
                    ReqHead.ApprovedBy = 0;
                    ReqHead.IsApproved = false;
               
                    ReqHead.IsMerged = false;
                    ReqHead.IsActive = true;

                    ReqHead.FileUploadBy = null;
                    ReqHead.FileUrl = null;


                    if (obj.Id > 0)
                    {
                        ReqHead.Modified = DateTime.Now;
                        ReqHead.ModifiedBy = request.User.UserId;
                        _context.TblOpPvRemoveResourceReqs.Update(ReqHead);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ReqHead.CreatedBy = request.User.UserId;

                        ReqHead.Created = DateTime.Now;
                        await _context.TblOpPvRemoveResourceReqs.AddAsync(ReqHead);
                        await _context.SaveChangesAsync();
                    }
                    

                    
                      
                    
                    Log.Info("----Info CreateUpdatePvRemoveResourceReq method Exit----");

                    await transaction.CommitAsync();
                    return ReqHead.Id;

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdatePvRemoveResourceReq Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;

                }
            }    
        }
        
    }

    #endregion
    #region GetPvRemoveResourceReqsPagedList

    public class GetPvRemoveResourceReqsPagedList : IRequest<PaginatedList<TblOpPvRemoveResourceReqsPaginationDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetPvRemoveResourceReqsPagedListHandler : IRequestHandler<GetPvRemoveResourceReqsPagedList, PaginatedList<TblOpPvRemoveResourceReqsPaginationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPvRemoveResourceReqsPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblOpPvRemoveResourceReqsPaginationDto>> Handle(GetPvRemoveResourceReqsPagedList request, CancellationToken cancellationToken)
        {
            bool isAdmin = _context.SystemLogins.AsNoTracking().FirstOrDefault(e => e.Id == request.User.UserId).LoginId == "Admin" ? true : false;
            var oprAuths = _context.TblOpAuthoritiesList.AsNoTracking();
            var Projects = _context.OP_HRM_TEMP_Projects.AsNoTracking();

            var Sites = _context.OprSites.AsNoTracking();
            var search = request.Input.Query;
            var list = await _context.TblOpPvRemoveResourceReqs.AsNoTracking()
               .OrderBy(request.Input.OrderBy).Select(d=> new TblOpPvRemoveResourceReqsPaginationDto {
                   Id = d.Id,
                   CustomerCode = d.CustomerCode,
                   SiteCode = d.SiteCode,
                   SiteName = Sites.FirstOrDefault(s => s.SiteCode == d.SiteCode).SiteName,
                   SiteNameAr = Sites.FirstOrDefault(s => s.SiteCode == d.SiteCode).SiteArbName,
                   ProjectCode = d.ProjectCode,
                   ProjectName = Projects.FirstOrDefault(p => p.ProjectCode == d.ProjectCode).ProjectNameEng,
                   ProjectNameAr = Projects.FirstOrDefault(p => p.ProjectCode == d.ProjectCode).ProjectNameArb,
                   EmployeeNumber = d.EmployeeNumber,
                   FromDate = d.FromDate,
                   IsActive = d.IsActive,
                   IsApproved = d.IsApproved,
                   CanEditReq=d.CreatedBy==request.User.UserId,
                   CanApproveReq= oprAuths.Any(a=>a.AppAuth==request.User.UserId &&a.CanApprovePvReq && a.BranchCode==Sites.FirstOrDefault(s=>s.SiteCode==d.SiteCode).SiteCityCode)||isAdmin
                   ,IsAdmin=isAdmin, 
                   ApprovedBy=d.ApprovedBy
                    ,FileUrl=d.FileUrl,
                   IsFileUploadRequired=true,
                   RequestNumber = d.Id,

               })
              .Where(e =>
                            (e.CustomerCode.Contains(search) ||
                            e.SiteCode.Contains(search) ||
                            e.ProjectCode.Contains(search) ||
                            e.SiteName.Contains(search) ||
                            e.SiteNameAr.Contains(search) ||
                            e.ProjectName.Contains(search) ||
                            e.ProjectNameAr.Contains(search) ||
                            e.EmployeeNumber.Contains(search) ||
                            search == "" || search == null
                             ) &&(e.CanEditReq|| e.CanApproveReq))
               
               .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion

    #region GetPvRemoveResourceReqById
    public class GetPvRemoveResourceReqById : IRequest<TblOpPvRemoveResourceReqDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetPvRemoveResourceReqByIdHandler : IRequestHandler<GetPvRemoveResourceReqById, TblOpPvRemoveResourceReqDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPvRemoveResourceReqByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpPvRemoveResourceReqDto> Handle(GetPvRemoveResourceReqById request, CancellationToken cancellationToken)
        {
            try
            {

                var PvRemoveResourceReq = await _context.TblOpPvRemoveResourceReqs.AsNoTracking().ProjectTo<TblOpPvRemoveResourceReqDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);


                return PvRemoveResourceReq;





            }
            catch (Exception ex)
            {
                Log.Error("Error in GetPvRemoveResourceReqByIdHandler Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion


    #region DeletePvRemoveResourceReqById
    public class DeletePvRemoveResourceReqById : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeletePvRemoveResourceReqByIdHandler : IRequestHandler<DeletePvRemoveResourceReqById, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public DeletePvRemoveResourceReqByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<long> Handle(DeletePvRemoveResourceReqById request, CancellationToken cancellationToken)
        {
           try
                {

                    var PvRemoveResourceReq = await _context.TblOpPvRemoveResourceReqs.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.TblOpPvRemoveResourceReqs.Remove(PvRemoveResourceReq);
                   
                    _context.SaveChanges();
                return request.Id;

                }
                catch (Exception ex)
                {
                   
                    Log.Error("Error in DeletePvRemoveResourceReqByIdHandler Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    #endregion


    #region ApproveReqPvRemoveResourceReqById
    public class ApproveReqPvRemoveResourceReqById : IRequest<TblOpPvRemoveResourceReqDto>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }

    public class ApproveReqPvRemoveResourceReqByIdHandler : IRequestHandler<ApproveReqPvRemoveResourceReqById, TblOpPvRemoveResourceReqDto>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public ApproveReqPvRemoveResourceReqByIdHandler(CINDBOneContext context,DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<TblOpPvRemoveResourceReqDto> Handle(ApproveReqPvRemoveResourceReqById request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                using (var transaction2 = await _contextDMC.Database.BeginTransactionAsync())
                {

                    try
                    {
                        Log.Info("----Info ApproveReqPvRemoveResourceReqById method start----");

                        TblOpPvRemoveResourceReq PvRemoveResourceReq = _context.TblOpPvRemoveResourceReqs.AsNoTracking().FirstOrDefault(e => e.Id == request.Id);
                        if (PvRemoveResourceReq is null)
                        {
                            return new() { Id = -1 };       //not exist
                        }
                        if (PvRemoveResourceReq.IsApproved)
                        {
                            return new() { Id = -2 };      //Already Approved
                        }
                        var projectData = _context.TblOpProjectSites.AsNoTracking().FirstOrDefault(p => p.ProjectCode == PvRemoveResourceReq.ProjectCode);
                        if (projectData is null)
                        {
                            return new() { Id = -3 };      //invalid project code
                        }

                        var roastersList = _context.TblOpMonthlyRoasterForSites.AsNoTracking().Where(m => m.MonthEndDate >= PvRemoveResourceReq.FromDate
                         && m.EmployeeNumber == PvRemoveResourceReq.EmployeeNumber
                         && m.ProjectCode == PvRemoveResourceReq.ProjectCode
                         && m.SiteCode == PvRemoveResourceReq.SiteCode).ToList();
                        if (roastersList.Count == 0)
                            return new() { Id = -4 };  //No Removements Found ,Resigned Employee not having roaster 
                        TblOpMonthlyRoasterForSite partialMonth = new();
                        List<TblOpMonthlyRoasterForSite> reaminingMonths = new();

                        bool isExistAttendance = false;
                        foreach(var r in roastersList)
                        {
                            var attendance = _context.EmployeeAttendance.AsNoTracking().Where(a => a.AttnDate >= PvRemoveResourceReq.FromDate
                         && a.ProjectCode == PvRemoveResourceReq.ProjectCode
                         && a.SiteCode == PvRemoveResourceReq.SiteCode
                         && (a.EmployeeNumber == PvRemoveResourceReq.EmployeeNumber
                             || a.AltEmployeeNumber == PvRemoveResourceReq.EmployeeNumber)).ToList();

                            if (attendance.Count > 0)
                            {
                                isExistAttendance = true;
                            }
                            if (r.MonthStartDate.Value.Month == PvRemoveResourceReq.FromDate.Value.Month && r.MonthStartDate.Value.Year == PvRemoveResourceReq.FromDate.Value.Year && r.MonthStartDate.Value.Day != PvRemoveResourceReq.FromDate.Value.Day)
                            {

                                partialMonth = r;

                            }
                            else
                            {
                                reaminingMonths.Add(r);
                            }

                        }

                        if (isExistAttendance)
                        {
                            return new() { Id = -6 };           //Attendance already entered

                        }

                        if (reaminingMonths.Count > 0)
                        {
                            _context.TblOpMonthlyRoasterForSites.RemoveRange(reaminingMonths);
                            _context.SaveChanges();
                        }

                        if (partialMonth != null && partialMonth.Id !=0)
                        {
                            var fromDateDay = PvRemoveResourceReq.FromDate.Value.Day;

                            for (var d = partialMonth.MonthStartDate.Value.Day; d <= partialMonth.MonthEndDate.Value.Day; d++)
                            {

                                switch (d)
                                {

                                    case 1:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S1 = "x";
                                        }


                                        break;
                                    case 2:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S2 = "x";
                                        }
                                        break;
                                    case 3:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S3 = "x";
                                        }
                                        break;
                                    case 4:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S4 = "x";
                                        }
                                        break;
                                    case 5:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S5 = "x";
                                        }
                                        break;
                                    case 6:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S6 = "x";
                                        }
                                        break;
                                    case 7:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S7 = "x";
                                        }
                                        break;
                                    case 8:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S8 = "x";
                                        }
                                        break;
                                    case 9:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S9 = "x";
                                        }
                                        break;
                                    case 10:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S10 = "x";
                                        }
                                        break;
                                    case 11:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S11 = "x";
                                        }
                                        break;
                                    case 12:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S12 = "x";
                                        }
                                        break;
                                    case 13:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S13 = "x";
                                        }
                                        break;
                                    case 14:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S14 = "x";
                                        }
                                        break;
                                    case 15:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S15 = "x";
                                        }
                                        break;
                                    case 16:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S16 = "x";
                                        }
                                        break;
                                    case 17:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S17 = "x";
                                        }
                                        break;
                                    case 18:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S18 = "x";
                                        }
                                        break;
                                    case 19:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S19 = "x";
                                        }
                                        break;
                                    case 20:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S20 = "x";
                                        }
                                        break;
                                    case 21:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S21 = "x";
                                        }
                                        break;
                                    case 22:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S22 = "x";
                                        }
                                        break;
                                    case 23:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S23 = "x";
                                        }
                                        break;
                                    case 24:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S24 = "x";
                                        }
                                        break;
                                    case 25:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S25 = "x";
                                        }
                                        break;
                                    case 26:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S26 = "x";
                                        }
                                        break;
                                    case 27:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S27 = "x";
                                        }
                                        break;
                                    case 28:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S28 = "x";
                                        }
                                        break;
                                    case 29:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S29 = "x";
                                        }
                                        break;
                                    case 30:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S30 = "x";
                                        }
                                        break;
                                    case 31:
                                        if (d >= fromDateDay)
                                        {

                                            partialMonth.S31 = "x";
                                        }
                                        break;
                                    default:
                                        transaction.Rollback();
                                        return new() { Id = 0 };
                                }


                            }

                            _context.TblOpMonthlyRoasterForSites.Update(partialMonth);
                            _context.SaveChanges();

                        }
                        PvRemoveResourceReq.IsMerged = true;
                        PvRemoveResourceReq.IsApproved = true;
                        PvRemoveResourceReq.ApprovedBy = request.User.UserId;
                        PvRemoveResourceReq.Modified =DateTime.UtcNow;
                        _context.TblOpPvRemoveResourceReqs.Update(PvRemoveResourceReq);
                        _context.SaveChanges();

                        List<HRM_DEF_EmployeeOff> offDays = new();
                        HRM_TRAN_Employee emp = _contextDMC.HRM_TRAN_Employees.AsNoTracking().FirstOrDefault(e => e.EmployeeNumber == PvRemoveResourceReq.EmployeeNumber);
                        if (emp is null || emp.EmployeeID == 0)
                        {
                            transaction.Rollback();
                            return new() { Id = -7 };          //Invalid Employee  Number

                        }

                        offDays = _contextDMC.HRM_DEF_EmployeeOffs.AsNoTracking().Where(e => e.EmployeeId == emp.EmployeeID
                          && e.Date >= PvRemoveResourceReq.FromDate
                          && e.Date <= projectData.EndDate
                          && e.SiteCode==PvRemoveResourceReq.SiteCode).ToList();

                   
                        if (offDays.Count > 0)
                        {
                            _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                            _contextDMC.SaveChanges();
                        }



                        bool isEmployeePrimarySiteLogExist = await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AsNoTracking().AnyAsync(e => e.EmployeeNumber == PvRemoveResourceReq.EmployeeNumber
                        &&e.TransferredDate<=PvRemoveResourceReq.FromDate);

                        if (!isEmployeePrimarySiteLogExist)
                        {
                            return new() { Id= -8 };          //Primary Site Log Not Found      

                        }

                        var PrimarySite= await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AsNoTracking().OrderByDescending(e=>e.TransferredDate).FirstOrDefaultAsync(e => e.EmployeeNumber == PvRemoveResourceReq.EmployeeNumber
                        && e.TransferredDate <= PvRemoveResourceReq.FromDate);

                        if (PrimarySite is not null) {
                            if (PrimarySite.SiteCode == PvRemoveResourceReq.SiteCode)
                            {
                                HRM_TRAN_EmployeePrimarySites_Log SiteLog = new()
                                {
                                    SiteCode = "0000000000",
                                    CreatedBy = request.User.UserId,
                                    CreatedDate = DateTime.UtcNow,
                                    EmployeeNumber = PvRemoveResourceReq.EmployeeNumber,
                                    TransferredDate = PvRemoveResourceReq.FromDate

                                };
                                await _contextDMC.AddAsync(SiteLog);
                                await _contextDMC.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            await transaction.RollbackAsync();
                            await transaction2.RollbackAsync();
                            return new() { Id = -8 }; //Primary Site Log Not Found      
                        }

                        






                        transaction.Commit();
                        transaction2.Commit();

                        return new() { Id = request.Id };





                    }
                    catch (Exception ex)
                    {
                         transaction.Rollback();
                         transaction2.Rollback();
                        Log.Error("Error in ApproveReqPvRemoveResourceReqByIdHandler Method");
                        Log.Error("Error occured time : " + DateTime.UtcNow);
                        Log.Error("Error message : " + ex.Message);
                        Log.Error("Error StackTrace : " + ex.StackTrace);
                 
                        return new() { Id = 0 };
                    }
                }
            }
        }
    }

    #endregion



    #region IsValidRemoveResourceRequest
    public class IsValidRemoveResourceRequest : IRequest<ValidityCheckDto>
    {
        public UserIdentityDto User { get; set; }
        public TblOpPvRemoveResourceReqDto InputDto { get; set; }
    }

    public class IsValidRemoveResourceRequestHandler : IRequestHandler<IsValidRemoveResourceRequest, ValidityCheckDto>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public IsValidRemoveResourceRequestHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<ValidityCheckDto> Handle(IsValidRemoveResourceRequest request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {

                request.InputDto.FromDate = Convert.ToDateTime(request.InputDto.FromDate, CultureInfo.InvariantCulture);

                try
                {
                    Log.Info("----Info IsValidRemoveResourceRequest method start----");

                    if (request.InputDto.Id > 0 && request.InputDto.IsApproved)
                    {
                        return new() { IsValidReq = false, ErrorId = -1, ErrorMsg = "Request Already Approved" };

                    }

                    if (!await _context.OP_HRM_TEMP_Projects.AnyAsync(e => e.ProjectCode == request.InputDto.ProjectCode))
                    {
                        return new() { IsValidReq = false, ErrorId = -2, ErrorMsg = "Invalid Source ProjectCode" };
                    }
                  



                   
                    if (!await _context.OprSites.AnyAsync(e => e.SiteCode == request.InputDto.SiteCode))
                    {
                        return new() { IsValidReq = false, ErrorId = -3, ErrorMsg = "Invalid Source SIteCode" };
                    }


                  

                    if (!(await _context.TblOpMonthlyRoasterForSites.AsNoTracking().AnyAsync(e => e.EmployeeNumber == request.InputDto.EmployeeNumber && e.ProjectCode == request.InputDto.ProjectCode && e.SiteCode == request.InputDto.SiteCode && e.MonthEndDate >= request.InputDto.FromDate && e.IsPrimaryResource)))
                    {
                        return new() { IsValidReq = false, ErrorId = -4, ErrorMsg = "Employee Not Found in the Project" };
                    }



              
                    if (await _context.EmployeeAttendance.AnyAsync(e => 
                   // e.EmployeeNumber == request.InputDto.EmployeeNumber && 
                    e.ProjectCode == request.InputDto.ProjectCode && e.SiteCode == request.InputDto.SiteCode && e.AttnDate >= request.InputDto.FromDate))
                    {
                        var attendanceExist = await _context.EmployeeAttendance.OrderByDescending(e => e.AttnDate).FirstOrDefaultAsync(e =>
                        //e.EmployeeNumber == request.InputDto.EmployeeNumber && 
                        e.ProjectCode == request.InputDto.ProjectCode && e.SiteCode == request.InputDto.SiteCode && e.AttnDate >= request.InputDto.FromDate);
                        return new() { IsValidReq = false, ErrorId = -5, ErrorMsg = "Attendance Exist for Project:" + attendanceExist.AttnDate.ToString() };

                    }



                    bool isEmployeePrimarySiteLogExist = await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AsNoTracking().AnyAsync(e => e.EmployeeNumber == request.InputDto.EmployeeNumber
                    && e.TransferredDate<=request.InputDto.FromDate);

                    if (!isEmployeePrimarySiteLogExist)
                    {
                        return new() { IsValidReq = false, ErrorId = -6, ErrorMsg = "Primary site log not exist for Employee" + request.InputDto.EmployeeNumber };

                    }
                  


                    return new() { IsValidReq = true };


                 

                }
                catch (Exception ex)
                {

                    Log.Error("Error in IsValidRemoveResourceRequestHandler Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    transaction.Rollback();
                    return new() { IsValidReq = false, ErrorId = 0, ErrorMsg = "Some thing went wrong" };
                }
            }

        }
    }

    #endregion
}






