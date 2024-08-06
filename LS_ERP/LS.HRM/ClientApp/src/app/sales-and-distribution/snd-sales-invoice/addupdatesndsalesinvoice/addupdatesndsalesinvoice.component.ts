import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import moment from 'moment';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CustomSelectListItem, LanCustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
import { CommonService } from '../../../services/common.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ParentSalesMgtComponent } from '../../../sharedcomponent/parentsalesmgt.component';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { SndupdatingcustomerComponent } from '../../snd-customer/sndupdatigcustomer/sndupdatingcustomer.component';

@Component({
  selector: 'app-addupdatesndsalesinvoice',
  templateUrl: './addupdatesndsalesinvoice.component.html'
})
export class AddupdatesndsalesinvoiceComponent  implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}

//export class AddupdatesndsalesinvoiceComponent extends ParentSalesMgtComponent implements OnInit {

//  form: FormGroup;
//  modalTitle: string;
//  id: number = 0;

//  customerList: Array<LanCustomSelectListItem> = [];
//  invoiceNumberList: Array<CustomSelectListItem> = [];
//  companyList: Array<CustomSelectListItem> = [];
//  paymentTermsList: Array<LanCustomSelectListItem> = [];

// // branchList: Array<CustomSelectListItem> = [];
//  warehouseList: Array<CustomSelectListItem> = [];

// // productList: Array<CustomSelectListItem> = [];
// itemList: Array<CustomSelectListItem> = [];
//  vatList: Array<CustomSelectListItem> = [];
//  segmentSetupList: Array<CustomSelectListItem> = [];
//  segmentTwoSetupList: Array<CustomSelectListItem> = [];

// // productId: number = 0;
//  itemId: string = '';
//  itemCode: string = '';
//  quantity: number = 0;
//  unitPrice: number = 0;
//  vat: number = 0;
//  vatAmount: number = 0;
//  total: number = 0;
//  discount: number = 0;
//  discountAmount: number = 0;
// footerDiscount: number = 0;
//  footerDiscountAmount: number = 0;
//  unitType: string = '';
//  description: string = '';
// // product: string = '';

//  item: string = '';

//  grandDiscountAmount: number = 0;
//  grandTotal: number = 0;
//  grandVatTotal: number = 0;
//  grandInvoiceTotal: number = 0;
//  grandTotalStr: string = '';
//  grandVatTotalStr: string = '';
//  grandInvoiceTotalStr: string = '';
//  grandFooterDiscountAmount: number = 0;

//  itemInvAmount: number = 0;

//  sequence: number = 1;
//  editsequence: number = 0;
//  listOfInvoices: Array<any> = [];

//  canShowNotes: boolean = false;
//  canShowRemarks: boolean = false;
//  invoiceItemObject: any;
//  isArab: boolean = false;
//  invoice_NumberId: string = '';
//  customerData: null;

//  unitTypesList: Array<any> = [];// [{ text: "BOX", value: "BOX" }, { text: "EACH", value: "EACH" }];

//  canSave: boolean = false;
//  priceLevel: number = 0;   //basePrice
//  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
//    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdatesndsalesinvoiceComponent>,
//    private notifyService: NotificationService, private validationService: ValidationService, private commonService: CommonService, public dialog: MatDialog) {
//    super(authService);
//  }

//  ngOnInit(): void {
//    this.isArab = this.utilService.isArabic();
//    //let $: any;
//    //$('.select2').select2();
//    this.setForm();
//    this.loadData();
//    // this.paymentTermsList.push({ text: 'first payment', value: '1' });
//  }
//  setForm() {
//    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
//    this.form = this.fb.group({

//      'customerId': ['', Validators.required],
//      'invoiceDate': ['', Validators.required],
//      'invoiceDueDate': ['', Validators.required],
//      'serviceDate1': ['', Validators.required],
//      'companyId': ['', Validators.required],
//      'warehouseCode': ['', Validators.required],
//      'invoiceRefNumber': [''],
//      //'docNum': ['', Validators.required],
//      'lpoContract': [''],
//      'paymentTermId': ['', Validators.required],
//      'taxIdNumber': ['', Validators.required],
//      'remarks': [''],
//      'invoiceNotes': [''],
//      'invoiceStatusId': ['1'],
//       'seg1': [''],
//      'seg2': [''],

//      //'': ['', Validators.required],
//      //'': ['', Validators.required],

//    });

//  }

//  //changeInvoiceDate(event: MatDatepickerInputEvent<Date>) {
//  //  console.log(event.target.value);
//  //  let mInvDate1 = moment as any;
//  //  const mInvDate = mInvDate1(event.target.value);
//  //  const invDateFormat = mInvDate.format('YYYY-MM-DD') + 'T00:00:00';
//  //  // console.log(invDateFormat);
//  //  this.form.controls['invoiceDueDate'].setValue(invDateFormat);

//  //}

//  chagnesSalesTerms(event: any) {
//    const tearmsValue = event.target.value;
//    if (this.paymentTermsList.length > 0 && tearmsValue) {
//      const payTerm = this.paymentTermsList.find(item => item.value == tearmsValue);
//      const invDate = this.form.controls['invoiceDate'].value;
//      if (invDate) {
//        const custInvDate = new Date(invDate);
//        custInvDate.setDate(custInvDate.getDate() + parseInt(payTerm?.textTwo as string));
//        // console.log(invDay1.toLocaleDateString());
//        //this.form.controls['invoiceDueDate'].setValue(invDay1.toLocaleDateString() + "T00:00:00.000Z");

//        let mInvDate1 = moment as any;
//        const mInvDate = mInvDate1(custInvDate);
//        //const invDateFormat = mInvDate.format('MM/DD/YYYY');
//        const invDateFormat = mInvDate.format('YYYY-MM-DD') + 'T00:00:00';
//        // console.log(invDateFormat);
//        this.form.controls['invoiceDueDate'].setValue(invDateFormat);

//      }
//    }
//    else
//      this.form.controls['invoiceDueDate'].setValue('');
//  }


//  private openDialogManage(width: number) {
//    let dialogRef = this.utilService.openDialogCongif(this.dialog, SndupdatingcustomerComponent, width);
//    (dialogRef.componentInstance as any).data = this.customerData;

//    dialogRef.afterClosed().subscribe(res => {
//      if (res) {
//        this.customerData = res.value;
//        this.form.value['custName'] = res.value?.custName;
//        this.form.value['custArbName'] = res.value?.custArbName;
//      }
//      //else
//      //console.log(this.customerData);
//      //  this.customerData = null;
//    });
//  }

//  updateCustomer() {
//    this.openDialogManage(50);
//  }

//  loadData() {


//    this.apiService.getall("vat/getSelectVatList").subscribe(res => {
//      if (res)
//        this.vatList = res;
//    });

//    //this.apiService.getall("product/getSelectProductList").subscribe(res => {
//    //  if (res)
//    //    this.productList = res;
//    //});

//    this.apiService.getall("item/getSelectItemMOUList").subscribe(res => {
//      if (res)
//        this.itemList = res;
//    });

  



//    this.apiService.getall("customer/getSelectCustomerList").subscribe(res => {
//      if (res) {
//        this.customerList = res;
//        const newList = this.customerList.map((i) => {
//          i.text = !this.isArab ? i.text : i.textTwo;
//          return i;
//        });
//      }
//    });
//    this.apiService.getall("company/getSelectCompanyList").subscribe(res => {
//      if (res)
//        this.companyList = res;
//    });

//    this.apiService.getall("salesTermsCode/getCustomSelectSalesTermsCodeList").subscribe(res => {
//      if (res)
//        this.paymentTermsList = res;
//    });


//    this.apiService.getall("segmentSetup/getSegmentSetupSelectList").subscribe(res => {
//      if (res)
//        this.segmentSetupList = res;
//    });

//    this.apiService.getall("segmentTwoSetup/getSegmentTwoSetupSelectList").subscribe(res => {
//      if (res)
//        this.segmentTwoSetupList = res;
//    });
//  }







//  customerChange(event: any) {
//    let custId = event.value, custName = event.text;
//    this.invoice_NumberId = '';
//    this.listOfInvoices = [];
//    this.apiService.getall(`customer/getCustomerById/${custId}`).subscribe(res => {
//      if (res) {
//        this.form.controls['taxIdNumber'].setValue(res['vatNumber'])
//      }
//    });

//    this.apiService.getall(`generateSndInvoice/getCustomerInvoiceNumberList/${custId}`).subscribe(res => {
//      if (res) {
//        this.invoiceNumberList = res;
//      }
//    });
//  }

//  //loadBranchs(event: any) {
//  //  let comId = event.target.value;
//  //  if (comId) {
//  //    this.apiService.getall(`branch/getSelectSysAccountBranchList`).subscribe(res => {
//  //      if (res)
//  //        this.branchList = res;
//  //    });
//  //  }
//  //}



//loadWarehousesForCompany(comId: number, result: any) {
//    if (comId) {
//      this.apiService.getall(`warehouse/getSelectWarehouseCodeList`).subscribe(res => {
//        if (res) {
//          this.warehouseList = res;
//          this.form.patchValue(result);
//          this.footerDiscount = result.footerDiscount;

//          this.form.patchValue({ 'customerId': `${result['customerId']}` });
//        }
//      });
//    }
//  }
  

//  loadWarehouses(event: any) {
//    let comId = event.target.value;
//    if (comId) {
//      this.apiService.getall(`warehouse/getSelectWarehouseCodeList`).subscribe(res => {
//        if (res)
//          this.warehouseList = res;
//      });
//    }
//  }

//  //resetProduct() {
//  //  this.product = '';
//  //  this.unitType = '';
//  //  this.description = '';
//  //  this.unitPrice = 0;
//  //  this.total = 0;
//  //  this.discount = 0;
//  //  this.discountAmount = 0;
//  //  this.quantity = 0;
//  //}
//  resetItem() {
//    this.item = '';
//    this.unitType = '';
//    this.description = '';
//    this.unitPrice = 0;
//    this.total = 0;
//    this.discount = 0;
//    this.discountAmount = 0;

//    this.quantity = 0;
//  }
//  invoiceNumberChange(event: any) {
//  this.apiService.getall(`generateSndInvoice/getCustomerInvoiceItemsByIdList/${event.value}`).subscribe(res => {
//      if (res) {
//        this.listOfInvoices = [];
//        //this.displayProducts(res);
//        //this.displayProducts(res);
//        this.displayItems(res);
//        this.displayItems(res);
//      }
//    });
//  }

//  resetinvoiceNumber() {
//    this.listOfInvoices = [];
//  }

//  //displayProducts(result: any) {
//  //  let items = result as Array<any>;
//  //  items.forEach(item => {
//  //    this.productId = item.productId;
//  //    this.product = item.productName,
//  //      this.description = item.description;
//  //    this.quantity = item.quantity;
//  //    this.unitType = item.unitType;
//  //    this.unitPrice = item.unitPrice;
//  //    this.vat = item.taxTariffPercentage;
//  //    this.vatAmount = item.taxAmount;
//  //    this.total = item.totalAmount;
//  //    this.discount = item.discount;
//  //    this.discountAmount = item.discountAmount;

//  //    this.addInvoice();
//  //  });
//  //}


//  displayItems(result: any) {

//    let items = result['itemList'] as Array<any>;

//    items.forEach(item => {
//      this.itemId = item.itemId.toString();
//      this.itemCode = item.itemCode;
//      this.item = item.itemName,
//        this.description = item.description;
//      this.quantity = item.quantity;
//      this.unitType = item.unitType;
//      this.unitPrice = item.unitPrice;
//      this.vat = item.taxTariffPercentage;
//      this.discount = item.discount;



//      this.total = item.unitPrice * item.quantity;
//      this.discountAmount = this.total * this.discount / 100;
//      this.footerDiscountAmount = (this.total - this.discountAmount) * this.footerDiscount / 100;
//      this.vatAmount = (this.total - this.discountAmount - this.footerDiscountAmount) * this.vat / 100;
//      this.itemInvAmount = this.total - this.discountAmount - this.footerDiscountAmount + this.vatAmount;


//      this.addInvoice();
//    });
//  }





//  //loadProductdata(event: any) {
//  //  let prodictId = event.value, productName = event.text;
//  //  if (prodictId) {

//  //    this.apiService.getall(`product/productUnitPriceItem/${prodictId}`).subscribe(res => {
//  //      if (res) {
//  //        this.product = res.nameEN;
//  //        this.unitType = res.unitTypeEN;
//  //        this.description = res.description;
//  //        this.unitPrice = res.unitPrice;
//  //      }
//  //      //this.branchList = res;
//  //    });
//  //  }
//  //  else {
//  //    this.unitType = '';
//  //    this.description = '';
//  //  }
//  //}

//  loadItemdata(event: any) {
//    let itemId = event.value, itemName = event.text;
//    if (itemId != '') {

//      this.apiService.getall(`item/itemUnitPriceItem/${itemId}`).subscribe(res => {
//        if (res) {
//          this.itemId = itemId.toString();
//          this.itemCode = res.itemCode;


//          this.item = res.nameEN;

//          this.description = res.description;
//          this.unitPrice = res.unitPrice;

//          this.apiService.getall(`item/getSelectItemMOUUnitTypeListByItem/${res.itemCode}`).subscribe(res2 => {
//            if (res2) {
//              this.unitTypesList = res2;
//              this.unitType = res.unitTypeEN;
//              this.calculate();
//            }
//            else {
//              this.resetItem();
//            }
//          });




//          // this.loadUnitTypes();



//        }
//        //this.branchList = res;
//      });
//    }
//    else {
//      this.resetItem();
//    }
//  }


//  addInvoice() {
//    if (this.total != 0 && parseInt(this.itemId) > 0) {
//      if (this.editsequence > 0) {
//        // this.removeInvoiceList(this.editsequence);
//        //let index: number = this.listOfInvoices.findIndex(a => a.sequence === this.editsequence);
//        //this.listOfInvoices.splice(index, 1);


//        var index: number = this.listOfInvoices.findIndex(a => a.sequence === this.editsequence);

//        let pItem = this.listOfInvoices[index];
//        pItem.sequence = this.editsequence;
//        pItem.item = this.item;
//        pItem.itemId = this.itemId;
//        pItem.itemCode = this.itemCode;
//        pItem.description = this.description;
//        pItem.quantity = this.quantity;
//        pItem.unitType = this.unitType;
//        pItem.unitPrice = this.unitPrice;
//        pItem.taxTariffPercentage = this.vat;
//        pItem.taxAmount = this.vatAmount;
//        pItem.totalAmount = this.total;
//        pItem.discount = this.discount;
//        pItem.discountAmount = this.discountAmount ?? 0;
//        pItem.footerDiscountAmount = this.footerDiscountAmount;
//        pItem.itemInvAmount = this.itemInvAmount;


//        this.editsequence = 0;
//      }
//      else {

//        this.listOfInvoices.push({
//          sequence: this.getSequence(),
//          item: this.item, itemId: this.itemId, itemCode: this.itemCode, description: this.description, quantity: this.quantity,
//          unitType: this.unitType, unitPrice: this.unitPrice,
//          taxTariffPercentage: this.vat, taxAmount: this.vatAmount, totalAmount: this.total,
//          discount: this.discount, discountAmount: this.discountAmount, footerDiscountAmount: this.footerDiscountAmount,


//          itemInvAmount: this.itemInvAmount
//        });
//      }
//      //this.setLabelPrices(this.total, this.vatAmount, '');
//      this.setGrandTotal();
//      this.setToDefault();
//    }
//  }


//  getSequence(): number { return this.sequence = this.sequence + 1 };

//  showCanShowNotes() { this.canShowNotes = !this.canShowNotes }
//  showCanShowRemarks() { this.canShowRemarks = !this.canShowRemarks }

//  deleteInvoiceItem(item: any) {
//    this.removeInvoiceList(item.sequence);
//    this.setGrandTotal();
//  }

//  removeInvoiceList(sequence: number) {
//    let index: number = this.listOfInvoices.findIndex(a => a.sequence === sequence);
//    this.listOfInvoices.splice(index, 1);
//  }

//  editInvoiceItem(item: any) {
//    this.apiService.getall(`item/getSelectItemMOUUnitTypeListByItem/${item.itemCode}`).subscribe(res2 => {
//      if (res2) {
//        this.unitTypesList = res2;
//      }
//      else {
//        this.resetItem();
//      }
//    });




//    console.log(item);
//    this.editsequence = item.sequence,
//      this.item = item.item;
//    this.itemCode = item.itemCode;
//    this.itemId = item.itemId;
//    this.description = item.description;
//    this.quantity = item.quantity;
//    this.unitType = item.unitType;
//    this.unitPrice = item.unitPrice;
//    this.vat = item.taxTariffPercentage;
//    this.vatAmount = item.taxAmount;
//    this.total = item.totalAmount;
//    this.discount = item.discount;
//    this.discountAmount = item.discountAmount;
//    this.footerDiscountAmount = item.footerDiscountAmount;
//    this.itemInvAmount = item.itemInvAmount;

//  }


//  setGrandTotal() {
//    this.grandTotal = 0;
//    this.grandVatTotal = 0;
//    this.grandInvoiceTotal = 0;
//    this.grandDiscountAmount = 0;
//    this.grandFooterDiscountAmount = 0;

//    this.listOfInvoices.forEach(inv => {
//      this.grandTotal += inv.totalAmount;
//      this.grandVatTotal += inv.taxAmount;
//      this.grandInvoiceTotal += (inv.itemInvAmount);
//      this.grandDiscountAmount += inv.discountAmount;
//      this.grandFooterDiscountAmount += inv.footerDiscountAmount;
//    });

//    this.form.value['totalAmount'] = this.grandTotal;
//    this.form.value['discountAmount'] = this.grandDiscountAmount;

//    this.form.value['taxAmount'] = this.grandVatTotal;
//    this.form.value['subTotal'] = this.grandInvoiceTotal;

//    this.grandTotalStr = this.grandTotal.toString();
//    this.grandVatTotalStr = this.grandVatTotal.toString();
//    this.grandInvoiceTotalStr = this.grandInvoiceTotal.toString();
//  }

//  //setLabelPrices(total: number, vatAmount: number, operation: string) {
//  //  this.grandTotal += total;
//  //  this.grandVatTotal += vatAmount;
//  //  this.grandInvoiceTotal += (total - vatAmount);

//  //  this.grandTotalStr = this.grandTotal.toString();
//  //  this.grandVatTotalStr = this.grandVatTotal.toString();
//  //  this.grandInvoiceTotalStr = this.grandInvoiceTotal.toString();
//  //}


//  setToDefault() {
//    this.itemId = '';
//    this.item = this.description = this.unitType = '';
//    this.quantity = this.unitPrice = this.vat = this.vatAmount = this.total = this.discount = this.discountAmount = this.itemInvAmount = 0;
//  }

//  calculate() {



//    if (this.discount > 100) {
//      this.discount = 0;
//    }


//    this.total = this.unitPrice * this.quantity;

//    this.discountAmount = parseFloat(((this.total * this.discount) / 100).toFixed(2));

//    this.footerDiscountAmount = ((this.total - this.discountAmount) * this.footerDiscount) / 100;





//    this.vatAmount = parseFloat((((this.total - this.discountAmount - this.footerDiscountAmount) * this.vat) / 100).toFixed(2));

//    this.itemInvAmount = this.total - this.discountAmount - this.footerDiscountAmount + this.vatAmount;

//    // this.setLabelPrices(this.total, this.vatAmount, '');
//  }


//  submit() {
//    this.calculate();

//    //this.invoiceItemObject = {
//    //  item: {},
//    //  itemList: this.listOfInvoices
//    //}; 


//    if (this.form.valid) {
//      if (this.id > 0)
//        this.form.value['id'] = this.id;


//      this.form.value['itemList'] = this.listOfInvoices;

//      this.form.value['invoiceDate'] = this.utilService.selectedDateTime(this.form.controls['invoiceDate'].value);
//      this.form.value['invoiceDueDate'] = this.utilService.selectedDateTime(this.form.controls['invoiceDueDate'].value);

//      this.form.value['footerDiscount'] = this.footerDiscount;

//      this.apiService.post('generateSndInvoice', this.form.value)
//        .subscribe(res => {
//          this.utilService.OkMessage();
//          this.reset();
//          this.dialogRef.close(true);
//        },
//          error => {
//            console.error(error);
//            this.utilService.ShowApiErrorMessage(error);
//          });
//    }
//    else
//      this.utilService.FillUpFields();

//  }
//  reset() {
//    this.form.reset();
//  }
//  closeModel() {
//    this.dialogRef.close();
//  }
//  changePriceLevel() {
//    this.priceLevel = (this.priceLevel+1) % 4;
//    switch (this.priceLevel) {
//      case 0: this.unitPrice = 123; break;
//      case 1: this.unitPrice = 223; break;
//      case 2: this.unitPrice = 323; break;
//      case 3: this.unitPrice = 423; break;




//    }
//    this.calculate();
//  }






//  addUpdateInvoice() {

//    if (this.total != 0 && parseInt(this.itemId) > 0) {
//      if (this.editsequence > 0) {
//        // this.removeInvoiceList(this.editsequence);
//        //let index: number = this.listOfInvoices.findIndex(a => a.sequence === this.editsequence);
//        //this.listOfInvoices.splice(index, 1);


//        var index: number = this.listOfInvoices.findIndex(a => a.sequence === this.editsequence);

//        let pItem = this.listOfInvoices[index];
//        pItem.sequence = this.editsequence;
//        pItem.item = this.item;
//        pItem.itemId = this.itemId;
//        pItem.itemCode = this.itemCode;
//        pItem.description = this.description;
//        pItem.quantity = this.quantity;
//        pItem.unitType = this.unitType;
//        pItem.unitPrice = this.unitPrice;
//        pItem.taxTariffPercentage = this.vat;
//        pItem.taxAmount = this.vatAmount;
//        pItem.totalAmount = this.total;
//        pItem.discount = this.discount;
//        pItem.discountAmount = this.discountAmount;
//        pItem.footerDiscountAmount = this.footerDiscountAmount;


//        pItem.itemInvAmount = this.itemInvAmount;
//        this.editsequence = 0;
//      }
//      else {

//        this.listOfInvoices.push({
//          sequence: this.getSequence(),
//          item: this.item, itemId: this.itemId, itemCode: this.itemCode, description: this.description, quantity: this.quantity,
//          unitType: this.unitType, unitPrice: this.unitPrice,
//          taxTariffPercentage: this.vat, taxAmount: this.vatAmount, totalAmount: this.total,
//          discount: this.discount, discountAmount: this.discountAmount, footerDiscountAmount: this.footerDiscountAmount,
//          itemInvAmount: this.itemInvAmount
//        });
//      }
//      //this.setLabelPrices(this.total, this.vatAmount, '');
//      this.setGrandTotal();
//      this.setToDefault();

//      this.canSave = true;
//    }
//  }
//  changeFooterDiscount() {






//    if (this.footerDiscount < 0 || this.footerDiscount > 100 || this.footerDiscount == null || this.footerDiscount.toString() == '') {
//      this.footerDiscount = 0;
//    }
//    this.footerDiscount = parseInt(this.footerDiscount.toString());

//    for (let i = 0; i < this.listOfInvoices.length; i++) {

//      this.listOfInvoices[i].footerDiscountAmount = (this.listOfInvoices[i].totalAmount - this.listOfInvoices[i].discountAmount) * this.footerDiscount / 100;
//      this.listOfInvoices[i].taxAmount = (this.listOfInvoices[i].totalAmount - this.listOfInvoices[i].discountAmount - this.listOfInvoices[i].footerDiscountAmount) * this.listOfInvoices[i].taxTariffPercentage / 100;
//      this.listOfInvoices[i].itemInvAmount = this.listOfInvoices[i].totalAmount - this.listOfInvoices[i].discountAmount - this.listOfInvoices[i].footerDiscountAmount;

//    }

//    // this.calculate();
//    this.setGrandTotal();
//  }

//  changeUnitType() {


//    if (this.unitType != '') {

//      this.apiService.getall(`item/itemUnitPriceItemUnit/${this.itemCode}/${this.unitType}`).subscribe(res => {
//        if (res) {
//          // this.itemId = itemId.toString();
//          //  this.itemCode = res.itemCode;


//          //  this.item = res.nameEN;
//          //this.unitType = res.unitTypeEN;
//          // this.description = res.description;
//          this.unitPrice = res.unitPrice;







//        }
//        //this.branchList = res;
//      });
//    }
//    else {
//      this.resetItem();
//    }

//  }

