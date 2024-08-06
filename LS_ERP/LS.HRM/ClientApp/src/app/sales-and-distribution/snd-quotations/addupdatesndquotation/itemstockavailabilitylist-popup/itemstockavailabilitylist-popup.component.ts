import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../../services/api.service';
import { UtilityService } from '../../../../services/utility.service';
import { ParentSalesMgtComponent } from '../../../../sharedcomponent/parentsalesmgt.component';
import { AddupdatesndquotationComponent } from '../addupdatesndquotation.component';

@Component({
  selector: 'app-itemstockavailabilitylist-popup',
  templateUrl: './itemstockavailabilitylist-popup.component.html'
})
export class ItemstockavailabilitylistPopupComponent extends ParentSalesMgtComponent implements OnInit {
  /*throug Input*/
  itemCode: string;
  warehouseCode: string;
  quantity: number;
  unitType: string='';


  tableData: Array<any> = [];
  isArabic: boolean = false;
  item: any;
  warehouse: any;
  ium: any;
  convertedQty: number = 0;
  constructor(public dialogRef: MatDialogRef<AddupdatesndquotationComponent>, private authService: AuthorizeService, private apiService: ApiService, private utilSerive: UtilityService) {
    super(authService);
  }

  ngOnInit(): void {
    this.isArabic = this.utilSerive.isArabic();
    this.getItemUnitMap();
    this.getItem();
    this.getWarehouse();
    this.CheckStockAvailability();
    
  }


  getItemUnitMap() {
    if (this.unitType == '')
      this.ium = { itemConvFactor: 1 };
    else {
      this.apiService.getall(`item/getItemUomMapByItemUnit/${this.itemCode}/${this.unitType}`).subscribe(res => {
        this.ium = res;
      });
    }
  }

  getItem() {
    this.apiService.getall(`item/getItemByItemCode/${this.itemCode}`).subscribe(res => {
      if (res != null) {
        console.log("item:");
        console.log(res);
        this.item = res;
      }
    });
  }
  getWarehouse() {
    this.apiService.getall(`warehouse/getWarehouseInfoByCode/${this.warehouseCode}`).subscribe(res => {
      if (res != null) {
        console.log("warehouse:");
        console.log(res);
        this.warehouse = res;
      }
    });
  }


  CheckStockAvailability() {
    if (this.itemCode != '') {
      this.apiService.getall(`item/itemStockAvailability/${this.itemCode}`).subscribe(res => {
        if (res)
          console.log(res);
        this.tableData = res as Array<any>;
       });
    }

  }



  closeModel() {
    this.dialogRef.close();
  }


  getStatusColor(item:any):string {
    let reqQty = this.quantity * this.ium?.itemConvFactor;
    if (item?.qtyOH - item?.qtyOnSalesOrder - item?.qtyReserved < 0) {
      return `statusNegativeStock`;
    }
    else if (item?.qtyOH - item?.qtyOnSalesOrder - item?.qtyReserved ==0) {
      return `statusNotAvailable`;
    }
    else if (item?.qtyOH - item?.qtyOnSalesOrder - item?.qtyReserved < reqQty) {
      return `statusPartialAvailable`;
    }
      else if (item?.qtyOH - item?.qtyOnSalesOrder - item?.qtyReserved >= reqQty) {
      return `statusAvailable`;
    }

    else
      return ``;
  }
}
