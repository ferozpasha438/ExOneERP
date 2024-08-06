import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';
import { NotificationService } from '../../../services/notification.service';
import { PaginationService } from '../../../sharedcomponent/pagination.service';
import { OprServicesService } from '../../opr-services.service';
import { UtilityService } from '../../../services/utility.service';
import { ApiService } from '../../../services/api.service';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { CreateUpdateAddResourceComponent } from './create-update-add-resource/create-update-add-resource.component';
import { DBOperation } from '../../../services/utility.constants';
import { TranslateService } from '@ngx-translate/core';
import { ViewPvAddResRequestComponent } from './view-pv-add-res-request/view-pv-add-res-request.component';
import { ConfirmDialogWindowComponent } from '../../confirm-dialog-window/confirm-dialog-window.component';
import { DeleteConfirmDialogComponent } from '../../../sharedcomponent/delete-confirm-dialog';
import { PvEmpToResMapComponent } from './pv-emp-to-res-map/pv-emp-to-res-map.component';
import { UploadAdendumComponent } from '../../upload-adendum/upload-adendum.component';
import { AddendumFormComponent } from '../../project/project-sites/addendum-form/addendum-form.component';

@Component({
  selector: 'app-add-resource',
  templateUrl: './add-resource.component.html'
})
export class AddResourceComponent
  extends ParentOptMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['customerCode', 'siteCode', 'projectCode', 'isApproved', 'isEmpMapped','isMerged', 'Actions'];
  data: MatTableDataSource<any>;
  totalItemsCount: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  form: FormGroup;

  isUdpating: boolean = false;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, private authService: AuthorizeService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService, private oprService: OprServicesService, private translate: TranslateService) {
    super(authService);
  }

  ngOnInit(): void {
    this.initialLoading();
  }

  refresh() {
    this.searchValue = '';
    this.initialLoading();
  }

  initialLoading() {
    this.loadList(0, this.pageService.pageCount,this.searchValue, this.sortingOrder);
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
    this.apiService.getPagination('PvAddResource/getPvAddResourceReqsPagedList', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
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
  private openDialogManage(id: number, dbops: DBOperation, modalTitle: string, modalBtnTitle: string,Component:any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }

  private openConfirmationDialogManage(id: number, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any, confirmType: string) {
    let dialogRef = this.oprService.confirmationDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;
    (dialogRef.componentInstance as any).confirmType = "general";

    dialogRef.afterClosed().subscribe(res => {
      if (res) {
        if (modalTitle =='Confirming_Approve_Request') {
          this.apiService.get('PvAddResource/ApproveReqPvAddResourceReqById', id).subscribe(res2 => {
            this.isUdpating = false;
            if (res2) {

              this.utilService.OkMessage();
              this.ngOnInit();


            }

          },
            error => {
              this.isUdpating = false;
              console.error(error);
              this.utilService.ShowApiErrorMessage(error);
            });
        }
        else if (modalTitle =='Confirming_Merge_Request')
       
          this.apiService.get('PvAddResource/mergePvAddResourceReqById', id).subscribe(res2 => {
          this.isUdpating = false;
          if (res2) {

            this.utilService.OkMessage();
            this.ngOnInit();


          }

        },
          error => {
            this.isUdpating = false;
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
          });
      }
      else {
        this.notifyService.showError(this.translate.instant("Something_Went_Wrong"));
        this.isUdpating = false;
      }
    });
  }









  public create() {
    this.openDialogManage(0, DBOperation.create, 'New_Request_For_Adding_Resources', 'Add', CreateUpdateAddResourceComponent);
  }

  public edit(id: number) {
    this.openDialogManage(id, DBOperation.update, 'Updating_Request_For_Adding_Resources', 'Update', CreateUpdateAddResourceComponent);
  }
  public delete(id: number) {
    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete && id > 0) {
        this.apiService.delete('PvAddResource', id).subscribe(res => {
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

  
  translateToolTip(msg: string) {
    //console.log(msg);
    //let str = this.translate.instant(msg);
    return `${this.translate.instant(msg)}`;

  }

  public viewRequest(id: number) {
    this.openDialogManage(id, DBOperation.create, 'Request_For_Add_Resource', 'View', CreateUpdateAddResourceComponent /*ViewPvAddResRequestComponent*/);
   

  }
  public viewAddendumForm(row:any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, AddendumFormComponent);
    (dialogRef.componentInstance as any).modalTitle = this.translate.instant("Addendum_For_Adding_Resources");
    (dialogRef.componentInstance as any).pvRequest = row;
    (dialogRef.componentInstance as any).type = "ForAddingResources";

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });

  }
  public viewMappings(id: number) {
    this.openDialogManage(id, DBOperation.create, 'Employee_Mapping_For_Add_Resources', 'View', PvEmpToResMapComponent /*ViewPvAddResRequestComponent*/);
  }

  public approveRequest(id: number) {
    if (!this.isUdpating) {
      this.isUdpating = true;
      this.openConfirmationDialogManage(id, DBOperation.update, 'Confirming_Approve_Request', 'Approve', ConfirmDialogWindowComponent, "general");

    }
    else {
      this.notifyService.showError(this.translate.instant("Please Wait..."));
    }


  }
  public mergeRequest(id: number) {
    if (!this.isUdpating) {
      this.isUdpating = true;
      this.openConfirmationDialogManage(id, DBOperation.update, 'Confirming_Merge_Request', 'Merge', ConfirmDialogWindowComponent, "general");

    }
    else {
      this.notifyService.showError(this.translate.instant("Please Wait..."));
    }


  }
  public mapEmpToRes(id: number) {
    this.openDialogManage(id, DBOperation.create, 'Employee_To_Resource_Mapping', 'Add', PvEmpToResMapComponent);
    
  }

  viewAddendum(row: any) {
    window.open(row.fileUrl);
  }

  uploadAdendum(row: any) {
    let dialogRef = this.oprService.openAutoWidthDialog(this.dialog, UploadAdendumComponent);
    (dialogRef.componentInstance as any).requestData = row;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
}
