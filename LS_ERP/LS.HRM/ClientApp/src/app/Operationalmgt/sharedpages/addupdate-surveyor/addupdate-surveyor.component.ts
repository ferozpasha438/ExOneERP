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
  selector: 'app-addupdate-surveyor',
  templateUrl: './addupdate-surveyor.component.html'
})
export class AddupdateSurveyorComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
  branchCode: string;
  branchCodeList: Array < CustomSelectListItem > =[];
  isDataLoading: boolean = false;
  readonly: string = "";
  userList: Array<any>;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<AddupdateSurveyorComponent>) {
    super(authService);
  }

  ngOnInit(): void {
    this.loadUsers();
    this.setForm();
    
    this.loadBranchCodes();

    if (this.id > 0) {
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
    'surveyorCode': ['SRYRXXXXXX'],
    'surveyorNameEng': ['', Validators.required],
    'surveyorNameArb': ['', Validators.required],
    'branch': ['', Validators.required],
    'userId': ['', Validators.required],
    'isActive': [true],
  });
}
submit() {
  if (this.form.valid) {
    if (this.id > 0)
      this.form.value['id'] = this.id;
    this.apiService.post('Surveyors', this.form.value)
      .subscribe(res => {
        if (res) {

            this.utilService.OkMessage();
           
            this.dialogRef.close(true);
          this.reset();
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
  this.apiService.get('Surveyors/getSurveyorById', this.id).subscribe(res => {
    if (res) {
      this.form.patchValue(res);
      this.form.patchValue({ 'id': 0 });

    }
  });
}






loadBranchCodes() {
  this.apiService.getall('Branch/getSelectBranchCodeList').subscribe(res => {
    this.branchCodeList = res;
  });
}
  loadUsers() {
    this.apiService.getall('Users/GetUserSelectionList').subscribe(res => {
      this.userList = res;

    });

  }

reset() {
  this.form.controls['surveyorCode'].setValue('');
  this.form.controls['surveyorNameEng'].setValue('');
  this.form.controls['surveyorNameArb'].setValue('');
  this.form.controls['branch'].setValue('');
  this.form.controls['user'].setValue('');
  this.form.controls['isActive'].setValue(true);
}




 }
