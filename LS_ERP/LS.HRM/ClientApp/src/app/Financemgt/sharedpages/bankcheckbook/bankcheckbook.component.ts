import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-bankcheckbook',
  templateUrl: './bankcheckbook.component.html',
  styles: [
  ],

})
export class BankcheckbookComponent implements OnInit {
  form: FormGroup;
  isCheckValue: boolean = false;

  constructor(private fb: FormBuilder, private apiService: ApiService, private utilService: UtilityService,
    private validationService: ValidationService, public dialog: MatDialog, private notifyService: NotificationService,
    public dialogRef: MatDialogRef<BankcheckbookComponent>) { }


  ngOnInit(): void {
    this.setForm();

    let paycodeCheckvalue = localStorage.getItem('paycodeCheckvalue');
    if (paycodeCheckvalue) {
      let cc = JSON.parse(paycodeCheckvalue);
      console.log(cc['stChkNum']);
      if (cc['stChkNum'] === '0' || cc['stChkNum'] === 0)
        this.isCheckValue = false;
      else
        this.isCheckValue = true;

      this.form.controls['stChkNum'].setValue(cc['stChkNum']);
      this.form.controls['endChkNum'].setValue(cc['endChkNum']);
      this.form.controls['checkLeavePrefix'].setValue(cc['checkLeavePrefix']);
    }
    else
      this.isCheckValue = false;
  }


  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      // 'finBranchCode': ['', Validators.required],
      // 'finPayCode': ['', Validators.required],
      'stChkNum': ['', Validators.compose([Validators.required])],
      'endChkNum': ['', Validators.compose([Validators.required])],
      'checkLeavePrefix': ['', Validators.compose([Validators.required])]
    })
  }


  submit() {
    if (this.form.valid) {
      this.dialogRef.close({ hasValue: true, value: this.form.value });
    }
    else
      this.utilService.FillUpFields();
  }
  closeModel() {
    localStorage.removeItem('paycodeCheckvalue')
    this.dialogRef.close({ hasValue: false, value: this.form.value });
  }
}
