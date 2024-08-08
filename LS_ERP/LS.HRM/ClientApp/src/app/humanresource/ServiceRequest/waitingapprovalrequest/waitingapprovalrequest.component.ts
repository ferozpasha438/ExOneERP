import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { SelectionModel } from '@angular/cdk/collections';
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
import { VacationrequestComponent } from '../vacationrequest/vacationrequest.component';
import { ServicerequestinfoComponent } from '../servicerequestinfo/servicerequestinfo.component';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { MultiapprovalrequestComponent } from '../shared/multiapprovalrequest/multiapprovalrequest.component';

@Component({
  selector: 'app-waitingapprovalrequest',
  templateUrl: './waitingapprovalrequest.component.html',
  styles: [
  ]
})
export class WaitingapprovalrequestComponent extends ParentHrmAdminComponent implements OnInit {

  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort!: MatSort;

  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  totalItemsCount: number = 0;
  data: MatTableDataSource<any> = new MatTableDataSource();
  displayedColumns: string[] = ['select', 'serviceRequestRefNo', 'serviceRequestType', 'employeeName', 'processedby', 'isApproved', 'Actions'];
  isArab: boolean = false;
  serviceRequestRefNo: string = '';
  lastRecordId: number = 0;
  listofVacRequest: Array<any> = [];
  empListSelectListItems: Array<CustomSelectListItem> = [];
  selection = new SelectionModel<any>(true, []);
  empId: any;
  empInfo: any;
  hasMore: boolean = false;
  constructor(private apiService: ApiService, private authService: AuthorizeService, private translate: TranslateService,
    private utilService: UtilityService, private notifyService: NotificationService, public dialog: MatDialog,
    public pageService: PaginationService) {
    super(authService);
  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.initialLoading();
    this.loadData();
  }

  refresh() {
    this.selection.clear();
    this.listofVacRequest = [];
    this.empInfo = null;
    this.lastRecordId = 0;
    this.empId = 0;
    this.searchValue = '';
    this.initialLoading();
  }
  loadData() {
    this.apiService.getall(`personalInformation/getEmployeeSelectListItem`).subscribe(res => {
      this.empListSelectListItems = res;
      this.isLoading = false;
    });
  }
  initialLoading() {
    this.loadList(0, this.pageService.pageCount, "", this.sortingOrder);
  }

  onPageSwitch() {//event: PageEvent) {
    // this.pageService.change(event);
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
  }
  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
    this.isLoading = true;

    this.apiService.getPagination('ServiceRequest/getWaitingApprovalServiceRequestList', this.utilService.getQueryString(page, pageCount, query, orderBy, '', '', this.lastRecordId, this.empId)).subscribe(result => {
      this.totalItemsCount = 0;
      if (result.items && result.items.length > 0) {
        this.hasMore = (result.items.length as number) == this.pageService.pageCount;
        (result.items as Array<any>).forEach(item => this.listofVacRequest.push(item));
      }
      else {
        this.hasMore = false;
      }

      this.data = new MatTableDataSource(this.listofVacRequest);

      ////this.totalItemsCount = result.totalCount;
      this.lastRecordId = result.pageIndex;

      ////setTimeout(() => {
      ////  this.paginator.pageIndex = page as number;
      ////  this.paginator.length = this.totalItemsCount;
      ////});
      ////this.data.sort = this.sort;

      //console.log(this.data.sort)
      //console.log(this.data.paginator)

      this.isLoading = false;
    }, error => this.utilService.ShowApiErrorMessage(error));
  }

  /** Whether the number of selected elements matches the total number of rows. */
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.data.data.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  toggleAllRows() {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }

    this.selection.select(...this.data.data);
  }

  approveSelectedRequests() {
    if (this.selection.selected.length > 0) {
      let dialogRef = this.utilService.openCrudDialog(this.dialog, MultiapprovalrequestComponent, '50', '35');
      (dialogRef.componentInstance as any).data = this.selection.selected.map(item => item.id);

      dialogRef.afterClosed().subscribe(res => {
        if (res && res === true) {
          this.refresh();
        }
      });
    }
    else
      this.notifyService.showError('select requests');
  }

  selectEmpSelect(evt: any) {
    if (evt) {
      let empIetm = this.empListSelectListItems.find(e => e.value == evt.value) as any;
      this.empId = empIetm.intValue;
    }
    else
      this.empId = 0;
  }
  getSelectedEmpInfo(empIem: any) {
    this.empId = empIem.intValue;
  }

  applyFilter(searchValue: any) {
    const search = searchValue;//.target.value as string;
    //if (search && search.length >= 3) {
    if (search || this.empId) {
      this.selection.clear();
      this.listofVacRequest = [];
      this.searchValue = search;
      this.lastRecordId = 0;
      this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
    }
  }

  private openDialogManage(id: number = 0, dbops: DBOperation, modalTitle: string, modalBtnTitle: string) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, ServicerequestinfoComponent);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).serviceRequestRefNo = this.serviceRequestRefNo;
    (dialogRef.componentInstance as any).id = id;
    (dialogRef.componentInstance as any).isFromAppoval = true;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }

  //public create() {
  //  this.openDialogManage(0, DBOperation.create, this.translate.instant('AddLeaveTemplate'), 'Add');
  //}
  public edit(row: any) {
    this.serviceRequestRefNo = row.serviceRequestRefNo;
    this.openDialogManage(row.id, DBOperation.update, this.translate.instant('UpdateLeaveTemplate'), 'Update');
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


