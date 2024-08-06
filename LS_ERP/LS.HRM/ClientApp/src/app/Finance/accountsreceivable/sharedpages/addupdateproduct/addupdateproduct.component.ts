import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../../models/MenuItemListDto';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { UtilityService } from '../../../../services/utility.service';
import { ValidationService } from '../../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-addupdateproduct',
  templateUrl: './addupdateproduct.component.html',
  styles: [
  ]
})
export class AddupdateproductComponent implements OnInit {

  id: number = 0;
  row: any;

  form: FormGroup;
  selectedPayment: string = '';
  paymentTermId: string = '';
  unitTypeList: Array<any> = [];
  productTypeList: Array<any> = [];
  isArab: boolean = false;

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private validationService: ValidationService,
    public dialogRef: MatDialogRef<AddupdateproductComponent>,
    private notifyService: NotificationService) {

  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.setForm();
    this.loadData()
    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.form.patchValue(this.row);
    }
  }

  setForm() {
    this.form = this.fb.group({
      'nameEN': ['', Validators.required],
      'nameAR': ['', Validators.required],
      'productTypeId': ['', Validators.required],
      'unitTypeId': ['', Validators.required],
      'description': ['', Validators.required],
      'unitPrice': ['', Validators.compose([Validators.required, this.validationService.decimalValidator])],
      'costPrice': ['', Validators.compose([Validators.required, this.validationService.decimalValidator])],
    });
  }


  loadData() {
    this.apiService.getall("unitType/getForCompany").subscribe(res => {
      if (res)
        this.unitTypeList = res;
    });
    this.apiService.getall("productType/getForCompany").subscribe(res => {
      if (res)
        this.productTypeList = res;
    });
  }

  submit() {
    if (this.form.valid) {

      if (this.id > 0)
        this.form.value['id'] = this.id;

      this.apiService.post('product', this.form.value)
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

