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
import { SnduserapprovalComponent } from '../sharedpages/snduserapproval/snduserapproval.component';
import { QuotationstockavailabilityPopupComponent } from '../snd-quotations/addupdatesndquotation/quotationstockavailability-popup/quotationstockavailability-popup.component';
import { SndServicesService } from '../snd-services.service';
import { EditdeliverynoteComponent } from './editdeliverynote/editdeliverynote.component';
import { SnddeliverynoteviewComponent } from './snddeliverynoteview/snddeliverynoteview.component';


@Component({
  selector: 'app-snd-delivery-notes',
  templateUrl: './snd-delivery-notes.component.html'
})
export class SndDeliveryNotesComponent extends ParentSalesMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  //@ViewChild(MatPaginator) paginator: MatPaginator;
  //@ViewChild(MatSort) sort: MatSort;
  displayedColumns: string[] = ['deliveryNumber', 'quotationNumber','revisedNumber', 'quotationDate', 'companyName', 'quotationRefNumber', 'warehouseName', /*'branchName', */'customerName', 'totalAmount', 'lpoContract', 'paymentTermId', 'taxIdNumber', 'quotationDueDate', 'Actions'];
  data: MatTableDataSource<any>;
  totalItemsCount: number = 0;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  // IsSettlentOpened: boolean = false;
  IsApproved: boolean = false;
  statusId: string = '';
  selectListType: string = "";


  itemList: Array<CustomSelectListItem> = [];
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
   
  }

  refresh() {
    this.searchValue = '';
    this.itemCode = 0;
    this.sortingOrder = 'id desc';
    this.initialLoading();
    const approvalList = document.getElementById('approvalList') as HTMLSelectElement;
    approvalList.value = "";
    this.selectListType = "";

  }

  

  initialLoading() {
    this.isLoading = true;
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
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
    const approvalList = document.getElementById('approvalList') as HTMLSelectElement;
    approval = approvalList.value;
    this.apiService.getPagination('SndDeliveryNote', this.utilService.getQueryString(page, pageCount, query, orderBy, approval, this.statusId, this.itemCode ?? 0)).subscribe(result => {
      // this.totalItemsCount = 0;
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
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder, evt.target.value);
  }


  setStatus(id: string) {
    this.statusId = `${id}`;
  }

  private openDialogManage<T>(id: number = 0, dbops: DBOperation, modalTitle: string = '', modalBtnTitle: string = '', component: T, moduleFile: MultiFileUploadDto = { module: '00', action: '00act' }, width: number = 100) {
    let dialogRef = this.sndService.openAutoWidthDialog(this.dialog, component);
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
  private openDialogManage2<T>(id: number = 0, dbops: DBOperation, modalTitle: string = '', modalBtnTitle: string = '', component: T, moduleFile: MultiFileUploadDto = { module: '00', action: '00act' }, width: number = 100) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, component);
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


 



 
  














  public editDeliveryNote(row: any) {
    if (!this.isLoading) {
      this.openDialogManage(row.id, DBOperation.update, this.translate.instant('Edit_Delivery_Note'), '', EditdeliverynoteComponent);
    }
  }


  public view(row: any) {
    if (!this.isLoading) {
      this.openDialogManage2(row.id, DBOperation.create, '', '', SnddeliverynoteviewComponent, { action: '', module: '' }, 100);
    }
  }


  public convertToInvoice(row: any) {

    if (this.isLoading) {
      this.notifyService.showError(this.translate.instant("Please_Wait") + "...");

    }
    else {
      let inputData: any = {
        operation: "ConvertDeliverynoteToInvoice",
        id: row.id,
      };
           this.openConfirmationDialog(DBOperation.create, 'Are_You_Sure? Do You Want to Convert Delivery Note To Invoice?', ConfirmationDialogWindowComponent, inputData);

    }

  }







  print() {

  }

  getStatusColor(row: any): string {

    if (row?.isVoid)
      return `statusVoid`;

    else if (row?.isConvertedDeliveryNoteToInvoice)
      return `statusConvToInv`
    else

      return `statusOpen`;

  }

  


  cancel(row: any) {
    if (this.isLoading) {
      this.notifyService.showError(this.translate.instant("Please_Wait") + "...");

    }
    else {
      let inputData: any = {
        operation: "deleteDeliveryNote",
        id: row.id
      };
      this.openConfirmationDialog(DBOperation.create, 'Are_You_Sure? Do You Want Delete?', ConfirmationDialogWindowComponent, inputData);

    }
  }

  private openConfirmationDialog(dbops: DBOperation, modalTitle: string, Component: any, InputData: any) {
    let dialogRef = this.sndService.confirmationDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).InputData = InputData;
    dialogRef.afterClosed().subscribe(res => {
      if (res == 'true') {
        if (InputData.operation == "deleteDeliveryNote") {
          let Dto: any = { id: InputData.id }
          this.apiService.post('SndDeliveryNote/cancelSndDeliveryNote', Dto)
            .subscribe(res => {
              this.utilService.OkMessage();

              this.ngOnInit();
            },
              error => {
                console.error(error);
                this.utilService.ShowApiErrorMessage(error);
              });

        }

        else if (InputData.operation == 'ConvertDeliverynoteToInvoice') {
          let Dto: any = { id: InputData.id }
          this.apiService.post('GenerateSndInvoice/convertDeliveryNoteToInvoiceByDeliveryNoteId', Dto)
            .subscribe(res => {
              this.utilService.OkMessage();

              this.ngOnInit();
            },
              error => {
                console.error(error);
                this.utilService.ShowApiErrorMessage(error);
              });
        }
      }
      else {
        console.log("False");
        this.isLoading = false;
      }
    });
  }


  checkStock(row: any) {
    let inputData: any = { id: row.id, source: 'D' };
    this.openDialogManage3(inputData, QuotationstockavailabilityPopupComponent);
  }

  private openDialogManage3<T>(inputData: any, component: T) {
    let dialogRef = this.sndService.openAutoWidthDialog(this.dialog, component);

    (dialogRef.componentInstance as any).inputData = inputData;


    dialogRef.afterClosed().subscribe(res => {

    });
  }

}

