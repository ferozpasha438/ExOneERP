import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ParentSystemSetupComponent } from '../../../sharedcomponent/parentsystemsetup.component';

@Component({
  selector: 'app-vendorpaymentvoucher',
  templateUrl: './vendorpaymentvoucher.component.html',
  styles: [
  ],

})
export class Vendorpaymentvoucher extends ParentSystemSetupComponent implements OnInit {
  id: number = 0;
  voucher: any;


  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<Vendorpaymentvoucher>,
    private notifyService: NotificationService) {
    super(authService);
  }

  ngOnInit(): void {
    if (this.id > 0)
      this.loadVoucher();
  }

  loadVoucher() {
    this.apiService.getall(`vendorPayment/getVendorpaymentvoucher?id=${this.id}`).subscribe(res => {
      this.voucher = res;
    });
  }

  printVoucher() {
    const printContent = document.getElementById("printcontainer") as HTMLElement;
    const WindowPrt: any = window.open('', '', 'left=0,top=0,width=2000,height=1000,toolbar=0,scrollbars=0,status=0');
    setTimeout(() => {
      WindowPrt.document.write(printContent.innerHTML);
      WindowPrt.document.close();
      WindowPrt.focus();
      WindowPrt.print();
      WindowPrt.close();
    }, 50);   

  }

  closeModel() {
    this.dialogRef.close();
  }

}
