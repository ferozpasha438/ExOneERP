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
export class UserapprovalComponent implements OnInit {
  id: number = 0;
  trantype: string = 'Invoice';
  zatca: string = '';
  zatcaUrl: string = '';
  zatcaTitle: string = 'User_Approval_Page';
  zatcaField: string = 'User_Approval';
  isdisabled: boolean = false;

  form!: FormGroup;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<UserapprovalComponent>,
    private notifyService: NotificationService) {

  }

  ngOnInit(): void {
    this.setForm();
  }
  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      // 'docNum': ['', Validators.required],
      'appRemarks': [this.zatca ? '' : '-', Validators.required],
      'trantype': [this.trantype],
      'tranSource': ['AR', Validators.required],

    });
  }

  submit() {
    if (this.form.valid) {      
      if (this.zatca && this.zatca.length > 0) {
        if (!isNaN(this.form.value['appRemarks']) && (this.form.value['appRemarks'] as string).length == 6) {
          this.isdisabled = true;
          this.apiService.getall(`${this.zatcaUrl}${this.form.value['appRemarks']}`)
            .subscribe(res => {
              this.utilService.OkMessage();
              this.dialogRef.close(true);
              this.isdisabled = false;
            },
              error => {
                this.isdisabled = false;
                console.error(error);
                this.utilService.ShowApiErrorMessage(error);
              });
        }
        else
          this.notifyService.showError('OTP must be 6 digits');
      }
      else {
        if (this.id > 0) {
          this.isdisabled = true;
          this.form.value['id'] = this.id;

          this.apiService.post('generateInvoice/createInvoiceApproval', this.form.value)
            .subscribe(res => {
              this.isdisabled = false;
              this.utilService.OkMessage();
              this.reset();
              this.dialogRef.close(true);
            },
              error => {
                this.isdisabled = false;
                this.utilService.ShowApiErrorMessage(error);
              });
        }
      }
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
