import { Component, Input, OnInit } from '@angular/core';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';


@Component({
  selector: 'app-poprintingformatonepage',
  templateUrl: './poprintingformatonepage.component.html',
  styles: [
  ]
})
export class PoprintingformatonepageComponent implements OnInit {

  @Input() data: any;
  isLoading: boolean = false;
  isArabic: boolean = false;
  typeOfSource: string = '';
  item: any;
  company: any;
  itemList: any;
  shipmentList: any;

  constructor(private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService,
    private notifyService: NotificationService) {

  }


  ngOnInit(): void {
    if (this.data) {
      this.loadSalesView();    
      this.typeOfSource = (this.data.from == 'PO' ? 'Purchase Order'
        : (this.data.from == 'PR' ? 'Purchase Requisition' : (this.data.from == 'prt' ? 'Purchase Return' : 'Goods Received Note - GRN ')));
    }
  }

  loadSalesView() {
    this.isLoading = true;
    this.apiService.getall(`PurchaseOrder/getPOPrintingOneList/${this.data.id}?type=${this.data.from}`).subscribe(res => {
      this.isLoading = false;
      if (res) {
        this.item = res;
        this.itemList = res['itemList'];
        this.shipmentList = res['shipmentList'];
        this.company = res['company'];

        this.isArabic = this.authService.isArabic();
      }
    });
  }
}
