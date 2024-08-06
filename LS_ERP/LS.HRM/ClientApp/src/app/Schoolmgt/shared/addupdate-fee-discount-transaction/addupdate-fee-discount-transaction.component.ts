import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
@Component({
  selector: 'app-addupdate-fee-discount-transaction',
  templateUrl: './addupdate-fee-discount-transaction.component.html',
  styleUrls: []
})
export class AddupdateFeeDiscountTransactionComponent implements OnInit {
  id: number = 0;
  row: any;

  form!: FormGroup;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateFeeDiscountTransactionComponent>,
    private notifyService: NotificationService) {

  }
  ngOnInit(): void {
    this.form = this.fb.group({
      'admissionNumber': ['', Validators.required],
      'admissionDate': ['', Validators.required],
      'nameOfStudent': ['', Validators.required],
      'nameOfStudentInArabic': ['', Validators.required],
      'branchCode': ['', Validators.required],
      'dateOfTransaction': ['', Validators.required],
      'feeTerm': ['', Validators.required],     
      'amountDue': ['', Validators.required],     
      'discountPercentage': ['', Validators.required],     
      'discountAmount': ['', Validators.required],     
      'remarks': ['', Validators.required],     
      'netFeeValue': ['', Validators.required],     
      'approvedRemarks': ['', Validators.required],     
      'approve': [false, Validators.required],
    });

    
    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.form.patchValue(this.row);
    }
  }
 


submit() {
  if (this.form.valid) {

    if (this.id > 0)
      this.form.value['id'] = this.id;      

    this.apiService.post('acedemicClassGrade', this.form.value)
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
