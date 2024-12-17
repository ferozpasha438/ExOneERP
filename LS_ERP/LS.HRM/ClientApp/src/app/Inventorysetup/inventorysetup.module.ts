import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { InventoryconfigurationComponent } from './inventoryconfiguration/inventoryconfiguration.component';
import { InventorysetupRoutingModule } from './inventorysetup-routing.module';
import { ProductHierarchyComponent } from './producthierarchy/producthierarchy.component';
import { UnitofmeasureComponent } from './unitofmeasure/unitofmeasure.component';
import { WarehouseComponent } from './warehouse/warehouse.component';
import { AddupdarewarehouseComponent } from './sharedpages/addupdarewarehouse/addupdarewarehouse.component';
import { SharedModule } from '../sharedcomponent/shared.module';
import { ProductnewcategoryComponent } from './sharedpages/productnewcategory/productnewcategory.component';
import { ProductnewclassComponent } from './sharedpages/productnewclass/productnewclass.component';
import { ProductnewsubclassComponent } from './sharedpages/productnewsubclass/productnewsubclass.component';
import { AddupdatenewunitsComponent } from './sharedpages/addupdatenewunits/addupdatenewunits.component';
import { CategoryComponent } from './category/category.component';
import { SubcategoryComponent } from './subcategory/subcategory.component';
import { ProductnewsubcategoryComponent } from './sharedpages/productnewsubcategory/productnewsubcategory.component';
import { ClassComponent } from './class/class.component';
import { SubclassComponent } from './subclass/subclass.component';
import { InventoryDistributionComponent } from './inventorydistribution/inventorydistribution.component';
import { AddupdateInventoryDistributionComponent } from './sharedpages/addupdateinventorydistribution/addupdateinventorydistribution.component';
import { InventorypodistributionComponent } from './inventorypodistribution/inventorypodistribution.component';
import { AddupdateinventorypodistributionComponent } from './sharedpages/addupdateinventorypodistribution/addupdateinventorypodistribution.component';
import { ListexpairybatchComponent } from '../Inventorymgt/sharedpages/listexpairybatch/listexpairybatch.component';



@NgModule({
  declarations: [
    InventoryconfigurationComponent,
    WarehouseComponent,
    ProductHierarchyComponent,
    UnitofmeasureComponent,
    AddupdarewarehouseComponent,
    ProductnewcategoryComponent,
    ProductnewclassComponent,
    ProductnewsubclassComponent,
    AddupdatenewunitsComponent,
    CategoryComponent,
    SubcategoryComponent,
    ProductnewsubcategoryComponent,
    ClassComponent,
    SubclassComponent,
    InventoryDistributionComponent,
    AddupdateInventoryDistributionComponent,
    InventorypodistributionComponent,
    AddupdateinventorypodistributionComponent,
    ListexpairybatchComponent
  ],
  imports: [    
    InventorysetupRoutingModule,
    SharedModule,
  ],
  exports: [CommonModule],
})
export class InventorysetupModule { }
