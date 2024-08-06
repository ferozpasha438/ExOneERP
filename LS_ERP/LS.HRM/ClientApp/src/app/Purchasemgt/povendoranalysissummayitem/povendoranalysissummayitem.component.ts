import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../models/MenuItemListDto';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';
import { ParentPurchaseMgtComponent } from '../../sharedcomponent/parentpurchasemgt.component';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-povendoranalysissummayitem',
  templateUrl: './povendoranalysissummayitem.component.html',
  styles: [
  ]
})
export class PovendoranalysissummayItemComponent extends ParentPurchaseMgtComponent implements OnInit {

  vendCode: string = '';
  isSummary: boolean = false;
  dateFrom: string = '';
  dateTo: string = '';
  vendorSummaryList: Array<Array<any>> = [];
  vendorList: Array<Array<any>> = [];
  codeControl = new FormControl('');
  //form: FormGroup;

  companyName: string = '';
  companyAddress: string = '';
  branchName: string = '';
  logoURL: any;

  isLoading: boolean = false;
  filteredOptions: Observable<Array<CustomSelectListItem>>;
  isCodeLoading: boolean = false;
  openingBalance: number = 0;
  closingBalance: number = 0;

  tranTotalCost: number = 0;
  totalItemQty: number = 0;
  taxes: number = 0;
  tranDiscAmount: number = 0;
  totalBalance: number = 0;
  netTotalBalanceAmount: number = 0;

  isBranchLoading: boolean = false;
  branchCodeControl = new FormControl('');
  filteredBranchOptions: Observable<Array<CustomSelectListItem>>;

  isItemLoading: boolean = false;
  itemControl = new FormControl('');
  filteredItemsOptions: Observable<Array<CustomSelectListItem>>;

  constructor(private apiService: ApiService,
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

    this.filteredItemsOptions = this.itemControl.valueChanges.pipe(
      startWith(''),
      debounceTime(400),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isItemLoading = true;
        return this.filterItems(val || '')
      })
    );

    this.filteredBranchOptions = this.branchCodeControl.valueChanges.pipe(
      startWith(''),
      debounceTime(400),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isBranchLoading = true;
        return this.filterBranch(val || '')
      })
    );


  }

  ngOnInit(): void {
    //this.setForm();
  }

  //setForm() {
  //  //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
  //  this.form = this.fb.group({
  //    'dateFrom': [''],
  //    'dateTo': [''],
  //    'isSummary': [false],
  //  });
  //}

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

  filterBranch(val: string): Observable<Array<CustomSelectListItem>> {
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

  filterItems(val: string): Observable<Array<CustomSelectListItem>> {
    return this.apiService.getall(`purchaseOrder/GetItemNameSelectList?search=${val.trim()}`)
      .pipe(
        map(response => {
          const res = response as Array<CustomSelectListItem>;
          //if (res && res.length == 0)
          //  this.notifyService.showError("enter branch name")
          this.isItemLoading = false;
          return res;
        })
      )
  }

  search() {
    this.isLoading = true;

    this.apiService.getall(`purchaseReport/getpoItemAnalysisSummary?type=povendanls&itemCode=${this.itemControl.value}&branchCode=${this.branchCodeControl.value}&vendCode=${this.codeControl.value}&from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}`).subscribe(res => {
      this.isLoading = false;

      if (res) {
        //if (this.isSummary)
        //  this.vendorSummaryList = res['list'];
        //else

        this.vendorList = res['listItems'];
        this.companyName = res['comapnyName'];
        this.companyAddress = res['address'];
        this.branchName = res['branchName'];
        this.logoURL = res['logoURL'];


        this.tranTotalCost = res['totalDrAmount'];
        this.tranDiscAmount = res['totalOpeningAmount'];
        this.totalBalance = res['totalBalance'];
        this.taxes = res['totalCrAmount'];
        this.netTotalBalanceAmount = res['netTotalBalanceAmount'];
        this.totalItemQty = res['totalItemQty'];

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

        // this.totalBalance = res['totalBalance'];
      }
    });

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
