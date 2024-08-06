import { Component } from "@angular/core";
import { AuthorizeService } from "../api-authorization/AuthorizeService";
//import { default as data } from "../../assets/i18n/apiuri.json";

@Component({
  selector: 'app-ParentHrmAdminComponent',
  template: '',
  styles: [
  ]
})
export class ParentHrmAdminComponent {
  constructor(authService: AuthorizeService) {
    //authService.SetApiEndPoint(data.hrmadminapiurl);
    authService.SetApiEndPoint(authService.getUser()?.hrmUrl);
  }
}
