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
import { CustomSelectListItem } from '../../../../models/MenuItemListDto';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { DBOperation } from '../../../../services/utility.constants';
import { UtilityService } from '../../../../services/utility.service';
import { ParentOptMgtComponent } from '../../../../sharedcomponent/parentoptmgt.component';
import { ValidationService } from '../../../../sharedcomponent/ValidationService';


@Component({
  selector: 'app-create-update-contract-form-elements',
  templateUrl: './create-update-contract-form-elements.component.html'
})
export class CreateUpdateContractFormElementsComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;

  elementData: any;
  id: number=0;
  isDataLoading: boolean = false;
  readonly: string = "";

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<CreateUpdateContractFormElementsComponent>) {
    super(authService);
  }

  ngOnInit(): void {
   
    this.setForm();

    

    if (this.id > 0) {
      this.setEditForm();
    }
    else {
      this.elementData = {
        clauseTitleEng : '',
        clauseTitleArb : '',
        clauseSubTitleEng : '',
        clauseSubTitleArb : '',
        DescriptionEng : '',
        DescriptionArb : ''

      };

     

    }

  }
  closeModel() {
    this.dialogRef.close();
  }
  setForm() {
    this.form = this.fb.group({
    
    });



  }
  submit() {
  



    if ((this.elementData?.clauseTitleEng == null || this.elementData?.clauseTitleArb == null || this.elementData?.clauseTitleEng == "" || this.elementData?.clauseTitleArb == "")
      && (this.elementData?.clauseSubTitleEng == null || this.elementData?.clauseSubTitleArb == null || this.elementData?.clauseSubTitleEng == "" || this.elementData?.clauseSubTitleArb == "")
      && (this.elementData?.clauseDescriptionEng == null || this.elementData?.clauseDescriptionArb == null || this.elementData?.clauseDescriptionEng == "" || this.elementData?.clauseDescriptionArb == "")
    ) {

      this.utilService.FillUpFields();

    }
    else {

      this.apiService.post('ContractFormTemplateElements', this.elementData)
        .subscribe(res => {
          if (res) {

            this.utilService.OkMessage();

            this.dialogRef.close(true);

          }
        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
          });
    }

  }

  setEditForm() {
    this.apiService.get('ContractFormTemplateElements/getContractFormTemplateElementById', this.id).subscribe(res => {
      this.elementData = res;
    });
  }






  

  



}
