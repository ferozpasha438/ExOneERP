import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ParentSalesMgtComponent } from '../../../sharedcomponent/parentsalesmgt.component';


@Component({
  selector: 'app-sndupdatingcustomer',
  templateUrl: './sndupdatingcustomer.component.html'
})
export class SndupdatingcustomerComponent extends ParentSalesMgtComponent implements OnInit {

  id: number = 0;
  data: any;
  form: FormGroup;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<SndupdatingcustomerComponent>,
    private notifyService: NotificationService) {
    super(authService);
  }

  ngOnInit(): void {
    this.setForm();
    if (this.data)
      this.form.patchValue(this.data);
  }
  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      // 'docNum': ['', Validators.required],
      'custName': ['', Validators.required],
      'custArbName': ['', Validators.required]
    });
  }

  submit() {
    if (this.form.valid) {
      this.dialogRef.close(this.form);
    }
    else
      this.utilService.FillUpFields();
  }

  reset() {
    this.form.reset();
  }
  closeModel() {
    this.dialogRef.close(null);
  }

}
