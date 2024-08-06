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
  selector: 'app-add-surveyor-to-enquiry',
  templateUrl: './add-surveyor-to-enquiry.component.html'
})
export class AddSurveyorToEnquiryComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number=0;
  enquiryID: number;
  
  surveyorsList: Array<CustomSelectListItem> = [];
  surveyorCode: string;
  
  branchCode: string;

  
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<AddSurveyorToEnquiryComponent>) {
    super(authService);

  }

  ngOnInit(): void {
    this.setForm();
    this.loadSurveyors();
  
   
  }
  closeModel() {
    this.dialogRef.close();
  }
 
  submit() {


    if (this.form.valid) {


      //this.apiService.post('ServiceEnquiries/addSurveyorToEnquiry?enquiryID=' + this.id + '&surveyorCode=' + this.form.controls['surveyorCode'].value, null)
      this.apiService.post('ServiceEnquiries/addSurveyorToEnquiry', this.form.value)
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

      'enquiryID': [this.id],
      'surveyorCode': ['', Validators.required],
      });

  }

  loadSurveyors() {

   
    this.apiService.getall(`Surveyors/getSelectSurveyorListForBranch/${this.branchCode}`).subscribe(res => {
      this.surveyorsList = res;
      if (this.surveyorsList.length == 0) {
        this.notifyService.showWarning("No SUrveyors Found For Branch:" + this.branchCode);
      }
    });
  }
}
