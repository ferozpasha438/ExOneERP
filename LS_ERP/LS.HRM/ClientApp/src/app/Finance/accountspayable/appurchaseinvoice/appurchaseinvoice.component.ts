import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { MultiFileUploadDto } from '../../../models/sharedDto';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { DeleteConfirmDialogComponent } from '../../../sharedcomponent/delete-confirm-dialog';
import { FileUploadComponent } from '../../../sharedcomponent/fileupload.component';
import { PaginationService } from '../../../sharedcomponent/pagination.service';
import { ParentFinMgtComponent } from '../../../sharedcomponent/parentfinmgt.component';
import { ParentSystemSetupComponent } from '../../../sharedcomponent/parentsystemsetup.component';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { PurchaseinvoiceComponent } from '../../sharedpages/purchaseinvoiceview/purchaseinvoice.component';
import { AddupdatepurchaseinvoiceComponent } from '../sharedpages/addupdatepurchaseinvoice/addupdatepurchaseinvoice.component';
import { EditpurchaseinvoiceComponent } from '../sharedpages/editpurchaseinvoice/editpurchaseinvoice.component';
import { PurchaseinvoiceprintingComponent } from '../sharedpages/purchaseinvoiceprinting/purchaseinvoiceprinting.component';
import { ApSettlementmodeComponent } from '../sharedpages/settlementmode/settlementmode.component';
import { ApUserapprovalComponent } from '../sharedpages/userapproval/userapproval.component';


@Component({
  selector: 'app-appurchaseinvoice',
  templateUrl: './appurchaseinvoice.component.html',
  styles: [
  ],
  
})
export class AppurchaseinvoiceComponent extends ParentFinMgtComponent implements OnInit { //
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  //@ViewChild(MatPaginator) paginator: MatPaginator;
  //@ViewChild(MatSort) sort: MatSort;
  displayedColumns: string[] = ['creditNumber', 'invoiceDate', 'companyName', 'invoiceRefNumber', 'branchName', 'customerName', 'code', 'totalAmount', 'lpoContract', 'paymentTerms', 'taxIdNumber', 'invoiceDueDate', 'Actions'];
  data: MatTableDataSource<any>;
  totalItemsCount: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;

  IsSettlentOpened: boolean = false;
  IsApproved: boolean = false;
  statusId: string = '';
  approvalStatus: string = '';

  productList: Array<CustomSelectListItem> = [];
  productId: number = 0;

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public pageService: PaginationService, public dialog: MatDialog) {
    super(authService);
  }


  ngOnInit(): void {
    //this.authService.SetApiEndPoint(this.authService.GetSystemSetupApiEndPoint());
    //this.authService.SetApiEndPoint('my own end point');
    this.initialLoading();
    this.loadProducts();
  }

  refresh() {
    this.searchValue = '';
    this.productId = 0;
    this.sortingOrder = 'id desc';
    this.initialLoading();
  }

  loadProducts() {
    this.apiService.getall("product/getSelectProductList").subscribe(res => {
      if (res)
        this.productList = res;
    });
  }

  initialLoading() {
    this.loadList(0, this.pageService.pageCount, "", this.sortingOrder);
  }

  onSortOrder(sort: any) {
    // sort.direction.direction === 'asc' ? 'desc' : 'asc';
    // console.log(sort.active + ' ' + sort.direction);

    this.sortingOrder = sort.active + ' ' + sort.direction;
    this.totalItemsCount = 0;
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
  }


  onPageSwitch(event: PageEvent) {
    this.pageService.change(event);
    this.loadList(event.pageIndex, event.pageSize, this.searchValue, this.sortingOrder);
  }

  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined, approval: string = "") {
    this.isLoading = true;

    this.apiService.getPagination('generateApInvoice', this.utilService.getQueryString(page, pageCount, query, orderBy, this.approvalStatus, this.statusId, this.productId ?? 0)).subscribe(result => {
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

  applyFilter(searchVal: any, productId: any) {
    const search = searchVal;//.target.value as string;
    //if (search && search.length >= 3) {
    if (search || productId) {
      this.searchValue = search;
      this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
    }
  }

  loadApprovals(evt: any) {
    this.approvalStatus = evt.target.value;
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder, this.approvalStatus);
  }

  loadInvoices(evt: any) {
    const approvalList = document.getElementById('approvalList') as HTMLSelectElement;
    approvalList.value = "";

    this.setStatus(evt.target.value);
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder, '');
  }

  setStatus(id: string) {
    this.statusId = `${id}`;
  }

    
  private openDialogManage<T>(id: number = 0, dbops: DBOperation, modalTitle: string = '', modalBtnTitle: string = '', component: T, moduleFile: MultiFileUploadDto = { module: '00', action: '00act' }, width: number = 100) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, component, width);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).id = id;
    (dialogRef.componentInstance as any).moduleFile = moduleFile;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }

  private openNormalDialogConfig<T>(id: number = 0, dbops: DBOperation, modalTitle: string = '', modalBtnTitle: string = '', component: T, moduleFile: MultiFileUploadDto = { module: '00', action: '00act' }, width: number = 100) {
    let dialogRef = this.utilService.openNormalDialogConfig(this.dialog, component, width);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).id = id;
    (dialogRef.componentInstance as any).moduleFile = moduleFile;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true) {
        this.initialLoading();
      }
    });
  }

  private openDialogManageApproval<T>(id: number = 0, dbops: DBOperation, trantype: string = '', modalBtnTitle: string = '', component: T, moduleFile: MultiFileUploadDto = { module: '00', action: '00act' }, width: number = 100) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, component, width);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).trantype = trantype;
    (dialogRef.componentInstance as any).id = id;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).moduleFile = moduleFile;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }

  public create() {
    this.openDialogManage(0, DBOperation.create, this.translate.instant('Create_New_Sales_Invoice'), '', AddupdatepurchaseinvoiceComponent);
  }

  public edit(id: number) {
    this.openDialogManage(id, DBOperation.update, this.translate.instant('Create_New_Sales_Invoice'), '', EditpurchaseinvoiceComponent);
  }

  public view(id: number) {
    this.openNormalDialogConfig(id, DBOperation.create, '', '', PurchaseinvoiceComponent, { action: '', module: '' }, 100);
  }
  public approve(id: number, trantype: string) {

    //let ele = document.getElementById('edit_' + id) as HTMLElement;
    //ele.style.display = 'none';

    this.openDialogManageApproval(id, DBOperation.create, trantype, '', ApUserapprovalComponent, { action: '', module: '' }, 50);
  }

  public settlePayment(row: any) {
    // this.IsSettlentOpened = true;
    this.openDialogManageApproval(row.id, DBOperation.create, row.trantype, row.creditNumber, ApSettlementmodeComponent, { action: '', module: '' }, 50);
  }

  //uploadFile() {
  //  this.openDialogManage(0, DBOperation.update, this.translate.instant('Create_New_Sales_Invoice'), '', FileUploadComponent, { module: 'FIN', action: 'sales' });
  //}

  printInvoice() {
    this.openDialogManage(0, DBOperation.create, '', '', PurchaseinvoiceprintingComponent);
  }
}


