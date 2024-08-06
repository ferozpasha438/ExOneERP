import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';

import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CustomSelectListItem, LanCustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ParentSalesMgtComponent } from '../../../sharedcomponent/parentsalesmgt.component';

@Component({
  selector: 'app-snd-report-inventory-transactions',
  templateUrl: './snd-report-inventory-transactions.component.html'
})
export class SndReportInventoryTransactionsComponent extends ParentSalesMgtComponent implements OnInit {

  isSummary: boolean = true;
  isArab: boolean = false;
 
  isLoading: boolean = false;


  form: FormGroup;
  transactionType: string = 'Transfers';

  companyName: string = '';
  companyAddress: string = '';
  branchName: string = '';
  logoURL: any;


  dateFrom: string = new Date().toISOString();
  dateTo: string = new Date().toISOString();
  siteCode: string = '';


  reportItems: Array<any> = [];
  

  whCode: string = '';
  whSelectionList: Array<any> = [];



  

  pageNumber: number = 0;
  pageSize: number = 10;   //No.Of Invoices
  summaryPageSize = 10;
  detailedPageSize = 50;
  totalItemsCount: number = 0;

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService,
    private notifyService: NotificationService) {
    super(authService);

    


  }



  ngOnInit(): void {
    let today = new Date();
    today.setDate(today.getDate() - 7);
    this.dateFrom = today.toISOString();

    this.isArab = this.utilService.isArabic();
    
    this.loadWareHouseList();
    this.search();


  }
  clearWhCode() {
   this.whCode = '';
  }

  
  clearReport() {
    this.reportItems = [];
    }

  

  search() {

    this.isLoading = true;
    this.reportItems = [];
    this.pageSize = this.isSummary ? this.summaryPageSize : this.detailedPageSize;
    this.pageNumber = 0;
    this.apiService.getall(`sndReports/getInventoryTransactionsReport?isSummary=${this.isSummary}&transactionType=${this.transactionType}&whCode=${this.whCode}&from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}&pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`).subscribe(res => {
      
      this.totalItemsCount = res['totalItemsCount'] as number;
      
        
        if (res['reportItems'].length == 0) {
          this.notifyService.showInfo("No Data Found");
          this.isLoading = false;
          }
          else {
          this.reportItems = res['reportItems'] as Array<any>;
         
          this.companyName = res['comapnyName'];
          this.companyAddress = res['address'];
          this.branchName = res['branchName'];
          this.logoURL = res['logoURL'];

          this.loadReport(++this.pageNumber);
            
          }
        
       

       

          });

  }

  loadReport(pageNumber: number) {
    if (pageNumber > Math.floor(this.totalItemsCount / this.pageSize)) {
      
      this.isLoading = false;
    }
    else {
      this.apiService.getall(`sndReports/getInventoryTransactionsReport?isSummary=${this.isSummary}&transactionType=${this.transactionType}&whCode=${this.whCode}&from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}&pageNumber=${pageNumber}&pageSize=${this.pageSize}`).subscribe(res => {


        this.reportItems = this.reportItems.concat(res['reportItems'] as Array<any>);
        this.loadReport(++this.pageNumber);
      });
    }
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

  

  loadWareHouseList() {
    this.apiService.getall('Warehouse/getSelectWarehouseCodeList').subscribe((res: any) => {
      this.whSelectionList = res as Array<any>;

      this.whSelectionList.forEach(e => {
        e.lable = e.value + "-" + e.text;
      });
    });

  }

  getWarehouse(whCode: string) {
    return this.whSelectionList.find(e => e.value == whCode);
  }
}

