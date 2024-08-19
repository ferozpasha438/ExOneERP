import { __decorate } from "tslib";
import { Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
//import { ApiService } from '../../services/api.service';
import { DBOperation } from '../../services/utility.constants';
import { ParentSchoolMgtComponent } from '../../sharedcomponent/parentschoolmgt.component';
import { AddupdateNotificationComponent } from '../shared/addupdate-notification/addupdate-notification.component';
import { ApproveNotificationComponent } from '../shared/approve-notification/approve-notification.component';
let NotificationComponent = class NotificationComponent extends ParentSchoolMgtComponent {
    constructor(fb, http, router, apiService, authService, translate, notifyService, utilService, validationService, dialog, pageService) {
        super(authService);
        this.fb = fb;
        this.http = http;
        this.router = router;
        this.apiService = apiService;
        this.authService = authService;
        this.translate = translate;
        this.notifyService = notifyService;
        this.utilService = utilService;
        this.validationService = validationService;
        this.dialog = dialog;
        this.pageService = pageService;
        this.displayedColumns = ['acadamicYear', 'notificationTitle', 'branchCode', 'isApproved', 'Actions'];
        this.searchValue = '';
        this.sortingOrder = 'id desc';
        this.isLoading = false;
        this.id = 0;
        this.isArab = false;
    }
    //get():Array<any>{
    //  return [
    //    {id:1,idNum:'1234567',draftedDate:'23/8/2021',branchCode:'BR001',sendBy:'User'},     
    // ]
    // }
    ngOnInit() {
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
    onSortOrder(sort) {
        this.totalItemsCount = 0;
        this.sortingOrder = sort.active + ' ' + sort.direction;
        this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
    }
    onPageSwitch(event) {
        this.pageService.change(event);
        this.loadList(event.pageIndex, event.pageSize, this.searchValue, this.sortingOrder);
    }
    loadList(page, pageCount, query, orderBy) {
        this.isLoading = true;
        //this.data = new MatTableDataSource(this.get());
        this.apiService.getPagination('WebNotification', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
            //this.totalItemsCount = 0;
            this.data = new MatTableDataSource(result.items);
            this.totalItemsCount = result.totalCount;
            //this.data.paginator = this.paginator;
            this.data.sort = this.sort;
            this.isLoading = false;
        }, error => this.utilService.ShowApiErrorMessage(error));
    }
    applyFilter(searchVal) {
        const search = searchVal; //.target.value as string;
        //if (search && search.length >= 3) {
        if (search) {
            this.searchValue = search;
            this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
        }
    }
    openDialogManage(id = 0, dbops, modalTitle, modalBtnTitle) {
        let dialogRef = this.utilService.openCrudDialog(this.dialog, AddupdateNotificationComponent);
        dialogRef.componentInstance.dbops = dbops;
        dialogRef.componentInstance.modalTitle = modalTitle;
        dialogRef.componentInstance.modalBtnTitle = modalBtnTitle;
        dialogRef.componentInstance.id = id;
        dialogRef.afterClosed().subscribe(res => {
            if (res && res === true)
                this.initialLoading();
        });
    }
    create() {
        this.openDialogManage(0, DBOperation.create, 'Adding_New_Customer', 'Add');
    }
    approvedDialog(id = 0, dbops, modalTitle, modalBtnTitle) {
        let dialogRef = this.utilService.openCrudDialog(this.dialog, ApproveNotificationComponent);
        dialogRef.componentInstance.dbops = dbops;
        dialogRef.componentInstance.modalTitle = modalTitle;
        dialogRef.componentInstance.modalBtnTitle = modalBtnTitle;
        dialogRef.componentInstance.id = id;
        dialogRef.afterClosed().subscribe(res => {
            if (res && res === true)
                this.initialLoading();
        });
    }
    approved() {
        this.approvedDialog(0, DBOperation.create, 'Approve', 'Approve');
    }
    closeModel() {
    }
    editDialogManage(row) {
        let dialogRef = this.utilService.openCrudDialog(this.dialog, AddupdateNotificationComponent);
        dialogRef.componentInstance.row = row;
        dialogRef.afterClosed().subscribe(res => {
            if (res && res === true)
                this.initialLoading();
        });
    }
    edit(row) {
        this.editDialogManage(row);
    }
};
__decorate([
    ViewChild(MatPaginator, { static: true })
], NotificationComponent.prototype, "paginator", void 0);
__decorate([
    ViewChild(MatSort, { static: true })
], NotificationComponent.prototype, "sort", void 0);
NotificationComponent = __decorate([
    Component({
        selector: 'app-notification',
        templateUrl: './notification.component.html',
        styleUrls: []
    })
], NotificationComponent);
export { NotificationComponent };
//# sourceMappingURL=notification.component.js.map