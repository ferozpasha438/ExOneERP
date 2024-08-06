import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FinanceRoutingModule } from './finance-routing.module';
import { AccountsbranchesComponent } from './accountsbranches/accountsbranches.component';
import { AccountsbranchmappingComponent } from './accountsbranchmapping/accountsbranchmapping.component';
import { AccountscategoryComponent } from './accountscategory/accountscategory.component';
import { FinancialsetupComponent } from './financialsetup/financialsetup.component';
import { PaymentCodesComponent } from './payment-codes/payment-codes.component';
import { TaxesComponentFinance } from './taxes/taxes.component';
import { SharedModule } from '../sharedcomponent/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTreeModule } from '@angular/material/tree';
import { MatTabsModule } from '@angular/material/tabs';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatTableModule } from '@angular/material/table';
import { HttpClientModule } from '@angular/common/http';
import { AccounttypeComponent } from './sharedpages/accounttype/accounttype.component';
import { AccounsubcategoryComponent } from './sharedpages/accounsubcategory/accounsubcategory.component';
import { AccouncodesComponent } from './sharedpages/accouncodes/accouncodes.component';
import { AddupdateaccountbranchComponent } from './sharedpages/addupdateaccountbranch/addupdateaccountbranch.component';
import { BankcheckbookComponent } from './sharedpages/bankcheckbook/bankcheckbook.component';
import { ApiService } from '../services/api.service';
import { FindistributionComponent } from './sharedpages/findistribution/findistribution.component';
import { AddupdatepaycodeComponent } from './sharedpages/addupdatepaycode/addupdatepaycode.component';
import { AddupdatetaxslabComponent } from './sharedpages/addupdatetaxslab/addupdatetaxslab.component';
import { SegmentsetupComponent } from './segmentsetup/segmentsetup.component';
import { BatchsetupComponent } from './batchsetup/batchsetup.component';
import { CostallocationsetupComponent } from './costallocationsetup/costallocationsetup.component';
import { SegmenttwosetupComponent } from './segmenttwosetup/segmenttwosetup.component';




@NgModule({
  declarations: [
    AccountsbranchesComponent, AccountsbranchmappingComponent, AccountscategoryComponent,
    FinancialsetupComponent, PaymentCodesComponent, TaxesComponentFinance,
    AccounttypeComponent, AccounsubcategoryComponent, AccouncodesComponent, AddupdateaccountbranchComponent, BankcheckbookComponent,
    FindistributionComponent, AddupdatepaycodeComponent, AddupdatetaxslabComponent, SegmentsetupComponent, BatchsetupComponent, CostallocationsetupComponent, SegmenttwosetupComponent
  ],
  imports: [    
    FinanceRoutingModule,
    SharedModule,
    //FormsModule,
    //ReactiveFormsModule,
    //MatTableModule,
    //MatSlideToggleModule,
    //// MatSnackBarModule,
    //MatPaginatorModule,
    //MatSortModule,
    //MatDialogModule,
    //MatDatepickerModule,
    //MatNativeDateModule,
    //MatTabsModule,
    //MatTreeModule,
    //MatIconModule,
    //MatButtonModule,
    //MatCheckboxModule,
    //MatProgressSpinnerModule,
  ],
  exports: [CommonModule],
 
})
export class FinanceModule { }





////////import { NgModule } from '@angular/core';
////////import { CommonModule } from '@angular/common';
////////import { FinanceRoutingModule } from './finance-routing.module';
////////import { AccountsbranchesComponent } from './accountsbranches/accountsbranches.component';
////////import { AccountsbranchmappingComponent } from './accountsbranchmapping/accountsbranchmapping.component';
////////import { AccountscategoryComponent } from './accountscategory/accountscategory.component';
////////import { FinancialsetupComponent } from './financialsetup/financialsetup.component';
////////import { PaymentCodesComponent } from './payment-codes/payment-codes.component';
////////import { TaxesComponentFinance } from './taxes/taxes.component';
////////import { SharedModule } from '../sharedcomponent/shared.module';
////////import { FormsModule, ReactiveFormsModule } from '@angular/forms';
////////import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
////////import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
////////import { MatCheckboxModule } from '@angular/material/checkbox';
////////import { MatButtonModule } from '@angular/material/button';
////////import { MatIconModule } from '@angular/material/icon';
////////import { MatTreeModule } from '@angular/material/tree';
////////import { MatTabsModule } from '@angular/material/tabs';
////////import { MatNativeDateModule } from '@angular/material/core';
////////import { MatDatepickerModule } from '@angular/material/datepicker';
////////import { MatDialogModule } from '@angular/material/dialog';
////////import { MatSortModule } from '@angular/material/sort';
////////import { MatPaginatorModule } from '@angular/material/paginator';
////////import { MatSlideToggleModule } from '@angular/material/slide-toggle';
////////import { MatTableModule } from '@angular/material/table';
////////import { HttpClientModule } from '@angular/common/http';


////////@NgModule({
////////  declarations: [
////////    AccountsbranchesComponent, AccountsbranchmappingComponent, AccountscategoryComponent,
////////    FinancialsetupComponent, PaymentCodesComponent, TaxesComponentFinance
////////  ],
////////  imports: [
////////    CommonModule,    
////////    HttpClientModule,
////////    //FormsModule,
////////    //ReactiveFormsModule,
////////    //SharedModule,
////////    FinanceRoutingModule,
////////    FormsModule,
////////    ReactiveFormsModule,
////////    //BrowserModule,
////////    //CommonModule,
////////    //BrowserAnimationsModule,
////////    //HttpClientModule,
////////    //FormsModule,
////////    //ReactiveFormsModule,
////////    CommonModule,
////////    MatTableModule,
////////    MatSlideToggleModule,
////////    // MatSnackBarModule,
////////    MatPaginatorModule,
////////    MatSortModule,
////////    MatDialogModule,
////////    MatDatepickerModule,
////////    MatNativeDateModule,
////////    MatTabsModule,
////////    MatTreeModule,
////////    MatIconModule,
////////    MatButtonModule,
////////    MatCheckboxModule,
////////    MatProgressSpinnerModule,
////////  ],
////////  exports: [CommonModule],
////////})
////////export class FinanceModule { }
