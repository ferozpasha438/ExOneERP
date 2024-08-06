import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { ParentSalesMgtComponent } from '../../../sharedcomponent/parentsalesmgt.component';

@Component({
  selector: 'app-snd-add-update-authorities',
  templateUrl: './snd-add-update-authorities.component.html'
})
export class SndAddUpdateAuthoritiesComponent extends ParentSalesMgtComponent implements OnInit {
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
  branchCodeList: Array < CustomSelectListItem > =[];
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  readonly: string = "";
  id: number = 0;
  branchCode: string;
  appAuth: number;

  selectAll: boolean = false;
  constructor(public dialogRef: MatDialogRef<SndAddUpdateAuthoritiesComponent >, private authService: AuthorizeService, private fb: FormBuilder, private apiService: ApiService, private utilService: UtilityService) {
    super(authService);
  }

  ngOnInit(): void {
    this.setForm();

    this.loadBranchCodes();
    if(this.id > 0) {

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

    'canCreateSndInvoice': [false],
    'canEditSndInvoice': [false],
    'canApproveSndInvoice': [false],
    'canPostSndInvoice': [false],
    'canVoidSndInvoice': [false],
    'canSettleSndInvoice': [false],

    'canCreateSndQuotation': [false],
    'canEditSndQuotation': [false],
    'canApproveSndQuotation': [false],
    'canVoidSndQuotation': [false],
    'canConvertSndQuotationToOrder': [false],
    'canConvertSndQuotationToInvoice': [false],
    'canConvertSndQuotationToDeliveryNote': [false],
    'canReviseSndQuotation': [false],



    'isFinalAuthority': [false],


    'allAuthorities': [false],
    

  });
  this.loadUsers();
}
removeItem(i: number) {
  return;
}
loadUsers() {
  this.apiService.getall('Users/GetUserSelectionList').subscribe(res => {
    this.userList = res;

  });

}

submit() {

  this.form.value['appAuth'] = this.appAuth;
  this.form.value['branchCode'] = this.branchCode;
  this.form.value['appLevel'] = 0;
  if (this.form.valid) {
    this.apiService.post('SndAuthorities', this.form.value)
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
  this.apiService.getall('Branch/getSelectBranchCodeList').subscribe(res => {
    this.branchCodeList = res;
  });
}
setEditForm() {
  this.apiService.get('SndAuthorities/getAuthorityById', this.id).subscribe(res => {
    if (res) {

      this.form.patchValue(res);
      this.branchCode = res.branchCode;
      this.appAuth = res.appAuth;
      this.form.controls['branchCode'].disable({ onlySelf: true });
      this.form.controls['appAuth'].disable({ onlySelf: true });
      this.form.controls['appLevel'].disable({ onlySelf: true });

      this.ResetAuthorities({ target: {checked:true}},0)
    }
  });



}


getAuthority() {

  if (this.form.controls['branchCode'].value != '' && this.form.controls['appAuth'].value != '') {
    this.branchCode = this.form.controls['branchCode'].value;
    this.appAuth = this.form.controls['appAuth'].value;
    this.apiService.getall(`SndAuthorities/getAuthorityByBranchUserId/${this.branchCode}/${this.appAuth}`).subscribe(res => {
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

  ResetAuthorities(event:any,type:number) {

    console.log(event);
    console.log(this.form.controls['allAuthorities'].value)


    if (type == 1) {
      if (event.target.checked) {
        this.checkAll();
      }
      else {
        this.unCheckAll();
      }
    }
    else {
      if (this.form.controls['canCreateSndInvoice'].value &&
        this.form.controls['canEditSndInvoice'].value &&
        this.form.controls['canApproveSndInvoice'].value &&
      this.form.controls['canPostSndInvoice'].value &&
      this.form.controls['canVoidSndInvoice'].value &&
        this.form.controls['canSettleSndInvoice'].value&&

        this.form.controls['canCreateSndQuotation'].value &&
        this.form.controls['canEditSndQuotation'].value &&
        this.form.controls['canApproveSndQuotation'].value &&
      this.form.controls['canVoidSndQuotation'].value &&
        this.form.controls['canReviseSndQuotation'].value &&
        this.form.controls['canConvertSndQuotationToOrder'].value&&
        this.form.controls['canConvertSndQuotationToInvoice'].value&&
        this.form.controls['canConvertSndQuotationToDeliveryNote'].value








      )
      {
        this.form.controls['allAuthorities'].setValue( true);
      }
         else {
        this.form.controls['allAuthorities'].setValue(false)
      }
      }
      

    }

  

  checkAll() {
    this.form.controls['canCreateSndInvoice'].setValue(true);
    this.form.controls['canEditSndInvoice'].setValue(true);
    this.form.controls['canApproveSndInvoice'].setValue(true);
    this.form.controls['canPostSndInvoice'].setValue(true);
    this.form.controls['canVoidSndInvoice'].setValue(true);
    this.form.controls['canSettleSndInvoice'].setValue(true);

    this.form.controls['canCreateSndQuotation'].setValue(true);
    this.form.controls['canEditSndQuotation'].setValue(true);
    this.form.controls['canApproveSndQuotation'].setValue(true);
    this.form.controls['canVoidSndQuotation'].setValue(true);
    this.form.controls['canReviseSndQuotation'].setValue(true);
    this.form.controls['canConvertSndQuotationToOrder'].setValue(true);
    this.form.controls['canConvertSndQuotationToInvoice'].setValue(true);
    this.form.controls['canConvertSndQuotationToDeliveryNote'].setValue(true);






  }
  unCheckAll() {
    this.form.controls['canCreateSndInvoice'].setValue(false);
    this.form.controls['canEditSndInvoice'].setValue(false);
    this.form.controls['canApproveSndInvoice'].setValue(false);
    this.form.controls['canPostSndInvoice'].setValue(false);
    this.form.controls['canVoidSndInvoice'].setValue(false);
    this.form.controls['canSettleSndInvoice'].setValue(false);

    this.form.controls['canCreateSndQuotation'].setValue(false);
    this.form.controls['canEditSndQuotation'].setValue(false);
    this.form.controls['canApproveSndQuotation'].setValue(false);
    this.form.controls['canVoidSndQuotation'].setValue(false);
    this.form.controls['canReviseSndQuotation'].setValue(false);
    this.form.controls['canConvertSndQuotationToOrder'].setValue(false);
    this.form.controls['canConvertSndQuotationToInvoice'].setValue(false);
    this.form.controls['canConvertSndQuotationToDeliveryNote'].setValue(false);

  }
}
