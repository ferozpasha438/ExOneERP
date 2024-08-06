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
import { AddupdatecustomerComponent } from '../../Finance/accountsreceivable/sharedpages/addupdatecustomer/addupdatecustomer.component';
//import { AddupdatecustomerComponent } from '../../Operationalmgt/sharedpages/addupdatecustomer/addupdatecustomer.component';


import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { DBOperation } from '../../services/utility.constants';
import { UtilityService } from '../../services/utility.service';
import { DeleteConfirmDialogComponent } from '../../sharedcomponent/delete-confirm-dialog';
import { FileUploadComponent } from '../../sharedcomponent/fileupload.component';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { ParentOptMgtComponent } from '../../sharedcomponent/parentoptmgt.component';
import { ParentSalesMgtComponent } from '../../sharedcomponent/parentsalesmgt.component';
import { ValidationService } from '../../sharedcomponent/ValidationService';
import { SndcustomerinvoicestatementComponent } from '../sharedpages/sndcustomerinvoicestatement/sndcustomerinvoicestatement.component';
import { SndcustomerstatementComponent } from '../sharedpages/sndcustomerstatement/sndcustomerstatement.component';
//import { SndaddupdatecustomerComponent } from './sndaddupdatecustomer/sndaddupdatecustomer.component';


@Component({
  selector: 'app-snd-customer',
  templateUrl: './snd-customer.component.html'
})
export class SndCustomerComponent extends ParentSalesMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['custCode', 'custName', 'custAlias', 'custAddress1', 'custCatCode', 'isActive', 'Actions'];
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
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
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
    this.apiService.getPagination('CustomerMaster', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
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


  applyFilter(searchVal: any) {
    const search = searchVal;//.target.value as string;
    //if (search && search.length >= 3) {
    if (search) {
      this.searchValue = search;
      this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
    }
  }
  private openDialogManage(id: number = 0, dbops: DBOperation, modalTitle: string, modalBtnTitle: string) {
    //let dialogRef = this.utilService.openCrudDialog(this.dialog, SndaddupdatecustomerComponent);
    let dialogRef = this.utilService.openCrudDialog(this.dialog, AddupdatecustomerComponent);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true) {
        //this.initialLoading();
        location.reload();
      }
      this.isLoading = false;
    });
  }

  private openDialogManageOne<T>(id: string = '', dbops: DBOperation, modalTitle: string = '', component: T) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).customerCode = id;

    //dialogRef.afterClosed().subscribe(res => {
    //  if (res && res === true)
    //    this.initialLoading();
    //});
  }

  public create() {
    this.openDialogManage(0, DBOperation.create, 'Adding_New_Customer', 'Add');
  }

  public edit(id: number) {
    this.openDialogManage(id, DBOperation.update, 'Updating_Customer', 'Update');
  }
  public print(customerCode: string) {
    this.openDialogManageOne(customerCode, DBOperation.update, this.translate.instant('Create_New_Sales_Invoice'), SndcustomerstatementComponent);

  }

  public printCustInvoices(customerCode: string) {
    this.openDialogManageOne(customerCode, DBOperation.update, this.translate.instant('Create_New_Sales_Invoice'), SndcustomerinvoicestatementComponent);

  }

 
  public delete (id: number) {
    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete && id > 0) {
        this.apiService.delete('CustomerMaster', id).subscribe(res => {
          this.utilService.OkMessage();
          this.ngOnInit();

        },
        );
      }
    },
      error => this.utilService.ShowApiErrorMessage(error));
  }
  submit() {

  }

  private openFileUploadDialogManage<T>(modalTitle: string = '', component: T, moduleFile: any, width: number = 80) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, component, width);
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).moduleFile = moduleFile;

    dialogRef.afterClosed().subscribe(res => {
    });
  }

  uploadFile(id: any) {
    this.openFileUploadDialogManage(this.translate.instant('Document_Upload'), FileUploadComponent, { module: 'SND', action: 'SND_CUST', id: id, sourceId: id });
  }
}

