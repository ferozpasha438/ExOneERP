import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../../services/api.service';
import { DBOperation } from '../../../../services/utility.constants';
import { ParentOptMgtComponent } from '../../../../sharedcomponent/parentoptmgt.component';

@Component({
  selector: 'app-view-pv-add-res-request',
  templateUrl: './view-pv-add-res-request.component.html'
})
export class ViewPvAddResRequestComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number;
  requestData: any;
  constructor(private authService: AuthorizeService, private apiService: ApiService) {

    super(authService);
  }

  ngOnInit(): void {

    this.LoadRequestData();
  }


  LoadRequestData() {
    this.apiService.get('PvAddResource/getPvAddResourceReqById', this.id).subscribe(res => {
      if (res != null) {
        this.requestData = res as any;
        

      }

    });

  }
}
