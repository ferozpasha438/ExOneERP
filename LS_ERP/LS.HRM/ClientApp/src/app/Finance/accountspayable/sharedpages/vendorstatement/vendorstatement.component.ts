
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../../services/api.service';
import { UtilityService } from '../../../../services/utility.service';


@Component({
  selector: 'app-vendorstatement',
  templateUrl: './vendorstatement.component.html',
  styles: [
  ]
})
export class VendorstatementComponent implements OnInit {
  isSavePDF: boolean = false;
  isArabic: boolean = false;
  today: any;
  custInfo: any;

  customerCode: string = '';
  list: Array<any> = [];
  totalCrAmount: number = 0;
  totalDrAmount: number = 0;
  totalBalance: number = 0;
  isLoading: boolean = false;

  constructor(private http: HttpClient, private apiService: ApiService, private utilService: UtilityService, private authService: AuthorizeService,
    public dialogRef: MatDialogRef<VendorstatementComponent>) {

  }

  ngOnInit(): void {
    this.today = new Date().toDateString();
    this.loadData();
  }


  loadData() {

    this.apiService.getall(`vendorPayment/getVendorStatementList?customerCode=${this.customerCode}`).subscribe(res => {
      if (res) {
        this.custInfo = res;
        this.list = res['list'] as any;

        this.list.forEach(item => {
          this.totalCrAmount += item['crAmount']
          this.totalDrAmount += item['drAmount']
        });

        this.totalDrAmount = parseFloat(this.totalDrAmount.toFixed(2).toString());
        this.totalCrAmount = parseFloat(this.totalCrAmount.toFixed(2).toString());
        this.totalBalance = parseFloat(this.totalDrAmount.toFixed(2).toString()) - parseFloat(this.totalCrAmount.toFixed(2).toString());

      }
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
