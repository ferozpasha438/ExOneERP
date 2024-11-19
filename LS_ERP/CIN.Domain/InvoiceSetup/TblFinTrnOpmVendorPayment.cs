using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.InvoiceSetup
{    
    [Table("tblFinTrnOpmVendorPayment")]
    public class TblFinTrnOpmVendorPayment : PrimaryKey<int>
    {

        [ForeignKey(nameof(BranchCode))]
        public TblErpSysCompanyBranch SysCompanyBranch { get; set; }
        public string BranchCode { get; set; }

        [ForeignKey(nameof(PaymentId))]
        public TblFinTrnOpmVendorPaymentHeader OpmPaymentHeader { get; set; }
        public int PaymentId { get; set; }

        [ForeignKey(nameof(InvoiceId))]
        public TblTranVenInvoice Invoice { get; set; }
        public long? InvoiceId { get; set; }

        [StringLength(150)]
        public string InvoiceNumber { get; set; }

        public DateTime? TranDate { get; set; }

        [ForeignKey(nameof(VendCode))]
        public TblSndDefVendorMaster SndVendorMaster { get; set; }
        [StringLength(20)]
        public string VendCode { get; set; }

        [Column(TypeName = "decimal(18, 3)")]
        public decimal? Amount { get; set; }

        [Column(TypeName = "decimal(18, 3)")]
        public decimal? CrAmount { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? DiscountAmount { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? NetAmount { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? BalanceAmount { get; set; }

        [Column(TypeName = "decimal(18, 3)")]
        public decimal? AppliedAmount { get; set; }

        [StringLength(50)]
        public string DocNum { get; set; }
        [StringLength(150)]
        public string InvoiceRefNumber { get; set; }
        [Column(TypeName = "date")]
        public DateTime? InvoiceDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? InvoiceDueDate { get; set; }       
        public bool IsPaid { get; set; }
        public bool Flag1 { get; set; }
        public bool Flag2 { get; set; }
        [StringLength(500)]
        public string Remarks { get; set; }
    }
}
