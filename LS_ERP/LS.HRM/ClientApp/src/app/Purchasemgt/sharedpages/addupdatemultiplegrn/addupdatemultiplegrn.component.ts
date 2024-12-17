import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { NotificationService } from '../../../services/notification.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { UtilityService } from '../../../services/utility.service';
import { ApiService } from '../../../services/api.service';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { from, Observable } from 'rxjs';
import { CommonService } from '../../../services/common.service';
import { DBOperation } from '../../../services/utility.constants';
import { PaginationService } from '../../../sharedcomponent/pagination.service';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { DeleteConfirmDialogComponent } from '../../../sharedcomponent/delete-confirm-dialog';
import { ApprovaldialogwindowsComponent } from '../../approvaldialogwindows/approvaldialogwindows.component';
import { MatSort } from '@angular/material/sort';
import { MultiFileUploadDto } from '../../../models/sharedDto';
import { AddupdateinvitemexpserialbatchComponent } from '../addupdateinvitemexpserialbatch/addupdateinvitemexpserialbatch.component';
import { AddupdateinvexpbatchComponent } from '../addupdateinvexpbatch/addupdateinvexpbatch.component';
import { TranslateService } from '@ngx-translate/core';
import { Addupdateexpairybatch } from '../addupdateexpairybatch/addupdateexpairybatch.component';


@Component({
  selector: 'app-addupdatemultiplegrn',
  templateUrl: './addupdatemultiplegrn.component.html',
  styleUrls: []
})
export class AddupdatemultiplegrnComponent implements OnInit {

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  //@ViewChild(MatPaginator) paginator: MatPaginator;
  //@ViewChild(MatSort) sort: MatSort;
  /* displayedColumns: string[] = [];*/
  /*  displayedColumns: string[] = ['request', 'vendor', 'docnum', 'branch', 'amount', 'vat', 'reference', 'Actions'];*/
  displayedColumns: string[] = ['purchaseOrderNO', 'tranDate', 'invRefNumber', 'branchCode', 'vendCode', 'paymentID', 'taxId', 'Actions'];
  data: MatTableDataSource<any> | null;
  totalItemsCount: number;
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
  DiscPerc: number = 0;

  sequence: number = 1;
  editsequence: number = 0;
  listOfInvoices: Array<any> = [];

  canShowNotes: boolean = false;
  canShowRemarks: boolean = false;
  invoiceItemObject: any;



  //data: MatTableDataSource<any> | null;
  //totalItemsCount: number;
  //sortingOrder: string = 'id';
  form!: FormGroup;
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
  isshown: boolean = false;
  pono: string = '';
  tranItemCode: number = 0;
  tranItemName: string = '';
  tranItemName2: string = '';
  tranItemQty: number = 0;
  tranItemUnitCode: string = '';
  tranUOMFactor: string = '';
  tranItemCost: number = 0;
  tranTotCost: number = 0;
  discPer: string = '';
  discAmt: number = 0;
  itemTax: string = '';
  itemTaxPer: string = '';
  taxAmount: number = 0;

  receivingQty: number = 0;
  balQty: number = 0;
  receivedQty: number = 0;

  itemTracking: number = 0;
  serExpTracking: string = '';
  WHCode: string = '';
  isExpiryButtonEnabled: boolean = false;
  buttonName: string = '';
  shouldHideButtonName: boolean = false;  // Condition to disable the name
  tranNumber: string='';

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdatemultiplegrnComponent>,
    private notifyService: NotificationService, private validationService: ValidationService, public pageService: PaginationService, public dialog: MatDialog,
    private translate: TranslateService  ) {



  }


  ngOnInit(): void {
    this.initialLoading();
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
      trantype: "2"
    });
    if (this.id > 0) {
      this.GRN();


    }

    this.isExpiryButtonEnabled = false;

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

  loadCompCode() {
    this.apiService.getall('PurchaseOrder/GetCompSelectList').subscribe(res => {
      if (res) {
        this.compCodeList = res;
      }
    })
  }
  loadBranchCode() {
    this.apiService.getall('PurchaseOrder/GetBranchSelectList').subscribe(res => {
      if (res) {
        this.BranchCodeList = res;
      }
    })
  }
  loadTaxCode() {
    this.apiService.getall('PurchaseOrder/GetTaxSelectList').subscribe(res => {
      if (res) {
        this.TaxCodeList = res;
      }
    })
  }
  loadVenCode() {
    this.apiService.getall('PurchaseOrder/GetVendorCodeSelectList').subscribe(res => {
      if (res) {
        this.VenCodeList = res;
      }
    })
  }
  loadVenName() {
    this.apiService.getall('PurchaseOrder/GetVendorNameSelectList').subscribe(res => {
      if (res) {
        this.VenNameList = res;
      }
    })
  }
  loadPaymentTerm() {
    this.apiService.getall('PurchaseOrder/GetPaymentTermSelectList').subscribe(res => {
      if (res) {
        this.PaymentTermList = res;
      }
    })
  }
  loadItemCode() {
    this.apiService.getall('PurchaseOrder/GetItemCodeSelectList').subscribe(res => {
      if (res) {
        this.ItemCodeList = res as Array<any>;
        this.ItemCodeList.forEach(e => {
          e.text = e.textTwo + " (" + e.value+")";
        });
      }
    })
  }
  loadItemName() {
    this.apiService.getall('PurchaseOrder/GetItemNameSelectList').subscribe(res => {
      if (res) {
        this.ItemNameList = res;
      }
    })
  }
  loadUnitCode() {
    this.apiService.getall('PurchaseOrder/GetUOMSelectList').subscribe(res => {
      if (res) {
        this.UOMList = res;
      }
    })
  }
  loadCurrencyCode() {
    this.apiService.getall('PurchaseOrder/GetCurrencySelectList').subscribe(res => {
      if (res) {
        this.CurrencyCodeList = res;
      }
    })
  }
  loadShipmentCode() {
    this.apiService.getall('PurchaseOrder/GetShipmentSelectList').subscribe(res => {
      if (res) {
        this.ShipmentCodeList = res;
      }
    })
  }
  loadUomdata(event: any) {
    let ItemUomCode = event.value;
    let ItemList = this.tranItemCode + '_' + ItemUomCode;
    if (ItemList) {

      this.apiService.getall(`PurchaseOrder/ProductUomtPriceItem/${ItemList}`).subscribe(res => {
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
      '': ['', Validators.required],
      /*itemList: this.fb.array([this.createAuthority()])//, Validators.required)*/
      'compCode': ['', Validators.required],
      'branchCode': ['', Validators.required],
      'tranDate': ['', Validators.required],
      'deliveryDate': ['', Validators.required],
      'invRefNumber': ['', Validators.required],
      'venCatCode': ['', Validators.required],
      'docNumber': ['', Validators.required],
      'taxId': ['', Validators.required],
      'remarks': ['', Validators.required],
      'poNotes': ['', Validators.required],
      'trantype': ['', Validators.required],
      'vendCode': ['', Validators.required],
      'paymentID': ['', Validators.required],
      'taxInclusive': ['', Validators.required],
      'tranCurrencyCode': ['', Validators.required],
      'tranShipMode': ['', Validators.required],
      'whCode': ['', Validators.required],
      'Prcode': ['', Validators.required],


      //authList: this.fb.array([this.createAuthority()]),



    });

  }
  loadPurchaseRequest() {
    this.apiService.getall('PurchaseOrder/GetGRNSelectList').subscribe(res => {
      if (res) {
        this.PurchaseRequestList = res;
        this.isExpiryButtonEnabled = false;
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


  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined, approval: string = "") {
    this.isLoading = true;

    this.apiService.getPagination('PurchaseOrder/GetPagedPurchaseOrderList', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
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



    this.apiService.post('PurchaseOrder', this.form.value)
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
        this.apiService.delete('PurchaseOrder', id).subscribe(res => {
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
    if (this.tranTotCost > 0) {
      if (this.editsequence > 0) {
        this.removeInvoiceList(this.editsequence);

        this.editsequence = 0;
      }
      this.listOfInvoices.push({
        sequence: this.getSequence(),

        tranNumber: "0", tranItemCode: this.tranItemCode, tranItemName: this.tranItemName, tranItemName2: '', tranItemQty: this.tranItemQty, tranItemUnitCode: this.tranItemUnitCode, tranUOMFactor: this.tranUOMFactor,
        tranItemCost: this.tranItemCost, tranTotCost: this.tranTotCost, discPer: this.discPer, discAmt: this.discAmt, itemTax: 0, itemTaxPer: this.itemTaxPer, whCode: this.WHCode,
        taxAmount: this.taxAmount, itemTracking: 0, receivingQty: this.receivingQty, balQty: this.balQty, receivedQty: this.receivedQty, serExpTracking: this.serExpTracking
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
      this.pono = item.PONO;
      this.form.value['Prcode'] = item.PONO;
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
      this.itemTracking = item.itemTracking,
      this.receivingQty = item.receivingQty,
      this.balQty = item.balQty,
      this.receivedQty = item.receivedQty,
      this.WHCode = item.WHCode,
      this.tranNumber = item.tranNumber
      this.serExpTracking = item.serExpTracking
    if(this.serExpTracking === 'EXP') {
      this.isExpiryButtonEnabled = true;
      this.buttonName = this.serExpTracking;
    } else if (this.serExpTracking === 'SRL') {
      this.isExpiryButtonEnabled = true;  // This is commented out
      this.buttonName = this.serExpTracking;
    } else {
      this.buttonName = this.serExpTracking;
    }
   
  }





 

  //setGrandTotal() {
  //  this.grandTotal = 0;
  //  this.grandVatTotal = 0;
  //  this.grandInvoiceTotal = 0;

  //  this.listOfInvoices.forEach(inv => {
  //    this.grandTotal += inv.tranTotCost;
  //    this.grandVatTotal += inv.taxAmount;
  //    this.grandInvoiceTotal += (inv.tranTotCost + inv.taxAmount);
  //  });

  //  //this.form.value['trantotalcost'] = this.grandTotal;
  //  //this.form.value['trandiscamount'] = this.grandVatTotal;
  //  //this.form.value['subTotal'] = this.grandInvoiceTotal;

  //  this.grandTotalStr = parseFloat(this.grandTotal.toString()).toFixed(2);
  //  this.grandVatTotalStr = parseFloat(this.grandVatTotal.toString()).toFixed(2);
  //  this.grandInvoiceTotalStr = parseFloat(this.grandInvoiceTotal.toString()).toFixed(2);
  //}
  setGrandTotal() {
    this.grandTotal = 0;
    this.grandVatTotal = 0;
    this.grandInvoiceTotal = 0;

    //this.listOfInvoices.forEach(inv => {
    //  this.grandTotal += inv.tranTotCost;
    //  this.grandVatTotal += inv.taxAmount;
    //  this.grandInvoiceTotal += (inv.tranTotCost + inv.taxAmount);
    //});
    if (this.form.value['taxInclusive'] == 0) {
      this.listOfInvoices.forEach(inv => {
        this.grandTotal += inv.tranTotCost;
        this.grandVatTotal += inv.taxAmount;
        this.grandInvoiceTotal += (inv.tranTotCost + inv.taxAmount);
      });
    }
    if (this.form.value['taxInclusive'] == 1) {
      this.listOfInvoices.forEach(inv => {
        this.grandInvoiceTotal += inv.tranTotCost;
        this.grandVatTotal += inv.taxAmount;
        this.grandTotal += (inv.tranTotCost - inv.taxAmount);
      });
    }

    //if (this.form.value['taxInclusive'] == 1) {
    //  this.grandTotalStr1 = parseFloat((this.grandTotal.toString())) - parseFloat(this.grandVatTotal.toString());
    //this.grandVatTotalStr = parseFloat(this.grandVatTotal.toString()).toFixed(3);
    //  this.grandInvoiceTotalStr1 = parseFloat((this.grandTotal.toString())) + parseFloat(this.grandVatTotal.toString());
    //}
    //else {
    //  this.grandTotalStr1 = parseFloat(this.grandTotal.toString());
    //  this.grandVatTotalStr = parseFloat(this.grandVatTotal.toString()).toFixed(3);
    //  this.grandInvoiceTotalStr1 = parseFloat(this.grandInvoiceTotal.toString());
    //}
    this.grandTotalStr = parseFloat(this.grandTotal.toString()).toFixed(3);
    this.grandVatTotalStr = parseFloat(this.grandVatTotal.toString()).toFixed(3);
    this.grandInvoiceTotalStr = parseFloat(this.grandInvoiceTotal.toString()).toFixed(3);
  }
  closeModel() {
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
      this.discPer = '',
      this.discAmt = 0,
      this.itemTax = '',
      this.itemTaxPer = '',
      this.taxAmount = 0,
      this.itemTracking = 0,
      this.receivingQty = 0,
      this.balQty = 0,
      this.receivedQty = 0,
      this.serExpTracking=''
    this.isExpiryButtonEnabled = false;
  }

  calculate() {

    var qty = this.receivingQty;
    var price = this.tranItemCost;
    var amount = parseFloat((qty * price).toString())
    this.tranTotCost = parseFloat(amount.toString());
  }

  calculateDis() {

    if (this.discPer != "" && this.tranItemCost != 0) {


      var itemvat = parseFloat(this.itemTaxPer.toString());
      if (parseFloat(this.discPer) > 100) {
        this.discPer = '0';
      }


      var discount = parseFloat(this.discPer);
      var qty = this.tranItemQty;
      var price = this.tranItemCost;

      var amount = parseFloat((qty * price).toString())
      var discountPrice = parseFloat(((amount * discount) / 100).toFixed(2));
      amount = parseFloat((amount - discountPrice).toString());
    
      this.discAmt = discountPrice;
      this.tranTotCost = parseFloat(amount.toString());

      var perc = parseFloat(this.itemTaxPer);
      if (this.form.value['taxInclusive'] != '') {
        if (this.form.value['taxInclusive'] == '0') {
          // this.taxAmount = parseFloat(((this.tranTotCost * perc) / 100).toFixed(2));
          //this.taxAmount = parseFloat(((amount * itemvat) / 100).toFixed(2));
          this.taxAmount = 0;
          this.itemTaxPer = '0';
        }
        else {
          //var C1 = (this.tranItemCost - (this.tranItemCost * parseFloat(this.discPer)));
          //var totoalCost = (C1 * this.tranItemQty);
          //this.taxAmount = parseFloat(((this.tranTotCost * perc) / ((perc) + 100)).toFixed(4));
          this.taxAmount =  parseFloat(((amount * itemvat) / 100).toFixed(2));
        }
      }
      else {
        this.taxAmount = 0;
        this.itemTaxPer = '0';
      }

      //  this.vatAmount = parseFloat(((amount * itemvat) / 100).toFixed(2));
      this.tranTotCost = parseFloat(this.taxAmount.toString()) + parseFloat(amount.toString());

      //this.calculate();
      //if ( this.tranItemCost != 0) {
      //  var cost = this.tranTotCost;

      //  var perc = parseFloat((parseFloat(this.discPer) / 100).toFixed(2));
      //  var DisAmount = parseFloat(((cost * perc) / 100).toFixed(2));
      //  this.discAmt = parseFloat(DisAmount.toString());
      //  var itemCost = this.tranItemCost;
      //  this.tranTotCost = ((itemCost) - (itemCost * perc));
      //  this.tranTotCost = (this.tranTotCost) * (this.receivingQty);
      //  var perc = parseFloat(this.itemTaxPer);
      //  if (this.form.value['taxInclusive'] == 0) {
      //    this.taxAmount = parseFloat(((this.tranTotCost * perc) / 100).toFixed(2));
      //  }
      //  else {
      //    this.taxAmount = parseFloat(((this.tranTotCost * perc) / ((perc) + 100)).toFixed(4));
      //  }

    }

  }
  calculatetaxAmount() {

    if (this.form.value['taxInclusive'] != '' && this.form.value['taxInclusive'] == '1') {
      var cost = this.tranTotCost;
      var perc = parseFloat(this.itemTaxPer);
      this.taxAmount = parseFloat(((cost * perc) / 100).toFixed(2));
    }
    else {
      this.taxAmount = 0;
      this.itemTaxPer = '0';
    }

    //var cost = this.tranTotCost;
    //var perc = parseFloat(this.itemTaxPer);
    //this.taxAmount = parseFloat(((cost * perc) / 100).toFixed(2));
  }
  submit() {
    if (this.id > 0)
      this.form.value['id'] = this.id;
    /* this.form.value['Prcode'] = "0";*/
    this.form.value['vendCode'] = this.form.value.venCatCode;
    this.form.value['purchaseOrderNO'] = this.form.value.Prcode;
    if (this.form.value.Prcode == null || this.form.value.Prcode=="") {
      this.form.value['purchaseOrderNO'] = this.pono;
     
    }


    this.form.value['itemList'] = this.listOfInvoices;
    this.form.value['tranNumber'] = "0";
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
    this.apiService.post('PurchaseOrder/CreateGRN', this.form.value)
      .subscribe(res => {
        //debugger;
        this.reset();
        this.dialogRef.close(true);
        this.utilService.OkMessage();

      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });
  }

  loadTaxdata(event: any) {

    let ItemCode = event.value;
    if (ItemCode) {

      this.apiService.getall(`PurchaseOrder/ProductTaxPrice/${ItemCode}`).subscribe(res => {


        if (res) {
          this.itemTaxPer = res[0].itemTaxperc;
          this.tranItemName = res[0].shortName;

          /* this.form.patchValue({ 'tranItemName': `${res[0].shortName}` });*/
          /*this.form.patchValue({ 'tranItemName': `${res[0]['shortName']}` });*/




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
  loadVendata(event: any) {

    let Vencode = event.value;
    if (Vencode) {

      this.apiService.getall(`PurchaseOrder/ProductVenPriceItem/${Vencode}`).subscribe(res => {
        if (res) {
          /*this.form.patchValue(res);*/
          this.form.patchValue({ 'vendCode': `${res['vendName']}` });
          this.form.patchValue({ 'paymentID': `${res['poTermsCode']}` });
        }
      });
    }

  }

  reset() {
    this.form.reset();
    this.listOfInvoices = [];
    this.grandTotalStr = '';
    this.grandVatTotalStr = '';
    this.grandInvoiceTotalStr = '';
  }
  public GRN() {
    /* this.isshown = false;*/
    this.listOfInvoices = [];
    this.apiService.getall(`PurchaseOrder/GRNDetails/${this.id}`).subscribe(res => {
      if (res) {
        this.isReadOnly = true;
        this.form.patchValue(res);
        this.form.patchValue({ 'tranDate': `${res['tranDate'].split('T')[0]}` });
        this.form.patchValue({ 'deliveryDate': `${res['deliveryDate'].split('T')[0]}` });
        this.form.patchValue({ 'compCode': `${res['compCode']}` });
        this.form.patchValue({ 'tranCurrencyCode': `${res['tranCurrencyCode']}` });
        this.form.patchValue({ 'poNotes': `${res['poNotes']}` });
        this.form.patchValue({ 'Prcode': `${res['purchaseOrderNO']}` });
        this.form.patchValue({ 'WHCode': `${res['whCode']}` });
        this.form.patchValue({ 'tranNumber': `${res['tranNumber']}` });
        let listOfInvoices = res['itemList'] as Array<any>;

        listOfInvoices.forEach(item => {
         // this.editInvoiceItem(item);
                this.listOfInvoices.push({
            sequence: this.getSequence(),
            tranNumber: item.tranNumber, tranItemCode: item.tranItemCode, tranItemName: item.tranItemName, tranItemName2: item.tranItemName2, tranItemQty: item.tranItemQty, tranItemUnitCode: item.tranItemUnitCode, tranUOMFactor: item.tranUOMFactor, PONO: item.poNo, WHCode: item.whCode,
            tranItemCost: item.tranItemCost, tranTotCost: item.tranTotCost, discPer: item.discPer, discAmt: item.discAmt, itemTax: item.itemTax, itemTaxPer: item.itemTaxPer, taxAmount: item.taxAmount, itemTracking: item.itemTracking, receivingQty: item.receivingQty, balQty: item.balQty, receivedQty: item.receivedQty, serExpTracking: item.serExpTracking
          });
        });
        this.setGrandTotal();
        this.id = res.id;
      }
    })
    //this.apiService.get('PurchaseOrder/GRN', this.id).subscribe(res => {

    //})
  }

  loadApprovals(evt: any) {
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder, evt.target.value);
  }
  approvePurchaseOrder(project: any) {
    let serviceType = 'PO';
    let serviceCode = project.purchaseOrderNO;
    let branchCode = project.branchCode;
    this.openApprovalDialog(branchCode, serviceCode, DBOperation.create, 'Purchase_Order', 'Save', serviceType, ApprovaldialogwindowsComponent);

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
  AccountPosting(id: number) {
    //this.apiService.getall('PurchaseOrder/GetPrSelectList').subscribe(res => {
    //  if (res) {
    //    this.PurchaseRequestList = res;
    //  }
    //})

    this.apiService.getall(`PurchaseOrder/AccountsPosting/${id}`).subscribe(res => {
      if (res) {
        this.utilService.OkMessage();
        this.refresh();

      }
    })
  }

  //public openIntExpSerial1(row: any) {

  //  this.openDialogManage(0, DBOperation.create, row.expSerial, row.PONO, AddupdateinvitemexpserialbatchComponent);

  //}

  public openIntExpSerial() {
    var item = { 'tranItemCode': this.tranItemCode, 'tranItemName': this.tranItemName, 'tranItemQty': this.tranItemQty, 'PONO': this.pono, 'tracking': this.serExpTracking, 'whCode': this.WHCode, 'grnId': this.tranNumber,'tranUOMFactor':this.tranUOMFactor };
    if (item.tracking == 'EXP') {
      this.openDialogManage1(item, DBOperation.create, this.translate.instant('Create_New_Grn_Request'), '', Addupdateexpairybatch);
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
      if (res && res === true) {
        this.initialLoading();
      }
    });
  }


  //public edit(id: number) {
  //  this.openDialogManage(id, DBOperation.update, this.translate.instant('Create_New_Grn_Request'), '', AddupdatemultiplegrnComponent);
  //}

  public createExp(item: any) {
    
    this.openDialogManage(item, DBOperation.update, this.translate.instant('Create_New_Expairy_Request'), '', Addupdateexpairybatch);
  }

  private openDialogManage<T>(item: any, dbops: DBOperation, modalTitle: string = '', modalBtnTitle: string = '', component: T, moduleFile: MultiFileUploadDto = { module: '00', action: '00act' }, width: number = 70, height: number=50) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, component, width);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).inputData = item;
    (dialogRef.componentInstance as any).moduleFile = moduleFile;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true) {
        this.initialLoading();
      }
    });
  }

  loadGRNdata(event: any) {
    let PONO = event.value;
    //this.isExpiryButtonEnabled = false;
    this.shouldHideButtonName = true;
    this.buttonName = '';
    this.apiService.getall(`PurchaseOrder/GetGRNList/${PONO}`).subscribe(res => {
      if (res) {
        this.setToDefault();
        this.isReadOnly = true;
        this.form.patchValue(res);
        this.form.patchValue({ 'tranDate': `${res['tranDate'].split('T')[0]}` });
        this.form.patchValue({ 'deliveryDate': `${res['deliveryDate'].split('T')[0]}` });
        this.form.patchValue({ 'compCode': `${res['compCode']}` });
        this.form.patchValue({ 'tranCurrencyCode': `${res['tranCurrencyCode']}` });
        this.form.patchValue({ 'poNotes': `${res['poNotes']}` });
        this.form.patchValue({ 'Prcode': `${res['purchaseOrderNO']}` });
        this.form.patchValue({ 'WHCode': `${res['whCode']}` });
        //this.id = res.id;

        this.listOfInvoices = [];
        let listOfInvoices = res['itemList'] as Array<any>;
        listOfInvoices.forEach(item => {
          this.listOfInvoices.push({
            sequence: this.getSequence(),
            tranNumber: "0", tranItemCode: item.tranItemCode, tranItemName: item.tranItemName, tranItemName2: item.tranItemName2, tranItemQty: item.tranItemQty, tranItemUnitCode: item.tranItemUnitCode, tranUOMFactor: item.tranUOMFactor, PONO: item.poNo, WHCode: item.whCode,
            tranItemCost: item.tranItemCost, tranTotCost: item.tranTotCost, discPer: item.discPer, discAmt: item.discAmt, itemTax: item.itemTax, itemTaxPer: item.itemTaxPer, taxAmount: item.taxAmount, itemTracking: item.itemTracking, receivingQty: 0, balQty: item.balQty, receivedQty: 0, serExpTracking: item.serExpTracking
          });
        });
        this.setGrandTotal();
      }
    })
  }
}
