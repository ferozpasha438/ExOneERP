import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { SharedModule } from '../sharedcomponent/shared.module';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { SnDRoutingModule } from './snd-routing.module';
import { SndCustomerComponent } from './snd-customer/snd-customer.component';
import { SndSalesInvoiceComponent } from './snd-sales-invoice/snd-sales-invoice.component';
import { AddupdatesndsalesinvoiceComponent } from './snd-sales-invoice/addupdatesndsalesinvoice/addupdatesndsalesinvoice.component';
import { EditsndsalesinvoiceComponent } from './snd-sales-invoice/editsndsalesinvoice/editsndsalesinvoice.component';
import { SndsalesinvoiceprintingComponent } from './snd-sales-invoice/sndsalesinvoiceprinting/sndsalesinvoiceprinting.component';
import { SnduserapprovalComponent } from './sharedpages/snduserapproval/snduserapproval.component';
import { SndsalesinvoiceviewComponent } from './snd-sales-invoice/sndsalesinvoiceview/sndsalesinvoiceview.component';
import { SndcustomerinvoicestatementComponent } from './sharedpages/sndcustomerinvoicestatement/sndcustomerinvoicestatement.component';
import { SndcustomerstatementComponent } from './sharedpages/sndcustomerstatement/sndcustomerstatement.component';
import { SndaddupdatecustomerComponent } from './snd-customer/sndaddupdatecustomer/sndaddupdatecustomer.component';
import { SndupdatingcustomerComponent } from './snd-customer/sndupdatigcustomer/sndupdatingcustomer.component';
import { InvoiceviewComponent } from './snd-sales-invoice/sndsalesinvoiceview/invoiceview/invoiceview.component';
import { InvoiceviewarComponent } from './snd-sales-invoice/sndsalesinvoiceview/invoiceviewar/invoiceviewar.component';
import { SndAuthoritiesComponent } from './snd-authorities/snd-authorities.component';
import { SndAddUpdateAuthoritiesComponent } from './snd-authorities/snd-add-update-authorities/snd-add-update-authorities.component';
import { SndsettleinvoiceComponent } from './sharedpages/sndsettleinvoice/sndsettleinvoice.component';
import { ConfirmationDialogWindowComponent } from './confirmation-dialog-window/confirmation-dialog-window.component';
import { SndQuotationsComponent } from './snd-quotations/snd-quotations.component';
import { AddupdatesndquotationComponent } from './snd-quotations/addupdatesndquotation/addupdatesndquotation.component';
import { SndquotationprintingComponent } from './snd-quotations/sndquotationprinting/sndquotationprinting.component';
import { SndquotationviewComponent } from './snd-quotations/sndquotationview/sndquotationview.component';
import { QuotationviewComponent } from './snd-quotations/sndquotationview/quotationview/quotationview.component';
import { SndquotationviewarComponent } from './snd-quotations/sndquotationview/sndquotationviewar/sndquotationviewar.component';
import { ItemstockavailabilitylistPopupComponent } from './snd-quotations/addupdatesndquotation/itemstockavailabilitylist-popup/itemstockavailabilitylist-popup.component';
import { QuotationstockavailabilityPopupComponent } from './snd-quotations/addupdatesndquotation/quotationstockavailability-popup/quotationstockavailability-popup.component';
import { SndDeliveryNotesComponent } from './snd-delivery-notes/snd-delivery-notes.component';
import { SnddeliverynoteviewComponent } from './snd-delivery-notes/snddeliverynoteview/snddeliverynoteview.component';
import { DeliverynoteviewComponent } from './snd-delivery-notes/snddeliverynoteview/deliverynoteview/deliverynoteview.component';
import { SnddeliverynoteviewarComponent } from './snd-delivery-notes/snddeliverynoteview/snddeliverynoteviewar/snddeliverynoteviewar.component';
import { EditdeliverynoteComponent } from './snd-delivery-notes/editdeliverynote/editdeliverynote.component';
import { SndInvoiceSummaryComponent } from './reports/snd-invoice-summary/snd-invoice-summary.component';
import { SndItemSalesSummaryComponent } from './reports/snd-item-sales-summary/snd-item-sales-summary.component';
import { SndReportCustomerSalesComponent } from './reports/snd-report-customer-sales/snd-report-customer-sales.component';
import { SndReportCustomerSalesMonthlyMatrixComponent } from './reports/snd-report-customer-sales-monthly-matrix/snd-report-customer-sales-monthly-matrix.component';
import { SndItemDepartmentReportComponent } from './reports/snd-item-department-report/snd-item-department-report.component';
import { SndReportInventoryStockLedgerComponent } from './reports/snd-report-inventory-stock-ledger/snd-report-inventory-stock-ledger.component';
import { SndReportInventoryStockAnalysisComponent } from './reports/snd-report-inventory-stock-analysis/snd-report-inventory-stock-analysis.component';
import { SndReportInventoryTransactionsComponent } from './reports/snd-report-inventory-transactions/snd-report-inventory-transactions.component';
import { SndReportInventoryStockTransactionsAnalysisComponent } from './reports/snd-report-inventory-stock-transactions-analysis/snd-report-inventory-stock-transactions-analysis.component';



@NgModule({
  declarations: [
    SndCustomerComponent,
    SndSalesInvoiceComponent,
    AddupdatesndsalesinvoiceComponent,
    EditsndsalesinvoiceComponent,
    SndsalesinvoiceprintingComponent,
    SnduserapprovalComponent,
    SndsalesinvoiceviewComponent,
    SndcustomerinvoicestatementComponent,
    SndcustomerstatementComponent,
    SndaddupdatecustomerComponent,
    SndupdatingcustomerComponent,
    InvoiceviewComponent,
    InvoiceviewarComponent,
    SndAuthoritiesComponent,
    SndAddUpdateAuthoritiesComponent,
    SndsettleinvoiceComponent,
    ConfirmationDialogWindowComponent,
    SndQuotationsComponent,
    AddupdatesndquotationComponent,
    SndquotationprintingComponent,
    SndquotationviewComponent,
    QuotationviewComponent,
    SndquotationviewarComponent,
    ItemstockavailabilitylistPopupComponent,
    QuotationstockavailabilityPopupComponent,
    SndDeliveryNotesComponent,
    SnddeliverynoteviewComponent,
    DeliverynoteviewComponent,
    SnddeliverynoteviewarComponent,
    EditdeliverynoteComponent,
    SndInvoiceSummaryComponent,
    SndItemSalesSummaryComponent,
    SndReportCustomerSalesComponent,
    SndReportCustomerSalesMonthlyMatrixComponent,
    SndItemDepartmentReportComponent,
    SndReportInventoryStockLedgerComponent,
    SndReportInventoryStockAnalysisComponent,
    SndReportInventoryTransactionsComponent,
    SndReportInventoryStockTransactionsAnalysisComponent
  ],
  imports: [
    SnDRoutingModule,
    SharedModule,
    SharedModule,
    MatButtonModule,
    MatSelectModule,
    MatTooltipModule,
    MatDatepickerModule,
    MatFormFieldModule],
  providers: [DatePipe],
  exports: [CommonModule],
  
})
export class SalesAndDistributionModule { }
