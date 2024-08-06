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


    #region MonthlyRoaster

    #region CreateUpdateMonthlyRoaster
    public class CreateUpdateMonthlyRoaster : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public OpMonthlyRoastersDto Roasters { get; set; }
    }

    public class CreateUpdateMonthlyRoasterHandler : IRequestHandler<CreateUpdateMonthlyRoaster, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateUpdateMonthlyRoasterHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUpdateMonthlyRoaster request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateUpdateMonthlyRoaster method start----");
                    var obj = request.Roasters;

                    if (request.Roasters.RoastersList.Count() > 0)
                    {
                        var oldRoastersList = await _context.TblOpMonthlyRoasters.Where(e => e.CustomerCode == request.Roasters.CustomerCode && e.SiteCode == request.Roasters.SiteCode && /*e.ProjectCode == request.Roasters.ProjectCode &&*/ e.Month == request.Roasters.Month && e.Year == request.Roasters.Year).ToListAsync();
                        _context.TblOpMonthlyRoasters.RemoveRange(oldRoastersList);

                        List<TblOpMonthlyRoaster> RoastersList = new();
                        foreach (var rst in request.Roasters.RoastersList)
                        {
                            TblOpMonthlyRoaster rstr = new()
                            {
                                CustomerCode = request.Roasters.CustomerCode,
                                SiteCode = request.Roasters.SiteCode,
                                //ProjectCode = request.Roasters.ProjectCode,
                                Month = request.Roasters.Month,
                                Year = request.Roasters.Year,
                                EmployeeNumber=rst.EmployeeNumber,
                                EmployeeName=rst.EmployeeName,
                                S1 = rst.s[0],
                                S2 = rst.s[1],
                                S3 = rst.s[2],
                                S4 = rst.s[3],
                                S5 = rst.s[4],
                                S6 = rst.s[5],
                                S7 = rst.s[6],
                                S8 = rst.s[7],
                                S9 = rst.s[8],
                                S10 = rst.s[9],
                                S11 = rst.s[10],
                                S12 = rst.s[11],
                                S13 = rst.s[12],
                                S14 = rst.s[13],
                                S15 = rst.s[14],
                                S16 = rst.s[15],
                                S17 = rst.s[16],
                                S18 = rst.s[17],
                                S19 = rst.s[18],
                                S20 = rst.s[19],
                                S21 = rst.s[20],
                                S22 = rst.s[21],
                                S23 = rst.s[22],
                                S24 = rst.s[23],
                                S25 = rst.s[24],
                                S26 = rst.s[25],
                                S27 = rst.s[26],
                                S28 = rst.s[27],
                                S29 = rst.s[28],
                                S30 = rst.s[29],
                                S31 = rst.s[30]
                            };
                            RoastersList.Add(rstr);
                        }
                        await _context.TblOpMonthlyRoasters.AddRangeAsync(RoastersList);
                        await _context.SaveChangesAsync();
                    }
                    Log.Info("----Info CreateUpdateMonthlyRoaster method Exit----");

                    await transaction.CommitAsync();
                    return 1;

                }
                catch (Exception ex)
                {
                    Log.Error("Error in CreateUpdateMonthlyRoaster Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;

                }
            }
        }
    }

    #endregion


    #region GetMonthlyRoaster
    public class GetMonthlyRoaster : IRequest<List<TblOpMonthlyRoasterDto>>
    {
        public UserIdentityDto User { get; set; }
        public string SiteCode { get; set; }
        public string CustomerCode { get; set;}
        //public string ProjectCode { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }

    }

    public class GetMonthlyRoasterHandler : IRequestHandler<GetMonthlyRoaster, List<TblOpMonthlyRoasterDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetMonthlyRoasterHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblOpMonthlyRoasterDto>> Handle(GetMonthlyRoaster request, CancellationToken cancellationToken)
        {
            List<TblOpMonthlyRoasterDto> roasters = new();
            try
            {
                roasters = await _context.TblOpMonthlyRoasters.AsNoTracking().ProjectTo<TblOpMonthlyRoasterDto>(_mapper.ConfigurationProvider).Where(e => e.CustomerCode == request.CustomerCode && e.SiteCode == request.SiteCode && /*e.ProjectCode == request.ProjectCode &&*/ e.Month == request.Month && e.Year == request.Year).ToListAsync();
               
                for (var i = 0; i < roasters.Count; i++)
                {
                    roasters[i].s =new List<string>();

                    roasters[i].s.Add(roasters[i].S1);
                    roasters[i].s.Add(roasters[i].S2);
                    roasters[i].s.Add(roasters[i].S3);
                    roasters[i].s.Add(roasters[i].S4);
                    roasters[i].s.Add(roasters[i].S5);
                    roasters[i].s.Add(roasters[i].S6);
                    roasters[i].s.Add(roasters[i].S7);
                    roasters[i].s.Add(roasters[i].S8);
                    roasters[i].s.Add(roasters[i].S9);
                    roasters[i].s.Add(roasters[i].S10);
                    roasters[i].s.Add(roasters[i].S11);
                    roasters[i].s.Add(roasters[i].S12);
                    roasters[i].s.Add(roasters[i].S13);
                    roasters[i].s.Add(roasters[i].S14);
                    roasters[i].s.Add(roasters[i].S15);
                    roasters[i].s.Add(roasters[i].S16);
                    roasters[i].s.Add(roasters[i].S17);
                    roasters[i].s.Add(roasters[i].S18);
                    roasters[i].s.Add(roasters[i].S19);
                    roasters[i].s.Add(roasters[i].S20);
                    roasters[i].s.Add(roasters[i].S21);
                    roasters[i].s.Add(roasters[i].S22);
                    roasters[i].s.Add(roasters[i].S23);
                    roasters[i].s.Add(roasters[i].S24);
                    roasters[i].s.Add(roasters[i].S25);
                    roasters[i].s.Add(roasters[i].S26);
                    roasters[i].s.Add(roasters[i].S27);
                    roasters[i].s.Add(roasters[i].S28);
                    roasters[i].s.Add(roasters[i].S29);
                    roasters[i].s.Add(roasters[i].S30);
                    roasters[i].s.Add(roasters[i].S31);

                }


                return roasters;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetShiftMasterById");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion

    #endregion



    #region MonthlyRoasterForSite
    #region GenerateMonthlyRoastersForSite
    public class GenerateMonthlyRoastersForSite : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public List<InputTblOpMonthlyRoasterForSiteDto> Input { get; set; }
    }

    public class GenerateMonthlyRoastersForSiteHandler : IRequestHandler<GenerateMonthlyRoastersForSite, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public GenerateMonthlyRoastersForSiteHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(GenerateMonthlyRoastersForSite request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try

                {
                    Log.Info("----Info GenerateMonthlyRoastersForSite method start----");
                    foreach(var input in request.Input) { 


                    var obj = input;

                        //if (input.ShiftCodesForMonth.Count() > 0)
                        //{
                        //var oldShiftCodesForMonth = await _context.TblOpMonthlyRoasters.Where(e => e.CustomerCode == input.CustomerCode && e.SiteCode == input.SiteCode && /*e.ProjectCode == input.ProjectCode &&*/ e.Month == input.Month && e.Year == input.Year).ToListAsync();
                        //  _context.TblOpMonthlyRoasterForSites.RemoveRange(oldShiftCodesForMonth);

                        //List<TblOpMonthlyRoaster> ShiftCodesForMonth = new();
                        //foreach (var rst in input.ShiftCodesForMonth)
                        //{
                        bool isRoasterExist = _context.TblOpMonthlyRoasterForSites.AsNoTracking().Any(e => e.CustomerCode == input.CustomerCode

                        && e.ProjectCode == input.ProjectCode
                        && e.SiteCode == input.SiteCode
                        && e.Year == input.Year
                        && e.Month == input.Month
                        && e.EmployeeNumber == input.EmployeeNumber);
                        if(isRoasterExist)
                        {
                            await transaction.RollbackAsync();
                            return -1;

                        }
                            TblOpMonthlyRoasterForSite rstr = new()
                            {
                                CustomerCode = input.CustomerCode,
                                SiteCode = input.SiteCode,
                                ProjectCode = input.ProjectCode,
                                Month = input.Month,
                                Year = input.Year,
                                MonthEndDate=input.MonthEndDate,
                                MonthStartDate=input.MonthStartDate,
                                SkillsetCode = input.SkillsetCode,
                                SkillsetName = input.SkillsetName,
                                S1 = input.ShiftCodesForMonth[0],
                                S2 = input.ShiftCodesForMonth[1],
                                S3 = input.ShiftCodesForMonth[2],
                                S4 = input.ShiftCodesForMonth[3],
                                S5 = input.ShiftCodesForMonth[4],
                                S6 = input.ShiftCodesForMonth[5],
                                S7 = input.ShiftCodesForMonth[6],
                                S8 = input.ShiftCodesForMonth[7],
                                S9 = input.ShiftCodesForMonth[8],
                                S10 = input.ShiftCodesForMonth[9],
                                S11 = input.ShiftCodesForMonth[10],
                                S12 = input.ShiftCodesForMonth[11],
                                S13 = input.ShiftCodesForMonth[12],
                                S14 = input.ShiftCodesForMonth[13],
                                S15 = input.ShiftCodesForMonth[14],
                                S16 = input.ShiftCodesForMonth[15],
                                S17 = input.ShiftCodesForMonth[16],
                                S18 = input.ShiftCodesForMonth[17],
                                S19 = input.ShiftCodesForMonth[18],
                                S20 = input.ShiftCodesForMonth[19],
                                S21 = input.ShiftCodesForMonth[20],
                                S22 = input.ShiftCodesForMonth[21],
                                S23 = input.ShiftCodesForMonth[22],
                                S24 = input.ShiftCodesForMonth[23],
                                S25 = input.ShiftCodesForMonth[24],
                                S26 = input.ShiftCodesForMonth[25],
                                S27 = input.ShiftCodesForMonth[26],
                                S28 = input.ShiftCodesForMonth[27],
                                S29 = input.ShiftCodesForMonth[28],
                                S30 = input.ShiftCodesForMonth[29],
                                S31 = input.ShiftCodesForMonth[30],


                                EmployeeID=input.EmployeeID,
                                EmployeeNumber=input.EmployeeNumber,
                                MapId=input.MapId,
                                IsPrimaryResource=true
                            };
                            //ShiftCodesForMonth.Add(rstr);
                        //}
                        await _context.TblOpMonthlyRoasterForSites.AddAsync(rstr);
                        await _context.SaveChangesAsync();


                    }













                    Log.Info("----Info GenerateMonthlyRoastersForSite method Exit----");

                    await transaction.CommitAsync();
                    return 1;

                }
                catch (Exception ex)
                {
                  await  transaction.RollbackAsync();
                    Log.Error("Error in GenerateMonthlyRoastersForSite Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    await transaction.RollbackAsync();
                    return 0;

                }
            }
        }
    }

    #endregion

    #region GetMonthlyRoasterForSite
    public class GetMonthlyRoasterForSite : IRequest<List<TblOpMonthlyRoasterForSiteDto>>
    {
        public UserIdentityDto User { get; set; }
        public string SiteCode { get; set; }
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }

    }

    public class GetMonthlyRoasterForSiteHandler : IRequestHandler<GetMonthlyRoasterForSite, List<TblOpMonthlyRoasterForSiteDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetMonthlyRoasterForSiteHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblOpMonthlyRoasterForSiteDto>> Handle(GetMonthlyRoasterForSite request, CancellationToken cancellationToken)
        {
            List<TblOpMonthlyRoasterForSiteDto> roasters = new();
            try
            {
                roasters = await _context.TblOpMonthlyRoasterForSites.AsNoTracking().ProjectTo<TblOpMonthlyRoasterForSiteDto>(_mapper.ConfigurationProvider).Where(e => e.CustomerCode == request.CustomerCode && e.SiteCode == request.SiteCode && e.ProjectCode == request.ProjectCode && e.Month == request.Month && e.Year == request.Year).ToListAsync();

                for (var i = 0; i < roasters.Count; i++)
                {
                    roasters[i].ShiftCodesForMonth = new List<string>();

                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S1);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S2);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S3);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S4);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S5);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S6);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S7);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S8);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S9);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S10);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S11);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S12);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S13);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S14);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S15);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S16);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S17);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S18);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S19);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S20);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S21);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S22);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S23);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S24);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S25);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S26);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S27);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S28);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S29);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S30);
                    roasters[i].ShiftCodesForMonth.Add(roasters[i].S31);

                }


                return roasters;
            }
            catch (Exception ex)
            {
                Log.Error("Error in GetMonthlyRoasterForSite");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return null;
            }
        }
    }

    #endregion

    #region IsExistMonthlyRoasterForProject
    public class IsExistMonthlyRoasterForProject : IRequest<bool>
    {
        public UserIdentityDto User { get; set; }
        public string SiteCode { get; set; }
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }

    }

    public class IsExistMonthlyRoasterForProjectHandler : IRequestHandler<IsExistMonthlyRoasterForProject, bool>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public IsExistMonthlyRoasterForProjectHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(IsExistMonthlyRoasterForProject request, CancellationToken cancellationToken)
        {
            var projectSite = await _context.TblOpProjectSites.FirstOrDefaultAsync(e => e.ProjectCode == request.ProjectCode && e.SiteCode == request.SiteCode);

            var isExist = await _context.TblOpMonthlyRoasterForSites.AnyAsync(e => e.ProjectCode == request.ProjectCode
            && e.SiteCode == request.SiteCode
            && e.MonthStartDate >= projectSite.StartDate
            );

            return isExist;

        }
    }

    #endregion

#region IsExistMonthlyRoasterForSiteMonth
    public class IsExistMonthlyRoasterForSiteMonth : IRequest<bool>
    {
        public UserIdentityDto User { get; set; }
        public string SiteCode { get; set; }
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }

    }

    public class IsExistMonthlyRoasterForSiteMonthHandler : IRequestHandler<IsExistMonthlyRoasterForSiteMonth, bool>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public IsExistMonthlyRoasterForSiteMonthHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(IsExistMonthlyRoasterForSiteMonth request, CancellationToken cancellationToken)
        {
            
           
              var isExist = await _context.TblOpMonthlyRoasterForSites.FirstOrDefaultAsync(e =>  e.ProjectCode == request.ProjectCode && 
              e.SiteCode==request.SiteCode
              && e.Month==request.Month
              && e.Year==request.Year);

            if (isExist == null)
                return false;
            else
                return true;
            }
            
        }


    #endregion

    #region UpdateShiftsForMonthlyRoaster
    public class UpdateShiftsForMonthlyRoaster : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public List<TblOpMonthlyRoasterForSiteDto> Roasters { get; set; }
    }

    public class UpdateShiftsForMonthlyRoasterHandler : IRequestHandler<UpdateShiftsForMonthlyRoaster, int>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public UpdateShiftsForMonthlyRoasterHandler(CINDBOneContext context,DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateShiftsForMonthlyRoaster request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                using (var transaction2 = _contextDMC.Database.BeginTransaction())
                {
                    try
                    {
                        //var employeesForSite = await _context.TblOpEmployeesToProjectSiteList.AsNoTracking().Where(e => e.ProjectCode == request.Roasters[0].ProjectCode && e.SiteCode == request.Roasters[0].SiteCode).ToListAsync();
                        var shifts =  _contextDMC.HRM_DEF_EmployeeShiftMasters.AsNoTracking().ToList();
                        var mappings = _context.TblOpEmployeeToResourceMapList.AsNoTracking().ToList();
                        Log.Info("----Info UpdateShiftsForMonthlyRoasterHandler method start----");
                        for (var i = 0; i < request.Roasters.Count; i++)
                        {
                            if (request.Roasters[i].ShiftCodesForMonth.Any(e=>e==""))
                            {
                                return -3;
                            }
                        
                        }


                        foreach (var rst in request.Roasters)
                        {



                            TblOpMonthlyRoasterForSite obj = new();
                      


                            if (rst.Id > 0)
                            {
                                obj = _context.TblOpMonthlyRoasterForSites.FirstOrDefault(e => e.Id == rst.Id);
                            }
                            else {

                              obj = new();
                                obj.Id = 0;
                            }

                            obj.CustomerCode = rst.CustomerCode;
                            obj.SiteCode = rst.SiteCode;
                            obj.ProjectCode = rst.ProjectCode;
                            obj.Month = rst.Month;
                            obj.Year = rst.Year;
                            obj.SkillsetCode = rst.SkillsetCode;
                            obj.SkillsetName = rst.SkillsetName;
                            obj.S1 = rst.ShiftCodesForMonth[0];
                            obj.S2 = rst.ShiftCodesForMonth[1];
                            obj.S3 = rst.ShiftCodesForMonth[2];
                            obj.S4 = rst.ShiftCodesForMonth[3];
                            obj.S5 = rst.ShiftCodesForMonth[4];
                            obj.S6 = rst.ShiftCodesForMonth[5];
                            obj.S7 = rst.ShiftCodesForMonth[6];
                            obj.S8 = rst.ShiftCodesForMonth[7];
                            obj.S9 = rst.ShiftCodesForMonth[8];
                            obj.S10 = rst.ShiftCodesForMonth[9];
                            obj.S11 = rst.ShiftCodesForMonth[10];
                            obj.S12 = rst.ShiftCodesForMonth[11];
                            obj.S13 = rst.ShiftCodesForMonth[12];
                            obj.S14 = rst.ShiftCodesForMonth[13];
                            obj.S15 = rst.ShiftCodesForMonth[14];
                            obj.S16 = rst.ShiftCodesForMonth[15];
                            obj.S17 = rst.ShiftCodesForMonth[16];
                            obj.S18 = rst.ShiftCodesForMonth[17];
                            obj.S19 = rst.ShiftCodesForMonth[18];
                            obj.S20 = rst.ShiftCodesForMonth[19];
                            obj.S21 = rst.ShiftCodesForMonth[20];
                            obj.S22 = rst.ShiftCodesForMonth[21];
                            obj.S23 = rst.ShiftCodesForMonth[22];
                            obj.S24 = rst.ShiftCodesForMonth[23];
                            obj.S25 = rst.ShiftCodesForMonth[24];
                            obj.S26 = rst.ShiftCodesForMonth[25];
                            obj.S27 = rst.ShiftCodesForMonth[26];
                            obj.S28 = rst.ShiftCodesForMonth[27];
                            obj.S29 = rst.ShiftCodesForMonth[28];
                            obj.S30 = rst.ShiftCodesForMonth[29];
                            obj.S31 = rst.ShiftCodesForMonth[30];
                            obj.EmployeeNumber = rst.EmployeeNumber;
                            obj.EmployeeID = rst.EmployeeID;
                            obj.MapId = rst.MapId;
                            obj.IsPrimaryResource = rst.IsPrimaryResource;
                            //ShiftCodesForMonth=obj.ShiftCodesForMonth

                            bool isDefEmployee = mappings.Any(e=>e.MapId==rst.MapId&&e.isPrimarySite);
                            
                            if (obj.Id > 0)
                            {
                                _context.TblOpMonthlyRoasterForSites.Update(obj);
                                _context.SaveChanges();

                                for (int i = 0; i < 31; i++)
                                {
                                    if (rst.ShiftCodesForMonth[i]!="x") {

                                        string date =rst.Year + "-" + rst.Month.ToString().PadLeft(2, '0') + "-" + (i + 1).ToString().PadLeft(2, '0');
                                        DateTime convertedDate = Convert.ToDateTime(date, CultureInfo.InvariantCulture);
                                        bool isExistAttendance = _context.EmployeeAttendance.AsNoTracking().Any(e=>e.ProjectCode==rst.ProjectCode && e.SiteCode==rst.SiteCode && e.AttnDate== convertedDate);
                                        if (isExistAttendance)
                                        {
                                            transaction2.Rollback();
                                            transaction.Rollback();
                                            return -2;          //Attendance Already Exist
                                        }

                                        long EmpId = _contextDMC.HRM_TRAN_Employees.FirstOrDefault(e => e.EmployeeNumber == rst.EmployeeNumber).EmployeeID;
                                        HRM_DEF_EmployeeOff existOff =  _contextDMC.HRM_DEF_EmployeeOffs.AsNoTracking().FirstOrDefault(e => e.EmployeeId == EmpId 
                                        && e.Date == convertedDate
                                        && e.SiteCode==rst.SiteCode);
                                        if (existOff is not null && rst.IsPrimaryResource) {
                                            _contextDMC.HRM_DEF_EmployeeOffs.Remove(existOff);
                                             _contextDMC.SaveChanges();

                                        }
                                    }
                                    
                                }


                            }
                            else
                            {
                                _context.TblOpMonthlyRoasterForSites.Add(obj);

                                _context.SaveChanges();


                            }

                            List<HRM_DEF_EmployeeOff> newOffs = new();
                            for (int i = 0; i < rst.ShiftCodesForMonth.Count; i++)
                            {
                                if (rst.ShiftCodesForMonth[i] != "x")
                                {
                                   
                                        if (shifts.Any(s => s.IsOff.Value==true && s.ShiftCode == rst.ShiftCodesForMonth[i]))
                                        {
                                            HRM_DEF_EmployeeOff off = new();

                                            off.EmployeeId = rst.EmployeeID;
                                            string offdate = rst.Year + "-" + rst.Month.ToString().PadLeft(2, '0') + "-" + (i + 1).ToString().PadLeft(2, '0');
                                           // DateTime offdate= DateTime.Parse(rst.Year + "-" + rst.Month.ToString().PadLeft(2, '0') + "-" + (i + 1).ToString().PadLeft(2, '0'));
                                            off.Date = Convert.ToDateTime(offdate, CultureInfo.InvariantCulture);
                                            off.ID = 0;
                                            off.SiteCode = rst.SiteCode;
                                            newOffs.Add(off);
                                            
                                        }

                                }
                            }

                            if (newOffs.Count > 0) {
                                _contextDMC.HRM_DEF_EmployeeOffs.AddRange(newOffs);
                                _contextDMC.SaveChanges();
                            }
                        }

                        Log.Info("----Info UpdateShiftsForMonthlyRoasterHandler method Exit----");

                       transaction2.Commit();
                       transaction.Commit();
                        return 1;

                    }
                    catch (Exception ex)
                    {
                        transaction2.Rollback();
                        transaction.Rollback();
                        Log.Error("Error in UpdateShiftsForMonthlyRoasterHandler Method");
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

    #endregion

    #region SingleRoaster

    #region UpdateShiftCodeForDay
    public class UpdateShiftCodeForDay : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public UpdateShiftCodeForDayDto Input { get; set; }
    }

    public class UpdateShiftCodeForDayHandler : IRequestHandler<UpdateShiftCodeForDay, int>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public UpdateShiftCodeForDayHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateShiftCodeForDay request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                using (var transaction2 = _contextDMC.Database.BeginTransaction())
                {
                    try
                    {
                        Log.Info(" UpdateShiftCodeForDay Method Starts");
                        var Roaster = await _context.TblOpMonthlyRoasterForSites.FirstOrDefaultAsync(e=>e.Id==request.Input.RoasterId);
                        if(Roaster==null)
                        {
                            return -1; //Roaster Not Found
                        }

                        var ShiftsForSites =await _context.TblOpShiftsPlanForProjects.Where(e => e.SiteCode == Roaster.SiteCode
                        && e.ProjectCode == Roaster.ProjectCode
                        ).ToListAsync();
                        var  ShiftsInHrm = await _contextDMC.HRM_DEF_EmployeeShiftMasters.ToListAsync();


                        var ShiftsData = (from Sfs in ShiftsForSites join Sin in ShiftsInHrm on Sfs.ShiftCode equals Sin.ShiftCode select new { Sfs.ShiftCode, Sin.IsOff }).ToList();

                        var Employee = await _contextDMC.HRM_TRAN_Employees.FirstOrDefaultAsync(e=>e.EmployeeNumber==Roaster.EmployeeNumber);
                        if (Employee==null)
                        {
                            return -2;          //Employee Not Found
                        }

                       DateTime date = new DateTime(Roaster.Year, Roaster.Month, request.Input.Day + 1);
                        bool isAttendanceExist = await _context.EmployeeAttendance.AnyAsync(e=>e.AttnDate.Value==date
                        && e.EmployeeNumber== Roaster.EmployeeNumber
                       // && e.ShiftCode==request.Input.ShiftCode
                        && e.ProjectCode== Roaster.ProjectCode
                        && e.SiteCode== Roaster.SiteCode
                        );

                        if (isAttendanceExist)
                        {
                            return -3;
                        }

                        switch (request.Input.Day+1)              //Day is starts from 0-30  --->from Array
                        {
                            
                            case 1:
                                 date = new DateTime(Roaster.Year, Roaster.Month, 1);

                                if (request.Input.ShiftCode == Roaster.S1)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S1 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 2:
                                date = new DateTime(Roaster.Year,Roaster.Month,2);

                                if (request.Input.ShiftCode == Roaster.S2)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S2 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 3:
                                date = new DateTime(Roaster.Year,Roaster.Month,3);

                                if (request.Input.ShiftCode == Roaster.S3)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S3 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 4:
                                date = new DateTime(Roaster.Year,Roaster.Month,4);

                                if (request.Input.ShiftCode == Roaster.S4)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S4 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 5:
                                date = new DateTime(Roaster.Year,Roaster.Month,5);

                                if (request.Input.ShiftCode == Roaster.S5)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S5 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 6:
                                date = new DateTime(Roaster.Year,Roaster.Month,6);

                                if (request.Input.ShiftCode == Roaster.S6)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S6 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 7:
                                date = new DateTime(Roaster.Year,Roaster.Month,7);

                                if (request.Input.ShiftCode == Roaster.S7)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S7 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 8:
                                date = new DateTime(Roaster.Year,Roaster.Month,8);

                                if (request.Input.ShiftCode == Roaster.S8)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S8 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 9:
                                date = new DateTime(Roaster.Year,Roaster.Month,1);

                                if (request.Input.ShiftCode == Roaster.S9)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S9 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 10:
                                date = new DateTime(Roaster.Year,Roaster.Month,10);

                                if (request.Input.ShiftCode == Roaster.S10)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S10 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 11:
                                date = new DateTime(Roaster.Year,Roaster.Month,11);

                                if (request.Input.ShiftCode == Roaster.S11)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S11 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 12:
                                date = new DateTime(Roaster.Year,Roaster.Month,12);

                                if (request.Input.ShiftCode == Roaster.S12)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S12 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 13:
                                date = new DateTime(Roaster.Year,Roaster.Month,13);

                                if (request.Input.ShiftCode == Roaster.S13)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S13 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 14:
                                date = new DateTime(Roaster.Year,Roaster.Month,14);

                                if (request.Input.ShiftCode == Roaster.S14)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S14 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 15:
                                date = new DateTime(Roaster.Year,Roaster.Month,15);

                                if (request.Input.ShiftCode == Roaster.S15)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S15 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 16:
                                date = new DateTime(Roaster.Year,Roaster.Month,16);

                                if (request.Input.ShiftCode == Roaster.S16)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S16= request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 17:
                                date = new DateTime(Roaster.Year,Roaster.Month,17);

                                if (request.Input.ShiftCode == Roaster.S17)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S17 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 18:
                                date = new DateTime(Roaster.Year,Roaster.Month,18);

                                if (request.Input.ShiftCode == Roaster.S18)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S18 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 19:
                                date = new DateTime(Roaster.Year,Roaster.Month,19);

                                if (request.Input.ShiftCode == Roaster.S19)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S19 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 20:
                                date = new DateTime(Roaster.Year,Roaster.Month,20);

                                if (request.Input.ShiftCode == Roaster.S20)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S20 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 21:
                                date = new DateTime(Roaster.Year,Roaster.Month,21);

                                if (request.Input.ShiftCode == Roaster.S21)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S21 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 22:
                                date = new DateTime(Roaster.Year,Roaster.Month,22);

                                if (request.Input.ShiftCode == Roaster.S22)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S22 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 23:
                                date = new DateTime(Roaster.Year,Roaster.Month,23);

                                if (request.Input.ShiftCode == Roaster.S23)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S23 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 24:
                                date = new DateTime(Roaster.Year,Roaster.Month,24);

                                if (request.Input.ShiftCode == Roaster.S24)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S24 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 25:
                                date = new DateTime(Roaster.Year,Roaster.Month,25);

                                if (request.Input.ShiftCode == Roaster.S25)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S25 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 26:
                                date = new DateTime(Roaster.Year,Roaster.Month,26);

                                if (request.Input.ShiftCode == Roaster.S26)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S26 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 27:
                                date = new DateTime(Roaster.Year,Roaster.Month,27);

                                if (request.Input.ShiftCode == Roaster.S27)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S27 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 28:
                                date = new DateTime(Roaster.Year,Roaster.Month,28);

                                if (request.Input.ShiftCode == Roaster.S28)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S28 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 29:
                                date = new DateTime(Roaster.Year,Roaster.Month,29);

                                if (request.Input.ShiftCode == Roaster.S29)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S29 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 30:
                                date = new DateTime(Roaster.Year,Roaster.Month,30);

                                if (request.Input.ShiftCode == Roaster.S30)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S30 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                                 case 31:
                                date = new DateTime(Roaster.Year,Roaster.Month,31);

                                if (request.Input.ShiftCode == Roaster.S31)
                                {
                                    return 0;
                                }
                                else
                                {
                                    
                                    if (ShiftsData.Any(s => s.ShiftCode == request.Input.ShiftCode && s.IsOff.Value))           //if new Shift is Off
                                    {
                                        HRM_DEF_EmployeeOff offDay = new() {
                                        ID=0,
                                        Date=date,
                                        EmployeeId=Employee.EmployeeID,
                                        SiteCode=Roaster.SiteCode                                        
                                        };
                                        await _contextDMC.HRM_DEF_EmployeeOffs.AddAsync(offDay);
                                        await _contextDMC.SaveChangesAsync();
                                    } 
                                   else          //if old Shift is Off
                                    {

                                        var offDays = await _contextDMC.HRM_DEF_EmployeeOffs.Where(e=>e.EmployeeId==Employee.EmployeeID&& e.Date==date && e.SiteCode==Roaster.SiteCode).ToListAsync();
                                        if(offDays.Count>0)
                                        _contextDMC.HRM_DEF_EmployeeOffs.RemoveRange(offDays);
                                        await _contextDMC.SaveChangesAsync();
                                    }


                                    Roaster.S31 = request.Input.ShiftCode;
                                    _context.TblOpMonthlyRoasterForSites.Update(Roaster);
                                    await _context.SaveChangesAsync();
                                }
                                break;
                            default:
                               return -3;      //Invalid Date

                        }
                        

                       
                        await transaction.CommitAsync();
                        await transaction2.CommitAsync();
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        transaction2.Rollback();
                        transaction.Rollback();
                        Log.Error("Error in UpdateShiftCodeForDay Method");
                        Log.Error("Error occured time : " + DateTime.UtcNow);
                        Log.Error("Error message : " + ex.Message);
                        Log.Error("Error StackTrace : " + ex.StackTrace);
                        return -4;

                    }
                }
            }
        }
    }

    #endregion

    #region GetSingleRoasterForEmployee
    public class GetSingleRoasterForEmployee : IRequest<TblOpMonthlyRoasterForSiteDto>
    {
        public UserIdentityDto User { get; set; }
        public InputEmployeeSingleRoasterDto Input { get; set; }
    }

    public class GetSingleRoasterForEmployeeHandler : IRequestHandler<GetSingleRoasterForEmployee, TblOpMonthlyRoasterForSiteDto>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public GetSingleRoasterForEmployeeHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<TblOpMonthlyRoasterForSiteDto> Handle(GetSingleRoasterForEmployee request, CancellationToken cancellationToken)
        {
        
                    try
                    {
                       return  _context.TblOpMonthlyRoasterForSites.ProjectTo<TblOpMonthlyRoasterForSiteDto>(_mapper.ConfigurationProvider).FirstOrDefault(e => e.ProjectCode == request.Input.ProjectCode
                        && e.SiteCode==request.Input.SiteCode
                        && e.Month==request.Input.Date.Value.Month
                        && e.Year==request.Input.Date.Value.Year
                        && e.EmployeeNumber== request.Input.EmployeeNumber
                       );
                      
                    }
                    catch (Exception ex)
                    {
                        
                        Log.Error("Error in GetSingleRoasterForEmployee Method");
                        Log.Error("Error occured time : " + DateTime.UtcNow);
                        Log.Error("Error message : " + ex.Message);
                        Log.Error("Error StackTrace : " + ex.StackTrace);
                        return null;

                }
        }
    }

    #endregion




    #endregion







}
