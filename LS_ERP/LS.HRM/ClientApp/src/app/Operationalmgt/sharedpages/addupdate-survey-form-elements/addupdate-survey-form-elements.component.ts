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
  selector: 'app-addupdate-survey-form-elements',
  templateUrl: './addupdate-survey-form-elements.component.html'
})
export class AddupdateSurveyFormElementsComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
  listVisible: boolean = false;
  minmaxVisible: boolean = false;
  readonly: string = "";

  isDataLoading: boolean = false;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<AddupdateSurveyFormElementsComponent>) {
    super(authService);


  }

  ngOnInit(): void {
    this.setForm();
    if (this.id > 0) {
      this.setEditForm();
      
     
      this.readonly = "readonly";

    }
    console.log("Before");
    console.log(this.form);
  }
  closeModel() {
    this.dialogRef.close();
  }
  setForm() {
    this.form = this.fb.group({

      'formElementCode': ['SFEXXXX'],
      'elementEngName': ['', Validators.required],
      'elementArbName': ['', Validators.required],
      'elementType': ['', Validators.required],
      'listValueString': [''],
      'minValue': [''],
      'maxValue': [''],
      'isActive': [true]
    });

  }
  submit() {
    console.log("After");
    console.log(this.form);
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;

      if (this.form.value['minValue'] == '')
        this.form.value['minValue'] = 0;
      if (this.form.value['maxValue'] == '')
        this.form.value['maxValue'] = 0;
     


      this.apiService.post('surveyFormElement', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.reset();
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

  setEditForm() {
    this.apiService.get('surveyFormElement/getSurveyFormElementById', this.id).subscribe(res => {

      if (res) {
        this.form.patchValue(res);
        if (res['elementType'] == 'List') {
          this.listVisible = true;
          this.minmaxVisible = false;
        }
        else if (res['elementType'] == 'Scale') {
          this.minmaxVisible = true;
          this.listVisible = false;
        }
        else {
          this.listVisible = false;
          this.minmaxVisible = false;
        }
      }


    });



  }
  reset() {
    this.form.controls['formElementCode'].setValue('');
    this.form.controls['elementEngName'].setValue('');
    this.form.controls['elementArbName'].setValue('');
    this.form.controls['elementType'].setValue('');
    this.form.controls['listValueString'].setValue('');
    this.form.controls['minValue'].setValue(0);
    this.form.controls['maxValue'].setValue(0);
  }

  elementTypeChanged(event: any) {
    const type = event.target.value;



      

    if (type == 'List') {
      this.listVisible = true;
      this.minmaxVisible = false;
    }
    else if (type == 'Scale') {
      this.minmaxVisible = true;
      this.listVisible = false;
    }
    else {
      this.listVisible = false;
      this.minmaxVisible = false;
    }



  }





}
