using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.PurchaseMgt
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using CIN.Domain.InventorySetup;
    using CIN.Domain.OpeartionsMgt;
    using CIN.Domain.PurchaseSetup;
    using CIN.Domain.SystemSetup;

    [Table("tblPopTrnPurchaseOrderDetails")]
    public class TblPopTrnPurchaseOrderDetails : AutoActiveGenerateIdAuditableKey<int>
    {

        [ForeignKey(nameof(TranId))]
        public TblPopTrnPurchaseOrderHeader Trans { get; set; }
        [Required]
        [StringLength(20)]
        public string TranId { get; set; } //Reference  BranchCode

        [StringLength(20)]
        [Key]
        public string TranNumber { get; set; }
        [Column(TypeName = "date")]
        public DateTime TranDate { get; set; }


        [ForeignKey(nameof(VendCode))]
        public TblSndDefVendorMaster Vendor { get; set; }
        //[Required]
        [StringLength(20)]
        public string VendCode { get; set; }


        [ForeignKey(nameof(CompCode))]
        public TblErpSysCompany SysCompany { get; set; }
        [Required]
        public int CompCode { get; set; }


        [ForeignKey(nameof(BranchCode))]
        public TblErpSysCompanyBranch BranchId { get; set; }
        [Required]
        [StringLength(20)]
        public string BranchCode { get; set; } //Reference  BranchCode


        [ForeignKey(nameof(TranVendorCode))]
        public TblSndDefVendorMaster VendorTrans { get; set; }
        //[Required]
        [StringLength(20)]
        public string TranVendorCode { get; set; }

        public sbyte ItemTracking { get; set; }


        [ForeignKey(nameof(TranItemCode))]
        public TblErpInvItemMaster InvItemMaster { get; set; }
        [StringLength(20)]
        [Required]
        public string TranItemCode { get; set; }
        [StringLength(100)]
        [Required]
        public string TranItemName { get; set; }
        [StringLength(100)]
        [Required]
        public string TranItemName2 { get; set; }
        [Column(TypeName = "decimal(12,5)")]
        public decimal TranItemQty { get; set; }


        [ForeignKey(nameof(TranItemUnitCode))]
        public TblInvDefUOM InvUoms { get; set; }
        [StringLength(10)]
        [Required]
        public string TranItemUnitCode { get; set; }


        [Column(TypeName = "decimal(12,5)")]
        [Required]
        public decimal TranUOMFactor { get; set; }
        [Column(TypeName = "decimal(12,3)")]
        public decimal TranItemCost { get; set; }
        [Column(TypeName = "decimal(12,3)")]
        public decimal TranTotCost { get; set; }
        [Column(TypeName = "decimal(6,3)")]
        public decimal DiscPer { get; set; }
        [Column(TypeName = "decimal(12,3)")]
        public decimal DiscAmt { get; set; }
        [Column(TypeName = "decimal(12,3)")]
        public decimal ItemTax { get; set; }
        [Column(TypeName = "decimal(6,3)")]
        public decimal ItemTaxPer { get; set; }
        [Column(TypeName = "decimal(12,3)")]
        public decimal TaxAmount { get; set; }
        [Column(TypeName = "decimal(12,5)")]
        public decimal POQtyReceived { get; set; }
        [Column(TypeName = "decimal(12,5)")]
        public decimal POQtyReceiving { get; set; }
        [Column(TypeName = "decimal(12,5)")]
        public decimal POQtyCancel { get; set; }
        [Column(TypeName = "decimal(12,3)")]
        public decimal POLineCost1 { get; set; }
        [Column(TypeName = "decimal(12,3)")]
        public decimal POLineCost2 { get; set; }
        [Column(TypeName = "decimal(12,3)")]
        public decimal POOHCostPerItem { get; set; }
        [Column(TypeName = "decimal(12,3)")]
        public decimal POLandedCost { get; set; }
        [Column(TypeName = "decimal(12,3)")]
        public decimal POLandedCostPerItem { get; set; }
        public Boolean TranVoidStatus { get; set; }
        public Boolean TranPostStatus { get; set; }
        public Boolean ForeClosed { get; set; }
        public Boolean Closed { get; set; }
        public Boolean IsGrn { get; set; }

    }
}
