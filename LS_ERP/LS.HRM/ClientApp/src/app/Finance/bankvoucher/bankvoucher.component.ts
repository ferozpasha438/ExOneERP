import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { DBOperation } from '../../services/utility.constants';
import { UtilityService } from '../../services/utility.service';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { ParentFinMgtComponent } from '../../sharedcomponent/parentfinmgt.component';
import { ValidationService } from '../../sharedcomponent/ValidationService';
import { AddupdatebankvoucherComponent } from './sharedpages/addupdatebankvoucher/addupdatebankvoucher.component';
import { BvprintComponent } from './sharedpages/bvprint/bvprint.component';
import { BvPostingComponent } from './sharedpages/posting/posting.component';
import { BvUserapprovalComponent } from './sharedpages/userapproval/userapproval.component';

@Component({
  selector: 'app-bankvoucher',
  templateUrl: './bankvoucher.component.html',
  styles: [
  ],
  
})
export class BankvoucherComponent extends ParentFinMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  //@ViewChild(MatPaginator) paginator: MatPaginator;
  //@ViewChild(MatSort) sort: MatSort;
  //displayedColumns: string[] = ['Bvnumber', 'Date', 'Branch', 'Amount', 'Batch', 'DocNumber', 'Remarks', 'Approved', 'AppDate', 'Posted', 'PostedDate', 'Actions'];
  displayedColumns: string[] = ['VoucherNumber', 'JvDate', 'ChequeNumber', 'TrnMode', 'BranchCode', 'Amount', 'Source', 'DocNum', 'Remarks', 'Approved', /*'ApprovedDate', */ 'Posted', 'Void', 'PostedDate', 'Actions'];
  data: MatTableDataSource<any>;
  totalItemsCount: number;

  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;

  constructor(private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public pageService: PaginationService, public dialog: MatDialog) {
    super(authService);
  }


  ngOnInit(): void {
    //this.authService.SetApiEndPoint(this.authService.GetSystemSetupApiEndPoint());
    //this.authService.SetApiEndPoint('my own end point');
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
    // sort.direction.direction === 'asc' ? 'desc' : 'asc';
    // console.log(sort.active + ' ' + sort.direction);

    this.sortingOrder = sort.active + ' ' + sort.direction;
    this.totalItemsCount = 0;
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
  }


  onPageSwitch(event: PageEvent) {
    this.pageService.change(event);
    this.loadList(event.pageIndex, event.pageSize, this.searchValue, this.sortingOrder);
  }

  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
    this.isLoading = true;

    this.apiService.getPagination('bankVoucher', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
      this.totalItemsCount = 0;
      this.data = new MatTableDataSource(result.items);

      this.totalItemsCount = result.totalCount;

      setTimeout(() => {
        this.paginator.pageIndex = page as number;
        this.paginator.length = this.totalItemsCount;
      });
      this.data.sort = this.sort;

      //console.log(this.data.sort)
      //console.log(this.data.paginator)

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

  private openDialogManage<T>(id: number = 0, dbops: DBOperation, modalTitle: string = '', modalBtnTitle: string = '', component: T, width: number = 100) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, component, width);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).id = id;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }

  public create() {
    this.openDialogManage(0, DBOperation.create, this.translate.instant('Create_New_Sales_Invoice'), '', AddupdatebankvoucherComponent);
  }

  public edit(id: number) {
    this.openDialogManage(id, DBOperation.update, this.translate.instant('Create_New_Sales_Invoice'), '', AddupdatebankvoucherComponent);
  }

  public print(id: number) {
    this.openDialogManage(id, DBOperation.update, this.translate.instant('Create_New_Sales_Invoice'), '', BvprintComponent);
  }
  public printList(id: number) {
    alert(id)
  }
  public approve(id: number) {
    this.openDialogManage(id, DBOperation.create, '', '', BvUserapprovalComponent, 50);
  }
  public posting(id: number) {
    this.openDialogManage(id, DBOperation.create, 'posting', '', BvPostingComponent, 50);
  }
  public voidStatus(id: number) {
    this.openDialogManage(id, DBOperation.create, 'void', '', BvPostingComponent, 50);
  }


  //public view(id: number) {
  //  this.openDialogManage(id, DBOperation.create, '', '', PurchaseinvoiceComponent, { action: '', module: '' }, 100);
  //}
  //public approve(id: number) {

  //  //let ele = document.getElementById('edit_' + id) as HTMLElement;
  //  //ele.style.display = 'none';

  //  this.openDialogManage(id, DBOperation.create, '', '', ApUserapprovalComponent, { action: '', module: '' }, 50);
  //}


  //getData(): Array<any> {
  //  let data: Array<any> = [
  //    { "jvnumber": "1", "date": "10/8/2001 ", "branch": "Banjara Hills", "amount": "$1,500", "source": "GL", "document": "Doc-001", "remarks": "Nil", "approved": "no", "appdate": " ", "posted": "no", "posteddate": " ", "id": 8 },
  //    { "jvnumber": "1", "date": "10/8/2001 ", "branch": "Banjara Hills", "amount": "$1,500", "source": "GL", "document": "Doc-001", "remarks": "Nil", "approved": "no", "appdate": " ", "posted": "no", "posteddate": " ", "id": 9 },
  //    { "jvnumber": "1", "date": "10/8/2001 ", "branch": "Banjara Hills", "amount": "$1,500", "source": "GL", "document": "Doc-001", "remarks": "Nil", "approved": "no", "appdate": " ", "posted": "no", "posteddate": " ", "id": 10 }

  //  ]
  //  return data;
  //}
}
