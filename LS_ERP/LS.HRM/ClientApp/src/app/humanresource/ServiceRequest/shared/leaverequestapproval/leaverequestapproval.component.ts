import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { DBOperation } from '../../../../services/utility.constants';
import { UtilityService } from '../../../../services/utility.service';
import { DeleteConfirmDialogComponent } from '../../../../sharedcomponent/delete-confirm-dialog';
import { PaginationService } from '../../../../sharedcomponent/pagination.service';
import { ParentHrmAdminComponent } from '../../../../sharedcomponent/ParentHrmAdmin.component';

@Component({
  selector: 'app-leaverequestapproval',
  templateUrl: './leaverequestapproval.component.html',
  styles: [
  ]
})
export class LeaverequestapprovalComponent implements OnInit {
  approvalList: Array<any> = [];
  @Input() empInfo: any;
  @Input() id: any;
  @Input() serviceRequestType: any;
  constructor(private apiService: ApiService, private authService: AuthorizeService, private translate: TranslateService,
    private utilService: UtilityService, private notifyService: NotificationService, public dialog: MatDialog,
    public pageService: PaginationService) {

  }

  ngOnInit(): void {
    this.apiService.getQueryString(`serviceRequest/getRequestApprovalSelectListItem`, `?id=${this.id}&employeeId=${this.empInfo.intValue}&serviceRequestType=${this.serviceRequestType}`).subscribe(res => {
      this.approvalList = res;
    });
  }

}
