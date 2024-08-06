import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
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
import { ActivatedRoute } from '@angular/router';
import { ParentOptMgtComponent } from '../../sharedcomponent/parentoptmgt.component';
import { TblSndDefSurveyFormHeadDto } from '../../models/Operationalmgt/tbl-snd-def-survey-form-head-dto.model';
import { TblSndDefSurveyFormElementDto } from '../../models/Operationalmgt/tbl-snd-def-survey-form-element-dto.model';
import { OprServicesService } from '../opr-services.service';
@Component({
  selector: 'app-view-survey-form-template',
  templateUrl: './view-survey-form-template.component.html'
})
export class ViewSurveyFormTemplateComponent extends ParentOptMgtComponent implements OnInit {
  isLoading: boolean = false;

  id: any;
  surveyFormHeader: TblSndDefSurveyFormHeadDto;
 
  surveyFormElements: Array < TblSndDefSurveyFormElementDto > =[];

  elementOptions: Array < string > =[];
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, private authService: AuthorizeService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService, private route: ActivatedRoute, private oprService: OprServicesService, public dialogRef: MatDialogRef<ViewSurveyFormTemplateComponent>) {
    super(authService);
  }
  ngOnInit(): void {
    //this.route.queryParams.subscribe(params => {

    //  this.id = params.id;
    

    //});
    

    this.loadData();

  }

  loadData() {

    this.isLoading = true;
    this.apiService.get('SurveyForm/getSurveyFormTemplateById',this.id).subscribe(result => {

  
      this.surveyFormHeader = result.head;
      this.surveyFormElements = result.elementsList;
     this.isLoading = false;
    }, error => this.utilService.ShowApiErrorMessage(error));

    localStorage.setItem('surveyFormTemplateId', this.id);
  }

  printComponent(prinatble:any) {
    let printContents = document.getElementById(prinatble) as HTMLElement;
    const WindowPrt: any = window.open('', '', 'left=0,top=0,width=2000,height=1000,toolbar=0,scrollbars=0,status=0');
    setTimeout(() => {
      WindowPrt.document.write(printContents.innerHTML);
      WindowPrt.document.close();
      WindowPrt.focus();
      WindowPrt.print();
      WindowPrt.close();
    }, 50);
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
  closeModel() {
    this.dialogRef.close();
  }
}

