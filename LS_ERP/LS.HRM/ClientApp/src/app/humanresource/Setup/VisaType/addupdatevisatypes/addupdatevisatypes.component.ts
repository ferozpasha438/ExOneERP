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
  selector: 'app-addupdatevisatypes',
  templateUrl: './addupdatevisatypes.component.html',
  styles: [
  ]
})
export class AddupdatevisatypesComponent extends ParentHrmAdminComponent implements OnInit {
  modalTitle!: string;
  modalBtnTitle!: string;
  dbops!: DBOperation;
  form!: FormGroup;
  id: number = 0;
  isReadOnly: boolean = false;
  countries: Array<CustomSelectListItem> = [];

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdatevisatypesComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {
    super(authService)
  };


  ngOnInit(): void {
    this.setForm();
    this.loadCountries();
    if (this.id > 0)
      this.setEditForm();
  }

  setForm() {
    this.form = this.fb.group(
      {
        'visaTypeCode': ['', Validators.required],
        'visaTypeNameEn': ['', Validators.required],
        'visaTypeNameAr': [''],
        'countryCode': ['', Validators.required],
        'isActive': [false],
      }
    );
  }
  setEditForm() {
    this.apiService.get('VisaType', this.id).subscribe(res => {
      if (res) {
        this.isReadOnly=true;
        this.form.patchValue(res);
      }
    });
  }
  closeModel() {
    this.dialogRef.close();
  }

  loadCountries() {
    this.apiService.getall('Country/getCountrySelectListItem').subscribe(res => {
      this.countries = res;
    });
  }

  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      this.apiService.post('VisaType', this.form.value)
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
    this.form.controls['visaTypeCode'].setValue('');
    this.form.controls['visaTypeNameEn'].setValue('');
    this.form.controls['visaTypeNameAr'].setValue('');
    this.form.controls['countryCode'].setValue('');
    this.form.controls['isActive'].setValue(false);
  }
}
