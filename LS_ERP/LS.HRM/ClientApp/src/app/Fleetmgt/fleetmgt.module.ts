import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from '../sharedcomponent/shared.module';
import { FleetmgtRoutingModule } from './fleetmgt-routing.module';
import { VehicleCompanyComponent } from './vehicle-company/vehicle-company.component';
import { AddupdateVehicleCompanyComponent } from './shared/addupdate-vehicle-company/addupdate-vehicle-company.component';
import { VehicleTypeComponent } from './vehicle-type/vehicle-type.component';
import { AddupdateVehicleTypeComponent } from './shared/addupdate-vehicle-type/addupdate-vehicle-type.component';
import { BrandComponent } from './brand/brand.component';
import { AddupdateBrandComponent } from './shared/addupdate-brand/addupdate-brand.component';
import { DriverComponent } from './driver/driver.component';
import { AddupdateDriverComponent } from './shared/addupdate-driver/addupdate-driver.component';
import { RouteMasterComponent } from './route-master/route-master.component';
import { AddupdateRouteMasterComponent } from './shared/addupdate-route-master/addupdate-route-master.component';
import { RoutePlanComponent } from './route-plan/route-plan.component';
import { AddupdateRoutePlanComponent } from './shared/addupdate-route-plan/addupdate-route-plan.component';
import { VehicleMasterComponent } from './vehicle-master/vehicle-master.component';
import { AddupdateVehicleMasterComponent } from './shared/addupdate-vehicle-master/addupdate-vehicle-master.component';
import { FuelEntryComponent } from './fuel-entry/fuel-entry.component';
import { AddupdateFuelEntryComponent } from './shared/addupdate-fuel-entry/addupdate-fuel-entry.component';
import { AssignDriverComponent } from './assign-driver/assign-driver.component';
import { AddupdateAssignDriverComponent } from './shared/addupdate-assign-driver/addupdate-assign-driver.component';
import { AssignRouteComponent } from './assign-route/assign-route.component';
import { AddupdateAssignRouteComponent } from './shared/addupdate-assign-route/addupdate-assign-route.component';
import { ServiceCodeComponent } from './service-code/service-code.component';
import { AddupdateServiceCodeComponent } from './shared/addupdate-service-code/addupdate-service-code.component';
import { ServiceProviderComponent } from './service-provider/service-provider.component';
import { AddupdateServiceProviderComponent } from './shared/addupdate-service-provider/addupdate-service-provider.component';

@NgModule({
  declarations: [
    VehicleCompanyComponent,
    AddupdateVehicleCompanyComponent,
    VehicleTypeComponent,
    AddupdateVehicleTypeComponent,
    BrandComponent,
    AddupdateBrandComponent,
    DriverComponent,
    AddupdateDriverComponent,
    RouteMasterComponent,
    AddupdateRouteMasterComponent,
    RoutePlanComponent,
    AddupdateRoutePlanComponent,
    VehicleMasterComponent,
    AddupdateVehicleMasterComponent,
    FuelEntryComponent,
    AddupdateFuelEntryComponent,
    AssignDriverComponent,
    AddupdateAssignDriverComponent,
    AssignRouteComponent,
    AddupdateAssignRouteComponent,
    ServiceCodeComponent,
    AddupdateServiceCodeComponent,
    ServiceProviderComponent,
    AddupdateServiceProviderComponent
  ],
  imports: [
    FleetmgtRoutingModule,
    SharedModule
  ],
  exports: [CommonModule],
})

export class FleetmgtModule {

}

