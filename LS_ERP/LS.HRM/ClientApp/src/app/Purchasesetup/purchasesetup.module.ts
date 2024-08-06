import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from '../sharedcomponent/shared.module';
import { PurchasecontrolComponent } from './purchasecontrol/purchasecontrol.component';
import { PurchasesetupRoutingModule } from './purchasesetup-routing.module';
import { PurchaseshipmentcodesComponent } from './purchaseshipmentcodes/purchaseshipmentcodes.component';
import { PurchasetermsComponent } from './purchaseterms/purchaseterms.component';
import { PurchasevendorcategoryComponent } from './purchasevendorcategory/purchasevendorcategory.component';
import { AddupdatepurchaceshipmentcodeComponent } from './sharedpages/addupdatepurchaceshipmentcode/addupdatepurchaceshipmentcode.component';
import { AddupdatepurchasevendorcategoryComponent } from './sharedpages/addupdatepurchasevendorcategory/addupdatepurchasevendorcategory.component';
import { AddupdatepurchasevendortermcodeComponent } from './sharedpages/addupdatepurchasevendortermcode/addupdatepurchasevendortermcode.component';


@NgModule({
  declarations: [
    PurchasecontrolComponent,
    PurchaseshipmentcodesComponent,
    PurchasetermsComponent,
    PurchasevendorcategoryComponent,
    AddupdatepurchaceshipmentcodeComponent,
    AddupdatepurchasevendorcategoryComponent,
    AddupdatepurchasevendortermcodeComponent
  ],
  imports: [   
    PurchasesetupRoutingModule,
    SharedModule
  ],
  exports: [CommonModule],
})
export class PurchasesetupModule { }
