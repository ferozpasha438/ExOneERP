import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from '../sharedcomponent/shared.module';
import { PurchasemgsetupRoutingModule } from './purchasemgsetup-routing.module';
import { PurchaseorderComponent } from './purchaseorder/purchaseorder.component';
import { PurchaserequestComponent } from './purchaserequest/purchaserequest.component';
import { AddupdatepurchaserequestComponent } from './sharedpages/addupdatepurchaserequest/addupdatepurchaserequest.component';
import { PurchasereturnComponent } from './purchasereturn/purchasereturn.component';
import { PurchaseauthoritiesComponent } from './purchaseauthorities/purchaseauthorities.component';
import { AddupdateauthoritiesComponent } from './purchaseauthorities/addupdateauthorities/addupdateauthorities.component';
import { ApprovaldialogwindowsComponent } from './approvaldialogwindows/approvaldialogwindows.component';
import { PurchaseinvoiceComponent } from './purchaseinvoice/purchaseinvoice.component';
import { AddupdatepurchaseinvoiceComponent } from './sharedpages/addupdatepurchaseinvoice/addupdatepurchaseinvoice.component';
import { EditpurchaseinvoiceComponent } from './sharedpages/editpurchaseinvoice/editpurchaseinvoice.component';
import { SettlementmodeComponent } from './sharedpages/settlementmode/settlementmode.component';
import { AddupdatepurchasevendorComponent } from './sharedpages/addupdatepurchasevendor/addupdatepurchasevendor.component';
import { PurchasevendorComponent } from './purchasevendor/purchasevendor.component';
import { AddupdatepurchaseorderComponent } from './sharedpages/addupdatepurchaseorder/addupdatepurchaseorder.component';
import { AddupdatepurchasereturnComponent } from './sharedpages/addupdatepurchasereturn/addupdatepurchasereturn.component';
import { AddupdatemultiplegrnComponent } from './sharedpages/addupdatemultiplegrn/addupdatemultiplegrn.component';
import { GRNComponent } from './grn/grn.component';
import { PoprintingpageComponent } from './sharedpages/poprintingpage/poprintingpage.component';
import { PoprintingformatonepageComponent } from './sharedpages/poprintingformatonepage/poprintingformatonepage.component';
import { PoprintingformattwopageComponent } from './sharedpages/poprintingformattwopage/poprintingformattwopage.component';
import { PoprintingformatthreepageComponent } from './sharedpages/poprintingformatthreepage/poprintingformatthreepage.component';
import { PoprintingformatfourpageComponent } from './sharedpages/poprintingformatfourpage/poprintingformatfourpage.component';
import { PurchaseordersummaryComponent } from './purchaseordersummary/purchaseordersummary.component';
import { VendorporeportComponent } from './vendorporeport/vendorporeport.component';
import { PoitemanalysissummaryComponent } from './poitemanalysissummary/poitemanalysissummary.component';
import { PovendoranalysissummayItemComponent } from './povendoranalysissummayitem/povendoranalysissummayitem.component';
import { AddupdateinvexpbatchComponent } from './sharedpages/addupdateinvexpbatch/addupdateinvexpbatch.component';
import { AddupdateinvserialbatchComponent } from './sharedpages/addupdateinvserialbatch/addupdateinvserialbatch.component';
import { AddupdateinvspecificationComponent } from './sharedpages/addupdateinvspecification/addupdateinvspecification.component';
import { AddupdateinvitemexpserialbatchComponent } from './sharedpages/addupdateinvitemexpserialbatch/addupdateinvitemexpserialbatch.component';

 
 


@NgModule({
  declarations: [    
    PurchaseorderComponent, PurchaserequestComponent, AddupdatepurchaserequestComponent, PurchasereturnComponent, PurchaseauthoritiesComponent,
    AddupdateauthoritiesComponent, ApprovaldialogwindowsComponent, PurchaseinvoiceComponent, AddupdatepurchaseinvoiceComponent,
    EditpurchaseinvoiceComponent, SettlementmodeComponent, PurchasevendorComponent, AddupdatepurchasevendorComponent, AddupdatepurchaseorderComponent,
    AddupdatepurchasereturnComponent, AddupdatemultiplegrnComponent, GRNComponent, PoprintingpageComponent, PoprintingformatonepageComponent,
    PoprintingformattwopageComponent, PoprintingformatthreepageComponent, PoprintingformatfourpageComponent, PurchaseordersummaryComponent, VendorporeportComponent,
    PoitemanalysissummaryComponent, PovendoranalysissummayItemComponent, AddupdateinvexpbatchComponent, AddupdateinvserialbatchComponent, AddupdateinvspecificationComponent, AddupdateinvitemexpserialbatchComponent
  ],
  imports: [    
    PurchasemgsetupRoutingModule,
    SharedModule,
  ],
  exports: [CommonModule,],
})
export class PurchasemgsetupModule { }
