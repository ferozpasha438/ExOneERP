import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { CustomSelectListItem, LanCustomSelectListItem } from '../../../../models/MenuItemListDto';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { UtilityService } from '../../../../services/utility.service';
import { ValidationService } from '../../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-addupdatecashvoucher',
  templateUrl: './addupdatecashvoucher.component.html',
  styles: [
  ]
})
export class AddupdatecashvoucherComponent implements OnInit {

  form: FormGroup;
  id: number = 0;
  isEdit: boolean = false;
  customerSiteList: Array<LanCustomSelectListItem> = [];
  customerList: Array<LanCustomSelectListItem> = [];
  companyList: Array<CustomSelectListItem> = [];
  branchList: Array<CustomSelectListItem> = [];
  //itemsBranchList: Array<CustomSelectListItem> = [];
  accountCodeList: Array<CustomSelectListItem> = [];
  segmentSetupList: Array<CustomSelectListItem> = [];
  segmentTwoSetupList: Array<CustomSelectListItem> = [];
  costAllocationList: Array<LanCustomSelectListItem> = [];
  costSegCodeList: Array<LanCustomSelectListItem> = [];
  batchSetupList: Array<CustomSelectListItem> = [];

  isSeg1: boolean = false;
  isSeg2: boolean = false;


  payCodeList: Array<CustomSelectListItem> = [];
  itemList: Array<any> = [];
  isPaymentType: boolean = false;
  isArab: boolean = false;
  isCodeSegmentReadonly: boolean = false;
  isEditingRecord: boolean = false;

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdatecashvoucherComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {

  }
  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.setForm();
    this.loadData();

    if (this.id > 0) {
      this.isEdit = true;
      this.setEditForm();
    }
  }


  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      'jvDate': [this.utilService.getCurrentDate(), Validators.required],
      'amount': ['', Validators.required],
      'customerId': [''],
      'siteCode': [''],
      'companyId': ['', Validators.required],
      'branchCode': ['', Validators.required],
      'docNum': ['', Validators.required],
      'batch': '',//['', Validators.required],
      'cBookCode': ['', Validators.required],
      'voucherType': ['', Validators.required],
      'remarks': [''],
      'narration': ['']
    });
  }

  hasBranchHidden: boolean = false;
  branchName: string = '';
  branchCode: string = '';
  finAcCode: string = '';
  description: string = '';
  remarks: string = '';
  batch: string = '';
  batch2: string = '';
  costAllocation: number = 0;
  costAllocationName: string = '';
  costSegCode: string = '';
  costSegCodeName: string = '';
  payment: number = 0;

  sequence: number = 1;
  editsequence: number = 0;


  loadData() {

    this.apiService.getall("customer/getSelectLanCustomerList").subscribe(res => {
      if (res) {
        this.customerList = res;
        const newList = this.customerList.map((i) => {
          i.text = !this.isArab ? i.text : i.textAr;
          return i;
        });
      }
    });


    this.apiService.getall("mainAccounts/financesetup").subscribe(res => {
      if (res) {
        const nubOfSeg = res['numOfSeg'] as number;

        this.isSeg1 = nubOfSeg == 1;
        this.isSeg2 = nubOfSeg == 2;

        if (this.isSeg1 || this.isSeg2) {
          this.apiService.getall("segmentSetup/getSegmentSetupSelectList").subscribe(res => {
            if (res)
              this.segmentSetupList = res;
          });
        }

        if (this.isSeg2) {
          this.apiService.getall("segmentTwoSetup/getSegmentTwoSetupSelectList").subscribe(res => {
            if (res)
              this.segmentTwoSetupList = res;
          });
        }

      }

    });

    this.apiService.getall("costAllocationSetup/getCostAllocationSetupSelectList").subscribe(res => {
      if (res)
        this.costAllocationList = res;
    });

    this.apiService.getall("batchSetup/getBatchSetupSelectList").subscribe(res => {
      if (res)
        this.batchSetupList = res;
    });

    this.apiService.getall("paymentcodes/getSelectPaymentCashCodeList").subscribe(res => {
      if (res) {
        console.log( res);
        this.payCodeList = res;
      }
    });

    this.apiService.getall("company/getSelectCompanyList").subscribe(res => {
      if (res)
        this.companyList = res;
    });

    //this.apiService.getall("branch/getSelectBranchNameCodeList").subscribe(res => {
    //  if (res) {
    //    this.itemsBranchList = res;
    //    if (this.itemsBranchList.length == 1) {
    //      this.loadAccountCodesForBranch(this.itemsBranchList[0].value);
    //    }
    //  }
    //});


  }

  hasBranchHiddenChecked(event: MatSlideToggleChange) {
    this.hasBranchHidden = event.checked;
  }


  loadSegCodedata(event: any) {
    const id: number = event.value as number;
    this.costAllocationName = event.text
    if (id > 0) {
      this.costSegCode = '';
      this.costSegCodeName = '';
      this.apiService.getall(`costAllocationSetup/getCostSegmentCodeSelectList/${id}`).subscribe(res => {
        if (res)
          this.costSegCodeList = res;

        const newList = this.costSegCodeList.map((i) => {
          i.text = !this.isArab ? i.text + ' ' + i.textTwo : i.textTwo + ' ' + i.textAr;
          return i;
        });

        this.costSegCode = event.custCode;

      });
    }
  }

  selectSegCodedata(event: any) {
    this.costSegCodeName = event.text
  }


  loadBranchs(event: any) {
    let comId = event.target.value;
    if (comId) {
      this.apiService.getall(`branch/getSelectSysBranchListByComId/${comId}`).subscribe(res => {
        if (res)
          this.branchList = res;

        if (this.branchList.length == 1) {
          this.loadAccountCodesForBranch(this.branchList[0].value);
        }
      });
    }
  }

  selectBranch(evt: any) {
    this.branchCode = evt.target.value;
    this.loadAccountCodesForBranch(this.branchCode);
  }


  loadAccountCodes(event: any) {
    let branchCode = event.target.value;
    this.loadAccountCodesForBranch(branchCode);
  }


  loadAccountCodesForBranch(branchCode: any) {
    if (branchCode) {
      this.finAcCode = '';
      this.apiService.getall(`accountsbranchmapping/getSelectAccountMappingList?branchCode=${branchCode}`).subscribe(res => {
        if (res)
          this.accountCodeList = res;

      });
    }
  }

  setEditForm() {
    this.apiService.get('cashVoucher', this.id).subscribe(res => {
      if (res) {
        this.loadBranchsForCompany(res['companyId'] as number, res);

        const items = res['itemList'] as Array<any>;
        this.displayJvItems(items);

        if (res['siteCode']) {
          this.isEditingRecord = true;
          this.customerSiteList = [{ text: res['siteCode'], value: res['siteCode'], textAr: '', textTwo: '' }];
        }

        this.form.patchValue(res)
      }
    })
  }

  displayJvItems(items: Array<any>) {
    items.forEach(item => {
      this.branchName = item.branchName;
      this.branchCode = item.branchCode,
        this.description = item.description;
      this.finAcCode = item.finAcCode;
      this.remarks = item.remarks;
      this.batch = item.batch;
      this.batch2 = item.batch2;
      this.costAllocation = item.costAllocation;
      this.costAllocationName = item.costAllocation ? (this.costAllocationList.find(citem => citem.value == item.costAllocation) as any)?.text : '';
      this.costSegCode = item.costSegCode;
      this.costSegCodeName = item.costSegCode;
      this.payment = item.payment;
      this.fillITems();
    });
  }

  //edtiItemsList: Array<any> = [];

  //laodAndEditItem(res: any, result: any) {
  //  this.edtiItemsList = [];
  //  this.loadAccountCodeForBranch(res['branchCode'], res['id'] as number, result);
  //  this.itemList.push(this.createAuthority(res));
  //}

  loadBranchsForCompany(comId: number, result: any) {
    if (comId) {
      this.apiService.getall(`branch/getSelectSysBranchListByComId/${comId}`).subscribe(res => {
        if (res) {
          this.branchList = res;
        }
      });
    }
  }

  loadAccountCodeForBranch(branchCode: string, finAcCode: string) {
    if (branchCode) {
      this.apiService.getall(`accountsbranchmapping/getSelectAccountMappingList?branchCode=${branchCode}`).subscribe(res => {
        if (res) {
          this.accountCodeList = res;
          this.finAcCode = finAcCode;
        }
      });
    }
  }

  fillITems() {
    this.itemList.push({
      sequence: this.getSequence(),
      branchName: this.branchName,
      branchCode: this.branchCode, finAcCode: this.finAcCode, description: this.description,
      remarks: this.remarks, batch: this.batch, batch2: this.batch2,
      costAllocation: this.costAllocation, costSegCode: this.costSegCode,
      costAllocationName: this.costAllocationName,
      costSegCodeName: this.costSegCodeName,
      payment: this.payment
    });
    this.setToDefault();
  }

  addItem() {

    if (this.payment > 0 ) {
      if (this.editsequence > 0) {
        this.removeInvoiceList(this.editsequence);
        //let index: number = this.listOfInvoices.findIndex(a => a.sequence === this.editsequence);
        //this.listOfInvoices.splice(index, 1);
        this.editsequence = 0;
      }
      this.itemList.push({
        sequence: this.getSequence(),
        branchName: this.getBranchName(this.branchCode),
        branchCode: this.branchCode, finAcCode: this.finAcCode, description: this.description,
        remarks: this.remarks, batch: this.batch, batch2: this.batch2,
        costAllocation: this.costAllocation, costSegCode: this.costSegCode,
        costAllocationName: this.costAllocationName, costSegCodeName: this.costSegCodeName,
        payment: this.payment,
      });
      //this.setLabelPrices(this.total, this.vatAmount, '');     
      this.setToDefault();
    }
  }

  getSequence(): number { return this.sequence += this.sequence + 1 };

  deleteInvoiceItem(item: any) {
    this.removeInvoiceList(item.sequence);
  }

  removeInvoiceList(sequence: number) {
    let index: number = this.itemList.findIndex(a => a.sequence === sequence);
    this.itemList.splice(index, 1);
  }
  getBranchName(branchCode: string) {
    return this.branchList.find(a => a.value === branchCode)?.text;
    //return this.itemsBranchList.find(a => a.value === branchCode)?.text;
  }

  editInvoiceItem(item: any) {
    this.editsequence = item.sequence,

      this.branchCode = item.branchCode;

    this.loadAccountCodeForBranch(this.branchCode, item.finAcCode);
    this.description = item.description;
    this.remarks = item.remarks;
    this.batch = item.batch;
    this.batch2 = item.batch2;
    this.costAllocation = item.costAllocation;
    this.costAllocationName = item.costAllocationName;
    this.costSegCode = item.costSegCode;
    this.costSegCodeName = item.costSegCodeName;
    this.payment = item.payment;
  }

  setToDefault() {
    this.editsequence = 0;

    if (this.hasBranchHidden)
      this.branchCode = '';

    this.finAcCode = '';
    this.description = '';
    this.remarks = '';
    this.batch = '';
    this.batch2 = '';

    if (!this.isCodeSegmentReadonly) {
      this.costAllocation = 0;
      this.costAllocationName = '';
      this.costSegCode = '';
      this.costSegCodeName = '';
    }

    this.payment = 0;
   
  }

  selectVoucherType(evt: any) {
    const value = evt.target.value;
    if (value !== '')
      this.isPaymentType = evt.target.value == 'Payment' ? true : false;
  }


  resetCodeSegmentReadonly() {
    this.isCodeSegmentReadonly = false;
    this.costSegCode = '';
    this.costAllocation = 0;
    this.form.controls['siteCode'].setValue(null);
  }

  customerChange(event: any) {


    let custId = event.value, custCode = event.textTwo;
    this.form.controls['siteCode'].setValue(null);
    this.isCodeSegmentReadonly = true;

    this.costAllocation = (this.costAllocationList.find(item => item.textTwo.toLowerCase() == 'customer') as any)?.value;
    this.loadSegCodedata({ value: this.costAllocation, custCode: custCode });
    this.apiService.getall(`customer/getCustomerSitesSelectList/${custId}`).subscribe(res => {
      if (res) {
        this.customerSiteList = res;

        this.customerSiteList.map((i) => {
          i.text = !this.isArab ? i.text : i.textTwo;
          return i;
        });
      }
    });


  }

  submit() {

    if (this.itemList.length <= 0) {
      this.utilService.FillUpFields();
      return;
    }


    if (this.form.valid) {

      if (!this.hasBranchHidden) {
        this.itemList.forEach(item => {
          item.branchCode = this.form.value['branchCode'];
        });
      }

      this.form.value['jvDate'] = this.utilService.selectedDateTime(this.form.controls['jvDate'].value);

      if (this.id > 0)
        this.form.value['id'] = this.id;

      this.form.value['itemList'] = this.itemList;
      this.apiService.post('cashVoucher', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.reset();
          this.dialogRef.close(true);
        },
          error => {
            this.utilService.ShowApiErrorMessage(error);
          });
    }
    else
      this.utilService.FillUpFields();
  }

  reset() {
    this.form.controls['jvDate'].setValue('');
    this.form.controls['companyId'].setValue('');
    this.form.controls['branchCode'].setValue('');
  }


  closeModel() {
    this.dialogRef.close();
  }

}
