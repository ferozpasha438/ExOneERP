import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SystemsetupRoutingModule } from './systemsetup-routing.module';
import { SharedModule } from '../sharedcomponent/shared.module';

import { BranchesComponent } from '../systemsetup/branches/branches.component';
import { CitiesComponent } from '../systemsetup/cities/cities.component';
import { CompanysetupComponent } from '../systemsetup/companysetup/companysetup.component';
import { CurrencyComponent } from '../systemsetup/currency/currency.component';
import { LoginandsecurityComponent } from '../systemsetup/loginandsecurity/loginandsecurity.component';
import { TaxesComponent } from '../systemsetup/taxes/taxes.component';
import { TransactionsequenceComponent } from '../systemsetup/transactionsequence/transactionsequence.component';
import { ApiService } from '../services/api.service';
import { AddupdatecompanysetupComponent } from '../systemsetup/addupdatecompanysetup/addupdatecompanysetup.component';
import { AddupdatecityComponent } from '../systemsetup/cities/addupdatecity.component';
import { AddupdatebranchComponent } from '../systemsetup/addupdatebranch/addupdatebranch.component';
import { TreeviewModule } from 'ngx-treeview';
import { AddingtopologyComponent } from './sharedpages/addingtopology/addingtopology.component';
import { FinancialmastersetupComponent } from './financialmastersetup/financialmastersetup.component';



@NgModule({
  declarations: [
    BranchesComponent, CitiesComponent, CompanysetupComponent, CurrencyComponent, LoginandsecurityComponent, TaxesComponent, TransactionsequenceComponent,
    FinancialmastersetupComponent,
    AddupdatebranchComponent,
    AddupdatecityComponent,
    AddupdatecompanysetupComponent,
    AddingtopologyComponent,
  ],
  imports: [
    SystemsetupRoutingModule,
    SharedModule,
    TreeviewModule.forRoot(),
  ],
  exports: [CommonModule],
  
})
export class SystemsetupModule { }
