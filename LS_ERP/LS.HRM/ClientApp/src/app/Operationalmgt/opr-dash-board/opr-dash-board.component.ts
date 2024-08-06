import { Component, OnInit } from '@angular/core';
import { Data } from '@angular/router';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { UtilityService } from '../../services/utility.service';
import { ParentOptMgtComponent } from '../../sharedcomponent/parentoptmgt.component';

@Component({
  selector: 'app-opr-dash-board',
  templateUrl: './opr-dash-board.component.html'
})
export class OprDashBoardComponent extends ParentOptMgtComponent implements OnInit {
  data: any;
  currentDate: Data = new Date();
  currentMonth: string='';
  currentYear: string='';
  constructor(private apiService: ApiService, private authService: AuthorizeService, private utilService: UtilityService) {
    super(authService);
  }
  ngOnInit(): void {
    
    this.initialLoading();
    

  }



  initialLoading() {
    this.currentMonth = this.currentDate.toString().substr(4,3)+','+this.currentDate.toString().substr(11,4)
    this.currentYear = this.currentDate.toString().substr(11,4)
    this.apiService.getall(`OperationsDashboard/getOpeartionsDashboard`).subscribe((res: any) => {
  

        this.data = res;
     
    });

  }


}
