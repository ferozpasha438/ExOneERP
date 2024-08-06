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
  selector: 'app-snd-report-inventory-stock-ledger',
  templateUrl: './snd-report-inventory-stock-ledger.component.html'
})
export class SndReportInventoryStockLedgerComponent extends ParentSalesMgtComponent implements OnInit {

  
  isArab: boolean = false;
 
  isLoading: boolean = false;
  
  isCodeLoading: boolean = false;


  form: FormGroup;
 

  companyName: string = '';
  companyAddress: string = '';
  branchName: string = '';
  logoURL: any;


  dateFrom: string = Date.now.toString();
  dateTo: string = '';
  siteCode: string = '';

  pageNumber: number = 0;
  pageSize: number =10;   //No.Of transactions
  totalItemsCount: number = 100;

  reports: Array<any> = [];
  

  whCode: string = '';
  whSelectionList: Array<any> = [];

  itemId: string = '';
  itemSelectionList: Array<any> = [];

  showCosts: boolean = false;

  
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService,
    private notifyService: NotificationService) {
    super(authService);

   


  }



  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
       //this.dateFrom = this.utilService.getStrtingYearDate();
    let today = new Date();
    today.setDate(today.getDate() - 7);
    this.dateFrom = today.toISOString();
    this.dateTo = new Date().toISOString();

    this.loadWareHouseList();
    this.loadItemSelectionList();
    this.search();


  }
  clearCode(code: string) {
    if (code == "whCode") this.whCode = '';
    else if (code == "itemId") this.itemId = '';
  }

 
  clearReport() {
    this.reports = [];
  }

  

  loadItemSelectionList() {
    // this.apiService.getall('Item/getSelectItemList').subscribe((res: any) => {
    this.apiService.getall('Item/getSelectItemMOUList').subscribe((res: any) => {
      this.itemSelectionList = res as Array<any>;


      this.itemSelectionList.forEach(e => {
        e.lable = this.isArab ? e.textTwo : e.text;
      });
    });
  }

  search() {
    
      this.isLoading = true;
      this.reports = [];
      this.pageNumber = 0;
      this.apiService.getall(`sndReports/getInventoryStockLedgerReport?itemId=${this.itemId}&whCode=${this.whCode}&from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}&pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`).subscribe(res => {
        this.companyName = res['comapnyName'];
        this.companyAddress = res['address'];
        this.branchName = res['branchName'];
        this.logoURL = res['logoURL'];
        if (res.reportItems.length == 0) {
          this.notifyService.showInfo("No data found");
          this.isLoading = false;
        }
        else {
         
          this.totalItemsCount = res.totalItemsCount;
          this.reports = res.reportItems as Array<any>;
          
            this.loadReport(++this.pageNumber);
          
        }

      });
    
  }





  loadReport(pageNumber: number) {

    if (pageNumber > Math.floor(this.totalItemsCount / this.pageSize)) {
      this.isLoading = false;
      this.reports.forEach(e => {
       
          let sumInQty = e.transactions.reduce((sumInQty: any, current: any) => sumInQty + current.inQty, e.openingBal.inQty);
          let sumOutQty = e.transactions.reduce((sumOutQty: any, current: any) => sumOutQty + current.outQty, e.openingBal.outQty);
          let sumInCost = e.transactions.reduce((sumInCost: any, current: any) => sumInCost + current.inCost, e.openingBal.inCost);
          let sumOutCost = e.transactions.reduce((sumOutCost: any, current: any) => sumOutCost + current.outCost, e.openingBal.outCost);

          e.closingBal.inQty = sumInQty;
          e.closingBal.outQty = sumOutQty;
          e.closingBal.balanceQty = sumInQty - sumOutQty;
         e.closingBal.inCost = sumInCost;
          e.closingBal.outCost = sumOutCost;
          e.closingBal.balanceCost = sumInCost - sumOutCost;
        
        


      });
    }
    else {
      this.apiService.getall(`sndReports/getInventoryStockLedgerReport?itemId=${this.itemId}&whCode=${this.whCode}&from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}&pageNumber=${pageNumber}&pageSize=${this.pageSize}`).subscribe((res: any) => {
        //this.reports = this.reports.concat(res.reportItems);
        let resItems = res.reportItems as Array<any>;
        resItems.forEach(e => {
          let index: number = this.reports.findIndex(i => i.itemCode == e.itemCode)
          if (index >= 0) {
            this.reports[index].transactions = this.reports[index].transactions.concat(e.transactions);

          }
          else {
            this.reports.push(e);
          }
       

        });
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
}






