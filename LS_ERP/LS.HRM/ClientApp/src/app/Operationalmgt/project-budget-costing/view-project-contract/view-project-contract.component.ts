import { Component, OnInit } from '@angular/core';
import { Validators } from '@angular/forms';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';

@Component({
  selector: 'app-view-project-contract',
  templateUrl: './view-project-contract.component.html'
})
export class ViewProjectContractComponent extends ParentOptMgtComponent implements OnInit {
  contractTermsList: Array<any>=[];
  paymentTermsList: Array<any>=[];
  contractDto: any;
  project: any;
  form: FormGroup;
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;


  liabilityInsuranceTerms: Array<any> = [];
  terminationClauseTerms: Array<any> = [];

  customerData: any;
  city: any;
  projectData: any;
  isArab: boolean = false;
  constructor(private translate: TranslateService, private notifyService: NotificationService, public dialog: MatDialog, private utilService: UtilityService, private apiService: ApiService, private authService: AuthorizeService, private fb: FormBuilder, public dialogRef: MatDialogRef<ViewProjectContractComponent>) {
    super(authService);
  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.setForm();
    this.getCustomerData();
    this.loadProjectData();
  }

  setForm() {
    this.form = this.fb.group({
      'projectCode': [this.project.projectCode],
      'customerCode': [this.project.customerCode],
      'branchCode': [this.project.branchCode],
      'startDate': [this.project.startDate.toString().substring(0, 10)],
      'endDate': [this.project.endDate.toString().substring(0, 10)],
      
         });
    this.loadContractData();
  }


  getCustomerData() {
    this.apiService.getall(`CustomerMaster/getCustomerByCustomerCode/${this.project.customerCode}`).subscribe(res => {
      if (res != null) {
        this.customerData = res;
        this.getCity(res.custCityCode1);
        console.log(res);
      }
      else {
        this.notifyService.showError(this.translate.instant("Error"));
        console.log("No Customer Found");
      }

    });

  }

  getCity(cityCode: string) {

    this.apiService.getall(`City/getStateCountrybyCityCode/${cityCode}`).subscribe(res => {
      this.city = res;

    });

  }


  loadProjectData() {
    this.apiService.getall(`Project/getProjectByProjectCode/${this.project.projectCode}`).subscribe(res => {
      if (res != null) {
        this.projectData = res;



      }
    });
  }

  loadContractData() {
    if (!this.project?.isAdendum) {
      this.apiService.getall(`ProjectContracts/getContractByProjectCode/${this.project.projectCode}`).subscribe(res => {
        this.contractDto = res;
        this.contractTermsList = res.contractTerms.slice();
        this.paymentTermsList = res.paymentTerms.slice();
        this.liabilityInsuranceTerms = this.contractTermsList.filter(e => e.isLiabilityAndInsurance);
        this.terminationClauseTerms = this.contractTermsList.filter(e => e.isTerminationClause);
      });


    }
    else {
      this.apiService.getall(`ProjectContracts/getContractByProjectAndSiteCode/${this.project.projectCode}/${this.project.siteCode}`).subscribe(res => {
        this.contractDto = res;
        this.contractTermsList = res.contractTerms.slice();
        this.paymentTermsList = res.paymentTerms.slice();
        this.liabilityInsuranceTerms = this.contractTermsList.filter(e => e.isLiabilityAndInsurance);
        this.terminationClauseTerms = this.contractTermsList.filter(e => e.isTerminationClause);
      });



    }

  }

  closeModel() {
    this.dialogRef.close();
  }
  submit() { }
  noOfdays(paymentTerm:any):number {

    let nofDays = (Date.parse(paymentTerm.monthEndDate) - Date.parse(paymentTerm.monthStartDate)) / (1000 * 60 * 60 * 24) + 1;
    return nofDays;
  }

  dateFormat(date: string):string {

    return date.slice(0,10);

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
}
