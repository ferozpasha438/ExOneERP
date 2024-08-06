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
  selector: 'app-addupdateshipmentcode',
  templateUrl: './addupdateshipmentcode.component.html',
  styles: [
  ]
})
export class AddupdateshipmentcodeComponent implements OnInit {
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
  TypeList = [
    { text: "Sea", value: "Sea" },
    { text: "Route", value: "Route" },
    { text: "Air", value: "Air" },
    { text: "Courier", value: "Courier" }

  ];
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateshipmentcodeComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {
  }




  ngOnInit(): void {
    this.setForm();

    if (this.id > 0)
      this.setEditForm();
  }



  setForm() {
    this.form = this.fb.group({
      'shipmentCode': ['', Validators.required],
      'shipmentName': ['', Validators.required],
      'shipmentDesc': '',
      'shipmentType': ''


    });
  }

  setEditForm() {
    this.apiService.get('SalesShipmentCode', this.id).subscribe(res => {
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

      this.apiService.post('SalesShipmentCode', this.form.value)
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
    this.form.controls['shipmentCode'].setValue('');
    this.form.controls['shipmentName'].setValue('');
    this.form.controls['shipmentDesc'].setValue('');
    this.form.controls['shipmentType'].setValue('');

  }

  closeModel() {
    this.dialogRef.close();
  }
  onTextchange(Value: string) {
    if (Value != null) {
      this.apiService.getall(`SalesShipmentCode/GetShipmentCode?CatCode=${Value}`).subscribe(res => {
        if (res) {
          this.isReadOnly = true;
          this.form.patchValue(res);
          this.id = res.id;
        }
      })
    }
  }
}
