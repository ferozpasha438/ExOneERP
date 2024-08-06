import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { DBOperation } from '../../services/utility.constants';
import { UtilityService } from '../../services/utility.service';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { ParentSystemSetupComponent } from '../../sharedcomponent/parentsystemsetup.component';
import { AddupdatecustcategoryComponent } from '../sharedpages/addupdatecustcategory/addupdatecustcategory.component';
@Component({
  selector: 'app-customercategory',
  templateUrl: './customercategory.component.html',
  styleUrls: [],
})
export class CustomercategoryComponent extends ParentSystemSetupComponent implements OnInit {

  displayedColumns: string[] = ['custCatCode', 'custCatName', 'custCatDesc', 'catPrefix', 'isActive', 'Actions'];
  data: MatTableDataSource<any>;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  totalItemsCount: number;
  isLoading: boolean;
  sortingOrder: string = '';
  canAddCustCateg: boolean = true;
  constructor(private apiService: ApiService, private authService: AuthorizeService,
    private utilService: UtilityService, private notifyService: NotificationService, public dialog: MatDialog,
    public pageService: PaginationService) { super(authService); }

  ngOnInit(): void {
    this.initialLoading();
  }

  initialLoading() {
    this.loadList(0, this.pageService.pageCount, "", this.sortingOrder);
  }

  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
    this.isLoading = true;

    this.apiService.getPagination('customerCategorySetup', '').subscribe(result => {
      this.totalItemsCount = 0;
      this.data = new MatTableDataSource(result);
      this.data.paginator = this.paginator;
      this.canAddCustCateg = result.length <= 0
      this.isLoading = false;
    }, error => this.utilService.ShowApiErrorMessage(error));
  }

  searchFilter(value: string) {
    this.data.filter = value.trim().toLowerCase();
  }

  private openDialogManage(row: any = null) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, AddupdatecustcategoryComponent);
    (dialogRef.componentInstance as any).row = row;
    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }

  public create() {
    this.openDialogManage();
  }

  public edit(row: any) {
    this.openDialogManage(row);
  }

}

