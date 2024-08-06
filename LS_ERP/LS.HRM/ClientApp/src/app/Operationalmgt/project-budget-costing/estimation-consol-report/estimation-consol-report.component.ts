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
import { DBOperation } from '../../../services/utility.constants';
export interface TblSndDefSiteMasterDto {
  siteCode: string;
  siteName: string;
  siteArbName: string;

}






export interface TblOpProjectBudgetEstimationDto {

  projectBudgetEstimationId: number;
  customerCode: string;
  projectCode: string;
  PreviousEstimatonId: number;
  siteWisePBCListForProject: Array<PBCForSiteDto>;
  grandTotalCostForProject: number;

}

export interface PBCForSiteDto {
  siteData: TblSndDefSiteMasterDto;
  totRcForSite: number;
  totLcForSite: number;
  totMcForSite: number;
  totFcForSite: number;
  prcListForSite: Array<ResourceCosting>;
  plcListForSite: Array<LogisticsCosting>;
  pmcListForSite: Array<MaterialEquipmentCosting>;
  pfcListForSite: Array<FinancialExpenseCosting>;
  grandTotalCostForSite: number;
}









export interface ProjectBudgetCosting {
  customerCode: string;
  projectBudgetCostingId: number;
  projectBudgetEstimationId: number;
  projectCode: string;
  siteCode: string;
  resourceCostingsList: Array<ResourceCosting>;
  logisticsCostingsList: Array<LogisticsCosting>;
  materialEquipmentCostingsList: Array<MaterialEquipmentCosting>;
  financialExpenseCostingsList: Array<FinancialExpenseCosting>;
  serviceType: string;
  status: number;

}
export interface ResourceCosting {
  costPerUnit: number;

  projectBudgetCostingId: number;
  quantity: number;
  resourceCostingId: number;
  resourceSubCostingList: Array<ResourceSubCosting>;
  skillsetCode: string;
  totResourceCost: number;
  siteCode: string;
}
export interface ResourceSubCosting {

  amount: number;
  costHead: string;
  resourceCostingId: number;
  resourceSubCostingId: number;
}


export interface FinancialExpenseCosting {
  costPerUnit: number;
  projectBudgetCostingId: number;
  financialExpenseCostingId: number;
  financialExpenseCode: string;
  siteCode: string;
}
export interface LogisticsCosting {
  costPerUnit: number;

  projectBudgetCostingId: number;

  logisticsCostingId: number;
  logisticsSubCostingList: Array<LogisticsSubCosting>;
  vehicleNumber: string;
  totLogisticsCost: number;
  siteCode: string;
  qty:number
}

export interface LogisticsSubCosting {

  amount: number;
  costHead: string;
  logisticsCostingId: number;
  logisticsSubCostingId: number;
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
  selector: 'app-estimation-consol-report',
  templateUrl: './estimation-consol-report.component.html'
})
export class EstimationConsolReportComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;

  project: any;

  projectBudgetCosting: ProjectBudgetCosting;
  projectBudgetEstimation: TblOpProjectBudgetEstimationDto;
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
  skillsetList: Array<any>;
  opeartionsExpenseHeadsList: Array<any>;
  materialEquipmentList: Array<any>;
  logisticsVehicleList: Array<any>;
  customerData: any;
  isArab: boolean = false;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<EstimationConsolReportComponent>) {
    super(authService);
  }

  ngOnInit(): void {
    console.log(this.project);
    this.isArab = this.utilService.isArabic();
    this.loadSkillSetsList();
    this.loadOperationsExpenseHeads();
    this.loadMaterialEquipments();
    this.loadLogisticsVehicles();
   
    this.loadInitialData();
    this.getCustomerData();


  }





  closeModel() {
    this.dialogRef.close();
  }
  submit() {


  }


  loadInitialData() {

    if (!this.project?.isAdendum) {
      this.apiService.getall(`projectBudgetCosting/getProjectEstimation/${this.project.customerCode}/${this.project.projectCode}`).subscribe(res => {
        if (res != null) {
          this.projectBudgetEstimation = res;
          this.findTotalCosts();
        }
        else {
          this.projectBudgetEstimation.siteWisePBCListForProject = [];
          this.projectBudgetEstimation.grandTotalCostForProject = -1;
        }
        if (this.modalBtnTitle == 'Print') {

          this.printComponent();



        }
      });
    }
    else {

      this.apiService.getall(`projectBudgetCosting/getProjectSiteEstimation/${this.project.customerCode}/${this.project.projectCode}/${this.project.siteCode}`).subscribe(res => {
        if (res != null) {
          this.projectBudgetEstimation = res;
          this.findTotalCosts();
        }
        else {
          this.projectBudgetEstimation.siteWisePBCListForProject = [];
          this.projectBudgetEstimation.grandTotalCostForProject = -1;
        }
        if (this.modalBtnTitle == 'Print') {

          this.printComponent();



        }
      });



    }

   


   
  }

  printComponent() {
    let printable:string = "printableArea";

    const printContent = document.getElementById(printable) as HTMLElement;
    const WindowPrt: any = window.open('', '', 'left=0,top=0,width=2000,height=1000,toolbar=0,scrollbars=0,status=0');
    setTimeout(() => {
      WindowPrt.document.write(printContent.innerHTML);
      WindowPrt.document.close();
      WindowPrt.focus();
      WindowPrt.print();
      WindowPrt.close();
    }, 50);







  }


  findTotalCosts() {
    this.projectBudgetEstimation.grandTotalCostForProject = 0;
    this.projectBudgetEstimation.siteWisePBCListForProject.forEach((pbe:any) => {
      pbe.totFcForSite = 0;
      pbe.totLcForSite = 0;
      pbe.totMcForSite = 0;
      pbe.totRcForSite = 0;


      pbe.grandTotalCostForSite = 0;
      if (pbe.prcListForSite.length != 0) {

        pbe.prcListForSite.forEach((prc:any) => {
          prc.totResourceCost = 0;
          if (prc.resourceSubCostingList.length != 0) {

          //  prc.costPerUnit = 0;
            prc.costPerUnit = prc.margin;
            prc.resourceSubCostingList.forEach((prsc:any) => {
              prc.costPerUnit = prc.costPerUnit + prsc.amount;
            });
            prc.totResourceCost = prc.costPerUnit * prc.quantity

          }
          pbe.totRcForSite = pbe.totRcForSite + prc.totResourceCost;

        });
      }
      if (pbe.plcListForSite.length != 0) {

        pbe.plcListForSite.forEach((plc:any) => {
          plc.totLogisticsCost = 0;

          if (plc.logisticsSubCostingList.length != 0) {
            plc.costPerUnit = plc.margin;
            plc.logisticsSubCostingList.forEach((plsc:any) => {

              plc.costPerUnit = plc.costPerUnit + plsc.amount;

            });
            plc.totLogisticsCost = plc.costPerUnit*plc.qty;
          }
          pbe.totLcForSite = pbe.totLcForSite + plc.totLogisticsCost;
        });

      }
      if (pbe.pmcListForSite.length != 0) {
        pbe.pmcListForSite.forEach((pmc:any) => {
          pmc.totMaterialEquipmentCost = 0;
          if (pmc.materialEquipmentSubCostingList.length != 0) {
            pmc.costPerUnit = 0;
            pmc.materialEquipmentSubCostingList.forEach((pmsc:any) => {
              pmc.costPerUnit = pmc.costPerUnit + pmsc.amount;
            });
            pmc.totMaterialEquipmentCost = pmc.costPerUnit * pmc.quantity;

          }
          pbe.totMcForSite = pbe.totMcForSite + pmc.totMaterialEquipmentCost;
        });
      }
      if (pbe.pfcListForSite.length != 0) {

        pbe.pfcListForSite.forEach((pfc:any) => {
          pbe.totFcForSite = pbe.totFcForSite + pfc.costPerUnit;

        });

      }
      pbe.grandTotalCostForSite = pbe.grandTotalCostForSite + pbe.totFcForSite + pbe.totLcForSite + pbe.totMcForSite + pbe.totRcForSite;

      this.projectBudgetEstimation.grandTotalCostForProject = this.projectBudgetEstimation.grandTotalCostForProject + pbe.grandTotalCostForSite;
    });

  

  }




  loadSkillSetsList() {
    this.apiService.getall('Skillset/getAllSkillsetList').subscribe(res => {
      this.skillsetList = res;
    });
  }

  getSkillset(code: string): string{
    let ss: any = this.skillsetList.find(e => e.skillSetCode == code);
    return ss;

  }
  loadOperationsExpenseHeads() {
    this.apiService.getall('OperationExpenseHead/getSelectOperationExpenseHeadList').subscribe(res => {
      this.opeartionsExpenseHeadsList = res;
    });
  }

  getopeartionsExpenseHead(code: string): string{
    let eh: any = this.opeartionsExpenseHeadsList.find(e => e.value == code);
    return eh;

  }
  loadMaterialEquipments() {
    this.apiService.getall('Materialequipment/getSelectMaterialequipmentList').subscribe(res => {
      this.materialEquipmentList = res;
    });
  }

  getMaterialEquipments(code: string): string{
    let me: any = this.materialEquipmentList.find(e => e.value == code);
    return me;

  }
  loadLogisticsVehicles() {
    this.apiService.getall('Logisticsandvehicle/getSelectLogisticsandvehicleList').subscribe(res => {
      this.logisticsVehicleList = res;
     
    });
  }

  getLogisticsVehicle(code: string): any{
    let me: any = this.logisticsVehicleList.find((e:any) => e.value == code);
    return me;

  }


  getCustomerData() {
    this.apiService.getall(`CustomerMaster/getCustomerByCustomerCode/${this.project.customerCode}`).subscribe(res => {
      if (res != null) {
        this.customerData = res;
       
      }
      else {
        this.notifyService.showError(this.translate.instant("Error"));
        console.log("No Customer Found");
      }

    });

  }


}
