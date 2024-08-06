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
  selector: 'app-voucherreporting',
  templateUrl: './voucherreporting.component.html',
  styles: [
  ]
})
export class VoucherreportingComponent extends ParentFinMgtComponent implements OnInit {

  dateFrom: string = '';
  dateTo: string = '';
  remarks: string = '';
  narration: string = '';
  voucherType: string = '';
  transactionType: string = '';
  branchCode: string = '';
  docNum: string = '';


  paymentList: any = [];
  company: any;
  branchCodeControl = new FormControl();
  filteredOptions: Observable<Array<CustomSelectListItem>>;
  isBranchLoading: boolean = false;
  payCodeTypeList: Array<CustomSelectListItem> = [];
  isLoading: boolean = false;

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

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.apiService.getall("paymentcodes/getSelectPaymentTypeList").subscribe(res => {
      if (res)
        this.payCodeTypeList = res;
    });
    this.apiService.getall("ledgerReport/accountVoucherPrint").subscribe(res => {
      if (res)
        this.paymentList = res;
    });
  }

  search() {
    this.isLoading = true;
    this.apiService.getall(`ledgerReport/accountVoucherPrint?voucherType=${this.voucherType}&narration=${this.narration}&remarks=${this.remarks}&transactionType=${this.transactionType}&from=${this.dateFrom ? this.utilService.getCommonDate(this.dateFrom) : ''}&to=${this.dateTo ? this.utilService.getCommonDate(this.dateTo) : ''}&branchCode=${this.branchCode}&docNum=${this.docNum}`).subscribe(res => {
      if (res) {
        this.isLoading = false;
        this.paymentList = res['list'];
        this.company = res['company'];       
      }     
    });
  }

  openPrint() {
    const printContent = document.getElementById("printcontainer") as HTMLElement;    
    this.utilService.printForLocale(printContent);

   
  }

}
