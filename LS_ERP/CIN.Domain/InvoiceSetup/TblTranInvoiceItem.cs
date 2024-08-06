using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.InvoiceSetup
{
    [Table("tblTranInvoiceItem")]
    public class TblTranInvoiceItem : PrimaryKey<long>
    {        
        [StringLength(50)]
        public string InvoiceNumber { get; set; }

        [ForeignKey(nameof(InvoiceId))]
        public TblTranInvoice Invoice { get; set; }
        public long? InvoiceId { get; set; }

        public long? CreditMemoId { get; set; }
        public long? DebitMemoId { get; set; }

        public short? InvoiceType { get; set; }
        //[ForeignKey(nameof(ProductId))]
        //public TblTranDefProduct Product { get; set; }

        public int? ProductId { get; set; }
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
        public decimal? Discount { get; set; }
        [StringLength(50)]
        public string SiteCode { get; set; } = string.Empty;

    }
}