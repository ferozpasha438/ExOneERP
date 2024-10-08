import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { DBOperation } from '../../../../services/utility.constants';
import { UtilityService } from '../../../../services/utility.service';
import { DeleteConfirmDialogComponent } from '../../../../sharedcomponent/delete-confirm-dialog';
import { PaginationService } from '../../../../sharedcomponent/pagination.service';
import { ParentHrmAdminComponent } from '../../../../sharedcomponent/ParentHrmAdmin.component';
import { AddupdatedocumentinfoComponent } from '../AddupdateDocument/addupdatedocumentinfo.component';
import { default as constants } from '../../../../../assets/i18n/constants.json';

@Component({
  selector: 'app-getemployeedocumentinfo',
  templateUrl: './getemployeedocumentinfo.component.html',
  styles: [],
})
export class GetemployeedocumentinfoComponent
  extends ParentHrmAdminComponent
  implements OnInit
{
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort!: MatSort;
  @Input() employeeNumber!: string;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  totalItemsCount: number = 0;
  data: MatTableDataSource<any> = new MatTableDataSource();
  displayedColumns: string[] = [
    'sno',
    'documentTypeName',
    'name',
    'documentNumber',
    'isVerified',
    'Actions',
  ];
  isArab: boolean = false;
  employeeBasicInfo: any;
  fileUrl = '';

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
    this.fileUrl = this.authService.ApiEndPoint();
    this.initialLoading();
  }

  refresh() {
    this.searchValue = '';
    this.initialLoading();
  }

  initialLoading() {
    this.getEmployeeBasicInfo();
    this.loadList(
      0,
      this.pageService.pageCount,
      '',
      this.sortingOrder,
      '',
      '',
      0,
      this.employeeNumber
    );
  }

  private getEmployeeBasicInfo() {
    this.apiService
      .getQueryString(
        `PersonalInformation/GetEmployeePersonalInformationById?employeeNumber=`,
        this.employeeNumber
      )
      .subscribe((res) => {
        if (res) {
          res.allowImageUpload = false;
          this.employeeBasicInfo = res;
          if (
            !(this.employeeBasicInfo.employeeImageUrl as string)?.includes(
              constants.employeeProfile
            )
          )
            this.employeeBasicInfo.employeeImageUrl = `${this.authService
              .ApiEndPoint()
              .replace('api', '')}${this.employeeBasicInfo.employeeImageUrl}`;
        }
      });
  }

  private loadList(
    page: number | undefined,
    pageCount: number | undefined,
    query: string | null | undefined,
    orderBy: string | null | undefined,
    approval: string = '',
    statusId: string = '',
    id: number = 0,
    code: string = ''
  ) {
    this.isLoading = true;

    this.apiService
      .getPagination(
        'EmployeeDocument',
        this.utilService.getQueryString(
          page,
          pageCount,
          query,
          orderBy,
          approval,
          statusId,
          id,
          code
        )
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

          this.isLoading = false;
        },
        (error) => this.utilService.ShowApiErrorMessage(error)
      );
  }

  applyFilter(searchValue: any) {
    const search = searchValue; //.target.value as string;
    if (search) {
      this.searchValue = search;
      this.loadList(
        0,
        this.pageService.pageCount,
        this.searchValue,
        this.sortingOrder,
        '',
        '',
        0,
        this.employeeNumber
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
      AddupdatedocumentinfoComponent,
      '80%',
      '55%'
    );
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;
    (dialogRef.componentInstance as any).employeeNumber = this.employeeNumber;

    dialogRef.afterClosed().subscribe((res) => {
      if (res && res === true) this.initialLoading();
    });
  }
  public create() {
    this.openDialogManage(
      0,
      DBOperation.create,
      this.translate.instant('AddEmployeeDocument'),
      'Add'
    );
  }
  public edit(id: number) {
    this.openDialogManage(
      id,
      DBOperation.update,
      this.translate.instant('UpdateEmployeeDocument'),
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
          let queryParam = `id=${encodeURIComponent(
            '' + id
          )}&employeeID=${encodeURIComponent(
            '' + Number(this.employeeNumber)
          )}&deleteDocument=true`;
          this.apiService
            .getQueryString(
              `EmployeeDocument/GetEmployeeDocumentById?`,
              queryParam
            )
            .subscribe((res) => {
              this.refresh();
              this.utilService.OkMessage();
            });
        }
      },
      (error) => this.utilService.ShowApiErrorMessage(error)
    );
  }
}
