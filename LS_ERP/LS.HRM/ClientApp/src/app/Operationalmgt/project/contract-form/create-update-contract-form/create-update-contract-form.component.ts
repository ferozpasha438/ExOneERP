import { HttpClient } from '@angular/common/http';

import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';

import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ParentOptMgtComponent } from '../../../../sharedcomponent/parentoptmgt.component';
import { OprServicesService } from '../../../opr-services.service';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { NotificationService } from '../../../../services/notification.service';
import { UtilityService } from '../../../../services/utility.service';
import { ValidationService } from '../../../../sharedcomponent/ValidationService';
import { TranslateService } from '@ngx-translate/core';
import { ApiService } from '../../../../services/api.service';
import { DBOperation } from '../../../../services/utility.constants';


@Component({
  selector: 'app-create-update-contract-form',
  templateUrl: './create-update-contract-form.component.html'
})
export class CreateUpdateContractFormComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;

  id: number;

 
  contractFormData: any;
 
  project: any;

  editHeadItemNumber: number = -1;
  editClauseIndex = -1;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<CreateUpdateContractFormComponent>) {
    super(authService);
  }

  ngOnInit(): void {

    this.form = this.fb.group({
    });
 
    console.log(this.contractFormData);
  }





 

  submit() {
    
    }
  
  closeModel() {
    this.dialogRef.close(true);
  }

  translateToolTip(msg: string) {

    return `${this.translate.instant(msg)}`;

  }

  editClause(index: number) {
    if (this.editClauseIndex == index) {

      if (
        (this.contractFormData.contractClauses[index].clauseTitleEng != "" && this.contractFormData.contractClauses[index].clauseTitleArb != "") ||
        (this.contractFormData.contractClauses[index].clauseSubTitleEng != "" && this.contractFormData.contractClauses[index].clauseSubTitleArb != "") ||
        (this.contractFormData.contractClauses[index].clauseDescriptionEng != "" && this.contractFormData.contractClauses[index].clauseDescriptionArb != "")

      )
        this.editClauseIndex = -1;
      else {
        this.notifyService.showWarning(this.translate.instant("Empty_Clause"));

      }
    }
    else if (this.editClauseIndex==-1)
      this.editClauseIndex = index;
  }


  
  moveUpClause(index: number) {

    let temp: any = this.contractFormData.contractClauses[index];

    this.contractFormData.contractClauses[index] = this.contractFormData.contractClauses[index - 1];
    this.contractFormData.contractClauses[index - 1] = temp;

  }
  moveDownClause(index: number) {
    let temp: any = this.contractFormData.contractClauses[index];
   
    this.contractFormData.contractClauses[index] = this.contractFormData.contractClauses[index + 1];
    this.contractFormData.contractClauses[index + 1] = temp;


  }

  saveContractForm() {
    for (let i = 0; i < this.contractFormData.contractClauses.length;i++) {

      this.contractFormData.contractClauses[i].serialNumber = i + 1;
    }


    this.apiService.post('contractForm', this.contractFormData)
      .subscribe(res2 => {
        if (res2) {

          this.utilService.OkMessage();
          this.dialogRef.close(true);
       

        }
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });


  }

  editHeadItem(HeadItemNumber: number) {
    if (this.editHeadItemNumber == HeadItemNumber) {
      this.editHeadItemNumber = -1;
    }
    else
      this.editHeadItemNumber = HeadItemNumber;
  }

  insertNewClause(index: number)  // index specifies where to insert
{
    let newItem: any = {
      clauseTitleEng: "",
      clauseTitleArb: "",
      clauseSubTitleEng: "",
      clauseSubTitleArb: "",
      clauseDescriptionEng: "",
      clauseDescriptionArb: "",
      id: 0,
      serialNumber: 0,
      contractFormId: this.contractFormData.contractFormHead.id
      
      
    };
    this.contractFormData.contractClauses.splice(index + 1, 0, newItem);
    this.editClauseIndex = index + 1;
  }

  removeClause(index: number) {
    if (this.contractFormData.contractClauses.length>2)
    this.contractFormData.contractClauses.splice(index,1);
  }

}
