import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
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
import { ParentOptMgtComponent } from '../../sharedcomponent/parentoptmgt.component';
import { OprServicesService } from '../opr-services.service';
import { ConvertToProjectComponent } from '../convert-to-project/convert-to-project.component';
import { ViewEnquiriesByEnquiryNumberComponent } from '../view-enquiries-by-enquiry-number/view-enquiries-by-enquiry-number.component';
import { PrintEnquiriesByEnquiryNumberComponent } from '../print-enquiries-by-enquiry-number/print-enquiries-by-enquiry-number.component';
import { ApprovalDialogWindowComponent } from '../approval-dialog-window/approval-dialog-window.component';
import { TranslateService } from '@ngx-translate/core';
import { AddExistingProjectComponent } from '../existing-projects/add-existing-project/add-existing-project.component';

@Component({
  selector: 'app-service-enquiry-forms',
  templateUrl: './service-enquiry-forms.component.html'
})
export class ServiceEnquiryFormsComponent extends ParentOptMgtComponent  implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['enquiryNumber', 'dateOfEnquiry', 'estimateClosingDate', /*'customerCode',*/'customerName','branchCode','branchName',/* 'stusEnquiryHead',*/'totalEstPrice','isActive', 'Actions'];
  data: MatTableDataSource<any>;
  totalItemsCount: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  form: FormGroup;

  isArab: boolean = false;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, private authService: AuthorizeService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService, private oprService: OprServicesService, private translate: TranslateService) {
    super(authService);
  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.initialLoading();
  }

  refresh() {
    this.searchValue = '';
    this.initialLoading();
  }

  initialLoading() {
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
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
    this.apiService.getPagination('ServiceEnquiryHeader/getSevriceEnquiryHeaderPagedList', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {


      this.totalItemsCount = 0;

      this.data = new MatTableDataSource(result.items);
      this.totalItemsCount = result.totalCount

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
  private openDialogManage(id: number, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;
    

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
  private openFullWindow(id: number, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {
    let dialogRef = this.oprService.fullWindow(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;
    

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.ngOnInit();
    });
  }
  private openEditWindow(isAdendum:boolean,enquiryHead: any, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {
    let dialogRef = this.oprService.fullWindow(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
   (dialogRef.componentInstance as any).enquiryHead = enquiryHead;
   (dialogRef.componentInstance as any).isAdendum = isAdendum;
    

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.ngOnInit();
    });
  }
  private openEnquiriesWindow(row: any, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {
    let dialogRef = this.oprService.fullWindow(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).enquiryNumber = row.enquiryNumber;
    (dialogRef.componentInstance as any).branchCode = row.branchCode;
    

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.ngOnInit();
    });
  }

  private openConvertDialog(enquiryHead: any, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).enquiryHead = enquiryHead;
    


    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }





  public create() {
    this.openFullWindow(0, DBOperation.create, 'Adding_New_Service_Enquiry_Form', 'Add', AddupdateEnquiryFormComponent);
  }




  //public edit(id: number) {
  //  this.openDialogManage(id, DBOperation.update, 'Updating_Service_Enquiry_Form', 'Update', AddupdateEnquiryFormComponent);
  //}
  public delete(id: any) {
    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete && id > 0) {
        this.apiService.delete('ServiceEnquiryHeader', id).subscribe(res => {
          this.utilService.OkMessage();
          this.ngOnInit();
        },
        );
      }
    },
      error => this.utilService.ShowApiErrorMessage(error));
  }
  submit() {

  }



  changeEnquiryFormStatus(enquiryNumber: string,enquiryStatus:string) {
    this.apiService.post('serviceEnquiryForm/changeEnquiryFormStatus?enquiryNumber=' + enquiryNumber + '&enquiryStatus=' + enquiryStatus, null)
      .subscribe(res => {
        this.utilService.OkMessage();
        this.ngOnInit();
         },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });
   

  }

  viewEnquiries(row:any)
  {
   
    this.openEnquiriesWindow(row, DBOperation.create, '', '', ViewEnquiriesByEnquiryNumberComponent);

  }

  printEnquiryForm(enquiryNumber: any)
  {
    //this.oprService.EnquiryNumber = enquiryNumber;
    this.openEnquiriesWindow(enquiryNumber, DBOperation.create, '', '', PrintEnquiriesByEnquiryNumberComponent);
    
  }
  convertToProject(enquiryHead:any) {
   
    this.openConvertDialog(enquiryHead,DBOperation.create, 'Converting_To_Project', 'Update', ConvertToProjectComponent);
  }




  approveEnquiry(enquiry: any) {

    let serviceType = "ENQ";
    let serviceCode = enquiry.enquiryNumber.toString() + (enquiry?.version == null ? "": "/"+enquiry?.version);


    let branchCode = enquiry?.branchCode;
    this.openApprovalDialog(branchCode, serviceCode, DBOperation.create, 'Enquiry_Approval', 'Save', serviceType, ApprovalDialogWindowComponent);
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

  translateToolTip(msg: string) {
    //console.log(msg);
    //let str = this.translate.instant(msg);
    return `${this.translate.instant(msg)}`;

  }


  public createExistingProject() {
    this.openFullWindow(0, DBOperation.create, 'Adding_New_Service_Enquiry_Form_For_Existing_Project', 'Add', AddExistingProjectComponent);
  }



  editEnquiry(row:any) {
    
  //  this.openEditWindow(false,row, DBOperation.update, 'Update_Service_Enquiry_Form', 'Update', AddupdateEnquiryFormComponent);
    let dialogRef = this.oprService.fullWindow(this.dialog, AddupdateEnquiryFormComponent);
    (dialogRef.componentInstance as any).dbops = DBOperation.update;
    (dialogRef.componentInstance as any).modalTitle = 'Update_Service_Enquiry_Form';
    (dialogRef.componentInstance as any).modalBtnTitle = 'Update';
    (dialogRef.componentInstance as any).enquiryHead = row;
    (dialogRef.componentInstance as any).isAdendum = false;


    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.ngOnInit();
    });


  }


  addEnquiry(row: any) {


    //this.openEditWindow(true,row, DBOperation.update, 'Add_Enquiry_For_New_Site', 'Update', AddupdateEnquiryFormComponent);
    let dialogRef = this.oprService.fullWindow(this.dialog, AddupdateEnquiryFormComponent);
    (dialogRef.componentInstance as any).dbops = DBOperation.update;
    (dialogRef.componentInstance as any).modalTitle = 'Add_Enquiry_For_New_Site';
    (dialogRef.componentInstance as any).modalBtnTitle = 'Update';
    (dialogRef.componentInstance as any).enquiryHead = row;
    (dialogRef.componentInstance as any).isAdendum = true;
    (dialogRef.componentInstance as any).isOldEnquiryForm = true;


    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.ngOnInit();
    });

  }





  skippSurvey(row: any) {
    this.openConvertDialog(row, DBOperation.create, 'Converting_To_Project', 'Update', ConvertToProjectComponent);

  }





}

