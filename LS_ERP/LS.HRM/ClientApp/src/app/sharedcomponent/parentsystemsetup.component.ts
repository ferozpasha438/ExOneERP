import { Component } from "@angular/core";
import { AuthorizeService } from "../api-authorization/AuthorizeService";

@Component({
  selector: 'app-ParentSystemSetupComponent',
  template: '',
  styles: [
  ]
})
export class ParentSystemSetupComponent {
  constructor(authService: AuthorizeService) {
    //authService.SetApiEndPoint(authService.GetSystemSetupApiEndPoint());
    authService.SetApiEndPoint(authService.getUser()?.admUrl);
  }
}
