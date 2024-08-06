
import { Component, Input, OnInit } from '@angular/core';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';

  @Component({
    selector: 'app-poprintingformatfourpage',
    templateUrl: './poprintingformatfourpage.component.html',
    styles: [
    ]
  })
  export class PoprintingformatfourpageComponent implements OnInit {

  //@Input() data: any;
  //isLoading: boolean = false;
  //isArabic: boolean = false;
  //item: any;
  //company: any;
  //itemList: any;

  //constructor(private apiService: ApiService,
  //  private authService: AuthorizeService, private utilService: UtilityService,
  //  private notifyService: NotificationService) {

  //}


  ngOnInit(): void {
  //  if (this.data > 0)
  //    this.loadSalesView();
  }

  //loadSalesView() {
  //  this.isLoading = true;
  //  this.apiService.get("PurchaseOrder/getPOPrintingFourList", this.data).subscribe(res => {
  //    this.isLoading = false;
  //    if (res) {
  //      this.item = res;
  //      this.itemList = res['itemList'];
  //      this.company = res['company'];

  //      this.isArabic = this.authService.isArabic();
  //    }
  //  });
  //}

}
