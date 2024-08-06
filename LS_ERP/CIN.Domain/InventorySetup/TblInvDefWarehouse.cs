namespace CIN.Domain.InventorySetup
{
    using CIN.Domain.SystemSetup;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("tblInvDefWarehouse")]
    public class TblInvDefWarehouse : AutoActiveGenerateIdAuditableKey<int>
    {
        [StringLength(10)]
        [Key]
        public string WHCode { get; set; }
        [StringLength(50)]
        [Required]
        public string WHName { get; set; }
        [StringLength(50)]
        public string WHDesc { get; set; }
        [StringLength(500)]
        [Required]
        public string WHAddress { get; set; }
        [Required]
        [StringLength(20)]
        public string WHCity { get; set; }
        [StringLength(50)]
        public string WHState { get; set; }
        [Required]
        [StringLength(50)]
        public string WHIncharge { get; set; }
        [ForeignKey(nameof(WHBranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        [StringLength(20)]
        public string WHBranchCode { get; set; }
        [ForeignKey(nameof(InvDistGroup))]
        public TblInvDefDistributionGroup InvDistributionGroup { get; set; }
        [StringLength(20)]
        public string InvDistGroup { get; set; }
        public bool WhAllowDirectPur { get; set; }
       
    }
}
