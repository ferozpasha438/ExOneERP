import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-addupdateinvspecification',
  templateUrl: './addupdateinvspecification.component.html',
  styles: [
  ]
})
export class AddupdateinvspecificationComponent implements OnInit {
  form!: FormGroup;
  itemCode: string = '';
  IsWarrantyChecked: boolean = false;
  specificatons: Array<any> = [];
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateinvspecificationComponent>,
    private notifyService: NotificationService, private validationService: ValidationService, public dialog: MatDialog) {

  }

  ngOnInit(): void {
    this.setForm();
    this.loadData();
  }
  setForm() {
    this.form = this.fb.group({
      'itemCode': [this.itemCode, Validators.required],
      'specification': ['', Validators.required],
      'unit': ['', Validators.required],
      'hasWarranty': [false],
      'warrTime': [''],
      'remarks': [''],
    });
  }

  loadData() {
    this.apiService.getall("inventoryExpirySerial/getInvItemSpecifications").subscribe(res => {
      if (res)
        this.specificatons = res;
    });
  }

  submitSpecification() {
    this.apiService.post('inventoryExpirySerial/createInvItemSpecification', this.form.value)
      .subscribe(res => {
        debugger;
        this.reset();
        this.dialogRef.close(true);
        this.utilService.OkMessage();

      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });
  }

  warrantyChecked(event: MatSlideToggleChange) {
    this.IsWarrantyChecked = !event.checked;
  }

  reset() {
    this.form.reset();
  }
  closeModel() {
    this.dialogRef.close();
  }

}
