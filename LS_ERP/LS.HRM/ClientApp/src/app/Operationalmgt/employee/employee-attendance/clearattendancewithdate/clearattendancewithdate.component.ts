import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { DBOperation } from '../../../../services/utility.constants';
import { UtilityService } from '../../../../services/utility.service';
import { ParentOptMgtComponent } from '../../../../sharedcomponent/parentoptmgt.component';

@Component({
  selector: 'app-clearattendancewithdate',
  templateUrl: './clearattendancewithdate.component.html'
})
export class ClearattendancewithdateComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  curDate: any = Date.now;
  isLoading: boolean = false;
  inputAttendanceData: any;
  maxDate: any;   //from input
  minDate: any;   //from input
  projectCode: any;   //from input
  siteCode: any;   //from input
  clearAttnFor : string = "selectedDay";

  constructor(private apiService: ApiService, private notifyService: NotificationService, private utilService: UtilityService, private translate: TranslateService, private authService: AuthorizeService, private fb: FormBuilder, public dialogRef: MatDialogRef<ClearattendancewithdateComponent>) {
    super(authService);
  }

  ngOnInit(): void {
    console.log(this.clearAttnFor);
    this.setForm();
  }



  setForm() {
    this.form = this.fb.group({
      "date": ['', Validators.required],

    });
    //if (this.curDate > this.maxPostDate) {
    //  this.maxPostDate = this.curDate;
    //}

  }
  submit() { }

  closeModel() {

    this.dialogRef.close();

  }
  claerAttendance() {


    if (!this.isLoading && this.form.valid) {
      this.isLoading = true;





      let fromdate = new Date(this.form.controls['date'].value);
      fromdate.setMinutes(fromdate.getMinutes() - fromdate.getTimezoneOffset());

      let postData: any = {

        projectCode: this.projectCode,
        siteCode: this.siteCode,
        fromdate: fromdate,
        todate: this.clearAttnFor == "selectedDay" ? fromdate : this.maxDate,
      };
      this.apiService.post('EmployeesAttendance/clearMonthlyAttendanceWithDate', postData)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.isLoading = false;
          this.dialogRef.close(true);

        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
            this.isLoading = false;
          });

    }
    else if (this.form.valid) {

      this.notifyService.showWarning(this.translate.instant("Please_wait") + "...");
    }
    else {
      this.notifyService.showWarning(this.translate.instant("Enter_Date") + "...");

    }

  }
  openDatePicker(dp: any) {
    dp.open();
  }
}
