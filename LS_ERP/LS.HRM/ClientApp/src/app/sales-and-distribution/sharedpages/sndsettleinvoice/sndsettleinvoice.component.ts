import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatRadioChange } from '@angular/material/radio';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ParentSalesMgtComponent } from '../../../sharedcomponent/parentsalesmgt.component';

@Component({
  selector: 'app-sndsettleinvoice',
  templateUrl: './sndsettleinvoice.component.html'
})
export class SndsettleinvoiceComponent extends ParentSalesMgtComponent implements OnInit {
  id: number=0;   //from input
  trantype: string = 'Invoice';
  modalBtnTitle: string = 'Invoice';
  inputData: any;
  form: FormGroup;
  selectedPayment: string = '';
  paymentTermId: string = '';
  paymentTermsList: Array<CustomSelectListItem> = [];
  paymentCodeList: Array<CustomSelectListItem> = [];
  isCreditSelected: boolean = true;


  paymentsList: Array<any> = [];

  settledAmount = 0;
  balanceAmount = 0;

  editPaymentData: any = { id: 0, invoiceId: 0, paymentCode: '', settledAmount: 0 ,remarks:''};
  editIndex = -1;

  canHaveCredit = false;//if payment days=0(based on paytermCode )
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<SndsettleinvoiceComponent>,
    private notifyService: NotificationService, private translate: TranslateService) {
    super(authService);
  }

  ngOnInit(): void {
    console.log(this.inputData);
   this.balanceAmount = this.inputData.totalAmount;
    this.settledAmount = 0;


    this.loadData();

    this.setForm();
  }

  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      // 'days': ['', Validators.required],
      // 'docNum': ['', Validators.required],
            'createdOn': '',
            'paymentType': '',
    });
  }


  loadData() {

    this.apiService.getall("paymentcodes/getSelectPaymentCodeList").subscribe(res => {
      if (res)
        this.paymentCodeList = res;
    });

    //this.apiService.getall("salesTermsCode/getSelectSalesTermsCodeList").subscribe(res => {
    //  if (res)
    //    this.paymentTermsList = res;
    //});
  }

  paymentSelected(event: MatRadioChange) {


    this.isCreditSelected = event.value == 'Credit' ? true : false;

    if (this.isCreditSelected) {
      this.paymentsList = [];
    }


    

  }

  submit() {

    if (this.form.valid) {


      if (this.form.controls['paymentType'].value == 'Cash') //  -->Cash/Credit
      {

        if (this.paymentsList.length == 0 || this.balanceAmount >= 0.01) {
          this.notifyService.showError("Incomplete_Settlement");
          return;
        }

      }

      else if (this.form.controls['paymentType'].value == '') {
        this.notifyService.showError("Select_PaymentType");
        return;
      }



      let SettlementData: any = {

        invoiceId: this.inputData.id,
        paymentType: this.form.controls['paymentType'].value,
        paymentsList: this.paymentsList.slice()

      };





      if (!this.inputData?.isApproved)    //save and settle invoice
      {

        this.inputData.saveType = 2;
        this.inputData.SettlementData = SettlementData;
        console.log(this.inputData)
        this.apiService.post('generateSndInvoice', this.inputData)
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
      else //settle invoice
      {

        this.apiService.post('SndInvoiceSettlements/CreateSndInvoicePaymentSettlement', SettlementData)
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
    }
    
    else {
      this.utilService.FillUpFields();
    }
}

      

  


  reset() {
    this.selectedPayment = this.paymentTermId = '';
  }
  closeModel() {
    this.dialogRef.close();
  }

  AddPaymentItem() {


    if (this.editPaymentData.paymentCode != '' && this.editPaymentData.settledAmount > 0 && this.editPaymentData.settledAmount <= (this.balanceAmount + 0.001)) {

      let index: number = this.paymentsList.findIndex(e => e.paymentCode == this.editPaymentData.paymentCode);
      if (index >= 0)
        this.paymentsList[index] = this.editPaymentData;
      else
        this.paymentsList.push(this.editPaymentData);
      this.ResetEditPaymentData();
      this.calculate();

    }
    else if (this.editPaymentData.settledAmount > this.balanceAmount) {
        this.notifyService.showWarning(this.translate.instant("Payments_Exceedig_invoice_Amount"));
      }
      else if (this.editPaymentData.paymentCode == '') {
        this.notifyService.showWarning(this.translate.instant("PaymentCode_Not_Selected"));

      }
      else if (this.editPaymentData.settledAmount <=0 || this.editPaymentData.settledAmount ==null) {
        this.notifyService.showWarning(this.translate.instant("Enter_The_Amount"));

      }

  }
  calculate() {
    this.settledAmount = 0;
    this.balanceAmount = this.inputData.totalAmount
    this.paymentsList.forEach(e => {
      this.settledAmount += e.settledAmount;
      this.balanceAmount -= e.settledAmount;
    });

  }
  ResetEditPaymentData() {
    this.editPaymentData = { id: 0, invoiceId: 0, paymentCode: '', settledAmount:0,remarks:''};

  }

  deletePayment(index: number) {
    this.paymentsList.splice(index, 1);
    this.calculate();
  }

  updatePaymentData(index: number) {
    this.editPaymentData = this.paymentsList[index];
    this.paymentsList.splice(index, 1);
    this.calculate();
  }
}
