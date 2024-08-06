import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PurchasecontrolComponent } from './purchasecontrol/purchasecontrol.component';
import { PurchaseshipmentcodesComponent } from './purchaseshipmentcodes/purchaseshipmentcodes.component';
import { PurchasetermsComponent } from './purchaseterms/purchaseterms.component';
import { PurchasevendorcategoryComponent } from './purchasevendorcategory/purchasevendorcategory.component';

const routes: Routes = [/*{ path: '', component: PurchasesetupComponent }*/
  { path: 'purchasecontrol', component: PurchasecontrolComponent },
  { path: 'purchaseshipmentcodes', component: PurchaseshipmentcodesComponent },
  { path: 'purchaseterms', component: PurchasetermsComponent },
  { path: 'purchasevendorcategory', component: PurchasevendorcategoryComponent },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PurchasesetupRoutingModule { }
