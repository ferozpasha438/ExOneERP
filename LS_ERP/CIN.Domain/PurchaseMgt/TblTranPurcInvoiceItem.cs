
using CIN.Domain.InventorySetup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.PurchaseMgt
{
    [Table("TblTranPurcInvoiceItem")]
    public class TblTranPurcInvoiceItem : PrimaryKey<long>
    {
        [StringLength(50)]
        public string CreditNumber { get; set; }

        [ForeignKey(nameof(CreditId))]
        public TblTranPurcInvoice Credit { get; set; }
        public long? CreditId { get; set; }

        public long? CreditMemoId { get; set; }
        public long? DebitMemoId { get; set; }

        //[ForeignKey(nameof(ProductId))]
        //public TblTranDefProduct Product { get; set; }
        //public int? ProductId { get; set; }

        [ForeignKey(nameof(ItemCode))]
        public TblErpInvItemMaster InvItemMaster { get; set; }
        [StringLength(20)]
        [Required]
        public string ItemCode { get; set; }


        [Column(TypeName = "decimal(18, 3)")]
        public int? Quantity { get; set; }
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

    }
}
