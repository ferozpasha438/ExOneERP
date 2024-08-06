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





    #region CreateUpdateProposalCosting
    public class CreateProposalCosting : IRequest<long>
    {
        public UserIdentityDto User { get; set; }
        public List<TblOpProposalCosting> Dtos { get; set; }
    }

    public class CreateProposalCostingHandler : IRequestHandler<CreateProposalCosting, long>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;

        public CreateProposalCostingHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<long> Handle(CreateProposalCosting request, CancellationToken cancellationToken)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
            {
                Log.Info("----Info CreateUpdateProposalCosting method start----");

                    var Estimation = await _context.TblOpProjectBudgetEstimations.SingleOrDefaultAsync(e => e.ProjectBudgetEstimationId == request.Dtos[0].ProjectBudgetEstimationId);
                    if (Estimation is null)
                    {
                        await transaction.RollbackAsync();
                        return -2;
                    }
                    var project = await _context.OP_HRM_TEMP_Projects.AsNoTracking().SingleOrDefaultAsync(e => e.ProjectCode == Estimation.ProjectCode);
                    if (project is null)
                    {
                        await transaction.RollbackAsync();
                        return -3;
                    }
                    else
                    {

                        project.IsConvertedToProposal = true;
                        _context.OP_HRM_TEMP_Projects.Update(project);
                        await _context.SaveChangesAsync();
                    }

                    List<TblOpProjectSites> projectSites = new();
                    if (!request.Dtos[0].IsForAdendum.Value)
                    {
                        projectSites = await _context.TblOpProjectSites.AsNoTracking().Where(e => e.ProjectCode == Estimation.ProjectCode).ToListAsync();
                    }
                    else
                    {
                        projectSites = await _context.TblOpProjectSites.AsNoTracking().Where(e => e.ProjectCode == Estimation.ProjectCode && e.SiteCode==request.Dtos[0].SiteCode).ToListAsync();

                    }

                    if (projectSites.Count==0)
                    {
                        await transaction.RollbackAsync();

                        return -4;

                    }
                    else
                    {
                        foreach (var ps in projectSites) {
                            ps.IsConvertedToProposal = true;
                        
                        }

                        _context.TblOpProjectSites.UpdateRange(projectSites);
                        await _context.SaveChangesAsync();

                    }
                    List<TblOpProposalCosting> existPropCostings = new();
                    if (!request.Dtos[0].IsForAdendum.Value) {
                        existPropCostings = _context.TblOpProposalCostings.AsNoTracking().Where(e => e.ProjectBudgetEstimationId == request.Dtos[0].ProjectBudgetEstimationId).ToList();
                    }
                    else
                    { 
                        existPropCostings = _context.TblOpProposalCostings.AsNoTracking().Where(e => e.ProjectBudgetEstimationId == request.Dtos[0].ProjectBudgetEstimationId
                        && e.SiteCode==request.Dtos[0].SiteCode
                        ).ToList();
                    }
                    if (existPropCostings.Count == 0) {

                    await _context.TblOpProposalCostings.AddRangeAsync(request.Dtos);
                    await _context.SaveChangesAsync();
                      await  transaction.CommitAsync();
                    return 1;
                }



                List<TblOpProposalCosting> removedCostings = new();
                List<TblOpProposalCosting> newCostings = new();
                List<TblOpProposalCosting> updatedCostings = new();

                existPropCostings.ForEach(e => {
                    if (request.Dtos.Any(x => x.Id == e.Id))
                    {
                        TblOpProposalCosting udto = request.Dtos.First(c => c.Id == e.Id);
                        udto.ModifiedOn = DateTime.UtcNow;
                        udto.ModifiedBy= request.User.UserId;
                        updatedCostings.Add(udto);

                    }
                    else {
                        e.CreatedOn = DateTime.UtcNow;
                        e.CreatedBy = request.User.UserId;
                        removedCostings.Add(e);
                    }

                });

                newCostings = request.Dtos.Where(e => e.Id == 0).ToList();


                if (removedCostings.Count > 0) {
                    _context.TblOpProposalCostings.RemoveRange(removedCostings);
                    _context.SaveChanges();
                }

                if (updatedCostings.Count > 0) {


                    _context.TblOpProposalCostings.UpdateRange(updatedCostings);
                    _context.SaveChanges();
                }

                if (newCostings.Count > 0) {
                    _context.TblOpProposalCostings.AddRange(newCostings);
                    _context.SaveChanges();
                }





             



                    await transaction.CommitAsync();

                    return 1;
                }
            catch (Exception ex)
            {
                    await transaction.RollbackAsync();
                Log.Error("Error in CreateUpdateProposalCosting Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                return 0;
            }
        }
        }
    }

    #endregion













    #region GetProposalCosting
    public class GetProposalCosting : IRequest<List<TblOpProposalCostingDto>>
    {
        public UserIdentityDto User { get; set; }

        public InputCustomerProjectSite Input { get; set; }
    }

    public class GetProposalCostingHandler : IRequestHandler<GetProposalCosting, List<TblOpProposalCostingDto>>
    {
        private readonly CINDBOneContext _context;
        private readonly IMapper _mapper;
        public GetProposalCostingHandler(CINDBOneContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TblOpProposalCostingDto>> Handle(GetProposalCosting request, CancellationToken cancellationToken)
        {

            List<TblOpProposalCostingDto> resList = new();

            try
            {
                var est = _context.TblOpProjectBudgetEstimations.AsNoTracking().FirstOrDefault(e => e.ProjectCode == request.Input.ProjectCode
                    && e.CustomerCode == request.Input.CustomerCode);

                if (est == null)
                {
                    return resList;
                }


                List<TblOpProposalCostingDto> propCostingList = new();

                if (!request.Input.IsAdendum)
                {
                    propCostingList = await _context.TblOpProposalCostings.AsNoTracking().ProjectTo<TblOpProposalCostingDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectBudgetEstimationId == est.ProjectBudgetEstimationId).ToListAsync();

                }
                else
                {

                    propCostingList = await _context.TblOpProposalCostings.AsNoTracking().ProjectTo<TblOpProposalCostingDto>(_mapper.ConfigurationProvider).Where(e => e.ProjectBudgetEstimationId == est.ProjectBudgetEstimationId && e.IsForAdendum.Value).ToListAsync();

                }


                if (propCostingList.Count > 0)
                {
                    return propCostingList;

                }


                var ests = _context.TblOpProjectBudgetEstimations.AsNoTracking();
                var budgetCostings = _context.TblOpProjectBudgetCostings.AsNoTracking();
                var resCostings =  _context.TblOpProjectResourceCosting.AsNoTracking();
                var matCostings =  _context.TblOpProjectMaterialEquipmentCostings.AsNoTracking();
                var expHeadCostings =  _context.TblOpProjectFinancialExpenseCostings.AsNoTracking();
                var logisticsCostings =  _context.TblOpProjectLogisticsCostings.AsNoTracking();
                var skillSetCodes =  _context.TblOpSkillsets.AsNoTracking();
                var logistics=_context.TblOpLogisticsandvehicles.AsNoTracking();
               

                List<TblOpProjectSites> sites = new();
                if ((request.Input.SiteCode == ""||request.Input.SiteCode == "-NA-" || request.Input.SiteCode == null) && !request.Input.IsAdendum)
                {
                    sites = await _context.TblOpProjectSites.AsNoTracking().Where(e => e.CustomerCode == request.Input.CustomerCode && e.ProjectCode == request.Input.ProjectCode && !e.IsAdendum).ToListAsync();

                }

                else

                {
                    sites = await _context.TblOpProjectSites.AsNoTracking().Where(e => e.CustomerCode == request.Input.CustomerCode && e.ProjectCode == request.Input.ProjectCode && e.SiteCode == request.Input.SiteCode).ToListAsync();
                }

                #region resources costing

                var resourcesJoinList = (from site in  sites join res3 in (from res1 in (from e in ests
                                                 join b in budgetCostings on e.ProjectBudgetEstimationId equals b.ProjectBudgetEstimationId
                                                 select new
                                                 {

                                                     e.CustomerCode,
                                                     e.ProjectCode,
                                                     e.ProjectBudgetEstimationId,
                                                     b.ServiceType,
                                                     b.ProjectBudgetCostingId,
                                                     b.SiteCode
                                                 }).Where(r1 => r1.ServiceType == "SKILLSET" && r1.ProjectCode==request.Input.ProjectCode
                                                 && r1.CustomerCode==request.Input.CustomerCode
                                                 ).ToList()
                                   join res2 in resCostings on res1.ProjectBudgetCostingId equals res2.ProjectBudgetCostingId
                                   select new
                                   {
                                       res2.Quantity,
                                       res2.CostPerUnit,
                                       res2.SkillsetCode,
                                       res1.ProjectBudgetEstimationId,
                                       res2.SiteCode,
                                   }).ToList() on site.SiteCode equals res3.SiteCode  select new{ 
                                       site.SiteCode,
                                       res3.Quantity,
                                       res3.SkillsetCode,
                                       res3.CostPerUnit,
                                       res3.ProjectBudgetEstimationId,
                                       ServiceType= "SKILLSET"
                                   }).ToList();

                foreach (var res in resourcesJoinList)
                {
                    TblOpProposalCostingDto resItem = new()
                    {
                        Id = 0,
                        ItemEng = skillSetCodes.FirstOrDefault(e => e.SkillSetCode == res.SkillsetCode).NameInEnglish,
                        ItemArab = skillSetCodes.FirstOrDefault(e => e.SkillSetCode == res.SkillsetCode).NameInArabic,
                        ProjectBudgetEstimationId = res.ProjectBudgetEstimationId,
                        Qty = res.Quantity,
                        Price = res.CostPerUnit,
                        SkillSetCode = res.SkillsetCode,
                        Total = res.Quantity * res.CostPerUnit,
                        IsForAdendum = request.Input.IsAdendum,
                        SiteCode = request.Input.IsAdendum ? request.Input.SiteCode : null
                    };


                    if (resList.Any(e => e.SkillSetCode == resItem.SkillSetCode))
                    {

                        var existRes = resList.FirstOrDefault(e => e.SkillSetCode == res.SkillsetCode);
                        existRes.Qty += resItem.Qty;
                        existRes.Total += resItem.Total;
                        existRes.Price = existRes.Total / existRes.Qty;
                    }
                    else
                    {
                        resList.Add(resItem);
                    }


                }

                #endregion



                #region remaining costings
                TblOpProposalCostingDto logisticsItem = new()
                {
                    Id = 0,
                    ItemEng = "Vehicle",
                    ItemArab = "Vehicle",
                    ProjectBudgetEstimationId = est.ProjectBudgetEstimationId,
                    Qty = 1,
                    Price =0,
                    SkillSetCode = "",
                    Total = 0,
                    IsForAdendum = request.Input.IsAdendum,
                    SiteCode = request.Input.IsAdendum ? request.Input.SiteCode : null
                };
                resList.Add(logisticsItem);
                TblOpProposalCostingDto materialItem = new()
                {
                    Id = 0,
                    ItemEng = "Material Costing",
                    ItemArab = "Material Costing",
                    ProjectBudgetEstimationId = est.ProjectBudgetEstimationId,
                    Qty = 1,
                    Price =0,
                    SkillSetCode = "",
                    Total = 0,
                    IsForAdendum = request.Input.IsAdendum,
                    SiteCode = request.Input.IsAdendum ? request.Input.SiteCode : null
                };
                resList.Add(materialItem);
                TblOpProposalCostingDto expOverHeadItem = new()
                {
                    Id = 0,
                    ItemEng = "Expence Overhead",
                    ItemArab = "Expence Overhead",
                    ProjectBudgetEstimationId = est.ProjectBudgetEstimationId,
                    Qty = 1,
                    Price =0,
                    SkillSetCode = "",
                    Total = 0,
                    IsForAdendum = request.Input.IsAdendum,
                    SiteCode = request.Input.IsAdendum ? request.Input.SiteCode : null
                };
                resList.Add(expOverHeadItem);


                #endregion








                return resList;
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostEmployeeAttendance Method");
                Log.Error("Error occured time : " + DateTime.UtcNow);
                Log.Error("Error message : " + ex.Message);
                Log.Error("Error StackTrace : " + ex.StackTrace);
                throw;
            }

        }
    }

    #endregion







}
