import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';
import { ParentSystemSetupComponent } from '../../sharedcomponent/parentsystemsetup.component';

@Component({
  selector: 'app-salescontrol',
  templateUrl: './salescontrol.component.html',
  styles: [
  ]
})
export class SalescontrolComponent extends ParentSystemSetupComponent implements OnInit {
  form: FormGroup;

  hasSalesControl: boolean = false;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService,
    private notifyService: NotificationService) {
    super(authService);
  }

  ngOnInit(): void {
    this.autoGenerateCustCode();
    this.setForm();
  }
  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      'autoGenCustCode': [false, Validators.required],
      'prefixCatCode': [false, Validators.required],
      'newCustIndicator': ['', Validators.required],
      'custLength': ['', Validators.required],
      'categoryLength': ['', Validators.required],
    });
  }

  autoGenerateCustCode() {
    this.apiService.getall('salesConfig').subscribe(res => {
      if (res) {
        this.hasSalesControl = res['canEdit'];
        //this.hasSalesControl = res.id > 0;
        this.form.patchValue(res);       
      }
    });
  }

  submit() {

    if (this.form.valid) {
      this.apiService.post('salesConfig', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
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

}
