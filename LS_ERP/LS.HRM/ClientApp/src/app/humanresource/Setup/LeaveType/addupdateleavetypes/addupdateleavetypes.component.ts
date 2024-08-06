import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from 'src/app/api-authorization/AuthorizeService';
import { ApiService } from 'src/app/services/api.service';
import { NotificationService } from 'src/app/services/notification.service';
import { DBOperation } from 'src/app/services/utility.constants';
import { UtilityService } from 'src/app/services/utility.service';
import { ParentHrmAdminComponent } from 'src/app/sharedcomponent/ParentHrmAdmin.component';
import { ValidationService } from 'src/app/sharedcomponent/ValidationService';
import { CustomSelectListItem } from '../../../../models/MenuItemListDto';

@Component({
  selector: 'app-addupdateleavetypes',
  templateUrl: './addupdateleavetypes.component.html',
  styles: [
  ]
})
export class AddupdateleavetypesComponent extends ParentHrmAdminComponent implements OnInit {
  modalTitle!: string;
  modalBtnTitle!: string;
  dbops!: DBOperation;
  form!: FormGroup;
  id: number = 0;
  isReadOnly: boolean = false;
  leaveTypes: Array<CustomSelectListItem> = [];
  countries: Array<CustomSelectListItem> = [];
  genders: Array<CustomSelectListItem> = [];
  isProRata: boolean = false;
  isPaid: boolean = false;
  isAccumulated: boolean = false;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateleavetypesComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {
    super(authService)
  };


  ngOnInit(): void {
    this.setData();
    this.setForm();
    if (this.id > 0)
      this.setEditForm();
  }
  setData() {
    this.apiService.getall("nationality/getNationalitySelectListItem").subscribe(res => {
      if (res)
        this.countries = res;
    });
    this.apiService.getall("gender/getGenderSelectListItem").subscribe(res => {
      if (res)
        this.genders = res;
    });


    this.leaveTypes = [{ text: 'Accrual', value: '1' }, { text: 'Pro-Rata', value: '2' }]
  }
  setForm() {
    this.form = this.fb.group(
      {
        'leaveTypeCode': ['', Validators.required],
        //'leaveTypeCode': ['', Validators.required],
        'leaveTypeEn': ['', Validators.required],
        'leaveTypeAr': ['', Validators.required],
        'nationalityId': [null, Validators.required],
        'type': ['', Validators.required],
        'maxLeaveDays': [0, Validators.required],
        'salaryPercentage': [0],
        'gender': ['', Validators.required],
        'maxAccumulatedLeave': [0],
        'monthlyProRataLeaves': [0],
        //'isReduceFromVacation': [false],
        'isSalaryPaid': [false],
        'isDocumentRequired': [false],
        //'isLocalColumnRequired': [false],
        //'isFinanceApprovalRequired': [false],
        'isCarryForward': [false],
        'isTravelRequired': [false],
        'isUsedForVacation': [false],
        'isExitAndreEntryRequired': [false],
        'isLeaveEncashment': [false],
        'isActive': [false],

      }
    );
    this.isReadOnly = false;
  }
  loadLeaveType(evt: any) {
    this.isProRata = evt.target.value == '2' ? true : false;
    if (!this.isProRata && this.id <= 0)
      this.form.controls['monthlyProRataLeaves'].setValue(0);
  }
  isPaidChecked(evt: any) {
    this.isPaid = evt.checked as boolean;
    if (!this.isPaid && this.id <= 0)
      this.form.controls['salaryPercentage'].setValue(0);
  }
  isAccumulatedChecked(evt: any) {
    this.isAccumulated = evt.checked as boolean;
    if (!this.isAccumulated && this.id <= 0)
      this.form.controls['maxAccumulatedLeave'].setValue(0);
  }

  setEditForm() {
    this.apiService.get('LeaveType', this.id).subscribe(res => {
      if (res) {
        this.isReadOnly = true;
        this.form.patchValue(res);
        if (res['type'] == '2')
          this.isProRata = true;
        if (res['isSalaryPaid'] as boolean == true)
          this.isPaid = true;
        if (res['isUsedForVacation'] as boolean == true)
          this.isAccumulated = true;

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

      //console.log(this.form.value);

        this.apiService.post('LeaveType', this.form.value)
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
    this.form.controls['leaveTypeCode'].setValue('');
    this.form.controls['leaveTypeEn'].setValue('');
    this.form.controls['leaveTypeAr'].setValue('');
  }
}

