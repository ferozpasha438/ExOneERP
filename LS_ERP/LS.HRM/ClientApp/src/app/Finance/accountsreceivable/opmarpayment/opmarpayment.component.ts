import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { ParentSystemSetupComponent } from '../../../sharedcomponent/parentsystemsetup.component';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TranslateService } from '@ngx-translate/core';
import { PaginationService } from '../../../sharedcomponent/pagination.service';
import { MatDialog } from '@angular/material/dialog';
import { DBOperation } from '../../../services/utility.constants';
import { MultiFileUploadDto } from '../../../models/sharedDto';
import { AddupdatecustomerpaymentComponent } from '../sharedpages/addupdatecustomerpayment/addupdatecustomerpayment.component';
import { DeleteConfirmDialogComponent } from '../../../sharedcomponent/delete-confirm-dialog';
import { CustomerstatementComponent } from '../sharedpages/customerstatement/customerstatement.component';
import { Customerreceiptvoucher } from '../../sharedpages/vouchers/customerreceiptvoucher.component';
import { ParentFinMgtComponent } from '../../../sharedcomponent/parentfinmgt.component';
import { AddupdateopmcustomerpaymentComponentComponent } from '../sharedpages/addupdateopmcustomerpayment-component/addupdateopmcustomerpayment-component.component';
import { OpmCustomerreceiptvoucher } from '../../sharedpages/vouchers/opmcustomerreceiptvoucher.component';
import { PaymentsettlementmodeComponent } from '../sharedpages/paymentsettlementmode/paymentsettlementmode.component';

@Component({
  selector: 'app-opmarpayment',
  templateUrl: './opmarpayment.component.html',
  styles: [
  ]
})
export class OpmarpaymentComponent extends ParentFinMgtComponent implements OnInit {
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
  customreCode: string = '';
  customerWidth: string = '98';
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

    this.apiService.getPagination('OpmArPayment', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
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

  private openDialogManage<T>(id: number = 0, dbops: DBOperation, modalTitle: string = '', modalBtnTitle: string = '', component: T,
    moduleFile: MultiFileUploadDto = { module: '00', action: '00act' }) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, component, this.customerWidth);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).id = id;
    (dialogRef.componentInstance as any).moduleFile = moduleFile;
    (dialogRef.componentInstance as any).customerCode = this.customreCode;

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
    this.customerWidth = '98';
    this.openDialogManage(0, DBOperation.create, this.translate.instant('Create_New_Sales_Invoice'), '', AddupdateopmcustomerpaymentComponentComponent);
  }

  public edit(id: number) {
    this.customerWidth = '98';
    this.openDialogManage(id, DBOperation.update, this.translate.instant('Create_New_Sales_Invoice'), '', AddupdateopmcustomerpaymentComponentComponent);
  }

  //public delete(id: number) {

  //  const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
  //  dialogRef.afterClosed().subscribe(canDelete => {
  //    if (canDelete && id > 0) {
  //      this.apiService.delete('customerPayment', id).subscribe(res => {
  //        this.refresh();
  //        this.utilService.OkMessage();
  //      },
  //      );
  //    }
  //  },
  //    error => this.utilService.ShowApiErrorMessage(error));
  //}

  public print(customerCode: string) {
    this.openDialogManageOne(customerCode, DBOperation.update, this.translate.instant('Create_New_Sales_Invoice'), CustomerstatementComponent);

  }

  public customerReceipt(id: number) {
    this.customerWidth = '98';
    this.openDialogManage(id, DBOperation.update, this.translate.instant('Create_New_Sales_Invoice'), '', OpmCustomerreceiptvoucher);

  }

  public post(row: any) {
    this.customreCode = row.custCode;
    this.customerWidth = '50';
    this.openDialogManage(row.id, DBOperation.update, '', '', PaymentsettlementmodeComponent );
  }

  //public post(row: any) {

  //  const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
  //  (dialogRef.componentInstance as any).modalTitle = `Are you sure to confirm payment (${row.voucherNumber})?`;
  //  dialogRef.afterClosed().subscribe(canDelete => {
  //    if (canDelete && row.id > 0) {

  //      this.apiService.post('opmArPayment/customerPaymentApproval', { customerCode: row.custCode, Id: row.id })
  //        .subscribe(res => {
  //          this.initialLoading();
  //          this.utilService.OkMessage();
  //        },
  //          error => {
  //            console.error(error);
  //            this.utilService.ShowApiErrorMessage(error);
  //          });
  //    }
  //  });

  //}


  //public view(id: number) {
  //  this.openDialogManage(id, DBOperation.update, '', '', SalesinvoiceviewComponent);
  //}

}

