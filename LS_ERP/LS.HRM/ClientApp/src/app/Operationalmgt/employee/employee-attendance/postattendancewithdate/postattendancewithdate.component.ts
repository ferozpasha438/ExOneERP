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
  selector: 'app-postattendancewithdate',
  templateUrl: './postattendancewithdate.component.html'
})
export class PostattendancewithdateComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  curDate: any = Date.now;
  isLoading: boolean = false;
  inputAttendanceData: any;
  maxPostDate: any;   //from input
  minPostDate: any;   //from input
  projectCode: any;   //from input
  siteCode: any;   //from input


  constructor(private apiService: ApiService,private notifyService: NotificationService,private utilService: UtilityService, private translate: TranslateService,private authService: AuthorizeService, private fb: FormBuilder, public dialogRef: MatDialogRef<PostattendancewithdateComponent>) {
    super(authService);
  }

  ngOnInit(): void {
    console.log(this.inputAttendanceData);
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
  postAttendance() {


    if (!this.isLoading && this.form.valid) {
      this.isLoading = true;





      let todate = new Date(this.form.controls['date'].value);
      todate.setMinutes(todate.getMinutes() - todate.getTimezoneOffset());

      let postData: any = {

        projectCode: this.projectCode,
        siteCode:this.siteCode,
        todate: todate,
        fromdate: this.minPostDate,
      };
      this.apiService.post('PostingMonthlyAttendance/postEmployeeAttendanceWithDate',postData)
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
