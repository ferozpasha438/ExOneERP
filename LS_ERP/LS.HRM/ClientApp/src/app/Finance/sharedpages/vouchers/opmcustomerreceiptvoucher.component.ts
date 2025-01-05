import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ParentSystemSetupComponent } from '../../../sharedcomponent/parentsystemsetup.component';

@Component({
  selector: 'app-opmcustomerreceiptvoucher',
  templateUrl: './opmcustomerreceiptvoucher.component.html',
  styles: [
  ],

})
export class OpmCustomerreceiptvoucher implements OnInit {
  id: number = 0;
  voucher: any;
  paidInvoices: Array<any> = [];
  unPaidInvoices: Array<any> = [];

  isLoading: boolean = false;

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<OpmCustomerreceiptvoucher>,
    private notifyService: NotificationService) {

  }

  ngOnInit(): void {
    if (this.id > 0)
      this.loadVoucher();
  }

  loadVoucher() {
    this.isLoading = true;
    this.apiService.getall(`opmArPayment/getOpmCustomerpaymentvoucher?id=${this.id}`).subscribe(res => {
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
    ////const printContent = document.getElementById("printcontainer") as HTMLElement;
    ////const printWindow: any = window.open('', '', 'left=0,top=0,width=2000,height=1000,toolbar=0,scrollbars=0,status=0');
    ////printWindow.document.write(printContent.innerHTML);
    ////printWindow.addEventListener('afterprint', function () {
    ////  printWindow.addEventListener('focus', function () {
    ////    printWindow.close();
    ////  });
    ////  printWindow.close();
    ////});

    ////printWindow.print();

    ////this.openPrint();
    //const printContent = document.getElementById("printcontainer") as HTMLElement;
    //const WindowPrt: any = window.open('', '', 'left=0,top=0,width=2000,height=1000,toolbar=0,scrollbars=0,status=0');
    //setTimeout(() => {
    //  WindowPrt.document.write(printContent.innerHTML);
    //  WindowPrt.document.close();
    //  WindowPrt.focus();
    //  WindowPrt.print();
    //  //WindowPrt.close();
    //}, 50);
    ////WindowPrt.print();
    //WindowPrt.onafterprint = this.back();

  }
    back() {
      window.history.back();
    }

  closeModel() {
    this.dialogRef.close();
  }
}
