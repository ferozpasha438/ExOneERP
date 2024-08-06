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
  selector: 'app-add-update-holiday',
  templateUrl: './add-update-holiday.component.html'
})
export class AddUpdateHolidayComponent extends ParentSchoolMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort!: MatSort;
  displayedColumns: string[] = ['branchCode', 'hName', 'hDate', 'Actions'];
  data!: MatTableDataSource<any>;
  totalItemsCount!: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  id: number = 0;
  branchCode: string = '';
  form!: FormGroup;
  isArab: boolean = false;
  row: any;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private translate: TranslateService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService, public dialogRef: MatDialogRef<AddUpdateHolidayComponent>) {
    super(authService);
  }
  ngOnInit(): void {
    this.form = this.fb.group({
      'hName': ['', Validators.required],
      'hName2': ['', Validators.required],
      'hDate': ['', Validators.required]
    });
    if (this.row) {
      this.branchCode = this.row['branchCode'];
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
    this.id = 0;
    this.apiService.getQueryString(`Branch/GetBranchHolidays?branchCode=`, this.branchCode).subscribe(res => {
      if (res) {
        this.totalItemsCount = 0;
        this.data = new MatTableDataSource(res);
        this.totalItemsCount = res.length;
        this.data.sort = this.sort;
        this.isLoading = false;
      }
    });
    //this.loadList(0, this.pageService.pageCount, "", this.sortingOrder);
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
    this.apiService.getPagination('Branch/GetBranchHolidays', this.utilService.getQueryString(page, pageCount, query, orderBy, "", "", this.id)).subscribe(result => {
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
    this.form.value['id'] = this.id;
    this.form.value['branchCode'] = this.branchCode;
    this.form.value['hDate'] = this.utilService.selectedDate(this.form.controls['hDate'].value);
    if (this.form.valid) {
      this.apiService.post('Branch/AddUpdateHolidays', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.reset();
          //this.dialogRef.close(true);
          this.initialLoading();
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
    this.id = 0;
  }
  closeModel() {
    this.dialogRef.close();
  }
  editHoliday(item: any) {
    this.id = item.id;
    this.branchCode = item.branchCode;
    if (item.id > 0) {
      this.form.patchValue({
        'branchCode': item.branchCode,
        'hName': item.hName,
        'hName2': item.hName2,
        'hDate': item.hDate
      });
    }
  }
}
