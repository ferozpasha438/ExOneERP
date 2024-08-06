import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';

@Component({
  selector: 'app-feeinvoice',
  templateUrl: './feeinvoice.component.html',
  styles: [
  ]
})
export class FeeinvoiceComponent implements OnInit {
  receiptVoucher: string = '';
  isSavePDF: boolean = false;
  isArabic: boolean = false;
  result: Array<any> = [];
  isLoading: boolean = false;
  layout1: boolean = false;
  layout2: boolean = false;
  voucherDate: string = '';
  voucherNum: string = '';
  studentName: string = '';
  stuAdmNum: string = '';
  totalFeeAmount: any = '';
  selectedType: string = '';
  payCode: string = '';
  constructor(private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<FeeinvoiceComponent>,
    private notifyService: NotificationService) {

  }

  ngOnInit(): void {
    this.selectedType = 'layout1';
    if (this.receiptVoucher != '')
      this.loadView();
    this.isArabic = this.authService.isArabic();
  }

  loadView() {
    this.isLoading = true;
    this.apiService.getall(`StudentFeeTransaction/GetFeeTransactionDetails/${this.receiptVoucher}`).subscribe(res => {
      if (res) {
        this.isLoading = false;
        this.result = res.termFeeDetails;
        this.layout1 = true;
        this.layout2 = false;
        this.voucherDate = res.voucherDate;
        this.voucherNum = res.voucherNumber;
        this.totalFeeAmount = res.totalFeeAmount;
        this.stuAdmNum = res.admissionNumber;
        this.payCode = res.payCode;
        this.studentName = this.authService.isArabic() ? res.studentName2 : res.studentName;
      }
    });
  }

  closeModel() {
    this.dialogRef.close();
  }

  printInvoice() {
    this.isSavePDF = false;
    this.openPrint();
  }

  openPrint() {
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
  changeOption() {
    if (this.selectedType=='layout1') {
      this.layout1 = true;
      this.layout2 = false;
    } else {
      this.layout1 = false;
      this.layout2 = true;
    }
  }
  saveAsPDF() {
    setTimeout(() => {
      this.isSavePDF = true;
      this.openPrint();
    }, 600);
  }


}
