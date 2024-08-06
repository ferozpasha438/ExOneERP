import { HttpClient } from '@angular/common/http';
import { Component, OnChanges, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { from, Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, mergeMap, startWith, switchMap } from 'rxjs/operators';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
//import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';
import { ValidationService } from '../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-approval-service-enquiry',
  templateUrl: './approval-service-enquiry.component.html'
})
export class ApprovalServiceEnquiryComponent extends ParentOptMgtComponent implements OnInit {
  form: FormGroup;
  enquiry: any;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<ApprovalServiceEnquiryComponent>) {
    super(authService);
  }

  ngOnInit(): void {
    this.setForm();
  }
  setForm() {
    this.form = this.fb.group({
      
    });
    
    
  }


  closeModel() {
    this.dialogRef.close();
  }
  submit() {
    this.form.value['statusEnquiry'] = "Approved";

    this.form.value['enquiryID'] = this.enquiry.enquiryID;
 
    this.apiService.post('ServiceEnquiries/changeEnquiryStatus', this.form.value)
      .subscribe(res => {
        this.utilService.OkMessage();
        this.ngOnInit();
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });
   
    this.closeModel();
  }
  rejectEnquiry() {
    this.form.value['statusEnquiry'] = "Cancelled";
   
    this.form.value['enquiryID'] = this.enquiry.enquiryID;
  

    this.apiService.post('ServiceEnquiries/changeEnquiryStatus', this.form.value)
      .subscribe(res => {
        this.utilService.OkMessage();
        this.ngOnInit();
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });

    this.closeModel();
    
  }
}
