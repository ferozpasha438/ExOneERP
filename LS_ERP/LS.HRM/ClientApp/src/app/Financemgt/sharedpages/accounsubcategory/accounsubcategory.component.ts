import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
import { UtilityService } from '../../../services/utility.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-accounsubcategory',
  templateUrl: './accounsubcategory.component.html',  
  
})
export class AccounsubcategoryComponent implements OnInit {
  accountTypeId: string = '';
  accountTypeCode: string = '';
  accountCategories: Array<CustomSelectListItem> = [];
  form: FormGroup;
  constructor(private fb: FormBuilder, private apiService: ApiService, private utilService: UtilityService,
    private validationService: ValidationService, public dialog: MatDialog, public dialogRef: MatDialogRef<AccounsubcategoryComponent>,) { }

  ngOnInit(): void {
    this.setForm();
   // this.loadAccCategories();
  }

  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      'finCatCode': ['', Validators.required],
      'finSubCatCode': ['', Validators.required],
      'finSubCatName': ['', Validators.required],
    })
    this.form.patchValue({ 'finCatCode': this.accountTypeCode })
  }

  //loadAccCategories() {
  //  this.apiService.getall('accountscategory').subscribe(res => {
  //    if (res) {
  //      this.accountCategories = res;
  //    }
  //  })
  //}

  submit() {
    if (this.form.valid) {
      this.apiService.post('accountscategory/createaccoutnsubcategory', this.form.value)
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
