import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PurchaseorderComponent } from './purchaseorder/purchaseorder.component';
import { PurchaserequestComponent } from './purchaserequest/purchaserequest.component';
import { PurchasereturnComponent } from './purchasereturn/purchasereturn.component'
import { PurchaseauthoritiesComponent } from './purchaseauthorities/purchaseauthorities.component'
import { PurchaseinvoiceComponent } from './purchaseinvoice/purchaseinvoice.component';
import { PurchasevendorComponent } from './purchasevendor/purchasevendor.component';
import { GRNComponent } from './grn/grn.component';
import { PurchaseordersummaryComponent } from './purchaseordersummary/purchaseordersummary.component';
import { VendorporeportComponent } from './vendorporeport/vendorporeport.component';
import { PoitemanalysissummaryComponent } from './poitemanalysissummary/poitemanalysissummary.component';
import { PovendoranalysissummayItemComponent } from './povendoranalysissummayitem/povendoranalysissummayitem.component';


const routes: Routes = [
  { path: 'purchaserequest', component: PurchaserequestComponent },
  { path: 'purchaseorder', component: PurchaseorderComponent },
  { path: 'purchasereturn', component: PurchasereturnComponent },
  { path: 'purchaseauthorities', component: PurchaseauthoritiesComponent },
  { path: 'Purchaseinvoice', component: PurchaseinvoiceComponent },
  { path: 'vendor', component: PurchasevendorComponent },
  { path: 'Grn', component: GRNComponent },
  { path: 'purchasesummary', component: PurchaseordersummaryComponent },
  { path: 'vendorlist', component: VendorporeportComponent },
  { path: 'itempurchasehistory', component: PoitemanalysissummaryComponent },
  { path: 'povendorpurchasehistory', component: PovendoranalysissummayItemComponent },


];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PurchasemgsetupRoutingModule { }
