import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { CustomSelectListItem } from '../../models/MenuItemListDto';
import { ParentPurchaseMgtComponent } from '../../sharedcomponent/parentpurchasemgt.component';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-purchaseordersummary',
  templateUrl: './purchaseordersummary.component.html',
  styles: [
  ]
})
export class PurchaseordersummaryComponent extends ParentPurchaseMgtComponent implements OnInit {

  vendCode: string = '';
  isAllVendors: boolean = false;
  dateFrom: string = '';
  dateTo: string = '';
  type: string = 'All';
  orderType: string = 'po';
  //branchCode: string = '';

  vendorList: Array<Array<any>> = [];
  codeControl = new FormControl('');
  form: FormGroup;

  tranTotalCost: number = 0;
  taxes: number = 0;
  tranDiscAmount: number = 0;
  totalBalance: number = 0;
  companyName: string = '';
  companyAddress: string = '';
  branchName: string = '';
  logoURL: any;

  isLoading: boolean = false;
  filteredOptions: Observable<Array<CustomSelectListItem>>;
  isCodeLoading: boolean = false;

  isBranchLoading: boolean = false;
  branchCodeControl = new FormControl('');
  filteredBranchOptions: Observable<Array<CustomSelectListItem>>;

  isItemLoading: boolean = false;
  itemControl = new FormControl('');
  filteredItemsOptions: Observable<Array<CustomSelectListItem>>;



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
    // this.setForm();
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

  search() {
    this.isLoading = true;

    this.apiService.getall(`purchaseReport/getPurchaseOrderSummaryList?orderType=${this.orderType}&isAllVendors=${this.isAllVendors}&type=${this.type}&itemCode=${this.itemControl.value}&branchCode=${this.branchCodeControl.value}&vendCode=${this.codeControl.value}&from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}`).subscribe(res => {
      this.isLoading = false;

      if (res) {
        this.vendorList = res['list'];
        this.companyName = res['comapnyName'];
        this.companyAddress = res['address'];
        this.branchName = res['branchName'];
        this.logoURL = res['logoURL'];

        this.tranTotalCost = res['totalDrAmount'];
        this.tranDiscAmount = 0.00;// res['totalOpeningAmount'];
        this.taxes = res['totalCrAmount'];
        this.totalBalance = res['totalBalance'];
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

