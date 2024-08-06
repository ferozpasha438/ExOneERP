import { Component, Input, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../../services/api.service';
//import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { UtilityService } from '../../../../services/utility.service';
import { default as data } from "../../../../../assets/i18n/siteConfig.json";

  @Component({
    selector: 'app-schoolinvoiceprinting',
    templateUrl: './schoolinvoiceprinting.component.html',
    styles: [
    ]
  })
  export class SchoolinvoiceprintingComponent implements OnInit {

  id: number = 0;
  isLoading: boolean = false;
  isArabic: boolean = false;
  item: any;
  company: any;
    companyData: any;
    bank: any;
  itemList: any;
  dashBoardType: string = '';
  constructor(private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<SchoolinvoiceprintingComponent>,
    private notifyService: NotificationService) {

  }


    ngOnInit(): void {
    this.dashBoardType = data.dashBoardType;
    if (this.id > 0)
      this.loadSalesView();
  }

  loadSalesView() {
    this.isLoading = true;
    this.apiService.get("GenerateInvoice/getSchoolSalesInvoicePrintingList", this.id).subscribe(res => {
      this.isLoading = false;
      if (res) {
        this.item = res;
        this.itemList = res['itemList'];
        this.company = res['company'];
        this.companyData = res['companyData'];
        this.bank = res['bank'];
        this.isArabic = this.authService.isArabic();
      }
    });
  }


    printInvoice() {
      const printContent = document.getElementById("printcontainer") as HTMLElement;      
      this.utilService.printForLocale(printContent);

      //const WindowPrt: any = window.open('', '', 'left=0,top=0,width=2000,height=1000,toolbar=0,scrollbars=0,status=0');
      //setTimeout(() => {
      //  WindowPrt.document.write(printContent.innerHTML);
      //  WindowPrt.document.close();
      //  WindowPrt.focus();
      //  WindowPrt.print();
      //  WindowPrt.close();
      //}, 50);
    }


    closeModel() {
      this.dialogRef.close();
    }
    hasNonArabChars(text: string): boolean {
      const hasMatch = /^[A-Za-z0-9]*$/.test(text.trim().replace(/ /g, ''));
      return hasMatch;
    }
}

