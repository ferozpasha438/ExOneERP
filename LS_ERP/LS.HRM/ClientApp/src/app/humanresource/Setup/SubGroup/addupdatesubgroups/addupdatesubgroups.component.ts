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
  selector: 'app-addupdatesubgroups',
  templateUrl: './addupdatesubgroups.component.html',
  styles: [],
})
export class AddupdatesubgroupsComponent
  extends ParentHrmAdminComponent
  implements OnInit
{
  modalTitle!: string;
  modalBtnTitle!: string;
  dbops!: DBOperation;
  form!: FormGroup;
  id: number = 0;
  isReadOnly: boolean = false;
  religions: Array<CustomSelectListItem> = [];
  groups: Array<CustomSelectListItem> = [];

  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private authService: AuthorizeService,
    private utilService: UtilityService,
    public dialogRef: MatDialogRef<AddupdatesubgroupsComponent>,
    private notifyService: NotificationService,
    private validationService: ValidationService
  ) {
    super(authService);
  }

  ngOnInit(): void {
    this.setForm();
    this.loadReligions();
    if (this.id > 0) this.setEditForm();
  }

  setForm() {
    this.form = this.fb.group({
      subGroupCode: ['', Validators.required],
      subGroupNameEn: ['', Validators.required],
      subGroupNameAr: [''],
      religionCode: ['', Validators.required],
      groupCode: ['', Validators.required],
      isActive: [false],
    });
  }
  setEditForm() {
    this.apiService.get('SubGroup', this.id).subscribe((res) => {
      if (res) {
        this.isReadOnly = true;
        this.form.patchValue(res);
        if (res['religionCode'] != null)
          this.loadGroups(res['religionCode'] as string);
      }
    });
  }
  closeModel() {
    this.dialogRef.close();
  }

  loadReligions() {
    this.apiService
      .getall('Religion/getReligionSelectListItem')
      .subscribe((res) => {
        this.religions = res;
      });
  }

  loadGroups(value: string) {
    const religionCode = value;
    this.apiService
      .getQueryString(
        `Group/getGroupSelectListItem?religionCode=`,
        religionCode
      )
      .subscribe((res) => {
        if (res) {
          this.groups = res;
        }
      });
  }

  submit() {
    if (this.form.valid) {
      if (this.id > 0) this.form.value['id'] = this.id;
      this.apiService.post('SubGroup', this.form.value).subscribe(
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
    this.form.controls['subGroupCode'].setValue('');
    this.form.controls['subGroupNameEn'].setValue('');
    this.form.controls['subGroupNameAr'].setValue('');
    this.form.controls['religionCode'].setValue('');
    this.form.controls['groupCode'].setValue('');
    this.form.controls['isActive'].setValue('');
  }
}
