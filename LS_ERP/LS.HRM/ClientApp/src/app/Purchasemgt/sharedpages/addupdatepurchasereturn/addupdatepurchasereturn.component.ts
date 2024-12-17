import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef } from '@angular/core';
import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DBOperation } from '../../../services/utility.constants';
import { PaginationService } from '../../../sharedcomponent/pagination.service';
import { ParentPurchaseMgtComponent } from '../../../sharedcomponent/parentpurchasemgt.component';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { DeleteConfirmDialogComponent } from '../../../sharedcomponent/delete-confirm-dialog';
import { ApprovaldialogwindowsComponent } from '../../approvaldialogwindows/approvaldialogwindows.component';
import { TranslateService } from '@ngx-translate/core';
import { AddupdatereturnexpairybatchComponent } from '../addupdatereturnexpairybatch/addupdatereturnexpairybatch.component';
import { MultiFileUploadDto } from '../../../models/sharedDto';

@Component({
  selector: 'app-addupdatepurchasereturn',
  templateUrl: './addupdatepurchasereturn.component.html',
  styleUrls: []
})
export class AddupdatepurchasereturnComponent implements OnInit  {
 // @ViewChild(AddupdatereturnexpairybatchComponent) expBatchComponent!: AddupdatereturnexpairybatchComponent; // Reference to the second component

  //@ViewChild(Addupdatereturnexpairybatch) expBatchComponent!: Addupdatereturnexpairybatch; // Reference to the second component
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort!: MatSort;
  //@ViewChild(MatPaginator) paginator: MatPaginator;
  //@ViewChild(MatSort) sort: MatSort;
  /* displayedColumns: string[] = [];*/
  /*  displayedColumns: string[] = ['request', 'vendor', 'docnum', 'branch', 'amount', 'vat', 'reference', 'Actions'];*/
  displayedColumns: string[] = ['tranNumber', 'tranDate', 'invRefNumber', 'branchCode', 'vendCode', 'paymentID', 'taxId', 'Actions'];
  data!: MatTableDataSource<any> | null;
  totalItemsCount!: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';


  customerList: Array<CustomSelectListItem> = [];
  companyList: Array<CustomSelectListItem> = [];
  paymentTermsList: Array<CustomSelectListItem> = [];
  branchList: Array<CustomSelectListItem> = [];
  productList: Array<CustomSelectListItem> = [];
  vatList: Array<CustomSelectListItem> = [];

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
  DiscPerc: number = 0;
  trantypeValue: string = '3';
  oldBalQty: number = 0;

  //data: MatTableDataSource<any> | null;
  //totalItemsCount: number;
  //sortingOrder: string = 'id';
  form !: FormGroup;
  //searchValue: string = '';
  isLoading: boolean = false;
  isReadOnly: boolean = false;
  id: number = 0;
  ShipmentCodeList: Array<CustomSelectListItem> = [];
  CurrencyCodeList: Array<CustomSelectListItem> = [];
  TaxCodeList: Array<CustomSelectListItem> = [];
  VenCodeList: Array<CustomSelectListItem> = [];
  compCodeList: Array<CustomSelectListItem> = [];
  BranchCodeList: Array<CustomSelectListItem> = [];
  VenNameList: Array<CustomSelectListItem> = [];
  PaymentTermList: Array<CustomSelectListItem> = [];


  ItemCodeList: Array<any> = [];
  ItemNameList: Array<CustomSelectListItem> = [];
  UOMList: Array<CustomSelectListItem> = [];
  PurchaseRequestList: Array<CustomSelectListItem> = [];
  warehouseList: Array<CustomSelectListItem> = [];
  ItemTrackingList = [
    { value: 1, text: "Yes" },
    { value: 0, text: "NO" }

  ]
  //ItemTranstypeList = [
  //  { value: 1, text: "Purchase Request" },
  //  { value: 2, text: "Purchase Order" }

  //]


  tranItemCode: number = 0;
  tranItemName: string = '';
  tranItemName2: string = '';
  tranItemQty: number = 0;
  tranItemUnitCode: string = '';
  tranUOMFactor: string = '';
  tranItemCost: number = 0;
  tranTotCost: number = 0;
  discPer: string = '0';
  discAmt: number = 0;
  itemTax: string = '0';
  itemTaxPer: string = '0';
  taxAmount: number = 0;
  receivedQty: number = 0;
  returnedQty: number = 0;
  balQty: number = 0;
  preBalQty: number = 0;

  isVatIncluded: boolean = false;
  isQuantityLoading: boolean = false;

  isExpiryButtonEnabled: boolean = false;

  itemTracking: number = 0;
  serExpTracking: string = '';
  serExpTracking1: string = '';
  buttonName: string = '';
  showButton: boolean=false;
  constructor(private fb: FormBuilder, private cdr: ChangeDetectorRef, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdatepurchasereturnComponent>,
    private notifyService: NotificationService, private validationService: ValidationService, public pageService: PaginationService, public dialog: MatDialog, private translate: TranslateService) {



  }


  ngOnInit(): void {
    // this.initialLoading();
    this.setForm();
    this.loadTaxCode();
    this.loadVenCode();
    this.loadCompCode();
    this.loadBranchCode();
    this.loadVenName();
    this.loadPaymentTerm();
    this.loadItemCode();
    this.loadItemName();
    this.loadUnitCode();

    this.loadCurrencyCode();
    this.loadShipmentCode();
    this.loadPurchaseRequest();
    this.loadWarehouses();
    this.form.patchValue({
      trantype: "3"
    });
    if (this.id > 0) {
      this.edit();
      //  this.view();

    }

    this.isExpiryButtonEnabled = false;
  }

  //ngAfterViewInit(): void {
  //  console.log("ngAfterViewInit called"); // Debug to check if ngAfterViewInit is executed
  //  if (this.expBatchComponent) {
  //    console.log("expBatchComponent is available"); // Check if the child component is available
  //    this.expBatchComponent.quantityUpdated.subscribe((qty: number) => {
  //      console.log("Received quantity from child component:", qty);
  //      this.tranItemQty = qty;
  //      this.cdr.detectChanges(); // Manually trigger change detection
  //    });
  //  } else {
  //    console.warn("expBatchComponent is not available in ngAfterViewInit");
  //  }
  //}
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

  loadCompCode() {
    this.apiService.getall('purchasereturn/GetCompSelectList').subscribe(res => {
      if (res) {
        this.compCodeList = res;
      }
    })
  }
  loadWarehouses() {
    this.apiService.getall(`warehouse/getSelectWarehouseList`).subscribe(res => {
      if (res) {
        this.warehouseList = res;
      }
    });
  }
  loadBranchCode() {
    this.apiService.getall('purchasereturn/GetBranchSelectList').subscribe(res => {
      if (res) {
        this.BranchCodeList = res;
      }
    })
  }
  loadTaxCode() {
    this.apiService.getall('purchasereturn/GetTaxSelectList').subscribe(res => {
      if (res) {
        this.TaxCodeList = res;
      }
    })
  }
  loadVenCode() {
    this.apiService.getall('purchasereturn/GetVendorCodeSelectList').subscribe(res => {
      if (res) {
        this.VenCodeList = res;
      }
    });

    this.apiService.getall('PurchaseOrder/GetGRNSelectList').subscribe(res => {
      if (res) {
        this.PurchaseRequestList = res;
      }
    });
  }
  loadVenName() {
    this.apiService.getall('purchasereturn/GetVendorNameSelectList').subscribe(res => {
      if (res) {
        this.VenNameList = res;
      }
    })
  }
  loadPaymentTerm() {
    this.apiService.getall('purchasereturn/GetPaymentTermSelectList').subscribe(res => {
      if (res) {
        this.PaymentTermList = res;
      }
    })
  }
  loadItemCode() {
    this.apiService.getall('purchasereturn/GetItemCodeSelectList').subscribe(res => {
      if (res) {
        this.ItemCodeList = res as Array<any>;
        this.ItemCodeList.forEach(e => {
          e.text = e.textTwo + " (" + e.value + ")";

        });
      }
    })
  }
  loadItemName() {
    this.apiService.getall('purchasereturn/GetItemNameSelectList').subscribe(res => {
      if (res) {
        this.ItemNameList = res;
      }
    })
  }
  loadUnitCode() {
    this.apiService.getall('purchasereturn/GetUOMSelectList').subscribe(res => {
      if (res) {
        this.UOMList = res;
      }
    })
  }
  loadCurrencyCode() {
    this.apiService.getall('purchasereturn/GetCurrencySelectList').subscribe(res => {
      if (res) {
        this.CurrencyCodeList = res;
      }
    })
  }
  loadShipmentCode() {
    this.apiService.getall('purchasereturn/GetShipmentSelectList').subscribe(res => {
      if (res) {
        this.ShipmentCodeList = res;
      }
    })
  }
  loadUomdata(event: any) {
    let ItemUomCode = event.value;
    let ItemList = this.tranItemCode + '_' + ItemUomCode;
    if (ItemList) {

      this.apiService.getall(`purchasereturn/ProductUomtPriceItem/${ItemList}`).subscribe(res => {
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
  loadVendata(event: any) {

    let Vencode = event.value;
    if (Vencode) {

      this.apiService.getall(`PurchaseOrder/ProductVenPriceItem/${Vencode}`).subscribe(res => {
        if (res) {
          this.form.patchValue({ 'vendCode': `${res['vendName']}` });
          this.form.patchValue({ 'paymentID': `${res['poTermsCode']}` });
        }
      });
    }

  }
  loadTaxdata(event: any) {

    let ItemCode = event.value;
    if (ItemCode) {

      this.apiService.getall(`PurchaseOrder/ProductTaxPrice/${ItemCode}`).subscribe(res => {


        if (res) {
          // this.itemTaxPer = res[0].itemTaxperc;
          this.tranItemName = res[0].shortName;
          this.serExpTracking = res[0].serExpTracking;
          if (this.serExpTracking === 'EXP') {
            this.isExpiryButtonEnabled = true;
            this.buttonName = this.serExpTracking;
            this.showButton = true;  // Show the button
            this.isReadOnly = true;
          } else if (this.serExpTracking === 'SRL') {
            this.isExpiryButtonEnabled = false; // Button will be disabled (not needed if we are hiding it)
            this.buttonName = this.serExpTracking;
            this.showButton = false; // Hide the button
            this.isReadOnly = false;
          } else {
            this.isExpiryButtonEnabled = false;
            this.buttonName = this.serExpTracking;
            this.showButton = false; // Hide the button
            this.isReadOnly = false;
          }
        }
        this.loadUomList(ItemCode);
      });
    }

  }
  loadUomList(event: any) {
    let ItemUomCode = event;
    if (ItemUomCode) {

      this.apiService.getall(`PurchaseOrder/GetUOMItemList/${ItemUomCode}`).subscribe(res => {
        //this.apiService.getall(`PurchaseOrder/ProductUomtPriceItem/${body}`).subscribe(res => {

        if (res) {
          this.UOMList = res;
        }
        //this.branchList = res;
      });
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
      //'': ['', Validators.required],
      //  'purchaseRequestNO': ['', Validators.required],
      /*itemList: this.fb.array([this.createAuthority()])//, Validators.required)*/
      'compCode': ['', Validators.required],
      'branchCode': ['', Validators.required],
      'tranDate': ['', Validators.required],
      'deliveryDate': ['', Validators.required],
      'invRefNumber': ['', Validators.required],
      'venCatCode': ['', Validators.required],
      'docNumber': ['', Validators.required],
      'taxId': ['', Validators.required],
      'remarks': [''],
      'poNotes': [''],
      'trantype': [this.trantypeValue, Validators.required],
      'vendCode': ['', Validators.required],
      'paymentID': ['', Validators.required],
      'taxInclusive': ['', Validators.required],
      'tranCurrencyCode': ['', Validators.required],
      'tranShipMode': [''],
      'shipmentMode': [''],
      'whCode': ['', Validators.required],


      //authList: this.fb.array([this.createAuthority()]),



    });

  }
  loadPurchaseRequest() {
    //this.apiService.getall('purchasereturn/GetPrSelectList').subscribe(res => {
    //  if (res) {
    //    this.PurchaseRequestList = res;
    //  }
    //})
  }

  checkReturnedQty() {
    const tranItemQty1 = parseInt(this.preBalQty.toString());
    const receivedQty1 = parseInt(this.receivedQty.toString());
    const returnedQty1 = parseInt(this.returnedQty.toString());
    const balQty1 = parseInt(this.balQty.toString());

    if (this.oldBalQty == 0)
      this.oldBalQty = balQty1;

    if (returnedQty1 > (tranItemQty1 - receivedQty1)) {
      this.returnedQty = 0;
      this.balQty = this.preBalQty;
    }
    else
      this.balQty = this.oldBalQty - this.returnedQty;
    //console.log(this.returnedQty, this.tranItemQty);
  }

  loadPRdata(event: any) {
    let PRValue = event.value;
    this.apiService.getall(`purchasereturn/GetPRList/${PRValue}`).subscribe(res => {
      if (res) {
        this.isReadOnly = true;
        this.form.patchValue(res);
        this.form.patchValue({ 'tranDate': `${res['tranDate'].split('T')[0]}` });
        if (res['deliveryDate'])
          this.form.patchValue({ 'deliveryDate': `${res['deliveryDate'].split('T')[0]}` });

        this.form.patchValue({ 'compCode': `${res['compCode']}` });
        this.form.patchValue({ 'tranCurrencyCode': `${res['tranCurrencyCode']}` });
        this.form.patchValue({ 'poNotes': `${res['poNotes']}` });
        /*this.form.patchValue({ 'Id': `${res['Id']}` });*/
        this.id = res.id;
        this.form.patchValue({
          trantype: "2"
        });
        let listOfInvoices = res['itemList'] as Array<any>;
        listOfInvoices.forEach(item => {
          this.listOfInvoices.push({
            sequence: this.getSequence(),
            tranNumber: "0", tranItemCode: item.tranItemCode, tranItemName: item.tranItemName, tranItemName2: item.tranItemName2, tranItemQty: item.tranItemQty, tranItemUnitCode: item.tranItemUnitCode, tranUOMFactor: item.tranUOMFactor,
            tranItemCost: item.tranItemCost, tranTotCost: item.tranTotCost, discPer: item.discPer, discAmt: item.discAmt, itemTax: item.itemTax, itemTaxPer: item.itemTaxPer, taxAmount: item.taxAmount, itemTracking: item.itemTracking,
            receivedQty: item.receivedQty, returnedQty: item.returnedQty, balQty: item.balQty, preBalQty: item.preBalQty
          });
        });
        this.setGrandTotal();
      }
    })
  }

  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined, approval: string = "") {
    this.isLoading = true;

    this.apiService.getPagination('purchasereturn', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
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

  setTaxValue(evt: any) {
    const vatIncl = evt.target.value;
    if (vatIncl) {
      if (vatIncl == '0') {
        this.isVatIncluded = false;
      }
      else
        this.isVatIncluded = true;

      this.itemTaxPer = '0';
      this.taxAmount = 0;
      this.form.controls['taxId'].setValue('');

    }
  }

  loadItemTax(evt: any) {
    if (this.isVatIncluded) {
      const vatIncl = evt.text;
      this.itemTaxPer = vatIncl;
    }
  }

  //public edit(id: number) {
  //  /*this.openDialogManage(id, DBOperation.update, 'Updating Warehouse', 'Update');*/
  //  this.apiService.get('PurchaseOrder', id).subscribe(res => {
  //    if (res) {
  //      this.isReadOnly = true;
  //      /* this.loadSubCategoryCode();*/
  //      this.form.patchValue(res);
  //      //this.authList.clear();
  //      //let authList = res['authList'] as Array<any>;
  //      //authList.forEach(item => {
  //      //  this.editItem(item);
  //      //});
  //      this.id = res.id;
  //    }
  //  })
  //}
  //public edit() {

  //  this.apiService.get('PurchaseOrder', this.id).subscribe(res => {
  //    if (res) {
  //      this.isReadOnly = true;
  //      //this.form.value['tranDate'] = this.utilService.selectedDateTime(this.form.controls['tranDate'].value);
  //      //this.form.value['deliveryDate'] = this.utilService.selectedDateTime(this.form.controls['deliveryDate'].value);


  //      //this.form.patchValue({ 'tranCurrencyCode': `${res['tranCurrencyCode']}` });

  //      this.form.patchValue(res);
  //      this.form.patchValue({ 'tranDate': `${res['tranDate'].split('T')[0]}` });
  //      this.form.patchValue({ 'deliveryDate': `${res['deliveryDate'].split('T')[0]}` });
  //      this.form.patchValue({ 'compCode': `${res['compCode']}` });
  //      this.form.patchValue({ 'tranCurrencyCode': `${res['tranCurrencyCode']}` });
  //      this.form.patchValue({ 'poNotes': `${res['poNotes']}` });

  //      /*this.listOfInvoices.clear();*/
  //      let listOfInvoices = res['itemList'] as Array<any>;
  //      listOfInvoices.forEach(item => {
  //        //this.editInvoiceItem(item);
  //        this.listOfInvoices.push({
  //          sequence: this.getSequence(),
  //          tranNumber: "0", tranItemCode: item.tranItemCode, tranItemName: item.tranItemName, tranItemName2: item.tranItemName2, tranItemQty: item.tranItemQty, tranItemUnitCode: item.tranItemUnitCode, tranUOMFactor: item.tranUOMFactor,
  //          tranItemCost: item.tranItemCost, tranTotCost: item.tranTotCost, discPer: item.discPer, discAmt: item.discAmt, itemTax: item.itemTax, itemTaxPer: item.itemTaxPer, taxAmount: item.taxAmount, itemTracking: item.itemTracking
  //        });
  //      });
  //      this.setGrandTotal();
  //      /*this.id = res.id;*/
  //    }
  //  })
  //}

  purchaseOrdersubmit() {
    if (this.id > 0)
      this.form.value['id'] = this.id;

    this.apiService.post('purchasereturn', this.form.value)
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
        this.apiService.delete('purchasereturn', id).subscribe(res => {
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

  loadBranchs(event: any) {
    let comId = event.target.value;
    if (comId) {
      this.apiService.getall(`branch/getSelectSysBranchListByComId/${comId}`).subscribe(res => {
        if (res)
          this.branchList = res;
      });
    }
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
   // if (this.tranTotCost > 0) {
      if (this.tranTotCost >= 0) {
      if (this.editsequence > 0) {
        this.removeInvoiceList(this.editsequence);

        this.editsequence = 0;
      }
      this.listOfInvoices.push({
        sequence: this.getSequence(),

        tranNumber: "0", tranItemCode: this.tranItemCode, tranItemName: this.tranItemName, tranItemName2: '', tranItemQty: this.tranItemQty, tranItemUnitCode: this.tranItemUnitCode, tranUOMFactor: this.tranUOMFactor,
        tranItemCost: this.tranItemCost, tranTotCost: this.tranTotCost, discPer: this.discPer, discAmt: this.discAmt, itemTax: 0, itemTaxPer: this.itemTaxPer, taxAmount: this.taxAmount, itemTracking: 0,
        receivedQty: this.receivedQty, returnedQty: this.returnedQty, balQty: this.balQty, preBalQty: this.preBalQty
      });
   //   this.calculate();
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
    this.editsequence = item.sequence,
      this.tranItemCode = item.tranItemCode,
      this.tranItemName = item.tranItemName,
      this.tranItemName2 = item.tranItemName2,
      this.tranItemQty = item.tranItemQty,
      this.tranItemUnitCode = item.tranItemUnitCode,
      this.tranUOMFactor = item.tranUOMFactor,
      this.tranItemCost = item.tranItemCost,
      this.tranTotCost = item.tranTotCost,
      this.discPer = item.discPer,
      this.discAmt = item.discAmt,
      this.itemTax = item.itemTax,
      this.itemTaxPer = item.itemTaxPer,
      this.taxAmount = item.taxAmount,
      this.itemTracking = item.itemTracking;
    this.receivedQty = item.receivedQty;
    this.returnedQty = item.returnedQty;
    this.balQty = item.balQty;
    this.preBalQty = item.preBalQty;
    this.serExpTracking = item.serExpTracking;
    if (this.serExpTracking === 'EXP') {
      this.isExpiryButtonEnabled = true;
      this.buttonName = this.serExpTracking;
      this.showButton = true;  // Show the button
      this.isReadOnly = true;  // Make textbox read-only
    } else if (this.serExpTracking === 'SRL') {
      this.isExpiryButtonEnabled = false; // Button will be disabled
      this.buttonName = this.serExpTracking;
      this.showButton = false; // Hide the button
      this.isReadOnly = false; // Remove readonly from textbox
    } else {
      this.isExpiryButtonEnabled = false;
      this.buttonName = this.serExpTracking;
      this.showButton = false; // Hide the button
      this.isReadOnly = false; // Remove readonly from textbox
    }
  }

  setGrandTotal() {
    this.grandTotal = 0;
    this.grandVatTotal = 0;
    this.grandInvoiceTotal = 0;

    this.listOfInvoices.forEach(inv => {
      this.grandTotal += inv.tranTotCost;
      this.grandVatTotal += inv.taxAmount;
      this.grandInvoiceTotal += (inv.tranTotCost + inv.taxAmount);
    });

    //this.form.value['trantotalcost'] = this.grandTotal;
    //this.form.value['trandiscamount'] = this.grandVatTotal;
    //this.form.value['subTotal'] = this.grandInvoiceTotal;

    this.grandTotalStr = parseFloat(this.grandTotal.toString()).toFixed(2);
    this.grandVatTotalStr = parseFloat(this.grandVatTotal.toString()).toFixed(2);
    this.grandInvoiceTotalStr = parseFloat(this.grandInvoiceTotal.toString()).toFixed(2);
  }
  closeModel() {
    /*  this.refresh();*/
    this.ngOnInit();
    this.listOfInvoices = [];
    this.dialogRef.close();
  }


  setToDefault() {
    //this.productId = 0;
    //this.product = this.description = this.unitType = '';
    //this.quantity = this.price = this.vat = this.vatAmount = this.total = 0;
    this.tranItemCode = 0,
      this.tranItemName = '',
      this.tranItemName2 = '',
      this.tranItemQty = 0,
      this.tranItemUnitCode = '',
      this.tranUOMFactor = '',
      this.tranItemCost = 0,
      this.tranTotCost = 0,
      this.discPer = '0',
      this.discAmt = 0,
      this.itemTax = '';

    if (!this.isVatIncluded)
      this.itemTaxPer = '0';

    this.taxAmount = 0,
      this.itemTracking = 0;
  }

  calculate() {
    //this.openIntExpSerial();
    //var itemvat = parseFloat(this.vat.toString());
    //var qty = this.quantity;
    //var price = this.price;
    //var amount = parseFloat((qty * price).toString())
    //this.vatAmount = parseFloat(((amount * itemvat) / 100).toFixed(2));
    //this.total = parseFloat(this.vatAmount.toString()) + parseFloat(amount.toString());


    var qty = this.tranItemQty;
    var price = this.tranItemCost;
    var amount = parseFloat((qty * price).toString())
    this.tranTotCost = parseFloat(amount.toString());
    this.calculateDis();
    this.calculatetaxAmount();
  }


  calculateQuantity() {
    
    const whCode = this.form.controls['whCode'].value;

    if (whCode && this.tranItemCode && this.tranItemQty) {
      this.isQuantityLoading = true;
      this.apiService.getall(`purchasereturn/getItemsForWarehouseSelectList?whCode=${whCode}&itemCode=${this.tranItemCode}&itemCount=${this.tranItemQty}`).subscribe(res => {
        if (res) {
          this.isQuantityLoading = false;
          if (res.value == "1") {
            this.notifyService.showError(` Warehouse Quantity (${res.text})`)
            this.tranItemQty = this.tranTotCost = 0;
          }
        }
      });
    }

  }

  //calculateDis() {
  //  //var cost = this.tranTotCost;
  //  //var perc = parseFloat(this.discPer);
  //  //var DisAmount = parseFloat(((cost * perc) / 100).toFixed(2));
  //  //this.discAmt = parseFloat(DisAmount.toString());

  //  if (this.discPer != "" && this.tranItemCost != 0) {
  //    var cost = this.tranTotCost;
  //   /* var perc = parseFloat(this.discPer);*/
  //    var perc = parseFloat((parseFloat(this.discPer) / 100).toFixed(2));
  //    var DisAmount = parseFloat(((cost * perc) / 100).toFixed(2));
  //    this.discAmt = parseFloat(DisAmount.toString());
  //    var itemCost = this.tranItemCost;
  //    /* this.tranItemCost = ((itemCost) - (this.discAmt));*/
  //    this.tranTotCost = ((cost) - (this.discAmt * this.tranItemQty));
  //    var perc = parseFloat(this.itemTaxPer);
  //    if (this.form.value['taxInclusive'] == 0) {
  //      this.taxAmount = parseFloat(((this.tranTotCost * perc) / 100).toFixed(2));
  //    }
  //    else {
  //      //var C1 = (this.tranItemCost - (this.tranItemCost * parseFloat(this.discPer)));
  //      //var totoalCost = (C1 * this.tranItemQty);
  //      this.taxAmount = parseFloat(((this.tranTotCost * perc) / ((perc) + 100)).toFixed(4));
  //    }

  //  }

  //}
  calculateDis() {
    if (this.discPer != "" && this.tranItemCost != 0) {
      var cost = this.tranTotCost;
      /*var perc = parseFloat(this.discPer);*/
      var perc = parseFloat((parseFloat(this.discPer) / 100).toFixed(2));
      var DisAmount = parseFloat(((cost * perc)).toFixed(2));/// 100
      this.discAmt = parseFloat(DisAmount.toString());
      var itemCost = this.tranItemCost;
      /* this.tranItemCost = ((itemCost) - (this.discAmt));*/
      /*this.tranTotCost = ((cost) - (this.discAmt * this.tranItemQty));*/
      //this.tranTotCost = ((itemCost) - (itemCost * this.discAmt));
      this.tranTotCost = ((itemCost) - (itemCost * perc));

      this.tranTotCost = (this.tranTotCost) * (this.tranItemQty);

      var perc = parseFloat(this.itemTaxPer);
      if (this.form.value['taxInclusive'] == 0) {
        this.taxAmount = parseFloat(((this.tranTotCost * perc) / 100).toFixed(2));
      }
      else {
        //var C1 = (this.tranItemCost - (this.tranItemCost * parseFloat(this.discPer)));
        //var totoalCost = (C1 * this.tranItemQty);
        this.taxAmount = parseFloat(((this.tranTotCost * perc) / ((perc) + 100)).toFixed(4));
      }

    }

  }
  calculatetaxAmount() {
    var cost = this.tranTotCost;
    var perc = parseFloat(this.itemTaxPer);
    this.taxAmount = parseFloat(((cost * perc) / 100).toFixed(2));
  }
  submit() {

    if (this.form.valid) {

      if (this.id > 0)
        this.form.value['id'] = this.id;


      if (!this.form.value['tranShipMode'] && !(this.form.value['shipmentMode'] as string).trim()) {
        this.utilService.localizeError('shipmentMode');
        return;
      }

      if (this.listOfInvoices.length > 0) {
        this.form.value['itemList'] = this.listOfInvoices;
        this.form.value['tranNumber'] = "0";
        this.form.value['Prcode'] = "0";
        this.form.value['vendCode'] = this.form.value.venCatCode;

        //this.grandTotal = 0;
        //this.grandVatTotal = 0;
        //this.listOfInvoices.forEach(inv => {
        //  this.grandTotal += inv.tranTotCost;
        //  this.grandVatTotal += inv.taxAmount;
        //});
        //this.form.value['tranTotalCost'] = this.grandTotal;
        //this.form.value['taxes'] = this.grandVatTotal;

        //this.grandTotal = 0;
        //this.grandVatTotal = 0;
        //this.grandInvoiceTotal = 0;
        //this.listOfInvoices.forEach(inv => {
        //  this.grandTotal += inv.tranTotCost;
        //  this.grandVatTotal += inv.taxAmount;
        //  this.grandInvoiceTotal += (inv.tranTotCost + inv.taxAmount);
        //});
        //this.grandTotalStr = parseFloat(this.grandTotal.toString()).toFixed(2);
        //this.grandVatTotalStr = parseFloat(this.grandVatTotal.toString()).toFixed(2);
        //this.grandInvoiceTotalStr = parseFloat(this.grandInvoiceTotal.toString()).toFixed(2);
        //this.form.value['tranTotalCost'] = this.grandInvoiceTotalStr;
        //this.form.value['taxes'] = this.grandVatTotalStr;

        this.grandTotal = 0;
        this.DiscPerc = 0;
        this.discAmt = 0;
        this.grandVatTotal = 0;


        this.listOfInvoices.forEach(inv => {
          this.grandTotal += inv.tranTotCost;
          this.grandVatTotal += inv.taxAmount;
          this.DiscPerc += parseFloat(inv.discPer);
          this.discAmt += inv.discAmt;


        });

        this.form.value['tranTotalCost'] = this.grandTotal;
        this.form.value['taxes'] = this.grandVatTotal;
        this.form.value['tranDiscPer'] = this.DiscPerc;
        this.form.value['tranDiscAmount'] = this.discAmt;

        this.apiService.post('purchasereturn/CreatePurchaserequest', this.form.value)
          .subscribe(res => {
            debugger;
            this.reset();
            this.dialogRef.close(true);
            this.utilService.OkMessage();
          },
            error => {
              console.error(error);
              this.utilService.ShowApiErrorMessage(error);
            });
      }
      else
        this.notifyService.showError("Line Items Empty");
    }
    else
      this.utilService.FillUpFields();
  }


  reset() {
    this.form.reset();
  }
  public edit() {

    this.apiService.get('purchasereturn', this.id).subscribe(res => {
      if (res) {
        this.isReadOnly = true;
        this.form.patchValue(res);
        this.form.patchValue({ 'tranDate': `${res['tranDate'].split('T')[0]}` });
        if (res['deliveryDate'])
          this.form.patchValue({ 'deliveryDate': `${res['deliveryDate'].split('T')[0]}` });

        this.form.patchValue({ 'compCode': `${res['compCode']}` });
        this.form.patchValue({ 'tranCurrencyCode': `${res['tranCurrencyCode']}` });
        this.form.patchValue({ 'poNotes': `${res['poNotes']}` });
        this.form.patchValue({ 'serExpTracking': `${res['serExpTracking']}` });
        let listOfInvoices = res['itemList'] as Array<any>;
        this.serExpTracking1 = res['serExpTracking'];

        listOfInvoices.forEach(item => {
          this.listOfInvoices.push({
            sequence: this.getSequence(),
            tranNumber: "0", tranItemCode: item.tranItemCode, tranItemName: item.tranItemName, tranItemName2: item.tranItemName2, tranItemQty: item.tranItemQty, tranItemUnitCode: item.tranItemUnitCode, tranUOMFactor: item.tranUOMFactor,
            tranItemCost: item.tranItemCost, tranTotCost: item.tranTotCost, discPer: item.discPer, discAmt: item.discAmt, itemTax: item.itemTax, itemTaxPer: item.itemTaxPer, taxAmount: item.taxAmount, itemTracking: item.itemTracking,
            receivedQty: item.receivedQty, returnedQty: item.returnedQty, balQty: item.balQty, preBalQty: item.preBalQty, serExpTracking: item.serExpTracking
          });
        });

        this.setGrandTotal();
        //this.id = res.id;
      }
    })
  }


  public openIntExpSerial() {
    const whCode = this.form.controls['whCode'].value;
    //const expItemTracking = this.form.controls['serExpTracking'].value
    var item = { 'tranitemcode': this.tranItemCode, 'tranitemname': this.tranItemName, 'tranitemqty': this.tranItemQty, 'pono': '000', 'tracking': this.serExpTracking, 'whcode': whCode, 'grnid': '0', 'tranuomfactor': this.tranUOMFactor };
    if (item.tracking == 'EXP' || this.serExpTracking1 =='EXP') {
      this.openDialogManage1(item, DBOperation.create, this.translate.instant('Create_New_Request'), '', AddupdatereturnexpairybatchComponent);
    } else {
      alert("Call Serial Popup");
    }
  }

  private openDialogManage1<T>(item: any, dbops: DBOperation, modalTitle: string = '', modalBtnTitle: string = '', component: T, moduleFile: MultiFileUploadDto = { module: '00', action: '00act' }, width: number = 60) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, component, width);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).inputData = item;
    (dialogRef.componentInstance as any).moduleFile = moduleFile;

    dialogRef.afterClosed().subscribe(res => {
      console.log("Dialog closed with result:", res);  // Debug log
      if (res && res.isOk && res.totalItemQuantity !== undefined) {
        this.tranItemQty = res.totalItemQuantity;
        this.initialLoading();
      }
    });
  }


  public view() {

    this.apiService.get('purchasereturn', this.id).subscribe(res => {
      if (res) {
        this.isReadOnly = true;
        this.form.patchValue(res);
        this.form.patchValue({ 'tranDate': `${res['tranDate'].split('T')[0]}` });
        if (res['deliveryDate'])
          this.form.patchValue({ 'deliveryDate': `${res['deliveryDate'].split('T')[0]}` });

        this.form.patchValue({ 'compCode': `${res['compCode']}` });
        this.form.patchValue({ 'tranCurrencyCode': `${res['tranCurrencyCode']}` });
        this.form.patchValue({ 'poNotes': `${res['poNotes']}` });
        let listOfInvoices = res['itemList'] as Array<any>;
        listOfInvoices.forEach(item => {
          this.listOfInvoices.push({
            sequence: this.getSequence(),
            tranNumber: "0", tranItemCode: item.tranItemCode, tranItemName: item.tranItemName, tranItemName2: item.tranItemName2, tranItemQty: item.tranItemQty, tranItemUnitCode: item.tranItemUnitCode, tranUOMFactor: item.tranUOMFactor,
            tranItemCost: item.tranItemCost, tranTotCost: item.tranTotCost, discPer: item.discPer, discAmt: item.discAmt, itemTax: item.itemTax, itemTaxPer: item.itemTaxPer, taxAmount: item.taxAmount, itemTracking: item.itemTracking,
            receivedQty: item.receivedQty, returnedQty: item.returnedQty, balQty: item.balQty, preBalQty: item.preBalQty
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
  approvePurchaseReturn(project: any) {
    let serviceType = 'PRT';
    let serviceCode = project.purchaseRequestNO;
    let branchCode = project.branchCode;
    this.openApprovalDialog(branchCode, serviceCode, DBOperation.create, 'Purchase_Return', 'Save', serviceType, ApprovaldialogwindowsComponent);

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


  loadGRNReturndata(event: any) {
    let PONO = event.value;
    this.apiService.getall(`purchasereturn/getGRNReturnDetails/${PONO}`).subscribe(res => {
      if (res) {
        this.isReadOnly = true;
        this.form.patchValue(res);
        this.form.patchValue({ 'tranDate': `${res['tranDate'].split('T')[0]}` });

        if (res['deliveryDate'])
          this.form.patchValue({ 'deliveryDate': `${res['deliveryDate'].split('T')[0]}` });

        this.form.patchValue({ 'compCode': `${res['compCode']}` });
        this.form.patchValue({ 'tranCurrencyCode': `${res['tranCurrencyCode']}` });
        this.form.patchValue({ 'poNotes': `${res['poNotes']}` });
        this.form.patchValue({ 'purchaseRequestNO': PONO });
        this.form.patchValue({ 'trantype': this.trantypeValue });
        //this.id = res.id;

        this.listOfInvoices = [];
        let listOfInvoices = res['itemList'] as Array<any>;
        listOfInvoices.forEach(item => {
          this.listOfInvoices.push({
            sequence: this.getSequence(),
            tranNumber: "0", tranItemCode: item.tranItemCode, tranItemName: item.tranItemName, tranItemName2: item.tranItemName2, tranItemQty: item.tranItemQty, tranItemUnitCode: item.tranItemUnitCode, tranUOMFactor: item.tranUOMFactor,
            tranItemCost: item.tranItemCost, tranTotCost: item.tranTotCost, discPer: item.discPer, discAmt: item.discAmt, itemTax: item.itemTax, itemTaxPer: item.itemTaxPer, taxAmount: item.taxAmount, itemTracking: item.itemTracking,
            receivedQty: item.receivedQty, returnedQty: item.returnedQty, balQty: item.balQty, preBalQty: item.preBalQty
          });
        });
        this.setGrandTotal();
      }
    })
  }

}
