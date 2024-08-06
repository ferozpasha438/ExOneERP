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
import { OprServicesService } from '../opr-services.service';
import { ApprovalServiceEnquiryComponent } from './approval-service-enquiry/approval-service-enquiry.component';
import { ApprovalDialogWindowComponent } from '../approval-dialog-window/approval-dialog-window.component';
import { PrintSurveyFormComponent } from '../print-survey-form/print-survey-form.component';
import { EditableSurveyFormComponent } from '../editable-survey-form/editable-survey-form.component';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-view-enquiries-by-enquiry-number',
  templateUrl: './view-enquiries-by-enquiry-number.component.html'
})

export class ViewEnquiriesByEnquiryNumberComponent extends ParentOptMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [/*'enquiryID', */'site', 'service', 'unitCode', 'pricePerUnit', 'serviceQuantity','unitQuantity', 'estimatedPrice',/* 'statusEnquiry',*/ 'Actions'];
  data: MatTableDataSource<any>;
  totalItemsCount: number;
  searchValue: string = '';
  sortingOrder: string = 'enquiryID desc';
  isLoading: boolean = false;
  form: FormGroup;
  enquiryNumber: string;
  branchCode: string;
  id: number;
  surveyorCode: string;

  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;


  isArab: boolean = false;





  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, private authService: AuthorizeService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService, private route: ActivatedRoute, private oprService: OprServicesService, private translate: TranslateService,public dialogRef: MatDialogRef<ViewEnquiriesByEnquiryNumberComponent>) {
    super(authService);
  }

  ngOnInit(): void {

    this.isArab = this.utilService.isArabic();
    this.initialLoading();
    
  }

  refresh() {
   this.setForm();
    this.searchValue = '';
   
    this.initialLoading();
  }

  initialLoading() {
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
   // localStorage.setItem('enquiryNumber', this.enquiryNumber);
    this.getEnquiryHead();
  }

  onSortOrder(sort: any) {
    this.totalItemsCount = 0;
    this.sortingOrder = sort.active + ' ' + sort.direction;
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
  }


  onPageSwitch(event: PageEvent) {
    this.pageService.change(event);
    this.loadList(event.pageIndex, event.pageSize, this.searchValue, this.sortingOrder);
  }

  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
    this.isLoading = true;
    this.apiService.getPagination('ServiceEnquiries/getSevriceEnquiriesByEnquiryNumberPagedList', this.utilService.getQueryString(page, pageCount, this.enquiryNumber, orderBy)).subscribe(result => {
      this.totalItemsCount = 0;

      this.data = new MatTableDataSource(result.items);
      this.totalItemsCount = result.totalCount
      console.log(this.data);
      setTimeout(() => {
        this.paginator.pageIndex = page as number;
        this.paginator.length = this.totalItemsCount;
      });
      //this.data.paginator = this.paginator;

      this.data.sort = this.sort;
      this.isLoading = false;
    }, error => this.utilService.ShowApiErrorMessage(error));
  }


  applyFilter(searchVal: any) {
    const search = searchVal;//.target.value as string;
    //if (search && search.length >= 3) {
    if (search) {
      this.searchValue = search;
      this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
    }
  }
  private openDialogManage(id: number = 0, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, component: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;
    (dialogRef.componentInstance as any).branchCode = this.branchCode;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
  private openDialogManage2(enquiry: any, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, component: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).inputData = enquiry;
   

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.ngOnInit();
    });
  }

  //public create() {
  //  this.openDialogManage(0, DBOperation.create, 'Adding_New_Enquiry', 'Add', AddupdateEnquiryFormComponent);
  //}

  //public edit(id: number) {
  //  this.openDialogManage(id, DBOperation.update, 'Updating_Enquiry', 'Update', AddupdateEnquiryFormComponent);
  //}

  public addSuveyorToEnquiry(enquiryID: number) {
    this.openDialogManage(enquiryID, DBOperation.create, 'Adding_Surveyor_To_Enquiry', 'Save', AddSurveyorToEnquiryComponent);

  }


  public delete (id: number) {
    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete && id > 0) {
        this.apiService.delete('ServiceEnquiries',id).subscribe(res => {
          this.utilService.OkMessage();
          this.ngOnInit();
        });

      }
    },
      error => this.utilService.ShowApiErrorMessage(error));
  }
  submit() {

  }
  setForm() {
    this.form = this.fb.group({

      'enquiryID': [0],
      'statusEnquiry': [''],

    });
  }


  cahngeEnquiryStatus(enquiryID: number, statusEnquiry: string) {
    this.form.value['enquiryID'] = enquiryID;
    this.form.value['statusEnquiry'] = statusEnquiry;
    this.apiService.post('ServiceEnquiries/changeEnquiryStatus', this.form.value)
      .subscribe(res => {
        this.utilService.OkMessage();
        this.ngOnInit();
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });
  }





  private ApprovalDialog(enquiry: any,Component: any) {
    let dialogRef = this.oprService.openApprovalDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).enquiry = enquiry;
    dialogRef.afterClosed().subscribe(res => {

      
        this.ngOnInit();
    });
  }
  approvalAuth(enquiry: any) {
    this.ApprovalDialog(enquiry, ApprovalServiceEnquiryComponent);

  }


  approveSurvey(enquiry:any) {
   
    let serviceType = "SUR";
    let serviceCode = enquiry.enquiryID.toString();


    let branchCode = this.branchCode;
    this.openApprovalDialog(branchCode, serviceCode, DBOperation.create, 'Survey_Approval', 'Save', serviceType, ApprovalDialogWindowComponent);
  }

  private openApprovalDialog(branchCode: string, serviceCode: string, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, serviceType: string, Component: any) {
    let dialogRef = this.oprService.openApprovalDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).serviceType = serviceType;
    (dialogRef.componentInstance as any).serviceCode = serviceCode;
    (dialogRef.componentInstance as any).branchCode = branchCode;

    dialogRef.afterClosed().subscribe(res => {

      if (res && res === true) {
        this.initialLoading();


      }
    });
  }

  getEnquiryHead() {
    this.apiService.getall(`ServiceEnquiryHeader/getEnquiryFormHeaderByEnquiryNumber/${this.enquiryNumber}`).subscribe(res => {

      this.branchCode = res.branchCode;
      
    });

  }

  printSurveyForm(enquiryID: number) {
    let enquiry: any = { enquiryID: enquiryID, enquiryNumber: this.enquiryNumber };
    this.openDialogManage2(enquiry, DBOperation.create, '', 'Print', PrintSurveyFormComponent);
  }
  editableSurveyForm(enquiryID: number) {
    let enquiry: any = { enquiryID: enquiryID, enquiryNumber: this.enquiryNumber };
    this.openDialogManage2(enquiry, DBOperation.create, '', 'Print', EditableSurveyFormComponent);
  }
  closeModel() {
    this.dialogRef.close(true);
  }
}








