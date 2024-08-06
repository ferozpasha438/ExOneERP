import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from 'src/app/api-authorization/AuthorizeService';
import { CustomSelectListItem } from 'src/app/models/MenuItemListDto';
import { ApiService } from 'src/app/services/api.service';
import { NotificationService } from 'src/app/services/notification.service';
import { DBOperation } from 'src/app/services/utility.constants';
import { UtilityService } from 'src/app/services/utility.service';
import { ParentHrmAdminComponent } from 'src/app/sharedcomponent/ParentHrmAdmin.component';
import { ValidationService } from 'src/app/sharedcomponent/ValidationService';
import { ParentpayrollmgtComponent } from 'src/app/sharedcomponent/parentpayrollmgt.component';

@Component({
  selector: 'app-addupdatepayrollcomponent',
  templateUrl: './addupdatepayrollcomponent.component.html',
  styles: [],
})
export class AddupdatepayrollcomponentComponent
  extends ParentpayrollmgtComponent
  implements OnInit
{
  modalTitle!: string;
  modalBtnTitle!: string;
  dbops!: DBOperation;
  form!: FormGroup;
  id: number = 0;
  isReadOnly: boolean = false;
  componentTypes: Array<CustomSelectListItem> = [];

  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private authService: AuthorizeService,
    private utilService: UtilityService,
    public dialogRef: MatDialogRef<AddupdatepayrollcomponentComponent>,
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
      payrollComponentCode: ['', Validators.required],
      payrollComponentNameEn: ['', Validators.required],
      payrollComponentNameAr: [''],
      payrollComponentType: ['', Validators.required],
      isFormula: [false, Validators.required],
      formulaQueryString: [''],
      isUsedForOtherPayrollComponent: [false, Validators.required],
      isApplicableForDeduction: [false, Validators.required],
      isActive: [false],
    });
    this.isReadOnly = false;
    this.loadComponentTypes();
  }

  setEditForm() {
    this.apiService.get('PayrollComponent', this.id).subscribe((res) => {
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
      this.apiService.post('PayrollComponent', this.form.value).subscribe(
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
    this.form.controls['payrollComponentCode'].setValue('');
    this.form.controls['payrollComponentNameEn'].setValue('');
    this.form.controls['payrollComponentNameAr'].setValue('');
    this.form.controls['payrollComponentType'].setValue('');
    this.form.controls['isFormula'].setValue(false);
    this.form.controls['formulaQueryString'].setValue('');
    this.form.controls['isUsedForOtherPayrollComponent'].setValue(false);
    this.form.controls['isApplicableForDeduction'].setValue(false);
    this.form.controls['isActive'].setValue(false);
  }

  loadComponentTypes() {
    this.apiService
      .getall('PayrollComponent/GetPayrollComponentTypeSelectListItem')
      .subscribe((res) => {
        this.componentTypes = res;
      });
  }
}
