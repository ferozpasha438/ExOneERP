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

import { DBOperation } from '../../../services/utility.constants';
import { TranslateService } from '@ngx-translate/core';

import { ConfirmDialogWindowComponent } from '../../confirm-dialog-window/confirm-dialog-window.component';
import { DeleteConfirmDialogComponent } from '../../../sharedcomponent/delete-confirm-dialog';
import { DatePipe } from '@angular/common';
import { CreateUpdateReqRemoveResourceComponent } from '../remove-resource/create-update-req-remove-resource/create-update-req-remove-resource.component';
import { PvEmpToResMapComponent } from '../add-resource/pv-emp-to-res-map/pv-emp-to-res-map.component';
import { CreateUpdateTransferResourceReqComponent } from '../transfer-employee/create-update-transfer-resource-req/create-update-transfer-resource-req.component';
import { CreateUpdateReqReplaceEmployeeComponent } from '../replace-employee/create-update-req-replace-employee/create-update-req-replace-employee.component';
import { CreateUpdatePvOpenCloseReqComponent } from '../pv-open-close-reqs/create-update-pv-open-close-req/create-update-pv-open-close-req.component';
import { CreateUpdateAddResourceComponent } from '../add-resource/create-update-add-resource/create-update-add-resource.component';
import { SwapEmployeesComponent } from '../swap-employees/swap-employees.component';
import { TransferWithReplacementComponent } from '../transfer-with-replacement/transfer-with-replacement.component';
import { CreateUpdateTransferWithReplacementComponent } from '../transfer-with-replacement/create-update-transfer-with-replacement/create-update-transfer-with-replacement.component';
import { UploadAdendumComponent } from '../../upload-adendum/upload-adendum.component';
import { AddendumFormComponent } from '../../project/project-sites/addendum-form/addendum-form.component';
import { CreateUpdateSwapEmployeesComponent } from '../swap-employees/create-update-swap-employees/create-update-swap-employees.component';

@Component({
  selector: 'app-all-requests',
  templateUrl: './all-requests.component.html'
})
export class AllRequestsComponent extends ParentOptMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['customerCode', 'siteCode', 'projectCode', 'requestType','requestNumber', 'requestedBy', 'requestedDate','effectiveDate', 'isApproved', /*'isMerged',*/ 'Actions'];
  data: MatTableDataSource<any>;
  totalItemsCount: number=0;
  searchValue: string = '';
  sortingOrder: string = 'requestedDate asc';
  isLoading: boolean = false;
  form: FormGroup;
  isUpdating: boolean = false;


  listType: string = "";
  approvedStatus: string = "";
  constructor(public datepipe: DatePipe, private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, private authService: AuthorizeService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService, private oprService: OprServicesService, private translate: TranslateService) {
    super(authService);
  }

  ngOnInit(): void {
    this.initialLoading();
  }



  initialLoading() {
    this.loadList(0, this.pageService.pageCount,this.searchValue, this.sortingOrder, "", "");
  }

  onSortOrder(sort: any) {
    this.totalItemsCount = 0;
    this.sortingOrder = sort.active + ' ' + sort.direction;
    this.loadList(0, this.pageService.pageCount,this.searchValue, this.sortingOrder, this.approvedStatus, this.listType);
  }


  onPageSwitch(event: PageEvent) {
    this.pageService.change(event);
    this.loadList(event.pageIndex, event.pageSize,this.searchValue, this.sortingOrder, this.approvedStatus, this.listType);
  }


  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined, approval: string = "", listType: string | null | undefined) {
    this.isLoading = true;
    approval = this.approvedStatus;
    listType = this.listType;
    this.apiService.getPagination(`PvAllRequests/getPvAllRequestsPagedList`, this.utilService.getOprQueryString(page, pageCount, query, orderBy, approval, '', 0, '', listType)).subscribe(result => {
      this.isLoading = false;
      this.totalItemsCount = 0;
      this.data = new MatTableDataSource(result.items);
      this.totalItemsCount = result.totalCount;
      setTimeout(() => {
        this.paginator.pageIndex = page as number;
        this.paginator.length = this.totalItemsCount;
        this.isLoading = false;

      });
      //this.data.paginator = this.paginator;

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
      this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder, this.approvedStatus, this.listType);
    }
  }
 


  private openConfirmationDialogManage(id: number,requestType:string, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any, confirmType: string) {
    let dialogRef = this.oprService.confirmationDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;
    (dialogRef.componentInstance as any).confirmType = "general";

    dialogRef.afterClosed().subscribe(res => {

      if (res) {

        if (requestType == "SwapEmployees") { 

          this.apiService.get('PvSwapEmployees/ApproveReqPvSwapEmployeesReqById', id).subscribe(res2 => {
          this.isUpdating = false;
          if (res2) {

            this.utilService.OkMessage();
            this.ngOnInit();


          }

        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
            this.isUpdating = false;

          });
        }

        else  if (requestType == "RemoveResource") { 

        this.apiService.get('PvRemoveResource/ApproveReqPvRemoveResourceReqById', id).subscribe(res2 => {
          this.isUpdating = false;
          if (res2) {

            this.utilService.OkMessage();
            this.ngOnInit();


          }

        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
            this.isUpdating = false;

          });
        }

        else
          if (requestType == "AddResource") {

            this.apiService.get('PvAddResource/ApproveReqPvAddResourceReqById', id).subscribe(res2 => {
              this.isUpdating = false;
              if (res2) {

                this.utilService.OkMessage();
                this.ngOnInit();


              }

            },
              error => {
                console.error(error);
                this.utilService.ShowApiErrorMessage(error);
                this.isUpdating = false;

              });
          }
          else
            if (requestType == "TransferResource") {

              this.apiService.get('PvTransferResource/ApproveReqPvTransferResourceReqById', id).subscribe(res2 => {
                this.isUpdating = false;
                if (res2) {

                  this.utilService.OkMessage();
                  this.ngOnInit();


                }

              },
                error => {
                  console.error(error);
                  this.utilService.ShowApiErrorMessage(error);
                  this.isUpdating = false;

                });
            }
            else
              if (requestType == "TransferWithReplacement") {

                this.apiService.get('PvTransferWithReplacement/ApproveReqPvTransferWithReplacementReqById', id).subscribe(res2 => {
                this.isUpdating = false;
                if (res2) {

                  this.utilService.OkMessage();
                  this.ngOnInit();


                }

              },
                error => {
                  console.error(error);
                  this.utilService.ShowApiErrorMessage(error);
                  this.isUpdating = false;

                });
            }
            else
              if (requestType == "ReplaceResource") {

                this.apiService.get('PvReplaceResource/ApproveReqPvReplaceResourceReqById', id).subscribe(res2 => {
                  this.isUpdating = false;
                  if (res2) {

                    this.utilService.OkMessage();
                    this.ngOnInit();


                  }

                },
                  error => {
                    console.error(error);
                    this.utilService.ShowApiErrorMessage(error);
                    this.isUpdating = false;

                  });
              }
              else
                if (requestType == "ProjectVariations") {

                  this.apiService.get('PvOpenCloseReq/ApproveReqPvOpenCloseReqById', id).subscribe(res2 => {
                    this.isUpdating = false;
                    if (res2) {

                      this.utilService.OkMessage();
                      this.ngOnInit();


                    }

                  },
                    error => {
                      console.error(error);
                      this.utilService.ShowApiErrorMessage(error);
                      this.isUpdating = false;

                    });
                }
                else {
                  this.isUpdating = false;
                }
      }
      else {
        this.isUpdating = false;
      }

    });


  }


  private openDialogManage(id: number, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {
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

  private openDialogManage2(requestData: any, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).requestData = requestData;
    (dialogRef.componentInstance as any).id = requestData.id;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }

  public approveRequest(row:any) {
    if (!this.isUpdating) {
      this.isUpdating = true;
      this.openConfirmationDialogManage(row?.requestNumber,row?.requestType, DBOperation.update, 'Conforming_Approve_Request', 'Approve', ConfirmDialogWindowComponent, "general");
    }
    else {
      this.notifyService.showError(this.translate.instant("Please Wait..."));
    }
   
  }



  edit(row: any) {
 
    let requestType = row.requestType;
    let id = row.requestNumber;

      if (requestType == "SwapEmployees") {

        this.openDialogManage(id, DBOperation.update, 'Updating_Request_For_Swap_Employees', 'Update', SwapEmployeesComponent);
      }

      else if (requestType == "RemoveResource") {

        this.openDialogManage(id, DBOperation.update, 'Updating_Request_For_Remove_Resource', 'Update', CreateUpdateReqRemoveResourceComponent);
      }

      else
        if (requestType == "AddResource") {

          this.openDialogManage(id, DBOperation.update, 'Updating_Request_For_Adding_Resources', 'Update', CreateUpdateAddResourceComponent);


        }
        else
          if (requestType == "TransferResource") {
            this.openDialogManage(id, DBOperation.update, 'Updating_Request_For_Transfer_Resource', 'Update', CreateUpdateTransferResourceReqComponent);
          }

          else
            if (requestType == "ReplaceResource") {
              this.openDialogManage(id, DBOperation.update, 'Updating_Request_For_Replace_Employee', 'Update', CreateUpdateReqReplaceEmployeeComponent);
            }
            else
              if (requestType == "TransferWithReplacement") {
              this.openDialogManage(id, DBOperation.update, 'Updating_Request_For_Transfer_With_Replacement', 'Update', CreateUpdateReqReplaceEmployeeComponent);
            }
            else
              if (requestType == "ProjectVariations") {



                this.apiService.get('PvOpenCloseReq/getPvOpenCloseReqById', id).subscribe(res => {

                  this.openDialogManage2(res, DBOperation.update, 'Updating_Request_For_Open_Close', 'Update', CreateUpdatePvOpenCloseReqComponent);


                });

              }
    }




  

  delete(row: any) {

    let requestType = row.requestType;
    let id = row.requestNumber;
    


    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete && id > 0) {
        if (requestType == "SwapEmployees") {

          this.apiService.delete('PvSwapEmployees', id).subscribe(res => {
            this.utilService.OkMessage();
            this.ngOnInit();
          });
        }

        else
 if (requestType == "RemoveResource") {

          this.apiService.delete('PvRemoveResource', id).subscribe(res => {
            this.utilService.OkMessage();
            this.ngOnInit();
          });
        }

        else
          if (requestType == "AddResource") {

            this.apiService.delete('PvAddResource', id).subscribe(res => {
              this.utilService.OkMessage();
              this.ngOnInit();
            });

          }
          else
            if (requestType == "TransferResource") {
              this.apiService.delete('PvTransferResource', id).subscribe(res => {
                this.utilService.OkMessage();
                this.ngOnInit();
              });
            }

            else
              if (requestType == "ReplaceResource") {
                this.apiService.delete('PvReplaceResource', id).subscribe(res => {
                  this.utilService.OkMessage();
                  this.ngOnInit();
                });

              }
              else
                if (requestType == "TransferWithReplacement") {
                  this.apiService.delete('PvTransferWithReplacement', id).subscribe(res => {
                  this.utilService.OkMessage();
                  this.ngOnInit();
                });

              }
              else
                if (requestType == "ProjectVariations") {
                  this.apiService.delete('PvOpenCloseReq', id).subscribe(res => {
                    this.utilService.OkMessage();
                    this.ngOnInit();
                  });
                }
      }
    },
      error => this.utilService.ShowApiErrorMessage(error));
        }
  

  viewRequest(row: any) {
    let requestType = row.requestType;

    if (requestType == "SwapEmployees") {

      this.openDialogManage(row.requestNumber, DBOperation.create, 'Request_For_Swap_Employees', 'View', CreateUpdateSwapEmployeesComponent);
    }

    else
if (requestType == "RemoveResource") {

      this.openDialogManage(row.requestNumber, DBOperation.create, 'Request_For_Remove_Resource', 'View', CreateUpdateReqRemoveResourceComponent);
    }

    else
      if (requestType == "AddResource") {

        this.openDialogManage(row.requestNumber, DBOperation.create, 'Request_For_Add_Resource', 'View', CreateUpdateAddResourceComponent /*ViewPvAddResRequestComponent*/);

      }
      else
        if (requestType == "TransferResource") {

              
          this.openDialogManage(row.requestNumber, DBOperation.create, 'Request_For_Transfer_Employee', 'View', CreateUpdateTransferResourceReqComponent);



            }

        else
          if (requestType == "ReplaceResource") {
            this.openDialogManage(row.requestNumber, DBOperation.create, 'Request_For_Replace_Resource', 'View', CreateUpdateReqReplaceEmployeeComponent);

          }
          else
            if (requestType == "ProjectVariations") {



              this.apiService.get('PvOpenCloseReq/getPvOpenCloseReqById', row.requestNumber).subscribe(res => {

                this.openDialogManage2(res, DBOperation.create, 'Request_For_Open_Close_Project_Site', 'View', CreateUpdatePvOpenCloseReqComponent);


              });

            }
            else if (requestType == "TransferWithReplacement") {

                this.openDialogManage(row.requestNumber, DBOperation.create, 'Request_For_Transfer_With_Replacement', 'View', CreateUpdateTransferWithReplacementComponent);
            }
  }


  translateToolTip(msg: string) {
    return `${this.translate.instant(msg)}`;

  }

  mapEmpToRes(row: any) {
    this.openDialogManage(row.requestNumber, DBOperation.create, 'Employee_To_Resource_Mapping', 'Add', PvEmpToResMapComponent);
  }


  public viewMappings(id: number) {
    this.openDialogManage(id, DBOperation.create, 'Employee_Mapping_For_Add_Resources', 'View', PvEmpToResMapComponent /*ViewPvAddResRequestComponent*/);
  }

  public mergeRequest(id: number) {

      this.openConfirmationDialogManage2(id, DBOperation.update, 'Confirming_Merge_Request', 'Merge', ConfirmDialogWindowComponent, "general");

  }

  private openConfirmationDialogManage2(id: number, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any, confirmType: string) {
    let dialogRef = this.oprService.confirmationDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;
    (dialogRef.componentInstance as any).confirmType = "general";

    dialogRef.afterClosed().subscribe(res => {
      if (res) {
       
         if (modalTitle == 'Confirming_Merge_Request')

          this.apiService.get('PvAddResource/mergePvAddResourceReqById', id).subscribe(res2 => {
            if (res2) {

              this.utilService.OkMessage();
              this.ngOnInit();


            }

          },
            error => {
              console.error(error);
              this.utilService.ShowApiErrorMessage(error);
            });
      }
      else {
        this.notifyService.showError(this.translate.instant("Something_Went_Wrong"));
      }
    });
  }

  refresh() {
    this.searchValue = '';
    this.sortingOrder = 'requestedDate desc';

    this.approvedStatus = "";
    this.listType = "";
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder, this.approvedStatus, this.listType);

  }
  loadApprovals() {
    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder, this.approvedStatus, this.listType);
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

  public viewAddendumForm(row: any) {

    let dialogRef = this.utilService.openCrudDialog(this.dialog, AddendumFormComponent);
    (dialogRef.componentInstance as any).pvRequest = row;
    (dialogRef.componentInstance as any).type = row.requestType == "AddResource" ? "ForAddingResources" :row.requestType=="RemoveResource"?"ForRemovingResources":"";

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });

  }
}
