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
import { MyrequestComponent } from '../../myrequest/myrequest.component';
import { CustomSelectListItem } from '../../../../models/MenuItemListDto';

@Component({
  selector: 'app-employeereportingback',
  templateUrl: './employeereportingback.component.html',
  styles: [
  ]
})
export class EmployeereportingbackComponent extends ParentHrmAdminComponent implements OnInit {
  modalTitle!: string;
  form!: FormGroup;
  data: any;
  isReadOnly: boolean = false;
  empListSelectListItems: Array<CustomSelectListItem> = [];

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<MyrequestComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {
    super(authService)
  };


  ngOnInit(): void {
    this.setForm();
    this.loadData();
  }

  setForm() {
    this.form = this.fb.group(
      {
        'employeeNumber': ['', Validators.required],
        'employeeName': ['', Validators.required],
        'reportingDate': ['', Validators.required],
        'reportingReason': [''],
        'managerEmployeeID': ['', Validators.required],
        'isApprovalLetterRequired': [false],
        'isJoiningReportSubmitted': [false],
        'isAllowedToResumeDuty': [false],
        //'addressTypeNameAr': [''],
        'actionRequired': [''],
        'remarks': [''],
        'isActive': [true],

      }
    );
    this.isReadOnly = false;
  }
  loadData() {
    this.apiService.get('serviceRequest/getVacationExitReEntryInfoByRequest', this.data.id).subscribe(res => {
      if (res) {
        this.isReadOnly = true;
        this.form.patchValue(res);
      }
    });
    this.apiService.getall(`personalInformation/getEmployeeSelectListItem`).subscribe(res => {
      this.empListSelectListItems = res;
    });
  }
  closeModel() {
    this.dialogRef.close();
  }

  submit() {
    if (this.form.valid) {
      if (this.data.id > 0)
        this.form.value['employeeServiceRequestID'] = this.data.id;

      console.log(this.form.value);

      ////this.apiService.post('serviceRequest/createVacationReportEntry', this.form.value)
      ////  .subscribe(res => {
      ////    this.utilService.OkMessage();
      ////    //this.reset();
      ////    this.dialogRef.close(true);
      ////  },
      ////    error => {
      ////      this.utilService.ShowApiErrorMessage(error);
      ////    });
    }
    else
      this.utilService.FillUpFields();
  }

  reset() {
    this.form.controls['employeeNumber'].setValue('');
    this.form.controls['employeeName'].setValue('');
    this.form.controls['exitReEntryNumber'].setValue('');
    this.form.controls['exitEffectiveDate'].setValue('');
    this.form.controls['validuptodate'].setValue('');
    this.form.controls['numberOfDays'].setValue('');
    this.form.controls['expectedDateofReporting'].setValue('');
    this.form.controls['vacationExtensionAllowed'].setValue('');
    this.form.controls['ticketNumber'].setValue('');
    this.form.controls['airLines'].setValue('');
    this.form.controls['ticketClass'].setValue('');
    this.form.controls['remarks'].setValue('');
    this.form.controls['replacementemployee'].setValue('');
    this.form.controls['nameOfTheReplacementEmployee'].setValue('');
    this.form.controls['isActive'].setValue('');
  }
}
