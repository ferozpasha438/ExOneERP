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
  selector: 'app-snd-report-inventory-stock-transactions-analysis',
  templateUrl: './snd-report-inventory-stock-transactions-analysis.component.html'
})
export class SndReportInventoryStockTransactionsAnalysisComponent extends ParentSalesMgtComponent implements OnInit {


  isArab: boolean = false;

  isLoading: boolean = false;

  isCodeLoading: boolean = false;
  showMargin: boolean = false;

  form: FormGroup;


  companyName: string = '';
  companyAddress: string = '';
  branchName: string = '';
  logoURL: any;


  dateFrom: string = '';
  dateTo: string = '';
  siteCode: string = '';

  pageNumber: number = 0;
  pageSize: number = 10;   //10 items at a time
  totalItemsCount: number = 0;

  reports: Array<any> = [];


  whCode: string = '';
  whSelectionList: Array<any> = [];

  itemId: string = '';
  itemSelectionList: Array<any> = [];




  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService,
    private notifyService: NotificationService) {
    super(authService);




  }



  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();

    //this.dateFrom = this.utilService.getStrtingYearDate();
    this.dateTo = new Date().toISOString();
    // this.dateTo = this.utilService.getCurrentDate();
    let today = new Date();
    today.setDate(today.getDate() - 7);
    this.dateFrom = today.toISOString();

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
    this.apiService.getall(`sndReports/getInventoryStockTransactionAnalysisReport?itemId=${this.itemId}&whCode=${this.whCode}&from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}&pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`).subscribe(res => {
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
        this.loadReports(++this.pageNumber);


      }
    });
  }

  loadReports(pageNumber: number) {


    if ( pageNumber>Math.floor(this.totalItemsCount / this.pageSize) ) {
      this.isLoading = false;

    }
    else {

      this.apiService.getall(`sndReports/getInventoryStockTransactionAnalysisReport?itemId=${this.itemId}&whCode=${this.whCode}&from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}&pageNumber=${pageNumber}&pageSize=${this.pageSize}`).subscribe((res: any) => {
        this.reports = this.reports.concat(res.reportItems);
        this.loadReports(++this.pageNumber);
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







