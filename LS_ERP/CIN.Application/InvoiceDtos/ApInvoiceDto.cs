using AutoMapper;
using CIN.Application.FinPurchaseMgtDto;
using CIN.Application.SystemSetupDtos;
using CIN.Domain.InvoiceSetup;
using CIN.Domain.OpeartionsMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CIN.Application.InvoiceDtos
{
    [AutoMap(typeof(TblFinTrnVendorInvoice))]
    public class TblFinTrnVendorInvoiceDto : PrimaryKeyDto<int>
    {
        public long? InvoiceId { get; set; }
        public int CompanyId { get; set; }
        public string BranchCode { get; set; }
        [StringLength(50)]
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public short CreditDays { get; set; }
        public DateTime? DueDate { get; set; }
        [StringLength(2)]
        [Required]
        public string TranSource { get; set; }
        [StringLength(50)]
        [Required]
        public string DocNum { get; set; }
        [StringLength(10)]
        [Required]
        public string Trantype { get; set; }

        public int LoginId { get; set; }

        [StringLength(20)]
        public string VendCode { get; set; }
        //[Key]
        //public string CustCode { get; set; }
        [StringLength(50)]
        public string ReferenceNumber { get; set; }

        [Required]
        public decimal? InvoiceAmount { get; set; }

        [Required]
        public decimal? DiscountAmount { get; set; }

        [Required]
        public decimal? NetAmount { get; set; }

        [Required]
        public decimal? PaidAmount { get; set; }

        [Required]
        public decimal? BalanceAmount { get; set; }
        [Required]

        public decimal? AppliedAmount { get; set; }
        public string Remarks1 { get; set; }
        [StringLength(50)]
        public string Remarks2 { get; set; }
        public bool IsPaid { get; set; }

    }
    [AutoMap(typeof(TblFinTrnVendorStatement))]
    public class TblFinTrnVendorStatementDto : PrimaryKeyDto<int>
    {
        public long? InvoiceId { get; set; }


        public int CompanyId { get; set; }

        public string BranchCode { get; set; }
        public DateTime? TranDate { get; set; }
        [Required]
        [StringLength(2)]
        public string TranSource { get; set; }
        [StringLength(50)]
        [Required]
        public string TranNumber { get; set; }
        [Required]
        [StringLength(10)]
        public string Trantype { get; set; }

        [StringLength(20)]
        public string VendCode { get; set; }
        //[Key]
        //[StringLength(20)]
        //public string CustCode { get; set; }
        [StringLength(50)]
        [Required]
        public string DocNum { get; set; }
        [StringLength(50)]
        public string ReferenceNumber { get; set; }

        [StringLength(10)]
        public string PaymentType { get; set; }
        [StringLength(10)]
        public string PamentCode { get; set; }
        [StringLength(10)]
        public string CheckNumber { get; set; }
        [StringLength(150)]
        public string Remarks1 { get; set; }
        [StringLength(150)]
        public string Remarks2 { get; set; }

        public int LoginId { get; set; }

        public decimal? DrAmount { get; set; }

        public decimal? CrAmount { get; set; }

    }

    [AutoMap(typeof(TblFinTrnVendorPayment))]
    public class TblFinTrnVendorPaymentDto : PrimaryKeyDto<int>
    {


        public int CompanyId { get; set; }


        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public int VoucherNumber { get; set; }
        public DateTime? TranDate { get; set; }


        [StringLength(20)]
        public string VendCode { get; set; }
        public string CustName { get; set; }
        [StringLength(10)]
        public string PayType { get; set; }

        public string PayCode { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }

        public decimal? Amount { get; set; }
        [StringLength(50)]
      //  [Required]
        public string DocNum { get; set; }
        [StringLength(20)]
        public string CheckNumber { get; set; }

        public DateTime? Checkdate { get; set; }

        public string Narration { get; set; }
        [StringLength(50)]
        public string Preparedby { get; set; }

        public bool IsPaid { get; set; }
    }
    public class TblTranVenInvoiceListDto : PrimaryKeyDto<long>
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

        public DateTime? InvoiceDate { get; set; }

        public DateTime? InvoiceDueDate { get; set; }
        public int? CurrencyId { get; set; }
        public int? CompanyId { get; set; }
        [StringLength(150)]
        public string LpoContract { get; set; }


        public string PaymentTerms { get; set; }
        public string PaymentTermId { get; set; }

        public long? CustomerId { get; set; }


        [StringLength(20)]
        public string BranchCode { get; set; }
        public int? InvoiceStatusId { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string CustomerName { get; set; }
        public string Code { get; set; }
        public decimal? SubTotal { get; set; }

        public decimal? DiscountAmount { get; set; }

        public decimal? AmountBeforeTax { get; set; }

        public decimal? TaxAmount { get; set; }

        public decimal? TotalAmount { get; set; }

        public decimal? TotalPayment { get; set; }

        public decimal? AmountDue { get; set; }
        public bool? IsDefaultConfig { get; set; }

        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }

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

        public string QRCode { get; set; }
        public bool ApprovedUser { get; set; }
        public bool IsApproved { get; set; }
        public bool IsSettled { get; set; }
        public bool CanSettle { get; set; }
        public bool HasAuthority { get; set; }
    }

    public class TblTranVenInvoiceDto : PrimaryKeyDto<long>
    {
        [StringLength(50)]
        public string SpCreditNumber { get; set; }
       
        [StringLength(150)]
        public string CreditNumber { get; set; }
        [StringLength(150)]
        public string TaxIdNumber { get; set; }
        [StringLength(150)]
        public string InvoiceRefNumber { get; set; }
        //[StringLength(150)]
        //public string DocNum { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public DateTime? InvoiceDueDate { get; set; }
        public int? CurrencyId { get; set; }
        public int? CompanyId { get; set; }
        [StringLength(150)]
        public string LpoContract { get; set; }
        public string PaymentTermId { get; set; }

        public string PaymentTerms { get; set; }


        public long? CustomerId { get; set; }


        [StringLength(20)]
        public string BranchCode { get; set; }
        public int? InvoiceStatusId { get; set; }

        public decimal? SubTotal { get; set; }

        public decimal? DiscountAmount { get; set; }
        public decimal? PaidAmount { get; set; }

        public decimal? AmountBeforeTax { get; set; }

        public decimal? TaxAmount { get; set; }

        public decimal? TotalAmount { get; set; }

        public decimal? TotalPayment { get; set; }

        public decimal? AmountDue { get; set; }
        public bool? IsDefaultConfig { get; set; }

        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }

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

        public string QRCode { get; set; }
        public string LogoImagePath { get; set; }
        public List<TblTranVenInvoiceItemDto> ItemList { get; set; } = new();
    }

    public class TblTranVenInvoiceItemDto : PrimaryKeyDto<long>
    {
        [StringLength(50)]
        public string CreditNumber { get; set; }
        public long? CreditId { get; set; }
        public long? CreditMemoId { get; set; }
        public long? DebitMemoId { get; set; }
        public short? InvoiceType { get; set; }
        public int? ProductId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? AmountBeforeTax { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public bool? IsDefaultConfig { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        public string UnitType { get; set; }
        public string ProductName { get; set; }
        public decimal? TaxTariffPercentage { get; set; }
        public short? Discount { get; set; }

    }


    public class PrintApInvoiceDto
    {
        public TblErpSysCompanyDto Company { get; set; }
        public TblSndDefVendorMasterDto Customer { get; set; }
        public TblTranVenInvoiceDto Invoice { get; set; }
        public TblTranInvoiceDto_AR Invoice_AR { get; set; }
        public List<TblTranVenInvoiceItemDto> InvoiceItems { get; set; }
        //  public List<InvoiceItemsDTO_AR> InvoiceItems_AR { get; set; }
        public TblErpSysCompanyBranchDto BankDetails { get; set; }

        public string TotalAmountEn { get; set; }
        public string TotalAmountAr { get; set; }
    }
   
}
