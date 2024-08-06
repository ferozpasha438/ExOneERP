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
import { MatDialogRef } from '@angular/material/dialog';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { from, Observable } from 'rxjs';
import { CommonService } from '../../../services/common.service';
import { DBOperation } from '../../../services/utility.constants';
import { TranslateService } from '@ngx-translate/core';
/*import { parse } from 'path';*/
@Component({
  selector: 'app-addupdatepurchaserequest',
  templateUrl: './addupdatepurchaserequest.component.html',
  styleUrls: []
})
export class AddupdatepurchaserequestComponent implements OnInit {
  isReadOnly: boolean = false;
  /* tranNumber: number = 0;*/
  form: FormGroup;
  modalTitle: string;
  id: number = 0;
  ShipmentCodeList: Array<CustomSelectListItem> = [];
  CurrencyCodeList: Array<CustomSelectListItem> = [];
  TaxCodeList: Array<CustomSelectListItem> = [];
  VenCodeList: Array<CustomSelectListItem> = [];
  compCodeList: Array<CustomSelectListItem> = [];
  BranchCodeList: Array<CustomSelectListItem> = [];
  VenNameList: Array<CustomSelectListItem> = [];
  PaymentTermList: Array<CustomSelectListItem> = [];
  customerList: Array<CustomSelectListItem> = [];
  companyList: Array<CustomSelectListItem> = [];

  ItemCodeList: Array<any> = [];
  ItemNameList: Array<CustomSelectListItem> = [];
  UOMList: Array<CustomSelectListItem> = [];
  warehouseList: Array<CustomSelectListItem> = [];
  isshown: boolean = false;

  ItemTrackingList = [
    { value: 1, text: "Yes" },
    { value: 0, text: "NO" }

  ]
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

  grandTotalStr1: number = 0;
  grandVatTotalStr1: number = 0;
  grandInvoiceTotalStr1: number = 0;

  sequence: number = 1;
  editsequence: number = 0;
  listOfInvoices: Array<any> = [];

  canShowNotes: boolean = false;
  canShowRemarks: boolean = false;
  invoiceItemObject: any;


  tranItemCode: number = 0;
  tranItemName: string = '';
  tranItemName2: string = '';
  tranItemQty: number = 0;
  tranItemUnitCode: string = '';
  tranUOMFactor: string = '';
  tranItemCost: number = 0;
  tranTotCost: number = 0;
  discPer: string = '0';
  DiscPerc: number = 0;
  discAmt: number = 0;
  itemTax: string = '';
  itemTaxPer: string = '0';
  taxAmount: number = 0;
  itemTracking: number = 0;

  isVatIncluded: boolean = false;
  isQuantityLoading: boolean = false;


  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdatepurchaserequestComponent>,
    private notifyService: NotificationService, private validationService: ValidationService, private commonService: CommonService) {



  }

  ngOnInit(): void {
    //let $: any;
    //$('.select2').select2();
    this.setForm();
    /* this.loadData();*/
    this.loadWarehouses();
    this.loadItemCode();
    this.loadItemName();
    this.loadUnitCode();
    this.loadTaxCode();
    this.loadVenCode();
    this.loadCompCode();
    this.loadBranchCode();
    this.loadVenName();
    this.loadPaymentTerm();
    this.loadCurrencyCode();
    this.loadShipmentCode();
    this.form.patchValue({
      trantype: "1"
    });
    // this.paymentTermsList.push({ text: 'first payment', value: '1' });
    if (this.id > 0) {
      this.edit();
      /*      this.view();*/

    }

  }

  setForm() {
    let MOBILE_PATTERN = /[0-9\+\-\ ]/;
    this.form = this.fb.group({
      //'': ['', Validators.required],
      /*itemList: this.fb.array([this.createAuthority()])//, Validators.required)*/
      'compCode': ['', Validators.required],
      'branchCode': ['', Validators.required],
      'tranDate': ['', Validators.required],
      'deliveryDate': [],// Validators.required],
      'invRefNumber': [''],// Validators.required],
      'docNumber': ['', Validators.required],
      'taxId': ['', Validators.required],
      'remarks': ['', Validators.required],
      'poNotes': ['', Validators.required],
      'trantype': ['', Validators.required],
      'venCatCode': [],
      //'venCatCode': ['', Validators.required],
      'vendCode': [],
      //'vendCode': ['', Validators.required],
      'paymentID': ['', Validators.required],
      'taxInclusive': ['', Validators.required],
      'tranCurrencyCode': ['', Validators.required],
      'tranShipMode': [''],
      'shipmentMode': [''],
      //'tranShipMode': ['', Validators.required],
      'whCode': ['', Validators.required],


      //authList: this.fb.array([this.createAuthority()]),



    });

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
  loadWarehouses() {
    this.apiService.getall(`warehouse/getSelectWarehouseList`).subscribe(res => {
      if (res) {
        this.warehouseList = res;
      }
    });
  }
  loadItemCode() {
    this.apiService.getall('PurchaseOrder/GetItemCodeSelectList').subscribe(res => {
      if (res) {
        this.ItemCodeList = res as Array<any>;
        this.ItemCodeList.forEach(e => {
          e.text = e.textTwo + " (" + e.value + ")";
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
  loadwarehouse(event: any) {
    let branchCode = event.value;
    if (branchCode) {

      this.apiService.getall(`PurchaseOrder/ProductWarehouse/${branchCode}`).subscribe(res => {
        if (res) {
          this.warehouseList = res;
        }
      });
      this.loadcompany(branchCode);
    }

  }
  loadcompany(event: any) {
    let branchCode = event;
    if (branchCode) {

      this.apiService.getall(`PurchaseOrder/Productcompany/${branchCode}`).subscribe(res => {
        if (res) {
          this.compCodeList = res;
        }
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
  loadTaxdata(event: any) {

    let ItemCode = event.value;
    if (ItemCode) {

      this.apiService.getall(`PurchaseOrder/ProductTaxPrice/${ItemCode}`).subscribe(res => {


        if (res) {
          //this.itemTaxPer = res[0].itemTaxperc;
          this.tranItemName = res[0].shortName;

          /* this.form.patchValue({ 'tranItemName': `${res[0].shortName}` });*/
          /*this.form.patchValue({ 'tranItemName': `${res[0]['shortName']}` });*/




        }
        this.loadUomList(ItemCode);
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
    //}
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

  setGrandTotal() {
    this.grandTotal = 0;
    this.grandVatTotal = 0;
    this.grandInvoiceTotal = 0;

    this.listOfInvoices.forEach(inv => {
      this.grandInvoiceTotal += (inv.tranTotCost + inv.taxAmount);
      this.grandVatTotal += inv.taxAmount;
      this.grandTotal += inv.tranTotCost;
    });

    //if (this.form.value['taxInclusive'] == 0) {
    //  this.listOfInvoices.forEach(inv => {
    //    this.grandTotal += inv.tranTotCost;
    //    this.grandVatTotal += inv.taxAmount;
    //    this.grandInvoiceTotal += (inv.tranTotCost + inv.taxAmount);
    //  });
    //}
    //if (this.form.value['taxInclusive'] == 1) {
    //  this.listOfInvoices.forEach(inv => {
    //    this.grandInvoiceTotal += inv.tranTotCost;
    //    this.grandVatTotal += inv.taxAmount;
    //    this.grandTotal += (inv.tranTotCost - inv.taxAmount);
    //  });
    //}

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

  //setLabelPrices(total: number, vatAmount: number, operation: string) {
  //  this.grandTotal += total;
  //  this.grandVatTotal += vatAmount;
  //  this.grandInvoiceTotal += (total - vatAmount);

  //  this.grandTotalStr = this.grandTotal.toString();
  //  this.grandVatTotalStr = this.grandVatTotal.toString();
  //  this.grandInvoiceTotalStr = this.grandInvoiceTotal.toString();
  //}

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
      this.itemTax = '0';

    if (!this.isVatIncluded)
      this.itemTaxPer = '0';

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
    this.calculateDis();
    this.calculatetaxAmount();
  }
  calculateDis() {
    if (this.discPer != "" && this.tranItemCost != 0) {
      var cost = this.tranTotCost;
      /*var perc = parseFloat(this.discPer);*/
      var perc = parseFloat((parseFloat(this.discPer) / 100).toFixed(2));
      var DisAmount = parseFloat(((cost * perc)).toFixed(2)); /// 100
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


  //calculateDis() {
  //  if (this.discPer != "" && this.tranItemCost != 0) {
  //    var cost = this.tranTotCost;
  //    /*var perc = parseFloat(this.discPer);*/
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
  calculatetaxAmount() {
    var cost = this.tranTotCost;
    var perc = parseFloat(this.itemTaxPer);
    this.taxAmount = parseFloat(((cost * perc) / 100).toFixed(2));
  }



  submit() {

    if (this.form.valid) {

      if (this.id > 0)
        this.form.value['id'] = this.id;

      //if (!this.form.value['tranShipMode'] && !(this.form.value['shipmentMode'] as string).trim()) {
      //  this.utilService.localizeError('shipmentMode');
      //  return;
      //}

      if (this.listOfInvoices.length > 0) {
        this.form.value['itemList'] = this.listOfInvoices;
        this.form.value['tranNumber'] = "0";
        this.form.value['Prcode'] = "0";
        this.form.value['vendCode'] = this.form.value.venCatCode;



        //this.form.value['cancelDate'] = null;
        //this.form.value['isApproved'] = null;
        //this.form.value['tranCreateUserDate'] = null;
        //this.form.value['tranCreateUser'] = null;
        //this.form.value['tranLastEditDate'] = null;
        //this.form.value['tranLastEditUser'] = null;
        //this.form.value['tranPostStatus'] = null;
        //this.form.value['tranPostDate'] = null;
        //this.form.value['tranpostUser'] = null;
        //this.form.value['tranVoidStatus'] = null;
        //this.form.value['tranVoidUser'] = null;
        //this.form.value['tranvoidDate'] = null;
        //this.form.value['tranCurrencyCode'] = null;
        //this.form.value['exRate'] = null;
        //this.form.value['tranTotalCost'] = null;
        //this.form.value['tranDiscPer'] = null;
        //this.form.value['tranDiscAmount'] = null;
        //this.form.value['oHCharges'] = null;
        //this.form.value['taxes'] = null;
        //this.form.value['pOClosedDate'] = null;
        //this.form.value['closedBy'] = null;
        //this.form.value['foreClosed'] = null;
        //this.form.value['closed'] = null;
        //this.form.value['isActive'] = true;
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


        if (!this.form.value['deliveryDate'])
          this.form.value['deliveryDate'] = null;



        this.apiService.post('PurchaseOrder/CreatePurchaserequest', this.form.value)
          .subscribe(res => {
            //     debugger;
            this.reset();
            this.dialogRef.close(true);
            this.utilService.OkMessage();
          },
            error => {
              console.error(error);
              this.utilService.ShowApiErrorMessage(error);
            });

        //this.form.value['invoiceDate'] = this.utilService.selectedDateTime(this.form.controls['invoiceDate'].value);
        //this.form.value['invoiceDueDate'] = this.utilService.selectedDateTime(this.form.controls['invoiceDueDate'].value);

        /*if (this.form.valid) {*/

        //if (this.id > 0)
        //  this.form.value['id'] = this.id;

        //this.form.value['itemList'] = this.listOfInvoices;


        //this.apiService.post('generateInvoice', this.form.value)
        //  .subscribe(res => {
        //    this.utilService.OkMessage();
        //    this.reset();
        //    this.dialogRef.close(true);
        //  },
        //    error => {
        //      console.error(error);
        //      this.utilService.ShowApiErrorMessage(error);
        //    });
        //}
        //else
        //  this.utilService.FillUpFields();

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
  closeModel() {
    this.ngOnInit();
    this.listOfInvoices = [];
    this.dialogRef.close();
  }
  public edit() {
    this.listOfInvoices = [];
    this.apiService.get('PurchaseOrder', this.id).subscribe(res => {
      if (res) {
        this.isReadOnly = true;
        //this.form.value['tranDate'] = this.utilService.selectedDateTime(this.form.controls['tranDate'].value);
        //this.form.value['deliveryDate'] = this.utilService.selectedDateTime(this.form.controls['deliveryDate'].value);


        //this.form.patchValue({ 'tranCurrencyCode': `${res['tranCurrencyCode']}` });

        this.form.patchValue(res);
        this.form.patchValue({ 'tranDate': `${res['tranDate'].split('T')[0]}` });
        if (res['deliveryDate'])
          this.form.patchValue({ 'deliveryDate': `${res['deliveryDate'].split('T')[0]}` });

        this.form.patchValue({ 'compCode': `${res['compCode']}` });
        this.form.patchValue({ 'tranCurrencyCode': `${res['tranCurrencyCode']}` });
        this.form.patchValue({ 'poNotes': `${res['poNotes']}` });

        /*this.listOfInvoices.clear();*/
        let listOfInvoices = res['itemList'] as Array<any>;
        listOfInvoices.forEach(item => {
          //this.editInvoiceItem(item);
          this.listOfInvoices.push({
            sequence: this.getSequence(),
            tranNumber: "0", tranItemCode: item.tranItemCode, tranItemName: item.tranItemName, tranItemName2: item.tranItemName2, tranItemQty: item.tranItemQty, tranItemUnitCode: item.tranItemUnitCode, tranUOMFactor: item.tranUOMFactor,
            tranItemCost: item.tranItemCost, tranTotCost: item.tranTotCost, discPer: item.discPer, discAmt: item.discAmt, itemTax: item.itemTax, itemTaxPer: item.itemTaxPer, taxAmount: item.taxAmount, itemTracking: item.itemTracking
          });
        });
        this.setGrandTotal();
        /*this.id = res.id;*/
      }
    })
  }
  public view() {
    this.listOfInvoices = [];
    /* this.isshown = true;*/
    this.apiService.get('PurchaseOrder', this.id).subscribe(res => {
      if (res) {
        this.isReadOnly = true;
        //this.form.value['tranDate'] = this.utilService.selectedDateTime(this.form.controls['tranDate'].value);
        //this.form.value['deliveryDate'] = this.utilService.selectedDateTime(this.form.controls['deliveryDate'].value);


        //this.form.patchValue({ 'tranCurrencyCode': `${res['tranCurrencyCode']}` });

        this.form.patchValue(res);
        this.form.patchValue({ 'tranDate': `${res['tranDate'].split('T')[0]}` });
        if (res['deliveryDate'])
          this.form.patchValue({ 'deliveryDate': `${res['deliveryDate'].split('T')[0]}` });

        this.form.patchValue({ 'compCode': `${res['compCode']}` });
        this.form.patchValue({ 'tranCurrencyCode': `${res['tranCurrencyCode']}` });
        this.form.patchValue({ 'poNotes': `${res['poNotes']}` });

        /*this.listOfInvoices.clear();*/
        let listOfInvoices1 = res['itemList'] as Array<any>;
        listOfInvoices1.forEach(item => {
          //this.editInvoiceItem(item);
          this.listOfInvoices.push({
            sequence: this.getSequence(),
            tranNumber: "0", tranItemCode: item.tranItemCode, tranItemName: item.tranItemName, tranItemName2: item.tranItemName2, tranItemQty: item.tranItemQty, tranItemUnitCode: item.tranItemUnitCode, tranUOMFactor: item.tranUOMFactor,
            tranItemCost: item.tranItemCost, tranTotCost: item.tranTotCost, discPer: item.discPer, discAmt: item.discAmt, itemTax: item.itemTax, itemTaxPer: item.itemTaxPer, taxAmount: item.taxAmount, itemTracking: item.itemTracking
          });
        });
        this.setGrandTotal();
        /*this.id = res.id;*/
      }
    })
  }
}
