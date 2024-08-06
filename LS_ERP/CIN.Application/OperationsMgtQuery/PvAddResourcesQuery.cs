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



    #region CreateUpdatePvAddResourceReq
    public class CreatePvAddResourceReq : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public InputPvAddResourceReq PvAddResourceReqDto { get; set; }
    }

    public class CreatePvAddResourceReqHandler : IRequestHandler<CreatePvAddResourceReq, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreatePvAddResourceReqHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<long> Handle(CreatePvAddResourceReq request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdatePvAddResourceReq method start----");




                    var obj = request.PvAddResourceReqDto;


                    TblOpPvAddResourceReqHead ReqHead = new();
                    if (obj.Id > 0)

                        ReqHead = await _context.PvAddResorceHeads.FirstOrDefaultAsync(e => e.Id == obj.Id);
             
                        ReqHead.IsActive = obj.IsActive;
                        ReqHead.CustomerCode = obj.CustomerCode;
                        ReqHead.ProjectCode = obj.ProjectCode;
                        ReqHead.SiteCode = obj.SiteCode;
                        ReqHead.IsApproved = false;
                        ReqHead.ApprovedBy = 0;
                        ReqHead.IsApproved = false;
                        ReqHead.IsEmpMapped = false;
                        ReqHead.IsMerged = false;
                        ReqHead.IsActive = true;

                    ReqHead.FileUploadBy = null;
                    ReqHead.FileUrl = null;
               


                    if (obj.Id > 0)
                    {
                        ReqHead.Modified = DateTime.Now;
                        ReqHead.ModifiedBy = request.User.UserId;


                        var ExistingEmpToResMappings = await _context.TblOpPvAddResourceEmployeeToResourceMaps.AsNoTracking().Where(e=>e.PvAddResReqId==ReqHead.Id).ToListAsync();

                        if (ExistingEmpToResMappings.Count > 0)
                        {
                            _context.RemoveRange(ExistingEmpToResMappings);
                          await  _context.SaveChangesAsync();
                        }

                        _context.PvAddResorceHeads.Update(ReqHead);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ReqHead.CreatedBy = request.User.UserId;

                        ReqHead.Created = DateTime.Now;
                        await _context.PvAddResorceHeads.AddAsync(ReqHead);
                        await _context.SaveChangesAsync();
                    }


                    if (request.PvAddResourceReqDto.ResourceList.Count() > 0)
                    {
                        var oldResourcesList = await _context.PvAddResorces.Where(e => e.AddResReqHeadId == ReqHead.Id).ToListAsync();
                        _context.PvAddResorces.RemoveRange(oldResourcesList);

                        List<TblOpPvAddResource> resourceList = new();
                        foreach (var req in request.PvAddResourceReqDto.ResourceList)
                        {
                            TblOpPvAddResource resource = new()
                            {
                                //Id=req.Id,
                                AddResReqHeadId = ReqHead.Id,
                                SkillsetCode = req.SkillsetCode,
                                Qty = req.Qty,
                                PricePerUnit = req.PricePerUnit,
                                IsActive = true,
                                FromDate = req.FromDate,
                                ToDate = req.ToDate
                            };
                            if (req.Id > 0)
                            {
                                resource.Modified = DateTime.Now;
                                resource.ModifiedBy = req.CreatedBy;
                                _context.PvAddResorces.Update(resource);
                            }
                            else
                            {
                                resource.CreatedBy = request.User.UserId;
                                resource.Created = DateTime.Now;
                                await _context.PvAddResorces.AddAsync(resource);
                            }

                            resourceList.Add(resource);
                        }
                        await _context.PvAddResorces.AddRangeAsync(resourceList);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return 0;

                    }
                    Log.Info("----Info CreateUpdatePvAddResourceReq method Exit----");

                    await transaction.CommitAsync();
                    return ReqHead.Id;

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdatePvAddResourceReq Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;

                }
            }    
        }
        
    }

    #endregion
    #region GetPvAddResourceReqsPagedList

    public class GetPvAddResourceReqsPagedList : IRequest<PaginatedList<PvAddResourceReqPagination>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetPvAddResourceReqsPagedListHandler : IRequestHandler<GetPvAddResourceReqsPagedList, PaginatedList<PvAddResourceReqPagination>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPvAddResourceReqsPagedListHandler(CINDBOneContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<PvAddResourceReqPagination>> Handle(GetPvAddResourceReqsPagedList request, CancellationToken cancellationToken)
        {
            bool isAdmin = _context.SystemLogins.AsNoTracking().FirstOrDefault(e => e.Id == request.User.UserId).LoginId == "Admin" ? true : false;
            var oprAuths = _context.TblOpAuthoritiesList.AsNoTracking();
            var Projects = _context.OP_HRM_TEMP_Projects.AsNoTracking();
            var Customers = _context.OprCustomers.AsNoTracking();
            var AddResReqs = _context.TblOpPvAddResourceEmployeeToResourceMaps.AsNoTracking();
            var Sites = _context.OprSites.AsNoTracking();
            var search = request.Input.Query;
            var list = await _context.PvAddResorceHeads.AsNoTracking()
               .OrderBy(request.Input.OrderBy).Select(d => new PvAddResourceReqPagination
               {
                   Id = d.Id,
                   CustomerCode = d.CustomerCode,
                   SiteCode = d.SiteCode,
                   SiteName = Sites.FirstOrDefault(s => s.SiteCode == d.SiteCode).SiteName,
                   SiteNameAr = Sites.FirstOrDefault(s => s.SiteCode == d.SiteCode).SiteArbName,
                   ProjectCode = d.ProjectCode,
                   ProjectName = Projects.FirstOrDefault(p => p.ProjectCode == d.ProjectCode).ProjectNameEng,
                   ProjectNameAr = Projects.FirstOrDefault(p => p.ProjectCode == d.ProjectCode).ProjectNameArb,
                  ApprovedBy=d.ApprovedBy,
                   Created=d.Created,
                   CreatedBy=d.CreatedBy,
                   CustomerName=Customers.FirstOrDefault(e=>e.CustCode==d.CustomerCode).CustName,
                   CustomerNameAr=Customers.FirstOrDefault(e => e.CustCode == d.CustomerCode).CustArbName,
                    IsEmpMapped= AddResReqs.Any(e=>e.PvAddResReqId==d.Id),
                   IsActive = d.IsActive,
                   IsApproved = d.IsApproved,
                   IsMerged=d.IsMerged,
                   RequestNumber=d.Id,
                   CanEditReq= d.CreatedBy == request.User.UserId,                 //if user he created request can delete req,edit request
                   IsAdmin=isAdmin,
                   CanApproveReq = oprAuths.Any(e=>e.AppAuth==request.User.UserId &&e.CanApprovePvReq &&e.BranchCode== Sites.FirstOrDefault(s=>s.SiteCode==d.SiteCode).SiteCityCode)||isAdmin,
               FileUploadBy=d.FileUploadBy,
               FileUrl=d.FileUrl,
               IsFileUploadRequired=true,
               })
              .Where(e =>(e.CustomerCode.Contains(search) ||
                            e.SiteCode.Contains(search) ||
                            e.ProjectCode.Contains(search) ||
                            e.SiteName.Contains(search) ||
                            e.SiteNameAr.Contains(search) ||
                            e.ProjectName.Contains(search) ||
                            e.ProjectNameAr.Contains(search) ||
                            search == "" || search == null
                             )&&(e.CanApproveReq||e.CanEditReq) )

               .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
    }
    #endregion

    #region GetPvAddResourceReqById
    public class GetPvAddResourceReqById : IRequest<OpPvAddResourceReqHeadDto>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }

    public class GetPvAddResourceReqByIdHandler : IRequestHandler<GetPvAddResourceReqById, OpPvAddResourceReqHeadDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPvAddResourceReqByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<OpPvAddResourceReqHeadDto> Handle(GetPvAddResourceReqById request, CancellationToken cancellationToken)
        {
            try
            {
                OpPvAddResourceReqHeadDto obj = new();
                var PvAddResourceReq = await _context.PvAddResorceHeads.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
             
                    List<TblOpPVAddResourceDto> ResourcesList = _context.PvAddResorces.AsNoTracking().ProjectTo<TblOpPVAddResourceDto>(_mapper.ConfigurationProvider).Where(e => e.AddResReqHeadId == request.Id).ToList();


                    obj.Id = PvAddResourceReq.Id;
                    obj.CustomerCode = PvAddResourceReq.CustomerCode;
                    obj.ProjectCode = PvAddResourceReq.ProjectCode;
                    obj.SiteCode = PvAddResourceReq.SiteCode;
                    obj.IsApproved = PvAddResourceReq.IsApproved;
                    obj.IsEmpMapped = PvAddResourceReq.IsEmpMapped;
                    obj.IsMerged = PvAddResourceReq.IsMerged;
                    obj.Created = PvAddResourceReq.Created;
                    obj.CreatedBy = PvAddResourceReq.CreatedBy;
                    obj.Modified = PvAddResourceReq.Modified;
                    obj.ModifiedBy = PvAddResourceReq.ModifiedBy;
                    obj.IsActive = PvAddResourceReq.IsActive;
                    obj.ResourceList = ResourcesList;
                    return obj;
                
                
                   
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetPvAddResourceReqByIdHandler Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion


    #region DeletePvAddResourceReqById
    public class DeletePvAddResourceReqById : IRequest<OpPvAddResourceReqHeadDto>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }

    public class DeletePvAddResourceReqByIdHandler : IRequestHandler<DeletePvAddResourceReqById, OpPvAddResourceReqHeadDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public DeletePvAddResourceReqByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<OpPvAddResourceReqHeadDto> Handle(DeletePvAddResourceReqById request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    var PvAddResourceReq = await _context.PvAddResorceHeads.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
                    var ResourcesList = await _context.PvAddResorces.AsNoTracking().Where(e => e.AddResReqHeadId == request.Id).ToListAsync();

                    _context.PvAddResorces.RemoveRange(ResourcesList);
                    _context.PvAddResorceHeads.Remove(PvAddResourceReq);

                    _context.SaveChanges();
                   await transaction.CommitAsync(); ;
                    return new() { Id= PvAddResourceReq .Id};

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in DeletePvAddResourceReqByIdHandler Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return null;
                }
            }
        }
    }

    #endregion
    #region ApproveReqPvAddResourceReqById
    public class ApproveReqPvAddResourceReqById : IRequest<TblOpPvAddResourceReqHead>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }

    public class ApproveReqPvAddResourceReqByIdHandler : IRequestHandler<ApproveReqPvAddResourceReqById, TblOpPvAddResourceReqHead>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public ApproveReqPvAddResourceReqByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpPvAddResourceReqHead> Handle(ApproveReqPvAddResourceReqById request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                

                    try
                    {
                        Log.Info("----Info ApproveReqPvAddResourceReqById method start----");




                        TblOpPvAddResourceReqHead PvAddResourceReq = await _context.PvAddResorceHeads.AsNoTracking().FirstOrDefaultAsync(e => e.Id == request.Id);
                        if (PvAddResourceReq is null)
                        {
                            return new() { Id = -1 };       // Request_not_exist
                        }
                        if (PvAddResourceReq.IsApproved)
                        {
                            return new() { Id = -2 };      //Request_Already_Approved
                        }
                        var projectData = await _context.TblOpProjectSites.AsNoTracking().FirstOrDefaultAsync(p => p.ProjectCode == PvAddResourceReq.ProjectCode);
                        if (projectData is null)
                        {
                            return new() { Id = -3 };      //invalid_project_code
                        }

                       
                        
                       
        




                      
                        PvAddResourceReq.IsApproved = true;
                        PvAddResourceReq.ApprovedBy = request.User.UserId;
                        PvAddResourceReq.Modified = DateTime.UtcNow;
                         _context.PvAddResorceHeads.Update(PvAddResourceReq);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                        return PvAddResourceReq;

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Log.Error("Error in ApproveReqPvAddResourceReqByIdHandler Method");
                        Log.Error("Error occured time : " + DateTime.UtcNow);
                        Log.Error("Error message : " + ex.Message);
                        Log.Error("Error StackTrace : " + ex.StackTrace);

                        return new() { Id = -1 };
                    }
                }
            
        }
    }

    #endregion


    #region CreateUpdatePvAddResourceEmployeeToResourceMap
    public class CreateUpdatePvAddResourceEmployeeToResourceMap : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public InputPvAddResourceEmployeeToResourceMap PvAddResourceEmpToResMaps { get; set; }
    }

    public class CreateUpdatePvAddResourceEmployeeToResourceMapHandler : IRequestHandler<CreateUpdatePvAddResourceEmployeeToResourceMap, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdatePvAddResourceEmployeeToResourceMapHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<long> Handle(CreateUpdatePvAddResourceEmployeeToResourceMap request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdatePvAddResourceEmployeeToResourceMap method start----");
                    List<TblOpPvAddResourceEmployeeToResourceMapDto> NewMappings = request.PvAddResourceEmpToResMaps.MappingsList;

                    if (NewMappings.Count == 0)
                    {
                        return -1;                                  //Empty Mappings

                    }



                    var PvAddResReq = await _context.PvAddResorceHeads.FirstOrDefaultAsync(e=>e.Id== NewMappings[0].PvAddResReqId);
                    if (PvAddResReq is null)
                    {
                        return -2; //invalid request Id
                    }



                        List<TblOpPvAddResourceEmployeeToResourceMap> EmpToResMapps = new();

                        for (int i = 0; i < NewMappings.Count; i++)
                        {
                            TblOpPvAddResourceEmployeeToResourceMap map = new()
                            {

                                MapId = NewMappings[i].MapId,
                                PvAddResReqId = NewMappings[i].PvAddResReqId,
                                EmployeeNumber = NewMappings[i].EmployeeNumber,
                                SkillSet = NewMappings[i].SkillSet,
                                SiteCode = NewMappings[i].SiteCode,
                                ProjectCode = NewMappings[i].ProjectCode,
                                DefShift = NewMappings[i].DefShift,
                                FromDate = NewMappings[i].FromDate,
                                ToDate = NewMappings[i].ToDate,
                                OffDay = NewMappings[i].OffDay
                            };
                        if (map.MapId == 0)
                        {
                            await _context.TblOpPvAddResourceEmployeeToResourceMaps.AddAsync(map);
                            await _context.SaveChangesAsync();
                        }
                        else {
                             _context.TblOpPvAddResourceEmployeeToResourceMaps.Update(map);
                            await _context.SaveChangesAsync();
                        }
                        
                    }
                    PvAddResReq.IsEmpMapped = true;
                    _context.PvAddResorceHeads.Update(PvAddResReq);
                    await _context.SaveChangesAsync();


                    await transaction.CommitAsync();
                    return NewMappings[0].PvAddResReqId;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Log.Error("Error in CreateUpdatePvAddResourceEmployeeToResourceMap Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;

                }
            }
        }

    }

    #endregion


    #region GetPvAddResourceEmpToResMapsByReqId
    public class GetPvAddResourceEmpToResMapsByReqIdById : IRequest<List<TblOpPvAddResourceEmployeeToResourceMapDto>>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }

    public class GetPvAddResourceEmpToResMapsByReqIdByIdHandler : IRequestHandler<GetPvAddResourceEmpToResMapsByReqIdById, List<TblOpPvAddResourceEmployeeToResourceMapDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetPvAddResourceEmpToResMapsByReqIdByIdHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblOpPvAddResourceEmployeeToResourceMapDto>> Handle(GetPvAddResourceEmpToResMapsByReqIdById request, CancellationToken cancellationToken)
        {
            try
            {
                var PvAddResourceReq = _context.PvAddResorceHeads.AsNoTracking().FirstOrDefault(e => e.Id == request.Id);
                List<TblOpPvAddResourceEmployeeToResourceMapDto>MappingsList = _context.TblOpPvAddResourceEmployeeToResourceMaps.AsNoTracking().ProjectTo<TblOpPvAddResourceEmployeeToResourceMapDto>(_mapper.ConfigurationProvider).Where(e => e.PvAddResReqId == request.Id).ToList();

                if (PvAddResourceReq is not null) 
                {

                    if (MappingsList.Count == 0)
                    {
                        var AddResReqs = _context.PvAddResorces.AsNoTracking().Where(r=>r.AddResReqHeadId==request.Id).ToList();
                        if (AddResReqs.Count != 0)
                        {
                            AddResReqs.ForEach(r=>{

                                for (int i=1;i<=r.Qty;i++)
                                {
                                    TblOpPvAddResourceEmployeeToResourceMapDto map = new()
                                    {
                                        MapId = 0,
                                        PvAddResReqId = request.Id,
                                        ProjectCode = PvAddResourceReq.ProjectCode,
                                        SiteCode = PvAddResourceReq.SiteCode,
                                        SkillSet = r.SkillsetCode,
                                        EmployeeNumber="",
                                        FromDate=r.FromDate,
                                        ToDate=r.ToDate,
                                         DefShift="",
                                         OffDay=-1,
                                    };

                                    MappingsList.Add(map);
                                }
                                
                            
                            });
                        }

                       
                    }

                }


                return MappingsList;



            }
            catch (Exception ex)
            {
                Log.Error("Error in GetPvAddResourceEmpToResMapsByReqIdByIdHandler Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion


    #region IsValidPvAddResourceEmployeeToResourceMap
    public class IsValidPvAddResourceEmployeeToResourceMap : IRequest<ValidityCheckDto>
    {
        public UserIdentityDto User { get; set; }
        public InputPvAddResourceEmployeeToResourceMap InputDto { get; set; }
    }

    public class IsValidPvAddResourceEmployeeToResourceMapHandler : IRequestHandler<IsValidPvAddResourceEmployeeToResourceMap, ValidityCheckDto>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public IsValidPvAddResourceEmployeeToResourceMapHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<ValidityCheckDto> Handle(IsValidPvAddResourceEmployeeToResourceMap request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {



                try
                {

                    Log.Info("----Info IsValidPvAddResourceEmployeeToResourceMap method start----");
                    if (request.InputDto.MappingsList[0].PvAddResReqId > 0 )
                    {
                        var AddReq = await _context.PvAddResorceHeads.FirstOrDefaultAsync(e => e.Id == request.InputDto.MappingsList[0].PvAddResReqId);
                        if (!AddReq.IsApproved)
                        {
                            return new() { IsValidReq = false, ErrorId = -1, ErrorMsg = "Request Not Approved" };

                        }

                    }

                    if (!(await _context.OP_HRM_TEMP_Projects.AnyAsync(e => e.ProjectCode == request.InputDto.MappingsList[0].ProjectCode)))
                    {
                        return new() { IsValidReq = false, ErrorId = -1, ErrorMsg = "Invalid ProjectCode" };
                    }

                    if (!await _context.OprSites.AnyAsync(e => e.SiteCode == request.InputDto.MappingsList[0].SiteCode))
                    {
                        return new() { IsValidReq = false, ErrorId = -2, ErrorMsg = "Invalid SiteCode" };
                    }

                    for (int i=0;i< request.InputDto.MappingsList.Count;i++)

                    {
                        if (!(await _contextDMC.HRM_TRAN_Employees.AnyAsync(e => e.EmployeeNumber == request.InputDto.MappingsList[i].EmployeeNumber)))
                        {
                            return new() { IsValidReq = false, ErrorId = -3, ErrorMsg = "Invalid EmployeeNumber:" + request.InputDto.MappingsList[i].EmployeeNumber};
                        }

                         if (await _context.EmployeeAttendance.AnyAsync(e => e.EmployeeNumber == request.InputDto.MappingsList[i].EmployeeNumber 
                         && e.ProjectCode == request.InputDto.MappingsList[i].ProjectCode
                         && e.SiteCode == request.InputDto.MappingsList[i].SiteCode
                         && (e.AttnDate >=request.InputDto.MappingsList[i].FromDate)
                         ))
                        {
                            var existAtt = await _context.EmployeeAttendance.OrderByDescending(e=>e.AttnDate).FirstOrDefaultAsync(e => e.EmployeeNumber == request.InputDto.MappingsList[i].EmployeeNumber
                           && e.ProjectCode == request.InputDto.MappingsList[i].ProjectCode
                           && e.SiteCode == request.InputDto.MappingsList[i].SiteCode
                           && (e.AttnDate >= request.InputDto.MappingsList[i].FromDate));

                            return new() { IsValidReq = false, ErrorId = -4, ErrorMsg = "Employee Attendance Already Exist, Employee Number:" + request.InputDto.MappingsList[i].EmployeeNumber+",Recent Date:"+existAtt.AttnDate.ToString()};
                        }

                        if (await _context.TblOpMonthlyRoasterForSites.AnyAsync(e => e.EmployeeNumber == request.InputDto.MappingsList[i].EmployeeNumber
                        && e.ProjectCode== request.InputDto.MappingsList[i].ProjectCode
                        && e.SiteCode==request.InputDto.MappingsList[i].SiteCode
                        && e.IsPrimaryResource
                        && ((e.MonthStartDate>=request.InputDto.MappingsList[i].FromDate && e.MonthStartDate <= request.InputDto.MappingsList[i].ToDate) || (e.MonthEndDate >= request.InputDto.MappingsList[i].FromDate && e.MonthEndDate <= request.InputDto.MappingsList[i].ToDate))
                        ))
                        {
                            return new() { IsValidReq = false, ErrorId = -5, ErrorMsg = "Employee Already Exist:" + request.InputDto.MappingsList[i].EmployeeNumber };
                        }

                        if (!(await _contextDMC.HRM_TRAN_EmployeePrimarySites_Logs.AnyAsync(e=>e.EmployeeNumber== request.InputDto.MappingsList[i].EmployeeNumber
                        && e.TransferredDate<= request.InputDto.MappingsList[i].FromDate))) { 
                            return new() { IsValidReq = false, ErrorId = -6, ErrorMsg = "Employee Primary Site Log Not Exist:" + request.InputDto.MappingsList[i].EmployeeNumber };

                        }
                    }


                    return new() { IsValidReq = true };


                    //latest count=-13

                }
                catch (Exception ex)
                {

                    Log.Error("Error in IsValidPvAddResourceEmployeeToResourceMapHandler Method");
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



    #region MergePvAddResourceReqById
    public class MergePvAddResourceReqById : IRequest<TblOpPvAddResourceReqHead>
    {
        public UserIdentityDto User { get; set; }
        public long Id { get; set; }
    }

    public class MergePvAddResourceReqByIdHandler : IRequestHandler<MergePvAddResourceReqById, TblOpPvAddResourceReqHead>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public MergePvAddResourceReqByIdHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
          _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<TblOpPvAddResourceReqHead> Handle(MergePvAddResourceReqById request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                using (var transaction2 = await _contextDMC.Database.BeginTransactionAsync())
                {

                    try
                    {
                        Log.Info("----Info MergePvAddResourceReqById method start----");




                        var PvAddResourceReq = await _context.PvAddResorceHeads.FirstOrDefaultAsync(e => e.Id == request.Id);
                        if (PvAddResourceReq is null)
                        {
                            return new() { Id = -1 };       // Request_not_exist
                        }
                        if (!PvAddResourceReq.IsApproved)
                        {
                            return new() { Id = -2 };      //Request_Not_Approved
                        }
                        var projectData = await _context.TblOpProjectSites.AsNoTracking().FirstOrDefaultAsync(p => p.ProjectCode == PvAddResourceReq.ProjectCode);
                        if (projectData is null)
                        {
                            return new() { Id = -3 };      //invalid_projec_code
                        }

                        var Mappings = await _context.TblOpPvAddResourceEmployeeToResourceMaps.AsNoTracking().Where(e => e.PvAddResReqId == request.Id).ToListAsync();
                        if (Mappings.Count == 0)
                        {
                            return new() { Id = -4 };          //No_Reource_Mapping_Exist
                        }
                        var ProjectData = await _context.TblOpProjectSites.AsNoTracking().FirstOrDefaultAsync(e => e.ProjectCode == Mappings[0].ProjectCode);
                        if (ProjectData is null || ProjectData.Id == 0)
                        {
                            return new() { Id = -5 };       //Invalid_Project_Code
                        }

                        var ShiftsForSite = await _context.TblOpShiftsPlanForProjects.AsNoTracking().Where(e => e.SiteCode == Mappings[0].SiteCode
                        && e.ProjectCode == Mappings[0].ProjectCode).ToListAsync();

                        var ShiftsFromMaster = await _contextDMC.HRM_DEF_EmployeeShiftMasters.AsNoTracking().ToListAsync();

                        if (ShiftsForSite.Count == 0)
                        {

                            return new() { Id = -6 };          //Shift_Plan_Not_Exist_for_Project/Site
                        }
                        string OffShift = "";
                        for (int i = 0; i < ShiftsForSite.Count; i++)
                        {
                            if (ShiftsFromMaster.Any(s => s.ShiftCode == ShiftsForSite[i].ShiftCode && s.IsOff.Value))
                            {
                                OffShift = ShiftsForSite[i].ShiftCode;
                                break;
                            }
                        }




                        List<TblOpMonthlyRoasterForSite> NewRoasters = new();
                        List<TblOpMonthlyRoasterForSite> NonPrimaryRoasters = new();

                        List<HRM_DEF_EmployeeOff> EmployeeOffs = new();

                        for (int i = 0; i < Mappings.Count; i++)
                        {

                            string EmployeeNumber = Mappings[i].EmployeeNumber;
                            long EmployeeID = _contextDMC.HRM_TRAN_Employees.FirstOrDefault(e => e.EmployeeNumber == Mappings[i].EmployeeNumber).EmployeeID;
                            string SkillSet = Mappings[i].SkillSet;
                            DateTime FromDate = (DateTime)Mappings[i].FromDate;
                            DateTime ToDate = (DateTime)Mappings[i].ToDate;
                            string ProjectCode = Mappings[i].ProjectCode;
                            string CustomerCode = ProjectData.CustomerCode;
                            string SiteCode = Mappings[i].SiteCode;
                            string DefShift = Mappings[i].DefShift;
                            int OffDay = Mappings[i].OffDay;
                            if (OffShift == "" && OffDay == -1)
                            {
                                return new() { Id = -7 };            //No_Off_Shifts_Found_in_Site

                            }

                            if (FromDate > ProjectData.EndDate || ToDate < ProjectData.StartDate)
                            {
                                return new() { Id = -8 };       //Incompatible_Dates_with_project_Dates

                            }



                            bool IsExistEmployee = await _context.TblOpMonthlyRoasterForSites.AsNoTracking().AnyAsync(e => e.ProjectCode == ProjectCode &&
                            e.SiteCode == SiteCode &&
                            e.EmployeeNumber == EmployeeNumber &&
                            e.MonthStartDate >= FromDate &&
                            e.MonthEndDate <= ToDate);

                            if (IsExistEmployee)
                            {
                                return new() { Id = -9 };       //Employee_Already_Exists

                            }

                            int fromMonth = FromDate.Month;
                            int fromYear = FromDate.Year;
                            int ToMonth = ToDate.Month;
                            int ToYear = ToDate.Year;


                            for (DateTime date = new DateTime(fromYear, fromMonth, 1); date <= new DateTime(ToYear, ToMonth, DateTime.DaysInMonth(ToYear, ToMonth));)
                            {

                                int startDay = (int)date.DayOfWeek;
                                int endDay = DateTime.DaysInMonth(date.Year, date.Month);



                                TblOpMonthlyRoasterForSite NewRoaster = new();

                                NewRoaster.Id = 0;
                                NewRoaster.EmployeeNumber = EmployeeNumber;
                                NewRoaster.ProjectCode = ProjectCode;
                                NewRoaster.EmployeeID = 0;
                                NewRoaster.CustomerCode = CustomerCode;
                                NewRoaster.SiteCode = SiteCode;
                                NewRoaster.IsPrimaryResource = true;
                                NewRoaster.MapId = 0;
                                NewRoaster.MonthStartDate = new DateTime(date.Year, date.Month, 1);
                                NewRoaster.MonthEndDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
                                NewRoaster.SkillsetCode = Mappings[i].SkillSet;
                                NewRoaster.Month = (short)date.Month;
                                NewRoaster.Year = (short)date.Year;
                                NewRoaster.S1 = date < FromDate || date > ToDate ? "x" : OffDay == -1 ? DefShift : startDay == OffDay ? OffShift : DefShift;
                                NewRoaster.S2 = new DateTime(date.Year, date.Month, 2) < FromDate || new DateTime(date.Year, date.Month, 2) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 1) % 7 == OffDay ? OffShift : DefShift;
                                NewRoaster.S3 = new DateTime(date.Year, date.Month, 3) < FromDate || new DateTime(date.Year, date.Month, 3) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 2) % 7 == OffDay ? OffShift : DefShift;
                                NewRoaster.S4 = new DateTime(date.Year, date.Month, 4) < FromDate || new DateTime(date.Year, date.Month, 4) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 3) % 7 == OffDay ? OffShift : DefShift;
                                NewRoaster.S5 = new DateTime(date.Year, date.Month, 5) < FromDate || new DateTime(date.Year, date.Month, 5) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 4) % 7 == OffDay ? OffShift : DefShift;
                                NewRoaster.S6 = new DateTime(date.Year, date.Month, 6) < FromDate || new DateTime(date.Year, date.Month, 6) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 5) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S7 = new DateTime(date.Year, date.Month, 7) < FromDate || new DateTime(date.Year, date.Month, 7) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 6) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S8 = new DateTime(date.Year, date.Month, 8) < FromDate || new DateTime(date.Year, date.Month, 8) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 7) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S9 = new DateTime(date.Year, date.Month, 9) < FromDate || new DateTime(date.Year, date.Month, 9) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 8) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S10 = new DateTime(date.Year, date.Month, 10) < FromDate || new DateTime(date.Year, date.Month, 10) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 9) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S11 = new DateTime(date.Year, date.Month, 11) < FromDate || new DateTime(date.Year, date.Month, 11) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 10) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S12 = new DateTime(date.Year, date.Month, 12) < FromDate || new DateTime(date.Year, date.Month, 12) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 11) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S13 = new DateTime(date.Year, date.Month, 13) < FromDate || new DateTime(date.Year, date.Month, 13) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 12) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S14 = new DateTime(date.Year, date.Month, 14) < FromDate || new DateTime(date.Year, date.Month, 14) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 13) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S15 = new DateTime(date.Year, date.Month, 15) < FromDate || new DateTime(date.Year, date.Month, 15) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 14) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S16 = new DateTime(date.Year, date.Month, 16) < FromDate || new DateTime(date.Year, date.Month, 16) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 15) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S17 = new DateTime(date.Year, date.Month, 17) < FromDate || new DateTime(date.Year, date.Month, 17) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 16) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S18 = new DateTime(date.Year, date.Month, 18) < FromDate || new DateTime(date.Year, date.Month, 18) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 17) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S19 = new DateTime(date.Year, date.Month, 19) < FromDate || new DateTime(date.Year, date.Month, 19) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 18) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S20 = new DateTime(date.Year, date.Month, 20) < FromDate || new DateTime(date.Year, date.Month, 20) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 19) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S21 = new DateTime(date.Year, date.Month, 21) < FromDate || new DateTime(date.Year, date.Month, 21) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 20) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S22 = new DateTime(date.Year, date.Month, 22) < FromDate || new DateTime(date.Year, date.Month, 22) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 21) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S23 = new DateTime(date.Year, date.Month, 23) < FromDate || new DateTime(date.Year, date.Month, 23) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 22) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S24 = new DateTime(date.Year, date.Month, 24) < FromDate || new DateTime(date.Year, date.Month, 24) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 23) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S25 = new DateTime(date.Year, date.Month, 25) < FromDate || new DateTime(date.Year, date.Month, 25) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 24) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S26 = new DateTime(date.Year, date.Month, 26) < FromDate || new DateTime(date.Year, date.Month, 26) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 25) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S27 = new DateTime(date.Year, date.Month, 27) < FromDate || new DateTime(date.Year, date.Month, 27) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 26) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S28 = new DateTime(date.Year, date.Month, 28) < FromDate || new DateTime(date.Year, date.Month, 28) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 27) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S29 = endDay < 29 ? "x" : new DateTime(date.Year, date.Month, 29) < FromDate || new DateTime(date.Year, date.Month, 29) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 28) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S30 = endDay < 30 ? "x" : new DateTime(date.Year, date.Month, 30) < FromDate || new DateTime(date.Year, date.Month, 30) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 29) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoaster.S31 = endDay < 31 ? "x" : new DateTime(date.Year, date.Month, 31) < FromDate || new DateTime(date.Year, date.Month, 31) > ToDate ? "x" : OffDay == -1 ? DefShift : (startDay + 30) % 7 == OffDay ? OffShift : DefShift; ;
                                NewRoasters.Add(NewRoaster);
                                TblOpMonthlyRoasterForSite NonPrimaryRoaster= await _context.TblOpMonthlyRoasterForSites.AsNoTracking().FirstOrDefaultAsync(e =>
                                  e.EmployeeNumber == NewRoaster.EmployeeNumber
                                  && e.Month == NewRoaster.Month
                                  && e.Year == NewRoaster.Year
                                  && e.IsPrimaryResource == false);
                                if (NonPrimaryRoaster is not null)
                                {
                                    NonPrimaryRoasters.Add(NonPrimaryRoaster);
                                }




                                for (int d = 1; d <= 31; d++)
                                {
                                    HRM_DEF_EmployeeOff empOff = new();

                                    empOff.ID = 0;
                                    empOff.EmployeeId = EmployeeID;
                                    empOff.SiteCode = SiteCode;


                                    switch (d)
                                    {



                                        case 1:
                                            if (NewRoaster.S1 != "x" && NewRoaster.S1 != "" && NewRoaster.S1 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);

                                            }
                                            break;
                                        case 2:
                                            if (NewRoaster.S2 != "x" && NewRoaster.S2 != "" && NewRoaster.S2 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;
                                        case 3:
                                            if (NewRoaster.S3 != "x" && NewRoaster.S3 != "" && NewRoaster.S3 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;
                                        case 4:
                                            if (NewRoaster.S4 != "x" && NewRoaster.S4 != "" && NewRoaster.S4 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;
                                        case 5:
                                            if (NewRoaster.S5 != "x" && NewRoaster.S5 != "" && NewRoaster.S5 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;
                                        case 6:
                                            if (NewRoaster.S6 != "x" && NewRoaster.S6 != "" && NewRoaster.S6 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;
                                        case 7:
                                            if (NewRoaster.S7 != "x" && NewRoaster.S7 != "" && NewRoaster.S7 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;
                                        case 8:
                                            if (NewRoaster.S8 != "x" && NewRoaster.S8 != "" && NewRoaster.S8 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;
                                        case 9:
                                            if (NewRoaster.S9 != "x" && NewRoaster.S9 != "" && NewRoaster.S9 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;
                                        case 10:
                                            if (NewRoaster.S10 != "x" && NewRoaster.S10 != "" && NewRoaster.S10 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;
                                        case 11:
                                            if (NewRoaster.S11 != "x" && NewRoaster.S11 != "" && NewRoaster.S11 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;
                                        case 12:
                                            if (NewRoaster.S12 != "x" && NewRoaster.S12 != "" && NewRoaster.S12 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;
                                        case 13:
                                            if (NewRoaster.S13 != "x" && NewRoaster.S13 != "" && NewRoaster.S13 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;
                                        case 14:
                                            if (NewRoaster.S14 != "x" && NewRoaster.S14 != "" && NewRoaster.S14 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;
                                        case 15:
                                            if (NewRoaster.S15 != "x" && NewRoaster.S15 != "" && NewRoaster.S15 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;
                                        case 16:
                                            if (NewRoaster.S16 != "x" && NewRoaster.S16 != "" && NewRoaster.S16 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;
                                        case 17:
                                            if (NewRoaster.S17 != "x" && NewRoaster.S17 != "" && NewRoaster.S17 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;
                                        case 18:
                                            if (NewRoaster.S18 != "x" && NewRoaster.S18 != "" && NewRoaster.S18 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;
                                        case 19:
                                            if (NewRoaster.S19 != "x" && NewRoaster.S19 != "" && NewRoaster.S19 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;
                                        case 20:
                                            if (NewRoaster.S20 != "x" && NewRoaster.S20 != "" && NewRoaster.S20 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;
                                        case 21:
                                            if (NewRoaster.S21 != "x" && NewRoaster.S21 != "" && NewRoaster.S21 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;


                                        case 22:
                                            if (NewRoaster.S22 != "x" && NewRoaster.S22 != "" && NewRoaster.S22 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;


                                        case 23:
                                            if (NewRoaster.S23 != "x" && NewRoaster.S23 != "" && NewRoaster.S23 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;


                                        case 24:
                                            if (NewRoaster.S24 != "x" && NewRoaster.S24 != "" && NewRoaster.S24 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;


                                        case 25:
                                            if (NewRoaster.S25 != "x" && NewRoaster.S25 != "" && NewRoaster.S25 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;


                                        case 26:
                                            if (NewRoaster.S26 != "x" && NewRoaster.S26 != "" && NewRoaster.S26 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;


                                        case 27:
                                            if (NewRoaster.S27 != "x" && NewRoaster.S27 != "" && NewRoaster.S27 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;


                                        case 28:
                                            if (NewRoaster.S28 != "x" && NewRoaster.S28 != "" && NewRoaster.S28 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;


                                        case 29:
                                            if (NewRoaster.S29 != "x" && NewRoaster.S29 != "" && NewRoaster.S29 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;


                                        case 30:
                                            if (NewRoaster.S30 != "x" && NewRoaster.S30 != "" && NewRoaster.S30 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;


                                        case 31:
                                            if (NewRoaster.S31 != "x" && NewRoaster.S31 != "" && NewRoaster.S31 == OffShift)
                                            {
                                                empOff.Date = new DateTime(date.Year, date.Month, d);
                                                EmployeeOffs.Add(empOff);
                                            }
                                            break;
                                    }


                                }



                                if (fromMonth == 12)
                                {
                                    fromMonth = 1;
                                    fromYear++;
                                }
                                else
                                {
                                    fromMonth++;
                                }

                                date = new DateTime(fromYear, fromMonth, 1);

                            }

                        }

                        if (EmployeeOffs.Count > 0)
                        {
                            await _contextDMC.HRM_DEF_EmployeeOffs.AddRangeAsync(EmployeeOffs);
                            await _contextDMC.SaveChangesAsync();
                        }
                        await _context.TblOpMonthlyRoasterForSites.AddRangeAsync(NewRoasters);
                        await _context.SaveChangesAsync();

                        _context.TblOpMonthlyRoasterForSites.RemoveRange(NonPrimaryRoasters);
                        await _context.SaveChangesAsync();




                        PvAddResourceReq.IsApproved = true;
                        PvAddResourceReq.ModifiedBy = request.User.UserId;
                        PvAddResourceReq.Modified = DateTime.UtcNow;
                        PvAddResourceReq.IsMerged = true;
                        _context.PvAddResorceHeads.Update(PvAddResourceReq);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                        await transaction2.CommitAsync();
                        return PvAddResourceReq;

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        transaction2.Rollback();
                        Log.Error("Error in MergePvAddResourceReqByIdHandler Method");
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
}
