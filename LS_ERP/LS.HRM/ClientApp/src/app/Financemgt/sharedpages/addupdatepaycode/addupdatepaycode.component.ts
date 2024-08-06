import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { PaginationService } from '../../../sharedcomponent/pagination.service';
import { ParentSystemSetupComponent } from '../../../sharedcomponent/parentsystemsetup.component';
import { BankcheckbookComponent } from '../../sharedpages/bankcheckbook/bankcheckbook.component';

@Component({
  selector: 'app-addupdatepaycode',
  templateUrl: './addupdatepaycode.component.html',
  styles: [
  ]
})
export class AddupdatepaycodeComponent extends ParentSystemSetupComponent implements OnInit {
  form: FormGroup;
  id: number = 0;
  checkRes: any;

  branchCodeControl = new FormControl();
  iACControl = new FormControl();
  pDCControl = new FormControl();
  isCheckBookAdded: boolean = false;
  canShowCheckBookAdded: boolean = false;

  isBranchLoading: boolean = false;
  isIACLoading: boolean = false;
  isPDCLoading: boolean = false;
  branchCodeError: string = '';

  filteredOptions: Observable<Array<CustomSelectListItem>>;
  filteredIACOptions: Observable<Array<CustomSelectListItem>>;
  filteredPDCOptions: Observable<Array<CustomSelectListItem>>;
  payCodeTypeList: Array<CustomSelectListItem> = [];

  constructor(private fb: FormBuilder, private apiService: ApiService, private authService: AuthorizeService,
    private utilService: UtilityService, private notifyService: NotificationService, public dialog: MatDialog,
    public pageService: PaginationService, public dialogRef: MatDialogRef<AddupdatepaycodeComponent>) {
    super(authService);

    this.filteredOptions = this.branchCodeControl.valueChanges.pipe(
      startWith(''),
      debounceTime(400),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isBranchLoading = true;
        return this.filter(val || '')
      })
    );
    this.filteredIACOptions = this.iACControl.valueChanges.pipe(
      startWith(''),
      debounceTime(400),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isIACLoading = true;
        return this.filterIAC(val || '')
      })
    );
    this.filteredPDCOptions = this.pDCControl.valueChanges.pipe(
      startWith(''),
      debounceTime(400),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isPDCLoading = true;
        return this.filterPDC(val || '')
      })
    );

  }


  ngOnInit(): void {
    this.setForm();
    this.loadData();

    if (this.id > 0)
      this.setEditForm();
  }

  loadData() {
    this.apiService.getall("paymentcodes/getSelectPaymentTypeList").subscribe(res => {
      if (res)
        this.payCodeTypeList = res;
    });
  }

  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      'finPayType': ['', Validators.required],
      'finPayCode': ['', Validators.required],
      'finPayName': ['', Validators.required],
      // 'finPayAcIntgrAC': ['', Validators.required],
      // 'finPayPDCClearAC': ['', Validators.required],
      'isActive': [false],
      'useByOtherBranches': [false],
      'systemGenCheckBook': [false]
    });
  }

  filter(val: string): Observable<Array<CustomSelectListItem>> {
    return this.apiService.getall(`branch/getSelectSysBranchList?search=${val.trim()}`)
      .pipe(
        map(response => {
          const res = response as Array<CustomSelectListItem>;
          //if (res && res.length == 0)
          //  this.notifyService.showError("enter branch name")
          this.isBranchLoading = false;
          return res;
        })
      )
  }
  filterIAC(val: string): Observable<Array<CustomSelectListItem>> {
    return this.apiService.getall(`accountscategory/getSelectMainIntegrationAccountList?search=${val}`)
      .pipe(
        map(response => {
          const res = response as Array<CustomSelectListItem>;
          //if (res && res.length == 0)
          //  this.notifyService.showError("enter branch name")
          this.isIACLoading = false;
          return res;
        })
      )
  }
  filterPDC(val: string): Observable<Array<CustomSelectListItem>> {
    return this.apiService.getall(`accountscategory/getSelectMainPdcAccountList?search=${val}`)
      .pipe(
        map(response => {
          const res = response as Array<CustomSelectListItem>;
          //if (res && res.length == 0)
          //  this.notifyService.showError("enter branch name")
          this.isPDCLoading = false;
          return res;
        })
      )
  }

  blurBranchCode(event: any) {
    this.apiService.getall(`branch/checkBranchCode?branchCode=${event.target.value}`).subscribe(res => {
      if (!res) {
        //if (!this.isOptionSelected) {
        //this.isOptionSelected = false;
        this.branchCodeControl.setValue('');
        this.branchCodeError = "Select BranchCode";
        this.form.patchValue({ 'finBranchName': '', 'finBranchAddress': '', });
        //}
      }
    });
  }
  selectedOption(event: MatAutocompleteSelectedEvent) {
    this.branchCodeError = '';
   // this.branchCodeControl.setValue(event.option.value);
  }

  payCodeSelect(event: any) {
    const payCodeId = event.target.value;
    if (this.utilService.hasValue(payCodeId)) {
      if (payCodeId === '1') {
        this.canShowCheckBookAdded = true;
      }
      else
        this.canShowCheckBookAdded = false;
    }
  }

  branchcodeBlur(event: any) {
    if (event.target.value && event.target.value !== '') {
      this.apiService.getall(`branch/getBranchByBranchCode?branchCode=${event.target.value}`).subscribe(res => {
      }, error => {
        this.branchCodeControl.setValue('');
      })
    }
  }
  //checkBookForBankOptions: Array<string> = [];
  private openDialogManage(id: number = 0, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, checkResult: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, BankcheckbookComponent);
    //(dialogRef.componentInstance as any).dbops = dbops;
    //(dialogRef.componentInstance as any).modalTitle = modalTitle;
    //(dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    if (id > 0)
      (dialogRef.componentInstance as any).checkValue = checkResult;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res.hasValue === true) {
        this.isCheckBookAdded = true;
        Object.keys(res.value).forEach(key => {
          //if (this.utilService.hasValue(res.value[key]))
          this.form.value[key] = res.value[key];
        });
        //console.log(res.value)
      }
      else
        this.isCheckBookAdded = false;

    });
  }

  public create() {
    localStorage.setItem('paycodeCheckvalue', JSON.stringify(this.checkRes))
    this.openDialogManage(0, DBOperation.create, '', '', this.checkRes);
  }


  checkBookForBank(): boolean {
    const payCodeType = +this.form.value['finPayType'];
    if (payCodeType === 1) {
      if (!this.isCheckBookAdded) {
        this.notifyService.showError('Add CheckBook for Bank');
        return false;
      }
    }
    return true;
  }

  setEditForm() {
    this.apiService.get('paymentcodes/getPayCodeById', this.id).subscribe(res => {
      if (res) {
        this.checkRes = res;
      
        //this.form.setValue(res['companyName']);
        this.branchCodeControl.setValue(res['finBranchCode']);
        this.iACControl.setValue(res['finPayAcIntgrAC']);
        this.pDCControl.setValue(res['finPayPDCClearAC']);

        this.form.patchValue(res);
        const payCodeId = res['finPayType'];
        
        if (payCodeId === '1') {
          this.isCheckBookAdded = true;

          this.canShowCheckBookAdded = true;
          this.form.value['stChkNum'] = res['stChkNum'];
          this.form.value['endChkNum'] = res['endChkNum'];
          this.form.value['checkLeavePrefix'] = res['checkLeavePrefix'];
        }
        else
          this.canShowCheckBookAdded = false;
      }
    })
  }


  submit() {
    if (this.form.valid) {
      if (!this.checkBookForBank())
        return;

      if (this.id > 0)
        this.form.value['id'] = this.id;

      let finBranchCode = this.branchCodeControl.value as string;
      let finPayAcIntgrAC = this.iACControl.value as string;
      let finPayPDCClearAC = this.pDCControl.value as string;

      if (this.utilService.hasValue(finBranchCode) && this.utilService.hasValue(finPayAcIntgrAC) && this.utilService.hasValue(finPayPDCClearAC)) {
        this.form.value['finBranchCode'] = finBranchCode;
        this.form.value['finPayAcIntgrAC'] = this.utilService.removeSqueres(finPayAcIntgrAC);
        this.form.value['finPayPDCClearAC'] = this.utilService.removeSqueres(finPayPDCClearAC);

        //console.log(this.form);
        this.apiService.post('paymentcodes', this.form.value)
          .subscribe(res => {
            this.utilService.OkMessage();
            this.form.reset();
            this.dialogRef.close(true);
          },
            error => {
              this.utilService.ShowApiErrorMessage(error);
            });

      }
      else
        this.utilService.FillUpFields();
    }
    else
      this.utilService.FillUpFields();
  }

  closeModel() {
    this.dialogRef.close();
  }

}
