import { HttpClient } from '@angular/common/http';
import { Component, OnChanges, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { from, Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, mergeMap, startWith, switchMap } from 'rxjs/operators';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
//import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';
import { ValidationService } from '../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-shifts-to-site-map',
  templateUrl: './shifts-to-site-map.component.html'
})
export class ShiftsToSiteMapComponent extends ParentOptMgtComponent implements OnInit {

  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
  readonly: string = "";

  isDataLoading: boolean = false;
  invoiceItemObject: any;
  listOfShifts: Array<any> = [];
  sequence: number = 1;
  editsequence: number = 0;
  remarks: string = '';
  shiftCode: string = '';
  shiftName_EN: string = '';
  shiftName_AR: string = '';

  custCodeControl = new FormControl('', Validators.required);
  siteCodeList: Array<CustomSelectListItem> = [];
  filteredCustCodes: Observable<Array<CustomSelectListItem>>;
  customerCode: string = '';
  listOfShiftCodes: Array<CustomSelectListItem> = [];

  filterCustCodes(val: string): Observable<Array<CustomSelectListItem>> {
    return this.apiService.getall(`CustomerMaster/getSelectCustomerList?search=${val}`)
      .pipe(
        map(response => {
          const res = response as Array<CustomSelectListItem>;
          this.isDataLoading = false;
          return res;
        })
      )
  }
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService) {
    super(authService);

    this.filteredCustCodes = this.custCodeControl.valueChanges.pipe(
      startWith(this.customerCode),
      debounceTime(utilService.autoDelay()),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isDataLoading = true;
        return this.filterCustCodes(val || '')
      })
    );


  }
  loadShiftCodes() {
    this.apiService.getall('ShiftMaster/getSelectShiftMasterList').subscribe(res => {
      this.listOfShiftCodes = res;
    });
  }
  ngOnInit(): void {
    this.setForm();
    this.readonly = "readonly";
  }

  setForm() {
    this.form = this.fb.group({
      //'shiftName_AR': ['', Validators.required],
      //'shiftName_EN': ['', Validators.required],
      'shiftCode': [''],
      'customerCode': ['', Validators.required],
      'siteCode': ['']
    });
    this.setToDefault();
  }
  submit() {
    if (this.form.valid && this.listOfShifts.length > 0) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      this.form.value['shiftsList'] = this.listOfShifts;


      this.apiService.post('ShiftMaster/CreateUpdateShiftsToSiteMaps', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
          });
      window.history.back();
    }
    else
      this.utilService.FillUpFields();

  }
  addShift() {
    if (this.shiftCode != '') {
      let shift:any = {
        sequence: this.getSequence(),
        shiftCode: this.shiftCode,
        shiftName_AR: this.shiftName_AR,
        shiftName_EN: this.shiftName_EN
      };

      if (!this.listOfShifts.find(s=>s.shiftCode==this.shiftCode))
      this.listOfShifts.push(shift);
    }
    this.setToDefault();
  }
  removeElement(i: number) {
    this.listOfShifts.splice(i, 1);
    this.downSequence();
  }
  setToDefault() {
    this.shiftCode = "";
    this.shiftName_AR = "";
    this.shiftName_EN = "";
  }
  getSequence(): number { return this.sequence += this.sequence + 1 };
  downSequence(): number { return this.sequence += this.sequence - 1 };
  reset() {
    this.form.controls['shiftName_AR'].setValue('');
    this.form.controls['shiftName_EN'].setValue('');
    this.form.controls['shiftCode'].setValue('');

  }
  goBack() {
    window.history.back();

  }
  getShiftDetails(event: any) {
    const shiftcode = event.target.value;
    this.apiService.getall('ShiftMaster/getShiftMasterByShiftMasterCode/' + shiftcode).subscribe(res => {
      this.shiftName_AR = res['shiftName_AR'],
        this.shiftName_EN = res['shiftName_EN']
    });
  }

  onSelectionCustomerCode(event: any,op:string) {
    let custCode: string = '';
    this.form.controls['siteCode'].setValue('');
      custCode =op=='change'? event.target.value:event.option.value; 

    this.apiService.getall('CustomerMaster/getCustomerByCustomerCode/' + custCode).subscribe(res => {
      if (res != null) {
        this.form.patchValue({'customerCode': res['custCode'] });
        let custCode = this.custCodeControl.value as string;
        this.form.value['customerCode'] = custCode;
        this.loadSiteCodes(custCode);
 
      }
      else {

        this.form.controls['customerCode'].setValue('');
        this.custCodeControl.setValue('');
        this.siteCodeList = [];
      }
      
      this.listOfShiftCodes = [];
      this.listOfShifts = [];
    });

  }
  onSelectSiteCode(event: any) {
    let siteCode: string = '';
    siteCode = event.target.value;
    this.form.value['siteCode'] = siteCode;
    this.apiService.getall(`ShiftMaster/getShiftsToSiteBySiteCode/${siteCode}`).subscribe(res => {
      this.listOfShifts = res;
      this.sequence = this.listOfShifts.length + 1;
    });

    this.loadShiftCodes();

  }

  loadSiteCodes(custCode: string) {
    this.apiService.getall(`CustomerSite/getSelectSiteListByCustCode/${custCode}`).subscribe(res => {
      this.siteCodeList = res;
    });
  }
}
