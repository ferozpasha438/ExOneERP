import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InventorymanagementlistComponent } from './inventorymanagementlist/inventorymanagementlist.component';
import { AdjustmentsComponent } from './adjustments/adjustments.component'
import { IssuesComponent } from './issues/issues.component';
import { ReceiptsComponent } from './receipts/receipts.component';
/*import { InventoryStockReconcilation } from './stockreconcilation/stockreconcilation.component';*/
import { TransferComponent } from './transfer/transfer.component';
//import { InventorytransactionissuesComponent } from './inventorytransactionissues/inventorytransactionissues.component';

//import { InventorytransactionadjustmentsComponent } from './inventorytransactionadjustments/inventorytransactionadjustments.component';
//import { InventorytransactionreceiptsComponent } from './inventorytransactionreceipts/inventorytransactionreceipts.component';
//import { InventorytransactiontransferComponent } from './inventorytransactiontransfer/inventorytransactiontransfer.component';
//import { InventorytransactionstockreconciliationComponent } from './inventorytransactionstockreconciliation/inventorytransactionstockreconciliation.component';


const routes: Routes = [
  { path: 'inventorymanagementlist', component: InventorymanagementlistComponent },
  { path: 'adjustments', component: AdjustmentsComponent },
  { path: 'Issues', component: IssuesComponent },
  { path: 'Receipts', component: ReceiptsComponent },
 /* { path: 'Stockreconcilation', component: InventoryStockReconcilation },*/
  { path: 'Transfer', component: TransferComponent },


  //{ path: 'issuesComponent', component: InventorytransactionissuesComponent },
  //{ path: 'receiptsComponent', component: InventorytransactionreceiptsComponent },
  //{ path: 'stockreconciliationComponent', component: InventorytransactionstockreconciliationComponent },
  //{ path: 'transferComponent', component: InventorytransactiontransferComponent },

  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InventorymgsetupRoutingModule { }
