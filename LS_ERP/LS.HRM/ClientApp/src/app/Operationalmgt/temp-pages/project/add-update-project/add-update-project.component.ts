import { HttpClient } from '@angular/common/http';
import { Component, OnChanges, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { from, Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, mergeMap, startWith, switchMap } from 'rxjs/operators';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { DBOperation } from '../../../../services/utility.constants';
import { UtilityService } from '../../../../services/utility.service';
import { ParentOptMgtComponent } from '../../../../sharedcomponent/parentoptmgt.component';
import { ValidationService } from '../../../../sharedcomponent/ValidationService';


@Component({
  selector: 'app-add-update-project',
  templateUrl: './add-update-project.component.html'
})
export class AddUpdateProjectComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
  //branchCode: string;
  // branchCodeList: Array<CustomSelectListItem> = [];
  isDataLoading: boolean = false;
  readonly: string = "";

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<AddUpdateProjectComponent>) {
    super(authService);
  }

  ngOnInit(): void {
    this.setForm();
    //this.loadBranchCodes();
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
      'projectCode': ['', Validators.required],
      'projectNameEng': ['', Validators.required],
      'projectNameArb': ['', Validators.required],
      'isActive': [true],
    });
  }
  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      this.apiService.post('Project', this.form.value)
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
    this.apiService.get('Project/getProjectById', this.id).subscribe(res => {
      if (res) {
        this.form.patchValue(res);
        this.form.patchValue({ 'id': 0 });

      }
    });
  }








  reset() {
    this.form.controls['projectCode'].setValue('');
    this.form.controls['projectNameEng'].setValue('');
    this.form.controls['projectNameArb'].setValue('');
    this.form.controls['isActive'].setValue(true);
  }




}
