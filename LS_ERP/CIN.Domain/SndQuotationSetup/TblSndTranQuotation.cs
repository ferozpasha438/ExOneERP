using CIN.Domain.InventorySetup;
using CIN.Domain.SalesSetup;
using CIN.Domain.SystemSetup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.SndQuotationSetup
    {
    [Table("tblSndTranQuotation")]
    public class TblSndTranQuotation : PrimaryKey<long>
    {
        [StringLength(50)]
        public string SpQuotationNumber { get; set; }
        [Required]
        [StringLength(150)]
        public string QuotationNumber { get; set; }
        [StringLength(150)]
        public string TaxIdNumber { get; set; }
        [StringLength(150)]
        public string QuotationRefNumber { get; set; }
        //[StringLength(150)]
        //public string DocNum { get; set; }
        [Column(TypeName = "date")]
        public DateTime? QuotationDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? QuotationDueDate { get; set; }
        public int? CurrencyId { get; set; }
        public int? CompanyId { get; set; }
        [StringLength(150)]
        public string LpoContract { get; set; }

        [ForeignKey(nameof(PaymentTermId))]
        public TblSndDefSalesTermsCode SndSalesTermsCode { get; set; }
        public string PaymentTermId { get; set; }


        public long? CustomerId { get; set; }

        [ForeignKey(nameof(WarehouseCode))]
        public TblInvDefWarehouse SysWarehouse { get; set; }
        [StringLength(10)]

        // public string BranchCode { get; set; }
        public string  WarehouseCode { get; set; }
     
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



        [StringLength(25)]
        public string QuotationStatus { get; set; }
        [StringLength(25)]
        public string QuotationModule { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }
        [StringLength(500)]
        public string QuotationNotes { get; set; }

        [StringLength(50)]
        public string ServiceDate1 { get; set; }

        [StringLength(200)]
        public string CustName { get; set; }
        [StringLength(200)]
        public string CustArbName { get; set; }

        public short FooterDiscount { get; set; }



        public bool IsApproved { get; set; }
    
        public bool IsVoid { get; set; }






        public bool IsConvertedSndQuotationToDeliveryNote { get; set; }
        public bool IsConvertedSndQuotationToOrder { get; set; }
        public bool IsConvertedSndQuotationToInvoice { get; set; }
        public bool IsRevised { get; set; }
        public byte RevisedNumber { get; set; }

        public long OriginalQuotationId { get; set; }
        public bool IsFinalRevision { get; set; }



    }
}