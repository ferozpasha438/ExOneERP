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
  selector: 'app-snd-item-sales-summary',
  templateUrl: './snd-item-sales-summary.component.html'
})
export class SndItemSalesSummaryComponent extends ParentSalesMgtComponent implements OnInit {

  isSummary: boolean = true;
  isArab: boolean = false;
  customerSiteList: Array<LanCustomSelectListItem> = [];
  codeControl = new FormControl('');
  isLoading: boolean = false;
  filteredOptions: Observable<Array<CustomSelectListItem>>;
  isCodeLoading: boolean = false;


  form: FormGroup;
  type: string = 'All';

  companyName: string = '';
  companyAddress: string = '';
  branchName: string = '';
  logoURL: any;


  dateFrom: string = '';
  dateTo: string = '';
  siteCode: string = '';


  summaryList: Array<any> = [];
  detailList: Array<any> = [];

  whCode: string = '';
  whSelectionList: Array<any> = [];

 itemId: string = '';
  itemSelectionList: Array<any> = [];



  totAmount: number = 0;
  totDiscount: number = 0;
  totNetAmountBT: number = 0;
  totTaxAmount: number = 0;
  totSalesAmount: number = 0;
  totCost = 0;
  totGrossMargin = 0;
  totGrossMarginPer = 0;

  pageNumber:number = 0;
  pageSize: number = 1;//Items per page
  summaryPageSize: number = 50;
  detailedPageSize: number = 1;

  totalItemsCount: number = 0;

  showMargin: boolean = false;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService,
    private notifyService: NotificationService) {
    super(authService);

    this.filteredOptions = this.codeControl.valueChanges.pipe(
      startWith(''),
      debounceTime(400),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isCodeLoading = true;
        return this.filter(val || '')
      })
    );


  }



  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    let today = new Date();
    today.setDate(today.getDate() - 7);
    this.dateFrom = today.toISOString();

    //this.dateTo = this.utilService.getCurrentDate();
    this.dateTo = new Date().toISOString();
    this.loadWareHouseList();
    this.loadItemSelectionList();
    this.search();


  }
  clearCode(code:string) {
    if (code == "whCode") this.whCode = '';
    else if (code == "itemId") this.itemId = '';
  }

  filter(val: string): Observable<Array<CustomSelectListItem>> {
    return this.apiService.getall(`customer/getCustomerCodeSelectList?search=${val.trim()}`)
      .pipe(
        map(response => {
          const res = response as Array<CustomSelectListItem>;
          this.isCodeLoading = false;
          return res;
        })
      )
  }
  clearReport() {
    this.summaryList = [];
    this.detailList = [];
  }


  loadItemSelectionList() {
   // this.apiService.getall('Item/getSelectItemList').subscribe((res: any) => {
    this.apiService.getall('Item/getSelectItemMOUList').subscribe((res: any) => {
      this.itemSelectionList = res as Array<any>;


      this.itemSelectionList.forEach(e => {
        e.lable = this.isArab?e.textTwo:e.text;
      });
    });
  }

  search() {
    this.isLoading = true;
    this.summaryList = [];
    this.detailList = [];
    this.pageNumber = 0;
    this.pageSize = this.isSummary ? this.summaryPageSize : this.detailedPageSize;
    this.apiService.getall(`sndReports/getSndItemSalesReport?isSummary=${this.isSummary}&type=${this.type}&custCode=${this.codeControl.value}&itemId=${this.itemId}&whCode=${this.whCode}&from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}&pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`).subscribe(res => {
     
      if (res['summaryReport'].length == 0) {
        this.notifyService.showInfo("No Data Found");
        this.isLoading = false;
      }
      else {
        if (this.isSummary) {
          this.summaryList = res['summaryReport'] as Array<any>;
        }
        else {
          this.detailList = res['detailedReport'] as Array<any>;
        }



        this.companyName = res['comapnyName'];
        this.companyAddress = res['address'];
        this.branchName = res['branchName'];
        this.logoURL = res['logoURL'];

        this.totAmount = res['totAmount'];
        this.totDiscount = res['totDiscount'];
        this.totNetAmountBT = res['totNetAmountBT'];
        this.totSalesAmount = res['totSalesAmount'];
        this.totTaxAmount = res['totTaxAmount'];
        this.totCost = res['totCost'];
        this.totGrossMargin = res['totGrossMargin'];
        this.totGrossMarginPer = res['totGrossMarginPer'];
        this.totalItemsCount = res['totalItemsCount'];
        this.loadReports(++this.pageNumber);
      }
      });

  }

  loadReports(pageNumber:number) {
    if (pageNumber > Math.floor(this.totalItemsCount / this.pageSize)) {
      this.isLoading = false;
      this.totGrossMarginPer = this.totSalesAmount == 0 ? 0 : this.totGrossMargin / this.totSalesAmount * 100;
    }
    else {
      this.apiService.getall(`sndReports/getSndItemSalesReport?isSummary=${this.isSummary}&type=${this.type}&custCode=${this.codeControl.value}&itemId=${this.itemId}&whCode=${this.whCode}&from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}&pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`).subscribe(res => {
        
          if (this.isSummary) {
            this.summaryList = this.summaryList.concat(res['summaryReport'] as Array<any>);
          }
          else {
            this.detailList = this.detailList.concat(res['detailedReport'] as Array<any>);
          }
        

        this.totAmount+= res['totAmount'];
        this.totDiscount += res['totDiscount'];
        this.totNetAmountBT += res['totNetAmountBT'];
        this.totSalesAmount += res['totSalesAmount'];
        this.totTaxAmount += res['totTaxAmount'];
        this.totCost += res['totCost'];
        this.totGrossMargin += res['totGrossMargin'];

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
