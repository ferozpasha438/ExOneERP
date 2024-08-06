import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../models/MenuItemListDto';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { DBOperation } from '../../services/utility.constants';
import { UtilityService } from '../../services/utility.service';
import { ParentSystemSetupComponent } from '../../sharedcomponent/parentsystemsetup.component';
import { ValidationService } from '../../sharedcomponent/ValidationService';


@Component({
  selector: 'app-addupdatecompanysetup',
  templateUrl: './addupdatecompanysetup.component.html',
  styles: [
  ],
})
export class AddupdatecompanysetupComponent extends ParentSystemSetupComponent implements OnInit {
  companyForm: FormGroup;
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  id: number = 0;
  cityList: Array<CustomSelectListItem> = [];
  //public myDatePickerOptions: IAngularMyDpOptions = {
  //  dateRange: false,
  //  dateFormat: 'dd-mm-yyyy'
  //};
  //@ViewChild('dp') myDp: AngularMyDatePickerDirective;

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdatecompanysetupComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {
    super(authService);
  }

  ngOnInit(): void {
    this.setForm();
    this.loadCities();
    if (this.id > 0)
      this.setEditForm();
  }

  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.companyForm = this.fb.group({
      'companyName': ['', Validators.required],
      'companyNameAr': ['', Validators.required],
      'companyAddress': ['', Validators.required],
      'companyAddressAr': ['', Validators.required],
      'mobile': ['', Validators.compose([Validators.required, this.validationService.mobile9Or10Validator])],
      'email': ['', Validators.compose([Validators.required, Validators.email])],
      'vatNumber': ['', Validators.required],
      'dateFormat': ['', Validators.required],
      // 'companyGeoLocation': ['', Validators.required],
      'logoURL': ['', Validators.required],
      'priceDecimal': ['', Validators.required],
      'city': ['', Validators.required],
      'state': ['', Validators.required],
      'country': ['', Validators.required],
      'phone': ['', Validators.compose([Validators.required, this.validationService.mobile9Or10Validator,
        //  this.validationService.numberValidator
      ])],
      'website': ['', Validators.required],
      'logoImagePath': ['', Validators.required],
      'quantityDecimal': ['', Validators.required],
      'geoLocLatitude': ['', Validators.required],
      'geoLocLongitude': ['', Validators.required],
      'crNumber': [''],
      'ccNumber': [''],
    });

  }


  setEditForm() {
    this.apiService.get('company', this.id).subscribe(res => {
      if (res) {
        this.companyForm.patchValue(res);
      }
    })
  }

  loadCities() {
    this.apiService.getall('city/getSelectList').subscribe(res => {
      if (res) {
        this.cityList = res;
      }
    })
  }

  checkCompanyName(evt: any) {
    if (this.id == 0) {
      this.apiService.getall(`company/checkCompanyName?companyName=${evt.target.value}`).subscribe(res => {
        if (res) {
          this.companyForm.controls['companyName'].setValue('');
        }
      })
    }
  }

  getStateCountrybyCityCode1(event: any) {
    const id = event.target.value;
    this.apiService.getall('City/getStateCountrybyCityCode/' + id).subscribe(res => {
      this.companyForm.patchValue({ 'state': res['stateName'], 'country': res['countryName'] });
    });
  }

  companySubmit() {
    if (this.companyForm.valid) {
      if (this.id > 0)
        this.companyForm.value['id'] = this.id;

      this.apiService.post('company', this.companyForm.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.reset();
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

  reset() {
    this.companyForm.controls['companyName'].setValue('');
    this.companyForm.controls['companyAddress'].setValue('');
    this.companyForm.controls['email'].setValue('');
  }

  closeModel() {
    this.dialogRef.close();
  }
}
