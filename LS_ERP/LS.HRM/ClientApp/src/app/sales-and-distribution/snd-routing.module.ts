import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SndInvoiceSummaryComponent } from './reports/snd-invoice-summary/snd-invoice-summary.component';
import { SndItemDepartmentReportComponent } from './reports/snd-item-department-report/snd-item-department-report.component';
import { SndItemSalesSummaryComponent } from './reports/snd-item-sales-summary/snd-item-sales-summary.component';
import { SndReportCustomerSalesMonthlyMatrixComponent } from './reports/snd-report-customer-sales-monthly-matrix/snd-report-customer-sales-monthly-matrix.component';
import { SndReportCustomerSalesComponent } from './reports/snd-report-customer-sales/snd-report-customer-sales.component';
import { SndReportInventoryStockAnalysisComponent } from './reports/snd-report-inventory-stock-analysis/snd-report-inventory-stock-analysis.component';
import { SndReportInventoryStockLedgerComponent } from './reports/snd-report-inventory-stock-ledger/snd-report-inventory-stock-ledger.component';
import { SndReportInventoryStockTransactionsAnalysisComponent } from './reports/snd-report-inventory-stock-transactions-analysis/snd-report-inventory-stock-transactions-analysis.component';
import { SndReportInventoryTransactionsComponent } from './reports/snd-report-inventory-transactions/snd-report-inventory-transactions.component';
import { SndAuthoritiesComponent } from './snd-authorities/snd-authorities.component';
import { SndCustomerComponent } from './snd-customer/snd-customer.component';
import { SndDeliveryNotesComponent } from './snd-delivery-notes/snd-delivery-notes.component';
import { SndQuotationsComponent } from './snd-quotations/snd-quotations.component';
import { SndSalesInvoiceComponent } from './snd-sales-invoice/snd-sales-invoice.component';

 

const routes: Routes = [
  

  { path: 'sndCustomer', component: SndCustomerComponent },
  { path: 'sndSalesInvoice', component: SndSalesInvoiceComponent },
  { path: 'sndAuthorities', component: SndAuthoritiesComponent },
  { path: 'sndQuotations', component: SndQuotationsComponent },
  { path: 'sndDeliveryNotes', component: SndDeliveryNotesComponent },
  { path: 'sndInvoiceSummary', component: SndInvoiceSummaryComponent },
  { path: 'sndItemSalesSummary', component: SndItemSalesSummaryComponent },
  { path: 'sndReportCustomerSales', component: SndReportCustomerSalesComponent },
  { path: 'sndReportCustomerSalesMonthlyMatrix', component: SndReportCustomerSalesMonthlyMatrixComponent },
  { path: 'sndItemDepartmentReport', component: SndItemDepartmentReportComponent },
  { path: 'sndInventoryStockLedgerReport', component:SndReportInventoryStockLedgerComponent },
  { path: 'sndInventoryStockAnalysisReport', component: SndReportInventoryStockAnalysisComponent },
  { path: 'sndInventoryTransactionsReport', component: SndReportInventoryTransactionsComponent },
  { path: 'sndStockTransactionsAnalysisReport', component: SndReportInventoryStockTransactionsAnalysisComponent }
  ];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],

})
export class SnDRoutingModule {
}
