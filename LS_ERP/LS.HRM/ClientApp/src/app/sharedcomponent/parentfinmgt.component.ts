import { Component } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { AuthorizeService } from "../api-authorization/AuthorizeService";
import { default as data } from "../../assets/i18n/apiuri.json";

@Component({
  selector: 'app-ParentFinMgtComponent',
  template: '',
  styles: [
  ]
})
export class ParentFinMgtComponent {
  constructor(authService: AuthorizeService) {
    //authService.SetApiEndPoint(authService.getUser().financeUri);    
    //authService.SetApiEndPoint(data.financemgtapiurl);
    authService.SetApiEndPoint(authService.getUser()?.finUrl);
  }
}
