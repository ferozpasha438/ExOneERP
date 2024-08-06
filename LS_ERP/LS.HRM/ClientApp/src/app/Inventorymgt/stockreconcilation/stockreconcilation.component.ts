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

@Component({
  selector: 'app-InventoryStockReconcilation',
  templateUrl: './InventoryStockReconcilation.component.html',
  styleUrls: []
})
export class InventoryStockReconcilation extends ParentInventoryMgtComponent implements OnInit {

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  //@ViewChild(MatPaginator) paginator: MatPaginator;
  //@ViewChild(MatSort) sort: MatSort;
  /* displayedColumns: string[] = [];*/
  /*  displayedColumns: string[] = ['request', 'vendor', 'docnum', 'branch', 'amount', 'vat', 'reference', 'Actions'];*/
  displayedColumns: string[] = ['tranNumber', 'tranDate', 'tranDocNumber', 'tranReference', 'tranLocation', 'tranToLocation', 'tranTotalCost', 'Actions'];
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
    private notifyService: NotificationService, private validationService: ValidationService, public pageService: PaginationService, public dialog: MatDialog) {

    super(authService);

  }


  ngOnInit(): void {
    this.initialLoading();
    this.setForm();
    this.Users();
    this.FromLocation();
    this.ToLocation();
    this.Accounts();
    this.AdjAccounts();
    this.JVNumber();
    this.loadItemCode();
    this.loadbarCode();
    this.loadUnitCode();

    this.form.patchValue({
      trantype: "4"
    });

  }
  onSortOrder(sort: any) {
    this.totalItemsCount = 0;
    this.sortingOrder = sort.active + ' ' + sort.direction;
    this.loadUser(0, this.pageService.pageCount, "", this.sortingOrder);
  }

  onPageSwitch(event: PageEvent) {
    this.pageService.change(event);
    this.loadUser(event.pageIndex, event.pageSize, "", this.sortingOrder);
  }
  private loadUser(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
    //this.apiService.getPagination('', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
    //  this.totalItemsCount = 0;
    //  //this.forecasts = result.items;
    this.data = new MatTableDataSource(this.getData());
    //  this.data = new MatTableDataSource(result.items);
    this.totalItemsCount = 2;
    //  //this.data.data = this.forecasts;
    //  this.data.paginator = this.paginator;
    //  this.data.sort = this.sort;

    //}, error => console.error(error));
  }


  Users() {
    this.apiService.getall('InventoryStockReconcilation/GetStockReconcilationUserSelectList').subscribe(res => {
      if (res) {
        this.TranUserList = res;
      }
    })
  }
  FromLocation() {
    this.apiService.getall('InventoryStockReconcilation/GetStockReconcilationToLocationList').subscribe(res => {
      if (res) {
        this.tranLocationList = res;
      }
    })
  }
  ToLocation() {
    this.apiService.getall('InventoryStockReconcilation/GetStockReconcilationToLocationList').subscribe(res => {
      if (res) {
        this.tranToLocationList = res;
      }
    })
  }
  Accounts() {
    this.apiService.getall('InventoryStockReconcilation/GetStockReconcilationAccountSelectList').subscribe(res => {
      if (res) {
        this.tranInvAccountList = res;
      }
    })
  }
  AdjAccounts() {
    this.apiService.getall('InventoryStockReconcilation/GetStockReconcilationAccountSelectList').subscribe(res => {
      if (res) {
        this.tranInvAdjAccountList = res;
      }
    })
  }
  JVNumber() {
    this.apiService.getall('InventoryStockReconcilation/GetStockReconcilationJVNumberSelectList').subscribe(res => {
      if (res) {
        this.jVNumList = res;
      }
    })
  }
  loadItemCode() {
    this.apiService.getall('InventoryStockReconcilation/GetStockReconcilationItemCodeList').subscribe(res => {
      if (res) {
        this.ItemCodeList = res;
      }
    })
  }
  loadbarCode() {
    this.apiService.getall('InventoryStockReconcilation/GetStockReconcilationBarCodeSelectList').subscribe(res => {
      if (res) {
        this.BarcodeList = res;
      }
    })
  }
  loadUnitCode() {
    this.apiService.getall('InventoryStockReconcilation/GetStockReconcilationUOMSelectList').subscribe(res => {
      if (res) {
        this.UOMList = res;
      }
    })
  }
  loadUomdata(event: any) {
    let ItemUomCode = event.value;
    let ItemList = this.tranItemCode + '_' + ItemUomCode;
    if (ItemList) {

      this.apiService.getall(`InventoryStockReconcilation/StockReconcilationProductUomtPriceItem/${ItemList}`).subscribe(res => {
        //this.apiService.getall(`PurchaseOrder/ProductUomtPriceItem/${body}`).subscribe(res => {

        if (res) {
          this.tranUOMFactor = res.tranItemUomFactor;
          this.tranItemCost = res.itemAvgcost;
        }
        //this.branchList = res;
      });
    }
    else {
      this.unitType = '';
      this.description = '';
    }
  }
  getData(): Array<any> {
    let data: Array<any> = [
      //{ "request": "0110101",  "vendor": "satyam ", "docnum": "doc1234 ", "branch": "kukatpally ", "amount": "$1200 ", "vat": "$14 ",  "reference": "reference1", "id": 1 },
      //{ "request": "0110101",  "vendor": "satyam ", "docnum": "doc2345 ", "branch": "kukatpally ", "amount": "$1200 ", "vat": "$14 ",   "reference": "reference1", "id": 2 }
    ]
    return data;
  }
  setForm() {
    let MOBILE_PATTERN = /[0-9\+\-\ ]/;
    this.form = this.fb.group({

      /*itemList: this.fb.array([this.createAuthority()])//, Validators.required)*/
      'tranDate': ['', Validators.required],
      'tranUser': ['', Validators.required],
      'tranLocation': ['', Validators.required],
      'tranToLocation': ['', Validators.required],
      'tranDocNumber': ['', Validators.required],
      'tranReference': ['', Validators.required],
      'trantype': ['', Validators.required],
      'tranRemarks': ['', Validators.required],
      'tranInvAccount': ['', Validators.required],
      'tranInvAdjAccount': ['', Validators.required],
      'jVNum': ['', Validators.required],



      //authList: this.fb.array([this.createAuthority()]),



    });

  }
  create() {
    this.setForm();
    this.refresh();
  }


  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined, approval: string = "") {
    this.isLoading = true;

    this.apiService.getPagination('InventoryStockReconcilation', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
      this.totalItemsCount = 0;
      this.data = new MatTableDataSource(result.items);

      this.totalItemsCount = result.totalCount;

      this.data.paginator = this.paginator;
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



  purchaseOrdersubmit() {
    if (this.id > 0)
      this.form.value['id'] = this.id;
    this.apiService.post('InventoryStockReconcilation', this.form.value)
      .subscribe(res => {
        debugger;
        /*this.itemcode = res['itemcode'];*/
        if (res) {
          //let output1 = res;
          //Object.values(res);
          //this.itemcode = Object.values(res)[0];
          this.utilService.OkMessage();
        }

        //this.form.patchValue({
        //  ItemInventoryCodes: this.itemcode,
        //  ItemUOMCodes: this.itemcode,
        //  ItemBarcodeCodes: this.itemcode,
        //  ItemNotesCodes: this.itemcode,
        //  ItemHistoryCodes: this.itemcode,
        //  itemCode: this.itemcode
        //});


      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });
  }
  public delete(id: number) {
    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete && id > 0) {
        this.apiService.delete('InventoryStockReconcilation', id).subscribe(res => {
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

  loadProductdata(event: any) {
    let prodictId = event.value, productName = event.text;
    if (prodictId) {

      this.apiService.getall(`product/productUnitPriceItem/${prodictId}`).subscribe(res => {
        if (res) {
          this.product = res.nameEN;
          this.unitType = res.unitTypeEN;
          this.description = res.description;
          this.price = res.unitPrice;
        }
        //this.branchList = res;
      });
    }
    else {
      this.unitType = '';
      this.description = '';
    }
  }


  addInvoice() {
    if (this.tranTotCost > 0) {
      if (this.editsequence > 0) {
        this.removeInvoiceList(this.editsequence);

        this.editsequence = 0;
      }
      this.listOfInvoices.push({
        sequence: this.getSequence(),

        tranNumber: "0",
        tranItemCode: this.tranItemCode,
        tranBarcode: this.tranBarcode,
        tranItemName: this.tranItemName,
        tranItemName2: this.tranItemName2,
        tranItemQty: this.tranItemQty,
        tranItemUnit: this.tranItemUnit,
        tranUOMFactor: this.tranUOMFactor,
        tranItemCost: this.tranItemCost,
        tranTotCost: this.tranTotCost,
        itemAttribute1: this.itemAttribute1,
        itemAttribute2: this.itemAttribute2,
        remarks: this.remarks,
        iNVAcc: this.iNVAcc,
        iNVADJAcc: this.iNVADJAcc
      });

      this.setGrandTotal();
      this.setToDefault();
    }
  }

  getSequence(): number { return this.sequence += this.sequence + 1 };

  showCanShowNotes() { this.canShowNotes = !this.canShowNotes }
  showCanShowRemarks() { this.canShowRemarks = !this.canShowRemarks }

  deleteInvoiceItem(item: any) {
    this.removeInvoiceList(item.sequence);
    this.setGrandTotal();
  }

  removeInvoiceList(sequence: number) {
    let index: number = this.listOfInvoices.findIndex(a => a.sequence === sequence);
    this.listOfInvoices.splice(index, 1);
  }

  editInvoiceItem(item: any) {


    //this.tranItemCode = item.tranItemCode,
    //  this.tranItemName = item.tranItemName,
    //  this.tranItemName2 = item.tranItemName2,
    //  this.tranItemQty = item.tranItemQty,
    //  this.tranItemUnitCode = item.tranItemUnitCode,
    //  this.tranUOMFactor = item.tranUOMFactor,
    //  this.tranItemCost = item.tranItemCost,
    //  this.tranTotCost = item.tranTotCost,
    //  this.discPer = item.discPer,
    //  this.discAmt = item.discAmt,
    //  this.itemTax = item.itemTax,
    //  this.itemTaxPer = item.itemTaxPer,
    //  this.taxAmount = item.taxAmount,
    //  this.itemTracking = item.itemTracking;


    this.tranItemCode = item.tranItemCode,
      this.tranBarcode = item.tranBarcode,
      this.tranItemName = item.tranItemName,
      this.tranItemName2 = item.tranItemName2,
      this.tranItemQty = item.tranItemQty,
      this.tranItemUnit = item.tranItemUnit,
      this.tranUOMFactor = item.tranUOMFactor,
      this.tranItemCost = item.tranItemCost,
      this.tranTotCost = item.tranTotCost,
      this.itemAttribute1 = item.itemAttribute1,
      this.itemAttribute2 = item.itemAttribute2,
      this.remarks = item.remarks,
      this.iNVAcc = item.iNVAcc,
      this.iNVADJAcc = item.iNVADJAcc;
  }

  setGrandTotal() {
    this.grandTotal = 0;
    //this.grandVatTotal = 0;
    //this.grandInvoiceTotal = 0;

    this.listOfInvoices.forEach(inv => {
      this.grandTotal += inv.tranTotCost;
      //this.grandVatTotal += inv.taxAmount;
      //this.grandInvoiceTotal += (inv.tranTotCost - inv.taxAmount);
    });


    this.grandTotalStr = this.grandTotal.toString();
    //this.grandVatTotalStr = this.grandVatTotal.toString();
    //this.grandInvoiceTotalStr = this.grandInvoiceTotal.toString();
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

  calculate() {



    var qty = this.tranItemQty;
    var price = this.tranItemCost;
    var amount = parseFloat((qty * price).toString())
    this.tranTotCost = parseFloat(amount.toString());
  }

  submit() {
    if (this.id > 0)
      this.form.value['id'] = this.id;
    this.form.value['itemList'] = this.listOfInvoices;
    this.form.value['tranNumber'] = "0";
    this.apiService.post('InventoryStockReconcilation/StockReconcilationCreateRequest', this.form.value)
      .subscribe(res => {
        debugger;
        this.reset();

        this.utilService.OkMessage();
        this.closeModel();
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });
  }



  reset() {
    this.listOfInvoices = [];
    this.grandTotalStr = "0";
    this.form.reset();
  }
  public edit(id: number) {

    this.apiService.get('InventoryStockReconcilation', id).subscribe(res => {
      if (res) {
        this.isReadOnly = true;
        this.form.patchValue(res);
        this.form.patchValue({ 'jVNum': `${res['jvNum']}` });
        this.form.patchValue({ 'tranDate': `${res['tranDate'].split('T')[0]}` });

        this.form.patchValue({
          trantype: "4"
        });
        this.form.value['itemList'] = this.listOfInvoices;
        this.listOfInvoices = [];
        //this.form.patchValue({ 'poNotes': `${res['poNotes']}` });
        let listOfInvoices = res['itemList'] as Array<any>;
        listOfInvoices.forEach(item => {
          this.listOfInvoices.push({
            sequence: this.getSequence(),
            tranNumber: "0",
            tranItemCode: item.tranItemCode,
            tranBarcode: item.tranBarcode,
            tranItemName: item.tranItemName,
            tranItemName2: item.tranItemName2,
            tranItemQty: item.tranItemQty,
            tranItemUnit: item.tranItemUnit,
            tranUOMFactor: item.tranUOMFactor,
            tranItemCost: item.tranItemCost,
            tranTotCost: item.tranTotCost,
            itemAttribute1: item.itemAttribute1,
            itemAttribute2: item.itemAttribute2,
            remarks: item.remarks,
            iNVAcc: item.invAcc,
            iNVADJAcc: item.invAdjAcc


          });
        });
        this.setGrandTotal();
        this.id = res.id;
      }
    })
  }
  loadApprovals(evt: any) {
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder, evt.target.value);
  }
  public view(id: number) {
    this.openDialogManage(id, DBOperation.create, '', '', PurchaseinvoiceComponent, { action: '', module: '' }, 100);
  }
  public approve(id: number) {

    this.openDialogManage(id, DBOperation.create, '', '', InventoryuserapprovalComponent, { action: '', module: '' }, 50);
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
