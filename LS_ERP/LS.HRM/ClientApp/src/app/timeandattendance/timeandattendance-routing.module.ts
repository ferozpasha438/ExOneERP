import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GetpayrollgroupsComponent } from './Setup/PayrollGroup/getpayrollgroups/getpayrollgroups.component';
import { GetemployeeroastersComponent } from './Management/EmployeeRoaster/getemployeeroasters/getemployeeroasters.component';
import { GetEmployeeattendanceComponent } from './Management/EmployeeAttendance/get-employeeattendance/get-employeeattendance.component';

const routes: Routes = [
  { path: 'getpayrollgroups', component: GetpayrollgroupsComponent },
  { path: 'GetEmployeeroaster', component: GetemployeeroastersComponent },
  { path: 'GetEmployeeattendance', component: GetEmployeeattendanceComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TimeandattendanceRoutingModule {}
