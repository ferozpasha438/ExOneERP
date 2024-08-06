import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatRadioModule } from '@angular/material/radio';
import { DateAdapter, MatNativeDateModule, MAT_DATE_LOCALE, NativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTreeModule } from '@angular/material/tree';
import { MatSliderModule } from '@angular/material/slider';
import { NgSelectModule } from '@ng-select/ng-select';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatChipsModule } from '@angular/material/chips';
import { TranslateModule, TranslateStore } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { AutoLoaderComponent } from './autoloader.component';
import { DeleteConfirmDialogComponent } from './delete-confirm-dialog';
import { FileUploadComponent } from './fileupload.component';
import { NoDataComponent } from './nodata-component';
import { SpinnerLoaderComponent } from './spinner.component';
import { SubmitLoaderComponent, SubmitSaveLoaderComponent } from './submit.loader';
import { ValidationMessagesComponent } from './ValidationMessagesComponent';
import { DecimalPipe, DecimalPipe2 } from './decimalpipe.component';
import { ComponentloaderDirective } from './componentloader.directive';
import { LeadingZerosPipe } from './SafeHtmlPipe';
import { MatSelectModule } from '@angular/material/select';
import { ParentpayrollmgtComponent } from './parentpayrollmgt.component';



// AoT requires an exported function for factories
export function HttpLoaderChildFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

//export function createTranslateLoader(http: HttpClient) {
//  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
//}

@NgModule({
  //imports: [
  //  ToastrModule.forRoot({
  //    timeOut: 2500,
  //    positionClass: 'toast-top-right',
  //    preventDuplicates: true,
  //  }),
  //  TranslateModule.forRoot({
  //    loader: {
  //      provide: TranslateLoader,
  //      useFactory: HttpLoaderFactory,
  //      deps: [HttpClient]
  //    }
  //  }),
  //],
  declarations: [
    SubmitLoaderComponent,
    SubmitSaveLoaderComponent,
    DeleteConfirmDialogComponent,
    SpinnerLoaderComponent,
    AutoLoaderComponent,
    NoDataComponent,
    ValidationMessagesComponent,
    FileUploadComponent,
    DecimalPipe,
    DecimalPipe2,
    LeadingZerosPipe,
    ComponentloaderDirective,
    ParentpayrollmgtComponent
  ],

  imports: [
  //  CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    //BrowserModule,
    //CommonModule,
    //BrowserAnimationsModule,
    //HttpClientModule,
    //FormsModule,
    //ReactiveFormsModule,
    CommonModule,
    MatSlideToggleModule,
    // MatSnackBarModule,
    MatPaginatorModule,
    MatSortModule,
    MatTableModule,
    MatDialogModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTabsModule,
    MatTreeModule,
    MatIconModule,
    MatButtonModule,
    MatCheckboxModule,
    MatRadioModule,
    MatProgressSpinnerModule,
    MatTooltipModule,
    MatAutocompleteModule,
    MatSliderModule,
    TranslateModule,
    NgSelectModule,
    //TranslateModule.forChild(
    //  {
    //    loader: {
    //      provide: TranslateLoader,
    //      useFactory: (HttpLoaderChildFactory),
    //      deps: [HttpClient]
    //    }
    //  }),

    
    MatSelectModule,
    FormsModule,
    ReactiveFormsModule,
    MatIconModule,
    MatChipsModule
  ],
  exports: [   
    FormsModule,
    ReactiveFormsModule,
    //BrowserModule,
    //CommonModule,
    //BrowserAnimationsModule,
    //HttpClientModule,
    //FormsModule,
    //ReactiveFormsModule,
    CommonModule,
    MatSlideToggleModule,
    // MatSnackBarModule,
    MatPaginatorModule,
    MatSortModule,
    MatTableModule,
    MatDialogModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTabsModule,
    MatTreeModule,
    MatIconModule,
    MatButtonModule,
    MatCheckboxModule,
    MatRadioModule,
    MatProgressSpinnerModule,
    MatTooltipModule,
    MatAutocompleteModule,
    MatSliderModule,
    TranslateModule,
    NgSelectModule,

    SubmitLoaderComponent,
    SubmitSaveLoaderComponent,
    DeleteConfirmDialogComponent,
    SpinnerLoaderComponent,
    AutoLoaderComponent,
    NoDataComponent,
    ValidationMessagesComponent,
    FileUploadComponent,
    DecimalPipe,
    DecimalPipe2,
    LeadingZerosPipe,
    ComponentloaderDirective,
    MatSelectModule,
    FormsModule,
    ReactiveFormsModule,
    MatIconModule,
    MatChipsModule
  ]
  , providers: [
    { provide: TranslateStore },
    { provide: MAT_DATE_LOCALE, useValue: 'en-US' },
    { provide: DateAdapter, useClass: NativeDateAdapter }
   
  ]
})
export class SharedModule {
 
}
