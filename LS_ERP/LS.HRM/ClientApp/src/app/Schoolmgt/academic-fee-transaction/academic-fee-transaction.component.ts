import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { UtilityService } from '../../services/utility.service';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { ParentSchoolMgtComponent } from '../../sharedcomponent/parentschoolmgt.component';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
//import { ApiService } from '../../services/api.service';
import { DBOperation } from '../../services/utility.constants';
import { ValidationService } from '../../sharedcomponent/ValidationService';
import { TranslateService } from '@ngx-translate/core';
import { AddupdateAcademicFeeTransactionComponent } from '../shared/addupdate-academic-fee-transaction/addupdate-academic-fee-transaction.component';
import { FeeinvoiceComponent } from '../shared/feeinvoice/feeinvoice.component';
import { MultiFileUploadDto } from '../../models/sharedDto';
import { SchoolinvoiceprintingComponent } from '../shared/schoolinvoiceprinting/schoolinvoiceprinting.component';
import { AddupdateAcademicBulkFeeTransactionComponent } from '../shared/addupdate-academic-bulkfee-transaction/addupdate-academic-bulkfee-transaction.component';


@Component({
  selector: 'app-academic-fee-transaction',
  templateUrl: './academic-fee-transaction.component.html',
  styleUrls: []
})
export class AcademicFeeTransactionComponent extends ParentSchoolMgtComponent implements OnInit{
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort!: MatSort;
  displayedColumns: string[] = ['admissionNumber', 'receiptVoucher', 'feeDate', 'feeTerm', 'netFeeAmount', 'paidOnline', 'paidTransNum', 'Actions'];
  data!: MatTableDataSource<any>;
  totalItemsCount!: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  id: number=0;
  form!: FormGroup;
  isArab: boolean = false;
  
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private translate: TranslateService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService) {
    super(authService);
  }

  //get():Array<any>{
  //  return [      
  //    {id:1,voucherNum:'Fee -1',regNum:'Fee -2',studentName:'Fee -3',feeDate:'Branch-1',term:'Branch-1',amount:'Branch-1',paidOnline:'Branch-1',transNum:'Branch-1'},     
  // ]
  // }

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
    //this.data = new MatTableDataSource(this.get());
    this.apiService.getPagination('StudentFeeTransaction', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
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
  private openDialogManage(id: number = 0, dbops: DBOperation, modalTitle: string, modalBtnTitle: string) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, AddupdateAcademicFeeTransactionComponent );
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
    this.openDialogManage(0, DBOperation.create, 'Adding_New_Customer', 'Add');
  }

  private openDialogManageBulkPay(id: number = 0, dbops: DBOperation, modalTitle: string, modalBtnTitle: string) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, AddupdateAcademicBulkFeeTransactionComponent);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }

  public bulkPay() {
    this.openDialogManageBulkPay(0, DBOperation.create, 'Adding_New_BulkPay', 'Add');
  }
  private openDialogManageForPrint<T>(receiptVoucher: string = '', dbops: DBOperation, modalTitle: string = '', modalBtnTitle: string = '', component: T, moduleFile: MultiFileUploadDto = { module: '00', action: '00act' }, width: number = 100) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, component, width);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).receiptVoucher = receiptVoucher;
    (dialogRef.componentInstance as any).moduleFile = moduleFile;
    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true) {
        this.initialLoading();
      }
    });
  }

  private openDialogManageInvoicePrint<T>(id: number = 0, dbops: DBOperation, modalTitle: string = '', modalBtnTitle: string = '', component: T, moduleFile: MultiFileUploadDto = { module: '00', action: '00act' }, width: number = 100) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, component, width);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).id = id;
    (dialogRef.componentInstance as any).moduleFile = moduleFile;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true) {
        this.initialLoading();
      }
    });
  }


  public printFeePayment(receiptVoucher: string) {
    this.openDialogManageForPrint(receiptVoucher, DBOperation.create, '', '', FeeinvoiceComponent, { action: '', module: '' }, 100);
  }

  public schoolview(id: number) {
    this.openDialogManageInvoicePrint(id, DBOperation.create, '', '', SchoolinvoiceprintingComponent, { action: '', module: '' }, 100);
  }
}
