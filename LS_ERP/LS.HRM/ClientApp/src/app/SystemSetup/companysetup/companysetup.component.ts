import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { DBOperation } from '../../services/utility.constants';
import { UtilityService } from '../../services/utility.service';
import { DeleteConfirmDialogComponent } from '../../sharedcomponent/delete-confirm-dialog';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { ParentSystemSetupComponent } from '../../sharedcomponent/parentsystemsetup.component';
import { ValidationService } from '../../sharedcomponent/ValidationService';
import { AddupdatecompanysetupComponent } from '../addupdatecompanysetup/addupdatecompanysetup.component';


@Component({
  selector: 'app-companysetup',
  templateUrl: './companysetup.component.html',
  styles: [
  ],
})
export class CompanysetupComponent extends ParentSystemSetupComponent implements OnInit {

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  //@ViewChild(MatPaginator) paginator: MatPaginator;
  //@ViewChild(MatSort) sort: MatSort;
  displayedColumns: string[] = ['companyname', 'Phone', 'Email', 'companyAddress', 'vat', 'Actions'];
  data: MatTableDataSource<any>;
  totalItemsCount: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;

  constructor(private apiService: ApiService, private authService: AuthorizeService, private translate: TranslateService,
    private utilService: UtilityService, private notifyService: NotificationService, public dialog: MatDialog,
    public pageService: PaginationService) {
    super(authService);
  }

  //ngOnChanges() {
  //  this.totalItemsCount = this.totalItemsCount;
  //}

  ngOnInit(): void {
    this.initialLoading();
  }

  refresh() {
    this.searchValue = '';
    this.initialLoading();
  }

  initialLoading() {
    this.loadList(0, this.pageService.pageCount, "", this.sortingOrder);
  }

  onSortOrder(sort: Sort) {
    //this.data.sort?.direction = sort.direction === 'asc' ? 'desc' : 'asc';
    //console.log(sort)
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
    this.apiService.getPagination('company', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
      this.totalItemsCount = 0;
      //this.forecasts = result.items;    
      this.data = new MatTableDataSource(result.items);
      this.totalItemsCount = result.totalCount
      this.data.paginator = this.paginator;
      this.data.sort = this.sort;

      //setTimeout(() => {
      //  this.paginator.pageIndex = page as number;
      //  this.paginator.length = this.totalItemsCount;
      //});    
      this.isLoading = false;
    }, error => this.utilService.ShowApiErrorMessage(error));
  }

  searchFilter(value: string) {
    this.data.filter = value.trim().toLowerCase();
  }

  //applyFilter(searchVal: any) {
  //  const search = searchVal;//.target.value as string;
  //  //if (search && search.length >= 3) {
  //  if (search) {
  //    this.searchValue = search;
  //    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
  //  }
  //}


  //private openDialogManage(id: number = 0, dbops: DBOperation, modalTitle: string, modalBtnTitle: string) {
  //  let dialogRef = this.utilService.openCrudDialog(this.dialog, AddupdatebranchComponent, '70%');
  //  dialogRef.componentInstance.dbops = dbops;
  //  dialogRef.componentInstance.modalTitle = modalTitle;
  //  dialogRef.componentInstance.modalBtnTitle = modalBtnTitle;
  //  dialogRef.componentInstance.id = id;

  //  dialogRef.afterClosed().subscribe(res => {
  //    this.initialLoading();
  //  });
  //}

  //getData(): Array<any> {
  //  let data: Array<any> = [
  //    { "branchCode": "BR0001", "branchName": "saba branch", "branchAddress": "hyd ", "phone": "4343434343", "mobile": "4343434343", "city": "1", "state": "Telangana", "authorityName": "test authority", "remarks": null, "isActive": true, "id": 8 },
  //    { "branchCode": "BR00777777721", "branchName": "branch", "branchAddress": "hyd one", "phone": "4343434343", "mobile": "4343434343", "city": "3", "state": "Telangana", "authorityName": "test authority.", "remarks": null, "isActive": false, "id": 9 }
  //  ]
  //  return data;
  //}

  //private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
  //  this.apiService.getPagination('branch', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
  //    this.totalItemsCount = 0;
  //    //this.forecasts = result.items;
  //    this.data = new MatTableDataSource(this.getData());
  //    //  this.data = new MatTableDataSource(result.items);
  //    this.totalItemsCount = 2;
  //    //this.totalItemsCount = result.totalCount
  //    //this.data.data = this.forecasts;
  //    this.data.paginator = this.paginator;
  //    this.data.sort = this.sort;

  //  }, error => console.error(error));
  //}




  private openDialogManage(id: number = 0, dbops: DBOperation, modalTitle: string, modalBtnTitle: string) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, AddupdatecompanysetupComponent);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }

  public create() {
    this.openDialogManage(0, DBOperation.create, this.translate.instant('AddNewCompany'), 'Add');
  }

  public edit(id: number) {
    this.openDialogManage(id, DBOperation.update, this.translate.instant('UpdateCompany'), 'Update');
  }

  public delete(id: number) {
    //const dialogRef = this.dialog.open(DeleteConfirmDialogComponent, {
    //  width: '350px',
    //  data: `Are you sure to delete ?`
    //});
    // dialogRef.componentInstance.modalTitle = 'Deletion';
    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete && id > 0) {
        //this.apiService.delete('branch', id).subscribe(res => {
        //  this.refresh();
        //  this.utilService.OkMessage();
        //},
        //);
      }
    },
      error => this.utilService.ShowApiErrorMessage(error));
  }

}
