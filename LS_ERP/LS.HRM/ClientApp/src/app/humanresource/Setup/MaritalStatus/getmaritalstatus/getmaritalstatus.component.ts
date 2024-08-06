import { Component, OnInit, ViewChild } from '@angular/core';
import { AddupdatemaritalstatusComponent } from '../addupdatemaritalstatus/addupdatemaritalstatus.component';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from 'src/app/api-authorization/AuthorizeService';
import { ApiService } from 'src/app/services/api.service';
import { NotificationService } from 'src/app/services/notification.service';
import { DBOperation } from 'src/app/services/utility.constants';
import { UtilityService } from 'src/app/services/utility.service';
import { DeleteConfirmDialogComponent } from 'src/app/sharedcomponent/delete-confirm-dialog';
import { PaginationService } from 'src/app/sharedcomponent/pagination.service';
import { ParentHrmAdminComponent } from 'src/app/sharedcomponent/ParentHrmAdmin.component';

@Component({
  selector: 'app-getmaritalstatus',
  templateUrl: './getmaritalstatus.component.html',
  styles: [],
})
export class GetmaritalstatusComponent
  extends ParentHrmAdminComponent
  implements OnInit
{
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort!: MatSort;

  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  totalItemsCount: number = 0;
  data: MatTableDataSource<any> = new MatTableDataSource();
  displayedColumns: string[] = [
    'sno',
    'maritalStatusCode',
    'maritalStatusName',
    'isActive',
    'Actions',
  ];
  isArab: boolean = false;

  constructor(
    private apiService: ApiService,
    private authService: AuthorizeService,
    private translate: TranslateService,
    private utilService: UtilityService,
    private notifyService: NotificationService,
    public dialog: MatDialog,
    public pageService: PaginationService
  ) {
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
    this.loadList(0, this.pageService.pageCount, '', this.sortingOrder);
  }

  private loadList(
    page: number | undefined,
    pageCount: number | undefined,
    query: string | null | undefined,
    orderBy: string | null | undefined
  ) {
    this.isLoading = true;

    this.apiService
      .getPagination(
        'MaritalStatus',
        this.utilService.getQueryString(page, pageCount, query, orderBy)
      )
      .subscribe(
        (result) => {
          this.totalItemsCount = 0;
          this.data = new MatTableDataSource(result.items);

          this.totalItemsCount = result.totalCount;

          setTimeout(() => {
            this.paginator.pageIndex = page as number;
            this.paginator.length = this.totalItemsCount;
          });
          this.data.sort = this.sort;

          //console.log(this.data.sort)
          //console.log(this.data.paginator)

          this.isLoading = false;
        },
        (error) => this.utilService.ShowApiErrorMessage(error)
      );
  }

  applyFilter(searchValue: any) {
    const search = searchValue; //.target.value as string;
    //if (search && search.length >= 3) {
    if (search) {
      this.searchValue = search;
      this.loadList(
        0,
        this.pageService.pageCount,
        this.searchValue,
        this.sortingOrder
      );
    }
  }

  private openDialogManage(
    id: number = 0,
    dbops: DBOperation,
    modalTitle: string,
    modalBtnTitle: string
  ) {
    let dialogRef = this.utilService.openCrudDialog(
      this.dialog,
      AddupdatemaritalstatusComponent
    );
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;

    dialogRef.afterClosed().subscribe((res) => {
      if (res && res === true) this.initialLoading();
    });
  }

  public create() {
    this.openDialogManage(
      0,
      DBOperation.create,
      this.translate.instant('AddMartialStatus'),
      'Add'
    );
  }
  public edit(id: number) {
    this.openDialogManage(
      id,
      DBOperation.update,
      this.translate.instant('UpdateMartialStatus'),
      'Update'
    );
  }

  public delete(id: number) {
    const dialogRef = this.utilService.openDeleteConfirmDialog(
      this.dialog,
      DeleteConfirmDialogComponent
    );
    dialogRef.afterClosed().subscribe(
      (canDelete) => {
        if (canDelete && id > 0) {
          this.apiService.delete('MaritalStatus', id).subscribe((res) => {
            this.refresh();
            this.utilService.OkMessage();
          });
        }
      },
      (error) => this.utilService.ShowApiErrorMessage(error)
    );
  }
}
