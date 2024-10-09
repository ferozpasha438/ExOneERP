import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';
import { ValidationService } from '../../sharedcomponent/ValidationService';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DBOperation } from '../../services/utility.constants';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { ParentPurchaseMgtComponent } from '../../sharedcomponent/parentpurchasemgt.component';
import { CustomSelectListItem } from '../../models/MenuItemListDto';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { DeleteConfirmDialogComponent } from '../../sharedcomponent/delete-confirm-dialog';
/*import { PurchaseinvoiceComponent } from '../sharedpages/purchaseinvoiceview/purchaseinvoice.component';*/
import { PurchaseinvoiceComponent } from '../../Finance/sharedpages/purchaseinvoiceview/purchaseinvoice.component'
import { MultiFileUploadDto } from '../../models/sharedDto';
/*import { MultiFileUploadDto } from '../../Finance/models/sharedDto';*/
/*import { ParentSystemSetupComponent } from '../../sharedcomponent/parentsystemsetup.component';*/

import { InventoryuserapprovalComponent } from '../sharedpages/inventoryuserapproval/inventoryuserapproval.component';
import { ParentInventoryMgtComponent } from '../../sharedcomponent/parentinventorymgt.component';
import { ApprovaldialogwindowsComponent } from '../../Purchasemgt/approvaldialogwindows/approvaldialogwindows.component';
import { AddupdateissuesComponent } from '../sharedpages/addupdateissues/addupdateissues.component';
import { TranslateService } from '@ngx-translate/core';
import { AddupdateadjacementComponent } from '../sharedpages/addupdateadjacement/addupdateadjacement.component';
@Component({
  selector: 'app-adjustments',
  templateUrl: './adjustments.component.html',
  styleUrls: []
})
export class AdjustmentsComponent extends ParentInventoryMgtComponent implements OnInit {

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  //@ViewChild(MatPaginator) paginator: MatPaginator;
  //@ViewChild(MatSort) sort: MatSort;
  /* displayedColumns: string[] = [];*/
  /*  displayedColumns: string[] = ['request', 'vendor', 'docnum', 'branch', 'amount', 'vat', 'reference', 'Actions'];*/
  displayedColumns: string[] = ['tranNumber', 'tranDate', 'tranDocNumber', 'tranReference', 'tranLocation', 'Actions'];
  data: MatTableDataSource<any> | null;
  totalItemsCount: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';




  productId: number = 0;
  quantity: number = 0;
  price: number = 0;
  vat: number = 0;
  vatAmount: number = 0;
  total: number = 0;
  unitType: string = '';
  description: string = '';
  product: string = '';

  grandTotal: number = 0;
  grandVatTotal: number = 0;
  grandInvoiceTotal: number = 0;
  grandTotalStr: string = '';
  grandVatTotalStr: string = '';
  grandInvoiceTotalStr: string = '';


  sequence: number = 1;
  editsequence: number = 0;
  listOfInvoices: Array<any> = [];

  canShowNotes: boolean = false;
  canShowRemarks: boolean = false;
  invoiceItemObject: any;



  //data: MatTableDataSource<any> | null;
  //totalItemsCount: number;
  //sortingOrder: string = 'id';
  form: FormGroup;
  //searchValue: string = '';
  isLoading: boolean = false;
  isReadOnly: boolean = false;
  id: number = 0;

  TranUserList: Array<CustomSelectListItem> = [];
  tranLocationList: Array<CustomSelectListItem> = [];
  tranToLocationList: Array<CustomSelectListItem> = [];
  tranInvAccountList: Array<CustomSelectListItem> = [];
  tranInvAdjAccountList: Array<CustomSelectListItem> = [];
  jVNumList: Array<CustomSelectListItem> = [];

  ItemCodeList: Array<CustomSelectListItem> = [];
  BarcodeList: Array<CustomSelectListItem> = [];
  UOMList: Array<CustomSelectListItem> = [];

  ItemTrackingList = [
    { value: 1, text: "Yes" },
    { value: 0, text: "NO" }

  ]
  //ItemTranstypeList = [
  //  { value: 1, text: "Purchase Request" },
  //  { value: 2, text: "Purchase Order" }

  //]


  tranItemCode: number = 0;
  tranBarcode: string = '';
  tranItemName: string = '';
  tranItemName2: string = '';
  tranItemQty: number = 0;
  tranItemUnit: string = '';
  tranUOMFactor: string = '';
  tranItemCost: number = 0;
  tranTotCost: number = 0;
  itemAttribute1: string = '';
  itemAttribute2: string = '';
  remarks: string = '';
  iNVAcc: string = '';
  iNVADJAcc: string = '';


  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService,
    private notifyService: NotificationService, private validationService: ValidationService, public pageService: PaginationService, public dialog: MatDialog, private translate: TranslateService) {

    super(authService);

  }


  ngOnInit(): void {
    this.initialLoading();
    //this.setForm();
    //this.Users();
    //this.FromLocation();
    //this.ToLocation();
    //this.Accounts();
    //this.AdjAccounts();
    //this.JVNumber();
    //this.loadItemCode();
    //this.loadbarCode();
    //this.loadUnitCode();

    this.form.patchValue({
      trantype: "3"
    });

  }
  onSortOrder(sort: any) {
    this.totalItemsCount = 0;
    this.sortingOrder = sort.active + ' ' + sort.direction;
    this.loadList(0, this.pageService.pageCount, "", this.sortingOrder);
  }


  getData(): Array<any> {
    let data: Array<any> = [
      //{ "request": "0110101",  "vendor": "satyam ", "docnum": "doc1234 ", "branch": "kukatpally ", "amount": "$1200 ", "vat": "$14 ",  "reference": "reference1", "id": 1 },
      //{ "request": "0110101",  "vendor": "satyam ", "docnum": "doc2345 ", "branch": "kukatpally ", "amount": "$1200 ", "vat": "$14 ",   "reference": "reference1", "id": 2 }
    ]
    return data;
  }

  public create() {
    this.openDialogManage(0, DBOperation.create, this.translate.instant('Create_New_Issues'), '', AddupdateadjacementComponent);
  }

  public edit(id: number) {
    this.openDialogManage(id, DBOperation.update, this.translate.instant('Create_New_Adjustments'), '', AddupdateadjacementComponent);
  }
  //public delete(id: number) {

  //  const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
  //  dialogRef.afterClosed().subscribe(canDelete => {
  //    if (canDelete && id > 0) {
  //      this.apiService.delete('PurchaseOrder', id).subscribe(res => {
  //        this.refresh();
  //        this.utilService.OkMessage();
  //      },
  //      );
  //    }
  //  },
  //    error => this.utilService.ShowApiErrorMessage(error));
  //}
  public view(id: number) {
    this.openDialogManage(id, DBOperation.update, this.translate.instant('Create_New_Adjustments'), '', AddupdateadjacementComponent);//AddupdateissuesComponent
  }

  onPageSwitch(event: PageEvent) {
    this.pageService.change(event);
    this.loadList(event.pageIndex, event.pageSize, this.searchValue, this.sortingOrder);
  }

  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined, approval: string = "") {
    this.isLoading = true;

    this.apiService.getPagination('InventoryAdjustments', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
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



  //purchaseOrdersubmit() {
  //  if (this.id > 0)
  //    this.form.value['id'] = this.id;
  //  this.apiService.post('InventoryAdjustments', this.form.value)
  //    .subscribe(res => {
  //      debugger;
  //      /*this.itemcode = res['itemcode'];*/
  //      if (res) {
  //        //let output1 = res;
  //        //Object.values(res);
  //        //this.itemcode = Object.values(res)[0];
  //        this.utilService.OkMessage();
  //      }

  //      //this.form.patchValue({
  //      //  ItemInventoryCodes: this.itemcode,
  //      //  ItemUOMCodes: this.itemcode,
  //      //  ItemBarcodeCodes: this.itemcode,
  //      //  ItemNotesCodes: this.itemcode,
  //      //  ItemHistoryCodes: this.itemcode,
  //      //  itemCode: this.itemcode
  //      //});


  //    },
  //      error => {
  //        console.error(error);
  //        this.utilService.ShowApiErrorMessage(error);
  //      });
  //}
  public delete(id: number) {
    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete && id > 0) {
        this.apiService.delete('InventoryAdjustments', id).subscribe(res => {
          this.refresh();
          this.utilService.OkMessage();
        },
        );
      }
    },
      error => this.utilService.ShowApiErrorMessage(error));
  }
  initialLoading() {
    this.loadList(0, this.pageService.pageCount, "", this.sortingOrder);
  }
  refresh() {
    this.searchValue = '';
    this.initialLoading();
  }



  Close() {
    this.refresh();
  }



  customerChange(event: any) {
    let custId = event.value, custName = event.text;
    this.apiService.getall(`customer/getCustomerById/${custId}`).subscribe(res => {
      if (res) {
        this.form.controls['taxIdNumber'].setValue(res['vatNumber'])
      }
    });
  }


  resetProduct() {
    this.product = '';
    this.unitType = '';
    this.description = '';
    this.price = 0;
    this.total = 0;
    this.quantity = 0;
  }


  closeModel() {
    this.refresh();
  }


  setToDefault() {


    this.tranItemCode = 0;
    this.tranBarcode = '';
    this.tranItemName = '';
    this.tranItemName2 = '';
    this.tranItemQty = 0;
    this.tranItemUnit = '';
    this.tranUOMFactor = '';
    this.tranItemCost = 0;
    this.tranTotCost = 0;
    this.itemAttribute1 = '';
    this.itemAttribute2 = '';
    this.remarks = '';
    this.iNVAcc = '';
    this.iNVADJAcc = '';
  }


  reset() {
    this.listOfInvoices = [];
    this.grandTotalStr = "0";
    this.form.reset();
  }

  loadApprovals(evt: any) {
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder, evt.target.value);
  }
  //public view(id: number) {
  //  this.openDialogManage(id, DBOperation.create, '', '', PurchaseinvoiceComponent, { action: '', module: '' }, 100);
  //}
  public approve(id: number) {

    this.openDialogManage(id, DBOperation.create, '', '', InventoryuserapprovalComponent, { action: '', module: '' }, 50);
  }
  approvePurchaseOrder(project: any) {
    //let serviceType = 'RP';
    let serviceType = 'ADJS';
    let serviceCode = project.tranNumber;
    let branchCode = project.tranBranch;
    this.openApprovalDialog(branchCode, serviceCode, DBOperation.create, 'Issues', 'Save', serviceType, ApprovaldialogwindowsComponent);

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

  public Issuessettelment(id: number) {
    if (id > 0) {
      const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
      dialogRef.afterClosed().subscribe(canTakeAction => {
        if (canTakeAction) {
          this.apiService.getall(`InventoryAdjustments/AdjustmentSettelementList/${id}`).subscribe(res => {
            if (res) {
              this.utilService.OkMessage();
              this.refresh();
            }
          });
        }
      })
    }
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



}

