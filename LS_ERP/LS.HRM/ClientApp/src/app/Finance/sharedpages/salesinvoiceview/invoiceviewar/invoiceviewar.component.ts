import { Input, SimpleChanges, OnChanges } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { default as data } from "../../../../../assets/i18n/siteConfig.json";

@Component({
  selector: 'app-invoiceviewar',
  templateUrl: './invoiceviewar.component.html',
  styles: [
  ]
})
export class InvoiceviewarComponent implements OnInit,OnChanges {
  bank: any;
  customer: any;
  company: any;
  invoice: any;
  items: Array<any> = [];
  totalAmountAr: string = '';
  totalAmountEn: string = '';
  @Input() public res: any;
  @Input() public OpacityValue: number = 1;
  @Input() public isSavePDF: boolean = false;
  @Input() public isArabic: boolean = false;
  dashBoardType: string = '';

  ngOnInit(): void {
    this.dashBoardType = data.dashBoardType;
  }
  ngOnChanges(changes: SimpleChanges): void {

    if (this.res) {
      console.log(this.isArabic);
      this.totalAmountAr = this.res['totalAmountAr'];
      this.totalAmountEn = this.res['totalAmountEn'];
      this.company = this.res['company'];
      this.customer = this.res['customer'];
      this.bank = this.res['bankDetails'];
      this.invoice = this.res['invoice'];
      this.items = this.res['invoiceItems'] as Array<any>;
    }
  }

  hasNonArabChars(text: string): boolean {
    const hasMatch = /^[A-Za-z0-9]*$/.test(text.trim().replace(/ /g, ''));   
    return hasMatch;
  }

}
