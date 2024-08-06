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
  selector: 'app-addupdate-survey-form',
  templateUrl: './addupdate-survey-form.component.html'
})
export class AddupdateSurveyFormComponent extends ParentOptMgtComponent implements OnInit {
  
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
  readonly: string = "";
  elementCodeList: Array<CustomSelectListItem> = [];
  isDataLoading: boolean = false;
  invoiceItemObject: any;
  listOfElements: Array<any> = [];
  sequence: number = 1;
  editsequence: number = 0;
  remarks: string = '';
  formElementCode: string = '';
  elementEngName: string = '';
  elementArbName: string = '';
  elementType: string = '';
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<AddupdateSurveyFormComponent>) {
    super(authService);
  }
  loadElementCodes() {
    this.apiService.getall('SurveyFormElement/getSelectSurveyFormElementList').subscribe(res => {
      this.elementCodeList = res;
    });
  }
  ngOnInit(): void {
    this.setForm();
    this.loadElementCodes();
    this.readonly = "readonly";
  }

  setForm() {

    this.form = this.fb.group({
      'surveyFormCode': [''],
      'surveyFormNameArb': ['', Validators.required],
      'surveyFormNameEng': ['', Validators.required],
      'remarks': [''],
      'formElementCode': ['']
    });
    this.setToDefault();
  }
  submit() {
    if (this.form.valid && this.listOfElements.length > 0) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      this.form.value['elementsList'] = this.listOfElements;
  

      this.apiService.post('SurveyForm', this.form.value)
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
  addElement() {
    if (this.formElementCode != '') {
      
      this.listOfElements.push({
        sequence: this.getSequence(),
        formElementCode: this.formElementCode,
        elementArbName: this.elementArbName,
        elementEngName: this.elementEngName,
        elementType: this.elementType
      });
    }
    this.setToDefault();
  }
  removeElement(i: number) {
    this.listOfElements.splice(i, 1);
    this.downSequence();
  }
  setToDefault() {
    this.formElementCode ="";
    this.elementArbName ="";
    this.elementEngName ="";
    this.elementType ="";    
  }
  getSequence(): number { return this.sequence += this.sequence + 1 };
  downSequence(): number { return this.sequence += this.sequence - 1 };
  reset() {
    this.form.controls['surveyFormCode'].setValue('');
    this.form.controls['surveyFormNameArb'].setValue('');
    this.form.controls['surveyFormNameEng'].setValue('');
    this.form.controls['remarks'].setValue('');
    this.form.controls['formElementCode'].setValue('');
  
    }
  closeModel() {
    this.dialogRef.close();
  }
  getElementDetails(event: any) {
    const eleCode = event.target.value;
    this.apiService.getall('SurveyFormElement/getSurveyFormElementBySurveyFormElementCode/' + eleCode).subscribe(res => {
      this.elementArbName = res['elementArbName'],
      this.elementEngName = res['elementEngName'],
        this.elementType = res['elementType']
    });
  }
 }



