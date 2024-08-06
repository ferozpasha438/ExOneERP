import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../../services/api.service';
import { UtilityService } from '../../../../services/utility.service';

@Component({
  selector: 'app-vendorinvoicestatement',
  templateUrl: './vendorinvoicestatement.component.html',
  styles: [
  ]
})
export class VendorinvoicestatementComponent implements OnInit {

  isSavePDF: boolean = false;
  isArabic: boolean = false;

  customerCode: string = '';
  list: Array<any> = [];
  constructor(private http: HttpClient, private apiService: ApiService, private utilService: UtilityService, private authService: AuthorizeService,
    public dialogRef: MatDialogRef<VendorinvoicestatementComponent>) {

  }

  ngOnInit(): void {
    this.loadData();
  }


  loadData() {

    this.apiService.getall(`vendorPayment/getVendorInvoiceStatementList?customerCode=${this.customerCode}`).subscribe(res => {
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
    this.utilService.printForLocale(printContent);

   

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

