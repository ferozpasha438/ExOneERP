using AutoMapper;
using CIN.Application.GeneralLedgerDtos;
using CIN.Application.SystemSetupDtos;
using CIN.Domain.InvoiceSetup;
using CIN.Domain.OpeartionsMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CIN.Application.InvoiceDtos
{


    public class CustomerReceiptVoucherListDto : VendorReceiptVoucherDto
    {
        public List<TblTranInvoiceDto> UnPaidList { get; set; }
        public List<TblTranInvoiceDto> PaidList { get; set; }
    }

    public class VendorReceiptVoucherDto : CustomerReceiptVoucherDto
    {
        public string VendorName { get; set; }
    }

    public class CustomerReceiptVoucherDto
    {
        public int VoucherNumber { get; set; }
        public int? CustomerId { get; set; }
        public string CustCode { get; set; }
        public string CustomerName { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string DocNum { get; set; }
        public DateTime? Date { get; set; }
        public string Logo { get; set; }

        public string BranchName { get; set; }
        public string BankName { get; set; }
        public string CheckNumber { get; set; }
        public DateTime? Checkdate { get; set; }
        public string ReceivedFrom { get; set; }
        public DateTime? VoucherDate { get; set; }
        public string PayType { get; set; }
        public decimal? Amount { get; set; }

    }

    [AutoMap(typeof(TblFinTrnCustomerInvoice))]
    public class TblFinTrnCustomerInvoiceDto : PrimaryKeyDto<int>
    {
        public TblTranInvoice Invoice { get; set; }
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
        public string CustCode { get; set; }
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


    [AutoMap(typeof(TblFinTrnCustomerStatement))]
    public class TblFinTrnCustomerStatementDto : PrimaryKeyDto<int>
    {
        public long? InvoiceId { get; set; }
        public int CompanyId { get; set; }
        public string CustName { get; set; }
        public string BranchName { get; set; }
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
        public string CustCode { get; set; }
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

    public class TblFinTrnCustomerStatementItemDto : CommonCustomerInfoItemDto
    {
        public List<TblFinTrnCustomerStatementDto> List { get; set; }
    }

    public class OpmCustomerPaymentSingleItemDto
    {
        public TblFinTrnCustomerPaymentDto Header { get; set; }
        public List<TblTranInvoiceListDto> List { get; set; }
        public List<TblTranInvoiceListDto> InvoiceList { get; set; }
    }



    [AutoMap(typeof(TblFinTrnCustomerPayment))]
    public class TblFinTrnCustomerPaymentDto : PrimaryKeyDto<int>
    {
        public int CompanyId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }

        public int VoucherNumber { get; set; }
        public DateTime? TranDate { get; set; }


        [StringLength(20)]
        public string CustCode { get; set; }
        public string CustName { get; set; }

        [StringLength(10)]
        public string PayType { get; set; }

        public string PayCode { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public decimal? Amount { get; set; }
        [StringLength(50)]
        // [Required]
        public string DocNum { get; set; }
        [StringLength(20)]
        public string CheckNumber { get; set; }

        public DateTime? CheckDate { get; set; }

        public string Narration { get; set; }
        [StringLength(50)]
        public string Preparedby { get; set; }
        public bool IsPaid { get; set; }
        public bool IsPosted { get; set; }
        public string SiteCode { get; set; }
        public bool? HasPdcClearance { get; set; }
        public bool? IsPdcCleared { get; set; }
        public DateTime? PdcClearedDate { get; set; }
        public string PdcClearedBy { get; set; }
    }



    public class CustomerPayAmountDto
    {
        public decimal? NetAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? AppliedAmount { get; set; }
        public decimal? TobePaidAmount { get; set; }
        public bool IsPaid { get; set; }
    }

    public class TblTranInvoiceApprovalDto
    {
        public int Id { get; set; }
        public string DocNum { get; set; }
        public string AppRemarks { get; set; }
        public string Trantype { get; set; }
        public string TranSource { get; set; }
    }
    public class CustomerPaymentApprovalDto
    {
        public int Id { get; set; }
        public string CustomerCode { get; set; }
        public string Remarks { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
    public class TblTranInvoiceSettlementDto
    {
        public int Id { get; set; }
        public short Days { get; set; }
        public string DocNum { get; set; }
        public string PaymentType { get; set; }
        public string Trantype { get; set; }
        public string TranSource { get; set; }
        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
        public string PayCode { get; set; }
        public string CheckNumber { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class PrintInvoiceDto
    {
        public TblErpSysCompanyDto Company { get; set; }
        public TblOprCustomerMasterDto Customer { get; set; }
        public TblTranInvoiceDto Invoice { get; set; }
        public TblTranInvoiceDto_AR Invoice_AR { get; set; }
        public List<TblTranInvoiceItemDto> InvoiceItems { get; set; }
        //  public List<InvoiceItemsDTO_AR> InvoiceItems_AR { get; set; }
        public TblErpSysCompanyBranchDto BankDetails { get; set; }

        public string TotalAmountEn { get; set; }
        public string TotalAmountAr { get; set; }
    }
    public class ProductUnitPriceDTO
    {
        public int ProductId { get; set; }
        public string Description { get; set; }
        public string UnitPrice { get; set; }
        public string UnitTypeEN { get; set; }
        public string UnitTypeAR { get; set; }
        public string NameEN { get; set; }
        public string NameAR { get; set; }
    }

    //public class TblTranInvoiceDto : PrimaryKeyDto<long>
    //{
    //}

    public class TblTranInvoiceItemDto : PrimaryKeyDto<long>
    {
        [StringLength(50)]
        public string InvoiceNumber { get; set; }

        public long? InvoiceId { get; set; }

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

        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public string UnitType { get; set; }
        public string ProductName { get; set; }
        public decimal? TaxTariffPercentage { get; set; }
        public decimal? Discount { get; set; }
        public string SiteCode { get; set; }
        public string SiteName { get; set; }

    }

    public class TblTranInvoicePrintDto
    {
        public string QRCode { get; set; }
    }

    public class TblTranInvoiceDto_AR : TblTranInvoiceDto
    {
        public string Quantity_AR { get; set; }
        public string UnitPrice_AR { get; set; }
        public string DiscountAmount_AR { get; set; }
        public string TaxTariffPercentage_AR { get; set; }
        public string AmountBeforeTax_AR { get; set; }
        //public string SubTotal_AR { get; set; }
        //public string TotalAmount_AR { get; set; }
        //public string TaxAmount_AR { get; set; }
    }

    public class TblTranInvoiceListDto : PrimaryKeyDto<long>
    {
        [StringLength(50)]
        public string SpInvoiceNumber { get; set; }
        [StringLength(150)]
        public string InvoiceNumber { get; set; }
        [StringLength(150)]
        public string TaxIdNumber { get; set; }
        [StringLength(150)]
        public string InvoiceRefNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? InvoiceDueDate { get; set; }
        public int? CurrencyId { get; set; }
        public int? CompanyId { get; set; }
        [StringLength(150)]
        public string LpoContract { get; set; }
        public string PaymentTermId { get; set; }
        public long? CustomerId { get; set; }
        public string BranchCode { get; set; }
        public int? InvoiceStatusId { get; set; } //InvoiceStatusIdType Enum
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string CustomerName { get; set; }
        public string SiteName { get; set; }
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
        public decimal? AppliedAmount { get; set; }
        public bool? ZatcaStatus { get; set; }
        public List<string> ProductList { get; set; }
    }

    public class TblTranInvoiceDto : PrimaryKeyDto<long>
    {
        [StringLength(50)]
        public string SpInvoiceNumber { get; set; }
        [StringLength(150)]
        public string InvoiceNumber { get; set; }
        [StringLength(150)]
        public string TaxIdNumber { get; set; }
        [StringLength(150)]
        public string InvoiceRefNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? InvoiceDueDate { get; set; }
        public int? CurrencyId { get; set; }
        public int? CompanyId { get; set; }
        [StringLength(150)]
        public string LpoContract { get; set; }
        public string PaymentTermId { get; set; }
        public long? CustomerId { get; set; }
        public string BranchCode { get; set; }
        public int? InvoiceStatusId { get; set; } //InvoiceStatusIdType Enum
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNameAr { get; set; }
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

        public List<TblTranInvoiceItemDto> ItemList { get; set; } = new();


        public string TotalAmount_AR { get; set; }
        public string TaxAmount_AR { get; set; }
        public string SubTotal_AR { get; set; }
        public CommonDataLedgerDto Company { get; set; }
        public string AccountNo { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string Iban { get; set; }
        public string Category { get; set; }

        public string TotalAmountEn { get; set; }
        public string TotalAmountAr { get; set; }

        public string CustName { get; set; }
        public string CustArbName { get; set; }
        [StringLength(50)]
        public string SiteCode { get; set; }
        public CustomSelectListItem SiteName { get; set; }

        public ErpSysCompanyDto CompanyData { get; set; } = new();
        public ErpSysCompanyBranchDto Bank { get; set; } = new();
    }

    [AutoMap(typeof(TblSndDefCustomerMaster))]
    public class TblOprCustomerMasterDto : AutoActiveGenerateIdAuditableKeyDto<int>
    {

        [StringLength(20)]
        public string CustCode { get; set; }
        [StringLength(200)]

        public string CustName { get; set; }
        [StringLength(200)]
        public string CustArbName { get; set; }
        [StringLength(50)]
        public string CustAlias { get; set; }
        public short CustType { get; set; }

        [StringLength(50)]
        public string VATNumber { get; set; }

        public string CustCatCode { get; set; }
        //[ForeignKey(nameof(CustCatId))]
        // public int CustCatId { get; set; }                      //reference Category Table-tblSndDefCustomerCategory
        public short CustRating { get; set; }



        public string SalesTermsCode { get; set; }

        //public int CustTermsid { get; set; }                    //reference terms table-tblSndDefSalesTermsCode


        public decimal CustDiscount { get; set; }

        public decimal CustCrLimit { get; set; }
        [StringLength(20)]
        public string CustSalesRep { get; set; }                 //reference user code-as of now no data
        [StringLength(100)]
        public string CustSalesArea { get; set; }


        public string CustARAc { get; set; }                        //Reference Account Code-tblFinDefMainAccounts

        public DateTime CustLastPaidDate { get; set; }

        public DateTime CustLastSalesDate { get; set; }

        public decimal CustLastPayAmt { get; set; }
        [StringLength(500)]
        public string CustAddress1 { get; set; }
        [StringLength(20)]

        public string CustCityCode1 { get; set; }
        //public int CustStateId1 { get; set; }
        //public int CustCountryId1 { get; set; }
        [StringLength(50)]
        public string CustMobile1 { get; set; }
        [StringLength(50)]
        public string CustPhone1 { get; set; }
        [StringLength(500)]
        public string CustEmail1 { get; set; }
        [StringLength(200)]
        public string CustContact1 { get; set; }
        [StringLength(500)]
        public string CustAddress2 { get; set; }


        public string CustCityCode2 { get; set; }


        //public int CustStateId2 { get; set; }
        //public int CustCountryId2 { get; set; }
        [StringLength(50)]
        public string CustMobile2 { get; set; }
        [StringLength(50)]
        public string CustPhone2 { get; set; }
        [StringLength(500)]
        public string CustEmail2 { get; set; }
        [StringLength(200)]
        public string CustContact2 { get; set; }
        [StringLength(200)]
        public string CustUDF1 { get; set; }
        [StringLength(200)]
        public string CustUDF2 { get; set; }
        [StringLength(200)]
        public string CustUDF3 { get; set; }
        // public bool CustIsActive { get; set; }
        public bool CustAllowCrsale { get; set; }
        public bool CustAlloCrOverride { get; set; }
        public bool CustOnHold { get; set; }
        public bool CustAlloChkPay { get; set; }
        public bool CustSetPriceLevel { get; set; }
        public short CustPriceLevel { get; set; }
        public bool CustIsVendor { get; set; }
        public bool CustArAcBranch { get; set; }

        public string CustArAcCode { get; set; }                            //Reference Account Code-tblFinDefMainAccounts

        public string CustDefExpAcCode { get; set; }                        //Reference Account Code-tblFinDefMainAccounts



        public string CustARAdjAcCode { get; set; }                         //Reference Account Code-tblFinDefMainAccounts

        public string CustARDiscAcCode { get; set; }
        public string CrNumber { get; set; }

        [StringLength(200)]
        public string CustNameAliasEn { get; set; }
        [StringLength(200)]
        public string CustNameAliasAr { get; set; }

        //[Required]
        //[StringLength(100)]
        //public string NameEN { get; set; }
        //[Required]
        //[StringLength(100)]
        //public string NameAR { get; set; }
        //[StringLength(20)]
        //public string City { get; set; }

        //[StringLength(20)]
        //public string State { get; set; }

        ////public bool? isActive { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public long? CreatedBy { get; set; }
        //public DateTime? ModifiedDate { get; set; }
        //public long? ModifiedBy { get; set; }
    }


    public class BulkPostingListDto
    {
        public List<long> Ids { get; set; }
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string BranchCode { get; set; }
        public string SiteCode { get; set; }
        public string PayType { get; set; }
        public bool IsCreditSettled { get; set; }
        public string PayCode { get; set; }
        public bool HasTranDate { get; set; }
        public bool IsAllSelected { get; set; }
        public DateTime? TranDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class CsIdResultDto
    {
        public string requestID { get; set; }
        public string dispositionMessage { get; set; }
        public string binarySecurityToken { get; set; }
        public string secret { get; set; }
        public object errors { get; set; }
    }


    public class ZatcaInvoiceDto
    {
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string VATNumber { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string City { get; set; }
        public string PostalZone { get; set; }
        public DateTime? InvoiceDueDate { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? TotalAmount { get; set; }
        public string CrNumber { get; set; }
        public string IcvNumber { get; set; }
        public string BuildingNumber { get; set; }
        public string PlotIdentification { get; set; }
        public string Certificate { get; set; }
        public string PrivateKey { get; set; }
        public string Authorization { get; set; }
        public string Xml { get; set; }
        public List<ZatcaInvoiceItemListDto> Lines { get; set; }

    }
    public class ZatcaInvoiceItemListDto
    {       
        public decimal? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TaxTariffPercentage { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public string ProductName { get; set; }

    }




}
