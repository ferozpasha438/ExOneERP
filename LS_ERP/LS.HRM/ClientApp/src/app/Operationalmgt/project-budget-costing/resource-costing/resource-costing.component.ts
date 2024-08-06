import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { UtilityService } from '../../../services/utility.service';
import { TranslateService } from '@ngx-translate/core';
import { ApiService } from '../../../services/api.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { NotificationService } from '../../../services/notification.service';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';


export interface TblSndDefSiteMasterDto {
  siteCode: string;
  siteName: string;
  siteArbName: string;

}

export interface ProjectBudgetCosting {
  customerCode: string;
  projectBudgetCostingId: number;
  projectBudgetEstimationId:number;
  projectCode: string;
  siteCode: string;
  resourceCostingsList: Array<ResourceCosting>;
  //logisticsCostingsList: Array<any>;
  //materialEquipmentCostingsList:Array<any>;
  serviceType: string;
  status: number;

}
export interface ResourceCosting {
  costPerUnit: number;
  
  projectBudgetCostingId: number;
  quantity: number;
  resourceCostingId: number;
  resourceSubCostingList: Array<ResourceSubCosting>;
  skillSetCode: string;
  nameInEnglish: string;
  nameInArabic: string;
  totResourceCost: number;
  siteCode: string;
  margin: number;
}
export interface ResourceSubCosting {

  amount: number;
  costHead: string;
  resourceCostingId: number;
  resourceSubCostingId: number;

}


@Component({
  selector: 'app-resource-costing',
  templateUrl: './resource-costing.component.html'
})
export class ResourceCostingComponent extends ParentOptMgtComponent implements OnInit {
  project: any;
  form: FormGroup;
  projectBudgetCosting: any;

  resourceSubCostingList: Array<ResourceSubCosting> = [];
  resourceCostingList: Array<ResourceCosting> = [];
  grandTotal: number = 0;
  totalForResourceUnit: number = -1;
  resourceSelectList: Array<any> = [];
  resourceExpenceHeadSelectList: Array<any> = [];
  resourceCode: string = '';
  quantity: number;
  activeResourceIndex: number = -1;
  siteCodeSelectList: Array<TblSndDefSiteMasterDto> = [];
  skillSetsList: Array<any> = [];

  numberRegEx = /\-?\d*\.?\d{1,2}/;
  margin: number;


  isArab: boolean = false;

  canSave: boolean = false;

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<ResourceCostingComponent>) {
    super(authService);
  }

  ngOnInit(): void {

    this.isArab = this.utilService.isArabic();
    this.getAllSkillSetList();
    this.setForm();
  }

  getAllSkillSetList() {
    this.apiService.getall(`Skillset/GetAllSkillsetList`).subscribe(res => {

      this.skillSetsList = res;
      console.log(res);


    });

  }
  setForm() {

    this.form = this.fb.group({
      'projectCode': [this.project.projectCode],
      'customerCode': [this.project.customerCode],
      'siteCode': [this.project?.siteCode == null ? '' : this.project.siteCode, Validators.required],
      'subCostAmount': [''],
      'subCostCostHead': [''],
      'margin': [0, Validators.pattern(this.numberRegEx)]

    });

    this.apiService.getall(`customerSite/getSelectSiteListByProjectCode/${this.project.projectCode}`).subscribe(res => {

      if (res != null) {
        this.siteCodeSelectList = res;
        if (this.project?.siteCode != null) {
          this.loadInitialData();
          this.form.controls['siteCode'].disable({ onlySelf: true });
        }
      }
      else
        this.siteCodeSelectList = [];


    });


  }




  closeModel() {
    this.dialogRef.close();
  }
  submit() {
    //if (this.projectBudgetCosting.status == 3) {

    //  this.notifyService.showError('Estimation is in Progress');

    //}
    //else
    if (this.form.valid && this.grandTotal != 0) {


      this.apiService.post('projectBudgetCosting/createProjectResourceCostingForSite', this.projectBudgetCosting)
        .subscribe(res => {
          if (res) {

            this.utilService.OkMessage();

            this.dialogRef.close(true);

          }
        },
          error => {

            console.log(error)
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
          });

    }
    else {


      this.utilService.FillUpFields();

    }

  }


  loadInitialData() {
    this.apiService.getall(`projectBudgetCosting/getProjectResourceCostingForSite/${this.project.customerCode}/${this.project.projectCode}/${this.form.controls['siteCode'].value}`).subscribe(res => {
      if (res.resourceCostingsList != null) {
        console.log(res.resourceCostingsList);
        for (let i = 0; i < res.resourceCostingsList.length; i++) {
          res.resourceCostingsList[i].nameInEnglish = this.skillSetsList.find(e => e.skillSetCode == res.resourceCostingsList[i].skillsetCode).nameInEnglish;
          res.resourceCostingsList[i].nameInArabic = this.skillSetsList.find(e => e.skillSetCode == res.resourceCostingsList[i].skillsetCode).nameInArabic;

        }


      }

      this.projectBudgetCosting = res;
      if (this.projectBudgetCosting.status == 0) {

        this.createNewProjectBudgetCosting();


      }
      else if (this.projectBudgetCosting.status != 3) {
        this.createNewResourceBudgetCostingList();
      }
      this.findTotalsCosts();
    });
    this.apiService.getall(`OperationExpenseHead/getOperationExpenseHeadsForResources`).subscribe(res => {

      this.resourceExpenceHeadSelectList = res;
    });

  }
  createNewProjectBudgetCosting() {

    this.projectBudgetCosting.customerCode = this.project.customerCode;
    this.projectBudgetCosting.projectCode = this.project.projectCode;
    this.projectBudgetCosting.siteCode = this.form.controls['siteCode'].value;

    this.projectBudgetCosting.serviceType = "SKILLSET";
    this.createNewResourceBudgetCostingList();
  }
  createNewResourceBudgetCostingList() {
    this.apiService.getall(`Skillset/getSkillsetsByProjectCodeAndSiteCode/${this.project.projectCode}/${this.form.controls['siteCode'].value}`).subscribe(res => {


      this.projectBudgetCosting.resourceCostingsList = [];
      let rscl: Array<ResourceSubCosting> = [];
      for (let i = 0; i < res?.length; i++) {

        let item: ResourceCosting = {
          costPerUnit: 0,
          projectBudgetCostingId: 0,
          quantity: res[i].quantity,
          margin: 0,
          resourceCostingId: 0,
          resourceSubCostingList: rscl.slice(),
          skillSetCode: res[i].skillSetCode,
          nameInEnglish: res[i].nameInEnglish,
          nameInArabic: res[i].nameInArabic,
          totResourceCost: 0,
          siteCode: this.form.controls['siteCode'].value

        };

        this.projectBudgetCosting.resourceCostingsList.push(item);
      }
    });
  }

  getResourceSubCostingList(rc: ResourceCosting, i: number) {
    this.activeResourceIndex = i;
    this.resourceSubCostingList = rc.resourceSubCostingList.slice();
    this.totalForResourceUnit = rc.costPerUnit;
    this.findTotalsCosts();

  }

  findTotalsCosts() {

    this.grandTotal = 0;

    if (this.projectBudgetCosting.resourceCostingsList?.length != 0) {
      this.projectBudgetCosting.resourceCostingsList?.forEach((prc: any) => {
        prc.costPerUnit = 0;
        prc.resourceSubCostingList.forEach((rsc: any) => {
          prc.costPerUnit += rsc.amount;

        });

        prc.totResourceCost = (prc.costPerUnit + prc.margin) * prc.quantity;


        this.grandTotal = this.grandTotal + prc.totResourceCost;


      });

    }
  }





  onSelectSiteCode(event: any) {
    //this.projectBudgetCosting = null;


    this.totalForResourceUnit = 0;
    this.form.controls['siteCode'].setValue(event.target.value);
    this.loadInitialData();
  }
  addUpdateSubCostings() {


    let amount = this.form.controls['subCostAmount'].value;
    let costhead = this.form.controls['subCostCostHead'].value;
    if (amount > 0 && amount != '' && costhead != '') {



      let newSubCost: any = {
        amount: amount,
        costHead: costhead,


      };
      let ExistCostingHead = this.projectBudgetCosting.resourceCostingsList[this.activeResourceIndex].resourceSubCostingList.find((e: any) => e.costHead == costhead);
      if (ExistCostingHead == null)
        this.projectBudgetCosting.resourceCostingsList[this.activeResourceIndex].resourceSubCostingList?.push(newSubCost);
      else {
        let index = this.projectBudgetCosting.resourceCostingsList[this.activeResourceIndex].resourceSubCostingList.findIndex((e: any) => e.costHead == costhead);
        this.projectBudgetCosting.resourceCostingsList[this.activeResourceIndex].resourceSubCostingList[index].amount = amount;
      }



      this.findTotalsCosts();
      this.getResourceSubCostingList(this.projectBudgetCosting.resourceCostingsList[this.activeResourceIndex], this.activeResourceIndex);
      this.resetSubCosting();
    }




    this.canSave = !this.projectBudgetCosting.resourceCostingsList.find((e: any) => e.totResourceCost <= 0);



  }

  removeSubCostings(index: number) {

    this.projectBudgetCosting.resourceCostingsList[this.activeResourceIndex].resourceSubCostingList.splice(index, 1);
    this.findTotalsCosts();
    this.getResourceSubCostingList(this.projectBudgetCosting.resourceCostingsList[this.activeResourceIndex], this.activeResourceIndex);
  }
  resetSubCosting() {
    this.form.controls['subCostAmount'].setValue('');
    this.form.controls['subCostCostHead'].setValue('');
  }

  selectCostHead(event: any) {
    let CostHead = event.target.value;

    if (this.activeResourceIndex != -1) {
      this.form.controls['subCostCostHead'].setValue(event.target.value);
      let ExistCostingHead = this.projectBudgetCosting.resourceCostingsList[this.activeResourceIndex].resourceSubCostingList.find((e: any) => e.costHead == CostHead);
      if (ExistCostingHead != null) {
        this.form.controls['subCostAmount'].setValue(ExistCostingHead.amount);
      }
      else {
        this.form.controls['subCostAmount'].setValue('');
      }
    }
    else {
      this.form.controls['subCostCostHead'].setValue('');
      this.form.controls['subCostAmount'].setValue('');

    }


  }


  changeMargin() {
    this.getResourceSubCostingList(this.projectBudgetCosting.resourceCostingsList[this.activeResourceIndex], this.activeResourceIndex)
    this.findTotalsCosts();

  }

  getCostHead(costHead: string) {
    return this.resourceExpenceHeadSelectList.find(e => e.costHead == costHead);
  }

  getSkillSet(code: string) {
    return this.skillSetsList.find(e => e.skillSetCode == code);
  }

  SubCostingOK() {
    //  console.log(this.projectBudgetCosting.resourceCostingsList);
    this.activeResourceIndex = -1;
    this.canSave = !this.projectBudgetCosting.resourceCostingsList.find((e: any) => e.totResourceCost <= 0);
  }

  cancelEditCosting() {
    this.activeResourceIndex = -1;
    this.canSave = !this.projectBudgetCosting.resourceCostingsList.find((e: any) => e.totResourceCost <= 0);

  }
  skippEstimationType() {
    let input: any = {
      projectCode: this.project.projectCode,
      siteCode: this.project.siteCode,
      type: "resource"
    };

    this.apiService.post('projectBudgetCosting/skippEstimationType', input)
      .subscribe(res => {
        if (res) {

          this.utilService.OkMessage();

          this.dialogRef.close(true);

        }
      },
        error => {

          console.log(error)
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });

  }
}




 
