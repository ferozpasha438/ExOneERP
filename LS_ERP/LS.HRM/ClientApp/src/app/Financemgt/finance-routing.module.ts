import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccountsbranchesComponent } from './accountsbranches/accountsbranches.component';
import { AccountsbranchmappingComponent } from './accountsbranchmapping/accountsbranchmapping.component';
import { AccountscategoryComponent } from './accountscategory/accountscategory.component';
import { BatchsetupComponent } from './batchsetup/batchsetup.component';
import { CostallocationsetupComponent } from './costallocationsetup/costallocationsetup.component';
import { FinancialsetupComponent } from './financialsetup/financialsetup.component';
import { PaymentCodesComponent } from './payment-codes/payment-codes.component';
import { SegmentsetupComponent } from './segmentsetup/segmentsetup.component';
import { SegmenttwosetupComponent } from './segmenttwosetup/segmenttwosetup.component';
import { TaxesComponentFinance } from './taxes/taxes.component';

const routes: Routes = [
  { path: 'accountsbranches', component: AccountsbranchesComponent },
  { path: 'accountsbranchmapping', component: AccountsbranchmappingComponent },
  { path: 'accountscategory', component: AccountscategoryComponent },  
  { path: 'financialsetup', component: FinancialsetupComponent },
  { path: 'paymentcodes', component: PaymentCodesComponent },
  { path: 'taxes', component: TaxesComponentFinance },
  { path: 'segmentsetup', component: SegmentsetupComponent },
  { path: 'segmenttwosetup', component: SegmenttwosetupComponent },
  { path: 'batchsetup', component: BatchsetupComponent },
  { path: 'costallocationsetup', component: CostallocationsetupComponent },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FinanceRoutingModule { }
