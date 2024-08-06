import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiService } from '../../../services/api.service';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { OprServicesService } from '../../opr-services.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { UtilityService } from '../../../services/utility.service';
import { NotificationService } from '../../../services/notification.service';
import { PaginationService } from '../../../sharedcomponent/pagination.service';
import { TranslateService } from '@ngx-translate/core';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';
import { DBOperation } from '../../../services/utility.constants';
import { ResourceCostingComponent } from '../../project-budget-costing/resource-costing/resource-costing.component';
import { LogisticsCostingComponent } from '../../project-budget-costing/logistics-costing/logistics-costing.component';
import { MaterialCostingComponent } from '../../project-budget-costing/material-costing/material-costing.component';
import { FinancialExpenseCostingComponent } from '../../project-budget-costing/financial-expense-costing/financial-expense-costing.component';
import { EstimationConsolReportComponent } from '../../project-budget-costing/estimation-consol-report/estimation-consol-report.component';
import { EstimationSummaryReportComponent } from '../../project-budget-costing/estimation-summary-report/estimation-summary-report.component';
import { ConvertToContractComponent } from '../../project-budget-costing/convert-to-contract/convert-to-contract.component';
import { PrintEstimationComponent } from '../../project-budget-costing/estimation-consol-report/print-estimation/print-estimation.component';
import { AssignEmployeesToProjectSiteComponent } from '../assign-employees-to-project-site/assign-employees-to-project-site.component';
import { MappingEmpToResourceForProjectSiteComponent } from '../mapping-emp-to-resource-for-project-site/mapping-emp-to-resource-for-project-site.component';
import { PrintProposalComponent } from '../../project-budget-costing/print-proposal/print-proposal.component';
import { ShiftplanForProjectComponent } from '../../shift-master/shiftplan-for-project/shiftplan-for-project.component';
import { SkillsetPlanForProjectComponent } from '../../skillset/skillset-plan-for-project/skillset-plan-for-project.component';
import { CalendarDaysComponent } from '../calendar-days/calendar-days.component';
import { ViewProjectContractComponent } from '../../project-budget-costing/view-project-contract/view-project-contract.component';
import { EmployeeAttendanceComponent } from '../../employee/employee-attendance/employee-attendance.component';
import { MonthylRoasterForProjectComponent } from '../../monthyl-roaster-for-project/monthyl-roaster-for-project.component';
import { ApprovalDialogWindowComponent } from '../../approval-dialog-window/approval-dialog-window.component';
import { ProjectComponent } from '../project.component';
import { ConfirmDialogWindowComponent } from '../../confirm-dialog-window/confirm-dialog-window.component';
import { CreateUpdatePvOpenCloseReqComponent } from '../../ProgectManagement/pv-open-close-reqs/create-update-pv-open-close-req/create-update-pv-open-close-req.component';
import { ContractFormComponent } from '../contract-form/contract-form.component';
import { PvRequestsPaginationByProjectSiteComponent } from '../../ProgectManagement/pv-requests-pagination-by-project-site/pv-requests-pagination-by-project-site.component';
import { AttendancePayrollReportComponent } from '../../employee/employee-attendance/attendance-payroll-report/attendance-payroll-report.component';
import { CustomerVisitFormsComponent } from '../../customer-visit-forms/customer-visit-forms.component';
import { CustomerComplaintsComponent } from '../../customer-complaints/customer-complaints.component';
import { UploadFileComponent } from '../../upload-file/upload-file.component';
@Component({
  selector: 'app-project-sites',
  templateUrl: './project-sites.component.html'
})
export class ProjectSitesComponent extends ParentOptMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['customerCode', 'projectCode', 'Project_Name', 'SiteCode', 'Branch_Code','startDate','endDate','isAdendum','Actions'];
  data: MatTableDataSource<any>;
  totalItemsCount: number;
  searchValue: string = "";
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  id: number;
  form: FormGroup;
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  project: any;
  isArab: boolean = false;

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, private authService: AuthorizeService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog, private oprService: OprServicesService,
    public pageService: PaginationService, private translate: TranslateService, public dialogRef: MatDialogRef<ProjectSitesComponent>) {
    super(authService);
  }

  ngOnInit(): void {

    this.isArab = this.utilService.isArabic();
    this.form = this.fb.group({});
    this.searchValue = this.project.projectCode;

    this.initialLoading();
  }

  refresh() {
    this.searchValue = this.project.projectCode;
    this.initialLoading();
  }

  initialLoading() {
    this.searchValue = this.project.projectCode;
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
  }

  onSortOrder(sort: any) {
    this.totalItemsCount = 0;
    this.sortingOrder = sort.active + ' ' + sort.direction;
    this.loadList(0, this.pageService.pageCount, this.project.projectCode, this.sortingOrder);
  }


  onPageSwitch(event: PageEvent) {
    this.pageService.change(event);
    this.loadList(event.pageIndex, event.pageSize, "", this.sortingOrder);
  }

  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
    this.isLoading = true;
    this.apiService.getPagination('ProjectSites/getProjectSitesPagedList', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
      this.totalItemsCount = 0;

      this.data = new MatTableDataSource(result.items);
      this.totalItemsCount = result.totalCount

      setTimeout(() => {
        this.paginator.pageIndex = page as number;
        this.paginator.length = this.totalItemsCount;
      });


      this.data.sort = this.sort;
      this.isLoading = false;
    }, error => this.utilService.ShowApiErrorMessage(error));
  }



  private openFullWindow(project: any, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {
    let dialogRef = this.oprService.fullWindow(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).project = project;
    (dialogRef.componentInstance as any).isArab = this.isArab;
    (dialogRef.componentInstance as any).type = "ForAddingSite";

    dialogRef.afterClosed().subscribe(res => {

      if (res && res === true)
        this.ngOnInit();
    });
  }

  private openDialogManage(project: any, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {
    let dialogRef = this.oprService.openAutoHeightWidthDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).project = project;
    (dialogRef.componentInstance as any).requestData = project;

    dialogRef.afterClosed().subscribe(res => {

      if (res && res === true)
        this.initialLoading();
    });
  }




   private openPaginationWindow(project: any, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {
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

  private openConfirmationDialog(dbops: DBOperation, modalTitle: string, Component: any, confirmType: string, operation: string, day: number) {
    let dialogRef = this.oprService.confirmationDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).confirmType = confirmType;
    dialogRef.afterClosed().subscribe(res => {
      if ((res && res == true) || res.res) {
        this.apiService.post('ProjectBudgetCosting/skippEstimation', this.project)
          .subscribe(res => {
           

            this.utilService.OkMessage();
            this.ngOnInit();

          },
            error => {
              console.error(error);
              this.utilService.ShowApiErrorMessage(error);
            
            });

      }

      
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
    this.openDialogManage(project, DBOperation.create, 'Project_Proposal', 'Print', PrintProposalComponent);
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
    this.openFullWindow(project, DBOperation.create, 'Adendum_Contract', 'View', ContractFormComponent);
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
    let serviceType = 'ADNDM';
    let serviceCode = project.projectCode +"/"+ project.siteCode;
    let branchCode = project.branchCode;
    this.openApprovalDialog(branchCode, serviceCode, DBOperation.create, 'Estimation_Approval', 'Save', serviceType, ApprovalDialogWindowComponent);

  }
  enableEditCostingButton(row: any) {
    row.isEstimationCompleted = false;
  }
  convertToProposal(project: any) {
   // this.openFullWindow(project, DBOperation.create, 'Generate_Proposal', 'Save', ConvertToProposalComponent);




    

    this.apiService.post('ProjectBudgetCosting/convertProjectSiteToProposal',project)
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
   
    return `${this.translate.instant(msg)}`;

  }
  applyFilter(searchVal: any) {

    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);

  }

  closeModel() {
    this.dialogRef.close(true);
  }

  skippEstimation(row:any) {
    this.project = row;
    console.log(row);
    this.openConfirmationDialog(DBOperation.create, 'Are_You_Sure?', ConfirmDialogWindowComponent, "general", "Skipp_Estimation", 0);

  }

  CloseProject(row: any) {
    row.isExtendProjReq = false;
    row.isReOpenReq = false;
    row.isRevokeSuspReq = false;
    row.isSuspendReq = false;
    row.isCloseReq = true;
    row.id = 0;
    this.openDialogManage(row, DBOperation.create, 'Close_Project_Site', 'Save', CreateUpdatePvOpenCloseReqComponent);
  }


  CancelProject() {

  }



  SuspendProject(row: any) {
    row.isExtendProjReq = false;
    row.isReOpenReq = false;
    row.isRevokeSuspReq = false;
    row.isSuspendReq = true;
    row.isCloseReq = false;
    row.id = 0;
    this.openDialogManage(row, DBOperation.create, 'Suspend_Project_Site', 'Save', CreateUpdatePvOpenCloseReqComponent);
  }

  ExtendProject(row:any) {
    row.isExtendProjReq = true;
    row.isReOpenReq = false;
    row.isRevokeSuspReq = false;
    row.isSuspendReq = false;
    row.isCloseReq = false;
    row.id = 0;
    this.openDialogManage(row, DBOperation.create, 'Extend_Project_Site', 'Save', CreateUpdatePvOpenCloseReqComponent);
  }
  ReopenProject(row: any) {
    row.isExtendProjReq = false;
    row.isReOpenReq = true;
    row.isRevokeSuspReq = false;
    row.isSuspendReq = false;
    row.isCloseReq = false;
    row.id = 0;
    this.openDialogManage(row, DBOperation.create, 'Reopen_Project_Site', 'Save', CreateUpdatePvOpenCloseReqComponent);
  }
  RevokeSuspension(row: any) {
    row.isExtendProjReq = false;
    row.isReOpenReq = false;
    row.isRevokeSuspReq = true;
    row.isSuspendReq = false;
    row.isCloseReq = false;
    row.id = 0;
    this.openDialogManage(row, DBOperation.create, 'Revoke_Suspension', 'Save', CreateUpdatePvOpenCloseReqComponent);
  }

  ViewRequests(row: any) {
    this.openPaginationWindow(row, DBOperation.create, '', '', PvRequestsPaginationByProjectSiteComponent);

  }
  ViewCustomerVisitForms(row: any) {
    let dialogRef = this.oprService.openAutoHeightWidthDialog(this.dialog, CustomerVisitFormsComponent);
    (dialogRef.componentInstance as any).dbops = DBOperation.create;
    (dialogRef.componentInstance as any).modalTitle = '';
    (dialogRef.componentInstance as any).modalBtnTitle = '';
    (dialogRef.componentInstance as any).project = row;

    dialogRef.afterClosed().subscribe(res => {

      if (res && res === true)
        this.initialLoading();
    });
  }

  ViewCustomerComplaints(row: any) {
    let dialogRef = this.oprService.openAutoHeightWidthDialog(this.dialog, CustomerComplaintsComponent);
    (dialogRef.componentInstance as any).dbops = DBOperation.create;
    (dialogRef.componentInstance as any).modalTitle = '';
    (dialogRef.componentInstance as any).modalBtnTitle = '';
    (dialogRef.componentInstance as any).project = row;

    dialogRef.afterClosed().subscribe(res => {

      if (res && res === true)
        this.initialLoading();
    });
  }











  employeePayrollAttendance(row: any) {
    let dialogRef = this.oprService.openAutoHeightWidthDialog(this.dialog, AttendancePayrollReportComponent);
    (dialogRef.componentInstance as any).projectCode = row.projectCode;
    (dialogRef.componentInstance as any).siteCode = row.siteCode;
    (dialogRef.componentInstance as any).isFromProjectSiteActions = true;
  
  

    dialogRef.afterClosed().subscribe(res => {

      
    });





  }




  uploadFile(projectSite:any) {
    let dialogRef = this.oprService.openAutoWidthDialog(this.dialog, UploadFileComponent);
    (dialogRef.componentInstance as any).projectSite = projectSite;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });

  }


  viewUploadedFile(row: any) {
    window.open(row.fileUrl);
  }




  getResourceColor(row: any): string {

    if (row.isResourcesAssigned)
      return `GreenText`;
    else return `RedText`;
  }
  getLogisticsColor(row: any): string { 
      if (row.isLogisticsAssigned)
        return `GreenText`;
      else return `RedText`
  }

  getMaterialColor(row:any) {
      if (row.isMaterialAssigned)
        return `GreenText`;
      else return `RedText`
    }
    getFinanceExpenceColor(row:any) {
      if (row.isExpenceOverheadsAssigned)
        return `GreenText`;
      else return `RedText`
    }

    
}
