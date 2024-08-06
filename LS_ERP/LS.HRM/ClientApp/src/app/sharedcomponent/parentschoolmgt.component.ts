import { Component } from "@angular/core";
import { AuthorizeService } from "../api-authorization/AuthorizeService";
import { default as data } from "../../assets/i18n/apiuri.json";

@Component({
  selector: 'app-ParentSchoolMgtComponent',
  template: '',
  styles: [
  ]
})
export class ParentSchoolMgtComponent {
  constructor(authService: AuthorizeService) {
    //authService.SetApiEndPoint(authService.getUser().schoolUri);
    //authService.SetApiEndPoint(data.schoolapiurl);
    authService.SetApiEndPoint(authService.getUser().schUrl);
  }
}



@Component({
  selector: 'app-ParentSchoolAPIMgtComponent',
  template: '',
  styles: [
  ]
})
export class ParentSchoolAPIMgtComponent {
  constructor(authService: AuthorizeService) {
    //authService.SetApiEndPoint(authService.getUser().schoolUri);
    //authService.SetApiEndPoint(data.schoolapiurl);
    authService.SetApiEndPoint(authService.getUser()?.schUrl);
  }
}
