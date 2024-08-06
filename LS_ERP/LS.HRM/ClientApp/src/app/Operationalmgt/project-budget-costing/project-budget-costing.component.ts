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
import { ParentOptMgtComponent } from '../../sharedcomponent/parentoptmgt.component';
import { ResourceCostingComponent } from './resource-costing/resource-costing.component';
import { MonthylRoasterForProjectComponent } from '../monthyl-roaster-for-project/monthyl-roaster-for-project.component';
import { LogisticsCostingComponent } from './logistics-costing/logistics-costing.component';
import { MaterialCostingComponent } from './material-costing/material-costing.component';
import { FinancialExpenseCostingComponent } from './financial-expense-costing/financial-expense-costing.component';
import { CostEstimationForProjectComponent } from './cost-estimation-for-project/cost-estimation-for-project.component';
import { EstimationConsolReportComponent } from './estimation-consol-report/estimation-consol-report.component';
import { EstimationSummaryReportComponent } from './estimation-summary-report/estimation-summary-report.component';
import { ApprovalDialogWindowComponent } from '../approval-dialog-window/approval-dialog-window.component';
import { OprServicesService } from '../opr-services.service';
import { ConvertToContractComponent } from './convert-to-contract/convert-to-contract.component';
import { ShiftplanForProjectComponent } from '../shift-master/shiftplan-for-project/shiftplan-for-project.component';
import { SkillsetPlanForProjectComponent } from '../skillset/skillset-plan-for-project/skillset-plan-for-project.component';
import { CalendarDaysComponent } from '../project/calendar-days/calendar-days.component';
import { ViewProjectContractComponent } from './view-project-contract/view-project-contract.component';
import { PrintProposalComponent } from './print-proposal/print-proposal.component';
import { PrintEstimationComponent } from './estimation-consol-report/print-estimation/print-estimation.component';
import { AssignEmployeesToProjectSiteComponent } from '../project/assign-employees-to-project-site/assign-employees-to-project-site.component';
import { MappingEmpToResourceForProjectSiteComponent } from '../project/mapping-emp-to-resource-for-project-site/mapping-emp-to-resource-for-project-site.component';
import { EmployeeAttendanceComponent } from '../employee/employee-attendance/employee-attendance.component';
import { TranslateService } from '@ngx-translate/core';



@Component({
  selector: 'app-project-budget-costing',
  templateUrl: './project-budget-costing.component.html'
})
export class ProjectBudgetCostingComponent extends ParentOptMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['customerCode', 'projectCode', 'Project_Name','Branch_Code'/*'projectNameEng', 'projectNameArb'*/,/* 'isActive',*/ 'Actions'];
  data: MatTableDataSource<any>;
  totalItemsCount: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  id: number;
  form: FormGroup;

  isArab: boolean = false;
 // projectCode: string;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, private authService: AuthorizeService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog, private oprService: OprServicesService,
    public pageService: PaginationService, private translate: TranslateService) {
    super(authService);
  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.form = this.fb.group({});
    this.initialLoading();

  }

  refresh() {
    this.searchValue = '';
    this.initialLoading();
  }

  initialLoading() {
    this.searchValue = '';
    this.loadList(0, this.pageService.pageCount, "", this.sortingOrder);
  }

  onSortOrder(sort: any) {
    this.totalItemsCount = 0;
    this.sortingOrder = sort.active + ' ' + sort.direction;
    this.loadList(0, this.pageService.pageCount, "", this.sortingOrder);
  }


  onPageSwitch(event: PageEvent) {
    this.pageService.change(event);
    this.loadList(event.pageIndex, event.pageSize, "", this.sortingOrder);
  }

  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
    this.isLoading = true;
    this.apiService.getPagination('Project/getProjectsPagedList', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
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
    (dialogRef.componentInstance as any).isArab = this.isArab;

    dialogRef.afterClosed().subscribe(res => {

      if (res && res === true)
        //this.initialLoading();
        this.ngOnInit();
    });
  }

  private openDialogManage(project: any, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).project = project;

    dialogRef.afterClosed().subscribe(res => {

      if (res && res === true)
        this.initialLoading();
    });
  }








  private openApprovalDialog(branchCode:string,serviceCode: string, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, serviceType:string, Component: any) {
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
    this.openDialogManage(project, DBOperation.create, 'Logistics_Vehicle_Costing', 'Save',LogisticsCostingComponent);
  }
public materialCosting(project: any) {
    this.openDialogManage(project, DBOperation.create, 'Material_Costing', 'Save',MaterialCostingComponent);
  }
  public financialExpenseCosting(project: any) {
    this.openDialogManage(project, DBOperation.create, 'Financial_Epense_Costing', 'Save',FinancialExpenseCostingComponent);
  }
  public estimationReport(project: any) {
    this.openDialogManage(project, DBOperation.create, 'Estimation_Cnsolidate_Report', 'Save',EstimationConsolReportComponent);
  }
  public summaryReport(project: any) {
    this.openDialogManage(project, DBOperation.create, 'Estimation_Summary_Report', 'Save',EstimationSummaryReportComponent);
  }
  public convertToContract(project: any) {
    this.openFullWindow(project, DBOperation.create, 'Creating_Contract', 'Save', ConvertToContractComponent);
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
  this.openDialogManage(project, DBOperation.create, 'Project_Proposal', 'Print', PrintProposalComponent);
  }


  public shiftPlan(project: any) {
    //console.log(projectCode);
    this.openDialogManage(project, DBOperation.create, 'Shifts_Plan_For_Project', 'Save', ShiftplanForProjectComponent);
  }

  public skillsetPlan(project: any) {
    this.openDialogManage(project, DBOperation.create, 'Skillset_Plan_For_Project', 'Save', SkillsetPlanForProjectComponent);
  }
  public calendarDays(project: any) {
    this.openFullWindow(project, DBOperation.create, 'Calander_Days', 'Save', CalendarDaysComponent);
  }

  public viewContract(project: any) {
    this.openFullWindow(project, DBOperation.create, 'Project_Contract', 'View', ViewProjectContractComponent);
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
    this.openApprovalDialog(branchCode,serviceCode, DBOperation.create, 'Estimation_Approval', 'Save', serviceType, ApprovalDialogWindowComponent);

  }

  convertToProposal(projectCode: string) {

    this.form.value['projectCode'] = projectCode;
  
    
    this.apiService.post('ProjectBudgetCosting/convertProjectToProposal',this.form.value)
      .subscribe(res => {
        this.utilService.OkMessage();
        this.ngOnInit();
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });
  }
  translateToolTip(msg: string){
    //console.log(msg);
    //let str = this.translate.instant(msg);
    return `${this.translate.instant(msg)}`;

  }
  applyFilter(searchVal: any) {
   
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);

  }
}
