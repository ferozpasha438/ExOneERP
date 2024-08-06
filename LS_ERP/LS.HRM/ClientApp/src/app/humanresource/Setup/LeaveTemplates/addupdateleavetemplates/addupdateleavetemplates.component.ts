import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from 'src/app/api-authorization/AuthorizeService';
import { CheckBoxSelectListItem } from 'src/app/models/MenuItemListDto';
import { ApiService } from 'src/app/services/api.service';
import { NotificationService } from 'src/app/services/notification.service';
import { DBOperation } from 'src/app/services/utility.constants';
import { UtilityService } from 'src/app/services/utility.service';
import { ParentHrmAdminComponent } from 'src/app/sharedcomponent/ParentHrmAdmin.component';
import { ValidationService } from 'src/app/sharedcomponent/ValidationService';
@Component({
  selector: 'app-addupdateleavetemplates',
  templateUrl: './addupdateleavetemplates.component.html',
  styles: [
  ]
})
export class AddupdateleavetemplatesComponent
  extends ParentHrmAdminComponent
  implements OnInit {

  modalTitle!: string;
  modalBtnTitle!: string;
  dbops!: DBOperation;
  form!: FormGroup;
  id: number = 0;
  grades: Array<CheckBoxSelectListItem> = [];
  positions: Array<CheckBoxSelectListItem> = [];
  leaveTypeCodes: Array<any> = [];
  selectdLeaveTypeCodes: Array<string> = [];
  isReadOnly: boolean = false;

  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private authService: AuthorizeService,
    private utilService: UtilityService,
    public dialogRef: MatDialogRef<AddupdateleavetemplatesComponent>,
    private notifyService: NotificationService,
    private validationService: ValidationService
  ) {
    super(authService);
  }

  ngOnInit(): void {
    this.setForm();
    if (this.id > 0) this.setEditForm();
    this.loadData();
  }

  setForm() {
    this.form = this.fb.group({
      templateCode: ['', Validators.required],
      templateNameEn: ['', Validators.required],
      templateNameAr: [''],
      gradeCode: ['', Validators.required],
      positionCode: ['', Validators.required],
      remarks: [''],
      isActive: [true],
      leaveTypeCodes: [],
    });
    this.isReadOnly = false;
  }

  setEditForm() {
    this.apiService.get('LeaveTemplate', this.id).subscribe((res) => {
      if (res) {
        this.loadLeaveTemplateMappingList(res['templateCode']);
        this.isReadOnly = true;
        this.form.patchValue(res);
      }
    });
  }

  loadData() {
    this.apiService.getall("Grade/getGradeSelectListItem").subscribe(res => {
      if (res)
        this.grades = res;
    });
    this.apiService.getall("Position/GetPositionSelectListItem").subscribe(res => {
      if (res)
        this.positions = res;
    });

    if (this.id <= 0)
      this.loadLeaveTemplateMappingList();
  }

  loadLeaveTemplateMappingList(templateCode: string = '') {
    this.apiService.getall(`LeaveTemplate/getLeaveTemplateMappingList?templateCode=${templateCode}`).subscribe(res => {
      if (res) {
        this.leaveTypeCodes = res as Array<any>;
      }
    });
  }

  leaveTypeChecked(evt: any, holiday: any) {
    console.log(evt, holiday);
    holiday.canSubmit = evt.checked
  }

  closeModel() {
    this.dialogRef.close();
  }

  submit() {

    //this.selectdLeaveTypeCodes = [];
    //this.leaveTypeCodes.forEach(item => {
    //  if (item.isChecked)
    //    this.selectdLeaveTypeCodes.push(item.leaveTypeCode)
    //});

    //this.leaveTypeCodes.forEach(item => {
    //  console.log(item);
    //});
    



    if (this.leaveTypeCodes.length > 0) {
      if (this.form.valid) {
        if (this.id > 0) this.form.value['id'] = this.id;

        this.form.value['leaveTypeCodes'] = this.leaveTypeCodes.filter(e => e.canSubmit);
        this.apiService.post('LeaveTemplate', this.form.value).subscribe(
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
    } else this.notifyService.showError('select LeaveType(s)');
  }

  reset() {
    this.form.controls['templateCode'].setValue('');
    this.form.controls['templateNameEn'].setValue('');
    this.form.controls['templateNameAr'].setValue('');
  }
}

