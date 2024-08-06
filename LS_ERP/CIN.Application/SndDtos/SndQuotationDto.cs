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
    public class SndQuotationDto
    {






        public class TblSndTranQuotationItemDto : PrimaryKeyDto<long>
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
            public decimal? FooterDiscountAmount { get; set; }
            public decimal? ItemInvAmount { get; set; }
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

        }




        public class TblSndTranQuotationListDto : PrimaryKeyDto<long>
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
            public bool ApprovedUser { get; set; }
            public bool IsApproved { get; set; }

            public bool IsVoid { get; set; }

            public bool IsConvertedSndQuotationToDeliveryNote { get; set; }
            public bool IsConvertedToOrder { get; set; }
            public bool IsConvertedSndQuotationToInvoice { get; set; }
            public bool IsRevised { get; set; }
            public byte RevisedNumber { get; set; }
            public long OriginalQuotationId { get; set; }

            public bool HasAuthority { get; set; }



            public decimal? AppliedAmount { get; set; }
            public List<string> ItemList { get; set; }

            public string PaymentTermName { get; set; }
            public int DueDays { get; set; }

            public bool IsFinalRevision { get; set; }


        }

        public class TblSndTranQuotationDto : PrimaryKeyDto<long>
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

            public List<TblSndTranQuotationItemDto> ItemList { get; set; }


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

            public bool IsConvertedSndQuotationToDeliveryNote { get; set; }
            public bool IsConvertedToOrder { get; set; }
            public bool IsConvertedToInvoice { get; set; }
            public bool IsRevised { get; set; }
            public byte RevisedNumber { get; set; }
            public long OriginalQuotationId { get; set; }

            public bool IsFinalRevision { get; set; }

        }

        public class InputTblSndTranQuotationDto : TblSndTranQuotationDto
        {
            public EnumQuotationSaveType SaveType { get; set; }



        }
        public class InputQuotationStockAvailabilityDto
        {
            public string WarehouseCode { get; set; }
            public List<TblSndTranQuotationItemDto> ItemList { get; set; }


        }


        public enum EnumQuotationSaveType
        {
            Save = 0, SaveAndApprove = 1, Revise=2,
        }




      


        public class PrintSndQuotationDto
        {
            public TblErpSysCompanyDto Company { get; set; }
            public TblOprCustomerMasterDto Customer { get; set; }
            public TblSndTranQuotationDto Quotation { get; set; }
            public TblSndTranQuotationDto_AR Quotation_AR { get; set; }
            public List<TblSndTranQuotationItemDto> QuotationItems { get; set; }
            public TblErpSysCompanyBranchDto BankDetails { get; set; }

            public string TotalAmountEn { get; set; }
            public string TotalAmountAr { get; set; }
        }

        








        public class TblSndTranQuotationPagedListDto : TblSndTranQuotationListDto
        {
            public string BranchCode { get; set; }
            public TblSndAuthoritiesDto Authority { get; set; }

        }




        public class TblSndTranQuotationDto_AR : TblSndTranQuotationDto
        {
            public string Quantity_AR { get; set; }
            public string UnitPrice_AR { get; set; }
            public string DiscountAmount_AR { get; set; }
            public string TaxTariffPercentage_AR { get; set; }
            public string AmountBeforeTax_AR { get; set; }

        }


      
        public class ItemStockAvailabilityDto
        {
            public string  ItemCode { get; set; } 
            public string  UnitType { get; set; }
            public decimal Quantity { get; set; }
            public decimal AvailableQuantity { get; set; }
            public TblErpInvItemsUOMDto IUM { get; set; }
            public TblErpInvItemInventoryDto StockDetails { get; set; }
            public TblErpInvItemMasterDto ItemMaster { get; set; }
            public decimal StockStatus { get; set; }
        }
    }
}
