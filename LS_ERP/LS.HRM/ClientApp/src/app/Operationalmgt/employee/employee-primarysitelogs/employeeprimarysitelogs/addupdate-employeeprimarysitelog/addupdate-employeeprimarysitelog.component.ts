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
import { ParentOptMgtComponent } from '../../../../../sharedcomponent/parentoptmgt.component';
import { NotificationService } from '../../../../../services/notification.service';
import { PaginationService } from '../../../../../sharedcomponent/pagination.service';
import { UtilityService } from '../../../../../services/utility.service';
import { OprServicesService } from '../../../../opr-services.service';
import { ApiService } from '../../../../../services/api.service';
import { ValidationService } from '../../../../../sharedcomponent/ValidationService';
import { AuthorizeService } from '../../../../../api-authorization/AuthorizeService';
import { DBOperation } from '../../../../../services/utility.constants';

@Component({
  selector: 'app-addupdate-employeeprimarysitelog',
  templateUrl: './addupdate-employeeprimarysitelog.component.html'
})
export class AddupdateEmployeeprimarysitelogComponent extends ParentOptMgtComponent implements OnInit {
  data: Array<any> = [];
  isLoading: boolean = false;
  form: FormGroup;
  employeePrimarySitesLogID: number = 0;
  inputData: any;
  modalTitle: string;
  interval: any;
  siteSelectionList: Array<any> = [];
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, private authService: AuthorizeService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService, private oprService: OprServicesService, private translate: TranslateService, public dialogRef: MatDialogRef<AddupdateEmployeeprimarysitelogComponent>) {
    super(authService);
  }

  ngOnInit(): void {
    this.setForm();

    var count = 0;
    this.interval = setInterval(() => {
      count++;
      if (count == 1)
        this.loadSiteSelectionList();

      if (count == 2) {
        this.initialLoading();
      }
    }, count > 2 ? 0 : 1000);
  
  }

  refresh() {
    this.initialLoading();
  }

  initialLoading() {
    if (this.employeePrimarySitesLogID > 0) {

      this.setEditForm();
    }
    
  }

  setForm() {
    this.form = this.fb.group({
      //'': ['', Validators.required],
      'employeeNumber': [this.employeePrimarySitesLogID > 0 ? '' : this.inputData.employeeNumber, Validators.required],
      'siteCode': ['', Validators.required],
      'transferredDate': ['', Validators.required]
    });
  }

  setEditForm() {
    this.apiService.get('Employee/getPrimaryLogById', this.employeePrimarySitesLogID).subscribe(res => {
      if (res) {

        this.form.patchValue(res);

     
        }
    });
  }


  openDatePicker(dp: any) {
    dp.open();
  }
  loadSiteSelectionList() {
    this.apiService.getall(`ProjectSites/getSelectProjectSitesList2`).subscribe((res:Array<any> )=> {
      this.siteSelectionList = res;
    });

  }


  submit() {
    if (this.form.valid) {


      let ed = new Date(this.form.controls['transferredDate'].value);
      ed.setMinutes(ed.getMinutes() - ed.getTimezoneOffset());
      this.form.value['transferredDate'] = ed;


      this.form.value['employeePrimarySitesLogID'] = this.employeePrimarySitesLogID;
      this.apiService.post('employee/addUpdatePrimarySiteLog', this.form.value).subscribe(res => {
        this.utilService.OkMessage();
        this.dialogRef.close(true);
      }, error => {
        this.utilService.FillUpFields();
      });
    }
    else {

      console.log(this.form);
    }
     }

  cancel() {
    this.dialogRef.close();
  }

  closeModel() {
    this.dialogRef.close();
  }

}
