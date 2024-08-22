import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './account/login.component';
import { Login2Component } from './account/Login/login2/login2.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { HomeComponent } from './home/home.component';
import { Home2Component } from './home2/home2.component';


//import { BranchesComponent } from './systemsetup/branches/branches.component';
//import { CompanysetupComponent } from './systemsetup/companysetup/companysetup.component';
//import { CitiesComponent } from './systemsetup/cities/cities.component';
//import { CurrencyComponent } from './systemsetup/currency/currency.component';
//import { LoginandsecurityComponent } from './systemsetup/loginandsecurity/loginandsecurity.component';
//import { TransactionsequenceComponent } from './systemsetup/transactionsequence/transactionsequence.component';
//import { TaxesComponent } from './systemsetup/taxes/taxes.component';


const routes: Routes = [
  //{ path: '', component: LoginComponent, pathMatch: 'full' },
   { path: '', component: Login2Component, pathMatch: 'full' },

  //{ path: 'login', component: LoginComponent },
  //{ path: 'home', component: HomeComponent },
  {
    path: 'dashboard', component: DashboardComponent,
    children: [

      // { path: '', component: HomeComponent },
      { path: '', component: Home2Component },


      //ADM
      //{ path: 'branchlist', component: BranchesComponent },
      //{ path: 'companysetup', component: CompanysetupComponent },
      //{ path: 'cities', component: CitiesComponent },
      //{ path: 'currencies', component: CurrencyComponent },
      //{ path: 'usersecurity', component: LoginandsecurityComponent },
      //{ path: 'transactionsequence', component: TransactionsequenceComponent },
      //{ path: 'taxlist', component: TaxesComponent },      


      //SySTEM
      { path: 'system', loadChildren: () => import('./SystemSetup/systemsetup.module').then(m => m.SystemsetupModule) },

      //FI
      { path: 'fin', loadChildren: () => import('./Finance/financemgsetup.module').then(m => m.FinancemgsetupModule) },

      //FIN
      { path: 'finance', loadChildren: () => import('./Financemgt/finance.module').then(m => m.FinanceModule) },

      //INVT
      { path: 'inventory', loadChildren: () => import('./Inventorysetup/inventorysetup.module').then(m => m.InventorysetupModule) },

      //INVENTORYMGT
      { path: 'invt', loadChildren: () => import('./Inventorymgt/inventorymgsetup.module').then(m => m.InventorymgsetupModule) },

      //SALESSETUP
      { path: 'sales', loadChildren: () => import('./Sales/salessetup.module').then(m => m.SalessetupModule) },

      //SALESMGT
      { path: 'salesmgt', loadChildren: () => import('./Salesmgt/salesmgt.module').then(m => m.SalesmgtModule) },

      //PUR
      { path: 'purchase', loadChildren: () => import('./Purchasesetup/purchasesetup.module').then(m => m.PurchasesetupModule) },

      //PURMGT
      { path: 'purc', loadChildren: () => import('./Purchasemgt/purchasemgsetup.module').then(m => m.PurchasemgsetupModule) },

      //OPERATION
      { path: 'operation', loadChildren: () => import('./Operationalmgt/operationalmgt.module').then(m => m.OperationalmgtModule) },

      //SALESANDDISTRIBUTION
      { path: 'snd', loadChildren: () => import('./sales-and-distribution/sales-and-distribution.module').then(m => m.SalesAndDistributionModule) },

      //SCHOOLMGT
      { path: 'school', loadChildren: () => import('./Schoolmgt/schoolmgt.module').then(m => m.SchoolmgtModule) },
      //FleetMGT
      { path: 'fleet', loadChildren: () => import('./Fleetmgt/fleetmgt.module').then(m => m.FleetmgtModule) },

      //HRM Admin
      { path: 'hrmadminsetup', loadChildren: () => import('./hrmadminsetup/hrmadminsetup.module').then(m => m.HrmadminsetupModule) },
      { path: 'humanresource', loadChildren: () => import('./humanresource/humanresource.module').then(m => m.HumanresourceModule) },
      { path: 'timeandattendance', loadChildren: () => import('./timeandattendance/timeandattendance.module').then(m => m.TimeandattendanceModule) },
      { path: 'payroll', loadChildren: () => import('./payroll/payroll.module').then(m => m.PayrollModule) },
      
    ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
