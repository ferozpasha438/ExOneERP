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
    #region Resource Costing

    #region CreateProjectResourceCosting
    public class CreateProjectResourceCostingForSite : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblOpProjectBudgetCostingDto Input { get; set; }
    }

    public class CreateProjectResourceCostingForSiteHandler : IRequestHandler<CreateProjectResourceCostingForSite, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateProjectResourceCostingForSiteHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateProjectResourceCostingForSite request, CancellationToken cancellationToken)
        {
            
           
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    
                    Log.Info("----Info CreateProjectResourceCostingForSite method start----");

                    TblOpProjectBudgetEstimation est = new TblOpProjectBudgetEstimation();
                    est.CustomerCode = request.Input.CustomerCode;
                    est.ProjectCode = request.Input.ProjectCode;
                    TblOpProjectBudgetCosting pbc = new TblOpProjectBudgetCosting();

                    var estExist = await _context.TblOpProjectBudgetEstimations.AsNoTracking().OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode);

                    if (estExist == null)
                    {

                        
                      
                        est.Created = DateTime.UtcNow;
                        est.CreatedBy = request.User.UserId;
                        await _context.TblOpProjectBudgetEstimations.AddAsync(est);
                        await _context.SaveChangesAsync();
                        pbc.ProjectBudgetEstimationId = est.ProjectBudgetEstimationId;
                    }
                    else
                    {
                        pbc.ProjectBudgetEstimationId = estExist.ProjectBudgetEstimationId;
                    
                    
                    }
                    var pbcExist = await _context.TblOpProjectBudgetCostings.AsNoTracking().OrderBy(e => e.Created).Where(e =>
                          e.ProjectBudgetEstimationId == est.ProjectBudgetEstimationId
                          && e.ServiceType == "SKILLSET"
                          && e.SiteCode==request.Input.SiteCode)

                        .FirstOrDefaultAsync();

                    if (pbcExist == null)
                    {
                        
                        pbc.ServiceType = "SKILLSET";
                        pbc.SiteCode = request.Input.SiteCode;
                        pbc.ProjectCode = request.Input.ProjectCode;
                        pbc.CustomerCode = request.Input.CustomerCode;
                        pbc.Created = DateTime.UtcNow;
                        await _context.TblOpProjectBudgetCostings.AddAsync(pbc);
                        await _context.SaveChangesAsync();

                    }
                    foreach (var prcInput in request.Input.ResourceCostingsList)
                        {
                            TblOpProjectResourceCosting plc = new TblOpProjectResourceCosting();
                            plc.ProjectBudgetCostingId = pbc.ProjectBudgetCostingId;
                            plc.SkillsetCode = prcInput.SkillsetCode;
                            plc.Quantity = prcInput.Quantity;
                            plc.Margin = prcInput.Margin;
                            plc.CostPerUnit = prcInput.CostPerUnit;
                        plc.SiteCode = request.Input.SiteCode;
                            await _context.TblOpProjectResourceCosting.AddAsync(plc);
                            await _context.SaveChangesAsync();
                     
                        foreach (var prscInput in prcInput.ResourceSubCostingList)
                        {
                            TblOpProjectResourceSubCosting prsc = new TblOpProjectResourceSubCosting();
                            prsc.CostHead = prscInput.CostHead;
                            prsc.Amount = prscInput.Amount;
                            prsc.ResourceCostingId = plc.ResourceCostingId;
                            
                            prsc.Created = DateTime.UtcNow;
                            prsc.CreatedBy = request.User.UserId;
                            await _context.TblOpProjectResourceSubCostings.AddAsync(prsc);
                            await _context.SaveChangesAsync();

                        }
                    }

                    var projectSite = await _context.TblOpProjectSites.AsNoTracking().FirstOrDefaultAsync(ps=>ps.ProjectCode==request.Input.ProjectCode&&
                    ps.SiteCode==request.Input.SiteCode);
                    
                    projectSite.IsEstimationCompleted = projectSite.IsLogisticsAssigned && projectSite.IsExpenceOverheadsAssigned && projectSite.IsMaterialAssigned;

                    projectSite.IsResourcesAssigned = true;
                        _context.TblOpProjectSites.Update(projectSite);
                    await _context.SaveChangesAsync();





                    var project = await _context.OP_HRM_TEMP_Projects.AsNoTracking().FirstOrDefaultAsync(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode);




                    // int noOfSitesforEnquiry = _context.OprEnquiries.AsNoTracking().Where(e => e.EnquiryNumber == project.ProjectCode).GroupBy(x=>x.SiteCode).Count();

                    //int noOfResPlansForProject = _context.TblOpProjectBudgetCostings.AsNoTracking().Where(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode && e.ServiceType == "SKILLSET").GroupBy(e => e.SiteCode).Count();
                    //int noOfLogPlansForProject = _context.TblOpProjectBudgetCostings.AsNoTracking().Where(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode && e.ServiceType == "LOGISTICS").GroupBy(e => e.SiteCode).Count();
                    //int noOfMatPlansForProject = _context.TblOpProjectBudgetCostings.AsNoTracking().Where(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode && e.ServiceType == "MATERIALEQUIPMENT").GroupBy(e => e.SiteCode).Count();
                    //int noOfExpPlansForProject = _context.TblOpProjectBudgetCostings.AsNoTracking().Where(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode && e.ServiceType == "FINANCIALEXPENSE").GroupBy(e => e.SiteCode).Count();


                    //project.IsResourcesAssigned = noOfResPlansForProject == noOfSitesforEnquiry;
                    //project.IsExpenceOverheadsAssigned = noOfExpPlansForProject == noOfSitesforEnquiry;
                    //project.IsMaterialAssigned = noOfMatPlansForProject == noOfSitesforEnquiry;
                    //project.IsLogisticsAssigned = noOfLogPlansForProject == noOfSitesforEnquiry;

               bool ira= project.IsResourcesAssigned = !_context.TblOpProjectSites.AsNoTracking().Any(e => !e.IsAdendum && !e.IsResourcesAssigned);
                bool iea= project.IsExpenceOverheadsAssigned = !_context.TblOpProjectSites.AsNoTracking().Any(e => !e.IsAdendum && !e.IsExpenceOverheadsAssigned);
                bool ima= project.IsMaterialAssigned = !_context.TblOpProjectSites.AsNoTracking().Any(e => !e.IsAdendum && !e.IsMaterialAssigned);
                bool ila=    project.IsLogisticsAssigned = !_context.TblOpProjectSites.AsNoTracking().Any(e => !e.IsAdendum && !e.IsLogisticsAssigned);

                    project.IsEstimationCompleted = (ira && iea && ima && ila);






                    //project.IsEstimationCompleted = (project.IsResourcesAssigned &&
                    //    project.IsExpenceOverheadsAssigned &&
                    //    project.IsMaterialAssigned &&
                    //    project.IsLogisticsAssigned
                    //    );
                    _context.OP_HRM_TEMP_Projects.Update(project);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    Log.Info("----Info CreateProjectResourceCosting method Exit----");
                    return project.Id;
                }
                catch (Exception ex)
                {
                    Log.Error("Error in CreateProjectResourceCosting Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }

    #endregion



    #region GetProjectResourceCostingForSite
    public class GetProjectResourceCostingForSite : IRequest<TblOpProjectBudgetCostingDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
    }

    public class GetProjectResourceCostingForSiteHandler : IRequestHandler<GetProjectResourceCostingForSite, TblOpProjectBudgetCostingDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetProjectResourceCostingForSiteHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpProjectBudgetCostingDto> Handle(GetProjectResourceCostingForSite request, CancellationToken cancellationToken)
        {

            TblOpProjectBudgetCostingDto res = new TblOpProjectBudgetCostingDto();

            TblOpProjectBudgetEstimationDto est = await _context.TblOpProjectBudgetEstimations.AsNoTracking().ProjectTo<TblOpProjectBudgetEstimationDto>(_mapper.ConfigurationProvider).OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectCode == request.ProjectCode && e.CustomerCode == request.CustomerCode);

            TblOpProjectBudgetCostingDto pbc = new TblOpProjectBudgetCostingDto();
            if (est == null)
            {
                res.ProjectBudgetCostingId = 0;
                res.Status = -1;
                res.ProjectBudgetEstimationId = -1;
                pbc = null; 
            }

            else {

                pbc = await _context.TblOpProjectBudgetCostings.AsNoTracking().ProjectTo<TblOpProjectBudgetCostingDto>(_mapper.ConfigurationProvider).OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectBudgetEstimationId == est.ProjectBudgetEstimationId);



            }
            if (pbc==null)
                {
                res.ProjectBudgetEstimationId = 0;
                res.ProjectBudgetCostingId = 0;
                    res.Status = 0;
                }
                else
                {
                res.ProjectBudgetEstimationId = est.ProjectBudgetEstimationId;
                var pbcForSite = await _context.TblOpProjectBudgetCostings.AsNoTracking().OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectBudgetEstimationId==est.ProjectBudgetEstimationId && e.SiteCode == request.SiteCode && e.ServiceType == "SKILLSET");
                    if (pbcForSite == null)
                    {
                        res.Status = 1;
                        res.ResourceCostingsList = new List<TblOpProjectResourceCostingDto>();
                    }
                    else
                    {
                        res.ProjectBudgetCostingId = pbcForSite.ProjectBudgetCostingId;

                        var plc = await _context.TblOpProjectResourceCosting.AsNoTracking().FirstOrDefaultAsync(e => e.ProjectBudgetCostingId == pbcForSite.ProjectBudgetCostingId && e.SiteCode == pbcForSite.SiteCode);
                        if (plc == null)
                        {
                            res.ResourceCostingsList = new List<TblOpProjectResourceCostingDto>();
                            res.Status = 2;


                        }
                        else
                        {
                            res.Status = 3;
                            res.ResourceCostingsList = await _context.TblOpProjectResourceCosting.AsNoTracking().ProjectTo<TblOpProjectResourceCostingDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectBudgetCostingId == pbcForSite.ProjectBudgetCostingId && e.SiteCode == pbcForSite.SiteCode).ToListAsync();

                            foreach (var item in res.ResourceCostingsList)
                            {
                                var prscList = await _context.TblOpProjectResourceSubCostings.AsNoTracking().ProjectTo<TblOpProjectResourceSubCostingDto>(_mapper.ConfigurationProvider).Where(e => e.ResourceCostingId == item.ResourceCostingId
                                ).ToListAsync();
                                item.ResourceSubCostingList = prscList;
                            }
                        }

                    }

                }
                res.CustomerCode = request.CustomerCode;
                res.ProjectCode = request.ProjectCode;
                res.ServiceType = "SKILLSET";
                res.SiteCode = request.SiteCode;
                            
           return res;
        }
    }

    #endregion
    #endregion




    #region Logistics Costing


    #region CreateProjectLogisticsCosting
    public class CreateProjectLogisticsCostingForSite : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblOpProjectBudgetCostingDto Input { get; set; }
    }

    public class CreateProjectLogisticsCostingForSiteHandler : IRequestHandler<CreateProjectLogisticsCostingForSite, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateProjectLogisticsCostingForSiteHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateProjectLogisticsCostingForSite request, CancellationToken cancellationToken)
        {


            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Log.Info("----Info CreateProjectLogisticsCostingForSite method start----");
                    TblOpProjectBudgetEstimation est = new TblOpProjectBudgetEstimation();
                    est.CustomerCode = request.Input.CustomerCode;
                    est.ProjectCode = request.Input.ProjectCode;
                    TblOpProjectBudgetCosting pbc = new TblOpProjectBudgetCosting();

                    var estExist = await _context.TblOpProjectBudgetEstimations.AsNoTracking().OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode);

                    if (estExist == null)
                    {
                        est.Created = DateTime.UtcNow;
                        est.CreatedBy = request.User.UserId;
                        await _context.TblOpProjectBudgetEstimations.AddAsync(est);
                        await _context.SaveChangesAsync();
                        pbc.ProjectBudgetEstimationId = est.ProjectBudgetEstimationId;
                    }
                    else
                    {
                        pbc.ProjectBudgetEstimationId = estExist.ProjectBudgetEstimationId;


                    }
                    var pbcExist = await _context.TblOpProjectBudgetCostings.AsNoTracking().OrderBy(e => e.Created).Where(e =>
                          e.ProjectBudgetEstimationId == est.ProjectBudgetEstimationId
                          && e.ServiceType == "LOGISTICS"
                          && e.SiteCode == request.Input.SiteCode)

                        .FirstOrDefaultAsync();

                    if (pbcExist == null)
                    {

                        pbc.ServiceType = "LOGISTICS";
                        pbc.SiteCode = request.Input.SiteCode;
                        pbc.ProjectCode = request.Input.ProjectCode;
                        pbc.CustomerCode = request.Input.CustomerCode;
                        
                        pbc.Created = DateTime.UtcNow;
                        await _context.TblOpProjectBudgetCostings.AddAsync(pbc);
                        await _context.SaveChangesAsync();

                    }
                    foreach (var prcInput in request.Input.LogisticsCostingsList)
                    {
                        TblOpProjectLogisticsCosting plc = new TblOpProjectLogisticsCosting();
                        plc.ProjectBudgetCostingId = pbc.ProjectBudgetCostingId;
                        plc.VehicleNumber = prcInput.VehicleNumber;
                        
                        plc.CostPerUnit = prcInput.CostPerUnit;
                        plc.Qty = prcInput.Qty;
                        plc.Margin = prcInput.Margin;
                        plc.SiteCode = request.Input.SiteCode;
                        await _context.TblOpProjectLogisticsCostings.AddAsync(plc);
                        await _context.SaveChangesAsync();

                        foreach (var prscInput in prcInput.LogisticsSubCostingList)
                        {
                            TblOpProjectLogisticsSubCosting prsc = new TblOpProjectLogisticsSubCosting();
                            prsc.CostHead = prscInput.CostHead;
                            prsc.Amount = prscInput.Amount;
                            prsc.LogisticsCostingId = plc.LogisticsCostingId;

                            prsc.Created = DateTime.UtcNow;
                            prsc.CreatedBy = request.User.UserId;
                            await _context.TblOpProjectLogisticsSubCostings.AddAsync(prsc);
                            await _context.SaveChangesAsync();

                        }
                    }

                    var projectSite = await _context.TblOpProjectSites.AsNoTracking().FirstOrDefaultAsync(ps => ps.ProjectCode == request.Input.ProjectCode &&
                 ps.SiteCode == request.Input.SiteCode);
                    projectSite.IsEstimationCompleted = projectSite.IsResourcesAssigned && projectSite.IsExpenceOverheadsAssigned && projectSite.IsMaterialAssigned;
                    projectSite.IsLogisticsAssigned = true;
                    _context.TblOpProjectSites.Update(projectSite);
                    await _context.SaveChangesAsync();





                    var project = await _context.OP_HRM_TEMP_Projects.AsNoTracking().FirstOrDefaultAsync(e=>e.ProjectCode==request.Input.ProjectCode&& e.CustomerCode==request.Input.CustomerCode);

                    //int noOfSitesforEnquiry = _context.OprEnquiries.AsNoTracking().Where(e => e.EnquiryNumber == project.ProjectCode).GroupBy(x=>x.SiteCode).Count();

                    //int noOfResPlansForProject = _context.TblOpProjectBudgetCostings.AsNoTracking().Where(e=>e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode && e.ServiceType == "SKILLSET").GroupBy(e => e.SiteCode).Count();
                    //int noOfLogPlansForProject = _context.TblOpProjectBudgetCostings.AsNoTracking().Where(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode && e.ServiceType == "LOGISTICS").GroupBy(e => e.SiteCode).Count();
                    //int noOfMatPlansForProject = _context.TblOpProjectBudgetCostings.AsNoTracking().Where(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode && e.ServiceType == "MATERIALEQUIPMENT").GroupBy(e => e.SiteCode).Count();
                    //int noOfExpPlansForProject = _context.TblOpProjectBudgetCostings.AsNoTracking().Where(e=>e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode && e.ServiceType == "FINANCIALEXPENSE").GroupBy(e => e.SiteCode).Count();


                    //project.IsResourcesAssigned = noOfResPlansForProject == noOfSitesforEnquiry;
                    //project.IsExpenceOverheadsAssigned = noOfExpPlansForProject == noOfSitesforEnquiry;
                    //project.IsMaterialAssigned = noOfMatPlansForProject == noOfSitesforEnquiry;
                    //project.IsLogisticsAssigned = noOfLogPlansForProject == noOfSitesforEnquiry;

                    bool ira = project.IsResourcesAssigned = !_context.TblOpProjectSites.AsNoTracking().Any(e => !e.IsAdendum && !e.IsResourcesAssigned);
                    bool iea = project.IsExpenceOverheadsAssigned = !_context.TblOpProjectSites.AsNoTracking().Any(e => !e.IsAdendum && !e.IsExpenceOverheadsAssigned);
                    bool ima = project.IsMaterialAssigned = !_context.TblOpProjectSites.AsNoTracking().Any(e => !e.IsAdendum && !e.IsMaterialAssigned);
                    bool ila = project.IsLogisticsAssigned = !_context.TblOpProjectSites.AsNoTracking().Any(e => !e.IsAdendum && !e.IsLogisticsAssigned);

                    project.IsEstimationCompleted = (ira && iea && ima && ila);



                    //project.IsEstimationCompleted = (project.IsResourcesAssigned &&
                    //        project.IsExpenceOverheadsAssigned &&
                    //        project.IsMaterialAssigned &&
                    //        project.IsLogisticsAssigned
                    //        );
                    _context.OP_HRM_TEMP_Projects.Update(project);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    Log.Info("----Info CreateProjectLogisticsCostingForSite method Exit----");
                    return project.Id;
                }
                catch (Exception ex)
                {
                    Log.Error("Error in CreateProjectLogisticsCosting Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }

    #endregion



    #region GetProjectLogisticsCostingForSite
    public class GetProjectLogisticsCostingForSite : IRequest<TblOpProjectBudgetCostingDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
    }

    public class GetProjectLogisticsCostingForSiteHandler : IRequestHandler<GetProjectLogisticsCostingForSite, TblOpProjectBudgetCostingDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetProjectLogisticsCostingForSiteHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpProjectBudgetCostingDto> Handle(GetProjectLogisticsCostingForSite request, CancellationToken cancellationToken)
        {

            TblOpProjectBudgetCostingDto log = new TblOpProjectBudgetCostingDto();

            TblOpProjectBudgetEstimationDto est = await _context.TblOpProjectBudgetEstimations.AsNoTracking().ProjectTo<TblOpProjectBudgetEstimationDto>(_mapper.ConfigurationProvider).OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectCode == request.ProjectCode && e.CustomerCode == request.CustomerCode);

            TblOpProjectBudgetCostingDto pbc = new TblOpProjectBudgetCostingDto();
            if (est == null)
            {
                log.ProjectBudgetCostingId = 0;
                log.Status = -1;
                log.ProjectBudgetEstimationId = -1;
                pbc = null;
            }

            else
            {

                pbc = await _context.TblOpProjectBudgetCostings.AsNoTracking().ProjectTo<TblOpProjectBudgetCostingDto>(_mapper.ConfigurationProvider).OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectBudgetEstimationId == est.ProjectBudgetEstimationId);



            }
            if (pbc == null)
            {
                log.ProjectBudgetEstimationId = 0;
                log.ProjectBudgetCostingId = 0;
                log.Status = 0;
            }
            else
            {
                log.ProjectBudgetEstimationId = est.ProjectBudgetEstimationId;
                var pbcForSiteLog = await _context.TblOpProjectBudgetCostings.AsNoTracking().OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectBudgetEstimationId == est.ProjectBudgetEstimationId && e.SiteCode == request.SiteCode&& e.ServiceType=="LOGISTICS");
                if (pbcForSiteLog == null)
                {
                    log.Status = 1;
                    log.LogisticsCostingsList = new List<TblOpProjectLogisticsCostingDto>();
                }
                else
                {
                    log.ProjectBudgetCostingId = pbcForSiteLog.ProjectBudgetCostingId;

                    var plc = await _context.TblOpProjectLogisticsCostings.AsNoTracking().FirstOrDefaultAsync(e => e.ProjectBudgetCostingId == pbcForSiteLog.ProjectBudgetCostingId && e.SiteCode == pbcForSiteLog.SiteCode );
                    if (plc == null)
                    {
                        log.LogisticsCostingsList = new List<TblOpProjectLogisticsCostingDto>();
                        log.Status = 2;


                    }
                    else
                    {
                        log.Status = 3;
                        log.LogisticsCostingsList = await _context.TblOpProjectLogisticsCostings.AsNoTracking().ProjectTo<TblOpProjectLogisticsCostingDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectBudgetCostingId == pbcForSiteLog.ProjectBudgetCostingId && e.SiteCode == pbcForSiteLog.SiteCode).ToListAsync();

                        foreach (var item in log.LogisticsCostingsList)
                        {
                            var prscList = await _context.TblOpProjectLogisticsSubCostings.AsNoTracking().ProjectTo<TblOpProjectLogisticsSubCostingDto>(_mapper.ConfigurationProvider).Where(e => e.LogisticsCostingId == item.LogisticsCostingId
                            ).ToListAsync();
                            item.LogisticsSubCostingList = prscList;
                        }
                    }

                }

            }
            log.CustomerCode = request.CustomerCode;
            log.ProjectCode = request.ProjectCode;
            log.ServiceType = "LOGISTICS";
            log.SiteCode = request.SiteCode;

            return log;
        }
    }

    #endregion



    #endregion

    #region MaterialEquipment Costing


    #region CreateProjectMaterialEquipmentCosting
    public class CreateProjectMaterialEquipmentCostingForSite : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblOpProjectBudgetCostingDto Input { get; set; }
    }

    public class CreateProjectMaterialEquipmentCostingForSiteHandler : IRequestHandler<CreateProjectMaterialEquipmentCostingForSite, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateProjectMaterialEquipmentCostingForSiteHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateProjectMaterialEquipmentCostingForSite request, CancellationToken cancellationToken)
        {


            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    Log.Info("----Info CreateProjectMaterialEquipmentCostingForSite method start----");

                    TblOpProjectBudgetEstimation est = new TblOpProjectBudgetEstimation();
                    est.CustomerCode = request.Input.CustomerCode;
                    est.ProjectCode = request.Input.ProjectCode;
                    TblOpProjectBudgetCosting pbc = new TblOpProjectBudgetCosting();

                    var estExist = await _context.TblOpProjectBudgetEstimations.AsNoTracking().OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode);

                    if (estExist == null)
                    {



                        est.Created = DateTime.UtcNow;
                        est.CreatedBy = request.User.UserId;
                        await _context.TblOpProjectBudgetEstimations.AddAsync(est);
                        await _context.SaveChangesAsync();
                        pbc.ProjectBudgetEstimationId = est.ProjectBudgetEstimationId;
                    }
                    else
                    {
                        pbc.ProjectBudgetEstimationId = estExist.ProjectBudgetEstimationId;


                    }
                    var pbcExist = await _context.TblOpProjectBudgetCostings.AsNoTracking().OrderBy(e => e.Created).Where(e =>
                          e.ProjectBudgetEstimationId == est.ProjectBudgetEstimationId
                          && e.ServiceType == "MATERIALEQUIPMENT"
                          && e.SiteCode == request.Input.SiteCode)

                        .FirstOrDefaultAsync();

                    if (pbcExist == null)
                    {

                        pbc.ServiceType = "MATERIALEQUIPMENT";
                        pbc.SiteCode = request.Input.SiteCode;
                        pbc.ProjectCode = request.Input.ProjectCode;
                        pbc.CustomerCode = request.Input.CustomerCode;
                        pbc.Created = DateTime.UtcNow;
                        await _context.TblOpProjectBudgetCostings.AddAsync(pbc);
                        await _context.SaveChangesAsync();

                    }
                    foreach (var prcInput in request.Input.MaterialEquipmentCostingsList)
                    {
                        TblOpProjectMaterialEquipmentCosting plc = new TblOpProjectMaterialEquipmentCosting();
                        plc.ProjectBudgetCostingId = pbc.ProjectBudgetCostingId;
                        plc.MaterialEquipmentCode = prcInput.MaterialEquipmentCode;
                        plc.Quantity = prcInput.Quantity;
                        plc.CostPerUnit = prcInput.CostPerUnit;
                        plc.SiteCode = request.Input.SiteCode;
                        await _context.TblOpProjectMaterialEquipmentCostings.AddAsync(plc);
                        await _context.SaveChangesAsync();

                        foreach (var prscInput in prcInput.MaterialEquipmentSubCostingList)
                        {
                            TblOpProjectMaterialEquipmentSubCosting prsc = new TblOpProjectMaterialEquipmentSubCosting();
                            prsc.CostHead = prscInput.CostHead;
                            prsc.Amount = prscInput.Amount;
                            prsc.MaterialEquipmentCostingId = plc.MaterialEquipmentCostingId;
                            
                            prsc.Created = DateTime.UtcNow;
                            prsc.CreatedBy = request.User.UserId;
                            await _context.TblOpProjectMaterialEquipmentSubCostings.AddAsync(prsc);
                            await _context.SaveChangesAsync();

                        }

                    }

                    var projectSite = await _context.TblOpProjectSites.AsNoTracking().FirstOrDefaultAsync(ps => ps.ProjectCode == request.Input.ProjectCode &&
                ps.SiteCode == request.Input.SiteCode);

                    projectSite.IsMaterialAssigned = true;
                    projectSite.IsEstimationCompleted = projectSite.IsResourcesAssigned && projectSite.IsLogisticsAssigned && projectSite.IsExpenceOverheadsAssigned;

                    _context.TblOpProjectSites.Update(projectSite);
                    await _context.SaveChangesAsync();





                    var project = await _context.OP_HRM_TEMP_Projects.AsNoTracking().FirstOrDefaultAsync(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode);

                    //int noOfSitesforEnquiry = _context.OprEnquiries.AsNoTracking().Where(e => e.EnquiryNumber == project.ProjectCode).GroupBy(x=>x.SiteCode).Count();

                    //int noOfResPlansForProject = _context.TblOpProjectBudgetCostings.AsNoTracking().Where(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode && e.ServiceType == "SKILLSET").GroupBy(e => e.SiteCode).Count();
                    //int noOfLogPlansForProject = _context.TblOpProjectBudgetCostings.AsNoTracking().Where(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode && e.ServiceType == "LOGISTICS").GroupBy(e => e.SiteCode).Count();
                    //int noOfMatPlansForProject = _context.TblOpProjectBudgetCostings.AsNoTracking().Where(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode && e.ServiceType == "MATERIALEQUIPMENT").GroupBy(e => e.SiteCode).Count();
                    //int noOfExpPlansForProject = _context.TblOpProjectBudgetCostings.AsNoTracking().Where(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode && e.ServiceType == "FINANCIALEXPENSE").GroupBy(e => e.SiteCode).Count();


                    //project.IsResourcesAssigned = noOfResPlansForProject == noOfSitesforEnquiry;
                    //project.IsExpenceOverheadsAssigned = noOfExpPlansForProject == noOfSitesforEnquiry;
                    //project.IsMaterialAssigned = noOfMatPlansForProject == noOfSitesforEnquiry;
                    //project.IsLogisticsAssigned = noOfLogPlansForProject == noOfSitesforEnquiry;

                    //project.IsEstimationCompleted = (project.IsResourcesAssigned &&
                    //    project.IsExpenceOverheadsAssigned &&
                    //    project.IsMaterialAssigned &&
                    //    project.IsLogisticsAssigned
                    //    );

                    bool ira = project.IsResourcesAssigned = !_context.TblOpProjectSites.AsNoTracking().Any(e => !e.IsAdendum && !e.IsResourcesAssigned);
                    bool iea = project.IsExpenceOverheadsAssigned = !_context.TblOpProjectSites.AsNoTracking().Any(e => !e.IsAdendum && !e.IsExpenceOverheadsAssigned);
                    bool ima = project.IsMaterialAssigned = !_context.TblOpProjectSites.AsNoTracking().Any(e => !e.IsAdendum && !e.IsMaterialAssigned);
                    bool ila = project.IsLogisticsAssigned = !_context.TblOpProjectSites.AsNoTracking().Any(e => !e.IsAdendum && !e.IsLogisticsAssigned);

                    project.IsEstimationCompleted = (ira && iea && ima && ila);





                    _context.OP_HRM_TEMP_Projects.Update(project);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    Log.Info("----Info CreateProjectMaterialEquipmentCosting method Exit----");
                    return project.Id;
                }
                catch (Exception ex)
                {
                    Log.Error("Error in CreateProjectMaterialEquipmentCosting Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }

    #endregion



    #region GetProjectMaterialEquipmentCostingForSite
    public class GetProjectMaterialEquipmentCostingForSite : IRequest<TblOpProjectBudgetCostingDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
    }

    public class GetProjectMaterialEquipmentCostingForSiteHandler : IRequestHandler<GetProjectMaterialEquipmentCostingForSite, TblOpProjectBudgetCostingDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetProjectMaterialEquipmentCostingForSiteHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpProjectBudgetCostingDto> Handle(GetProjectMaterialEquipmentCostingForSite request, CancellationToken cancellationToken)
        {

            TblOpProjectBudgetCostingDto mat = new TblOpProjectBudgetCostingDto();

            TblOpProjectBudgetEstimationDto est = await _context.TblOpProjectBudgetEstimations.AsNoTracking().ProjectTo<TblOpProjectBudgetEstimationDto>(_mapper.ConfigurationProvider).OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectCode == request.ProjectCode && e.CustomerCode == request.CustomerCode);

            TblOpProjectBudgetCostingDto pbc = new TblOpProjectBudgetCostingDto();
            if (est == null)
            {
                mat.ProjectBudgetCostingId = 0;
                mat.Status = -1;
                mat.ProjectBudgetEstimationId = -1;
                pbc = null;
            }

            else
            {

                pbc = await _context.TblOpProjectBudgetCostings.AsNoTracking().ProjectTo<TblOpProjectBudgetCostingDto>(_mapper.ConfigurationProvider).OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectBudgetEstimationId == est.ProjectBudgetEstimationId);



            }
            if (pbc == null)
            {
                mat.ProjectBudgetEstimationId = 0;
                mat.ProjectBudgetCostingId = 0;
                mat.Status = 0;
            }
            else
            {
                mat.ProjectBudgetEstimationId = est.ProjectBudgetEstimationId;
                var pbcForSiteMat = await _context.TblOpProjectBudgetCostings.AsNoTracking().OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectBudgetEstimationId == est.ProjectBudgetEstimationId && e.SiteCode == request.SiteCode && e.ServiceType == "MATERIALEQUIPMENT");
                if (pbcForSiteMat == null)
                {
                    mat.Status = 1;
                    mat.MaterialEquipmentCostingsList = new List<TblOpProjectMaterialEquipmentCostingDto>();
                }
                else
                {
                    mat.ProjectBudgetCostingId = pbcForSiteMat.ProjectBudgetCostingId;

                    var pmc = await _context.TblOpProjectMaterialEquipmentCostings.AsNoTracking().FirstOrDefaultAsync(e => e.ProjectBudgetCostingId == pbcForSiteMat.ProjectBudgetCostingId && e.SiteCode == pbcForSiteMat.SiteCode);
                    if (pmc == null)
                    {
                        mat.MaterialEquipmentCostingsList = new List<TblOpProjectMaterialEquipmentCostingDto>();
                        mat.Status = 2;


                    }
                    else
                    {
                        mat.Status = 3;
                        mat.MaterialEquipmentCostingsList = await _context.TblOpProjectMaterialEquipmentCostings.AsNoTracking().ProjectTo<TblOpProjectMaterialEquipmentCostingDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectBudgetCostingId == pbcForSiteMat.ProjectBudgetCostingId && e.SiteCode == pbcForSiteMat.SiteCode).ToListAsync();

                        foreach (var item in mat.MaterialEquipmentCostingsList)
                        {
                            var pmscList = await _context.TblOpProjectMaterialEquipmentSubCostings.AsNoTracking().ProjectTo<TblOpProjectMaterialEquipmentSubCostingDto>(_mapper.ConfigurationProvider).Where(e => e.MaterialEquipmentCostingId == item.MaterialEquipmentCostingId
                            ).ToListAsync();
                            item.MaterialEquipmentSubCostingList = pmscList;
                        }
                    }

                }

            }
            mat.CustomerCode = request.CustomerCode;
            mat.ProjectCode = request.ProjectCode;
            mat.ServiceType = "MATERIALEQUIPMENT";
            mat.SiteCode = request.SiteCode;

            return mat;
        }
    }

    #endregion



    #endregion

    #region FinancialExpense Costing


    #region CreateProjectFinancialExpenseCosting
    public class CreateProjectFinancialExpenseCostingForSite : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblOpProjectBudgetCostingDto Input { get; set; }
    }

    public class CreateProjectFinancialExpenseCostingForSiteHandler : IRequestHandler<CreateProjectFinancialExpenseCostingForSite, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateProjectFinancialExpenseCostingForSiteHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateProjectFinancialExpenseCostingForSite request, CancellationToken cancellationToken)
        {


            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    Log.Info("----Info CreateProjectFinancialExpenseCostingForSite method start----");

                    TblOpProjectBudgetEstimation est = new TblOpProjectBudgetEstimation();
                    est.CustomerCode = request.Input.CustomerCode;
                    est.ProjectCode = request.Input.ProjectCode;
                    TblOpProjectBudgetCosting pbc = new TblOpProjectBudgetCosting();

                    var estExist = await _context.TblOpProjectBudgetEstimations.AsNoTracking().OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode);

                    if (estExist == null)
                    {



                        est.Created = DateTime.UtcNow;
                        est.CreatedBy = request.User.UserId;
                        await _context.TblOpProjectBudgetEstimations.AddAsync(est);
                        await _context.SaveChangesAsync();
                        pbc.ProjectBudgetEstimationId = est.ProjectBudgetEstimationId;
                    }
                    else
                    {
                        pbc.ProjectBudgetEstimationId = estExist.ProjectBudgetEstimationId;


                    }
                    var pbcExist = await _context.TblOpProjectBudgetCostings.AsNoTracking().OrderBy(e => e.Created).Where(e =>
                          e.ProjectBudgetEstimationId == est.ProjectBudgetEstimationId
                          && e.ServiceType == "FINANCIALEXPENSE"
                          && e.SiteCode == request.Input.SiteCode)

                        .FirstOrDefaultAsync();

                    if (pbcExist == null)
                    {

                        pbc.ServiceType = "FINANCIALEXPENSE";
                        pbc.SiteCode = request.Input.SiteCode;
                        pbc.ProjectCode = request.Input.ProjectCode;
                        pbc.CustomerCode = request.Input.CustomerCode;
                        pbc.Created = DateTime.UtcNow;
                        await _context.TblOpProjectBudgetCostings.AddAsync(pbc);
                        await _context.SaveChangesAsync();

                    }
                    foreach (var prcInput in request.Input.FinancialExpenseCostingsList)
                    {
                        TblOpProjectFinancialExpenseCosting plc = new TblOpProjectFinancialExpenseCosting();
                        plc.ProjectBudgetCostingId = pbc.ProjectBudgetCostingId;
                        plc.FinancialExpenseCode = prcInput.FinancialExpenseCode;
                        plc.Created = DateTime.UtcNow;
                        plc.CreatedBy = request.User.UserId;
                        plc.CostPerUnit = prcInput.CostPerUnit;
                     
                        await _context.TblOpProjectFinancialExpenseCostings.AddAsync(plc);
                        await _context.SaveChangesAsync();

                       
                    }



                    var projectSite = await _context.TblOpProjectSites.AsNoTracking().FirstOrDefaultAsync(ps => ps.ProjectCode == request.Input.ProjectCode &&
             ps.SiteCode == request.Input.SiteCode);

                    projectSite.IsExpenceOverheadsAssigned = true;
                    projectSite.IsEstimationCompleted = projectSite.IsResourcesAssigned && projectSite.IsLogisticsAssigned && projectSite.IsMaterialAssigned;
                    _context.TblOpProjectSites.Update(projectSite);
                    await _context.SaveChangesAsync();





                    var project = await _context.OP_HRM_TEMP_Projects.AsNoTracking().FirstOrDefaultAsync(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode);

                    //int noOfSitesforEnquiry = _context.OprEnquiries.AsNoTracking().Where(e => e.EnquiryNumber == project.ProjectCode).GroupBy(x=>x.SiteCode).Count();

                    //int noOfResPlansForProject = _context.TblOpProjectBudgetCostings.AsNoTracking().Where(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode && e.ServiceType == "SKILLSET").GroupBy(e => e.SiteCode).Count();
                    //int noOfLogPlansForProject = _context.TblOpProjectBudgetCostings.AsNoTracking().Where(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode && e.ServiceType == "LOGISTICS").GroupBy(e => e.SiteCode).Count();
                    //int noOfMatPlansForProject = _context.TblOpProjectBudgetCostings.AsNoTracking().Where(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode && e.ServiceType == "MATERIALEQUIPMENT").GroupBy(e => e.SiteCode).Count();
                    //int noOfExpPlansForProject = _context.TblOpProjectBudgetCostings.AsNoTracking().Where(e => e.ProjectCode == request.Input.ProjectCode && e.CustomerCode == request.Input.CustomerCode && e.ServiceType == "FINANCIALEXPENSE").GroupBy(e => e.SiteCode).Count();


                    //project.IsResourcesAssigned = noOfResPlansForProject == noOfSitesforEnquiry;
                    //project.IsExpenceOverheadsAssigned = noOfExpPlansForProject == noOfSitesforEnquiry;
                    //project.IsMaterialAssigned = noOfMatPlansForProject == noOfSitesforEnquiry;
                    //project.IsLogisticsAssigned = noOfLogPlansForProject == noOfSitesforEnquiry;

                    //project.IsEstimationCompleted = (project.IsResourcesAssigned &&
                    //    project.IsExpenceOverheadsAssigned &&
                    //    project.IsMaterialAssigned &&
                    //    project.IsLogisticsAssigned
                    //    );

                    bool ira = project.IsResourcesAssigned = !_context.TblOpProjectSites.AsNoTracking().Any(e => !e.IsAdendum && !e.IsResourcesAssigned);
                    bool iea = project.IsExpenceOverheadsAssigned = !_context.TblOpProjectSites.AsNoTracking().Any(e => !e.IsAdendum && !e.IsExpenceOverheadsAssigned);
                    bool ima = project.IsMaterialAssigned = !_context.TblOpProjectSites.AsNoTracking().Any(e => !e.IsAdendum && !e.IsMaterialAssigned);
                    bool ila = project.IsLogisticsAssigned = !_context.TblOpProjectSites.AsNoTracking().Any(e => !e.IsAdendum && !e.IsLogisticsAssigned);

                    project.IsEstimationCompleted = (ira && iea && ima && ila);



                    _context.OP_HRM_TEMP_Projects.Update(project);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    Log.Info("----Info CreateProjectMaterialEquipmentCosting method Exit----");
                    return project.Id;
                }
                catch (Exception ex)
                {
                    Log.Error("Error in CreateProjectFinancialExpenseCosting Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }
    }

    #endregion



    #region GetProjectFinancialExpenseCostingForSite
    public class GetProjectFinancialExpenseCostingForSite : IRequest<TblOpProjectBudgetCostingDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
    }

    public class GetProjectFinancialExpenseCostingForSiteHandler : IRequestHandler<GetProjectFinancialExpenseCostingForSite, TblOpProjectBudgetCostingDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetProjectFinancialExpenseCostingForSiteHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpProjectBudgetCostingDto> Handle(GetProjectFinancialExpenseCostingForSite request, CancellationToken cancellationToken)
        {

            TblOpProjectBudgetCostingDto finExp = new TblOpProjectBudgetCostingDto();

            TblOpProjectBudgetEstimationDto est = await _context.TblOpProjectBudgetEstimations.AsNoTracking().ProjectTo<TblOpProjectBudgetEstimationDto>(_mapper.ConfigurationProvider).OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectCode == request.ProjectCode && e.CustomerCode == request.CustomerCode);

            TblOpProjectBudgetCostingDto pbc = new TblOpProjectBudgetCostingDto();
            if (est == null)
            {
                finExp.ProjectBudgetCostingId = 0;
                finExp.Status = -1;
                finExp.ProjectBudgetEstimationId = -1;
                pbc = null;
            }

            else
            {

                pbc = await _context.TblOpProjectBudgetCostings.AsNoTracking().ProjectTo<TblOpProjectBudgetCostingDto>(_mapper.ConfigurationProvider).OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectBudgetEstimationId == est.ProjectBudgetEstimationId);



            }
            if (pbc == null)
            {
                finExp.ProjectBudgetEstimationId = 0;
                finExp.ProjectBudgetCostingId = 0;
                finExp.Status = 0;
            }
            else
            {
                finExp.ProjectBudgetEstimationId = est.ProjectBudgetEstimationId;
                var pbcForSiteFinExp = await _context.TblOpProjectBudgetCostings.AsNoTracking().OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectBudgetEstimationId == est.ProjectBudgetEstimationId && e.SiteCode == request.SiteCode && e.ServiceType == "FINANCIALEXPENSE");
                if (pbcForSiteFinExp == null)
                {
                    finExp.Status = 1;
                    finExp.FinancialExpenseCostingsList = new List<TblOpProjectFinancialExpenseCostingDto>();
                }
                else
                {
                    finExp.ProjectBudgetCostingId = pbcForSiteFinExp.ProjectBudgetCostingId;

                    var pfc = await _context.TblOpProjectFinancialExpenseCostings.AsNoTracking().FirstOrDefaultAsync(e => e.ProjectBudgetCostingId == pbcForSiteFinExp.ProjectBudgetCostingId);
                    if (pfc == null)
                    {
                        finExp.FinancialExpenseCostingsList = new List<TblOpProjectFinancialExpenseCostingDto>();
                        finExp.Status = 2;


                    }
                    else
                    {
                        finExp.Status = 3;
                        finExp.FinancialExpenseCostingsList = await _context.TblOpProjectFinancialExpenseCostings.AsNoTracking().ProjectTo<TblOpProjectFinancialExpenseCostingDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectBudgetCostingId == pbcForSiteFinExp.ProjectBudgetCostingId ).ToListAsync();

                        
                    }

                }

            }
            finExp.CustomerCode = request.CustomerCode;
            finExp.ProjectCode = request.ProjectCode;
            finExp.ServiceType = "FINANCIALEXPENSE";
            finExp.SiteCode = request.SiteCode;

            return finExp;
        }
    }

    #endregion



    #endregion

  


 



    #region GetProjectEstimation
    public class GetProjectEstimation : IRequest<TblOpProjectBudgetEstimationDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        
    }

    public class GetProjectEstimationHandler : IRequestHandler<GetProjectEstimation, TblOpProjectBudgetEstimationDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetProjectEstimationHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpProjectBudgetEstimationDto> Handle(GetProjectEstimation request, CancellationToken cancellationToken)
        {
            TblOpProjectBudgetEstimationDto resEst = new TblOpProjectBudgetEstimationDto();
            resEst.CustomerCode = request.CustomerCode;
            resEst.ProjectCode = request.ProjectCode;
            TblOpProjectBudgetEstimationDto est = await _context.TblOpProjectBudgetEstimations.AsNoTracking().ProjectTo<TblOpProjectBudgetEstimationDto>(_mapper.ConfigurationProvider).OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectCode == request.ProjectCode && e.CustomerCode == request.CustomerCode);

            if (est == null)
            {
                return null;
            }
            else {

                resEst.ProjectBudgetEstimationId = est.ProjectBudgetEstimationId;
                resEst.PreviousEstimatonId = est.PreviousEstimatonId;
                resEst.SiteWisePBCListForProject= resEst.SiteWisePBCListForProject = new List<PBCForSiteDto>();
                resEst.GrandTotalCostForProject = 0;
            }

            var sites = await _context.OprSites.AsNoTracking().ToListAsync();
           // var enquiries = await _context.OprEnquiries.AsNoTracking().Where(e => e.EnquiryNumber == request.ProjectCode).ToListAsync();
            var projectSites = await _context.TblOpProjectSites.AsNoTracking().Where(e => e.ProjectCode == request.ProjectCode && !e.IsAdendum).ToListAsync();
            List<TblSndDefSiteMasterDto> selectionSiteList = (from e in projectSites
                                                              group e by e.SiteCode into sg
                                                              join s in sites on sg.FirstOrDefault().SiteCode equals s.SiteCode
                                                              select new TblSndDefSiteMasterDto
                                                              {
                                                                  Id = s.Id,
                                                                  SiteCode = s.SiteCode,
                                                                  SiteName = s.SiteName,
                                                                  SiteArbName = s.SiteArbName,
                                                                  CustomerCode = s.CustomerCode,
                                                                  SiteAddress = s.SiteAddress,
                                                                  SiteCityCode = s.SiteCityCode,
                                                                  SiteGeoLatitude = s.SiteGeoLatitude,
                                                                  SiteGeoLongitude = s.SiteGeoLongitude,
                                                                  SiteGeoGain = s.SiteGeoGain
                                                              }).ToList();
            

           

            foreach (var site in selectionSiteList)
            {
                PBCForSiteDto pbcforsite = new PBCForSiteDto();
                pbcforsite.siteData = site;

                TblOpProjectBudgetCostingDto res = new TblOpProjectBudgetCostingDto();
                TblOpProjectBudgetCostingDto log = new TblOpProjectBudgetCostingDto();
                TblOpProjectBudgetCostingDto mat = new TblOpProjectBudgetCostingDto();
                TblOpProjectBudgetCostingDto finExp = new TblOpProjectBudgetCostingDto();
                //TblOpProjectBudgetCostingDto pbc = new TblOpProjectBudgetCostingDto();
                //pbc = await _context.TblOpProjectBudgetCostings.AsNoTracking().ProjectTo<TblOpProjectBudgetCostingDto>(_mapper.ConfigurationProvider).OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectBudgetEstimationId == est.ProjectBudgetEstimationId);
                res.ProjectBudgetEstimationId = est.ProjectBudgetEstimationId;
                
                var pbcForSiteRes = await _context.TblOpProjectBudgetCostings.AsNoTracking().OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectBudgetEstimationId == est.ProjectBudgetEstimationId && e.SiteCode == site.SiteCode && e.ServiceType == "SKILLSET");
                if (pbcForSiteRes == null)
                {
                    res.Status = 1;
                    res.ResourceCostingsList = new List<TblOpProjectResourceCostingDto>();
                }
                else
                {
                    res.ProjectBudgetCostingId = pbcForSiteRes.ProjectBudgetCostingId;

                    var plc = await _context.TblOpProjectResourceCosting.AsNoTracking().FirstOrDefaultAsync(e => e.ProjectBudgetCostingId == pbcForSiteRes.ProjectBudgetCostingId && e.SiteCode == pbcForSiteRes.SiteCode);
                    if (plc == null)
                    {
                        res.ResourceCostingsList = new List<TblOpProjectResourceCostingDto>();
                        res.Status = 2;


                    }
                    else
                    {
                        res.Status = 3;
                        res.ResourceCostingsList = await _context.TblOpProjectResourceCosting.AsNoTracking().ProjectTo<TblOpProjectResourceCostingDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectBudgetCostingId == pbcForSiteRes.ProjectBudgetCostingId && e.SiteCode == site.SiteCode).ToListAsync();

                        foreach (var item in res.ResourceCostingsList)
                        {
                            var prscList = await _context.TblOpProjectResourceSubCostings.AsNoTracking().ProjectTo<TblOpProjectResourceSubCostingDto>(_mapper.ConfigurationProvider).Where(e => e.ResourceCostingId == item.ResourceCostingId
                            ).ToListAsync();
                            item.ResourceSubCostingList = prscList;
                        }
                    }

                }
            res.CustomerCode = request.CustomerCode;
            res.ProjectCode = request.ProjectCode;
            res.ServiceType = "SKILLSET";
            res.SiteCode = site.SiteCode;
            pbcforsite.PrcListForSite = res.ResourceCostingsList;


                log.ProjectBudgetEstimationId = est.ProjectBudgetEstimationId;
                var pbcForSiteLog = await _context.TblOpProjectBudgetCostings.AsNoTracking().OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectBudgetEstimationId == est.ProjectBudgetEstimationId && e.SiteCode == site.SiteCode && e.ServiceType == "LOGISTICS");
                if (pbcForSiteLog == null)
                {
                    log.Status = 1;
                    log.LogisticsCostingsList = new List<TblOpProjectLogisticsCostingDto>();
                }
                else
                {
                    log.ProjectBudgetCostingId = pbcForSiteLog.ProjectBudgetCostingId;

                    var plc = await _context.TblOpProjectLogisticsCostings.AsNoTracking().FirstOrDefaultAsync(e => e.ProjectBudgetCostingId == pbcForSiteLog.ProjectBudgetCostingId);
                    if (plc == null)
                    {
                        log.LogisticsCostingsList = new List<TblOpProjectLogisticsCostingDto>();
                        log.Status = 2;


                    }
                    else
                    {
                        log.Status = 3;
                        log.LogisticsCostingsList = await _context.TblOpProjectLogisticsCostings.AsNoTracking().ProjectTo<TblOpProjectLogisticsCostingDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectBudgetCostingId == pbcForSiteLog.ProjectBudgetCostingId).ToListAsync();

                        foreach (var item in log.LogisticsCostingsList)
                        {
                            var prscList = await _context.TblOpProjectLogisticsSubCostings.AsNoTracking().ProjectTo<TblOpProjectLogisticsSubCostingDto>(_mapper.ConfigurationProvider).Where(e => e.LogisticsCostingId == item.LogisticsCostingId
                            ).ToListAsync();
                            item.LogisticsSubCostingList = prscList;
                        }
                    }

                }
            log.CustomerCode = request.CustomerCode;
            log.ProjectCode = request.ProjectCode;
            log.ServiceType = "LOGISTICS";
            log.SiteCode = site.SiteCode;
            pbcforsite.PlcListForSite = log.LogisticsCostingsList;


                mat.ProjectBudgetEstimationId = est.ProjectBudgetEstimationId;
                var pbcForSiteMat = await _context.TblOpProjectBudgetCostings.AsNoTracking().OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectBudgetEstimationId == est.ProjectBudgetEstimationId && e.SiteCode == site.SiteCode && e.ServiceType == "MATERIALEQUIPMENT");
                if (pbcForSiteMat == null)
                {
                    mat.Status = 1;
                    mat.MaterialEquipmentCostingsList = new List<TblOpProjectMaterialEquipmentCostingDto>();
                }
                else
                {
                    mat.ProjectBudgetCostingId = pbcForSiteMat.ProjectBudgetCostingId;

                    var pmc = await _context.TblOpProjectMaterialEquipmentCostings.AsNoTracking().FirstOrDefaultAsync(e => e.ProjectBudgetCostingId == pbcForSiteMat.ProjectBudgetCostingId);
                    if (pmc == null)
                    {
                        mat.MaterialEquipmentCostingsList = new List<TblOpProjectMaterialEquipmentCostingDto>();
                        mat.Status = 2;


                    }
                    else
                    {
                        mat.Status = 3;
                        mat.MaterialEquipmentCostingsList = await _context.TblOpProjectMaterialEquipmentCostings.AsNoTracking().ProjectTo<TblOpProjectMaterialEquipmentCostingDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectBudgetCostingId == pbcForSiteMat.ProjectBudgetCostingId && e.SiteCode == pbcForSiteMat.SiteCode).ToListAsync();

                        foreach (var item in mat.MaterialEquipmentCostingsList)
                        {
                            var pmscList = await _context.TblOpProjectMaterialEquipmentSubCostings.AsNoTracking().ProjectTo<TblOpProjectMaterialEquipmentSubCostingDto>(_mapper.ConfigurationProvider).Where(e => e.MaterialEquipmentCostingId == item.MaterialEquipmentCostingId
                            ).ToListAsync();
                            item.MaterialEquipmentSubCostingList = pmscList;
                        }
                    }

                }

            
            mat.CustomerCode = request.CustomerCode;
            mat.ProjectCode = request.ProjectCode;
            mat.ServiceType = "MATERIALEQUIPMENT";
            mat.SiteCode = site.SiteCode;
            pbcforsite.PmcListForSite = mat.MaterialEquipmentCostingsList;


                finExp.ProjectBudgetEstimationId = est.ProjectBudgetEstimationId;
                var pbcForSiteFinExp = await _context.TblOpProjectBudgetCostings.AsNoTracking().OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectBudgetEstimationId == est.ProjectBudgetEstimationId && e.SiteCode == site.SiteCode && e.ServiceType == "FINANCIALEXPENSE");
                if (pbcForSiteFinExp == null)
                {
                    finExp.Status = 1;
                    finExp.FinancialExpenseCostingsList = new List<TblOpProjectFinancialExpenseCostingDto>();
                }
                else
                {
                    finExp.ProjectBudgetCostingId = pbcForSiteFinExp.ProjectBudgetCostingId;

                    var pfc = await _context.TblOpProjectFinancialExpenseCostings.AsNoTracking().FirstOrDefaultAsync(e => e.ProjectBudgetCostingId == pbcForSiteFinExp.ProjectBudgetCostingId);
                    if (pfc == null)
                    {
                        finExp.FinancialExpenseCostingsList = new List<TblOpProjectFinancialExpenseCostingDto>();
                        finExp.Status = 2;


                    }
                    else
                    {
                        finExp.Status = 3;
                        finExp.FinancialExpenseCostingsList = await _context.TblOpProjectFinancialExpenseCostings.AsNoTracking().ProjectTo<TblOpProjectFinancialExpenseCostingDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectBudgetCostingId == pbcForSiteFinExp.ProjectBudgetCostingId).ToListAsync();


                    }

                }
            
            finExp.CustomerCode = request.CustomerCode;
            finExp.ProjectCode = request.ProjectCode;
            finExp.ServiceType = "FINANCIALEXPENSE";
            finExp.SiteCode = site.SiteCode;
                pbcforsite.PfcListForSite = finExp.FinancialExpenseCostingsList;
                resEst.SiteWisePBCListForProject.Add(pbcforsite);
        }
            return resEst;

        }
    }

    #endregion




  

    #region ConvertProjectToProposal
    public class ConvertProjectToProposal : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblOpProjectBudgetCostingDto Project { get; set; }
    }

    public class ConvertProjectToProposalHandler : IRequestHandler<ConvertProjectToProposal, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public ConvertProjectToProposalHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(ConvertProjectToProposal request, CancellationToken cancellationToken)
        {


          
                try
                {

                    Log.Info("----Info ConvertProjectToProposal method start----");

                    var project = await _context.OP_HRM_TEMP_Projects.AsNoTracking().FirstOrDefaultAsync(e=>e.ProjectCode==request.Project.ProjectCode);
                    if (project is null)
                        return 0;
                    project.IsConvertedToProposal = true;
                    _context.Update(project);
                 _context.SaveChanges();


                var projectSites = await _context.TblOpProjectSites.AsNoTracking().Where(e=>e.ProjectCode==request.Project.ProjectCode && !e.IsAdendum).ToListAsync();
                if (projectSites.Count>0)
                {
                    foreach (var site in projectSites) {
                        site.IsConvertedToProposal = true;
                    } ;
                    _context.TblOpProjectSites.UpdateRange(projectSites);
                    _context.SaveChanges();
                }
                  
                    Log.Info("----Info ConvertProjectToProposal method Exit----");
                    return 1;
                }
                catch (Exception ex)
                {
                    Log.Error("Error in ConvertProjectToProposal Method");
                    Log.Error("Error occured time : " + DateTime.UtcNow);
                    Log.Error("Error message : " + ex.Message);
                    Log.Error("Error StackTrace : " + ex.StackTrace);
                    return 0;
                }
            }
        }

    #endregion



    #region GetProjectSiteEstimation
    public class GetProjectSiteEstimation : IRequest<TblOpProjectBudgetEstimationDto>
    {
        public UserIdentityDto User { get; set; }
        public string CustomerCode { get; set; }
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }

    }

    public class GetProjectSiteEstimationHandler : IRequestHandler<GetProjectSiteEstimation, TblOpProjectBudgetEstimationDto>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetProjectSiteEstimationHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TblOpProjectBudgetEstimationDto> Handle(GetProjectSiteEstimation request, CancellationToken cancellationToken)
        {
            TblOpProjectBudgetEstimationDto resEst = new TblOpProjectBudgetEstimationDto();
            resEst.CustomerCode = request.CustomerCode;
            resEst.ProjectCode = request.ProjectCode;
            TblOpProjectBudgetEstimationDto est = await _context.TblOpProjectBudgetEstimations.AsNoTracking().ProjectTo<TblOpProjectBudgetEstimationDto>(_mapper.ConfigurationProvider).OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectCode == request.ProjectCode && e.CustomerCode == request.CustomerCode);

            if (est == null)
            {
                return null;
            }
            else
            {

                resEst.ProjectBudgetEstimationId = est.ProjectBudgetEstimationId;
                resEst.PreviousEstimatonId = est.PreviousEstimatonId;
                resEst.SiteWisePBCListForProject = resEst.SiteWisePBCListForProject = new List<PBCForSiteDto>();
                resEst.GrandTotalCostForProject = 0;
            }

            var sites = await _context.OprSites.AsNoTracking().Where(e=>e.SiteCode==request.SiteCode).ToListAsync();
            // var enquiries = await _context.OprEnquiries.AsNoTracking().Where(e => e.EnquiryNumber == request.ProjectCode).ToListAsync();
            var projectSites = await _context.TblOpProjectSites.AsNoTracking().Where(e => e.ProjectCode == request.ProjectCode && e.SiteCode== request.SiteCode).ToListAsync();
            List<TblSndDefSiteMasterDto> selectionSiteList = (from e in projectSites
                                                              group e by e.SiteCode into sg
                                                              join s in sites on sg.FirstOrDefault().SiteCode equals s.SiteCode
                                                              select new TblSndDefSiteMasterDto
                                                              {
                                                                  Id = s.Id,
                                                                  SiteCode = s.SiteCode,
                                                                  SiteName = s.SiteName,
                                                                  SiteArbName = s.SiteArbName,
                                                                  CustomerCode = s.CustomerCode,
                                                                  SiteAddress = s.SiteAddress,
                                                                  SiteCityCode = s.SiteCityCode,
                                                                  SiteGeoLatitude = s.SiteGeoLatitude,
                                                                  SiteGeoLongitude = s.SiteGeoLongitude,
                                                                  SiteGeoGain = s.SiteGeoGain
                                                              }).ToList();




            foreach (var site in selectionSiteList)
            {
                PBCForSiteDto pbcforsite = new PBCForSiteDto();
                pbcforsite.siteData = site;

                TblOpProjectBudgetCostingDto res = new TblOpProjectBudgetCostingDto();
                TblOpProjectBudgetCostingDto log = new TblOpProjectBudgetCostingDto();
                TblOpProjectBudgetCostingDto mat = new TblOpProjectBudgetCostingDto();
                TblOpProjectBudgetCostingDto finExp = new TblOpProjectBudgetCostingDto();
                //TblOpProjectBudgetCostingDto pbc = new TblOpProjectBudgetCostingDto();
                //pbc = await _context.TblOpProjectBudgetCostings.AsNoTracking().ProjectTo<TblOpProjectBudgetCostingDto>(_mapper.ConfigurationProvider).OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectBudgetEstimationId == est.ProjectBudgetEstimationId);
                res.ProjectBudgetEstimationId = est.ProjectBudgetEstimationId;

                var pbcForSiteRes = await _context.TblOpProjectBudgetCostings.AsNoTracking().OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectBudgetEstimationId == est.ProjectBudgetEstimationId && e.SiteCode == site.SiteCode && e.ServiceType == "SKILLSET");
                if (pbcForSiteRes == null)
                {
                    res.Status = 1;
                    res.ResourceCostingsList = new List<TblOpProjectResourceCostingDto>();
                }
                else
                {
                    res.ProjectBudgetCostingId = pbcForSiteRes.ProjectBudgetCostingId;

                    var plc = await _context.TblOpProjectResourceCosting.AsNoTracking().FirstOrDefaultAsync(e => e.ProjectBudgetCostingId == pbcForSiteRes.ProjectBudgetCostingId && e.SiteCode == pbcForSiteRes.SiteCode);
                    if (plc == null)
                    {
                        res.ResourceCostingsList = new List<TblOpProjectResourceCostingDto>();
                        res.Status = 2;


                    }
                    else
                    {
                        res.Status = 3;
                        res.ResourceCostingsList = await _context.TblOpProjectResourceCosting.AsNoTracking().ProjectTo<TblOpProjectResourceCostingDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectBudgetCostingId == pbcForSiteRes.ProjectBudgetCostingId && e.SiteCode == site.SiteCode).ToListAsync();

                        foreach (var item in res.ResourceCostingsList)
                        {
                            var prscList = await _context.TblOpProjectResourceSubCostings.AsNoTracking().ProjectTo<TblOpProjectResourceSubCostingDto>(_mapper.ConfigurationProvider).Where(e => e.ResourceCostingId == item.ResourceCostingId
                            ).ToListAsync();
                            item.ResourceSubCostingList = prscList;
                        }
                    }

                }
                res.CustomerCode = request.CustomerCode;
                res.ProjectCode = request.ProjectCode;
                res.ServiceType = "SKILLSET";
                res.SiteCode = site.SiteCode;
                pbcforsite.PrcListForSite = res.ResourceCostingsList;


                log.ProjectBudgetEstimationId = est.ProjectBudgetEstimationId;
                var pbcForSiteLog = await _context.TblOpProjectBudgetCostings.AsNoTracking().OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectBudgetEstimationId == est.ProjectBudgetEstimationId && e.SiteCode == site.SiteCode && e.ServiceType == "LOGISTICS");
                if (pbcForSiteLog == null)
                {
                    log.Status = 1;
                    log.LogisticsCostingsList = new List<TblOpProjectLogisticsCostingDto>();
                }
                else
                {
                    log.ProjectBudgetCostingId = pbcForSiteLog.ProjectBudgetCostingId;

                    var plc = await _context.TblOpProjectLogisticsCostings.AsNoTracking().FirstOrDefaultAsync(e => e.ProjectBudgetCostingId == pbcForSiteLog.ProjectBudgetCostingId);
                    if (plc == null)
                    {
                        log.LogisticsCostingsList = new List<TblOpProjectLogisticsCostingDto>();
                        log.Status = 2;


                    }
                    else
                    {
                        log.Status = 3;
                        log.LogisticsCostingsList = await _context.TblOpProjectLogisticsCostings.AsNoTracking().ProjectTo<TblOpProjectLogisticsCostingDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectBudgetCostingId == pbcForSiteLog.ProjectBudgetCostingId).ToListAsync();

                        foreach (var item in log.LogisticsCostingsList)
                        {
                            var prscList = await _context.TblOpProjectLogisticsSubCostings.AsNoTracking().ProjectTo<TblOpProjectLogisticsSubCostingDto>(_mapper.ConfigurationProvider).Where(e => e.LogisticsCostingId == item.LogisticsCostingId
                            ).ToListAsync();
                            item.LogisticsSubCostingList = prscList;
                        }
                    }

                }
                log.CustomerCode = request.CustomerCode;
                log.ProjectCode = request.ProjectCode;
                log.ServiceType = "LOGISTICS";
                log.SiteCode = site.SiteCode;
                pbcforsite.PlcListForSite = log.LogisticsCostingsList;


                mat.ProjectBudgetEstimationId = est.ProjectBudgetEstimationId;
                var pbcForSiteMat = await _context.TblOpProjectBudgetCostings.AsNoTracking().OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectBudgetEstimationId == est.ProjectBudgetEstimationId && e.SiteCode == site.SiteCode && e.ServiceType == "MATERIALEQUIPMENT");
                if (pbcForSiteMat == null)
                {
                    mat.Status = 1;
                    mat.MaterialEquipmentCostingsList = new List<TblOpProjectMaterialEquipmentCostingDto>();
                }
                else
                {
                    mat.ProjectBudgetCostingId = pbcForSiteMat.ProjectBudgetCostingId;

                    var pmc = await _context.TblOpProjectMaterialEquipmentCostings.AsNoTracking().FirstOrDefaultAsync(e => e.ProjectBudgetCostingId == pbcForSiteMat.ProjectBudgetCostingId);
                    if (pmc == null)
                    {
                        mat.MaterialEquipmentCostingsList = new List<TblOpProjectMaterialEquipmentCostingDto>();
                        mat.Status = 2;


                    }
                    else
                    {
                        mat.Status = 3;
                        mat.MaterialEquipmentCostingsList = await _context.TblOpProjectMaterialEquipmentCostings.AsNoTracking().ProjectTo<TblOpProjectMaterialEquipmentCostingDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectBudgetCostingId == pbcForSiteMat.ProjectBudgetCostingId && e.SiteCode == pbcForSiteMat.SiteCode).ToListAsync();

                        foreach (var item in mat.MaterialEquipmentCostingsList)
                        {
                            var pmscList = await _context.TblOpProjectMaterialEquipmentSubCostings.AsNoTracking().ProjectTo<TblOpProjectMaterialEquipmentSubCostingDto>(_mapper.ConfigurationProvider).Where(e => e.MaterialEquipmentCostingId == item.MaterialEquipmentCostingId
                            ).ToListAsync();
                            item.MaterialEquipmentSubCostingList = pmscList;
                        }
                    }

                }


                mat.CustomerCode = request.CustomerCode;
                mat.ProjectCode = request.ProjectCode;
                mat.ServiceType = "MATERIALEQUIPMENT";
                mat.SiteCode = site.SiteCode;
                pbcforsite.PmcListForSite = mat.MaterialEquipmentCostingsList;


                finExp.ProjectBudgetEstimationId = est.ProjectBudgetEstimationId;
                var pbcForSiteFinExp = await _context.TblOpProjectBudgetCostings.AsNoTracking().OrderByDescending(e => e.Created).FirstOrDefaultAsync(e => e.ProjectBudgetEstimationId == est.ProjectBudgetEstimationId && e.SiteCode == site.SiteCode && e.ServiceType == "FINANCIALEXPENSE");
                if (pbcForSiteFinExp == null)
                {
                    finExp.Status = 1;
                    finExp.FinancialExpenseCostingsList = new List<TblOpProjectFinancialExpenseCostingDto>();
                }
                else
                {
                    finExp.ProjectBudgetCostingId = pbcForSiteFinExp.ProjectBudgetCostingId;

                    var pfc = await _context.TblOpProjectFinancialExpenseCostings.AsNoTracking().FirstOrDefaultAsync(e => e.ProjectBudgetCostingId == pbcForSiteFinExp.ProjectBudgetCostingId);
                    if (pfc == null)
                    {
                        finExp.FinancialExpenseCostingsList = new List<TblOpProjectFinancialExpenseCostingDto>();
                        finExp.Status = 2;


                    }
                    else
                    {
                        finExp.Status = 3;
                        finExp.FinancialExpenseCostingsList = await _context.TblOpProjectFinancialExpenseCostings.AsNoTracking().ProjectTo<TblOpProjectFinancialExpenseCostingDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectBudgetCostingId == pbcForSiteFinExp.ProjectBudgetCostingId).ToListAsync();


                    }

                }

                finExp.CustomerCode = request.CustomerCode;
                finExp.ProjectCode = request.ProjectCode;
                finExp.ServiceType = "FINANCIALEXPENSE";
                finExp.SiteCode = site.SiteCode;
                pbcforsite.PfcListForSite = finExp.FinancialExpenseCostingsList;
                resEst.SiteWisePBCListForProject.Add(pbcforsite);
            }
            return resEst;

        }
    }

    #endregion
    #region ConvertProjectSiteToProposal
    public class ConvertProjectSiteToProposal : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblOpProjectSites_PaginationDto Project { get; set; }
    }

    public class ConvertProjectSiteToProposalHandler : IRequestHandler<ConvertProjectSiteToProposal, int>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public ConvertProjectSiteToProposalHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(ConvertProjectSiteToProposal request, CancellationToken cancellationToken)
        {



            try
            {

                Log.Info("----Info ConvertProjectSiteToProposal method start----");

                var projectSite = await _context.TblOpProjectSites.AsNoTracking().FirstOrDefaultAsync(e => e.ProjectCode == request.Project.ProjectCode &&e.SiteCode==request.Project.SiteCode);
                if (projectSite is null)
                    return 0;
                projectSite.IsConvertedToProposal = true;
                _context.Update(projectSite);
                await _context.SaveChangesAsync();

                Log.Info("----Info ConvertProjectSiteToProposal method Exit----");
                return 1;
            }
            catch (Exception ex)
            {
                Log.Error("Error in ConvertProjectSiteToProposal Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
    }

    #endregion



    #region SkippEstimation
    public class SkippEstimation : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public TblOpProjectSites_PaginationDto Project { get; set; }
    }

    public class SkippEstimationHandler : IRequestHandler<SkippEstimation, int>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public SkippEstimationHandler(CINDBOneContext context,DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<int> Handle(SkippEstimation request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                using (var transaction2 = await _contextDMC.Database.BeginTransactionAsync())
                {
                    try
                    {

                        Log.Info("----Info SkippEstimation method start----");
                        #region old_Code  Directly Converted To Contract
                        //var project = await _context.OP_HRM_TEMP_Projects.AsNoTracking().FirstOrDefaultAsync(e => e.ProjectCode == request.Project.ProjectCode);
                        //if (project is null)
                        //    return 0;
                        //project.IsConvrtedToContract = true;
                        //_context.OP_HRM_TEMP_Projects.Update(project);
                        // _context.SaveChanges();


                        //var projectSites =  _context.TblOpProjectSites.AsNoTracking().Where(e => e.ProjectCode == request.Project.ProjectCode).ToList();
                        //if (projectSites.Count == 0)
                        //{
                        //    transaction.Rollback();
                        //    return 0;
                        //}


                        //foreach (var projectSite in projectSites)
                        //{
                        //    projectSite.IsConvrtedToContract = true;
                        //    _context.TblOpProjectSites.UpdateRange(projectSites);
                        //    _context.SaveChanges();

                        //    var isProjectExistInDMC = _contextDMC.HRM_DEF_Projects.AsNoTracking().Any(p => p.CustomerCode == project.CustomerCode || p.ProjectCode == project.CustomerCode);
                        //    if (!isProjectExistInDMC)
                        //    {
                        //        HRM_DEF_Project projectDMC = new();
                        //        //projectDMC.ProjectID = Project.Id;
                        //        projectDMC.CustomerCode = project.CustomerCode;                                     //customer Code and Project code both are same in HRM
                        //        projectDMC.ProjectCode = project.CustomerCode;
                        //        projectDMC.ProjectName_EN = project.ProjectNameEng;
                        //        projectDMC.ProjectName_AR = project.ProjectNameArb;
                        //        projectDMC.ProjectDescription = "From Operations/skip estimations";
                        //        projectDMC.IsActive = true;
                        //        projectDMC.IsSystem = 0;
                        //        projectDMC.CreatedBy = request.User.UserId;
                        //        projectDMC.CreatedDate = DateTime.UtcNow;
                        //        projectDMC.ModifiedBy = request.User.UserId;
                        //        projectDMC.ProjectSiteID = 0;
                        //        projectDMC.ProjectDescription = "From Operations";
                        //        projectDMC.SiteCode = "N/A";
                        //        _contextDMC.HRM_DEF_Projects.Add(projectDMC);
                        //        _contextDMC.SaveChanges();
                        //    }
                        //        var isSiteExistInDMC = _contextDMC.HRM_DEF_Sites.AsNoTracking().Any(s => s.ProjectCode == project.CustomerCode && s.SiteCode == projectSite.SiteCode);

                        //        if (!isSiteExistInDMC)
                        //        {
                        //            var HrmProject = _contextDMC.HRM_DEF_Projects.AsNoTracking().FirstOrDefault(p => p.ProjectCode == project.CustomerCode || p.CustomerCode == projectSite.CustomerCode);

                        //            //var OprSite = _context.OprSites.AsNoTracking().FirstOrDefault(s=>s.SiteCode==enquiry.SiteCode);

                        //            bool isBranchExist = _contextDMC.HRM_DEF_Branches.AsNoTracking().Any(b => b.BranchCode == projectSite.BranchCode);


                        //            if (!isBranchExist)
                        //            {
                        //                HRM_DEF_Branch newBranch = new()
                        //                {
                        //                    BranchCode = projectSite.BranchCode,
                        //                    BranchDescription = "From Operations",
                        //                    BranchName_AR = "N/A",
                        //                    BranchName_EN = "N/A",
                        //                    IsActive = true,
                        //                    IsSystem = 0,
                        //                    CreatedBy=request.User.UserId,
                        //                    CreatedDate=DateTime.UtcNow
                        //                };

                        //                _contextDMC.HRM_DEF_Branches.Add(newBranch);
                        //                _contextDMC.SaveChanges();


                        //            }

                        //            var HrmBranch = _contextDMC.HRM_DEF_Branches.AsNoTracking().FirstOrDefault(b => b.BranchCode == projectSite.BranchCode);

                        //            HRM_DEF_Site siteDMC = new();
                        //            siteDMC.IsSystem = false;
                        //            siteDMC.SiteCode = projectSite.SiteCode;
                        //            // siteDMC.SiteID = _context.OprSites.FirstOrDefault(s=>s.SiteCode==enquiry.SiteCode).Id;
                        //            siteDMC.ProjectCode = projectSite.CustomerCode;                     
                        //            siteDMC.ProjectID = HrmProject.ProjectID;
                        //            siteDMC.SiteName_EN = projectSite.ProjectNameEng;
                        //            siteDMC.SiteName_AR = projectSite.ProjectNameArb;
                        //            siteDMC.BranchID = HrmBranch.BranchID;
                        //            siteDMC.CityCode = HrmBranch.BranchCode;
                        //        siteDMC.CreatedDate = DateTime.UtcNow;
                        //        siteDMC.CreatedBy = request.User.UserId;
                        //            siteDMC.IsActive = true;
                        //            siteDMC.SiteDescription = "From Operations/skip Estimations";
                        //            _contextDMC.HRM_DEF_Sites.Add(siteDMC);
                        //            _contextDMC.SaveChanges();

                        //        }

                        //    }



                        //Log.Info("----Info SkippEstimation method Exit----");
                        //await transaction.CommitAsync();
                        //await transaction2.CommitAsync();

                        #endregion


                        #region new code  Set All costings Completed

                        var projectSite = await _context.TblOpProjectSites.AsNoTracking().SingleOrDefaultAsync(e => e.ProjectCode == request.Project.ProjectCode && e.SiteCode==request.Project.SiteCode);
                        if (projectSite is null)
                            return 0;


                        //projectSite.IsResourcesAssigned=true;
                        //projectSite.IsLogisticsAssigned=true;
                        //projectSite.IsMaterialAssigned=true;
                        //projectSite.IsExpenceOverheadsAssigned=true;
                        projectSite.IsEstimationCompleted=true;

                        _context.TblOpProjectSites.Update(projectSite);
                       await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        #endregion













                        return 1;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        await transaction2.RollbackAsync();

                        Log.Error("Error in SkippEstimation Method");
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
  #region SkippEstimationType
    public class SkippEstimationType : IRequest<int>
    {
        public UserIdentityDto User { get; set; }
        public SkippEstimationTypeDto Input { get; set; }
    }

    public class SkippEstimationTypeHandler : IRequestHandler<SkippEstimationType, int>
    {
        private readonly CINDBOneContext _context;
        private readonly DMCContext _contextDMC;
        private readonly IMapper _mapper;

        public SkippEstimationTypeHandler(CINDBOneContext context,DMCContext contextDMC, IMapper mapper)
        {
            _context = context;
            _contextDMC = contextDMC;
            _mapper = mapper;
        }

        public async Task<int> Handle(SkippEstimationType request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                using (var transaction2 = await _contextDMC.Database.BeginTransactionAsync())
                {
                    try
                    {

                        Log.Info("----Info SkippEstimationType method start----");
                        


                        #region new code  Set All costings Completed

                        var projectSite = await _context.TblOpProjectSites.AsNoTracking().SingleOrDefaultAsync(e => e.ProjectCode == request.Input.ProjectCode && e.SiteCode==request.Input.SiteCode);
                        if (projectSite is null)
                            return 0;


                        if (request.Input.Type == "resource")
                        {
                            projectSite.IsResourcesAssigned=true;

                        }
                       else  if (request.Input.Type == "logistics")
                        {
                            projectSite.IsLogisticsAssigned=true;


                        }
                         else  if (request.Input.Type == "material")
                        {
                            projectSite.IsMaterialAssigned=true;
                                                    }
                          else  if (request.Input.Type == "financeExpence")
                        {
                            projectSite.IsExpenceOverheadsAssigned=true;
                        }
                        if (projectSite.IsResourcesAssigned
                            && projectSite.IsLogisticsAssigned
                            && projectSite.IsMaterialAssigned
                            && projectSite.IsExpenceOverheadsAssigned)
                        {
                            projectSite.IsEstimationCompleted = true;
                        }
                        _context.TblOpProjectSites.Update(projectSite);
                       await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        #endregion













                        return 1;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        await transaction2.RollbackAsync();

                        Log.Error("Error in SkippEstimation Method");
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
