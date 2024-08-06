import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VehicleCompanyComponent } from './vehicle-company/vehicle-company.component';
import { VehicleTypeComponent } from './vehicle-type/vehicle-type.component';
import { BrandComponent } from './brand/brand.component';
import { DriverComponent } from './driver/driver.component';
import { RouteMasterComponent } from './route-master/route-master.component';
import { RoutePlanComponent } from './route-plan/route-plan.component';
import { VehicleMasterComponent } from './vehicle-master/vehicle-master.component';
import { FuelEntryComponent } from './fuel-entry/fuel-entry.component';
import { AssignDriverComponent } from './assign-driver/assign-driver.component';
import { AssignRouteComponent } from './assign-route/assign-route.component';
import { ServiceCodeComponent } from './service-code/service-code.component';
import { ServiceProviderComponent } from './service-provider/service-provider.component';





const routes: Routes = [/*{ path: '', component: PurchasesetupComponent }*/
  { path: 'vehicleCompany', component: VehicleCompanyComponent },
  { path: 'vehicleType', component: VehicleTypeComponent },
  { path: 'brand', component: BrandComponent },
  { path: 'driver', component: DriverComponent },
  { path: 'routeMaster', component: RouteMasterComponent },
  { path: 'routePlan', component: RoutePlanComponent },
  { path: 'vehicleMaster', component: VehicleMasterComponent },
  { path: 'fuelEntry', component: FuelEntryComponent },
  { path: 'assignDriver', component: AssignDriverComponent },
  { path: 'assignRoute', component: AssignRouteComponent },
  { path: 'serviceCode', component: ServiceCodeComponent },
  { path: 'serviceProvider', component: ServiceProviderComponent }
 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FleetmgtRoutingModule { }
