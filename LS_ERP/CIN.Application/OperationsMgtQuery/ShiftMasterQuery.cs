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


namespace CIN.Application.OperationsMgtQuery
{
    #region shiftMaster
    #region GetShiftMastersPagedList

    public class GetShiftMastersPagedList : IRequest<PaginatedList<HRM_DEF_EmployeeShiftMasterPaginationDto>>
    {
        public UserIdentityDto User { get; set; }
        public PaginationFilterDto Input { get; set; }
    }

    public class GetShiftMastersPagedListHandler : IRequestHandler<GetShiftMastersPagedList, PaginatedList<HRM_DEF_EmployeeShiftMasterPaginationDto>>
    {
        //private readonly CINDBOneContext _context;
        private readonly DMCContext _context;
        private readonly IMapper _mapper;
        public GetShiftMastersPagedListHandler(DMCContext context, AutoMapper.IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<HRM_DEF_EmployeeShiftMasterPaginationDto>> Handle(GetShiftMastersPagedList request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Info("----Info GetShiftMastersPagedList method start----");
                var search = request.Input.Query;
            var list = await _context.HRM_DEF_EmployeeShiftMasters.AsNoTracking()
              .Where(e =>
                            (e.ShiftCode.Contains(search) ||
                            e.ShiftName_AR.Contains(search) ||
                            e.ShiftName_EN.Contains(search)||
                            search == "" || search == null
                             ))
               .OrderBy(request.Input.OrderBy)
              .ProjectTo<HRM_DEF_EmployeeShiftMasterPaginationDto>(_mapper.ConfigurationProvider)
                 .PaginationListAsync(request.Input.Page, request.Input.PageCount, cancellationToken);

            return list;
        }
            catch (Exception ex)
            {
                Log.Error("Error in GetShiftMastersPagedList Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }
    #endregion

    //#region CreateUpdateShiftMaster
    //public class CreateShiftMaster : IRequest<long>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public HRM_DEF_EmployeeShiftMasterAddUpdateDto ShiftMasterDto { get; set; }
    //}

    //public class CreateShiftMasterHandler : IRequestHandler<CreateShiftMaster, long>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;

    //    public CreateShiftMasterHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }

    //    public async Task<long> Handle(CreateShiftMaster request, CancellationToken cancellationToken)
    //    {
    //        try
    //        {
    //            Log.Info("----Info CreateUpdateShiftMaster method start----");



    //            var obj = request.ShiftMasterDto;


    //            HRM_DEF_EmployeeShiftMaster ShiftMaster = new();
    //            if (obj.ShiftId > 0)
    //                ShiftMaster = await _context.HRM_DEF_EmployeeShiftMasters.AsNoTracking().FirstOrDefaultAsync(e => e.ShiftId == obj.ShiftId);
    //            else
    //            {
    //                if (_context.HRM_DEF_EmployeeShiftMasters.Any(x => x.ShiftCode == obj.ShiftCode))
    //                {
    //                    return -1;
    //                }
    //                ShiftMaster.ShiftCode = obj.ShiftCode.ToUpper();
    //            }

    //            ShiftMaster.ShiftName_EN = obj.ShiftName_EN;
    //            ShiftMaster.ShiftName_AR = obj.ShiftName_AR;
    //            ShiftMaster.BreakTime = TimeSpan.Parse("00:" + obj.BreakTime);
    //            ShiftMaster.InGrace = TimeSpan.Parse("00:" + obj.InGrace);
    //            ShiftMaster.InTime = TimeSpan.Parse(obj.InTime);
    //            ShiftMaster.IsOff = obj.IsOff;
    //            ShiftMaster.OutGrace = TimeSpan.Parse("00:" + obj.OutGrace); ;
    //            ShiftMaster.OutTime = TimeSpan.Parse(obj.OutTime);
               

    //            //TimeSpan inTime = TimeSpan.Parse(obj.InTime);
    //            //TimeSpan outTime = TimeSpan.Parse(obj.OutTime);
    //            //TimeSpan breakTime = TimeSpan.Parse("00:"+obj.BreakTime);
    //            TimeSpan workingTime = ShiftMaster.OutTime> ShiftMaster.InTime?TimeSpan.Parse((ShiftMaster.OutTime - ShiftMaster.InTime).ToString()): TimeSpan.Parse((TimeSpan.Parse("23:59:59")+ShiftMaster.OutTime -ShiftMaster.InTime+TimeSpan.Parse("00:00:01")).ToString());
    //            TimeSpan netWorkingTime = TimeSpan.Parse((workingTime - ShiftMaster.BreakTime).ToString());






    //            ShiftMaster.WorkingTime = workingTime; ;



    //            ShiftMaster.NetWorkingTime = netWorkingTime;

    //            if (obj.ShiftId > 0)
    //            {
    //                ShiftMaster.ShiftCode = obj.ShiftCode;
    //                _context.HRM_DEF_EmployeeShiftMasters.Update(ShiftMaster);
    //            }
    //            else
    //            {
    //                await _context.HRM_DEF_EmployeeShiftMasters.AddAsync(ShiftMaster);
    //            }
    //            await _context.SaveChangesAsync();
    //            Log.Info("----Info CreateUpdateShiftMaster method Exit----");
    //            return ShiftMaster.ShiftId;
    //        }
    //        catch (Exception ex)
    //        {
    //            Log.Error("Error in CreateUpdateShiftMaster Method");
    //            Log.Error("Error occured time : " + DateTime.UtcNow);
    //            Log.Error("Error message : " + ex.Message);
    //            Log.Error("Error StackTrace : " + ex.StackTrace);
    //            return 0;
    //        }
    //    }
    //}

    //#endregion

  




    #region GetShiftMasterByShiftMasterCode
    public class GetShiftMasterByShiftMasterCode : IRequest<HRM_DEF_EmployeeShiftMasterAddUpdateDto>
    {
        public UserIdentityDto User { get; set; }
        public string ShiftMasterCode { get; set; }
    }

    public class GetShiftMasterByShiftMasterCodeHandler : IRequestHandler<GetShiftMasterByShiftMasterCode, HRM_DEF_EmployeeShiftMasterAddUpdateDto>
    {
        //private readonly CINDBOneContext _context;
        private readonly DMCContext _context;
        private readonly IMapper _mapper;
        public GetShiftMasterByShiftMasterCodeHandler(DMCContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<HRM_DEF_EmployeeShiftMasterAddUpdateDto> Handle(GetShiftMasterByShiftMasterCode request, CancellationToken cancellationToken)
        {

            try
            {
                var ShiftMaster = await _context.HRM_DEF_EmployeeShiftMasters.AsNoTracking().FirstOrDefaultAsync(e => e.ShiftCode == request.ShiftMasterCode);

                if (ShiftMaster != null)
                {
                    HRM_DEF_EmployeeShiftMasterAddUpdateDto res = new HRM_DEF_EmployeeShiftMasterAddUpdateDto();
                    res.ShiftId = ShiftMaster.ShiftId;
                    res.ShiftName_EN = ShiftMaster.ShiftName_EN;
                    res.ShiftName_AR = ShiftMaster.ShiftName_AR;
                    res.BreakTime = short.Parse(ShiftMaster.BreakTime.ToString().Substring(3, 2));
                   

                    res.InGrace = short.Parse(ShiftMaster.InGrace.ToString().Substring(3, 2));
                    res.OutGrace = short.Parse(ShiftMaster.OutGrace.ToString().Substring(3, 2));

                    //res.InGrace = ShiftMaster.InGrace;
                    //res.OutGrace =ShiftMaster.OutGrace;
                    res.IsOff = ShiftMaster.IsOff;
                    res.NetWorkingTime = ShiftMaster.NetWorkingTime.ToString().Substring(0, 5);
                   
                    res.ShiftCode = ShiftMaster.ShiftCode;
                    res.WorkingTime = ShiftMaster.WorkingTime.ToString().Substring(0, 5);
                    res.OutTime = ShiftMaster.OutTime.ToString().Substring(0, 5);
                    res.InTime = ShiftMaster.InTime.ToString().Substring(0, 5);
                    return res;
                }

                else return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetShiftMasterById");
                return null;
            }
        }
    }

    #endregion


    


    #region GetShiftMasterById
    public class GetShiftMasterById : IRequest<HRM_DEF_EmployeeShiftMasterAddUpdateDto>
    {
        public UserIdentityDto User { get; set; }
        public int Id { get; set; }
    }

    public class GetShiftMasterByIdHandler : IRequestHandler<GetShiftMasterById, HRM_DEF_EmployeeShiftMasterAddUpdateDto>
    {
       // private readonly CINDBOneContext _context;
        private readonly DMCContext _context;
        private readonly IMapper _mapper;
        public GetShiftMasterByIdHandler(DMCContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<HRM_DEF_EmployeeShiftMasterAddUpdateDto> Handle(GetShiftMasterById request, CancellationToken cancellationToken)
        {
            try
            {
                var ShiftMaster = await _context.HRM_DEF_EmployeeShiftMasters.AsNoTracking().FirstOrDefaultAsync(e => e.ShiftId == request.Id);

                if (ShiftMaster != null)
                {
                    HRM_DEF_EmployeeShiftMasterAddUpdateDto res = new HRM_DEF_EmployeeShiftMasterAddUpdateDto();
                    res.ShiftId = ShiftMaster.ShiftId;
                    res.ShiftName_EN = ShiftMaster.ShiftName_EN;
                    res.ShiftName_AR = ShiftMaster.ShiftName_AR;
                    res.BreakTime = short.Parse(ShiftMaster.BreakTime.ToString().Substring(3, 2));
                    res.InGrace = short.Parse(ShiftMaster.InGrace.ToString().Substring(3, 2));
                    res.InTime = ShiftMaster.InTime.ToString().Substring(0, 5);
                    res.IsOff = ShiftMaster.IsOff;
                    res.NetWorkingTime = ShiftMaster.NetWorkingTime.ToString().Substring(0, 5);
                    res.OutGrace = short.Parse(ShiftMaster.OutGrace.ToString().Substring(3, 2));
                    res.ShiftCode = ShiftMaster.ShiftCode;
                    res.WorkingTime = ShiftMaster.WorkingTime.ToString().Substring(0, 5);
                    res.OutTime = ShiftMaster.OutTime.ToString().Substring(0, 5);
                    return res;
                }

                else return null;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetShiftMasterById");
                return null;
            }
        }
    }

    #endregion

    #region GetSelectShiftMasterList

    public class GetSelectShiftMasterList : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
    }

    public class GetSelectShiftMasterListHandler : IRequestHandler<GetSelectShiftMasterList, List<CustomSelectListItem>>
    {
        //private readonly CINDBOneContext _context;
        private readonly DMCContext _context;
        private readonly IMapper _mapper;
        public GetSelectShiftMasterListHandler(DMCContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectShiftMasterList request, CancellationToken cancellationToken)
        {

            var list = await _context.HRM_DEF_EmployeeShiftMasters.AsNoTracking()
              .Select(e => new CustomSelectListItem { Text = e.ShiftName_EN, Value = e.ShiftCode, TextTwo = e.IsOff.ToString() })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion


    #region GetSelectShiftMasterListForProjectSite         // based on sitecode we can get working hours and it filters

    public class GetSelectShiftMasterListForProjectSite : IRequest<List<CustomSelectListItem>>
    {
        public UserIdentityDto User { get; set; }
        public string projectCode { get; set; }
        public string siteCode { get; set; }
    }

    public class GetSelectShiftMasterListForProjectSiteHandler : IRequestHandler<GetSelectShiftMasterListForProjectSite, List<CustomSelectListItem>>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public GetSelectShiftMasterListForProjectSiteHandler(DMCContext contextDMC, CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<List<CustomSelectListItem>> Handle(GetSelectShiftMasterListForProjectSite request, CancellationToken cancellationToken)
        {
            var projectSite = await _context.TblOpProjectSites.FirstOrDefaultAsync(e=>e.SiteCode==request.siteCode && e.ProjectCode==request.projectCode);
            TimeSpan whrs =new TimeSpan(projectSite.SiteWorkingHours??0,0,0);
            var list = await _contextDMC.HRM_DEF_EmployeeShiftMasters.Where(s=>s.WorkingTime.Value== whrs
            || s.WorkingTime.Value==TimeSpan.Parse("00:00:00")
            ).Select(e => new CustomSelectListItem { Text = e.ShiftName_EN, Value = e.ShiftCode, TextTwo = e.IsOff.ToString() })
                 .ToListAsync(cancellationToken);
            return list;
        }
    }

    #endregion

    


    //#region DeleteShiftMaster
    //public class DeleteShiftMaster : IRequest<int>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public int Id { get; set; }
    //}

    //public class DeleteShiftMasterQueryHandler : IRequestHandler<DeleteShiftMaster, int>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;

    //    public DeleteShiftMasterQueryHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }

    //    public async Task<int> Handle(DeleteShiftMaster request, CancellationToken cancellationToken)
    //    {
    //        try
    //        {
    //            Log.Info("----Info DeleteShiftMaster start----");

    //            if (request.Id > 0)
    //            {
    //                var ShiftMaster = await _context.HRM_DEF_EmployeeShiftMasters.FirstOrDefaultAsync(e => e.ShiftId == request.Id);
    //                _context.Remove(ShiftMaster);

    //                await _context.SaveChangesAsync();

    //                return request.Id;
    //            }
    //            return 0;
    //        }
    //        catch (Exception ex)
    //        {

    //            Log.Error("Error in DeleteShiftMaster");
    //            Log.Error("Error occured time : " + DateTime.UtcNow);
    //            Log.Error("Error message : " + ex.Message);
    //            Log.Error("Error StackTrace : " + ex.StackTrace);
    //            return 0;
    //        }

    //    }
    //}

    //#endregion


    #endregion
    #region ShiftsToSite

    //#region CreateUpdateShiftsToSiteMap
    //public class CreateUpdateShiftsToSiteMap : IRequest<int>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public OpShiftSiteMapDto Shifts { get; set; }
    //}

    //public class CreateUpdateShiftsToSiteMapHandler : IRequestHandler<CreateUpdateShiftsToSiteMap, int>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly IMapper _mapper;

    //    public CreateUpdateShiftsToSiteMapHandler(CINDBOneContext context, IMapper mapper)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //    }

    //    public async Task<int> Handle(CreateUpdateShiftsToSiteMap request, CancellationToken cancellationToken)
    //    {
    //        using (var transaction = await _context.Database.BeginTransactionAsync())
    //        {
    //            try
    //            {
    //                Log.Info("----Info CreateUpdateShiftsToSiteMap method start----");
    //                var obj = request.Shifts;

    //                if (request.Shifts.ShiftsList.Count() > 0)
    //                {
    //                    var oldShiftsList = await _context.TblOpShiftSiteMaps.Where(e => e.SiteCode == request.Shifts.SiteCode).ToListAsync();
    //                    _context.TblOpShiftSiteMaps.RemoveRange(oldShiftsList);

    //                    List<TblOpShiftSiteMap> ShiftsList = new();
    //                    foreach (var shft in request.Shifts.ShiftsList)
    //                    {
    //                        TblOpShiftSiteMap shift = new()
    //                        {
    //                            SiteCode = request.Shifts.SiteCode,
    //                            ShiftCode = shft.ShiftCode

    //                        };


    //                        ShiftsList.Add(shift);
    //                    }
    //                    await _context.TblOpShiftSiteMaps.AddRangeAsync(ShiftsList);
    //                    await _context.SaveChangesAsync();
    //                }
    //                Log.Info("----Info CreateUpdateServicesEnquiryForm method Exit----");

    //                await transaction.CommitAsync();
    //                return 1;

    //            }
    //            catch (Exception ex)
    //            {
    //                Log.Error("Error in CreateUpdateShiftsToSiteMap Method");
    //                Log.Error("Error occured time : " + DateTime.UtcNow);
    //                Log.Error("Error message : " + ex.Message);
    //                Log.Error("Error StackTrace : " + ex.StackTrace);
    //                return 0;

    //            }
    //        }
    //    }
    //}

    //#endregion


    //#region GetShiftsToSiteBySiteCode
    //public class GetShiftsToSiteBySiteCode : IRequest<List<HRM_DEF_EmployeeShiftMasterDto>>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public string SiteCode { get; set; }
    //}

    //public class GetShiftsToSiteBySiteCodeHandler : IRequestHandler<GetShiftsToSiteBySiteCode, List<HRM_DEF_EmployeeShiftMasterDto>>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly DMCContext _contextDMC;
    //    private readonly IMapper _mapper;
    //    public GetShiftsToSiteBySiteCodeHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
    //    {
    //        _context = context;
    //        _contextDMC = contextDMC;
    //        _mapper = mapper;
    //    }
    //    public async Task<List<HRM_DEF_EmployeeShiftMasterDto>> Handle(GetShiftsToSiteBySiteCode request, CancellationToken cancellationToken)
    //    {
    //        try
    //        {

    //            var shifts = (from m in _context.TblOpShiftSiteMaps
    //                          join s in _contextDMC.HRM_DEF_EmployeeShiftMasters
    //                          on m.ShiftCode equals s.ShiftCode
    //                          where m.SiteCode == request.SiteCode
    //                          select new HRM_DEF_EmployeeShiftMasterDto
    //                          {
    //                              ShiftName_EN = s.ShiftName_EN,
    //                              ShiftName_AR = s.ShiftName_AR,
    //                              ShiftCode = s.ShiftCode,
    //                          }).ToList();
    //            return shifts;
    //        }
    //        catch (Exception ex)
    //        {
    //            Log.Error("Error in GetShiftMasterById");
    //            return null;
    //        }
    //    }
    //}

    //#endregion
    #region GetShiftsByProjectAndSiteCode
    public class GetShiftsByProjectAndSiteCode : IRequest<List<HRM_DEF_EmployeeShiftMasterDto>>
    {
        public UserIdentityDto User { get; set; }
        public string SiteCode { get; set; }
        public string ProjectCode { get; set; }
    }

    public class GetShiftsByProjectAndSiteCodeHandler : IRequestHandler<GetShiftsByProjectAndSiteCode, List<HRM_DEF_EmployeeShiftMasterDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public GetShiftsByProjectAndSiteCodeHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<List<HRM_DEF_EmployeeShiftMasterDto>> Handle(GetShiftsByProjectAndSiteCode request, CancellationToken cancellationToken)
        {
            List<HRM_DEF_EmployeeShiftMasterDto> shiftList = new();
            try
            {
                var spp =await  _context.TblOpShiftsPlanForProjects.Where(e=>e.SiteCode==request.SiteCode&&e.ProjectCode==request.ProjectCode).ToListAsync();
                var esm = await _contextDMC.HRM_DEF_EmployeeShiftMasters.ToListAsync();
                var Roasters = await _context.TblOpMonthlyRoasterForSites.Where(e => e.ProjectCode ==
                  request.ProjectCode
                  && e.SiteCode == request.SiteCode).ToListAsync();
                //var shifts = (from m in spp
                //              join s in esm
                //              on m.ShiftCode equals s.ShiftCode
                //              where m.SiteCode == request.SiteCode && m.ProjectCode==request.ProjectCode
                //              select new HRM_DEF_EmployeeShiftMasterDto
                //              {
                //                  ShiftName_EN = s.ShiftName_EN,
                //                  ShiftName_AR = s.ShiftName_AR,
                //                  ShiftCode = s.ShiftCode,
                //                  InGrace= TimeSpan.Parse(s.InGrace.ToString().Substring(3, 2)),
                //                  OutGrace = TimeSpan.Parse(s.OutGrace.ToString().Substring(3, 2)),             


                //                  IsOff=s.IsOff
                //              }).ToList();

                foreach (var m in spp) {
                    HRM_DEF_EmployeeShiftMaster s =await _contextDMC.HRM_DEF_EmployeeShiftMasters.FirstOrDefaultAsync(e=>e.ShiftCode==m.ShiftCode);

                    HRM_DEF_EmployeeShiftMasterDto sift = new() {
                        ShiftName_EN = s.ShiftName_EN,
                        ShiftName_AR = s.ShiftName_AR,
                        ShiftCode = s.ShiftCode,
                        InGrace = TimeSpan.Parse(s.InGrace.ToString().Substring(3, 2)),
                        OutGrace = TimeSpan.Parse(s.OutGrace.ToString().Substring(3, 2)),
                        ShiftId = s.ShiftId,
                        IsOff = s.IsOff,
                        CanDelete = !Roasters.Any(e=>
                         e.S1 == s.ShiftCode
                        || e.S2 == s.ShiftCode
                        || e.S3 == s.ShiftCode
|| e.S4 == s.ShiftCode
|| e.S5 == s.ShiftCode
|| e.S6 == s.ShiftCode
|| e.S7 == s.ShiftCode
|| e.S8 == s.ShiftCode
|| e.S9 == s.ShiftCode
|| e.S10 == s.ShiftCode
|| e.S11 == s.ShiftCode
|| e.S12 == s.ShiftCode
|| e.S13 == s.ShiftCode
|| e.S14 == s.ShiftCode
|| e.S15 == s.ShiftCode
|| e.S16 == s.ShiftCode
|| e.S17 == s.ShiftCode
|| e.S18 == s.ShiftCode
|| e.S19 == s.ShiftCode
|| e.S20 == s.ShiftCode
|| e.S21 == s.ShiftCode
|| e.S22 == s.ShiftCode
|| e.S23 == s.ShiftCode
|| e.S24 == s.ShiftCode
|| e.S25 == s.ShiftCode
|| e.S26 == s.ShiftCode
|| e.S27 == s.ShiftCode
|| e.S28 == s.ShiftCode
|| e.S29 == s.ShiftCode
|| e.S30 == s.ShiftCode
|| e.S31 == s.ShiftCode)

                    };
                    shiftList.Add(sift);

                }





                return shiftList;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetShiftMasterById");
                return null;
            }
        }
    }

    #endregion
    #region CanDeleteShiftForProjectAndSiteCode
    public class CanDeleteShiftForProjectAndSiteCode : IRequest<bool>
    {
        public UserIdentityDto User { get; set; }
        public string SiteCode { get; set; }
        public string ProjectCode { get; set; }
        public string ShiftCode { get; set; }
    }

    public class CanDeleteShiftForProjectAndSiteCodeHandler : IRequestHandler<CanDeleteShiftForProjectAndSiteCode, bool>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public CanDeleteShiftForProjectAndSiteCodeHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<bool> Handle(CanDeleteShiftForProjectAndSiteCode request, CancellationToken cancellationToken)
        {

            try
            {
                return !(await _context.TblOpMonthlyRoasterForSites.AnyAsync(e => e.ProjectCode == request.ProjectCode
                 && e.SiteCode == request.SiteCode
                 && (e.S1 == request.ShiftCode
                 || e.S2 == request.ShiftCode
                 || e.S3 == request.ShiftCode
     || e.S4 == request.ShiftCode
     || e.S5 == request.ShiftCode
     || e.S6 == request.ShiftCode
     || e.S7 == request.ShiftCode
     || e.S8 == request.ShiftCode
     || e.S9 == request.ShiftCode
     || e.S10 == request.ShiftCode
     || e.S11 == request.ShiftCode
     || e.S12 == request.ShiftCode
     || e.S13 == request.ShiftCode
     || e.S14 == request.ShiftCode
     || e.S15 == request.ShiftCode
     || e.S16 == request.ShiftCode
     || e.S17 == request.ShiftCode
     || e.S18 == request.ShiftCode
     || e.S19 == request.ShiftCode
     || e.S20 == request.ShiftCode
     || e.S21 == request.ShiftCode
     || e.S22 == request.ShiftCode
     || e.S23 == request.ShiftCode
     || e.S24 == request.ShiftCode
     || e.S25 == request.ShiftCode
     || e.S26 == request.ShiftCode
     || e.S27 == request.ShiftCode
     || e.S28 == request.ShiftCode
     || e.S29 == request.ShiftCode
     || e.S30 == request.ShiftCode
     || e.S31 == request.ShiftCode
     )));
            }
            catch (Exception)
            {

                
                return false;

            }
               
        }
    }

    #endregion









    #region GetShiftsByProjectAndSiteCode2
    public class GetShiftsByProjectAndSiteCode2 : IRequest<List<HRM_DEF_EmployeeShiftMasterAddUpdateDto>>
    {
        public UserIdentityDto User { get; set; }
        public string SiteCode { get; set; }
        public string ProjectCode { get; set; }
    }

    public class GetShiftsByProjectAndSiteCode2Handler : IRequestHandler<GetShiftsByProjectAndSiteCode2, List<HRM_DEF_EmployeeShiftMasterAddUpdateDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;
        public GetShiftsByProjectAndSiteCode2Handler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }
        public async Task<List<HRM_DEF_EmployeeShiftMasterAddUpdateDto>> Handle(GetShiftsByProjectAndSiteCode2 request, CancellationToken cancellationToken)
        {
            List<HRM_DEF_EmployeeShiftMasterAddUpdateDto> shiftList = new();
            try
            {
                var spp = _context.TblOpShiftsPlanForProjects.AsNoTracking().Where(e => e.SiteCode == request.SiteCode && e.ProjectCode == request.ProjectCode);
                var esm = _contextDMC.HRM_DEF_EmployeeShiftMasters.AsNoTracking();

             

                foreach (var m in spp)
                {
                    HRM_DEF_EmployeeShiftMaster s = await _contextDMC.HRM_DEF_EmployeeShiftMasters.FirstOrDefaultAsync(e => e.ShiftCode == m.ShiftCode);

                    HRM_DEF_EmployeeShiftMasterAddUpdateDto sift = new()
                    {
                        ShiftName_EN = s.ShiftName_EN,
                        ShiftName_AR = s.ShiftName_AR,
                        ShiftCode = s.ShiftCode,
                        InGrace = short.Parse(s.InGrace.ToString().Substring(3, 2)),
                        OutGrace = short.Parse(s.InGrace.ToString().Substring(3, 2)),
                        OutTime = s.OutTime.ToString().Substring(0, 5),
                        WorkingTime=s.WorkingTime.ToString().Substring(0,5),
               InTime = s.InTime.ToString().Substring(0, 5),
               ShiftId=s.ShiftId,
                    IsOff = s.IsOff

                    };
                    shiftList.Add(sift);

                }

                shiftList = shiftList.OrderBy(e => e.InTime).ToList();



                return shiftList;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetShiftMasterById");
                return null;
            }
        }
    }

    #endregion




    #endregion


    #region ShiftsPlanForProject
    #region CreateUpdateShiftsPlanForProject
    public class CreateUpdateShiftsPlanForProject : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public OpShiftsPlanForprojectDto Shifts { get; set; }
    }

    public class CreateUpdateShiftsPlanForProjectHandler : IRequestHandler<CreateUpdateShiftsPlanForProject, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateShiftsPlanForProjectHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateShiftsPlanForProject request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateShiftsToSiteMap method start----");
                    var obj = request.Shifts;

                    if (request.Shifts.ShiftsList.Count() > 0)
                    {
                        var oldShiftsList = await _context.TblOpShiftsPlanForProjects.Where(e => e.SiteCode == request.Shifts.SiteCode && e.ProjectCode==request.Shifts.ProjectCode).ToListAsync();
                        _context.TblOpShiftsPlanForProjects.RemoveRange(oldShiftsList);

                        List<TblOpShiftsPlanForProject> ShiftsList = new();
                        foreach (var shft in request.Shifts.ShiftsList)
                        {
                            TblOpShiftsPlanForProject shift = new()
                            {
                                SiteCode = request.Shifts.SiteCode,
                                ShiftCode = shft.ShiftCode,
                                ProjectCode= request.Shifts.ProjectCode
                            };


                            ShiftsList.Add(shift);
                        }
                        await _context.TblOpShiftsPlanForProjects.AddRangeAsync(ShiftsList);
                        await _context.SaveChangesAsync();


                        int noOfShiftsPlans = _context.TblOpShiftsPlanForProjects.AsNoTracking().Where(e => e.ProjectCode == request.Shifts.ProjectCode).GroupBy(e => e.SiteCode).Count();

                        int noOfSitesforEnquiry = _context.OprEnquiries.AsNoTracking().Where(e => e.EnquiryNumber == request.Shifts.ProjectCode).GroupBy(x => x.SiteCode).Count();

                      

                        var projectSite = await _context.TblOpProjectSites.AsNoTracking().FirstOrDefaultAsync(e=>e.ProjectCode==request.Shifts.ProjectCode&&
                        e.SiteCode==request.Shifts.SiteCode);

                        projectSite.IsShiftsAssigned = true;
                        _context.TblOpProjectSites.Update(projectSite);
                        await _context.SaveChangesAsync();




                        var project = await _context.OP_HRM_TEMP_Projects.AsNoTracking().FirstOrDefaultAsync(e => e.ProjectCode == request.Shifts.ProjectCode);
                        project.IsShiftsAssigned = !_context.TblOpProjectSites.AsNoTracking().Any(e => e.ProjectCode == request.Shifts.ProjectCode && e.SiteCode == request.Shifts.SiteCode && !e.IsAdendum && !e.IsSkillSetsMapped);

                        _context.OP_HRM_TEMP_Projects.Update(project);
                        await _context.SaveChangesAsync();




                    }



                    Log.Info("----Info CreateUpdateServicesEnquiryForm method Exit----");

                    await transaction.CommitAsync();
                    return 1;

                }
                catch (Exception ex)
                {
                    Log.Error("Error in CreateUpdateShiftsToSiteMap Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;

                }
            }
        }
    }

    #endregion


    //#region GetSelectShiftsToSiteBySiteCode
    //public class GetSelectShiftsToSiteBySiteCode : IRequest<List<CustomSelectListItem>>
    //{
    //    public UserIdentityDto User { get; set; }
    //    public string SiteCode { get; set; }
    //}

    //public class GetSelectShiftsToSiteBySiteCodeHandler : IRequestHandler<GetSelectShiftsToSiteBySiteCode, List<CustomSelectListItem>>
    //{
    //    private readonly CINDBOneContext _context;
    //    private readonly DMCContext _contextDMC;
    //    private readonly IMapper _mapper;
    //    public GetSelectShiftsToSiteBySiteCodeHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
    //    {
    //        _context = context;
    //        _contextDMC = contextDMC;
    //        _mapper = mapper;
    //    }
    //    public async Task<List<CustomSelectListItem>> Handle(GetSelectShiftsToSiteBySiteCode request, CancellationToken cancellationToken)
    //    {
    //        try
    //        {

    //            var shifts = (from m in _context.TblOpShiftSiteMaps
    //                          join s in _contextDMC.HRM_DEF_EmployeeShiftMasters
    //                          on m.ShiftCode equals s.ShiftCode
    //                          where m.SiteCode == request.SiteCode
    //                          select new CustomSelectListItem
    //                          {
    //                              Text = s.ShiftName_EN,
    //                              TextTwo = s.IsOff.ToString(),
    //                              Value = s.ShiftCode
    //                          }).ToList();
    //            return shifts;
    //        }
    //        catch (Exception ex)
    //        {
    //            Log.Error("Error in GetShiftMasterById");
    //            return null;
    //        }
    //    }
    //}

    //#endregion

    #endregion

}
