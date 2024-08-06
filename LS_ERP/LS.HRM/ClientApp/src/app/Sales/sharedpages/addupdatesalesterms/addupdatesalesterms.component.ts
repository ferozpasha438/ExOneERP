import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { ParentSystemSetupComponent } from '../../../sharedcomponent/parentsystemsetup.component';

@Component({
  selector: 'app-addupdatesalesterms',
  templateUrl: './addupdatesalesterms.component.html',
  styles: [
  ]
})
export class AddupdatesalestermsComponent implements OnInit {

  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
  /* isCompanyLoading: boolean = false;*/
  isReadOnly: boolean = false;
  //companyControl = new FormControl();
  //filteredOptions: Observable<Array<CustomSelectListItem>>;
  //cityList: Array<CustomSelectListItem> = [];

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdatesalestermsComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {
  }




  ngOnInit(): void {
    this.setForm();

    if (this.id > 0)
      this.setEditForm();
  }



  setForm() {
    this.form = this.fb.group({
      'salesTermsCode': ['', Validators.required],
      'salesTermsName': ['', Validators.required],
      'salesTermsDesc': '',
      'salesTermsDueDays': '',
      'salesTermDiscDays': ''


    });
  }

  setEditForm() {
    this.apiService.get('salesTermsCode', this.id).subscribe(res => {
      if (res) {
        this.isReadOnly = true;
        this.form.patchValue(res);

      }
    })
  }

  submit() {
    if (this.form.valid) {

      if (this.id > 0)
        this.form.value['id'] = this.id;

      //console.log(this.form.controls['salesTermsDueDays']);
      //this.form.value['salesTermsDueDays'] = this.utilService.selectedDateDays(this.form.controls['salesTermsDueDays'].value);
      //this.form.value['salesTermDiscDays'] = this.utilService.selectedDateDays(this.form.controls['salesTermDiscDays'].value)

      this.apiService.post('salesTermsCode', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.reset();
          this.dialogRef.close(true);
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
    this.form.controls['salesTermsCode'].setValue('');
    this.form.controls['salesTermsName'].setValue('');
    this.form.controls['salesTermsDesc'].setValue('');
    this.form.controls['salesTermsDueDays'].setValue('');
    this.form.controls['salesTermDiscDays'].setValue('');


  }

  closeModel() {
    this.dialogRef.close();
  }

  onTextchange(Value: string) {
    if (Value != null) {
      this.apiService.getall(`salesTermsCode/getSalesTermByCode?catCode=${Value}`).subscribe(res => {
        if (res) {
          this.isReadOnly = true;
          this.form.patchValue(res);
          this.id = res.id;
        }
      })
    }
  }

}

