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
  logisticsCostingsList: Array<LogisticsCosting>;
  resourceCostingsList: Array<any>;
  serviceType: string;
  status: number;

}
export interface LogisticsCosting {
  costPerUnit: number;
  qty: number;
  projectBudgetCostingId: number;
  margin: number;
 
  logisticsCostingId: number;
  logisticsSubCostingList: Array<LogisticsSubCosting>;
  vehicleNumber: string;
  totLogisticsCost: number;
  siteCode: string;
}
export interface LogisticsSubCosting {

  amount: number;
  costHead: string;
  logisticsCostingId: number;
  logisticsSubCostingId: number;
}

@Component({
  selector: 'app-logistics-costing',
  templateUrl: './logistics-costing.component.html'
})
export class LogisticsCostingComponent extends ParentOptMgtComponent implements OnInit {
  project: any;
  form: FormGroup;
  projectBudgetCosting: ProjectBudgetCosting;

  logisticsSubCostingList: Array<LogisticsSubCosting> = [];
  logisticsCostingList: Array<LogisticsCosting> = [];
  grandTotal: number = 0;
  totalForLogisticsUnit: number = -1;
  logisticsExpenceHeadSelectList: Array<any> = [];
  logisticsCode: string = '';

  activeLogisticsIndex: number = -1;
  siteCodeSelectList: Array<TblSndDefSiteMasterDto> = [];
  logisticsSelectionList: Array<any> = [];

  isDataLoading: boolean = false;

  vehicleNumber: string = '';
  margin: number=0;
  filteredVehicleNumbers: Observable<Array<any>>;
  vehicleNumberControl = new FormControl('');

  isArab: boolean = false;

  canSave: boolean = false;
  filterVehicleNumbers(val: string): Observable<Array<any>> {
    return this.apiService.getall(`Logisticsandvehicle/getAutoSelectLogisticsandvehicleList?search=${val}`)
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
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<LogisticsCostingComponent>) {
    super(authService);

    this.filteredVehicleNumbers = this.vehicleNumberControl.valueChanges.pipe(
      startWith(this.vehicleNumber),
      debounceTime(utilService.autoDelay()),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isDataLoading = true;
        return this.filterVehicleNumbers(val || '')
      })
    );

  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.setForm();
  }
  setForm() {

    this.form = this.fb.group({
      'projectCode': [this.project.projectCode],
      'customerCode': [this.project.customerCode],
      'siteCode': [this.project?.siteCode==null?'':this.project.siteCode, Validators.required],
      'subCostAmount': [''],
      'subCostCostHead': [''],
      'vehicleNumber': [''],
      'qty': ['']

    });

    this.apiService.getall(`customerSite/getSelectSiteListByProjectCode/${this.project.projectCode}`).subscribe(res => {

      if (res != null)
        this.siteCodeSelectList = res;
      else
        this.siteCodeSelectList = [];


      if (this.project?.siteCode != null) {
        this.loadInitialData();
        this.form.controls['siteCode'].disable({ onlySelf: true });
      }


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

      if (this.projectBudgetCosting.logisticsCostingsList.length != 0) {

        if (this.projectBudgetCosting?.logisticsCostingsList[0]?.logisticsSubCostingList.length != 0) {
          this.apiService.post('projectBudgetCosting/createProjectLogisticsCostingForSite', this.projectBudgetCosting)
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
    this.apiService.getall(`projectBudgetCosting/getProjectLogisticsCostingForSite/${this.project.customerCode}/${this.project.projectCode}/${this.form.controls['siteCode'].value}`).subscribe(res => {
      this.projectBudgetCosting = res;
     

      if (this.projectBudgetCosting.status == 0) {

        this.createNewProjectBudgetCosting();


      }
      else if (this.projectBudgetCosting.status != 3) {
        this.createNewLogisticsBudgetCostingList();
      }
      this.findTotalsCosts();
    });
    this.apiService.getall(`OperationExpenseHead/getOperationExpenseHeadsForLogistics`).subscribe(res => {

      this.logisticsExpenceHeadSelectList = res;
    });

  }
  createNewProjectBudgetCosting() {

    this.projectBudgetCosting.customerCode = this.project.customerCode;
    this.projectBudgetCosting.projectCode = this.project.projectCode;
    this.projectBudgetCosting.siteCode = this.form.controls['siteCode'].value;

    this.projectBudgetCosting.serviceType = "LOGISTICS";
    this.createNewLogisticsBudgetCostingList();
  }
  createNewLogisticsBudgetCostingList() {
 

      this.projectBudgetCosting.logisticsCostingsList = [];
      
 
  }

  getLogisticsSubCostingList(lc: LogisticsCosting, i: number) {
    this.activeLogisticsIndex = i;
    this.logisticsSubCostingList = lc.logisticsSubCostingList.slice();
    this.totalForLogisticsUnit = lc.costPerUnit;
    this.findTotalsCosts();

  }

  findTotalsCosts() {

    this.grandTotal = 0;

    if (this.projectBudgetCosting.logisticsCostingsList.length != 0) {
      this.projectBudgetCosting.logisticsCostingsList.forEach((plc:any) => {
        plc.costPerUnit = 0;
        plc.logisticsSubCostingList.forEach((rsc:any) => {
          plc.costPerUnit += rsc.amount;

        });

        plc.totLogisticsCost = (plc.costPerUnit+plc.margin)*plc.qty;


        this.grandTotal = this.grandTotal + plc.totLogisticsCost;


      });

    }
  }





  onSelectSiteCode(event: any) {
   // this.projectBudgetCosting = null;


    this.totalForLogisticsUnit = 0;
    this.form.controls['siteCode'].setValue(event.target.value);
    this.loadInitialData();
    this.activeLogisticsIndex = -1;
  }
  addUpdateSubCostings() {
    let amount = this.form.controls['subCostAmount'].value;
 //   console.log(this.form.controls['subCostAmount'].value);
    let costhead = this.form.controls['subCostCostHead'].value;
    if (/*amount != 0 &&*/amount!=''&& costhead != '' && this.form.controls['subCostAmount'].value!=null) {
      let newSubCost: any = {
        amount: amount,
        costHead: costhead


      };

      let ExistCostingHead = this.projectBudgetCosting.logisticsCostingsList[this.activeLogisticsIndex].logisticsSubCostingList.find((e: any) => e.costHead == costhead);
      if (ExistCostingHead == null)
        this.projectBudgetCosting.logisticsCostingsList[this.activeLogisticsIndex].logisticsSubCostingList?.push(newSubCost);
      else {
        let index = this.projectBudgetCosting.logisticsCostingsList[this.activeLogisticsIndex].logisticsSubCostingList.findIndex((e: any) => e.costHead == costhead);
        this.projectBudgetCosting.logisticsCostingsList[this.activeLogisticsIndex].logisticsSubCostingList[index].amount = amount;
      }






      this.findTotalsCosts();
      this.getLogisticsSubCostingList(this.projectBudgetCosting.logisticsCostingsList[this.activeLogisticsIndex], this.activeLogisticsIndex);
      this.resetSubCosting();
    }
   
  }

  removeSubCostings(index: number) {

    this.projectBudgetCosting.logisticsCostingsList[this.activeLogisticsIndex].logisticsSubCostingList.splice(index, 1);
    this.findTotalsCosts();
    this.getLogisticsSubCostingList(this.projectBudgetCosting.logisticsCostingsList[this.activeLogisticsIndex], this.activeLogisticsIndex);
  }
  resetSubCosting() {
    this.form.controls['subCostAmount'].setValue('');
    this.form.controls['subCostCostHead'].setValue('');
  }

  selectCostHead(event: any) {

    let CostHead = event.target.value;

    if (this.activeLogisticsIndex != -1) {
      this.form.controls['subCostCostHead'].setValue(event.target.value);
      let ExistCostingHead = this.projectBudgetCosting.logisticsCostingsList[this.activeLogisticsIndex].logisticsSubCostingList.find((e: any) => e.costHead == CostHead);
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



  onSelectionVehicleNumber(event: any)
  {
    this.form.controls['vehicleNumber'].setValue(event.option.value);
    this.vehicleNumberControl.setValue(event.option.value);
    this.vehicleNumber = event.option.value;
  }



  addUpdateVehicle() {
    if (this.vehicleNumber != '') {
      let vehicleSubCostList:Array<LogisticsSubCosting>=[]
      let vehicle: any = {
        costPerUnit: 0,

        projectBudgetCostingId: this.projectBudgetCosting.projectBudgetCostingId,

        logisticsCostingId: 0,
        logisticsSubCostingList: vehicleSubCostList.splice(0,1),
        vehicleNumber: this.vehicleNumber,
        qty: this.form.controls['qty'].value,
        totLogisticCost:0,
        siteCode: this.projectBudgetCosting.siteCode,
        margin:0

      };
      this.projectBudgetCosting.logisticsCostingsList.push(vehicle);
     
      
    }
    else {

      this.notifyService.showError('Incorrect Vehicle Data');
    }
    this.vehicleNumber = '';
    this.vehicleNumberControl.setValue('');
    this.form.controls['qty'].setValue(0);

    this.canSave = false;
  }
    

    
  removeVehicle(index:number) {

    this.projectBudgetCosting.logisticsCostingsList.splice(index,1);
    this.findTotalsCosts();
    this.activeLogisticsIndex = -1;
    
  }
  


  changeMargin() {
    this.getLogisticsSubCostingList(this.projectBudgetCosting.logisticsCostingsList[this.activeLogisticsIndex], this.activeLogisticsIndex)
    this.findTotalsCosts();

  }
  getCostHead(costHead: string) {
    return this.logisticsExpenceHeadSelectList.find(e => e.costHead == costHead);
  }


  SubCostingOK() {
    console.log(this.projectBudgetCosting.logisticsCostingsList);
    this.activeLogisticsIndex = -1;
    this.canSave = !this.projectBudgetCosting.logisticsCostingsList.find((e: any) => e.totLogisticsCost <= 0);
  }

  cancelEditCosting() {
    this.activeLogisticsIndex = -1;
    this.canSave = !this.projectBudgetCosting.logisticsCostingsList.find((e: any) => e.totLogisticsCost <= 0);
  }

  skippEstimationType() {
    let input: any = {
      projectCode: this.project.projectCode,
      siteCode: this.project.siteCode,
      type: "logistics"
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

