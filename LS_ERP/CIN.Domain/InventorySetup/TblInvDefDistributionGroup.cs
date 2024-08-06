

namespace CIN.Domain.InventorySetup
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    using System.ComponentModel.DataAnnotations;
    using CIN.Domain.FinanceMgt;

    [Table("tblInvDefDistributionGroup")]
    public class TblInvDefDistributionGroup : AutoGenerateIdAuditableKey<int>
    {
        [StringLength(20)]
        [Key]
        public string InvDistGroup { get; set; }
        [ForeignKey(nameof(InvAssetAc))]
        public TblFinDefMainAccounts FinMainAccounts { get; set; }
        [StringLength(50)]
        public string InvAssetAc { get; set; }
        
        [ForeignKey(nameof(InvNonAssetAc))]
        public TblFinDefMainAccounts FinInvNonAssetAcMainAccounts { get; set; }
        [StringLength(50)]
        public string InvNonAssetAc { get; set; }
       
        [ForeignKey(nameof(InvCashPOAC))]
        public TblFinDefMainAccounts FinInvCashPOACMainAccounts { get; set; }
        [StringLength(50)]
        public string InvCashPOAC { get; set; }
        
        [ForeignKey(nameof(InvCOGSAc))]
        public TblFinDefMainAccounts FinInvCOGSAcMainAccounts { get; set; }
        [StringLength(50)]
        public string InvCOGSAc { get; set; }
        
        [ForeignKey(nameof(InvAdjAc))]
        public TblFinDefMainAccounts FinInvAdjAcMainAccounts { get; set; }
        [StringLength(50)]
        public string InvAdjAc { get; set; }
        
        [ForeignKey(nameof(InvSalesAc))]
        public TblFinDefMainAccounts FinInvSalesAcMainAccounts { get; set; }
        [StringLength(50)]
        public string InvSalesAc { get; set; }       
        [StringLength(50)]
        [ForeignKey(nameof(InvInTransitAc))]
        public TblFinDefMainAccounts FinInvInTransitAcMainAccounts { get; set; }
        public string InvInTransitAc { get; set; }
        
        [ForeignKey(nameof(InvDefaultAPAc))]
        public TblFinDefMainAccounts FinInvDefaultAPAcMainAccounts { get; set; }
        [StringLength(50)]
        public string InvDefaultAPAc { get; set; }        
        [StringLength(50)]
        [ForeignKey(nameof(InvCostCorAc))]
        public TblFinDefMainAccounts FinInvCostCorAcMainAccounts { get; set; }
        public string InvCostCorAc { get; set; }
        [ForeignKey(nameof(InvWIPAc))]
        public TblFinDefMainAccounts FinInvWIPAcMainAccounts { get; set; }
        [StringLength(50)]
        public string InvWIPAc { get; set; }
        [ForeignKey(nameof(InvWriteOffAc))]
        public TblFinDefMainAccounts FinInvWriteOffAcMainAccounts { get; set; }
        [StringLength(50)]
        public string InvWriteOffAc { get; set; }

       
    }
}
