import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from 'src/app/api-authorization/AuthorizeService';
import { ApiService } from 'src/app/services/api.service';
import { NotificationService } from 'src/app/services/notification.service';
import { DBOperation } from 'src/app/services/utility.constants';
import { UtilityService } from 'src/app/services/utility.service';
import { ParentHrmAdminComponent } from 'src/app/sharedcomponent/ParentHrmAdmin.component';
import { ValidationService } from 'src/app/sharedcomponent/ValidationService';
import { GetemployeeleavetransactionComponent } from '../getemployeeleavetransaction/getemployeeleavetransaction.component';
import { CustomSelectListItem } from '../../../../../models/MenuItemListDto';

@Component({
  selector: 'app-addupdateemployeeleavetransaction',
  templateUrl: './addupdateemployeeleavetransaction.component.html',
  styles: [],
})
export class AddupdateemployeeleavetransactionComponent extends ParentHrmAdminComponent implements OnInit {
  modalTitle!: string;
  modalBtnTitle!: string;
  dbops!: DBOperation;
  form!: FormGroup;
  id: number = 0;
  isReadOnly: boolean = false;
  empListSelectListItems: Array<CustomSelectListItem> = [];
  leaveTypeSelectListItems: Array<any> = [];

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateemployeeleavetransactionComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {
    super(authService)
  };


  ngOnInit(): void {
    this.loadData();
    this.setForm();

    if (this.id > 0)
      this.setEditForm();
  }

  setForm() {
    this.form = this.fb.group(
      {
        'employeeID': ['', Validators.required],
        'tranDate': ['', Validators.required],
        'leaveTypeCode': ['', Validators.required],
        'typeOfAdj': ['', Validators.required],
        'noOfDays': ['', Validators.required],
        'remarks': [''],
        'approvalAuthorityName': [''],
        'isActive': [false],
      }
    );
    this.isReadOnly = false;
  }

  loadData() {
    this.apiService.getall(`personalInformation/getEmployeeSelectListItem`).subscribe(res => {
      this.empListSelectListItems = res;
    });
  }

  loadLeaveTypeForEmpEvent(evt: any) {
    this.loadLeaveTypeForEmp(evt.intValue);
  }

  loadLeaveTypeForEmp(empId: number) {
    this.apiService.getQueryString(`leaveType/getLeaveTypeSelectListItem`, `?employeeId=${empId}&requestType=`).subscribe(res => {
      this.leaveTypeSelectListItems = res;
    });
  }

  setEditForm() {
    this.apiService.get('employeeLeave/getLeaveAdjTransactionById', this.id).subscribe(res => {
      if (res) {
        this.isReadOnly = true;
        this.loadLeaveTypeForEmp(res.employeeID);
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
      // console.log(this.form.value);

      this.apiService.post('employeeLeave/createUpdateLeaveAdjTransaction', { 'EmployeeLeave': this.form.value })
        .subscribe(res => {
          this.utilService.OkMessage();
          // this.reset();
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
    this.form.controls['employeenumber'].setValue('');
    this.form.controls['date'].setValue('');
    this.form.controls['employeeName'].setValue('');
    this.form.controls['selectLeave'].setValue('');
    this.form.controls['selectType'].setValue('');
    this.form.controls['approvalAuthorityName'].setValue('');
    this.form.controls['isActive'].setValue('');
  }
}
