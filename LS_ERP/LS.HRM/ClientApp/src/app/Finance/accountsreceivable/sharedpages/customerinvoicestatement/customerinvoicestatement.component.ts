
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../../services/api.service';

@Component({
  selector: 'app-customerinvoicestatement',
  templateUrl: './customerinvoicestatement.component.html',
  styles: [
  ]
})
export class CustomerinvoicestatementComponent implements OnInit {

  isSavePDF: boolean = false;
  isArabic: boolean = false;

  customerCode: string = '';
  list: Array<any> = [];
  constructor(private http: HttpClient, private apiService: ApiService, private authService: AuthorizeService,
    public dialogRef: MatDialogRef<CustomerinvoicestatementComponent>) {

  }

  ngOnInit(): void {
    this.loadData();
  }


  loadData() {

    this.apiService.getall(`customerPayment/getCustomerInvoiceStatementList?customerCode=${this.customerCode}`).subscribe(res => {
      if (res)
        this.list = res as any;
    });

    //this.http.get(`${this.authService.GetSystemSetupApiEndPoint()}/customerPayment/getCustomerStatementList?customerCode=${this.customerCode}`).subscribe(res => {
    //  if (res)
    //    this.list = res as any;
    //});
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
    this.isSavePDF = true;
    this.openPrint();
  }

}

