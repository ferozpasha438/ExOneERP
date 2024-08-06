import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ParentSalesMgtComponent } from '../../../sharedcomponent/parentsalesmgt.component';

@Component({
  selector: 'app-snduserapproval',
  templateUrl: './snduserapproval.component.html'
})
export class SnduserapprovalComponent extends ParentSalesMgtComponent implements OnInit {
  id: number = 0;
  serviceType: number = 0;//SndInvoice=0,SndQuotation=1   from input  Default Asuming As Invoice Approval
  serviceCode:string= this.id.toString();
  inputData: any;
  form: FormGroup;

  isLoading: boolean = false;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<SnduserapprovalComponent>,
    private notifyService: NotificationService) {
    super(authService);

  }

  ngOnInit(): void {
    this.setForm();
  }
  setForm() {
    
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      // 'docNum': ['', Validators.required],
      'appRemarks': ['', Validators.required],
      'serviceType': [this.serviceType, Validators.required],    
      'serviceCode': [this.serviceCode, Validators.required],

    });
  }

  submit() {
    console.log(this.inputData);
    if (!this.isLoading) {
      if (this.form.valid) {

        this.form.value['id'] = this.id;
        console.log(this.form.value);

        this.isLoading = true;
        this.apiService.post('SndApproval/createSndApproval', this.form.value)
          .subscribe(res => {
            this.isLoading = false;

            this.utilService.OkMessage();
            this.reset();
            this.dialogRef.close(true);
          },
            error => {
              this.isLoading = false;
              this.utilService.ShowApiErrorMessage(error);
            });


      }
      else {
        this.notifyService.showWarning("Enter Remarks");
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
