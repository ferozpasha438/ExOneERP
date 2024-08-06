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







    

    

#region PostAttendance

    public class PostEmployeeAttendance : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public List<TblOpEmployeeAttendanceDto> Input { get; set; }
    }

    public class PostEmployeeAttendanceHandler : IRequestHandler<PostEmployeeAttendance, long>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public PostEmployeeAttendanceHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<long> Handle(PostEmployeeAttendance request, CancellationToken cancellationToken)
        {
           
            var sites = _context.OprSites.AsNoTracking();
            var shifts = _contextDMC.HRM_DEF_EmployeeShiftMasters.AsNoTracking();
            using (var transaction = _context.Database.BeginTransaction())
            { 
            using (var transaction2 = _contextDMC.Database.BeginTransaction())
            {

                try
                {
                    foreach (var singAtt in request.Input)
                    {


                      

                            var empAtt = await _context.EmployeeAttendance.AsNoTracking().FirstOrDefaultAsync(e => e.Id == singAtt.Id && e.Id > 0);  //Sometimes Applying leave and sending attendance, that time attendance should not post
                            if (empAtt is not null)
                            {


                                var postAtt = await _contextDMC.EmployeePostedAttendance.AsNoTracking().FirstOrDefaultAsync(e => e.EmployeeNumber == singAtt.EmployeeNumber
                                                                                                                     && e.ProjectCode == singAtt.ProjectCode
                                                                                                                     && e.SiteCode == singAtt.SiteCode
                                                                                                                     && e.ShiftCode == singAtt.ShiftCode
                                                                                                                     && e.Date == singAtt.AttnDate);


                                if (postAtt is null)
                                {
                                    
                                    HRM_TRAN_EmployeeTimeChart newAtt = new();
                                    newAtt.ProjectID = _context.OP_HRM_TEMP_Projects.AsNoTracking().FirstOrDefault(p => p.ProjectCode == singAtt.ProjectCode).Id;
                                    newAtt.ProjectCode = singAtt.ProjectCode;
                                    newAtt.CustomerCode = _context.OP_HRM_TEMP_Projects.AsNoTracking().FirstOrDefault(c => c.ProjectCode == singAtt.ProjectCode).CustomerCode;
                                    newAtt.SiteCode = singAtt.SiteCode;
                                    newAtt.ShiftCode = singAtt.isDefShiftOff ? singAtt.AltShiftCode : singAtt.ShiftCode;
                                    newAtt.Date = Convert.ToDateTime(singAtt.AttnDate, CultureInfo.InvariantCulture);
                                    newAtt.InTime = TimeSpan.Parse(singAtt.InTime);
                                    newAtt.OutTime = TimeSpan.Parse(singAtt.OutTime);
                                    newAtt.EmployeeNumber = singAtt.EmployeeNumber;
                                    newAtt.EmployeeID = _contextDMC.HRM_TRAN_Employees.FirstOrDefault(e => e.EmployeeNumber == singAtt.EmployeeNumber).EmployeeID;
                                    newAtt.SiteID = sites.FirstOrDefault(e => e.SiteCode == singAtt.SiteCode).Id;
                                    newAtt.ShiftId = shifts.FirstOrDefault(e => e.ShiftCode == singAtt.ShiftCode).ShiftId;
                                    newAtt.ShiftNumber = singAtt.isPrimarySite ? Convert.ToByte(singAtt.ShiftNumber) : Convert.ToByte(2);

                                    newAtt.OPShiftNumber = singAtt.isPrimarySite ? Convert.ToByte(singAtt.ShiftNumber) : Convert.ToByte(2);

                                   // newAtt.ShiftNumber = singAtt.isPrimarySite ? Convert.ToByte(singAtt.ShiftNumber) : Convert.ToByte(3);   //if reliever concept implemented
                                    newAtt.AttnFlag = 'P';
                                    newAtt.CreatedDate = DateTime.UtcNow;
                                    await _contextDMC.EmployeePostedAttendance.AddAsync(newAtt);
                                    await _contextDMC.SaveChangesAsync();
                                  

                                }
                                else
                                {

                                    postAtt.ProjectID = _context.OP_HRM_TEMP_Projects.AsNoTracking().FirstOrDefault(p => p.ProjectCode == singAtt.ProjectCode).Id;
                                    postAtt.ProjectCode = singAtt.ProjectCode;
                                    postAtt.CustomerCode = _context.OP_HRM_TEMP_Projects.AsNoTracking().FirstOrDefault(c => c.ProjectCode == singAtt.ProjectCode).CustomerCode;
                                    postAtt.SiteCode = singAtt.SiteCode;
                                    postAtt.ShiftCode = singAtt.isDefShiftOff ? singAtt.AltShiftCode : singAtt.ShiftCode;
                                    postAtt.Date = Convert.ToDateTime(singAtt.AttnDate, CultureInfo.InvariantCulture);
                                    postAtt.InTime = TimeSpan.Parse(singAtt.InTime);
                                    postAtt.OutTime = TimeSpan.Parse(singAtt.OutTime);
                                    postAtt.EmployeeNumber = singAtt.EmployeeNumber;
                                    postAtt.EmployeeID = _contextDMC.HRM_TRAN_Employees.FirstOrDefault(e => e.EmployeeNumber == singAtt.EmployeeNumber).EmployeeID;
                                    postAtt.SiteID = sites.FirstOrDefault(e => e.SiteCode == singAtt.SiteCode).Id;
                                    postAtt.ShiftId = shifts.FirstOrDefault(e => e.ShiftCode == singAtt.ShiftCode).ShiftId;
                                    postAtt.ShiftNumber = singAtt.isPrimarySite ? Convert.ToByte(singAtt.ShiftNumber) : Convert.ToByte(2);
                                    postAtt.OPShiftNumber = singAtt.isPrimarySite ? Convert.ToByte(singAtt.ShiftNumber) : Convert.ToByte(2);
                                    postAtt.AttnFlag = 'P';
                                    postAtt.ModifiedDate = DateTime.UtcNow;
                                    _contextDMC.EmployeePostedAttendance.Update(postAtt);
                                    await _contextDMC.SaveChangesAsync();
                                    
                                }


                                empAtt.isPosted = true;
                                _context.EmployeeAttendance.Update(empAtt);
                                if (empAtt.RefIdForAlt > 0)
                                { 
                                var baseAttendance= await _context.EmployeeAttendance.AsNoTracking().FirstOrDefaultAsync(e => e.Id == empAtt.RefIdForAlt);
                                    if (baseAttendance is null)
                                    {
                                        await transaction.RollbackAsync();
                                        await transaction2.RollbackAsync();
                                        return -1;
                                    }
                                    else
                                        if (baseAttendance.isPosted)
                                    {
                                        await transaction.RollbackAsync();
                                        await transaction2.RollbackAsync();
                                        return -2;
                                    }
                                    else
                                    {
                                        baseAttendance.isPosted = true;
                                        _context.EmployeeAttendance.Update(baseAttendance);
                                    }
                                }



                                await _context.SaveChangesAsync();

                            }
                            //else
                            //{
                            //    await transaction.RollbackAsync();
                            //    await transaction2.RollbackAsync();
                            //    return 0;

                            //}

                        }
                   await transaction.CommitAsync();
                  await  transaction2.CommitAsync();
                    Log.Info("----Info PostEmployeeAttendance method Exit----");
                    return 1;
                }


                catch (Exception ex)
                {
                   await transaction.RollbackAsync();
                    await transaction2.RollbackAsync();
                    Log.Error("Error in PostEmployeeAttendance Method");
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


    #region PostAttendanceWithDate

    public class PostEmployeeAttendanceWithDate : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public InputPostingAttendanceWithDate Input { get; set; }
    }

    public class PostEmployeeAttendanceWithDateHandler : IRequestHandler<PostEmployeeAttendanceWithDate, long>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public PostEmployeeAttendanceWithDateHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<long> Handle(PostEmployeeAttendanceWithDate request, CancellationToken cancellationToken)
        {
            var Project = _context.OP_HRM_TEMP_Projects.FirstOrDefault(p => p.ProjectCode == request.Input.ProjectCode);
          //  var sites = _context.OprSites.AsNoTracking();
            var site = await _context.OprSites.SingleOrDefaultAsync(e=>e.SiteCode==request.Input.SiteCode);
            var shifts = _contextDMC.HRM_DEF_EmployeeShiftMasters.AsNoTracking();
            using (var transaction = _context.Database.BeginTransaction())
            {
                using (var transaction2 = _contextDMC.Database.BeginTransaction())
                {

                    try
                    {
                        var PostingTillDate = Convert.ToDateTime(request.Input.Todate, CultureInfo.InvariantCulture);
                        var CurrentDate = Convert.ToDateTime(DateTime.Today, CultureInfo.InvariantCulture);
                        var Todate= Convert.ToDateTime(request.Input.Todate, CultureInfo.InvariantCulture);
                        var FromDate =request.Input.Fromdate is null? new DateTime(Todate.Year,Todate.Month,1): Convert.ToDateTime(request.Input.Fromdate,CultureInfo.InvariantCulture);

                        var attedanceList = await _context.EmployeeAttendance.AsNoTracking().Where(e => (e.Attendance == "P" || e.Attendance == "OT")
                        && !e.isPosted 
                        && e.AttnDate>=FromDate
                        && e.AttnDate<=Todate
                        && e.AttnDate<=CurrentDate
                        && e.ProjectCode==request.Input.ProjectCode
                        && e.SiteCode==request.Input.SiteCode
                        
                        ).ToListAsync();




                        if (attedanceList.Count > 0)
                        {


                            foreach (var singAtt in attedanceList)
                            {

                                var AttnDate = Convert.ToDateTime(singAtt.AttnDate, CultureInfo.InvariantCulture);



                                var empAtt = await _context.EmployeeAttendance.AsNoTracking().FirstOrDefaultAsync(e => e.Id == singAtt.Id);
                                if (empAtt is not null)
                                {


                                    var postAtt = await _contextDMC.EmployeePostedAttendance.FirstOrDefaultAsync(e => e.EmployeeNumber == singAtt.EmployeeNumber
                                                                                                                         && e.ProjectCode == singAtt.ProjectCode
                                                                                                                         && e.SiteCode == singAtt.SiteCode
                                                                                                                         && e.ShiftCode == singAtt.ShiftCode
                                                                                                                         && e.Date == singAtt.AttnDate);


                                    if (postAtt is null)
                                    {

                                        HRM_TRAN_EmployeeTimeChart newAtt = new();
                                        newAtt.ProjectID = Project.Id;
                                        newAtt.ProjectCode = singAtt.ProjectCode;
                                        newAtt.CustomerCode = Project.CustomerCode;
                                        newAtt.SiteCode = singAtt.SiteCode;
                                        newAtt.ShiftCode = singAtt.isDefShiftOff ? singAtt.AltShiftCode : singAtt.ShiftCode;
                                        newAtt.Date = Convert.ToDateTime(singAtt.AttnDate, CultureInfo.InvariantCulture);
                                        newAtt.InTime = singAtt.InTime;
                                        newAtt.OutTime = singAtt.OutTime;
                                        newAtt.EmployeeNumber = singAtt.EmployeeNumber;
                                        newAtt.EmployeeID = _contextDMC.HRM_TRAN_Employees.FirstOrDefault(e => e.EmployeeNumber == singAtt.EmployeeNumber).EmployeeID;
                                        newAtt.SiteID = site.Id;
                                        newAtt.ShiftId = shifts.FirstOrDefault(e => e.ShiftCode == singAtt.ShiftCode).ShiftId;
                                        newAtt.ShiftNumber = singAtt.isPrimarySite ? Convert.ToByte(singAtt.ShiftNumber) : Convert.ToByte(2);

                                        newAtt.OPShiftNumber = singAtt.isPrimarySite ? Convert.ToByte(singAtt.ShiftNumber) : Convert.ToByte(2);

                                        // newAtt.ShiftNumber = singAtt.isPrimarySite ? Convert.ToByte(singAtt.ShiftNumber) : Convert.ToByte(3);   //if reliever concept implemented
                                        newAtt.AttnFlag = 'P';
                                        newAtt.CreatedDate = DateTime.UtcNow;
                                        newAtt.IsApproved = false;

                                        await _contextDMC.EmployeePostedAttendance.AddAsync(newAtt);
                                        await _contextDMC.SaveChangesAsync();


                                    }
                                    else
                                    {

                                        postAtt.ProjectID = _context.OP_HRM_TEMP_Projects.AsNoTracking().FirstOrDefault(p => p.ProjectCode == singAtt.ProjectCode).Id;

                                        if (!postAtt.IsApproved.Value)
                                        {
                                            postAtt.ProjectCode = singAtt.ProjectCode;
                                            postAtt.CustomerCode = Project.CustomerCode;
                                            postAtt.SiteCode = singAtt.SiteCode;
                                            postAtt.ShiftCode = singAtt.isDefShiftOff ? singAtt.AltShiftCode : singAtt.ShiftCode;
                                            postAtt.Date = Convert.ToDateTime(singAtt.AttnDate, CultureInfo.InvariantCulture);
                                            postAtt.InTime = singAtt.InTime;
                                            postAtt.OutTime = singAtt.OutTime;
                                            postAtt.EmployeeNumber = singAtt.EmployeeNumber;
                                            postAtt.EmployeeID = _contextDMC.HRM_TRAN_Employees.FirstOrDefault(e => e.EmployeeNumber == singAtt.EmployeeNumber).EmployeeID;
                                            postAtt.SiteID = site.Id;
                                            postAtt.ShiftId = shifts.FirstOrDefault(e => e.ShiftCode == singAtt.ShiftCode).ShiftId;
                                            postAtt.ShiftNumber = singAtt.isPrimarySite ? Convert.ToByte(singAtt.ShiftNumber) : Convert.ToByte(2);
                                            postAtt.OPShiftNumber = singAtt.isPrimarySite ? Convert.ToByte(singAtt.ShiftNumber) : Convert.ToByte(2);
                                            postAtt.AttnFlag = 'P';
                                            postAtt.ModifiedDate = DateTime.UtcNow;
                                            _contextDMC.EmployeePostedAttendance.Update(postAtt);
                                            await _contextDMC.SaveChangesAsync();
                                        }
                                        

                                    }


                                    empAtt.isPosted = true;
                                    _context.EmployeeAttendance.Update(empAtt);
                                    await _context.SaveChangesAsync();

                                    if (empAtt.RefIdForAlt > 0)
                                    {
                                        var baseAttendance = await _context.EmployeeAttendance.AsNoTracking().FirstOrDefaultAsync(e => e.Id == empAtt.RefIdForAlt);
                                        if (baseAttendance is null)
                                        {
                                            await transaction.RollbackAsync();
                                            await transaction2.RollbackAsync();
                                            return -1;
                                        }
                                        else if (baseAttendance.isPosted)
                                        {
                                            await transaction.RollbackAsync();
                                            await transaction2.RollbackAsync();
                                            return -2;
                                        }
                                        else
                                        {
                                            baseAttendance.isPosted = true;
                                            _context.EmployeeAttendance.Update(baseAttendance);
                                            await _context.SaveChangesAsync();
                                        }
                                    }

                                    await _context.SaveChangesAsync();

                                }


                            }

                            #region Sending Withdrwals To HRM
                            List<HRM_TRAN_EmployeeTimeChart> NewWithDrawalList = new();
                            var EmployeesInRoaster = await _context.TblOpMonthlyRoasterForSites.Where(e => e.IsPrimaryResource
                            && e.Month == request.Input.Todate.Month && e.Year == request.Input.Todate.Year
                            && e.ProjectCode == request.Input.ProjectCode
                            && e.SiteCode == request.Input.SiteCode
                            ).Select(e => new { e.EmployeeNumber }).ToListAsync();

                            for (int i = 0; i < EmployeesInRoaster.Count; i++)
                            {
                                var emp = EmployeesInRoaster[i];

                                DateTime fromDate = new DateTime(request.Input.Todate.Year, request.Input.Todate.Month, 1);


                                var WithdrwalsInDMC = await _contextDMC.EmployeePostedAttendance.AsNoTracking().Where(
                                    e => e.ProjectCode == request.Input.ProjectCode
                                    && e.SiteCode == request.Input.SiteCode
                                    && e.Date >= FromDate
                                    && e.Date <= request.Input.Todate
                                    && e.EmployeeNumber == emp.EmployeeNumber
                                    && e.AttnFlag == 'W'
                                    ).ToListAsync();


                                var WithdrwalsInOpr = await _context.EmployeeLeaves.AsNoTracking().Where(
                                  e =>
                                   e.AttnDate >= FromDate
                                   && e.AttnDate <= request.Input.Todate
                                   && e.W
                                    && e.EmployeeNumber == emp.EmployeeNumber
                                    && e.ProjectCode == request.Input.ProjectCode
                                    && e.SiteCode == request.Input.SiteCode
                                   ).ToListAsync();

                                for (var j = 0; j < WithdrwalsInOpr.Count; j++)
                                {
                                    if (!WithdrwalsInDMC.Any(e => e.EmployeeNumber == emp.EmployeeNumber
                                     && e.Date.Value == Convert.ToDateTime(WithdrwalsInOpr[j].AttnDate,CultureInfo.InvariantCulture)
                                     && e.ShiftCode == WithdrwalsInOpr[j].ShiftCode
                                     && e.ProjectCode == WithdrwalsInOpr[j].ProjectCode
                                     && e.SiteCode == WithdrwalsInOpr[j].SiteCode))
                                    {
                                        HRM_TRAN_EmployeeTimeChart newWD = new()
                                        {
                                            EmployeeNumber = emp.EmployeeNumber,
                                            AttnFlag = 'W',
                                            EmployeeID = _contextDMC.HRM_TRAN_Employees.SingleOrDefault(h => h.EmployeeNumber == emp.EmployeeNumber).EmployeeID,
                                            Date = WithdrwalsInOpr[j].AttnDate,
                                            OPShiftNumber = 1,
                                            ShiftNumber = 1,
                                            ProjectCode = request.Input.ProjectCode,
                                            CustomerCode = Project.CustomerCode,
                                            ProjectID = Project.Id,
                                            SiteCode = request.Input.SiteCode,
                                            SiteID = site.Id,
                                            ShiftId = shifts.FirstOrDefault(e => e.ShiftCode == WithdrwalsInOpr[j].ShiftCode).ShiftId,
                                            ShiftCode = WithdrwalsInOpr[j].ShiftCode,
                                            CreatedDate = DateTime.UtcNow,
                                            IsApproved=false

                                        };

                                        NewWithDrawalList.Add(newWD);
                                    }
                                }


                            }
                            if (NewWithDrawalList.Count > 0)
                            {
                                await _contextDMC.EmployeePostedAttendance.AddRangeAsync(NewWithDrawalList);
                                await _contextDMC.SaveChangesAsync();
                            }

                            #endregion
                        }
                        else if(attedanceList.Count==0)
                        {
                            await transaction.RollbackAsync();
                            await transaction2.RollbackAsync();
                            return 0;
                        }


                        await transaction.CommitAsync();
                        await transaction2.CommitAsync();
                        Log.Info("----Info PostEmployeeAttendanceWithDate method Exit----");
                        return 1;
                    }


                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        await transaction2.RollbackAsync();
                        Log.Error("Error in PostEmployeeAttendanceWithDate Method");
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

}






