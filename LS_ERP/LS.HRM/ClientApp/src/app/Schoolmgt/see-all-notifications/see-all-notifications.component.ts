import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { DBOperation } from '../../services/utility.constants';
import { UtilityService } from '../../services/utility.service';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { ParentSchoolMgtComponent } from '../../sharedcomponent/parentschoolmgt.component';
import { ValidationService } from '../../sharedcomponent/ValidationService';
import { AddupdateSchoolReligionComponent } from '../shared/addupdate-school-religion/addupdate-school-religion.component';
import { AddupdateStudentRegistrationComponent } from '../shared/addupdate-student-registration/addupdate-student-registration.component';
import { AddupdateStudentMasterComponent } from '../shared/addupdate-student-master/addupdate-student-master.component';
import { StudentFeeMasterComponent } from '../shared/student-fee-master/student-fee-master.component';
import { StudentNoticeAndMessagingComponent } from '../shared/student-notice-and-messaging/student-notice-and-messaging.component';
import { StudentApplyLeaveComponent } from '../shared/student-apply-leave/student-apply-leave.component';
import { StudentAttendanceComponent } from '../shared/student-attendance/student-attendance.component';
import { StudentAcademicsComponent } from '../shared/student-academics/student-academics.component';
import { WebStudentRegistrationListComponent } from '../shared/web-student-registration-list/web-student-registration-list.component';
import { StudentAddressComponent } from '../shared/student-address/student-address.component';
import { StudentSiblingDataComponent } from '../shared/student-sibling-data/student-sibling-data.component';
import { AddUpdateStudentNotificationComponent } from '../shared/add-update-student-notification/add-update-student-notification.component';


@Component({
  selector: 'app-see-all-notifications',
  templateUrl: './see-all-notifications.component.html',
  styleUrls: []
})
export class SeeAllNotificationsComponent extends ParentSchoolMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort!: MatSort;
  data!: MatTableDataSource<any>;
  totalItemsCount!: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  id: number = 0;
  form!: FormGroup;
  isArab: boolean = false;
  notificationList: Array<any> = [];

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private translate: TranslateService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService) {
    super(authService);
  }
  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.initialLoading();
  }
  initialLoading() {
    this.apiService.getSchoolUrl('WebNotification/GetWebTopNotifications')
      .subscribe(res => {
        if (res) {
          this.notificationList = res;
        }
      });
  }
}
