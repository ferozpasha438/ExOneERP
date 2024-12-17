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
import { ApprovaldialogwindowsComponent } from '../approvaldialogwindows/approvaldialogwindows.component';
import { ParentSystemSetupComponent } from '../../sharedcomponent/parentsystemsetup.component';
import { AddupdatepurchaseorderComponent } from '../sharedpages/addupdatepurchaseorder/addupdatepurchaseorder.component';
import { MultiFileUploadDto } from '../../models/sharedDto';
import { TranslateService } from '@ngx-translate/core';
import { AddupdatemultiplegrnComponent } from '../sharedpages/addupdatemultiplegrn/addupdatemultiplegrn.component';
import { PoprintingpageComponent } from '../sharedpages/poprintingpage/poprintingpage.component';

@Component({
  selector: 'app-grn',
  templateUrl: './grn.component.html',
  styleUrls: []
}) export class GRNComponent extends ParentPurchaseMgtComponent implements OnInit {

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  //@ViewChild(MatPaginator) paginator: MatPaginator;
  //@ViewChild(MatSort) sort: MatSort;
  /* displayedColumns: string[] = [];*/
  /*  displayedColumns: string[] = ['request', 'vendor', 'docnum', 'branch', 'amount', 'vat', 'reference', 'Actions'];*/
  displayedColumns: string[] = ['tranNumber','purchaseOrderNO', 'tranDate', 'invRefNumber', 'branchCode', 'vendCode', 'amount', 'paymentID', 'taxId', 'itemName', 'Actions'];
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
  form: FormGroup;
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


  ItemCodeList: Array<CustomSelectListItem> = [];
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
  itemTracking: number = 0;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService,
    private notifyService: NotificationService, private validationService: ValidationService, public pageService: PaginationService, public dialog: MatDialog, private translate: TranslateService) {

    super(authService);

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

  }
  onSortOrder(sort: any) {
    this.totalItemsCount = 0;
    this.sortingOrder = sort.active + ' ' + sort.direction;
    this.loadUser(0, this.pageService.pageCount, "", this.sortingOrder);
  }

  //onPageSwitch(event: PageEvent) {
  //  this.pageService.change(event);
  //  this.loadUser(event.pageIndex, event.pageSize, "", this.sortingOrder);
  //}

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
        this.ItemCodeList = res;
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


      //authList: this.fb.array([this.createAuthority()]),



    });

  }
  loadPurchaseRequest() {
    this.apiService.getall('PurchaseOrder/GetPrSelectList').subscribe(res => {
      if (res) {
        this.PurchaseRequestList = res;
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
  loadPRdata(event: any) {
    let PRValue = event.value;
    this.apiService.getall(`PurchaseOrder/GetPRList/${PRValue}`).subscribe(res => {
      if (res) {
        this.isReadOnly = true;
        this.form.patchValue(res);
        //this.form.value['tranTotalCost'] = this.grandTotal;
        //this.form.value['taxes'] = this.grandVatTotal;
        //this.form.value['tranDiscPer'] = this.DiscPerc;
        //this.form.value['tranDiscAmount'] = this.discAmt;



        this.form.patchValue({ 'tranDate': `${res['tranDate'].split('T')[0]}` });
        this.form.patchValue({ 'deliveryDate': `${res['deliveryDate'].split('T')[0]}` });
        this.form.patchValue({ 'compCode': `${res['compCode']}` });
        this.form.patchValue({ 'tranCurrencyCode': `${res['tranCurrencyCode']}` });
        this.form.patchValue({ 'poNotes': `${res['poNotes']}` });
        /*this.form.patchValue({ 'Id': `${res['Id']}` });*/
        this.id = res.id;
        this.form.patchValue({
          trantype: "2"
        });
        this.listOfInvoices = [];
        let listOfInvoices = res['itemList'] as Array<any>;
        listOfInvoices.forEach(item => {
          this.listOfInvoices.push({
            sequence: this.getSequence(),
            tranNumber: "0", tranItemCode: item.tranItemCode, tranItemName: item.tranItemName, tranItemName2: item.tranItemName2, tranItemQty: item.tranItemQty, tranItemUnitCode: item.tranItemUnitCode, tranUOMFactor: item.tranUOMFactor,
            tranItemCost: item.tranItemCost, tranTotCost: item.tranTotCost, discPer: item.discPer, discAmt: item.discAmt, itemTax: item.itemTax, itemTaxPer: item.itemTaxPer, taxAmount: item.taxAmount, itemTracking: item.itemTracking
          });
        });
        this.setGrandTotal();
      }
    })
  }

  onPageSwitch(event: PageEvent) {
    this.pageService.change(event);
    this.loadList(event.pageIndex, event.pageSize, this.searchValue, this.sortingOrder);
  }

  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined, approval: string = "") {
    this.isLoading = true;

    this.apiService.getPagination('PurchaseOrder/GetPagedGRNList', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
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


  purchaseOrdersubmit() {
    if (this.id > 0)
      this.form.value['id'] = this.id;



    this.apiService.post('PurchaseOrder', this.form.value)
      .subscribe(res => {
        debugger;

        if (res) {

          this.utilService.OkMessage();
        }




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
        this.apiService.delete('PurchaseOrder/GRNDelete', id).subscribe(res => {
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
    this.sortingOrder = 'id desc';
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
        tranItemCost: this.tranItemCost, tranTotCost: this.tranTotCost, discPer: this.discPer, discAmt: this.discAmt, itemTax: 0, itemTaxPer: this.itemTaxPer, taxAmount: this.taxAmount, itemTracking: 0
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
    /*  this.refresh();*/
    /* this.ngOnInit();*/
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
    this.listOfInvoices = [];

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
      this.itemTracking = 0;
  }

  calculate() {

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
  }
  //calculateDis() {
  //  var cost = this.tranTotCost;
  //  var perc = parseFloat(this.discPer);
  //  var DisAmount = parseFloat(((cost * perc) / 100).toFixed(2));
  //  this.discAmt = parseFloat(DisAmount.toString());
  //}
  //calculateDis() {
  //  if (this.discPer != "" && this.tranItemCost != 0) {
  //    var cost = this.tranTotCost;
  //    /*var perc = parseFloat(this.discPer);*/
  //    var perc = parseFloat((parseFloat(this.discPer) / 100).toFixed(2));
  //    var DisAmount = parseFloat(((cost * perc) / 100).toFixed(2));
  //    this.discAmt = parseFloat(DisAmount.toString());
  //    var itemCost = this.tranItemCost;
  //    /* this.tranItemCost = ((itemCost) - (this.discAmt));*/
  //    /*this.tranTotCost = ((cost) - (this.discAmt * this.tranItemQty));*/
  //    //this.tranTotCost = ((itemCost) - (itemCost * this.discAmt));
  //    this.tranTotCost = ((itemCost) - (itemCost * perc));

  //    this.tranTotCost = (this.tranTotCost) * (this.tranItemQty);

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
      var DisAmount = parseFloat(((cost * perc) / 100).toFixed(2));
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
    if (this.id > 0)
      this.form.value['id'] = this.id;
    this.form.value['Prcode'] = "0";
    this.form.value['vendCode'] = this.form.value.venCatCode;

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
        debugger;
        this.reset();
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
  //public edit(id: number) {
  //  this.listOfInvoices = [];
  //  this.apiService.get('PurchaseOrder', id).subscribe(res => {
  //    if (res) {
  //      this.isReadOnly = true;
  //      this.form.patchValue(res);
  //      this.form.patchValue({ 'tranDate': `${res['tranDate'].split('T')[0]}` });
  //      this.form.patchValue({ 'deliveryDate': `${res['deliveryDate'].split('T')[0]}` });
  //      this.form.patchValue({ 'compCode': `${res['compCode']}` });
  //      this.form.patchValue({ 'tranCurrencyCode': `${res['tranCurrencyCode']}` });
  //      this.form.patchValue({ 'poNotes': `${res['poNotes']}` });
  //      let listOfInvoices = res['itemList'] as Array<any>;

  //      listOfInvoices.forEach(item => {
  //        this.listOfInvoices.push({
  //          sequence: this.getSequence(),
  //          tranNumber: "0", tranItemCode: item.tranItemCode, tranItemName: item.tranItemName, tranItemName2: item.tranItemName2, tranItemQty: item.tranItemQty, tranItemUnitCode: item.tranItemUnitCode, tranUOMFactor: item.tranUOMFactor,
  //          tranItemCost: item.tranItemCost, tranTotCost: item.tranTotCost, discPer: item.discPer, discAmt: item.discAmt, itemTax: item.itemTax, itemTaxPer: item.itemTaxPer, taxAmount: item.taxAmount, itemTracking: item.itemTracking
  //        });
  //      });
  //      this.setGrandTotal();
  //      this.id = res.id;
  //    }
  //  })
  //}
  //public view(id: number) {
  //  this.isshown = true;
  //  this.listOfInvoices = [];
  //  this.apiService.get('PurchaseOrder', id).subscribe(res => {
  //    if (res) {
  //      this.isReadOnly = false;
  //      this.form.patchValue(res);
  //      this.form.patchValue({ 'tranDate': `${res['tranDate'].split('T')[0]}` });
  //      this.form.patchValue({ 'deliveryDate': `${res['deliveryDate'].split('T')[0]}` });
  //      this.form.patchValue({ 'compCode': `${res['compCode']}` });
  //      this.form.patchValue({ 'tranCurrencyCode': `${res['tranCurrencyCode']}` });
  //      this.form.patchValue({ 'poNotes': `${res['poNotes']}` });
  //      let listOfInvoices = res['itemList'] as Array<any>;

  //      listOfInvoices.forEach(item => {
  //        this.listOfInvoices.push({
  //          sequence: this.getSequence(),
  //          tranNumber: "0", tranItemCode: item.tranItemCode, tranItemName: item.tranItemName, tranItemName2: item.tranItemName2, tranItemQty: item.tranItemQty, tranItemUnitCode: item.tranItemUnitCode, tranUOMFactor: item.tranUOMFactor,
  //          tranItemCost: item.tranItemCost, tranTotCost: item.tranTotCost, discPer: item.discPer, discAmt: item.discAmt, itemTax: item.itemTax, itemTaxPer: item.itemTaxPer, taxAmount: item.taxAmount, itemTracking: item.itemTracking
  //        });
  //      });
  //      this.setGrandTotal();
  //      this.id = res.id;
  //    }
  //  })
  //}
  loadApprovals(evt: any) {
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder, evt.target.value);
  }
  approvePurchaseOrder(project: any) {
    let serviceType = 'GN';
    let serviceCode = project.tranNumber;
    let branchCode = project.branchCode;
    this.openApprovalDialog(branchCode, serviceCode, DBOperation.create, 'GRN', 'Save', serviceType, ApprovaldialogwindowsComponent);

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
    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canTakeAction => {
      if (canTakeAction) {
        this.apiService.getall(`PurchaseOrder/GRNAccountsPosting/${id}`).subscribe(res => {
          if (res) {
            this.utilService.OkMessage();
            this.refresh();
          }
        })
      }
    })
  }

  public create() {
    this.openDialogManage(0, DBOperation.create, this.translate.instant('Create_New_Grn_Request'), '', AddupdatemultiplegrnComponent);
  }

  public edit(id: number) {
    this.openDialogManage(id, DBOperation.update, this.translate.instant('Create_New_Grn_Request'), '', AddupdatemultiplegrnComponent);
  }
  public MultipleGrn(id: number) {
    this.openDialogManage(id, DBOperation.update, this.translate.instant('Create_New_GRN'), '', AddupdatemultiplegrnComponent);
  }
  public view(id: number) {
    this.openDialogManage(id, DBOperation.update, this.translate.instant('Create_New_Grn_Request'), '', AddupdatemultiplegrnComponent);
  }

  public printing(id: number) {
    this.openDialogManage(id, DBOperation.update, this.translate.instant('Create_New_Purchase_Request'), 'grn', PoprintingpageComponent);
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

}
