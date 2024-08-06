import { Input, SimpleChanges, OnChanges } from '@angular/core';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-purchaseinvoiceviewar',
  templateUrl: './purchaseinvoiceviewar.component.html',
  styles: [
  ]
})
export class PurchaseinvoiceviewarComponent implements OnChanges {
  bank: any;
  customer: any;
  company: any;
  invoice: any;
  items: Array<any> = [];
  totalAmountAr: string = '';
  totalAmountEn: string = '';
  @Input() public res: any;
  @Input() public isSavePDF: boolean = false;

  ngOnChanges(changes: SimpleChanges): void {
    if (this.res) {
      this.totalAmountAr = this.res['totalAmountAr'];
      this.totalAmountEn = this.res['totalAmountEn'];
      this.company = this.res['company'];
      this.customer = this.res['customer'];
      this.bank = this.res['bankDetails'];
      this.invoice = this.res['invoice'];
      this.items = this.res['invoiceItems'] as Array<any>;
    }
  }
}
