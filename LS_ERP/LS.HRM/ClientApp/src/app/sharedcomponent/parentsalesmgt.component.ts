import { Component } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { AuthorizeService } from "../api-authorization/AuthorizeService";
import { default as data } from "../../assets/i18n/apiuri.json";

@Component({
  selector: 'app-ParentSalesMgtComponent',
  template: '',
  styles: [
  ]
})
export class ParentSalesMgtComponent {
  constructor(authService: AuthorizeService) {    
    //authService.SetApiEndPoint(data.salesapiurl);
    authService.SetApiEndPoint(authService.getUser()?.sndUrl);
  }
}
