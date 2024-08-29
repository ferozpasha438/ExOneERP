import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from 'src/app/api-authorization/AuthorizeService';
import { CustomSelectListItem } from 'src/app/models/MenuItemListDto';
import { ApiService } from 'src/app/services/api.service';
import { NotificationService } from 'src/app/services/notification.service';
import { UtilityService } from 'src/app/services/utility.service';
import { ParentpayrollmgtComponent } from 'src/app/sharedcomponent/parentpayrollmgt.component';

@Component({
  selector: 'app-payrollprocess',
  templateUrl: './payrollprocess.component.html',
  styles: [],
})
export class PayrollprocessComponent
  extends ParentpayrollmgtComponent
  implements OnInit
{
  constructor(
    private fb: FormBuilder,
    private authService: AuthorizeService,
    private apiService: ApiService,
    private utilService: UtilityService,
    private notifyService: NotificationService,
    private translate: TranslateService
  ) {
    super(authService);
  }
  payrollGroups: Array<CustomSelectListItem> = [];
  branches: Array<CustomSelectListItem> = [];
  companies: Array<CustomSelectListItem> = [];
  branchCode: string = '';
  payrollGroupCode: string = '';

  ngOnInit(): void {
    this.loadCompanies();
    this.loadPayrollGroups();
  }

  loadCompanies() {
    this.apiService
      .getall('Company/GetCompanySelectItemList')
      .subscribe((res) => {
        this.companies = res;
      });
  }

  loadBranches(event: any) {
    let id: string = event.target.value;
    this.apiService
      .getQueryString(`Branch/GetBranchesByCompany?id=`, id)
      .subscribe((res) => {
        this.branches = res;
      });
  }

  loadPayrollGroups() {
    this.apiService
      .getall('PayrollGroup/GetPayrollGroupSelectListItem')
      .subscribe((res) => {
        this.payrollGroups = res;
      });
  }
}
