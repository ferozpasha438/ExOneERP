import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../../services/api.service';
import { ParentOptMgtComponent } from '../../../../sharedcomponent/parentoptmgt.component';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { DBOperation } from '../../../../services/utility.constants';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { CustomSelectListItem } from '../../../../models/MenuItemListDto';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { UtilityService } from '../../../../services/utility.service';
import { TranslateService } from '@ngx-translate/core';
import { DatePipe } from '@angular/common';
import { NotificationService } from '../../../../services/notification.service';

@Component({
  selector: 'app-create-update-pv-open-close-req',
  templateUrl: './create-update-pv-open-close-req.component.html'
})
export class CreateUpdatePvOpenCloseReqComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number;

  readonly: string = "";
  requestData: any;

  currentDate: Date = new Date();


  isUpdating: boolean = false;


 

 
 
 
  
 
 
  isArabic: boolean = false;


  min1: Date=new Date();
  min2: Date= new Date();
  max1: Date = new Date();
  max2: Date = new Date();
 
  interval: any;
  
  recentAttendanceDate: Date = new Date();

  constructor(private notifyService: NotificationService,public datepipe: DatePipe, private translate: TranslateService, private fb: FormBuilder, private authService: AuthorizeService, private utilService: UtilityService, private apiService: ApiService, public dialogRef: MatDialogRef<CreateUpdatePvOpenCloseReqComponent>) {

    super(authService);



  }

  ngOnInit(): void {
    this.setForm();
    
    this.isArabic = this.utilService.isArabic();
    
   

  }

  getRecentAttendanceDate() {
    if (this.requestData != null) {
      this.apiService.getall(`ProjectSites/getRecenetAttendanceDate/${this.requestData.projectCode}/${this.requestData.siteCode}`)
        .subscribe(res => {
        
          this.recentAttendanceDate = res;
          this.min1 = res as Date;
        });

      this.interval = setInterval(() => {


      }, 1000);


    }

  }


  setForm() {
    



    this.form=this.fb.group({

      'effectiveDate': [this.requestData.id > 0 ? this.requestData?.effectiveDate : ''],
      'extensionDate': [this.requestData.id > 0 ? this.requestData?.extensionDate : '']

    });

    if (this.requestData.id == 0) {
      this.requestData.effectiveDate = '';
      this.requestData.extensionDate = '';

    }
    if (this.requestData?.isExtendProjReq || this.requestData?.isReOpenReq) {

        this.form.controls['extensionDate'].addValidators(Validators.required);
        let m: Date = new Date(this.requestData.endDate);
      this.min2 = new Date(m.setDate(m.getDate() + 1));

      if (this.requestData?.isReOpenReq) {

        this.form.controls['effectiveDate'].addValidators(Validators.required);
        let prevEndDate: Date = new Date(this.requestData.endDate)
        this.min1 = new Date(prevEndDate.getFullYear(),prevEndDate.getMonth(),prevEndDate.getDate()+1);
        this.max1.setUTCFullYear(this.currentDate.getUTCFullYear() + 1);

      }



    }
    else if (this.requestData?.isCloseReq || this.requestData?.isSuspendReq || this.requestData.isCancelReq) {
      this.form.controls['effectiveDate'].addValidators(Validators.required);

     // this.min1 = this.currentDate;
      this.getRecentAttendanceDate();

      this.max1=new Date(this.requestData.endDate);

    }

    
  }
  





  
 


 




 
  


  
  closeModel() {
    this.dialogRef.close();
  }

  submit() {

    if (!this.isUpdating)
    {
      this.isUpdating = true;
      if (this.form.valid) {
        
        if (this.requestData.isRevokeSuspReq) {

          this.requestData.extensionDate = null;
          this.requestData.effectiveDate = null;

        }
        else if (this.requestData?.isExtendProjReq) {

          let ed = new Date(this.form.controls['extensionDate'].value);
          ed.setMinutes(ed.getMinutes() - ed.getTimezoneOffset());
          this.form.value['extensionDate'] = ed;
          this.requestData.extensionDate = ed;
          this.requestData.effectiveDate = null;


        }
        else if (this.requestData?.isCloseReq || this.requestData?.isSuspendReq || this.requestData.isCancelReq) {
          let fd = new Date(this.form.controls['effectiveDate'].value);
          fd.setMinutes(fd.getMinutes() - fd.getTimezoneOffset());
          this.form.value['effectiveDate'] = fd;
          this.requestData.effectiveDate = fd;
          this.requestData.extensionDate = null;
        }

        else if (this.requestData.isReOpenReq) {
          let ed = new Date(this.form.controls['extensionDate'].value);
          ed.setMinutes(ed.getMinutes() - ed.getTimezoneOffset());
          this.form.value['extensionDate'] = ed;
          this.requestData.extensionDate = ed;

          let fd = new Date(this.form.controls['effectiveDate'].value);
          fd.setMinutes(fd.getMinutes() - fd.getTimezoneOffset());
          this.form.value['effectiveDate'] = fd;
          this.requestData.effectiveDate = fd;
        }







        this.requestData.isCloseReq = this.requestData?.isCloseReq ? true : false;
        this.requestData.isSuspendReq = this.requestData?.isSuspendReq ? true : false;
        this.requestData.isCancelReq = this.requestData?.isCancelReq ? true : false;
        this.requestData.isReOpenReq = this.requestData?.isReOpenReq ? true : false;
        this.requestData.isRevokeSuspReq = this.requestData?.isRevokeSuspReq ? true : false;
        this.requestData.isExtendProjReq = this.requestData?.isExtendProjReq ? true : false;
        this.requestData.id = this.requestData.id > 0 ? this.requestData.id : 0;

        this.isUpdating = true;

        this.apiService.post('PvOpenCloseReq', this.requestData)
          .subscribe(res => {
            this.isUpdating = false;
            if (res) {

              this.utilService.OkMessage();

              this.dialogRef.close(true);

            }
          },
            error => {
              this.isUpdating = false;
              console.error(error);
              this.utilService.ShowApiErrorMessage(error);
            });



      }
      else {
        this.isUpdating = false;
        this.utilService.FillUpFields();
      }



    }
    else {
      
      this.notifyService.showError(this.translate.instant("Please wait..."));
    }

  }

  


  translateToolTip(text: string) {
    return this.translate.instant(text);
  }


  ToDateString(date: any) {

    if (date != null)
      return this.datepipe.transform(date.toString(), 'yyyy-MM-dd')?.toString();
    else
      return "";
  }


  openDatePicker(dp: any) {
    dp.open();
  }

}
