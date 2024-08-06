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
  selector: 'app-student-notice-and-messaging',
  templateUrl: './student-notice-and-messaging.component.html',
  styleUrls: []
})
export class StudentNoticeAndMessagingComponent extends ParentSchoolMgtComponent implements OnInit{
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort!: MatSort;
  displayedColumns: string[] = ['noticeDate', 'reasonType', 'reasonCode', 'reportedBy', 'remarks',  'Actions'];
  data!: MatTableDataSource<any>;
  totalItemsCount!: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  id: number=0;
  form!: FormGroup;
  isArab: boolean = false;  
  isShowApproval: boolean = false;
  isShowForm: boolean = false;
  stuAdmNum: string = '';
  selectedReasonType: string = '';
  selectedReasonCode: string = '';
  row: any;
  stuNoticeId: number = 0;
  reasonSortList: Array<any> = [];
  reasonList: Array<any> = [];
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private translate: TranslateService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService, public dialogRef: MatDialogRef<StudentNoticeAndMessagingComponent>) {
    super(authService);
  }
  //get():Array<any>{
  //  return [
  // ]
  // }

  ngOnInit(): void {   
      this.form = this.fb.group({
        'stuAdmNum': ['', Validators.required],
        'stuName': [''],
        'stuName2': [''],
        'noticeDate': ['', Validators.required],
        'reasonType': ['', Validators.required],
        'reasonCode': ['', Validators.required],
        'reportedBy': ['', Validators.required],
        'remarks': [''],
        'approveDate': [''],
        'approvedBy': ['']    
      });
    this.isArab = this.utilService.isArabic();
    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.stuAdmNum = this.row['stuAdmNum'];
      this.form.patchValue(this.row);
    }
    this.initialLoading();
  }
  changeReasonType(type:number) {
    if (type==1) {
      this.apiService.getall(`StudentNoticesReasonCode/GetReasonCodesByType/${this.selectedReasonType}`).subscribe(res => {
        if (res)
          this.reasonSortList = res;
      });
    } else if (type == 2) {
      const reasonType: string = this.form.value['reasonType'] as string;
      this.apiService.getall(`StudentNoticesReasonCode/GetReasonCodesByType/${reasonType}`).subscribe(res => {
        if (res)
          this.reasonList = res;
      });
    }
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
    //this.data = new MatTableDataSource(this.get());
    this.apiService.getPagination('StudentNotices/GetStudentNoticesList', this.utilService.getStudentQueryString(this.stuAdmNum,page, pageCount, query, orderBy)).subscribe(result => {
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
  submit() {
    if (this.form.valid) {
      if (this.stuNoticeId > 0)
        this.form.value['id'] = this.stuNoticeId;
      this.form.value['stuAdmNum'] = this.stuAdmNum;
      this.form.value['isApproved'] = false;
      this.form.value['noticeDate'] = this.utilService.selectedDate(this.form.controls['noticeDate'].value);
      this.apiService.post('StudentNotices', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.reset();
          this.dialogRef.close(true);
        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
          });
    }
    else
      this.utilService.FillUpFields();
  }
  reset() {
    this.form.reset();
  }
  closeModel() {
    this.dialogRef.close();
  }
  showCreateForm() {
    this.isShowForm = true;
  }
  public edit(item: any) {
    this.stuNoticeId = item.id;
    if (item.id > 0) {
      this.form.patchValue({
        'noticeDate': item.noticeDate,
        'reasonType': item.reasonType,
        'reasonCode': item.reasonCode,
        'reportedBy': item.reportedBy,
        'remarks': item.remarks
      });
      this.isShowForm = true;
      this.changeReasonType(2);
    }
  }
}
