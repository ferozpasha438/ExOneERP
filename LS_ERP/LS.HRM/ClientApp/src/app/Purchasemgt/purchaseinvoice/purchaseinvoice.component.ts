import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ParentPurchaseMgtComponent } from '../../sharedcomponent/parentpurchasemgt.component';

@Component({
  selector: 'app-purchaseinvoice',
  templateUrl: './purchaseinvoice.component.html',
  styleUrls: []
})
export class PurchaseinvoiceComponent extends ParentPurchaseMgtComponent implements OnInit { //


  constructor(private authService: AuthorizeService, private router: Router) {
    super(authService);
  }


  ngOnInit(): void {
    //this.authService.SetApiEndPoint(this.authService.GetSystemSetupApiEndPoint());
    //this.authService.SetApiEndPoint('my own end point');
    this.router.navigate(['/dashboard/fin/appurchaseinvoice']);
  }
}
