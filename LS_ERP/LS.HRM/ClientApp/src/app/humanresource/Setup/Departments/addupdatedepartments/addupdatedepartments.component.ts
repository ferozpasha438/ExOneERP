import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from 'src/app/api-authorization/AuthorizeService';
import { CustomSelectListItem } from 'src/app/models/MenuItemListDto';
import { ApiService } from 'src/app/services/api.service';
import { NotificationService } from 'src/app/services/notification.service';
import { DBOperation } from 'src/app/services/utility.constants';
import { UtilityService } from 'src/app/services/utility.service';
import { ParentHrmAdminComponent } from 'src/app/sharedcomponent/ParentHrmAdmin.component';
import { ValidationService } from 'src/app/sharedcomponent/ValidationService';

@Component({
  selector: 'app-addupdatedepartments',
  templateUrl: './addupdatedepartments.component.html',
})
export class AddupdatedepartmentsComponent
  extends ParentHrmAdminComponent
  implements OnInit
{
  modalTitle!: string;
  modalBtnTitle!: string;
  dbops!: DBOperation;
  form!: FormGroup;
  id: number = 0;
  isReadOnly: boolean = false;
  divisions: Array<CustomSelectListItem> = [];

  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private authService: AuthorizeService,
    private utilService: UtilityService,
    public dialogRef: MatDialogRef<AddupdatedepartmentsComponent>,
    private notifyService: NotificationService,
    private validationService: ValidationService
  ) {
    super(authService);
  }

  ngOnInit(): void {
    this.loadDivisions();
    this.setForm();
    if (this.id > 0) this.setEditForm();
  }

  loadDivisions() {
    this.apiService
      .getall('Division/GetDivisionSelectListItem')
      .subscribe((res) => {
        if (res) {
          this.divisions = res;
        }
      });
  }

  setForm() {
    this.form = this.fb.group({
      departmentCode: ['', Validators.required],
      departmentNameEn: ['', Validators.required],
      departmentNameAr: [''],
      divisionCode: ['', Validators.required],
      isActive: [false],
    });
  }
  setEditForm() {
    this.apiService.get('Department', this.id).subscribe((res) => {
      if (res) {
        this.isReadOnly=true;
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
      this.apiService.post('Department', this.form.value).subscribe(
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
    this.form.controls['departmentCode'].setValue('');
    this.form.controls['departmentNameEn'].setValue('');
    this.form.controls['departmentNameAr'].setValue('');
    this.form.controls['divisionCode'].setValue('');
    this.form.controls['isActive'].setValue(false);
  }
}
