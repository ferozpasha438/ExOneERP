////import { HttpClient } from '@angular/common/http';
////import { ViewChild } from '@angular/core';
////import { Component, OnInit } from '@angular/core';
////import { FormBuilder, FormGroup, Validators } from '@angular/forms';
////import { Router } from '@angular/router';
////import { MatSlideToggleModule } from '@angular/material/slide-toggle';
////import { AuthorizeService } from '../../api-authorization/AuthorizeService';
////import { ApiService } from '../../services/api.service';
////import { NotificationService } from '../../services/notification.service';
////import { UtilityService } from '../../services/utility.service';
////import { ValidationService } from '../../sharedcomponent/ValidationService';
//// import { MatDialog } from '@angular/material/dialog';
////import { MatPaginator, PageEvent } from '@angular/material/paginator';
////import { MatSort } from '@angular/material/sort';
////import { MatTableDataSource } from '@angular/material/table';
//// import { DBOperation } from '../../services/utility.constants';
////import { PaginationService } from '../../sharedcomponent/pagination.service';
////import { ParentPurchaseMgtComponent } from '../../sharedcomponent/parentpurchasemgt.component';

import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { MultiFileUploadDto } from '../../models/sharedDto';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { DBOperation } from '../../services/utility.constants';
import { UtilityService } from '../../services/utility.service';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { ParentSystemSetupComponent } from '../../sharedcomponent/parentsystemsetup.component';
import { ValidationService } from '../../sharedcomponent/ValidationService';
//import { SalesinvoiceviewComponent } from '../sharedpages/salesinvoiceview/salesinvoiceview.component';
//import { AddupdatesalesinvoiceComponent } from '../sharedpages/addupdatesalesinvoice/addupdatesalesinvoice.component';
//import { EditArsalesinvoiceComponent } from '../sharedpages/editsalesinvoice/editsalesinvoice.component';
//import { SettlementmodeComponent } from '../sharedpages/settlementmode/settlementmode.component';
//import { UserapprovalComponent } from '../sharedpages/userapproval/userapproval.component';
import { ParentPurchaseMgtComponent } from '../../sharedcomponent/parentpurchasemgt.component';
import { AddupdatepurchaserequestComponent } from '../sharedpages/addupdatepurchaserequest/addupdatepurchaserequest.component';
import { DeleteConfirmDialogComponent } from '../../sharedcomponent/delete-confirm-dialog';
import { ApprovaldialogwindowsComponent } from '../approvaldialogwindows/approvaldialogwindows.component';
import { CustomSelectListItem } from '../../models/MenuItemListDto';
import { PoprintingpageComponent } from '../sharedpages/poprintingpage/poprintingpage.component';
import { compileClassMetadata } from '@angular/compiler';
import { FileUploadComponent } from '../../sharedcomponent/fileupload.component';
@Component({
  selector: 'app-purchaserequest',
  templateUrl: './purchaserequest.component.html',
  styleUrls: []
})
export class PurchaserequestComponent extends ParentPurchaseMgtComponent implements OnInit {

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  //@ViewChild(MatPaginator) paginator: MatPaginator;
  //@ViewChild(MatSort) sort: MatSort;
  displayedColumns: string[] = ['purchaseRequestNO', 'tranDate', 'invRefNumber', 'branchCode', 'vendName', 'vendCode', 'amount', 'paymentID', 'taxId', 'remarks', 'Actions']; /*'itemName',*/
  data: MatTableDataSource<any> | null;
  totalItemsCount: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  IsSettlentOpened: boolean = false;
  IsApproved: boolean = false;

  tranItemCode: string = '';
  ItemCodeList: Array<CustomSelectListItem> = [];

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public pageService: PaginationService, public dialog: MatDialog) {
    super(authService);
  }


  ngOnInit(): void {
    this.initialLoading();
    this.loadItemCode();
  }

  refresh() {
    this.searchValue = '';
    this.tranItemCode = '';
    this.sortingOrder = 'id desc';
    this.initialLoading();
  }


  loadItemCode() {
    this.apiService.getall('purchasereturn/GetItemCodeSelectList').subscribe(res => {
      if (res) {
        this.ItemCodeList = res;
      }
    })
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

  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined, approval: string = "") {
    this.isLoading = true;

    this.apiService.getPagination('PurchaseOrder', this.utilService.getQueryString(page, pageCount, query, orderBy, approval, '', 0, this.tranItemCode ?? '')).subscribe(result => {
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

  applyFilter(searchVal: any, tranItemCode: any) {
    const search = searchVal;//.target.value as string;
    //if (search && search.length >= 3) {
    if (search || tranItemCode) {
      this.searchValue = search;
      this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
    }
  }

  loadApprovals(evt: any) {
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder, evt.target.value);
  }

  private openDialogManage<T>(id: number = 0, dbops: DBOperation, modalTitle: string = '', modalBtnTitle: string = '', component: T, moduleFile: MultiFileUploadDto = { module: '00', action: '00act' }, width: number = 100) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, component, width);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;
    (dialogRef.componentInstance as any).moduleFile = moduleFile;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true) {
        this.initialLoading();
      }
    });
  }

  //private openDialogManage(id: number = 0, dbops: DBOperation, modalTitle: string = '', modalBtnTitle: string = '') {
  //  let dialogRef = this.utilService.openCrudDialog(this.dialog, AddupdatesalesinvoiceComponent);
  //  (dialogRef.componentInstance as any).dbops = dbops;
  //  (dialogRef.componentInstance as any).modalTitle = modalTitle;
  //  (dialogRef.componentInstance as any).id = id;

  //  dialogRef.afterClosed().subscribe(res => {
  //    if (res && res === true)
  //      this.initialLoading();
  //  });
  //}

  public create() {
    this.openDialogManage(0, DBOperation.create, this.translate.instant('Create_New_Purchase_Request'), '', AddupdatepurchaserequestComponent);
  }

  public edit(id: number) {
    this.openDialogManage(id, DBOperation.update, this.translate.instant('Create_New_Purchase_Request'), '', AddupdatepurchaserequestComponent);
  }

  public delete(id: number) {

    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete && id > 0) {
        this.apiService.delete('PurchaseOrder', id).subscribe(res => {
          this.refresh();
          this.utilService.OkMessage();
        },
        );
      }
    },
      error => this.utilService.ShowApiErrorMessage(error));
  }
  public view(id: number) {
    this.openDialogManage(id, DBOperation.update, this.translate.instant('Create_New_Purchase_Request'), '', AddupdatepurchaserequestComponent);
  }

  public printing(id: number) {
    this.openDialogManage(id, DBOperation.update, this.translate.instant('Create_New_Purchase_Request'), 'PR', PoprintingpageComponent);
  }

  public approve(id: number) {

    //let ele = document.getElementById('edit_' + id) as HTMLElement;
    //ele.style.display = 'none';


  }

  public settlePayment(id: number) {
    // this.IsSettlentOpened = true;

  }
  approvePurchaseRequest(project: any) {
    let serviceType = 'PR';
    let serviceCode = project.purchaseRequestNO;
    let branchCode = project.branchCode;
    this.openApprovalDialog(branchCode, serviceCode, DBOperation.create, 'Purchase_Request', 'Save', serviceType, ApprovaldialogwindowsComponent);

  }
  private openApprovalDialog(branchCode: string, serviceCode: string, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, serviceType: string, Component: any) {
    let dialogRef = this.openApprovalDialog1(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).serviceType = serviceType;
    (dialogRef.componentInstance as any).serviceCode = serviceCode;
    (dialogRef.componentInstance as any).branchCode = branchCode;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }


  openApprovalDialog1(dialog: MatDialog, component: any) {
    const dialogRef = dialog.open(component, {
      disableClose: true,
    });
    return dialogRef;
  }


  private openFileUploadDialogManage<T>(modalTitle: string = '', component: T, moduleFile: any, width: number = 80) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, component, width);
    (dialogRef.componentInstance as any).modalTitle = modalTitle;    
    (dialogRef.componentInstance as any).moduleFile = moduleFile;

    dialogRef.afterClosed().subscribe(res => {
    });
  }
  uploadFile(id: any) {
    this.openFileUploadDialogManage(this.translate.instant('Document_Upload'), FileUploadComponent, { module: 'PURMGT', action: 'PR', id: id, sourceId: id });
  }
}
