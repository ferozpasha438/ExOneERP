import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ParentSystemSetupComponent } from '../../../sharedcomponent/parentsystemsetup.component';

@Component({
  selector: 'app-opmvendorpaymentvoucher',
  templateUrl: './opmvendorpaymentvoucher.component.html',
  styles: [
  ],

})
export class OpmVendorpaymentvoucher implements OnInit {
  id: number = 0;
  voucher: any;
  paidInvoices: Array<any> = [];
  unPaidInvoices: Array<any> = [];

  isLoading: boolean = false;

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<OpmVendorpaymentvoucher>,
    private notifyService: NotificationService) {
   
  }

  ngOnInit(): void {
    if (this.id > 0)
      this.loadVoucher();
  }

  loadVoucher() {
    this.isLoading = true;
    this.apiService.getall(`opmApPayment/getOpmVendorpaymentvoucher?id=${this.id}`).subscribe(res => {
      this.isLoading = false;
      this.voucher = res;
      this.paidInvoices = res['paidList'];
      this.unPaidInvoices = res['unPaidList'];
    });
  }

  openPrint() {
    const printContent = document.getElementById("printcontainer") as HTMLElement;
    this.utilService.printForLocale(printContent,true);
  }

  printVoucher() {
    this.openPrint();
    //const printContent = document.getElementById("printcontainer") as HTMLElement;
    //const WindowPrt: any = window.open('', '', 'left=0,top=0,width=2000,height=1000,toolbar=0,scrollbars=0,status=0');
    //setTimeout(() => {
    //  WindowPrt.document.write(printContent.innerHTML);
    //  WindowPrt.document.close();
    //  WindowPrt.focus();
    //  WindowPrt.print();
    //  WindowPrt.close();
    //}, 50);

  }

  closeModel() {
    this.dialogRef.close();
  }

}
