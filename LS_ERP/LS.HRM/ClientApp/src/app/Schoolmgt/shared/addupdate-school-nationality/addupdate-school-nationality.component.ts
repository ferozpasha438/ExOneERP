import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';

@Component({
  selector: 'app-addupdate-school-nationality',
  templateUrl: './addupdate-school-nationality.component.html',
  styleUrls: []
})
export class AddupdateSchoolNationalityComponent implements OnInit {
  id: number = 0;
  row: any;
  form!: FormGroup;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateSchoolNationalityComponent>,
    private notifyService: NotificationService) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      'natCode': ['', Validators.required],
      'natName': ['', Validators.required],
      'natName2': ['', Validators.required],
      'isActive': [false]
    }); if (this.row) {
      this.id = parseInt(this.row['id']);
      this.form.patchValue(this.row);
    }
  }
  submit() {
    if (this.form.valid) {

      if (this.id > 0)
        this.form.value['id'] = this.id;

      this.apiService.post('schoolNational', this.form.value)
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
