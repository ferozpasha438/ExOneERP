import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ApadjustmentComponent } from './accountspayable/apadjustment/apadjustment.component';
import { ApadvancepaymentComponent } from './accountspayable/apadvancepayment/apadvancepayment.component';
import { AppaymentComponent } from './accountspayable/appayment/appayment.component';
import { AppurchaseinvoiceComponent } from './accountspayable/appurchaseinvoice/appurchaseinvoice.component';
import { ApaginganalysisComponent } from './accountspayable/apreports/apaginganalysis/apaginganalysis.component';
import { ApreportsComponent } from './accountspayable/apreports/apreports.component';
import { OpmvendorsummaryComponent } from './accountspayable/apreports/opmvendorsummary/opmvendorsummary.component';
import { VendorbalancesummaryComponent } from './accountspayable/apreports/vendorbalancesummary/vendorbalancesummary.component';
import { VendorinvoicesummaryComponent } from './accountspayable/apreports/vendorinvoicesummary/vendorinvoicesummary.component';
import { VendorpaymentsummaryComponent } from './accountspayable/apreports/vendorpaymentsummary/vendorpaymentsummary.component';
import { VendorpaymentvouchersummaryComponent } from './accountspayable/apreports/vendorpaymentvouchersummary/vendorpaymentvouchersummary.component';
import { VendorvouchersummaryComponent } from './accountspayable/apreports/vendorvouchersummary/vendorvouchersummary.component';
import { ApvendorComponent } from './accountspayable/apvendor/apvendor.component';
import { OpmappaymentComponent } from './accountspayable/opmappayment/opmappayment.component';
import { PdclistComponent } from './accountspayable/pdclist/pdclist.component';
import { AdvancepaymentComponent } from './accountsreceivable/advancepayment/advancepayment.component';
import { AradjustmentComponent } from './accountsreceivable/aradjustment/aradjustment.component';
import { ArcustomerComponent } from './accountsreceivable/arcustomer/arcustomer.component';
import { ArpaymentComponent } from './accountsreceivable/arpayment/arpayment.component';
import { AginganalysisComponent } from './accountsreceivable/arreports/aginganalysis/aginganalysis.component';
import { ArreportsComponent } from './accountsreceivable/arreports/arreports.component';
import { CustomerbalancesummaryComponent } from './accountsreceivable/arreports/customerbalancesummary/customerbalancesummary.component';
import { CustomerinvoicesummaryComponent } from './accountsreceivable/arreports/customerinvoicesummary/customerinvoicesummary.component';
import { CustomerpaymentsummaryComponent } from './accountsreceivable/arreports/customerpaymentsummary/customerpaymentsummary.component';
import { CustomerpaymentvouchersummaryComponent } from './accountsreceivable/arreports/customerpaymentvouchersummary/customerpaymentvouchersummary.component';
import { CustomervouchersummaryComponent } from './accountsreceivable/arreports/customervouchersummary/customervouchersummary.component';
import { OpmcustomersummaryComponent } from './accountsreceivable/arreports/opmcustomersummary/opmcustomersummary.component';
import { ArsalesinvoiceComponent } from './accountsreceivable/arsalesinvoice/arsalesinvoice.component';
import { OpmarpaymentComponent } from './accountsreceivable/opmarpayment/opmarpayment.component';
import { ProductsComponent } from './accountsreceivable/products/products.component';
import { ProducttypesComponent } from './accountsreceivable/producttypes/producttypes.component';
import { UnittypesComponent } from './accountsreceivable/unittypes/unittypes.component';
import { BankvoucherComponent } from './bankvoucher/bankvoucher.component';
import { BrsstatementComponent } from './brsstatement/brsstatement.component';
import { CashvoucherComponent } from './cashvoucher/cashvoucher.component';
import { JournalvoucherComponent } from './journalvoucher/journalvoucher.component';
import { LedgeraccountsComponent } from './ledgeraccounts/ledgeraccounts.component';
import { BalancesheetComponent } from './reports/balancesheet/balancesheet.component';
import { ChartofaccountsComponent } from './reports/chartofaccounts/chartofaccounts.component';
import { CustodyledgeranalysisComponent } from './reports/custodyledgeranalysis/custodyledgeranalysis.component';
import { LederanalysisComponent } from './reports/lederanalysis/lederanalysis.component';
import { ProfitandlossaccountComponent } from './reports/profitandlossaccount/profitandlossaccount.component';
import { RevenueanalysisreportComponent } from './reports/revenueanalysisreport/revenueanalysisreport.component';
import { TaxreportingComponent } from './reports/taxreporting/taxreporting.component';
import { TrialbalanceComponent } from './reports/trialbalance/trialbalance.component';
import { VoucherreportingComponent } from './reports/voucherreporting/voucherreporting.component';

const routes: Routes = [
  //{ path: 'cashvoucher', component: CashvoucherComponent },
  //{ path: 'bankvoucher', component: BankvoucherComponent },
  //{ path: 'journalvoucher', component: JournalvoucherComponent },
  //{ path: 'ledgeraccounts', component: LedgeraccountsComponent },

  //AR
  { path: 'arcustomer', component: ArcustomerComponent },
  { path: 'arsalesinvoice', component: ArsalesinvoiceComponent },
  { path: 'aradjustment', component: AdvancepaymentComponent },
  //{ path: 'aradjustment', component: AradjustmentComponent },
  { path: 'arpayment', component: ArpaymentComponent },
  { path: 'opmarpayment', component: OpmarpaymentComponent },
  { path: 'arcustbalancesummary', component: CustomerbalancesummaryComponent },
  { path: 'arcustpaymentsummary', component: CustomerpaymentsummaryComponent },
  { path: 'arcustvouchersummary', component: CustomervouchersummaryComponent },
  { path: 'opmarcustvouchersummary', component: OpmcustomersummaryComponent },
  { path: 'arcustinvoicesummary', component: CustomerinvoicesummaryComponent },
  { path: 'arcustpaymentvoucherhist', component: CustomerpaymentvouchersummaryComponent },
  { path: 'araginganalysis', component: AginganalysisComponent },

  //AP
  { path: 'apvendor', component: ApvendorComponent },
  { path: 'appurchaseinvoice', component: AppurchaseinvoiceComponent },
  { path: 'apadjustment', component: ApadvancepaymentComponent },
  //{ path: 'apadjustment', component: ApadjustmentComponent },
  { path: 'appayment', component: AppaymentComponent },
  { path: 'opmappayment', component: OpmappaymentComponent },
  { path: 'apvendbalancesummary', component: VendorbalancesummaryComponent },
  { path: 'apvendpaymentsummary', component: VendorpaymentsummaryComponent },
  { path: 'apvendvouchersummary', component: VendorvouchersummaryComponent },
  { path: 'opmapvendvouchersummary', component: OpmvendorsummaryComponent },
  { path: 'apvendinvoicesummary', component: VendorinvoicesummaryComponent },
  { path: 'apvendpaymentvoucherhist', component: VendorpaymentvouchersummaryComponent },  
  { path: 'apaginganalysis', component: ApaginganalysisComponent },
  { path: 'pdclist', component: PdclistComponent },

  //GL
  { path: 'journalvoucher', component: JournalvoucherComponent },
  { path: 'cashvoucher', component: CashvoucherComponent },
  { path: 'bankvoucher', component: BankvoucherComponent },
  //{ path: 'brsvoucher', component: BrsstatementComponent },
  { path: 'ledgeraccounts', component: BrsstatementComponent },
  //{ path: 'ledgeraccounts', component: LedgeraccountsComponent },


  //Reports
  { path: 'lederanalysis', component: LederanalysisComponent },
  { path: 'voucherreporting', component: VoucherreportingComponent },
  { path: 'chartofaccounts', component: ChartofaccountsComponent },
  { path: 'custodychartofaccounts', component: CustodyledgeranalysisComponent },
  { path: 'trialbalance', component: TrialbalanceComponent },
  { path: 'profitandlossaccount', component: ProfitandlossaccountComponent },
  { path: 'balancesheet', component: BalancesheetComponent },
  { path: 'taxreporting', component: TaxreportingComponent },
  { path: 'revenueanalysisreport', component: RevenueanalysisreportComponent },
  


  ///Products
  { path: 'unittype', component: UnittypesComponent },
  { path: 'producttype', component: ProducttypesComponent },
  { path: 'products', component: ProductsComponent },


];
;

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FinancemgsetupRoutingModule { }
