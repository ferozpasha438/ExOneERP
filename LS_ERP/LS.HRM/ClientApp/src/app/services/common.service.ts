import { HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { ApiMessageDto } from "../models/CINServerMetaDataDto";
import { CustomSelectListItem } from "../models/MenuItemListDto";
import { ApiService } from "./api.service";
import { NotificationService } from "./notification.service";
import { ErrorMessage } from "./utility.constants";

@Injectable({
  providedIn: 'root'
})
export class CommonService {
  constructor(private apiService: ApiService) {

  }

  filter(val: string): Observable<Array<CustomSelectListItem>> {
    return this.apiService.getall(`branch/getSelectSysBranchList?search=${val.trim()}`)
      .pipe(
        map(response => {
          const res = response as Array<CustomSelectListItem>;        
          return res;
        })
      )
  }

}
