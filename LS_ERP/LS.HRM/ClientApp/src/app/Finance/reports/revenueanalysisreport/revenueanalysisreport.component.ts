import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ParentFinMgtComponent } from '../../../sharedcomponent/parentfinmgt.component';
import { debounceTime, distinctUntilChanged, map, mergeMap, startWith, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-revenueanalysisreport',
  templateUrl: './revenueanalysisreport.component.html',
  styles: [
  ]
})
export class RevenueanalysisreportComponent extends ParentFinMgtComponent implements OnInit {

  List: Array<any> = [];
  isLoading: boolean = false;
  company: any;
  summary: any;
  customerList: Array<CustomSelectListItem> = [];

  dateFrom: string = '';
  dateTo: string = '';  
  customerId: string = '';

  totalSaleAmount: number = 0;
  totalPurchaseAmount: number = 0;
  totalInputTaxAomunt: number = 0;
  totalOutputTaxAomunt: number = 0;
  totalTax: number = 0;
  totalAmount: number = 0;

  branchCodeControl = new FormControl();
  options = [];
  filteredOptions!: Observable<Array<CustomSelectListItem>>;
  isBranchLoading: boolean = false;

  constructor(private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService,
    private notifyService: NotificationService) {
    super(authService);

    this.filteredOptions = this.branchCodeControl.valueChanges.pipe(
      startWith(''),
      debounceTime(utilService.autoDelay()),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isBranchLoading = true;
        return this.filter(val || '')
      })
    )

  }

  ngOnInit(): void {
    this.apiService.getall("customer/getSelectLanCustomerList").subscribe(res => {
      if (res)
        this.customerList = res;
    });
  }


  filter(val: string): Observable<Array<CustomSelectListItem>> {
    return this.apiService.getall(`branch/getSelectSysBranchList?search=${val.trim()}`)
      .pipe(
        map(response => {
          const res = response as Array<CustomSelectListItem>;
          //if (res && res.length == 0)
          //  this.notifyService.showError("enter branch name")
          this.isBranchLoading = false;
          return res;
        })
      )
  }
  search() {
    if (this.dateFrom && this.dateTo) {
      this.isLoading = true;
      //this.apiService.getall(`report/getTaxReportingPrintList?type=BS`).subscribe(res => {
      this.apiService.getall(`report/getCustomerRevenueAnalysis?custCode=${this.customerId ?? 0}&branchCode=${this.branchCodeControl.value ?? ''}&from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}`).subscribe(res => {
        this.isLoading = false;
        if (res) {
          this.List = res['list'];
          this.company = res['company'];
          this.summary = res['summary']
        }
      });
    }
    else
      this.notifyService.showError("Select Dates");
  }



  openPrint() {
    const printContent = document.getElementById("printcontainer") as HTMLElement;    
    this.utilService.printForLocale(printContent);

   
  }

}
