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
import { AddupdateschoolacademicbatchesComponent } from '../shared/addupdateschoolacademicbatches/addupdateschoolacademicbatches.component';


@Component({
  selector: 'app-arcustomer',
  templateUrl: './schoolacademicbatches.component.html',
  styles: [
  ],

})

export class SchoolacademicbatchesComponent extends ParentSchoolMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['academicYear', 'startingDate', 'endingDate', 'Actions'];
  data: MatTableDataSource<any>;
  totalItemsCount: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  id: number = 0;
  form: FormGroup;
  isArab: boolean = false;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private translate: TranslateService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService) {
    super(authService);
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

    this.apiService.getPagination('academicBatches', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
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

  get(): Array<any> {
    return [
      { id: 1, academicYear: '2020', startingDate: '20/5/2020', endingDate: '30/5/2020' },

      // {id:1,custCode:'888',custArbName:'2022',custName:'NAME',custAlias:'ALS'}
    ]
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
    let dialogRef = this.utilService.openCrudDialog(this.dialog, AddupdateschoolacademicbatchesComponent);
    (dialogRef.componentInstance as any).row = row;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
  addStudentsToNextGrade(row: any) {
    this.apiService.getall(`SchoolStudentMaster/addStudentsToNextGrade/${row.academicYear}`)
      .subscribe(res => {
        if (res) {
          if (res.data) {
            this.utilService.OkMessage();
          } 
        }        
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });
  }
  addFeeStructure(row: any) {
    this.apiService.getall(`SchoolStudentMaster/addFeeStructure/${row.academicYear}`)
      .subscribe(res => {
        if (res) {
          if (res.data) {
            this.utilService.OkMessage();
          }
        }
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });
  }
  public create() {
    this.openDialogManage(null);
  }
  public edit(row: any) {
    this.openDialogManage(row);
  }

}

