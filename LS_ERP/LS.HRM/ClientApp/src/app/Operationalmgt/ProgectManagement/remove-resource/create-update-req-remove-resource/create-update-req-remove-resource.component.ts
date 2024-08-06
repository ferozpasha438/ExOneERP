import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../../services/api.service';
import { ParentOptMgtComponent } from '../../../../sharedcomponent/parentoptmgt.component';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { DBOperation } from '../../../../services/utility.constants';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { CustomSelectListItem } from '../../../../models/MenuItemListDto';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { UtilityService } from '../../../../services/utility.service';
import { TranslateService } from '@ngx-translate/core';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-create-update-req-remove-resource',
  templateUrl: './create-update-req-remove-resource.component.html'
})
export class CreateUpdateReqRemoveResourceComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number;
 
  readonly: string = "";

  projectData: any;



  requestData: any;


  siteCodeList: Array<CustomSelectListItem> = [];
 
  customerCode: string = '';
  empNumber: string = '';
  isDataLoading: boolean = false;
  filteredCustCodes: Observable<Array<CustomSelectListItem>>;
  filteredEmployees: Observable<Array<CustomSelectListItem>>;
  custCodeControl = new FormControl('', Validators.required);
  empNumberControl = new FormControl('', Validators.required);
  isArabic: boolean = false;


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

  filterEmpNumbers(val: string): Observable<Array<CustomSelectListItem>> {
    return this.apiService.getall(`EmployeesToProjectSite/getAutoFillEmployeeListForProjectSite/${this.form.controls['projectCode'].value}/${this.form.controls['siteCode'].value}/?search=${val}`)
      .pipe(
        map(response => {
          const res = response as Array<any>;
          this.isDataLoading = false;
          return res;
        })
      )
  }


  constructor(public datepipe: DatePipe, private translate: TranslateService, private fb: FormBuilder, private authService: AuthorizeService, private utilService: UtilityService, private apiService: ApiService, public dialogRef: MatDialogRef<CreateUpdateReqRemoveResourceComponent>) {

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

  ngOnInit(): void {
    this.isDataLoading = true;
    this.setForm();
   
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


  setForm() {

    this.form = this.fb.group({

      'customerCode': [ '', Validators.required],
      'projectCode': [ '', Validators.required],
      'siteCode': ['', Validators.required],
      'id': [this.id],
      'fromDate': ['',Validators.required],
      'employeeNumber': ['', Validators.required],
     






    });


    if (this.isFromProjectsAction) {
      this.custCodeControl.setValue(this.projectData.customerCode);
      this.form.controls['siteCode'].setValue(this.projectData.siteCode);
      this.form.controls['projectCode'].setValue(this.projectData.projectCode);
      this.form.controls['customerCode'].setValue(this.projectData.customerCode);
      this.loadProjectsList(this.projectData.customerCode);
      this.loadSiteCodes(this.projectData.projectCode);
      this.onSelectionSiteCode();
    }
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







      }/*, error => {*/
      //}
      else {

        this.form.controls['customerCode'].setValue('');


      }
      this.form.controls['projectCode'].setValue('');
      this.form.controls['siteCode'].setValue('');
      this.form.controls['employeeNumber'].setValue('');
      this.form.controls['fromDate'].setValue('');
      this.empNumberControl.setValue('');
      this.projectsList = [];
      this.siteCodeList = [];

    });
 
  }
  onSelectionEmployee(event: any, op: number) {
    let empNumber: string = '';
    if (op == 1) { empNumber = event.option.value.toString(); }
    else if (op == 2) { empNumber = event.target.value.toString(); }
    else if (op == 3) { empNumber = event }

    this.apiService.getall('Employee/getEmployeeByEmployeeNumber/' + empNumber).subscribe(res => {
      if (res != null) {

        let empNumber = this.empNumberControl.value as string;


        this.form.controls['employeeNumber'].setValue(empNumber);
      

      }/*, error => {*/
      //}
      else {

        this.form.controls['employeeNumber'].setValue('');


      }

    });
   
  }
  onSelectionProjectCode() {

   
      this.loadSiteCodes(this.form.controls['projectCode'].value);

      if (this.form.controls['projectCode'].value != '') {
        this.isDataLoading = true;



        this.form.controls['siteCode'].setValue('');
        this.form.controls['employeeNumber'].setValue('');
        this.form.controls['fromDate'].setValue('');
        this.empNumberControl.setValue('');


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

    this.apiService.get('PvRemoveResource/getPvRemoveResourceReqById', this.id).subscribe(res => {

      if (res != null) {
      this.requestData = res as any;

      
        this.custCodeControl.setValue(res.customerCode);
        this.empNumberControl.setValue(res.employeeNumber);

        this.loadProjectsList(res.customerCode);
        this.loadSiteCodes(res.projectCode);

      
        //this.form.controls['projectCode'].disable({ onlySelf: true });
        //this.form.controls['siteCode'].disable({ onlySelf: true });

        this.form.controls['projectCode'].setValue(res.projectCode);
        this.form.controls['customerCode'].setValue(res.customerCode);
        this.form.controls['siteCode'].setValue(res.siteCode);
        this.form.controls['employeeNumber'].setValue(res.employeeNumber);

        this.form.controls['fromDate'].setValue(res.fromDate);
       
        this.getProjectData();

       
        this.isDataLoading = false;





      }


    });

  }
  closeModel() {
    this.dialogRef.close();
  }

  submit() {

    this.form.value['projectCode'] = this.form.controls['projectCode'].value;
    this.form.value['siteCode'] = this.form.controls['siteCode'].value;



    let fd = new Date(this.form.controls['fromDate'].value);
    fd.setMinutes(fd.getMinutes() - fd.getTimezoneOffset());
    this.form.value['fromDate'] = fd;

    if (this.form.valid) {
      this.apiService.post('PvRemoveResource', this.form.value)
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

      this.form.controls['fromDate'].setValue('');
      this.filteredEmployees = this.empNumberControl.valueChanges.pipe(
        startWith(this.empNumber),
        debounceTime(this.utilService.autoDelay()),
        distinctUntilChanged(),
        switchMap((val: string) => {
          if (val.trim() !== '')
            this.isDataLoading = true;
          return this.filterEmpNumbers(val || '')
        })
      );
      this.getProjectData();

      this.form.controls['employeeNumber'].setValue('');
      this.form.controls['fromDate'].setValue('');
      this.empNumberControl.setValue('');

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
 
}
