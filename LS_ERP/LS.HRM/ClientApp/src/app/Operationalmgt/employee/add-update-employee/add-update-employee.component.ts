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
  selector: 'app-add-update-employee',
  templateUrl: './add-update-employee.component.html'
})
export class AddUpdateEmployeeComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  employeeID: number=0;
  //branchCode: string;
 // branchCodeList: Array<CustomSelectListItem> = [];
  isDataLoading: boolean = false;
  readonly: string = "";

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<AddUpdateEmployeeComponent>) {
    super(authService);
  }

  ngOnInit(): void {
  
    this.setForm();
    //this.loadBranchCodes();
    if (this.employeeID > 0) {
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
      'employeeNumber': ['', Validators.required],
      'employeeName': ['', Validators.required],
      'employeeID': ['', Validators.required]
    });
  }
  submit() {

    if (this.form.valid) {
      if (this.employeeID > 0)
        this.form.value['employeeID'] = this.employeeID;
      this.apiService.post('Employee', this.form.value)
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
    this.apiService.get('Employee/getEmployeeById', this.employeeID).subscribe(res => {
      if (res) {
        this.form.patchValue(res);
        //this.form.patchValue({ 'iD': 0 });

      }
    });
  }






  

  reset() {
    this.form.controls['employeeNumber'].setValue('');
    this.form.controls['employeeName'].setValue('');
    this.form.controls['employeeID'].setValue('');
  }




}
