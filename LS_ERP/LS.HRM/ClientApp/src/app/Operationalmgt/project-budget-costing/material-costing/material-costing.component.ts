import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { UtilityService } from '../../../services/utility.service';
import { TranslateService } from '@ngx-translate/core';
import { ApiService } from '../../../services/api.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { NotificationService } from '../../../services/notification.service';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, mergeMap, startWith, switchMap } from 'rxjs/operators';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';



export interface TblSndDefSiteMasterDto {
  siteCode: string;
  siteName: string;
  siteArbName: string;

}

export interface ProjectBudgetCosting {
  customerCode: string;
  projectBudgetCostingId: number;
  projectBudgetEstimationId: number;
  projectCode: string;
  siteCode: string;
  materialEquipmentCostingsList: Array<MaterialEquipmentCosting>;
  resourceCostingsList: Array<any>;
  serviceType: string;
  status: number;

}
export interface MaterialEquipmentCosting {
  costPerUnit: number;

  projectBudgetCostingId: number;
  quantity: number;
  materialEquipmentCostingId: number;
  materialEquipmentSubCostingList: Array<MaterialEquipmentSubCosting>;
  materialEquipmentCode: string;
  totMaterialEquipmentCost: number;
  siteCode: string;
}
export interface MaterialEquipmentSubCosting {

  amount: number;
  costHead: string;
  materialEquipmentCostingId: number;
  materialEquipmentSubCostingId: number;
}



@Component({
  selector: 'app-material-costing',
  templateUrl: './material-costing.component.html'
})
export class MaterialCostingComponent extends ParentOptMgtComponent implements OnInit {
  project: any;
  form: FormGroup;
  projectBudgetCosting: ProjectBudgetCosting;

  materialEquipmentSubCostingList: Array<MaterialEquipmentSubCosting> = [];
  materialEquipmentCostingList: Array<MaterialEquipmentCosting> = [];
  grandTotal: number = 0;
  totalForMaterialEquipmentUnit: number = -1;
  materialEquipmentExpenceHeadSelectList: Array<any> = [];
  materialEquipmentCode: string = '';

  activeMaterialEquipmentIndex: number = -1;
  siteCodeSelectList: Array<TblSndDefSiteMasterDto> = [];
  materialEquipmentSelectionList: Array<any> = [];

  isDataLoading: boolean = false;
  quantity: number=0;

  isArab: boolean = false;

  canSave: boolean = false;
  filteredMaterialEquipments: Observable<Array<any>>;
  materialEquipmentCodeControl = new FormControl('');
  filterMaterialEquipments(val: string): Observable<Array<any>> {
    return this.apiService.getall(`MaterialEquipment/getAutoSelectMaterialEquipmentList?search=${val}`)
      .pipe(
        map(response => {
          const res = response as Array<any>;
          this.isDataLoading = false;
        
          return res;
        })
      )
  }

















  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<MaterialCostingComponent>) {
    super(authService);

    this.filteredMaterialEquipments = this.materialEquipmentCodeControl.valueChanges.pipe(
      startWith(this.materialEquipmentCode),
      debounceTime(utilService.autoDelay()),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isDataLoading = true;
        return this.filterMaterialEquipments(val || '')
      })
    );

  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.setForm();
    this.getAllMaterialEquipmentsList();
  }

  getAllMaterialEquipmentsList() {
    this.apiService.getall(`Materialequipment/GetSelectMaterialequipmentList`).subscribe(res => {

      this.materialEquipmentSelectionList = res;
     


    });
    



  }
  setForm() {

    this.form = this.fb.group({
      'projectCode': [this.project.projectCode],
      'customerCode': [this.project.customerCode],
      'siteCode': [this.project?.siteCode == null ? '' : this.project.siteCode, Validators.required],
      'subCostAmount': [],
      'subCostCostHead': [''],
      'materialEquipmentCode': [''],
      'quantity': [0]

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
      // if (this.form.valid && this.grandTotal != 0) {
      if (this.projectBudgetCosting.materialEquipmentCostingsList.length != 0) {

        if (this.projectBudgetCosting.materialEquipmentCostingsList[0].materialEquipmentSubCostingList.length != 0) {
          this.apiService.post('projectBudgetCosting/createProjectMaterialEquipmentCostingForSite', this.projectBudgetCosting)
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
      else {


        this.utilService.FillUpFields();

      }
  }





  loadInitialData() {
    this.apiService.getall(`ProjectBudgetCosting/getProjectMaterialEquipmentCostingForSite/${this.project.customerCode}/${this.project.projectCode}/${this.form.controls['siteCode'].value}`).subscribe(res => {
      this.projectBudgetCosting = res;
    

      if (this.projectBudgetCosting.status == 0) {

        this.createNewProjectBudgetCosting();


      }
      else if (this.projectBudgetCosting.status != 3) {
        this.createNewMaterialEquipmentBudgetCostingList();
      }
      this.findTotalsCosts();
    });
    this.apiService.getall(`OperationExpenseHead/getOperationExpenseHeadsForMaterialEquipment`).subscribe(res => {

      this.materialEquipmentExpenceHeadSelectList = res;
    });

  }



  createNewProjectBudgetCosting() {

    this.projectBudgetCosting.customerCode = this.project.customerCode;
    this.projectBudgetCosting.projectCode = this.project.projectCode;
    this.projectBudgetCosting.siteCode = this.form.controls['siteCode'].value;

    this.projectBudgetCosting.serviceType = "MATERIALEQUIPMENT";
    this.createNewMaterialEquipmentBudgetCostingList();
  }
  createNewMaterialEquipmentBudgetCostingList() {


    this.projectBudgetCosting.materialEquipmentCostingsList = [];


  }

  getMaterialEquipmentSubCostingList(lc: MaterialEquipmentCosting, i: number) {
    this.activeMaterialEquipmentIndex = i;
    this.materialEquipmentSubCostingList = lc.materialEquipmentSubCostingList.slice();
    this.totalForMaterialEquipmentUnit = lc.costPerUnit;
    this.findTotalsCosts();

  }

  findTotalsCosts() {

    this.grandTotal = 0;

    if (this.projectBudgetCosting.materialEquipmentCostingsList.length != 0) {
      this.projectBudgetCosting.materialEquipmentCostingsList.forEach((plc:any)=> {
        plc.costPerUnit = 0;
        plc.materialEquipmentSubCostingList.forEach((rsc:any) => {
          plc.costPerUnit += rsc.amount;

        });

        plc.totMaterialEquipmentCost = plc.costPerUnit * plc.quantity;


        this.grandTotal = this.grandTotal + plc.totMaterialEquipmentCost;


      });

    }
  }





  onSelectSiteCode(event: any) {
   // this.projectBudgetCosting = null;


    this.totalForMaterialEquipmentUnit = 0;
    this.form.controls['siteCode'].setValue(event.target.value);
    this.loadInitialData();
    this.activeMaterialEquipmentIndex = -1;
  }
  addUpdateSubCostings() {
    if (this.activeMaterialEquipmentIndex != -1) {
      let amount = this.form.controls['subCostAmount'].value;
      let costhead = this.form.controls['subCostCostHead'].value;
     // if (amount != 0 && costhead != '') {
      if (costhead != '' && amount !='') {
        let newSubCost: any = {
          amount: amount,
          costHead: costhead,


        };


        let ExistCostingHead = this.projectBudgetCosting.materialEquipmentCostingsList[this.activeMaterialEquipmentIndex].materialEquipmentSubCostingList.find((e: any) => e.costHead == costhead);
        if (ExistCostingHead == null)
          this.projectBudgetCosting.materialEquipmentCostingsList[this.activeMaterialEquipmentIndex].materialEquipmentSubCostingList?.push(newSubCost);
        else {
          let index = this.projectBudgetCosting.materialEquipmentCostingsList[this.activeMaterialEquipmentIndex].materialEquipmentSubCostingList.findIndex((e: any) => e.costHead == costhead);
          this.projectBudgetCosting.materialEquipmentCostingsList[this.activeMaterialEquipmentIndex].materialEquipmentSubCostingList[index].amount = amount;
        }














        this.findTotalsCosts();
        this.getMaterialEquipmentSubCostingList(this.projectBudgetCosting.materialEquipmentCostingsList[this.activeMaterialEquipmentIndex], this.activeMaterialEquipmentIndex);
        this.resetSubCosting();
      }
      

    }
    else {
      this.notifyService.showError(`${this.translate.instant('Please Select Material Equipment')}`);
    }

  }

  removeSubCostings(index: number) {

    this.projectBudgetCosting.materialEquipmentCostingsList[this.activeMaterialEquipmentIndex].materialEquipmentSubCostingList.splice(index, 1);
    this.findTotalsCosts();
    this.getMaterialEquipmentSubCostingList(this.projectBudgetCosting.materialEquipmentCostingsList[this.activeMaterialEquipmentIndex], this.activeMaterialEquipmentIndex);
  }
  resetSubCosting() {
    this.form.controls['subCostAmount'].setValue('');
    this.form.controls['subCostCostHead'].setValue('');
  }

  selectCostHead(event: any) {
    let CostHead = event.target.value;

    if (this.activeMaterialEquipmentIndex != -1) {
      this.form.controls['subCostCostHead'].setValue(event.target.value);
      let ExistCostingHead = this.projectBudgetCosting.materialEquipmentCostingsList[this.activeMaterialEquipmentIndex].materialEquipmentSubCostingList.find((e: any) => e.costHead == CostHead);
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
  totalPerMaterialEquipment(lsc: any): number {
    return lsc.costPerUnit;

  }


  onSelectionMaterialEquipment(event: any) {
   
   let name:string = !this.isArab? event.option.value.nameInEnglish : event.option.value.nameInArabic;
    this.form.controls['materialEquipmentCode'].setValue(name);
    this.materialEquipmentCodeControl.setValue(name);
    this.materialEquipmentCode = event.option.value.code;
  }



  addUpdateMaterialEquipment() {
    if (this.materialEquipmentCode != ''
      /*&& this.form.controls['quantity'].value != 0*/
      && this.form.controls['quantity'].value != '') {
      let materialEquipmentSubCostList: Array<MaterialEquipmentSubCosting> = []
      let materialEquipment: any = {
        costPerUnit: 0,
        quantity: this.form.controls['quantity'].value,
        projectBudgetCostingId: this.projectBudgetCosting.projectBudgetCostingId,

        materialEquipmentCostingId: 0,
        materialEquipmentSubCostingList: materialEquipmentSubCostList.slice(),
        materialEquipmentCode: this.materialEquipmentCode,

      siteCode: this.projectBudgetCosting.siteCode


      };
      this.projectBudgetCosting.materialEquipmentCostingsList.push(materialEquipment);


    }
    else {

      this.notifyService.showError(`${this.translate.instant('Incorrect Material Data')}`);
    }
    this.materialEquipmentCode = '';
    this.materialEquipmentCodeControl.setValue('');
    this.quantity = 0;
    this.form.controls['quantity'].setValue('');
    this.canSave = false;
  }



  removeMaterial(index: number) {

    this.projectBudgetCosting.materialEquipmentCostingsList.splice(index, 1);
    this.findTotalsCosts();
    this.activeMaterialEquipmentIndex = -1;

  }


  getCostHead(costHead: string) {
    return this.materialEquipmentExpenceHeadSelectList.find(e => e.costHead == costHead);
  }
  getMaterialEquipment(code: string) {
    return this.materialEquipmentSelectionList.find(e => e.value == code);


  }


  SubCostingOK() {
    console.log(this.projectBudgetCosting.materialEquipmentCostingsList);
    this.activeMaterialEquipmentIndex = -1;
    this.canSave = !this.projectBudgetCosting.materialEquipmentCostingsList.find((e: any) => e.totLogisticsCost <= 0);
  }

  cancelEditCosting() {
    this.activeMaterialEquipmentIndex = -1;
    this.canSave = !this.projectBudgetCosting.materialEquipmentCostingsList.find((e: any) => e.totLogisticsCost <= 0);
  }
  skippEstimationType() {
    let input: any = {
      projectCode: this.project.projectCode,
      siteCode: this.project.siteCode,
      type: "material"
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
