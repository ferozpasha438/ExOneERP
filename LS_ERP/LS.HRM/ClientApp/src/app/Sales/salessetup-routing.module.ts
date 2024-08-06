import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomercategoryComponent } from './customercategory/customercategory.component';
import { CustomerratingComponent } from './customerrating/customerrating.component';
import { SalescontrolComponent } from './salescontrol/salescontrol.component';
import { SalesshipmentcodesComponent } from './salesshipmentcodes/salesshipmentcodes.component';
import { SalestermsComponent } from './salesterms/salesterms.component';

const routes: Routes = [
  { path: 'customercategory', component: CustomercategoryComponent },
  { path: 'customerrating', component: CustomerratingComponent },
  { path: 'salescontrol', component: SalescontrolComponent },
  { path: 'salesterms', component: SalestermsComponent },
  { path: 'salesshipmentcodes', component: SalesshipmentcodesComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SalessetupRoutingModule { }
