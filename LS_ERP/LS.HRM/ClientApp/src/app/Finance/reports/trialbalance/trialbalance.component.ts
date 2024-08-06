import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ParentFinMgtComponent } from '../../../sharedcomponent/parentfinmgt.component';

@Component({
  selector: 'app-trialbalance',
  templateUrl: './trialbalance.component.html',
  styles: [
  ]
})
export class TrialbalanceComponent extends ParentFinMgtComponent implements OnInit {

  trailList: any = [];
  total: any;
  //totalDrBalance: string = '';
  //totalCrBalance: string = '';
  companyName: string = '';
  company: any;
  dateFrom: string = '';
  dateTo: string = '';
  isLoading: boolean = false;

  constructor(private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService,
    private notifyService: NotificationService) {
    super(authService);
  }

  ngOnInit(): void {    
  }

  search() {

    if (this.dateFrom) {//&& this.dateTo
      this.isLoading = true;
      this.apiService.getall(`ledgerReport/trialBalanceCutOffReport?from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}`).subscribe(res => {
        if (res) {
          this.isLoading = false;
          this.trailList = res['list'];
          this.company = res['company'];
          this.companyName = res['companyName'];
          this.total = res;
          //this.totalCrBalance = res['totalCrBalance'];
          //this.totalDrBalance = res['totalDrBalance'];
          //this.totalBalance = res['totalBalance'];
        }
      });
    }
    else
      this.notifyService.showError("Select Date");
  }

  openPrint() {
    const printContent = document.getElementById("printcontainer") as HTMLElement;    
    this.utilService.printForLocale(printContent);

    
  }

}
