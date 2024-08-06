import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';

@Component({
  selector: 'app-purchaseinvoice',
  templateUrl: './purchaseinvoice.component.html',
  styles: [
  ]
})
export class PurchaseinvoiceComponent implements OnInit {

  id: number = 0;

  isSavePDF: boolean = true;
  isArabic: boolean = false;
  result: any;
  isLoading: boolean = false;

  constructor(private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<PurchaseinvoiceComponent>,
    private notifyService: NotificationService) {

  }

  ngOnInit(): void {
    if (this.id > 0)
      this.loadSalesView();
  }

  loadSalesView() {
    this.isLoading = true;
    this.apiService.get("generateApInvoice/generateApInvoiceReportById", this.id).subscribe(res => {
      this.isLoading = false;
      if (res) {        
        this.result = res;
        this.isArabic = this.authService.isArabic();
      }
    });
  }

  closeModel() {
    this.dialogRef.close();
  }

  printInvoice() {
    this.isSavePDF = true;
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

    //let printContents = document.getElementById(cmpName).innerHTML;
    //let originalContents = document.body.innerHTML;
    //document.body.innerHTML = printContent.innerHTML;
    //window.print();


    //$('#topHeader').hide();
    //$('#bottomFooter').hide();
    //$('.print-container').kinziPrint({
    //  importCSS: true,
    //  header: '',
    //  footer: ''
    //});

  }

  saveAsPDF() {
    setTimeout(() => {
      this.isSavePDF = false;
      this.openPrint();
    }, 600);
  }


}
