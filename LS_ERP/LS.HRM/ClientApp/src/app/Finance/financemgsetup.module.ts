import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from '../sharedcomponent/shared.module';
import { FinancemgsetupRoutingModule } from './financemgsetup-routing.module';
//import { CashvoucherComponent } from './cashvoucher/cashvoucher.component';
//import { BankvoucherComponent } from './bankvoucher/bankvoucher.component';
//import { JournalvoucherComponent } from './journalvoucher/journalvoucher.component';
//import { LedgeraccountsComponent } from './ledgeraccounts/ledgeraccounts.component';
import { ArcustomerComponent } from './accountsreceivable/arcustomer/arcustomer.component';
import { ArsalesinvoiceComponent } from './accountsreceivable/arsalesinvoice/arsalesinvoice.component';
import { AradjustmentComponent } from './accountsreceivable/aradjustment/aradjustment.component';
import { ArpaymentComponent } from './accountsreceivable/arpayment/arpayment.component';
import { ArreportsComponent } from './accountsreceivable/arreports/arreports.component';
import { ApvendorComponent } from './accountspayable/apvendor/apvendor.component';
import { AppurchaseinvoiceComponent } from './accountspayable/appurchaseinvoice/appurchaseinvoice.component';
import { ApadjustmentComponent } from './accountspayable/apadjustment/apadjustment.component';
import { AppaymentComponent } from './accountspayable/appayment/appayment.component';
import { ApreportsComponent } from './accountspayable/apreports/apreports.component';
import { CheckbookregisterComponent } from './finacialutilities/checkbookregister/checkbookregister.component';
import { BankreconciliationComponent } from './finacialutilities/bankreconciliation/bankreconciliation.component';
import { PdcmanagementComponent } from './finacialutilities/pdcmanagement/pdcmanagement.component';
import { LederanalysisComponent } from './reports/lederanalysis/lederanalysis.component';
import { VoucherreportingComponent } from './reports/voucherreporting/voucherreporting.component';
import { ChartofaccountsComponent } from './reports/chartofaccounts/chartofaccounts.component';
import { TrialbalanceComponent } from './reports/trialbalance/trialbalance.component';
import { ProfitandlossaccountComponent } from './reports/profitandlossaccount/profitandlossaccount.component';
import { BalancesheetComponent } from './reports/balancesheet/balancesheet.component';
import { AddupdatecustomerComponent } from './accountsreceivable/sharedpages/addupdatecustomer/addupdatecustomer.component';
import { AddupdatesalesinvoiceComponent } from './accountsreceivable/sharedpages/addupdatesalesinvoice/addupdatesalesinvoice.component';
import { EditArsalesinvoiceComponent } from './accountsreceivable/sharedpages/editsalesinvoice/editsalesinvoice.component';
import { SalesinvoiceviewComponent } from './sharedpages/salesinvoiceview/salesinvoiceview.component';
import { AddupdatecustomerpaymentComponent } from './accountsreceivable/sharedpages/addupdatecustomerpayment/addupdatecustomerpayment.component';
import { InvoiceviewComponent } from './sharedpages/salesinvoiceview/invoiceview/invoiceview.component';
import { InvoiceviewarComponent } from './sharedpages/salesinvoiceview/invoiceviewar/invoiceviewar.component';
import { SettlementmodeComponent } from './accountsreceivable/sharedpages/settlementmode/settlementmode.component';
import { UserapprovalComponent } from './accountsreceivable/sharedpages/userapproval/userapproval.component';
import { CustomerstatementComponent } from './accountsreceivable/sharedpages/customerstatement/customerstatement.component';
import { CustomerinvoicestatementComponent } from './accountsreceivable/sharedpages/customerinvoicestatement/customerinvoicestatement.component';
import { AddupdatevendorComponent } from './accountspayable/sharedpages/addupdatevendor/addupdatevendor.component';
import { AddupdatevendorpaymentComponent } from './accountspayable/sharedpages/addupdatevendorpayment/addupdatevendorpayment.component';
import { VendorinvoicestatementComponent } from './accountspayable/sharedpages/vendorinvoicestatement/vendorinvoicestatement.component';
import { VendorstatementComponent } from './accountspayable/sharedpages/vendorstatement/vendorstatement.component';
import { AddupdatepurchaseinvoiceComponent } from './accountspayable/sharedpages/addupdatepurchaseinvoice/addupdatepurchaseinvoice.component';
import { EditpurchaseinvoiceComponent } from './accountspayable/sharedpages/editpurchaseinvoice/editpurchaseinvoice.component';
import { PurchaseinvoiceviewarComponent } from './sharedpages/purchaseinvoiceview/purchaseinvoiceviewar/purchaseinvoiceviewar.component';
import { PurchaseinvoiceComponent } from './sharedpages/purchaseinvoiceview/purchaseinvoice.component';
import { ApSettlementmodeComponent } from './accountspayable/sharedpages/settlementmode/settlementmode.component';
import { ApUserapprovalComponent } from './accountspayable/sharedpages/userapproval/userapproval.component';
import { Customerreceiptvoucher } from './sharedpages/vouchers/customerreceiptvoucher.component';
import { Vendorpaymentvoucher } from './sharedpages/vouchers/vendorpaymentvoucher.component';
import { BankvoucherComponent } from './bankvoucher/bankvoucher.component';
import { CashvoucherComponent } from './cashvoucher/cashvoucher.component';
import { JournalvoucherComponent } from './journalvoucher/journalvoucher.component';
import { LedgeraccountsComponent } from './ledgeraccounts/ledgeraccounts.component';
import { AddupdatejournalvoucherComponent } from './journalvoucher/sharedpages/addupdatejournalvoucher/addupdatejournalvoucher.component';
import { PostingComponent } from './journalvoucher/sharedpages/posting/posting.component';
import { JvUserapprovalComponent } from './journalvoucher/sharedpages/userapproval/userapproval.component';
import { AddupdatecashvoucherComponent } from './cashvoucher/sharedpages/addupdatecashvoucher/addupdatecashvoucher.component';
import { ChPostingComponent } from './cashvoucher/sharedpages/posting/posting.component';
import { ChUserapprovalComponent } from './cashvoucher/sharedpages/userapproval/userapproval.component';
import { AddupdatebankvoucherComponent } from './bankvoucher/sharedpages/addupdatebankvoucher/addupdatebankvoucher.component';
import { BvPostingComponent } from './bankvoucher/sharedpages/posting/posting.component';
import { BvUserapprovalComponent } from './bankvoucher/sharedpages/userapproval/userapproval.component';
import { BvprintComponent } from './bankvoucher/sharedpages/bvprint/bvprint.component';
import { CvprintComponent } from './cashvoucher/sharedpages/cvprint/cvprint.component';
import { JvprintComponent } from './journalvoucher/sharedpages/jvprint/jvprint.component';

import { CustomerbalancesummaryComponent } from './accountsreceivable/arreports/customerbalancesummary/customerbalancesummary.component';
import { CustomervouchersummaryComponent } from './accountsreceivable/arreports/customervouchersummary/customervouchersummary.component';
import { CustomerpaymentsummaryComponent } from './accountsreceivable/arreports/customerpaymentsummary/customerpaymentsummary.component';

import { VendorvouchersummaryComponent } from './accountspayable/apreports/vendorvouchersummary/vendorvouchersummary.component';
import { VendorbalancesummaryComponent } from './accountspayable/apreports/vendorbalancesummary/vendorbalancesummary.component';
import { VendorpaymentsummaryComponent } from './accountspayable/apreports/vendorpaymentsummary/vendorpaymentsummary.component';
import { OpmarpaymentComponent } from './accountsreceivable/opmarpayment/opmarpayment.component';
import { AddupdateopmcustomerpaymentComponentComponent } from './accountsreceivable/sharedpages/addupdateopmcustomerpayment-component/addupdateopmcustomerpayment-component.component';
import { OpmCustomerreceiptvoucher } from './sharedpages/vouchers/opmcustomerreceiptvoucher.component';
import { OpmcustomersummaryComponent } from './accountsreceivable/arreports/opmcustomersummary/opmcustomersummary.component';
import { OpmvendorsummaryComponent } from './accountspayable/apreports/opmvendorsummary/opmvendorsummary.component';
import { OpmappaymentComponent } from './accountspayable/opmappayment/opmappayment.component';
import { AddupdateopmvendorpaymentComponent } from './accountspayable/sharedpages/addupdateopmvendorpayment/addupdateopmvendorpayment.component';
import { OpmVendorpaymentvoucher } from './sharedpages/vouchers/opmvendorpaymentvoucher.component';
import { PurchaseinvoiceviewComponent } from './sharedpages/purchaseinvoiceview/purchaseinvoiceview/purchaseinvoiceview.component';
import { ProductsComponent } from './accountsreceivable/products/products.component';
import { ProducttypesComponent } from './accountsreceivable/producttypes/producttypes.component';
import { UnittypesComponent } from './accountsreceivable/unittypes/unittypes.component';
import { AddupdateunittypeComponent } from './accountsreceivable/sharedpages/addupdateunittype/addupdateunittype.component';
import { AddupdateproducttypeComponent } from './accountsreceivable/sharedpages/addupdateproducttype/addupdateproducttype.component';
import { AddupdateproductComponent } from './accountsreceivable/sharedpages/addupdateproduct/addupdateproduct.component';
import { CustomerinvoicesummaryComponent } from './accountsreceivable/arreports/customerinvoicesummary/customerinvoicesummary.component';
import { VendorinvoicesummaryComponent } from './accountspayable/apreports/vendorinvoicesummary/vendorinvoicesummary.component';
import { VendorpaymentvouchersummaryComponent } from './accountspayable/apreports/vendorpaymentvouchersummary/vendorpaymentvouchersummary.component';
import { CustomerpaymentvouchersummaryComponent } from './accountsreceivable/arreports/customerpaymentvouchersummary/customerpaymentvouchersummary.component';
import { ApaginganalysisComponent } from './accountspayable/apreports/apaginganalysis/apaginganalysis.component';
import { AginganalysisComponent } from './accountsreceivable/arreports/aginganalysis/aginganalysis.component';
import { TaxreportingComponent } from './reports/taxreporting/taxreporting.component';
import { PurchaseinvoiceprintingComponent } from './accountspayable/sharedpages/purchaseinvoiceprinting/purchaseinvoiceprinting.component';
import { RevenueanalysisreportComponent } from './reports/revenueanalysisreport/revenueanalysisreport.component';
import { SalesinvoiceprintingComponent } from './accountsreceivable/sharedpages/salesinvoiceprinting/salesinvoiceprinting.component';
import { CustodyledgeranalysisComponent } from './reports/custodyledgeranalysis/custodyledgeranalysis.component';
import { SchoolinvoiceprintingComponent } from './accountsreceivable/sharedpages/schoolinvoiceprinting/schoolinvoiceprinting.component';
import { UpdatingcustomerComponent } from './accountsreceivable/sharedpages/updatingcustomer/updatingcustomer.component';
import { PostingadvancepaymentComponent } from './accountsreceivable/sharedpages/postingadvancepayment/postingadvancepayment.component';
import { AdvancepaymentComponent } from './accountsreceivable/advancepayment/advancepayment.component';
import { AddupdateadvancepaymentComponent } from './accountsreceivable/sharedpages/addupdateadvancepayment/addupdateadvancepayment.component';
import { BulkpostingComponent } from './accountsreceivable/sharedpages/bulkposting/bulkposting.component';
import { ApadvancepaymentComponent } from './accountspayable/apadvancepayment/apadvancepayment.component';
import { AddupdateapadvancepaymentComponent } from './accountspayable/sharedpages/addupdateapadvancepayment/addupdateapadvancepayment.component';
import { BrsstatementComponent } from './brsstatement/brsstatement.component';
import { BrsprintComponent } from './brsstatement/brsprint/brsprint.component';
import { AddupdatebrsstatementComponent } from './brsstatement/addupdatebrsstatement/addupdatebrsstatement.component';
import { PaymentsettlementmodeComponent } from './accountsreceivable/sharedpages/paymentsettlementmode/paymentsettlementmode.component';
import { PdclistComponent } from './accountspayable/pdclist/pdclist.component';
import { EditcheckdateComponent } from './accountspayable/sharedpages/editcheckdate/editcheckdate.component';
//import { AddupdatesalesinvoiceComponent } from './sharedpages/addupdatesalesinvoice/addupdatesalesinvoice.component';


@NgModule({
  declarations: [
    CashvoucherComponent, BankvoucherComponent, JournalvoucherComponent, LedgeraccountsComponent, ArcustomerComponent,
    ArsalesinvoiceComponent, AradjustmentComponent, ArpaymentComponent, ArreportsComponent, ApvendorComponent,
    AppurchaseinvoiceComponent, ApadjustmentComponent, AppaymentComponent, ApreportsComponent, CheckbookregisterComponent,
    BankreconciliationComponent, PdcmanagementComponent, LederanalysisComponent, VoucherreportingComponent, ChartofaccountsComponent,
    TrialbalanceComponent, ProfitandlossaccountComponent, BalancesheetComponent, AddupdatecustomerComponent, AddupdatesalesinvoiceComponent,
    AppurchaseinvoiceComponent, EditArsalesinvoiceComponent, SalesinvoiceviewComponent, AddupdatecustomerpaymentComponent,
    InvoiceviewComponent, InvoiceviewarComponent, PurchaseinvoiceviewComponent, PurchaseinvoiceviewarComponent,
    SettlementmodeComponent, ApSettlementmodeComponent, UserapprovalComponent, ApUserapprovalComponent, JvUserapprovalComponent, CustomerstatementComponent, CustomerinvoicestatementComponent,
    AddupdatevendorComponent, AddupdatevendorpaymentComponent, VendorinvoicestatementComponent, VendorstatementComponent,
    AddupdatepurchaseinvoiceComponent, EditpurchaseinvoiceComponent, PurchaseinvoiceviewarComponent, PurchaseinvoiceComponent,
    Customerreceiptvoucher, Vendorpaymentvoucher, AddupdatejournalvoucherComponent, PostingComponent, AddupdatecashvoucherComponent,
    ChPostingComponent, ChUserapprovalComponent, AddupdatebankvoucherComponent, BvPostingComponent, BvUserapprovalComponent, BvprintComponent, CvprintComponent, JvprintComponent,
    CustomerbalancesummaryComponent, CustomervouchersummaryComponent, CustomerpaymentsummaryComponent, VendorvouchersummaryComponent, VendorbalancesummaryComponent, VendorpaymentsummaryComponent,
    OpmarpaymentComponent, AddupdateopmcustomerpaymentComponentComponent, OpmCustomerreceiptvoucher, OpmVendorpaymentvoucher,
    OpmcustomersummaryComponent, OpmvendorsummaryComponent, OpmappaymentComponent, AddupdateopmvendorpaymentComponent, ProductsComponent, ProducttypesComponent, UnittypesComponent, AddupdateunittypeComponent,
    AddupdateproducttypeComponent, AddupdateproductComponent, CustomerinvoicesummaryComponent,
    VendorinvoicesummaryComponent, VendorpaymentvouchersummaryComponent, CustomerpaymentvouchersummaryComponent, ApaginganalysisComponent,
    AginganalysisComponent, TaxreportingComponent, PurchaseinvoiceprintingComponent, RevenueanalysisreportComponent, SalesinvoiceprintingComponent,
    CustodyledgeranalysisComponent, SchoolinvoiceprintingComponent, UpdatingcustomerComponent, PostingadvancepaymentComponent,
    AdvancepaymentComponent, AddupdateadvancepaymentComponent, BulkpostingComponent, ApadvancepaymentComponent,
    AddupdateapadvancepaymentComponent, BrsstatementComponent, BrsprintComponent, AddupdatebrsstatementComponent, PaymentsettlementmodeComponent, PdclistComponent, EditcheckdateComponent
  ],
  imports: [
    FinancemgsetupRoutingModule,
    SharedModule
  ],
  exports: [CommonModule],

})
export class FinancemgsetupModule { }
