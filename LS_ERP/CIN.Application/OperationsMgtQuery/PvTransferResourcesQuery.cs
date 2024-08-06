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

    #region IsValidTransferResourceRequest
    public class IsValidTransferResourceRequest : IRequest<ValidityCheckDto>
    {
        public UserIdentityDto User { get; set; }
        public TblOpPvTransferResourceReqDto InputDto { get; set; }
    }

    public class IsValidTransferResourceRequestHandler : IRequestHandler<IsValidTransferResourceRequest, ValidityCheckDto>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public IsValidTransferResourceRequestHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<ValidityCheckDto> Handle(IsValidTransferResourceRequest request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {



                try
                {
                    Log.Info("----Info IsValidTransferResourceRequest method start----");

                    if (request.InputDto.Id > 0 && request.InputDto.IsApproved)
                    {
                        return new() { IsValidReq = false, ErrorId = -11, ErrorMsg = "Request Already Approved" };

                    }

                    if (!await _context.OP_HRM_TEMP_Projects.AnyAsync(e => e.ProjectCode == request.InputDto.SrcProjectCode))
                    {
                        return new() { IsValidReq = false, ErrorId = -1, ErrorMsg = "Invalid Source ProjectCode" };
                    }
                    if (!await _context.OP_HRM_TEMP_Projects.AnyAsync(e => e.ProjectCode == request.InputDto.DestProjectCode))
                    {
                        return new() { IsValidReq = false, ErrorId = -2, ErrorMsg = "Invalid Destination ProjectCode" };
                    }



                    if (!await _context.OprSites.AnyAsync(e => e.SiteCode == request.InputDto.DestSiteCode))
                    {
                        return new() { IsValidReq = false, ErrorId = -3, ErrorMsg = "Invalid Destination SiteCode" };
                    }
                    if (!await _context.OprSites.AnyAsync(e => e.SiteCode == request.InputDto.SrcSiteCode))
                    {
                        return new() { IsValidReq = false, ErrorId = -4, ErrorMsg = "Invalid Source SIteCode" };
                    }

                    var srcProject = await _context.TblOpProjectSites.AsNoTracking().FirstAsync(e => e.ProjectCode == request.InputDto.SrcProjectCode && e.SiteCode == request.InputDto.SrcSiteCode);
                    var destProject = await _context.TblOpProjectSites.AsNoTracking().FirstAsync(e => e.ProjectCode == request.InputDto.DestProjectCode && e.SiteCode == request.InputDto.DestSiteCode);

                    if (srcProject.StartDate > request.InputDto.FromDate ||
                        destProject.StartDate > request.InputDto.FromDate ||
                        srcProject.EndDate < request.InputDto.FromDate ||
                        destProject.EndDate < request.InputDto.FromDate
                        )
                    {
                        return new() { IsValidReq = false, ErrorId = -5, ErrorMsg = "Incompatible From Date" };
                    }

                    if (!(await _context.TblOpMonthlyRoasterForSites.AsNoTracking().AnyAsync(e => e.EmployeeNumber == request.InputDto.EmployeeNumber && e.ProjectCode == request.InputDto.SrcProjectCode && e.SiteCode == request.InputDto.SrcSiteCode && e.MonthEndDate >= request.InputDto.FromDate && e.IsPrimaryResource)))
                    {
                        return new() { IsValidReq = false, ErrorId = -6, ErrorMsg = "Employee Not Found in the Project" };
                    }

                  

                    if (await _context.TblOpMonthlyRoasterForSites.AsNoTracking().AnyAsync(e => e.ProjectCode == request.InputDto.DestProjectCode && e.SiteCode == request.InputDto.DestSiteCode && e.MonthEndDate >= request.InputDto.FromDate && e.IsPrimaryResource && e.EmployeeNumber == request.InputDto.EmployeeNumber))
                    {
                        return new() { IsValidReq = false, ErrorId = -8, ErrorMsg = "Employee Already Exist in the Dest Project" };
                    }

                    if (await _context.EmployeeAttendance.AnyAsync(e => 
                  //  e.EmployeeNumber == request.InputDto.EmployeeNumber &&
                     e.ProjectCode == request.InputDto.SrcProjectCode && e.SiteCode == request.InputDto.SrcSiteCode && e.AttnDate >= request.InputDto.FromDate))
                    {
                        var attendanceExist = await _context.EmployeeAttendance.OrderByDescending(e => e.AttnDate).FirstOrDefaultAsync(e =>
                    //e.EmployeeNumber == request.InputDto.EmployeeNumber && 
                    e.ProjectCode == request.InputDto.SrcProjectCode && e.SiteCode == request.InputDto.SrcSiteCode && e.AttnDate >= request.InputDto.FromDate);
                        return new() { IsValidReq = false, ErrorId = -9, ErrorMsg = "Source Employee Attendance Exist:" + attendanceExist.AttnDate.ToString() };

                    }
              
                 

                    bool isEmployeeTransferLogExist = await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AnyAsync(e => e.EmployeeNumber == request.InputDto.EmployeeNumber
                    && e.TransferredDate <= request.InputDto.FromDate);

                    if (!isEmployeeTransferLogExist)
                    {
                        return new() { IsValidReq = false, ErrorId = -13, ErrorMsg = "Primary site log not exist for Employee" + request.InputDto.EmployeeNumber };

                    }




                    return new() { IsValidReq = true };


                    //latest count=-13

                }
                catch (Exception ex)
                {

                    Log.Error("Error in IsValidTransferResourceRequestHandler Method");
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


    #region CreateUpdatePvTransferResourceReq
    public class CreatePvTransferResourceReq : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public TblOpPvTransferResourceReqDto PvTransferResourceReqDto { get; set; }
    }

    public class CreatePvTransferResourceReqHandler : IRequestHandler<CreatePvTransferResourceReq, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreatePvTransferResourceReqHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<long> Handle(CreatePvTransferResourceReq request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdatePvTransferResourceReq method start----");




                    var obj = request.PvTransferResourceReqDto;


                    TblOpPvTransferResourceReq ReqHead = new();
                    if (obj.Id > 0)

                        ReqHead = await _context.TblOpPvTransferResourceReqs.FirstOrDefaultAsync(e => e.Id == obj.Id);
                 
                        ReqHead.IsActive = obj.IsActive;
                        ReqHead.SrcCustomerCode = obj.SrcCustomerCode;
                        ReqHead.SrcProjectCode = obj.SrcProjectCode;
                        ReqHead.SrcSiteCode = obj.SrcSiteCode; 
                    ReqHead.DestCustomerCode = obj.DestCustomerCode;
                        ReqHead.DestProjectCode = obj.DestProjectCode;
                        ReqHead.DestSiteCode = obj.DestSiteCode;
                        ReqHead.EmployeeNumber = obj.EmployeeNumber;
                        ReqHead.FromDate = obj.FromDate;
                    ReqHead.IsApproved = false;
                    ReqHead.ApprovedBy = 0;
                    ReqHead.IsApproved = false;
                  
                    ReqHead.IsMerged = false;
                    ReqHead.IsActive = true;




                    if (obj.Id > 0)
                    {
                        ReqHead.Modified = DateTime.Now;
                        ReqHead.ModifiedBy = request.User.UserId;
                        _context.TblOpPvTransferResourceReqs.Update(ReqHead);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ReqHead.Id = 0;
                        ReqHead.CreatedBy = request.User.UserId;

                        ReqHead.Created = DateTime.Now;
                        await _context.TblOpPvTransferResourceReqs.AddAsync(ReqHead);
                        await _context.SaveChangesAsync();
                    }
                    

                    
                      
                    
                    Log.Info("----Info CreateUpdatePvTransferResourceReq method Exit----");

                    await transaction.CommitAsync();
                    return ReqHead.Id;

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdatePvTransferResourceReq Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;

                }
            }    
        }
        
    }

    #endregion

    #region GetPvTransferResourceReqsPagedList

    public class GetPvTransferResourceReqsPagedList : IRequest<PaginatedList<TblOpPvTransferResourceReqsPaginationDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetPvTransferResourceReqsPagedListHandler : IRequestHandler<GetPvTransferResourceReqsPagedList, PaginatedList<TblOpPvTransferResourceReqsPaginationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPvTransferResourceReqsPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblOpPvTransferResourceReqsPaginationDto>> Handle(GetPvTransferResourceReqsPagedList request, CancellationToken cancellationToken)
        {
            bool isAdmin = _context.SystemLogins.AsNoTracking().FirstOrDefault(e => e.Id == request.User.UserId).LoginId == "Admin" ? true : false;

            var oprAuths = _context.TblOpAuthoritiesList.AsNoTracking();
            var Projects = _context.OP_HRM_TEMP_Projects.AsNoTracking();

            var Sites = _context.OprSites.AsNoTracking();
            var search = request.Input.Query;
            var list = await _context.TblOpPvTransferResourceReqs.AsNoTracking()
               .OrderBy(request.Input.OrderBy).Select(d => new TblOpPvTransferResourceReqsPaginationDto
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
                   EmployeeNumber = d.EmployeeNumber,
                   FromDate = d.FromDate,
                   IsActive = d.IsActive,
                   IsApproved = d.IsApproved,
                   CanEditReq=request.User.UserId==d.CreatedBy,
                   CanApproveReq=oprAuths.Any(a=>a.AppAuth==request.User.UserId &&a.CanApprovePvReq&& a.BranchCode==Sites.FirstOrDefault(s=>s.SiteCode==d.DestSiteCode).SiteCityCode)||isAdmin
                   , ApprovedBy=d.ApprovedBy,
                   IsAdmin=d.IsApproved
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
                            e.EmployeeNumber.Contains(search) ||
                            search == "" || search == null
                             )&&(e.CanApproveReq||e.CanEditReq))

               .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion

    #region GetPvTransferResourceReqById
    public class GetPvTransferResourceReqById : IRequest<TblOpPvTransferResourceReqDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetPvTransferResourceReqByIdHandler : IRequestHandler<GetPvTransferResourceReqById, TblOpPvTransferResourceReqDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPvTransferResourceReqByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpPvTransferResourceReqDto> Handle(GetPvTransferResourceReqById request, CancellationToken cancellationToken)
        {
            try
            {

                var PvTransferResourceReq = await _context.TblOpPvTransferResourceReqs.AsNoTracking().ProjectTo<TblOpPvTransferResourceReqDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);


                return PvTransferResourceReq;





            }
            catch (Exception ex)
            {
                Log.Error("Error in GetPvTransferResourceReqByIdHandler Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion


    #region DeletePvTransferResourceReqById
    public class DeletePvTransferResourceReqById : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeletePvTransferResourceReqByIdHandler : IRequestHandler<DeletePvTransferResourceReqById, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public DeletePvTransferResourceReqByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<long> Handle(DeletePvTransferResourceReqById request, CancellationToken cancellationToken)
        {
           try
                {

                    var PvTransferResourceReq = await _context.TblOpPvTransferResourceReqs.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.TblOpPvTransferResourceReqs.Remove(PvTransferResourceReq);
                   
                    _context.SaveChanges();
                return request.Id;

                }
                catch (Exception ex)
                {
                   
                    Log.Error("Error in DeletePvTransferResourceReqByIdHandler Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    #endregion

    #region ApproveReqPvTransferResourceReqById
    public class ApproveReqPvTransferResourceReqById : IRequest<TblOpPvTransferResourceReqDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class ApproveReqPvTransferResourceReqByIdHandler : IRequestHandler<ApproveReqPvTransferResourceReqById, TblOpPvTransferResourceReqDto>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public ApproveReqPvTransferResourceReqByIdHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<TblOpPvTransferResourceReqDto> Handle(ApproveReqPvTransferResourceReqById request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                using (var transaction2 = _contextDMC.Database.BeginTransaction())
                {

                    try
                    {
                        Log.Info("----Info ApproveReqPvTransferResourceReqById method start----");







                        TblOpPvTransferResourceReq PvTransferResourceReq = _context.TblOpPvTransferResourceReqs.AsNoTracking().FirstOrDefault(e => e.Id == request.Id);
                        if (PvTransferResourceReq is null)
                        {
                            return new() { Id = -1 };       //not exist
                        }
                        if (PvTransferResourceReq.IsApproved)
                        {
                            return new() { Id = -2 };      //Already Approved
                        }
                        TblOpProjectSites SrcProjectData = _context.TblOpProjectSites.AsNoTracking().FirstOrDefault(p => p.ProjectCode == PvTransferResourceReq.SrcProjectCode);
                        if (SrcProjectData is null)
                        {
                            return new() { Id = -3 };      //invalid SrcProject code
                        }
                        TblOpProjectSites DestProjectData = _context.TblOpProjectSites.AsNoTracking().FirstOrDefault(p => p.ProjectCode == PvTransferResourceReq.DestProjectCode);
                        if (DestProjectData is null)
                        {
                            return new() { Id = -4 };      //invalid DesPproject code
                        }


                        if (SrcProjectData.StartDate > DestProjectData.EndDate || SrcProjectData.EndDate < DestProjectData.StartDate || PvTransferResourceReq.FromDate > DestProjectData.EndDate)
                        {

                            return new() { Id = -5 };     //Incompatible Projects Dates
                        }
                        var roastersList = _context.TblOpMonthlyRoasterForSites.AsNoTracking().Where(m => m.MonthEndDate >= PvTransferResourceReq.FromDate
                         && m.EmployeeNumber == PvTransferResourceReq.EmployeeNumber
                         && m.ProjectCode == PvTransferResourceReq.SrcProjectCode
                         && m.SiteCode == PvTransferResourceReq.SrcSiteCode).ToList();
                        if (roastersList.Count == 0)
                            return new() { Id = -6 };  //Employee not having roaster 


                        var isRoasterExistInDest = _context.TblOpMonthlyRoasterForSites.AsNoTracking().Any(m => m.MonthEndDate >= PvTransferResourceReq.FromDate
                        && m.ProjectCode == PvTransferResourceReq.DestProjectCode
                        && m.SiteCode == PvTransferResourceReq.DestSiteCode);
                        if (!isRoasterExistInDest)
                            return new() { Id = -14 };  //Destination Not Having Roaster 





                        HRM_TRAN_Employee emp = _contextDMC.HRM_TRAN_Employees.AsNoTracking().FirstOrDefault(e => e.EmployeeNumber == PvTransferResourceReq.EmployeeNumber);
                        if (emp is null || emp.EmployeeID == 0)
                        {
                            transaction.Rollback();
                            return new() { Id = -12 };          //Invalid Employee  Number

                        }


                        var isEmployeeExistInDestProject = _context.TblOpMonthlyRoasterForSites.AsNoTracking().Any(m => m.MonthEndDate >= PvTransferResourceReq.FromDate
                           && m.EmployeeNumber == PvTransferResourceReq.EmployeeNumber
                           && m.ProjectCode == PvTransferResourceReq.DestProjectCode
                           && m.SiteCode == PvTransferResourceReq.DestSiteCode);
                        if (isEmployeeExistInDestProject)
                        {
                            return new() { Id = -7 };          //Employee Already Exist in Dest Project
                        }






                        DateTime resEndDate = SrcProjectData.EndDate.Value > DestProjectData.EndDate.Value ? SrcProjectData.EndDate.Value : DestProjectData.EndDate.Value;


                        TblOpMonthlyRoasterForSite partialMonth = new();
                        List<TblOpMonthlyRoasterForSite> remainingMonths = new();

                        bool isExistAttendance = false;
                        roastersList.ForEach(r =>
                        {
                            List<TblOpEmployeeAttendance> attendance = _context.EmployeeAttendance.AsNoTracking().Where(a => a.AttnDate >= PvTransferResourceReq.FromDate
                    && ((a.ProjectCode == PvTransferResourceReq.SrcProjectCode && a.SiteCode == PvTransferResourceReq.SrcSiteCode) || (a.ProjectCode == PvTransferResourceReq.DestProjectCode && a.SiteCode == PvTransferResourceReq.DestSiteCode))
                    && (a.EmployeeNumber == PvTransferResourceReq.EmployeeNumber
                        || a.AltEmployeeNumber == PvTransferResourceReq.EmployeeNumber)

                         ).ToList();

                            if (attendance.Count > 0)
                            {
                                isExistAttendance = true;
                            }
                            if (r.MonthStartDate.Value.Month == PvTransferResourceReq.FromDate.Value.Month && r.MonthStartDate.Value.Year == PvTransferResourceReq.FromDate.Value.Year && r.MonthStartDate.Value.Day != PvTransferResourceReq.FromDate.Value.Day)
                            {

                                partialMonth = r;

                            }
                            else
                            {
                                remainingMonths.Add(r);
                            }

                        });

                        if (isExistAttendance)
                        {

                            return new() { Id = -8 };           //Attendance already entered

                        }

                        if (remainingMonths.Count > 0)
                        {


                            for (int i = 0; i < remainingMonths.Count; i++)
                            {

                                if (remainingMonths[i].MonthEndDate <= DestProjectData.EndDate)
                                {
                                    remainingMonths[i].ProjectCode = PvTransferResourceReq.DestProjectCode;
                                    remainingMonths[i].SiteCode = PvTransferResourceReq.DestSiteCode;
                                    remainingMonths[i].CustomerCode = PvTransferResourceReq.DestCustomerCode;

                                }
                                else
                                {
                                    remainingMonths.RemoveAt(i);
                                    i--;
                                }

                            }


                            if (remainingMonths.Count > 0)
                            {
                                _context.TblOpMonthlyRoasterForSites.UpdateRange(remainingMonths);
                                _context.SaveChanges();
                            }






                        }



                        if (partialMonth.Id != 0)
                        {
                            TblOpMonthlyRoasterForSite NewRoaster = new();
                            NewRoaster.Id = 0;
                            NewRoaster.EmployeeNumber = PvTransferResourceReq.EmployeeNumber;
                            NewRoaster.Month = partialMonth.Month;
                            NewRoaster.Year = partialMonth.Year;
                            NewRoaster.MonthEndDate = partialMonth.MonthEndDate;
                            NewRoaster.MonthStartDate = partialMonth.MonthStartDate;
                            NewRoaster.S1 = "x";
                            NewRoaster.S2 = "x";
                            NewRoaster.S3 = "x";
                            NewRoaster.S4 = "x";
                            NewRoaster.S5 = "x";
                            NewRoaster.S6 = "x";
                            NewRoaster.S7 = "x";
                            NewRoaster.S8 = "x";
                            NewRoaster.S9 = "x";
                            NewRoaster.S10 = "x";
                            NewRoaster.S11 = "x";
                            NewRoaster.S12 = "x";
                            NewRoaster.S13 = "x";
                            NewRoaster.S14 = "x";
                            NewRoaster.S15 = "x";
                            NewRoaster.S16 = "x";
                            NewRoaster.S17 = "x";
                            NewRoaster.S18 = "x";
                            NewRoaster.S19 = "x";

                            NewRoaster.S20 = "x";
                            NewRoaster.S21 = "x";
                            NewRoaster.S22 = "x";
                            NewRoaster.S23 = "x";
                            NewRoaster.S24 = "x";
                            NewRoaster.S25 = "x";
                            NewRoaster.S26 = "x";
                            NewRoaster.S27 = "x";
                            NewRoaster.S28 = "x";
                            NewRoaster.S29 = "x";
                            NewRoaster.S30 = "x";
                            NewRoaster.S31 = "x";
                            NewRoaster.IsPrimaryResource = true;
                            NewRoaster.CustomerCode = PvTransferResourceReq.DestCustomerCode;
                            NewRoaster.ProjectCode = PvTransferResourceReq.DestProjectCode;
                            NewRoaster.SiteCode = PvTransferResourceReq.DestSiteCode;
                            NewRoaster.SkillsetCode = partialMonth.SkillsetCode;
                            NewRoaster.SkillsetName = partialMonth.SkillsetName;









                            var fromDateDay = PvTransferResourceReq.FromDate.Value.Day;

                            for (var d = partialMonth.MonthStartDate.Value.Day; d <= partialMonth.MonthEndDate.Value.Day; d++)
                            {

                                switch (d)
                                {

                                    case 1:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S1 = partialMonth.S1;
                                            partialMonth.S1 = "x";
                                        }


                                        break;
                                    case 2:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S2 = partialMonth.S2;
                                            partialMonth.S2 = "x";
                                        }
                                        break;
                                    case 3:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S3 = partialMonth.S3;
                                            partialMonth.S3 = "x";
                                        }
                                        break;
                                    case 4:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S4 = partialMonth.S4;
                                            partialMonth.S4 = "x";
                                        }
                                        break;
                                    case 5:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S5 = partialMonth.S5;
                                            partialMonth.S5 = "x";
                                        }
                                        break;
                                    case 6:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S6 = partialMonth.S6;
                                            partialMonth.S6 = "x";
                                        }
                                        break;
                                    case 7:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S7 = partialMonth.S7;
                                            partialMonth.S7 = "x";
                                        }
                                        break;
                                    case 8:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S8 = partialMonth.S8;
                                            partialMonth.S8 = "x";
                                        }
                                        break;
                                    case 9:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S9 = partialMonth.S9;
                                            partialMonth.S9 = "x";
                                        }
                                        break;
                                    case 10:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S10 = partialMonth.S10;
                                            partialMonth.S10 = "x";
                                        }
                                        break;
                                    case 11:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S11 = partialMonth.S11;
                                            partialMonth.S11 = "x";
                                        }
                                        break;
                                    case 12:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S12 = partialMonth.S12;
                                            partialMonth.S12 = "x";
                                        }
                                        break;
                                    case 13:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S13 = partialMonth.S13;
                                            partialMonth.S13 = "x";
                                        }
                                        break;
                                    case 14:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S14 = partialMonth.S14;
                                            partialMonth.S14 = "x";
                                        }
                                        break;
                                    case 15:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S15 = partialMonth.S15;
                                            partialMonth.S15 = "x";
                                        }
                                        break;
                                    case 16:
                                        if (d > fromDateDay)
                                        {
                                            NewRoaster.S16 = partialMonth.S16;
                                            partialMonth.S16 = "x";
                                        }
                                        break;
                                    case 17:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S17 = partialMonth.S17;
                                            partialMonth.S17 = "x";
                                        }
                                        break;
                                    case 18:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S18 = partialMonth.S18;
                                            partialMonth.S18 = "x";
                                        }
                                        break;
                                    case 19:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S19 = partialMonth.S19;
                                            partialMonth.S19 = "x";
                                        }
                                        break;
                                    case 20:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S20 = partialMonth.S20;
                                            partialMonth.S20 = "x";
                                        }
                                        break;
                                    case 21:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S21 = partialMonth.S21;
                                            partialMonth.S21 = "x";
                                        }
                                        break;
                                    case 22:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S22 = partialMonth.S22;
                                            partialMonth.S22 = "x";
                                        }
                                        break;
                                    case 23:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S23 = partialMonth.S23;
                                            partialMonth.S23 = "x";
                                        }
                                        break;
                                    case 24:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S24 = partialMonth.S24;
                                            partialMonth.S24 = "x";
                                        }
                                        break;
                                    case 25:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S25 = partialMonth.S25;
                                            partialMonth.S25 = "x";
                                        }
                                        break;
                                    case 26:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S26 = partialMonth.S26;
                                            partialMonth.S26 = "x";
                                        }
                                        break;
                                    case 27:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S27 = partialMonth.S27;
                                            partialMonth.S27 = "x";
                                        }
                                        break;
                                    case 28:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S28 = partialMonth.S28;
                                            partialMonth.S28 = "x";
                                        }
                                        break;
                                    case 29:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S29 = partialMonth.S29;
                                            partialMonth.S29 = "x";
                                        }
                                        break;
                                    case 30:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S30 = partialMonth.S30;
                                            partialMonth.S30 = "x";
                                        }
                                        break;
                                    case 31:
                                        if (d >= fromDateDay)
                                        {
                                            NewRoaster.S31 = partialMonth.S31;
                                            partialMonth.S31 = "x";
                                        }
                                        break;
                                    default:
                                        transaction.Rollback();
                                        return new() { Id = -1 };
                                }






                            }

                            if (partialMonth.Id > 0)
                            {
                                _context.TblOpMonthlyRoasterForSites.Update(partialMonth);
                                _context.SaveChanges();
                            }

                            bool IsRoasterExistInDestInPartialMonth = _context.TblOpMonthlyRoasterForSites.Any(r => r.Month == partialMonth.Month && r.Year == partialMonth.Year
                       && r.ProjectCode == PvTransferResourceReq.DestProjectCode
                       && r.SiteCode == PvTransferResourceReq.DestSiteCode
                      );

                            if (!IsRoasterExistInDestInPartialMonth)
                            {

                                transaction.Rollback();
                                return new() { Id = -10 };                 //Roaster For Partial Month Not Found in Destination

                            }
                            _context.TblOpMonthlyRoasterForSites.Add(NewRoaster);

                            _context.SaveChanges();
                        }




                        if (SrcProjectData.EndDate < DestProjectData.EndDate)
                        {
                            var ShiftsListMaster = _contextDMC.HRM_DEF_EmployeeShiftMasters.AsNoTracking();
                            var ShiftsForProjectSite = _context.TblOpShiftsPlanForProjects.AsNoTracking().Where(s => s.ProjectCode == partialMonth.ProjectCode
                            && s.SiteCode == partialMonth.SiteCode).ToList();

                            //var offShift = (from sfp in ShiftsForProjectSite
                            //                join sl in ShiftsListMaster on sfp.ShiftCode equals sl.ShiftCode
                            //         into t
                            //                from rt in t.DefaultIfEmpty()
                            //                orderby rt.ShiftId
                            //                select new
                            //                {
                            //                    IsOff = rt.IsOff,
                            //                    rt.ShiftId,
                            //                    rt.ShiftCode,
                            //                }).FirstOrDefault(e => e.IsOff.Value);
                            var offShift = ShiftsListMaster.SingleOrDefault(e => e.ShiftCode == "O");
                            string offShiftCode = offShift is not null ? offShift.ShiftCode : "-NA-";
                            string defaultShift = "";
                            int OffDay = -1;

                            if (roastersList[0].S1 != "" && roastersList[0].S1 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S1 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S1;
                                else if (OffDay == -1)
                                {
                                    OffDay = 1;
                                }

                            }
                            if (roastersList[0].S2 != "" && roastersList[0].S2 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S2 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S2;
                                else if (OffDay == -1)
                                {
                                    OffDay = 2;
                                }

                            }
                            if (roastersList[0].S3 != "" && roastersList[0].S3 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S3 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S3;
                                else if (OffDay == -1)
                                {
                                    OffDay = 3;
                                }

                            }
                            if (roastersList[0].S4 != "" && roastersList[0].S4 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S4 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S4;
                                else if (OffDay == -1)
                                {
                                    OffDay = 4;
                                }

                            }
                            if (roastersList[0].S5 != "" && roastersList[0].S5 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S5 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S5;
                                else if (OffDay == -1)
                                {
                                    OffDay = 5;
                                }

                            }
                            if (roastersList[0].S6 != "" && roastersList[0].S6 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S6 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S6;
                                else if (OffDay == -1)
                                {
                                    OffDay = 6;
                                }

                            }
                            if (roastersList[0].S7 != "" && roastersList[0].S7 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S7 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S7;
                                else if (OffDay == -1)
                                {
                                    OffDay = 7;
                                }

                            }

                            if (roastersList[0].S8 != "" && roastersList[0].S8 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S8 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S8;
                                else if (OffDay == -1)
                                {
                                    OffDay = 8;
                                }

                            }
                            if (roastersList[0].S9 != "" && roastersList[0].S9 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S9 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S9;
                                else if (OffDay == -1)
                                {
                                    OffDay = 9;
                                }

                            }


                            if (roastersList[0].S10 != "" && roastersList[0].S10 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S10 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S10;
                                else if (OffDay == -1)
                                {
                                    OffDay = 10;
                                }

                            }

                            if (roastersList[0].S11 != "" && roastersList[0].S11 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S11 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S11;
                                else if (OffDay == -1)
                                {
                                    OffDay = 11;
                                }

                            }

                            if (roastersList[0].S12 != "" && roastersList[0].S12 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S12 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S12;
                                else if (OffDay == -1)
                                {
                                    OffDay = 12;
                                }

                            }
                            if (roastersList[0].S13 != "" && roastersList[0].S13 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S13 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S13;
                                else if (OffDay == -1)
                                {
                                    OffDay = 13;
                                }

                            }
                            if (roastersList[0].S14 != "" && roastersList[0].S14 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S14 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S14;
                                else if (OffDay == -1)
                                {
                                    OffDay = 14;
                                }

                            }

                            if (roastersList[0].S15 != "" && roastersList[0].S15 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S15 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S15;
                                else if (OffDay == -1)
                                {
                                    OffDay = 15;
                                }

                            }
                            if (roastersList[0].S16 != "" && roastersList[0].S16 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S16 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S16;
                                else if (OffDay == -1)
                                {
                                    OffDay = 16;
                                }

                            }
                            if (roastersList[0].S17 != "" && roastersList[0].S17 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S17 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S17;
                                else if (OffDay == -1)
                                {
                                    OffDay = 17;
                                }

                            }
                            if (roastersList[0].S18 != "" && roastersList[0].S18 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S18 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S18;
                                else if (OffDay == -1)
                                {
                                    OffDay = 18;
                                }

                            }
                            if (roastersList[0].S19 != "" && roastersList[0].S19 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S19 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S19;
                                else if (OffDay == -1)
                                {
                                    OffDay = 19;
                                }

                            }
                            if (roastersList[0].S20 != "" && roastersList[0].S20 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S20 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S20;
                                else if (OffDay == -1)
                                {
                                    OffDay = 20;
                                }

                            }
                            if (roastersList[0].S21 != "" && roastersList[0].S21 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S21 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S21;
                                else if (OffDay == -1)
                                {
                                    OffDay = 21;
                                }

                            }
                            if (roastersList[0].S22 != "" && roastersList[0].S22 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S22 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S22;
                                else if (OffDay == -1)
                                {
                                    OffDay = 22;
                                }

                            }

                            if (roastersList[0].S23 != "" && roastersList[0].S23 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S23 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S23;
                                else if (OffDay == -1)
                                {
                                    OffDay = 23;
                                }

                            }
                            if (roastersList[0].S24 != "" && roastersList[0].S24 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S24 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S24;
                                else if (OffDay == -1)
                                {
                                    OffDay = 24;
                                }

                            }


                            if (roastersList[0].S25 != "" && roastersList[0].S25 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S25 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S25;
                                else if (OffDay == -1)
                                {
                                    OffDay = 25;
                                }

                            }

                            if (roastersList[0].S26 != "" && roastersList[0].S26 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S26 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S26;
                                else if (OffDay == -1)
                                {
                                    OffDay = 26;
                                }

                            }

                            if (roastersList[0].S27 != "" && roastersList[0].S27 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S27 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S27;
                                else if (OffDay == -1)
                                {
                                    OffDay = 27;
                                }

                            }
                            if (roastersList[0].S28 != "" && roastersList[0].S28 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S28 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S28;
                                else if (OffDay == -1)
                                {
                                    OffDay = 28;
                                }

                            }
                            if (roastersList[0].S29 != "" && roastersList[0].S29 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S29 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S29;
                                else if (OffDay == -1)
                                {
                                    OffDay = 29;
                                }

                            }

                            if (roastersList[0].S30 != "" && roastersList[0].S30 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S30 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S30;
                                else if (OffDay == -1)
                                {
                                    OffDay = 30;
                                }

                            }

                            if (roastersList[0].S31 != "" && roastersList[0].S31 != "x" && (defaultShift == "" || OffDay == -1))
                            {
                                if (roastersList[0].S31 != offShiftCode && defaultShift == "")
                                    defaultShift = roastersList[0].S31;
                                else if (OffDay == -1)
                                {
                                    OffDay = 31;
                                }

                            }


                            if (OffDay != -1)
                            {
                                DateTime dateValue = new DateTime(roastersList[0].Year, roastersList[0].Month, OffDay % 7 + 1);
                                OffDay = (int)dateValue.DayOfWeek;
                            }

                            if (defaultShift == "")
                            {
                                transaction.Rollback();
                                return new() { Id = -11 };
                            }

                            string SkillsetCode = _context.TblOpMonthlyRoasterForSites.AsNoTracking().FirstOrDefault(e => e.ProjectCode == PvTransferResourceReq.SrcProjectCode
                            && e.SiteCode == PvTransferResourceReq.SrcSiteCode
                            && e.Month == roastersList[0].Month
                            && e.Year == roastersList[0].Year).SkillsetCode;
                            List<TblOpMonthlyRoasterForSite> NewRoastersForDest = new();

                            if (SrcProjectData.EndDate <= DestProjectData.EndDate)
                            {
                                var roasterToDelete = _context.TblOpMonthlyRoasterForSites.FirstOrDefault(m => m.ProjectCode == PvTransferResourceReq.DestProjectCode
                                && m.SiteCode == PvTransferResourceReq.DestSiteCode
                                && m.EmployeeNumber == PvTransferResourceReq.EmployeeNumber
                                && m.Month == SrcProjectData.EndDate.Value.Month
                                && m.Year == SrcProjectData.EndDate.Value.Year);
                                if (roasterToDelete is not null)
                                {
                                    _context.TblOpMonthlyRoasterForSites.Remove(roasterToDelete);
                                    _context.SaveChanges();
                                }
                            }





                            for (DateTime date = SrcProjectData.EndDate.Value; date < DestProjectData.EndDate;)
                            {

                                short m = (short)date.Month;
                                short y = (short)date.Year;

                                var isRoasterExistForDestProj = _context.TblOpMonthlyRoasterForSites.AsNoTracking().Any(r => r.Month == m && r.Year == y

                                 && r.ProjectCode == PvTransferResourceReq.DestProjectCode
                                 && r.SiteCode == PvTransferResourceReq.DestSiteCode);


                                if (!isRoasterExistForDestProj)
                                {
                                    transaction.Rollback();
                                    return new() { Id = -9 };          //Roaster Incomplete or not found in Dest Project
                                }

                                int endDay = DateTime.DaysInMonth(y, m);
                                int startDay = (int)(new DateTime(y, m, 1)).DayOfWeek;




                                TblOpMonthlyRoasterForSite NewRoaster = new()
                                {
                                    Id = 0,
                                    EmployeeNumber = PvTransferResourceReq.EmployeeNumber,
                                    Month = m,
                                    Year = y,
                                    MonthEndDate = new DateTime(y, m, endDay),
                                    MonthStartDate = new DateTime(y, m, 1),
                                    SkillsetCode = SkillsetCode,
                                    CustomerCode = PvTransferResourceReq.DestCustomerCode,
                                    ProjectCode = PvTransferResourceReq.DestProjectCode,
                                    SiteCode = PvTransferResourceReq.DestSiteCode,
                                    MapId = 0,
                                    IsPrimaryResource = true,
                                    EmployeeID = 0,
                                    SkillsetName = "",
                                    S1 = new DateTime(y, m, 1) < PvTransferResourceReq.FromDate || new DateTime(y, m, 1) > resEndDate ? "x" : (startDay == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S2 = new DateTime(y, m, 2) < PvTransferResourceReq.FromDate || new DateTime(y, m, 2) > resEndDate ? "x" : ((startDay + 1) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S3 = new DateTime(y, m, 3) < PvTransferResourceReq.FromDate || new DateTime(y, m, 3) > resEndDate ? "x" : ((startDay + 2) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S4 = new DateTime(y, m, 4) < PvTransferResourceReq.FromDate || new DateTime(y, m, 4) > resEndDate ? "x" : ((startDay + 3) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S5 = new DateTime(y, m, 5) < PvTransferResourceReq.FromDate || new DateTime(y, m, 5) > resEndDate ? "x" : ((startDay + 4) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S6 = new DateTime(y, m, 6) < PvTransferResourceReq.FromDate || new DateTime(y, m, 6) > resEndDate ? "x" : ((startDay + 5) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S7 = new DateTime(y, m, 7) < PvTransferResourceReq.FromDate || new DateTime(y, m, 7) > resEndDate ? "x" : ((startDay + 6) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S8 = new DateTime(y, m, 8) < PvTransferResourceReq.FromDate || new DateTime(y, m, 8) > resEndDate ? "x" : ((startDay + 7) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S9 = new DateTime(y, m, 9) < PvTransferResourceReq.FromDate || new DateTime(y, m, 9) > resEndDate ? "x" : ((startDay + 8) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S10 = new DateTime(y, m, 10) < PvTransferResourceReq.FromDate || new DateTime(y, m, 10) > resEndDate ? "x" : ((startDay + 9) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S11 = new DateTime(y, m, 11) < PvTransferResourceReq.FromDate || new DateTime(y, m, 11) > resEndDate ? "x" : ((startDay + 10) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S12 = new DateTime(y, m, 12) < PvTransferResourceReq.FromDate || new DateTime(y, m, 12) > resEndDate ? "x" : ((startDay + 11) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S13 = new DateTime(y, m, 13) < PvTransferResourceReq.FromDate || new DateTime(y, m, 13) > resEndDate ? "x" : ((startDay + 12) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S14 = new DateTime(y, m, 14) < PvTransferResourceReq.FromDate || new DateTime(y, m, 14) > resEndDate ? "x" : ((startDay + 13) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S15 = new DateTime(y, m, 15) < PvTransferResourceReq.FromDate || new DateTime(y, m, 15) > resEndDate ? "x" : ((startDay + 14) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S16 = new DateTime(y, m, 16) < PvTransferResourceReq.FromDate || new DateTime(y, m, 16) > resEndDate ? "x" : ((startDay + 15) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S17 = new DateTime(y, m, 17) < PvTransferResourceReq.FromDate || new DateTime(y, m, 17) > resEndDate ? "x" : ((startDay + 16) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S18 = new DateTime(y, m, 18) < PvTransferResourceReq.FromDate || new DateTime(y, m, 18) > resEndDate ? "x" : ((startDay + 17) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S19 = new DateTime(y, m, 19) < PvTransferResourceReq.FromDate || new DateTime(y, m, 19) > resEndDate ? "x" : ((startDay + 18) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S20 = new DateTime(y, m, 20) < PvTransferResourceReq.FromDate || new DateTime(y, m, 20) > resEndDate ? "x" : ((startDay + 19) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S21 = new DateTime(y, m, 21) < PvTransferResourceReq.FromDate || new DateTime(y, m, 21) > resEndDate ? "x" : ((startDay + 20) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S22 = new DateTime(y, m, 22) < PvTransferResourceReq.FromDate || new DateTime(y, m, 22) > resEndDate ? "x" : ((startDay + 21) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S23 = new DateTime(y, m, 23) < PvTransferResourceReq.FromDate || new DateTime(y, m, 23) > resEndDate ? "x" : ((startDay + 22) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S24 = new DateTime(y, m, 24) < PvTransferResourceReq.FromDate || new DateTime(y, m, 24) > resEndDate ? "x" : ((startDay + 23) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S25 = new DateTime(y, m, 25) < PvTransferResourceReq.FromDate || new DateTime(y, m, 25) > resEndDate ? "x" : ((startDay + 24) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S26 = new DateTime(y, m, 26) < PvTransferResourceReq.FromDate || new DateTime(y, m, 26) > resEndDate ? "x" : ((startDay + 25) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S27 = new DateTime(y, m, 27) < PvTransferResourceReq.FromDate || new DateTime(y, m, 27) > resEndDate ? "x" : ((startDay + 26) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S28 = new DateTime(y, m, 28) < PvTransferResourceReq.FromDate || new DateTime(y, m, 28) > resEndDate ? "x" : ((startDay + 27) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S29 = endDay < 29 ? "x" : new DateTime(y, m, 29) < PvTransferResourceReq.FromDate || new DateTime(y, m, 29) > resEndDate ? "x" : ((startDay + 28) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S30 = endDay < 30 ? "x" : new DateTime(y, m, 30) < PvTransferResourceReq.FromDate || new DateTime(y, m, 30) > resEndDate ? "x" : ((startDay + 29) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                    S31 = endDay < 31 ? "x" : new DateTime(y, m, 31) < PvTransferResourceReq.FromDate || new DateTime(y, m, 31) > resEndDate ? "x" : ((startDay + 30) % 7 == OffDay) ? offShift.ShiftCode : defaultShift,
                                };
                                NewRoastersForDest.Add(NewRoaster);
                                if (m == 12)
                                {
                                    m = 1;
                                    y++;
                                }
                                else
                                {
                                    m++;

                                }
                                date = new DateTime(y, m, 1);
                            }

                            _context.TblOpMonthlyRoasterForSites.AddRange(NewRoastersForDest);

                            _context.SaveChanges();

                            List<HRM_DEF_EmployeeOff> offDays = new();
                         

                          

                            for (DateTime d = ((DateTime)SrcProjectData.EndDate).AddDays(1); d <= DestProjectData.EndDate; d = d.AddDays(1))
                            {
                                if ((int)d.DayOfWeek == OffDay)
                                {

                                    offDays.Add(new()
                                    {
                                        ID = 0,
                                        EmployeeId = emp.EmployeeID,
                                        Date = d,
                                        SiteCode = PvTransferResourceReq.DestSiteCode
                                    });
                                }
                            }
                            if (offDays.Count > 0)
                            {
                                _contextDMC.HRM_DEF_EmployeeOffs.AddRange(offDays);
                                _contextDMC.SaveChanges();
                            }


                        }









                       

                        var existOffDays = _contextDMC.HRM_DEF_EmployeeOffs.Where(e => e.SiteCode == PvTransferResourceReq.SrcSiteCode
                          && e.EmployeeId == emp.EmployeeID
                          && e.Date >= PvTransferResourceReq.FromDate && e.Date <= SrcProjectData.EndDate
                         ).ToList();

                        if (existOffDays.Count > 0)
                        {
                            for (int i = 0; i < existOffDays.Count; i++)
                            {
                                existOffDays[i].SiteCode = PvTransferResourceReq.DestSiteCode;

                            }
                            _contextDMC.HRM_DEF_EmployeeOffs.UpdateRange(existOffDays);
                            _contextDMC.SaveChanges();

                        }



                        var EmpMapInProjectSite = await _context.TblOpEmployeesToProjectSiteList.FirstOrDefaultAsync(e => e.EmployeeNumber == PvTransferResourceReq.EmployeeNumber && e.SiteCode == PvTransferResourceReq.SrcSiteCode && e.ProjectCode == PvTransferResourceReq.SrcProjectCode);

                        if (EmpMapInProjectSite is not null)
                        {
                            EmpMapInProjectSite.ProjectCode = PvTransferResourceReq.DestProjectCode;
                            EmpMapInProjectSite.SiteCode = PvTransferResourceReq.DestSiteCode;
                            _context.TblOpEmployeesToProjectSiteList.Update(EmpMapInProjectSite);
                            await _context.SaveChangesAsync();

                        }



                        bool isEmployeeTransferLogExist = await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AnyAsync(e => e.EmployeeNumber == PvTransferResourceReq.EmployeeNumber
                        && e.TransferredDate <=PvTransferResourceReq.FromDate);

                        if (!isEmployeeTransferLogExist)
                        {
                            transaction.Rollback();
                            transaction2.Rollback();
                            return new() { Id = -13 };
                        }
                        else
                        {
                            var primarySite = _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AsNoTracking().OrderByDescending(o => o.TransferredDate).FirstOrDefault(e => e.EmployeeNumber == PvTransferResourceReq.EmployeeNumber
                           && e.TransferredDate <= PvTransferResourceReq.FromDate);
                            bool isSourceSitePrimarySite = primarySite.SiteCode == PvTransferResourceReq.SrcSiteCode;
                            if (isSourceSitePrimarySite)
                            {
                                HRM_TRAN_EmployeePrimarySites_Log logData = new()
                                {
                                    CreatedBy = request.User.UserId,
                                    CreatedDate = DateTime.UtcNow,
                                    SiteCode = PvTransferResourceReq.DestSiteCode,
                                    EmployeeNumber = PvTransferResourceReq.EmployeeNumber,
                                    TransferredDate = PvTransferResourceReq.FromDate


                                };
                                await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AddAsync(logData);
                                await _contextDMC.SaveChangesAsync();

                            }
                        }


                        PvTransferResourceReq.IsMerged = true;
                        PvTransferResourceReq.IsApproved = true;
                        PvTransferResourceReq.ApprovedBy = request.User.UserId;
                        PvTransferResourceReq.Modified = DateTime.UtcNow;
                        _context.TblOpPvTransferResourceReqs.Update(PvTransferResourceReq);
                        _context.SaveChanges();



                        //Removing Non Primary Roasters for replaced Employee
                        var NonPrimaryRoasters = await _context.TblOpMonthlyRoasterForSites.AsNoTracking().Where(e =>
                        e.ProjectCode == PvTransferResourceReq.DestProjectCode && e.SiteCode == PvTransferResourceReq.DestSiteCode && e.EmployeeNumber == PvTransferResourceReq.EmployeeNumber
                        && e.MonthEndDate >= PvTransferResourceReq.FromDate
                        && e.IsPrimaryResource == false).ToListAsync();

                        if (NonPrimaryRoasters.Count > 0)
                        {
                            _context.TblOpMonthlyRoasterForSites.RemoveRange(NonPrimaryRoasters);
                            await _context.SaveChangesAsync();
                        }








                        transaction.Commit();
                        transaction2.Commit();

                        return new() { Id = request.Id };





                    }
                    catch (Exception ex)
                    {

                        Log.Error("Error in ApproveReqPvTransferResourceReqByIdHandler Method");
                        Log.Error("Error occured time : " + DateTime.UtcNow);
                        Log.Error("Error message : " + ex.Message);
                        Log.Error("Error StackTrace : " + ex.StackTrace);
                        transaction.Rollback();
                        transaction2.Rollback();
                        return new() { Id = 0 };
                    }
                }
            }
        }
    }

    #endregion



}






