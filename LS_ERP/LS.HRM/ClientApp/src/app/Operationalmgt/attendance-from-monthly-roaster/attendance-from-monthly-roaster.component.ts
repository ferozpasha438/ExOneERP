import { HttpClient } from '@angular/common/http';
import { Component, OnChanges, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { from, Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, mergeMap, startWith, switchMap } from 'rxjs/operators';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../models/MenuItemListDto';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { DBOperation } from '../../services/utility.constants';
import { UtilityService } from '../../services/utility.service';
import { ParentOptMgtComponent } from '../../sharedcomponent/parentoptmgt.component';
import { ValidationService } from '../../sharedcomponent/ValidationService';
import * as moment from "moment";
import { Moment } from "moment";
import { MatFormFieldModule } from '@angular/material/form-field';
import { roaster } from '../models/roaster.model';
export interface CustomSelectShift {
  text: string;
  value: string;
  textTwo: string;
}
@Component({
  selector: 'app-attendance-from-monthly-roaster',
  templateUrl: './attendance-from-monthly-roaster.component.html'
})
export class AttendanceFromMonthlyRoasterComponent extends ParentOptMgtComponent implements OnInit {

  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
  readonly: string = "";

  isDataLoading: boolean = false;
  invoiceItemObject: any;
  listOfRoasters: Array<roaster> = [];
  listOfDays: Array<any> = [];

  sequence: number = 1;
  editsequence: number = 0;
  remarks: string = '';

  shiftsFormControls = new FormArray([new FormControl('')]);

  noOfDays: number = 0;
  saveEnable: boolean = false;
  editEnable: boolean = false;
  monthYear: string = '';
  employeeNumber: string = '';
  employeeName: string = '';
  s: Array<string> = [];
  custCodeControl = new FormControl('', Validators.required);
  empCodeControl = new FormControl('');
  siteCodeList: Array<CustomSelectListItem> = [];
  // projectCodeList: Array<CustomSelectListItem> = [];
  filteredCustCodes: Observable<Array<CustomSelectListItem>>;
  filteredEmployeeNumbers: Observable<Array<CustomSelectListItem>>;
  customerCode: string = '';
  listOfShiftCodes: Array<CustomSelectShift> = [];
  employeeList: Array<CustomSelectListItem> = [];

  //monthsList: Array<CustomSelectListItem> = [];
  //yearsList: Array<CustomSelectListItem> = [];
  siteCode: string = '';
  projectCode: string = '';
  day: number = 0;
  month: number = 0;
  year: number = 0;
  shiftCode: string = '';

  totalShiftsRow: Array<number> = [];
  totalShiftsCol: Array<number> = [];
  grandTotalShifts: number = 0;
  newTotalShifts: number = 0;

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
  //filterEmployeeNumbers(val: string): Observable<Array<CustomSelectListItem>> {
  //  return this.apiService.getall(`Employee/getAutoFillEmployeeList?search=${val}`)
  //    .pipe(
  //      map(response => {
  //        const res = response as Array<CustomSelectListItem>;
  //        this.isDataLoading = false;
  //        return res;
  //      })
  //    )
  //}
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
    //this.filteredEmployeeNumbers = this.empCodeControl.valueChanges.pipe(
    //  startWith(this.employeeNumber),
    //  debounceTime(utilService.autoDelay()),
    //  distinctUntilChanged(),
    //  switchMap((val: string) => {
    //    if (val.trim() !== '')
    //      this.isDataLoading = true;
    //    return this.filterEmployeeNumbers(val || '')
    //  })
    //);
  }
  loadShiftCodes() {
    this.apiService.getall('ShiftMaster/getSelectShiftsToSiteBySiteCode/' + this.siteCode).subscribe(res => {
      this.listOfShiftCodes = res;
      this.findNoOfShifts();

    });
  }
  //loadEmployees() {
  //  this.apiService.getall('Employee/getSelectEmployeeList').subscribe(res => {
  //    this.employeeList = res;
  //  });
  //}
  ngOnInit(): void {
    let date = new Date(Date.now());
    this.year = date.getFullYear();
    this.setForm();
    this.readonly = "readonly";
    //this.siteCodeList = [];
  }

  setForm() {
    this.form = this.fb.group({

      "customerCode": ['', Validators.required],
      "monthYear": [''],
      "siteCode": ['', Validators.required],
      /*   "projectCode": ['', Validators.required],*/
      "month": [0],
      "year": [this.year],
      'employeeNumber': [''],
      'shiftCode': [''],
    });
  }
  //submit() {
  //  if (this.saveEnable) {



  //    this.form.controls['customerCode'].setValue(this.customerCode);
  //    this.form.controls['siteCode'].setValue(this.siteCode);
  //    this.form.controls['employeeNumber'].setValue(this.employeeNumber);

  //    this.form.value['roastersList'] = this.listOfRoasters;
  //    this.form.value['s'] = this.s;
  //    this.form.value['month'] = this.month;
  //    this.form.value['year'] = this.year;
  //    this.form.value['employeeNumber'] = this.employeeNumber;

  //    this.form.value['employeeName'] = this.employeeName;
  //    this.form.value['customerCode'] = this.customerCode;
  //    this.form.value['siteCode'] = this.siteCode;
  //    if (/*this.form.valid &&*/this.listOfRoasters.length > 0) {
  //      if (this.id > 0)
  //        this.form.value['id'] = this.id;
  //      this.apiService.post('MonthlyRoaster/createUpdateMonthlyRoaster', this.form.value)
  //        .subscribe(res => {
  //          this.utilService.OkMessage();
  //        },
  //          error => {
  //            console.log(error)
  //            console.error(error);
  //            this.utilService.ShowApiErrorMessage(error);
  //          });
  //    }
  //    else
  //      this.utilService.FillUpFields();
  //  }

  //}
  //addEntry() {

  //  if (this.employeeNumber != '' && this.employeeNumber != null && this.newTotalShifts != 0) {

  //    let index = this.listOfRoasters.findIndex(s => s.employeeNumber == this.employeeNumber);
  //    let entry: any = {
  //      employeeNumber: this.employeeNumber,
  //      employeeName: this.employeeName,
  //      s: this.s.slice()
  //    };

  //    console.log("index=" + index);
  //    if (index != -1) {

  //      this.removeEntry(index);
  //      this.listOfRoasters.splice(index, 0, entry);
  //    }
  //    else {
  //      this.listOfRoasters.push(entry);
  //    }
  //    this.findNoOfShifts();
  //    this.saveEnable = true;
  //    this.employeeNumber = '';
  //    this.form.controls['employeeNumber'].setValue('');
  //    this.empCodeControl.setValue('');
  //    this.resetShiftsFormControls();
  //  }
  //}
  removeEntry(i: number) {
    this.listOfRoasters.splice(i, 1);
    this.saveEnable = true;
    this.findNoOfShifts();
  }

  getSequence(): number { return this.sequence += this.sequence + 1 };
  downSequence(): number { return this.sequence += this.sequence - 1 };
  reset() {
    this.form.controls['employeeNumber'].setValue('');
    this.form.controls['employeeName'].setValue('');
  }
  goBack() {
    window.history.back();
  }

  onSelectionCustomerCode(event: any, op: string) {
    let custCode: string = '';
    custCode = op == 'change' ? event.target.value : event.option.value;
    this.apiService.getall('CustomerMaster/getCustomerByCustomerCode/' + custCode).subscribe(res => {
      if (res != null) {
        this.form.patchValue({ 'customerCode': res['custCode'] });
        let custCode = this.custCodeControl.value as string;
        this.form.value['customerCode'] = custCode;
        this.customerCode = custCode;
        this.loadSiteCodes(custCode);
      }
      else {
        this.form.controls['customerCode'].setValue('');
        this.custCodeControl.setValue('');
        this.customerCode = '';
        this.siteCodeList = [];

        //this.projectCodeList = [];
      }
    });

    this.form.controls['siteCode'].setValue('');
    this.siteCode = '';
    this.loadData();
  }


  //autoSelectionEmployeeNumber(event: any, op: string) {
  //  let empNumber: string = this.empCodeControl.value;
  //  let index = -1;
  //  this.employeeNumber = empNumber;
  //  this.apiService.getall('Employee/getEmployeeByEmployeeNumber/' + empNumber).subscribe(res => {
  //    if (res != null) {
  //      this.employeeName = res['employeeName'];
  //      this.form.value['employeeNumber'] = empNumber;
  //      this.form.value['employeeName'] = res['employeeName'];
  //      this.s = [];
  //      for (var i = 1; i <= 31; i++) {
  //        this.s.push('');
  //      }
  //      this.resetShiftsFormControls();

  //      index = this.listOfRoasters.findIndex(s => s.employeeNumber == this.employeeNumber);
  //      console.log("index=" + index);
  //      if (index >= 0)
  //        this.editRoasterEntry(index);
  //    }
  //    else {
  //      this.empCodeControl.setValue('');
  //      this.employeeNumber = '';
  //      this.employeeName = '';
  //      this.resetShiftsFormControls();
  //    }

  //  });

  //}

  onSelectSiteCode(event: any) {
    const siteCode= event.target.value;;

    this.form.value['siteCode'] = siteCode;

    this.form.controls['siteCode'].setValue(siteCode);
    this.siteCode = siteCode;

    this.loadData();
  }

  loadData() {

    if (this.customerCode != '' && this.siteCode != '' && this.month != 0 && this.year != 0) {

      let fromDate = new Date(this.year, this.month - 1, 1);
      let toDate = new Date(this.year, this.month, 0);
      this.listOfDays = [];
      this.noOfDays = toDate.getDate() - fromDate.getDate() + 1;

      this.apiService.getall(`MonthlyRoaster/getMonthlyRoaster/${this.customerCode}/${this.siteCode}/${this.month}/${this.year}`).subscribe(res => {
        this.listOfRoasters = res;
        this.sequence = this.listOfRoasters.length + 1;

      });

      this.loadWeekDays();
      this.shiftsFormControls.clear();
      for (var i = 1; i <= this.noOfDays; i++) {
        this.shiftsFormControls.push(new FormControl(''));
      }
      this.loadShiftCodes();
      this.listOfShiftCodes = [];
    }
    else {
      this.listOfRoasters = [];
    }
   
  }

  loadWeekDays() {
    this.form.controls['monthYear'].setValue(new Date(this.year, this.month - 1, this.day));
    for (var i = 1; i <= this.noOfDays; i++) {
      let day = new Date(this.year, this.month - 1, i);

      let dayText = day.toDateString().substring(0, 1);

      let DayObj: any = { dayText: dayText, dayNumber: i, dayFullName: day.toDateString() };
      this.listOfDays.push(DayObj);
    }

  }
  loadSiteCodes(custCode: string) {
    this.apiService.getall(`CustomerSite/getSelectSiteListByCustCode/${custCode}`).subscribe(res => {
      this.siteCodeList = res;
    });

  }
  //loadProjectCodes(custCode: string) {
  //  this.apiService.getall(`Project/getSelectProjectList`).subscribe(res => {
  //      this.projectCodeList = res;
  //    });
  //}

  //onSelectShift(event: any, index: number) {

  //  if (this.newTotalShifts != 0) {
  //    this.s[index] = event.target.value;
  //    this.shiftsFormControls.controls[index].setValue(event.target.value);
  //  }
  //  else {
  //    for (var i = 1; i <= this.noOfDays; i++) {
  //      this.s[i - 1] = event.target.value;
  //      this.shiftsFormControls.controls[i - 1].setValue(event.target.value);

  //    }

  //  }
  //  this.findTotalNewShifts();
  //}
  //resetShiftsFormControls() {
  //  this.shiftsFormControls.clear();
  //  for (var i = 1; i <= this.noOfDays; i++) {
  //    this.shiftsFormControls.push(new FormControl(''));
  //    this.s[i - 1] = '';
  //  }
  //  this.findTotalNewShifts();
  //}
  openDatePicker(dp:any) {
    dp.open();
  }

  closeDatePicker(eventData: any, dp?: any) {

    let picker = moment as any;
    let pickerDate = picker(eventData).format('YYYY-MM');
   
    this.day = parseInt(pickerDate.toString().substring(8, 10));
    this.month = parseInt(pickerDate.toString().substring(5, 7));
    this.year = Number(pickerDate.toString().substring(0, 4));
    if (this.customerCode != '' && this.siteCode != '') {
      this.loadData();
    }
   
  }

  findNoOfShifts() {
    this.totalShiftsRow = [];
    this.totalShiftsCol = [];
    this.grandTotalShifts = 0;
    let offShift = this.listOfShiftCodes.find(e => e.textTwo == "True")?.value as string;
    for (var k = 0; k < this.noOfDays; k++) {

      let cc = 0;
      for (var i = 0; i < this.listOfRoasters.length; i++) {
        let rc = 0;
        let r = this.listOfRoasters[i];
        for (var j = 0; j < r.s.length; j++) {
          if (r.s[j] == offShift || r.s[j] == '' || r.s[j] == null) { }
          else {
            rc++;
            if (j == k)
              cc++;
          }
        }
        this.totalShiftsRow.push(rc);
      }
      this.totalShiftsCol.push(cc);
      this.grandTotalShifts = this.grandTotalShifts + this.totalShiftsCol[k];
    }
  }

  //findTotalNewShifts() {
  //  let offShift = this.listOfShiftCodes.find(e => e.textTwo == "True").value;
  //  let count = 0;
  //  for (var i = 0; i < this.noOfDays; i++) {
  //    if (this.s[i] == offShift || this.s[i] == '' || this.s[i] == null) { }

  //    else {
  //      count++;
  //    }
  //    this.newTotalShifts = count;
  //  }
  //}

  RoastersCount(): number {
    return this.listOfRoasters.length;
  }

  getCellInfo(r: number, c: number) {
    return `Date:${(new Date(this.year, this.month, this.day)).toDateString()}
Shift: ${this.getShiftInfo(r, this.day)}`;
  }
  getShiftInfo(r: number, c: number) {

    return `${this.listOfRoasters[r].s[c]}`
  }

  //editRoasterEntry(r: number) {
  //  let roaster: any = this.listOfRoasters[r];

  //  this.employeeNumber = roaster.employeeNumber;
  //  this.employeeName = roaster.employeeName;
  //  this.empCodeControl.setValue(this.employeeNumber);
  //  this.s = roaster.s.slice();
  //  for (var i = 1; i <= this.noOfDays; i++) {
  //    this.shiftsFormControls.controls[i - 1].setValue(this.s[i - 1]);
  //    this.empCodeControl.setValue(this.employeeNumber);
  //  }
  //  this.findTotalNewShifts();
  //  let ele = <HTMLElement>document.getElementById('inputEntry');
  //  ele.scrollIntoView();



  //}
  getDay(r: roaster) {
    return r.s[this.day];
  }


}
