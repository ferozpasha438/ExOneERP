import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { FormGroup } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { TranslateService } from "@ngx-translate/core";
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';
import { MonthylRoasterForProjectComponent } from '../../monthyl-roaster-for-project/monthyl-roaster-for-project.component';
import { Moment } from 'moment';
import * as moment from 'moment';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
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
export interface ContractDto {
 customerCode:string;
projectCode :string;
paymentTerms:Array<any>;
contractTerms:Array<any>;
branchCode:string;


}

@Component({
  selector: 'app-convert-to-contract',
  templateUrl: './convert-to-contract.component.html'
})
export class ConvertToContractComponent extends ParentOptMgtComponent implements OnInit {
  project: any;
  form: FormGroup;
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  projectStartDate: Date;
  projectEndDate: Date;
  startYear: number;
  startMonth: number;
  startDay: number;
  endYear: number;
  endMonth: number;
  endDay: number;
  noOfDays: number;
  shiftsForSite: Array<any> = [];
  siteCode: string = '';
  siteCodeList: Array<any> = [];
  tableData: Array<any> = [];
  footerData: Array<any> = [];
  monthsDataList: Array<any> = [];
  skillsetsListForSite: Array<any> = [];
  monthlyRoasterForSite: Array<any> = [];
  isRoasterGenerated: boolean = false;



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


  contractTermsList: Array<any> = [];
  paymentTermsList: Array<any> = [];
  paymentTerms: Array<any> = [];
  contractDto: ContractDto;
  balance: number = 0;
  autoPayment: boolean = true;


  minDate: Date;
  maxDate: Date;


  /*liabilityAndInsurance: string = '';*/
  liabilityInsuranceTerms: Array<any> = [];
  terminationClauseTerms: Array<any> = [];


  constructor(private translate:TranslateService,private notifyService: NotificationService,public dialog: MatDialog, private utilService: UtilityService, private apiService: ApiService, private authService: AuthorizeService, private fb: FormBuilder, public dialogRef: MatDialogRef<ConvertToContractComponent>) {
    super(authService);
  }

  ngOnInit(): void {

    //console.log(this.project);
    this.calculateDates();
  
    this.loadContractData();
    //this.loadTableData();
    
   




  }

  loadContractData() {

    if (!this.project?.isAdendum)
    {
      this.apiService.getall(`ProjectContracts/getContractByProjectCode/${this.project.projectCode}`).subscribe(res => {
        this.contractDto = res;
        this.contractTermsList = res.contractTerms.slice();
        this.paymentTermsList = res.paymentTerms.slice();
        this.liabilityInsuranceTerms = this.contractTermsList.filter(e => e.isLiabilityAndInsurance);
        this.terminationClauseTerms = this.contractTermsList.filter(e => e.isTerminationClause);


        // console.log(res);
      });

    }
    else
    {
      this.apiService.getall(`ProjectContracts/getContractByProjectAndSiteCode/${this.project.projectCode}/${this.project.siteCode}`).subscribe(res => {
        this.contractDto = res;
        this.contractTermsList = res.contractTerms.slice();
        this.paymentTermsList = res.paymentTerms.slice();
        this.liabilityInsuranceTerms = this.contractTermsList.filter(e => e.isLiabilityAndInsurance);
        this.terminationClauseTerms = this.contractTermsList.filter(e => e.isTerminationClause);


        // console.log(res);
      });


    }



    

  }

  setForm() {
    this.form = this.fb.group({
      'projectCode': [this.project.projectCode],
      'customerCode': [this.project.customerCode],
      'siteCode': [this.project?.siteCode],
      
      'startDate': [this.project.startDate.toString().substring(0, 10)],
      'endDate': [this.project.endDate.toString().substring(0, 10)],
      'noOfDays': [this.noOfDays],

     // 'contractTerm':[''],
      'liabilityAndInsurance':[''],
      'terminationClause':[''],
      'instDate':[''],
      'amount':[''],
      'particular':['']
    });
    this.loadInitialData();
  }
  closeModel() {
    this.dialogRef.close();
  }
  submit() { }

  calculateDates() {
    



    this.monthsDataList = [];
    let startDate = new Date(this.project.startDate);
    let endDate = new Date(this.project.endDate);


    this.minDate = new Date() > startDate ? startDate : new Date();
    this.maxDate = endDate;





    this.noOfDays = (endDate.getTime() - startDate.getTime()) / (1000 * 60 * 60 * 24) + 1;
    for (let y = startDate.getFullYear(); y <= endDate.getFullYear(); y++) {
      let sm = y == startDate.getFullYear() ? startDate.getMonth() : 1;
      let em = y == endDate.getFullYear() ? endDate.getMonth() : 12;

      for (let m = sm; m <= em; m++) {
        let sd = (m == sm && y == startDate.getFullYear()) ? startDate.getDate() : 1;
        let ed = (m == em && y == endDate.getFullYear()) ? endDate.getDate() : new Date(y, m + 1, 0).getDate();
        let noOfDays = (new Date(y, m, ed).getTime() - new Date(y, m, sd).getTime()) / (1000 * 60 * 60 * 24) + 1;

        let monthData: any = {
          sd: sd,
          ed: ed,
          mStartDate: new Date(y, m, sd).toDateString(),
          mEndDate: new Date(y, m, ed).toDateString(),
          mNoOfDays: noOfDays

        };
        this.monthsDataList.push(monthData);
      }
    }
    this.setForm();
   

  
  }

  loadInitialData() {

    if (this.project?.isAdendum) {
      this.apiService.getall(`projectBudgetCosting/getProjectSiteEstimation/${this.project.customerCode}/${this.project.projectCode}/${this.project.siteCode}`).subscribe(res => {
        if (res != null) {
          this.projectBudgetEstimation = res;

          this.findTotalCosts();
        }
        else {
          this.projectBudgetEstimation.siteWisePBCListForProject = [];
          this.projectBudgetEstimation.grandTotalCostForProject = -1;
        }

        this.loadTableData();
      });
    }
    else {
      this.apiService.getall(`projectBudgetCosting/getProjectEstimation/${this.project.customerCode}/${this.project.projectCode}`).subscribe(res => {
        if (res != null) {
          this.projectBudgetEstimation = res;

          this.findTotalCosts();
        }
        else {
          this.projectBudgetEstimation.siteWisePBCListForProject = [];
          this.projectBudgetEstimation.grandTotalCostForProject = -1;
        }

        this.loadTableData();
      });
    }

  }












  loadTableData() {
  

    this.tableData = [];
  
   
    for (let m = 0; m < this.monthsDataList.length; m++) {

  

        let tablerow: any = {
          sd: this.monthsDataList[m].sd,
          ed: this.monthsDataList[m].ed,
          startDate: this.monthsDataList[m].mStartDate,
          endDate: this.monthsDataList[m].mEndDate,
          noOfDays: this.monthsDataList[m].mNoOfDays,
          amount: ((this.projectBudgetEstimation.grandTotalCostForProject / this.noOfDays) * this.monthsDataList[m].mNoOfDays).toFixed(0)
        };


        this.tableData.push(tablerow);



    }

    this.paymentTermsList = [];
    for (var i = 0; i < this.tableData.length; i++) {
      let pt: any = {
        customerCode: this.project.customerCode,
        projectCode: this.project.projectCode,
        siteCode: this.project?.isAdendum ? this.project.siteCode : null,
        branchCode: this.project.branchCode,

        instDate: this.convertStrToDate(this.tableData[i].startDate),
        particular: 'Payment-'+(i+1),
        amount: this.tableData[i].amount,
        noOfDays: this.tableData[i].noOfDays,
        

        //monthStartDate: this.convertStrToDate(this.tableData[i].startDate),
        //monthEndDate: this.convertStrToDate(this.tableData[i].endDate),

      };
      this.paymentTermsList.push(pt);
    }










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

            prc.costPerUnit = 0;
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
            plc.costPerUnit = 0;
            plc.logisticsSubCostingList.forEach((plsc:any) => {

              plc.costPerUnit = plc.costPerUnit + plsc.amount;

            });
            plc.totLogisticsCost = plc.costPerUnit;
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

  //  this.form.controls['amount'].setValue(this.remainingBalance());
    this.balance = this.remainingBalance();
  }





  removeContractTerm(i: number) {

    this.contractTermsList.splice(i, 1);

  }
removeLiabilityAndInsuranceTerm(i: number) {

  this.liabilityInsuranceTerms.splice(i, 1);

  }
removeTerminationClauseTerm(i: number) {

  this.terminationClauseTerms.splice(i,1);
  }



  addUpdateContractTerm() {


    if (this.form.controls['liabilityAndInsurance'].value != '' || this.form.controls['terminationClause'].value!='') {
      let term: any = {

        contractTerm: this.form.controls['liabilityAndInsurance'].value != ''?this.form.controls['liabilityAndInsurance'].value : this.form.controls['terminationClause'].value,
        customerCode: this.project.customerCode,
        projectCode: this.project.projectCode,
        siteCode: this.project?.isAdendum ? this.project.siteCode : null,
        branchCode: this.project.branchCode,
        isLiabilityAndInsurance: this.form.controls['liabilityAndInsurance'].value != '',
        isTerminationClause: this.form.controls['terminationClause'].value != '',

      }

      this.contractTermsList.push(term);
      this.form.controls['liabilityAndInsurance'].setValue('');
      this.form.controls['terminationClause'].setValue('');
    }
    else this.notifyService.showWarning(`${this.translate.instant("Enter_Contract_Terms")}`, `${this.translate.instant("Warning")}`);

  }
  addUpdateLiabilityAndInsuranceTerm() {


    if (this.form.controls['liabilityAndInsurance'].value != '') {
      let term: any = {

        contractTerm: this.form.controls['liabilityAndInsurance'].value,
        customerCode: this.project.customerCode,
        projectCode: this.project.projectCode,
        sitCode: this.project?.isAdendum? this.project.siteCode : null,
        branchCode: this.project.branchCode,
        isLiabilityAndInsurance: true,
        isTerminationClause: false,

      }

      this.liabilityInsuranceTerms.push(term);
      this.form.controls['liabilityAndInsurance'].setValue('');
      this.form.controls['terminationClause'].setValue('');
    }
    else this.notifyService.showWarning(`${this.translate.instant("Enter_Contract_Terms")}`, `${this.translate.instant("Warning")}`);

  }
  addUpdateTerminationClauseTerm() {


    if (this.form.controls['terminationClause'].value!='') {
      let term: any = {

        contractTerm: this.form.controls['terminationClause'].value,
        customerCode: this.project.customerCode,
        projectCode: this.project.projectCode,
        siteCode:  this.project?.isAdendum?this.project.siteCode:null,
        branchCode: this.project.branchCode,
        isLiabilityAndInsurance:false,
        isTerminationClause: true,

      }

      this.terminationClauseTerms.push(term);
      this.form.controls['liabilityAndInsurance'].setValue('');
      this.form.controls['terminationClause'].setValue('');
    }
    else this.notifyService.showWarning(`${this.translate.instant("Enter_Contract_Terms")}`, `${this.translate.instant("Warning")}`);

  }


  generateContractForProject() {
    this.contractTermsList = [];
    this.contractTermsList = this.liabilityInsuranceTerms.concat(this.terminationClauseTerms).slice();
    //this.contractTermsList.concat(this.terminationClauseTerms);

    if (this.contractTermsList.length == 0) {
      this.notifyService.showWarning(`${this.translate.instant("Enter_Contract_Terms")}`, `${this.translate.instant("Warning")}`);

    }

    else if (!this.autoPayment && this.paymentTerms.length == 0) {
      this.notifyService.showWarning(`${this.translate.instant("Enter")}` +" "+ `${this.translate.instant("Payment_Terms")}`, `${this.translate.instant("Warning")}`);

    }
     else {


      


      let contDto: any = {
        customerCode: this.project.customerCode,
        projectCode: this.project.projectCode,
        siteCode: this.project?.isAdendum ? this.project.siteCode : null,
        paymentTerms: this.autoPayment ? this.paymentTermsList : this.paymentTerms,
        contractTerms: this.contractTermsList,
        branchCode: this.project.branchCode,
      };

      this.contractDto = contDto;
     
      if (!this.project?.isAdendum) {
        this.apiService.post('ProjectContracts', this.contractDto)
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
        this.apiService.post('ProjectContracts/CreateSiteContract', this.contractDto)       //for adendum site
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
  }

  convertStrToDate(str:string) {
  var date = new Date(str),
    mnth = ("0" + (date.getMonth() + 1)).slice(-2),
    day = ("0" + date.getDate()).slice(-2);
  return [date.getFullYear(), mnth, day].join("-");
  }
  convertDateToString(dateString: any) {
    return new Date(dateString);
  }


  openDatePicker(dp: any) {
    dp.open();
  }

  addUpdatePaymentTerm() {

    
    if (this.form.controls['instDate'].value != '' && this.form.controls['amount'].value != '' && this.form.controls['particular'].value != '') {
      if (this.form.controls['amount'].value <= this.balance) {
        let payterm: any = {

          customerCode: this.project.customerCode,
          projectCode: this.project.projectCode,
          siteCode: this.project?.isAdendum?this.project.siteCode:null,
          branchCode: this.project.branchCode,
          //instDate: this.form.controls['instDate'].value.toString().slice(0, 16),
          instDate: this.form.controls['instDate'].value,
          particular: this.form.controls['particular'].value,
          amount: this.form.controls['amount'].value
        }
       
        this.paymentTerms.push(payterm);
        this.balance = this.remainingBalance();
       // this.form.controls['instDate'].setValue('');
        this.form.controls['particular'].setValue('');
        this.form.controls['amount'].setValue('');

      }
      else
        this.notifyService.showWarning(`${this.translate.instant("Payment_Exceeding_Balance")}`, `${this.translate.instant("Warning")}`);
      this.form.controls['amount'].setValue('');
    }
    else {
      this.notifyService.showWarning(`${this.translate.instant("Enter_Payment_Terms")}`, `${this.translate.instant("Warning")}`);
     
    }
    this.balance = this.remainingBalance();
  }
  removPaymentTerm(i: number) {

    this.paymentTerms.splice(i, 1);
    this.balance = this.remainingBalance();
  }
  remainingBalance()
  {
    //console.log(this.projectBudgetEstimation.grandTotalCostForProject);
    let paytotal = 0;
    let bal: number = 0;
    if (this.paymentTerms.length == 0)
      return this.projectBudgetEstimation.grandTotalCostForProject;
    this.paymentTerms.forEach((e:any) => {

      paytotal += e.amount;
    });
   bal= this.projectBudgetEstimation.grandTotalCostForProject - paytotal;
    return bal;
  }
  changeAutoPayment(): boolean {
    this.autoPayment =!this.autoPayment;
    return this.autoPayment;
  }


}
