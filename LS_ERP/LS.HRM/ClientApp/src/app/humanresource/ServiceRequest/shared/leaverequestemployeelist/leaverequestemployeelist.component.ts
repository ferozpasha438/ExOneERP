import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ApiService } from '../../../../services/api.service';
import { CustomSelectListItem } from '../../../../models/MenuItemListDto';

@Component({
  selector: 'app-leaverequestemployeelist',
  templateUrl: './leaverequestemployeelist.component.html',
  styles: [
  ]
})
export class LeaverequestemployeelistComponent implements OnInit {
  empListSelectListItems: Array<CustomSelectListItem> = [];
  isLoading: boolean = false;
  @Output() empSelectEvent = new EventEmitter<any>();
  constructor(private apiService: ApiService) {

  };

  ngOnInit(): void {
    this.isLoading = true;
    this.apiService.getall(`personalInformation/getEmployeeSelectListItem`).subscribe(res => {
      this.empListSelectListItems = res;
      this.isLoading = false;
    });
  }

  selectEmpSelect(evt: any) {
    if (evt)
      this.empSelectEvent.emit(this.empListSelectListItems.find(e => e.value == evt.value));
    else
      this.empSelectEvent.emit(null);
  }

}
