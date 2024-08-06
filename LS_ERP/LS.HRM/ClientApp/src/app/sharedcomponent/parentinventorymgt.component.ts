import { Component } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { AuthorizeService } from "../api-authorization/AuthorizeService";
import { default as data } from "../../assets/i18n/apiuri.json";

@Component({
  selector: 'app-ParentInventoryMgtComponent',
  template: '',
  styles: [
  ]
})
export class ParentInventoryMgtComponent {
  constructor(authService: AuthorizeService) {
    //authService.SetApiEndPoint(authService.getUser().inventoryUri);
    //authService.SetApiEndPoint(data.inventorymgtapiurl);
    authService.SetApiEndPoint(authService.getUser()?.invUrl);
  }
}
