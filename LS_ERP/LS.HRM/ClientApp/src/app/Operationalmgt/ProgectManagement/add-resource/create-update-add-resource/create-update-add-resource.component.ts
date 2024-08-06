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
import { NotificationService } from '../../../../services/notification.service';
@Component({
  selector: 'app-create-update-add-resource',
  templateUrl: './create-update-add-resource.component.html'
})
export class CreateUpdateAddResourceComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number;
  resourceList: Array<any> = [];
  editResource: any = { id: '', skillsetCode: '', qty: '', pricePerUnit: '', toDate: '', fromDate:''};
  emptyResource: any = { id: '', skillsetCode: '', qty: '', pricePerUnit: '', toDate: '', fromDate:''};
  readonly: string = "";

  projectData: any;



  requestData: any;
  

  siteCodeList: Array<CustomSelectListItem> = [];
  skillSetsList: Array<CustomSelectListItem> = [];
  customerCode: string = '';
  isDataLoading: boolean = false;
  filteredCustCodes: Observable<Array<CustomSelectListItem>>;
  custCodeControl = new FormControl('', Validators.required);
  isArabic: boolean = false;
  isUpdating: boolean = false;

  projectsList: Array<CustomSelectListItem> = [];

  isFromProjectsAction: boolean = false;
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



  constructor(private notifyService: NotificationService,public datepipe: DatePipe,private translate: TranslateService,private fb: FormBuilder,private authService: AuthorizeService, private utilService: UtilityService, private apiService: ApiService, public dialogRef: MatDialogRef<CreateUpdateAddResourceComponent>) {

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
    this.loadSkillSetsList();
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


  setForm() {

    this.form = this.fb.group({
   
      'customerCode': ['', Validators.required],
      'projectCode': ['', Validators.required],
      'siteCode': ['', Validators.required],
      'id': [this.id],
      'resourceList': [this.resourceList],
      
     





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

  loadSkillSetsList() {
    this.apiService.getall('Skillset/GetSelectSkillsetList/').subscribe(res => {

      this.skillSetsList = res as Array<any>;

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
      
      }/*, error => {*/
      //}
      else {

        this.form.controls['customerCode'].setValue('');
        this.custCodeControl.setValue('');
        this.projectsList = [];
       
      }

    });
    this.form.controls['projectCode'].setValue('');
    this.form.controls['siteCode'].setValue('');
    this.siteCodeList = [];
    this.resourceList = [];
  }
  onSelectionProjectCode() {
    this.loadSiteCodes(this.form.controls['projectCode'].value); 
    this.form.controls['siteCode'].setValue('');
    this.resourceList = [];
  }

  getProjectData() {

    if (this.form.controls['siteCode'].value != '') {
      this.isDataLoading = true;
      this.apiService.getall(`ProjectSites/getProjectSiteByProjectAndSiteCode/${this.form.controls['projectCode'].value}/${this.form.controls['siteCode'].value}`).subscribe(res => {
        this.projectData = res;
      });
      this.isDataLoading = false;





    }
    
  }


  loadSiteCodes(projectCode: string) {
    //if(this.id==0)
    //  this.form.controls['siteCode'].setValue('');

    this.isDataLoading = true;

    this.apiService.getall(`CustomerSite/getSelectSiteListByProjectCode/${projectCode}`).subscribe(res => {
      this.siteCodeList = res;
     
   



    });
    this.isDataLoading = false;
  }
  loadProjectsList(custCode: string) {
 
    this.isDataLoading = true;

    this.apiService.getall(`project/getSelectProjectListByCustomerCode/${custCode}`).subscribe(res => {
      this.projectsList = res;

      

    });
    this.isDataLoading = false;
  }


  editForm() {
 

    this.apiService.get('PvAddResource/getPvAddResourceReqById', this.id).subscribe(res => {
      if (res != null) {
      
       
      
        this.requestData = res as any;
     
        this.resourceList = res.resourceList;
        this.custCodeControl.setValue(res.customerCode);

     
      
        //console.log(res);
        //this.form.controls['projectCode'].disable({ onlySelf: true });
        //this.form.controls['siteCode'].disable({ onlySelf: true });
        this.form.controls['projectCode'].setValue(res.projectCode);

       
        this.form.controls['customerCode'].setValue(res.customerCode);
        this.form.controls['siteCode'].setValue(res.siteCode);
        this.getProjectData();
        this.form.controls['resourceList'].setValue(res.resourceList);

        this.loadProjectsList(res.customerCode);
        this.loadSiteCodes(res.projectCode);
        this.isDataLoading = false;
       


      
       
      }
     

    });

  }
  closeModel() {
    this.dialogRef.close();
  }

  submit() {
    if (!this.isUpdating) {
      if (this.resourceList.length > 0) {


        this.resourceList.forEach(r => {
          let fd: Date = new Date(r.fromDate);
          fd.setMinutes(fd.getMinutes() - fd.getTimezoneOffset());
          r.fromDate = fd;
          let td: Date = new Date(r.toDate);
          td.setMinutes(td.getMinutes() - td.getTimezoneOffset());
          r.toDate = td;



        });

        this.form.controls['resourceList'].setValue(this.resourceList);
      }


      if (this.form.valid && this.resourceList.length > 0) {
        this.isUpdating = true;
        this.apiService.post('PvAddResource', this.form.value)
          .subscribe(res => {
            this.isUpdating = false;

            if (res) {

              this.utilService.OkMessage();

              this.dialogRef.close(true);

            }
          },
            error => {
              console.error(error);
              this.utilService.ShowApiErrorMessage(error);
              this.isUpdating = false;
            });

      }
      else {
        this.utilService.FillUpFields();
      }
    }
    else {
      this.notifyService.showError(this.translate.instant("Please Wait..."));
    }
   

  }

  onSelectionSiteCode() {

   

    this.getProjectData();

    this.resourceList = [];
    this.editResource.fromDate = '';
  }
  edit(row: number) {
   
    this.editResource = this.resourceList[row];
    if (this.resourceList[row].id==0) {
      this.editResource.id = -1;
    }

    let ele = <HTMLElement>document.getElementById('inputEntry');
    ele.scrollIntoView();

  }
  delete(i: number) {
    this.resourceList.splice(i,1);
  }
  translateToolTip(text: string) {
    return this.translate.instant(text);
  }
  openDatePicker(dp: any) {
    dp.open();
  }

  ToDateString(date: any) {
    
    if (date != null)
      return this.datepipe.transform(date.toString(), 'yyyy-MM-dd')?.toString();
    else
      return "";
  }

  addUpdate() {
    if (this.editResource.skillsetCode != '' && this.editResource.qty * this.editResource.pricePerUnit > 0 && this.editResource.fromDate != '' && this.editResource.toDate != '') {

      if (new Date(this.editResource.fromDate) < new Date(this.editResource.toDate)) {

        if (this.editResource.id == 0 || this.editResource.id == '') {

          this.editResource.id = 0;

          this.resourceList.push(this.editResource);
        }
        let editResource: any = { id: '', skillsetCode: '', qty: '', pricePerUnit: '', toDate: '', fromDate: '' };
        this.editResource = editResource;
      }
    }
  }

  getSkillSet(code: string) {
    let ss:any=this.skillSetsList.find(e => e.value == code);
    if (this.isArabic) {
      ss.text = ss.textTwo;
    }
    return ss;
  }
}
