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
  selector: 'app-addupdatevisainfo',
  templateUrl: './addupdatevisainfo.component.html',
  styles: [
  ]
})
export class AddupdatevisainfoComponent extends ParentHrmAdminComponent implements OnInit {
  modalTitle!: string;
  modalBtnTitle!: string;
  dbops!: DBOperation;
  form!: FormGroup;
  id: number = 0;
  employeeNumber!: string;
  countries: Array<CustomSelectListItem> = [];
  visaTypes: Array<CustomSelectListItem> = [];

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdatevisainfoComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {
    super(authService)
  };

  ngOnInit(): void {
    this.loadCountries();
    this.setForm();
    if (this.id > 0)
      this.setEditForm();
  }

  loadCountries() {
    this.apiService.getall('Country/getCountrySelectListItem').subscribe(res => {
      this.countries = res;
    });
  }

  loadVisaTypes(value: string) {
    this.apiService.getQueryString(`VisaType/GetVisaTypesByCountrySelectListItem?countryCode=`, value)
    .subscribe(res => {
      if (res) {
        this.visaTypes = res;
      }
    });
  }

  setForm() {
    this.form = this.fb.group(
      {
        'employeenumber': [this.employeeNumber],
        'countryCode': ['', Validators.required],
        'visaTypeCode': ['', Validators.required],
        'visaNumber': ['', Validators.required],
        'validFrom': ['', Validators.required],
        'validTo': ['', Validators.required],
        'issueLocation': [''],
        'isActive': [true],
      }
    );
  }
  setEditForm() {
    let queryParam = `id=${encodeURIComponent("" + this.id)}&employeeID=${encodeURIComponent("" + Number(this.employeeNumber))}`;
    this.apiService.getQueryString(`EmployeeVisa/GetEmployeeVisaById?`, queryParam).subscribe(res => {
      if (res) {
        this.form.patchValue(res);
        if(res['countryCode'] != null)
        this.loadVisaTypes(res['countryCode'] as string);
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
      this.form.value['validFrom'] = this.utilService.selectedDateTime(this.form.controls['validFrom'].value);
      this.form.value['validTo'] = this.utilService.selectedDateTime(this.form.controls['validTo'].value);
      this.apiService.post('EmployeeVisa', this.form.value)
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
    this.form.controls['countryCode'].setValue('');
    this.form.controls['visaTypeCode'].setValue('');
    this.form.controls['visaNumber'].setValue('');
    this.form.controls['validFrom'].setValue('');
    this.form.controls['validTo'].setValue('');
    this.form.controls['issueLocation'].setValue('');
    this.form.controls['isActive'].setValue(true);    
  }
}
