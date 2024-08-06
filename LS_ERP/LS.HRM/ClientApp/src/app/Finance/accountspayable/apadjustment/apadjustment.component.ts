import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { ParentSystemSetupComponent } from '../../../sharedcomponent/parentsystemsetup.component';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TranslateService } from '@ngx-translate/core';
import { PaginationService } from '../../../sharedcomponent/pagination.service';
import { MatDialog } from '@angular/material/dialog';
import { DBOperation } from '../../../services/utility.constants';
import { MultiFileUploadDto } from '../../../models/sharedDto';
import { DeleteConfirmDialogComponent } from '../../../sharedcomponent/delete-confirm-dialog';
import { AddupdatevendorpaymentComponent } from '../sharedpages/addupdatevendorpayment/addupdatevendorpayment.component';
import { VendorstatementComponent } from '../sharedpages/vendorstatement/vendorstatement.component';
import { Vendorpaymentvoucher } from '../../sharedpages/vouchers/vendorpaymentvoucher.component';
import { ParentFinMgtComponent } from '../../../sharedcomponent/parentfinmgt.component';

@Component({
  selector: 'app-apadjustment',
  templateUrl: './apadjustment.component.html',
  styles: [
  ]
})
export class ApadjustmentComponent extends ParentFinMgtComponent implements OnInit {

  constructor(private fb: FormBuilder, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public pageService: PaginationService, public dialog: MatDialog) {
    super(authService);
  }

  ngOnInit(): void {
  }

}
