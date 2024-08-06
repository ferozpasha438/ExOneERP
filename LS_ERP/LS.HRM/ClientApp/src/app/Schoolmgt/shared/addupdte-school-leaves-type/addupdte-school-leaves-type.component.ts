 
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';


@Component({
  selector: 'app-addupdte-school-leaves-type',
  templateUrl: './addupdte-school-leaves-type.component.html',
  styleUrls: []
})
export class AddupdteSchoolLeavesTypeComponent implements OnInit {
  id: number = 0;
  row: any;

  form!: FormGroup;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdteSchoolLeavesTypeComponent>,
    private notifyService: NotificationService) { }
  ngOnInit(): void {
    this.form = this.fb.group({
      'leaveCode': ['', Validators.required],
      'leaveName': ['', Validators.required],
      'leaveName2': ['', Validators.required],
      'maxLeavePerReq': ['', Validators.required],
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

      this.apiService.post('schoolStuLeaveType', this.form.value)
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
