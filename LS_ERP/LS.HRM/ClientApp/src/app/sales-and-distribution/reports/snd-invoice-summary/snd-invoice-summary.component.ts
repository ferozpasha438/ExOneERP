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
  selector: 'app-snd-invoice-summary',
  templateUrl: './snd-invoice-summary.component.html'
})
export class SndInvoiceSummaryComponent extends ParentSalesMgtComponent implements OnInit {

  isSummary: boolean = true;
  isArab: boolean = false;
 
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


  dateFrom: string = new Date().toISOString();
  dateTo: string = new Date().toISOString();
  siteCode: string = '';


  summaryList: Array<any> = [];
  detailList: Array<any> = [];

  whCode: string = '';
  whSelectionList: Array<any> = [];



  totAmount: number = 0;
  totDiscount:number = 0;
  totNetAmountBT: number = 0;
  totTaxAmount: number = 0;
  totSalesAmount: number = 0;
  totCost: number = 0;
  totGrossMargin: number = 0;
  totGrossMarginPer: number = 0;

  pageNumber: number = 0;
  pageSize: number = 100;   //No.Of Invoices
  summaryPageSize = 500;
  detailedPageSize = 100;
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
    //this.dateFrom = this.utilService.getStrtingYearDate();
    //this.dateTo = this.utilService.getCurrentDate();
    let today = new Date();
    today.setDate(today.getDate() - 7);
    this.dateFrom = today.toISOString();
    this.loadWareHouseList();
    this.search();


  }
  clearWhCode() {
    this.whCode = '';
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

  

  search() {
    this.isLoading = true;
    this.summaryList = [];
    this.detailList = [];
    this.pageSize = this.isSummary ? this.summaryPageSize : this.detailedPageSize;
    this.pageNumber = 0;
    this.apiService.getall(`sndReports/getSndInvoiceReport?isSummary=${this.isSummary}&type=${this.type}&custCode=${this.codeControl.value}&whCode=${this.whCode}&from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}&pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`).subscribe(res => {
      this.totalItemsCount = res['totalItemsCount'] as number;
      if (res) {
        if (this.isSummary) {
          if (res['summaryList'].length == 0) {
            this.notifyService.showInfo("No Data Found");
            this.isLoading = false;
          }
          else {
            this.summaryList = res['summaryList'] as Array<any>;
    
              this.loadReport(++this.pageNumber);
            
          }
        }
        else {
          if (res['detailList'].length == 0) {
            this.notifyService.showInfo("No Data Found");
          }
          else {
            this.detailList = res['detailList'] as Array<any>;
            this.loadReport(++this.pageNumber);
          }
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

      }
    });

  }

  loadReport(pageNumber:number)
  {
    if (pageNumber>Math.floor(this.totalItemsCount / this.pageSize)) {
      this.isLoading = false;
    }
    else {
      this.apiService.getall(`sndReports/getSndInvoiceReport?isSummary=${this.isSummary}&type=${this.type}&custCode=${this.codeControl.value}&whCode=${this.whCode}&from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}&pageNumber=${pageNumber}&pageSize=${this.pageSize}`).subscribe(res => {
        if (this.isSummary) {
          this.summaryList = this.summaryList.concat(res['summaryList'] as Array<any>);
        }
        else {
          this.detailList = this.detailList.concat(res['detailList'] as Array<any>);
        }


        this.totAmount += res['totAmount'];
        this.totDiscount += res['totDiscount'];
        this.totNetAmountBT += res['totNetAmountBT'];
        this.totSalesAmount += res['totSalesAmount'];
        this.totTaxAmount += res['totTaxAmount'];
        this.totCost += res['totCost'];
        this.totGrossMargin += res['totGrossMargin'];
        
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
