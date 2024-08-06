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

@Component({
  selector: 'app-addupdate-customer-complaint',
  templateUrl: './addupdate-customer-complaint.component.html'
})
export class AddupdateCustomerComplaintComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;

  readonly: string = "";

  projectData: any;


  resultData: any;


  siteCodeList: Array<CustomSelectListItem> = [];

  action: string = '';
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


  proofForComplaintUrl: string | ArrayBuffer | null = null;
  proofForActionUrl: string | ArrayBuffer | null = null;




  constructor(public datepipe: DatePipe, private translate: TranslateService, private fb: FormBuilder, private authService: AuthorizeService, private utilService: UtilityService, private apiService: ApiService, public dialogRef: MatDialogRef<AddupdateCustomerComplaintComponent>) {

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
    }
    else {
      this.readonly = "";
      this.isDataLoading = false;

    }


  }

  LoadReasonCodeSelectionList() {
    this.apiService.getall('ReasonCode/getSelectReasonCodeListForCustomerComplaint').subscribe(res => {
      if (res != null) {
        this.reasonCodeSelectionList = res as Array<any>;

        this.reasonCodeSelectionList.forEach(e => {
          e.text = this.isArabic ? e.textTwo + "-" + e.value : e.text + "-" + e.value;
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
      'complaintBy': ['', Validators.required],
      'complaintDescription': ['', Validators.required],
      'proofForComplaintFileName': [''],
      'proofForComplaintIForm': [''],
      
      'isActionRequired': [false],
      'complaintDate': ['', Validators.required],
      'bookedBy': ['', Validators.required],
      'proofForAction': [''],
      'proofForComplaint': [''],
      'branchCode': [''],
     


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

    if (this.action == "closing") {
      this.form.controls['customerCode'].disable({ onlySelf: true });
      this.custCodeControl.disable({ onlySelf: true });
      this.form.controls['projectCode'].disable({ onlySelf: true });
      this.form.controls['siteCode'].disable({ onlySelf: true });
      this.form.controls['reasonCode'].disable({ onlySelf: true });
      this.form.controls['complaintBy'].disable({ onlySelf: true });
      this.form.controls['complaintDescription'].disable({ onlySelf: true });
      this.form.controls['isActionRequired'].disable({ onlySelf: true });
      this.form.controls['complaintDate'].disable({ onlySelf: true });
      this.form.controls['bookedBy'].disable({ onlySelf: true });
      this.form.controls['proofForComplaintIForm'].disable({ onlySelf: true });


      this.form.addControl("closingDate", new FormControl('', Validators.required));
      this.form.addControl("actionDescription", new FormControl('', Validators.required));
      this.form.addControl("proofForActionIForm", new FormControl(''));
      this.form.addControl("proofForActionFileName", new FormControl(''));
    }



  }
  loadCustomerData(customerCode: string) {

    this.apiService.getall('CustomerMaster/getCustomerByCustomerCode/' + customerCode).subscribe(res => {
      if (res != null) {
        this.CustomerData = res as any;
        this.loadCompanyData(res);
      }
    });
  }
  loadCompanyData(CustomerData: any) {

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

    if (this.isFromProjectsAction) {

      this.form.controls['customerCode'].disable({ onlySelf: true });
      this.custCodeControl.disable({ onlySelf: true });
      this.form.controls['projectCode'].disable({ onlySelf: true });
      this.form.controls['siteCode'].disable({ onlySelf: true });


      this.custCodeControl.setValue(this.projectData.customerCode);

      this.loadProjectsList(this.projectData.customerCode);
      this.loadSiteCodes(this.projectData.projectCode);
    }




    this.apiService.get('CustomerComplaints/getById', this.id).subscribe(res => {

      if (res != null) {
        this.resultData = res as any;
        this.form.patchValue(res);

        this.proofForComplaintUrl = res.proofForComplaint;
        this.proofForActionUrl = res.proofForAction;

        


        this.loadCustomerData(res.customerCode);

        this.form.controls['projectCode'].setValue(res.projectCode);
        this.form.controls['customerCode'].setValue(res.customerCode);
        this.form.controls['siteCode'].setValue(res.siteCode);
        this.form.controls['reasonCode'].setValue(res.reasonCode);

        this.form.controls['bookedBy'].setValue(res.bookedBy.toString());
        this.proofForComplaintUrl = res?.proofForComplaint;
        this.proofForActionUrl = res?.proofForAction;
      

        this.getProjectData();


        this.isDataLoading = false;





      }


    });

  }
  closeModel() {
    this.dialogRef.close();
  }

  submit() {
    let ed = new Date(this.form.controls['complaintDate'].value);
    ed.setMinutes(ed.getMinutes() - ed.getTimezoneOffset());
    this.form.value['complaintDate'] = ed;

    if (this.form.valid) {

   
        if (this.id > 0)
          this.form.value['id'] = this.id;
        const formData = new FormData();
      formData.append("id", this.id.toString());


      formData.append("proofForComplaintFileName", this.authService.ApiEndPoint().replace("api", "") + 'ProofsForComplaints/');
      formData.append("proofForActionFileName", this.authService.ApiEndPoint().replace("api", "") + 'ProofsForActions/');
      formData.append("proofForComplaintIForm", this.form.controls['proofForComplaintIForm'].value);
      formData.append("isActionRequired", this.form.controls['isActionRequired'].value);


      formData.append("customerCode", this.form.controls['customerCode'].value);
      formData.append("projectCode", this.form.controls['projectCode'].value);
      formData.append("siteCode", this.form.controls['siteCode'].value);
      formData.append("reasonCode", this.form.controls['reasonCode'].value);
      formData.append("branchCode", this.form.controls['branchCode'].value);
      formData.append("complaintBy", this.form.controls['complaintBy'].value);
      formData.append("bookedBy", this.form.controls['bookedBy'].value);
      formData.append("action", this.action);
      formData.append("complaintDate", this.form.controls['complaintDate'].value);
      formData.append("complaintDescription", this.form.controls['complaintDescription'].value);
      if (this.action == 'closing') {
        formData.append("proofForActionIForm", this.form.controls['proofForActionIForm'].value);
        formData.append("actionDescription", this.form.controls['actionDescription'].value);
        formData.append("closingDate", this.form.controls['closingDate'].value);
      }





      
      this.apiService.post('CustomerComplaints', formData)
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




  onFileChanged(event: any, type: number) {
    let reader = new FileReader();
    if (event.target.files && event.target.files.length > 0) {
      let file = event.target.files[0];
      reader.readAsDataURL(file);
      reader.onload = () => {
        if (type === 1) {
          this.proofForComplaintUrl = reader.result;
          this.form.patchValue({
            'proofForComplaintIForm': file,
            'proofForComplaintFileName': reader.result
          });
        } else if (type === 2) {
          this.proofForActionUrl = reader.result;
          this.form.patchValue({
            'proofForActionIForm': file,
            'proofForActionFileName': reader.result,
          });
        }
      };
    }
  }
}

