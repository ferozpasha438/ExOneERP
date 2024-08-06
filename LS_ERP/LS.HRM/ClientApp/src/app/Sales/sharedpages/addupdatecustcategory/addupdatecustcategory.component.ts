import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-addupdatecustcategory',
  templateUrl: './addupdatecustcategory.component.html',
  styles: [
  ]
})
export class AddupdatecustcategoryComponent implements OnInit {
  id: number = 0;
  form: FormGroup;
  modalTitle: string;
  row: any;

  maxLength: number = 5;
  defaultMaxLength: number = 5;
  prefixCatCode: boolean = false;

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdatecustcategoryComponent>,
    private notifyService: NotificationService) {

  }

  ngOnInit(): void {
    this.autoGenerateCustCode();
    this.setForm();
    if (this.row) {
      this.form.patchValue(this.row);
      this.id = parseInt(this.row['id']);
    }
  }
  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      'custCatCode': ['', Validators.required],
      'custCatName': ['', Validators.required],
      'custCatDesc': ['', Validators.required],
      'catPrefix': ['']
    });
  }

  autoGenerateCustCode() {
    this.apiService.getall('salesConfig').subscribe(res => {
      this.prefixCatCode = res.prefixCatCode as boolean;
      const categoryLength = res.categoryLength as number
      this.maxLength = categoryLength == 0 ? this.defaultMaxLength : res.categoryLength as number;
    });
  }

  submit() {


    if (this.form.valid) {

      if (this.prefixCatCode) {
        const catPrefix = this.form.controls['catPrefix'].value;
        if (!this.utilService.hasValue(catPrefix as string)) {
          this.notifyService.showError('Please enter CategoryPrefix');
          return;
        }
      }

      if (this.maxLength != (this.form.controls['catPrefix'].value as string).trim().length) {
        this.notifyService.showError(`Category Prefix must be ${this.maxLength} characters`);
        return;
      }

      if (this.id > 0)
        this.form.value['id'] = this.id;

      this.apiService.post('customerCategorySetup', this.form.value)
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
    this.form.reset();
  }
  closeModel() {
    this.dialogRef.close();
  }
}
