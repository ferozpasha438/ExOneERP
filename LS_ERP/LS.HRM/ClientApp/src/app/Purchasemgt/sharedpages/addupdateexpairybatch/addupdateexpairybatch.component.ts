import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-Addupdateexpairybatch',
  templateUrl: './Addupdateexpairybatch.component.html',
  styles: [
  ]
})
export class Addupdateexpairybatch implements OnInit {
  form!: FormGroup;
  inputData!: any;
  id: number = 0;
  tranItemCode: string = '';
  receivedQty: number = 0;
  tranItemName: string = '';
  sequence: number = 1;
  itemCode: string = '';
  listOfInvoices: Array<any> = [];
  isReadOnly: boolean = false;

  tranItemName2: string = '';
  tranItemQty: number = 0;
  tranItemUnitCode: string = '';
  tranUOMFactor: string = '';
  tranItemCost: number = 0;
  tranTotCost: number = 0;
  discPer: string = '';
  discAmt: number = 0;
  itemTax: string = '';
  itemTaxPer: string = '';
  taxAmount: number = 0;

  receivingQty: number = 0;
  balQty: number = 0;

  itemTracking: number = 0;
  serExpTracking: string = '';

  batchNumber: string = '';
  mfgDate: string = '';
  expDate: string = '';
  qty: number = 0;
  remarks: string = '';

 // sequence: number = 1;
  editsequence: number = 0;
  records: Array<any> = [];
  isEditing: boolean = false;
  editingIndex: number | null = null;
  poNumber: string = '';
  itemName: string = '';

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<Addupdateexpairybatch>,
    private notifyService: NotificationService, private validationService: ValidationService) {
  }

  ngOnInit(): void {
    this.setForm();
    if (this.inputData) {
      this.tranItemCode = this.inputData.tranItemCode;
      this.tranItemName = this.inputData.tranItemName;
      this.tranItemQty = this.inputData.tranItemQty;
      this.poNumber = this.inputData.PONO;
      this.itemCode = this.inputData.tranItemCode;
      this.loadExpiryItems(this.inputData.tranItemCode);
    }
  }

  setForm() {
    this.form = this.fb.group({
      batchNumber: [''],
      mfgDate: [null],
      expDate: [null],
      qty: [''],
      remarks: [''],
    });
  }

  //loadExpiryItems(itemCode: any) {

    
  //  this.apiService.getall(`inventoryExpirySerial/GetExpairyDetails?ItemCode=${this.itemCode}`).subscribe(res => {
  //    if (res) {
  //      this.form.patchValue({ 'batchNumber': `${res['batchNumber']}` });
  //      this.form.patchValue({ 'mfgDate': `${res['mfgDate']}` });
  //      this.form.patchValue({ 'expDate': `${res['expDate']}` });
  //      this.form.patchValue({ 'qty': `${res['qty']}` });
  //      this.form.patchValue({ 'remarks': `${res['remarks']}` });
  //      //this.expItems = res['exps'];
  //      //this.serialItems = res['serials'];
  //    }
  //  });
  //}

  loadExpiryItems(itemCode: any) {
    this.apiService.getall(`InventoryExpirySerial/GetExpairyDetails/${itemCode}`).subscribe(res => {
      if (res && Array.isArray(res)) {
        res.forEach(record => {
          this.listOfInvoices.push({
            sequence: this.getSequence(),
            poNumber: record.poNumber,
            itemCode: record.itemCode,
            itemName: record.itemName,
            batchNumber: record.batchNumber,
            mfgDate: this.utilService.selectedDate(record.mfgDate),
            expDate: this.utilService.selectedDate(record.expDate),
            qty: record.qty,
            remarks: record.remarks
          });
        });
      }
    });

    //this.apiService.getall(`InventoryExpirySerial/GetExpairyDetails/${itemCode}`).subscribe(res => {
    //  if (res) {
    //    this.listOfInvoices.push({
    //      batchNumber: res[0].batchNumber,
    //      mfgDate: this.utilService.selectedDate(res[0].mfgDate),
    //      expDate: this.utilService.selectedDate(res[0].expDate),
    //      //mfgDate: this.formatDate(res[0].mfgDate),
    //      //expDate: this.formatDate(res[0].expDate),
    //      qty: res[0].qty,
    //      remarks: res[0].remarks
    //    });
    //    // Uncomment and assign if needed
    //    // this.expItems = res.exps;
    //    // this.serialItems = res.serials;
    //  }
    //});
  }
  //formatDate(dateString:Date) {
  //  const date = new Date(dateString);
  //  const day = ('0' + date.getDate()).slice(-2);
  //  const month = ('0' + (date.getMonth() + 1)).slice(-2);
  //  const year = date.getFullYear();
  //  return `${day}-${month}-${year}`;
  //}


  addExpRecord() {
    if (this.editsequence > 0) {
      this.removeInvoiceList(this.editsequence);
      this.editsequence = 0;
    }
    this.listOfInvoices.push({
       sequence: this.getSequence(),
        batchNumber: this.batchNumber,
        //mfgDate: this.mfgDate,
        //expDate: this.expDate,
        mfgDate: this.utilService.selectedDate(this.mfgDate),
        expDate: this.utilService.selectedDate(this.expDate),
        qty: this.qty,
        remarks: this.remarks,
        poNumber: this.poNumber,
        itemCode: this.tranItemCode,
        itemName: this.tranItemName
    });
    this.form.reset();
    }

  //addExpRecord() {
  //  if (this.isEditing) {
  //    this.listOfInvoices.push( {
  //      sequence: this.sequence++,
  //      batchNumber: this.batchNumber,
  //      //mfgDate: this.mfgDate,
  //      //expDate: this.expDate,
  //      mfgDate: this.utilService.selectedDate(this.mfgDate),
  //      expDate: this.utilService.selectedDate(this.expDate),
  //      qty: this.qty,
  //      remarks: this.remarks,
  //      poNumber: this.poNumber,
  //      itemCode: this.tranItemCode,
  //      itemName: this.tranItemName
  //    });
  //    this.isEditing = false;
  //    this.editingIndex = null;
  //  } else {
  //    this.listOfInvoices.push({
  //      sequence: this.sequence++,
  //      batchNumber: this.batchNumber,
  //      //mfgDate: this.mfgDate,
  //      //expDate: this.expDate,
  //      mfgDate: this.utilService.selectedDate(this.mfgDate),
  //      expDate: this.utilService.selectedDate(this.expDate),
  //      qty: this.qty,
  //      remarks: this.remarks,
  //      poNumber: this.poNumber,
  //      itemCode: this.tranItemCode,
  //      itemName: this.tranItemName
        

  //    });
  //  }
  // this.resetForm();
  //}


  getSequence(): number { return this.sequence += this.sequence + 1 };

  deleteInvoiceItem(item: any) {
    this.removeInvoiceList(item.sequence);
   // this.setGrandTotal();
  }

  

  removeInvoiceList(sequence: number) {
    let index: number = this.listOfInvoices.findIndex(a => a.sequence === sequence);
    this.listOfInvoices.splice(index, 1);
  }


  editRecord(item: any) {
    this.isEditing = true;
   // this.editingIndex = item;
      sequence: this.sequence++,
      this.editsequence = item.sequence,
      this.batchNumber = item.batchNumber,
      this.mfgDate = this.utilService.selectedDate(item.mfgDate),
      this.expDate = this.utilService.selectedDate(item.expDate),
      this.qty= item.qty,
      this.remarks= item.remarks
      
    //this.addExpRecord();
    //this.form.patchValue(this.records[index]);
  }

  deleteRecord(sequence: number) {
    let index: number = this.listOfInvoices.findIndex(a => a.sequence === sequence);
    this.listOfInvoices.splice(index, 1);
  }

  //submit() {
  //  this.form.value['poNumber'] = this.poNumber;
  //  this.form.value['itemCode'] = this.tranItemCode;
  //  this.form.value['itemName'] = this.tranItemName;
  //  this.form.value['batchNumber'] = this.batchNumber;
  //  this.form.value['mfgDate'] = this.utilService.selectedDateTime(this.form.controls['mfgDate'].value);
  //  this.form.value['expDate'] = this.utilService.selectedDateTime(this.form.controls['expDate'].value);
  //  this.form.value['qty'] = this.form.controls['qty'].value;
  //  this.form.value['remarks'] = this.form.controls['qty'].value;
   
  //  this.apiService.post('inventoryExpirySerial/createInvItemExpiryBatch', this.form.value)
  //    .subscribe(res => {
  //      this.resetForm();
  //      this.dialogRef.close(true);
  //      this.utilService.OkMessage();
  //    },
  //      error => {
  //        console.error(error);
  //        this.utilService.ShowApiErrorMessage(error);
  //      });
  //}

  submit() {

    //this.invoiceItemObject = {
    //  item: {},
    //  itemList: this.listOfInvoices
    //};

    this.form.value['mfgDate'] = this.utilService.selectedDate(this.form.controls['mfgDate'].value);
     this.form.value['expDate'] = this.utilService.selectedDate(this.form.controls['expDate'].value);

    if (this.form.valid) {
      //if (this.id > 0)
      //  this.form.value['id'] = this.id;

      if (this.listOfInvoices.length > 0) {
        this.form.value['Items'] = this.listOfInvoices;

        this.apiService.post('inventoryExpirySerial/createInvItemExpiryBatch', this.form.value)
          .subscribe(res => {
            this.utilService.OkMessage();
            this.resetForm();
            this.dialogRef.close(true);
          },
            error => {
              console.error(error);
              this.utilService.ShowApiErrorMessage(error);
            });
      }
      else
        this.notifyService.showError("Line Items Empty");
    }
    else
      this.utilService.FillUpFields();

  }




  resetForm() {
    this.form.reset();
  }

  closeModel() {
    this.dialogRef.close();
  }
}
