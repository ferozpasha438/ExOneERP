
import { Component, EventEmitter, Output} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-Addupdatereturnexpairybatch',
  templateUrl: './Addupdatereturnexpairybatch.component.html',
  styles: [
  ]
})
export class AddupdatereturnexpairybatchComponent {

  @Output() quantityUpdated: EventEmitter<number> = new EventEmitter<number>(); // Create an event emitter for qty

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
  whCode: string = '';
  
  totalItemQuantity: number = 0;
  grnId: string = '';

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdatereturnexpairybatchComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {
  }

  ngOnInit(): void {
    this.setForm();
    if (this.inputData) {
      this.tranItemCode = this.inputData.tranitemcode;
      this.tranItemName = this.inputData.tranitemname;
      this.tranItemQty = this.inputData.tranitemqty;
      this.poNumber = this.inputData.PONO;
      this.itemCode = this.inputData.tranitemcode;
      this.whCode = this.inputData.whcode;
      this.grnId = this.inputData.grnId;
      this.tranUOMFactor = this.inputData.tranuomfactor;
      this.loadExpiryItems(this.inputData.tranitemcode);
      this.calculateTotalItemQuantity();
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
    this.apiService.getall(`InventoryExpirySerial/GetPRExpairyDetails/${itemCode}`).subscribe(res => {
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
            tranUOMFactor: this.inputData.tranUOMFactor,
            baseQty: parseFloat((parseFloat(this.tranUOMFactor) * this.qty).toString()),
            whCode: record.whCode,
            grnId: record.grnId,
            remarks: record.remarks
            
          });
        });
        this.totalItemQuantity = this.calculateTotalItemQuantity();
      }
     
    });


  }
  


  addExpRecord() {
    if (this.editsequence > 0) {
      this.removeInvoiceList(this.editsequence);
      this.editsequence = 0;
    }
   
    // Add the new invoice to the list
    this.listOfInvoices.push({
      sequence: this.getSequence(),
      batchNumber: this.batchNumber,
      mfgDate: this.utilService.selectedDate(this.mfgDate),
      expDate: this.utilService.selectedDate(this.expDate),
      qty:this.qty,
      baseQty: parseFloat((parseFloat(this.tranUOMFactor) * this.qty).toString()),
      tranUOMFactor: parseFloat(this.tranUOMFactor),
      remarks: this.remarks,
      poNumber: this.poNumber,
      itemCode: this.tranItemCode,
      itemName: this.tranItemName,
      whCode: this.whCode,
      grnId: this.grnId
    });
    this.totalItemQuantity = this.calculateTotalItemQuantity();
    // Calculate total item quantity
   //this.calculateTotalItemQuantity();

    // Reset the form after adding the record
    this.form.reset();
  }

  totalQty: number = 0;

  //addExpRecord() {
  //  if (this.editsequence > 0) {
  //    this.removeInvoiceList(this.editsequence);
  //    this.editsequence = 0;
  //  }

  //  // Find if an invoice with the same batchNumber already exists
  //  let existingInvoice = this.listOfInvoices.find(invoice => invoice.batchNumber === this.batchNumber);

  //  if (existingInvoice) {
  //    // Sum the quantity of the existing invoice and the new one
  //    existingInvoice.qty = Number(existingInvoice.qty) + Number(this.qty);
  //  } else {
  //    // Add the new invoice to the list if no matching batchNumber is found
  //    this.listOfInvoices.push({
  //      sequence: this.getSequence(),
  //      batchNumber: this.batchNumber,       
  //      mfgDate: this.utilService.selectedDate(this.mfgDate),
  //      expDate: this.utilService.selectedDate(this.expDate),
  //      qty: Number(this.qty),  // Ensure qty is treated as a number
  //      remarks: this.remarks,
  //      poNumber: this.poNumber,
  //      itemCode: this.tranItemCode,
  //      itemName: this.tranItemName,
  //      tranNumber: this.tranNumber,
  //      whCode: this.whCode
  //    });
  //  }

  //  // Calculate total item quantity
  //  this.totalItemQuantity = this.calculateTotalItemQuantity();

  //  // Reset the form after adding the record
  //  this.form.reset();
  //}










  // Method to calculate the total item quantity
  calculateTotalItemQuantity() {
    return this.listOfInvoices.reduce((total, invoice) => {
      return total + parseFloat(invoice.qty);
    }, 0);
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
      this.remarks = item.remarks,
      this.grnId = item.grnId,
      this.whCode = item.whCode
      
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

    this.form.value['mfgDate'] = this.utilService.selectedDate(this.form.controls['mfgDate'].value);
    this.form.value['expDate'] = this.utilService.selectedDate(this.form.controls['expDate'].value);

    if (this.form.valid) {
      //if (this.id > 0)
      //  this.form.value['id'] = this.id;

      if (this.listOfInvoices.length > 0) {
        this.form.value['Items'] = this.listOfInvoices;

        this.apiService.post('inventoryExpirySerial/UpdateInvPRItemExpiryBatch', this.form.value)
          .subscribe(res => {
            this.utilService.OkMessage();
            this.resetForm();
            this.dialogRef.close({ isOk: true, totalItemQuantity:this.totalItemQuantity});
            // Emit the total item quantity after successful save
           // this.quantityUpdated.emit(this.totalItemQuantity); // Emit the updated qty
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
