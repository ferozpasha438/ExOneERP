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
  selector: 'app-student-attendance',
  templateUrl: './student-attendance.component.html',
  styleUrls: []
})
export class StudentAttendanceComponent extends ParentSchoolMgtComponent implements OnInit{
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort!: MatSort;
  row: any;
  displayedColumns: string[] = ['attDate', 'inTime', 'outTime', 'flag', 'leaveCode'];
  displayedlist: string[] = ['fromDate', 'toDate', 'noOfDays', 'presentDays', 'absentDays', 'leaveDays', 'holidayDays'];
  data!: MatTableDataSource<any>;
  data2!: MatTableDataSource<any>;
  data3: Array<any>= [];
  totalItemsCount!: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  id: number=0;
  isArab: boolean = false;
  selectedMonth: string = '';
  selectedYear: string = '';
  stuAdmNum: string = '';
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private translate: TranslateService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService, public dialogRef: MatDialogRef<StudentAttendanceComponent>) {
    super(authService);
  }
  //get():Array<any>{
  //  return [
  //    {id:1,attendenceList:'Previous Month',days:'10',absent:'5',leaves:'14',holiday:'3'}, 
  // ]
  // }
  ngOnInit(): void {
    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.stuAdmNum = this.row['stuAdmNum'];
    }
    this.isArab = this.utilService.isArabic();
    this.initialLoading();
  }
  getAttandaceData() {
    if (this.selectedYear != null && this.selectedYear != "" && this.selectedMonth != null && this.selectedMonth!="") {
      this.apiService.getall(`SchoolStudentMaster/GetStudentAttandace/${this.stuAdmNum}/${this.selectedYear}/${this.selectedMonth}`).subscribe(res => {
        if (res) {
          this.totalItemsCount = 0;
          for (var i = 0; i < res.attnDaywiseDataList.length; i++) {
            res.attnDaywiseDataList[i].attDate = this.utilService.selectedDate(res.attnDaywiseDataList[i].attDate);
          }
          this.data = new MatTableDataSource(res.attnDaywiseDataList);
          res.studentAttandanceData.fromDate = this.utilService.selectedDate(res.studentAttandanceData.fromDate);
          res.studentAttandanceData.toDate = this.utilService.selectedDate(res.studentAttandanceData.toDate);
          this.data3 = [];
          this.data3.push(res.studentAttandanceData);
          this.data2 = new MatTableDataSource(this.data3);
          this.totalItemsCount = res.items.attnDaywiseDataList.length;
          setTimeout(() => {
            this.paginator.pageIndex = 1;
            this.paginator.length = this.totalItemsCount;
          });
          this.data.sort = this.sort;
          this.isLoading = false;
        }
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });
    }
  }
  refresh() {
    this.searchValue = '';
    this.sortingOrder = 'id desc';
    this.selectedMonth = '';
    this.selectedYear = '';
    this.getAttandaceData();
    //this.initialLoading();
  }

  initialLoading() {
    //this.loadList(0, this.pageService.pageCount, "", this.sortingOrder);
  }

  onSortOrder(sort: any) {
    this.totalItemsCount = 0;
    this.sortingOrder = sort.active + ' ' + sort.direction;
    //this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
  }


  onPageSwitch(event: PageEvent) {
    this.pageService.change(event);
    //this.loadList(event.pageIndex, event.pageSize, this.searchValue, this.sortingOrder);
  }

  //private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
  //  this.isLoading = true;
  //  this.data2 = new MatTableDataSource(this.get());
  //  this.apiService.getPagination('SchoolStudentAttn/StudentAttendanceByStuAdmNum', this.utilService.getStudentQueryString(this.stuAdmNum,page, pageCount, query, orderBy)).subscribe(result => {
  //    this.totalItemsCount = 0;
  //    this.data = new MatTableDataSource(result.items);
  //    this.totalItemsCount = result.totalCount
  //    //this.data.paginator = this.paginator;
  //    this.data.sort = this.sort;
  //    this.isLoading = false;
  //  }, error => this.utilService.ShowApiErrorMessage(error));
  //}



  applyFilter(searchVal: any) {
    const search = searchVal;//.target.value as string;
    //if (search && search.length >= 3) {
    if (search) {
      this.searchValue = search;
      //this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
    }
  }
  

 
  closeModel() {
    this.dialogRef.close();
  }
  submit() {

  }
}
