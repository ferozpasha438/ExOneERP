import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';

@Component({
  selector: 'app-salesinvoiceview',
  templateUrl: './salesinvoiceview.component.html',
  styles: [
  ]
})
export class SalesinvoiceviewComponent implements OnInit {

  id: number = 0;

  isSavePDF: boolean = true;
  isArabic: boolean = false;
  result: any;
  isLoading: boolean = false;
  OpacityValue: number = 1;
  constructor(private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<SalesinvoiceviewComponent>,
    private notifyService: NotificationService) {

  }

  ngOnInit(): void {
    if (this.id > 0)
      this.loadSalesView();
  }

  loadSalesView() {
    this.isLoading = true;
    this.apiService.get("generateInvoice/generateInvoiceReportById", this.id).subscribe(res => {
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
    this.OpacityValue = 1;
    const printContent = document.getElementById("printcontainer") as HTMLElement;
    const WindowPrt: any = window.open('', '', 'left=0,top=0,width=2000,height=1000,toolbar=0,scrollbars=0,status=0');
    setTimeout(() => {
      //var content = printContent.innerHTML.replace(/border: 0.3px solid #878787!important;/gi, '456_456');
      //content = content.replace(/456_456/gi, 'border:0.3px ridge #878787!important;');
      //console.log(content);
      //WindowPrt.document.write(content);
      //WindowPrt.document.write(printContent.innerHTML.replace(/border:2px solid #878787!important;/gi,'border:0.5px solid #878787!important;'));
      WindowPrt.document.write(`<html><head><style>table, th, td {
    
    }</style></head><body>` + printContent.innerHTML + '</body > </html>');
      WindowPrt.document.close();
      WindowPrt.focus();
      WindowPrt.print();
      WindowPrt.close();
    }, 50);

    //border-width:medium;
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
