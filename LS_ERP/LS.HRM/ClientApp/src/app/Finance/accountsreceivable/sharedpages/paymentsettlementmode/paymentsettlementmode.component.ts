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
  selector: 'app-paymentsettlementmode',
  templateUrl: './paymentsettlementmode.component.html',
  styles: [
  ]
})
export class PaymentsettlementmodeComponent implements OnInit {

  id: number = 0;
  trantype: string = 'Invoice';
  customerCode: string = '';
  modalBtnTitle: string = 'Invoice';

  form!: FormGroup;
  selectedPayment: string = '';
  paymentTermId: string = '';
  paymentTermsList: Array<CustomSelectListItem> = [];
  payCodeList: Array<CustomSelectListItem> = [];
  isCreditSelected: boolean = true;
  createdOn: any;

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<PaymentsettlementmodeComponent>,
    private notifyService: NotificationService) {

  }

  ngOnInit(): void {
  }


  submit() {
    let createdOn = this.createdOn;
    if (createdOn && this.customerCode.length > 0 && this.id > 0) {
      let formObj = { customerCode: this.customerCode, createdOn: this.utilService.selectedDateTime(createdOn), id: this.id };
      

      this.apiService.post('opmArPayment/customerPaymentApproval', formObj)
        .subscribe(res => {
          this.utilService.OkMessage();
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


  closeModel() {
    this.dialogRef.close();
  }


}
