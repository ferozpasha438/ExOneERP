import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSlideToggleChange, MatSlideToggleModule } from '@angular/material/slide-toggle';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { UtilityService } from '../../services/utility.service';
import { NotificationService } from '../../services/notification.service';
import { ValidationService } from '../../sharedcomponent/ValidationService';
//import { ApiService } from '../../services/api.service';
import { DBOperation } from '../../services/utility.constants';

import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

import { DeleteConfirmDialogComponent } from '../../sharedcomponent/delete-confirm-dialog';
import { PaginationService } from '../../sharedcomponent/pagination.service';
//import { AddupdatecustomerComponent } from '../sharedpages/addupdatecustomer/addupdatecustomer.component';
import { ApiService } from '../../services/api.service';
import { ParentOptMgtComponent } from '../../sharedcomponent/parentoptmgt.component';
import { AddSiteToCustomerComponent } from '../sharedpages/add-site-to-customer/add-site-to-customer.component';
import { AddupdatesitesComponent } from '../sharedpages/addupdatesites/addupdatesites.component';
import { OprServicesService } from '../opr-services.service';
import { SitesListByCustomerCodeComponent } from '../sites-list-by-customer-code/sites-list-by-customer-code.component';
import * as XLSX from "xlsx";
import { AddupdatecustomerComponent } from '../../Finance/accountsreceivable/sharedpages/addupdatecustomer/addupdatecustomer.component';
import { TranslateService } from '@ngx-translate/core';
import { FileUploadComponent } from '../../sharedcomponent/fileupload.component';
//import { FileUploadComponent } from '../file-upload/file-upload.component';


@Component({
  selector: 'app-opcustomer',

  templateUrl: './opcustomer.component.html'
})
export class OpcustomerComponent extends ParentOptMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['custCode', 'custName', 'custCatCode', 'custCityCode1', 'custMobile1', 'custPhone1', 'custSalesRep','numberOfSites', 'Actions'];
  data: MatTableDataSource<any>;
  totalItemsCount: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  id: number;
  form: FormGroup;
  isArabic: boolean = false;


  filter: any = { branchCode: '' };
  reportData: Array<any> = [];
  isArab: boolean = false;
  citySelectionList: Array<any> = [];
  
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, private authService: AuthorizeService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService, private oprService: OprServicesService, private translate: TranslateService) {
    super(authService);
  }

  ngOnInit(): void {
    this.isArabic = this.utilService.isArabic();
    this.initialLoading();
  }

  refresh() {
    this.reportData = [];
    this.resetFilter();
  }
  resetFilter() {
    this.filter = { branchCode: '' };
    this.applyFilter('');
  }
  initialLoading() {
    this.searchValue = '';

    this.loadCitiesList();

    this.loadList(0, this.pageService.pageCount, "", this.sortingOrder);
  }

  loadCitiesList() {
    //  this.apiService.getall('City/getCitiesSelectList').subscribe((res: any) => {
    this.apiService.getall('Branch/getBranchSelectListForUser').subscribe((res: any) => {
      this.citySelectionList = res as Array<any>;

      this.citySelectionList.forEach(e => {
        e.lable = e.value + "-" + e.text;
      });
    });

  }
  onSortOrder(sort: any) {
    this.reportData = [];
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
    //query = "";
    this.oprService.getPaginationWithFilter('CustomerMaster/getCustomersPagedListWithFilter', this.utilService.getQueryString(page, pageCount, query, orderBy),this.filter).subscribe(result => {
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
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
    this.reportData = [];
  }
  private openDialogManage(id: number, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, component: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;
   

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true) {
        //this.initialLoading();
        location.reload();
      }
      this.isLoading = false;

    });
  }
  private openFullWindow(custId: number, customerCode: string, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, component: any) {
    let dialogRef = this.oprService.fullWindow(this.dialog, component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).customerCode = customerCode;
    (dialogRef.componentInstance as any).custId = custId;
    (dialogRef.componentInstance as any).custId = custId;


    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }

  public createCust() {
    this.openDialogManage(0, DBOperation.create, 'Adding_New_Customer', 'Add', AddupdatecustomerComponent);
  }
  public createSite() {
    this.openDialogManage(0, DBOperation.create, 'Adding_New_Site', 'Add', AddupdatesitesComponent);
  }

  public addsitetocustomer(id: number) {
    this.openDialogManage(id, DBOperation.create, 'Adding_New_Site_to_Customer', 'Add', AddSiteToCustomerComponent);
  }

  public edit(id: number) {
    console.log("id="+id);
    this.openDialogManage(id, DBOperation.update, 'Updating_Customer', 'Update', AddupdatecustomerComponent);
  }
  public delete(id: number) {
    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete && id > 0) {
        this.apiService.delete('CustomerMaster', id).subscribe(res => {
          this.utilService.OkMessage();
          this.ngOnInit();
        },

        );

      }
    },
      error => this.utilService.ShowApiErrorMessage(error));
  }
  submit() {
    //empty
  }
  viewCustomerSites(customerCode: string, id: number) {
    this.openFullWindow(id, customerCode, DBOperation.create, 'Customer_Sites', '', SitesListByCustomerCodeComponent);
  }

  clearReport() {
    this.reportData = [];
  }
  private loadListReport(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
    this.isLoading = true;
    this.oprService.getPaginationWithFilter('CustomerMaster/getCustomersPagedListWithFilter', this.utilService.getQueryString(page, pageCount, query, orderBy), this.filter).subscribe(result => {

      this.isLoading = false;
      this.reportData = this.reportData.length == 0 ? result.items : this.reportData.concat(result.items);
    }, error => this.utilService.ShowApiErrorMessage(error));
  }

  generateReport() {

    this.reportData = [];
    for (let i = 0; i <= Math.floor(this.totalItemsCount / 100); i++) {
      let event: any = { pageIndex: i, pageSize: 100, previousPageIndex: i == 0 ? 0 : i - 1, length: 0 };
      //this.pageService.change(event);
      this.loadListReport(event.pageIndex, event.pageSize, this.searchValue, this.sortingOrder);

    }


  }
  exportexcel(): void {
    let element = document.getElementById('printcontainer');
    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(element);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
    XLSX.writeFile(wb, "CustomersReport.xlsx");
  }
  private openFileUploadDialogManage<T>(modalTitle: string = '', component: T, moduleFile: any, width: number = 80) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, component, width);
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).moduleFile = moduleFile;

    dialogRef.afterClosed().subscribe(res => {
    });
  }

  uploadFile(id: any) {
    this.openFileUploadDialogManage(this.translate.instant('Document_Upload'), FileUploadComponent, { module: 'OPR', action: 'OPR_CUST', id: id, sourceId: id });
  }
  CustomerActiveChecked(evt: MatSlideToggleChange) {
    this.filter.isActive = evt.checked;
    //this.loadList(0, this.pageService.pageCount, "", this.sortingOrder, this.isActive);
    this.applyFilter(this.searchValue);
  }
}
