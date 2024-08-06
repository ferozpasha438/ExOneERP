import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';
import { ApiService } from '../../../services/api.service';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { PaginationService } from '../../../sharedcomponent/pagination.service';
import { OprServicesService } from '../../opr-services.service';
import { DBOperation } from '../../../services/utility.constants';
import { TranslateService } from '@ngx-translate/core';
import { EmployeeprimarysitelogsComponent } from './employeeprimarysitelogs/employeeprimarysitelogs.component';
import { AddupdateEmployeeprimarysitelogComponent } from './employeeprimarysitelogs/addupdate-employeeprimarysitelog/addupdate-employeeprimarysitelog.component';


@Component({
  selector: 'app-employee-primarysitelogs',
  templateUrl: './employee-primarysitelogs.component.html'
})
export class EmployeePrimarysitelogsComponent
  extends ParentOptMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['employeeNumber', 'employeeName', 'employeeNameAr', 'primarySiteCode','primarySiteName', 'primarySiteNameAr',/*'lastTransferDate',*/ 'Actions'];
  data: MatTableDataSource<any>;
  totalItemsCount: number;
  searchValue: string = '';
  sortingOrder: string = 'primarySiteLogId desc';
  isLoading: boolean = false;
  form: FormGroup;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, private authService: AuthorizeService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService, private oprService: OprServicesService, private translate: TranslateService) {
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
    this.loadList(0, this.pageService.pageCount, "", this.sortingOrder);
  }


  onPageSwitch(event: PageEvent) {
    this.pageService.change(event);
    this.loadList(event.pageIndex, event.pageSize, "", this.sortingOrder);
  }

  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
    this.isLoading = true;
    this.apiService.getPagination('Employee/getEmployeesPrimarySiteLogPagedList', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
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
    if (search=='' || search.length >= 3) {
      if (search) {
        this.searchValue = search;
        this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
      }
    }
  }
  //private openDialogManage(id: number = 0, dbops: DBOperation, modalTitle: string, modalBtnTitle: string) {
  //  let dialogRef = this.utilService.openCrudDialog(this.dialog, AddupdateSurveyorComponent);
  //  (dialogRef.componentInstance as any).dbops = dbops;
  //  (dialogRef.componentInstance as any).modalTitle = modalTitle;
  //  (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
  //  (dialogRef.componentInstance as any).id = id;

  //  dialogRef.afterClosed().subscribe(res => {
  //    if (res && res === true)
  //      this.initialLoading();
  //  });
  //}


  add(row:any) {
    if (!this.isLoading)
      this.openDialogManage2(row,0, DBOperation.update, row.primarySiteLogId > 0 ? 'Updating_Primary_Site_Log' : 'Adding_Primary_Site_Log', 'Update', AddupdateEmployeeprimarysitelogComponent);
  }
 
  view(row: any) {
    if (!this.isLoading)
      this.openDialogManage(row, DBOperation.update, 'Primary_Sites_Log', 'View', EmployeeprimarysitelogsComponent);

  }


  private openDialogManage(requestData: any, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).requestData = requestData;
    (dialogRef.componentInstance as any).employeeNumber = requestData.employeeNumber;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
  private openDialogManage2(inputData:any,employeePrimarySitesLogID: number, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {
     let dialogRef = this.oprService.openFullWidthDialog(this.dialog, Component);
     (dialogRef.componentInstance as any).dbops = dbops;
     (dialogRef.componentInstance as any).modalTitle = modalTitle;
     (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
     (dialogRef.componentInstance as any).employeePrimarySitesLogID = employeePrimarySitesLogID;
    (dialogRef.componentInstance as any).inputData = { employeeNumber: inputData.employeeNumber, employeeName: inputData.employeeName, employeeNameAr: inputData.employeeNameAr };

     dialogRef.afterClosed().subscribe(res => {
       if (res && res === true)
         this.initialLoading();
     });
  }

}
