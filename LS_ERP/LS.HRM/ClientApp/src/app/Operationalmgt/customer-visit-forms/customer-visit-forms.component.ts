import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { TranslateService } from '@ngx-translate/core';

import { DatePipe } from '@angular/common';
import { ParentOptMgtComponent } from '../../sharedcomponent/parentoptmgt.component';
import { NotificationService } from '../../services/notification.service';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { OprServicesService } from '../opr-services.service';
import { UtilityService } from '../../services/utility.service';
import { ValidationService } from '../../sharedcomponent/ValidationService';
import { ApiService } from '../../services/api.service';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { DBOperation } from '../../services/utility.constants';
import { DeleteConfirmDialogComponent } from '../../sharedcomponent/delete-confirm-dialog';
import { AddupdateCustomerVistFormComponent } from './addupdate-customer-vist-form/addupdate-customer-vist-form.component';
import { ConfirmDialogWindowComponent } from '../confirm-dialog-window/confirm-dialog-window.component';


@Component({
  selector: 'app-customer-visit-forms',
  templateUrl: './customer-visit-forms.component.html'
})
export class CustomerVisitFormsComponent extends ParentOptMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['id','scheduleDateTime','customerCode', 'siteCode', 'projectCode', /*'reasonCode',*/'reasonCodeNameEng', 'reasonCodeNameAr','nameSupervisorId','status', 'Actions'];
  data: MatTableDataSource<any>;
  totalItemsCount: number;
  searchValue: string = '';
  sortingOrder: string = 'id asc';
  isLoading: boolean = false;
  form: FormGroup;
  isUpdating: boolean = false;


  listType: string = "";
  approvedStatus: string = "";

  project: any = {projectCode:"-NA-",siteCode:"-NA-"};
  isArabic: boolean = false;

  reasonCodeSelectionList: Array<any> = [];


  constructor(public datepipe: DatePipe, private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, private authService: AuthorizeService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService, private oprService: OprServicesService, private translate: TranslateService, public dialogRef: MatDialogRef<CustomerVisitFormsComponent> /*public dialogRef: MatDialogRef<CustomerVisitFormsComponent>*/) {
    super(authService);
  }

  ngOnInit(): void {
    this.isArabic = this.utilService.isArabic();
    this.initialLoading();
  }



  initialLoading() {
    this.LoadReasonCodeSelectionList();
    this.loadList(0, this.pageService.pageCount, "", this.sortingOrder, "", "");
  }

  LoadReasonCodeSelectionList() {
    this.apiService.getall('ReasonCode/getSelectReasonCodeList').subscribe(res => {
      if (res != null) {
        this.reasonCodeSelectionList = res as Array<any>;

        this.reasonCodeSelectionList.forEach(e => {
          e.text = this.isArabic ? e.textTwo + "-" + e.value : e.text + "-" + e.value;
        });

      }
    });

  }
  onSortOrder(sort: any) {
    this.totalItemsCount = 0;
    this.sortingOrder = sort.active + ' ' + sort.direction;
    this.loadList(0, this.pageService.pageCount, "", this.sortingOrder, this.approvedStatus, this.listType);
  }


  onPageSwitch(event: PageEvent) {
    this.pageService.change(event);
    this.loadList(event.pageIndex, event.pageSize, "", this.sortingOrder, this.approvedStatus, this.listType);
  }


  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined, approval: string = "", listType: string | null | undefined) {
    this.isLoading = true;
    approval = this.approvedStatus;
    listType = this.listType;
  
    this.apiService.getPagination(`CustomerVisitForm/getCustomerVisitFormsPagedListByProjectSite/${this.project.projectCode}/${this.project.siteCode}`, this.utilService.getOprQueryString(page, pageCount, query, orderBy, approval, '', 0, '', listType)).subscribe(result => {

    this.isLoading = false;
      this.totalItemsCount = 0;
      this.data = new MatTableDataSource(result.items);
     this.totalItemsCount = result.totalCount;
      setTimeout(() => {
        this.paginator.pageIndex = page as number;
        this.paginator.length = this.totalItemsCount;
        this.isLoading = false;

      });
  

      this.data.sort = this.sort;
    }, error => {



      this.utilService.ShowApiErrorMessage(error);
      this.isLoading = false;

    });
  }


  applyFilter(searchVal: any, listType: any) {
    const search = searchVal;//.target.value as string;
    if (search || listType) {
      this.searchValue = search;
      this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder, "", this.listType);
    }
  }



 

 

 
  


  

  translateToolTip(msg: string) {
    return `${this.translate.instant(msg)}`;

  }

 


 
 

 

  refresh() {
    this.searchValue = '';
    this.sortingOrder = 'id desc';

    this.approvedStatus = "";
    this.listType = "";
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder, this.approvedStatus, this.listType);

  }
  loadApprovals() {
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder, this.approvedStatus, this.listType);
  }

  closeModel() {
     this.dialogRef.close(true);
    //location.reload();
  }


  

  delete(row: any) {

    let requestType = row.requestType;
    let id = row.requestNumber;



    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete && id > 0) {


        this.apiService.delete('CustomerVisitFrom', id).subscribe(res => {
          this.utilService.OkMessage();
          this.ngOnInit();
        });




      }
    },
      error => this.utilService.ShowApiErrorMessage(error));
  }


  createUpdate(id: number, action: string) {
    let dialogRef = this.oprService.openAutoHeightWidthDialog(this.dialog, AddupdateCustomerVistFormComponent);
    (dialogRef.componentInstance as any).dbops = DBOperation.create;
    (dialogRef.componentInstance as any).modalTitle = "Customer_Visit_Form";
    (dialogRef.componentInstance as any).modalBtnTitle = "createUpdate";
    (dialogRef.componentInstance as any).id = id;
    (dialogRef.componentInstance as any).action = action;  //new,edit,confirm,visit
    if (this.project.projectCode != "-NA-") {
      (dialogRef.componentInstance as any).isFromProjectsAction = true;
    }
    (dialogRef.componentInstance as any).projectData = this.project;

    

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });

  }

  confirmVisit(row: any) {
    let dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, ConfirmDialogWindowComponent);
    dialogRef.afterClosed().subscribe(res => {
      if (res) {
        row.action = "confirm";
        row.modalTitle = "modalTitle";
        row.modalBtnTitle = "modalBtnTitle";
        this.apiService.post('CustomerVisitForm', row)
          .subscribe(res2 => {
            if (res2) {

              this.utilService.OkMessage();
              this.initialLoading();

            }
          },
            error => {
              console.error(error);
              this.utilService.ShowApiErrorMessage(error);
            });


      }
    });
    }
}
