import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { CustomSelectListItem, LanCustomSelectListItem } from '../../../../models/MenuItemListDto';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { UtilityService } from '../../../../services/utility.service';
import { ParentSystemSetupComponent } from '../../../../sharedcomponent/parentsystemsetup.component';
import { ValidationService } from '../../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-addupdatecustomerpayment',
  templateUrl: './addupdatecustomerpayment.component.html',
  styles: [
  ]
})
export class AddupdatecustomerpaymentComponent implements OnInit {
  form: FormGroup;
  id: number = 0;

  toBePaidAmount: string = '';
  customerSiteList: Array<LanCustomSelectListItem> = [];
  customerList: Array<LanCustomSelectListItem> = [];
  companyList: Array<CustomSelectListItem> = [];
  payCodeList: Array<CustomSelectListItem> = [];
  branchList: Array<CustomSelectListItem> = [];
  payCodeTypeList: Array<CustomSelectListItem> = [];
  IsCheckSelected: boolean = false;
  payType: string = '';
  isArab: boolean = false;

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdatecustomerpaymentComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {
  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.setForm();
    this.loadData();
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
      'amount': ['', Validators.compose([Validators.required])],// this.validationService.numberValidator])],
     // 'voucherNumber': ['', Validators.compose([Validators.required])],// this.validationService.numberValidator])],
      'narration': ['', Validators.required],
      'preparedby': ['', Validators.required],
      //'docNum': ['', Validators.required],      
      'remarks': [''],
      'checkNumber': [''],
      'checkDate': [null],
      //'': ['', Validators.required],
      //'': ['', Validators.required],

    });
  }

  setEditForm() {
    this.apiService.get('opmArPayment/getOpmSingleItem', this.id).subscribe(res => {
      if (res) {
        //this.companyControl.setValue(res['companyName']);
        this.loadBranchsForCompany(res['companyId'] as number, res);
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


  loadData() {

    //this.productList = [{ text: 'product One', value: '1' }, { text: 'product Two', value: '2' }]

    this.apiService.getall("paymentcodes/getSelectPaymentCodeList").subscribe(res => {
      if (res)
        this.payCodeList = res;
    });
    this.apiService.getall("customer/getSelectLanCustomerList?isPayment=true").subscribe(res => {
      if (res)
        this.customerList = res;
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

  loadProductdata(event: any) {
    let custCode = event.textTwo;
    let custId = event.value;
    this.form.controls['siteCode'].setValue(null);

    if (custCode) {

      this.apiService.getall(`customerPayment/getCustomerToBePaidAmount?customerCode=${custCode}`).subscribe(res => {
        if (res) {
          let isPaid = res['isPaid'] as boolean;
          if (!isPaid)
            this.toBePaidAmount = `Amount to be Paid : <br> (Invoice Amount) = ${res['tobePaidAmount']} `;
           // this.toBePaidAmount = `Amount to be Paid : <br> (Invoice Amount) ${res['netAmount']} - [ (Paid Amount) ${res['paidAmount']} + (Advance Amount) ${res['appliedAmount']} ] = ${res['tobePaidAmount']} `;
          else
            this.toBePaidAmount = 'Amount to be Paid : 0';
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


    }
    else {
      this.toBePaidAmount = 'Amount to be Paid : 0';
    }
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

  submit() {
    if (this.form.valid) {

      if (this.id > 0)
        this.form.value['id'] = this.id;
      if (this.payType !== '1')
        delete this.form.value.checkdate;

      this.form.value['tranDate'] = this.utilService.selectedDate(this.form.controls['tranDate'].value);
      if (this.form.controls['checkDate'].value)
        this.form.value['checkDate'] = this.utilService.selectedDate(this.form.controls['checkDate'].value);

      this.apiService.post('customerPayment', this.form.value)
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
