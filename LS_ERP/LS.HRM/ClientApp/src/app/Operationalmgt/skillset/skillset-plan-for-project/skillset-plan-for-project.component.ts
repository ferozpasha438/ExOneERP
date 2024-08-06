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
  selector: 'app-skillset-plan-for-project',
  templateUrl: './skillset-plan-for-project.component.html'
})
export class SkillsetPlanForProjectComponent extends ParentOptMgtComponent implements OnInit {

  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
  readonly: string = "";

  isDataLoading: boolean = false;
  invoiceItemObject: any;
  //listOfShifts: Array<any> = [];
  listOfSkillsets: Array<any> = [];
  sequence: number = 1;
  editsequence: number = 0;
  remarks: string = '';
  
  skillSetCode: string = '';
  nameInEnglish: string = '';
  nameInArabic: string = '';
  quantity: number = 0;
  //custCodeControl = new FormControl('', Validators.required);
  siteCodeList: Array<CustomSelectListItem> = [];
  filteredCustCodes: Observable<Array<CustomSelectListItem>>;
  // customerCode: string = '';
  listOfSkillsetCodes: Array<CustomSelectListItem> = [];
  project: any;
  isArab: boolean = false;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<SkillsetPlanForProjectComponent>) {
    super(authService);




  }
  loadSkillsetCodes() {
    this.apiService.getall('Skillset/getSelectSkillsetList').subscribe(res => {
      this.listOfSkillsetCodes = res;
    });
  }
  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.setForm();
    this.loadSiteCodes(this.project.customerCode);
    this.readonly = "readonly";
  }

  setForm() {
    this.form = this.fb.group({
      'projectCode': [this.project.projectCode],
      'customerCode': [this.project.customerCode],
      'siteCode': [this.project.siteCode != null ? this.project.siteCode : '', Validators.required],
    });
    if (this.project.siteCode != null) {

      this.form.controls['siteCode'].disable({ onlySelf: true });
      let event: any = {
        target: { value: this.project.siteCode }

      };
      this.onSelectSiteCode(event);
    }
    this.setToDefault();
  }
  submit() {
    if (this.form.valid && this.listOfSkillsets.length > 0) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      this.form.value['siteCode'] = this.project.siteCode;
      //this.form.value['shiftsList'] = this.listOfShifts;
      this.form.value['skillsetList'] = this.listOfSkillsets;



      this.apiService.post('Skillset/CreateUpdateSkillsetPlanForProject', this.form.value)
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

  addSkillset() {

    if (this.skillSetCode != '' && this.quantity != 0) {
     
      let skillset: any = {
        sequence: this.getSequence(),
        skillSetCode: this.skillSetCode,
        nameInEnglish: this.nameInEnglish,
        nameInArabic: this.nameInArabic,
        quantity: this.quantity
      };

      let index = this.listOfSkillsets.findIndex(s => s.skillSetCode == this.skillSetCode);
      if (index >= 0)
        this.listOfSkillsets.splice(index, 1);

        this.listOfSkillsets.push(skillset);
      
      this.setToDefault();
    }
   
  }

  editElement(i:number) {
    
   

      this.skillSetCode = this.listOfSkillsets[i].skillSetCode;
      this.nameInEnglish = this.listOfSkillsets[i].nameInEnglish;
      this.nameInArabic = this.listOfSkillsets[i].nameInArabic;
      this.quantity = this.listOfSkillsets[i].quantity;
     
  
  }

  removeElement(i: number) {
    this.listOfSkillsets.splice(i, 1);
    this.downSequence();
  }




  setToDefault() {
    this.skillSetCode = "";
    this.quantity=0;
    this.nameInEnglish = "";
    this.nameInArabic = "";
  }
  getSequence(): number { return this.sequence += this.sequence + 1 };
  downSequence(): number { return this.sequence += this.sequence - 1 };
  reset() {
    this.form.controls['nameInEnglish'].setValue('');
    this.form.controls['nameInArabic'].setValue('');
    this.form.controls['skillSetCode'].setValue('');
    this.form.controls['quantity'].setValue('');

  }
  
  getSkillsetDetails(event: any) {
    const skillsetcode = event.target.value;
  
    let index = this.listOfSkillsets.findIndex(s => s.skillSetCode == skillsetcode);
    
    if (index >= 0) {
      this.nameInArabic = this.listOfSkillsets[index].nameInArabic;
      this.nameInEnglish = this.listOfSkillsets[index].nameInEnglish;
      this.quantity = this.listOfSkillsets[index].quantity;

    }
    
    else {
      this.apiService.getall('Skillset/getSkillsetBySkillsetCode/' + skillsetcode).subscribe(res => {
        this.nameInArabic = res['nameInArabic'],
          this.nameInEnglish = res['nameInEnglish'],
          this.quantity = 0;




      });
    }
  }
 
  onSelectSiteCode(event: any) {

    let siteCode = event.target.value;
    this.form.value['siteCode'] = siteCode;
   
    if (siteCode != '') {
 this.project.siteCode = siteCode;
      this.apiService.getall(`Skillset/getSkillsetsByProjectCodeAndSiteCode/${this.project.projectCode}/${siteCode}`).subscribe(res => {
        this.listOfSkillsets = res;
        this.sequence = this.listOfSkillsets.length + 1;
      });

      this.loadSkillsetCodes();
    }
    else {
      this.listOfSkillsets = [];
      this.listOfSkillsetCodes = [];
    }
    this.setToDefault();
  }

  loadSiteCodes(projectCode: string) {
    this.apiService.getall(`customerSite/getSelectSiteListByProjectCode/${this.project.projectCode}`).subscribe(res => {
      this.siteCodeList = res;
    });
  }

  closeModel() {
    this.dialogRef.close();
  }
  
}
