import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from 'src/app/api-authorization/AuthorizeService';
import { ApiService } from 'src/app/services/api.service';
import { NotificationService } from 'src/app/services/notification.service';
import { DBOperation } from 'src/app/services/utility.constants';
import { UtilityService } from 'src/app/services/utility.service';
import { ValidationService } from 'src/app/sharedcomponent/ValidationService';
import { ParenttnamgtComponent } from 'src/app/sharedcomponent/parenttnamgt.component';

@Component({
  selector: 'app-addupdatepayrollgroups',
  templateUrl: './addupdatepayrollgroups.component.html',
  styles: [
  ]
})
export class AddupdatepayrollgroupsComponent extends ParenttnamgtComponent implements OnInit {

  modalTitle!: string;
  modalBtnTitle!: string;
  dbops!: DBOperation;
  form!: FormGroup;
  id: number = 0;
  isReadOnly: boolean = false;

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdatepayrollgroupsComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {
    super(authService)
  };


  ngOnInit(): void {
    this.setForm();
    if (this.id > 0)
      this.setEditForm();
  }

  setForm() {
    this.form = this.fb.group(
      {
        'payrollGroupCode': ['', Validators.required],
        'payrollGroupNameEn': ['', Validators.required],
        'payrollGroupNameAr': [''],
        'payrollGroupStartDate': ['', Validators.required],
        'payrollGroupEndDate': ['', Validators.required],
        'isActive': [false],
      }
    );
    this.isReadOnly = false;
  }
  setEditForm() {
    this.apiService.get('PayrollGroup', this.id).subscribe(res => {
      if (res) {
        this.isReadOnly = true;
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
      this.form.value['payrollGroupStartDate'] = this.utilService.selectedDateTime(this.form.controls['payrollGroupStartDate'].value);
      this.form.value['payrollGroupEndDate'] = this.utilService.selectedDateTime(this.form.controls['payrollGroupEndDate'].value);
      this.apiService.post('PayrollGroup', this.form.value)
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
    this.form.controls['payrollGroupCode'].setValue('');
    this.form.controls['payrollGroupNameEn'].setValue('');
    this.form.controls['payrollGroupNameAr'].setValue('');
    this.form.controls['payrollGroupStartDate'].setValue('');
    this.form.controls['payrollGroupEndDate'].setValue('');
    this.form.controls['isActive'].setValue('');
  }
}
