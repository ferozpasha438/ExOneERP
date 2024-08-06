import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../api-authorization/AuthorizeService';

@Component({
  selector: 'app-parenttnamgt',
  // selector: 'app-ParenttnamgtComponent',
  template: ``,
  styles: [
  ]
})
export class ParenttnamgtComponent {
  constructor(authService: AuthorizeService) {
    authService.SetApiEndPoint(authService.getUser()?.hraUrl);
  }
}
