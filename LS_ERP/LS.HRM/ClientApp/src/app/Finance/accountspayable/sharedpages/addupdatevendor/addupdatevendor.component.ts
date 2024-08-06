
import { HttpClient } from '@angular/common/http';
import { Component, OnChanges, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { from, Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, mergeMap, startWith, switchMap } from 'rxjs/operators';
import { ValidationService } from '../../../../sharedcomponent/ValidationService';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../../models/MenuItemListDto';
import { ApiService } from '../../../../services/api.service';
//import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { DBOperation } from '../../../../services/utility.constants';
import { UtilityService } from '../../../../services/utility.service';
import { ParentOptMgtComponent } from '../../../../sharedcomponent/parentoptmgt.component';

@Component({
  selector: 'app-addupdatevendor',
  templateUrl: './addupdatevendor.component.html',
  styles: [
  ]
})
export class AddupdatevendorComponent implements OnInit {

  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
  custCityCode1: string;
  custCityCode2: string;
  companyLogo: string='';

  cityList1: Array<CustomSelectListItem> = [];
  cityList2: Array<CustomSelectListItem> = [];
  userTypes: Array<CustomSelectListItem> = [];
  ratingList: Array<CustomSelectListItem> = [];

  categoryCodeControl = new FormControl('', Validators.required);
  salesTermsCodeControl = new FormControl('', Validators.required);
  filteredCategoryCodes: Observable<Array<CustomSelectListItem>>;
  filteredSalesTermsCodes: Observable<Array<CustomSelectListItem>>;


  custARAcControl = new FormControl('', Validators.required);
  filteredcustARAc: Observable<Array<CustomSelectListItem>>;

  custArAcCodeControl = new FormControl('', Validators.required);
  filteredcustArAcCode: Observable<Array<CustomSelectListItem>>;

  custDefExpAcCodeControl = new FormControl('', Validators.required);
  filteredcustDefExpAcCode: Observable<Array<CustomSelectListItem>>;

  custARAdjAcCodeControl = new FormControl('', Validators.required);
  filteredcustARAdjAcCode: Observable<Array<CustomSelectListItem>>;

  custARDiscAcCodeControl = new FormControl('', Validators.required);
  filteredcustARDiscAcCode: Observable<Array<CustomSelectListItem>>;


  readonly: string = "";
  canAutoGenCustCode: boolean = false;
  isDataLoading: boolean = false;
  isArab: boolean = false;

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<AddupdatevendorComponent>) {
   
    this.filteredCategoryCodes = this.categoryCodeControl.valueChanges.pipe(
      startWith(''),
      debounceTime(utilService.autoDelay()),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isDataLoading = true;


        return this.filterCategoryCodes(val || '')
      })
    );

    this.filteredSalesTermsCodes = this.salesTermsCodeControl.valueChanges.pipe(
      startWith(''),
      debounceTime(utilService.autoDelay()),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isDataLoading = true;
        return this.filterSalesTermsCodes(val || '')
      })
    );

    this.filteredcustARAc = this.custARAcControl.valueChanges.pipe(
      startWith(''),
      debounceTime(utilService.autoDelay()),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isDataLoading = true;
        return this.filterAccountCodes(val || '')
      })
    );

    this.filteredcustArAcCode = this.custArAcCodeControl.valueChanges.pipe(
      startWith(''),
      debounceTime(utilService.autoDelay()),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isDataLoading = true;
        return this.filterAccountCodes(val || '')
      })
    );
    this.filteredcustDefExpAcCode = this.custDefExpAcCodeControl.valueChanges.pipe(
      startWith(''),
      debounceTime(utilService.autoDelay()),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isDataLoading = true;
        return this.filterAccountCodes(val || '')
      })
    );
    this.filteredcustARAdjAcCode = this.custARAdjAcCodeControl.valueChanges.pipe(
      startWith(''),
      debounceTime(utilService.autoDelay()),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isDataLoading = true;
        return this.filterAccountCodes(val || '')
      })
    );
    this.filteredcustARDiscAcCode = this.custARDiscAcCodeControl.valueChanges.pipe(
      startWith(''),
      debounceTime(utilService.autoDelay()),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isDataLoading = true;
        return this.filterAccountCodes(val || '')
      })
    );

  }

  ngOnInit(): void {

    this.isArab = this.utilService.isArabic();
    this.companyLogo = this.utilService.logoUrl();
    this.setForm();
    this.canAutoGenerateVendCode();
    this.loadCities();
    if (this.id > 0) {
      this.readonly = "readonly";
      this.setEditForm();



    }
  }
  closeModel() {
    this.dialogRef.close();
  }
 

  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      //'': ['', Validators.required],
      //   'stateone': [''],
      //   'countryone': [''],
      //   'statetwo': [''],
      //   'countrytwo': [''],
      'vendCode': [''],
      'vendName': ['', Validators.required],
      'vendArbName': ['', Validators.required],
      'vendAlias': ['', Validators.required],
      //   'vendType': ['',Validators.required],
      'vatNumber': ['', Validators.required],
      'vendRating': ['0', Validators.required],
      'vendDiscount': ['0'],
      'vendCrLimit': ['0'],
      'vendPoRep': [''],
      'vendPoArea': [''],
      'vendARAc': [''],
      'vendLastPaidDate': ['', Validators.required],
      'vendLastPoDate': ['', Validators.required],
      'vendLastPayAmt': ['0', Validators.required],
      'vendAddress1': ['', Validators.required],
      'crNumber': [''],
      'iban': [''],
      //   'vendCityId1': [''],

      'vendMobile1': ['', Validators.compose([Validators.required, this.validationService.mobileValidator])],
      'vendPhone1': ['', Validators.compose([Validators.required, this.validationService.mobileValidator])],
      'vendEmail1': ['', Validators.compose([Validators.required, Validators.email])],
      'vendContact1': ['', Validators.compose([Validators.required])],

      'vendAddress2': [''],
      //'vendCityId2': [''],
      'vendMobile2': ['', Validators.compose([this.validationService.mobileOptionalValidator])],
      'vendPhone2': ['', Validators.compose([this.validationService.mobileOptionalValidator])],
      'vendEmail2': ['', Validators.compose([Validators.email])],
      'vendContact2': [''],
      //   'vendUDF1': [''],
      //   'vendUDF2': [''],
      //   'vendUDF3': [''],
      //   'vendAllowCrsale': [false],
      //   'vendAlloCrOverride': [false],
      //   'vendOnHold': [false],
      //   'vendAlloChkPay': [false],
      //   'vendSetPriceLevel': [false],
      //   'vendIsVendor': [false],
      //   'vendArAcBranch': [false],
      //   'vendArAcCode': ['', Validators.required],
      //   'vendDefExpAcCode': ['',Validators.required],
      //   'vendARAdjAcCode': ['',Validators.required],
      //   'vendARDiscAcCode': ['',Validators.required],
      //   'vendCatCode': [''],
      //   'poTermsCode': [''],
      //   'vendCityCode1': [''],
      //   'vendCityCode2': [''],
      'isActive': [true],
      //   'vendOutStandBal': [''],
      //   'vendAvailCrLimit': [''],










      'stateone': [''],
      'countryone': [''],
      'statetwo': [''],
      'countrytwo': [''],
      //   'vendCode': [''],
      //  'vendName': [''],
      //  'vendArbName': [''],
      // 'vendAlias': [''],
      // 'vendRating': [''],
      'vendType': ['', Validators.required],
      'vendCatCode': [''],
      'poTermsCode': [''],
      // 'vendDiscount': [''],
      // 'vendCrLimit': [''],
      // 'vendPoRep': [''],
      //'vendPoArea': [''],
      //'vendARAc': [''],
      //'vendLastPaidDate': [''],
      //'vendLastPoDate': [''],
      //'vendLastPayAmt': [''],
      // 'vendAddress1': [''],
      'vendCityCode1': ['', Validators.required],

      //'vendMobile1': [''],
      //'vendPhone1': [''],
      //'vendEmail1': [''],
      //'vendContact1': [''],
      //'vendAddress2': [''],
      'vendCityCode2': [''],

      //'vendMobile2': [''],
      //'vendPhone2': [''],
      //'vendEmail2': [''],
      //'vendContact2': [''],
      'vendUDF1': [''],
      'vendUDF2': [''],
      'vendUDF3': [''],
      //'isActive': [true],
      'vendAllowCrsale': [false],
      'vendAlloCrOverride': [false],
      'vendOnHold': [false],
      'vendAlloChkPay': [false],
      'vendSetPriceLevel': [false],
      //'vendPriceLevel': [100],
      'vendIsVendor': [false],
      'vendArAcBranch': [false],
      'vendArAcCode': [''],
      'vendDefExpAcCode': [''],
      'vendARAdjAcCode': [''],
      'vendARDiscAcCode': [''],

      'vendOutStandBal': [0],
      'vendAvailCrLimit': ['0']



    });

  }

  submit() {
    if (this.id > 0)
      this.form.value['id'] = this.id;

    let custCatCode = this.categoryCodeControl.value as string;
    let salesTermsCode = this.salesTermsCodeControl.value as string;
    let custARAc = this.custARAcControl.value as string;

    let custARDiscAcCode = this.custARDiscAcCodeControl.value as string;
    let custDefExpAcCode = this.custDefExpAcCodeControl.value as string;
    let custARAdjAcCode = this.custARAdjAcCodeControl.value as string;
    let custArAcCode = this.custArAcCodeControl.value as string;    

    if (this.utilService.hasValue(custCatCode)) {
      this.form.value['vendCatCode'] = this.utilService.removeSqueres(custCatCode);
      this.categoryCodeControl.setValue(custCatCode);
    }
    else {
      console.log("vendCatCode-");
    }

    if (this.utilService.hasValue(salesTermsCode)) {

      this.form.value['poTermsCode'] = this.utilService.removeSqueres(salesTermsCode);
      this.salesTermsCodeControl.setValue(salesTermsCode);
    }
    else {
      console.log("poTermsCode-");
    }



    if (this.utilService.hasValue(custARAc)) {
      this.form.value['vendARAc'] = this.utilService.removeSqueres(custARAc);
      this.custARAcControl.setValue(custARAc);

    }
    else {
      console.log("vendARAc-");
    }

    if (this.utilService.hasValue(custArAcCode)) {
      this.form.value['vendArAcCode'] = this.utilService.removeSqueres(custArAcCode);
      this.custArAcCodeControl.setValue(custArAcCode);

    }
    else {
      console.log("vendArAcCode-");
    }

    if (this.utilService.hasValue(custDefExpAcCode)) {
      this.form.value['vendDefExpAcCode'] = this.utilService.removeSqueres(custDefExpAcCode);
      this.custDefExpAcCodeControl.setValue(custDefExpAcCode);

    }
    else {
      console.log("custDefExpAcCode-");
    }
    if (this.utilService.hasValue(custARAdjAcCode)) {
      this.form.value['vendARAdjAcCode'] = this.utilService.removeSqueres(custARAdjAcCode);
      this.custARAdjAcCodeControl.setValue(custARAdjAcCode);

    }
    else {
      console.log("custARAdjAcCode-");
    }
    if (this.utilService.hasValue(custARDiscAcCode)) {
      this.form.value['vendARDiscAcCode'] = this.utilService.removeSqueres(custARDiscAcCode);
      this.custARDiscAcCodeControl.setValue(custARDiscAcCode);

    }
    else {
      console.log("custARDiscAcCode-");
    }

    if (this.form.valid) {

      if (!this.utilService.hasValue(custARDiscAcCode) && !this.utilService.hasValue(custDefExpAcCode)
        && !this.utilService.hasValue(custARAdjAcCode) && !this.utilService.hasValue(custArAcCode)) {
        this.notifyService.showError('Select Account Mapping');
        return;
      }

      this.apiService.post('vendorMaster', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          // this.reset();
          this.dialogRef.close(true);
        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
          });

    }
    else
      this.utilService.FillUpFields();

  }

  setEditForm() {
    this.apiService.get('vendorMaster', this.id).subscribe(res => {
      if (res) {
        this.categoryCodeControl.setValue(res['vendCatCode']);
        this.salesTermsCodeControl.setValue(res['poTermsCode']);
        this.custARAcControl.setValue(res['vendARAc']);
        this.custDefExpAcCodeControl.setValue(res['vendDefExpAcCode']);
        this.custARAdjAcCodeControl.setValue(res['vendARAdjAcCode']);
        this.custArAcCodeControl.setValue(res['vendArAcCode']);
        this.custARDiscAcCodeControl.setValue(res['vendARDiscAcCode']);

        this.form.patchValue(res);


        this.apiService.getall('City/getStateCountrybyCityCode/' + res['vendCityCode1']).subscribe(res => {
          this.form.patchValue({ 'stateone': res['stateName'], 'countryone': res['countryName'] });
        });

        this.apiService.getall('City/getStateCountrybyCityCode/' + res['vendCityCode2']).subscribe(res => {
          this.form.patchValue({ 'statetwo': res['stateName'], 'countrytwo': res['countryName'] });
        });

        this.form.patchValue({ 'id': 0 });

      }
    });



  }






  loadCities() {
    this.apiService.getall('City/getCitiesSelectList').subscribe(res => {
      this.cityList1 = res;
      this.cityList2 = res;
    });

    this.apiService.getall('userType/getUserTypeSelectList').subscribe(res => {
      this.userTypes = res;
    });

    this.apiService.getall('vat/getSelectRatingList').subscribe(res => {
      this.ratingList = res;
    });

  }

  canAutoGenerateVendCode() {
    this.apiService.getall('purchaseConfig/canAutoGenerateVendCode').subscribe(res => {
      this.canAutoGenCustCode = res;
    });
  }

  getStateCountrybyCityCode1(event: any) {
    const id = event.target.value;
    this.apiService.getall('City/getStateCountrybyCityCode/' + id).subscribe(res => {
      this.form.patchValue({ 'vendCityCode1': res['cityCode'], 'stateone': res['stateName'], 'countryone': res['countryName'] });
    });



  }
  getStateCountrybyCityCode2(event: any) {
    const id = event.target.value;
    this.apiService.getall('City/getStateCountrybyCityCode/' + id).subscribe(res => {
      this.form.patchValue({ 'vendCityCode2': res['cityCode'], 'statetwo': res['stateName'], 'countrytwo': res['countryName'] });
    });
  }

  reset() {
    this.form.controls['vendName'].setValue('');
  }

  filterCategoryCodes(val: string): Observable<Array<CustomSelectListItem>> {
    return this.apiService.getall(`vendorCategory/getSelectVendorCategoryCodeList?search=${val}`)
      .pipe(
        map(response => {
          const res = response as Array<CustomSelectListItem>;
          this.isDataLoading = false;
          /* if (res.length == 0) { this.categoryCodeControl.setValue('');}*/

          return res;
        })
      )
  }
  filterSalesTermsCodes(val: string): Observable<Array<CustomSelectListItem>> {
    return this.apiService.getall(`poTermsCode/getSelectPoTermsCodeList?search=${val}`)
      .pipe(
        map(response => {
          const res = response as Array<CustomSelectListItem>;
          this.isDataLoading = false;
          return res;
        })
      )
  }

  filterAccountCodes(val: string): Observable<Array<CustomSelectListItem>> {
    return this.apiService.getall(`mainAccounts/getSelectMainAccountList?search=${val}`)
      .pipe(
        map(response => {
          const res = response as Array<CustomSelectListItem>;
          this.isDataLoading = false;
          return res;
        })
      )
  }

  validate(event: MatAutocompleteSelectedEvent, control: string, action: string) {
    let value: string = '';
    if (action == "change") {
      value = this.utilService.removeSqueres(event.option.value);
    }

    switch (control) {
      case "categoryCodeControl":

        this.apiService.getall('vendorCategory/getCustCatByVenCatCode/' + value).subscribe(res => {
          if (res != null)
            this.form.value['vendCatCode'] = res['vendCatCode'];
          else {
            this.form.value['vendCatCode'] = '';
            this.categoryCodeControl.setValue('');
          }
        });
        break;
      case "salesTermsCodeControl":

        this.apiService.getall('poTermsCode/getPoTermsByTermsCode/' + value).subscribe(res => {
          if (res != null) {
            this.form.value['poTermsCode'] = res['poTermsCode'];
          }
          else {
            this.form.value['poTermsCode'] = '';
            this.salesTermsCodeControl.setValue('');
          }
        });
        break;


      case "ARAcControl":

        this.apiService.getall('MainAccounts/getAccountByAccountCode/' + value).subscribe(res => {
          if (res != null) {
            this.form.value['vendARAc'] = res['finAcCode'];
          }
          else {
            this.form.value['vendARAc'] = '';
            this.custARAcControl.setValue('');
          }
        });
        break;
      case "custDefExpAcCodeControl":

        this.apiService.getall('MainAccounts/getAccountByAccountCode/' + value).subscribe(res => {

          if (res != null) {
            this.form.value['vendDefExpAcCode'] = res['finAcCode'];
          }
          else {
            this.form.value['vendDefExpAcCode'] = '';
            this.custDefExpAcCodeControl.setValue('');
          }
        });
        break;
      case "custARAdjAcCodeControl":

        this.apiService.getall('MainAccounts/getAccountByAccountCode/' + value).subscribe(res => {
          if (res != null)
            this.form.value['vendARAdjAcCode'] = res['finAcCode'];
          else {
            this.form.value['vendARAdjAcCode'] = '';
            this.custARAdjAcCodeControl.setValue('');
          }
        });
        break;
      case "custArAcCodeControl":

        this.apiService.getall('MainAccounts/getAccountByAccountCode/' + value).subscribe(res => {
          if (res != null)
            this.form.value['vendArAcCode'] = res['finAcCode'];
          else {
            this.form.value['vendArAcCode'] = '';
            this.custArAcCodeControl.setValue('');
          }
        });
        break;
      case "custARDiscAcCodeControl":

        this.apiService.getall('MainAccounts/getAccountByAccountCode/' + value).subscribe(res => {
          if (res != null)
            this.form.value['vendARDiscAcCode'] = res['finAcCode'];
          else {
            this.form.value['vendARDiscAcCode'] = '';
            this.custARDiscAcCodeControl.setValue('');
          }
        });
        break;
      default: ;

    }

  }


}
