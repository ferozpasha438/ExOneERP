using CIN.Domain.SalesSetup;
using CIN.Domain.SystemSetup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.SchoolMgt
{
    [Table("tblSchoolTranInvoice")]
    public class TblSchoolTranInvoice : PrimaryKey<long>
    {
        [StringLength(50)]
        public string SpInvoiceNumber { get; set; }
        [Required]
        [StringLength(150)]
        public string InvoiceNumber { get; set; }
        [StringLength(150)]
        public string TaxIdNumber { get; set; }
        [StringLength(150)]
        public string InvoiceRefNumber { get; set; }
        //[StringLength(150)]
        //public string DocNum { get; set; }
        [Column(TypeName = "date")]
        public DateTime? InvoiceDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? InvoiceDueDate { get; set; }
        public int? CurrencyId { get; set; }
        public int? CompanyId { get; set; }
        [StringLength(150)]
        public string LpoContract { get; set; }

        [ForeignKey(nameof(PaymentTerms))]
        public TblSndDefSalesTermsCode SndSalesTermsCode { get; set; }
        public string PaymentTerms { get; set; }


        public long? CustomerId { get; set; }

        [ForeignKey(nameof(BranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        [StringLength(20)]
        public string BranchCode { get; set; }
        public int? InvoiceStatusId { get; set; }
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
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? TotalPayment { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? AmountDue { get; set; }
        public bool? IsDefaultConfig { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? VatPercentage { get; set; }
        public bool IsCreditConverted { get; set; }
        [StringLength(25)]
        public string InvoiceStatus { get; set; }
        [StringLength(25)]
        public string InvoiceModule { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }
        [StringLength(500)]
        public string InvoiceNotes { get; set; }

        [StringLength(50)]
        public string ServiceDate1 { get; set; }

        [StringLength(200)]
        public string CustName { get; set; }
        [StringLength(200)]
        public string CustArbName { get; set; }

        [StringLength(50)]
        public string SiteCode { get; set; }

        public bool IsApproved { get; set; }
        public bool IsPosted { get; set; }
        public bool IsSettled { get; set; }
        public bool IsPaid { get; set; }
    }

    [Table("tblSchoolTranInvoiceItem")]
    public class TblSchoolTranInvoiceItem : PrimaryKey<long>
    {
        [StringLength(50)]
        public string InvoiceNumber { get; set; }

        [ForeignKey(nameof(InvoiceId))]
        public TblSchoolTranInvoice Invoice { get; set; }
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
        public short? Discount { get; set; }
        [StringLength(50)]
        public string SiteCode { get; set; } = string.Empty;

    }
}
