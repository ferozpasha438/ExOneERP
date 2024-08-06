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
import { ParentOptMgtComponent } from '../../../../sharedcomponent/parentoptmgt.component';
import { UtilityService } from '../../../../services/utility.service';
import { NotificationService } from '../../../../services/notification.service';
import { PaginationService } from '../../../../sharedcomponent/pagination.service';
import { OprServicesService } from '../../../opr-services.service';
import { ApiService } from '../../../../services/api.service';
import { ValidationService } from '../../../../sharedcomponent/ValidationService';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { DBOperation } from '../../../../services/utility.constants';
import { AddupdateEmployeeprimarysitelogComponent } from './addupdate-employeeprimarysitelog/addupdate-employeeprimarysitelog.component';


@Component({
  selector: 'app-employeeprimarysitelogs',
  templateUrl: './employeeprimarysitelogs.component.html'
})
export class EmployeeprimarysitelogsComponent extends ParentOptMgtComponent implements OnInit {
  data:Array<any>=[];
  isLoading: boolean = false;
  form: FormGroup;
  employeeNumber: string;
  modalTitle: string;
  employeeName: string ="";
  employeeNameAr: string = "";
  
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, private authService: AuthorizeService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService, private oprService: OprServicesService, private translate: TranslateService, public dialogRef: MatDialogRef<EmployeeprimarysitelogsComponent>) {
    super(authService);
  }

  ngOnInit(): void {
    this.initialLoading();
  }

  refresh() {
    this.initialLoading();
  }

  initialLoading() {
    this.isLoading = true;
    this.apiService.getall(`Employee/getEmployeesPrimarySiteLogs/${this.employeeNumber}`).subscribe(result => {
      this.data=result as Array<any>
      this.isLoading = false;
    }, error => this.utilService.ShowApiErrorMessage(error));

    this.apiService.getall(`Employee/getEmployeeByEmployeeNumber/${this.employeeNumber}`).subscribe(result2 => {
      this.employeeName = result2.employeeName;
      this.employeeNameAr = result2.employeeName_AR;

    }, error => this.utilService.ShowApiErrorMessage(error));



  }




  
  


  
  add(row: any) {
    
  }

  edit(row: any) {
    if (!this.isLoading)
      this.openDialogManage(row.primarySiteLogId, DBOperation.update, row.primarySiteLogId > 0 ? 'Updating_Primary_Site_Log' : 'Adding_Primary_Site_Log', 'Update', AddupdateEmployeeprimarysitelogComponent);
  }

  closeModel() {
    this.dialogRef.close();
  }

  private openDialogManage(employeePrimarySitesLogID: number, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {
    let dialogRef = this.oprService.openFullWidthDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).employeePrimarySitesLogID = employeePrimarySitesLogID;
    (dialogRef.componentInstance as any).inputData = { employeeNumber: this.employeeNumber, employeeName: this.employeeName,employeeNameAr:this.employeeNameAr };

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.dialogRef.close(true);
    });
  }
}
