import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ApSettlementmodeComponent } from '../../../Finance/accountspayable/sharedpages/settlementmode/settlementmode.component';

@Component({
  selector: 'app-settlementmode',
  templateUrl: './settlementmode.component.html',
  styleUrls: []
})
export class SettlementmodeComponent implements OnInit {

  id: number = 0;
  trantype: string = 'Invoice';

  form: FormGroup;
  selectedPayment: string = '';
  paymentTermId: string = '';
  paymentTermsList: Array<CustomSelectListItem> = [];

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<ApSettlementmodeComponent>,
    private notifyService: NotificationService) {

  }

  ngOnInit(): void {
    // this.loadData()
    this.setForm();
  }

  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      // 'days': ['', Validators.required],
      // 'docNum': ['', Validators.required],
      'paymentType': ['', Validators.required],
      'trantype': [this.trantype],
      'tranSource': ['AP', Validators.required],
      'remarks1': '',
      'remarks2': '',

    });
  }


  //loadData() {
  //  this.apiService.getall("salesTermsCode/getSelectSalesTermsCodeList").subscribe(res => {
  //    if (res)
  //      this.paymentTermsList = res;
  //  });
  //}

  submit() {
    if (this.form.valid) {
      if (this.id > 0) {
        this.form.value['id'] = this.id;

        this.apiService.post('PurchaseInvoice/createApInvoiceSettlement', this.form.value)
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
  }


  reset() {
    this.selectedPayment = this.paymentTermId = '';
  }
  closeModel() {
    this.dialogRef.close();
  }


}
