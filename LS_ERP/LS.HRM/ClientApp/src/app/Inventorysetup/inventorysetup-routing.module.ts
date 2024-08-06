import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InventoryconfigurationComponent } from './inventoryconfiguration/inventoryconfiguration.component';
import { ProductHierarchyComponent } from './producthierarchy/producthierarchy.component';
import { UnitofmeasureComponent } from './unitofmeasure/unitofmeasure.component';
import { WarehouseComponent } from './warehouse/warehouse.component';
import { CategoryComponent } from './category/category.component';
import { SubcategoryComponent } from './subcategory/subcategory.component';
import { ClassComponent } from './class/class.component';
import { SubclassComponent } from './subclass/subclass.component';
import { InventoryDistributionComponent } from './inventorydistribution/inventorydistribution.component';
import { InventorypodistributionComponent } from './inventorypodistribution/inventorypodistribution.component';

const routes: Routes = [
  { path: 'warehouse', component: WarehouseComponent },
  { path: 'invtconfiguration', component: InventoryconfigurationComponent },
  { path: 'invtunitofmeasure', component: UnitofmeasureComponent },
  { path: 'producthierarchy', component: ProductHierarchyComponent },
  { path: 'category', component: CategoryComponent },
  { path: 'subcategory', component: SubcategoryComponent },
  { path: 'class', component: ClassComponent },
  { path: 'subclass', component: SubclassComponent },
  { path: 'invdistribution', component: InventoryDistributionComponent },
  { path: 'invpodistribution', component: InventorypodistributionComponent },



/*  { path: 'inventory', loadChildren: () => import('./inventorysetup.module').then(m => m.InventorysetupModule) },*/
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InventorysetupRoutingModule { }
