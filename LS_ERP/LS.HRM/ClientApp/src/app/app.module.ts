import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';

import { LoginComponent } from './account/login.component';
import { PaymentComponent } from './account/payment.component';

import { AuthorizeInterceptor } from './api-authorization/authorize.interceptor';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

//import { BranchesComponent } from './systemsetup/branches/branches.component';
//import { CitiesComponent } from './systemsetup/cities/cities.component';
//import { CompanysetupComponent } from './systemsetup/companysetup/companysetup.component';
//import { CurrencyComponent } from './systemsetup/currency/currency.component';
//import { LoginandsecurityComponent } from './systemsetup/loginandsecurity/loginandsecurity.component';
//import { TaxesComponent } from './systemsetup/taxes/taxes.component';
//import { TransactionsequenceComponent } from './systemsetup/transactionsequence/transactionsequence.component';
//import { FinancialmastersetupComponent } from './SystemSetup/financialmastersetup/financialmastersetup.component';
//import { AddupdatebranchComponent } from './systemsetup/addupdatebranch/addupdatebranch.component';
//import { AddupdatecityComponent } from './systemsetup/cities/addupdatecity.component';
//import { AddupdatecompanysetupComponent } from './systemsetup/addupdatecompanysetup/addupdatecompanysetup.component';


import { DashboardComponent } from './dashboard/dashboard.component';
import { HomeComponent } from './home/home.component';
import { LeftMenuComponent } from './left-menu/left-menu.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { SubmitLoaderComponent, SubmitSaveLoaderComponent } from './sharedcomponent/submit.loader';
import { MatNativeDateModule } from '@angular/material/core';
import { DeleteConfirmDialogComponent } from './sharedcomponent/delete-confirm-dialog';


import { ValidationMessagesComponent } from './sharedcomponent/ValidationMessagesComponent';
import { SpinnerLoaderComponent } from './sharedcomponent/spinner.component';
import { NoDataComponent } from './sharedcomponent/nodata-component';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateLoader, TranslateModule, TranslateService, TranslateStore } from '@ngx-translate/core';
import { SharedModule } from './sharedcomponent/shared.module';

import { TreeviewModule } from 'ngx-treeview';
import { IModuleTranslationOptions, ModuleTranslateLoader } from '@larscom/ngx-translate-module-loader';
import { Injector } from '@angular/core';
import { LOCATION_INITIALIZED } from '@angular/common';
import { APP_INITIALIZER } from '@angular/core';
import { NgxPrintModule } from 'ngx-print';
import { NgApexchartsModule } from "ng-apexcharts";
import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';
import { TestExampleComponent } from './Operationalmgt/test-example/test-example.component';
import { Login2Component } from './account/Login/login2/login2.component';
//import { Login2Component } from './account/login2/login2.component';



// AoT requires an exported function for factories
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, 'assets/i18n/', '.json');
}


//export function ModuleHttpLoaderFactory(http: HttpClient) {
//  const baseTranslateUrl = './assets/i18n';
//  const options: IModuleTranslationOptions = {
//    modules: [
//      // Fetches file at e.g. /assets/i18n/en.json
//      { baseTranslateUrl },
//      // Fetches file at e.g. /assets/i18n/non-lazy/en.json
//     // { baseTranslateUrl, moduleName: 'non-lazy', namespace: 'FEATURE_FOO' }
//    ]
//  };
//  return new ModuleTranslateLoader(http, options);
//}

//export function ApplicationInitializerFactory(
//  translate: TranslateService,
//  injector: Injector
//) {
//  return async () => {
//    await injector.get(LOCATION_INITIALIZED, Promise.resolve(null));

//    const deaultLang = 'en';
//    translate.addLangs(['en', 'ar']);
//    translate.setDefaultLang(deaultLang);
//    try {
//      await translate.use(deaultLang).toPromise();
//    } catch (err) {
//      console.log(err);
//    }

//  };
//}

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent,
    PaymentComponent,
    DashboardComponent,
    LeftMenuComponent,
    TestExampleComponent,
    Login2Component,
    //Login2Component,
    //SubmitLoaderComponent,
    //SubmitSaveLoaderComponent,
    //DeleteConfirmDialogComponent,
    //SpinnerLoaderComponent,
    //NoDataComponent,
    //ValidationMessagesComponent,

    //CompanysetupComponent,
    //BranchesComponent,
    //CitiesComponent,
    //CurrencyComponent,
    //LoginandsecurityComponent,
    //TaxesComponent,
    //TransactionsequenceComponent,


    //FinancialmastersetupComponent,
    //AddupdatebranchComponent,
    //AddupdatecityComponent,
    //AddupdatecompanysetupComponent,



    //AccountsbranchesComponent, AccountsbranchmappingComponent, AccountscategoryComponent,
    //FinancialsetupComponent, PaymentCodesComponent, TaxesComponentFinance,



  ],
  entryComponents: [
    DeleteConfirmDialogComponent,
    SpinnerLoaderComponent
  ],
  imports: [
    // BrowserModule,    
    BrowserAnimationsModule,
    NgxPrintModule,
    NgApexchartsModule,
    NgxMaterialTimepickerModule,

    //HttpClientModule,
    //FormsModule,
    //ReactiveFormsModule,
    AppRoutingModule,
    ToastrModule.forRoot({
      timeOut: 2500,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
    }),

    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }),
    //TranslateModule.forRoot({
    //  loader: {
    //    provide: TranslateLoader,
    //    useFactory: HttpLoaderFactory,
    //    deps: [HttpClient]
    //  }
    //}),

    //TreeviewModule.forRoot(),

    SharedModule,
    //    FinanceModule,
    // MatTableModule,
    // MatSlideToggleModule,
    //// MatSnackBarModule,
    // MatPaginatorModule,
    // MatSortModule,
    // MatDialogModule,
    // MatDatepickerModule,
    // MatNativeDateModule,
    // MatTabsModule,
    // MatTreeModule,
    // MatIconModule,
    // MatButtonModule,
    // MatCheckboxModule,
    // MatProgressSpinnerModule,
    
  ],
  providers: [
    //{
    //  provide: APP_INITIALIZER,
    //  useFactory: ApplicationInitializerFactory,
    //  deps: [TranslateService, Injector],
    //  multi: true
    //},    
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
    
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
