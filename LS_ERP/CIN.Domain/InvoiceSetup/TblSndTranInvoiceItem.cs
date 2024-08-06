using CIN.Domain.InventorySetup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.InvoiceSetup
{
    [Table("tblSndTranInvoiceItem")]
    public class TblSndTranInvoiceItem : PrimaryKey<long>
    {        
        [StringLength(50)]
        public string InvoiceNumber { get; set; }

        [ForeignKey(nameof(InvoiceId))]
        public TblSndTranInvoice Invoice { get; set; }
        public long? InvoiceId { get; set; }

        public long? CreditMemoId { get; set; }
        public long? DebitMemoId { get; set; }

        public short? InvoiceType { get; set; }
        [ForeignKey(nameof(ItemCode))]
        public TblErpInvItemMaster Item { get; set; }

        public string ItemCode { get; set; }

        public string UnitType { get; set; }

        [ForeignKey(nameof(UnitType))]
        public TblInvDefUOM SysUnitType { get; set; }

        [Column(TypeName = "decimal(18, 3)")]
        public decimal? Quantity { get; set; }
       
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? UnitPrice { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? SubTotal { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? DiscountAmount { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? AmountBeforeTax { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? TaxAmount { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? TotalAmount { get; set; }
        public bool? IsDefaultConfig { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "date")]
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? TaxTariffPercentage { get; set; }
        public short? Discount { get; set; }
        [StringLength(50)]
        public string SiteCode { get; set; } = string.Empty;
        public decimal? ItemAvgCost { get; set; }

        [Column(TypeName = "decimal(18, 3)")]
        public decimal? NetQuantity { get; set; }

    }
}