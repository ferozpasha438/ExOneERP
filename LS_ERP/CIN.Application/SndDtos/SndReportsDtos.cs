using CIN.Application.InventoryDtos;
using CIN.Application.InvoiceDtos;
using CIN.Domain.InventorySetup;
using CIN.Domain.InvoiceSetup;
using CIN.Domain.OpeartionsMgt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.SndDtos
{


    public class SndCommanReportsDto
    {
        public string ComapnyName { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string LogoURL { get; set; }
        public string Address { get; set; }
        public string BranchName { get; set; }
        public string CustomerName { get; set; }
        public string CustCode { get; set; }
        public string CustAddress1 { get; set; }
        public string CustAddress2 { get; set; }
        public string SiteName { get; set; }

        public int TotalItemsCount { get; set; }

    }


    #region SndInvoiceReport

    public class SndInvoiceSumaryItem : TblSndTranInvoiceDto
    {


    }
    public class SndInvoiceDetailItemDto : TblSndTranInvoiceItemDto
    {
        public int Count { get; set; }

    }

    public class SndInvoiceDetailItem
    {
        public SndInvoiceSumaryItem InvoiceSummary { get; set; }
        public List<SndInvoiceDetailItemDto> InvoiceLineItems { get; set; }
    }


    public class SndInvoiceSumaryListReport : SndCommanReportsDto
    {
        public decimal TotAmount { get; set; }
        public decimal TotDiscount { get; set; }
        public decimal TotNetAmountBT { get; set; }
        public decimal TotTaxAmount { get; set; }
        public decimal TotSalesAmount { get; set; }
        public decimal TotCost { get; set; }
        public List<SndInvoiceSumaryItem> SummaryList { get; set; }
        public List<SndInvoiceDetailItem> DetailList { get; set; }

        public decimal TotGrossMargin { get; set; }
        public decimal TotGrossMarginPer { get; set; }
    }

    #endregion

    #region SndItemSalesReport
    public class SndItemSalesReportDto : SndCommanReportsDto
    {
        public decimal TotAmount { get; set; }
        public decimal TotDiscount { get; set; }
        public decimal TotNetAmountBT { get; set; }
        public decimal TotTaxAmount { get; set; }
        public decimal TotSalesAmount { get; set; }
        public decimal TotCost { get; set; }
        public decimal TotGrossMargin { get; set; }
        public decimal TotGrossMarginPer { get; set; }

        public List<SndItemSaleSummaryItemDto> SummaryReport { get; set; }
        public List<SndItemSaleDetailedItemDto> DetailedReport { get; set; }
    }


    public class SndItemSaleSummaryItemDto
    {
        public long Id { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal NetPrice { get; set; }//Before Tax
        public decimal SalePrice { get; set; }
        public decimal AvgCost { get; set; }
        public decimal NetCost { get; set; }
        public decimal GrossMargin { get; set; }
        public decimal GrossMarginPer { get; set; }
    }
    public class SndDetailedItemLineDto : SndItemSaleSummaryItemDto
    {
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string WarehouseCode { get; set; }
        public bool? IsQuantityDeducted { get; set; }
        public bool? IsPosted { get; set; }
        public bool? isCreditConverted { get; set; }
    }
    public class SndItemSaleDetailedItemDto
    {

        public SndItemSaleSummaryItemDto Summary { get; set; }
        public List<SndDetailedItemLineDto> ItemLines { get; set; }

    }


    #endregion


    #region SndItemSalesReport
    public class SndCustomerSalesReportDto : SndCommanReportsDto
    {
        public int TotCount { get; set; }
        public decimal TotAmount { get; set; }
        public decimal TotDiscount { get; set; }
        public decimal TotNetAmountBT { get; set; }
        public decimal TotTaxAmount { get; set; }
        public decimal TotSalesAmount { get; set; }
        public decimal TotCost { get; set; }
        public decimal TotGrossMargin { get; set; }
        public decimal TotGrossMarginPer { get; set; }

        public List<SndCustomerSalesSummaryItemDto> SummaryReport { get; set; }
    }
    public class SndCustomerSalesSummaryItemDto
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public List<SndInvoiceSumaryItem> InvoicesSummaryList { get; set; }


        public decimal SubTotal { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalCost { get; set; }
        public decimal GrossMargin { get; set; }
        public decimal GrossMarginPer { get; set; }
        public decimal InvoiceCount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal AmountBeforeTax { get; set; }
        public int Count { get; set; }
    }



    #endregion




    #region CustomerSalesMonthlyReportDtos

    public class SndCustomerSalesMonthlyReportDto : SndCommanReportsDto
    {
        public decimal TotSalesAmount { get; set; }
        public decimal TotCost { get; set; }
        public decimal TotGrossMargin { get; set; }
        public decimal TotGrossMarginPer { get; set; }
        public decimal TotCount { get; set; }
        public List<DateTime> Columns { get; set; } = new();
        public List<SndCustomerSalesMonthlyReportItem> MonthlyReports { get; set; } = new();
        public List<SndCustomerSalesMonthlyReportItem> MonthlyTotals { get; set; } = new();
    }

    public class SndCustomerSalesMonthlyReportItem
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime MonthDt { get; set; }
        public decimal SalesAmount { get; set; }
        public decimal Cost { get; set; }
        public decimal GrossMargin { get; set; }
        public decimal GrossMarginPer { get; set; }

        public List<SndCustomerSalesMonthlyReportItem> MonthlyReportsPerCustomer { get; set; } = new();

    }


    #endregion



    #region SndItemDepartmentReport
    public class SndItemDepartmentReportDto: SndCommanReportsDto
    {
       public List<CustomSelectListItem> ItemCategoriesSelectionList { get; set; } = new();
        public decimal TotSalesAmount { get; set; }
        public decimal TotCost { get; set; }
        public decimal TotGrossMargin { get; set; }
        public List<SndItemDepartmentReportSummaryItem> SummaryReport { get; set; } = new();
        public List<SndItemDepartmentReportDetailedItem> DetailedReport { get; set; } = new();

    }
    public class SndItemDepartmentReportSummaryItem
    {
        public string ItemType { get; set; }
        public string ItemCategory { get; set; }
        public string ItemCategoryName { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public int ItemId { get; set; }
        public decimal NetQuantity { get; set; }
        public decimal SalesAmount { get; set; }
        public decimal Cost { get; set; }
        public decimal GrossMargin { get; set; }
        public decimal GrossMarginPer { get; set; }

    }
    public class SndItemDepartmentReportDetailedItem
    {
        public SndItemDepartmentReportSummaryItem Summary { get; set; } = new();
        public List<SndInvoiceDetailItemDto> DetailedItems { get; set; }

    }

    #endregion



    #region InventoryStockLedgerReport
    public class InventoryStockLedgerReportDto : SndCommanReportsDto
    {
        public List<InventoryStockLedgerReportPerItemDto> ReportItems { get; set; }
    }
     public class InventoryStockLedgerReportPerItemDto 
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public long ItemId { get; set; } 
      
        public SndInventoryHistoryDto OpeningBal { get; set; }
        public List<SndInventoryHistoryDto> Transactions { get; set; } = new();
        public SndInventoryHistoryDto TransactionBal { get; set; } = new();
        public SndInventoryHistoryDto ClosingBal { get; set; } = new();
    }
    




        public class SndInventoryHistoryDto 
    {

        // public string InvoiceNumber { get; set; }

        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }

        public string TranDescription { get; set; }
        public string DcNumber { get; set; }
        public string Source { get; set; }
        public DateTime TranDate { get; set; }
        public decimal? InQty { get; set; }
        public decimal? BalanceQty { get; set; }
      
        public decimal? OutQty { get; set; }

         public decimal? InCost { get; set; }
        public decimal? BalanceCost { get; set; }
      
        public decimal? OutCost { get; set; }


    }
    #endregion



    #region InventoryTransactionsReportDtos
    public class SndInventoryTransactionsReportDto:SndCommanReportsDto
    {
        public List<SndInventoryTransactionsDto> ReportItems { get; set; }

    }

    public class SndInventoryTransactionsDto
    {
        public DateTime TranDate { get; set; }
        public string TranNumber { get; set; }
        public string TranLocation { get; set; }
        public string TranToLocation { get; set; }
        public string TranDocNumber { get; set; }
        public sbyte TranPostStatus { get; set; } = 0;
        public decimal TranTotalCost { get; set; } = 0;
        public string TranReference { get; set; }
        public string TranType { get; set; }
        public string TranType2 { get; set; }

        public List<SndInventoryTransactionsItemDto> TransactionItems { get; set; } = new();
    }
     public class SndInventoryTransactionsItemDto
    {
        public string TranItemName { get; set; }

        public decimal TranItemQty { get; set; } = 0;

        public string TranItemUnit { get; set; }

        public decimal TranUOMFactor { get; set; } = 1;

        public decimal TranItemCost { get; set; } = 0;

        public decimal TranTotCost { get; set; } = 0;
        public decimal TranTotQty { get; set; } = 0;

    }

    #endregion


    #region InventoryStockTransactionsAnalysisReportDtos

    public class StockTransactionAnalysisReportDto : SndCommanReportsDto
    {
        public List<InventoryStockTransactionAnalysisReportPerItemDto> ReportItems { get; set; }
    }

    public class InventoryStockTransactionAnalysisReportPerItemDto
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public long ItemId { get; set; }

        public SndInventoryStockAnalysisHistoryDto OpeningBal { get; set; }
        public SndInventoryStockAnalysisHistoryDto SalesBal { get; set; } = new();
        public SndInventoryStockAnalysisHistoryDto PurchaseBal { get; set; } = new();
        public SndInventoryStockAnalysisHistoryDto AdjustmentBal { get; set; } = new();
        public SndInventoryStockAnalysisHistoryDto ClosingBal { get; set; } = new();
    }

    public class SndInventoryStockAnalysisHistoryDto
    {

       public decimal Qty { get; set; }
        public decimal Cost { get; set; }
        public decimal SalePrice { get; set; } = 0;


    }

    #endregion
}
