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
  selector: 'app-addupdatedependentinfo',
  templateUrl: './addupdatedependentinfo.component.html',
  styles: [
  ]
})
export class AddupdatedependentinfoComponent extends ParentHrmAdminComponent implements OnInit {

  modalTitle!: string;
  modalBtnTitle!: string;
  dbops!: DBOperation;
  form!: FormGroup;
  id: number = 0;
  employeeNumber!: string;
  dependentTypes: Array<CustomSelectListItem> = [];
  genders: Array<CustomSelectListItem> = [];
  addressTypes: Array<CustomSelectListItem> = [];
  insuranceClasses: Array<CustomSelectListItem> = [];

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdatedependentinfoComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {
    super(authService)
  };

  ngOnInit(): void {
    this.loadDependentTypes();
    this.loadGenders();
    this.loadAddressTypes();
    this.loadInsuranceClasses();
    this.setForm();    
    if (this.id > 0)
      this.setEditForm();
  }

  setForm() {
    this.form = this.fb.group(
      {
        'employeenumber':[this.employeeNumber],
        'dependentTypeCode': ['', Validators.required],
        'genderCode': ['', Validators.required],
        'nameInIdEn': ['', Validators.required],
        'nameInIdAr': ['',],
        'idNumber': [''],
        'idExpiryDate': [''],
        'dateOfBirth': ['', Validators.required],
        'phoneNumber': ['', Validators.required],
        'isEmergencyNumber': [false],
        'email': [''],
        'passportExpiryDate': [''],
        'isEligibleForExitReEntry': [false],
        'isEligibleForAirTicket': [false],
        'useEmployeeAddress': [false],
        'addressTypeCode': ['', Validators.required],
        'address': [''],
        'isEligibleForSchooling': [false],
        'isEligibleForInsurance': [false],
        'insuranceClassCode': [''],
        'isActive': [false],
      }
    );
  }
  setEditForm() {
    let queryParam = `id=${encodeURIComponent("" + this.id)}&employeeID=${encodeURIComponent("" + Number(this.employeeNumber))}`;
    this.apiService.getQueryString(`EmployeeDependent/GetEmployeeDependentById?`, queryParam).subscribe(res => {
      if (res) {
        this.form.patchValue(res);
      }
    });
  }

  closeModel() {
    this.dialogRef.close();
  }

  loadDependentTypes() {
    this.apiService.getall('DependentType/GetDependentTypeSelectListItem').subscribe(res => {
      this.dependentTypes = res;
    });
  }

  loadGenders() {
    this.apiService.getall('Gender/getGenderSelectListItem').subscribe(res => {
      this.genders = res;
    });
  }

  loadAddressTypes() {
    this.apiService.getall('AddressType/GetAddressTypeSelectListItem').subscribe(res => {
      this.addressTypes = res;
    });
  }

  loadInsuranceClasses() {
    this.apiService.getall('InsuranceClass/GetInsuranceClassSelectListItem').subscribe(res => {
      this.insuranceClasses = res;
    });
  }

  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      this.form.value['employeeID'] = Number(this.employeeNumber);
      this.form.value['idExpiryDate'] = this.utilService.selectedDateTime(this.form.controls['idExpiryDate'].value);
      this.form.value['dateOfBirth'] = this.utilService.selectedDateTime(this.form.controls['dateOfBirth'].value);
      this.form.value['passportExpiryDate'] = this.utilService.selectedDateTime(this.form.controls['passportExpiryDate'].value);
      this.apiService.post('EmployeeDependent', this.form.value)
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
    this.form.controls['dependentTypeCode'].setValue('');
    this.form.controls['genderCode'].setValue('');
    this.form.controls['nameInIdEn'].setValue('');
    this.form.controls['nameInIdAr'].setValue('');
    this.form.controls['idNumber'].setValue('');
    this.form.controls['idExpiryDate'].setValue('');
    this.form.controls['dateOfBirth'].setValue('');
    this.form.controls['phoneNumber'].setValue('');
    this.form.controls['isEmergencyNumber'].setValue(false);
    this.form.controls['email'].setValue('');
    this.form.controls['passportExpiryDate'].setValue('');
    this.form.controls['isEligibleForExitReEntry'].setValue(false);
    this.form.controls['isEligibleForAirTicket'].setValue(false);
    this.form.controls['useEmployeeAddress'].setValue(false);
    this.form.controls['addressTypeCode'].setValue('');
    this.form.controls['address'].setValue('');
    this.form.controls['isEligibleForSchooling'].setValue(false);
    this.form.controls['isEligibleForInsurance'].setValue(false);
    this.form.controls['insuranceClassCode'].setValue('');
    this.form.controls['isActive'].setValue(false);
  }
}
