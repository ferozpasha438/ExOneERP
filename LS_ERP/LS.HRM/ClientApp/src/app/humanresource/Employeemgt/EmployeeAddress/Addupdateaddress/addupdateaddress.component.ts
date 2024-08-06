import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { DBOperation } from '../../../../services/utility.constants';
import { UtilityService } from '../../../../services/utility.service';
import { ParentHrmAdminComponent } from '../../../../sharedcomponent/ParentHrmAdmin.component';
import { ValidationService } from '../../../../sharedcomponent/ValidationService';
import { CustomSelectListItem } from 'src/app/models/MenuItemListDto';

@Component({
  selector: 'app-addupdateaddress',
  templateUrl: './addupdateaddress.component.html',
  styles: [
  ]
})
export class AddupdateaddressComponent extends ParentHrmAdminComponent implements OnInit {

  modalTitle!: string;
  modalBtnTitle!: string;
  dbops!: DBOperation;
  form!: FormGroup;
  id: number = 0;
  employeeNumber!: string;
  addressTypes:Array<CustomSelectListItem> = [];
  countries:Array<CustomSelectListItem> = [];
  zones:Array<CustomSelectListItem> = [];
  states: Array<CustomSelectListItem> = [];
  cities: Array<CustomSelectListItem> = [];

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateaddressComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {
    super(authService)
  };

  loadAddressTypes() {
    this.apiService.getall('AddressType/GetAddressTypeSelectListItem').subscribe(res => {
      this.addressTypes = res;
    });
  }

  loadCountries() {
    this.apiService.getall('Country/getCountrySelectListItem').subscribe(res => {
      this.countries = res;
    });
  }

  loadzones() {
    this.apiService.getall('Zone/GetZoneSelectListItem').subscribe(res => {
      this.zones = res;
    });
  }

  loadStates(value: string) {
    this.apiService.getQueryString(`State/GetStatesByCountry?countryCode=`, value)
    .subscribe(res => {
      if (res) {
        this.states = res;
      }
    });
  }

  loadCities(value: string) {
    this.apiService.getQueryString(`City/GetCitiesByState?stateCode=`, value)
    .subscribe(res => {
      if (res) {
        this.cities = res;
      }
    });
  }

  ngOnInit(): void {
    this.loadAddressTypes();
    this.loadCountries();
    this.loadzones();
    this.setForm();
    if (this.id > 0)
      this.setEditForm();
  }

  setForm() {
    this.form = this.fb.group(
      {
        'employeenumber':[this.employeeNumber],
        'addressTypeCode': ['', Validators.required],
        'countryCode': ['', Validators.required],
        'zoneCode': ['', Validators.required],
        'stateCode': ['', Validators.required],
        'cityCode': ['', Validators.required],
        'postCode': [''],
        'buildingNumber': [''],
        'additionalNumber': [''],
        'unitNumber': [''],
        'isActive': [false],
      }
    );
  }
  setEditForm() {
    let queryParam = `id=${encodeURIComponent("" + this.id)}&employeeID=${encodeURIComponent("" + Number(this.employeeNumber))}`;
    this.apiService.getQueryString(`EmployeeAddress/GetEmployeeAddressById?`, queryParam).subscribe(res => {
      if (res) {
        this.form.patchValue(res);
        if(res['countryCode'] != null)
          this.loadStates(res['countryCode'] as string);
        if(res['stateCode'] != null)
          this.loadCities(res['stateCode'] as string);
      }
    });
  }
  closeModel() {
    this.dialogRef.close();
  }

  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      this.form.value['employeeID'] = Number(this.employeeNumber);
      this.apiService.post('EmployeeAddress', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.reset();
          this.dialogRef.close(true);
        },
          error => {
            this.utilService.ShowApiErrorMessage(error);
          });
    }
    else
      this.utilService.FillUpFields();
  }
  reset() {
    this.form.controls['employeenumber'].setValue(this.employeeNumber);
    this.form.controls['addressTypeCode'].setValue('');
    this.form.controls['countryCode'].setValue('');
    this.form.controls['zoneCode'].setValue('');
    this.form.controls['stateCode'].setValue('');
    this.form.controls['cityCode'].setValue('');
    this.form.controls['postCode'].setValue('');
    this.form.controls['buildingNumber'].setValue('');
    this.form.controls['additionalNumber'].setValue('');
    this.form.controls['unitNumber'].setValue('');
    this.form.controls['isActive'].setValue(false);

  }
}


