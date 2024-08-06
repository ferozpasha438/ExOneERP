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
  selector: 'app-snd-report-customer-sales',
  templateUrl: './snd-report-customer-sales.component.html'
})
export class SndReportCustomerSalesComponent extends ParentSalesMgtComponent implements OnInit {

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

 



  totAmount: number = 0;
  totDiscount: number = 0;
  totNetAmountBT: number = 0;
  totTaxAmount: number = 0;
  totSalesAmount: number = 0;
  totCost: number = 0;
  totGrossMargin: number = 0;
  totGrossMarginPer: number = 0;
  totCount: number = 0;


  pageNumber: number = 0;
  pageSize: number = 1;   //No.Of Customers
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

   // this.dateTo = this.utilService.getCurrentDate();
    this.dateTo = new Date().toISOString();
    this.search();


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
    this.totAmount = 0;
    this.totDiscount = 0;
    this.totNetAmountBT =0;
    this.totSalesAmount = 0;
    this.totTaxAmount = 0;
    this.totCost = 0;
    this.totGrossMargin =0;
    this.totGrossMarginPer = 0;
    this.totCount = 0;
    this.pageNumber = 0;
  }


  search() {
    this.isLoading = true;
    //this.summaryList = [];
    //this.detailList = [];
    //this.pageNumber = 0;
    this.clearReport();
    this.apiService.getall(`sndReports/getCustomerSales?isSummary=${this.isSummary}&type=${this.type}&custCode=${this.codeControl.value}&from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}&pageSize=${this.pageSize}&pageNumber=${this.pageNumber}`).subscribe(res => {
      this.totalItemsCount = res['totalItemsCount'] as number;



      if (res['summaryReport'].length == 0)
      {
        this.notifyService.showInfo("No Data Found");
        this.isLoading = false;
        }
      else
      {
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
        this.totCount = res['totCount'];

        if (this.isSummary)
        {
            this.summaryList = res['summaryReport'] as Array<any>;
          }
        else
        {
            this.detailList = res['summaryReport'] as Array<any>;
          }
          
          this.loadReports(++this.pageNumber);

        }

    });

  }
  loadReports(pageNumber: number) {
    if (pageNumber > Math.floor(this.totalItemsCount / this.pageSize)) {
      this.isLoading = false;
      this.totGrossMarginPer = (this.totGrossMargin / Math.abs(this.totSalesAmount)) * 100;


    }
    else {
      this.apiService.getall(`sndReports/getCustomerSales?isSummary=${this.isSummary}&type=${this.type}&custCode=${this.codeControl.value}&from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}&pageSize=${this.pageSize}&pageNumber=${pageNumber}`).subscribe(res => {
        if (this.isSummary) {
          this.summaryList = this.summaryList.concat(res['summaryReport'] as Array<any>);
        }
        else {
          this.detailList = this.detailList.concat(res['summaryReport'] as Array<any>);
        }


      

        this.totAmount += res['totAmount'];
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

  

  
}

