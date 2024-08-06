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
import { CreateUpdateProposalTemplateComponent } from '../../project-budget-costing/create-update-proposal-template/create-update-proposal-template.component';
import { CreateUpdateProposalCostingsComponent } from '../../project-budget-costing/create-update-proposal-costings/create-update-proposal-costings.component';
import { ConfirmDialogWindowComponent } from '../../confirm-dialog-window/confirm-dialog-window.component';
import { OprServicesService } from '../../opr-services.service';
import { CreateUpdateContractFormComponent } from './create-update-contract-form/create-update-contract-form.component';
import { CreateUpdateContractFormTemplateComponent } from '../../contract-form-templates/create-update-contract-form-template/create-update-contract-form-template.component';


@Component({
  selector: 'app-contract-form',
  templateUrl: './contract-form.component.html'
})
export class ContractFormComponent extends ParentOptMgtComponent implements OnInit {

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

  type: string ="";         //ForProject,ForAddingSite,ForAddingResource,ForRemoveResource
  templatesSelectionList: Array<any> = [];


  contractFormData: any;
  isArabic: boolean = false;

  date: Date = new Date();
  constructor(private oprService: OprServicesService,private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, public dialog: MatDialog,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<ContractFormComponent>) {
    super(authService);
  }

  ngOnInit(): void {

    this.loadData();



  }
  loadData() {

    this.templateData = null;
    this.isArabic = this.utilService.isArabic();
    this.loadCustomerData();
    this.loadProjectData();

    this.loadSkillSetsList();
    this.loadOperationsExpenseHeads();
    this.loadMaterialEquipments();
    this.loadLogisticsVehicles();
    this.loadContractForm();
    this.loadProposalCostingData();
    this.loadContractTemplates();
  }
  loadContractForm() {
   // let siteCode = this.project?.siteCode == null ? "-NA-" : this.project?.siteCode;
    let Input: any = {
      projectCode: this.project.projectCode,
      siteCode: this.project.siteCode,
      customerCode: this.project.customerCode,
      isAdendum: this.project?.isAdendum??false
    };

      
    // this.apiService.getall(`ContractForm/getContractForm/${this.project.customerCode}/${this.project.projectCode}/${siteCode}`).subscribe(res => {
    this.apiService.post('ContractForm/getContractForm', Input).subscribe(res => {
      if (res != null) {
        this.contractFormData = res;
      }
    });
  }

  loadContractTemplates() {
   
    this.apiService.getall(`ContractFormTemplates/getSelectionContractFormTemplates/${this.type}`).subscribe(res => {
      if (res != null) {
        this.templatesSelectionList = res;
      }
    });


  }
  OnTemplateSelection(event: any) {
    if (event.target.value != '') {
      let Id: number = Number(event.target.value);
      this.apiService.get('ContractFormTemplates/getContractFormTemplateById', Id).subscribe(res => {
        if (res != null) {
          //res.coveringLetter = res.coveringLetter.replace(/<<customer>>/gi, this.customerData.custName);
          this.templateData = res;
          for (let i = 0; i < res.clauses.length; i++) {
            res.clauses[i].id = 0;
          }


          this.contractFormData.contractClauses = res.clauses;
          let contractFormId = this.contractFormData.contractFormHead.id;

          this.contractFormData.contractFormHead = res.templateHead;
          this.contractFormData.contractFormHead.id = contractFormId;


        }
      });
    }
    else {
      this.templateData = null;
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
        console.log(res);
        this.apiService.getall(`Company/getCompanyByCityCode/${res.custCityCode1}`).subscribe(res2 => {
          if (res2 != null) {
            this.companyData = res2;
            console.log(res2);
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




  loadProposalCostingData() {
    let Input: any = {
      customerCode: this.project.customerCode,
      siteCode: this.project.siteCode,
      projectCode: this.project.projectCode,
      isAdendum: this.project?.isAdendum
    };
    // this.apiService.getall(`proposalCosting/getProposalCosting/${this.project.customerCode}/${this.project.projectCode}/${this.project.siteCode}`).subscribe(res => {
    this.apiService.post('proposalCosting/getProposalCosting', Input).subscribe((res:any) => {
      this.proposalCosting = res as Array<any>;
      
      for (let i = 0; i < res.length;i++) {
        this.grandTotal += res[i].price * res[i].qty;
      }


      this.getReferenceNumber();
    });
  }













  getCity(cityCode: string) {

    this.apiService.getall(`City/getStateCountrybyCityCode/${cityCode}`).subscribe(res => {
      this.city = res;
      console.log(res);
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
            plc.totLogisticsCost = plc.costPerUnit * plc.qty;
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


  private openDialogManage3(contractFormHeaderId: number, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, component: any) {
    let dialogRef = this.oprService.fullWindow(this.dialog, component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = contractFormHeaderId;
    (dialogRef.componentInstance as any).project = this.project;
    (dialogRef.componentInstance as any).contractFormData = this.contractFormData;


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
  createContractTemplate() {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, CreateUpdateContractFormTemplateComponent);
    (dialogRef.componentInstance as any).modalTitle = "Create Template";
    (dialogRef.componentInstance as any).modalBtnTitle = "Add";
    (dialogRef.componentInstance as any).contractTemplateData = this.contractFormData;
    (dialogRef.componentInstance as any).id = 0;
    (dialogRef.componentInstance as any).type = this.type;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.loadData();
    });

  }
  editContractTemplate() {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, CreateUpdateContractFormTemplateComponent);
    (dialogRef.componentInstance as any).modalTitle = "Update Template";
    (dialogRef.componentInstance as any).modalBtnTitle = "Update";
    (dialogRef.componentInstance as any).contractTemplateData = this.templateData;
    (dialogRef.componentInstance as any).id = this.templateData.templateId;
    (dialogRef.componentInstance as any).type = this.type;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.loadData();
    });

  }


  editProposalCosting() {
    let inpuData: any = {
      projectCode: this.project.projectCode,
      customerCode: this.project.customerCode,
      siteCode: this.project.siteCode == null ? "" : this.project.siteCode,
      proposalCosting: this.proposalCosting

    }
    this.openDialogManage2(inpuData, DBOperation.create, 'Edit_Proposal_Costing', 'Edit');
  }



  

  getReferenceNumber() {
    if (this.proposalCosting?.length > 0) {

      this.referenceNumber = this.project.projectCode + "/" + (this.project?.isAdendum ? this.project.siteCode+"/":"") + this.proposalCosting[0]?.projectBudgetEstimationId.toString();


      if (this.proposalCosting[0].id == 0) {
        this.editProposalCosting();
      }
    }



  }


  private openConfirmationDialog(dbops: DBOperation, modalTitle: string, Component: any, operation: string) {
    let dialogRef = this.oprService.confirmationDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
   
  



    dialogRef.afterClosed().subscribe(res => {
      this.contractFormData.contractFormHead.projectCode = this.project.projectCode;
      this.contractFormData.contractFormHead.customerCode = this.project.customerCode;
      this.contractFormData.contractFormHead.siteCode = this.project?.isAdendum ? this.project.siteCode : null;
      this.contractFormData.contractFormHead.isAdendum = this.project?.isAdendum;
      if ((res && res == true) || res.res) {
        if (operation == "SaveContractForm")  
        {
          this.apiService.post('contractForm', this.contractFormData)
            .subscribe(res2 => {
              if (res2) {

                this.utilService.OkMessage();
                this.loadData();
                //   this.dialogRef.close(true);

              }
            },
              error => {
                console.error(error);
                this.utilService.ShowApiErrorMessage(error);
              });


        }
        else if (operation == "ApproveContractForm")
        {
          let input: any = {
            projectCode: this.project.projectCode,
            siteCode: this.project.isAdendum? this.project.siteCode:null,
            customerCode: this.project.customerCode,
            contractFormHeadId: this.contractFormData.contractFormHead.id

          };

          this.apiService.post('projectContracts/approveContractForm', input)
            .subscribe(res2 => {
              

                this.utilService.OkMessage();
                this.loadData();
                

             
            },
              error => {
                console.error(error);
                this.utilService.ShowApiErrorMessage(error);
              });


        }
       


      }
    });
  }






  saveContractForm() {
    if (this.contractFormData != null) {
      this.openConfirmationDialog(DBOperation.create, 'Are_You_Sure?', ConfirmDialogWindowComponent, "SaveContractForm");
    }
    else {
      this.notifyService.showError(this.translate.instant("No Contract Data Found"));
    }

  }
  editContractForm() {
    if (this.contractFormData != null) {
      this.openDialogManage3(this.contractFormData.contractFormHead.id, DBOperation.create, 'Update_Contract_Form', "Update", CreateUpdateContractFormComponent);
    }
    else {
      this.notifyService.showError(this.translate.instant("No Contract Data Found"));
    }

  }
  approveContract() {
    if (this.contractFormData != null) {
      this.openConfirmationDialog(DBOperation.create, 'Are_You_Sure?', ConfirmDialogWindowComponent, "ApproveContractForm");
    }
    else {
      this.notifyService.showError(this.translate.instant("No Contract Data Found"));
    }

  }

  TranslateTaggedDataEng(str: string): string {
    let customerData: string = "Second: " + this.customerData?.custName + " " + "Commercial Register " + this.referenceNumber +  "," + " Whose address -" + this.customerData.custAddress1 + "," + this.city?.cityName + ","+ " Kingdom of Saudi Arabia, Phone: " + this.customerData?.custPhone1 + ", email:" + this.customerData.custEmail1 + " represented in signing this contract by " + this.customerData?.custSalesRep + ".";
    let res1: string = str.replace(/<<customer>>/gi, customerData);

    let CompanyData = "Thanks God alone and then: On " + this.referenceNumber + " - Corresponding to " + this.translate.instant(this.date.toLocaleDateString()) + " A.D. Agreement in city of " + this.city?.cityName + " between both: First: " + this.companyData?.companyName + ", address: " + this.companyData?.companyAddress + ", Kingdom of Saudi Arabia, Phone: " + this.companyData?.phone + "  represented in signing this contract by Mohammed Abdullah Al Mansaky as being COO STS Group.";
    let res2: string = res1.replace(/<<company>>/gi, CompanyData);
    return res2;
 }
  
  TranslateTaggedDataArb(str: string): string {
    let customerData: string = "ثانيا: - شركة " + this.customerData.custArbName + " " + "سجل تجاري " + this.referenceNumber + "," + "عنوانها" + this.customerData.custAddress1 + "," + this.city?.cityNameAr + "," + " المملكة العربية السعودية ، الهاتف  " + this.customerData?.custPhone1 + ", ايميل" + this.customerData.custEmail1+ " ويمثلها في التوقيع على هذا العقد الأستاذ " + this.customerData?.custSalesRep + ".";
    let res1: string = str.replace(/<<customer>>/gi, customerData);
    let CompanyData = "اهـ الموافق لحمد لله وحده وبعد: - في يوم" + this.referenceNumber + "م تم الاتفاق" + this.translate.instant(this.date.toLocaleDateString()) + " A.D. اتفاقية في مدينة " + this.city?.cityName + "بين الاثنين: أولا:" + this.companyData?.companyName + ", عنوان: " + this.companyData?.companyAddress + ", المملكة العربية السعودية، هاتف: " + this.companyData?.phone + "  ممثلة في توقيع هذا العقد من قبل Mohammed Abdullah Al Mansaky كما يجري  COO STS Group.";
    let res2: string = res1.replace(/<<company>>/gi, CompanyData);
    return res2;
 }

  }

