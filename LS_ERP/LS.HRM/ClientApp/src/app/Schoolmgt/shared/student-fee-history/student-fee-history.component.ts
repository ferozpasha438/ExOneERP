import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { UtilityService } from '../../../services/utility.service';
import { PaginationService } from '../../../sharedcomponent/pagination.service';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { TranslateService } from '@ngx-translate/core';
import { ParentSchoolMgtComponent } from '../../../sharedcomponent/parentschoolmgt.component';

@Component({
  selector: 'app-student-fee-history',
  templateUrl: './student-fee-history.component.html',
  styleUrls: []
})
export class StudentFeeHistoryComponent extends ParentSchoolMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort!: MatSort;
  displayedColumns: string[] = ['feeDueDate', 'termCode', 'netFeeAmount','isPaid'];
  data!: MatTableDataSource<any>;
  totalItemsCount!: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  id: number = 0;
  stuAdmNum: string = '';
  form!: FormGroup;
  isArab: boolean = false;
  row: any;
  termCode: string = '';
  feeDetails: Array<any> = [];
  feeList: Array<any> = [];
  stuFeeDetailId: number = 0;
  feeStructCode: string = '';
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private translate: TranslateService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService, public dialogRef: MatDialogRef<StudentFeeHistoryComponent>) {
    super(authService);
  }
  //get():Array<any>{
  //  return [{id:1,termCode:'Term1',dueDate:'22/6/2022',totalFeeAmount:'9999.99',isPaid:'No',paidOn:'10:20'}]
  // }
  ngOnInit(): void {
    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.stuAdmNum = this.row['stuAdmNum'];
    }
    this.isArab = this.utilService.isArabic();
    this.initialLoading();
  }
  refresh() {
    this.searchValue = '';
    this.sortingOrder = 'id desc';
    this.initialLoading();
  }
  initialLoading() {
    this.isLoading = true;
    this.apiService.getQueryString(`schoolStudentMaster/GetStudentFeeHeaderByStuAdmNum?`, `stuAdmNum=${this.stuAdmNum}&year=${this.searchValue}`).subscribe(res => {
      if (res) {
        this.totalItemsCount = 0;
        this.data = new MatTableDataSource(res);
        this.totalItemsCount = res.length;
        this.data.sort = this.sort;
        this.isLoading = false;
      }
    });
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
    //this.data = new MatTableDataSource(this.get());
    this.apiService.getPagination('schoolStudentMaster/getStudentFeeHeaderByStuID', this.utilService.getQueryString(page, pageCount, query, orderBy, "", "", this.id)).subscribe(result => {
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
      this.initialLoading();
      //this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
    }
  }
  
  closeModel() {
    this.dialogRef.close();
  }

}

