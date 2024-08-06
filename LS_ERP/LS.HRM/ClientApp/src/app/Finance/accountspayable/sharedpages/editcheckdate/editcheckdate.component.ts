import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../../models/MenuItemListDto';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { UtilityService } from '../../../../services/utility.service';


@Component({
  selector: 'app-editcheckdate',
  templateUrl: './editcheckdate.component.html',
  styles: [
  ]
})
export class EditcheckdateComponent implements OnInit {
  id: number = 0;
  trantype: string = 'Invoice';
  payCodeList: Array<CustomSelectListItem> = [];
  form!: FormGroup;
  checkData: any;

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<EditcheckdateComponent>,
    private notifyService: NotificationService) {

  }

  ngOnInit(): void {
    this.setForm();
    this.loadData();

  }
  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      'branchCode': ['', Validators.required],
      'payCode': ['', Validators.required],
      'remarks': ['-', Validators.required],      
      'checkNumber': ['', Validators.required],
      'checkDate': [null, Validators.required],
    });
  }

  loadData() {    
    this.apiService.getall("paymentcodes/getSelectPaymentBankCodeList").subscribe(res => {
      if (res)
        this.payCodeList = res;
    });

    this.apiService.getall(`brsVoucher/getPDCCustVendPaymentItem/${this.id}`).subscribe(res => {
      if (res) {
        this.checkData = res;
        this.form.patchValue(res);
      }
    });


  }

  setCheckNumber(event: any) {
    let payCode = event.target.value;
    if (payCode !== '') {
      this.apiService.getall(`paymentcodes?payCode=${payCode}`).subscribe(res => {
        if (res) {
          this.form.controls['branchCode'].setValue(res['finBranchCode']);          
        }
      });
    }
  }


  submit() {
    if (this.form.valid) {
      if (this.id > 0) {
        this.form.value['id'] = this.id;

        this.apiService.post('brsVoucher/changeCheckDate', this.form.value)
          .subscribe(res => {
            this.utilService.OkMessage();
            this.reset();
            this.dialogRef.close(true);
          },
            error => {
              this.utilService.ShowApiErrorMessage(error);
            });
      }
    }
  }

  reset() {
    this.form.reset();
  }
  closeModel() {
    this.dialogRef.close();
  }

}
