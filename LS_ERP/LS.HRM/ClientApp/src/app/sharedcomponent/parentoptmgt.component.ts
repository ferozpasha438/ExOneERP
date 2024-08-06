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
export class ParentOptMgtComponent {
  constructor(authService: AuthorizeService) {
    //authService.SetApiEndPoint('my own api');
    //authService.SetApiEndPoint(authService.getUser().operationUri);
    //authService.SetApiEndPoint(data.operationapiurl);
    authService.SetApiEndPoint(authService.getUser()?.opmUrl);
  }
}
