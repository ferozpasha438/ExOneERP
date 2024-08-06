import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ParentSystemSetupComponent } from '../../../sharedcomponent/parentsystemsetup.component';
import { ValidationService } from '../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-addupdatetaxslab',
  templateUrl: './addupdatetaxslab.component.html',
  styles: [
  ]
})

export class AddupdatetaxslabComponent extends ParentSystemSetupComponent implements OnInit {
  form: FormGroup;
  accountList: Array<CustomSelectListItem> = [];
  vatList: Array<CustomSelectListItem> = [];
  id: number = 0;
  isLoading: boolean = false;

  inputAcCode01: string = '';
  outputAcCode01: string = '';

  inputAcCode02: string = '';
  outputAcCode02: string = '';


  constructor(private fb: FormBuilder, private apiService: ApiService, private authService: AuthorizeService, private utilService: UtilityService,
    private validationService: ValidationService, private notifyService: NotificationService, public dialogRef: MatDialogRef<AddupdatetaxslabComponent>,
  ) { super(authService); }

  ngOnInit(): void {
    this.setForm();
    this.laodAccounts();

    if (this.id > 0)
      this.setEditForm();
  }

  setEditForm() {
    this.isLoading = true;

    this.apiService.get('taxes', this.id).subscribe(res => {
      this.isLoading = false;
      if (res) {
        this.form.patchValue(res);
      }
    });
  }


  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      // 'finBranchCode': ['', Validators.required],
      'taxCode': ['', Validators.required],
      'taxName': ['', Validators.required],
      'isActive': [false],
      'isInterState': [false],

      'taxComponent01': ['', Validators.required],
      'taxper01': ['', Validators.compose([Validators.required])],
      'inputAcCode01': ['', Validators.required],
      'outputAcCode01': ['', Validators.required],

      'taxComponent02': [''],
      'taxper02': ['0'],
      'inputAcCode02': [''],
      'outputAcCode02': [''],

      //'taxComponent02': ['', Validators.required],
      //'taxper02': ['', Validators.compose([Validators.required])],
      //'inputAcCode02': ['', Validators.required],
      //'outputAcCode02': ['', Validators.required],

      'taxComponent03': [''],
      'taxper03': ['0'],
      'inputAcCode03': [''],
      'outputAcCode03': [''],

      'taxComponent04': [''],
      'taxper04': ['0'],
      'inputAcCode04': [''],
      'outputAcCode04': [''],

      'taxComponent05': [''],//Tax5
      'taxper05': ['0'],
      'inputAcCode05': [''],
      'outputAcCode05': [''],

    })
  }

  laodAccounts() {
    this.apiService.getall(`accountscategory/getSelectMainAllAccountList`).subscribe(res => {
      if (res) {
        this.accountList = res;
      }
    });
    this.apiService.getall("vat/getSelectVatList").subscribe(res => {
      if (res)
        this.vatList = res;
    });

  }

  submit() {
    //console.log(this.form.value)
    if (this.form.valid) {

      if (this.id > 0)
        this.form.value['id'] = this.id;

      this.apiService.post('taxes', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.dialogRef.close(true);
          this.reset();
        },
          error => {
            this.utilService.ShowApiErrorMessage(error);
          });
    }
    else
      this.utilService.FillUpFields();
  }

  reset() {
    this.form.controls['taxCode'].setValue('');
    this.form.controls['taxName'].setValue('');
    this.form.controls['isActive'].setValue(false);
    this.form.controls['isInterState'].setValue(false);
  }

  closeModel() {
    this.dialogRef.close();
  }

}
