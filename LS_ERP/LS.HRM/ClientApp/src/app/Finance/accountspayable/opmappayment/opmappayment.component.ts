import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { MultiFileUploadDto } from '../../../models/sharedDto';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { DeleteConfirmDialogComponent } from '../../../sharedcomponent/delete-confirm-dialog';
import { PaginationService } from '../../../sharedcomponent/pagination.service';
import { ParentFinMgtComponent } from '../../../sharedcomponent/parentfinmgt.component';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { OpmVendorpaymentvoucher } from '../../sharedpages/vouchers/opmvendorpaymentvoucher.component';
import { AddupdateopmvendorpaymentComponent } from '../sharedpages/addupdateopmvendorpayment/addupdateopmvendorpayment.component';
import { AddupdatevendorpaymentComponent } from '../sharedpages/addupdatevendorpayment/addupdatevendorpayment.component';
import { VendorstatementComponent } from '../sharedpages/vendorstatement/vendorstatement.component';

  @Component({
    selector: 'app-opmappayment',
    templateUrl: './opmappayment.component.html',
    styles: [
    ]
  })
  export class OpmappaymentComponent extends ParentFinMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  //@ViewChild(MatPaginator) paginator: MatPaginator;
  //@ViewChild(MatSort) sort: MatSort;
  displayedColumns: string[] = ['voucherNumber', 'custName', 'branchName', 'tranDate', 'docNum', 'checkNumber', 'checkdate', 'amount', 'Actions'];
  data: MatTableDataSource<any>;
  totalItemsCount: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;


  constructor(private fb: FormBuilder, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public pageService: PaginationService, public dialog: MatDialog) {
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
    // sort.direction.direction === 'asc' ? 'desc' : 'asc';
    // console.log(sort.active + ' ' + sort.direction);

    this.sortingOrder = sort.active + ' ' + sort.direction;
    this.totalItemsCount = 0;
    this.loadList(0, this.pageService.pageCount, "", this.sortingOrder);
  }


  onPageSwitch(event: PageEvent) {
    this.pageService.change(event);
    this.loadList(event.pageIndex, event.pageSize, "", this.sortingOrder);
  }

  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
    this.isLoading = true;

    this.apiService.getPagination('OpmApPayment', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
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

  applyFilter(searchVal: any) {
    const search = searchVal;//.target.value as string;
    //if (search && search.length >= 3) {
    if (search) {
      this.searchValue = search;
      this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
    }
  }

  private openDialogManage<T>(id: number = 0, dbops: DBOperation, modalTitle: string = '', modalBtnTitle: string = '', component: T, moduleFile: MultiFileUploadDto = { module: '00', action: '00act' }) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, component,'93');
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).id = id;
    (dialogRef.componentInstance as any).moduleFile = moduleFile;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }

  private openDialogManageOne<T>(id: string = '', dbops: DBOperation, modalTitle: string = '', component: T) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).customerCode = id;

    //dialogRef.afterClosed().subscribe(res => {
    //  if (res && res === true)
    //    this.initialLoading();
    //});
  }


  public create() {
    this.openDialogManage(0, DBOperation.create, this.translate.instant('Create_New_Sales_Invoice'), '', AddupdateopmvendorpaymentComponent);
  }

  public edit(id: number) {
    this.openDialogManage(id, DBOperation.update, this.translate.instant('Create_New_Sales_Invoice'), '', AddupdateopmvendorpaymentComponent);
  }

  //public delete(id: number) {

  //  const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
  //  dialogRef.afterClosed().subscribe(canDelete => {
  //    if (canDelete && id > 0) {
  //      this.apiService.delete('vendorPayment', id).subscribe(res => {
  //        this.refresh();
  //        this.utilService.OkMessage();
  //      },
  //      );
  //    }
  //  },
  //    error => this.utilService.ShowApiErrorMessage(error));
  //}

  public print(customerCode: string) {
    this.openDialogManageOne(customerCode, DBOperation.update, this.translate.instant('Create_New_Sales_Invoice'), VendorstatementComponent);

  }

  public vendorReceipt(id: number) {
    this.openDialogManage(id, DBOperation.update, this.translate.instant('Create_New_Sales_Invoice'), '', OpmVendorpaymentvoucher);

  }

    public post(row: any) {

    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
      (dialogRef.componentInstance as any).modalTitle = `Are you sure to confirm payment (${row.voucherNumber})?`;
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete && row.id > 0) {

        this.apiService.post('OpmApPayment/opmVendorPaymentApproval', { customerCode: row.custCode, Id: row.id })
      .subscribe(res => {
        this.initialLoading();
        this.utilService.OkMessage();
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });

      }
    });
  }


  //public view(id: number) {
  //  this.openDialogManage(id, DBOperation.update, '', '', SalesinvoiceviewComponent);
  //}

}

