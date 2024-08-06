import { Input, SimpleChanges, OnChanges } from '@angular/core';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-snddeliverynoteviewar',
  templateUrl: './snddeliverynoteviewar.component.html'
})
export class SnddeliverynoteviewarComponent implements OnChanges {
  bank: any;
  customer: any;
  company: any;
  deliveryNote: any;
  items: Array<any> = [];
  totalAmountAr: string = '';
  totalAmountEn: string = '';
  footerDiscount: number = 0;
  tableData: Array<any> = [];

  grandTotalData: any = { total: 0, discAmnt: 0, footerDiscAmnt: 0, vatAmnt: 0, invTotalAmnt: 0 };
  @Input() public res: any;

  @Input() public isSavePDF: boolean = false;
  @Input() public format: string = "format1";

  ngOnChanges(changes: SimpleChanges): void {
    if (this.res) {

      //      console.log(this.res);
      this.totalAmountAr = this.res['totalAmountAr'];
      this.totalAmountEn = this.res['totalAmountEn'];
      this.company = this.res['company'];
      this.customer = this.res['customer'];
      this.bank = this.res['bankDetails'];
      this.deliveryNote = this.res['deliveryNoteHead'];
      this.items = this.res['deliveryNoteItems'] as Array<any>;
      this.footerDiscount = this.deliveryNote['footerDiscount'];

      console.log(this.res);

      if (this.items.length > 0) {
        this.tableData = [];

        this.grandTotalData = { total: 0.00, discAmnt: 0.00, footerDiscAmnt: 0.00, vatAmnt: 0.00, invTotalAmnt: 0.00 };


        this.items.forEach(e => {
          let item: any = e;
          item.calTotal = e.quantity * item.unitPrice * 1.00;
          item.calDiscAmnt = item.calTotal * (item.discount / 100);
          item.calFooterDiscAmnt = item.calTotal * (1 - item.discount / 100) * (this.footerDiscount / 100);
          item.calVatAmnt = (item.calTotal - item.calDiscAmnt - item.calFooterDiscAmnt) * (item.taxTariffPercentage / 100);
          item.calInvAmnt = (item.calTotal - item.calDiscAmnt - item.calFooterDiscAmnt + item.calVatAmnt);
          item.calUnitDiscPrice = item.unitPrice * (item.discount / 100);
          item.calUnitFooterDiscPrice = item.unitPrice * (item.footerDiscount / 100);
          item.calUnitTotalDiscPrice = item.unitPrice * (1 - item.discount / 100) * (1 - this.footerDiscount / 100);
          item.calSubTotal = item.calUnitTotalDiscPrice * item.quantity;



          this.grandTotalData.total += item.calTotal;
          this.grandTotalData.discAmnt += item.calDiscAmnt;
          this.grandTotalData.footerDiscAmnt += item.calFooterDiscAmnt;
          this.grandTotalData.vatAmnt += item.calVatAmnt;
          this.grandTotalData.invTotalAmnt += item.calInvAmnt;

          this.grandTotalData.totalBeforeVat = this.grandTotalData.total - this.grandTotalData.discAmnt - this.grandTotalData.footerDiscAmnt;
          this.grandTotalData.totalBeforeFooterDis = this.grandTotalData.total - this.grandTotalData.discAmnt;


          this.tableData.push(item);
        });

        console.log(this.tableData);
        console.log(this.grandTotalData);

      }



    }
  }
}
