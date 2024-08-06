import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../../models/MenuItemListDto';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { UtilityService } from '../../../../services/utility.service';

@Component({
  selector: 'app-addupdateunittype',
  templateUrl: './addupdateunittype.component.html',
  styles: [
  ]
})
export class AddupdateunittypeComponent implements OnInit {

  id: number = 0;
  row: any;

  form: FormGroup;
  selectedPayment: string = '';
  paymentTermId: string = '';
  paymentTermsList: Array<CustomSelectListItem> = [];

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateunittypeComponent>,
    private notifyService: NotificationService) {

  }

  ngOnInit(): void {
    // this.loadData()
    this.setForm();
    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.form.patchValue(this.row);
    }
  }

  setForm() {
    this.form = this.fb.group({
      'nameEN': ['', Validators.required],
      'nameAR': ['', Validators.required]
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

      if (this.id > 0)
        this.form.value['id'] = this.id;

      this.apiService.post('unitType', this.form.value)
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
    this.selectedPayment = this.paymentTermId = '';
  }
  closeModel() {
    this.dialogRef.close();
  }


}
