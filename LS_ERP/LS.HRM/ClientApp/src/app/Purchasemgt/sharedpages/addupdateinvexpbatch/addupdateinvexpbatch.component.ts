import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-addupdateinvexpbatch',
  templateUrl: './addupdateinvexpbatch.component.html',
  styles: [
  ]
})
export class AddupdateinvexpbatchComponent implements OnInit {
  form!: FormGroup;
  itemCode: string = '';
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateinvexpbatchComponent>,
    private notifyService: NotificationService, private validationService: ValidationService, public dialog: MatDialog) {

  }

  ngOnInit(): void {
    this.setForm();
  }
  setForm() {

    this.form = this.fb.group({
      'itemCode': [this.itemCode, Validators.required],
      'batchNumber': ['', Validators.required],
      'mfgDate': [null, Validators.required],
      'expDate': [null, Validators.required],
      'qty': ['', Validators.required],
      // 'qtyCommitted': [0],
      'remarks': [''],
    });
  }
  submit() {
    this.apiService.post('inventoryExpirySerial/createInvItemExpiryBatch', this.form.value)
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

  reset() {
    this.form.reset();
  }
  closeModel() {   
    this.dialogRef.close();
  }

}
