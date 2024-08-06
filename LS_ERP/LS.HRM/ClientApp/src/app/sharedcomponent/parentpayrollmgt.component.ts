import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../api-authorization/AuthorizeService';

@Component({
  selector: 'app-parentpayrollmgt',
  template: ``,
  styles: [
  ]
})
export class ParentpayrollmgtComponent {
  constructor(authService: AuthorizeService) {
    authService.SetApiEndPoint(authService.getUser()?.hrsUrl);
  }
}
