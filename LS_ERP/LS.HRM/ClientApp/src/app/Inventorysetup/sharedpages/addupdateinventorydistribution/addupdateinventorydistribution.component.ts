import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';

@Component({
  selector: 'app-addupdate-inventory-distribution',
  templateUrl: './addupdateinventorydistribution.component.html',
})
export class AddupdateInventoryDistributionComponent implements OnInit {
  form!: FormGroup;
  id: number = 0;
  row: any;

  accountCodeList: Array<CustomSelectListItem> = [];
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateInventoryDistributionComponent>,
    private notifyService: NotificationService) {

  }

  ngOnInit(): void {
    this.form = this.fb.group({
      'invDistGroup': ['', Validators.required],
      'invAssetAc': ['', Validators.required],
      'invNonAssetAc': ['', Validators.required],
      'invCashPOAC': ['', Validators.required],
      'invCOGSAc': ['', Validators.required],
      'invAdjAc': ['', Validators.required],
      'invSalesAc': ['', Validators.required],
      'invInTransitAc': ['', Validators.required],
      'invDefaultAPAc': ['', Validators.required],
      'invCostCorAc': ['', Validators.required],
      'invWIPAc': ['', Validators.required],
      'invWriteOffAc': ['', Validators.required],

    });

    this.loadData();

    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.form.patchValue(this.row);
    }
  }

  loadData() {
    this.apiService.getall("accountscategory/getSelectMainAllAccountList").subscribe(res => {
      if (res)
        this.accountCodeList = res;
    });

  }


  submit() {
    if (this.form.valid) {

      if (this.id > 0)
        this.form.value['id'] = this.id;

      this.apiService.post('inventoryDistribution', this.form.value)
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
    this.form.reset();
  }

  closeModel() {
    this.dialogRef.close();
  }

}


