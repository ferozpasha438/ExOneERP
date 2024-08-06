import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { PaginationService } from '../../../sharedcomponent/pagination.service';
import { ParentSchoolMgtComponent } from '../../../sharedcomponent/parentschoolmgt.component';
import { ValidationService } from '../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-student-apply-leave',
  templateUrl: './student-apply-leave.component.html',
  styleUrls: []
})
export class StudentApplyLeaveComponent extends ParentSchoolMgtComponent  implements  OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort!: MatSort;
  displayedColumns: string[] = ['leaveCode', 'leaveReason', 'leaveStartDate', 'leaveEndDate', 'leaveMessage', 'Actions'];
  data!: MatTableDataSource<any>;
  totalItemsCount!: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  id: number = 0;
  form!: FormGroup;
  isArab: boolean = false;
  row: any;
  studentList: Array<any> = [];
  isShowApproval: boolean = false;
  stuAdmNum: string = '';
  stuLeaveId: number = 0;
  leaveCodeList: Array<any> = [];
   isShowForm:boolean = false;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private translate: TranslateService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService, public dialogRef: MatDialogRef<StudentApplyLeaveComponent>) {
    super(authService);
  }
  ngOnInit(): void {
    this.form = this.fb.group({
      'stuAdmNum': ['', Validators.required],
      'stuName': ['', Validators.required],
      'stuName2': ['', Validators.required],
      'leaveCode': ['', Validators.required],
      'leaveReason': ['', Validators.required],
      'leaveStartDate': ['', Validators.required],
      'leaveEndDate': ['', Validators.required],
      'leaveMessage': ['', Validators.required],
      'leaveFile': ['', Validators.required],
      'isApproved': [''],
      'approvalRemarks': [''],
      'approveDate': [''],
      'approvedBy': ['']   
    });
    this.loadData();
    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.stuAdmNum = this.row['stuAdmNum'];
      this.form.patchValue(this.row);
    }
    this.initialLoading();
  }
  loadData() {
    this.apiService.getall('StudentApplyLeave/GetLeaveCodes').subscribe(res => {
      this.leaveCodeList = res;
    });
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
    this.apiService.getPagination('StudentApplyLeave/GetStudentLeaves', this.utilService.getStudentQueryString(this.stuAdmNum, page, pageCount, query, orderBy)).subscribe(result => {
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
      if (this.stuLeaveId > 0)
        this.form.value['id'] = this.stuLeaveId;
      this.form.value['stuAdmNum'] = this.stuAdmNum;
      this.form.value['isApproved'] = false;
      this.form.value['leaveStartDate'] = this.utilService.selectedDate(this.form.controls['leaveStartDate'].value);
      this.form.value['leaveEndDate'] = this.utilService.selectedDate(this.form.controls['leaveEndDate'].value);
      this.apiService.post('StudentApplyLeave', this.form.value)
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
    this.stuLeaveId = item.id;
    if (item.id > 0) {
      this.form.patchValue({
        'leaveCode': item.leaveCode,
        'leaveReason': item.leaveReason,
        'leaveStartDate': item.leaveStartDate,
        'leaveEndDate': item.leaveEndDate,
        'leaveMessage': item.leaveMessage
      });
      this.isShowForm = true;
      this.loadData();
    }
  }
 
}
