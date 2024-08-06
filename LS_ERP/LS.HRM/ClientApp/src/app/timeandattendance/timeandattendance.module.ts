import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TimeandattendanceRoutingModule } from './timeandattendance-routing.module';
import { GetpayrollgroupsComponent } from './Setup/PayrollGroup/getpayrollgroups/getpayrollgroups.component';
import { SharedModule } from '../sharedcomponent/shared.module';
import { AddupdatepayrollgroupsComponent } from './Setup/PayrollGroup/addupdatepayrollgroups/addupdatepayrollgroups.component';
import { GetemployeeroastersComponent } from './Management/EmployeeRoaster/getemployeeroasters/getemployeeroasters.component';
import { GetEmployeeattendanceComponent } from './Management/EmployeeAttendance/get-employeeattendance/get-employeeattendance.component';
import { ImportemployeeattendanceComponent } from './Management/EmployeeAttendance/importemployeeattendance/importemployeeattendance.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import { ConsolidateemployeeattendanceComponent } from './Management/EmployeeAttendance/consolidateemployeeattendance/consolidateemployeeattendance.component';


@NgModule({
  declarations: [
  
    GetpayrollgroupsComponent,
        AddupdatepayrollgroupsComponent,
        GetemployeeroastersComponent,
        GetEmployeeattendanceComponent,
        ImportemployeeattendanceComponent,
        ConsolidateemployeeattendanceComponent
  ],
  imports: [
    CommonModule,
    TimeandattendanceRoutingModule,
    SharedModule, 
    MatTooltipModule,
    MatButtonModule,
    MatIconModule
  ]
})
export class TimeandattendanceModule { }
