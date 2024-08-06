import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../../models/MenuItemListDto';
import { ApiService } from '../../../../services/api.service';
//import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { UtilityService } from '../../../../services/utility.service';
import { ValidationService } from '../../../../sharedcomponent/ValidationService';

  @Component({
    selector: 'app-salesinvoiceprinting',
    templateUrl: './salesinvoiceprinting.component.html',
    styles: [
    ]
  })
  export class SalesinvoiceprintingComponent implements OnInit {

  List: Array<any> = [];
  company: any;
  dateFrom: string = '';
  dateTo: string = '';
  customerId: string = '';

  branchCodeControl = new FormControl();
  filteredOptions!: Observable<Array<CustomSelectListItem>>;
  customerList: Array<CustomSelectListItem> = [];
  isBranchLoading: boolean = false;
  isLoading: boolean = false;

  constructor(private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<SalesinvoiceprintingComponent>) {

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

    this.apiService.getall("customer/getSelectCustomerList").subscribe(res => {
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
    //console.log(this.dateFrom, this.dateTo, this.customerId, this.branchCodeControl.value );
    if (this.dateFrom && this.dateTo) {


      this.isLoading = true;
      //this.apiService.getall(`report/getTaxReportingPrintList?type=BS`).subscribe(res => {
      this.apiService.getall(`generateInvoice/getSalesInvoicePrintingList?id=${this.customerId ?? 0}&branchCode=${this.branchCodeControl.value ?? ''}&from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}`).subscribe(res => {
        this.isLoading = false;
        if (res) {
          this.List = res['itemPrintingList'];
          this.company = res['company'];                  
        }
      });

    }
    else
      this.notifyService.showError("Select Dates");

  }


  openPrint() {
    const printContent = document.getElementById("printcontainer") as HTMLElement;    
    this.utilService.printForLocale(printContent);

    //const WindowPrt: any = window.open('', '', 'left=0,top=0,width=2000,height=1000,toolbar=0,scrollbars=0,status=0');
    //setTimeout(() => {
    //  WindowPrt.document.write(printContent.innerHTML);
    //  WindowPrt.document.close();
    //  WindowPrt.focus();
    //  WindowPrt.print();
    //  WindowPrt.close();
    //}, 50);
  }

  closeModel() {
    this.dialogRef.close();
  }


}
