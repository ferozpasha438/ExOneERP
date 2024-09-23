import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from 'src/app/api-authorization/AuthorizeService';
import { TblHRMTrnEmployeeContractInfoDto } from 'src/app/models/HumanResource/EmployeeContractInfoDto';
import { CustomSelectListItem } from 'src/app/models/MenuItemListDto';
import { BaseEmployeePaySlipDto } from 'src/app/models/Payroll/BaseEmployeePaySlipDto';
import { ApiService } from 'src/app/services/api.service';
import { NotificationService } from 'src/app/services/notification.service';
import { UtilityService } from 'src/app/services/utility.service';
import { ParentpayrollmgtComponent } from 'src/app/sharedcomponent/parentpayrollmgt.component';
import { MatAccordion, MatExpansionModule } from '@angular/material/expansion';
import { TblPRLTrnPayrollProcessFiltersLogDto } from 'src/app/models/Payroll/PayrollProcessFiltersLogDto';

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
  employeeList: Array<TblHRMTrnEmployeeContractInfoDto> = [];
  filtersLog!: TblPRLTrnPayrollProcessFiltersLogDto;

  ngOnInit(): void {
    this.loadCompanies();
    this.loadPayrollGroups();
    this.filtersLog = {
      branchCode: '',
      payrollGroupCode: '',
      payrollMonth: '',
      isApproved: false,
      isReleased: false,
    };
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

  RunProcess(isPreRun: boolean, isApproved?: boolean, isReleased?: boolean) {
    if (this.branchCode && this.payrollGroupCode) {
      this.RetrieveEmployeesByFilters(isPreRun, isApproved, isReleased);
    } else
      this.notifyService.showError(this.translate.instant('SelectFilters'));
  }

  RetrieveEmployeesByFilters(
    isPreRun: boolean,
    isApproved?: boolean,
    isReleased?: boolean
  ) {
    let queryParam = `branchCode=${encodeURIComponent(
      '' + this.branchCode
    )}&payrollGroupCode=${encodeURIComponent('' + this.payrollGroupCode)}`;
    this.apiService
      .getQueryString(`EmployeeContract/GetEmployeeListByFilters?`, queryParam)
      .subscribe((res) => {
        if (res) {
          this.employeeList = res;
          let lastEmployeeContractInfoId: number =
            this.employeeList[this.employeeList.length - 1].id;
          this.employeeList.forEach((e) => {
            e.isPreRun = isPreRun;
            e.isApproved = isApproved;
            e.isReleased = isReleased;
            e.lastEmployeeContractInfoId = lastEmployeeContractInfoId;
          });
        }
      });
  }

  GetPayrollProcessFilterLog() {
    if (this.branchCode && this.payrollGroupCode) {
      let queryParam = `branchCode=${encodeURIComponent(
        '' + this.branchCode
      )}&payrollGroupCode=${encodeURIComponent('' + this.payrollGroupCode)}`;
      this.apiService
        .getQueryString(
          `PayrollProcess/GetPayrollProcessFilterLog?`,
          queryParam
        )
        .subscribe((res) => {
          if (res) {
            this.filtersLog = res;
          } else {
            this.filtersLog = {
              branchCode: '',
              payrollGroupCode: '',
              payrollMonth: '',
              isApproved: false,
              isReleased: false,
            };
          }
        });
    } else
      this.notifyService.showError(this.translate.instant('SelectFilters'));
  }

  CreateUpdatePayrollProcessFilterLog(
    employeeContractInfo: TblHRMTrnEmployeeContractInfoDto
  ) {
    let queryParam = `branchCode=${encodeURIComponent(
      '' + employeeContractInfo.branchCode
    )}&payrollGroupCode=${encodeURIComponent(
      '' + employeeContractInfo.payrollGroupCode
    )}&isApproved=${encodeURIComponent(
      '' + employeeContractInfo.isApproved
    )}&isReleased=${encodeURIComponent(
      '' + employeeContractInfo.isReleased
    )}&payrollMonth=''`;
    this.apiService
      .getQueryString(
        `PayrollProcess/CreateUpdatePayrollProcessFilterLog?`,
        queryParam
      )
      .subscribe(
        (res) => {
          this.GetPayrollProcessFilterLog();
          this.utilService.OkMessage();
        },
        (error) => {
          this.utilService.ShowApiErrorMessage(error);
        }
      );
  }
}
