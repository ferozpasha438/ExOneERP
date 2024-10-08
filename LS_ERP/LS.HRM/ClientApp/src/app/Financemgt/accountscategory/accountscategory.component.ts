import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { DBOperation } from '../../services/utility.constants';
import { UtilityService } from '../../services/utility.service';
import { ParentSystemSetupComponent } from '../../sharedcomponent/parentsystemsetup.component';
import { AccouncodesComponent } from '../sharedpages/accouncodes/accouncodes.component';
import { AccounsubcategoryComponent } from '../sharedpages/accounsubcategory/accounsubcategory.component';
import { AccounttypeComponent } from '../sharedpages/accounttype/accounttype.component';
@Component({
  selector: 'app-accountscategory',
  templateUrl: './accountscategory.component.html',
  styles: [
    `.cu-pointer {cursor: pointer;}`
  ],

})
export class AccountscategoryComponent extends ParentSystemSetupComponent implements OnInit {
  listOfCategories: Array<any> = [];
  listOfCategoriesNew: Array<any> = [];

  fInSysGenAcCode: boolean = false;
  accountSearch: string = '';
  isLoading: boolean = false;

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
          //slide.style.display = 'block';
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


  loadCategoryTypeList() {
    this.isLoading = true;
    this.apiService.getall('accountscategory/getCategoryTypeList').subscribe(res => {
      this.isLoading = false;
      if (res) {
        this.listOfCategories = this.listOfCategoriesNew = res['list'];
        this.fInSysGenAcCode = res['fInSysGenAcCode'] as boolean;
      }
    })
  }

  private openDialogManage<T>(component: T, accountTypeId: string = '', accountTypeCode: string = '', fInSysGenAcCode: boolean = false, dbops: DBOperation = DBOperation.create, modalTitle: string = '', modalBtnTitle: string = '') {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, component);
    (dialogRef.componentInstance as any).accountTypeCode = accountTypeCode;
    (dialogRef.componentInstance as any).accountTypeId = accountTypeId;
    (dialogRef.componentInstance as any).fInSysGenAcCode = fInSysGenAcCode;
    //(dialogRef.componentInstance as any).modalTitle = modalTitle;
    //(dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    //(dialogRef.componentInstance as any).id = id;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true) {
        this.loadCategoryTypeList();
        this.utilService.OkMessage();
      }
    });
  }

  createAsset(accountTypeId: string) {
    this.openDialogManage(AccounttypeComponent, accountTypeId);
  }

  createCateg(accountTypeCode: string, accountTypeId: string) {
    this.openDialogManage(AccounsubcategoryComponent, accountTypeId, accountTypeCode);
  }

  createSubCateg(accountTypeCode: string, accountTypeId: string) {
    this.openDialogManage(AccouncodesComponent, accountTypeId, accountTypeCode, this.fInSysGenAcCode);
  }

  editAccountCode(id: any) {
    this.openDialogManage(AccouncodesComponent, id, 'accountTypeCode', this.fInSysGenAcCode);
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
    let expan = document.getElementsByClassName(className +'_expan')[0] as HTMLSpanElement;
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
}
