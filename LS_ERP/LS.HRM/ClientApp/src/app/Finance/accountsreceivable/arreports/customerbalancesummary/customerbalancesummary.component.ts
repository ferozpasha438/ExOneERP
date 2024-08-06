import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { CustomSelectListItem, LanCustomSelectListItem } from '../../../../models/MenuItemListDto';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { UtilityService } from '../../../../services/utility.service';
import { ParentFinMgtComponent } from '../../../../sharedcomponent/parentfinmgt.component';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ReportPaginationService } from '../../../../sharedcomponent/pagination.service';

@Component({
  selector: 'app-customerbalancesummary',
  templateUrl: './customerbalancesummary.component.html',
  styles: [
  ]
})
export class CustomerbalancesummaryComponent extends ParentFinMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  totalItemsCount: number = 0;

  vendCode: string = '';
  isSummary: boolean = false;
  dateFrom: string = '';
  dateTo: string = '';
  siteCode: string = '';

  isArab: boolean = false;
  isSiteLoading: boolean = false;
  customerSiteList: Array<LanCustomSelectListItem> = [];
  vendorSummaryList: Array<Array<any>> = [];
  vendorList: Array<Array<any>> = [];
  codeControl = new FormControl('');
  form!: FormGroup;

  totalBalance: number = 0;
  companyName: string = '';
  companyAddress: string = '';
  branchName: string = '';
  logoURL: any;

  isLoading: boolean = false;
  filteredOptions: Observable<Array<CustomSelectListItem>>;
  isCodeLoading: boolean = false;
  openingBalance: number = 0;
  closingBalance: number = 0;


  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public pageService: ReportPaginationService,
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

    this.setForm();
    this.dateFrom = this.utilService.getStrtingYearDate();
    this.dateTo = this.utilService.getCurrentDate();
    this.search();
  }

  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      'dateFrom': [''],
      'dateTo': [''],
      'isSummary': [false],
    });
  }

  filter(val: string): Observable<Array<CustomSelectListItem>> {
    return this.apiService.getall(`customer/getCustomerCodeSelectList?search=${val.trim()}`)
      .pipe(
        map(response => {
          const res = response as Array<CustomSelectListItem>;
          //if (res && res.length == 0)
          //  this.notifyService.showError("enter branch name")
          this.isCodeLoading = false;
          return res;
        })
      )
  }

  resetSiteInfo() {
    this.siteCode = '';
  }

  onPageSwitch(event: PageEvent) {
    this.search(event.pageIndex, event.pageSize);
  }

  search(page: number = this.pageService.page, pageCount: number = this.pageService.pageCount) {
    this.isLoading = true;

    this.apiService.getall(`report/${this.isSummary ? 'getCustomerBalanceSummaryList' : 'getCustomerBalanceDetailsList'}?page=${page}&pageCount=${pageCount}&isSummary=${this.isSummary}&custCode=${this.codeControl.value}&siteCode=${this.siteCode}&from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}`).subscribe(res => {
      this.isLoading = false;

      if (res) {
        if (this.isSummary)
          this.vendorSummaryList = res['list'];
        else {

          const listItem = res['listItems'];
          this.vendorList = listItem['list'];
          this.totalItemsCount = listItem['reportCount'];
        }

        this.companyName = res['comapnyName'];
        this.companyAddress = res['address'];
        this.branchName = res['branchName'];
        this.logoURL = res['logoURL'];


        //if (!this.isSummary) {
        //  this.vendorList.forEach(lItems => {
        //    lItems.forEach(lItem => {
        //      if (lItem.isOpening == true) {
        //        //this.openingBalance = lItem.openingBalance;
        //        console.log(lItem.openingBalance);
        //      }
        //      else if (lItem.isClosing == true) {
        //        //this.closingBalance = lItem.closingBalance;
        //        console.log(lItem.closingBalance);
        //      }

        //    });
        //  });
        //}

        this.totalBalance = res['totalBalance'];
      }
    });

  }


  openPrint() {
    const printContent = document.getElementById("printcontainer") as HTMLElement;
    this.utilService.printForLocale(printContent);

    //const WindowPrt: any = window.open('', '', 'left=0,top=0,width=2000,height=1000,toolbar=0,scrollbars=0,status=0');
    //setTimeout(() => {
    //  WindowPrt.document.write(this.utilService.getPrintForLocale(printContent));
    //  WindowPrt.document.close();
    //  WindowPrt.focus();
    //  WindowPrt.print();
    //  WindowPrt.close();
    //}, 50);

  }


  customerCodeSelected(evt: MatAutocompleteSelectedEvent) {
    this.customerSiteList = [];
    this.siteCode = '';
    //console.log(evt.option.value, evt.option.viewValue);
    this.apiService.getall(`customer/getCustomerSitesSelectList/0?custCode=${evt.option.value}`).subscribe(res => {
      if (res) {
        this.customerSiteList = res;

        this.customerSiteList.map((i) => {
          i.text = !this.isArab ? i.text : i.textTwo;
          return i;
        });
      }
    });
  }


}
