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



    #region CopyEmployees
    public class CopyEmployees : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
    }

    public class CopyEmployeesHandler : IRequestHandler<CopyEmployees, int>
    {
        private readonly DMCContext _context;
        private readonly DMC2Context _contextDMC2;
        private readonly IMapper _mapper;

        public CopyEmployeesHandler(DMCContext context, DMC2Context contextDMC2, IMapper mapper)
        {
            _context = context;
            _contextDMC2 = contextDMC2;
            _mapper = mapper;
        }

        public async Task<int> Handle(CopyEmployees request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CopyEmployees method start----");

                    List<HRM_TRAN_Employee> employeesFromDMC = await _contextDMC2.HRM_TRAN_Employees.AsNoTracking().ToListAsync();
                   foreach(var e in employeesFromDMC) {
                        var emp = await _context.HRM_TRAN_Employees.FirstOrDefaultAsync(x => x.EmployeeNumber == e.EmployeeNumber && e.EmployeeNumber!="" && e.EmployeeNumber !=null);
                        if (emp is null)
                        {
                            HRM_TRAN_Employee newemp = new() { 
                            
                            EmployeeName=e.EmployeeName,
                            EmployeeNumber=e.EmployeeNumber,
                            CreatedBy=e.CreatedBy,
                            CreatedDate=e.CreatedDate,
                            ModifiedBy=e.ModifiedBy,
                            IsActive=e.IsActive

                            };
                           await _context.HRM_TRAN_Employees.AddAsync(newemp);
                            await _context.SaveChangesAsync();
                        }
                    }

                    Log.Info("----Info CopyEmployees method Exit----");
                    await transaction.CommitAsync();
                    return 1;
                }
                catch (Exception ex)
                {
                    Log.Error("Error in CopyEmployees Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }

    #endregion
    #region CopyShifts
    public class CopyShifts : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
    }

    public class CopyShiftsHandler : IRequestHandler<CopyShifts, int>
    {
        private readonly DMCContext _context;
        private readonly DMC2Context _contextDMC2;
        private readonly IMapper _mapper;

        public CopyShiftsHandler(DMCContext context, DMC2Context contextDMC2, IMapper mapper)
        {
            _context = context;
            _contextDMC2 = contextDMC2;
            _mapper = mapper;
        }

        public async Task<int> Handle(CopyShifts request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CopyEmployees method start----");

                    List<HRM_DEF_EmployeeShiftMaster> shiftsFromDMC = await _contextDMC2.HRM_DEF_EmployeeShiftMasters.AsNoTracking().ToListAsync();
                   foreach(var e in shiftsFromDMC) {
                        var emp = await _context.HRM_DEF_EmployeeShiftMasters.FirstOrDefaultAsync(x => x.ShiftCode == e.ShiftCode && e.ShiftCode!="" && e.ShiftCode != null);
                        if (emp is null)
                        {
                            HRM_DEF_EmployeeShiftMaster newshift = new()
                            {

                                ShiftCode = e.ShiftCode,
                                ShiftName_EN = e.ShiftName_EN,
                                ShiftName_AR = e.ShiftName_AR,
                                InTime = e.InTime,
                                OutTime = e.OutTime,
                                BreakTime = e.BreakTime,
                                InGrace = e.InGrace,
                                OutGrace = e.OutGrace,
                                WorkingTime = e.WorkingTime,
                                NetWorkingTime = e.NetWorkingTime,
                                IsOff = e.IsOff,
                                IsActive = e.IsActive

                            };
                            await _context.HRM_DEF_EmployeeShiftMasters.AddAsync(newshift);
                            await _context.SaveChangesAsync();
                        }
                        else {
                            _context.HRM_DEF_EmployeeShiftMasters.Update(e);
                            await _context.SaveChangesAsync();

                        }
                    }

                    Log.Info("----Info CopyShifts method Exit----");
                    await transaction.CommitAsync();
                    return 1;
                }
                catch (Exception ex)
                {
                    Log.Error("Error in CopyShifts Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }

    #endregion
    #region CopyProjectsAndSitesDataToHRM
    public class CopyProjectsAndSitesDataToHRM : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
    }

    public class CopyProjectsAndSitesDataToHRMHandler : IRequestHandler<CopyProjectsAndSitesDataToHRM, int>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public CopyProjectsAndSitesDataToHRMHandler(CINDBOneContext context, DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<int> Handle(CopyProjectsAndSitesDataToHRM request, CancellationToken cancellationToken)
        {
          
                try
                {
                    Log.Info("----Info CopyProjectsAndSitesDataToHRM method start----");
                    List<OP_HRM_TEMP_Project> projects =_context.OP_HRM_TEMP_Projects.AsNoTracking().ToList();

                if (projects.Count != 0)
                {
                    foreach (var project in projects)
                    {
                        var projInHRM = _contextDMC.HRM_DEF_Projects.AsNoTracking().FirstOrDefault(p => p.ProjectID ==project.Id);
                        if (projInHRM is null)
                        {
                            HRM_DEF_Project newProjForHRM = new()
                            {
                                ProjectDescription = "",
                                ProjectID = project.Id,
                                ProjectName_AR = project.ProjectNameArb,
                                ProjectName_EN = project.ProjectNameEng,
                                CreatedBy = 0,
                                CreatedDate = DateTime.Now,
                                IsActive = true,
                                IsSystem = 0,
                                ModifiedBy = 0,
                                ModifiedDate = DateTime.Now,
                                 ProjectSiteID=0,
                                
                            };

                            _contextDMC.HRM_DEF_Projects.Add(newProjForHRM);
                            _contextDMC.SaveChanges();
                        }


                    }
                
                        var projectsInDmc = _contextDMC.HRM_DEF_Projects.AsNoTracking().ToList();
                if (projectsInDmc.Count != 0)
                {
                    foreach (var DMCproject in projectsInDmc)
                    {
                            int pid = Convert.ToInt32(DMCproject.ProjectID);
                  //      var sitesInHRM = _contextDMC.HRM_DEF_Sites.AsNoTracking().Where(s => s.ProjectID == DMCproject.ProjectID).ToList();
                        var proj = _context.OP_HRM_TEMP_Projects.AsNoTracking().FirstOrDefault(p => p.Id == pid);
                        var sitesInOp = _context.OprSites.AsNoTracking().Where(s => proj.CustomerCode == s.CustomerCode).ToList();
                        if(sitesInOp.Count!=0)
                        foreach (var siteInOp in sitesInOp)
                        {
                                    long sid = Convert.ToInt64(siteInOp.Id);
                            var siteExistInHrm =_contextDMC.HRM_DEF_Sites.AsNoTracking().Any(s =>s.SiteCode == siteInOp.SiteCode);
                            if (!siteExistInHrm)
                            {
                                HRM_DEF_Site newsiteForHRM = new()
                                {
                                    ProjectID = DMCproject.ProjectID,
                                    SiteName_AR = siteInOp.SiteArbName,
                                    SiteName_EN = siteInOp.SiteName,
                                    SiteCode = siteInOp.SiteCode,
                                    SiteDescription = "",
                                    SiteID =siteInOp.Id,
                                    //IsSystem = false,
                                    BranchID = 0,
                                    CreatedBy = 0,
                                    CreatedDate = DateTime.Now,
                                    IsActive = true,
                                    ModifiedBy = 0,
                                    ModifiedDate = DateTime.Now



                                };

                                await _contextDMC.HRM_DEF_Sites.AddAsync(newsiteForHRM);
                                _contextDMC.SaveChanges();

                            }

                        }

                    }
                }
                else
                {
                    return -2;
                }
                    return 1;
                }
           
                     else
                    {
                        return -1;
                    }

                   
                }
                catch (Exception ex)
                {
                    Log.Error("Error in CopyProjectsAndSitesDataToHRM Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            
        }
    }

    #endregion

    
}
