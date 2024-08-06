import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';

@Component({
  selector: 'app-testingsalespage',
  templateUrl: './testingsalespage.component.html',
  styles: [
  ]
})
export class TestingsalespageComponent implements OnInit { //extends ParentSalesMgtComponent

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService,) {
  }

  ngOnInit(): void {
  //  this.authService.SetApiEndPoint(this.translate.instant('Inventoryapiservice'));
  }

  setPage() {
    this.authService.SetApiEndPoint(this.translate.instant('Inventoryapiservice'));

  }
}
