using CIN.Application.GeneralLedgerDtos;
using CIN.Application.InventoryDtos;
using CIN.Application.InvoiceDtos;
using CIN.Application.SystemSetupDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.SndDtos
{
    public class SndDeliveryNoteDto
    {






        public class TblSndDeliveryNoteLineDto : PrimaryKeyDto<long>
        {

            public string QuotationNumber { get; set; }


            public long? QuotationId { get; set; }

            public long? CreditMemoId { get; set; }
            public long? DebitMemoId { get; set; }

            public short? QuotationType { get; set; }

            public string ItemCode { get; set; }
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

            public decimal? TaxTariffPercentage { get; set; }
            public short? Discount { get; set; }


            public string UnitType { get; set; }
            public long ItemId { get; set; }
            public string ItemName { get; set; }

            public long DeliveryNoteId { get; set; }
           

            public decimal Delivery { get; set; }
            public decimal Delivered { get; set; }
            public decimal BackOrder { get; set; }

            public string Remarks { get; set; }
            public bool DelvFlg1 { get; set; }
            public bool DelvFlg2 { get; set; }


        }




        public class TblSndDeliveryNoteListDto : PrimaryKeyDto<long>
        {

            public string SpQuotationNumber { get; set; }
            [StringLength(150)]
            public string QuotationNumber { get; set; }
            [StringLength(150)]
            public string TaxIdNumber { get; set; }
            [StringLength(150)]
            public string QuotationRefNumber { get; set; }
            public DateTime? QuotationDate { get; set; }
            public DateTime? QuotationDueDate { get; set; }
            public int? CurrencyId { get; set; }
            public int? CompanyId { get; set; }
            [StringLength(150)]
            public string LpoContract { get; set; }
            public string PaymentTermId { get; set; }
            public long? CustomerId { get; set; }

            public string WarehouseCode { get; set; }

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

            [StringLength(25)]
            public string QuotationModule { get; set; }

            [StringLength(500)]
            public string Remarks { get; set; }
            [StringLength(500)]
            public string QuotationNotes { get; set; }

            [StringLength(50)]
            public string ServiceDate1 { get; set; }

            public string QRCode { get; set; }

            public bool IsVoid { get; set; }

            public byte RevisedNumber { get; set; }
            public long OriginalQuotationId { get; set; }

            public bool HasAuthority { get; set; }



            public decimal? AppliedAmount { get; set; }
            public List<string> ItemList { get; set; }

            public string PaymentTermName { get; set; }
            public int DueDays { get; set; }


            public long QuotationHeadId { get; set; }
            public int ConvertedBy { get; set; }
            public DateTime? ConvertedDate { get; set; }
            public bool IsClosed { get; set; }
            public string DeliveryNumber { get; set; }

            public bool IsConvertedFromQuotation { get; set; }
            public bool IsCovertedFromOrder { get; set; }
            public bool IsConvertedDeliveryNoteToInvoice { get; set; }

        }

        public class TblSndDeliveryNoteDto : PrimaryKeyDto<long>
        {
            [StringLength(50)]
            public string SpQuotationNumber { get; set; }
            [StringLength(150)]
            public string QuotationNumber { get; set; }
            [StringLength(150)]
            public string TaxIdNumber { get; set; }
            [StringLength(150)]
            public string QuotationRefNumber { get; set; }
            public DateTime? QuotationDate { get; set; }
            public DateTime? QuotationDueDate { get; set; }
            public int? CurrencyId { get; set; }
            public int? CompanyId { get; set; }
            [StringLength(150)]
            public string LpoContract { get; set; }
            public string PaymentTermId { get; set; }
            public long? CustomerId { get; set; }
            public string WarehouseCode { get; set; }

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

            public string QRCode { get; set; }
            public string LogoImagePath { get; set; }

            public List<TblSndDeliveryNoteLineDto> ItemList { get; set; }


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


            public bool IsVoid { get; set; }

            public byte RevisedNumber { get; set; }
            public long OriginalQuotationId { get; set; }






            public long QuotationHeadId { get; set; }


            public bool IsConvertedFromQuotation { get; set; }
            public bool IsCovertedFromOrder { get; set; }
            public bool IsConvertedDeliveryNoteToInvoice { get; set; }

            public int ConvertedBy { get; set; }
            public DateTime? ConvertedDate { get; set; }
            public bool IsClosed { get; set; }
            public string DeliveryNumber { get; set; }
        }

        public class InputTblSndDeliveryNoteDto : TblSndDeliveryNoteDto
        {
           



        }
        public class InputDeliveryNoteStockAvailabilityDto
        {
            public string WarehouseCode { get; set; }
            public List<TblSndDeliveryNoteDto> ItemList { get; set; }


        }
 

       



      


        public class PrintSndDeliveryNoteDto
        {
            public TblErpSysCompanyDto Company { get; set; }
            public TblOprCustomerMasterDto Customer { get; set; }
            public TblSndDeliveryNoteDto DeliveryNoteHead { get; set; }
            public TblSndDeliveryNoteDto_AR Quotation_AR { get; set; }
            public List<TblSndDeliveryNoteLineDto> DeliveryNoteItems { get; set; }
            public TblErpSysCompanyBranchDto BankDetails { get; set; }

            public string TotalAmountEn { get; set; }
            public string TotalAmountAr { get; set; }
        }

        








        public class TblSndDeliveryNotePagedListDto : TblSndDeliveryNoteListDto
        {
            public string BranchCode { get; set; }
            public TblSndAuthoritiesDto Authority { get; set; }

        }




        public class TblSndDeliveryNoteDto_AR : TblSndDeliveryNoteDto
        {
            public string Quantity_AR { get; set; }
            public string UnitPrice_AR { get; set; }
            public string DiscountAmount_AR { get; set; }
            public string TaxTariffPercentage_AR { get; set; }
            public string AmountBeforeTax_AR { get; set; }

        }


      
       
    }
}
