import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ParentFinMgtComponent } from '../../../sharedcomponent/parentfinmgt.component';

@Component({
  selector: 'app-profitandlossaccount',
  templateUrl: './profitandlossaccount.component.html',
  styles: [
  ]
})
export class ProfitandlossaccountComponent extends ParentFinMgtComponent implements OnInit {

  profitLossList: Array<any> = [];

  totalDrAmount: number = 0;
  totalCrAmount: number = 0;
  totalBalance: number = 0;
  isLoading: boolean = false;
  company: any;
  dateFrom: string = '';
  dateTo: string = '';

  constructor(private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService,
    private notifyService: NotificationService) {
    super(authService);
  }

  ngOnInit(): void {
  }

  search() {

    if (this.dateFrom && this.dateTo) {
      this.isLoading = true;
      this.apiService.getall(`report/getProfitAndLossList?type=PL&from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}`).subscribe(res => {
        this.isLoading = false;
        if (res) {
          this.profitLossList = res['list'];
          this.company = res['company'];

          this.totalCrAmount = res['totalCrAmount'];
          this.totalDrAmount = res['totalDrAmount'];
          this.totalBalance = res['totalBalance'];
        }
      });
    }
    else
      this.notifyService.showError("Select Dates");
  }



  openPrint() {
    const printContent = document.getElementById("printcontainer") as HTMLElement;    
    this.utilService.printForLocale(printContent);

   
  }

}
