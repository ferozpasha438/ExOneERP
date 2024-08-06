using CIN.Domain.PurchaseSetup;
using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.PurchaseMgt
{
    [Table("TblTranPurcInvoice")]
    public class TblTranPurcInvoice : PrimaryKey<long>
    {
        [StringLength(50)]
        public string SpCreditNumber { get; set; }
        [Required]
        [StringLength(150)]
        public string CreditNumber { get; set; }
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
        public TblPopDefVendorPOTermsCode SndPoTermsCode { get; set; }
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
    }
}
