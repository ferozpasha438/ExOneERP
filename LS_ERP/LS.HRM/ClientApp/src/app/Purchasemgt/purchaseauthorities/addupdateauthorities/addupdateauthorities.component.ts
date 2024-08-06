import { Component, OnInit } from '@angular/core';
import { Validators } from '@angular/forms';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { ParentPurchaseMgtComponent } from '../../../sharedcomponent/parentpurchasemgt.component';
/*import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';*/




@Component({
  selector: 'app-addupdateauthorities',
  templateUrl: './addupdateauthorities.component.html'
  
})
export class AddupdateauthoritiesComponent extends ParentPurchaseMgtComponent implements OnInit {
  form: FormGroup;
  //appAuth: string = '';
  //appLevel: number;
  //canApproveSurvey: boolean;
  //canApproveEnquiry: boolean;
  //canApproveProposal: boolean;
  //canModifyEstimation: boolean;
  //canConvertEnqToProject: boolean;
  //canCreateRoaster: boolean;
  //canEditRoaster: boolean;
  //isFinalAuthority: boolean;
  userList: Array<any>;
  branchCodeList: Array<CustomSelectListItem> = [];
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  readonly: string = "";
  id: number = 0;
  branchCode: string;
  appAuth: number;

  constructor(public dialogRef: MatDialogRef<AddupdateauthoritiesComponent>, private authService: AuthorizeService, private fb: FormBuilder, private apiService: ApiService, private utilService: UtilityService) {
    super(authService);
  }

  ngOnInit(): void {
    this.setForm();

    this.loadBranchCodes();
    if (this.id > 0) {

      this.setEditForm();
      this.readonly = "readonly";
    }
  }

  setForm() {


    this.form = this.fb.group({
      'id': [this.id],
      'branchCode': ['', Validators.required],
      'appAuth': ['', Validators.required], /*userId*/
      'appLevel': [''],
      'purchaseRequest': [false],
      'purchaseOrder': [false],
      'purchaseReturn': [false]
      
    });
    this.loadUsers();
  }
  removeItem(i: number) {
    return;
  }
  loadUsers() {
    this.apiService.getall('PurchaseAuthorities/GetUserSelectionList').subscribe(res => {
      this.userList = res;

    });

  }

  submit() {

    this.form.value['appAuth'] = this.appAuth;
    this.form.value['branchCode'] = this.branchCode;
    this.form.value['appLevel'] = 0;
    if (this.form.valid) {
      this.apiService.post('PurchaseAuthorities', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.dialogRef.close(true);
        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
          });

    }
    else
      this.utilService.FillUpFields();
  }


  loadBranchCodes() {
    this.apiService.getall('PurchaseAuthorities/getSelectBranchCodeList').subscribe(res => {
      this.branchCodeList = res;
    });
  }
  setEditForm() {
    this.apiService.get('PurchaseAuthorities/getAuthorityById', this.id).subscribe(res => {
      if (res) {

        this.form.patchValue(res);
        this.branchCode = res.branchCode;
        this.appAuth = res.appAuth;
        this.form.controls['branchCode'].disable({ onlySelf: true });
        this.form.controls['appAuth'].disable({ onlySelf: true });
        this.form.controls['appLevel'].disable({ onlySelf: true });

      }
    });



  }


  getAuthority() {

    if (this.form.controls['branchCode'].value != '' && this.form.controls['appAuth'].value != '') {
      this.branchCode = this.form.controls['branchCode'].value;
      this.appAuth = this.form.controls['appAuth'].value;
      this.apiService.getall(`PurchaseAuthorities/getAuthorityByBranchUserId/${this.branchCode}/${this.appAuth}`).subscribe(res => {
        if (res != null) {
          this.id = res.id;
          this.setEditForm();

        }
      });
    }

  }
  closeModel() {
    this.dialogRef.close();
  }
}


