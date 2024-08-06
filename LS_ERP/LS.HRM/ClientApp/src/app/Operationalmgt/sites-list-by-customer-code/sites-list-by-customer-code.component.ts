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
import { AddupdatesitesComponent } from '../sharedpages/addupdatesites/addupdatesites.component';
import { AddSiteToCustomerComponent } from '../sharedpages/add-site-to-customer/add-site-to-customer.component';
import { OprServicesService } from '../opr-services.service';
@Component({
  selector: 'app-sites-list-by-customer-code',
  templateUrl: './sites-list-by-customer-code.component.html'
})
export class SitesListByCustomerCodeComponent extends ParentOptMgtComponent implements OnInit {

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['siteCode', 'siteName', 'siteArbName', 'siteAddress', 'siteCityCode', 'isActive', 'Actions'];
  data: MatTableDataSource<any>;
  totalItemsCount: number;
  searchValue: string = '';
  sortingOrder: string = 'customerCode desc';
  isLoading: boolean = false;
  form: FormGroup;
  customerCode: string;
  id: number;
  surveyorCode: string;
  custId: number;

  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, private authService: AuthorizeService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService, private route: ActivatedRoute, private oprService: OprServicesService, public dialogRef: MatDialogRef<SitesListByCustomerCodeComponent>) {
    super(authService);
  }

  ngOnInit(): void {
    //this.route.queryParams.subscribe(params => {


    //  this.customerCode = params.customerCode;

    //});
    this.initialLoading();
  }

  refresh() {
    this.searchValue = '';
    this.initialLoading();
  }

  initialLoading() {
    this.loadList(0, this.pageService.pageCount, "", this.sortingOrder);
  }

  onSortOrder(sort: any) {
    this.totalItemsCount = 0;
    this.sortingOrder = sort.active + ' ' + sort.direction;
    this.loadList(0, this.pageService.pageCount, "", this.sortingOrder);
  }


  onPageSwitch(event: PageEvent) {
    this.pageService.change(event);
    this.loadList(event.pageIndex, event.pageSize, "", this.sortingOrder);
  }

  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
    this.isLoading = true;
    query = this.customerCode;
     this.apiService.getPagination('CustomerSite/getCustomerSitesByCustCodePagedList', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
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
  private openDialogManage(id: number = 0, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, component: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }



  public create(id:number) {
    this.openDialogManage(id, DBOperation.create, 'Adding_New_Enquiry', 'Add', AddSiteToCustomerComponent);
  }

  public editSite(id: number) {
    this.openDialogManage(id, DBOperation.update, 'Updating_Site', 'Update', AddupdatesitesComponent);
  }

  //public addSuveyorToEnquiry(enquiryID: number) {
  //  this.openDialogManage(enquiryID, DBOperation.create, 'Adding_Surveyor_To_Enquiry', 'Save', AddSurveyorToEnquiryComponent);

  //}


  public delete(id: number) {
    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete && id > 0) {
        this.apiService.delete('CustomerSite', id).subscribe(res => {
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



  //cahngeEnquiryStatus(enquiryID: number, statusEnquiry: string) {
  //  this.apiService.post('ServiceEnquiries/changeEnquiryStatus?enquiryID=' + enquiryID + '&statusEnquiry=' + statusEnquiry, null)
  //    .subscribe(res => {
  //      this.utilService.OkMessage();
  //      this.ngOnInit();
  //    },
  //      error => {
  //        console.error(error);
  //        this.utilService.ShowApiErrorMessage(error);
  //      });


  //}
  public addsitetocustomer(custId: number) {
    this.openDialogManage(custId, DBOperation.create, 'Adding_New_Site_to_Customer', 'Add', AddSiteToCustomerComponent);
  }
  closeModel() {
    this.dialogRef.close();
  }
}
