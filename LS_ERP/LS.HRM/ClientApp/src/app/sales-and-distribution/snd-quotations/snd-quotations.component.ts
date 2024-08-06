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
import { SndServicesService } from '../snd-services.service';
import { AddupdatesndquotationComponent } from './addupdatesndquotation/addupdatesndquotation.component';
import { QuotationstockavailabilityPopupComponent } from './addupdatesndquotation/quotationstockavailability-popup/quotationstockavailability-popup.component';
import { SndquotationprintingComponent } from './sndquotationprinting/sndquotationprinting.component';
import { SndquotationviewComponent } from './sndquotationview/sndquotationview.component';

@Component({
  selector: 'app-snd-quotations',
  templateUrl: './snd-quotations.component.html'
})
export class SndQuotationsComponent extends ParentSalesMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  //@ViewChild(MatPaginator) paginator: MatPaginator;
  //@ViewChild(MatSort) sort: MatSort;
  displayedColumns: string[] = ['quotationNumber', 'revisedNumber', 'quotationDate', 'companyName', 'quotationRefNumber', 'warehouseName', /*'branchName', */'customerName', 'totalAmount', 'lpoContract', 'paymentTermId', 'taxIdNumber', 'quotationDueDate', 'Actions'];
  data: MatTableDataSource<any>;
  totalItemsCount: number = 0;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
 // IsSettlentOpened: boolean = false;
  IsApproved: boolean = false;
  statusId: string = '';
  


  itemList: Array < any > =[];
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
    this.initialLoading();
    const approvalList = document.getElementById('approvalList') as HTMLSelectElement;
    approvalList.value = "";
   

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

  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined, approval: string = "") {
    this.isLoading = true;
    const approvalList = document.getElementById('approvalList') as HTMLSelectElement;
    approval = approvalList.value;
    this.apiService.getPagination('generateSndQuotation', this.utilService.getQueryString(page, pageCount, query, orderBy, approval, this.statusId, this.itemCode ?? 0)).subscribe(result => {
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

  loadQuotations(evt: any) {
    const approvalList = document.getElementById('approvalList') as HTMLSelectElement;
  //  approvalList.value = "";

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
    (dialogRef.componentInstance as any).id = id;
    (dialogRef.componentInstance as any).moduleFile = moduleFile;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true) {
        this.initialLoading();
      }
    });
  }



  private openDialogManage2<T>(id: number = 0,isRevising:boolean=true, dbops: DBOperation, modalTitle: string = '', modalBtnTitle: string = '', component: T, moduleFile: MultiFileUploadDto = { module: '00', action: '00act' }, width: number = 100) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, component, width);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).id = id;
    (dialogRef.componentInstance as any).moduleFile = moduleFile;
    (dialogRef.componentInstance as any).moduleFile = moduleFile;
    (dialogRef.componentInstance as any).isRevising = isRevising;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true) {
        this.initialLoading();
      }
    });
  }

  private openDialogManage3<T>(inputData:any, component: T) {
    let dialogRef = this.sndService.openAutoWidthDialog(this.dialog, component);
    
    (dialogRef.componentInstance as any).inputData = inputData;
    

    dialogRef.afterClosed().subscribe(res => {
     
    });
  }







  private openDialogManageApproval<T>(row: any, id: number = 0, dbops: DBOperation, modalBtnTitle: string = '', component: T, moduleFile: MultiFileUploadDto = { module: '00', action: '00act' }, width: number = 100) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, component, width);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).serviceType = 1;     //SndInvoice=0,SndQuotation=1 
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
      this.openDialogManage(0, DBOperation.create, this.translate.instant('Create_New_Sales_Quotation'), '', AddupdatesndquotationComponent);
    }
  }

  public edit(row: any) {
    if (!this.isLoading) {
      this.openDialogManage(row.id, DBOperation.update, this.translate.instant('Create_New_Sales_Quotation'), '', AddupdatesndquotationComponent);
    }
  }

  public revise(row: any) {
    if (!this.isLoading) {
      this.openDialogManage2(row.id, true, DBOperation.update, this.translate.instant('Revise_Quotation'), '', AddupdatesndquotationComponent);
    }
  }
  public view(row: any) {
    if (!this.isLoading) {
      this.openDialogManage(row.id, DBOperation.create, '', '', SndquotationviewComponent, { action: '', module: '' }, 100);
    }
  }

  public converToOrder(row: any) {


  }
  


  public approve(row: any) {
    if (!this.isLoading) {

      this.openDialogManageApproval(row, row.id, DBOperation.create, '', SnduserapprovalComponent, { action: '', module: '' }, 50);
    }
    else {
      this.notifyService.showError(this.translate.instant("Please_Wait") + "...");
      }
  }
  public convertQuotationToDeliveryNote(row: any) {
    if (this.isLoading) {
      this.notifyService.showError(this.translate.instant("Please_Wait") + "...");
    
    }
    else {
      let inputData: any = { id: row.id, operation: "ConvertQuotationToDeliveryNote" };
      this.openConfirmationDialog(DBOperation.create, 'Are_You_Sure?', ConfirmationDialogWindowComponent, inputData);
    }
  }




  printQuotation() {
    this.openDialogManage(0, DBOperation.create, '', '', SndquotationprintingComponent);
  }

  getStatusColor(row: any): string {

    if (row?.isVoid)
      return `statusVoid`;

    else if (row?.isApproved && (!row?.isConvertedSndQuotationToDeliveryNote && !row?.isConvertedSndQuotationToOrder && !row?.isConvertedSndQuotationToInvoice ))
      return `statusApproved`;
    else if (row?.isConvertedSndQuotationToDeliveryNote)
    {
      return `statusConvertedToDeliveryNote`;
    }
    else if (row?.isConvertedSndQuotationToInvoice)
    {
      return `statusConvertedToInvoice`;
    }else if (row?.isConvertedSndQuotationToOrder)
    {
      return `statusConvertedToOrder`;
    }
    else
      return `statusOpen`;

  }

  private openConfirmationDialog(dbops: DBOperation, modalTitle: string, Component: any, InputData: any) {
    let dialogRef = this.sndService.confirmationDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).InputData = InputData;
    dialogRef.afterClosed().subscribe(res => {
      if (res == 'true') {
        if (InputData.operation == "deleteQuotation") {
          let Dto: any = { id: InputData.id }
          this.apiService.post('GenerateSndQuotation/cancelSndlQuotation', Dto)
            .subscribe(res2 => {
              this.utilService.OkMessage();

              this.ngOnInit();
            },
              error => {
                console.error(error);
                this.utilService.ShowApiErrorMessage(error);
              });

        }
        else if (InputData.operation =="ConvertQuotationToDeliveryNote") {
          this.isLoading = true;
          this.apiService.post('SndDeliveryNote/generateSndDeliveryNoteByQuotationId', { id: InputData.id }).subscribe(res3 => {
            this.utilService.OkMessage();
              this.isLoading = false;
            this.ngOnInit();
          },
            error => {
              console.error(error);
              this.utilService.ShowApiErrorMessage(error);
              this.isLoading = false;

            });
        }
        else if (InputData.operation =="ConvertQuotationToInvoice") {
          this.isLoading = true;
          this.apiService.post('GenerateSndInvoice/convertQuotationToInvoiceByQuotationId', { id: InputData.id }).subscribe(res3 => {
            this.utilService.OkMessage();
              this.isLoading = false;
            this.ngOnInit();
          },
            error => {
              console.error(error);
              this.utilService.ShowApiErrorMessage(error);
              this.isLoading = false;

            });
        }

      }
      else {
        console.log("False");
        this.isLoading = false;
      }
    });
  }


  cancelQuotation(row: any) {
    if (this.isLoading) {
      this.notifyService.showError(this.translate.instant("Please_Wait") + "...");

    }
    else {
      let inputData: any = {
        operation: "deleteQuotation",
        id: row.id
      };
      this.openConfirmationDialog(DBOperation.create, 'Are_You_Sure?', ConfirmationDialogWindowComponent, inputData);

    }
    

  }

  checkStock(row: any) {
    let inputData:any={ id:row.id,source:'Q'};
    this.openDialogManage3(inputData, QuotationstockavailabilityPopupComponent);
  }


  convertQuotationToInvoice(row:any) {
    if (this.isLoading) {
      this.notifyService.showError(this.translate.instant("Please_Wait") + "...");

    }
    else {
      let inputData: any = { id: row.id, operation: "ConvertQuotationToInvoice" };
      this.openConfirmationDialog(DBOperation.create, 'Are_You_Sure?', ConfirmationDialogWindowComponent, inputData);
    }
  }
}
