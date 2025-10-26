import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { UtilityService } from '../../services/utility.service';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { ParentOptMgtComponent } from '../../sharedcomponent/parentoptmgt.component';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
//import { ApiService } from '../../services/api.service';
import { DBOperation } from '../../services/utility.constants';
import { DeleteConfirmDialogComponent } from '../../sharedcomponent/delete-confirm-dialog';
import { ValidationService } from '../../sharedcomponent/ValidationService';
import { TranslateService } from '@ngx-translate/core';
import { ParentSchoolMgtComponent, ParentSchoolAPIMgtComponent } from '../../sharedcomponent/parentschoolmgt.component';
import { AddupdateStudentRegistrationComponent } from '../shared/addupdate-student-registration/addupdate-student-registration.component';



@Component({
  selector: 'app-student-registration',
  templateUrl: './student-registration.component.html',
  styleUrls: []
})
export class StudentRegistrationComponent extends ParentSchoolAPIMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort!: MatSort;
  displayedColumns: string[] = ['regNum', 'studentName', 'grade', 'phone', 'email', 'city', 'Actions'];
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
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private translate: TranslateService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService) {
    super(authService);
  }

  get(): Array<any> {
    return [
      { id: 1, regNum: 'Religion -1', studentName: 'Religion -1', grade: 'Religion -1', phone: 'Religion -1', email: 'Religion -1', idNum: 'Religion -1', city: 'Religion -1' },


    ]
  }

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
    this.isLoading = true;
    this.apiService.getPagination('WebStudentRegistration', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
      this.totalItemsCount = 0;

      this.data = new MatTableDataSource(result.items);
      this.totalItemsCount = result.totalCount


      //this.data.paginator = this.paginator; webStudentRegistration

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
    let dialogRef = this.utilService.openCrudDialog(this.dialog, AddupdateStudentRegistrationComponent);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }



  public create() {
    this.openDialogManage(0, DBOperation.create, 'New_Registration', 'Add');
  }
  public edit(row: any) {
    this.openDialogManage(row.id, DBOperation.update, 'Edit_Registration', 'Add');
  }

  public delete(id: number) {
    //const dialogRef = this.dialog.open(DeleteConfirmDialogComponent, {
    //  width: '350px',
    //  data: `Are you sure to delete ?`
    //});
    // dialogRef.componentInstance.modalTitle = 'Deletion';
    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete && id > 0) {
        this.apiService.delete('WebStudentRegistration', id).subscribe(res => {
          this.refresh();
          this.utilService.OkMessage();
        },
        );
      }
    },
      error => this.utilService.ShowApiErrorMessage(error));
  }

  exportToExcel() {
    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete) {
        this.isExpoerting = true;
        this.apiService.downloadfile('ExcelExport/exportStudentRegistrations?action=stdreg').subscribe(data => {
          this.utilService.downLoadExcel('student_registration_list.xlsx', data);
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

      const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
      (dialogRef.componentInstance as any).modalTitle = `Are you sure to import ${this.files.name}?`;

      dialogRef.afterClosed().subscribe(canDelete => {
        if (canDelete) {
          this.isImporting = true;
          this.apiService
            .post('excelImport/importWebStudentRegistration', formData)
            .subscribe(
              (res) => {
                this.isImporting = false;
                this.utilService.OkMessage();
                this.refresh();
              },
              (error) => {
                this.isImporting = false;
                this.utilService.ShowApiErrorMessage(error);
              }
            );
        }
      })


    }
    else
      this.notifyService.showError('please import excel file');
  }

}
