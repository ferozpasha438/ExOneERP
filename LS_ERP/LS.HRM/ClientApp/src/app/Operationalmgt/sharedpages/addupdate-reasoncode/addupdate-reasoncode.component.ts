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
  selector: 'app-addupdate-reasoncode',
  templateUrl: './addupdate-reasoncode.component.html'
})
export class AddupdateReasoncodeComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;


  isDataLoading: boolean = false;
  readonly: string = "";

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<AddupdateReasoncodeComponent>) {
    super(authService);
  }

  ngOnInit(): void {
  
    this.setForm();

  

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
      'reasonCode': ['REASXXXXXX'],
      'reasonCodeNameEng': ['', Validators.required],
      'reasonCodeNameArb': ['', Validators.required],
      'descriptionEng': ['', Validators.required],
      'descriptionArb': ['', Validators.required],
      'isActive': [true],
      'isForCustomerVisit': [false],
      'isForCustomerComplaint': [false],
    });
  }
  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      this.apiService.post('ReasonCode', this.form.value)
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
    this.apiService.get('ReasonCode/getReasonCodeById', this.id).subscribe(res => {
      if (res) {
        this.form.patchValue(res);
        this.form.patchValue({ 'id': 0 });

      }
    });
  }









  reset() {
    this.form.controls['reasonCode'].setValue('');
    this.form.controls['reasonCodeNameEng'].setValue('');
    this.form.controls['reasonCodeNameArb'].setValue('');
    this.form.controls['descriptionEng'].setValue('');
    this.form.controls['descriptionArb'].setValue('');
    this.form.controls['isActive'].setValue(true);
    this.form.controls['isForCustomerVisit'].setValue(false);
    this.form.controls['isForCustomerComplaint'].setValue(false);
  }




}
