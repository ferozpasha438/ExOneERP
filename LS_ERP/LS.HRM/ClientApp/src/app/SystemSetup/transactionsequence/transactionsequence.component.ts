import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ParentSystemSetupComponent } from '../../sharedcomponent/parentsystemsetup.component';

@Component({
  selector: 'app-transactionsequence',
  templateUrl: './transactionsequence.component.html',
  styles: [
  ]
})
export class TransactionsequenceComponent extends ParentSystemSetupComponent implements OnInit {

  constructor(private authService: AuthorizeService) {
    super(authService);
  }

  ngOnInit(): void {
  }

}
