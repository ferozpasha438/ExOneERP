using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.OpeartionsMgt
{

    [Table("tblOpProjectBudgetEstimation")]
    public class TblOpProjectBudgetEstimation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectBudgetEstimationId { get; set; }
        [StringLength(20)]
        public string CustomerCode { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }
        public int PreviousEstimatonId { get; set; }
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        public int BranchId { get; set; }

    }





        [Table("tblOpProjectBudgetCosting")]
    public class TblOpProjectBudgetCosting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectBudgetCostingId { get; set; }
        public int ProjectBudgetEstimationId { get; set; }
        [StringLength(20)]
        public string CustomerCode { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string ServiceType { get; set; }         //SKILLSET,MATERIAL,LOGISTIC,FINANCIALEXPENSE

        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }

    }



#region Resource Costing

    [Table("tblOpProjectResourceCosting")]
    public class TblOpProjectResourceCosting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResourceCostingId { get; set; }
        public int ProjectBudgetCostingId { get; set; }
        [StringLength(20)]
        public string SkillsetCode { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }

        public int Quantity { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal Margin { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal CostPerUnit { get; set; }

        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }



    }
    [Table("tblOpProjectResourceSubCosting")]
    public class TblOpProjectResourceSubCosting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResourceSubCostingId { get; set; }
        public int ResourceCostingId { get; set; }
      
        [StringLength(20)]
        public string CostHead { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal Amount { get; set; }
        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }


    }

    #endregion

#region Logistics Costing

    [Table("tblOpProjectLogisticsCosting")]
    public class TblOpProjectLogisticsCosting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogisticsCostingId { get; set; }
        public int ProjectBudgetCostingId { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string VehicleNumber { get; set; }
        public int Qty { get; set; }

        [Column(TypeName = "decimal(17,3)")]
        public decimal CostPerUnit { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal Margin { get; set; }
        
        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }



    }
    [Table("tblOpProjectLogisticsSubCosting")]
    public class TblOpProjectLogisticsSubCosting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogisticsSubCostingId { get; set; }
        public int LogisticsCostingId { get; set; }
        
        [StringLength(20)]
        public string CostHead { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal Amount { get; set; }
        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }


    }

    #endregion

    #region MaterialEquipment Costing

    [Table("tblOpProjectMaterialEquipmentCosting")]
    public class TblOpProjectMaterialEquipmentCosting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaterialEquipmentCostingId { get; set; }
        public int ProjectBudgetCostingId { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string MaterialEquipmentCode { get; set; }

        [Column(TypeName = "decimal(17,3)")]
        public decimal CostPerUnit { get; set; }
        public int Quantity { get; set; }

        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }



    }
    [Table("tblOpProjectMaterialEquipmentSubCosting")]
    public class TblOpProjectMaterialEquipmentSubCosting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaterialEquipmentSubCostingId { get; set; }
        public int MaterialEquipmentCostingId { get; set; }

        [StringLength(20)]
        public string CostHead { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal Amount { get; set; }
        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }


    }

    #endregion
#region FinancialExpense Costing

    [Table("tblOpProjectFinancialExpenseCosting")]
    public class TblOpProjectFinancialExpenseCosting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FinancialExpenseCostingId { get; set; }
        public int ProjectBudgetCostingId { get; set; }
       
        [StringLength(20)]
        public string FinancialExpenseCode { get; set; }

        [Column(TypeName = "decimal(17,3)")]
        public decimal CostPerUnit { get; set; }
      

        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }



    }
   

    #endregion

}
