import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';
import { ParentSystemSetupComponent } from '../../sharedcomponent/parentsystemsetup.component';
import { ValidationService } from '../../sharedcomponent/ValidationService';


@Component({
  selector: 'app-taxes',
  templateUrl: './taxes.component.html',
  styles: [
  ]
})
export class TaxesComponent extends ParentSystemSetupComponent implements OnInit {

  constructor(private authService: AuthorizeService) {
    super(authService);
  }

  ngOnInit(): void {
    
  }

  
}
