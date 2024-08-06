import { Input, OnChanges, SimpleChanges } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';
import { ValidationService } from '../../sharedcomponent/ValidationService';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { DBOperation } from '../../services/utility.constants';
import { DeleteConfirmDialogComponent } from '../../sharedcomponent/delete-confirm-dialog';
import { AddupdateSurveyorComponent } from '../sharedpages/addupdate-surveyor/addupdate-surveyor.component';
import { AddupdateServicesEnquiryComponent } from '../sharedpages/addupdate-services-enquiry/addupdate-services-enquiry.component';
import { AddupdateEnquiryFormComponent } from '../sharedpages/addupdate-enquiry-form/addupdate-enquiry-form.component';
import { AddSurveyorToEnquiryComponent } from '../sharedpages/add-surveyor-to-enquiry/add-surveyor-to-enquiry.component';
import { ParentOptMgtComponent } from '../../sharedcomponent/parentoptmgt.component';
import { TblSndDefCustomerMasterDto } from '../../models/Operationalmgt/tbl-snd-def-customer-master-dto.model';
import { TblSndDefServiceEnquiryHeaderDto } from '../../models/Operationalmgt/tbl-snd-def-service-enquiry-header-dto.model';
import { TblSndDefSiteMasterDto } from '../../models/Operationalmgt/tbl-snd-def-site-master-dto.model';
import { TblSndDefSurveyFormHeadDto } from '../../models/Operationalmgt/tbl-snd-def-survey-form-head-dto.model';
import { TblSndDefSurveyorDto } from '../../models/Operationalmgt/tbl-snd-def-surveyor-dto.model';
import { TblSndDefServiceMasterDto } from '../../models/Operationalmgt/tbl-snd-def-service-master-dto.model';
import { TblSndDefSurveyFormDataEntryDto } from '../models/tbl-snd-def-survey-form-data-entry-dto.model';
import { EnquiriesDto } from '../models/enquiries-dto.model';

@Component({
  selector: 'app-editable-survey-form',
  templateUrl: './editable-survey-form.component.html'
})
export class EditableSurveyFormComponent extends ParentOptMgtComponent implements OnInit {
  isLoading: boolean = true;
  showElements: boolean = true;
  entry: string ="";
  enquiryID: number;
  enquiryNumber: string;
  siteID: string;
  custID: string;
  serviceCode: string;
  form: FormGroup;
  customer: TblSndDefCustomerMasterDto;
  enquiryHeader: TblSndDefServiceEnquiryHeaderDto;
  site: TblSndDefSiteMasterDto;
  surveyFormHeader: TblSndDefSurveyFormHeadDto;
  surveyor: TblSndDefSurveyorDto;
  surveyFormDataEntries: Array<TblSndDefSurveyFormDataEntryDto> = [];
  entryValues: Array<string>=[];
  service: TblSndDefServiceMasterDto;
  elementOptions: Array<string> = [];
  enquiry: any;
  inputData: any;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, private authService: AuthorizeService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService, private route: ActivatedRoute, public dialogRef: MatDialogRef<EditableSurveyFormComponent>) {
    super(authService);
  }
  ngOnInit(): void {
    this.form = this.fb.group({});
    this.enquiryID = this.inputData.enquiryID;
      this.loadData();

   
   

    



  }

  loadData() {
    
    this.isLoading = true;
    this.apiService.get('SurveyFormData/getSurveyFormDataByEnquiryId', this.enquiryID).subscribe(result => {
      
      this.enquiry = result.enquiry;
     
          this.customer = result.customer;
          this.enquiryHeader = result.enquiryHeader;
          this.site = result.site;
          this.surveyFormHeader = result.surveyFormHeader;
      this.surveyFormDataEntries = result.surveyFormDataEntries;



          this.surveyor = result.surveyor;
      this.service = result.service;
      }, error => this.utilService.ShowApiErrorMessage(error));
    this.isLoading = false;
   
  }


  printComponent(prinatble: any) {
    this.showElements = false;
    const printContent = document.getElementById(prinatble) as HTMLElement;
    const WindowPrt: any = window.open('', '', 'left=0,top=0,width=2000,height=1000,toolbar=0,scrollbars=0,status=0');
    setTimeout(() => {
      WindowPrt.document.write(printContent.innerHTML);
      WindowPrt.document.close();
      WindowPrt.focus();
      WindowPrt.print();

      WindowPrt.close();
    }, 5);
  }

  goBack() {
    window.history.back();
  }

  submit(Action: string) {
    this.form.value['enquiryID'] = this.enquiryID;
    this.form.value['surveyFormDataEntries'] = this.surveyFormDataEntries;
    this.form.value['action'] = Action;

  
    this.apiService.post('SurveyFormData', this.form.value)
      .subscribe(res => {
        this.utilService.OkMessage();
        this.dialogRef.close(true);
       
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });
  }
  
  setEntryValue(event:any, index: number) {
      var inputValue = event.target.value;
    this.surveyFormDataEntries[index].entryValue = inputValue;


  }

  getValue(i:any) {
      (<HTMLInputElement>document.getElementById(i.toString())).value = this.surveyFormDataEntries[i].entryValue;

  }
  closeModel() {
    this.dialogRef.close();
  }
}
