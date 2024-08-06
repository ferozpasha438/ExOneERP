using AutoMapper;
using CIN.Domain.OpeartionsMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.OperationsMgtDtos
{


    [AutoMap(typeof(TblOpProjectBudgetEstimation))]
    public class TblOpProjectBudgetEstimationDto
    {
       
        public int ProjectBudgetEstimationId { get; set; }
        [StringLength(20)]
        public string CustomerCode { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }
        public int PreviousEstimatonId { get; set; }
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }

        public List<PBCForSiteDto> SiteWisePBCListForProject { get; set; }

        public decimal GrandTotalCostForProject { get; set; }
      
    }


    public class PBCForSiteDto
    {
        public TblSndDefSiteMasterDto siteData { get; set; }
        public decimal TotRcForSite { get; set; }
        public decimal TotMcForSite { get; set; }
        public decimal TotLcForSite { get; set; }
        public decimal TotFcForSite { get; set; }
        public List<TblOpProjectResourceCostingDto> PrcListForSite {get;set;}
        public List<TblOpProjectLogisticsCostingDto> PlcListForSite {get;set;}
        public List<TblOpProjectMaterialEquipmentCostingDto> PmcListForSite {get;set;}
        public List<TblOpProjectFinancialExpenseCostingDto>PfcListForSite {get;set;}
        public decimal GrandTotalCostForSite { get; set; }
    }








        [AutoMap(typeof(TblOpProjectBudgetCosting))]
    public class TblOpProjectBudgetCostingDto
    {
        public int ProjectBudgetCostingId { get; set; }
        public int ProjectBudgetEstimationId { get; set; }


        [StringLength(20)]
        public string CustomerCode { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string ServiceType { get; set; }         //SKILLSET,MATERIAL,LOGISTIC
      
       
        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }
        public int Status { get; set; }



       

        public List<TblOpProjectResourceCostingDto> ResourceCostingsList { get; set; }
        public List<TblOpProjectLogisticsCostingDto> LogisticsCostingsList { get; set; }
        public List<TblOpProjectMaterialEquipmentCostingDto> MaterialEquipmentCostingsList { get; set; }
        public List<TblOpProjectFinancialExpenseCostingDto> FinancialExpenseCostingsList { get; set; }

    }





    #region ResourceCosting

    [AutoMap(typeof(TblOpProjectResourceCosting))]

    public class TblOpProjectResourceCostingDto
    {
        public int ResourceCostingId { get; set; }
        public int ProjectBudgetCostingId { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string SkillsetCode { get; set; }
        public int Quantity { get; set; }
        public decimal Margin { get; set; }
        public decimal CostPerUnit { get; set; }
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        public decimal TotResourceCost { get; set; }
        public List<TblOpProjectResourceSubCostingDto> ResourceSubCostingList { get; set; }

    }
    [AutoMap(typeof(TblOpProjectResourceSubCosting))]
    public class TblOpProjectResourceSubCostingDto
    {
        public int ResourceSubCostingId { get; set; }
        public int ResourceCostingId { get; set; }
       
        [StringLength(20)]
        public string CostHead { get; set; }
        public decimal Amount { get; set; }
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }
    }

    #endregion

    #region Logistics Costing

    [AutoMap(typeof(TblOpProjectLogisticsCosting))]

    public class TblOpProjectLogisticsCostingDto
    {
        public int LogisticsCostingId { get; set; }
        public int ProjectBudgetCostingId { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string VehicleNumber { get; set; }
        public int Qty { get; set; }

        public decimal CostPerUnit { get; set; }
        public decimal Margin { get; set; }
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        public decimal TotLogisticsCost { get; set; }
        public List<TblOpProjectLogisticsSubCostingDto> LogisticsSubCostingList { get; set; }

    }
    [AutoMap(typeof(TblOpProjectLogisticsSubCosting))]
    public class TblOpProjectLogisticsSubCostingDto
    {
        public int LogisticsSubCostingId { get; set; }
        public int LogisticsCostingId { get; set; }
       
        [StringLength(20)]
        public string CostHead { get; set; }
        public decimal Amount { get; set; }
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }
    }

    #endregion


    #region MaterialEquipment Costing

    [AutoMap(typeof(TblOpProjectMaterialEquipmentCosting))]

    public class TblOpProjectMaterialEquipmentCostingDto
    {
        public int MaterialEquipmentCostingId { get; set; }
        public int ProjectBudgetCostingId { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string MaterialEquipmentCode { get; set; }

        public decimal CostPerUnit { get; set; }
        public int Quantity { get; set; }
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        public decimal TotMaterialEquipmentCost { get; set; }
        public List<TblOpProjectMaterialEquipmentSubCostingDto> MaterialEquipmentSubCostingList { get; set; }

    }
    [AutoMap(typeof(TblOpProjectMaterialEquipmentSubCosting))]
    public class TblOpProjectMaterialEquipmentSubCostingDto
    {
        public int MaterialEquipmentSubCostingId { get; set; }
        public int MaterialEquipmentCostingId { get; set; }

        [StringLength(20)]
        public string CostHead { get; set; }
        public decimal Amount { get; set; }
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }
    }

    #endregion

    #region MaterialEquipment Costing

    [AutoMap(typeof(TblOpProjectFinancialExpenseCosting))]

    public class TblOpProjectFinancialExpenseCostingDto
    {
        public int FinancialExpenseCostingId { get; set; }
        public int ProjectBudgetCostingId { get; set; }
        [StringLength(20)]
        public string FinancialExpenseCode { get; set; }
        public decimal CostPerUnit { get; set; }
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }

    }
   

    #endregion





}