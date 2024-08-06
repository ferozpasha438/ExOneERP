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
import { AddupdateTeacherMasterComponent } from '../shared/addupdate-teacher-master/addupdate-teacher-master.component';
import { TeacherLanguageMappingComponent } from '../teacher-language-mapping/teacher-language-mapping.component';
import { TeacherQualificationMappingComponent } from '../teacher-qualification-mapping/teacher-qualification-mapping.component';
import { TeacherSubjectMappingComponent } from '../teacher-subject-mapping/teacher-subject-mapping.component';
import { TeacherClassMappingComponent } from '../teacher-class-mapping/teacher-class-mapping.component';
import { TeacherNotificationComponent } from '../teacher-notification/teacher-notification.component';


@Component({
  selector: 'app-teacher-master',
  templateUrl: './teacher-master.component.html',
  styleUrls: []
})
export class TeacherMasterComponent extends ParentSchoolMgtComponent implements OnInit{
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort!: MatSort;
  //displayedColumns: string[] = ['teacherCode', 'teacherName', 'pPhone1', 'dateJoin', 'pcity', 'nationality', 'idNumber', 'Actions'];
  displayedColumns: string[] = ['teacherCode', 'teacherName1', 'pPhone1','teacherEmail', 'dateJoin', 'pcity', 'nationalityCode', 'nationalityID', 'Actions'];
  data!: MatTableDataSource<any>;
  totalItemsCount!: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  id: number=0;
  form!: FormGroup;
  isArab: boolean = false;
  
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private translate: TranslateService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService) {
    super(authService);
  }

  //get():Array<any>{
  //  return [
  //    {id:1,code:'Leave',teacherName:'Teacher',phone:'9689854865',dateJoin:'25/2/2021',city:'City Name',nationality:'IND',IdNumber:'AD2454254'},       
  //    {id:1,code:'Leave',teacherName:'Teacher',phone:'9689854865',dateJoin:'25/2/2021',city:'City Name',nationality:'IND',IdNumber:'AD2454254'},       
  //    {id:1,code:'Leave',teacherName:'Teacher',phone:'9689854865',dateJoin:'25/2/2021',city:'City Name',nationality:'IND',IdNumber:'AD2454254'},       
  // ]
  // }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.initialLoading();
  }
  refresh() {
    this.searchValue = '';
    this.sortingOrder = 'id desc';
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
    //this.data = new MatTableDataSource(this.get());
    this.apiService.getPagination('TeacherMaster', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
      this.totalItemsCount = 0;
      this.data = new MatTableDataSource(result.items);
      this.totalItemsCount = result.totalCount
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
  private openDialogManage(id: number = 0, dbops: DBOperation, modalTitle: string, modalBtnTitle: string) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, AddupdateTeacherMasterComponent );
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
    this.openDialogManage(0, DBOperation.create, 'Adding_New_Customer', 'Add');
  }
  private openLanguageManage(row: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, TeacherLanguageMappingComponent);
    (dialogRef.componentInstance as any).row = row;
    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
  public languageMapping(row: any) {
    this.openLanguageManage(row);
  }
  private openQualificationManage(row: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, TeacherQualificationMappingComponent);
    (dialogRef.componentInstance as any).row = row;
    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
  public qualificationMapping(row: any) {
    this.openQualificationManage(row);
  }
  private openSubjectManage(row: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, TeacherSubjectMappingComponent);
    (dialogRef.componentInstance as any).row = row;
    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
  public subjectMapping(row: any) {
    this.openSubjectManage(row);
  }
  private openClasstManage(row: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, TeacherClassMappingComponent);
    (dialogRef.componentInstance as any).row = row;
    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
  public classMapping(row: any) {
    this.openClasstManage(row);
  }
  private openNotification(row: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, TeacherNotificationComponent);
    (dialogRef.componentInstance as any).row = row;
    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
  public notification(row: any) {
    this.openNotification(row);
  }
  private editDialogManage(row: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, AddupdateTeacherMasterComponent);
    (dialogRef.componentInstance as any).row = row;
    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
  public edit(row: any) {
    this.editDialogManage(row);
  }
}
