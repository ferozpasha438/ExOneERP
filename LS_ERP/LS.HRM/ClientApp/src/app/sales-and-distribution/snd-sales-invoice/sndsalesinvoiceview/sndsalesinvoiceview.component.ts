import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ParentSalesMgtComponent } from '../../../sharedcomponent/parentsalesmgt.component';

@Component({
  selector: 'app-sndsalesinvoiceview',
  templateUrl: './sndsalesinvoiceview.component.html'
})
export class SndsalesinvoiceviewComponent extends ParentSalesMgtComponent implements OnInit {

  id: number = 0;

  isSavePDF: boolean = false;
  isArabic: boolean = false;
  result: any;
  isLoading: boolean = false;
  printFormats: Array<any> = [{text:"format1",value:"format1"},{text:"format2",value:"format2"},{text:"format3",value:"format3"},{text:"format4",value:"format4"},{text:"format5",value:"format5"},{text:"format6",value:"format6"},{text:"format7",value:"format7"},{text:"format8",value:"format8"},{text:"format9",value:"format9"},{text:"format10",value:"format10"}];

  format: string = "format1";
  constructor(private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<SndsalesinvoiceviewComponent>,
    private notifyService: NotificationService) {
    super(authService);
  }

  ngOnInit(): void {
    if (this.id > 0)
      this.loadSalesView();
   
   
   
  }

  loadSalesView() {
    this.isLoading = true;
    this.apiService.get("generateSndInvoice/generateSndInvoiceReportById", this.id).subscribe(res => {
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

  saveAsPDF() {
    setTimeout(() => {
      this.isSavePDF = true;
      this.openPrint();
    }, 600);
  }


}
