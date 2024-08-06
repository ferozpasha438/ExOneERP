import { HttpClient } from '@angular/common/http';
import { Component, OnChanges, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { from, Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, mergeMap, startWith, switchMap } from 'rxjs/operators';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
//import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';
import { ValidationService } from '../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-addupdate-enquiry-form',
  templateUrl: './addupdate-enquiry-form.component.html'
})
export class AddupdateEnquiryFormComponent extends ParentOptMgtComponent implements OnInit {
  readonly: string = "";

  showApprove: boolean = true;
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
  unitCode: string;
  siteCode: string;
  branchCode: string;
  siteCodeList: Array<CustomSelectListItem> = [];
  siteCodeListAll: Array<CustomSelectListItem> = [];
  unitCodeList: Array<CustomSelectListItem> = [];
  serviceCodeList: Array<CustomSelectListItem> = [];
  customerCode: string = '';
  filteredCustCodes: Observable<Array<CustomSelectListItem>>;
  custCodeControl = new FormControl('', Validators.required);
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




  branchCodeList: Array<CustomSelectListItem> = [];
  saleRep: string;
  isDataLoading: boolean = false;
  invoiceItemObject: any;
  listOfEnquiries: Array<any> = [];
  sequence: number = 1;
  editsequence: number = 0;
  enquiryNumber: string ='';

  serviceCode: string = '';
  serviceQuantity: number = 0;
  pricePerUnit: number = 0;
  unitQuantity: number = 0;
  estimatedPrice: number = 0;
  totalEstPrice: number = 0;
  statusEnquiry: string = "Open";
  serviceNameArb: string = '';
  serviceNameEng: string = '';
  custAddress: string = '';
  isActive: boolean = true;

  stusEnquiryHead: string = "Open";
  enquiryHead: any;
  isOldEnquiryForm: boolean= false;

  isArabic: boolean = false;

  isAdednum: boolean=false;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<AddupdateEnquiryFormComponent>) {
    super(authService);



    this.filteredCustCodes = this.custCodeControl.valueChanges.pipe(
      startWith(this.customerCode),
      debounceTime(utilService.autoDelay()),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isDataLoading = true;
        return this.filterCustCodes(val || '')
      })
    );





  }

  loadBranchCodes() {
   
    this.apiService.getall('Branch/getSelectBranchCodeList').subscribe((res: Array<any>)=> {
      console.log(res);
     // this.branchCodeList = [];
      this.apiService.getall('OpAuthorities/getOpAuthoritiesListByUserId').subscribe((res2: Array<any>) => {
        console.log(res2);
       
       if (res2.length == 0) {
        
         this.utilService.ShowApiErrorMessage(this.translate.instant("You_Can_Not_Create_Enquiry"));
         this.dialogRef.close(false);
       }
       else {
         for (let i = 0; i < res2.length;i++) {
           let branch = res.find(b => b.value == res2[i].branchCode);
           if (branch != null && this.branchCodeList.findIndex(c => c.value == branch.branchCode) == -1) {
             this.branchCodeList.push(branch);

           }

         }
         console.log(this.branchCodeList);

       }
       
     });

     
      
    });
  }



  loadSiteCodes(custCode: string) {
    this.apiService.getall(`CustomerSite/getSelectSiteListByCustCode/${custCode}`).subscribe(res => {
      this.siteCodeList = res;
      this.siteCodeListAll = res;
      if (this.id > 0) {
        this.loadSiteListNotCovertedAsProject();
      }
    });
  }
  loadServiceCodes() {
    this.apiService.getall('Services/getSelectServiceList').subscribe(res => {
      this.serviceCodeList = res;
   
    });
  }


  loadUnitCodes(serviceCode:string) {
    this.apiService.getall('Units/getSelectUnitListByServiceCode/'+serviceCode).subscribe(res => {
      this.unitCodeList = res;
      console.log(res);
    });
  }


  ngOnInit(): void {
    this.isArabic = this.utilService.isArabic();
      this.setForm();
    if (this.enquiryHead != null)
      this.setEditForm();
    this.readonly = "readonly";
    this.loadBranchCodes();

  }

  setForm() {

    this.form = this.fb.group({
      //'': ['', Validators.required],
      /*   EnquiryFormHeader*/
      'sitecode': [''],
      'enquiryNumber': ['XXXXXXXXXX'],
      'customerCode': ['', Validators.required],
      'branchCode': ['', Validators.required],
      'dateOfEnquiry': ['', Validators.required],
      'estimateClosingDate': ['', Validators.required],
      'userName': [''],
      'totalEstPrice': [],
      'remarks': [''],
      'stusEnquiryHead': ["Open"],
      'isActive': [true],

      /* Auto*/
      'saleRep': [''],
      'custAddress': [''],
      'serviceNameArb': [''],
      'serviceNameEng': [''],

      'serviceCode': [''],
      'unitCode': [''],


      //EnquiryDetails





    });
    this.setToDefault();
  }

  submit() {


    let sd = new Date(this.form.controls['dateOfEnquiry'].value);
    let ed = new Date(this.form.controls['estimateClosingDate'].value);
    sd.setMinutes(sd.getMinutes() - sd.getTimezoneOffset());
    ed.setMinutes(ed.getMinutes() - ed.getTimezoneOffset());


    this.form.value['dateOfEnquiry'] = sd;
    this.form.value['estimateClosingDate'] = ed;




    if (this.form.valid && this.listOfEnquiries.length > 0) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      this.form.value['enquiriesList'] = this.listOfEnquiries;
      this.form.value['custAddress'] = this.custAddress;
      this.form.value['estimatedPrice'] = this.estimatedPrice;
      this.form.value['serviceNameArb'] = this.serviceNameArb;
      this.form.value['serviceNameEng'] = this.serviceNameEng;

      this.apiService.post('ServiceEnquiryForm', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.dialogRef.close(true);
        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
          });
    }
    else
      this.utilService.FillUpFields();

  }

  setEditForm() {
    this.form.controls['customerCode'].disable({ onlySelf: true });
    this.custCodeControl.disable({ onlySelf: true });
        this.apiService.getall(`ServiceEnquiryHeader/getEnquiryFormHeaderByEnquiryNumber/${this.enquiryHead.enquiryNumber}`).subscribe(res => {
      if (res) {
        
        this.custCodeControl.setValue(this.enquiryHead.customerCode);
          this.customerCode = this.enquiryHead.customerCode;
        this.form.controls['customerCode'].setValue(this.enquiryHead.customerCode);
       
        this.form.patchValue(res);
        this.form.patchValue({ 'id': res.id });
        this.id = res.id;
        this.getEnquiriesList();
        this.loadSiteCodes(this.enquiryHead.customerCode);
        this.totalEstPrice = res.totalEstPrice;
        this.onSelectionCustomerCode(this.enquiryHead.customerCode, 3);
      }
    });
    


  }

  getEnquiriesList() {
    this.apiService.getall(`ServiceEnquiries/getSevriceEnquiriesByEnquiryNumber/${this.enquiryHead.enquiryNumber}`).subscribe((res:Array<any>) => {
      if (res) {
        this.listOfEnquiries = res as Array<any>;
        if (res.findIndex(e => e.isApproved) >= 0) {
          this.loadSiteListNotCovertedAsProject();
        }

      }
    });

  }


  loadSiteListNotCovertedAsProject() {
    this.enquiryNumber = this.form.value['enquiryNumber'];
    this.apiService.getall(`projectSites/getSelectSiteListWhicAreNotConvertedAsProjectByEnquiryNumber/${this.enquiryNumber}`).subscribe(res => {
      this.siteCodeList = res;

    });
  }

  addEnquiry() {

    this.enquiryNumber = this.form.value['enquiryNumber'];
    if (/*this.enquiryNumber != "" && */this.siteCode != null && this.siteCode != "" && this.serviceCode != null && this.serviceCode != "" && this.unitCode != null && this.unitCode != "" && this.serviceQuantity > 0 && this.unitQuantity > 0 && this.pricePerUnit>0 && this.statusEnquiry!="") {
      this.estimatedPrice = this.pricePerUnit * this.serviceQuantity * this.unitQuantity;
      this.totalEstPrice = this.totalEstPrice+this.estimatedPrice;
      this.listOfEnquiries.push({
        sequence: this.getSequence(),
        enquiryID:0,
        enquiryNumber: this.enquiryNumber,
        siteCode: this.siteCode,
        serviceCode: this.serviceCode,
        unitCode: this.unitCode,
        serviceQuantity: this.serviceQuantity,
        unitQuantity: this.unitQuantity,
        estimatedPrice: this.estimatedPrice,
        statusEnquiry: this.statusEnquiry,
        serviceNameArb: this.serviceNameArb,
        serviceNameEng: this.serviceNameEng,
        pricePerUnit: this.pricePerUnit,
        totalEstPrice: this.totalEstPrice,
        isAssignedSurveyor:false

      });
      this.form.value['totalEstPrice'] = this.totalEstPrice;
      this.form.value['enquiriesList'] = this.listOfEnquiries;
      this.setToDefault();
      
     
    }


  }
  removeEnquiry(i: number) {

    this.totalEstPrice = this.totalEstPrice - this.listOfEnquiries[i].estimatedPrice;
    this.form.value['totalEstPrice'] = this.totalEstPrice;
    this.setToDefault();
    this.listOfEnquiries.splice(i, 1);



    this.downSequence();
  }



  setToDefault() {

    this.siteCode = this.serviceCode = this.serviceNameEng = this.serviceNameArb = this.unitCode = "";
    this.estimatedPrice = this.serviceQuantity = this.pricePerUnit = this.unitQuantity = 0; this.isActive = true;
   

  }
  getSequence(): number { return this.sequence += this.sequence + 1 };
  downSequence(): number { return this.sequence += this.sequence - 1 };



  reset() {
    this.form.controls['enquiryNumber'].setValue('');
    this.form.controls['customerCode'].setValue('');
    this.form.controls['siteCode'].setValue('');
    this.form.controls['unitCode'].setValue('');
    this.form.controls['pricePerUnit'].setValue(0);

    this.form.controls['dateOfEnquiry'].setValue('');
    this.form.controls['estimateClosingDate'].setValue('');
    this.form.controls['userName'].setValue('');
    this.form.controls['totalEstPrice'].setValue(0);
    this.form.controls['stusEnquiryHead'].setValue('');
    this.form.controls['remarks'].setValue('');
    this.form.controls['isActive'].setValue(true);
    this.form.controls['saleRep'].setValue('');
    this.form.controls['custAddress'].setValue('');


  }

  closeModel() {
    this.dialogRef.close();
  }

  onSelectionCustomerCode(event: any, op: number) {
    let custCode: string='';
    if (op == 1) { custCode = event.option.value; }
    else if (op == 2) {custCode = event.target.value;}
    else if (op == 3) {custCode = event}
    
    this.apiService.getall('CustomerMaster/getCustomerByCustomerCode/' + custCode).subscribe(res => {
      if (res != null) {
        this.form.patchValue({ 'custAddress': res['custAddress1'], 'saleRep': res['custSalesRep'], 'customerCode': res['custCode'] });
        let custCode = this.custCodeControl.value as string;

        this.form.value['customerCode'] = custCode;

        this.loadSiteCodes(custCode);
        this.loadServiceCodes();
      }/*, error => {*/
      //}
      else {

        this.form.controls['customerCode'].setValue('');
        this.form.controls['custAddress'].setValue('');
        this.form.controls['saleRep'].setValue('');
        this.custCodeControl.setValue('');
        this.siteCodeList = [];
      }

    });

  }
  getServiceDetails(event: any) {
    if (event.target.value != "") {
      const serviceCode = event.target.value;
      this.apiService.getall('Services/getServiceByServiceCode/' + serviceCode).subscribe(res => {
        this.serviceNameEng = res['serviceNameEng'],
          this.serviceNameArb = res['serviceNameArb'],
          this.serviceCode = serviceCode
      });
      this.loadUnitCodes(serviceCode);
    }
    else {
      this.unitCodeList = [];
     
    }
    this.serviceQuantity = 0;
    this.unitQuantity = 0;
    this.pricePerUnit = 0;
    this.estimatedPrice = 0;
  }


  getServiceUnitPrice(event: any) {
    if (event.target.value != "") {
      const unitCode = event.target.value;
      this.apiService.getall('ServiceUnitMap/getServiceUnitMapByServiceAndUnitCode?UnitCode=' + unitCode + '&ServiceCode=' + this.serviceCode).subscribe(res => {
        this.pricePerUnit = res['pricePerUnit'],
          this.unitCode = unitCode
      });
    }
    else {
      this.serviceQuantity = 0;
      this.unitQuantity = 0;
      this.pricePerUnit = 0;
      this.estimatedPrice = 0;}
   

  }

  calculateEstimatePrice(event: any, value: string) {
    if (event !== null) {
      let target = event.target.value;

      this.estimatedPrice = value == 'serviceQuantity' ? this.pricePerUnit * this.unitQuantity * target : value == 'pricePerUnit' ? target * this.serviceQuantity * this.unitQuantity : value == 'unitQuantity' ? target * this.serviceQuantity * this.pricePerUnit: 0;
    }
    else
      this.estimatedPrice = this.pricePerUnit * this.serviceQuantity * this.unitQuantity;
  }

  getService(serviceCode:string) {

    return this.serviceCodeList.find(e => e.value == serviceCode);
  
  }
  getSite(siteCode:string) {

    return this.siteCodeListAll.find(e => e.value == siteCode);
  }
}



