import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-accounttype',
  templateUrl: './accounttype.component.html',
  styles: [
  ],

})
export class AccounttypeComponent implements OnInit {
  accountTypeId: string = '';
  accountType: Array<any> = [];
  form: FormGroup;
  constructor(private fb: FormBuilder, private apiService: ApiService, private utilService: UtilityService, private notifyService: NotificationService,
    private validationService: ValidationService, public dialog: MatDialog, public dialogRef: MatDialogRef<AccounttypeComponent>,) { }

  ngOnInit(): void {
    this.setForm();
    // this.loadAccTypes();
  }
  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      'finType': ['', Validators.required],
      'finCatCode': ['', Validators.required],
      'finCatName': ['', Validators.required],
    });
    this.form.patchValue({ 'finType': this.accountTypeId })
  }


  //loadAccTypes() {
  //  this.apiService.getall('accountType').subscribe(res => {
  //    if (res) {
  //      this.accountType = res;
  //    }
  //  })
  //}

  //checkCategory(evt: any, type: string) {
    //const bCode = evt.target.value as string;
    //this.apiService.getall(`accountscategory/?type=${bCode}&s=${type}`).subscribe(res => {
    //  if (res) {
    //    if (type === 'c')
    //      this.form.controls['finCatCode'].setValue('');
    //    else if (type === 'n')
    //      this.form.controls['finCatName'].setValue('');

    //    this.notifyService.showError('Already Exists.');
    //  }
    //});
 // }


  submit() {
    if (this.form.valid) {
      this.apiService.post('accountscategory', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.form.reset();
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
}
