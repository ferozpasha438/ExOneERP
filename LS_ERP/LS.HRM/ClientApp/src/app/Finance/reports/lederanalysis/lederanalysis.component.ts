import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ParentFinMgtComponent } from '../../../sharedcomponent/parentfinmgt.component';

@Component({
  selector: 'app-lederanalysis',
  templateUrl: './lederanalysis.component.html',
  styles: [
  ]
})
export class LederanalysisComponent extends ParentFinMgtComponent implements OnInit {

  trailList: any = [];
  totalDrBalance: string = '';
  totalCrBalance: string = '';
  totalBalance: string = '';
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

    if (this.dateFrom && this.dateTo) {
      this.isLoading = true;
      this.apiService.getall(`ledgerReport/ledgerReport?from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}`).subscribe(res => {
        if (res) {
          this.isLoading = false;
          this.trailList = res['list'];
          this.company = res['company'];

          this.totalCrBalance = res['totalCrBalance'];
          this.totalDrBalance = res['totalDrBalance'];
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
