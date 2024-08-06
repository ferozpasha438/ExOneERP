import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { CustomSelectListItem, LanCustomSelectListItem } from '../../../../models/MenuItemListDto';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { UtilityService } from '../../../../services/utility.service';
import { ParentSystemSetupComponent } from '../../../../sharedcomponent/parentsystemsetup.component';
import { ValidationService } from '../../../../sharedcomponent/ValidationService';
import { PostingadvancepaymentComponent } from '../postingadvancepayment/postingadvancepayment.component';

@Component({
  selector: 'app-addupdateopmcustomerpayment-component',
  templateUrl: './addupdateopmcustomerpayment-component.component.html',
  styles: [
  ]
})
export class AddupdateopmcustomerpaymentComponentComponent implements OnInit {
  form: FormGroup;
  id: number = 0;

  toBePaidAmount: string = '';
  customerSiteList: Array<LanCustomSelectListItem> = [];
  customerList: Array<LanCustomSelectListItem> = [];
  companyList: Array<CustomSelectListItem> = [];
  payCodeList: Array<CustomSelectListItem> = [];
  branchList: Array<CustomSelectListItem> = [];
  payCodeTypeList: Array<CustomSelectListItem> = [];
  invoiceItems: Array<any> = [];
  selectedInvoiceItems: Array<any> = [];
  selectedInvoiceIDs: Array<number> = [];
  selectedInvoiceIdList: Array<any> = [];

  IsCheckSelected: boolean = false;
  payType: string = '';
  customerCode: string = '';
  totalAmount: number = 0;
  totalAmountStr: string = '';
  isArab: boolean = false;
  isSiteSelected: boolean = false;

  finsetup: any = { minCutOffShortAmt: 0, maxCutOffOverAmr: 0, advAmount: 0 };

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService,
    public dialogRef: MatDialogRef<AddupdateopmcustomerpaymentComponentComponent>, public dialog: MatDialog,
    private notifyService: NotificationService, private validationService: ValidationService) {
  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.setForm();
    this.loadData();
    this.loadFinSetupData();

    if (this.id > 0)
      this.setEditForm();

  }
  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      'custCode': ['', Validators.required],
      'siteCode': [null],
      'companyId': ['', Validators.required],
      'branchCode': ['', Validators.required],
      'tranDate': ['', Validators.required],
      'payCode': ['', Validators.required],
      'payType': ['', Validators.required],
      'amount': [''],
      //'amount': ['', Validators.compose([Validators.required])],// this.validationService.numberValidator])],
      // 'voucherNumber': ['', Validators.compose([Validators.required])],// this.validationService.numberValidator])],
      //'narration': ['', Validators.required],
      'preparedby': this.authService.getUserName(),
      //'docNum': ['', Validators.required],      
      'remarks': [''],
      'checkNumber': [''],
      'checkDate': [null],
      'isPdcCleared': true,
     // 'pdcClearedDate': [null],
     // 'pdcClearedBy': this.authService.getUserName(),
      //'': ['', Validators.required],
      //'': ['', Validators.required],

    });
  }

  setEditForm() {
    this.apiService.get('opmArPayment/getOpmSingleItem', this.id).subscribe(res => {
      if (res) {
        const header = res['header'];
        const list = res['list'] as Array<any>;
        const invoielist = res['invoiceList'] as Array<any>;

        this.invoiceItems = [];
        this.selectedInvoiceItems = [];
        this.selectedInvoiceIDs = [];
        this.selectedInvoiceIDs = [];

        this.setEditTotalAmount();

        invoielist.forEach(item => {
          this.invoiceItems.push(item);
        });

        list.forEach(item => {
          this.selectedInvoiceItems.push(item);
          this.selectedInvoiceIDs.push(item.id);
          this.invoiceItems.push(item);
        });

        //this.companyControl.setValue(res['companyName']);
        this.loadBranchsForCompany(header['companyId'] as number, header);
      }
    })
  }

  loadBranchsForCompany(comId: number, result: any) {
    if (comId) {
      this.apiService.getall(`branch/getSelectSysBranchListByComId/${comId}`).subscribe(res => {
        if (res) {
          this.branchList = res;
          if (result['payType'] === '1')
            this.IsCheckSelected = true;

          this.form.patchValue(result);
          //this.form.patchValue({ 'custCode': `${result['custCode']}` });
        }
      });
    }
  }

  loadFinSetupData() {
    this.apiService.getall("financialsetup/getFinSetup").subscribe(res => {
      if (res) {
        this.finsetup = { minCutOffShortAmt: res.minCutOffShortAmt, maxCutOffOverAmr: res.maxCutOffOverAmr, advAmount: 0 };
      }
    });
  }

  loadData() {

    //this.productList = [{ text: 'product One', value: '1' }, { text: 'product Two', value: '2' }]

    this.apiService.getall("paymentcodes/getSelectPaymentCodeList").subscribe(res => {
      if (res)
        this.payCodeList = res;
    });
    this.apiService.getall("customer/getSelectLanCustomerList?isPayment=true").subscribe(res => {
      if (res) {
        this.customerList = res;
        const newList = this.customerList.map((i) => {
          i.text = !this.isArab ? '(' + i.textTwo + ') ' + i.text : '(' + i.textTwo + ') ' + i.textAr;
          return i;
        });
      }
    });

    this.apiService.getall("company/getSelectCompanyList").subscribe(res => {
      if (res)
        this.companyList = res;
    });
    this.apiService.getall("paymentcodes/getSelectPaymentTypeList").subscribe(res => {
      if (res)
        this.payCodeTypeList = res;
    });


  }


  setCheckNumber(event: any) {
    let payCode = event.target.value;
    if (payCode !== '') {
      this.apiService.getall(`paymentcodes?payCode=${payCode}`).subscribe(res => {
        if (res) {
          this.payType = res['finPayType']
          this.setCheckNumberInput(this.payType);

        }
      });
    }
    else
      this.setCheckNumberInput('');
  }


  setCheckNumberInput(payType: string) {

    if (payType !== '') {
      this.form.controls['payType'].setValue(payType);
      if (payType === '1') {
        this.payType = payType;
        this.IsCheckSelected = true;

        this.form.controls['checkNumber'].setValidators([Validators.required]);
        this.form.controls['checkDate'].setValidators([Validators.required]);
        //this.form.controls['isPdcCleared'].setValue(true);
        //this.form.controls['isPdcCleared'].enable({ onlySelf: true });
      }
      else {
        this.setCheckNumberInputDefault();
      }

      this.form.controls['checkNumber'].updateValueAndValidity();
      this.form.controls['checkDate'].updateValueAndValidity();
    }
    else {
      this.form.controls['payType'].setValue('');
      this.setCheckNumberInputDefault();
    }
  }

  //isPdcClearedChecked(evt: any) {

  receiptDateChanged(evt: MatDatepickerInputEvent<Date>) {

    //let checkDate = this.form.controls['checkDate'].value;
    //if (checkDate) {

    //  let receiptDate = this.utilService.selectedDateTime(evt.value).split('T')[0];
    //  let greceiptDate = receiptDate.replace(/-/g, '');

    //  let selectedcheckDate = this.utilService.selectedDateTime(checkDate);
    //  let gcheckDate = selectedcheckDate.split('T')[0].replace(/-/g, '');
    //  if (parseInt(gcheckDate) > parseInt(greceiptDate)) {
    //    this.form.controls['isPdcCleared'].setValue(false);
    //    //console.log(greceiptDate, gcheckdate);
    //  }
    //  else
    //    this.form.controls['isPdcCleared'].setValue(true);
    //}

  }

  checkdateChanged(evt: MatDatepickerInputEvent<Date>) {

    //let receiptDate = this.form.controls['tranDate'].value;
    //if (receiptDate) {

    //  let checkdate = this.utilService.selectedDateTime(evt.value).split('T')[0];
    //  let gcheckdate = checkdate.replace(/-/g, '');

    //  let selectedReceiptDate = this.utilService.selectedDateTime(receiptDate);
    //  let greceiptDate = selectedReceiptDate.split('T')[0].replace(/-/g, '');
    //  if (parseInt(gcheckdate) > parseInt(greceiptDate)) {
    //    this.form.controls['isPdcCleared'].setValue(false);
    //    //console.log(greceiptDate, gcheckdate);
    //  }
    //  else
    //    this.form.controls['isPdcCleared'].setValue(true);
    //}

  }

  setCheckNumberInputDefault() {
    this.form.controls['checkNumber'].setValue('');
    this.form.controls['checkDate'].setValue(null);
    //this.form.controls['isPdcCleared'].setValue(false);


    this.form.controls['checkNumber'].clearValidators();
    this.form.controls['checkDate'].clearValidators();

    this.IsCheckSelected = false;

  }
  resetSiteInfo() {
    this.isSiteSelected = false;
    this.loadCustomerInvoices(this.customerCode);
  }

  selectSiteInfo(event: any) {
    let siteCode = event.value;

    this.selectedInvoiceItems.forEach(item => {
      // this.deleteItem(item);

      item.isApproved = false;

      if (this.invoiceItems.findIndex(itm => itm.id == item.id) === -1)
        this.invoiceItems.push(item);

      this.selectedInvoiceIDs.splice(this.selectedInvoiceIDs.findIndex(itm => itm == item.id), 1);
      this.selectedInvoiceItems.splice(this.selectedInvoiceItems.findIndex(itm => itm.id == item.id), 1);
      this.setTotalAmount();

    });

    if (siteCode) {
      this.isSiteSelected = true;
      this.loadCustomerInvoices(this.customerCode, siteCode);
    }
    else {
      this.isSiteSelected = false;
      this.loadCustomerInvoices(this.customerCode);
    }
  }


  loadProductdata(event: any) {
    let custCode = event.textTwo;
    let custId = event.value;
    this.form.controls['siteCode'].setValue(null);
    this.isSiteSelected = false;
    if (custCode) {
      this.customerCode = custCode;
      this.selectedInvoiceIDs = [];
      this.selectedInvoiceItems = [];
      this.totalAmount = 0;

      this.setTotalAmount();

      this.loadCustomerInvoices(custCode);
      this.loadCustomerWalletPayment(custCode);


      this.apiService.getall(`customer/getCustomerSitesSelectList/${custId}`).subscribe(res => {
        if (res) {
          this.customerSiteList = res;

          this.customerSiteList.map((i) => {
            i.text = !this.isArab ? i.text : i.textTwo;
            return i;
          });
        }
      });



      //this.apiService.getall(`customerPayment/getCustomerToBePaidAmount?customerCode=${custCode}`).subscribe(res => {
      //  if (res) {
      //    let isPaid = res['isPaid'] as boolean;
      //    if (!isPaid)
      //      this.toBePaidAmount = `Amount to be Paid : <br> (Invoice Amount) = ${res['tobePaidAmount']} `;
      //    //this.toBePaidAmount = `Amount to be Paid : <br> (Invoice Amount) ${res['netAmount']} - [ (Paid Amount) ${res['paidAmount']} + (Advance Amount) ${res['appliedAmount']} ] = ${res['tobePaidAmount']} `;
      //    else
      //      this.toBePaidAmount = 'Amount to be Paid : 0';
      //  }
      //});


    }
    else {
      this.toBePaidAmount = 'Amount to be Paid : 0';
    }
  }

  loadCustomerInvoices(custCode: string, siteCode: string = '') {
    this.apiService.getall(`opmArPayment/getOpmArInvoiceList?customerCode=${custCode}&siteCode=${siteCode}`).subscribe(res => {
      if (res)
        this.invoiceItems = res;
    });
  }

  loadCustomerWalletPayment(custCode: string, siteCode: string = '') {
    this.apiService.getall(`opmArPayment/getOpmCustomerwallet?customerCode=${custCode}&siteCode=${siteCode}`).subscribe(res => {
      if (res)
        this.finsetup.advAmount = parseFloat(res['advAmount']);
    });
  }

  calculateTotal() {

    this.setTotalAmount();
  }

  checkAmount(evt: any, totalAmount: number, appliedAmount: number, id: number) {

    if (parseFloat(evt.target.value) > (totalAmount - appliedAmount)) {
      const input = document.getElementById('appliendAmount_' + id) as HTMLInputElement;
      input.value = '';
      this.setTotalAmount();
    }
    else
      this.setTotalAmount();
  }

  selectedItem(item: any) {
    item.isApproved = true;
    this.selectedInvoiceItems.push(item);
    this.selectedInvoiceIDs.push(item.id);

    setTimeout(() => {
      this.setTotalAmount();
    }, 200);
  }

  deleteItem(item: any) {
    item.isApproved = false;

    if (this.invoiceItems.findIndex(itm => itm.id == item.id) === -1)
      this.invoiceItems.push(item);

    this.selectedInvoiceIDs.splice(this.selectedInvoiceIDs.findIndex(itm => itm == item.id), 1);
    this.selectedInvoiceItems.splice(this.selectedInvoiceItems.findIndex(itm => itm.id == item.id), 1);
    this.setTotalAmount();
  }

  setTotalAmount() {

    this.totalAmount = 0;

    this.selectedInvoiceIDs.forEach(id => {
      const input = document.getElementById('appliendAmount_' + id) as HTMLInputElement;

      const appliendAmount = isNaN(parseFloat(input.value.trim())) ? 0 : parseFloat(input.value.trim());
      this.totalAmount += appliendAmount;
    });

    this.totalAmountStr = this.totalAmount.toFixed(2);
    this.form.controls['amount'].setValue(parseFloat(this.totalAmount.toFixed(2)));

    //this.totalAmount = 0;
    //this.selectedInvoiceItems.forEach(itm => {
    //  this.totalAmount += itm.totalAmount;
    //});
    //this.totalAmountStr = this.totalAmount.toFixed(2);
    //this.form.controls['amount'].setValue(parseFloat(this.totalAmount.toFixed(2)));


  }

  setEditTotalAmount() {
    this.totalAmount = 0;
    this.selectedInvoiceItems.forEach(itm => {
      this.totalAmount += itm.appliedAmount;
    });
    this.totalAmountStr = this.totalAmount.toFixed(2);
    this.form.controls['amount'].setValue(parseFloat(this.totalAmount.toFixed(2)));
  }

  //setCheckNumber(event: any) {
  //  let payType = event.target.value;
  //  if (payType === '1') {
  //    this.payType = payType;
  //    this.IsCheckSelected = true;
  //  }
  //  else {
  //    this.form.controls['checkNumber'].setValue('');
  //    this.form.controls['checkdate'].setValue('');
  //    this.IsCheckSelected = false;
  //  }
  //}

  loadBranchs(event: any) {
    let comId = event.target.value;
    if (comId) {
      this.apiService.getall(`branch/getSelectSysBranchListByComId/${comId}`).subscribe(res => {
        if (res)
          this.branchList = res;
      });
    }
  }

  calculateamountforinvoices(): Boolean {
    this.selectedInvoiceIdList = [];

    //this.selectedInvoiceIDs.forEach(id => {

    //  const input = document.getElementById('appliendAmount_' + id) as HTMLInputElement;
    //  this.selectedInvoiceIdList.push({ id: id, amount: parseFloat(input.value) + parseFloat(this.selectedInvoiceItems.find(e => e.id == id).appliedAmount) });
    //});

    //console.log(this.selectedInvoiceIdList);

    if (this.form.valid) {

      if (this.selectedInvoiceIDs.length <= 0) {
        this.notifyService.showError('Select Invoices');
        return false;
      }

      this.form.value['tranDate'] = this.utilService.selectedDate(this.form.controls['tranDate'].value);
      if (this.form.controls['checkDate'].value)
        this.form.value['checkDate'] = this.utilService.selectedDate(this.form.controls['checkDate'].value);

      this.selectedInvoiceIDs.forEach(id => {

        const input = document.getElementById('appliendAmount_' + id) as HTMLInputElement;
        const already = document.getElementById('already_' + id) as HTMLInputElement;
        this.selectedInvoiceIdList.push({ id: id, amount: input.value, appliedAmount: already.value });
      });

      const amount = parseFloat(this.form.controls['amount'].value);
      if (amount <= 0) {
        this.notifyService.showError('Amount should be > 0');
        return false;
      }

      if (amount > this.totalAmount) {
        this.notifyService.showError('Amount not more than InvoiceAmount');
        return false;
      }


      if (this.id > 0)
        this.form.value['id'] = this.id;
      if (this.payType !== '1')
        delete this.form.value.checkdate;

      this.form.value['inviceIds'] = this.selectedInvoiceIdList;
      return true;
    }
    else {
      this.utilService.FillUpFields();
      return false;
    }
  }

  submitCalculateamountforinvoices() {

    this.apiService.post('opmArPayment', this.form.value)
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

  submit() {

    if (this.calculateamountforinvoices())
      this.submitCalculateamountforinvoices();
  }

  reset() {
    this.form.controls['custCode'].setValue('');
    this.form.controls['branchCode'].setValue('');
    this.form.controls['companyId'].setValue('');
    // this.form.reset();
  }
  closeModel() {
    this.dialogRef.close();
  }


  //calcaulating Post And Advance Balance

  private openDialogManageApproval<T>(width: number = 100) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, PostingadvancepaymentComponent, width);
    const arAmount = parseFloat(this.totalAmount.toFixed(2));//this.form.controls['amount'].setValue(parseFloat(this.totalAmount.toFixed(2)));
    (dialogRef.componentInstance as any).data = { arAmount: arAmount, fin: this.finsetup };

    dialogRef.afterClosed().subscribe(res => {
      if (res.hasData == 0) {
        this.notifyService.showError('there is no Amount');
      }
      else if (res.hasData == 1) {
        if (res.amountType != 'advance') {
          this.form.value['advancePayment'] = res;
          this.submitCalculateamountforinvoices();
        }
        else
          this.notifyService.showError('No Advance Payment');

      }
    });
  }


  public postAdvancePay() {

    //this.notifyService.showSuccess("Coming Soon...");    

    if (this.calculateamountforinvoices()) {
      this.openDialogManageApproval(50);
      // alert('post advance')
    }

    //if (this.form.valid) {
    //  this.calculateamountforinvoices();
    //  this.openDialogManageApproval(50);
    //}
    //else
    //  this.utilService.FillUpFields();


  }


}

