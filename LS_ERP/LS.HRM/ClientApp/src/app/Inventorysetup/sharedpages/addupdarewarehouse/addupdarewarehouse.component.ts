import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
//import { MatDialog } from '@angular/material/dialog';
//import { MatPaginator, PageEvent } from '@angular/material/paginator';
//import { MatSort } from '@angular/material/sort';
//import { MatTableDataSource } from '@angular/material/table';
import { MatDialogRef, MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService'
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
//import { BrowserModule } from '@angular/platform-browser';
//import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
//import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
//import { SharedModule } from '../../../sharedcomponent/shared.module';
import { ParentSystemSetupComponent } from '../../../sharedcomponent/parentsystemsetup.component';
@Component({
  selector: 'app-addupdarewarehouse',
  templateUrl: './addupdarewarehouse.component.html',
  styles: [
  ]
})
export class AddupdarewarehouseComponent extends ParentSystemSetupComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
  branchCodeList: Array<CustomSelectListItem> = [];
  cityList: Array<CustomSelectListItem> = [];
  DstgroupList: Array<CustomSelectListItem> = [];
  isReadOnly: boolean = false;


  constructor(private fb: FormBuilder, private apiService: ApiService, private authService: AuthorizeService, private utilService: UtilityService,
    private validationService: ValidationService, public dialog: MatDialog,private notifyService: NotificationService, public dialogRef: MatDialogRef<AddupdarewarehouseComponent>,
  ) { super(authService);}

  ngOnInit(): void {
    this.loadBranches();
    this.loadCities();
    this.loadDistGroup();
    this.setForm();
    
    if (this.id > 0)
      this.setEditForm();
  }

  loadBranches() {
    this.apiService.getall('Warehouse/getSelectSysBranchList').subscribe(res => {
      if (res) {
        this.branchCodeList = res;
      }
    })
  }
  loadCities() {
    this.apiService.getall('city/getSelectList').subscribe(res => {
      if (res) {
        this.cityList = res;
      }
    })
  }
  loadDistGroup() {
    this.apiService.getall('Warehouse/getSelectDistributionGroupList').subscribe(res => {
      if (res) {
        this.DstgroupList = res;
      }
    })
  }

  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      'whCode': ['', Validators.required],
      'whName': ['', Validators.required],
      'whDesc': '',
      'invDistGroup': ['', Validators.required],
      'whAddress': ['', Validators.required],
      'whCity': ['', Validators.required],
      'whState': '',
      'whIncharge': ['', Validators.required],
      'whBranchCode': ['', Validators.required],
      'whAllowDirectPur': [true],
 //     'isActive': [],

    })
  }

  setEditForm() {
    this.apiService.get('warehouse', this.id).subscribe(res => {
      if (res) {
        this.isReadOnly = true;
        this.form.patchValue(res);
      }
    })
  }


  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;

      this.apiService.post('warehouse', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.reset();
          this.dialogRef.close(true);
        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
          });
    }
    else
      this.utilService.FillUpFields();
  }

  reset() {
    this.form.controls['whCode'].setValue('');
    this.form.controls['whName'].setValue('');
    this.form.controls['whDesc'].setValue('');
    this.form.controls['invDistGroup'].setValue('');
    this.form.controls['whAddress'].setValue('');
    this.form.controls['whCity'].setValue('');
    this.form.controls['whState'].setValue('');
    this.form.controls['whIncharge'].setValue('');
    this.form.controls['whBranchCode'].setValue('');
    this.form.controls['whAllowDirectPur'].setValue('');
  /*  this.form.controls['isActive'].setValue('');*/
  }

  closeModel() {
    this.dialogRef.close();
  }

  onTextchange(Value: string) {
    if (Value != null) {
      this.apiService.getall(`warehouse/GetWarehouseItems?whcode=${Value}`).subscribe(res => {
        if (res) {
          this.isReadOnly = true;
          this.form.patchValue(res);
          this.id = res.id;
        }
       
      })
    }
  }

 
}
