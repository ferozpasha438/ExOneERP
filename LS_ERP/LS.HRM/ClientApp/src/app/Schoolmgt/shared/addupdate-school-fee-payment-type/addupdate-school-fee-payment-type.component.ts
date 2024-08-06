import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';

@Component({
  selector: 'app-addupdate-school-fee-payment-type',
  templateUrl: './addupdate-school-fee-payment-type.component.html',
  styleUrls: []
})
export class AddupdateSchoolFeePaymentTypeComponent implements OnInit {
 
  id: number = 0;
  row: any;

  form!: FormGroup;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateSchoolFeePaymentTypeComponent>,
    private notifyService: NotificationService) {

  }
  ngOnInit(): void {
    this.form = this.fb.group({
      'payCode': ['', Validators.required],
      'payName': ['', Validators.required],
      'paName2': ['', Validators.required],
      'branchCode': ['', Validators.required],
      'allowOtherBranchUse': ['', Validators.required],
      'glAccount': ['', Validators.required],
      'isActive': [false, Validators.required],
      'remarks': ['', Validators.required],
    });

    
    if (this.row) {
      //this.id = parseInt(this.row['id']);
      //this.form.patchValue(this.row);
    }
  } 

submit() {
  if (this.form.valid) {

    if (this.id > 0)
      this.form.value['id'] = this.id;

    this.apiService.post('studentPayType', this.form.value)
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
