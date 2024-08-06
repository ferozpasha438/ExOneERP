import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from '../sharedcomponent/shared.module';
import { InventorymanagementlistComponent } from './inventorymanagementlist/inventorymanagementlist.component';
import { InventorymgsetupRoutingModule } from './inventorymgsetup-routing.module';
import { AdjustmentsComponent } from './adjustments/adjustments.component';
import { InventoryuserapprovalComponent } from './sharedpages/inventoryuserapproval/inventoryuserapproval.component';
import { IssuesComponent } from './issues/issues.component';
import { ReceiptsComponent } from './receipts/receipts.component';
/*import { InventoryStockReconcilation } from './stockreconcilation/stockreconcilation.component';*/
import { TransferComponent } from './transfer/transfer.component';
import { AddupdateinvmanagemenetlistComponent } from './sharedpages/addupdateinvmanagemenetlist/addupdateinvmanagemenetlist.component';
import { AddupdatereceiptsComponent } from './sharedpages/addupdatereceipts/addupdatereceipts.component';
import { AddupdateissuesComponent } from './sharedpages/addupdateissues/addupdateissues.component';
import { AddupdateadjacementComponent } from './sharedpages/addupdateadjacement/addupdateadjacement.component';
import { AddupdatetransferComponent } from './sharedpages/addupdatetransfer/addupdatetransfer.component';
import { AddupdateinventorymanagementlistComponent } from './sharedpages/addupdateinventorymanagementlist/addupdateinventorymanagementlist.component';




 
 


@NgModule({
  declarations: [    
    InventorymanagementlistComponent, AdjustmentsComponent, InventoryuserapprovalComponent, IssuesComponent, ReceiptsComponent, TransferComponent, AddupdateinvmanagemenetlistComponent, AddupdatereceiptsComponent, AddupdateissuesComponent, AddupdateadjacementComponent, AddupdatetransferComponent, AddupdateinventorymanagementlistComponent
  ],
  imports: [    
    InventorymgsetupRoutingModule,
    SharedModule
  ],
  exports: [CommonModule],
})
export class InventorymgsetupModule { }
