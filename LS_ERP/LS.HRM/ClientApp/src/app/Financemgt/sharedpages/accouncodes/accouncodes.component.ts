import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-accouncodes',
  templateUrl: './accouncodes.component.html',
  styles: [
  ],

})
export class AccouncodesComponent implements OnInit {

  accountTypeId: string = '';
  accountTypeCode: string = '';
  fInSysGenAcCode: boolean = false;
  accountCategories: Array<CustomSelectListItem> = [];
  form: FormGroup;
  id: number = 0;
  isEdit: boolean = false;
  isAccountLengthOk: boolean = true;
  isRevenueChecked: boolean = false;

  constructor(private fb: FormBuilder, private apiService: ApiService, private utilService: UtilityService, private notifyService: NotificationService,
    private validationService: ValidationService, public dialog: MatDialog, public dialogRef: MatDialogRef<AccouncodesComponent>) { }

  ngOnInit(): void {
    this.setForm();

    if (this.accountTypeId !== '')
      this.setEdit();

    // this.loadAccCategories();
  }

  setEdit() {
    this.apiService.getall(`accountscategory/getAccountCode?id=${this.accountTypeId}`).subscribe(res => {
      if (res) {
        this.form.patchValue(res);
        this.isRevenueChecked = res['finIsRevenue'] as boolean;
        this.id = parseInt(res['id']);
        this.isEdit = true;
      }
    })
  }


  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      'finAcCode': [''],// this.validationService.accountValidator],
      'finAcName': ['', Validators.required],
      'finAcDesc': ['', Validators.required],
      'finAcAlias': ['', Validators.required],
      'finIsPayCode': [false],
      'finPayCodetype': [''],// Validators.required],
      'finIsIntegrationAC': [false],
      'finSubCatCode': '',
      'finIsRevenue': [false],
      'finIsRevenuetype': '',
      'status': [false],
    })
    this.form.patchValue({ 'finSubCatCode': this.accountTypeCode })
  }

  revenueChecked(evt: MatSlideToggleChange) {
    this.isRevenueChecked = evt.checked;
  }

  checkAccountCode(evt: any) {
    const bCode = evt.target.value as string;

    //if (!bCode.match(/(\d{5})/)) {
    //  this.isAccountLengthOk = false;
    //}
    //else
    //  this.isAccountLengthOk = true;



    if (!this.isEdit) {
      if (bCode.trim() !== '') {
        this.apiService.getall(`accountscategory/checkAccountCode?accountCode=${bCode}`).subscribe(res => {
          if (res) {
            this.form.controls['finAcCode'].setValue('');
            this.notifyService.showError('Already Exists.');
          }
        });
      }

    }
  }



  //loadAccCategories() {
  //  this.apiService.getall('accountscategory').subscribe(res => {
  //    if (res) {
  //      this.accountCategories = res;
  //    }
  //  })
  //}

  submit() {

    const finIsRevenuetype = this.form.controls['finIsRevenuetype'].value as string;
    if (this.isRevenueChecked && !this.utilService.hasValue(finIsRevenuetype)) {
      this.notifyService.showError('Select Revenue');
      return;
    }

    if (!this.fInSysGenAcCode) {

      const finAcCode = this.form.controls['finAcCode'].value as string;
      if (finAcCode === undefined || finAcCode === '') {
        this.utilService.FillUpFields();
        return;
      }
      else {
        if (!finAcCode.match(/(\d{5})/)) {
          this.isAccountLengthOk = false;
          //this.notifyService.showError('Account Code length should be 5');
          //this.notifyService.showError('AccountCode Format (5) : (xxxxx)  رمز الحساب  التنسيق : (xxxxx)');
          return;
        }
        else
          this.isAccountLengthOk = true;
      }
    }

   


    if (this.form.valid) {

      if (this.id > 0)
        this.form.value['id'] = this.id;

      this.apiService.post('accountscategory/createaccoutcode', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();

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
