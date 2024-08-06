import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { PaginationService } from '../../../sharedcomponent/pagination.service';
import { ParentFinMgtComponent } from '../../../sharedcomponent/parentfinmgt.component';
import { ValidationService } from '../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-aradjustment',
  templateUrl: './aradjustment.component.html',
  styles: [
  ]
})
export class AradjustmentComponent extends ParentFinMgtComponent implements OnInit {

  constructor(private fb: FormBuilder, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public pageService: PaginationService, public dialog: MatDialog) {
    super(authService);
  }

  ngOnInit(): void {
  }

}
