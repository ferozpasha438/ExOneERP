import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../../models/MenuItemListDto';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { UtilityService } from '../../../../services/utility.service';


@Component({
  selector: 'app-userapproval',
  templateUrl: './userapproval.component.html',
  styles: [
  ]
})
export class ApUserapprovalComponent implements OnInit {
  id: number = 0;
  trantype: string = 'Invoice';

  form: FormGroup;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<ApUserapprovalComponent>,
    private notifyService: NotificationService) {

  }

  ngOnInit(): void {
    this.setForm();
  }
  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      // 'docNum': ['', Validators.required],
      'appRemarks': ['-', Validators.required],
      'trantype': [this.trantype],
      'tranSource': ['AP', Validators.required],

    });
  }

  submit() {
    if (this.form.valid) {
      if (this.id > 0) {
        this.form.value['id'] = this.id;

        this.apiService.post('generateApInvoice/createApInvoiceApproval', this.form.value)
          .subscribe(res => {
            this.utilService.OkMessage();
            this.reset();
            this.dialogRef.close(true);
          },
            error => {
              this.utilService.ShowApiErrorMessage(error);
            });
      }
    }
  }

  reset() {
    this.form.reset();
  }
  closeModel() {
    this.dialogRef.close();
  }

}
