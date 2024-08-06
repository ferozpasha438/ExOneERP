
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../../models/MenuItemListDto';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { UtilityService } from '../../../../services/utility.service';
import { ParentFinMgtComponent } from '../../../../sharedcomponent/parentfinmgt.component';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { Router } from '@angular/router';

  @Component({
    selector: 'app-vendorpaymentsummary',
    templateUrl: './vendorpaymentsummary.component.html',
    styles: [
    ]
  })
  export class VendorpaymentsummaryComponent  extends ParentFinMgtComponent implements OnInit {

  vendCode: string = '';
  isAllVendors: boolean = false;
  dateFrom: string = '';
  dateTo: string = '';
  vendorList: Array<any> = [];
  codeControl = new FormControl('');
  form: FormGroup;

  totalBalance: number = 0;
  companyName: string = '';
  companyAddress: string = '';
  branchName: string = '';
  logoURL: any;

  isLoading: boolean = false;
  filteredOptions: Observable<Array<CustomSelectListItem>>;
  isCodeLoading: boolean = false;

    constructor(private fb: FormBuilder, private router: Router, private apiService: ApiService,
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

      if (!this.utilService.hasPaymenRoute()) {
        this.apiService.getall("customerPayment/checkOpenItemMenthod").subscribe(res => {
          if (res) {
            localStorage.setItem('hasPaymenRoute', 'hasPayment');
            this.router.navigateByUrl('dashboard/fin/opmapvendvouchersummary');
          }
          else
            this.setForm();
        });
      }
      else
        this.router.navigateByUrl('dashboard/fin/opmapvendvouchersummary');

   
  }

  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      'dateFrom': [''],
      'dateTo': [''],
      'isAllVendors': [false],
    });
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

  search() {
    this.isLoading = true;

    this.apiService.getall(`report/getVendorPaymentSummaryList?isAllVendors=${this.isAllVendors}&custCode=${this.codeControl.value}&from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}`).subscribe(res => {
      this.isLoading = false;

      if (res) {
        this.vendorList = res['list'];
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
