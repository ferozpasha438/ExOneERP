import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { NotificationService } from '../../../../services/notification.service';

@Component({
  selector: 'app-postingadvancepayment',
  templateUrl: './postingadvancepayment.component.html',
  styles: [
  ]
})
export class PostingadvancepaymentComponent implements OnInit {

  data: any;
  arVoucherAmt: number = 0;
  discountAmt: number = 0;
  netAmt: number = 0;
  writeOffAmt: number = 0;
  paidAmt: number = 0;
  minCutOffShortAmt: number = 0;
  maxCutOffOverAmr: number = 0;
  advAmount: number = 0;
  remainingAdvAmount: number = 0;
  hasDiscount: boolean = false;
  paidAmtReadOnly: boolean = false;

  constructor(private notifyService: NotificationService, public dialogRef: MatDialogRef<PostingadvancepaymentComponent>,) { }

  ngOnInit(): void {
    if (this.data) {
      this.arVoucherAmt = this.data.arAmount;
      this.netAmt = this.arVoucherAmt;
      console.log(this.data);
      this.minCutOffShortAmt = this.data.fin.minCutOffShortAmt;
      this.maxCutOffOverAmr = this.data.fin.maxCutOffOverAmr;
      this.advAmount = this.data.fin.advAmount;

      if (this.advAmount > 0) {
        this.paidAmtReadOnly = true;
        this.writeOffAmt = 0;
        if (this.advAmount >= this.arVoucherAmt)
          this.paidAmt = this.arVoucherAmt;
        else {
          this.paidAmt = parseFloat((this.arVoucherAmt - this.advAmount).toFixed(2));
          // this.paidAmtReadOnly = false;
        }

        this.remainingAdvAmount = parseFloat((this.advAmount - this.arVoucherAmt).toFixed(2));
      }

    }
    else
      this.dialogRef.close({ hasData: 0 });
  }

  calculateDis() {
    this.netAmt = parseFloat((this.data.arAmount - this.discountAmt).toFixed(2));
    this.writeOffAmt = 0;
    this.hasDiscount = this.discountAmt != 0;
  }
  totalArAmtCal() {
    if (this.paidAmt > 0 && !this.hasDiscount && !this.paidAmtReadOnly) {
      this.writeOffAmt = parseFloat((this.paidAmt - this.netAmt).toFixed(2));
    }
  }

  postAdvanceAmt() {
    this.totalArAmtCal();
    //console.log(this.writeOffAmt, this.minCutOffShortAmt, this.maxCutOffOverAmr);
    let response: any = { hasData: 1, paidAmount: this.paidAmt, source: 'ar', advanceAmount: 0, remainingAdvAmount: this.remainingAdvAmount };
    if (this.writeOffAmt != 0 && !this.hasDiscount) {
      if (this.writeOffAmt > this.maxCutOffOverAmr) { //advance payment
        // response.advanceAmount = this.writeOffAmt;
        response.amountType = 'advance';
        //  this.notifyService.showSuccess('advance amt');
        // this.dialogRef.close(response);
        this.notifyService.showError(`writeOff Amount range should be min : ${this.minCutOffShortAmt} and max : ${this.maxCutOffOverAmr} `); //
        //this.notifyService.showError('No Advance Payment');
      }
      else if (this.writeOffAmt >= this.minCutOffShortAmt && this.writeOffAmt <= this.maxCutOffOverAmr) {

        response.writeOffAmount = this.writeOffAmt;

        if (this.writeOffAmt < 0) { // for short payment
          response.amountType = 'short';
          response.writeOffAmount = (-1 * this.writeOffAmt);
          //  this.notifyService.showSuccess('short amt');
        }
        else { //for over payment
          response.amountType = 'over';
          //  this.notifyService.showSuccess('over amt');
        }
        this.dialogRef.close(response);
      }
      else {
        this.notifyService.showError(`writeOff Amount range should be min : ${this.minCutOffShortAmt} and max : ${this.maxCutOffOverAmr} `); //
      }
    }
    else {
      response.writeOffAmount = 0;
      if (this.paidAmt >= this.netAmt) { //advance payment
        if (this.paidAmt > this.netAmt) { //advance payment
          //can submit the data
          // response.advanceAmount = parseFloat((this.paidAmt - this.netAmt).toFixed(2));
          response.amountType = 'advance';
          //  this.notifyService.showSuccess('advance amt');
         // this.notifyService.showError('No Advance Payment');
          this.notifyService.showError(`writeOff Amount range should be min : ${this.minCutOffShortAmt} and max : ${this.maxCutOffOverAmr} `); //
        }
        else {
          response.amountType = 'equal';
          // this.notifyService.showSuccess('equal amt');
          this.dialogRef.close(response);
        }
      }
      else if (this.paidAmtReadOnly)
        this.dialogRef.close(response);
      else
        this.notifyService.showError('amount should be >= ' + this.netAmt);
    }
  }

  closeModel() {
    this.dialogRef.close({ hasData: -1 });
  }
}
