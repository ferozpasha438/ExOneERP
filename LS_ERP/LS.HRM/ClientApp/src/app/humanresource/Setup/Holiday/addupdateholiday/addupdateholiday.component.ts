import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
} from '@angular/forms';
import { DateAdapter, NativeDateAdapter } from '@angular/material/core';
import { MatDatepicker } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import moment, { Moment } from 'moment';
import { AuthorizeService } from 'src/app/api-authorization/AuthorizeService';
import { ApiService } from 'src/app/services/api.service';
import { NotificationService } from 'src/app/services/notification.service';
import { DBOperation } from 'src/app/services/utility.constants';
import { UtilityService } from 'src/app/services/utility.service';
import { ParentHrmAdminComponent } from 'src/app/sharedcomponent/ParentHrmAdmin.component';
import { ValidationService } from 'src/app/sharedcomponent/ValidationService';

@Component({
  selector: 'app-addupdateholiday',
  templateUrl: './addupdateholiday.component.html',
})
export class AddupdateholidayComponent
  extends ParentHrmAdminComponent
  implements OnInit
{
  modalTitle!: string;
  modalBtnTitle!: string;
  dbops!: DBOperation;
  form!: FormGroup;
  id: number = 0;
  isReadOnly: boolean = false;

  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private authService: AuthorizeService,
    private utilService: UtilityService,
    public dialogRef: MatDialogRef<AddupdateholidayComponent>,
    private notifyService: NotificationService,
    private validationService: ValidationService
  ) {
    super(authService);
  }

  ngOnInit(): void {
    this.setForm();
    if (this.id > 0) this.setEditForm();
  }

  setForm() {
    this.form = this.fb.group({
      holidayCode: ['', Validators.required],
      holidayNameEn: ['', Validators.required],
      holidayNameAr: [''],
      date: ['', Validators.required],
      isActive: [false],
    });
    this.isReadOnly = false;
  }

  setEditForm() {
    this.apiService.get('Holiday', this.id).subscribe((res) => {
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
      if (this.id > 0) this.form.value['id'] = this.id;
      this.form.value['date'] = this.utilService.selectedDateTime(
        this.form.controls['date'].value
      );
      this.apiService.post('Holiday', this.form.value).subscribe(
        (res) => {
          this.utilService.OkMessage();
          this.reset();
          this.dialogRef.close(true);
        },
        (error) => {
          this.utilService.ShowApiErrorMessage(error);
        }
      );
    } else this.utilService.FillUpFields();
  }

  reset() {
    this.form.controls['holidayCode'].setValue('');
    this.form.controls['holidayNameEn'].setValue('');
    this.form.controls['holidayNameAr'].setValue('');
    this.form.controls['date'].setValue('');
  }
}
