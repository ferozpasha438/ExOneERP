import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { UpdatingcustomerComponent } from '../../../Finance/accountsreceivable/sharedpages/updatingcustomer/updatingcustomer.component';
import { CustomSelectListItem, LanCustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
import { CommonService } from '../../../services/common.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ParentSalesMgtComponent } from '../../../sharedcomponent/parentsalesmgt.component';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { ItemstockavailabilitylistPopupComponent } from '../../snd-quotations/addupdatesndquotation/itemstockavailabilitylist-popup/itemstockavailabilitylist-popup.component';
import { QuotationstockavailabilityPopupComponent } from '../../snd-quotations/addupdatesndquotation/quotationstockavailability-popup/quotationstockavailability-popup.component';
import { SndServicesService } from '../../snd-services.service';
@Component({
  selector: 'app-editdeliverynote',
  templateUrl: './editdeliverynote.component.html'
})
export class EditdeliverynoteComponent extends ParentSalesMgtComponent implements OnInit {

  form: FormGroup;
  modalTitle: string;
  id: number = 0;


  

  customerList: Array<LanCustomSelectListItem> = [];
  companyList: Array<CustomSelectListItem> = [];
  paymentTermsList: Array<LanCustomSelectListItem> = [];
  warehouseList: Array<CustomSelectListItem> = [];
  itemList: Array<CustomSelectListItem> = [];
  vatList: Array<CustomSelectListItem> = [];

  itemId: string = '';
  quantity: number = 0;
  unitPrice: number = 0;
  vat: number = 0;
  vatAmount: number = 0;
  total: number = 0;
  itemInvAmount: number = 0;
  discount: number = 0;
  discountAmount: number = 0;


  unitType: string = '';
  description: string = '';
  item: string = '';
  itemCode: string = '';


  grandDiscountAmount: number = 0;
  grandTotal: number = 0;
  grandFooterDiscountAmount: number = 0;
  grandVatTotal: number = 0;

  grandQuotationTotal: number = 0;
  grandTotalStr: string = '';
  grandVatTotalStr: string = '';
  grandQuotationTotalStr: string = '';

  grandFooterDiscountAmountStr: string = '';

  footerDiscount: number = 0;
  footerDiscountAmount: number = 0;
  sequence: number = 1;
  editsequence: number = 0;
  listOfDeliveryNotes: Array<any> = [];

  canShowNotes: boolean = false;
  canShowRemarks: boolean = false;
  deliveryNoteItemObject: any;
  isArab: boolean = false;
  customerData: null;
 
  delivery: number=0;

  unitTypesList: Array<any> = [];// [{ text: "BOX", value: "BOX" }, { text: "EACH", value: "EACH" }];

  editingItem: any = {delivery:0};
  editingInitialDelivery=0;
  editRowNumber = -1;
  warehouseCode: string;

  isLoading: boolean = false;


  isUpdated: boolean = false;
  constructor(private sndService: SndServicesService, private translate: TranslateService, private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<EditdeliverynoteComponent>,
    private notifyService: NotificationService, private validationService: ValidationService, private commonService: CommonService, public dialog: MatDialog) {
    super(authService);
  }

  ngOnInit(): void {
   
   
      this.seteditform();
  }
 
  seteditform() {


    this.apiService.get("SndDeliveryNote/getSingleSndDeliveryNoteById", this.id).subscribe(res => {
      if (res) {

        this.footerDiscount = res.footerDiscount;
        this.warehouseCode = res.warehouseCode;
        this.listOfDeliveryNotes = res.itemList as Array<any>;

      }
    });
  }


  editDeliveryNoteItem(index: number) {

  
    this.editingInitialDelivery = this.listOfDeliveryNotes[index].delivery;

    this.editingItem = this.listOfDeliveryNotes[index];

    this.editRowNumber = index;
  }

 



 
 


  







  
  closeModel() {
    this.dialogRef.close(this.isUpdated);
  }
 
  changeDeliveryQuantity(index: number) {

      if (this.editingItem.delivery >= 0&& this.editingItem.quantity - this.editingItem.delivery >= 0 && this.editingItem.quantity != null) {



        this.listOfDeliveryNotes[index].delivery = this.editingItem.delivery;
        this.listOfDeliveryNotes[index].backOrder = this.editingItem.quantity - this.editingItem.delivery;
        this.editRowNumber = index;
      }
      else {
        this.editingItem.delivery = 0;
        this.listOfDeliveryNotes[index].delivery = this.listOfDeliveryNotes[index].quantity;
        this.listOfDeliveryNotes[index].backOrder = 0;
      }

    }
  
 
    
 
  

 

  
 



  public CheckStockAvailability(item: any) {
    let itemCode = item == null ? this.itemCode : item.itemCode;
    let quantity = item == null ? this.quantity : item.quantity;              // may be item.delivery
    let unitType = item == null ? this.unitType : item.unitType;
    
    if (itemCode != '' && this.warehouseCode && quantity > 0 && unitType != '') {

      this.openDialogManage2(itemCode, this.warehouseCode, quantity, unitType, ItemstockavailabilitylistPopupComponent);
    }
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

  checkQuotationStock() {
    this.form.value['id'] = 0;
    this.form.value['itemList'] = this.listOfDeliveryNotes.slice(0);

    let inputData: any = this.form.value;
    this.openDialogManage3(inputData, QuotationstockavailabilityPopupComponent);
  }
  private openDialogManage3<T>(inputData: any, component: T) {
    let dialogRef = this.sndService.openAutoWidthDialog(this.dialog, component);

    (dialogRef.componentInstance as any).inputData = inputData;


    dialogRef.afterClosed().subscribe(res => {

    });
  }
  cancelEditing() {




    this.listOfDeliveryNotes[this.editRowNumber].delivery = this.editingInitialDelivery;
    this.listOfDeliveryNotes[this.editRowNumber].backOrder = this.editingItem.quantity - this.editingItem.delivery;
    this.editRowNumber = -1;
    this.editingItem = null;

  }
  save() {
    if (!this.isLoading) {
      if (this.editRowNumber != -1) {
        let input = this.listOfDeliveryNotes[this.editRowNumber];
        this.isLoading = true;
        this.apiService.post('SndDeliveryNote/updateSndDeliveryNoteLineDeliveryQty',input).subscribe(res => {
          this.utilService.OkMessage();
          this.seteditform();
          this.editRowNumber = -1;
          this.isLoading = false;
          this.isUpdated = true;
        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
            this.isLoading = false;
          });
      }

    }
   
  }
}
