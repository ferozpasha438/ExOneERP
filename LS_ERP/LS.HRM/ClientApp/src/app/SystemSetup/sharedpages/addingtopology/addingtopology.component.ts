import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { CustomerSitesComponent } from '../../../Operationalmgt/customer-sites/customer-sites.component';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';

@Component({
  selector: 'app-addingtopology',
  templateUrl: './addingtopology.component.html',
  styles: [
  ]
})
export class AddingtopologyComponent implements OnInit {

  form: FormGroup;
  segmentList: Array<CustomSelectListItem> = [{ text: '5', value: '5' }];//, { text: '2', value: '2' }, { text: '3', value: '3' }]

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddingtopologyComponent>,
    private notifyService: NotificationService) { }

  ngOnInit(): void {
    this.setForm();
    this.apiService.getall('financialsetup/getAcCodeSegment')
      .subscribe(data => {
        console.log(data);
      });
  }

  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      'codeType1': ['', Validators.required],
      'segment1': ['', Validators.required],
     // 'codeType2': ['', Validators.required],
    //  'segment2': ['', Validators.required],
      //'codeType3': ['', Validators.required],
      //'segment3': ['', Validators.required],
    });
  }

  submit() {
    if (this.form.valid) {
      this.apiService.post('financialsetup/accountCodeTopology', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.reset();
          this.dialogRef.close(true);
        },
          error => {
            this.utilService.ShowApiErrorMessage(error);
          });
    }
    else
      this.utilService.FillUpFields();
  }

  closeModel() {
    this.dialogRef.close();
  }
  reset() {
    this.form.controls['codeType1'].setValue('');
    this.form.controls['segment1'].setValue('');
   // this.form.controls['codeType2'].setValue('');
   // this.form.controls['segment2'].setValue('');
   // this.form.controls['codeType3'].setValue('');
   // this.form.controls['segment3'].setValue('');
  }

}
