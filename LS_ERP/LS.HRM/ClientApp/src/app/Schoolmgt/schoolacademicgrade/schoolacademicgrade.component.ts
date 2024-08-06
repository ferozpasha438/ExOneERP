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
import { ParentSchoolMgtComponent } from '../../sharedcomponent/parentschoolmgt.component';
import { AddupdateschoolacademicgradeComponent } from '../shared/addupdateschoolacademicgrade/addupdateschoolacademicgrade.component';
import { GradesectionmappingComponent } from '../gradesectionmapping/gradesectionmapping.component';
import { SemesterssubjectmappingwithgradeComponent } from '../semesterssubjectmappingwithgrade/semesterssubjectmappingwithgrade.component';
import { AddUpdateGradeDocumentComponent } from '../shared/add-update-grade-document/add-update-grade-document.component';



@Component({
  selector: 'app-schoolacademicgrade',
  templateUrl: './schoolacademicgrade.component.html',
  styleUrls: []
})
export class SchoolacademicgradeComponent extends ParentSchoolMgtComponent implements OnInit {

  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort!: MatSort;
  displayedColumns: string[] = ['gradeCode', 'gradeName', 'gradeNameInArabic','gradeOrder', 'Actions'];
  data!: MatTableDataSource<any>;
  totalItemsCount!: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  id: number = 0;
  form!: FormGroup;
  isArab: boolean = false;

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private translate: TranslateService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService) {
    super(authService);
  }

  get(): Array<any> {
    return [
      { id: 1, gradeCode: 'SOCIAL', gradeName: 'Social Studied', gradeNameInArabic: 'الدراسات الاجتماعية' },
      { id: 1, gradeCode: 'SOCIAL', gradeName: 'Social Studied', gradeNameInArabic: 'الدراسات الاجتماعية' },


    ]
  }

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
    this.isLoading = true;
    this.apiService.getPagination('acedemicClassGrade', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
      this.totalItemsCount = 0;
      this.data = new MatTableDataSource(result.items);
      this.totalItemsCount = result.totalCount


      //this.data.paginator = this.paginator;

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
  private openDialogManage(row: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, AddupdateschoolacademicgradeComponent);
    (dialogRef.componentInstance as any).row = row;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
  private openDialogSection(code: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, GradesectionmappingComponent);
    (dialogRef.componentInstance as any).code = code;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
  private openDialogSemester(code: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, SemesterssubjectmappingwithgradeComponent);
    (dialogRef.componentInstance as any).code = code;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
   
  create() {
    this.openDialogManage(null);
  }
  public edit(row: any) {
    this.openDialogManage(row);
  }

  public SectionOpen(gradeCode: string) {
    this.openDialogSection(gradeCode);
  }
  public SemesterOpen(gradeCode: string) {
    this.openDialogSemester(gradeCode);
  }
  private openDialogUploadDocument(row: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, AddUpdateGradeDocumentComponent);
    (dialogRef.componentInstance as any).row = row;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
  public DocumentPopup(row: any) {
    this.openDialogUploadDocument(row);
  }

  submit() {

  }

}
