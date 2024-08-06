import { Component, OnInit } from '@angular/core';


import { MatDialog, MatDialogRef } from '@angular/material/dialog';

import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { DatePipe } from '@angular/common';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { UtilityService } from '../../../services/utility.service';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { DBOperation } from '../../../services/utility.constants';
import { ApiService } from '../../../services/api.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-addupdate-customer-vist-form',
  templateUrl: './addupdate-customer-vist-form.component.html'
})
export class AddupdateCustomerVistFormComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number=0;

  readonly: string = "";

  projectData: any;


  resultData: any;


  siteCodeList: Array<CustomSelectListItem> = [];

  action: string = '';       
  empNumber: string = '';
  isDataLoading: boolean = false;
  filteredCustCodes: Observable<Array<CustomSelectListItem>>;
  custCodeControl = new FormControl('', Validators.required);
  isArabic: boolean = false;

  userSelectionList: Array<any> = [];
  reasonCodeSelectionList: Array<any> = [];
  isFromProjectsAction: boolean = false;

  projectsList: Array<CustomSelectListItem> = [];
  filterCustCodes(val: string): Observable<Array<CustomSelectListItem>> {
    return this.apiService.getall(`CustomerMaster/getSelectCustomerList?search=${val}`)
      .pipe(
        map(response => {
          const res = response as Array<CustomSelectListItem>;
          this.isDataLoading = false;
          return res;
        })
      )
  }

 
  CustomerData: any;
  CompanyData: any;
  constructor(private validationService: ValidationService,public datepipe: DatePipe, private translate: TranslateService, private fb: FormBuilder, private authService: AuthorizeService, private utilService: UtilityService, private apiService: ApiService, public dialogRef: MatDialogRef<AddupdateCustomerVistFormComponent>) {

    super(authService);

    this.filteredCustCodes = this.custCodeControl.valueChanges.pipe(
      startWith(''),
      debounceTime(utilService.autoDelay()),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isDataLoading = true;
        return this.filterCustCodes(val || '')
      })
    );

  }

  ngOnInit(): void {
    this.isDataLoading = true;
    this.setForm();
    this.LoadReasonCodeSelectionList();
    this.LoadUserSelectionList();
    this.isArabic = this.utilService.isArabic();


    if (this.id > 0) {
      this.readonly = "readonly";
      this.editForm();
      // this.loadSiteCodes(this.form.controls['siteCode'].value);
    }
    else {
      this.readonly = "";
      this.isDataLoading = false;

    }


  }

  LoadReasonCodeSelectionList() {
    this.apiService.getall('ReasonCode/getSelectReasonCodeListForCustomerVisit').subscribe(res => {
      if (res != null) {
        this.reasonCodeSelectionList = res as Array<any>;
         
        this.reasonCodeSelectionList.forEach(e => {
          e.text = this.isArabic? e.textTwo + "-" + e.value:e.text+"-"+e.value;
          });
        
      }
    });

  }
  LoadUserSelectionList() {
    this.apiService.getall('Users/GetUserSelectionList').subscribe(res => {
      if (res != null) {
        this.userSelectionList = res as Array<any>;

      }
    });

    
  }
  setForm() {

    this.form = this.fb.group({

      'customerCode': ['', Validators.required],
      'projectCode': ['', Validators.required],
      'siteCode': ['', Validators.required],
      'reasonCode': ['', Validators.required],
      'contactNumber': ['',Validators.compose([Validators.required, this.validationService.mobileValidator])],
     
      'supervisorId': ['', Validators.required],
      'scheduleDateTime': ['', Validators.required],
      'isOpen': [true],
      'isClosed': [false],
      'isInprogress': [false],
      'id': [this.id],
      
    });


    if (this.isFromProjectsAction) {
      this.custCodeControl.setValue(this.projectData.customerCode);
      this.form.controls['siteCode'].setValue(this.projectData.siteCode);
      this.form.controls['projectCode'].setValue(this.projectData.projectCode);
      this.form.controls['customerCode'].setValue(this.projectData.customerCode);
      this.form.controls['customerCode'].disable({ onlySelf: true });
      this.custCodeControl.disable({ onlySelf: true });
      this.form.controls['projectCode'].disable({ onlySelf: true });
      this.form.controls['siteCode'].disable({ onlySelf: true });
     this.loadCustomerData(this.projectData.customerCode);
      this.loadProjectsList(this.projectData.customerCode);
      this.loadSiteCodes(this.projectData.projectCode);
    }

    if (this.action == "visit") {
      this.form.controls['customerCode'].disable({ onlySelf: true });
      this.custCodeControl.disable({ onlySelf:true });
      this.form.controls['projectCode'].disable({ onlySelf: true });
      this.form.controls['siteCode'].disable({ onlySelf: true });
      this.form.controls['reasonCode'].disable({ onlySelf: true });
      this.form.controls['contactNumber'].disable({ onlySelf: true });
      this.form.controls['supervisorId'].disable({ onlySelf: true });
      this.form.controls['scheduleDateTime'].disable({ onlySelf: true });


      this.form.addControl("supervisorRemarks", new FormControl('', Validators.required));
      this.form.addControl("customerRemarks", new FormControl('', Validators.required));
      this.form.addControl("actionTerms", new FormControl('', Validators.required));
      this.form.addControl("customerNotes", new FormControl('', Validators.required));
      this.form.addControl("visitedBy", new FormControl('', Validators.required));
      this.form.addControl("visitedDateTime", new FormControl('', Validators.required));


      
    }
    


  }
  loadCustomerData(customerCode:string) {

    this.apiService.getall('CustomerMaster/getCustomerByCustomerCode/' + customerCode).subscribe(res => {
      if (res != null) {
        this.CustomerData = res as any;
        this.form.controls['contactNumber'].setValue(res.custMobile1);
        this.loadCompanyData(res);
      }
    });
  }
  loadCompanyData(CustomerData:any) {
    
    this.apiService.getall(`Company/getCompanyByCityCode/${CustomerData.custCityCode1}`).subscribe(res2 => {
      if (res2 != null) {
        this.CompanyData = res2;
      }
    });
  }



  onSelectionCustomerCode(event: any, op: number) {
    let custCode: string = '';
    if (op == 1) { custCode = event.option.value; }
    else if (op == 2) { custCode = event.target.value; }
    else if (op == 3) { custCode = event }

    this.apiService.getall('CustomerMaster/getCustomerByCustomerCode/' + custCode).subscribe(res => {
      if (res != null) {

        let custCode = this.custCodeControl.value as string;


        this.form.controls['customerCode'].setValue(custCode);
        this.form.controls['contactNumber'].setValue(res.custMobile1);

        this.loadProjectsList(custCode);

      }
      else {

        this.form.controls['customerCode'].setValue('');


      }
      this.form.controls['projectCode'].setValue('');
      this.form.controls['siteCode'].setValue('');
      this.projectsList = [];
      this.siteCodeList = [];

    });

  }
 
  onSelectionProjectCode() {


    this.loadSiteCodes(this.form.controls['projectCode'].value);

    if (this.form.controls['projectCode'].value != '') {
      this.isDataLoading = true;

      this.form.controls['siteCode'].setValue('');
    
    }
    this.isDataLoading = false;

  }

  getProjectData() {

    this.apiService.getall(`ProjectSites/getProjectSiteByProjectAndSiteCode/${this.form.controls['projectCode'].value}/${this.form.controls['siteCode'].value}`).subscribe(res => {
      this.projectData = res;
    });
  }




  loadSiteCodes(projectCode: string) {


    this.isDataLoading = true;

    this.apiService.getall(`CustomerSite/getSelectSiteListByProjectCode/${projectCode}`).subscribe(res => {
      this.siteCodeList = res;
      this.isDataLoading = false;



    });
  }
  loadProjectsList(custCode: string) {


    this.isDataLoading = true;

    this.apiService.getall(`project/getSelectProjectListByCustomerCode/${custCode}`).subscribe(res => {
      this.projectsList = res;
      this.isDataLoading = false;



    });
  }


  editForm() {
    this.isDataLoading = true;

    this.apiService.get('CustomerVisitForm/getById', this.id).subscribe(res => {

      if (res != null) {
        this.resultData = res as any;
        this.form.patchValue(res);


        this.custCodeControl.setValue(res.customerCode);

        //this.loadProjectsList(res.customerCode);
        //this.loadSiteCodes(res.projectCode);


      // this.loadCustomerData(res.customerCode);

        this.form.controls['projectCode'].setValue(res.projectCode);
        this.form.controls['customerCode'].setValue(res.customerCode);
        this.form.controls['siteCode'].setValue(res.siteCode);
        this.form.controls['contactNumber'].setValue(res.contactNumber);
        this.form.controls['supervisorId'].setValue(res.supervisorId.toString());
        if(res.visitedBy==null || res.visitedBy==0)
          this.form.controls['visitedBy'].setValue('');
        else
          this.form.controls['visitedBy'].setValue(res.visitedBy.toString());

        this.getProjectData();


        this.isDataLoading = false;





      }


    });

  }
  closeModel() {
    this.dialogRef.close();
  }

  submit() {

    this.form.value['customerCode'] = this.custCodeControl.value;
    this.form.value['projectCode'] = this.form.controls['projectCode'].value;
    this.form.value['siteCode'] = this.form.controls['siteCode'].value;
    this.form.value['action'] = this.action;



    if (this.form.valid) {
      this.apiService.post('CustomerVisitForm', this.form.value)
        .subscribe(res => {
          if (res) {

            this.utilService.OkMessage();

            this.dialogRef.close(true);

          }
        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
          });
    }
    else {
      this.utilService.FillUpFields();
    }





  }

  onSelectionSiteCode() {
    if (this.form.controls['siteCode'].value != '') {

      this.getProjectData();

     
    }
  }


  translateToolTip(text: string) {
    return this.translate.instant(text);
  }


  ToDateString(date: any) {

    if (date != null)
      return this.datepipe.transform(date.toString(), 'yyyy-MM-dd')?.toString();
    else
      return "";
  }


  openDatePicker(dp: any) {
    dp.open();
  }




  print() {
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
}
