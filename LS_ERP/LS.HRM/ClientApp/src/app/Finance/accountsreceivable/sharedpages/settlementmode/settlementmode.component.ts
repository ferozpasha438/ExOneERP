import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatRadioChange } from '@angular/material/radio';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../../models/MenuItemListDto';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { UtilityService } from '../../../../services/utility.service';


@Component({
  selector: 'app-settlementmode',
  templateUrl: './settlementmode.component.html',
  styles: [
  ]
})
export class SettlementmodeComponent implements OnInit {

  id: number = 0;
  trantype: string = 'Invoice';
  modalBtnTitle: string = 'Invoice';

  form!: FormGroup;
  selectedPayment: string = '';
  paymentTermId: string = '';
  paymentTermsList: Array<CustomSelectListItem> = [];
  payCodeList: Array<CustomSelectListItem> = [];
  isCreditSelected: boolean = true;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<SettlementmodeComponent>,
    private notifyService: NotificationService) {

  }

  ngOnInit(): void {
    this.loadData()
    this.setForm();
  }

  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      // 'days': ['', Validators.required],
      // 'docNum': ['', Validators.required],
      'paymentType': ['', Validators.required],
      'trantype': 'Invoice',
      'tranSource': ['AR', Validators.required],
      'remarks1': '-',
      'remarks2': '-',
      'payCode': '',
      'createdOn': '',
    });
  }


  loadData() {

    this.apiService.getall("paymentcodes/getSelectPaymentCodeList").subscribe(res => {
      if (res)
        this.payCodeList = res;
    });

    //this.apiService.getall("salesTermsCode/getSelectSalesTermsCodeList").subscribe(res => {
    //  if (res)
    //    this.paymentTermsList = res;
    //});
  }

  paymentSelected(event: MatRadioChange) {
    this.isCreditSelected = event.value == 'Credit' ? true : false;
  }

  submit() {
   
    if (this.form.valid) {
      if (this.id > 0) {
        this.form.value['id'] = this.id;

        if (!this.isCreditSelected) {
          const payCode: string = this.form.value['payCode'];
          if (!this.utilService.hasValue(payCode)) {
            this.notifyService.showError('Select PayCode');
            return;
          }
        }

        if (this.form.controls['createdOn'].value)
        this.form.value['createdOn'] = this.utilService.selectedDateTime(this.form.controls['createdOn'].value);

        this.apiService.post('generateInvoice/createSettlePayment', this.form.value)
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
    else
      this.utilService.FillUpFields();

  }


  reset() {
    this.selectedPayment = this.paymentTermId = '';
  }
  closeModel() {
    this.dialogRef.close();
  }


}
