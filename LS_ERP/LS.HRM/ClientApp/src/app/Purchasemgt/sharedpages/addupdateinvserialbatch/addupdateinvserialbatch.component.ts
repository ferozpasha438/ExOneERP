import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-addupdateinvserialbatch',
  templateUrl: './addupdateinvserialbatch.component.html',
  styles: [
  ]
})
export class AddupdateinvserialbatchComponent implements OnInit {
  form!: FormGroup;
  itemCode: string = '';
  list: Array<any> = [];
  quantity: number = 0;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateinvserialbatchComponent>,
    private notifyService: NotificationService, private validationService: ValidationService, public dialog: MatDialog) {

  }

  ngOnInit(): void {
    if (this.quantity > 0) {
      this.setForm();
      this.addItems(this.quantity - 1);
    }
  }

  setForm() {

    this.form = this.fb.group({
      serialList: this.fb.array([this.createSerialBatch()])
    });
  }

  createSerialBatch(): FormGroup {
    return this.fb.group({
      'itemCode': [this.itemCode, Validators.required],
      'serialNumber': ['', Validators.required],
      'poDate': [null, Validators.required],
      'poNumber': ['', Validators.required],
    })
  }

  addItems(rows: number) {
    for (var i = 0; i < rows; i++) {
      this.serialList.push(this.createSerialBatch());
    }
  }
  get serialList(): FormArray {
    return <FormArray>this.form.get('serialList');
  }


  submit() {
    this.apiService.post('inventoryExpirySerial/createInvItemSerialBatch', this.form.value)
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
