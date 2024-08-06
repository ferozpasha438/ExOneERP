import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GetpayrollcomponentComponent } from './Setup/PayrollComponent/getpayrollcomponent/getpayrollcomponent.component';
import { GetpayrollperiodComponent } from './Setup/PayrollPeriod/getpayrollperiod/getpayrollperiod.component';
import { GetpayrollpackageComponent } from './Management/PayrollPackage/getpayrollpackage/getpayrollpackage.component';

const routes: Routes = [
  { path: 'getpayrollcomponent', component: GetpayrollcomponentComponent },
  { path: 'getpayrollperiod', component: GetpayrollperiodComponent },
  { path: 'getpayrollpackage', component: GetpayrollpackageComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PayrollRoutingModule { }
