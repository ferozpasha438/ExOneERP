import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../../models/MenuItemListDto';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { UtilityService } from '../../../../services/utility.service';
import { ParentFinMgtComponent } from '../../../../sharedcomponent/parentfinmgt.component';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ReportPaginationService } from '../../../../sharedcomponent/pagination.service';

@Component({
  selector: 'app-opmcustomersummary',
  templateUrl: './opmcustomersummary.component.html',
  styles: [
  ]
})
export class OpmcustomersummaryComponent extends ParentFinMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  totalItemsCount: number = 0;

  vendCode: string = '';
  isAllCustomers: boolean = false;
  dateFrom: string = '';
  dateTo: string = '';
  type: string = 'All';

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
      'isAllBranches': [false],
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

  onPageSwitch(event: PageEvent) {
    this.search(event.pageIndex, event.pageSize);
  }

  search(page: number = this.pageService.page, pageCount: number = this.pageService.pageCount) {
    this.isLoading = true;

    this.apiService.getall(`report/getOpmCustomerSummaryList?page=${page}&pageCount=${pageCount}&isAllCustomers=${this.isAllCustomers}&type=${this.type}&custCode=${this.codeControl.value}&from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}`).subscribe(res => {
      this.isLoading = false;

      if (res) {
        const listItem = res['list'];
        this.vendorList = listItem['list'];
        this.totalItemsCount = listItem['reportCount'];
        
        this.companyName = res['comapnyName'];
        this.companyAddress = res['address'];
        this.branchName = res['branchName'];
        this.logoURL = res['logoURL'];

        this.totalBalance = res['totalBalance'];
      }
    });

  }


  openPrint() {
    const printContent = document.getElementById("printcontainer") as HTMLElement;    
    this.utilService.printForLocale(printContent);
   
  }



}

