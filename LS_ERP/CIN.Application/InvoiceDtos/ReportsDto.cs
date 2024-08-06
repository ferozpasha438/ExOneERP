using CIN.Application.Common;
using CIN.Application.GeneralLedgerDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.InvoiceDtos
{

    public class CustomerRevenueAnalysisListDto
    {
        public CustomerRevenueAnalysisDto Summary { get; set; }
        public CommonDataLedgerDto Company { get; set; }
        public List<CustomerRevenueAnalysisDto> List { get; set; }
    }

    public class CustomerRevenueAnalysisDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal? Rv { get; set; }
        public decimal? So { get; set; }
        public decimal? De { get; set; }
        public decimal? Ae { get; set; }
        public decimal? Ot { get; set; }

        public decimal? ExpTotal { get; set; }
        public decimal? GrossTotal { get; set; }
        public decimal? NetTotal { get; set; }

    }

    public class PurchaseItemsReportingPrintListDto
    {
        public List<PurchaseReportingPrintListDto> ItemPrintingList { get; set; }
        public CommonDataLedgerDto Company { get; set; }
    }
    public class PurchaseReportingPrintListDto
    {
        public List<PurchaseInvoiceReportingPrintListDto> ItemList { get; set; }
        public CommonDataLedgerDto Company { get; set; }
    }

    public class TaxPricingDto
    {
        public decimal? TotalSaleAmount { get; set; }
        public decimal? TotalPurchaseAmount { get; set; }
        public decimal? TotalInputTaxAomunt { get; set; }
        public decimal? TotalOutputTaxAomunt { get; set; }
    }

    public class PurchaseInvoiceReportingPrintListDto
    {
        //public decimal? TotalTax { get; set; }
        //public decimal? TotalAmount { get; set; }

        public TaxPricingDto InvoicePrice { get; set; }
        public TaxPricingDto CreditPrice { get; set; }
        //  public List<TaxPricingDto> SummaryList { get; set; }
        public List<TaxReportingPrintDto> List { get; set; }
        public List<TaxReportingPrintDto> CreditList { get; set; }
    }

    public class TaxReportingPrintListDto : TaxPricingDto
    {
        public decimal? TotalTax { get; set; }
        public decimal? TotalAmount { get; set; }
        public CommonDataLedgerDto Company { get; set; }
        public List<TaxReportingPrintDto> List { get; set; }
    }

    public class TaxReportingPrintDto
    {
        public string TranNumber { get; set; }
        public DateTime? Date { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public string Code { get; set; }
        public bool IsCredit { get; set; }
        public string Name { get; set; }
        public string TaxIdNumber { get; set; }
        public string TaxCode { get; set; }
        public decimal? Amount { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? InputTaxAmount { get; set; }
        public decimal? OutputTaxAmount { get; set; }
    }

    public class ProfitAndLossListDto
    {
        //public string ComapnyName { get; set; }
        //public string LogoURL { get; set; }
        //public string Address { get; set; }
        //public string BranchName { get; set; }
        public decimal? TotalBalance { get; set; }
        public decimal? TotalDrAmount { get; set; }
        public decimal? TotalCrAmount { get; set; }
        public decimal? TotalProfitLossAmount { get; set; }
        public CommonDataLedgerDto Company { get; set; }
        public List<ProfitAndLossItemDto> List { get; set; }
    }
    public class ProfitAndLossItemDto
    {
        public string AcCode { get; set; }
        public string Level { get; set; }
        public string FinAcName { get; set; }
        public decimal? DrAmount { get; set; }
        public decimal? CrAmount { get; set; }

        public decimal? DrAmount_Bal { get; set; }
        public decimal? CrAmount_Bal { get; set; }

    }

    public class CustomerVoucherSummaryListDto
    {
        public string ComapnyName { get; set; }
        public string LogoURL { get; set; }
        public string Address { get; set; }
        public string BranchName { get; set; }
        public decimal? TotalBalance { get; set; }
        public decimal? TotalDrAmount { get; set; }
        public decimal? TotalCrAmount { get; set; }
        public decimal? TotalAppliedAmount { get; set; }
        public decimal? TotalNetBalance { get; set; }
        public ReportListDto<List<CustomerVoucherSummaryDto>> List { get; set; }
    }
    public class CustomerInvoiceSummaryListDto : CustomerVoucherSummaryListDto
    {
        public List<CustomerVoucherSummaryDto> Invoices { get; set; }
    }

    public class CustomerVoucherSummaryDto
    {

        public string BranchName { get; set; }
        public string CheckNumber { get; set; }
        public string VoucherNumber { get; set; }
        public DateTime? TranDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public string Trantype { get; set; }
        public string Remarks { get; set; }
        public string PayCode { get; set; }
        public string PayAcCode { get; set; }
        public DateTime? CheckDate { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string VendCode { get; set; }
        public string DocNum { get; set; }
        public string VendName { get; set; }
        public decimal? DrAmount { get; set; }
        public decimal? CrAmount { get; set; }
        public decimal? AppliedAmount { get; set; }
        public decimal? Balance { get; set; }
        public decimal? NetBalance { get; set; }
        public string SiteName { get; set; }
    }

    public class CommonSummaryListDto
    {
        public string ComapnyName { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string LogoURL { get; set; }
        public string Address { get; set; }
        public string BranchName { get; set; }
        public decimal? TotalOpeningAmount { get; set; }
        public decimal? TotalBalance { get; set; }
        public decimal? TotalDrAmount { get; set; }
        public decimal? TotalCrAmount { get; set; }
        public decimal? TotalItemQty { get; set; }
        public decimal? NetTotalBalanceAmount { get; set; }
    }
    public class CommonCustomerInfoItemDto : CommonSummaryListDto
    {
        public string CustomerName { get; set; }
        public string CustCode { get; set; }
        public string CustAddress1 { get; set; }
        public string CustAddress2 { get; set; }

    }

    public class CustomerBalanceSummaryListDto : CommonSummaryListDto
    {
        //public string Code { get; set; }    
        //public string Name { get; set; }    
        //public decimal? C { get; set; }    

        public List<CustomerBalanceSummaryDto> List { get; set; }
        public ReportListDto<List<CustomerBalanceSummaryDto>> ListItems { get; set; }
        public List<CustomSelectListItem> WarehouseItems { get; set; }
    }
    public class CustomerBalanceSummaryDto
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Warehouse { get; set; }
        public string VendCode { get; set; }
        public string VendName { get; set; }
        public decimal? DrAmount { get; set; }
        public decimal? CrAmount { get; set; }
        public decimal? Balance { get; set; }
        public decimal? OpeningBalance { get; set; }
        public decimal? NetTotalBalanceAmount { get; set; }
        public decimal? ClosingBalance { get; set; }
        public long? InvoiceId { get; set; }
        public List<string> RefInvoiceIds { get; set; }
        public int? PaymentId { get; set; }
        public string DocNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string Remarks { get; set; }
        public string Trantype { get; set; }
        public DateTime? TranDate { get; set; }
        public bool IsOpening { get; set; }
        public bool IsClosing { get; set; }
        public string SiteCode { get; set; }
        public string SiteName { get; set; }
    }

    public class AgeingReportAnalysisListDto : CommonSummaryListDto
    {
        public ReportListDto<AgeingReportAnalysisDto> List { get; set; }
    }
    public class AgeingReportAnalysisDto
    {
        public string VendCode { get; set; }
        public string VendName { get; set; }
        public decimal? Gpr1 { get; set; }
        public decimal? Gpr2 { get; set; }
        public decimal? Gpr3 { get; set; }
        public decimal? Gpr4 { get; set; }
        public decimal? Gpr5 { get; set; }
        public decimal? Gpr6 { get; set; }
        public decimal? Gpr7 { get; set; }
        public decimal? Balance { get; set; }
        public DateTime? DueDate { get; set; }

    }



}
