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
  selector: 'app-attach-survey-form-to-service',
  templateUrl: './attach-survey-form-to-service.component.html'
})
export class AttachSurveyFormToServiceComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
  serviceCodeList: Array<CustomSelectListItem> = [];
  serviceCode: string;
  surveyFormCode: string = '';


  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<AttachSurveyFormToServiceComponent>) {
    super(authService);

  }

  ngOnInit(): void {
  
    
    this.setForm();
    this.getFormCode();
    this.loadServices();
    

  }
  closeModel() {
    this.dialogRef.close();
  }

  submit() {

   

    if (this.form.valid) {


      this.apiService.post('Services/AttachSurveyFormToService', this.form.value)
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


  setForm() {
  
    this.form = this.fb.group({

      'serviceCode': [''],
      'surveyFormCode': [''],
    });

  }

  loadServices() {


    this.apiService.getall('Services/getSelectServiceList').subscribe(res => {
      this.serviceCodeList = res;
    });
  }

  getFormCode() {
    this.apiService.getall('SurveyForm/getSurveyFormHeadById/'+this.id).subscribe(res => {
      this.surveyFormCode = res['surveyFormCode'];
      this.form.controls['surveyFormCode'].setValue(res['surveyFormCode']);
    });
  }



}
