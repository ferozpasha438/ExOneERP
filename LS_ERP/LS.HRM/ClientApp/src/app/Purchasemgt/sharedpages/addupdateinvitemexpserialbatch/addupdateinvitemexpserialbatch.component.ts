import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';


@Component({
  selector: 'app-addupdateinvitemexpserialbatch',
  templateUrl: './addupdateinvitemexpserialbatch.component.html',
  styles: [
  ]
})
export class AddupdateinvitemexpserialbatchComponent implements OnInit {
  modalBtnTitle: string = ''; // PoNumber
  modalTitle: string = ''; // expSerial

  itemDisable: any = { expiry: true, serial: true };
  labelClass: any = ['text-danger'];
  selectedIndex: number = 0;

  expform!: FormGroup;
  expItems: Array<any> = [];
  expFormItems: Array<any> = [];
  expMessage: string = '';
  expItemCode: string = '';
  qtyError: string = '';
  expSelectedItemQty: number = 0;
  expLineItemsQty: number = 0;
  expSelectedItem: any;
  expErrorMessage: string = '';
  expformIndex: number = 0;
  editId: number = 0;

  serialform!: FormGroup;
  serialItems: Array<any> = [];
  serialFormItems: Array<any> = [];
  serialMessage: string = '';
  serialItemCode: string = '';
  //qtyError: string = '';
  serialSelectedItemQty: number = 0;
  serialLineItemsQty: number = 0;
  serialSelectedItem: any;
  serialErrorMessage: string = '';

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateinvitemexpserialbatchComponent>,
    private notifyService: NotificationService, private validationService: ValidationService, public dialog: MatDialog) {

  }

  ngOnInit(): void {
    this.setExpForm();
    this.setSerialForm();
    //if (!this.itemDisable.expiry || !this.itemDisable.serial)
    this.loadExpirySerialItems();
    this.setItemDisable();
  }

  setItemDisable() {
    if (this.modalTitle == '1') {
      this.itemDisable = { expiry: true, serial: false };
      this.selectedIndex = 0;
    }
    else if (this.modalTitle == '2') {
      this.selectedIndex = 1;
      this.itemDisable = { expiry: false, serial: true };
    }
  }

  setExpForm() {
    this.expform = this.fb.group({
      'index': [this.expformIndex],
      'itemCode': ['', Validators.required],
      'batchNumber': ['', Validators.required],
      'poNumber': [this.modalBtnTitle, Validators.required],
      'mfgDate': [null, Validators.required],
      'expDate': [null, Validators.required],
      'qty': ['', Validators.required],
      // 'qtyCommitted': [0],
      'remarks': [''],
    });
  }

  loadExpirySerialItems() {
    this.apiService.getall(`inventoryExpirySerial/getInvItemSelectListByPoNumber?purchaseOrderNO=${this.modalBtnTitle}`).subscribe(res => {
      if (res) {
        this.expItems = res['exps'];
        this.serialItems = res['serials'];
      }
    });
  }


  expChange(event: any) {
    this.expSelectedItemQty = parseFloat(this.expItems.find(e => e.value == event.value).decValue);
    this.expform.controls['itemCode'].setValue(event.value);
    this.expItemCode = event.value;
    this.expMessage = `${event.text} item is having ${this.expSelectedItemQty} Qty`;
    //console.log(this.expItems.find(e => e.value == event.value).decValue);
  }

  expAdd() {
    if (this.expform.valid) {

      const qty = parseFloat(this.expform.controls['qty'].value);

      const batchNumber = this.expform.controls['batchNumber'].value;
      if (this.expFormItems.find(e => e['batchNumber'] == batchNumber)) {
        this.notifyService.showError(`BatchNumber ${batchNumber} is duplicate`);
        return;
      }

      if (this.editId > 0) {
        this.expFormItems.splice(this.editId, 1);
        this.editId = 0;
      }

      if (qty <= this.expSelectedItemQty) {
        let totalQty = 0, totalCurrentQty;
        this.expFormItems.forEach(e => { totalQty += parseFloat(e['qty']); });
        totalCurrentQty = totalQty;
        totalQty = totalQty + qty;
        if (totalQty <= this.expSelectedItemQty) {
          this.expLineItemsQty = 0;
          //if (totalQty != this.expSelectedItemQty) {
          

          this.expform.controls['index'].setValue(this.expformIndex++);
          this.expFormItems.push(this.expform.value);
          this.expFormItems.forEach(e => { this.expLineItemsQty += parseFloat(e['qty']); });
          this.resetExp();
          this.expform.controls['itemCode'].setValue(this.expItemCode);
          this.expform.controls['poNumber'].setValue(this.modalBtnTitle);

          this.qtyError = '';
          //}
          //else {
          //  this.resetExp();
          //  this.notifyService.showError(`You can not add more than ${this.expSelectedItemQty} qty`);
          //}
        }
        else {
          if ((this.expSelectedItemQty - totalCurrentQty) > 0)
            this.qtyError = `you can add only ${(this.expSelectedItemQty - totalCurrentQty)} qty here`;

          this.notifyService.showError(`total line Items Qty should not more than ${this.expSelectedItemQty}`);
        }
      }
      else
        this.notifyService.showError(`Quantity should not more than Item Quantity of ${this.expSelectedItemQty}`);
    }
    else
      this.utilService.FillUpFields();
  }

  expSubmit() {
    if (this.expFormItems.length > 0) {
      //console.log(this.expFormItems);
      this.apiService.post('inventoryExpirySerial/createInvItemExpiryBatch', { items: this.expFormItems })
        .subscribe(res => {
          this.expSetDefault();
          //    this.dialogRef.close(true);
          this.utilService.OkMessage();

        },
          error => {
            console.error(error);
            this.expErrorMessage = error.error?.message;
            this.utilService.ShowApiErrorMessage(error);
          });
    } else {
      this.notifyService.showError(`Quantity should not equal to ${this.expSelectedItemQty}`);
    }
  }

  editExp(item: any) {
    this.editId = item.index;   
    this.expform.patchValue(item);
  }

  expSetDefault() {
    this.expSelectedItem = null;
    this.loadExpirySerialItems();
    this.expform.controls['itemCode'].setValue('');
    this.expFormItems = [];
    this.expSelectedItemQty = 0;
    this.expLineItemsQty = 0;
    this.expItemCode = '';
    this.expMessage = '';
    this.qtyError = '';
    this.resetExp();
  }
  resetExp() {
    this.expform.reset();
  }


  //For SerialBatch

  serialChange(event: any) {
    this.serialSelectedItemQty = parseFloat(this.serialItems.find(e => e.value == event.value).decValue);
    //console.log(this.serialSelectedItemQty);
    if (this.serialSelectedItemQty > 0) {
      this.addItems(this.serialSelectedItemQty);
    }
    //this.serialform.controls['itemCode'].setValue(event.value);
    this.serialItemCode = event.value;
    this.setSerialItemCodePo(this.serialItemCode);
    this.serialMessage = `${event.text} item is having ${this.serialSelectedItemQty} Qty`;
    //console.log(this.serialItems.find(e => e.value == event.value).decValue);
  }

  setSerialForm() {
    this.serialform = this.fb.group({
      serialList: this.fb.array([this.createSerialBatch()])
    });
  }

  createSerialBatch(): FormGroup {
    return this.fb.group({
      'itemCode': ['', Validators.required],
      'serialNumber': ['', Validators.required],
      'poDate': [null, Validators.required],
      'poNumber': ['', Validators.required],
    })
  }

  addItems(rows: number) {
    //const length = this.serialList.controls.length;
    //if (length > 1)
    //  for (var len = 0; len < length; len++) {
    //    this.serialList.removeAt(len);
    //    console.log('removing at ' + len);
    //  }

    this.serialList.controls = [];
    for (var i = 0; i < rows; i++) {
      this.serialList.push(this.createSerialBatch());
    }
  }
  get serialList(): FormArray {
    return <FormArray>this.serialform.get('serialList');
  }

  checkSerial(evt: any) {
    //let serialNumberArray = [];
    let twiceSerialNumber = 0;
    //console.log(evt.target.value);
    for (let control of this.serialList.controls) {
      if (control instanceof FormGroup) {
        const serialNumber = control.controls['serialNumber'].value;
        if (serialNumber && this.utilService.hasValue(serialNumber)) {
          if (serialNumber == evt.target.value) {
            twiceSerialNumber++;
            if (twiceSerialNumber == 2) {
              this.notifyService.showError('duplicate serialNumber');
              control.controls['serialNumber'].setValue('');
            }
          }
        }
      }
    }
  }

  serialSubmit() {
    // let serialNumberArray = [];
    let hasValidData: boolean = false;
    for (let control of this.serialList.controls) {
      if (control instanceof FormGroup) {
        const serialNumber = control.controls['serialNumber'].value as string;
        const poDate = control.controls['poDate'].value;
        if (serialNumber && poDate) {
          hasValidData = true;
          // serialNumberArray.push(serialNumber);

        }
        else {
          hasValidData = false;
          this.utilService.FillUpFields();
        }
      }
    }
    if (hasValidData) {
      this.apiService.post('inventoryExpirySerial/createInvItemSerialBatch', this.serialform.value)
        .subscribe(res => {
          this.serialSetDefault();
          this.dialogRef.close(true);
          this.utilService.OkMessage();

        },
          error => {
            console.error(error);
            this.serialErrorMessage = error.error?.message;
            //this.utilService.ShowApiErrorMessage(error);
          });

    }


  }


  setSerialItemCodePo(itemCode: string) {
    for (let control of this.serialList.controls) {
      if (control instanceof FormGroup) {
        control.controls['itemCode'].setValue(itemCode);
        control.controls['poNumber'].setValue(this.modalBtnTitle);
      }
    }
  }

  serialSetDefault() {
    this.serialSelectedItem = null;
    this.loadExpirySerialItems();
    this.serialList.controls = [];
    this.serialFormItems = [];
    this.serialSelectedItemQty = 0;
    this.serialLineItemsQty = 0;
    this.serialItemCode = '';
    this.serialMessage = '';
    //this.qtyError = '';
    this.resetSerial();
  }

  resetSerial() {
    this.serialform.reset();
  }


  closeModel() {
    this.dialogRef.close();
  }

}
