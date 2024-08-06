import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../../services/api.service';
import { UtilityService } from '../../../../services/utility.service';
import { ParentSalesMgtComponent } from '../../../../sharedcomponent/parentsalesmgt.component';
import { SndServicesService } from '../../../snd-services.service';
import { AddupdatesndquotationComponent } from '../addupdatesndquotation.component';
import { ItemstockavailabilitylistPopupComponent } from '../itemstockavailabilitylist-popup/itemstockavailabilitylist-popup.component';
@Component({
  selector: 'app-quotationstockavailability-popup',
  templateUrl: './quotationstockavailability-popup.component.html'
})
export class QuotationstockavailabilityPopupComponent extends ParentSalesMgtComponent implements OnInit {
  //input begin
  inputData: any;
  //input end
  

  isArabic: boolean = false;
  tableData: Array<any> = [];
  warehouse: any;
  constructor(public dialog: MatDialog,private sndService: SndServicesService,public dialogRef: MatDialogRef<AddupdatesndquotationComponent>, private authService: AuthorizeService, private apiService: ApiService, private utilSerive: UtilityService) {
    super(authService);
  }

  ngOnInit(): void {
    this.isArabic = this.utilSerive.isArabic();
    
    if (this.inputData.id > 0) {


      if (this.inputData.source == 'D') {
        this.apiService.get('SndDeliveryNote/getSingleSndDeliveryNoteById', this.inputData.id).subscribe(res1 => {


          if (res1.itemList.length > 0) {
            this.inputData.itemList = res1.itemList.slice(0);
            this.apiService.getall(`Warehouse/getWarehouseInfoByCode/${res1.warehouseCode}`).subscribe(res3 => {
              if (res3 != null)

                this.warehouse = res3;
            });


          }


          console.log(res1);
          let inputDto = { warehouseCode: res1.warehouseCode, itemList: this.inputData.itemList.slice(0) };
          this.apiService.post('generateSndQuotation/quotationStockAvailabilty', inputDto).subscribe(res2 => {
            console.log(res2);
            this.tableData = res2 as Array<any>;
          },
            error => {


            });
        });
      }
      else if (this.inputData.source == 'Q' || this.inputData == null){
        //By default quotation
        this.apiService.get('generateSndQuotation/getSingleSndQuotationById', this.inputData.id).subscribe(res1 => {


          if (res1.itemList.length > 0) {
            this.inputData.itemList = res1.itemList.slice(0);
            this.apiService.getall(`Warehouse/getWarehouseInfoByCode/${res1.warehouseCode}`).subscribe(res3 => {
              if (res3 != null)

                this.warehouse = res3;
            });


          }


          console.log(res1);
          let inputDto = { warehouseCode: res1.warehouseCode, itemList: this.inputData.itemList.slice(0) };
          this.apiService.post('generateSndQuotation/quotationStockAvailabilty', inputDto).subscribe(res2 => {
            console.log(res2);
            this.tableData = res2 as Array<any>;
          },
            error => {


            });
        });
      }



    }
    else if (this.inputData.id == 0) {
   
      this.apiService.getall(`Warehouse/getWarehouseInfoByCode/${this.inputData.warehouseCode}`).subscribe(res3 => {
        if (res3 != null)

          this.warehouse = res3;
      });
      let inputDto = { warehouseCode: this.inputData.warehouseCode, itemList: this.inputData.itemList};
      this.apiService.post('generateSndQuotation/quotationStockAvailabilty', inputDto).subscribe(res2 => {
        console.log(res2);
        this.tableData = res2 as Array<any>;
      },
        error => {


        });
    
    }

  }
  closeModel() {
    this.dialogRef.close();
  }
  getStatusColor(item: any): string {
    
    if (item?.stockDetails?.qtyOH - item?.stockDetails?.qtyOnSalesOrder - item?.stockDetails?.qtyReserved < 0) {
      return `statusNegativeStock`;
    }
    else if (item?.stockDetails?.qtyOH - item?.stockDetails?.qtyOnSalesOrder - item?.stockDetails?.qtyReserved == 0) {
      return `statusNotAvailable`;
    }
    else if (item?.availableQuantity < item?.quantity) {
      return `statusPartialAvailable`;
    }
    else if (item?.availableQuantity >= item?.quantity) {
      return `statusAvailable`;
    }

    else
      return ``;
  }

  checkItemStockAvailability(item:any) {

    this.openDialogManage2(item.itemCode, this.warehouse.whCode, item.quantity, '', ItemstockavailabilitylistPopupComponent);

  }
  private openDialogManage2<T>(itemCode: string, WarehouseCode: string, quantity: number, unitType: string, component: T) {
    let dialogRef = this.sndService.openAutoWidthDialog(this.dialog, component);
    (dialogRef.componentInstance as any).itemCode = itemCode;
    (dialogRef.componentInstance as any).warehouseCode = WarehouseCode;
    (dialogRef.componentInstance as any).quantity = quantity;
    (dialogRef.componentInstance as any).unitType = unitType;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true) {

      }
    });
  }
}
