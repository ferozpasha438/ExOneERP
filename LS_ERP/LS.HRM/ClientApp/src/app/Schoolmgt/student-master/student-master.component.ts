import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
//import { ApiService } from '../../services/api.service';
import { DBOperation } from '../../services/utility.constants';
import { UtilityService } from '../../services/utility.service';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { ParentSchoolMgtComponent } from '../../sharedcomponent/parentschoolmgt.component';
import { ValidationService } from '../../sharedcomponent/ValidationService';
import { AddupdateSchoolReligionComponent } from '../shared/addupdate-school-religion/addupdate-school-religion.component';
import { AddupdateStudentRegistrationComponent } from '../shared/addupdate-student-registration/addupdate-student-registration.component';
import { AddupdateStudentMasterComponent } from '../shared/addupdate-student-master/addupdate-student-master.component';
import { StudentFeeMasterComponent } from '../shared/student-fee-master/student-fee-master.component';
import { StudentNoticeAndMessagingComponent } from '../shared/student-notice-and-messaging/student-notice-and-messaging.component';
import { StudentApplyLeaveComponent } from '../shared/student-apply-leave/student-apply-leave.component';
import { StudentAttendanceComponent } from '../shared/student-attendance/student-attendance.component';
import { StudentAcademicsComponent } from '../shared/student-academics/student-academics.component';
import { WebStudentRegistrationListComponent } from '../shared/web-student-registration-list/web-student-registration-list.component';
import { StudentAddressComponent } from '../shared/student-address/student-address.component';
import { StudentSiblingDataComponent } from '../shared/student-sibling-data/student-sibling-data.component';
import { AddUpdateStudentNotificationComponent } from '../shared/add-update-student-notification/add-update-student-notification.component';
import { StudentFeeHistoryComponent } from '../shared/student-fee-history/student-fee-history.component';
import { DeleteConfirmDialogComponent } from '../../sharedcomponent/delete-confirm-dialog';


@Component({
  selector: 'app-student-master',
  templateUrl: './student-master.component.html',
  styleUrls: []
})
export class StudentMasterComponent extends ParentSchoolMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort!: MatSort;
  displayedColumns: string[] = ['image1Path', 'stuAdmNum', 'stuName', 'stuAdmDate', 'gradeCode', 'gradeSectionCode', 'natCode', 'isActive', 'iDNumber', 'fatherName', 'Actions'];
  data!: MatTableDataSource<any>;
  totalItemsCount!: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  id: number = 0;
  form!: FormGroup;
  isArab: boolean = false;
  isExpoerting: boolean = false;
  isImporting: boolean = false;
  files!: File;
  hasFile: boolean = false;
  uploadError: string='';
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private translate: TranslateService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService) {
    super(authService);
  }

  //get():Array<any>{
  //  return [
  //    {id:1,regNum:'Religion -1',studentName:'Religion -1',regDate:'Religion -1',grade:'Religion -1',section:'Religion -1',nationality:'Religion -1',idNum:'Religion -1',fatherName:'Religion -1'},  
  // ]
  // }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.initialLoading();

  }

  refresh() {
    this.searchValue = '';
    this.sortingOrder = 'id desc';
    (document.getElementById('excel_file') as HTMLInputElement).value = '';
    this.hasFile = false;
    this.initialLoading();
  }

  initialLoading() {
    this.loadList(0, this.pageService.pageCount, "", this.sortingOrder);
  }

  onSortOrder(sort: any) {
    this.totalItemsCount = 0;
    this.sortingOrder = sort.active + ' ' + sort.direction;
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
  }


  onPageSwitch(event: PageEvent) {
    this.pageService.change(event);
    this.loadList(event.pageIndex, event.pageSize, this.searchValue, this.sortingOrder);
  }

  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
    this.isLoading = false;
    this.apiService.getPagination('schoolStudentMaster', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
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
  applyFilter(searchVal: any) {
    const search = searchVal;//.target.value as string;
    //if (search && search.length >= 3) {
    if (search) {
      this.searchValue = search;
      this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
    }
  }
  private openDialogManage(id: number = 0, dbops: DBOperation, modalTitle: string, modalBtnTitle: string) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, AddupdateStudentMasterComponent);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }

  updateStatus(isChecked: boolean, stuAdmNum: string) {
    this.apiService.getall(`SchoolStudentMaster/updateStatus/${stuAdmNum}/${isChecked}`).subscribe(res => {
      if (res) {
        this.utilService.OkMessage();
      }
    },
      error => {
        console.error(error);
        this.utilService.ShowApiErrorMessage(error);
      });
  }

  public create() {
    this.openDialogManage(0, DBOperation.create, 'Adding_New_Customer', 'Add');
  }

  //private openFeeMaster(id: number = 0, dbops: DBOperation, modalTitle: string, modalBtnTitle: string) {
  //  let dialogRef = this.utilService.openCrudDialog(this.dialog, StudentFeeMasterComponent );
  //  (dialogRef.componentInstance as any).dbops = dbops;
  //  (dialogRef.componentInstance as any).modalTitle = modalTitle;
  //  (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
  //  (dialogRef.componentInstance as any).id = id;

  //  dialogRef.afterClosed().subscribe(res => {
  //    if (res && res === true)
  //      this.initialLoading();
  //  });
  //}
  private editFeeMaster(row: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, StudentFeeMasterComponent);
    (dialogRef.componentInstance as any).row = row;
    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }


  public openFee(row: any) {
    this.editFeeMaster(row);
  }

  //private openStudentAttendence(id: number = 0, dbops: DBOperation, modalTitle: string, modalBtnTitle: string) {
  //  let dialogRef = this.utilService.openCrudDialog(this.dialog, StudentAttendanceComponent );
  //  (dialogRef.componentInstance as any).dbops = dbops;
  //  (dialogRef.componentInstance as any).modalTitle = modalTitle;
  //  (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
  //  (dialogRef.componentInstance as any).id = id;

  //  dialogRef.afterClosed().subscribe(res => {
  //    if (res && res === true)
  //      this.initialLoading();
  //  });
  //}

  private editStudentAttendence(row: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, StudentAttendanceComponent);
    (dialogRef.componentInstance as any).row = row;
    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }

  public openAttendence(row: any) {
    this.editStudentAttendence(row);
  }


  private editStudentNotice(row: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, StudentNoticeAndMessagingComponent);
    (dialogRef.componentInstance as any).row = row;
    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
  public openNotice(row: any) {
    this.editStudentNotice(row);
  }
  //private openStudentAcademics(id: number = 0, dbops: DBOperation, modalTitle: string, modalBtnTitle: string) {
  //  let dialogRef = this.utilService.openCrudDialog(this.dialog, StudentAcademicsComponent);
  //  (dialogRef.componentInstance as any).dbops = dbops;
  //  (dialogRef.componentInstance as any).modalTitle = modalTitle;
  //  (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
  //  (dialogRef.componentInstance as any).id = id;

  //  dialogRef.afterClosed().subscribe(res => {
  //    if (res && res === true)
  //      this.initialLoading();
  //  });
  //}
  private editStudentAcademics(row: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, StudentAcademicsComponent);
    (dialogRef.componentInstance as any).row = row;
    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
  public openAcademics(row: any) {
    this.editStudentAcademics(row);
  }
  private editStudentAddress(row: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, StudentAddressComponent);
    (dialogRef.componentInstance as any).row = row;
    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
  public openAddress(row: any) {
    this.editStudentAddress(row);
  }
  private editStudentSiblingData(row: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, StudentSiblingDataComponent);
    (dialogRef.componentInstance as any).row = row;
    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
  public openStudentSiblingData(row: any) {
    this.editStudentSiblingData(row);
  }


  private applyLeave(row: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, StudentApplyLeaveComponent);
    (dialogRef.componentInstance as any).row = row;
    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
  public openApply(row: any) {
    this.applyLeave(row);
  }
  private openWebStudentRegistrationList(id: number = 0, dbops: DBOperation, modalTitle: string, modalBtnTitle: string) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, WebStudentRegistrationListComponent);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
  public showWebStudentRegistrationList() {
    this.openWebStudentRegistrationList(0, DBOperation.create, 'Adding_New_Customer', 'Add');
  }
  private editDialogManage(row: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, AddupdateStudentMasterComponent);
    (dialogRef.componentInstance as any).row = row;
    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
  public edit(row: any) {
    this.editDialogManage(row);
  }
  private openNotification(row: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, AddUpdateStudentNotificationComponent);
    (dialogRef.componentInstance as any).row = row;
    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
  public notification(row: any) {
    this.openNotification(row);
  }
  private openStudentFeeHistory(row: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, StudentFeeHistoryComponent);
    (dialogRef.componentInstance as any).row = row;
    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
  public studentFeeHistory(row: any) {
    this.openStudentFeeHistory(row);
  }
  public studentArrivalAtGate(row: any) {
    // send Notification for student arrived at the school gate
    this.apiService.getall(`WebNotification/SaveStudentTemplateNoticefication/${row.stuAdmNum}/1`)
      .subscribe(res => {
        this.utilService.OkMessage();
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });
  }

  exportToExcel() {
    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete) {
        this.isExpoerting = true;
        this.apiService.downloadfile('ExcelExport/exportStudentRegistrations?action=stdmst').subscribe(data => {
          this.utilService.downLoadExcel('student_master_list.xlsx', data);
          this.isExpoerting = false;
          //window.URL.revokeObjectURL(url);
        });
      }
    });
  }

  onFileChanged(event: any) {
    let reader = new FileReader();
    if (event.target.files && event.target.files.length > 0) {
      this.files = event.target.files[0];
      this.hasFile = true;
    }
  }

  importFile() {
    if (this.hasFile) {
      let formData = new FormData();
      formData.append('file', this.files, this.files.name);

      //formData.append("studentImageFileName", this.authService.ApiEndPoint().replace("api", "") + 'Signaturefiles/');
      //formData.append("fatherSignatureFileName", this.authService.ApiEndPoint().replace("api", "") + 'Signaturefiles/');
      //formData.append("motherSignatureFileName", this.authService.ApiEndPoint().replace("api", "") + 'Signaturefiles/');

      const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
      (dialogRef.componentInstance as any).modalTitle = `Are you sure to import ${this.files.name}?`;

      dialogRef.afterClosed().subscribe(canDelete => {
        if (canDelete) {
          this.isImporting = true;
          const pathLocation = this.authService.ApiEndPoint().replace("api", "") + 'Signaturefiles/';
          this.apiService
            .post(`excelImport/importStudentMasters?pathLocation=${pathLocation}`, formData)
            .subscribe(
              (res) => {
                this.isImporting = false;
                this.utilService.OkMessage();
                this.refresh();
              },
              (error) => {
                this.isImporting = false;
                this.utilService.ShowApiErrorMessage(error);
                //this.uploadError = error;
              }
            );
        }
      })


    }
    else
      this.notifyService.showError('please import excel file');
  }

}
