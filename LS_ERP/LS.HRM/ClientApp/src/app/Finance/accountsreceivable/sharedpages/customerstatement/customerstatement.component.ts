import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../../services/api.service';

@Component({
  selector: 'app-customerstatement',
  templateUrl: './customerstatement.component.html',
  styles: [
  ]
})
export class CustomerstatementComponent implements OnInit {
  isSavePDF: boolean = false;
  isArabic: boolean = false;
  today: any;
  //companyName: string = '';
  //companyAddress: string = '';
  //branchName: string = '';
  //logoURL: any;

  //CustomerName: string = '';
  //CustCode: string = '';
  //CustAddress1: string = '';
  //CustAddress2: string = '';

  custInfo: any;

  customerCode: string = '';
  list: Array<any> = [];
  totalCrAmount: number = 0;
  totalDrAmount: number = 0;
  totalBalance: number = 0;
  isLoading: boolean = false;

  constructor(private http: HttpClient, private apiService: ApiService, private authService: AuthorizeService,
    public dialogRef: MatDialogRef<CustomerstatementComponent>) {

  }

  ngOnInit(): void {
    this.today = new Date().toDateString();
    this.loadData();
  }


  loadData() {
    this.isLoading = true;
    this.apiService.getall(`customerPayment/getCustomerStatementList?customerCode=${this.customerCode}`).subscribe(res => {
      this.isLoading = false;
      if (res) {
        this.custInfo = res;
        this.list = res['list'] as any;

        //this.companyName = res['comapnyName'];
        //this.companyAddress = res['address'];
        //this.branchName = res['branchName'];
        //this.logoURL = res['logoURL'];

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
