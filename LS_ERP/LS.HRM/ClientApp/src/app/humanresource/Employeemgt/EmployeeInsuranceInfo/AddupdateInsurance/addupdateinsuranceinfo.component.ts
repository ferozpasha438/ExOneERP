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
  selector: 'app-addupdateinsuranceinfo',
  templateUrl: './addupdateinsuranceinfo.component.html',
  styles: [
  ]
})
export class AddupdateinsuranceinfoComponent extends ParentHrmAdminComponent implements OnInit {
  modalTitle!: string;
  modalBtnTitle!: string;
  dbops!: DBOperation;
  form!: FormGroup;
  id: number = 0;
  employeeNumber!: string;
  insuranceTypes: Array<CustomSelectListItem> = [];
  insuranceProviders: Array<CustomSelectListItem>=[];
  insuranceClasses: Array<CustomSelectListItem>=[];

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateinsuranceinfoComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {
    super(authService)
  };

  ngOnInit(): void {
    this.loadInsuranceTypes();
    this.loadInsuranceProviders();
    this.loadInsuranceClasses();
    this.setForm();
    if (this.id > 0)
      this.setEditForm();
  }

  loadInsuranceTypes() {
    this.apiService.getall('InsuranceType/GetInsuranceTypeSelectListItem').subscribe(res => {
      this.insuranceTypes = res;
    });
  }

  loadInsuranceProviders() {
    this.apiService.getall('InsuranceProvider/GetInsuranceProviderSelectListItem').subscribe(res => {
      this.insuranceProviders = res;
    });
  }

  loadInsuranceClasses() {
    this.apiService.getall('InsuranceClass/GetInsuranceClassSelectListItem').subscribe(res => {
      this.insuranceClasses = res;
    });
  }

  setForm() {
    this.form = this.fb.group(
      {
        'employeenumber': [this.employeeNumber],
        'insuranceTypeCode': ['', Validators.required],
        'insuranceProviderCode': ['', Validators.required],
        'insuranceClassCode': ['', Validators.required],
        'policyName': ['', Validators.required],
        'policyNumber': ['', Validators.required],
        'policyHolder': ['', Validators.required],
        'policyStartDate': ['', Validators.required],
        'policyExpiryDate': ['', Validators.required],
        'premiumPerYear': ['', Validators.required],
        'remarks': [''],
        'isActive': [true],
      }
    );
  }
  setEditForm() {
    let queryParam = `id=${encodeURIComponent("" + this.id)}&employeeID=${encodeURIComponent("" + Number(this.employeeNumber))}`;
    this.apiService.getQueryString(`EmployeeInsurance/GetEmployeeInsuranceById?`, queryParam).subscribe(res => {
      if (res) {
        this.form.patchValue(res);
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
      this.form.value['policyStartDate'] = this.utilService.selectedDateTime(this.form.controls['policyStartDate'].value);
      this.form.value['policyExpiryDate'] = this.utilService.selectedDateTime(this.form.controls['policyExpiryDate'].value);
      this.apiService.post('EmployeeInsurance', this.form.value)
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
    this.form.controls['insuranceTypeCode'].setValue('');
    this.form.controls['insuranceProviderCode'].setValue('');
    this.form.controls['insuranceClassCode'].setValue('');
    this.form.controls['policyName'].setValue('');
    this.form.controls['policyNumber'].setValue('');
    this.form.controls['policyHolder'].setValue('');
    this.form.controls['policyStartDate'].setValue('');
    this.form.controls['policyExpiryDate'].setValue('');
    this.form.controls['premiumPerYear'].setValue('');
    this.form.controls['remarks'].setValue('');
    this.form.controls['isActive'].setValue(true);
  }
}
