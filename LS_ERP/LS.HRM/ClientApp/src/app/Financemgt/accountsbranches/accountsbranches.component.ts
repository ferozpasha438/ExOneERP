import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TranslateService } from '@ngx-translate/core';

import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { DBOperation } from '../../services/utility.constants';
import { UtilityService } from '../../services/utility.service';
import { DeleteConfirmDialogComponent } from '../../sharedcomponent/delete-confirm-dialog';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { ParentSystemSetupComponent } from '../../sharedcomponent/parentsystemsetup.component';
import { AddupdateaccountbranchComponent } from '../sharedpages/addupdateaccountbranch/addupdateaccountbranch.component';
import { FindistributionComponent } from '../sharedpages/findistribution/findistribution.component';

@Component({
  selector: 'app-accountsbranches',
  templateUrl: './accountsbranches.component.html',
  //styleUrls: ['./accountsbranches.component.scss']  
  
})
export class AccountsbranchesComponent extends ParentSystemSetupComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort!: MatSort;
  //@ViewChild(MatPaginator) paginator: MatPaginator;
  //@ViewChild(MatSort) sort: MatSort;
  displayedColumns: string[] = ['finBranchCode', 'finBranchName', 'finBranchAddress', 'finBranchType', 'finBranchIsActive','Actions'];
  data!: MatTableDataSource<any>;
  totalItemsCount!: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;

  constructor(private apiService: ApiService, private authService: AuthorizeService, private translate: TranslateService,
    private utilService: UtilityService, private notifyService: NotificationService, public dialog: MatDialog,
    public pageService: PaginationService) {
    super(authService);
  }

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
    this.apiService.getPagination('accountsbranches', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
      this.totalItemsCount = 0;
      //this.forecasts = result.items;    
      this.data = new MatTableDataSource(result.items);
      this.totalItemsCount = result.totalCount
      //this.data.data = this.forecasts;
      setTimeout(() => {
        this.paginator.pageIndex = page as number;
        this.paginator.length = this.totalItemsCount;
      });
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


  private openDialogManage<T>(id: number = 0, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, component: T, finBranchCode: string = '') {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;
    (dialogRef.componentInstance as any).finBranchCode = finBranchCode;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }

  public create() {
    this.openDialogManage(0, DBOperation.create, this.translate.instant('AddBranch'), 'Add', AddupdateaccountbranchComponent);
  }

  public edit(id: number) {
    this.openDialogManage(id, DBOperation.update, this.translate.instant('UpdateBranch'), 'Update', AddupdateaccountbranchComponent);
  }

  public distribution(id: number, finBranchCode : string) {
    this.openDialogManage(id, DBOperation.update, this.translate.instant('UpdateBranch'), 'Update', FindistributionComponent, finBranchCode);
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
        this.apiService.delete('', id).subscribe(res => {
          this.utilService.OkMessage();
        },
        );
      }
    },
      error => this.utilService.ShowApiErrorMessage(error));
  }

}
