import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { DBOperation } from '../../services/utility.constants';
import { UtilityService } from '../../services/utility.service';
import { DeleteConfirmDialogComponent } from '../../sharedcomponent/delete-confirm-dialog';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { ParentSalesMgtComponent } from '../../sharedcomponent/parentsalesmgt.component';
import { ValidationService } from '../../sharedcomponent/ValidationService';
import { SndAddUpdateAuthoritiesComponent } from './snd-add-update-authorities/snd-add-update-authorities.component';

@Component({
  selector: 'app-snd-authorities',
  templateUrl: './snd-authorities.component.html'
})
export class SndAuthoritiesComponent extends ParentSalesMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  //@ViewChild(MatPaginator) paginator: MatPaginator;
  //@ViewChild(MatSort) sort: MatSort;
  displayedColumns: string[] = ['branchCode', 'appAuth', 'userName', 'canCreateSndInvoice',
    'canEditSndInvoice', 'canApproveSndInvoice', 'canPostSndInvoice', 'canSettleSndInvoice', 'canVoidSndInvoice',
    'canCreateSndQuotation',
    'canEditSndQuotation', 'canApproveSndQuotation', 'canReviseSndQuotation', 'canVoidSndQuotation', 'canConvertSndQuotationToOrder', 'canConvertSndQuotationToInvoice',
    'canConvertSndQuotationToDeliveryNote',

    'Actions'];
  data: MatTableDataSource<any>;
  totalItemsCount: number;
  sortingOrder: string = 'id desc';
  form: FormGroup;
  isLoading: boolean = false;
  searchValue: string = '';
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, private authService: AuthorizeService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService) {
    super(authService);
  }


  ngOnInit(): void {
    this.initialLoading();
  }


  initialLoading() {
    this.loadServices(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
  }
  onSortOrder(sort: any) {
    this.totalItemsCount = 0;
    this.sortingOrder = sort.active + ' ' + sort.direction;
    this.loadServices(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
  }


  onPageSwitch(event: PageEvent) {
    this.pageService.change(event);
    this.loadServices(event.pageIndex, event.pageSize, this.searchValue, this.sortingOrder);
  }

  private loadServices(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
    this.isLoading = true;
    this.apiService.getPagination('SndAuthorities/getAuthoritiesPagedList', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
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
      this.loadServices(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
    }
  }
  private openDialogManage(id: number = 0, dbops: DBOperation, modalTitle: string, modalBtnTitle: string) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, SndAddUpdateAuthoritiesComponent);
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
    this.openDialogManage(0, DBOperation.create, 'Adding_New_Authority', 'Add');
  }
  public edit(id: number) {
    this.openDialogManage(id, DBOperation.update, 'Updating_Authority', 'Update');
  }
  public delete(id: number) {
    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete && id > 0) {
        this.apiService.delete('SndAuthorities', id).subscribe(res => {
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
}

