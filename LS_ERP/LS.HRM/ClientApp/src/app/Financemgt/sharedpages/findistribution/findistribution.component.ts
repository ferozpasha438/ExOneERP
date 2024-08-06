import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';

@Component({
  selector: 'app-findistribution',
  templateUrl: './findistribution.component.html',
  styles: [
  ]
})
export class FindistributionComponent implements OnInit {

  id: number = 0;
  finBranchCode: string = '';
  distribution: any;
  form: FormGroup;

  list1: Array<CustomSelectListItem> = [];
  list2: Array<CustomSelectListItem> = [];
  list3: Array<CustomSelectListItem> = [];
  list4: Array<CustomSelectListItem> = [];
  list5: Array<CustomSelectListItem> = [];
  list6: Array<CustomSelectListItem> = [];
  list7: Array<CustomSelectListItem> = [];
  list8: Array<CustomSelectListItem> = [];
  list9: Array<CustomSelectListItem> = [];
  list10: Array<CustomSelectListItem> = [];
  list11: Array<CustomSelectListItem> = [];

  constructor(private fb: FormBuilder, private apiService: ApiService, private utilService: UtilityService, private notifyService: NotificationService
    , public dialogRef: MatDialogRef<FindistributionComponent>) {
  }

  ngOnInit(): void {
    this.setForm();
    if (this.id > 0)
      this.getFinDistrobution();
  }

  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      // 'finBranchCode': ['', Validators.required],
      'finBranchCode': ['', Validators.required],
      'finBranchName': ['', Validators.required],      
      'inventoryAccount': ['', Validators.required],
      'cashPurchase': ['', Validators.required],
      'costofSalesAccount': ['', Validators.required],
      'inventoryAdjustment': ['', Validators.required],
      'defaultSalesAccount': ['', Validators.required],
      'defaultSalesReturn': ['', Validators.required],
      'inventoryTransfer': ['', Validators.required],
      'defaultPayable': ['', Validators.required],
      'costCorrection': ['', Validators.required],
      'wipUsageConsumption': ['', Validators.required],
      'reserved': ['', Validators.required],
    });
  }

  getFinDistrobution() {
    this.apiService.getall(`accountsbranches/getFinDistribution/${this.id}`).subscribe(res => {
      if (res) {
        this.distribution = res;
        this.form.patchValue({ 'finBranchCode': res['finBranchCode'], 'finBranchName': res['finBranchName'] });

        this.list1 = res['list1'];
        this.list2 = res['list2'];
        this.list3 = res['list3'];
        this.list4 = res['list4'];
        this.list5 = res['list5'];
        this.list6 = res['list6'];
        this.list7 = res['list7'];
        this.list8 = res['list8'];
        this.list9 = res['list9'];
        this.list10 = res['list10'];
        this.list11 = res['list11'];

      }
    });

    this.apiService.getall(`subAccountBranchMapping/getSubAccountBranchMapping?branchCode=${this.finBranchCode}`).subscribe(res => {
      if (res) {
        this.form.patchValue(res);
      }
    });

  }

  submit() {
    console.log(this.form.value);
    if (this.form.valid) {
      this.apiService.post('subAccountBranchMapping', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();

          this.dialogRef.close(true);
        },
          error => {
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
