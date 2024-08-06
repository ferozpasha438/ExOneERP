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
import { CreateUpdateProposalTemplateComponent } from '../create-update-proposal-template/create-update-proposal-template.component';
import { CreateUpdateProposalCostingsComponent } from '../create-update-proposal-costings/create-update-proposal-costings.component';

@Component({
  selector: 'app-print-proposal',
  templateUrl: './print-proposal.component.html'
})
export class PrintProposalComponent extends ParentOptMgtComponent implements OnInit {

  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;

  referenceNumber: string = "000/0000";

  project: any;
  projectData: any;
  customerData: any;
  companyData: any;
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
  proposalCosting: Array<any>;
  templateData: any;
  templatesSelectionList: Array<any> = [];

  isArabic: boolean = false;

  date: Date = new Date();
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, public dialog: MatDialog,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<PrintProposalComponent>) {
    super(authService);
  }

  ngOnInit(): void {

    this.loadData();

    

  }
  loadData() {

    this.templateData = null;
    this.isArabic = this.utilService.isArabic();

    this.loadProjectData();
    this.loadCustomerData();

    this.loadSkillSetsList();
    this.loadOperationsExpenseHeads();
    this.loadMaterialEquipments();
    this.loadLogisticsVehicles();
    //this.loadInitialData();
    this.loadProposalCostingData();
    this.loadProposalTemplates();
  }


  loadProposalTemplates() {
    let Input:any = {
      projectCode:this.project.projectCode,
      siteCode:this.project.siteCode,
      customerCode:this.project.customerCode,

    }
    //this.apiService.getall(`proposalTemplates/getSelectionProposalTemplates/${this.project.customerCode}/${this.project.projectCode}/""`).subscribe(res => {
    this.apiService.post('ProposalTemplates/getSelectionProposalTemplates', Input).subscribe(res => {
      if (res != null) {
        this.templatesSelectionList = res as Array<any>;
      }
    });


  }
  OnTemplateSelection(event:any) {
    if (event.target.value != '') {
      let Id: number = Number(event.target.value);
      this.apiService.get('proposalTemplates/getProposalTemplateById', Id ).subscribe(res => {
        if (res != null) {
          res.coveringLetter = res.coveringLetter.replace(/<<customer>>/gi, this.customerData.custName);
          
          

          this.templateData = res;
          console.log(this.templateData.coveringLetter);
        }
      });
    }
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

        this.apiService.getall(`Company/getCompanyByCityCode/${res.custCityCode1}`).subscribe(res2 => {
          if (res2 != null) {
            this.companyData = res2;
          }
          });
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
    else
    {
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




  loadProposalCostingData() {
    if (!this.project?.isAdendum) {
      this.project.siteCode = null;
    }

    let Input: any = {
      customerCode: this.project.customerCode,
      siteCode: this.project.siteCode,
      projectCode: this.project.projectCode,
      isAdendum:this.project?.isAdendum
    };
   // this.apiService.getall(`proposalCosting/getProposalCosting/${this.project.customerCode}/${this.project.projectCode}/${this.project.siteCode}`).subscribe(res => {
    this.apiService.post('proposalCosting/getProposalCosting',Input).subscribe(res => {
      this.proposalCosting = res as Array<any>;
      console.log(res);
      this.getReferenceNumber();
      });
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
    this.projectBudgetEstimation.siteWisePBCListForProject.forEach((pbe: any) => {
      pbe.totFcForSite = 0;
      pbe.totLcForSite = 0;
      pbe.totMcForSite = 0;
      pbe.totRcForSite = 0;


      pbe.grandTotalCostForSite = 0;
      if (pbe.prcListForSite.length != 0) {

        pbe.prcListForSite.forEach((prc: any) => {
          prc.totResourceCost = 0;
          if (prc.resourceSubCostingList.length != 0) {

            prc.costPerUnit = prc.margin;
            prc.resourceSubCostingList.forEach((prsc: any) => {
              prc.costPerUnit = prc.costPerUnit + prsc.amount;
            });
            prc.totResourceCost = prc.costPerUnit * prc.quantity

          }
          pbe.totRcForSite = pbe.totRcForSite + prc.totResourceCost;

        });
      }
      if (pbe.plcListForSite.length != 0) {

        pbe.plcListForSite.forEach((plc: any) => {
          plc.totLogisticsCost = 0;

          if (plc.logisticsSubCostingList.length != 0) {
            plc.costPerUnit = plc.margin;
            plc.logisticsSubCostingList.forEach((plsc: any) => {

              plc.costPerUnit = plc.costPerUnit + plsc.amount;

            });
            plc.totLogisticsCost = plc.costPerUnit*plc.qty;
          }
          pbe.totLcForSite = pbe.totLcForSite + plc.totLogisticsCost;
        });

      }
      if (pbe.pmcListForSite.length != 0) {
        pbe.pmcListForSite.forEach((pmc: any) => {
          pmc.totMaterialEquipmentCost = 0;
          if (pmc.materialEquipmentSubCostingList.length != 0) {
            pmc.costPerUnit = 0;
            pmc.materialEquipmentSubCostingList.forEach((pmsc: any) => {
              pmc.costPerUnit = pmc.costPerUnit + pmsc.amount;
            });
            pmc.totMaterialEquipmentCost = pmc.costPerUnit * pmc.quantity;

          }
          pbe.totMcForSite = pbe.totMcForSite + pmc.totMaterialEquipmentCost;
        });
      }
      if (pbe.pfcListForSite.length != 0) {

        pbe.pfcListForSite.forEach((pfc: any) => {
          pbe.totFcForSite = pbe.totFcForSite + pfc.costPerUnit;

        });

      }
      pbe.grandTotalCostForSite = pbe.grandTotalCostForSite + pbe.totFcForSite + pbe.totLcForSite + pbe.totMcForSite + pbe.totRcForSite;

      this.projectBudgetEstimation.grandTotalCostForProject = this.projectBudgetEstimation.grandTotalCostForProject + pbe.grandTotalCostForSite;
    });



  }

  private openDialogManage(inpuData: any, dbops: DBOperation, modalTitle: string, modalBtnTitle: string) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, CreateUpdateProposalTemplateComponent);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).inputData = inpuData;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.loadData();
    });
  }
  private openDialogManage2(inpuData: any, dbops: DBOperation, modalTitle: string, modalBtnTitle: string) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, CreateUpdateProposalCostingsComponent);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).inputData = inpuData;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.loadData();
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
    this.dialogRef.close(true);
  }
  createProposalTemplate() {
    let inpuData: any = {
      projectCode: this.project.projectCode,
      customerCode: this.project.customerCode,
      siteCode: this.project.siteCode == null ? "" : this.project.siteCode,
      id:0

    }

    this.openDialogManage(inpuData, DBOperation.create, 'Create_New_Proposal_Template', 'Add');



  }


  editProposalCosting() {
    let inpuData: any = {
      projectCode: this.project.projectCode,
      customerCode: this.project.customerCode,
      siteCode: this.project.siteCode == null ? "" : this.project.siteCode,
      proposalCosting:this.proposalCosting

    }
    this.openDialogManage2(inpuData, DBOperation.create, 'Edit_Proposal_Costing', 'Edit');
  }



  editProposalTemplate() {
    let inpuData: any = {
      projectCode: this.project.projectCode,
      customerCode:this.project.customerCode,
      siteCode: this.project.siteCode,
      id: this.templateData.id,

    }

    this.openDialogManage(inpuData, DBOperation.create, 'Update_Proposal_Template', 'Update');



  }
  
  getReferenceNumber() {
    if (this.proposalCosting.length > 0) {

      this.referenceNumber = this.project.customerCode + "/" + this.proposalCosting[0].projectBudgetEstimationId.toString();
      if (this.proposalCosting[0].id == 0) {
        this.editProposalCosting();
      }
    }



  }

}
