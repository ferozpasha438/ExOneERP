import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { stream } from 'xlsx';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { DBOperation } from '../../services/utility.constants';
import { UtilityService } from '../../services/utility.service';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { ParentSchoolMgtComponent } from '../../sharedcomponent/parentschoolmgt.component';
import { ValidationService } from '../../sharedcomponent/ValidationService';
import * as XLSX from "xlsx";
import { CustomSelectListItem } from '../../models/MenuItemListDto';


@Component({
  selector: 'app-term-due-payment',
  templateUrl: './term-due-payment.component.html',
  styleUrls: []
})
export class TermDuePaymentComponent extends ParentSchoolMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort!: MatSort;
  displayedColumns: any[] = [];
  //data: any[] = [];
  data!: MatTableDataSource<any>;
  totalItemsCount!: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  id: number = 0;
  form!: FormGroup;
  isArab: boolean = false;
  termCodeList: Array<any> = [];
  branchCodeList: Array<any> = [];
  branchCode: string = '';
  gradeCode: string = '';
  academicYear: number = 0;
  isShow: boolean= false;
  resultList: Array<any> = [];
  resultData: string[] = [];
  gradeList: Array<CustomSelectListItem> = [];
  acYearList: Array<CustomSelectListItem> = [];
  reportData: Array<any> = [];
  filter: any = { branchCode: '' };

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private translate: TranslateService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService) {
    super(authService);
    //this.displayedColumns = result.items.concat(this.columns.map(x => x.columnDef));
    this.apiService.getall('reports/getFeeTerms').subscribe(res => {
      this.termCodeList = res;
      this.displayedColumns = ['stuAdmNum', 'stuName', 'gradeCode', 'gradeSectionCode'];
      for (var i = 0; i < res.length; i++) {
        this.displayedColumns.push(res[i].toLowerCase());
      }
      this.displayedColumns.push('feeDue');
      this.isShow = true;
    });
  }
  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.initialLoading();
  }
  refresh() {
    this.reportData = [];
    this.searchValue = '';
    this.sortingOrder = 'id desc';
    this.initialLoading();
    this.filter = { branchCode: '' };
    this.branchCode = this.gradeCode = '';
    this.academicYear = 0;
  }
  initialLoading() {
    this.apiService.getall('schoolBranches/getSchoolBranchList').subscribe(res => {
      this.branchCodeList = res;
    });
    this.apiService.getall('acedemicClassGrade/getSelectAcedemicClassGrades').subscribe(res => {
      this.gradeList = res;
    });
    this.apiService.getall('academicBatches/getSelectSchoolAcademicBatches').subscribe(res => {
      this.acYearList = res;
    });
  }
  loadResultList() {
    this.loadList(0, this.pageService.pageCount, "", this.sortingOrder);
    this.filter = { branchCode: this.branchCode };
  }
  onSortOrder(sort: any) {
    this.reportData = [];
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
    this.apiService.getPagination('Reports/TermDuePaymentReport', this.utilService.getQueryString(page, pageCount, query, orderBy, this.gradeCode, "", this.academicYear, this.branchCode)).subscribe(result => {
      this.totalItemsCount = 0;
      //for (var i = 0; i < result.items.length; i++) {
      //  this.resultData = [];
      //  this.resultData.push(result.items[i].stuAdmNum);
      //  this.resultData.push(result.items[i].stuName);
      //  this.resultData.push(result.items[i].gradeCode);
      //  this.resultData.push(result.items[i].gradeSectionCode);
      //  for (var j = 0; j < this.termCodeList.length; j++) {
      //    this.resultData.push(result.items[i].termDues[this.termCodeList[j]].toString());
      //  }
      //  this.resultData.push(result.items[i].feeDue);
      //  this.resultList.push(this.resultData);
      //}
      //this.data = new MatTableDataSource(this.resultList);

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
    this.reportData = [];
  }

  private loadListReport(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
    this.isLoading = true;
    this.apiService.getPagination('Reports/TermDuePaymentReport', this.utilService.getQueryString(page, pageCount, query, orderBy, this.gradeCode, "", this.academicYear, this.branchCode)).subscribe(result => {
      this.isLoading = false;
      this.reportData = this.reportData.length == 0 ? result.items : this.reportData.concat(result.items);
    }, error => this.utilService.ShowApiErrorMessage(error));
  }
  sendFeeDueNotification() {
    this.apiService.getall(`Reports/SendTermDuePaymentNotifications/${this.branchCode}`).subscribe(res => {
      if (res) 
        this.utilService.OkMessage();
    });
  }
  generateReport() {
    this.reportData = [];
    for (let i = 0; i <= Math.floor(this.totalItemsCount / 100); i++) {
      let event: any = { pageIndex: i, pageSize: 100, previousPageIndex: i == 0 ? 0 : i - 1, length: 0 };
      this.loadListReport(event.pageIndex, event.pageSize, this.searchValue, this.sortingOrder);
    }
  }
  exportexcel(): void {
    let element = document.getElementById('printcontainer');
    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(element);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
    XLSX.writeFile(wb, "TermDuePaymentReport.xlsx");
  }

  openPrint() {
    if (this.reportData.length > 0) {
      if (!this.isLoading) {
        const printContent = document.getElementById("printcontainer") as HTMLElement;
        const WindowPrt: any = window.open('', '', 'left=0,top=0,width=2000,height=1000,toolbar=0,scrollbars=0,status=0');
        setTimeout(() => {
          WindowPrt.document.write(printContent.innerHTML);
          WindowPrt.document.close();
          WindowPrt.focus();
          WindowPrt.print();
          WindowPrt.close();
        }, 50);
      }
      else {
        this.notifyService.showError("Loading Data,Please Wait...");
      }
    }
    else {
      this.notifyService.showError("No_Data_Found");
    }

  }
  
}

