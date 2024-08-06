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
  selector: 'app-shiftplan-for-project',
  templateUrl: './shiftplan-for-project.component.html'
})
export class ShiftplanForProjectComponent extends ParentOptMgtComponent implements OnInit {

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
  shiftId: number = 0;

  //custCodeControl = new FormControl('', Validators.required);
  siteCodeList: Array<CustomSelectListItem> = [];
  filteredCustCodes: Observable<Array<CustomSelectListItem>>;
  // customerCode: string = '';
  listOfShiftCodes: Array<CustomSelectListItem> = [];
  project: any;
  isArab: boolean = false;

  isRoasterGenerated: boolean = false;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<ShiftplanForProjectComponent>) {
    super(authService);




  }
  loadShiftCodes() {
    this.apiService.getall(`ShiftMaster/getSelectShiftMasterListForProjectSite/${this.project.projectCode}/${this.project.siteCode}`).subscribe(res => {
      this.listOfShiftCodes = res;
    });
  }
  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.setForm();
    this.loadSiteCodes(this.project.customerCode);
    this.readonly = "readonly";
  }

  setForm() {

    this.form = this.fb.group({
      //'shiftName_AR': ['', Validators.required],
      //'shiftName_EN': ['', Validators.required],
      'shiftCode': [''],
      'projectCode': [this.project.projectCode],
      'customerCode': [this.project.customerCode],
      'siteCode': [this.project.siteCode != null ? this.project.siteCode : '', Validators.required],
      'shiftId': [0]
    });
    if (this.project.siteCode != null) {
      this.form.controls['siteCode'].disable({ onlySelf: true });
      let event: any = {
        target: { value: this.project.siteCode }

      };
      this.onSelectSiteCode(event);
    }

    this.setToDefault();
  }
  submit() {
    if (this.form.valid && this.listOfShifts.length > 0) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      this.form.value['shiftsList'] = this.listOfShifts;
      this.form.value['projectCode'] = this.project.projectCode;
      this.form.value['siteCode'] = this.project.siteCode;

      this.apiService.post('ShiftMaster/CreateUpdateShiftsPlanForProject', this.form.value)
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
  addShift() {
    if (this.shiftCode != '') {
      let shift: any = {
        sequence: this.getSequence(),
        shiftCode: this.shiftCode,
        shiftName_AR: this.shiftName_AR,
        shiftName_EN: this.shiftName_EN,
        shiftId: 0
      };

      if (!this.listOfShifts.find(s => s.shiftCode == this.shiftCode))
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
    this.shiftId = 0;
  }
  getSequence(): number { return this.sequence += this.sequence + 1 };
  downSequence(): number { return this.sequence += this.sequence - 1 };
  reset() {
    this.form.controls['shiftName_AR'].setValue('');
    this.form.controls['shiftName_EN'].setValue('');
    this.form.controls['shiftCode'].setValue('');
    this.form.controls['shiftId'].setValue(0);

  }

  getShiftDetails(event: any) {
    const shiftcode = event.target.value;
    this.apiService.getall('ShiftMaster/getShiftMasterByShiftMasterCode/' + shiftcode).subscribe(res => {
      this.shiftName_AR = res['shiftName_AR'],
        this.shiftName_EN = res['shiftName_EN']

    });
  }

  onSelectSiteCode(event: any) {

    let siteCode = event.target.value;
    this.project.siteCode = siteCode;
    this.form.value['siteCode'] = siteCode;
    if (siteCode != '') {
      this.apiService.getall(`ShiftMaster/getShiftsByProjectAndSiteCode/${this.project.projectCode}/${siteCode}`).subscribe(res => {
        this.listOfShifts = res;
        console.log(res);
        this.sequence = this.listOfShifts.length + 1;
        this.apiService.getall(`MonthlyRoaster/isExistMonthlyRoasterForProjectSite/${this.project.projectCode}/${siteCode}`).subscribe(res => {
          this.isRoasterGenerated = res;

        });

      });

      this.loadShiftCodes();

    }
    else {
      this.listOfShifts = [];
      this.listOfShiftCodes = [];
      this.isRoasterGenerated = false;
    }
  }

  loadSiteCodes(projectCode: string) {
    this.apiService.getall(`customerSite/getSelectSiteListByProjectCode/${this.project.projectCode}`).subscribe(res => {
      this.siteCodeList = res;
    });
  }

  closeModel() {
    this.dialogRef.close();
  }


  GetFirstChar(str: string) {
    return str.substring(0,1);
  }
}
