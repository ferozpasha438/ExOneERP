using AutoMapper;
using CIN.Application.GeneralLedgerDtos;
using CIN.Application.SndDtos;
using CIN.Application.SystemSetupDtos;
using CIN.Domain.InvoiceSetup;
using CIN.Domain.OpeartionsMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CIN.Application.InvoiceDtos
{











    public class TblSndTranInvoiceItemDto : PrimaryKeyDto<long>
    {

        public string InvoiceNumber { get; set; }

        public long InvoiceId { get; set; }

        public long? CreditMemoId { get; set; }
        public long? DebitMemoId { get; set; }
        public short? InvoiceType { get; set; }
        public string ItemCode { get; set; }

        public long ItemId { get; set; }

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

       
        public string Description { get; set; }

        public string UnitType { get; set; }
        public string ItemName { get; set; }
        public string ItemCategoryName { get; set; }
        public decimal? TaxTariffPercentage { get; set; }
        public short? Discount { get; set; }

      
        public string SiteCode { get; set; } = string.Empty;
        public decimal? ItemAvgCost { get; set; }
        public decimal? NetCost { get; set; }
        public decimal GrossMargin { get; set; }
        public decimal GrossMarginPer { get; set; }

       
        public decimal? NetQuantity { get; set; }
    }




    public class TblSndTranInvoiceListDto : PrimaryKeyDto<long>
    {

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

        public string WarehouseCode { get; set; }
        public int? InvoiceStatusId { get; set; } //InvoiceStatusIdType Enum
        public string CompanyName { get; set; }
        public string WarehouseName { get; set; }
        public string CustomerName { get; set; }
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
        public bool IsPosted { get; set; }
        public bool IsVoid { get; set; }
        public bool IsPaid { get; set; }
        public bool CanSettle { get; set; }
        public bool HasAuthority { get; set; }



        public decimal? AppliedAmount { get; set; }
        public List<string> ItemList { get; set; }

        public string PaymentTermName { get; set; }
        public int DueDays { get; set; }

        public bool? IsQtyDeducted { get; set; }
        public string SiteCode { get; set; }

        public decimal? ItemAvgCost { get; set; }
    }

    public class TblSndTranInvoiceDto : PrimaryKeyDto<long>
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
        public int? CustomerId { get; set; }
        public string WarehouseCode { get; set; }
        public int? InvoiceStatusId { get; set; } //InvoiceStatusIdType Enum
        public string CompanyName { get; set; }
        public string WarehouseName { get; set; }
        public string CustomerName { get; set; }
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

        public List<TblSndTranInvoiceItemDto> ItemList { get; set; }


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

        public short FooterDiscount { get; set; }


        public bool IsApproved { get; set; }
        public bool IsSettled { get; set; }
        public bool IsPosted { get; set; }
        public bool IsVoid { get; set; }
        public bool IsPaid { get; set; }


        public string Source { get; set; }        //Q->Quotation, D->Deliverynote
        public string DeleveryRefNumber { get; set; }       //if Source is Q-> Quotation Id, D->DeliveryNote Id

        public bool? IsQtyDeducted { get; set; }
        public string SiteCode { get; set; }


        public decimal? TotalCost { get; set; }
        public TblSndDefCustomerMaster SysCustomer { get; set; }
        public string? CustomerCode { get; set; }
        public decimal? GrossMargin { get; set; }
        public decimal? GrossMarginPer { get; set; }
    }

    public class InputTblSndTranInvoiceDto : TblSndTranInvoiceDto
    {
        public EnumSaveType SaveType { get; set; }
        public int? DueDays { get; set; }
        public IOTblSndTranInvoicePaymentSettlementsDto SettlementData { get; set; }
        public long? DeliveryNoteId { get; set; }
        public long? QuotationId { get; set; }


    }


    public enum EnumSaveType
    {
        Save = 0, SaveAndApprove = 1, SaveAndSettle = 2, ConvertDelNoteToInvoice = 3, ConvertQuotToInvoice = 4
    }




    public class ItemUnitPriceDTO
    {
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string UnitPrice { get; set; }
        public string UnitTypeEN { get; set; }
        public string UnitTypeAR { get; set; }
        public string UnitType { get; set; }
        public string NameEN { get; set; }
        public string NameAR { get; set; }

        public decimal Vat { get; set; }
    }


    public class PrintSndInvoiceDto
    {
        public TblErpSysCompanyDto Company { get; set; }
        public TblOprCustomerMasterDto Customer { get; set; }
        public TblSndTranInvoiceDto Invoice { get; set; }
        public TblSndTranInvoiceDto_AR Invoice_AR { get; set; }
        public List<TblSndTranInvoiceItemDto> InvoiceItems { get; set; }
        //  public List<InvoiceItemsDTO_AR> InvoiceItems_AR { get; set; }
        public TblErpSysCompanyBranchDto BankDetails { get; set; }

        public string TotalAmountEn { get; set; }
        public string TotalAmountAr { get; set; }
    }

    public class TblSndTranInvoiceDto_AR : TblSndTranInvoiceDto
    {
        public string Quantity_AR { get; set; }
        public string UnitPrice_AR { get; set; }
        public string DiscountAmount_AR { get; set; }
        public string TaxTariffPercentage_AR { get; set; }
        public string AmountBeforeTax_AR { get; set; }

    }









    public class TblSndTranInvoicePagedListDto : TblSndTranInvoiceListDto
    {
        public string BranchCode { get; set; }
        public TblSndAuthoritiesDto Authority { get; set; }

    }







    [AutoMap(typeof(TblSndTranInvoicePaymentSettlements))]
    public class TblSndTranInvoicePaymentSettlementsDto : PrimaryKeyDto<long>
    {

        public long InvoiceId { get; set; }


        public string PaymentCode { get; set; }

        public decimal SettledAmount { get; set; }


        public DateTime? SettledDate { get; set; }
        public DateTime? DueDate { get; set; }
        public bool? IsPaid { get; set; }

        public string Remarks { get; set; }
        public long? SettledBy { get; set; }
    }


    public class IOTblSndTranInvoicePaymentSettlementsDto
    {
        public long InvoiceId { get; set; }
        public string PaymentType { get; set; }     //Cash, Credit
        public List<TblSndTranInvoicePaymentSettlementsDto> PaymentsList { get; set; }

    }


    public class InputSndTranInvoicePostDto
    {
        public long? Id { get; set; }
        public short? Days { get; set; }
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
}
