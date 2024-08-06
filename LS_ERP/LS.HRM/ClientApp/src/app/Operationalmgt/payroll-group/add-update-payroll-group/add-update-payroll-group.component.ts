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
  selector: 'app-add-update-payroll-group',
  templateUrl: './add-update-payroll-group.component.html'
})
export class AddUpdatePayrollGroupComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number;
  payrollGroupID: number = 0;
  //branchCode: string;
  // branchCodeList: Array<CustomSelectListItem> = [];
  isDataLoading: boolean = false;
  readonly: string = "";

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<AddUpdatePayrollGroupComponent>) {
    super(authService);
  }

  ngOnInit(): void {
    
    this.setForm();
    //this.loadBranchCodes();
    if (this.id > 0) {
      this.payrollGroupID = this.id;
      this.setEditForm();
      this.readonly = "readonly";
    }
  }
  closeModel() {
    this.dialogRef.close();
  }
  setForm() {
    this.form = this.fb.group({
      //'': ['', Validators.required],
      'payrollGroupID': [0],
      'payrollGroupName_EN': ['', Validators.required],
      'payrollGroupName_AR': ['', Validators.required],
      'companyID': [''],
      'siteID': [''],
      'projectId': [''],
      'businessUnitID': [''],
      'divisionID': [''],
      'branchID': [''],
      'countryID': [''],
      'startPayRollDate': [''],
      'currentPayRollMonth': [''],
      'endPayRollDate': [''],
      'currentPayRollYear': [''],
      'remarks': [''],
      'isForFutureEmployee': [true],
      'isForAllEmployee': [true]
    });
  }
  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['payrollGroupID'] = this.payrollGroupID;
      this.apiService.post('PayRollGroup', this.form.value)
        .subscribe(res => {
          if (res) {

            this.utilService.OkMessage();
            this.reset();
            this.dialogRef.close(true);

          }
        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
          });
    }
    else
      this.utilService.FillUpFields();

  }

  setEditForm() {
    this.apiService.get('PayRollGroup/getPayRollGroupById', this.id).subscribe(res => {
      if (res) {
        this.form.patchValue(res);
        this.form.patchValue({ 'payrollGroupID': 0 });

      }
    });
  }








  reset() {
    this.form.controls['payrollGroupID'].setValue(0);
    this.form.controls['payrollGroupName_EN'].setValue('');
    this.form.controls['payrollGroupName_AR'].setValue('');
    this.form.controls['companyID'].setValue('');
    this.form.controls['siteID'].setValue('');
    this.form.controls['projectId'].setValue('');
    this.form.controls['businessUnitID'].setValue('');
    this.form.controls['divisionID'].setValue('');
    this.form.controls['branchID'].setValue('');
    this.form.controls['countryID'].setValue('');
    this.form.controls['startPayRollDate'].setValue('');
    this.form.controls['currentPayRollMonth'].setValue('');
    this.form.controls['endPayRollDate'].setValue('');
    this.form.controls['currentPayRollYear'].setValue('');
    this.form.controls['remarks'].setValue('');
    this.form.controls['isForFutureEmployee'].setValue(true);
    this.form.controls['isForAllEmployee'].setValue(true);
    
  }




}
