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



    #region CreatePvOpenCloseReq
    public class CreatePvOpenCloseReq : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public TblOpPvOpenCloseReqDto PvOpenCloseReqDto { get; set; }
    }

    public class CreatePvOpenCloseReqHandler : IRequestHandler<CreatePvOpenCloseReq, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreatePvOpenCloseReqHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<long> Handle(CreatePvOpenCloseReq request, CancellationToken cancellationToken)
        {
           
                try
                {
                    Log.Info("----Info CreatePvOpenCloseReq method start----");




                    var obj = request.PvOpenCloseReqDto;


                    TblOpPvOpenCloseReq Req = new();
                if (obj.Id > 0)
                {
                    Req = await _context.TblOpPvOpenCloseReqs.FirstOrDefaultAsync(e => e.Id == obj.Id);
                }
                else
                {
                    var existReq = await _context.TblOpPvOpenCloseReqs.FirstOrDefaultAsync(e => e.ProjectCode == obj.ProjectCode &&
                     e.SiteCode == obj.SiteCode
                     && e.CustomerCode == obj.CustomerCode
                     && !e.IsApproved);

                    if (existReq is not null)
                    {
                        return -1;
                    }
                    Req = new() { Id = 0 };
                }

                   
                       
                        Req.CustomerCode = obj.CustomerCode;
                        Req.ProjectCode = obj.ProjectCode;
                        Req.SiteCode = obj.SiteCode;

                        Req.IsCancelReq = obj.IsCancelReq;
                        Req.IsCloseReq = obj.IsCloseReq;
                        Req.IsExtendProjReq = obj.IsExtendProjReq;
                        Req.IsRevokeSuspReq = obj.IsRevokeSuspReq;
                        Req.IsReOpenReq = obj.IsReOpenReq;
                        Req.IsSuspendReq = obj.IsSuspendReq;


                        Req.IsApproved = false;
                        Req.ApprovedBy = 0;
                        Req.EffectiveDate = obj.EffectiveDate;
                        Req.ExtensionDate = obj.ExtensionDate;


                    if (obj.Id > 0)
                    {
                        Req.Modified = DateTime.UtcNow;
                        Req.ModifiedBy = request.User.UserId;
                        _context.TblOpPvOpenCloseReqs.Update(Req);
                        await _context.SaveChangesAsync();

                    }
                    else {

                        Req.Created = DateTime.UtcNow;
                        Req.CreatedBy = request.User.UserId;
                       await _context.TblOpPvOpenCloseReqs.AddAsync(Req);
                        await _context.SaveChangesAsync();
                    }


                    return Req.Id;






                }
                catch (Exception ex)
                {
                   
                    Log.Error("Error in CreatePvOpenCloseReq Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;

                }
                    }
        
    }

    #endregion

    #region GetPvOpenCloseReqsPagedList

    public class GetPvOpenCloseReqsPagedList : IRequest<PaginatedList<TblOpPvOpenCloseReqsPaginationDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetPvOpenCloseReqsPagedListHandler : IRequestHandler<GetPvOpenCloseReqsPagedList, PaginatedList<TblOpPvOpenCloseReqsPaginationDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPvOpenCloseReqsPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TblOpPvOpenCloseReqsPaginationDto>> Handle(GetPvOpenCloseReqsPagedList request, CancellationToken cancellationToken)
        {
            bool isAdmin = _context.SystemLogins.AsNoTracking().FirstOrDefault(e => e.Id == request.User.UserId).LoginId == "Admin" ? true : false;

            var oprAuths = _context.TblOpAuthoritiesList.AsNoTracking();
            var Projects = _context.OP_HRM_TEMP_Projects.AsNoTracking();

            var Sites = _context.OprSites.AsNoTracking();
            var search = request.Input.Query;
            var list = await _context.TblOpPvOpenCloseReqs.AsNoTracking()
               .OrderBy(request.Input.OrderBy).Select(d => new TblOpPvOpenCloseReqsPaginationDto
               {
                   Id = d.Id,
                   CustomerCode = d.CustomerCode,
                   SiteCode = d.SiteCode,
                   SiteName = Sites.FirstOrDefault(s => s.SiteCode == d.SiteCode).SiteName,
                   SiteNameAr = Sites.FirstOrDefault(s => s.SiteCode == d.SiteCode).SiteArbName,
                   ProjectCode = d.ProjectCode,
                   ProjectName = Projects.FirstOrDefault(p => p.ProjectCode == d.ProjectCode).ProjectNameEng,
                   ProjectNameAr = Projects.FirstOrDefault(p => p.ProjectCode == d.ProjectCode).ProjectNameArb,
                   EffectiveDate=d.EffectiveDate,
                   ExtensionDate=d.ExtensionDate,
                   IsCancelReq=d.IsCancelReq,
                   IsCloseReq=d.IsCloseReq,
                   IsExtendProjReq=d.IsExtendProjReq,
                   IsReOpenReq=d.IsReOpenReq,
                   IsRevokeSuspReq=d.IsRevokeSuspReq,
                   IsSuspendReq=d.IsSuspendReq,
                   ApprovedBy=d.ApprovedBy,
                    Created=d.Created,
                    CreatedBy=d.CreatedBy,
                     
                   IsApproved = d.IsApproved,

                   ReqType= d.IsCancelReq?"Cancel":d.IsSuspendReq?"Suspend_Project":d.IsRevokeSuspReq?"Revoke_Suspension":d.IsCloseReq?"Close_Project_Site":d.IsExtendProjReq?"Extend_Project_Site":d.IsReOpenReq?"Reopen_Project_Site":"",
                   RequestSubType=d.IsRevokeSuspReq? "Revoke_Project_Suspension_Request": d.IsCancelReq? "Project_Cancel_Request" : d.IsSuspendReq? "Project_Suspension_Request" :d.IsCloseReq? "Project_Closing_Request" : d.IsExtendProjReq? "Project_Extend_Request" : d.IsReOpenReq? "Project_Reopen_Request" : "",
                   CanEditReq=d.CreatedBy==request.User.UserId,
                   CanApproveReq= oprAuths.Any(a=>a.AppAuth==request.User.UserId&&a.CanApprovePvReq &&a.BranchCode== Sites.FirstOrDefault(s=>s.SiteCode==d.SiteCode).SiteCityCode)|| isAdmin,
                    IsAdmin=isAdmin,
                   RequestNumber=d.Id,
                   FileUploadBy = d.FileUploadBy,
                   FileUrl = d.FileUrl,
                   IsFileUploadRequired = false,
               })
              .Where(e =>
                            (e.CustomerCode.Contains(search) ||
                            e.SiteCode.Contains(search) ||
                            e.ProjectCode.Contains(search) ||
                            e.SiteName.Contains(search) ||
                            e.SiteNameAr.Contains(search) ||
                            e.ProjectName.Contains(search) ||
                            e.ProjectNameAr.Contains(search) ||
                           
                            search == "" || search == null
                             ) && (e.CanEditReq || e.CanApproveReq))

               .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion

    #region DeletePvOpenCloseReqById
    public class DeletePvOpenCloseReqById : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class DeletePvOpenCloseReqByIdHandler : IRequestHandler<DeletePvOpenCloseReqById, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public DeletePvOpenCloseReqByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<long> Handle(DeletePvOpenCloseReqById request, CancellationToken cancellationToken)
        {
          
                try
                {

                    var PvOpenCloseReq = await _context.TblOpPvOpenCloseReqs.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);





                    _context.TblOpPvOpenCloseReqs.Remove(PvOpenCloseReq);

                    _context.SaveChanges();

                return 1;

                }
                catch (Exception ex)
                {
                  
                    Log.Error("Error in DeletePvOpenCloseReqByIdHandler Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }


    #endregion

    #region ApproveReqPvOpenCloseReqById
    public class ApproveReqPvOpenCloseReqById : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }

    public class ApproveReqPvOpenCloseReqByIdHandler : IRequestHandler<ApproveReqPvOpenCloseReqById, long>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public ApproveReqPvOpenCloseReqByIdHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<long> Handle(ApproveReqPvOpenCloseReqById request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                using (var transaction2 = await _contextDMC.Database.BeginTransactionAsync())
                {

                    try
                    {
                        Log.Info("----Info ApproveReqPvOpenCloseReqById method start----");




                        var PvOpenCloseReq = await _context.TblOpPvOpenCloseReqs.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
                        if (PvOpenCloseReq is null)
                        {
                            return -1;       // Request_not_exist
                        }
                        if (PvOpenCloseReq.IsApproved)
                        {
                            return -2;      //Request_Already_Approved
                        }
                        var projectData = await _context.OP_HRM_TEMP_Projects.AsNoTracking().FirstOrDefaultAsync(p => p.ProjectCode == PvOpenCloseReq.ProjectCode);
                        if (projectData is null)
                        {
                            return -3;      //invalid_projec_code
                        }
                        var projectSite = await _context.TblOpProjectSites.AsNoTracking().FirstOrDefaultAsync(s => s.SiteCode == PvOpenCloseReq.SiteCode && s.ProjectCode == PvOpenCloseReq.ProjectCode);
                        if (projectSite is null)
                        {
                            return -4;      //invalid_Site_code
                        }
                        if (PvOpenCloseReq.IsSuspendReq)                //Approving Suspension Request
                        {
                            projectSite.IsSuspended = true;
                            projectSite.IsClosed = false;
                            projectSite.IsInActive = true;
                            projectSite.IsCancelled = false;


                            projectSite.ActualEndDate = projectSite.ActualEndDate is null ? projectSite.EndDate : projectSite.ActualEndDate;
                            projectSite.EndDate = PvOpenCloseReq.EffectiveDate;

                            _context.TblOpProjectSites.Update(projectSite);
                            await _context.SaveChangesAsync();

                        }
                        else if (PvOpenCloseReq.IsCloseReq)
                        {
                            return -102;// closing blocked

                            projectSite.IsSuspended = false;
                            projectSite.IsClosed = true;
                            projectSite.IsCancelled = false;
                            projectSite.IsInActive = true;

                            projectSite.ActualEndDate = projectSite.ActualEndDate is null ? projectSite.EndDate : projectSite.ActualEndDate;
                            projectSite.EndDate = PvOpenCloseReq.EffectiveDate;

                            _context.TblOpProjectSites.Update(projectSite);
                            await _context.SaveChangesAsync();


                            bool isAttedanceExist = await _context.EmployeeAttendance.AnyAsync(e => e.AttnDate >= PvOpenCloseReq.EffectiveDate.Value
                            && PvOpenCloseReq.ProjectCode == e.ProjectCode
                            && e.SiteCode == PvOpenCloseReq.SiteCode);

                            if (isAttedanceExist)           //If already entered attendance for greater than Closing Date
                            {
                                await transaction.RollbackAsync();
                                return -5;              //Attendance_Already_Exist
                            }

                            var removableRoasters = await _context.TblOpMonthlyRoasterForSites.AsNoTracking().Where(e=>e.MonthStartDate>=PvOpenCloseReq.EffectiveDate.Value).ToListAsync();


                            if(removableRoasters.Count>0)
                            {
                                _context.RemoveRange(removableRoasters);
                                await _context.SaveChangesAsync();

                            }



                         







                        }

                        //else if (PvOpenCloseReq.IsCancelReq)
                        //{
                        //    projectSite.IsCancelled = true;
                        //    projectSite.IsSuspended = false;
                        //    projectSite.IsClosed = false;
                        //    projectSite.IsInActive = true;

                        //    projectSite.ActualEndDate = projectSite.ActualEndDate is null ? projectSite.EndDate : projectSite.ActualEndDate;
                        //    projectSite.EndDate = PvOpenCloseReq.EffectiveDate;

                        //    _context.TblOpProjectSites.Update(projectSite);
                        //    await _context.SaveChangesAsync();

                        //}

                        else if (PvOpenCloseReq.IsRevokeSuspReq)
                        {
                            projectSite.IsCancelled = false;
                            projectSite.IsSuspended = false;
                            projectSite.IsClosed = false;
                            projectSite.IsInActive = false;
                            projectSite.EndDate = projectSite.ActualEndDate;
                            projectSite.ActualEndDate = null;



                            _context.TblOpProjectSites.Update(projectSite);
                            await _context.SaveChangesAsync();



                        }

                        else if (PvOpenCloseReq.IsExtendProjReq)
                        {
                            DateTime previousEndDate = Convert.ToDateTime(projectSite.EndDate.Value, CultureInfo.InvariantCulture);

                            bool isExistAttendance = await _context.EmployeeAttendance.AsNoTracking().AnyAsync(a => a.AttnDate > previousEndDate
                              && a.ProjectCode == projectData.ProjectCode
                              && a.SiteCode == projectSite.SiteCode);

                            if (isExistAttendance)
                            {
                                await transaction.RollbackAsync();
                                return -7;              //Attendance_Already_Exist

                            }

                            DateTime NextMonthStartDateOfPreviousEndDate = new DateTime(previousEndDate.Year, previousEndDate.Month,1).AddDays(DateTime.DaysInMonth(previousEndDate.Year,previousEndDate.Month));
                            NextMonthStartDateOfPreviousEndDate = Convert.ToDateTime(NextMonthStartDateOfPreviousEndDate,CultureInfo.InvariantCulture);
                            
                            
                            var RemovableRoasters = await _context.TblOpMonthlyRoasterForSites.AsNoTracking().Where(e=>e.ProjectCode==PvOpenCloseReq.ProjectCode
                            &&e.SiteCode==PvOpenCloseReq.SiteCode
                            &&e.MonthStartDate>= NextMonthStartDateOfPreviousEndDate).ToListAsync();
                            if (RemovableRoasters.Count>0)
                            {
                                 _context.TblOpMonthlyRoasterForSites.RemoveRange(RemovableRoasters);
                                await _context.SaveChangesAsync();
                            }

                            var AddResorces = _context.PvAddResorces.ToList();
                            var AddResReqHeads = _context.PvAddResorceHeads.Where(e => e.ProjectCode == PvOpenCloseReq.ProjectCode
                            && e.SiteCode == PvOpenCloseReq.SiteCode).ToList();
                            var AddResReqs = (from ar in AddResorces
                                              join arh in AddResReqHeads on ar.AddResReqHeadId equals arh.Id
                                              select new
                                              {
                                                  ar.Id,
                                                  ar.AddResReqHeadId,
                                                  ar.FromDate,
                                                  ar.ToDate,
                                              }).ToList();

                            if (previousEndDate.Day!=DateTime.DaysInMonth(previousEndDate.Year, previousEndDate.Month)) //exist Partial Roasters
                            {
                            var PartialRoasters=await _context.TblOpMonthlyRoasterForSites.AsNoTracking().Where(e => e.ProjectCode == PvOpenCloseReq.ProjectCode
                            && e.SiteCode == PvOpenCloseReq.SiteCode
                            && e.Month == previousEndDate.Month && e.Year == previousEndDate.Year).ToListAsync();

                                if (PartialRoasters.Count > 0)
                                {

                                    for (int row=0;row<PartialRoasters.Count;row++)
                                    {
                                        DateTime previosMonthEndDate = Convert.ToDateTime(PartialRoasters[row].MonthEndDate.Value,CultureInfo.InvariantCulture);
                                        DateTime actualMonthEndDate = Convert.ToDateTime(new DateTime(previosMonthEndDate.Year,previosMonthEndDate.Month,DateTime.DaysInMonth(previosMonthEndDate.Year,previosMonthEndDate.Month)), CultureInfo.InvariantCulture);
                                        DateTime NewMonthEndDate = actualMonthEndDate < PvOpenCloseReq.ExtensionDate ? actualMonthEndDate : Convert.ToDateTime(PvOpenCloseReq.ExtensionDate.Value, CultureInfo.InvariantCulture) ;
                                        PartialRoasters[row].MonthEndDate = NewMonthEndDate;
                                        var EmpMaps = _context.TblOpPvAddResourceEmployeeToResourceMaps.Where(e => e.EmployeeNumber == PartialRoasters[row].EmployeeNumber).ToList();
                                        bool isEmpAddedToMiddle = false;
                                        if (EmpMaps.Count > 0)
                                        {
                                            var AddResReqsEmpMaps = (from arr in AddResReqs
                                                                     join em in EmpMaps on arr.Id equals em.PvAddResReqId
                                                                     select new
                                                                     {
                                                                         em.EmployeeNumber,
                                                                         em.FromDate,
                                                                         em.ToDate
                                                                     }).ToList();
                                            isEmpAddedToMiddle = AddResReqsEmpMaps.Any(e => e.ToDate >= PartialRoasters[row].MonthStartDate && e.ToDate <= PartialRoasters[row].MonthEndDate);

                                        }
                                           

                                        var isRegignedOrTransfered = await _context.TblOpPvTransferResourceReqs.AnyAsync(e => e.IsApproved && e.EmployeeNumber == PartialRoasters[row].EmployeeNumber && (e.FromDate >= PartialRoasters[row].MonthStartDate.Value && e.FromDate <= PartialRoasters[row].MonthEndDate.Value) && e.SrcSiteCode == PvOpenCloseReq.SiteCode && e.SrcProjectCode == PvOpenCloseReq.ProjectCode)
                                        || await _context.TblOpPvTransferWithReplacementReqs.AnyAsync(e => e.IsApproved && e.SrcEmployeeNumber == PartialRoasters[row].EmployeeNumber && e.SrcProjectCode == PartialRoasters[row].ProjectCode && e.SrcSiteCode == PartialRoasters[row].SiteCode && (e.FromDate >= PartialRoasters[row].MonthStartDate.Value && e.FromDate <= PartialRoasters[row].MonthEndDate.Value))
                                        || await _context.TblOpPvRemoveResourceReqs.AnyAsync(e => e.IsApproved && e.EmployeeNumber == PartialRoasters[row].EmployeeNumber && e.ProjectCode == PartialRoasters[row].ProjectCode && e.SiteCode == PartialRoasters[row].SiteCode && (e.FromDate >= PartialRoasters[row].MonthStartDate.Value && e.FromDate <= PartialRoasters[row].MonthEndDate.Value))
                                        || await _context.TblOpPvSwapEmployeesReqs.AnyAsync(e => e.IsApproved && e.SrcEmployeeNumber == PartialRoasters[row].EmployeeNumber && e.SrcProjectCode == PartialRoasters[row].ProjectCode && e.SrcSiteCode == PartialRoasters[row].SiteCode && (e.FromDate >= PartialRoasters[row].MonthStartDate.Value && e.FromDate <= PartialRoasters[row].MonthEndDate.Value))
                                        || await _context.TblOpPvReplaceResourceReqs.AnyAsync(e => e.IsApproved && e.ResignedEmployeeNumber == PartialRoasters[row].EmployeeNumber && e.ProjectCode == PartialRoasters[row].ProjectCode && e.SiteCode == PartialRoasters[row].SiteCode && (e.FromDate >= PartialRoasters[row].MonthStartDate.Value && e.FromDate <= PartialRoasters[row].MonthEndDate.Value))
                                        || isEmpAddedToMiddle;

                                        if (!isRegignedOrTransfered)
                                        {


                                            DateTime LastDayOfPartialMonth = new DateTime(previousEndDate.Year, previousEndDate.Month, DateTime.DaysInMonth(previousEndDate.Year, previousEndDate.Month));
                                            LastDayOfPartialMonth = Convert.ToDateTime(LastDayOfPartialMonth, CultureInfo.InvariantCulture);
                                            short OffDay = -1;
                                            string ShiftCode = "";
                                            if (PartialRoasters[row].S1 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 1)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S2 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 2)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S3 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 3)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S4 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 4)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S5 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 5)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S6 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 6)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S7 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 7)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S8 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 8)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S9 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 9)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S10 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 10)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S11 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 11)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S12 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 12)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S13 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 13)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S14 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 14)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S15 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 15)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S16 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 16)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S17 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 17)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S18 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 18)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S19 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 19)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S20 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 20)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S21 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 21)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S22 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 22)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S23 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 23)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S24 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 24)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S25 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 25)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S26 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 26)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S27 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 27)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S28 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 28)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S29 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 29)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S30 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 30)).DayOfWeek;
                                            }
                                            else if (PartialRoasters[row].S31 == "O")
                                            {
                                                OffDay = (short)(new DateTime(previousEndDate.Year, previousEndDate.Month, 31)).DayOfWeek;
                                            }

                                            if (PartialRoasters[row].S1 != "x" && PartialRoasters[row].S1 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S1;
                                            }
                                            else if (PartialRoasters[row].S2 != "x" && PartialRoasters[row].S2 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S2;
                                            }
                                            else if (PartialRoasters[row].S3 != "x" && PartialRoasters[row].S3 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S3;
                                            }
                                            else if (PartialRoasters[row].S4 != "x" && PartialRoasters[row].S4 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S4;
                                            }
                                            else if (PartialRoasters[row].S5 != "x" && PartialRoasters[row].S5 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S5;
                                            }
                                            else if (PartialRoasters[row].S6 != "x" && PartialRoasters[row].S6 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S6;
                                            }
                                            else if (PartialRoasters[row].S7 != "x" && PartialRoasters[row].S7 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S7;
                                            }
                                            else if (PartialRoasters[row].S8 != "x" && PartialRoasters[row].S8 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S8;
                                            }
                                            else if (PartialRoasters[row].S9 != "x" && PartialRoasters[row].S9 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S9;
                                            }
                                            else if (PartialRoasters[row].S10 != "x" && PartialRoasters[row].S10 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S10;
                                            }
                                            if (PartialRoasters[row].S11 != "x" && PartialRoasters[row].S11 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S11;
                                            }
                                            if (PartialRoasters[row].S12 != "x" && PartialRoasters[row].S12 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S12;
                                            }
                                            if (PartialRoasters[row].S13 != "x" && PartialRoasters[row].S13 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S13;
                                            }
                                            if (PartialRoasters[row].S14 != "x" && PartialRoasters[row].S14 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S14;
                                            }
                                            if (PartialRoasters[row].S15 != "x" && PartialRoasters[row].S15 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S15;
                                            }
                                            if (PartialRoasters[row].S16 != "x" && PartialRoasters[row].S16 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S16;
                                            }
                                            if (PartialRoasters[row].S17 != "x" && PartialRoasters[row].S17 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S17;
                                            }
                                            if (PartialRoasters[row].S18 != "x" && PartialRoasters[row].S18 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S18;
                                            }
                                            if (PartialRoasters[row].S19 != "x" && PartialRoasters[row].S19 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S19;
                                            }
                                            if (PartialRoasters[row].S20 != "x" && PartialRoasters[row].S20 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S20;
                                            }
                                            else if (PartialRoasters[row].S21 != "x" && PartialRoasters[row].S21 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S21;
                                            }
                                            else if (PartialRoasters[row].S22 != "x" && PartialRoasters[row].S22 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S22;
                                            }
                                            else if (PartialRoasters[row].S23 != "x" && PartialRoasters[row].S23 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S23;
                                            }
                                            else if (PartialRoasters[row].S24 != "x" && PartialRoasters[row].S24 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S24;
                                            }
                                            else if (PartialRoasters[row].S25 != "x" && PartialRoasters[row].S25 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S25;
                                            }
                                            else if (PartialRoasters[row].S26 != "x" && PartialRoasters[row].S26 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S26;
                                            }
                                            else if (PartialRoasters[row].S27 != "x" && PartialRoasters[row].S27 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S27;
                                            }
                                            else if (PartialRoasters[row].S28 != "x" && PartialRoasters[row].S28 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S28;
                                            }
                                            else if (PartialRoasters[row].S29 != "x" && PartialRoasters[row].S29 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S29;
                                            }
                                            else if (PartialRoasters[row].S30 != "x" && PartialRoasters[row].S30 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S30;
                                            }
                                            else if (PartialRoasters[row].S31 != "x" && PartialRoasters[row].S31 != "O")
                                            {
                                                ShiftCode = PartialRoasters[row].S31;
                                            }


                                            for (int d = 1; d <= 31; d++)
                                            {
                                                DateTime date = new DateTime(previousEndDate.Year, previousEndDate.Month, 1).AddDays(d - 1);
                                                date = Convert.ToDateTime(date, CultureInfo.InvariantCulture);

                                                switch (d)
                                                {
                                                    case 1:
                                                        PartialRoasters[row].S1 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S1;

                                                        break;
                                                    case 2:
                                                        PartialRoasters[row].S2 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S2;

                                                        break;
                                                    case 3:
                                                        PartialRoasters[row].S3 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S3;

                                                        break;
                                                    case 4:
                                                        PartialRoasters[row].S4 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S4;

                                                        break;
                                                    case 5:
                                                        PartialRoasters[row].S5 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S5;

                                                        break;
                                                    case 6:
                                                        PartialRoasters[row].S6 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S6;

                                                        break;
                                                    case 7:
                                                        PartialRoasters[row].S7 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S7;

                                                        break;
                                                    case 8:
                                                        PartialRoasters[row].S8 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S8;

                                                        break;
                                                    case 9:
                                                        PartialRoasters[row].S9 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S9;

                                                        break;
                                                    case 10:
                                                        PartialRoasters[row].S10 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S10;

                                                        break;
                                                    case 11:
                                                        PartialRoasters[row].S11 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S11;

                                                        break;
                                                    case 12:
                                                        PartialRoasters[row].S12 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S12;

                                                        break;
                                                    case 13:
                                                        PartialRoasters[row].S13 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S13;

                                                        break;
                                                    case 14:
                                                        PartialRoasters[row].S14 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S14;

                                                        break;
                                                    case 15:
                                                        PartialRoasters[row].S15 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S15;

                                                        break;
                                                    case 16:
                                                        PartialRoasters[row].S16 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S16;

                                                        break;
                                                    case 17:
                                                        PartialRoasters[row].S17 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S17;

                                                        break;
                                                    case 18:
                                                        PartialRoasters[row].S18 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S18;

                                                        break;
                                                    case 19:
                                                        PartialRoasters[row].S19 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S19;

                                                        break;
                                                    case 20:
                                                        PartialRoasters[row].S20 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S20;

                                                        break;
                                                    case 21:
                                                        PartialRoasters[row].S21 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S21;

                                                        break;
                                                    case 22:
                                                        PartialRoasters[row].S22 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S22;

                                                        break;
                                                    case 23:
                                                        PartialRoasters[row].S23 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S23;

                                                        break;
                                                    case 24:
                                                        PartialRoasters[row].S24 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S24;

                                                        break;
                                                    case 25:
                                                        PartialRoasters[row].S25 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S25;

                                                        break;
                                                    case 26:
                                                        PartialRoasters[row].S26 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S26;

                                                        break;
                                                    case 27:
                                                        PartialRoasters[row].S27 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S27;

                                                        break;
                                                    case 28:
                                                        PartialRoasters[row].S28 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S28;

                                                        break;
                                                    case 29:
                                                        PartialRoasters[row].S29 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S29;

                                                        break;
                                                    case 30:
                                                        PartialRoasters[row].S30 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S30;

                                                        break;
                                                    case 31:
                                                        PartialRoasters[row].S31 = date > previousEndDate ? date > PvOpenCloseReq.ExtensionDate || date > PartialRoasters[row].MonthEndDate ? "x" : OffDay == (short)date.DayOfWeek ? "O" : ShiftCode : PartialRoasters[row].S31;

                                                        break;

                                                }


                                            }
                                        }

                                    }
                                     _context.UpdateRange(PartialRoasters);
                                    await _context.SaveChangesAsync();
                                }
                                else
                                {

                                    transaction.Rollback();
                                    transaction2.Rollback();
                                    return -9;//No roaster Found In Previous End Month
                                }


                            }

                            var RecentRoasters = await _context.TblOpMonthlyRoasterForSites.AsNoTracking().Where(e=>e.ProjectCode==PvOpenCloseReq.ProjectCode
                            &&e.SiteCode==PvOpenCloseReq.SiteCode
                            && e.Month==previousEndDate.Month
                            &&e.Year==previousEndDate.Year
                            && e.IsPrimaryResource).ToListAsync();

                            if (RecentRoasters.Count==0)
                            {
                                transaction.Rollback();
                                transaction2.Rollback();
                                return -9;
                            }

                            List<TblOpMonthlyRoasterForSite> NewRoasters = new();
                           

                            for (var Mdate = NextMonthStartDateOfPreviousEndDate; Mdate <= PvOpenCloseReq.ExtensionDate; Mdate = Mdate.AddDays(DateTime.DaysInMonth(Mdate.Year, Mdate.Month)))
                                {


                                DateTime MendDate = Mdate.AddDays(DateTime.DaysInMonth(Mdate.Year, Mdate.Month) - 1);
                                for (var i = 0; i < RecentRoasters.Count; i++)
                                {
                                    var EmpMaps = _context.TblOpPvAddResourceEmployeeToResourceMaps.Where(e => e.EmployeeNumber == RecentRoasters[i].EmployeeNumber).ToList();
                                    bool isEmpAddedToMiddle = false;
                                    if (EmpMaps.Count > 0)
                                    {
                                        var AddResReqsEmpMaps = (from arr in AddResReqs
                                                                 join em in EmpMaps on arr.Id equals em.PvAddResReqId
                                                                 select new
                                                                 {
                                                                     em.EmployeeNumber,
                                                                     em.FromDate,
                                                                     em.ToDate
                                                                 }).ToList();
                                        isEmpAddedToMiddle = AddResReqsEmpMaps.Any(e => e.ToDate >= RecentRoasters[i].MonthStartDate && e.ToDate <= RecentRoasters[i].MonthEndDate);

                                    }


                                    var isRegignedOrTransfered = await _context.TblOpPvTransferResourceReqs.AnyAsync(e => e.IsApproved && e.EmployeeNumber == RecentRoasters[i].EmployeeNumber && (e.FromDate >= RecentRoasters[i].MonthStartDate.Value && e.FromDate <= RecentRoasters[i].MonthEndDate.Value) && e.SrcSiteCode == PvOpenCloseReq.SiteCode && e.SrcProjectCode == PvOpenCloseReq.ProjectCode)
                                    || await _context.TblOpPvTransferWithReplacementReqs.AnyAsync(e => e.IsApproved && e.SrcEmployeeNumber == RecentRoasters[i].EmployeeNumber && e.SrcProjectCode == RecentRoasters[i].ProjectCode && e.SrcSiteCode == RecentRoasters[i].SiteCode && (e.FromDate >= RecentRoasters[i].MonthStartDate.Value && e.FromDate <= RecentRoasters[i].MonthEndDate.Value))
                                    || await _context.TblOpPvRemoveResourceReqs.AnyAsync(e => e.IsApproved && e.EmployeeNumber == RecentRoasters[i].EmployeeNumber && e.ProjectCode == RecentRoasters[i].ProjectCode && e.SiteCode == RecentRoasters[i].SiteCode && (e.FromDate >= RecentRoasters[i].MonthStartDate.Value && e.FromDate <= RecentRoasters[i].MonthEndDate.Value))
                                    || await _context.TblOpPvSwapEmployeesReqs.AnyAsync(e => e.IsApproved && e.SrcEmployeeNumber == RecentRoasters[i].EmployeeNumber && e.SrcProjectCode == RecentRoasters[i].ProjectCode && e.SrcSiteCode == RecentRoasters[i].SiteCode && (e.FromDate >= RecentRoasters[i].MonthStartDate.Value && e.FromDate <= RecentRoasters[i].MonthEndDate.Value))
                                    || await _context.TblOpPvReplaceResourceReqs.AnyAsync(e => e.IsApproved && e.ResignedEmployeeNumber == RecentRoasters[i].EmployeeNumber && e.ProjectCode == RecentRoasters[i].ProjectCode && e.SiteCode == RecentRoasters[i].SiteCode && (e.FromDate >= RecentRoasters[i].MonthStartDate.Value && e.FromDate <= RecentRoasters[i].MonthEndDate.Value))
                                    || isEmpAddedToMiddle;




                                    if (!isRegignedOrTransfered)
                                    {
                                        TblOpMonthlyRoasterForSite NewRoaster = new();

                                        NewRoaster.EmployeeNumber = RecentRoasters[i].EmployeeNumber;
                                        NewRoaster.ProjectCode = RecentRoasters[i].ProjectCode;
                                        NewRoaster.CustomerCode = RecentRoasters[i].CustomerCode;
                                        NewRoaster.SiteCode = RecentRoasters[i].SiteCode;
                                        NewRoaster.SkillsetCode = RecentRoasters[i].SkillsetCode;
                                        NewRoaster.SkillsetName = RecentRoasters[i].SkillsetName;
                                        NewRoaster.MonthStartDate = new DateTime(Mdate.Year, Mdate.Month, 1);
                                        NewRoaster.MonthEndDate = new DateTime(Mdate.Year, Mdate.Month, DateTime.DaysInMonth(Mdate.Year, Mdate.Month));
                                        NewRoaster.IsPrimaryResource = true;
                                        NewRoaster.Month = (short)Mdate.Month;
                                        NewRoaster.Year = (short)Mdate.Year;
                                        NewRoaster.EmployeeID = 0;
                                        for (int d = 1; d <= 31; d++)
                                        {
                                            DateTime date = Mdate.AddDays(d - 1);
                                            date = Convert.ToDateTime(date, CultureInfo.InvariantCulture);

                                            switch (d)
                                            {
                                                case 1: NewRoaster.S1 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 2: NewRoaster.S2 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 3: NewRoaster.S3 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 4: NewRoaster.S4 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 5: NewRoaster.S5 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 6: NewRoaster.S6 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 7: NewRoaster.S7 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 8: NewRoaster.S8 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 9: NewRoaster.S9 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 10: NewRoaster.S10 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 11: NewRoaster.S11 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 12: NewRoaster.S12 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 13: NewRoaster.S13 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 14: NewRoaster.S14 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 15: NewRoaster.S15 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 16: NewRoaster.S16 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 17: NewRoaster.S17 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 18: NewRoaster.S18 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 19: NewRoaster.S19 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 20: NewRoaster.S20 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 21: NewRoaster.S21 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 22: NewRoaster.S22 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 23: NewRoaster.S23 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 24: NewRoaster.S24 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 25: NewRoaster.S25 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 26: NewRoaster.S26 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 27: NewRoaster.S27 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 28: NewRoaster.S28 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 29: NewRoaster.S29 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 30: NewRoaster.S30 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                                case 31: NewRoaster.S31 = date <= PvOpenCloseReq.ExtensionDate && date <= MendDate ? "" : "x"; break;
                                            }
                                        }
                                        NewRoasters.Add(NewRoaster);
                                    }


                                   
                                }

                                }
                            
                            if (NewRoasters.Count>0)
                            {
                                await _context.TblOpMonthlyRoasterForSites.AddRangeAsync(NewRoasters);
                                await _context.SaveChangesAsync();
                            }

                           


                            projectSite.IsCancelled = false;
                            projectSite.IsSuspended = false;
                            projectSite.IsClosed = false;
                            projectSite.IsInActive = false;


                            projectSite.EndDate = PvOpenCloseReq.ExtensionDate;
                            projectSite.ActualEndDate = null;



                            _context.TblOpProjectSites.Update(projectSite);
                            await _context.SaveChangesAsync();


                            if ( PvOpenCloseReq.ExtensionDate > projectData.EndDate)
                            {
                                projectData.EndDate = PvOpenCloseReq.ExtensionDate;
                                _context.OP_HRM_TEMP_Projects.Update(projectData);
                                await _context.SaveChangesAsync();
                            }


                        }
                        else if (PvOpenCloseReq.IsReOpenReq)
                        {
                          //  return -101;    //  Reopen Blocked
                            
                            
                            bool isExistAttendance = await _context.EmployeeAttendance.AsNoTracking().AnyAsync(a => a.AttnDate >= PvOpenCloseReq.EffectiveDate
                                    && a.ProjectCode == projectData.ProjectCode
                                    && a.SiteCode == projectSite.SiteCode);

                            if (isExistAttendance)
                            {
                                await transaction.RollbackAsync();
                                await transaction2.RollbackAsync();
                                return -7;
                            }
                            projectSite.IsCancelled = false;
                            projectSite.IsSuspended = false;
                            projectSite.IsClosed = false;
                            projectSite.IsInActive = false;
                            projectSite.StartDate = PvOpenCloseReq.EffectiveDate;
                            projectSite.EndDate = PvOpenCloseReq.ExtensionDate;
                            projectSite.ActualEndDate = null;



                            _context.TblOpProjectSites.Update(projectSite);
                            await _context.SaveChangesAsync();



                            var RoastersToDelete = await _context.TblOpMonthlyRoasterForSites.AsNoTracking().OrderByDescending(e => e.MonthStartDate).Where(r => 
                              r.ProjectCode == projectData.ProjectCode 
                              && r.SiteCode == projectSite.SiteCode
                              && r.MonthEndDate >= PvOpenCloseReq.EffectiveDate
                              ).ToListAsync();
                             _context.TblOpMonthlyRoasterForSites.RemoveRange(RoastersToDelete);
                            await _context.SaveChangesAsync();
                            var Project = await _context.OP_HRM_TEMP_Projects.AsNoTracking().FirstOrDefaultAsync(e=>e.ProjectCode==PvOpenCloseReq.ProjectCode);
                            var ProjectSites = await _context.TblOpProjectSites.AsNoTracking().Where(e=>e.ProjectCode==PvOpenCloseReq.ProjectCode).ToListAsync();
                            if(ProjectSites.Count>0)
                            {
                                DateTime ProjectStartDate = Convert.ToDateTime(ProjectSites[0].StartDate.Value,CultureInfo.InvariantCulture);
                                DateTime ProjectEndDate = Convert.ToDateTime(ProjectSites[0].EndDate.Value,CultureInfo.InvariantCulture);


                                foreach (var site in ProjectSites) { 
                                if(site.StartDate.Value<ProjectStartDate)
                                    {
                                        ProjectStartDate = site.StartDate.Value;
                                    }
                                    if(site.EndDate.Value<ProjectEndDate)
                                    {
                                        ProjectEndDate = site.EndDate.Value;

                                    }
                                }
                              
                                    Project.StartDate = ProjectStartDate;
                                    Project.EndDate = ProjectEndDate;
                                    _context.OP_HRM_TEMP_Projects.Update(Project);
                                    await _context.SaveChangesAsync();

                                
                            }

                        }
                        else
                        {
                            await transaction.RollbackAsync();
                            await transaction2.RollbackAsync();
                            return -8;          //invalid request type
                        }




                        PvOpenCloseReq.IsApproved = true;
                        PvOpenCloseReq.ApprovedBy = request.User.UserId;
                        PvOpenCloseReq.Modified = DateTime.UtcNow;
                        _context.TblOpPvOpenCloseReqs.Update(PvOpenCloseReq);
                        await _context.SaveChangesAsync();



                        var RoastersWithEmptyShifts = await _context.TblOpMonthlyRoasterForSites.AsNoTracking().Where(e => e.ProjectCode == PvOpenCloseReq.ProjectCode
                           && e.SiteCode == PvOpenCloseReq.SiteCode
                           && e.S1 == "x" && e.S2 == "x" && e.S3 == "x" && e.S4 == "x" && e.S5 == "x" && e.S6 == "x" && e.S7 == "x" && e.S8 == "x" && e.S9 == "x" && e.S10 == "x"
                           && e.S11 == "x" && e.S12 == "x" && e.S13 == "x" && e.S14 == "x" && e.S15 == "x" && e.S16 == "x" && e.S17 == "x" && e.S18 == "x" && e.S19 == "x" && e.S20 == "x"
                           && e.S21 == "x" && e.S22 == "x" && e.S23 == "x" && e.S24 == "x" && e.S25 == "x" && e.S26 == "x" && e.S27 == "x" && e.S28 == "x" && e.S29 == "x" && e.S30 == "x"
                          && e.S31 == "x"

                           ).ToListAsync();

                        if (RoastersWithEmptyShifts.Count > 0)
                        {
                            _context.TblOpMonthlyRoasterForSites.RemoveRange(RoastersWithEmptyShifts);
                            await _context.SaveChangesAsync();
                        }

                        transaction.Commit();
                        transaction2.Commit();

                        return PvOpenCloseReq.Id;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        transaction2.Rollback();
                        Log.Error("Error in ApproveReqPvOpenCloseReqByIdHandler Method");
                        Log.Error("Error occured time : " + DateTime.UtcNow);
                        Log.Error("Error message : " + ex.Message);
                        Log.Error("Error StackTrace : " + ex.StackTrace);

                        return 0;
                    }
                }
            }
        }
    }

    #endregion
    #region GetPvOpenCloseReqById
    public class GetPvOpenCloseReqById : IRequest<TblOpPvOpenCloseReqsPaginationDto>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }

    public class GetPvOpenCloseReqByIdHandler : IRequestHandler<GetPvOpenCloseReqById, TblOpPvOpenCloseReqsPaginationDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPvOpenCloseReqByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpPvOpenCloseReqsPaginationDto> Handle(GetPvOpenCloseReqById request, CancellationToken cancellationToken)
        {
            try
            {

                var PvTransferResourceReq = await _context.TblOpPvOpenCloseReqs.AsNoTracking().ProjectTo<TblOpPvOpenCloseReqsPaginationDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(e => e.Id == request.Id);


                return PvTransferResourceReq;





            }
            catch (Exception ex)
            {
                Log.Error("Error in GetPvOpenCloseReqByIdHandler Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion


}
