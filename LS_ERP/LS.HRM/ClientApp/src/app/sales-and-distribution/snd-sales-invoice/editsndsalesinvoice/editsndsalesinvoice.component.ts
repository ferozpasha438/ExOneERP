import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { CustomSelectListItem, LanCustomSelectListItem } from '../../../models/MenuItemListDto';
import { UtilityService } from '../../../services/utility.service';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { NotificationService } from '../../../services/notification.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { CommonService } from '../../../services/common.service';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ApiService } from '../../../services/api.service';
import { UpdatingcustomerComponent } from '../../../Finance/accountsreceivable/sharedpages/updatingcustomer/updatingcustomer.component';
import { ParentSalesMgtComponent } from '../../../sharedcomponent/parentsalesmgt.component';
import moment from 'moment';
import { DBOperation } from '../../../services/utility.constants';
import { MultiFileUploadDto } from '../../../models/sharedDto';
import { SndsettleinvoiceComponent } from '../../sharedpages/sndsettleinvoice/sndsettleinvoice.component';



@Component({
  selector: 'app-editsndsalesinvoice',
  templateUrl: './editsndsalesinvoice.component.html'
})
export class EditsndsalesinvoiceComponent extends ParentSalesMgtComponent implements OnInit {

  form: FormGroup;
  modalTitle: string;
  id: number = 0;

  customerList: Array<LanCustomSelectListItem> = [];
  companyList: Array<CustomSelectListItem> = [];
  paymentTermsList: Array<LanCustomSelectListItem> = [];
  warehouseList: Array<any> = [];
  itemList: Array<any> = [];
  vatList: Array<CustomSelectListItem> = [];

  itemId: any = null;
  quantity: number = 0;
  unitPrice: number = 0;
  vat: number = 0;
  vatAmount: number = 0;
  total: number = 0;
  itemInvAmount: number = 0;
  discount: number = 0;
  discountAmount: number = 0;
  

  unitType: string = '';
  description: string = '';
  item: string = '';
  itemCode: string = '';


  grandDiscountAmount: number = 0;
  grandTotal: number = 0;
  grandFooterDiscountAmount: number = 0;
  grandVatTotal: number = 0;

  grandInvoiceTotal: number = 0;
  grandTotalStr: string = '';
  grandVatTotalStr: string = '';
  grandInvoiceTotalStr: string = '';

  grandFooterDiscountAmountStr: string = '';

  footerDiscount: number = 0;
  footerDiscountAmount: number = 0;
  sequence: number = 1;
  editsequence: number = 0;
  listOfInvoices: Array<any> = [];

  canShowNotes: boolean = false;
  canShowRemarks: boolean = false;
  invoiceItemObject: any;
  isArab: boolean = false;
  customerData: null;
  //minDate = new Date(2022, 0, 1);
  //maxDate = new Date(2022, 0, 1);
  canSave: boolean = false;
  canSaveAndApprove: boolean = false;
  canSaveAndSettle: boolean = false;
  canCreate: boolean = false;
  unitTypesList: Array<any> = [];// [{ text: "BOX", value: "BOX" }, { text: "EACH", value: "EACH" }];

  invoiceStatusId: any = '1';


  isBarcodeScannerOn: boolean = true;
  itemBarcode: any = '';
    interval: any;
  isLoading: boolean = false;

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<EditsndsalesinvoiceComponent>,
    private notifyService: NotificationService, private validationService: ValidationService, private commonService: CommonService, public dialog: MatDialog) {
    super(authService);
  }

  ngOnInit(): void {
    //let $: any;
    //$('.select2').select2();
    this.setForm();
    this.loadData();
    // this.paymentTermsList.push({ text: 'first payment', value: '1' });
    if (this.id > 0)
      this.seteditform();
  }
  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      'customerId': ['', Validators.required],
      'invoiceDate': ['', Validators.required],
      'invoiceDueDate': ['', Validators.required],
      'serviceDate1': ['', Validators.required],
      'companyId': ['', Validators.required],
      'warehouseCode': ['', Validators.required],
      'invoiceRefNumber': [''],
      'lpoContract': [''],
      'paymentTermId': ['', Validators.required],
      'taxIdNumber': ['', Validators.required],
      'remarks': [''],
      'invoiceNotes': [''],
     // 'invoiceStatusId':['1'],







      //'': ['', Validators.required],
      //'': ['', Validators.required],

    });
  }

  seteditform() {

    this.apiService.get("generateSndInvoice/getSingleSndInvoiceById", this.id).subscribe(res => {
      if (res) {
        this.invoiceStatusId = res.invoiceStatusId;
        this.footerDiscount = res.footerDiscount;
        this.loadWarehousesForCompany(res['companyId'] as number, res);

        this.loadBranch();
        this.displayItems(res);

       
      }
    });
  }


  private openDialogManage(width: number) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, UpdatingcustomerComponent, width);
    (dialogRef.componentInstance as any).data = this.customerData;

    dialogRef.afterClosed().subscribe(res => {
      if (res) {
        this.customerData = res.value;
        this.form.value['custName'] = res.value?.custName;
        this.form.value['custArbName'] = res.value?.custArbName;
      }
      //else
      //console.log(this.customerData);
      //  this.customerData = null;
    });
  }

  updateCustomer() {
    this.openDialogManage(50);
  }


  loadData() {

    //this.itemList = [{ text: 'item One', value: '1' }, { text: 'item Two', value: '2' }]

    this.apiService.getall("vat/getSelectVatList").subscribe(res => {
      if (res)
        this.vatList = res;
    });

    this.apiService.getall("item/getSelectItemMOUList").subscribe(res => {
      if (res) {
        this.itemList = res as Array<any>;
        this.itemList.forEach(e => {
          e.text = e.text + " (" + e.textTwo + ")";
        });
      }

    });
    this.apiService.getall("customer/getSelectCustomerList").subscribe(res => {
      if (res) {
        this.customerList = res;
        const newList = this.customerList.map((i) => {
          i.text = !this.isArab ? i.text : i.textTwo;
          return i;
        });
      }
    });
    this.apiService.getall("company/getSelectCompanyList").subscribe(res => {
      if (res)
        this.companyList = res;
    });
    this.apiService.getall("salesTermsCode/getCustomSelectSalesTermsCodeList").subscribe(res => {
      if (res)
        this.paymentTermsList = res;
     
    });
  }

  customerChange(event: any) {
    let custId = event.value, custName = event.text;
    this.apiService.getall(`customer/getCustomerById/${custId}`).subscribe(res => {
      if (res) {
        this.form.controls['taxIdNumber'].setValue(res['vatNumber'])
      }
    });
  }




  loadWarehousesForCompany(comId: number, result: any) {
    if (comId) {
      this.apiService.getall(`warehouse/getSelectWarehouseCodeList`).subscribe(res => {
        if (res) {
          this.warehouseList = res;
          this.warehouseList.forEach(e => {
            e.lable = e.value + "-" + e.text;
          });
          this.form.patchValue(result);
          this.footerDiscount = result.footerDiscount;

          this.form.patchValue({ 'customerId': `${result['customerId']}` });

        }
        this.loadBranch();
      });

     
    }

  }

  displayItems(result: any) {

    let items = result['itemList'] as Array<any>;

    items.forEach(item => {
      this.itemId = item.itemId.toString();
      this.itemCode = item.itemCode;
      this.item = item.itemName,
        this.description = item.description;
      this.quantity = item.quantity;
      this.unitType = item.unitType;
      this.unitPrice = item.unitPrice;
      this.vat = item.taxTariffPercentage;
      this.discount = item.discount;



      this.total = item.unitPrice * item.quantity;
      this.discountAmount = this.total * this.discount / 100;
      this.footerDiscountAmount = (this.total - this.discountAmount) * this.footerDiscount / 100;
      this.vatAmount = (this.total - this.discountAmount - this.footerDiscountAmount) * this.vat / 100;
      this.itemInvAmount = this.total - this.discountAmount - this.footerDiscountAmount + this.vatAmount;


      this.addInvoice();
    });
  }

  checkUserAuthority(branchCode:string) {

    this.apiService.getall(`sndAuthorities/getAuthorityByBranchCurrentUser/${branchCode}`).subscribe(res => {
      if (res) {
        this.canCreate = res['canCreateSndInvoice'];
        this.canSaveAndApprove = res['canCreateSndInvoice'] && res['canApproveSndInvoice'];
        this.canSaveAndSettle = res['canCreateSndInvoice'] && res['canApproveSndInvoice'] && res['canSettleSndInvoice'];
      }
      else {
        this.canCreate = false;
        this.canSaveAndApprove = false;
        this.canSaveAndSettle = false;
      }
    });


  }

  loadWarehouses(event: any) {
    let comId = event.target.value;
    if (comId) {
      this.apiService.getall(`warehouse/getSelectWarehouseCodeList`).subscribe(res => {
        if (res)
          this.warehouseList = res;
        this.warehouseList.forEach(e => {
          e.lable = e.value + "-" + e.text;
        });



      });
    }
  }

  resetItem() {
   
    this.item = '';
    this.itemCode = '';
    this.itemId = '';
    this.unitType = '';
    this.description = '';
    this.unitPrice = 0;
    this.total = 0;
    this.discount = 0;
    this.discountAmount = 0;
    this.vat = 0;
    this.quantity = 0;
    this.itemInvAmount = 0;
  }

  loadItemdata(event: any) {
    
   
    let itemId = event != null ? event.value : '';
    if (itemId!='') {
      this.resetItem();
      this.quantity = 1;

      this.apiService.getall(`item/itemUnitPriceItem/${itemId}`).subscribe(res => {
        if (res) {
          this.itemId = itemId.toString();
          this.itemCode = res.itemCode;
          this.item = res.nameEN;
       
          this.description = res.description;
          this.unitPrice = res.unitPrice;
          this.vat = res.vat;
          this.apiService.getall(`item/getSelectItemMOUUnitTypeListByItem/${res.itemCode}`).subscribe((res2: Array<any>) => {
            if (res2) {
              if (res2.findIndex(e => e.value == res.unitType) >=0) {
                this.unitTypesList = res2;
                this.unitType = res.unitType;
                this.calculate();
              }
              else {
                this.resetItem();
                this.notifyService.showInfo("No Base Unit Mapping Found:"+res.itemCode+"x"+res.unitType);
              }
            }
            else {
              this.resetItem();
            }
          });

          


         // this.loadUnitTypes();

   

        }
        //this.branchList = res;
      });
    }
    else {
       this.resetItem();
    }
  }

  addUpdateInvoice() {
    if (this.isLoading)
      return;

    if (this.total> 0 && parseInt(this.itemId) > 0) {
      if (this.editsequence > 0) {
        // this.removeInvoiceList(this.editsequence);
        //let index: number = this.listOfInvoices.findIndex(a => a.sequence === this.editsequence);
        //this.listOfInvoices.splice(index, 1);


        var index: number = this.listOfInvoices.findIndex(a => a.sequence === this.editsequence);

        let pItem = this.listOfInvoices[index];
        pItem.sequence = this.editsequence;
        pItem.item = this.item;
        pItem.itemId = this.itemId;
        pItem.itemCode = this.itemCode;
        pItem.description = this.description;
        pItem.quantity = this.quantity;
        pItem.unitType = this.unitType;
        pItem.unitPrice = this.unitPrice;
        pItem.taxTariffPercentage = this.vat;
        pItem.taxAmount = this.vatAmount;
        pItem.totalAmount = this.total;
        pItem.discount = this.discount;
        pItem.discountAmount = this.discountAmount;
        pItem.footerDiscountAmount = this.footerDiscountAmount;


        pItem.itemInvAmount = this.itemInvAmount;
        pItem.itemBarcode = this.itemBarcode;
        this.editsequence = 0;
      }
      else {
        var index: number = this.listOfInvoices.findIndex(a => a.itemCode == this.itemCode && a.unitType == this.unitType);

        if (index>=0) {
          let pItem = this.listOfInvoices[index];
          pItem.sequence = this.editsequence;
          pItem.item = this.item;
          pItem.itemId = this.itemId;
          pItem.itemCode = this.itemCode;
          pItem.description = this.description;
          pItem.quantity = this.quantity;
          pItem.unitType = this.unitType;
          pItem.unitPrice = this.unitPrice;
          pItem.taxTariffPercentage = this.vat;
          pItem.taxAmount = this.vatAmount;
          pItem.totalAmount = this.total;
          pItem.discount = this.discount;
          pItem.discountAmount = this.discountAmount;
          pItem.footerDiscountAmount = this.footerDiscountAmount;


          pItem.itemInvAmount = this.itemInvAmount;
          pItem.itemBarcode = this.itemBarcode;
          this.editsequence = 0;
        }
        else {
          this.listOfInvoices.push({
            sequence: this.getSequence(),
            item: this.item, itemId: this.itemId, itemCode: this.itemCode, description: this.description, quantity: this.quantity,
            unitType: this.unitType, unitPrice: this.unitPrice,
            taxTariffPercentage: this.vat, taxAmount: this.vatAmount, totalAmount: this.total,
            discount: this.discount, discountAmount: this.discountAmount, footerDiscountAmount: this.footerDiscountAmount,
            itemInvAmount: this.itemInvAmount,
           itemBarcode : this.itemBarcode
          });


        }


        



      }
      //this.setLabelPrices(this.total, this.vatAmount, '');
      this.setGrandTotal();
      this.setToDefault();

      this.canSave = true;
    }
  }

  addInvoice() {
      if (this.editsequence > 0) {
        // this.removeInvoiceList(this.editsequence);
        //let index: number = this.listOfInvoices.findIndex(a => a.sequence === this.editsequence);
        //this.listOfInvoices.splice(index, 1);


        var index: number = this.listOfInvoices.findIndex(a => a.sequence === this.editsequence);

        let pItem = this.listOfInvoices[index];
        pItem.sequence = this.editsequence;
        pItem.item = this.item;
        pItem.itemId = this.itemId;
        pItem.itemCode = this.itemCode;
        pItem.description = this.description;
        pItem.quantity = this.quantity;
        pItem.unitType = this.unitType;
        pItem.unitPrice = this.unitPrice;
        pItem.taxTariffPercentage = this.vat;
        pItem.taxAmount = this.vatAmount;
        pItem.totalAmount = this.total;
        pItem.discount = this.discount;
        pItem.discountAmount = this.discountAmount ?? 0;
        pItem.footerDiscountAmount = this.footerDiscountAmount;
        pItem.itemInvAmount = this.itemInvAmount;
        pItem.itemBarcode = this.itemBarcode;

        this.editsequence = 0;
      }
      else {

        this.listOfInvoices.push({
          sequence: this.getSequence(),
          item: this.item, itemId: this.itemId, itemCode: this.itemCode, description: this.description, quantity: this.quantity,
          unitType: this.unitType, unitPrice: this.unitPrice,
          taxTariffPercentage: this.vat, taxAmount: this.vatAmount, totalAmount: this.total,
          discount: this.discount, discountAmount: this.discountAmount, footerDiscountAmount: this.footerDiscountAmount,


          itemInvAmount: this.itemInvAmount,
         itemBarcode : this.itemBarcode,
        });
      }
      //this.setLabelPrices(this.total, this.vatAmount, '');
      this.setGrandTotal();
      this.setToDefault();
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

    this.unitTypesList = [];
    this.unitType = '';
    
    this.apiService.getall(`item/getSelectItemMOUUnitTypeListByItem/${item.itemCode}`).subscribe(res2 => {
      if (res2) {
        this.unitTypesList = res2;
        this.unitType = item.unitType;
     }
      else {
        this.resetItem();
      }
    });




    this.editsequence = item.sequence,
      this.item = item.item;
    this.itemCode = item.itemCode;
    this.itemId = item.itemId;
    this.description = item.description;
    this.quantity = item.quantity;
    this.unitType = item.unitType;
    this.unitPrice = item.unitPrice;
    this.vat = item.taxTariffPercentage;
    this.vatAmount = item.taxAmount;
    this.total = item.totalAmount;
    this.discount = item.discount;
    this.discountAmount = item.discountAmount;
    this.footerDiscountAmount = item.footerDiscountAmount;
    this.itemInvAmount = item.itemInvAmount;
    this.itemBarcode = item?.itemBarcode;
  }

  setGrandTotal() {
    this.grandTotal = 0;
    this.grandVatTotal = 0;
    this.grandInvoiceTotal = 0;
    this.grandDiscountAmount = 0;
    this.grandFooterDiscountAmount = 0;

    this.listOfInvoices.forEach(inv => {
      this.grandTotal += inv.totalAmount;
      this.grandVatTotal += inv.taxAmount;
      this.grandInvoiceTotal += (inv.itemInvAmount);
      this.grandDiscountAmount += inv.discountAmount;
      this.grandFooterDiscountAmount += inv.footerDiscountAmount;
    });

   
    this.form.value['discountAmount'] = this.grandDiscountAmount;

    this.form.value['taxAmount'] = this.grandVatTotal;
    this.form.value['subTotal'] = this.grandTotal;

    this.grandTotalStr = this.grandTotal.toString();
    this.grandVatTotalStr = this.grandVatTotal.toString();
    this.grandInvoiceTotalStr = this.grandInvoiceTotal.toString();
  }



  setToDefault() {
    this.itemId = '';
    this.item = this.description = this.unitType = '';
    this.quantity = this.unitPrice = this.vat = this.vatAmount = this.total = this.discount = this.discountAmount = this.itemInvAmount = 0;
    this.itemBarcode = '';
    this.itemInvAmount = 0;
  }

  calculate() {



    if (this.discount > 100) {
      this.setToDefault();
    }


    this.total = this.unitPrice * this.quantity;

    this.discountAmount = parseFloat(((this.total * this.discount) / 100).toFixed(2));

    this.footerDiscountAmount = ((this.total - this.discountAmount) * this.footerDiscount) / 100;





    this.vatAmount = parseFloat((((this.total - this.discountAmount - this.footerDiscountAmount) * this.vat) / 100).toFixed(2));

    this.itemInvAmount = this.total - this.discountAmount - this.footerDiscountAmount + this.vatAmount;

    // this.setLabelPrices(this.total, this.vatAmount, '');
  }


  submit() {
  }
  Save(type:number) {

    this.calculate();

    //this.invoiceItemObject = {
    //  item: {},
    //  itemList: this.listOfInvoices
    //}; 
    this.form.value['invoiceStatusId'] = this.invoiceStatusId as number;

    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;


      this.form.value['itemList'] = this.listOfInvoices;

      this.form.value['invoiceDate'] = this.utilService.selectedDateTime(this.form.controls['invoiceDate'].value);
      this.form.value['invoiceDueDate'] = this.utilService.selectedDateTime(this.form.controls['invoiceDueDate'].value);


      this.form.value['footerDiscount'] = this.footerDiscount;
      this.form.value['totalAmount'] = this.grandInvoiceTotal;





      this.form.value['saveType'] = type;          //Dto is: EnumSaveType    0-Save, 1-SaveAndApprove, 2-SaveAndSettle



      if (type == 0 || type == 1) {
        this.apiService.post('generateSndInvoice', this.form.value)
          .subscribe(res => {
            this.utilService.OkMessage();
            this.reset();
            this.dialogRef.close(true);
          },
            error => {
              console.error(error);
              this.utilService.ShowApiErrorMessage(error);
            });

      }
        
      else if (type == 2) {


        this.form.value['dueDays'] =  this.paymentTermsList.find(e => e.value == this.form.controls['paymentTermId'].value)?.textTwo;
        this.form.value['totalAmount'] = this.grandInvoiceTotal;
        this.form.value['isApproved'] = false;

        this.openDialogManageApproval(this.form.value, this.id, DBOperation.create, '', '', SndsettleinvoiceComponent, { action: '', module: '' }, 75);

      }

    }
    else
      this.utilService.FillUpFields();

  }






  reset() {
    this.form.reset();
  }
  closeModel() {
    this.dialogRef.close();
  }
  changeFooterDiscount() {

    if (this.footerDiscount < 0 || this.footerDiscount > 100 || this.footerDiscount == null || this.footerDiscount.toString() == '') {
      this.footerDiscount = 0;
    }
    this.footerDiscount = parseInt(this.footerDiscount.toString());

    for (let i = 0; i < this.listOfInvoices.length; i++) {

      this.listOfInvoices[i].footerDiscountAmount = (this.listOfInvoices[i].totalAmount - this.listOfInvoices[i].discountAmount) * this.footerDiscount / 100;
      this.listOfInvoices[i].taxAmount = (this.listOfInvoices[i].totalAmount - this.listOfInvoices[i].discountAmount - this.listOfInvoices[i].footerDiscountAmount) * this.listOfInvoices[i].taxTariffPercentage / 100;
      this.listOfInvoices[i].itemInvAmount = this.listOfInvoices[i].totalAmount - this.listOfInvoices[i].discountAmount - this.listOfInvoices[i].footerDiscountAmount + this.listOfInvoices[i].taxAmount;

    }

    // this.calculate();
    this.setGrandTotal();

    this.canSave = true;

  }


  changeUnitType() {

   
    if (this.unitType!='') {

      this.apiService.getall(`item/itemUnitPriceItemUnit/${this.itemCode}/${this.unitType}`).subscribe(res => {
        if (res) {
          // this.itemId = itemId.toString();
          //  this.itemCode = res.itemCode;


          //  this.item = res.nameEN;
          //this.unitType = res.unitTypeEN;
          // this.description = res.description;
          this.unitPrice = res.unitPrice;

        }
        else {
          this.notifyService.showWarning("Item Unit Mapping Not Found");
          this.resetItem();
        }
          //this.branchList = res;
        });
      }
      else {
             this.resetItem();
           }

  }

  loadUnitTypes()
  {
    this.apiService.getall(`item/getSelectItemMOUUnitTypeListByItem/${this.itemCode}`).subscribe(res => {
      if (res)
        this.unitTypesList = res;
    });
  }


  loadBranch() {
    if (this.form.controls['warehouseCode'].value != '') {

      this.apiService.getall(`Warehouse/getWarehouseInfoByCode/${this.form.controls['warehouseCode'].value}`).subscribe(res => {
        if (res !=null)
        
        this.checkUserAuthority(res['whBranchCode']);
      });
    }
    else {
      this.canCreate = false;
      this.canSaveAndApprove = false;
      this.canSaveAndSettle = false;
    
    }
 
  }
  private openDialogManageApproval<T>(row: any, id:number, dbops: DBOperation, trantype: string = '', modalBtnTitle: string = '', component: T, moduleFile: MultiFileUploadDto = { module: '00', action: '00act' }, width: number = 100) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, component, width);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).trantype = trantype;
    (dialogRef.componentInstance as any).id = id;
    (dialogRef.componentInstance as any).inputData = row;
    (dialogRef.componentInstance as any).moduleFile = moduleFile;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true) {
        this.dialogRef.close(true);
      }
    });
  }


  SaveAndSettle() {
    this.form.value['dueDays'] = this.paymentTermsList.find(e => e.value == this.form.controls['paymentTermId'].value)?.textTwo;
    this.form.value['totalAmount'] = this.grandInvoiceTotal;
    this.form.value['isApproved'] = false;

    this.openDialogManageApproval(this.form.value, this.id, DBOperation.create, '', '', SndsettleinvoiceComponent, { action: '', module: '' }, 75);

  }

  onChangeItemCodeInput() {
    if (!this.isBarcodeScannerOn) {
      this.itemBarcode = '';
    }
    this.resetItem();
  }

  loadItemdataByBarcode(event: any) {
    if (event.target.value == '' || this.isLoading) {
      if (event.target.value == '')
      {
        this.resetItem();
      }
      return;
    }


    this.apiService.getall(`item/getSelctedItemByItemBarcode/${event.target.value}`).subscribe(res => {
      if (res) {
       this.itemId = res.value;
        let t: number = 0;
        this.isLoading = true;
        this.interval = 1000;
        this.interval = setInterval(() => {
          t++;
          if (t == 1) {
            this.loadItemdata(res);
          }
          else if (t == 2) {
            this.itemCode = res.text;
            this.itemId = res.value;
            this.unitType = res.textTwo;

            this.changeUnitType();
            this.isLoading = false;
          }
      },t>2?0:500);
       
      }
      else {
        this.resetItem();
        this.notifyService.showWarning("Item Not Found");
      }
    }, error => {
      this.notifyService.showWarning("error");
    });

  }




}
