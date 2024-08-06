import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HrmadminsetupRoutingModule } from './hrmadminsetup-routing.module';
import { PositionComponent } from './position/position.component';
import { SharedModule } from '../sharedcomponent/shared.module';
import { AddupdatepositionComponent } from './addupdateposition/addupdateposition.component';


@NgModule({
  declarations: [
    PositionComponent,
    AddupdatepositionComponent
  ],
  imports: [
    CommonModule,
    HrmadminsetupRoutingModule,
    SharedModule,
  ]
})
export class HrmadminsetupModule { }
