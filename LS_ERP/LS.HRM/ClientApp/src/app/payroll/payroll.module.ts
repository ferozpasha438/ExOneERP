import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PayrollRoutingModule } from './payroll-routing.module';
import { SharedModule } from '../sharedcomponent/shared.module';
import { GetpayrollcomponentComponent } from './Setup/PayrollComponent/getpayrollcomponent/getpayrollcomponent.component';
import { GetpayrollperiodComponent } from './Setup/PayrollPeriod/getpayrollperiod/getpayrollperiod.component';
import { AddupdatepayrollperiodComponent } from './Setup/PayrollPeriod/addupdatepayrollperiod/addupdatepayrollperiod.component';
import { AddupdatepayrollcomponentComponent } from './Setup/PayrollComponent/addupdatepayrollcomponent/addupdatepayrollcomponent.component';
import { GetpayrollpackageComponent } from './Management/PayrollPackage/getpayrollpackage/getpayrollpackage.component';
import { AddupdatepayrollpackageComponent } from './Management/PayrollPackage/addupdatepayrollpackage/addupdatepayrollpackage.component';


@NgModule({
  declarations: [
    GetpayrollcomponentComponent,
    GetpayrollperiodComponent,
    AddupdatepayrollperiodComponent,
    AddupdatepayrollcomponentComponent,
    GetpayrollpackageComponent,
    AddupdatepayrollpackageComponent,
  ],
  imports: [
    CommonModule,
    PayrollRoutingModule,
    SharedModule
  ]
})
export class PayrollModule { }
