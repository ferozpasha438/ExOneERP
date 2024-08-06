import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { BranchesComponent } from '../systemsetup/branches/branches.component';
import { CitiesComponent } from '../systemsetup/cities/cities.component';
import { CompanysetupComponent } from '../systemsetup/companysetup/companysetup.component';
import { CurrencyComponent } from '../systemsetup/currency/currency.component';
import { LoginandsecurityComponent } from '../systemsetup/loginandsecurity/loginandsecurity.component';
import { TaxesComponent } from '../systemsetup/taxes/taxes.component';
import { TransactionsequenceComponent } from '../systemsetup/transactionsequence/transactionsequence.component';

const routes: Routes = [
  { path: 'branchlist', component: BranchesComponent },
  { path: 'companysetup', component: CompanysetupComponent },
  { path: 'cities', component: CitiesComponent },
  { path: 'currencies', component: CurrencyComponent },
  { path: 'usersecurity', component: LoginandsecurityComponent },
  { path: 'transactionsequence', component: TransactionsequenceComponent },
  { path: 'taxlist', component: TaxesComponent },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SystemsetupRoutingModule { }
