import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../../models/MenuItemListDto';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { UtilityService } from '../../../../services/utility.service';

@Component({
  selector: 'app-bvposting',
  templateUrl: './posting.component.html',
  styles: [
  ]
})
export class BvPostingComponent implements OnInit {
  id: number = 0;
  modalTitle: string = ''
  form: FormGroup;
  selectedPayment: string = '';
  paymentTermId: string = '';
  paymentTermsList: Array<CustomSelectListItem> = [];

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<BvPostingComponent>,
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
      'paymentType': this.modalTitle,
      'trantype': ['Invoice', Validators.required],
      'tranSource': ['GL', Validators.required],
      'remarks1': '-',
      'remarks2': '-',
      'createdOn': '',
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

        if (this.form.controls['createdOn'].value)
          this.form.value['createdOn'] = this.utilService.selectedDateTime(this.form.controls['createdOn'].value);

        this.apiService.post('bankVoucher/createbankVoucherPosting', this.form.value)
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

