import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CustomSelectListItem } from '../../models/MenuItemListDto';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';
import { ValidationService } from '../../sharedcomponent/ValidationService';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { ParentSystemSetupComponent } from '../../sharedcomponent/parentsystemsetup.component';
@Component({
  selector: 'app-inventoryconfiguration',
  templateUrl: './inventoryconfiguration.component.html',
})
export class InventoryconfigurationComponent extends ParentSystemSetupComponent implements OnInit {

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  form: FormGroup;
  warehouseList: Array<CustomSelectListItem> = [];
  data: MatTableDataSource<any> = new MatTableDataSource();
  totalItemsCount: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  isshown: boolean = false;
  invtConfig: any;
  canEditConfig: boolean = false;
  constructor(private fb: FormBuilder, private apiService: ApiService, private utilService: UtilityService,
    private validationService: ValidationService, private notifyService: NotificationService, private authService: AuthorizeService, public dialog: MatDialog, public pageService: PaginationService
  ) {
    super(authService);
  }


  ngOnInit(): void {
    this.setForm();
    this.loadWarehouses();
  }

  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      // 'CentralWHCode': ['', Validators.required],
      'centralWHCode': ['', Validators.required],
      'newItemIndicator': ['', Validators.required],
      'itemLength': ['', Validators.required],
      'categoryLength': ['', Validators.required],
      'autoGenItemCode': [false],
      'prefixCatCode': [false],
    });
  }
  onWarehouse(Value: string) {
    if (Value != null) {
      this.apiService.getall(`warehouse/GetWarehouseDetails?Warehouse=${Value}`).subscribe(res => {
        if (res) {
          if (res[0].qtyOH > 0) {
            this.isshown = true;
          }
          else
            this.isshown = false;
          /*this.notifyService.showError('Duplicate Barcode...');*/

        }
      })
    }
  }
  loadWarehouses() {
    this.laodInvtConfig();
    this.apiService.getall(`warehouse/getSelectWarehouseList`).subscribe(res => {
      if (res) {
        this.warehouseList = res;
      }
    });

  }

  laodInvtConfig() {

    this.apiService.getall(`inventoryconfiguration/getInvtConfig`).subscribe(res => {
      if (res) {
        this.form.patchValue(res);
        this.canEditConfig = res['canEdit'];
      }
    });

  }

  submit() {
    //console.log(this.form.value)
    if (this.form.valid) {
      this.apiService.post('inventoryconfiguration', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.reset();
          this.laodInvtConfig();
        },
          error => {
            this.utilService.ShowApiErrorMessage(error);
          });
    }
    else
      this.utilService.FillUpFields();
  }

  reset() {
    this.form.controls['centralWHCode'].setValue('');
    this.form.controls['newItemIndicator'].setValue('');
    this.form.controls['itemLength'].setValue('');
    this.form.controls['categoryLength'].setValue('');

    this.form.controls['autoGenItemCode'].setValue(false);
    this.form.controls['prefixCatCode'].setValue(false);
  }
  //closeModel() {
  //  this.dialogRef.close();
  //}
}
