import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';
import { ValidationService } from '../../sharedcomponent/ValidationService';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { DBOperation } from '../../services/utility.constants';
import { AddUpdateSkillsetComponent } from './add-update-skillset/add-update-skillset.component';
import { DeleteConfirmDialogComponent } from '../../sharedcomponent/delete-confirm-dialog';
import { ParentOptMgtComponent } from '../../sharedcomponent/parentoptmgt.component';
@Component({
  selector: 'app-skillset',
  templateUrl: './skillset.component.html'
})
export class SkillsetComponent extends ParentOptMgtComponent implements OnInit{
 
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  //@ViewChild(MatPaginator) paginator: MatPaginator;
  //@ViewChild(MatSort) sort: MatSort;
  displayedColumns: string[] = ['skillSetCode', 'nameInEnglish', 'nameInArabic', 'totalSkillsetCost', 'minMarginRequired', 'Actions'];
  data: MatTableDataSource<any> ;
  totalItemsCount: number;
  form: FormGroup;
  searchValue: string = '';
  isLoading: boolean = false;
  sortingOrder: string = 'id desc';
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService,
    private notifyService: NotificationService, private validationService: ValidationService, public pageService: PaginationService, public dialog: MatDialog) {
    super(authService);
  }


  ngOnInit(): void {
    this.initialLoading();
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
 

  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({})
  }

 

  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
    this.isLoading = true;
    this.apiService.getPagination('Skillset/getSkillsetsPagedList', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
      this.totalItemsCount = 0;

      this.data = new MatTableDataSource(result.items);
      this.totalItemsCount = result.totalCount

      setTimeout(() => {
        this.paginator.pageIndex = page as number;
        this.paginator.length = this.totalItemsCount;
      });
      //this.data.paginator = this.paginator;

      this.data.sort = this.sort;
      this.isLoading = false;
    }, error => this.utilService.ShowApiErrorMessage(error));
  }

  initialLoading() {
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
  }
  private openDialogManage(id: number = 0, dbops: DBOperation, modalTitle: string, modalBtnTitle: string) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, AddUpdateSkillsetComponent);
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
    this.openDialogManage(0, DBOperation.create, 'Adding_New_Skillset', 'Add');
  }
  public edit(id: number) {
    this.openDialogManage(id, DBOperation.update, 'Updating_Skillset', 'Update');
  }

  refresh() {
    this.searchValue = '';
    this.initialLoading();
  }
  applyFilter(searchVal: any) {
    const search = searchVal;//.target.value as string;
    //if (search && search.length >= 3) {
    if (search) {
      this.searchValue = search;
      this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
    }
  }
  public delete(id: number) {
    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete && id > 0) {
        this.apiService.delete('Skillset', id).subscribe(res => {
          this.utilService.OkMessage();
          this.ngOnInit();
        },
        );
      }
    },
      error => this.utilService.ShowApiErrorMessage(error));
  }
}
