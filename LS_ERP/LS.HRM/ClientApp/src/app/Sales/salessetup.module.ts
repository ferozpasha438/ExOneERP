import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SalessetupRoutingModule } from './salessetup-routing.module';
import { CustomercategoryComponent } from './customercategory/customercategory.component';
import { CustomerratingComponent } from './customerrating/customerrating.component';
import { SalescontrolComponent } from './salescontrol/salescontrol.component';
import { SalesshipmentcodesComponent } from './salesshipmentcodes/salesshipmentcodes.component';
import { SalestermsComponent } from './salesterms/salesterms.component';
import { SharedModule } from '../sharedcomponent/shared.module';
import { AddupdateshipmentcodeComponent } from './sharedpages/addupdateshipmentcode/addupdateshipmentcode.component';
import { AddupdatesalestermsComponent } from './sharedpages/addupdatesalesterms/addupdatesalesterms.component';
import { ApiService } from '../services/api.service';
import { AddupdatecustcategoryComponent } from './sharedpages/addupdatecustcategory/addupdatecustcategory.component';


@NgModule({
  declarations: [
    CustomercategoryComponent,
    CustomerratingComponent,
    SalescontrolComponent,
    SalesshipmentcodesComponent,
    SalestermsComponent,
    AddupdateshipmentcodeComponent,
    AddupdatesalestermsComponent,
    AddupdatecustcategoryComponent
  ],
  imports: [    
    SalessetupRoutingModule,
    SharedModule
  ],
  exports: [CommonModule],
  
})
export class SalessetupModule { }
