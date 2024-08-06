import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SalesmgtRoutingModule } from './salesmgt-routing.module';
import { TestingsalespageComponent } from './testingsalespage/testingsalespage.component';
import { SharedModule } from '../sharedcomponent/shared.module';


@NgModule({
  declarations: [  
    TestingsalespageComponent
  ],
  imports: [    
    SalesmgtRoutingModule,
    SharedModule
  ],
  exports: [CommonModule],
})
export class SalesmgtModule { }
