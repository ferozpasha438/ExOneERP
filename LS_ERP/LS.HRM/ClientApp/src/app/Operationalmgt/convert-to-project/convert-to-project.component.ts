import { HttpClient } from '@angular/common/http';
import { Component, OnChanges, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import * as moment from 'moment';
import { from, Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, mergeMap, startWith, switchMap } from 'rxjs/operators';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../models/MenuItemListDto';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { DBOperation } from '../../services/utility.constants';
import { UtilityService } from '../../services/utility.service';
import { ParentOptMgtComponent } from '../../sharedcomponent/parentoptmgt.component';
import { ValidationService } from '../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-convert-to-project',
  templateUrl: './convert-to-project.component.html'
})
export class ConvertToProjectComponent extends ParentOptMgtComponent implements OnInit {
  readonly: string = "";
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
 // projectCode: string;
  //enquiryNumber: string;
  enquiryHead: any;
  enquiries: Array<any> = [];
  sitesForEnquiry: Array<any> = [];

  projectSites: Array<any> = [];
  customerData: any;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<ConvertToProjectComponent>) {
    super(authService);
  }

  ngOnInit(): void {
    this.setForm();

  }
  closeModel() {
    this.dialogRef.close();
  }
  setForm() {




    console.log(this.enquiryHead);
  
    this.form = this.fb.group({
      'projectCode': [this.enquiryHead.enquiryNumber],
      'customerCode': [this.enquiryHead.customerCode],
      'branchCode': [this.enquiryHead.branchCode],
      'projectNameEng': [''],
      'projectNameArb': [''],
      'enquiryNumber': [this.enquiryHead.enquiryNumber],
      'startDate': ['', Validators.required],
      'endDate': ['', Validators.required],
     
    });
    this.apiService.getall('CustomerMaster/getCustomerByCustomerCode/'+ this.enquiryHead.customerCode).subscribe(res => {
      if (res != null) {
        this.customerData = res;
        this.form.controls['projectNameEng'].setValue(res.custName);
        this.form.controls['projectNameArb'].setValue(res.custArbName);
      }
      else {
        console.log("Error in getCustomerByCustomerCode Method");
  
      }
      this.getEnquiriesForEnquiry(this.enquiryHead.enquiryNumber);

    });
  }
  submit() {





    //let sd = new Date(this.form.controls['startDate'].value);
    //let ed = new Date(this.form.controls['endDate'].value);
    //sd.setMinutes(sd.getMinutes() - sd.getTimezoneOffset());
    //ed.setMinutes(ed.getMinutes() - ed.getTimezoneOffset());

    //this.form.value['startDate'] = sd;
    //this.form.value['endDate'] = ed;

    // if (this.form.valid) {
    if (this.projectSites.length > 0) {

      this.projectSites.forEach(e => {
        let sd = new Date(e.startDate);
        let ed = new Date(e.endDate);
        sd.setMinutes(sd.getMinutes() - sd.getTimezoneOffset());
    ed.setMinutes(ed.getMinutes() - ed.getTimezoneOffset());

        e.startDate = sd;
        e.endDate = ed;
        e.projectNameEng = this.form.controls['projectNameEng'].value;//this.customerData.custName,
        e.projectNameArb = this.form.controls['projectNameArb'].value;//this.customerData.custArbName,
        
      });
      

      
      let input: any = {projectSites: this.projectSites };
        // this.apiService.post('Project/ConvertEnquiryToProject', this.form.value)
      this.apiService.post('ProjectSites/ConvertEnquiryToProjectSites', input)
          .subscribe(res => {
            this.utilService.OkMessage();
            this.dialogRef.close(true);
          },
            error => {
              console.error(error);
              this.utilService.ShowApiErrorMessage(error);
            });
      
    }
      else {

      this.notifyService.showError(this.translate.instant("no enquiries found"));
        this.dialogRef.close(true);
      }
    
    
  
  }

  openDatePicker(dp:any) {
    dp.open();
  }




  
  getEnquiriesForEnquiry(enquiryNumber: string) {
    this.projectSites = [];
    this.apiService.getall(`ServiceEnquiries/getSevriceEnquiriesByEnquiryNumber/${enquiryNumber}`).subscribe(res => {
      this.enquiries = res as Array<any>;
      let tempEnquiries :Array<any>= [];
      for (let i = 0; i < this.enquiries.length;i++) {

        if (tempEnquiries.findIndex(e => e.siteCode == this.enquiries[i].siteCode)<0) {
          this.apiService.getall(`ProjectSites/getProjectSiteByProjectAndSiteCode/${enquiryNumber}/${this.enquiries[i].siteCode}`).subscribe(res => {
            if (res.id ==0) {

              let projectSite: any = {
                'id':0,
                'projectCode': this.enquiryHead.enquiryNumber,
                'customerCode': this.enquiryHead.customerCode,
                'branchCode': this.enquiryHead.branchCode,
                'projectNameEng': this.form.controls['projectNameEng'].value,//this.customerData.custName,
                'projectNameArb': this.form.controls['projectNameArb'].value,//this.customerData.custArbName,
                'enquiryNumber': [this.enquiryHead.enquiryNumber],
                'startDate': '',
                'endDate': '',
                'siteCode': this.enquiries[i].siteCode,
              };
              this.projectSites.push(projectSite);


            }
            else {
              this.projectSites.push(res);
            }
          },

            error => {

            });


        }
        tempEnquiries.push(this.enquiries[i]);
      }
    });
  }


  canSave(): boolean {
    if (this.projectSites.findIndex(e => e.startDate == '' || e.endDate == '' || (e?.siteWorkingHours <= 0 || e?.siteWorkingHours==null)) >= 0)
      return false;
    else
      return true;
  }

  changeWorkingHours(i:number) {
    if (this.projectSites[i].siteWorkingHours < 0) {
      this.projectSites[i].siteWorkingHours = 0;
    }
    else if (this.projectSites[i].siteWorkingHours > 24) {
      this.projectSites[i].siteWorkingHours = 24;

    }
  }
}
