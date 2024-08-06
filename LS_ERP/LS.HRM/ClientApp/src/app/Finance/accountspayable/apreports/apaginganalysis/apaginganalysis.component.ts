import { Component, OnInit, ViewChild } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../../models/MenuItemListDto';
import { ApiService } from '../../../../services/api.service';
import { UtilityService } from '../../../../services/utility.service';
import { ParentFinMgtComponent } from '../../../../sharedcomponent/parentfinmgt.component';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { FormControl } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ReportPaginationService } from '../../../../sharedcomponent/pagination.service';

@Component({
  selector: 'app-apaginganalysis',
  templateUrl: './apaginganalysis.component.html',
  styles: [
  ]
})
export class ApaginganalysisComponent extends ParentFinMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  totalItemsCount: number = 0;

  vendorList: Array<any> = [];
  companyName: string = '';
  companyAddress: string = '';
  branchName: string = '';
  logoURL: any;
  totalBalance: any;  
  isLoading: boolean = false;
  filteredOptions: Observable<Array<CustomSelectListItem>>;
  isCodeLoading: boolean = false;
  codeControl = new FormControl('');

  constructor(private apiService: ApiService, public pageService: ReportPaginationService,
    private authService: AuthorizeService, private utilService: UtilityService) {
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
    this.search();
  }

  filter(val: string): Observable<Array<CustomSelectListItem>> {
    return this.apiService.getall(`vendor/getVendorSelectItemList?search=${val.trim()}`)
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
    this.apiService.getall(`report/getCustomerVendorAgeingAnalysisList?page=${page}&pageCount=${pageCount}&type=Vendor&custCode=${this.codeControl.value}`).subscribe(res => {
      this.isLoading = false;
      if (res != null) {
        const listItem = res['list'];
        this.vendorList = listItem['list'];
        this.totalItemsCount = listItem['reportCount'];
        //this.vendorList = res['list'];


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

