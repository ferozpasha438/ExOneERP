import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ParentSystemSetupComponent } from '../../sharedcomponent/parentsystemsetup.component';

@Component({
  selector: 'app-currency',
  templateUrl: './currency.component.html',
  styles: [
  ]
})
export class CurrencyComponent extends ParentSystemSetupComponent implements OnInit {

  constructor(private authService: AuthorizeService) {
    super(authService);
  }

  ngOnInit(): void {
  }

}
