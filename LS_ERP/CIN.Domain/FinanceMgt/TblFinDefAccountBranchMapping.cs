using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.FinanceMgt
{
    [Table("tblFinDefAccountBranchMapping")]
    public class TblFinDefAccountBranchMapping : PrimaryKey<int>
    {
        [ForeignKey(nameof(FinBranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        [StringLength(20)]
        public string FinBranchCode { get; set; } //Reference  BranchCode
        [Required]
        [StringLength(50)]
        public string FinBranchName { get; set; }
        [StringLength(50)]
        public string InventoryAccount { get; set; }
        [StringLength(50)]
        public string CashPurchase { get; set; }
        [StringLength(50)]
        public string CostofSalesAccount { get; set; }
        [StringLength(50)]
        public string InventoryAdjustment { get; set; }
        [StringLength(50)]
        public string DefaultSalesAccount { get; set; }
        [StringLength(50)]
        public string DefaultSalesReturn { get; set; }
        [StringLength(50)]
        public string InventoryTransfer { get; set; }
        [StringLength(50)]
        public string DefaultPayable { get; set; }
        [StringLength(50)]
        public string CostCorrection { get; set; }
        [StringLength(50)]
        public string WIPUsageConsumption { get; set; }
        [StringLength(50)]
        public string Reserved { get; set; }
    }
}
