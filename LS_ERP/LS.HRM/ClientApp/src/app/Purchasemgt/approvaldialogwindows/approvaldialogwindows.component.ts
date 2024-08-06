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
import { ValidationService } from '../../sharedcomponent/ValidationService';
import { ParentPurchaseMgtComponent } from '../../sharedcomponent/parentpurchasemgt.component';


@Component({
  selector: 'app-approvaldialogwindows',
  templateUrl: './approvaldialogwindows.component.html',
  styleUrls: []
})

export class ApprovaldialogwindowsComponent implements OnInit { // extends ParentPurchaseMgtComponent
  readonly: string = "";
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  serviceType: any;
  serviceCode: any;
  branchCode: any;
  // projectCode: string;
  //enquiryNumber: string;
  enquiryHead: any;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<ApprovaldialogwindowsComponent>) {
   // super(authService);
  }

  ngOnInit(): void {
    this.setForm();

  }
  closeModel() {
    this.dialogRef.close();
  }
  setForm() {

    this.form = this.fb.group({
      'serviceCode': [this.serviceCode],
      'branchCode': [this.branchCode],
      'serviceType': [this.serviceType],
      'isApproved': [false],
      'appRemarks': ['', Validators.required]
    });
  }
  submit() {


  }


  postApproval(Acceptance: any) {
    console.log(this.form.value);
    if (Acceptance == 'True')
      this.form.value['isApproved'] = true;
    else
      this.form.value['isApproved'] = false;

    if (this.form.valid) {

      /*this.form.value['appRemarks'] = this.form.controls['appRemarks'].value;*/
      this.apiService.post('PurchaseAuthorities/PurApprovals', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.dialogRef.close(true);
        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
          });

    }
    else
      this.utilService.FillUpFields();

  }









}
