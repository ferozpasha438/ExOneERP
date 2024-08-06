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
import { MonthylRoasterForProjectComponent } from '../monthyl-roaster-for-project/monthyl-roaster-for-project.component';
import { ApprovalDialogWindowComponent } from '../approval-dialog-window/approval-dialog-window.component';
import { OprServicesService } from '../opr-services.service';
import { ShiftplanForProjectComponent } from '../shift-master/shiftplan-for-project/shiftplan-for-project.component';
import { SkillsetPlanForProjectComponent } from '../skillset/skillset-plan-for-project/skillset-plan-for-project.component';
import { CalendarDaysComponent } from '../project/calendar-days/calendar-days.component';
import { AssignEmployeesToProjectSiteComponent } from '../project/assign-employees-to-project-site/assign-employees-to-project-site.component';
import { MappingEmpToResourceForProjectSiteComponent } from '../project/mapping-emp-to-resource-for-project-site/mapping-emp-to-resource-for-project-site.component';
import { EmployeeAttendanceComponent } from '../employee/employee-attendance/employee-attendance.component';
import { TranslateService } from '@ngx-translate/core';
import { AddupdateEnquiryFormComponent } from '../sharedpages/addupdate-enquiry-form/addupdate-enquiry-form.component';
import { AddExistingProjectComponent } from './add-existing-project/add-existing-project.component';

@Component({
  selector: 'app-existing-projects',
  templateUrl: './existing-projects.component.html'
})
export class ExistingProjectsComponent extends ParentOptMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['customerCode', 'projectCode', 'projectNameEng', 'projectNameArb'/*, 'isActive', 'Actions'*/];
  data: MatTableDataSource<any>;
  totalItemsCount: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  id: number;
  form: FormGroup;
  // projectCode: string;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, private authService: AuthorizeService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog, private oprService: OprServicesService,
    public pageService: PaginationService, private translate: TranslateService) {
    super(authService);
  }

  ngOnInit(): void {
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


  applyFilter(searchVal: any) {
    const search = searchVal;//.target.value as string;
    //if (search && search.length >= 3) {
    if (search) {
      this.searchValue = search;
      this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
    }
  }
  private openFullWindow(project: any, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {
    let dialogRef = this.oprService.fullWindow(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).project = project;

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


  
  submit() {

  }

  public create() {
    this.openFullWindow(0, DBOperation.create, 'Adding_New_Service_Enquiry_Form_For_Existing_Project', 'Add', AddExistingProjectComponent);
  }

}
