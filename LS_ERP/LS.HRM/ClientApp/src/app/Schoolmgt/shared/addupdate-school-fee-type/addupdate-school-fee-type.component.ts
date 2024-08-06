import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';

@Component({
  selector: 'app-addupdate-school-fee-type',
  templateUrl: './addupdate-school-fee-type.component.html',
  styleUrls: []
})
export class AddupdateSchoolFeeTypeComponent implements OnInit {
  id: number = 0;
  row: any;

  form!: FormGroup;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateSchoolFeeTypeComponent>,
    private notifyService: NotificationService) {

  }

  ngOnInit(): void {
    this.form = this.fb.group({
      'feeCode': ['', Validators.required],
      'feesName': ['', Validators.required],
      'feeName2': ['', Validators.required],
      'frequency': ['', Validators.required],
      'estFeeAmount': ['', Validators.required],
      'minFeeAmount': ['', Validators.required],
      'maxFeeAmount': ['', Validators.required],
      'isDiscountable': ['', Validators.required],
      'maxDiscPer': ['', Validators.required],
      'taxApplicable': ['', Validators.required],
      'taxCode': ['', Validators.required],
      'remarks': ['', Validators.required],
      'feeGLAccount': ['', Validators.required],
      'feeTaxAccount': ['', Validators.required],
      'isActive': [false, Validators.required]
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


      this.apiService.post('schoolFeeType', this.form.value)
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
