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
  selector: 'app-addupdatepurchasevendortermcode',
  templateUrl: './addupdatepurchasevendortermcode.component.html',
  styleUrls: []
})
export class AddupdatepurchasevendortermcodeComponent extends ParentSystemSetupComponent implements OnInit {

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
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdatepurchasevendortermcodeComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {
    super(authService);
  }




  ngOnInit(): void {
    this.setForm();

    if (this.id > 0)
      this.setEditForm();
  }



  setForm() {
    this.form = this.fb.group({
      'poTermsCode': ['', Validators.required],
      'poTermsName': ['', Validators.required],
      'poTermsDesc': '',
      'poTermsDueDays': '',
      'poTermDiscDays': ''


    });
  }

  setEditForm() {
    this.apiService.get('purchaseterms', this.id).subscribe(res => {
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
      //let Duedays = this.form.value.poTermsDueDays.getDate();
      //let Discountdays = this.form.value.poTermDiscDays.getDate();
      //this.form.value['poTermsDueDays'] = Duedays;
      //this.form.value['poTermDiscDays'] = Discountdays
      this.apiService.post('purchaseterms', this.form.value)
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
    this.form.controls['poTermsCode'].setValue('');
    this.form.controls['poTermsName'].setValue('');
    this.form.controls['poTermsDesc'].setValue('');
    this.form.controls['poTermsDueDays'].setValue('');
    this.form.controls['poTermDiscDays'].setValue('');


  }

  closeModel() {
    this.dialogRef.close();
  }
  onTextchange(Value: string) {
    if (Value != null) {
      this.apiService.getall(`purchaseterms/GetPurchaseTerm?CatCode=${Value}`).subscribe(res => {
        if (res) {
          this.isReadOnly = true;
          this.form.patchValue(res);
          this.id = res.id;
        }
      })
    }
  }

}
