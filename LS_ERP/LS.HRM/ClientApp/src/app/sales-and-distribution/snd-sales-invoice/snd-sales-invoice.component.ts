import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';




import { CustomSelectListItem } from '../../models/MenuItemListDto';
import { MultiFileUploadDto } from '../../models/sharedDto';
import { SettlementmodeComponent } from '../../Purchasemgt/sharedpages/settlementmode/settlementmode.component';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { DBOperation } from '../../services/utility.constants';
import { UtilityService } from '../../services/utility.service';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { ParentSalesMgtComponent } from '../../sharedcomponent/parentsalesmgt.component';
import { ValidationService } from '../../sharedcomponent/ValidationService';
import { ConfirmationDialogWindowComponent } from '../confirmation-dialog-window/confirmation-dialog-window.component';
import { SndsettleinvoiceComponent } from '../sharedpages/sndsettleinvoice/sndsettleinvoice.component';
import { SnduserapprovalComponent } from '../sharedpages/snduserapproval/snduserapproval.component';
import { SndServicesService } from '../snd-services.service';
import { AddupdatesndsalesinvoiceComponent } from './addupdatesndsalesinvoice/addupdatesndsalesinvoice.component';
import { EditsndsalesinvoiceComponent } from './editsndsalesinvoice/editsndsalesinvoice.component';
import { SndsalesinvoiceprintingComponent } from './sndsalesinvoiceprinting/sndsalesinvoiceprinting.component';
import { SndsalesinvoiceviewComponent } from './sndsalesinvoiceview/sndsalesinvoiceview.component';


@Component({
  selector: 'app-snd-sales-invoice',
  templateUrl: './snd-sales-invoice.component.html'
})
export class SndSalesInvoiceComponent extends ParentSalesMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  //@ViewChild(MatPaginator) paginator: MatPaginator;
  //@ViewChild(MatSort) sort: MatSort;
  displayedColumns: string[] = ['invoiceNumber', 'invoiceDate', 'companyName', 'invoiceRefNumber', 'warehouseName', /*'branchName', */'customerName', 'totalAmount', 'lpoContract', 'paymentTermId', 'taxIdNumber', 'invoiceDueDate', 'Actions'];
  data: MatTableDataSource<any>;
  totalItemsCount: number = 0;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  IsSettlentOpened: boolean = false;
  IsApproved: boolean = false;
  statusId: string = '';
  selectListType: string = "";


  itemList: Array<any> = [];
  itemCode: number = 0;
 // productList: Array<CustomSelectListItem> = [];
 // productId: number = 0;
  isArab: boolean = false;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private sndService: SndServicesService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public pageService: PaginationService, public dialog: MatDialog) {
    super(authService);
  }


  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.initialLoading();
    this.loadItems();
  }

  refresh() {
    this.searchValue = '';
    this.itemCode = 0;
    this.sortingOrder = 'id desc';
    const approvalList = document.getElementById('approvalList') as HTMLSelectElement;
    approvalList.value = "";
    this.selectListType = "";
    this.initialLoading();
  }

  loadItems() {
    this.apiService.getall("item/getSelectItemMOUList").subscribe(res => {
      if (res) {
        this.itemList = res as Array<any>;
        this.itemList.forEach(e => {
          e.text = e.text + " (" + e.textTwo + ")";
        });
      }
      });
  
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

  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined, approval: string = this.selectListType) {
    this.isLoading = true;
  
    this.apiService.getPagination('generateSndInvoice', this.utilService.getQueryString(page, pageCount, query, orderBy, approval, this.statusId, this.itemCode ?? 0)).subscribe(result => {
      this.isLoading = false;
      this.data = new MatTableDataSource(result.items);

      this.totalItemsCount = result.totalCount;

      setTimeout(() => {
        this.paginator.pageIndex = page as number;
        this.paginator.length = this.totalItemsCount;
      });

      //this.data.paginator = this.paginator;
      //this.data.paginator.length = this.totalItemsCount;

      this.data.sort = this.sort;

      //console.log(this.data.sort)
      //console.log(this.data.paginator)

      this.isLoading = false;
    }, error => this.utilService.ShowApiErrorMessage(error));
  }

  applyFilter(searchVal: any, itemId: any) {
    const search = searchVal;//.target.value as string;
    //if (search && search.length >= 3) {
    if (search || itemId) {
      this.searchValue = search;
      this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
    }
  }

  loadApprovals(evt: any) {
    this.selectListType = evt.target.value;
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder, evt.target.value);
  }

  loadInvoices(evt: any) {
    const approvalList = document.getElementById('approvalList') as HTMLSelectElement;
    approvalList.value = "";

    this.setStatus(evt.target.value);
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder, '');
  }

  setStatus(id: string) {
    this.statusId = `${id}`;
  }

  private openDialogManage<T>(id: number = 0, dbops: DBOperation, modalTitle: string = '', modalBtnTitle: string = '', component: T, moduleFile: MultiFileUploadDto = { module: '00', action: '00act' }, width: number = 100) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, component, width);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).id =id;
    (dialogRef.componentInstance as any).moduleFile = moduleFile;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true) {
        this.initialLoading();
      }
    });
  }


  private openDialogManageApproval<T>(row:any,id: number = 0, dbops: DBOperation,  modalBtnTitle: string = '', component: T, moduleFile: MultiFileUploadDto = { module: '00', action: '00act' }, width: number = 100) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, component, width);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).serviceType = 0;     //SndInvoice=0,SndQuotation=1 
    (dialogRef.componentInstance as any).id = id;
    (dialogRef.componentInstance as any).inputData = row;
    (dialogRef.componentInstance as any).moduleFile = moduleFile;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true) {
        this.initialLoading();
      }
    });
  }
 



  public create() {
    if (!this.isLoading) {
      this.openDialogManage(0, DBOperation.create, this.translate.instant('Create_New_Sales_Invoice'), '', EditsndsalesinvoiceComponent);

    }
  }

  public edit(row: any) {
    if (!this.isLoading) {
      this.openDialogManage(row.id, DBOperation.update, this.translate.instant('Update_Sales_Invoice'), '', EditsndsalesinvoiceComponent);
    }
  }
  public view(row: any) {
    if (!this.isLoading) {
      this.openDialogManage(row.id, DBOperation.create, '', '', SndsalesinvoiceviewComponent, { action: '', module: '' }, 100);
    }
  }

  public approve(row: any) {
    if (!this.isLoading) {
      this.openDialogManageApproval(row, row.id, DBOperation.create, '', SnduserapprovalComponent, { action: '', module: '' }, 50);
    }
  }

  public settlePayment(row: any) {
    // this.IsSettlentOpened = true;
    if (!this.isLoading) {
      this.openDialogManageApproval(row, row.id, DBOperation.create, '', SndsettleinvoiceComponent, { action: '', module: '' }, 75);
    }
  }

  printInvoice() {
    if (!this.isLoading) {
      this.openDialogManage(0, DBOperation.create, '', '', SndsalesinvoiceprintingComponent);
    }
  }

  getStatusColor(row:any):string {

    if (row?.isVoid)
      return `statusVoid`;

    else if (row?.isPaid)
      return `statusPaid`;
    else if (row?.isPosted)
      return `statusPosted`;
    else if (row?.isSettled)
      return `statusSettled`;
     else  if (row?.isApproved) 
      return `statusApproved`;
      else
      return `statusOpen`;
    
  }







  postInvoice(row: any) {
    let dialogRef = this.sndService.confirmationDialog(this.dialog, ConfirmationDialogWindowComponent);
    (dialogRef.componentInstance as any).modalTitle = "Are you Sure? Do You Want To Post Invoice?";
    dialogRef.afterClosed().subscribe(res => {
      if (res==true || res=='true') {
        this.apiService.post('GenerateSndInvoice/postSndInvoice', { Id: row.id})
          .subscribe(res2 => {
            this.isLoading = false;
            this.utilService.OkMessage();

            this.ngOnInit();
          },
            error => {
              this.isLoading = false;
              console.error(error);
              this.utilService.ShowApiErrorMessage(error);
            });
      }
    });
  }

        private openConfirmationDialog(dbops: DBOperation, modalTitle: string, Component: any, InputData: any) {
    let dialogRef = this.sndService.confirmationDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).InputData =InputData;
    dialogRef.afterClosed().subscribe(res => {
      if (res == 'true') {
        if (InputData.operation == "deleteInvoice") {
          this.isLoading = true;
          let Dto: any = { id: InputData.id }
          this.apiService.post('GenerateSndInvoice/cancelSndlInvoice', Dto)
            .subscribe(res2 => {
              this.isLoading = false;
              this.utilService.OkMessage();

              this.ngOnInit();
            },
              error => {
                this.isLoading = false;
                console.error(error);
                this.utilService.ShowApiErrorMessage(error);
              });

        }

      }
      else {
        console.log("False");
      }
    });
  }


  cancelInvoice(row: any) {
    if (!this.isLoading) {
      let inputData: any = {

        operation: "deleteInvoice",
        id: row.id
      };
      this.openConfirmationDialog(DBOperation.create, 'Are_You_Sure?', ConfirmationDialogWindowComponent, inputData);

    } else {
      this.notifyService.showInfo(this.translate.instant("Please_Wait"));
    }
  

  }

 
}

