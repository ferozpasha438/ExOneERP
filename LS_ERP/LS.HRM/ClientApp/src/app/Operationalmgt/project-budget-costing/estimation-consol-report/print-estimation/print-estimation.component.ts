import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { UtilityService } from '../../../../services/utility.service';
import { TranslateService } from '@ngx-translate/core';
import { ApiService } from '../../../../services/api.service';
import { ValidationService } from '../../../../sharedcomponent/ValidationService';
import { NotificationService } from '../../../../services/notification.service';
import { DBOperation } from '../../../../services/utility.constants';
import { ParentOptMgtComponent } from '../../../../sharedcomponent/parentoptmgt.component';

@Component({
  selector: 'app-print-estimation',
  templateUrl: './print-estimation.component.html'
})
export class PrintEstimationComponent extends ParentOptMgtComponent implements OnInit {

  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;

  project: any;
  projectData: any;
  customerData: any;
  projectBudgetCosting: any;
  projectBudgetEstimation: any;
  city: any;
  resourceSubCostingList: Array<any> = [];
  resourceCostingList: Array<any> = [];
  grandTotal: number = 0;
  totalForResourceUnit: number = -1;
  resourceSelectList: Array<any> = [];
  resourceExpenceHeadSelectList: Array<any> = [];
  resourceCode: string = '';
  quantity: number;
  activeResourceIndex: number = -1;
  siteCodeSelectList: Array<any> = [];
  skillsetList: Array<any>;
  opeartionsExpenseHeadsList: Array<any>;
  materialEquipmentList: Array<any>;
  logisticsVehicleList: Array<any>;
  isArab: boolean = false;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<PrintEstimationComponent>) {
    super(authService);
  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.loadProjectData();
    this.loadCustomerData();
    this.loadSkillSetsList();
    this.loadOperationsExpenseHeads();
    this.loadMaterialEquipments();
    this.loadLogisticsVehicles();
    this.loadInitialData();



  }






  submit() {


  }

  loadProjectData() {
    this.apiService.getall(`Project/getProjectByProjectCode/${this.project.projectCode}`).subscribe(res => {
      if (res != null) {
        this.projectData = res;



      }
    });


  }
  loadCustomerData() {
    this.apiService.getall(`CustomerMaster/getCustomerByCustomerCode/${this.project.customerCode}`).subscribe(res => {
      if (res != null) {
        this.customerData = res;
        this.getCity(res.custCityCode1);
      }
    });


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
        
      });



    }


  }

  getCity(cityCode: string) {

    this.apiService.getall(`City/getStateCountrybyCityCode/${cityCode}`).subscribe(res => {
      this.city = res;

    });

  }



  printComponent() {
    let printable: string = "printableArea";

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

  getSkillset(code: string): string {
    let ss: any = this.skillsetList.find(e => e.skillSetCode == code);
    return ss;

  }
  loadOperationsExpenseHeads() {
    this.apiService.getall('OperationExpenseHead/getSelectOperationExpenseHeadList').subscribe(res => {
      this.opeartionsExpenseHeadsList = res;
    });
  }

  getopeartionsExpenseHead(code: string): string {
    let eh: any = this.opeartionsExpenseHeadsList.find(e => e.value == code);
    return eh;

  }
  loadMaterialEquipments() {
    this.apiService.getall('Materialequipment/getSelectMaterialequipmentList').subscribe(res => {
      this.materialEquipmentList = res;
    });
  }

  getMaterialEquipments(code: string): string {
    let me: any = this.materialEquipmentList.find(e => e.value == code);
    return me;

  }
  loadLogisticsVehicles() {
    this.apiService.getall('Logisticsandvehicle/getSelectLogisticsandvehicleList').subscribe(res => {
      this.logisticsVehicleList = res;
     
    });
  }

  getLogisticsVehicle(code: string): any {
    let me: any = this.logisticsVehicleList.find(e => e.value == code);
    return me;

  }


  printInvoice() {
    this.openPrint();
  }
  openPrint() {
    const printContent = document.getElementById("printcontainer") as HTMLElement;
    const WindowPrt: any = window.open('', '', 'left=0,top=0,width=2000,height=1000,toolbar=0,scrollbars=0,status=0');
    setTimeout(() => {
      WindowPrt.document.write(printContent.innerHTML);
      WindowPrt.document.close();
      WindowPrt.focus();
      WindowPrt.print();
      WindowPrt.close();
    }, 50);

  }

  closeModel() {
    this.dialogRef.close();
  }

}
