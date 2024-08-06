import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from 'src/app/api-authorization/AuthorizeService';
import { AddupdateaddresstypesComponent } from 'src/app/humanresource/Setup/AddressType/addupdateaddresstypes/addupdateaddresstypes.component';
import { ApiService } from 'src/app/services/api.service';
import { NotificationService } from 'src/app/services/notification.service';
import { DBOperation } from 'src/app/services/utility.constants';
import { UtilityService } from 'src/app/services/utility.service';
import { ParentHrmAdminComponent } from 'src/app/sharedcomponent/ParentHrmAdmin.component';
import { ValidationService } from 'src/app/sharedcomponent/ValidationService';
import { ParentpayrollmgtComponent } from 'src/app/sharedcomponent/parentpayrollmgt.component';

@Component({
  selector: 'app-addupdatepayrollperiod',
  templateUrl: './addupdatepayrollperiod.component.html',
  styles: [],
})
export class AddupdatepayrollperiodComponent
  extends ParentpayrollmgtComponent
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
    public dialogRef: MatDialogRef<AddupdatepayrollperiodComponent>,
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
      payrollPeriodCode: [''],
      payrollPeriodNameEn: ['', Validators.required],
      payrollPeriodNameAr: [''],
      payrollPeriodStartDate: ['', Validators.required],
      payrollPeriodEndDate: ['', Validators.required],
      isOpen: [false],
      isClose: [false],
      isActive: [false],
    });
    this.isReadOnly = false;
  }
  setEditForm() {
    this.apiService.get('PayrollPeriod', this.id).subscribe((res) => {
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
      this.form.value['payrollPeriodStartDate'] =
        this.utilService.selectedDateTime(
          this.form.controls['payrollPeriodStartDate'].value
        );
      this.form.value['payrollPeriodEndDate'] =
        this.utilService.selectedDateTime(
          this.form.controls['payrollPeriodEndDate'].value
        );
      this.apiService.post('PayrollPeriod', this.form.value).subscribe(
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
    this.form.controls['payrollPeriodCode'].setValue('');
    this.form.controls['payrollPeriodNameEn'].setValue('');
    this.form.controls['payrollPeriodNameAr'].setValue('');
    this.form.controls['payrollPeriodStartDate'].setValue('');
    this.form.controls['payrollPeriodEndDate'].setValue('');
    this.form.controls['isOpen'].setValue('');
    this.form.controls['isClose'].setValue('');
    this.form.controls['isActive'].setValue('');
  }
}
