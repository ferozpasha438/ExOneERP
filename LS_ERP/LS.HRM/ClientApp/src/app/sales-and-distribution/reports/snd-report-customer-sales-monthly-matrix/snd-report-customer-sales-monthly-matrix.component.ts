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
  selector: 'app-snd-report-customer-sales-monthly-matrix',
  templateUrl: './snd-report-customer-sales-monthly-matrix.component.html'
})
export class SndReportCustomerSalesMonthlyMatrixComponent extends ParentSalesMgtComponent implements OnInit {

  
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


  monthlyReports: Array<any> = [];
  monthlyTotals: Array<any> = [];
  columns: Array<any> = [];
  


  totAmount: number = 0;
  totDiscount: number = 0;
  totNetAmountBT: number = 0;
  totTaxAmount: number = 0;
  totSalesAmount: number = 0;
  totCost: number = 0;
  totGrossMargin: number = 0;
  totGrossMarginPer: number = 0;

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
    this.monthlyReports = [];
  }

 
  search() {
    this.isLoading = true;
    this.monthlyReports = [];
    this.pageNumber = 0;

    this.apiService.getall(`sndReports/getCustomerSalesMonthlyReport?custCode=${this.codeControl.value}&from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}&pageNumber=${0}&pageSize=${this.pageSize}`).subscribe(res => {
     

      if (res) {
        this.totalItemsCount = res['totalItemsCount'] as number;
        if (res['monthlyReports'].length == 0) {
          this.notifyService.showInfo("No Data Found");
          this.isLoading = false;
        }
        else {
          this.monthlyReports = res['monthlyReports'] as Array<any>;
          this.monthlyTotals = res['monthlyTotals'] as Array<any>;
          this.columns = res['columns'] as Array<any>;
         
            this.loadReport(++this.pageNumber);
          

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
        this.columns = res['columns'] as Array<any>;
      }

      setTimeout(() => {

      }, 5000);
    });

  }


  loadReport(pageNumber: number) {

    if (pageNumber > Math.floor(this.totalItemsCount / this.pageSize)) {
      this.isLoading = false;
      let totCost = this.monthlyTotals.reduce((totCost: any, current: any) => totCost + current.cost, 0);
      let totSalesAmount = this.monthlyTotals.reduce((totSalesAmount: any, current: any) => totSalesAmount + current.salesAmount, 0);

      this.totCost = totCost;
      this.totSalesAmount = totSalesAmount;

      this.totGrossMarginPer = (this.totGrossMargin /  this.totSalesAmount) * 100;
    }
    else {

      this.apiService.getall(`sndReports/getCustomerSalesMonthlyReport?custCode=${this.codeControl.value}&from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}&pageNumber=${pageNumber}&pageSize=${this.pageSize}`).subscribe(res => {
        this.monthlyReports = this.monthlyReports.concat(res['monthlyReports'] as Array<any>);
        let mTotals = res['monthlyTotals'] as Array<any>;
        this.monthlyTotals.forEach(e => {
          e.cost += (mTotals.find(r => r.monthDt == e.monthDt).cost) ?? 0;
          e.salesAmount += mTotals.find(r => r.monthDt == e.monthDt).salesAmount ?? 0;
          e.grossMargin += (e.salesAmount - e.cost);
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

  
 
}
