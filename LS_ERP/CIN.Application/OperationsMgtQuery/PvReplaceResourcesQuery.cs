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



    #region CreateUpdatePvReplaceResourceReq
    public class CreatePvReplaceResourceReq : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public TblOpPvReplaceResourceReqDto PvReplaceResourceReqDto { get; set; }
    }

    public class CreatePvReplaceResourceReqHandler : IRequestHandler<CreatePvReplaceResourceReq, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreatePvReplaceResourceReqHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<long> Handle(CreatePvReplaceResourceReq request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdatePvReplaceResourceReq method start----");




                    var obj = request.PvReplaceResourceReqDto;


                    TblOpPvReplaceResourceReq ReqHead = new();
                    if (obj.Id > 0)

                        ReqHead = await _context.TblOpPvReplaceResourceReqs.FirstOrDefaultAsync(e => e.Id == obj.Id);
                 
                        ReqHead.IsActive = obj.IsActive;
                        ReqHead.CustomerCode = obj.CustomerCode;
                        ReqHead.ProjectCode = obj.ProjectCode;
                        ReqHead.SiteCode = obj.SiteCode;
                        ReqHead.ResignedEmployeeNumber = obj.ResignedEmployeeNumber;
                        ReqHead.ReplacedEmployeeNumber = obj.ReplacedEmployeeNumber;
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
                        _context.TblOpPvReplaceResourceReqs.Update(ReqHead);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ReqHead.CreatedBy = request.User.UserId;

                        ReqHead.Created = DateTime.Now;
                        await _context.TblOpPvReplaceResourceReqs.AddAsync(ReqHead);
                        await _context.SaveChangesAsync();
                    }
                    

                    
                      
                    
                    Log.Info("----Info CreateUpdatePvReplaceResourceReq method Exit----");

                    await transaction.CommitAsync();
                    return ReqHead.Id;

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdatePvReplaceResourceReq Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;

                }
            }    
        }
        
    }

    #endregion
    #region GetPvReplaceResourceReqsPagedList

    public class GetPvReplaceResourceReqsPagedList : IRequest<PaginatedList<TblOpPvReplaceResourceReqsPaginationDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetPvReplaceResourceReqsPagedListHandler : IRequestHandler<GetPvReplaceResourceReqsPagedList, PaginatedList<TblOpPvReplaceResourceReqsPaginationDto>>
    {
        private readonly CINDBOneContext _context;
    
        private readonly IMapper _mapper;
        public GetPvReplaceResourceReqsPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
           
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblOpPvReplaceResourceReqsPaginationDto>> Handle(GetPvReplaceResourceReqsPagedList request, CancellationToken cancellationToken)
        {
            try {
                bool isAdmin = _context.SystemLogins.AsNoTracking().FirstOrDefault(e => e.Id == request.User.UserId).LoginId == "Admin" ? true : false;
                var oprAuths = _context.TblOpAuthoritiesList.AsNoTracking();
            var Projects = _context.OP_HRM_TEMP_Projects.AsNoTracking();
           
            var Sites = _context.OprSites.AsNoTracking();
            var search = request.Input.Query;

            var list = await _context.TblOpPvReplaceResourceReqs.AsNoTracking()

               .OrderBy(request.Input.OrderBy).Select(d => new TblOpPvReplaceResourceReqsPaginationDto
               {
                   Id = d.Id,
                   CustomerCode = d.CustomerCode,
                   SiteCode = d.SiteCode,
                   SiteName = Sites.FirstOrDefault(s => s.SiteCode == d.SiteCode).SiteName,
                   SiteNameAr = Sites.FirstOrDefault(s => s.SiteCode == d.SiteCode).SiteArbName,
                   ProjectCode = d.ProjectCode,
                   ProjectName = Projects.FirstOrDefault(p => p.ProjectCode == d.ProjectCode).ProjectNameEng,
                   ProjectNameAr = Projects.FirstOrDefault(p => p.ProjectCode == d.ProjectCode).ProjectNameArb,
                   ResignedEmployeeNumber = d.ResignedEmployeeNumber,
                   ReplacedEmployeeNumber = d.ReplacedEmployeeNumber,
                   FromDate = d.FromDate,
                   IsActive = d.IsActive,
                   IsApproved = d.IsApproved,
                  CanEditReq=d.CreatedBy==request.User.UserId,
                  CanApproveReq=oprAuths.Any(a=>a.AppAuth==request.User.UserId&&a.CanApprovePvReq && a.BranchCode== Sites.FirstOrDefault(s=>s.SiteCode==d.SiteCode).SiteCityCode)||isAdmin
                  , ApprovedBy=d.ApprovedBy,
                   IsAdmin=isAdmin,
                    IsMerged=d.IsApproved

               }).Where(e => //e.CompanyId == request.CompanyId &&
                            (e.CustomerCode.Contains(search) ||
                            e.SiteCode.Contains(search) ||
                            e.SiteName.Contains(search) ||
                            e.SiteNameAr.Contains(search) ||
                            e.ProjectCode.Contains(search) ||
                            e.ProjectName.Contains(search) ||
                            e.ProjectNameAr.Contains(search) ||
                            e.ResignedEmployeeNumber.Contains(search) ||
                            e.ReplacedEmployeeNumber.Contains(search) ||
                            search == "" || search == null
                             ) &&(e.CanEditReq||e.CanApproveReq))
                 //.ProjectTo<TblOpPvReplaceResourceReqsPaginationDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;

        }
         catch (Exception ex)
                {
                  
        Log.Error("Error in GetPvReplaceResourceReqsPagedList Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
}
        }
    }
    #endregion

    #region GetPvReplaceResourceReqById
    public class GetPvReplaceResourceReqById : IRequest<TblOpPvReplaceResourceReqDto>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }

    public class GetPvReplaceResourceReqByIdHandler : IRequestHandler<GetPvReplaceResourceReqById, TblOpPvReplaceResourceReqDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPvReplaceResourceReqByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpPvReplaceResourceReqDto> Handle(GetPvReplaceResourceReqById request, CancellationToken cancellationToken)
        {
            try
            {

                var PvReplaceResourceReq = await _context.TblOpPvReplaceResourceReqs.ProjectTo<TblOpPvReplaceResourceReqDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);


                return PvReplaceResourceReq;





            }
            catch (Exception ex)
            {
                Log.Error("Error in GetPvReplaceResourceReqByIdHandler Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion 


    #region ApproveReqPvReplaceResourceReqById
    public class ApproveReqPvReplaceResourceReqById : IRequest<TblOpPvReplaceResourceReqDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class ApproveReqPvReplaceResourceReqByIdHandler : IRequestHandler<ApproveReqPvReplaceResourceReqById, TblOpPvReplaceResourceReqDto>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public ApproveReqPvReplaceResourceReqByIdHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<TblOpPvReplaceResourceReqDto> Handle(ApproveReqPvReplaceResourceReqById request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                using (var transaction2 = await _contextDMC.Database.BeginTransactionAsync())
                {

                    try
                    {
                        Log.Info("----Info ApproveReqPvReplaceResourceReqById method start----");

                        TblOpPvReplaceResourceReq PvReplaceResourceReq = _context.TblOpPvReplaceResourceReqs.AsNoTracking().FirstOrDefault(e => e.Id == request.Id);
                

                        var roastersList = _context.TblOpMonthlyRoasterForSites.AsNoTracking().Where(m => m.MonthEndDate >= PvReplaceResourceReq.FromDate
                         && m.EmployeeNumber == PvReplaceResourceReq.ResignedEmployeeNumber
                         && m.ProjectCode == PvReplaceResourceReq.ProjectCode
                         && m.SiteCode == PvReplaceResourceReq.SiteCode).ToList();
                        if (roastersList.Count == 0)
                            return new() { Id = -4 };  //No Replacements Found ,Resigned Employee not having roaster 


                       


                        TblOpMonthlyRoasterForSite partialMonth = new();
                        List<TblOpMonthlyRoasterForSite> reaminingMonths = new();

                      
                        roastersList.ForEach(r =>
                        {
                       
                            if (r.MonthStartDate.Value.Month == PvReplaceResourceReq.FromDate.Value.Month && r.MonthStartDate.Value.Year == PvReplaceResourceReq.FromDate.Value.Year && r.MonthStartDate.Value.Day != PvReplaceResourceReq.FromDate.Value.Day)
                            {

                                partialMonth = r;

                            }
                            else
                            {
                                reaminingMonths.Add(r);
                            }

                        });



                        reaminingMonths.ForEach(r =>
                        {

                            r.EmployeeNumber = PvReplaceResourceReq.ReplacedEmployeeNumber;



                        });
                        _context.TblOpMonthlyRoasterForSites.UpdateRange(reaminingMonths);
                        _context.SaveChanges();

                        if (partialMonth.Id != 0)
                        {
                            TblOpMonthlyRoasterForSite NewRoaster = new();
                            NewRoaster.Id = 0;
                            NewRoaster.EmployeeNumber = PvReplaceResourceReq.ReplacedEmployeeNumber;
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
                            NewRoaster.CustomerCode = partialMonth.CustomerCode;
                            NewRoaster.ProjectCode = partialMonth.ProjectCode;
                            NewRoaster.SiteCode = partialMonth.SiteCode;
                            NewRoaster.SkillsetCode = partialMonth.SkillsetCode;
                            NewRoaster.SkillsetName = partialMonth.SkillsetName;

                            var fromDateDay = PvReplaceResourceReq.FromDate.Value.Day;

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
                                        if (d >= fromDateDay)
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
                            _context.TblOpMonthlyRoasterForSites.Add(NewRoaster);
                            _context.SaveChanges();


                            _context.TblOpMonthlyRoasterForSites.Update(partialMonth);
                            _context.SaveChanges();


                        }




                    


                        PvReplaceResourceReq.IsMerged = true;
                        PvReplaceResourceReq.IsApproved = true;
                        PvReplaceResourceReq.ApprovedBy = request.User.UserId;
                        PvReplaceResourceReq.Modified = DateTime.UtcNow;
                        _context.TblOpPvReplaceResourceReqs.Update(PvReplaceResourceReq);
                        _context.SaveChanges();

                        var PrimarySiteForResignedEmployee = await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AsNoTracking().OrderByDescending(e => e.TransferredDate).FirstOrDefaultAsync(e=>e.EmployeeNumber==PvReplaceResourceReq.ResignedEmployeeNumber 
                        && e.TransferredDate<=PvReplaceResourceReq.FromDate);

                        if (PrimarySiteForResignedEmployee is not null)
                        {
                            if (PrimarySiteForResignedEmployee.SiteCode==PvReplaceResourceReq.SiteCode)
                            {
                                HRM_TRAN_EmployeePrimarySites_Log siteLog = new()
                                {
                                    EmployeeNumber = PvReplaceResourceReq.ResignedEmployeeNumber,
                                    SiteCode = "0000000000",
                                    TransferredDate = PvReplaceResourceReq.FromDate,
                                    CreatedBy = request.User.UserId,
                                    CreatedDate = DateTime.UtcNow
                                };

                                await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AddAsync(siteLog);
                                await _contextDMC.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            await transaction.RollbackAsync();
                            await transaction2.RollbackAsync();
                            return new() { Id=-8};      // Primary Site log not exist for resigned Employee
                        
                        }

                        //Removing Non Primary Roasters for replaced Employee
                        var NonPrimaryRoasters = await _context.TblOpMonthlyRoasterForSites.AsNoTracking().Where(e=>
                        e.ProjectCode == PvReplaceResourceReq.ProjectCode && e.SiteCode == PvReplaceResourceReq.SiteCode 
                        && e.EmployeeNumber == PvReplaceResourceReq.ReplacedEmployeeNumber
                        && e.MonthEndDate >= PvReplaceResourceReq.FromDate
                        &&e.IsPrimaryResource==false).ToListAsync();

                        if (NonPrimaryRoasters.Count>0)
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
                        Log.Error("Error in ApproveReqPvReplaceResourceReqByIdHandler Method");
                        Log.Error("Error occured time : " + DateTime.UtcNow);
                        Log.Error("Error message : " + ex.Message);
                        Log.Error("Error StackTrace : " + ex.StackTrace);
                       await transaction.RollbackAsync();
                        await transaction2.RollbackAsync();
                        return new() { Id = -1 };
                    }
                }
            }
        }
    }

    #endregion


    #region DeletePvReplaceResourceReqById
    public class DeletePvReplaceResourceReqById : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeletePvReplaceResourceReqByIdHandler : IRequestHandler<DeletePvReplaceResourceReqById, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public DeletePvReplaceResourceReqByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<long> Handle(DeletePvReplaceResourceReqById request, CancellationToken cancellationToken)
        {
           try
                {

                    var PvReplaceResourceReq = await _context.TblOpPvReplaceResourceReqs.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
                    _context.TblOpPvReplaceResourceReqs.Remove(PvReplaceResourceReq);
                   
                    _context.SaveChanges();
                return request.Id;

                }
                catch (Exception ex)
                {
                   
                    Log.Error("Error in DeletePvReplaceResourceReqByIdHandler Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    #endregion



    #region IsValidReplaceResourceRequest
    public class IsValidReplaceResourceRequest : IRequest<ValidityCheckDto>
    {
        public UserIdentityDto User { get; set; }
        public TblOpPvReplaceResourceReqDto InputDto { get; set; }
    }

    public class IsValidReplaceResourceRequestHandler : IRequestHandler<IsValidReplaceResourceRequest, ValidityCheckDto>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public IsValidReplaceResourceRequestHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<ValidityCheckDto> Handle(IsValidReplaceResourceRequest request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {



                try
                {
                    Log.Info("----Info IsValidReplaceResourceRequest method start----");

                    if (request.InputDto.Id > 0 && request.InputDto.IsApproved)
                    {
                        return new() { IsValidReq = false, ErrorId = -1, ErrorMsg = "Request Already Approved" };

                    }

                    if (!await _context.OP_HRM_TEMP_Projects.AnyAsync(e => e.ProjectCode == request.InputDto.ProjectCode))
                    {
                        return new() { IsValidReq = false, ErrorId = -2, ErrorMsg = "Invalid ProjectCode" };
                    }
                    if (!await _context.OprSites.AnyAsync(e => e.SiteCode == request.InputDto.SiteCode))
                    {
                        return new() { IsValidReq = false, ErrorId = -3, ErrorMsg = "Invalid SIteCode" };
                    }
                     if (!await _contextDMC.HRM_TRAN_Employees.AnyAsync(e => e.EmployeeNumber == request.InputDto.ReplacedEmployeeNumber))
                    {
                        return new() { IsValidReq = false, ErrorId = -4, ErrorMsg = "Invalid Employee Number:"+ request.InputDto.ReplacedEmployeeNumber };
                    }

                     if (!await _contextDMC.HRM_TRAN_Employees.AnyAsync(e => e.EmployeeNumber == request.InputDto.ResignedEmployeeNumber))
                    {
                        return new() { IsValidReq = false, ErrorId = -5, ErrorMsg = "Invalid Employee Number:"+ request.InputDto.ResignedEmployeeNumber };
                    }

                 

                    if (!(await _context.TblOpMonthlyRoasterForSites.AsNoTracking().AnyAsync(e => e.EmployeeNumber == request.InputDto.ResignedEmployeeNumber && e.ProjectCode == request.InputDto.ProjectCode && e.SiteCode == request.InputDto.SiteCode && e.MonthEndDate >= request.InputDto.FromDate && e.IsPrimaryResource)))
                    {
                        return new() { IsValidReq = false, ErrorId = -6, ErrorMsg = "Employee:"+ request.InputDto.ResignedEmployeeNumber+" Not Found in the Project:"+ request.InputDto.ProjectCode };
                    }

                    var LastRoasterForResignedEmployee = await _context.TblOpMonthlyRoasterForSites.AsNoTracking().OrderByDescending(e=>e.MonthEndDate).FirstOrDefaultAsync(e => e.EmployeeNumber == request.InputDto.ResignedEmployeeNumber && e.ProjectCode == request.InputDto.ProjectCode && e.SiteCode == request.InputDto.SiteCode && e.MonthEndDate >= request.InputDto.FromDate && e.IsPrimaryResource);



                    if (await _context.TblOpMonthlyRoasterForSites.AsNoTracking().AnyAsync(e => e.EmployeeNumber == request.InputDto.ReplacedEmployeeNumber && e.ProjectCode == request.InputDto.ProjectCode && e.SiteCode == request.InputDto.SiteCode && e.MonthEndDate >= request.InputDto.FromDate && e.IsPrimaryResource && e.MonthEndDate<=LastRoasterForResignedEmployee.MonthEndDate))
                    {
                       var LastRoasterForReplaceExist= await _context.TblOpMonthlyRoasterForSites.AsNoTracking().OrderByDescending(e=>e.MonthEndDate).FirstOrDefaultAsync(e => e.EmployeeNumber == request.InputDto.ReplacedEmployeeNumber && e.ProjectCode == request.InputDto.ProjectCode && e.SiteCode == request.InputDto.SiteCode && e.MonthEndDate >= request.InputDto.FromDate && e.IsPrimaryResource && e.MonthEndDate <= LastRoasterForResignedEmployee.MonthEndDate);
                        return new() { IsValidReq = false, ErrorId = -7, ErrorMsg = "Employee:" + request.InputDto.ReplacedEmployeeNumber + " Already Exist in the Project:" + request.InputDto.ProjectCode+", Year:"+ LastRoasterForReplaceExist.Year+",Month:"+ LastRoasterForReplaceExist.Month };

                    }


                    if (await _context.EmployeeAttendance.AnyAsync(e => e.EmployeeNumber == request.InputDto.ResignedEmployeeNumber && e.ProjectCode == request.InputDto.ProjectCode && e.SiteCode == request.InputDto.SiteCode && e.AttnDate >= request.InputDto.FromDate))
                    {
                        var attendanceExist = await _context.EmployeeAttendance.OrderByDescending(e => e.AttnDate).FirstOrDefaultAsync(e => e.EmployeeNumber == request.InputDto.ResignedEmployeeNumber && e.ProjectCode == request.InputDto.ProjectCode && e.SiteCode == request.InputDto.SiteCode && e.AttnDate >= request.InputDto.FromDate);
                        return new() { IsValidReq = false, ErrorId = -8, ErrorMsg = "Employee Attendance Exist, Employee Number:" + request.InputDto.ResignedEmployeeNumber + ", Date:" + attendanceExist.AttnDate.ToString() };

                    }

                     if (await _context.EmployeeAttendance.AnyAsync(e => e.EmployeeNumber == request.InputDto.ReplacedEmployeeNumber && e.ProjectCode == request.InputDto.ProjectCode && e.SiteCode == request.InputDto.SiteCode && e.AttnDate >= request.InputDto.FromDate))
                    {
                        var attendanceExist = await _context.EmployeeAttendance.OrderByDescending(e => e.AttnDate).FirstOrDefaultAsync(e => e.EmployeeNumber == request.InputDto.ResignedEmployeeNumber && e.ProjectCode == request.InputDto.ProjectCode && e.SiteCode == request.InputDto.SiteCode && e.AttnDate >= request.InputDto.FromDate);
                        return new() { IsValidReq = false, ErrorId = -9, ErrorMsg = "Employee Attendance Exist, Employee Number:" + request.InputDto.ReplacedEmployeeNumber + ", Date:" + attendanceExist.AttnDate.ToString() };

                    }




                   

                    bool isEmployeeTransferLogExistForResignedEMp = await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AnyAsync(e => e.EmployeeNumber == request.InputDto.ResignedEmployeeNumber
                    &&e.TransferredDate<=request.InputDto.FromDate);

                    if (!isEmployeeTransferLogExistForResignedEMp)
                    {
                        return new() { IsValidReq = false, ErrorId = -10, ErrorMsg = "Primary site log not exist for Employee" + request.InputDto.ResignedEmployeeNumber };

                    }
                       
                    bool isEmployeeTransferLogExistForReplacedEMp = await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AnyAsync(e => e.EmployeeNumber == request.InputDto.ReplacedEmployeeNumber
                    && e.TransferredDate <= request.InputDto.FromDate);

                    if (!isEmployeeTransferLogExistForReplacedEMp)
                    {
                        return new() { IsValidReq = false, ErrorId = -11, ErrorMsg = "Primary site log not exist for Employee" + request.InputDto.ReplacedEmployeeNumber };

                    }


                   

                    return new() { IsValidReq = true };


                    

                }
                catch (Exception ex)
                {

                    Log.Error("Error in IsValidReplaceResourceRequestHandler Method");
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






