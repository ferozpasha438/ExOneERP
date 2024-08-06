import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';
import { ValidationService } from '../../sharedcomponent/ValidationService';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { DBOperation } from '../../services/utility.constants';
import { DeleteConfirmDialogComponent } from '../../sharedcomponent/delete-confirm-dialog';
import { AddupdateSurveyorComponent } from '../sharedpages/addupdate-surveyor/addupdate-surveyor.component';
import { ParentOptMgtComponent } from '../../sharedcomponent/parentoptmgt.component';
import { ShiftplanForProjectComponent } from '../shift-master/shiftplan-for-project/shiftplan-for-project.component';
import { SkillsetPlanForProjectComponent } from '../skillset/skillset-plan-for-project/skillset-plan-for-project.component';
import { CalendarDaysComponent } from './calendar-days/calendar-days.component';
import { MonthylRoasterForProjectComponent } from '../monthyl-roaster-for-project/monthyl-roaster-for-project.component';
import { TranslateService } from '@ngx-translate/core';
import { ResourceCostingComponent } from '../project-budget-costing/resource-costing/resource-costing.component';
import { LogisticsCostingComponent } from '../project-budget-costing/logistics-costing/logistics-costing.component';
import { MaterialCostingComponent } from '../project-budget-costing/material-costing/material-costing.component';
import { FinancialExpenseCostingComponent } from '../project-budget-costing/financial-expense-costing/financial-expense-costing.component';
import { EstimationConsolReportComponent } from '../project-budget-costing/estimation-consol-report/estimation-consol-report.component';
import { EstimationSummaryReportComponent } from '../project-budget-costing/estimation-summary-report/estimation-summary-report.component';
import { ConvertToContractComponent } from '../project-budget-costing/convert-to-contract/convert-to-contract.component';
import { PrintEstimationComponent } from '../project-budget-costing/estimation-consol-report/print-estimation/print-estimation.component';
import { AssignEmployeesToProjectSiteComponent } from './assign-employees-to-project-site/assign-employees-to-project-site.component';
import { MappingEmpToResourceForProjectSiteComponent } from './mapping-emp-to-resource-for-project-site/mapping-emp-to-resource-for-project-site.component';
import { PrintProposalComponent } from '../project-budget-costing/print-proposal/print-proposal.component';
import { ViewProjectContractComponent } from '../project-budget-costing/view-project-contract/view-project-contract.component';
import { EmployeeAttendanceComponent } from '../employee/employee-attendance/employee-attendance.component';
import { OprServicesService } from '../opr-services.service';
import { ApprovalDialogWindowComponent } from '../approval-dialog-window/approval-dialog-window.component';
import { ProjectSitesComponent } from './project-sites/project-sites.component';
import { ContractFormComponent } from './contract-form/contract-form.component';
import { UploadFileComponent } from '../upload-file/upload-file.component';
import * as XLSX from "xlsx";
import { ProjectSites2Component } from './project-sites/project-sites2.component';


@Component({
  selector: 'app-project2',
  templateUrl: './project2.component.html'
})
export class Project2Component extends ParentOptMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['customerCode', 'projectCode', 'Project_Name', 'branchCode', 'startDate', 'endDate', 'Actions'];
  data: MatTableDataSource<any>;
  reportData: Array<any> = [];
  totalItemsCount: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  id: number;
  form: FormGroup;

  customerSelectionList: Array<any> = [];
  citySelectionList: Array<any> = [];
  isArab: boolean = false;
  filter: any = { customerCode: '', branchCode: '' };
  filterDates: any = { startDateFrom: null, startDateTo: null, endDateFrom: null, endDateTo: null };
  // projectCode: string;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, private authService: AuthorizeService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog, private oprService: OprServicesService,
    public pageService: PaginationService, private translate: TranslateService) {
    super(authService);
  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.form = this.fb.group({

    });

    this.initialLoading();

  }

  openDatePicker(dp: any) {
    dp.open();
  }
  refresh() {
    this.reportData = [];
    this.resetFilter();
    // location.reload();
    //this.loadList(0, this.pageService.pageCount,this.searchValue, this.sortingOrder);


  }
  resetFilter() {
    this.filter = { customerCode: '', branchCode: '', startDateFrom: null, startDateTo: null, endDateFrom: null, endDateTo: null };
    this.filterDates = { startDateFrom: null, startDateTo: null, endDateFrom: null, endDateTo: null };

    this.applyFilter('');
  }

  clearReport() {
    this.reportData = [];
  }
  initialLoading() {
    this.searchValue = '';
    this.loadCitiesList();
    this.loadCustomersList();
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
  }
  loadCitiesList() {
  //  this.apiService.getall('City/getCitiesSelectList').subscribe((res: any) => {
        this.apiService.getall('Branch/getBranchSelectListForUser').subscribe((res: any) => {
      this.citySelectionList = res as Array<any>;

      this.citySelectionList.forEach(e => {
        e.lable = e.value + "-" + e.text;
      });
    });

  }
  loadCustomersList() {
    this.apiService.getall('CustomerMaster/getSelectCustomerList').subscribe((res: any) => {
      this.customerSelectionList = res as Array<any>;

      this.customerSelectionList.forEach(e => {
        e.lable = this.isArab ? e.value + "-" + e.textTwo : e.value + "-" + e.text;
      });
    });

  }
  onSortOrder(sort: any) {
    this.reportData = [];
    if (sort.active == 'Project_Name') {
      sort.active = this.isArab ? 'projectNameArb' : 'projectNameEng'
    }


    this.totalItemsCount = 0;
    this.sortingOrder = sort.active + ' ' + sort.direction;
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
  }


  onPageSwitch(event: PageEvent) {
    this.pageService.change(event);
    this.loadList(event.pageIndex, event.pageSize, this.searchValue, this.sortingOrder);
  }
  generateReport() {

    this.reportData = [];
    for (let i = 0; i <= Math.floor(this.totalItemsCount / 100); i++) {
      let event: any = { pageIndex: i, pageSize: 100, previousPageIndex: i == 0 ? 0 : i - 1, length: 0 };
      //this.pageService.change(event);
      this.loadListReport(event.pageIndex, event.pageSize, this.searchValue, this.sortingOrder);

    }
  }

  setFilterDates() {
    if (this.filterDates.startDateFrom != null) {
      let fd1: Date = new Date(this.filterDates.startDateFrom);
      fd1.setMinutes(fd1.getMinutes() - fd1.getTimezoneOffset());
      this.filter.startDateFrom = fd1;
    }
    if (this.filterDates.startDateTo != null) {
      let fd2: Date = new Date(this.filterDates.startDateTo);
      fd2.setMinutes(fd2.getMinutes() - fd2.getTimezoneOffset());
      this.filter.startDateTo = fd2;
    }
    if (this.filterDates.endDateFrom != null) {
      let fd3: Date = new Date(this.filterDates.endDateFrom);
      fd3.setMinutes(fd3.getMinutes() - fd3.getTimezoneOffset());
      this.filter.endDateFrom = fd3;
    }
    if (this.filterDates.endDateTo != null) {
      let fd4: Date = new Date(this.filterDates.endDateTo);
      fd4.setMinutes(fd4.getMinutes() - fd4.getTimezoneOffset());
      this.filter.endDateTo = fd4;

    }
  }
  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
    this.isLoading = true;
    this.setFilterDates();
    this.oprService.getPaginationWithFilter('Project/getProjectsPagedListWithFilter', this.utilService.getQueryString(page, pageCount, query, orderBy), this.filter).subscribe(result => {
      this.totalItemsCount = 0;

      this.data = new MatTableDataSource(result.items);
      this.totalItemsCount = result.totalCount

      setTimeout(() => {
        this.paginator.pageIndex = page as number;
        this.paginator.length = this.totalItemsCount;
      });
      //this.data.paginator = this.paginator;

      this.data.sort = this.sort;
      this.isLoading = false;
    }, error => this.utilService.ShowApiErrorMessage(error));
  }
  private loadListReport(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
    this.isLoading = true;
    this.setFilterDates();
    this.oprService.getPaginationWithFilter('Project/getProjectsPagedListWithFilter', this.utilService.getQueryString(page, pageCount, query, orderBy), this.filter).subscribe(result => {

      this.isLoading = false;
      this.reportData = this.reportData.length == 0 ? result.items : this.reportData.concat(result.items);
    }, error => this.utilService.ShowApiErrorMessage(error));
  }


  //applyFilter(searchVal: any) {
  //  const search = searchVal;//.target.value as string;
  //  //if (search && search.length >= 3) {
  //  if (search) {
  //    this.searchValue = search;
  //    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
  //  }
  //}
  private openFullWindow(project: any, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {
    let dialogRef = this.oprService.fullWindow(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).project = project;
    (dialogRef.componentInstance as any).type = "ForProject";
    (dialogRef.componentInstance as any).isArab = this.isArab;

    dialogRef.afterClosed().subscribe(res => {

      if (res && res === true)
        //this.initialLoading();
        this.ngOnInit();
    });
  }

  private openDialogManage(project: any, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {
    let dialogRef = this.oprService.openAutoHeightWidthDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).project = project;

    dialogRef.afterClosed().subscribe(res => {

      if (res && res === true)
        this.initialLoading();
    });
  }

  private openDialogManage2(project: any, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {
    let dialogRef = this.oprService.fullWindow(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).project = project;

    dialogRef.afterClosed().subscribe(res => {

      if (res && res === true)
        this.initialLoading();
    });
  }








  private openApprovalDialog(branchCode: string, serviceCode: string, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, serviceType: string, Component: any) {
    let dialogRef = this.oprService.openApprovalDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).serviceType = serviceType;
    (dialogRef.componentInstance as any).serviceCode = serviceCode;
    (dialogRef.componentInstance as any).branchCode = branchCode;

    dialogRef.afterClosed().subscribe(res => {

      if (res && res === true)
        this.initialLoading();
    });
  }


  public resourceCosting(project: any) {
    this.openDialogManage(project, DBOperation.create, 'Resource_Costing', 'Save', ResourceCostingComponent);
  }
  public logisticsCosting(project: any) {
    this.openDialogManage(project, DBOperation.create, 'Logistics_Vehicle_Costing', 'Save', LogisticsCostingComponent);
  }
  public materialCosting(project: any) {
    this.openDialogManage(project, DBOperation.create, 'Material_Costing', 'Save', MaterialCostingComponent);
  }
  public financialExpenseCosting(project: any) {
    this.openDialogManage(project, DBOperation.create, 'Financial_Epense_Costing', 'Save', FinancialExpenseCostingComponent);
  }
  public estimationReport(project: any) {
    this.openDialogManage(project, DBOperation.create, 'Estimation_Cnsolidate_Report', 'Save', EstimationConsolReportComponent);
  }
  public summaryReport(project: any) {
    this.openDialogManage(project, DBOperation.create, 'Estimation_Summary_Report', 'Save', EstimationSummaryReportComponent);
  }
  public convertToContract(project: any) {
    //this.openFullWindow(project, DBOperation.create, 'Creating_Contract', 'Save', ConvertToContractComponent);
    this.openFullWindow(project, DBOperation.create, 'Creating_Contract', 'Save', ContractFormComponent);
  }
  public printEstimation(project: any) {
    this.openDialogManage(project, DBOperation.create, 'Estimation_Cnsolidate_Report', 'Print', PrintEstimationComponent);
  }
  public assignEmployyesToProjectSite(project: any) {
    this.openFullWindow(project, DBOperation.create, 'Assigning_Employees_To_Site', 'Save', AssignEmployeesToProjectSiteComponent);
  }
  public mappingEmpToResourceForProjectSite(project: any) {
    this.openDialogManage(project, DBOperation.create, 'Mapping_Employees_To_Resources_For_Site', 'Save', MappingEmpToResourceForProjectSiteComponent);
  }
  public printProposal(project: any) {
    this.openDialogManage2(project, DBOperation.create, 'Project_Proposal', 'Print', PrintProposalComponent);
  }


  public shiftPlan(project: any) {

    this.openDialogManage(project, DBOperation.create, 'Shifts_Plan_For_Project', 'Save', ShiftplanForProjectComponent);
  }

  public skillsetPlan(project: any) {
    this.openDialogManage(project, DBOperation.create, 'Skillset_Plan_For_Project', 'Save', SkillsetPlanForProjectComponent);
  }
  public calendarDays(project: any) {
    this.openFullWindow(project, DBOperation.create, 'Calander_Days', 'Save', CalendarDaysComponent);
  }

  public viewContract(project: any) {
    // this.openFullWindow(project, DBOperation.create, 'Project_Contract', 'View', ViewProjectContractComponent);
    this.openFullWindow(project, DBOperation.create, 'Creating_Contract', 'View', ContractFormComponent);
  }
  public employeeAttendance(project: any) {
    this.openFullWindow(project, DBOperation.create, 'Employee_Attendance', 'View', EmployeeAttendanceComponent);
  }




  submit() {

  }

  openMonthlyRoaster(project: any) {
    this.openFullWindow(project, DBOperation.create, 'Monthly_Roaster', 'Save', MonthylRoasterForProjectComponent);

  }

  approveEstimation(project: any) {
    let serviceType = 'EST';
    let serviceCode = project.projectCode;
    let branchCode = project.branchCode;
    this.openApprovalDialog(branchCode, serviceCode, DBOperation.create, 'Estimation_Approval', 'Save', serviceType, ApprovalDialogWindowComponent);

  }

  convertToProposal(project: any) {

    //this.form.value['projectCode'] = projectCode;


    this.apiService.post('ProjectBudgetCosting/convertProjectToProposal', project)
      .subscribe(res => {
        this.utilService.OkMessage();
        this.ngOnInit();
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });
  }







  translateToolTip(msg: string) {
    //console.log(msg);
    //let str = this.translate.instant(msg);
    return `${this.translate.instant(msg)}`;

  }
  applyFilter(searchVal: any) {


    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
    this.reportData = [];
  }

  viewProjectSites(project: any) {
    this.openDialogManage(project, DBOperation.create, 'List_Of_Sites_In_Project', 'Pagination', ProjectSites2Component);
  }

  uploadFile(project: any) {
    let dialogRef = this.oprService.openAutoWidthDialog(this.dialog, UploadFileComponent);
    (dialogRef.componentInstance as any).project = project;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });

  }


  viewUploadedFile(row: any) {
    window.open(row.fileUrl);
  }

  exportexcel(): void {
    let element = document.getElementById('printcontainer');
    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(element);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
    XLSX.writeFile(wb, "projectsReport.xlsx");
  }
}
