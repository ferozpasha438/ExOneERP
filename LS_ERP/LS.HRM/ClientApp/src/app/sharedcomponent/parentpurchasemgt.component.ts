import { Component } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { AuthorizeService } from "../api-authorization/AuthorizeService";
import { default as data } from "../../assets/i18n/apiuri.json";

@Component({
  selector: 'app-ParentOptMgtComponent',
  template: '',
  styles: [
  ]
})
export class ParentPurchaseMgtComponent {
  constructor(authService: AuthorizeService) {
    //authService.SetApiEndPoint('my own api');
    //authService.SetApiEndPoint(authService.getUser().purchaseUri);
    //authService.SetApiEndPoint(data.purchaseapiurl);
    authService.SetApiEndPoint(authService.getUser()?.popUrl);
  }
}
