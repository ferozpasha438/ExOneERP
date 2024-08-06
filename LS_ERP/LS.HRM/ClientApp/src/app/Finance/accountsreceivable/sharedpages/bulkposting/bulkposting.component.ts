import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { CustomSelectListItem, LanCustomSelectListItem } from '../../../../models/MenuItemListDto';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { UtilityService } from '../../../../services/utility.service';
import { ValidationService } from '../../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-bulkposting',
  templateUrl: './bulkposting.component.html',
  styles: [
  ]
})
export class BulkpostingComponent implements OnInit {

  form!: FormGroup;
  payCodeList: Array<CustomSelectListItem> = [];
  branchList: Array<CustomSelectListItem> = [];
  customerSiteList: Array<LanCustomSelectListItem> = [];
  customerList: Array<LanCustomSelectListItem> = [];
  isChecked: boolean = false;
  isDisabled: boolean = false;
  isMainChecked: boolean = false;
  isArab: boolean = false;
  IstramDateChecked: boolean = false;
  isLoading: boolean = false;
  isCreditSettled: boolean = true;
  hasTranDate: boolean = true;
  listOfAcCodes: Array<number> = [];
  invoiceList: Array<any> = [];

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService,
    public dialogRef: MatDialogRef<BulkpostingComponent>, public dialog: MatDialog,
    private notifyService: NotificationService, private validationService: ValidationService) {
  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.setForm();
    this.loadData();
  }
  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      'customerId': [0],
      'siteCode': [null],
      'branchCode': [null, Validators.required],
      'fromDate': [this.utilService.getStrtingYearDate(), Validators.required],
      'toDate': [this.utilService.getCurrentDate(), Validators.required],
      'payCode': [''],
      'hasTranDate': [true],
      'isCreditSettled': [false],
      'tranDate': [null],
      'isAllSelected': [false],
    });
  }

  checkAll(evt: MatSlideToggleChange) {
    this.isMainChecked = evt.checked;
    if (evt.checked) {
      this.invoiceList.forEach(item => {
        this.listOfAcCodes.push(item.value);
      });
      this.isChecked = true;
      // this.setCheckBoxItems(true);
    }
    else {
      this.listOfAcCodes = [];
      this.isChecked = false;
    }

  }

  resetCustomerInfo() {
    this.form.controls['siteCode'].setValue('');
    this.customerSiteList = [];
  }
  selectMapping(event: MatSlideToggleChange, id: number) {
    const isChecked = event.checked;
    if (isChecked) {
      this.listOfAcCodes.push(id);
    }
    else {
      let index: number = this.listOfAcCodes.findIndex(a => a === id);
      this.listOfAcCodes.splice(index, 1);
    }
  }

  customerChange(event: any) {
    let custId = event.value, custName = event.text;
    this.apiService.getall(`customer/getCustomerSitesSelectList/${custId}`).subscribe(res => {
      if (res) {
        this.customerSiteList = res;

        this.customerSiteList.map((i) => {
          i.text = !this.isArab ? i.text : i.textTwo;
          return i;
        });
      }
    });
  }

  loadData() {

    this.apiService.getall("paymentcodes/getSelectPaymentCodeList").subscribe(res => {
      if (res)
        this.payCodeList = res;
    });

    this.apiService.getall("branch/getSelectBranchCodeList").subscribe(res => {
      if (res)
        this.branchList = res;
    });

    this.apiService.getall("customer/getSelectCustomerList").subscribe(res => {
      if (res) {
        this.customerList = res;
        this.customerList.map((i) => {
          i.text = !this.isArab ? i.text : i.textTwo;
          return i;
        });
      }
    });
  }

  tramDateChecked(event: MatSlideToggleChange) {
    this.IstramDateChecked = !event.checked;
    this.hasTranDate = event.checked;
    this.form.controls['tranDate'].setValue(null);
  }

  creditSettledChecked(event: MatSlideToggleChange) {
    this.isCreditSettled = !event.checked;        
  }

  search() {

    if (this.form.valid) {
      this.isLoading = true;
      this.listOfAcCodes = [];
      this.apiService.postData(`generateInvoice/getBulkPostingInvoiceList`, this.form.value).subscribe(res => {
        this.isLoading = false;
        //  Object.values(res);        
        //this.invoiceList = Object.values(res)[0];
        this.invoiceList = res;
      });
    }
    else
      this.notifyService.showError("Select Dates and Branch");
  }
  submit() {

    let tranDate = this.form.controls['tranDate'].value;
    if (!this.hasTranDate && !tranDate) {
      this.notifyService.showError("Select TransactionDate");
      return;
    }

    if (tranDate)
      this.form.value['tranDate'] = this.utilService.selectedDateTime(this.form.controls['tranDate'].value);
    let IsCreditSettled = this.form.controls['isCreditSettled'].value;
    if (!IsCreditSettled) {
      let payCode = this.form.controls['payCode'].value;
      if (!this.utilService.hasValue(payCode)) {
        this.notifyService.showError("Select PayCode");
        return;
      }
    }

    if (this.listOfAcCodes.length > 0) {
      console.log(this.form.value, this.listOfAcCodes);
      this.form.value['ids'] = this.listOfAcCodes;
      this.listOfAcCodes = [];

      this.isDisabled = true;
      this.apiService.post(`generateInvoice/bulkPostingList`, this.form.value).subscribe(res => {
        this.isDisabled = false;
        this.search();
        this.utilService.OkMessage();
      },
        error => {
          this.search();
          this.isDisabled = false;
          this.utilService.OkMessage();
        });

    }
    else
      this.notifyService.showError("Select Invoices");
  }
  closeModel() {
    this.dialogRef.close(true);
  }
}
