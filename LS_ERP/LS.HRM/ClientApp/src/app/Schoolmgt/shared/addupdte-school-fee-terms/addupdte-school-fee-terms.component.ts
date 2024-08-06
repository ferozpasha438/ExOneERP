import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';

@Component({
  selector: 'app-addupdte-school-fee-terms',
  templateUrl: './addupdte-school-fee-terms.component.html',
  styleUrls: []
})
export class AddupdteSchoolFeeTermsComponent implements OnInit {
  id: number = 0;
  row: any;
  form!: FormGroup;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdteSchoolFeeTermsComponent>,
    private notifyService: NotificationService  ) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      'termCode': ['', Validators.required],
      'termName': ['', Validators.required],
      'termName2': ['', Validators.required],
      'feeDueDate': ['', Validators.required],
      'termStartDate': ['', Validators.required],
      'termEndDate': ['', Validators.required],
      'remarks': ['', Validators.required],

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

      this.form.value['feeDueDate'] = this.utilService.selectedDate(this.form.controls['feeDueDate'].value);
      this.form.value['termStartDate'] = this.utilService.selectedDate(this.form.controls['termStartDate'].value);
      this.form.value['termEndDate'] = this.utilService.selectedDate(this.form.controls['termEndDate'].value);

      this.apiService.post('schoolFeeTerms', this.form.value)
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
