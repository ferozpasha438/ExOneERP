import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ParentOptMgtComponent } from '../../../../sharedcomponent/parentoptmgt.component';
import { OprServicesService } from '../../../opr-services.service';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { NotificationService } from '../../../../services/notification.service';
import { TranslateService } from '@ngx-translate/core';
import { ValidationService } from '../../../../sharedcomponent/ValidationService';
import { ContractFormComponent } from '../../contract-form/contract-form.component';
import { ApiService } from '../../../../services/api.service';
import { UtilityService } from '../../../../services/utility.service';
import { DBOperation } from '../../../../services/utility.constants';
import { CreateUpdateProposalTemplateComponent } from '../../../project-budget-costing/create-update-proposal-template/create-update-proposal-template.component';
import { CreateUpdateProposalCostingsComponent } from '../../../project-budget-costing/create-update-proposal-costings/create-update-proposal-costings.component';
import { CreateUpdateContractFormTemplateComponent } from '../../../contract-form-templates/create-update-contract-form-template/create-update-contract-form-template.component';
import { ConfirmDialogWindowComponent } from '../../../confirm-dialog-window/confirm-dialog-window.component';
import { CustomSelectListItem } from '../../../../models/MenuItemListDto';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-addendum-form',
  templateUrl: './addendum-form.component.html'
})
export class AddendumFormComponent extends ParentOptMgtComponent implements OnInit {

  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;

  referenceNumber: string = "000/0000";    //  CustomerCode/PvReqNumber


  project: any;
  projectData: any;
  customerData: any;
  companyData: any;
 
  city: any;
  
  grandTotal: number = 0;
 
  siteCodeSelectList: Array<any> = [];
 
  templateData: any;

  type: string = "";         //ForAddingResource,ForRemoveResource
  templatesSelectionList: Array<any> = [];

  skillSetsList: Array<CustomSelectListItem> = [];

  contractFormData: any = { contractClauses: [], contractFormHead:''};
  isArabic: boolean = false;
  pvRequest: any;
  date: Date = new Date();
  resourceCostings: any;
  constructor(public datepipe: DatePipe,private oprService: OprServicesService, private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, public dialog: MatDialog,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<ContractFormComponent>) {
    super(authService);
  }

  ngOnInit(): void {

    this.loadInitialData();


  }


  loadCosting() {


    switch (this.type) {

      case "ForAddingResources": this.loadAddResourceCosting(); break;
      case "ForRemovingResources": this.getSkillsetOfEmployee(this.pvRequest.employeeNumber); break;
    }

  }
  loadAddResourceCosting() {

    this.apiService.get('PvAddResource/getPvAddResourceReqById', this.pvRequest.id).subscribe(res => {
      if (res != null) {
         res.totalCost = 0;
        for (let i = 0; i < res?.resourceList.length; i++) {
        res.totalCost += res?.resourceList[i].pricePerUnit *res?.resourceList[i].qty;
        }
        this.resourceCostings = res as any;

      }
    });


  }
  loadSkillSetsList() {
    this.apiService.getall('Skillset/GetSelectSkillsetList/').subscribe(res => {

      this.skillSetsList = res as Array<any>;

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
          this.templateData = res;
          for (let i = 0; i < res.clauses.length; i++) {
            res.clauses[i].id = 0;
          }


          this.contractFormData.contractClauses = res.clauses.slice();

          this.contractFormData.contractFormHead = res.templateHead;
          let contractFormId = this.contractFormData.contractFormHead.id;

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
    this.apiService.getall(`Project/getProjectByProjectCode/${this.pvRequest.projectCode}`).subscribe(res => {
      if (res != null) {
        this.projectData = res;



      }
    });


  }

  loadDefaultContractForm() {
    let Input: any = {
      projectCode: this.pvRequest.projectCode,
      siteCode: this.pvRequest.siteCode,
      customerCode: this.pvRequest.customerCode,
      isAdendum:true
    };


    this.apiService.post('ContractForm/getContractForm', Input).subscribe(res => {
      if (res != null) {
        this.contractFormData = res;
      }
    });
  }
  loadCustomerData() {
    this.apiService.getall(`CustomerMaster/getCustomerByCustomerCode/${this.pvRequest.customerCode}`).subscribe(res => {
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
    this.loadDefaultContractForm();
    this.templateData = null;
    this.isArabic = this.utilService.isArabic();
    this.loadSkillSetsList();

    this.loadProjectData();
    this.loadCustomerData();
    this.loadContractTemplates();


   
      this.loadCosting();
   

    this.getReferenceNumber();

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
        this.loadInitialData();
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
        this.loadInitialData();
    });

  }


 





  getReferenceNumber() {
    let typeCode = this.type == "ForAddingResources" ? "AR" : this.type == "ForRemovingResources" ? "RR" : "XX";
    this.referenceNumber = this.pvRequest.projectCode + "/" +this.pvRequest.siteCode + "/" + typeCode + "/" + this.pvRequest.id;
    }



  TranslateTaggedDataEng(str: string): string {
    let customerData: string = "Second: " + this.customerData.custName + " " + "Commercial Register " + this.referenceNumber + "," + " Whose address -" + this.customerData.custAddress1 + "," + this.city?.cityName + "," + " Kingdom of Saudi Arabia, Phone: " + this.customerData?.custPhone1 + ", email:" + this.customerData.custEmail1 + " represented in signing this contract by " + this.customerData.custSalesRep + ".";
    let res1: string = str.replace(/<<customer>>/gi, customerData);

    let CompanyData = "Thanks God alone and then: On " + this.referenceNumber + " - Corresponding to " + this.translate.instant(this.date.toLocaleDateString()) + " A.D. Agreement in city of " + this.city?.cityName + " between both: First: " + this.companyData?.companyName + ", address: " + this.companyData?.companyAddress + ", Kingdom of Saudi Arabia, Phone: " + this.companyData?.phone + "  represented in signing this contract by Mohammed Abdullah Al Mansaky as being COO STS Group.";
    let res2: string = res1.replace(/<<company>>/gi, CompanyData);
    return res2;
  }

  TranslateTaggedDataArb(str: string): string {
    let customerData: string = "ثانيا: - شركة " + this.customerData.custArbName + " " + "سجل تجاري " + this.referenceNumber + "," + "عنوانها" + this.customerData.custAddress1 + "," + this.city?.cityNameAr + "," + " المملكة العربية السعودية ، الهاتف  " + this.customerData?.custPhone1 + ", ايميل" + this.customerData.custEmail1 + " ويمثلها في التوقيع على هذا العقد الأستاذ " + this.customerData?.custSalesRep + ".";
    let res1: string = str.replace(/<<customer>>/gi, customerData);
    let CompanyData = "اهـ الموافق لحمد لله وحده وبعد: - في يوم" + this.referenceNumber + "م تم الاتفاق" + this.translate.instant(this.date.toLocaleDateString()) + " A.D. اتفاقية في مدينة " + this.city?.cityName + "بين الاثنين: أولا:" + this.companyData?.companyName + ", عنوان: " + this.companyData?.companyAddress + ", المملكة العربية السعودية، هاتف: " + this.companyData?.phone + "  ممثلة في توقيع هذا العقد من قبل Mohammed Abdullah Al Mansaky كما يجري  COO STS Group.";
    let res2: string = res1.replace(/<<company>>/gi, CompanyData);
    return res2;
  }
  getSkillSet(code: string) {
    let ss: any = this.skillSetsList.find(e => e.value == code);
    if (this.isArabic) {
      ss.text = ss.textTwo;
    }
    return ss;
  }
  ToDateString(date: any) {

    if (date != null)
      return this.datepipe.transform(date.toString(), 'yyyy-MM-dd')?.toString();
    else
      return "";
  }


  getSkillsetOfEmployee(employeeNumber: string) {
    let input: any = { employeeNumber: employeeNumber, projectCode: this.pvRequest.projectCode, siteCode: this.pvRequest.siteCode, date: this.pvRequest.fromDate }
    this.apiService.post('skillset/employeeSkillsetForMonth', input).subscribe((res: any) => {
      this.pvRequest.nameInEnglish = res?.nameInEnglish;
      this.pvRequest.nameInArabic = res?.nameInArabic;

    });
  }
}

