import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ParentSystemSetupComponent } from '../../sharedcomponent/parentsystemsetup.component';

@Component({
  selector: 'app-financialmastersetup',
  templateUrl: './financialmastersetup.component.html',
  styles: [
  ]
})
export class FinancialmastersetupComponent extends ParentSystemSetupComponent implements OnInit {

  constructor(private authService: AuthorizeService) {
    super(authService);
  }

  ngOnInit(): void {
  }

}
