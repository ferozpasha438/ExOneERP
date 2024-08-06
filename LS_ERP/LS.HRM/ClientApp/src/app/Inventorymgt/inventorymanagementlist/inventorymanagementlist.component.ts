import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { FormArray,FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';
import { ValidationService } from '../../sharedcomponent/ValidationService';
 import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
 import { DBOperation, ErrorMessage } from '../../services/utility.constants';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { CustomSelectListItem } from '../../models/MenuItemListDto';
import { DeleteConfirmDialogComponent } from '../../sharedcomponent/delete-confirm-dialog';
import { ParentInventoryMgtComponent } from '../../sharedcomponent/parentinventorymgt.component';
import { AddupdateinventorymanagementlistComponent } from '../sharedpages/addupdateinventorymanagementlist/addupdateinventorymanagementlist.component';
import { MultiFileUploadDto } from '../../models/sharedDto';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-inventorymanagementlist',
  templateUrl: './inventorymanagementlist.component.html',
  styleUrls: []
})
export class InventorymanagementlistComponent extends ParentInventoryMgtComponent implements OnInit {

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  //@ViewChild(MatPaginator) paginator: MatPaginator;
  //@ViewChild(MatSort) sort: MatSort;
  displayedColumns: string[] = ['ItemCode', 'h_code', 'shortname', 'itemdescription', 'category', 'itemavgcost', 'itemstandardcost', 'Actions'];
  data: MatTableDataSource<any> | null;
  totalItemsCount: number;
  sortingOrder: string = 'id desc';
  form: FormGroup;
  searchValue: string = '';
  isLoading: boolean = false;
  isReadOnly: boolean = false;
  displayedColumns1: string[] = ['WareHouse', 'TranDate', 'TranType', 'TranNumber', 'TranUnit', 'unitConvFactor', 'TranTotQty', 'TranPrice', 'ItemAvgCost', 'TranRemarks'];
  data1: MatTableDataSource<any> | null;
  /*ItemMasterform: FormGroup;*/

  VenCodeList: Array<CustomSelectListItem> = [];
  CategoryCodeList: Array<CustomSelectListItem> = [];
  SubCategoryCodeList: Array<CustomSelectListItem> = [];
  ClassCodeList: Array<CustomSelectListItem> = [];
  SubClassCodeList: Array<CustomSelectListItem> = [];
  TaxCodeList: Array<CustomSelectListItem> = [];
  UomLIst: Array<CustomSelectListItem> = [];
  warehouseList: Array<CustomSelectListItem> = [];
  ItemTrackingList: Array<CustomSelectListItem> = [];
  ItemTypeLIst: Array<CustomSelectListItem> = [];

  CategoryCode: string = '';
  id: number = 0;
  itemcode: string = '';


  listOfInvoices: Array<any> = [];

  itemCode: string = '';
  wHCode: string = '';
  tranDate: string = '';
  tranNumber: string = '';
  tranUnit: string = '';
  tranQty: string = '';
  unitConvFactor: string = '';
  tranTotQty: string = '';
  tranPrice: string = '';
  itemAvgCost: string = '';
  tranRemarks: string = '';
  ItemCodes: string = '';

  sequence: number = 1;
  editsequence: number = 0;
  editInventoryUOMItemsequence: number = 0;
  editBarcodesequence: number = 0;
  listOfinventory: Array<any> = [];

  whCode: string = '';
  qtyOH: number = 0;
  qtyOnSalesOrder: number = 0;
  qtyOnPO: number = 0;
  qtyReserved: number = 0;
  invitemAvgCost: number = 0;
  itemLastPOCost: number = 0;
  itemLandedCost: number = 0;
  minQty: number = 0;
  maxQty: number = 0;
  eoq: number = 0;

  listOfUOMinventory: Array<any> = [];
  itemUOM: string = '';
  itemConvFactor: number = 0;
  itemUOMPrice1: number = 0;
  itemUOMPrice2: number = 0;
  itemUOMPrice3: number = 0;
  itemUOMDiscPer: number = 0;
  itemUOMPrice4: number = 0;
  itemUomAvgCost: number = 0;
  itemUomLastPOCost: number = 0;
  itemUomLandedCost: number = 0;

  listOfBarcodeinventory: Array<any> = [];
  itemBarUOM: string = '';
  itemBarcode: string = '';
  
  listOfNotesinventory: Array<any> = [];
  noteDates: string = '';
  name: string = '';
  notes: string = '';



  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService,
    private notifyService: NotificationService, private validationService: ValidationService, public pageService: PaginationService, public dialog: MatDialog, private translate: TranslateService) {
    super(authService);
  }

  ngOnInit(): void {
   
    this.setForm();
    this.loadCategoryCode();
  
    this.loadClassCode();
    this.loadSubClassCode();
    this.loadTaxCode();
    this.loadUOMCode();
    this.loadWarehouses();
   
    this.loadVenCode();
   
    this.initialLoading();
    
    this.loadType();
  }
  loadinventoryHistory(Value: string) {
    if (Value != null) {
      this.apiService.getall(`Inventorymanagementlist/InventoryHistory?ItemCodes=${Value}`).subscribe(res => {

        if (res) {
          /*this.listOfInvoices.clear();*/
          let listOfInvoices = res['historyList'] as Array<any>;
          listOfInvoices.forEach(item => {
            //this.editInvoiceItem(item);
            this.listOfInvoices.push({
              itemCode: item.itemCode, wHCode: item.whCode, tranDate: item.tranDate, tranNumber: item.tranNumber, tranUnit: item.tranUnit, tranQty: item.tranQty,
              unitConvFactor: item.unitConvFactor, tranTotQty: item.tranTotQty, tranPrice: item.tranPrice, itemAvgCost: item.itemAvgCost, tranRemarks: item.tranRemarks
            });
          });
          /*this.id = res.id;*/
        }
      })
    }
     
  }
  initialLoading() {
    this.loadList(0, this.pageService.pageCount, "", this.sortingOrder);
  }
  GenerateItemNumber() {
    this.apiService.getall(`Inventorymanagementlist/GenerateItemNumber`).subscribe(res => {
      if (res) {
        this.form.patchValue({
          itemCode: ""+res.itemCode+""
        });
        //this.form.patchValue(res);
      }
    });
  }
  loadVenCode() {
    this.apiService.getall('PurchaseOrder/GetVendorCodeSelectList').subscribe(res => {
      if (res) {
        this.VenCodeList = res;
      }
    })
  }
  loadWarehouses() {
    this.apiService.getall(`warehouse/getSelectWarehouseList`).subscribe(res => {
      if (res) {
        this.warehouseList = res;
      }
    });
  }
  loadType() {
    this.apiService.getall(`warehouse/GetSelectSysTypeList`).subscribe(res => {
      if (res) {
        this.ItemTypeLIst = res;
      }
    });
  }
  ontype(Value: string) {
    if (Value == "INV") {
      this.apiService.getall(`warehouse/GetSelectTrackingList?Code=${Value}`).subscribe(res => {
        if (res) {
          this.ItemTrackingList = res;
        }
      })
    }
  
  }
 
  loadCategoryCode() {
    this.apiService.getall('producthierarchy/GetCategorySelectList').subscribe(res => {
      if (res) {
        this.CategoryCodeList = res;
      }
    })
  }
  loadTaxCode() {
    this.apiService.getall('Inventorymanagementlist/GetTaxSelectList').subscribe(res => {
      if (res) {
        this.TaxCodeList = res;
      }
    })
  }
  //loadSubCategoryCode() {
  //  this.apiService.getall('Inventorymanagementlist/GetSelectSubCategoryList').subscribe(res => {
  //    if (res) {
       
  //      this.SubCategoryCodeList = res;
  //    }
  //  })
  //}
  loadClassCode() {
    this.apiService.getall('Inventorymanagementlist/GetSelectClass').subscribe(res => {
      if (res) {
        this.ClassCodeList = res;
      }
    })
  }
  loadSubClassCode() {
    this.apiService.getall('Inventorymanagementlist/getSelectSubClassList').subscribe(res => {
      if (res) {
        this.SubClassCodeList = res;
      }
    })
  }
  loadUOMCode() {
    this.apiService.getall('Inventorymanagementlist/GetUOMSelectList').subscribe(res => {
      if (res) {
        this.UomLIst = res;
      }
    })
  }
  onSubCategory(Value: string) {
    if (Value != null) {
      this.apiService.getall(`Inventorymanagementlist/GetSelectSubCategoryList?CategoryCode=${Value}`).subscribe(res => {
        if (res) {
          this.SubCategoryCodeList = res;
        }
      })
    }
   

    //this.apiService.getall('Inventorymanagementlist/GetSelectSubCategoryList?search=${val}').subscribe(res => {
    //  if (res) {
    //    this.form.patchValue(res);
    //    this.form.controls['itemCatName'].setValue(res.itemCatCode);
    //  }
    //})
  }
  //onSortOrder(sort: any) {
  //  this.totalItemsCount = 0;
  //  this.sortingOrder = sort.active + ' ' + sort.direction;
  //  this.loadUser(0, this.pageService.pageCount, "", this.sortingOrder);
  //}
  setForm() {
    let MOBILE_PATTERN = /[0-9\+\-\ ]/;
    this.form = this.fb.group({
      '': ['', Validators.required],
      /*itemList: this.fb.array([this.createAuthority()])//, Validators.required)*/
      'itemCode': '',
      'hSNCode': '',
      'itemDescription': '',
      'itemDescriptionAr': '',
      'shortName': '',
      'shortNameAr': '',
      'itemCat': ['', Validators.required],
      'itemSubCat': ['', Validators.required],
      'itemClass': ['', Validators.required],
      'itemSubClass': ['', Validators.required],
      'itemAvgCost': '',
      'itemStandardCost': '',
      'itemBaseUnit': ['', Validators.required],
      'itemPrimeVendor': '',
     /* 'itemStandardPrice1': ['', Validators.required],*/
      'itemStandardPrice1': ['', Validators.required],

      'itemStandardPrice2': ['', Validators.required],
      'itemStandardPrice3': ['', Validators.required],
      'itemType': '',
      'itemTracking': '',
      'itemWeight': '',
      'itemTaxCode': ['', Validators.required],
      'isActive': [false],
      'allowPriceOverride': [false],
      'allowDiscounts': [false],
      'allowQuantityOverride': [false],
      'ItemInventoryCodes': '',
      'ItemUOMCodes': '',
      'ItemBarcodeCodes': '',
      'ItemNotesCodes': '',
      'ItemHistoryCodes': '',
      authList: this.fb.array([this.createAuthority()]),
      inventoryList: this.fb.array([this.createInventory()]),
      barcodeList: this.fb.array([this.createBarcode()]),
      notesList: this.fb.array([this.createNotes()]),
      historyList: this.fb.array([this.createHistory()]),
      

    });

  }
  //onPageSwitch(event: PageEvent) {
  //  this.pageService.change(event);
  //  this.loadUser(event.pageIndex, event.pageSize, "", this.sortingOrder);
  //}
  private loadUser(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
    //this.apiService.getPagination('', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
    //  this.totalItemsCount = 0;
    //  //this.forecasts = result.items;
    this.data = new MatTableDataSource(this.getData());
    //  this.data = new MatTableDataSource(result.items);
    this.totalItemsCount = 2;
    //  //this.data.data = this.forecasts;
    //  this.data.paginator = this.paginator;
    //  this.data.sort = this.sort;

    //}, error => console.error(error));
  }
  submit() {
    alert('submit');
  }
  inventorysubmit() {
    if (this.id > 0)
      this.form.value['id'] = this.id;
    this.apiService.post('Inventorymanagementlist', this.form.value)
      .subscribe(res => {
       
        /*this.itemcode = res['itemcode'];*/
        if (res) {
          let output1 = res;
          Object.values(res);
          this.itemcode = Object.values(res)[0];
          
        }
       
        this.form.patchValue({
          ItemInventoryCodes: this.itemcode,
          ItemUOMCodes: this.itemcode,
          ItemBarcodeCodes: this.itemcode,
          ItemNotesCodes: this.itemcode,
          ItemHistoryCodes: this.itemcode,
          itemCode: this.itemcode
        });
       
        this.setInventoryForm(this.itemcode);
        this.setUOMForm(this.itemcode);
        this.setBarcodeForm(this.itemcode);
        this.utilService.OkMessage();
        //this.reset();
        //this.dialogRef.close(true);
      },
        error => {
          console.error(error);
          //this.utilService.ShowApiErrorMessage("Duplicate");
          this.utilService.ShowApiErrorMessage(error);
        });
  }

/*  UOMTab*/
  setUOMForm(itemvalue: any) {
    this.apiService.getall(`Inventorymanagementlist/GetUOMListByID?itemvalue=${itemvalue}`).subscribe(res => {
      if (res) {
        //this.authList.clear();
        //let authList = res['authList'] as Array<any>;
        //authList.forEach(item => {
        //  this.editItem(item);
        //});
        let listOfUOMinventory = res['authList'] as Array<any>;
        listOfUOMinventory.forEach(item => {
          //this.editInvoiceItem(item);
          this.listOfUOMinventory.push({
            sequence: this.getSequence(),
            tranNumber: "0",
            itemUOM: item.itemUOM,
            itemConvFactor: item.itemConvFactor,
            itemUOMPrice1: item.itemUOMPrice1,
            itemUOMPrice2: item.itemUOMPrice2,
            itemUOMPrice3: item.itemUOMPrice3,
            itemUOMDiscPer: item.itemUOMDiscPer,
            itemUOMPrice4: item.itemUOMPrice4,
            itemUomAvgCost: item.itemUomAvgCost,
            itemUomLastPOCost: item.itemUomLastPOCost,
            itemUomLandedCost: item.itemUomLandedCost

          });
        });
      }
    })
  }
  editItem(res: any) {
    this.authList.push(this.createAuthority(res));
  }
  createAuthority(res?: any): FormGroup {
    if (res) {
      return this.fb.group(res);
    }
    return this.fb.group({
      'itemUOM': ['', Validators.required],
      'itemConvFactor': ['', Validators.required],
      'itemUOMPrice1': ['', Validators.required],
      'itemUOMPrice2': ['', Validators.required],
      'itemUOMPrice3': ['', Validators.required],
      'itemUOMDiscPer': ['', Validators.required],
      'itemUOMPrice4': ['', Validators.required],
      'itemUomAvgCost': ['', Validators.required],
      'itemUomLastPOCost': ['', Validators.required],
      'itemUomLandedCost': ['', Validators.required],
    })
  }
  get authList(): FormArray {
    return <FormArray>this.form.get('authList');
  }
  addItem() {
    this.authList.push(this.createAuthority());
  }
  AddUOM() {
    if (this.id > 0)
      this.form.value['id'] = this.id;
    this.form.value['authList'] = this.listOfUOMinventory;
    this.apiService.post('Inventorymanagementlist/CreateUomItem', this.form.value)
      .subscribe(res => {
      
        this.setBarcodeForm(this.itemcode);
        this.utilService.OkMessage();
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });
  }
  removeItem(item: number) {
    this.authList.removeAt(item);
  }
  /*UOM Tab*/
  /*Item Inventory*/
  addItemInventory() {
    this.inventoryList.push(this.createInventory());
  }
  AddInventory() {
    if (this.id > 0)
      this.form.value['id'] = this.id;
    this.form.value['inventoryList'] = this.listOfinventory;
    this.apiService.post('Inventorymanagementlist/CreateInventoryItem', this.form.value)
      .subscribe(res => {
        debugger;
        this.utilService.OkMessage();
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });

   
  }
  removeItemInventory(item: number) {
    this.inventoryList.removeAt(item);
  }
  setInventoryForm(itemvalue: any) {
    this.apiService.getall(`Inventorymanagementlist/GetInventoryListByID?itemvalue=${itemvalue}`).subscribe(res => {
      if (res) {
      
        //this.inventoryList.clear();
        //let inventoryList = res['inventoryList'] as Array<any>;
        //inventoryList.forEach(item => {
        //  this.editInventory(item);
        //});

        let listOfinventory = res['inventoryList'] as Array<any>;
        listOfinventory.forEach(item => {
          //this.editInvoiceItem(item);
          this.listOfinventory.push({
            sequence: this.getSequence(),
            tranNumber: "0", whCode: item.whCode, qtyOH: item.qtyOH, qtyOnSalesOrder: item.qtyOnSalesOrder, qtyOnPO: item.qtyOnPO, qtyReserved: item.qtyReserved,
            invitemAvgCost: item.invItemAvgCost, itemLastPOCost: item.itemLastPOCost, itemLandedCost: item.itemLandedCost, minQty: item.minQty, maxQty: item.maxQty, eoq: item.eoq
          });
        });

      }
    })
  }
  editInventory(res: any) {
    this.inventoryList.push(this.createInventory(res));
  }
  createInventory(res?: any): FormGroup {
    if (res) {
      return this.fb.group(res);
    }
    return this.fb.group({
      'whCode': ['', Validators.required],
      'qtyOH': ['', Validators.required],
      'qtyOnSalesOrder': ['', Validators.required],
      'qtyOnPO': ['', Validators.required],
      'qtyReserved': ['', Validators.required],
      'invitemAvgCost': ['', Validators.required],
      'itemLastPOCost': ['', Validators.required],
      'itemLandedCost': ['', Validators.required],
      'minQty': ['', Validators.required],
      'maxQty': ['', Validators.required],
      'eoq': ['', Validators.required],

    })
  }
  get inventoryList(): FormArray {
    return <FormArray>this.form.get('inventoryList');
  }
  /* Item Inventory*/
  /*Item BarCode*/
  addItemBarcode() {
    this.barcodeList.push(this.createBarcode());
  }
  AddBarcode() {
  /*  alert('ItemInventory');*/
    if (this.id > 0)
      this.form.value['id'] = this.id;
    this.form.value['barcodeList'] = this.listOfBarcodeinventory;
    this.apiService.post('Inventorymanagementlist/CreateBarcodeItem', this.form.value)
      .subscribe(res => {
       
        this.utilService.OkMessage();
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });
  }
  removeItemBarcode(item: number) {
    this.barcodeList.removeAt(item);
  }
  //setBarcodeForm(itemvalue: any) {
  //  this.apiService.getall(`Inventorymanagementlist/GetInventoryListByID?itemvalue=${itemvalue}`).subscribe(res => {
  //    if (res) {
  //      debugger;
  //      this.barcodeList.clear();
  //      let barcodeList = res['barcodeList'] as Array<any>;
  //      barcodeList.forEach(item => {
  //        this.editBarcode(item);
  //      });
  //    }
  //  })
  //}
  editBarcode(res: any) {
    this.barcodeList.push(this.createBarcode(res));
  }
  createBarcode(res?: any): FormGroup {
    if (res) {
      return this.fb.group(res);
    }
    return this.fb.group({
      'itemBarcode': ['', Validators.required],
      'itemUOM': ['', Validators.required],

    })
  }
  get barcodeList(): FormArray {
    return <FormArray>this.form.get('barcodeList');
  }
  setBarcodeForm(itemvalue: any) {
    this.apiService.getall(`Inventorymanagementlist/GetBarcodeListByID?itemvalue=${itemvalue}`).subscribe(res => {
      if (res) {
        
        //this.barcodeList.clear();
        //let barcodeList = res['barCodeList'] as Array<any>;
        //barcodeList.forEach(item => {
        //  this.editBarcode(item);
        //});
        let listOfBarcodeinventory = res['barCodeList'] as Array<any>;
        listOfBarcodeinventory.forEach(item => {
          //this.editInvoiceItem(item);
          this.listOfBarcodeinventory.push({
            sequence: this.getSequence(),
            tranNumber: "0",
            itemBarUOM: item.itemBarUOM ,
            itemBarcode: item.itemBarcode 
          });
        });
      }
    })
  }
  onBarcodechange(Value: string) {
    if (Value != null) {
      this.apiService.getall(`Inventorymanagementlist/GetBarcode?Barcode=${Value}`).subscribe(res => {
        if (res) {
          this.notifyService.showError('Duplicate Barcode...');
          
        }
      })
    }
  }
  /* Item BarCode*/
  /*Item Notes*/
  addItemNotes() {
    this.notesList.push(this.createNotes());
  }
  AddNotes() {
    //alert('ItemInventory');
    if (this.id > 0)
      this.form.value['id'] = this.id;
    this.form.value['notesList'] = this.listOfNotesinventory;
    this.apiService.post('Inventorymanagementlist/CreateNotesItem', this.form.value)
      .subscribe(res => {
       
        this.utilService.OkMessage();
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });
  }
  removeItemNotes(item: number) {
    this.notesList.removeAt(item);
  }
  //setNotesForm(itemvalue: any) {
  //  this.apiService.getall(`Inventorymanagementlist/GetInventoryListByID?itemvalue=${itemvalue}`).subscribe(res => {
  //    if (res) {
  //      debugger;
  //      this.notesList.clear();
  //      let notesList = res['notesList'] as Array<any>;
  //      notesList.forEach(item => {
  //        this.editNotes(item);
  //      });
  //    }
  //  })
  //}
  editNotes(res: any) {
    this.notesList.push(this.createNotes(res));
  }
  createNotes(res?: any): FormGroup {
    if (res) {
      return this.fb.group(res);
    }
    return this.fb.group({
      'noteDates': ['', Validators.required],
      'name': ['', Validators.required],
      'notes': ['', Validators.required],
      


    })
  }
  get notesList(): FormArray {
    return <FormArray>this.form.get('notesList');
  }
  /* Item BarCode*/
  /*Item History*/
  addItemHistory() {
    this.historyList.push(this.createHistory());
  }
  AddHistory() {
    //alert('ItemInventory');
    if (this.id > 0)
      this.form.value['id'] = this.id;
    this.apiService.post('Inventorymanagementlist/CreateItemHistory', this.form.value)
      .subscribe(res => {
       
        this.utilService.OkMessage();
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });
  }
  removeItemHistory(item: number) {
    this.historyList.removeAt(item);
  }
 
  editHistory(res: any) {
    this.historyList.push(this.createHistory(res));
  }
  createHistory(res?: any): FormGroup {
    if (res) {
      return this.fb.group(res);
    }
    return this.fb.group({
      'whCode': ['', Validators.required],
      'tranDate': ['', Validators.required],
      'tranType': ['', Validators.required],
      'tranNumber': ['', Validators.required],
      'tranUnit': ['', Validators.required],
      'tranQty': ['', Validators.required],
      'unitConvFactor': ['', Validators.required],
      'tranTotQty': ['', Validators.required],
      'tranPrice': ['', Validators.required],
      'itemAvgCost': ['', Validators.required],
      'tranRemarks': ['', Validators.required],
    })
  }
  get historyList(): FormArray {
    return <FormArray>this.form.get('historyList');
  }
  /* Item History*/

  getData(): Array<any> {
    let data: Array<any> = [
      { "ItemCode": "0110101", "h_code": "012121011", "shortname": "shortname ", "itemdescription": "Appearls ", "category": "Appearls ", "itemavgcost": "$1200 ", "itemstandardcost": "$1400 ", "id": 1 },
      { "ItemCode": "0110101", "h_code": "012121011", "shortname": "shortname ", "itemdescription": "Appearls ", "category": "Appearls ", "itemavgcost": "$1200 ", "itemstandardcost": "$1400 ", "id": 2 }
    ]
    return data;
  }
  /*  Inventory Table*/
  onSortOrder(sort: any) {


    this.sortingOrder = sort.active + ' ' + sort.direction;
    this.totalItemsCount = 0;
    this.loadList(0, this.pageService.pageCount, "", this.sortingOrder);
  }


  onPageSwitch(event: PageEvent) {
    this.pageService.change(event);
    this.loadList(event.pageIndex, event.pageSize, "", this.sortingOrder);
  }

  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
    this.isLoading = true;

    this.apiService.getPagination('Inventorymanagementlist', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
      this.totalItemsCount = 0;
      this.data = new MatTableDataSource(result.items);

      this.totalItemsCount = result.totalCount;
      setTimeout(() => {
        this.paginator.pageIndex = page as number;
        this.paginator.length = this.totalItemsCount;
      });
      this.data.sort = this.sort;

      //console.log(this.data.sort)
      //console.log(this.data.paginator)

      this.isLoading = false;
    }, error => this.utilService.ShowApiErrorMessage(error));
  }


  applyFilter(searchVal: any) {
    const search = searchVal;//.target.value as string;
    //if (search && search.length >= 3) {
    if (search) {
      this.searchValue = search;
      this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
    }
  }
  getSequence(): number { return this.sequence += this.sequence + 1 };

  //public edit(id: number) {
  //  /*this.openDialogManage(id, DBOperation.update, 'Updating Warehouse', 'Update');*/
  //  this.apiService.get('Inventorymanagementlist', id).subscribe(res => {
  //    if (res) {
  //      this.isReadOnly = true;
  //     /* this.loadSubCategoryCode();*/
  //      this.onSubCategory(res.itemCat)
  //      this.ontype(res.itemType)
  //      this.form.patchValue({
  //        ItemInventoryCodes: res.itemCode,
  //        ItemUOMCodes: res.itemCode,
  //        ItemBarcodeCodes: res.itemCode,
  //        ItemNotesCodes: res.itemCode,
  //        ItemHistoryCodes: res.itemCode

  //      });
  //      this.form.patchValue(res);
  //      //this.loadinventoryHistory(res.itemCode);

  //      //this.inventoryList.clear();
  //      //let inventoryList = res['inventoryList'] as Array<any>;
  //      //inventoryList.forEach(item => {
  //      //  this.editInventory(item);
  //      //});
       

  //      /*this.listOfInvoices.clear();*/
  //      let listOfinventory = res['inventoryList'] as Array<any>;
  //      listOfinventory.forEach(item => {
  //        //this.editInvoiceItem(item);
  //        this.listOfinventory.push({
  //          sequence: this.getSequence(),
  //          tranNumber: "0", whCode: item.whCode, qtyOH: item.qtyOH, qtyOnSalesOrder: item.qtyOnSalesOrder, qtyOnPO: item.qtyOnPO, qtyReserved: item.qtyReserved,
  //          invitemAvgCost: item.invItemAvgCost, itemLastPOCost: item.itemLastPOCost, itemLandedCost: item.itemLandedCost, minQty: item.minQty, maxQty: item.maxQty, eoq: item.eoq
  //        });
  //      });

  //      //this.authList.clear();
  //      //let authList = res['authList'] as Array<any>;
  //      //authList.forEach(item => {
  //      //  this.editItem(item);
  //      //});
  //      let listOfUOMinventory = res['authList'] as Array<any>;
  //      listOfUOMinventory.forEach(item => {
  //        //this.editInvoiceItem(item);
  //        this.listOfUOMinventory.push({
  //          sequence: this.getSequence(),
  //          tranNumber: "0",
  //          itemUOM : item.itemUOM,
  //          itemConvFactor : item.itemConvFactor,
  //          itemUOMPrice1 : item.itemUOMPrice1,
  //          itemUOMPrice2 : item.itemUOMPrice2,
  //          itemUOMPrice3 : item.itemUOMPrice3,
  //          itemUOMDiscPer : item.itemUOMDiscPer,
  //          itemUOMPrice4 : item.itemUOMPrice4,
  //          itemUomAvgCost : item.itemUomAvgCost,
  //          itemUomLastPOCost : item.itemUomLastPOCost,
  //          itemUomLandedCost : item.itemUomLandedCost

  //        });
  //      });

  //      //this.barcodeList.clear();
  //      //let barcodeList = res['barcodeList'] as Array<any>;
  //      //barcodeList.forEach(item => {
  //      //  this.editBarcode(item);
  //      //});
  //      let listOfBarcodeinventory = res['barcodeList'] as Array<any>;
  //      listOfBarcodeinventory.forEach(item => {
  //        //this.editInvoiceItem(item);
  //        this.listOfBarcodeinventory.push({
  //          sequence: this.getSequence(),
  //          tranNumber: "0",
  //          itemBarUOM: item.itemBarUOM,
  //          itemBarcode: item.itemBarcode
  //        });
  //      });

  //      //this.notesList.clear();
  //      //let notesList = res['notesList'] as Array<any>;
  //      //notesList.forEach(item => {
  //      //  this.editNotes(item);
  //      //});
  //      let listOfNotesinventory = res['notesList'] as Array<any>;
  //      listOfNotesinventory.forEach(item => {
  //        //this.editInvoiceItem(item);
  //        this.listOfNotesinventory.push({
  //          sequence: this.getSequence(),
  //          tranNumber: "0",
  //          noteDates: item.noteDates,
  //          name: item.name,
  //          notes: item.notes

  //        });
  //      });
  //      //this.historyList.clear();
  //      //let historyList = res['historyList'] as Array<any>;
  //      //historyList.forEach(item => {
  //      //  this.editHistory(item);
  //      //});
  //      let listOfInvoices = res['historyList'] as Array<any>;
  //      listOfInvoices.forEach(item => {
  //        //this.editInvoiceItem(item);
  //        this.listOfInvoices.push({
  //          itemCode: item.itemCode, wHCode: item.whCode, tranDate: item.tranDate, tranNumber: item.tranNumber, tranUnit: item.tranUnit, tranQty: item.tranQty,
  //          unitConvFactor: item.unitConvFactor, tranTotQty: item.tranTotQty, tranPrice: item.tranPrice, itemAvgCost: item.itemAvgCost, tranRemarks: item.tranRemarks
  //        });
  //      });

  //      //this.listOfInvoices = [];
  //      //let listOfInvoices = res['historyList'] as Array<any>;
  //      //listOfInvoices.forEach(item => {
  //      //  this.editHistory(item);
  //      //});

  //      this.id = res.id;
  //    }
  //  })
  //}

  public delete(id: number) {
    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete && id > 0) {
        this.apiService.delete('Inventorymanagementlist', id).subscribe(res => {
          this.refresh();
          this.utilService.OkMessage();
        },
        );
      }
    },
      error => this.utilService.ShowApiErrorMessage(error));
  }
  refresh() {
    this.searchValue = '';
    this.initialLoading();
  }
    closeModel() {
     /* this.refresh();*/
      this.ngOnInit();
      this.listOfNotesinventory = [];
      this.listOfBarcodeinventory = [];
      this.listOfUOMinventory = [];
      this.listOfInvoices = [];
  }
  onTextchange(Value: string) {
    if (Value != null) {
      this.apiService.getall(`Inventorymanagementlist/GetInventoryItems?ItemCode=${Value}`).subscribe(res => {
        if (res) {
          this.isReadOnly = true;
        
          this.onSubCategory(res.itemCat)
          this.form.patchValue({
            ItemInventoryCodes: res.itemCode,
            ItemUOMCodes: res.itemCode,
            ItemBarcodeCodes: res.itemCode,
            ItemNotesCodes: res.itemCode,
            ItemHistoryCodes: res.itemCode

          });
          this.form.patchValue(res);

          this.inventoryList.clear();
          let inventoryList = res['inventoryList'] as Array<any>;
          inventoryList.forEach(item => {
            this.editInventory(item);
          });

          this.authList.clear();
          let authList = res['authList'] as Array<any>;
          authList.forEach(item => {
            this.editItem(item);
          });

          this.barcodeList.clear();
          let barcodeList = res['barcodeList'] as Array<any>;
          barcodeList.forEach(item => {
            this.editBarcode(item);
          });

          this.notesList.clear();
          let notesList = res['notesList'] as Array<any>;
          notesList.forEach(item => {
            this.editNotes(item);
          });

          this.historyList.clear();
          let historyList = res['historyList'] as Array<any>;
          historyList.forEach(item => {
            this.editHistory(item);
          });
          this.id = res.id;
        }
      })
    }
  }

  Close() {
    this.refresh();
  }
  ErrorMessage() { this.notifyService.showError(this.isArabic() ? 'املأ جميع الحقول' : 'Cannot Delete The  Item '); }
  ErrorMesg() { this.notifyService.showError(this.isArabic() ? 'املأ جميع الحقول' : 'Duplicate Item '); }
  isArabic() {
    return this.selectedLanguage() == 'ar';
  }
  selectedLanguage() {
    return localStorage.getItem('language') ?? 'en-US'
  }
  deleteInventoryItem(item: any) {
    if (item.qtyOH > 0) {
      this.ErrorMessage();
    }
    else
    this.removeInventoryList(item.sequence);
  }

  removeInventoryList(sequence: number) {
    let index: number = this.listOfinventory.findIndex(a => a.sequence === sequence);
    this.listOfinventory.splice(index, 1);
  }

  editInventoryItem(item: any) {
    this.editsequence = item.sequence,
    this.whCode = item.whCode,
      this.qtyOH = item.qtyOH,
      this.qtyOnSalesOrder = item.qtyOnSalesOrder,
      this.qtyOnPO = item.qtyOnPO,
      this.qtyReserved = item.qtyReserved,
      this.invitemAvgCost = item.invitemAvgCost,
      this.itemLastPOCost = item.itemLastPOCost,
      this.itemLandedCost = item.itemLandedCost,
      this.minQty = item.minQty,
      this.maxQty = item.maxQty,
      this.eoq = item.eoq;
  }
  additem_inv() {
      if (this.editsequence > 0) {
        this.removeInventoryList(this.editsequence);
        this.editsequence = 0;
      }
    this.listOfinventory.push({
        sequence: this.getSequence(),

        tranNumber: "0", whCode: this.whCode, qtyOH: this.qtyOH, qtyOnSalesOrder: this.qtyOnSalesOrder, qtyOnPO: this.qtyOnPO, qtyReserved: this.qtyReserved,
        invitemAvgCost: this.invitemAvgCost, itemLastPOCost: this.itemLastPOCost, itemLandedCost: this.itemLandedCost, minQty: this.minQty, maxQty: this.maxQty, eoq: this.eoq
      });
      this.setToDefault();
  }
  
  setToDefault() {
    this.whCode = '',
      this.qtyOH = 0,
      this.qtyOnSalesOrder = 0,
      this.qtyOnPO = 0,
      this.qtyReserved = 0,
      this.invitemAvgCost = 0,
      this.itemLastPOCost = 0,
      this.itemLandedCost = 0,
      this.minQty = 0,
      this.maxQty = 0,
      this.eoq = 0;
  }

  deleteInventoryUOMItem(item: any) {
    this.removeInventoryUOMList(item.sequence);
  }

  removeInventoryUOMList(sequence: number) {
    let index: number = this.listOfUOMinventory.findIndex(a => a.sequence === sequence);
    this.listOfUOMinventory.splice(index, 1);
  }

  editInventoryUOMItem(item: any) {
    this.editInventoryUOMItemsequence = item.sequence,
    this.itemUOM = item.itemUOM,
      this.itemConvFactor = item.itemConvFactor,
      this.itemUOMPrice1 = item.itemUOMPrice1,
      this.itemUOMPrice2 = item.itemUOMPrice2,
      this.itemUOMPrice3 = item.itemUOMPrice3,
      this.itemUOMDiscPer = item.itemUOMDiscPer,
      this.itemUOMPrice4 = item.itemUOMPrice4,
      this.itemUomAvgCost = item.itemUomAvgCost,
      this.itemUomLastPOCost = item.itemUomLastPOCost,
      this.itemUomLandedCost = item.itemUomLandedCost;
  }
  additem_invUOM() {
    if (this.editInventoryUOMItemsequence > 0) {
      this.removeInventoryUOMList(this.editInventoryUOMItemsequence);
      this.editInventoryUOMItemsequence = 0;
    }
    //if (this.listOfUOMinventory[0].itemUOM = this.itemUOM) {
    //  this.ErrorMessage();
    //  return;
    //}
    for (let i = 0; i < this.listOfUOMinventory.length; i++) {
      if(this.listOfUOMinventory[i].itemUOM == this.itemUOM)
      {
        this.ErrorMesg();
        return;
      }
    }
    this.listOfUOMinventory.push({
      sequence: this.getSequence(),
      tranNumber: "0",
      itemUOM: this.itemUOM,
      itemConvFactor: this.itemConvFactor,
      itemUOMPrice1: this.itemUOMPrice1,
      itemUOMPrice2: this.itemUOMPrice2,
      itemUOMPrice3: this.itemUOMPrice3,
      itemUOMDiscPer: this.itemUOMDiscPer,
      itemUOMPrice4: this.itemUOMPrice4,
      itemUomAvgCost: this.itemUomAvgCost,
      itemUomLastPOCost: this.itemUomLastPOCost,
      itemUomLandedCost: this.itemUomLandedCost
    });
    this.setToDefaultUOM();
  }

  setToDefaultUOM() {
    this.itemUOM = '',
      this.itemConvFactor = 0,
      this.itemUOMPrice1 = 0,
      this.itemUOMPrice2 = 0,
      this.itemUOMPrice3 = 0,
      this.itemUOMDiscPer = 0,
      this.itemUOMPrice4 = 0,
      this.itemUomAvgCost = 0,
      this.itemUomLastPOCost = 0,
      this.itemUomLandedCost = 0;
      
      
  }


  deleteInventoryBarcodeItem(item: any) {
    this.removeInventoryBarcodeList(item.sequence);
  }

  removeInventoryBarcodeList(sequence: number) {
    let index: number = this.listOfBarcodeinventory.findIndex(a => a.sequence === sequence);
    this.listOfBarcodeinventory.splice(index, 1);
  }

  editInventoryBarcodeItem(item: any) {
    this.editBarcodesequence = item.sequence,
    this.itemBarUOM = item.itemBarUOM,
      this.itemBarcode = item.itemBarcode;
  }
  additem_invBarcode() {
    if (this.editBarcodesequence > 0) {
      this.removeInventoryBarcodeList(this.editBarcodesequence);
      this.editBarcodesequence = 0;
    }
    this.listOfBarcodeinventory.push({
      sequence: this.getSequence(),
      tranNumber: "0",
      itemBarUOM: this.itemBarUOM,
      itemBarcode: this.itemBarcode
     
    });
    this.setToDefaultBarcode();
  }

  setToDefaultBarcode() {
    this.itemBarUOM = '',
      this.itemBarcode = '';
      

  }

  deleteInventoryNotesItem(item: any) {
    this.removeInventoryNotesList(item.sequence);
  }

  removeInventoryNotesList(sequence: number) {
    let index: number = this.listOfNotesinventory.findIndex(a => a.sequence === sequence);
    this.listOfNotesinventory.splice(index, 1);
  }

  editInventoryNotesItem(item: any) {
    this.editsequence = item.sequence,
    this.noteDates = item.noteDates,
      this.name = item.name,
      this.notes = item.notes;
  }
  additem_invNotes() {
    if (this.editsequence > 0) {
      this.deleteInventoryNotesItem(this.editsequence);
      this.editsequence = 0;
    }
    this.listOfNotesinventory.push({
      sequence: this.getSequence(),
      tranNumber: "0",
      noteDates: this.noteDates,
      name: this.name,
      notes: this.notes


    });
    this.setToDefaultNotes();
  }

  setToDefaultNotes() {
    this.noteDates = '',
      this.name = '';
    this.notes = '';

  }
  public create() {
    this.openDialogManage(0, DBOperation.create, this.translate.instant('Create_New_Issues'), '', AddupdateinventorymanagementlistComponent);
  }

  public edit(id: number) {
    this.openDialogManage(id, DBOperation.update, this.translate.instant('Create_New_Issues'), '', AddupdateinventorymanagementlistComponent);
  }

  public view(id: number) {
    this.openDialogManage(id, DBOperation.update, this.translate.instant('Create_New_Issues'), '', AddupdateinventorymanagementlistComponent);
  }
  private openApprovalDialog(branchCode: string, serviceCode: string, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, serviceType: string, Component: any) {
    let dialogRef = this.openApprovalDialog1(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).serviceType = serviceType;
    (dialogRef.componentInstance as any).serviceCode = serviceCode;
    (dialogRef.componentInstance as any).branchCode = branchCode;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
  openApprovalDialog1(dialog: MatDialog, component: any) {
    const dialogRef = dialog.open(component, {
      disableClose: true,
    });
    return dialogRef;
  }
  private openDialogManage<T>(id: number = 0, dbops: DBOperation, modalTitle: string = '', modalBtnTitle: string = '', component: T, moduleFile: MultiFileUploadDto = { module: '00', action: '00act' }, width: number = 100) {
    let dialogRef = this.utilService.openDialogCongif(this.dialog, component, width);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).id = id;
    (dialogRef.componentInstance as any).moduleFile = moduleFile;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }
}
