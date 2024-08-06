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
import { TblSndDefSurveyFormElementDto } from '../../models/Operationalmgt/tbl-snd-def-survey-form-element-dto.model';
import { TblSndDefServiceMasterDto } from '../../models/Operationalmgt/tbl-snd-def-service-master-dto.model';
@Component({
  selector: 'app-print-survey-form',
  templateUrl: './print-survey-form.component.html'
})
export class PrintSurveyFormComponent extends ParentOptMgtComponent implements OnInit {
  isLoading: boolean = false;

  id: number;
  enquiryID: number;
  enquiryNumber: string;
  siteID: string;
  custID: string;
  serviceCode: string;

  customer: TblSndDefCustomerMasterDto;
  enquiryHeader: TblSndDefServiceEnquiryHeaderDto;
  site: TblSndDefSiteMasterDto;
  surveyFormHeader: TblSndDefSurveyFormHeadDto;
  surveyor: TblSndDefSurveyorDto;
  surveyFormElements: Array<TblSndDefSurveyFormElementDto> = [];
  service: TblSndDefServiceMasterDto;
  elementOptions: Array<string> = [];
  inputData: any;
  enquiry: any;
  surveyFormDataEntries: Array<any> = [];
  showElements: boolean = true;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, private authService: AuthorizeService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService, private route: ActivatedRoute, public dialogRef: MatDialogRef<PrintSurveyFormComponent>) {
    super(authService);
  }
  ngOnInit(): void {
    this.enquiryID = this.inputData.enquiryID;

    //this.route.queryParams.subscribe(params => {

    this.enquiryNumber = this.inputData.enquiryNumber;
    //  this.enquiryID = params.enquiryID;

    this.loadData();
   

    /*  });*/
  }

  loadData() {

    this.isLoading = true;
    this.apiService.get('SurveyFormData/getSurveyFormDataByEnquiryId', this.enquiryID).subscribe(result => {
      
      this.customer = result.customer;
      this.enquiryHeader = result.enquiryHeader;
      this.site = result.site;
      this.surveyFormHeader = result.surveyFormHeader;
      this.surveyFormElements = result.surveyFormElements;
      this.surveyor = result.surveyor;
      this.service = result.service;
      this.enquiry = result.enquiry;
      this.surveyFormDataEntries = result.surveyFormDataEntries;
      this.surveyor = result.surveyor;
      this.service = result.service;
    }, error => this.utilService.ShowApiErrorMessage(error));
    this.isLoading = false;
  }

  printComponent(prinatble: any) {




    const printContent = document.getElementById(prinatble) as HTMLElement;
    const WindowPrt: any = window.open('', '', 'left=0,top=0,width=2000,height=1000,toolbar=0,scrollbars=0,status=0');
    setTimeout(() => {
      WindowPrt.document.write(printContent.innerHTML);
      WindowPrt.document.close();
      WindowPrt.focus();
      WindowPrt.print();
      WindowPrt.close();
    }, 50);
    //let printContents = document.getElementById(prinatble).innerHTML;
    //let originalContents = document.body.innerHTML;

    //document.body.innerHTML = printContents;

    //window.print();

    //document.body.innerHTML = originalContents;
  }
  goBack() {
    window.history.back();

  }









  printSurveyForm() {
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

  printInvoice() {
    this.openPrint();
  }

  closeModel() {

    this.dialogRef.close();
  }



  






}
