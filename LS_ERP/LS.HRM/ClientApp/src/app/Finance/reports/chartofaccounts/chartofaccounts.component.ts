import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { argThresholdOpts } from 'moment';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CustomSelectListItem, LanCustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { ParentFinMgtComponent } from '../../../sharedcomponent/parentfinmgt.component';
import { ParentSystemSetupComponent } from '../../../sharedcomponent/parentsystemsetup.component';


@Component({
  selector: 'app-chartofaccounts',
  templateUrl: './chartofaccounts.component.html',
  styles: [
    `.cu-pointer {cursor: pointer;}`
  ],
})
export class ChartofaccountsComponent extends ParentFinMgtComponent implements OnInit {

  listOfCategories: Array<any> = [];
  listOfCategoriesNew: Array<any> = [];

  listOfLedger: any
  company: any
  listOfBranchLedgers: Array<any> = [];
  fInSysGenAcCode: boolean = false;
  isLoading: boolean = false;
  isLoading1: boolean = false;

  isCategory: boolean = false;
  finAcNameHeader: string = '';

  pFinAcCode: string = '';
  pFinAcName: string = '';
  pBranchCode: string = '';

  dateFrom: string = '';
  dateTo: string = '';

  Seg1: string = '';
  Seg2: string = '';
  Batch: string = '';
  CostAllocation: string = '';
  CostSegCode: string = '';

  Seg1Name: string = '';
  Seg2Name: string = '';
  BatchName: string = '';
  CostAllocationName: string = '';
  CostSegCodeName: string = '';

  totalDrAmount: number = 0;
  totalCrAmount: number = 0;
  totalBalance: number = 0;

  accountSearch: string = '';

  hasAccountCode: boolean = false;

  segmentSetupList: Array<CustomSelectListItem> = [];
  segmentTwoSetupList: Array<CustomSelectListItem> = [];
  costAllocationList: Array<CustomSelectListItem> = [];
  batchSetupList: Array<CustomSelectListItem> = [];
  costSegCodeList: Array<LanCustomSelectListItem> = [];

  constructor(private utilService: UtilityService, private authService: AuthorizeService, private notifyService: NotificationService,
    private apiService: ApiService, public dialog: MatDialog) { super(authService); }

  ngOnInit(): void {
    this.loadCategoryTypeList();
  }

  accountSearchChage() {

    if (this.accountSearch !== '') {
      if (this.listOfCategoriesNew.length > 0) {

        const bList = document.getElementsByClassName("bfinAcCode") as any;
        [...bList].forEach(slide => {
          const td0 = slide.content as string;
          if (td0) {
            slide.style.display = td0.toLocaleLowerCase().trim().indexOf(this.accountSearch.toLocaleLowerCase().trim()) > -1 ? '' : 'none';
          }
        });

      }

    }
    else {
      this.listOfCategories = this.listOfCategoriesNew;
      const bList = document.getElementsByClassName("bfinAcCode") as any;
      [...bList].forEach(slide => {
        slide.style.display = 'block';
      });

    }
  }

  test() {
    //this.listOfCategories = this.listOfCategories.filter(item => {
    //  const scg = item['list'];
    //  if (scg != null) {
    //    return (scg as Array<any>).find(scgItem => {
    //      const acCode = scgItem['list'];
    //      if (acCode != null) {
    //        return (acCode as Array<any>).find(acCodeItem => {
    //          const code = acCodeItem['list'];
    //          if (code != null) {
    //            //console.log(code);
    //            return (code as Array<any>).find(codeItem => {
    //             // console.log((codeItem['finAcCode'] as string) === this.accountSearch);
    //              //console.log((codeItem['finAcCode'] as string).indexOf(this.accountSearch));
    //              //return codeItem['finAcCode'] as string === this.accountSearch;
    //              !!(code as Array<any>).find(item2 => item2['finAcCode'] === this.accountSearch)
    //              //return (codeItem['finAcCode'] as string).indexOf(this.accountSearch) !== -1;
    //            });
    //          }
    //          return false;
    //        });
    //      }
    //      return false;
    //    });
    //    return false;
    //  }
    //  return false;
    //});


    // Loop through all table rows, and hide those who don't match the search query
    //for (const i: number = 0; i < bList.length; i++) {
    //  td0 = divList[i].getAttribute('data-content');
    //  if (td0) {
    //    if (td0.toUpperCase().indexOf(text_filter) > -1) {
    //      divList[i].style.display = "";
    //    } else {
    //      divList[i].style.display = "none";
    //    }
    //  }
    //}


    ////this.listOfCategoriesNew.filter(item => {
    ////  //  debugger;
    ////  const scg = item['list'] as Array<any>;
    ////  scg.find(scgItem => {
    ////    if (scgItem['list'])

    ////      (scgItem['list'] as Array<any>).filter(accountItem => {
    ////        if (accountItem['list']) {

    ////          const accountItemArrayList = (accountItem['list'] as Array<any>).filter(codeItem => {
    ////            if ((codeItem['finAcCode'] as string).toLocaleLowerCase().indexOf(this.accountSearch.toLocaleLowerCase()) !== -1) {
    ////              return true;
    ////            }
    ////            return false;
    ////          });

    ////          //const accountItemArray = (accountItem['list'] as Array<any>).push(codeItem);

    ////          //const iItem = item;
    ////          //accountItem['list'] = 
    ////          //iItem['list'] = scg;
    ////          //iItem['list'] = scg;
    ////          accountItem['list'] = accountItemArrayList;

    ////          this.listOfCategories.push();
    ////          console.log(accountItemArrayList);
    ////        }
    ////      });

    ////    //finAcCode
    ////    //if ((scgItem['finCatCode'] as string).toLocaleLowerCase().indexOf(this.accountSearch.toLocaleLowerCase()) !== -1)
    ////    //  return true;
    ////    //return false;
    ////  });

    ////});





    ////this.listOfCategories = this.listOfCategoriesNew.filter(item => {
    ////  //  debugger;
    ////  const scg = item['list'] as Array<any>;
    ////  return scg.find(scgItem => {
    ////    if (scgItem['list'])
    ////      return (scgItem['list'] as Array<any>).find(accountItem => {
    ////        if (accountItem['list'])
    ////          return (accountItem['list'] as Array<any>).find(codeItem => {
    ////            if ((codeItem['finAcCode'] as string).toLocaleLowerCase() === this.accountSearch.toLocaleLowerCase()) {
    ////            //if ((codeItem['finAcCode'] as string).toLocaleLowerCase().indexOf(this.accountSearch.toLocaleLowerCase()) !== -1) {

    ////              return true;
    ////            }
    ////          return false;
    ////        });
    ////      });
    ////    //finAcCode
    ////    //if ((scgItem['finCatCode'] as string).toLocaleLowerCase().indexOf(this.accountSearch.toLocaleLowerCase()) !== -1)
    ////    //  return true;
    ////    //return false;
    ////  });

    ////});


    //const result = this.listOfCategories.map(f => {
    //  if (f['list'])
    //    (f['list'] as Array<any>).map(second => {
    //      if (second['list'])
    //        (second['list'] as Array<any>).map(third => {
    //          if (third['list'])
    //            (third['list'] as Array<any>).map(fourth => {                     
    //                console.log(fourth);
    //            });
    //        });
    //    });
    //  //          this.listOfCategories.filter(({ colorCode: c }) => +c === f)));
    //});
    //this.listOfCategories = this.listOfCategories.

    //  console.log(this.listOfCategories);

    //this.listOfCategories = this.listOfCategories.filter(item => {

    //  const scg = item['list'] as Array<any>;
    //  if (scg && scg.length > 0) {
    //    scg.filter(scgItem => {
    //      const acCode = scgItem['list'] as Array<any>;
    //      if (acCode && acCode.length > 0) {
    //        acCode.filter(acCodeItem => {

    //          const code = acCodeItem['list'] as Array<any>;
    //          if (code && code.length > 0) {
    //            code.filter(codeItem => {
    //              //!!activeCheckbuttons.find(codeItem => i codeItem['finAcCode'] == this.accountSearch;
    //              // return codeItem['finAcCode'] == this.accountSearch;
    //              return (codeItem['finAcCode'] as string).indexOf(this.accountSearch) !== -1;
    //              //return true;
    //            });
    //          }

    //        });
    //      }
    //    });
    //  }
    //});


  }

  loadCategoryTypeList() {
    this.isLoading1 = true;
    this.apiService.getall('accountscategory/getCategoryTypeList').subscribe(res => {
      this.isLoading1 = false;
      if (res) {
        this.listOfCategories = this.listOfCategoriesNew = res['list'];
        this.fInSysGenAcCode = res['fInSysGenAcCode'] as boolean;
      }
    });
    this.apiService.getall('ledgerReport/ledgerBranchViewList').subscribe(res => {
      if (res) {
        this.listOfBranchLedgers = res;
      }
    });


    this.apiService.getall("segmentSetup/getSegmentSetupSelectList").subscribe(res => {
      if (res)
        this.segmentSetupList = res;
    });

    this.apiService.getall("segmentTwoSetup/getSegmentTwoSetupSelectList").subscribe(res => {
      if (res)
        this.segmentTwoSetupList = res;
    });

    this.apiService.getall("costAllocationSetup/getCostAllocationSetupSelectList").subscribe(res => {
      if (res)
        this.costAllocationList = res;
    });
    this.apiService.getall("batchSetup/getBatchSetupSelectList").subscribe(res => {
      if (res)
        this.batchSetupList = res;
    });

  }

  resetSeg1() {
    this.Seg1Name = '';
  }
  resetSeg2() {
    this.Seg2Name = '';
  }
  resetBatch() {
    this.BatchName = '';
  }
  resetCostAllocation() {
    this.CostAllocationName = '';
    this.CostSegCodeName = '';
  }
  resetCostSegCode() {
    this.CostSegCodeName = '';
  }

  selectCostSegCode(evt: any) {
    this.CostSegCodeName = evt.textAr;
  }
  selectBatch(evt: any) {
    this.BatchName = evt.text;
  }
  selectSeg2(evt: any) {
    this.Seg2Name = evt.text;

  }
  selectSeg1(evt: any) {
    this.Seg1Name = evt.text;

  }

  loadSegCodedata(event: any) {
    const id: number = event.value as number;
    this.CostAllocationName = event.text;
    this.CostSegCodeName = '';
    this.CostSegCode = '';
    if (id > 0) {
      this.apiService.getall(`costAllocationSetup/getCostSegmentCodeSelectList/${id}`).subscribe(res => {
        if (res)
          this.costSegCodeList = res;

        const newList = this.costSegCodeList.map((i) => {
          i.textAr = i.textAr + ' ' + i.textTwo;
          return i;
        });

      });
    }
  }

  changeCategoryView() {
    this.isCategory = !this.isCategory;
  }

  loadLedger(finAcCode: string, branchCode: string, finAcName: string) {
    this.finAcNameHeader = `(${finAcCode}) ${finAcName}`;
    this.pFinAcCode = finAcCode;
    this.pFinAcName = finAcName;
    this.pBranchCode = branchCode;

    this.setDefaults();
    this.hasAccountCode = true;
    this.filter(finAcCode, branchCode, finAcName);
  }

  search() {
    this.filter(this.pFinAcCode, this.pBranchCode, this.pFinAcName);
  }

  refresh() {
    this.setDefaults();
    this.filter(this.pFinAcCode, this.pBranchCode, this.pFinAcName);
  }
  setDefaults() {
    this.Seg1 = this.Seg2 = this.Batch = this.CostAllocation = this.CostSegCode = this.dateFrom = this.dateTo = '';
    this.Seg1Name = this.Seg2Name = this.BatchName = this.CostAllocationName = this.CostSegCodeName = '';

  }

  filter(finAcCode: string, branchCode: string, finAcName: string) {
    console.log(this.Seg1);
    this.isLoading = true;
    this.apiService.getall(`ledgerReport/viewLedgerReport?seg1=${this.Seg1 ? this.Seg1 : ''}&seg2=${this.Seg2 ? this.Seg2 : ''}&costAllocation=${this.CostAllocation ? this.CostAllocation : ''}&costSegCode=${this.CostSegCode ? this.CostSegCode : ''}&batch=${this.Batch ? this.Batch : ''}&finAcCode=${finAcCode}&branchCode=${branchCode}&from=${this.utilService.getCommonDate(this.dateFrom)}&to=${this.utilService.getCommonDate(this.dateTo)}`).subscribe(res => {
      this.isLoading = false;
      if (res) {
        this.listOfLedger = res;
        this.company = res['company'];
        this.totalBalance = res['totalBalance'];
        this.totalDrAmount = res['totalDrAmount'];
        this.totalCrAmount = res['totalCrAmount'];

      }
    });
  }



  categoryCheck(event: any, className: string) {
    this.expandCollapseMenu(event, className);
  }
  subCategoryCheck(event: any, className: string) {
    this.expandCollapseMenu(event, className);
  }

  itemCategoryCheck(event: any, className: string) {
    this.expandCollapseMenu(event, className);
  }

  expandCollapseMenu(event: any, className: string) {
    // console.log(className);
    let ulList = document.getElementsByClassName(className) as any;
    let expan = document.getElementsByClassName(className + '_expan')[0] as HTMLSpanElement;
    if (event.target.checked) {
      expan.textContent = '+';
      [...ulList].forEach(slide => {
        slide.style.display = 'none';
      });
    }
    else {
      expan.textContent = '⇩';
      [...ulList].forEach(slide => {
        slide.style.display = 'block';
      });
    }

  }


  printInvoice() {
    const printContent = document.getElementById("printcontainer") as HTMLElement;    
    this.utilService.printForLocale(printContent);   
   
  }

}
