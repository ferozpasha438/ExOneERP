import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { CustomSelectListItem, LanCustomSelectListItem } from '../../../../models/MenuItemListDto';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { UtilityService } from '../../../../services/utility.service';
import { ParentSystemSetupComponent } from '../../../../sharedcomponent/parentsystemsetup.component';
import { ValidationService } from '../../../../sharedcomponent/ValidationService';
import { PostingadvancepaymentComponent } from '../postingadvancepayment/postingadvancepayment.component';

@Component({
  selector: 'app-addupdateadvancepayment-component',
  templateUrl: './addupdateadvancepayment.component.html',
  styles: [
  ]
})
export class AddupdateadvancepaymentComponent implements OnInit {
  form!: FormGroup;
  id: number = 0;
  IsEnableInvoice: boolean = false;
  toBePaidAmount: string = '';

  invoiceNumberList: Array<CustomSelectListItem> = [];
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

  finsetup: any = { minCutOffShortAmt: 0, maxCutOffOverAmr: 0 };

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService,
    public dialogRef: MatDialogRef<AddupdateadvancepaymentComponent>, public dialog: MatDialog,
    private notifyService: NotificationService, private validationService: ValidationService) {
  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.setForm();
    this.loadData();
    this.loadFinSetupData();

    //if (this.id > 0)
    //  this.setEditForm();

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
     // 'payType': ['', Validators.required],
      'advAmount': ['0', Validators.required],
      'invoiceNumber': [],
      //'amount': ['', Validators.compose([Validators.required])],// this.validationService.numberValidator])],
      // 'voucherNumber': ['', Validators.compose([Validators.required])],// this.validationService.numberValidator])],
      //'narration': ['', Validators.required],
      'preparedby': this.authService.getUserName(),
      //'docNum': ['', Validators.required],      
      'remarks': [''],
      'notes': [''],
      'docNum': [''],
      'checkNumber': [''],
      'checkDate': [null],
      //'': ['', Validators.required],
      //'': ['', Validators.required],

    });
  }


  enableInvoice(event: MatSlideToggleChange) {
    this.IsEnableInvoice = event.checked;
    this.form.controls['invoiceNumber'].setValue(null);
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
        this.finsetup = { minCutOffShortAmt: res.minCutOffShortAmt, maxCutOffOverAmr: res.maxCutOffOverAmr };
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
          i.text = !this.isArab ? i.text : i.textAr;
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

  setCheckNumberInputDefault() {
    this.form.controls['checkNumber'].setValue('');
    this.form.controls['checkDate'].setValue(null);

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
    this.form.controls['invoiceNumber'].setValue(null);

    if (custCode) {
      this.customerCode = custCode;
      this.selectedInvoiceIDs = [];
      this.selectedInvoiceItems = [];
      this.totalAmount = 0;

      this.apiService.getall(`generateInvoice/getCustomerInvoiceNumberList/${custId}`).subscribe(res => {
        if (res) {
          this.invoiceNumberList = res;
        }
      });


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

  loadBranchs(event: any) {
    let comId = event.target.value;
    if (comId) {
      this.apiService.getall(`branch/getSelectSysBranchListByComId/${comId}`).subscribe(res => {
        if (res)
          this.branchList = res;
      });
    }
  }

  submit() {
    if (this.form.valid) {
      this.form.value['tranDate'] = this.utilService.selectedDateTime(this.form.controls['tranDate'].value);
      this.apiService.post('opmArPayment/createOpmCustomerAdvancePayment', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.reset();
          this.dialogRef.close(true);
        },
          error => {
            this.utilService.ShowApiErrorMessage(error);
          });
    }
    else
      this.utilService.FillUpFields();
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


}

