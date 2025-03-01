import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';

@Component({
  selector: 'app-addupdate-assign-driver',
  templateUrl: './addupdate-assign-driver.component.html',
  styleUrls: []  
})
export class AddupdateAssignDriverComponent implements OnInit {
  id: number = 0;
  row: any;
  driverList: Array<any> = [];
  vehicleNumberList: Array<any> = [];
  form!: FormGroup;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateAssignDriverComponent>,
    private notifyService: NotificationService) {

  }

  ngOnInit(): void {
    this.form = this.fb.group({
      'vehicleNumber': ['', Validators.required],
      'driverName': ['', Validators.required],
      'assignDate': ['', Validators.required],
      'notesForDriver': ['', Validators.required],
      'remarks': ['', Validators.required],
      'isActive': [true, Validators.required]
    });


    this.loadData();
    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.form.patchValue(this.row);
    }
  }

  
 
  submit() {

    if (this.form.valid) {

      if (this.id > 0)
        this.form.value['id'] = this.id;


      this.apiService.post('assignDriver', this.form.value)
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

  loadData() {
    this.apiService.getPagination('driver', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
        if (res)
          this.driverList = res['items'];
    });

    this.apiService.getPagination('vehicleMaster', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res)
        this.vehicleNumberList = res['items'];
    })

  }


  reset() {
    this.form.reset();
  }


  closeModel() {
    this.dialogRef.close();
  }

}
