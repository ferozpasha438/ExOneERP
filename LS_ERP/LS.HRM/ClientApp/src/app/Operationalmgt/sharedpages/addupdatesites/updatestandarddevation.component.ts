import { HttpClient } from '@angular/common/http';
import { Component, OnChanges, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { from, Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, mergeMap, startWith, switchMap } from 'rxjs/operators';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
//import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';
import { ValidationService } from '../../../sharedcomponent/ValidationService';


@Component({
  selector: 'app-updatestandarddevation',
  templateUrl: './updatestandarddevation.component.html'
})
export class UpdatestandarddevationComponent extends ParentOptMgtComponent implements OnInit {
  readonly: string = "";

  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
  customerCode: string;
  custCityCode: string;
  cityList: Array<CustomSelectListItem> = [];
  custCodeControl = new FormControl('', Validators.required);
  filteredCustCodes: Observable<Array<CustomSelectListItem>>;
  isDataLoading: boolean = false;
  isChildCustomer: boolean = false;

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<UpdatestandarddevationComponent>) {
    super(authService);
    this.filteredCustCodes = this.custCodeControl.valueChanges.pipe(
      startWith(''),
      debounceTime(utilService.autoDelay()),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isDataLoading = true;
        return this.filterCustCodes(val || '')
      })
    );

  }

  ngOnInit(): void {
    this.loadCities();

    this.setForm();
    if (this.id > 0) {
      this.setEditForm();
      this.readonly = "readonly";
    }
  }
  closeModel() {
    this.dialogRef.close();
  }
  setForm() {
    this.form = this.fb.group({

      'siteCode': ['SITEXXXXXX'],
      'siteName': ['', Validators.required],
      'siteArbName': ['', Validators.required],
      'customerCode': [''],
      'siteAddress': ['', Validators.required],
      'siteCityCode': ['', Validators.required],
      'siteGeoLatitude': ['', Validators.required],
      'siteGeoLongitude': ['', Validators.required],
      'siteGeoGain': ['', Validators.required],
      'isChildCustomer': [false],
      'vatNumber': [''],
      'isActive': [true],
      'standardDeviation':[0]
    });

  }
  submit() {

    if (this.id > 0)
      this.form.value['id'] = this.id;

    let custCode = this.custCodeControl.value as string;

    if (this.utilService.hasValue(custCode)) {
      this.form.value['customerCode'] = custCode;
      this.custCodeControl.setValue(custCode);

    }
    else {
      this.utilService.FillUpFields();
    }
    if (this.form.valid) {

      if (this.isChildCustomer) {
        const vatNumber = this.form.controls['vatNumber'].value;
        if (!this.utilService.hasValue(vatNumber)) {
          this.notifyService.showError("Enter vatNumber");
          return;
        }

      }      

      this.apiService.post('CustomerSite', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.dialogRef.close(true);
        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
          });

    }
    else
      this.utilService.FillUpFields();
    // this.ngOnInit();
  }

  setEditForm() {
    this.apiService.get('CustomerSite/getSiteById', this.id).subscribe(res => {

      if (res) {
        this.form.patchValue(res);
        this.custCodeControl.setValue(res['customerCode']);
        const isChildCustomerFlag = res['isChildCustomer'];
        this.isChildCustomer = isChildCustomerFlag ? isChildCustomerFlag as boolean : false;
        this.form.patchValue({ 'id': 0 });
      }


    });



  }
  reset() {
    this.form.controls['siteCode'].setValue('');
    this.form.controls['siteName'].setValue('');
    this.form.controls['SiteArbName'].setValue('');
    this.form.controls['customerCode'].setValue('');
    this.form.controls['siteAddress'].setValue('');
    this.form.controls['siteCityCode'].setValue('');
    this.form.controls['siteGeoLatitude'].setValue('');
    this.form.controls['siteGeoLongitude'].setValue('');
    this.form.controls['siteGeoGain'].setValue('');
    this.form.controls['isChildCustomer'].setValue('');
    this.form.controls['vatNumber'].setValue('');
    this.form.controls['isActive'].setValue(true);

  }

  filterCustCodes(val: string): Observable<Array<CustomSelectListItem>> {
    return this.apiService.getall(`CustomerMaster/getSelectCustomerList?search=${val}`)
      .pipe(
        map(response => {
          const res = response as Array<CustomSelectListItem>;
          this.isDataLoading = false;
          return res;
        })
      )
  }
  loadCities() {
    this.apiService.getall('City/getCitiesSelectList').subscribe(res => {
      this.cityList = res;
    });
  }

  isChildCustomerChecked(event: MatSlideToggleChange) {
    this.isChildCustomer = event.checked;
   // this.form.controls['vatNumber'].setValue('');
  }

  validate(event: any, control: string, action: string) {
    let value: string = '';
    if (action == "change") {
      value = event.target.value;
    }

    switch (control) {
      case "custCodeControl":
        this.apiService.getall('CustomerMaster/getCustomerByCustomerCode/' + value).subscribe(res => {
          if (res != null) {
            this.form.patchValue({ 'customerCode': res['custCode'] });
            let custCode = this.custCodeControl.value as string;

            this.form.value['customerCode'] = custCode;


          } else {

            this.form.controls['customerCode'].setValue('');
            this.custCodeControl.setValue('');

          }
        });
        break;
      default: ;
    }

  }

}
