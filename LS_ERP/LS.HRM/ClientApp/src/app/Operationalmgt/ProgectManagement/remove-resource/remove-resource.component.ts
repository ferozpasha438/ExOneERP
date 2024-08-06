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
import { NotificationService } from '../../../services/notification.service';
import { PaginationService } from '../../../sharedcomponent/pagination.service';
import { OprServicesService } from '../../opr-services.service';
import { UtilityService } from '../../../services/utility.service';
import { ApiService } from '../../../services/api.service';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ValidationService } from '../../../sharedcomponent/ValidationService';

import { DBOperation } from '../../../services/utility.constants';
import { TranslateService } from '@ngx-translate/core';

import { ConfirmDialogWindowComponent } from '../../confirm-dialog-window/confirm-dialog-window.component';
import { DeleteConfirmDialogComponent } from '../../../sharedcomponent/delete-confirm-dialog';
import { DatePipe } from '@angular/common';
import { CreateUpdateReqRemoveResourceComponent } from './create-update-req-remove-resource/create-update-req-remove-resource.component';
import { UploadAdendumComponent } from '../../upload-adendum/upload-adendum.component';
import { AddendumFormComponent } from '../../project/project-sites/addendum-form/addendum-form.component';


@Component({
  selector: 'app-remove-resource',
  templateUrl: './remove-resource.component.html'
})
export class RemoveResourceComponent extends ParentOptMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['customerCode', 'siteCode', 'projectCode','employeeNumber','fromDate', 'isApproved', /*'isMerged',*/ 'Actions'];
  data: MatTableDataSource<any>;
  totalItemsCount: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  form: FormGroup;
  isUpdating: boolean = false;
  constructor(public datepipe: DatePipe,private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, private authService: AuthorizeService,
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
    this.loadList(0, this.pageService.pageCount,this.searchValue, this.sortingOrder);
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
    this.apiService.getPagination('PvRemoveResource/getPvRemoveResourceReqsPagedList', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
      this.totalItemsCount = 0;
      console.log(result.items);
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


  private openConfirmationDialogManage(id: number, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any, confirmType: string) {
    let dialogRef = this.oprService.confirmationDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;
    (dialogRef.componentInstance as any).confirmType = "general";

    dialogRef.afterClosed().subscribe(res => {

      if (res) {
        this.apiService.get('PvRemoveResource/ApproveReqPvRemoveResourceReqById', id).subscribe(res2 => {
          this.isUpdating = false;
          if (res2) {

            this.utilService.OkMessage();
            this.ngOnInit();


          }

        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
            this.isUpdating = false;

          });
      }
      else {
        this.isUpdating = false;
      }

    });
  

  }

  public create() {
    if (!this.isUpdating && !this.isLoading) {
    this.openDialogManage(0, DBOperation.create, 'New_Request_For_Remove_Resource', 'Add', CreateUpdateReqRemoveResourceComponent);
  }
  }

  public edit(id: number) {
    if (!this.isUpdating && !this.isLoading) {
    this.openDialogManage(id, DBOperation.update, 'Updating_Request_For_Remove_Resource', 'Update', CreateUpdateReqRemoveResourceComponent);
  }
  }
  public delete(id: number) {
    if (!this.isUpdating && !this.isUpdating) {
      const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
      dialogRef.afterClosed().subscribe(canDelete => {
        if (canDelete && id > 0) {
          this.isUpdating = true;
          this.apiService.delete('PvRemoveResource', id).subscribe(res => {
            this.isUpdating = false;
            this.utilService.OkMessage();
            this.ngOnInit();
          },
          );
        }
      },
        error => {
          this.isUpdating = false;
          this.utilService.ShowApiErrorMessage(error);
        });
    }
  
  }
  submit() {

  }


  translateToolTip(msg: string) {
       return `${this.translate.instant(msg)}`;

  }

  public viewRequest(row: any) {
    this.openDialogManage(row.id, DBOperation.create, 'Request_For_Remove_Resource', 'View', CreateUpdateReqRemoveResourceComponent);
  }
  public mergeRequest(id: number) {

  }
  public approveRequest(id: number) {

    if (!this.isUpdating && !this.isLoading) {
      this.isUpdating = true;
      this.openConfirmationDialogManage(id, DBOperation.update, 'Confirming_Approve_Request', 'Approve', ConfirmDialogWindowComponent, "general");
    }
    else {
      this.notifyService.showError(this.translate.instant("Please Wait..."));
    }
  }
 


  ToDateString(date: any) {

    if (date != null)
      return this.datepipe.transform(date.toString(), 'yyyy-MM-dd')?.toString();
    else
      return "";
  }

  viewAddendum(row: any) {
    window.open(row.fileUrl);
  }

  uploadAdendum(row: any) {
    let dialogRef = this.oprService.openAutoWidthDialog(this.dialog, UploadAdendumComponent);
    (dialogRef.componentInstance as any).requestData = row;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }

  public viewAddendumForm(row: any) {

    let dialogRef = this.utilService.openCrudDialog(this.dialog, AddendumFormComponent);
    (dialogRef.componentInstance as any).pvRequest = row;
    (dialogRef.componentInstance as any).type = row.requestType == "AddResource" ? "ForAddingResources" : row.requestType == "RemoveResource" ? "ForRemovingResources" : "";

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });

  }
}


