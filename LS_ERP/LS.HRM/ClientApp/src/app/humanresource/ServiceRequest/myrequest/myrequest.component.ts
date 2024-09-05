import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { DeleteConfirmDialogComponent } from '../../../sharedcomponent/delete-confirm-dialog';
import { PaginationService } from '../../../sharedcomponent/pagination.service';
import { ParentHrmAdminComponent } from '../../../sharedcomponent/ParentHrmAdmin.component';
import { ServicerequestinfoComponent } from '../servicerequestinfo/servicerequestinfo.component';
import { EmployeeexitreentryComponent } from '../shared/employeeexitreentry/employeeexitreentry.component';
import { EmployeereportingbackComponent } from '../shared/employeereportingback/employeereportingback.component';


@Component({
  selector: 'app-myrequest',
  templateUrl: './myrequest.component.html',
  styles: [
  ]
})
export class MyrequestComponent extends ParentHrmAdminComponent implements OnInit {

  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort!: MatSort;

  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  totalItemsCount: number = 0;
  data: MatTableDataSource<any> = new MatTableDataSource();
  displayedColumns: string[] = ['serviceRequestRefNo', 'serviceRequestType', 'employeeName', 'isApproved', 'Actions'];
  isArab: boolean = false;
  serviceRequestRefNo: string = '';
  constructor(private apiService: ApiService, private authService: AuthorizeService, private translate: TranslateService,
    private utilService: UtilityService, private notifyService: NotificationService, public dialog: MatDialog,
    public pageService: PaginationService) {
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
    this.loadList(0, this.pageService.pageCount, "", this.sortingOrder);
  }
  onPageSwitch(event: PageEvent) {
    this.pageService.change(event);
    this.loadList(event.pageIndex, event.pageSize, this.searchValue, this.sortingOrder);
  }
  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
    this.isLoading = true;

    this.apiService.getPagination('ServiceRequest/getMyServiceRequestList', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
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
    }, error => this.utilService.ShowApiErrorMessage(error));
  }

  applyFilter(searchValue: any) {
    const search = searchValue;//.target.value as string;
    //if (search && search.length >= 3) {
    if (search) {
      this.searchValue = search;
      this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
    }
  }

  private openDialogManage(id: number = 0, dbops: DBOperation, modalTitle: string, modalBtnTitle: string) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, ServicerequestinfoComponent);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).serviceRequestRefNo = this.serviceRequestRefNo;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;
    (dialogRef.componentInstance as any).isFromAppoval = false;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }

  private openComponentDialogManage<T>(data: any, modalTitle: string, component: T) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, component, 75);
    (dialogRef.componentInstance as any).data = data;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }

  public create() {
    this.serviceRequestRefNo = '';
    this.openDialogManage(0, DBOperation.create, this.translate.instant('AddLeaveTemplate'), 'Add');
  }
  public edit(row: any) {
    this.serviceRequestRefNo = row.serviceRequestRefNo;
    this.openDialogManage(row.id, DBOperation.update, this.translate.instant('UpdateLeaveTemplate'), 'Update');
  }
  public release(row: any) {
    this.serviceRequestRefNo = row.serviceRequestRefNo;
    this.openComponentDialogManage(row, this.translate.instant('Employeeexitreentry'), EmployeeexitreentryComponent);
  }
  public reporting(row: any) {
    this.serviceRequestRefNo = row.serviceRequestRefNo;
    this.openComponentDialogManage(row, this.translate.instant('Employeeexitreentry'), EmployeereportingbackComponent);
  }
  public cancel(row: any) {
    this.serviceRequestRefNo = row.serviceRequestRefNo;
    this.openComponentDialogManage(row, this.translate.instant('Employeereportingback'), EmployeereportingbackComponent);
  }

  public delete(id: number) {
    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete && id > 0) {
        this.apiService.delete('LeaveTemplate', id).subscribe(res => {
          this.refresh();
          this.utilService.OkMessage();
        },
        );
      }
    },
      error => this.utilService.ShowApiErrorMessage(error));
  }

}


