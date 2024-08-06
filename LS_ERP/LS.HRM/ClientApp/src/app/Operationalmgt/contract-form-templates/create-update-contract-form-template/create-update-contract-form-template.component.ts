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
import { OprServicesService } from '../../opr-services.service';
import { CreateUpdateContractFormElementsComponent } from '../contract-form-elements/create-update-contract-form-elements/create-update-contract-form-elements.component';








@Component({
  selector: 'app-create-update-contract-form-template',
  templateUrl: './create-update-contract-form-template.component.html'
})
export class CreateUpdateContractFormTemplateComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;

  id: number;

  clausesSelectionList: Array<any> = [];
  clausesTitleSelectionList: Array<any> = [];
  clausesSubTitleSelectionList: Array<any> = [];
  clausesDescriptionSelectionList: Array<any> = [];



  isArabic: boolean = false;
  
  clauses: Array<any> = [];
  contractTemplateData: any;
  project: any;

  editHeadItemNumber: number = -1;
  editClauseIndex = -1;

  editingClauseId: number = -1;
  type: string = "";

  constructor(public dialog: MatDialog,private oprService: OprServicesService, private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<CreateUpdateContractFormTemplateComponent>) {
    super(authService);

  }

  ngOnInit(): void {
    this.isArabic = this.utilService.isArabic();
    this.loadClausesSelectionList();
    if (this.id > 0) {
      this.apiService.get('ContractFormTemplates/getContractFormTemplateById', this.id).subscribe(res => {
        console.log(res);
        if (res!=null) {
          this.contractTemplateData.clauses = res.clauses.splice(0);
          this.contractTemplateData.templateHead = res.templateHead;
          
        }
      });

    }
    else {
      this.contractTemplateData.clauses = this.contractTemplateData.contractClauses.splice(0);
      this.contractTemplateData.templateHead = this.contractTemplateData.contractFormHead;
      console.log(this.contractTemplateData);

    }
    this.form = this.fb.group({
    });

    console.log(this.contractTemplateData);
  }




  loadClausesSelectionList() {

   // this.clausesSelectionList=[];
    //this.clausesTitleSelectionList=[];
    //this.clausesSubTitleSelectionList=[];
    //this.clausesDescriptionSelectionList=[];








    this.apiService.getall('contractFormTemplateElements/getAllContractFormTemplateElements').subscribe(res => {
      console.log(res);
       this.clausesTitleSelectionList=[];
    this.clausesSubTitleSelectionList=[];
    this.clausesDescriptionSelectionList=[];

      if (res != null) {
        this.clausesSelectionList = res as Array<any>;
        this.clausesSelectionList.forEach(e => {
         
          if (e.clauseTitleEng != null && e.clauseTitleArb != null) {
            this.clausesTitleSelectionList.push(e);
          }
          if (e.clauseSubTitleEng != null && e.clauseSubTitleArb != null) {
            this.clausesSubTitleSelectionList.push(e);
          }
          if (e.clauseDescriptionEng != null && e.clauseDescriptionArb != null) {
            this.clausesDescriptionSelectionList.push(e);
          }
          
        });

      }
    });

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
    if (this.editClauseIndex == -1) {
      this.editClauseIndex = index;
     // this.editingClauseId = this.contractTemplateData.clauses[index].id;
      this.editingClauseId =-1;

      let ele = <HTMLElement>document.getElementById('clauseSelection');
      ele.scrollIntoView({ block: 'start', behavior: 'smooth', inline: 'nearest' });
     }

  }



  moveUpClause(index: number) {

    let temp: any = this.contractTemplateData.clauses[index];

    this.contractTemplateData.clauses[index] = this.contractTemplateData.clauses[index - 1];
    this.contractTemplateData.clauses[index - 1] = temp;

  }
  moveDownClause(index: number) {
    let temp: any = this.contractTemplateData.clauses[index];

    this.contractTemplateData.clauses[index] = this.contractTemplateData.clauses[index + 1];
    this.contractTemplateData.clauses[index + 1] = temp;


  }

  saveContractFormTemplate() {
    if (this.contractTemplateData.templateHead.isForProject || this.contractTemplateData.templateHead.isForAddingSite
      || this.contractTemplateData.templateHead.isForAddingResources || this.contractTemplateData.templateHead.isForRemovingResources) {
    for (let i = 0; i < this.contractTemplateData.clauses.length; i++) {

      this.contractTemplateData.clauses[i].serialNumber = i + 1;
    }

    
      this.apiService.post('contractFormTemplates', this.contractTemplateData)
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
    else {
      this.notifyService.showInfo(this.translate.instant("Select_Contract_Type"));
    }
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
      mappingId:0,
      contractFormId: this.contractTemplateData.templateHead.id


    };
    this.contractTemplateData.clauses.splice(index + 1, 0, newItem);
    this.editClauseIndex = index + 1;
  }

  removeClause(index: number) {
    if (this.contractTemplateData.clauses.length > 2)
      this.contractTemplateData.clauses.splice(index, 1);
    this.editClauseIndex = -1;
  }
  cancel() {

    if (this.contractTemplateData.clauses[this.editClauseIndex].id == 0) {
      this.contractTemplateData.clauses.splice(this.editClauseIndex, 1);
    }





    this.editClauseIndex = -1;
    this.editingClauseId = -1;
    this.editHeadItemNumber = -1;
  }

  afterSelectClause() {

    if (this.editClauseIndex != -1 && this.editingClauseId != null && this.editingClauseId != -1) {

     // if (this.contractTemplateData.clauses[this.editClauseIndex].id == 0) {
      if (this.editingClauseId == -1 && this.contractTemplateData.clauses.mappingId==0 ) {
        this.contractTemplateData.clauses.splice(this.editClauseIndex, 1);
      }
      else {

        let i = this.clausesSelectionList.findIndex(e => e.id == this.editingClauseId);

        this.contractTemplateData.clauses[this.editClauseIndex].clauseTitleEng = this.clausesSelectionList[i]?.clauseTitleEng;
        this.contractTemplateData.clauses[this.editClauseIndex].clauseTitleArb = this.clausesSelectionList[i]?.clauseTitleArb;
        this.contractTemplateData.clauses[this.editClauseIndex].clauseSubTitleEng = this.clausesSelectionList[i]?.clauseSubTitleEng;
        this.contractTemplateData.clauses[this.editClauseIndex].clauseSubTitleArb = this.clausesSelectionList[i]?.clauseSubTitleArb;
        this.contractTemplateData.clauses[this.editClauseIndex].clauseDescriptionEng = this.clausesSelectionList[i]?.clauseDescriptionEng;
        this.contractTemplateData.clauses[this.editClauseIndex].clauseDescriptionArb = this.clausesSelectionList[i]?.clauseDescriptionArb;

       
        this.contractTemplateData.clauses[this.editClauseIndex].mappingId = this.contractTemplateData.clauses[this.editClauseIndex].mappingId > 0 ? this.contractTemplateData.clauses[this.editClauseIndex].mappingId:0;
        


      }
      this.editClauseIndex = -1;
      this.editingClauseId = -1;

    }
 
   
  }
  getRowBackGroud(index: number): string {

    if (this.editClauseIndex==index) {
      return `editRow`;
    }
    else if (this.contractTemplateData.clauses[index].mappingId == 0 || this.contractTemplateData.clauses[index].mappingId == null) {
      return `newRow`;
    }
    else
      return `whiteBg`;

  }

  createNewClause() {


    let dialogRef = this.oprService.fullWindow(this.dialog, CreateUpdateContractFormElementsComponent);
    (dialogRef.componentInstance as any).modalTitle = "Create_New_Contract_Clause";
    (dialogRef.componentInstance as any).modalBtnTitle = "New";
    (dialogRef.componentInstance as any).id = 0;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.loadClausesSelectionList();
    });
  }
  }

