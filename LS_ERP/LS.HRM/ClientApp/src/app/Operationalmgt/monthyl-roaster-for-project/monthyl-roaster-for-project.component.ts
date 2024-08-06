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

import { AttendanceFromMonthlyRoasterComponent } from '../attendance-from-monthly-roaster/attendance-from-monthly-roaster.component';


export interface CustomSelectShift {
  text: string;
  value: string;
  textTwo: string;
}
export interface roaster {

  id:number,
  customerCode: string,
  siteCode: string,
  projectCode: string,
  month: number,
  year: number,
  skillsetCode: string,
  skillsetName: string,
  shiftCodesForMonth: Array<string>
}




@Component({
  selector: 'app-monthyl-roaster-for-project',
  templateUrl: './monthyl-roaster-for-project.component.html'
})
export class MonthylRoasterForProjectComponent extends ParentOptMgtComponent implements OnInit {

  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
  readonly: string = "";

  isDataLoading: boolean = false;
  invoiceItemObject: any;
  listOfRoasters: Array<any> = [];
  listOfDays: Array<any> = [];

  sequence: number = 1;
  editsequence: number = 0;
  remarks: string = '';

  shiftsFormControls = new FormArray([new FormControl('')]);

  noOfDays: number = 0;
  saveEnable: boolean = false;
  editEnable: boolean = false;
  monthYear: string = '';
  skillsetCode: string = '';
  skillsetName: string = '';
  shiftCodesForMonth: Array<string> = [];
  inActiveDays: number = 0;
  siteCodeList: Array<CustomSelectListItem> = [];

  filteredCustCodes: Observable<Array<CustomSelectListItem>>;
  filteredskillsetCodes: Observable<Array<CustomSelectListItem>>;
  customerCode: string = '';
  listOfShiftCodes: Array<any> = [];


  siteCode: string = '';

  month: number = 0;
  year: number = 0;
  shiftCode: string = '';

  totalShiftsRow: Array<number> = [];
  totalShiftsCol: Array<number> = [];
  grandTotalShifts: number = 0;
  newTotalShifts: number = 0;
  project: any;
  skillsetSelectionList: Array<any>;
  selectedMonth: string;
  shiftsReport: Array<any> = [];





  empCodeControl = new FormControl('');
  filteredEmployeeNumbers: Observable<Array<CustomSelectListItem>>;
  employeeList: Array<CustomSelectListItem> = [];
  employeeNumber: string = '';
  minDate: Date;
  maxDate: Date;

  employeeID: number;
  mapId: number;
  isPrimaryResource: boolean;

  isLoading: boolean = false;

  isArab: boolean = false;
  canSave: boolean = false;

  workingHoursInSite: number = 0;

  filterEmployeeNumbers(val: string): Observable<Array<CustomSelectListItem>> {
    return this.apiService.getall(`Employee/getAutoFillEmployeeList?search=${val}`)
      .pipe(
        map(response => {
          const res = response as Array<CustomSelectListItem>;
          this.isDataLoading = false;
          return res;
        })
      )
  }
  constructor(public dialogRef: MatDialogRef<AttendanceFromMonthlyRoasterComponent>, private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService) {
    super(authService);
    this.filteredEmployeeNumbers = this.empCodeControl.valueChanges.pipe(
      startWith(this.employeeNumber),
      debounceTime(utilService.autoDelay()),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isDataLoading = true;
        return this.filterEmployeeNumbers(val || '')
      })
    );

  }
  loadShiftCodes() {

    this.apiService.getall(`ShiftMaster/getShiftsByProjectAndSiteCode/${this.project.projectCode}/${this.siteCode}`).subscribe(res => {
      this.listOfShiftCodes = res;
      this.findNoOfShifts();

    });
  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();

   
    this.setForm();
    this.readonly = "readonly";
    this.loadSiteCodes(this.project.customerCode);
    this.loadSkillsetSelectionList();

  }

  setForm() {
    this.form = this.fb.group({
      "startDate": [''],
      "endDate": [''],
      "customerCode": [this.project.customerCode],
      "monthYear": [''],
      'siteCode': [this.project?.siteCode != null ? this.project.siteCode : ''],
      "projectCode": [this.project.projectCode],
      "month": [0],
      "year": [this.year],
      "skillsetCode": [''],
      'shiftCode': [''],
    });

    if (this.project?.siteCode != null) {
      this.siteCode = this.project.siteCode;
      this.form.controls['siteCode'].disable({ onlySelf: true });
      this.minDate = this.project.startDate;
      this.maxDate = this.project.endDate;
      this.form.controls['startDate'].setValue(this.project.startDate.toString().substring(0, 10));
      this.form.controls['endDate'].setValue(this.project.endDate.toString().substring(0, 10));
      let event: any = {
         target: { value: this.project.siteCode } 
      };
      this.onSelectSiteCode(event);
    }

  }
  submit() {





    if (this.saveEnable && !this.isLoading) {

      this.isLoading = true;

     this.canSave = true;
      for (var i = 0; i < this.listOfRoasters.length; i++) {
        if (this.listOfRoasters[i].shiftCodesForMonth.findIndex((e: string) => e == "")>=0) {
          this.canSave = false;
          break;
        }
      }


      this.form.value['roastersList'] = this.listOfRoasters;

      if (/*this.form.valid &&*/this.listOfRoasters.length > 0 && this.canSave) {
        this.apiService.post('MonthlyRoaster/updateShiftsForMonthlyRoaster', this.listOfRoasters)
          .subscribe(res => {
            this.utilService.OkMessage();
            this.isLoading = false;
            this.closeModel();
          },
            error => {
              console.log(error)
              console.error(error);
              this.utilService.ShowApiErrorMessage(error);
              this.isLoading = false;
            });

      }
      else if (!this.canSave) {
        this.notifyService.showError(this.translate.instant("InComplete_Shifts_Assignment"));
      }
      else
        this.utilService.FillUpFields();

    }
    else if (this.isLoading) {
      this.notifyService.showError(this.translate.instant("Please_Wait"));

    }
  }
  getSkillset(code: string): any {
    return this.skillsetSelectionList.find(e => e.value == code);

  }
  loadSkillsetSelectionList() {

    this.apiService.getall(`Skillset/getSelectSkillsetList`).subscribe(res => {
      this.skillsetSelectionList = res;

    });

  }





  addEntry() {

    if (/*this.skillsetCode != '' && this.skillsetCode != null && */this.newTotalShifts != 0) {

      //let index = this.listOfRoasters.findIndex(s => s.id == this.id);
      let index = this.listOfRoasters.findIndex(s => s.employeeNumber == this.employeeNumber);

      let entry: any = {
        id: this.id,
        customerCode: this.project.customerCode,
        siteCode: this.siteCode,
        projectCode: this.project.projectCode,
        month: this.month,
        year: this.year,
        skillsetCode: this.skillsetCode,
        skillsetName: this.skillsetName,
        employeeNumber: this.employeeNumber,
        shiftCodesForMonth: this.shiftCodesForMonth.slice(),
        employeeID: this.employeeID,
        mapId: this.mapId,
        isPrimaryResource: this.isPrimaryResource,
      };

      /*  console.log("index=" + index);*/
      if (index != -1) {

        this.removeEntry(index);
        this.listOfRoasters.splice(index, 0, entry);
      }
      else {
        this.listOfRoasters.push(entry);
      }
      this.findNoOfShifts();
      this.saveEnable = true;
      this.skillsetCode = '';
      this.employeeNumber = '';
    
      this.resetShiftsFormControls();
    }
  }


  clearEntry() {

   

     
    for (var i = 1; i <= this.noOfDays; i++) {
      if (this.shiftsFormControls.value[i - 1] != "x") this.shiftsFormControls.controls[i - 1].setValue('');
      
      
    }
    this.newTotalShifts =0;
    
  }




  removeEntry(i: number) {
    this.listOfRoasters.splice(i, 1);
    this.saveEnable = true;
    this.findNoOfShifts();
  }


  onSelectSiteCode(event: any) {
    if (event.target.value != '') {

      let siteCode = event.target.value;
      this.form.controls['monthYear'].setValue('');
      this.month = 0;
      this.year = 0;
      this.apiService.getall(`ProjectSites/getProjectSiteByProjectAndSiteCode/${this.project.projectCode}/${siteCode}`).subscribe(res => {

        this.project = res;
        this.workingHoursInSite = res.siteWorkingHours;
        this.loadEmployees();
        //let startDate = new Date(this.project.startDate);
        //let endDate = new Date(this.project.endDate);


        //this.minDate = startDate;
        //this.maxDate = endDate;

      
        //this.form.controls['startDate'].setValue(this.project.startDate.toString().substring(0, 10));
        //this.form.controls['endDate'].setValue(this.project.endDate.toString().substring(0, 10));


        let date = new Date(Date.now());
        this.year = date.getFullYear();
        this.form.value['siteCode'] = siteCode;
        this.form.controls['siteCode'].setValue(siteCode);
        this.siteCode = siteCode;
        
        this.loadData();

      });




    }
    else {
      this.listOfRoasters = [];
    }


  }

  loadData() {



    if (this.project.customerCode != '' && this.siteCode != '' && this.month != 0 && this.year != 0) {

      let fromDate = new Date(this.year, this.month - 1, 1);
      let toDate = new Date(this.year, this.month, 0);

      this.listOfDays = [];
      this.noOfDays = toDate.getDate() - fromDate.getDate() + 1;

      this.listOfRoasters = [];

      this.apiService.getall(`MonthlyRoaster/getMonthlyRoasterForSite/${this.project.customerCode}/${this.project.projectCode}/${this.siteCode}/${this.month}/${this.year}`).subscribe(res => {
      
        this.listOfRoasters = res as Array<any>;
       



        if (this.listOfRoasters.length == 0)
          this.notifyService.showError(this.translate.instant("Roaster_Not_Generated"));
        else {
          for (let i = 0; i < this.listOfRoasters.length;i++)
          {
            if (!this.listOfRoasters[i].isPrimaryResource)
            {
              this.listOfRoasters.splice(i, 1);
              i--;
            }
          }




        }

        this.sequence = this.listOfRoasters.length + 1;
      });

      this.loadWeekDays();
      this.shiftsFormControls.clear();
      for (var i = 1; i <= this.noOfDays; i++) {

        this.shiftsFormControls.push(new FormControl(''));
        this.shiftsFormControls.controls[i - 1].disable({ onlySelf: true });


      }
      this.loadShiftCodes();
      this.listOfShiftCodes = [];

    }
    else {
      this.listOfRoasters = [];


    }

  }

  loadWeekDays() {
    this.form.controls['monthYear'].setValue(new Date(this.year, this.month - 1, 1));
    for (var i = 1; i <= this.noOfDays; i++) {
      let day = new Date(this.year, this.month - 1, i);

      let dayText = day.toDateString().substring(0, 1);

      let DayObj: any = { dayText: dayText, dayNumber: i, dayFullName: day.toDateString() };
      this.listOfDays.push(DayObj);
    }

  }
  loadSiteCodes(custCode: string) {
    this.apiService.getall(`customerSite/getSelectSiteListByProjectCode/${this.project.projectCode}`).subscribe(res => {
      this.siteCodeList = res;

    });

  }


  onSelectShift(event: any, index: number) {
  
    let shft = this.listOfShiftCodes.find(e => e.shiftCode == event.target.value);
    if (this.newTotalShifts != 0 && event.target.value != '') {
      
      if (shft.isOff) {

        for (var i = 1; i <= this.noOfDays; i++) {
          if (this.shiftCodesForMonth[i - 1] != 'x') {
            if ((i-1) % 7 == index % 7) {
              this.shiftCodesForMonth[i - 1] = event.target.value;
              this.shiftsFormControls.controls[i - 1].setValue(event.target.value);

            }


          }
        }



      }
      else {

        this.shiftCodesForMonth[index] = event.target.value;
        this.shiftsFormControls.controls[index].setValue(event.target.value);
      }


    }
    else if (event.target.value != '') {
      let shft = this.listOfShiftCodes.find(e => e.shiftCode == event.target.value);
      if (shft.isOff) {

        for (var i = 1; i <= this.noOfDays; i++) {
          if (this.shiftCodesForMonth[i - 1] != 'x') {
            if ((i-1)%7==index%7) {
              this.shiftCodesForMonth[i - 1] = event.target.value;
              this.shiftsFormControls.controls[i - 1].setValue(event.target.value);

            }

            
          }
        }



      }
      else {
        for (var i = 1; i <= this.noOfDays; i++) {
        if (this.shiftCodesForMonth[i - 1] != 'x') {



          this.shiftCodesForMonth[i - 1] = event.target.value;
          this.shiftsFormControls.controls[i - 1].setValue(event.target.value);
        }
      }
      }
      

    }

    this.findTotalNewShifts();
  }
  resetShiftsFormControls() {

    this.shiftsFormControls.clear();
    for (var i = 1; i <= this.noOfDays; i++) {
      this.shiftsFormControls.push(new FormControl(''));
      this.shiftCodesForMonth[i - 1] = '';
      this.shiftsFormControls.controls[i - 1].disable({ onlySelf: true });
    }
    this.findTotalNewShifts();
  }
  openDatePicker(dp: any) {
    dp.open();
  }

  closeDatePicker(eventData: any, dp?: any) {
    let picker = moment as any;
    let pickerDate = picker(eventData).format('YYYY-MM');

    this.selectedMonth = eventData.toString().substring(4, 7) + '-' + eventData.toString().substring(11, 15);



    this.month = parseInt(pickerDate.toString().substring(5, 7));
    this.year = Number(pickerDate.toString().substring(0, 4));
    if (this.project.customerCode != '' && this.siteCode != '') {


      this.loadData();

    }
    dp.close();

  }

  findNoOfShifts() {



    this.totalShiftsRow = [];
    this.totalShiftsCol = [];
    this.grandTotalShifts = 0;


    for (var k = 0; k < this.noOfDays; k++) {

      let cc = 0;
      for (var i = 0; i < this.listOfRoasters.length; i++) {
        let rc = 0;
        let r = this.listOfRoasters[i];
        for (var j = 0; j < r.shiftCodesForMonth.length; j++) {

          rc = rc + this.shiftValue(r.shiftCodesForMonth[j]);
          if (j == k)
            cc = cc + this.shiftValue(r.shiftCodesForMonth[j]);

        }
        this.listOfRoasters[i].totShifts = rc;
        this.totalShiftsRow.push(rc);

      }
      this.totalShiftsCol.push(cc);
      this.grandTotalShifts = this.grandTotalShifts + this.totalShiftsCol[k];
    }
    this.getShiftsReport();
  }



  findTotalNewShifts() {
    let count = 0;
    for (var i = 0; i < this.noOfDays; i++) {
      count = count + this.shiftValue(this.shiftCodesForMonth[i]);
    }
    this.newTotalShifts = count;

  }


  shiftValue(shiftCode: string): number {
  

    let shft = this.listOfShiftCodes.find(e => e.shiftCode == shiftCode);

    if (shft == null || shiftCode == '' || shiftCode == null || shiftCode == "")
      return 0;
    else if (shft.isOff == false) {
      return 1;
    }
    else return 0;
  }







  RoastersCount(): number {

    return this.listOfRoasters.length;
  }

  getCellInfo(r: number, c: number) {
    return `Date:${(new Date(this.year, this.month - 1, c + 1)).toDateString()}
Shift: ${this.getShiftInfo(r, c)}`;
  }
  getShiftInfo(r: number, c: number) {

    return `Shift Name: ${this.listOfRoasters[r].shiftCodesForMonth[c]}`
  }

  editRoasterEntry(r: number) {

    let roaster: any = this.listOfRoasters[r];

    this.id = roaster.id;

    this.skillsetName = roaster.skillsetName;
    this.skillsetCode = roaster.skillsetCode;
    this.employeeNumber = roaster.employeeNumber;
    this.employeeID = roaster.employeeID;
    this.mapId = roaster.mapId;
    this.isPrimaryResource = roaster.isPrimaryResource;
    //this.form.controls['skillsetCode'].setValue(this.skillsetCode);

    this.shiftCodesForMonth = roaster.shiftCodesForMonth.slice();

    for (var i = 1; i <= this.noOfDays; i++) {
      this.shiftsFormControls.controls[i - 1].setValue(this.shiftCodesForMonth[i - 1]);
      if (this.shiftCodesForMonth[i - 1] == 'x') {
        this.shiftsFormControls.controls[i - 1].disable({ onlySelf: true });

      }
      else
        this.shiftsFormControls.controls[i - 1].enable({ onlySelf: true });

    }
    this.findTotalNewShifts();
    let ele = <HTMLElement>document.getElementById('inputEntry');
    ele.scrollIntoView();



  }
  closeModel() {
    this.dialogRef.close();
  }
  canAddShifftCode(col: number): boolean {
    return this.listOfRoasters[0].shiftCodesForMonth[col] != 'x';
  }

  

  groupBy(list: Array<any>, keyGetter: any) {
    const map = new Map();
    list.forEach((item: any) => {
      const key = keyGetter(item);
      const collection = map.get(key);
      if (!collection) {
        map.set(key, [item]);

      } else {
        collection.push(item);
      }
    });
    return map;
  }


  loadEmployees() {
    this.apiService.getall('Employee/getSelectEmployeeList').subscribe(res => {
      this.employeeList = res;
    });
  }


 

  getEmployee(employeeNumber: string) {

    return this.employeeList.find(e => e.value == employeeNumber);

  }


  getShiftsReport() {
    this.apiService.getall(`Skillset/getSkillsetsByProjectCodeAndSiteCode/${this.project.projectCode}/${this.siteCode}`).subscribe((ssPlan:Array<any>) => {
      //console.log(ssPlan);
      //console.log(this.listOfRoasters);
      this.shiftsReport = [];
      ssPlan.forEach((ss: any) => {
       // console.log(ss);
        let monthlyShifts: number = 0;
        let monthlyOffs: number = 0;
       // console.log(this.listOfRoasters.filter(e => e.skillSetCode == ss.skillsetCode));
        this.listOfRoasters.filter(e => e.skillsetCode == ss.skillSetCode).forEach((roaster: any) => {



          roaster.shiftCodesForMonth.forEach((shiftCode: any) => {



          
            let isOff: boolean = this.listOfShiftCodes.find(e => e.shiftCode == shiftCode)?.isOff;

            if (this.listOfShiftCodes.findIndex((e: any) => shiftCode!='x' && e.shiftCode ==shiftCode)>=0 && isOff) {
              monthlyOffs++;
            }
            else if (this.listOfShiftCodes.findIndex((e: any) => e.shiftCode == shiftCode) >= 0 && !isOff && shiftCode != 'x' && shiftCode != '') {
              monthlyShifts++;

            }




          });

          

        });



        let shift: any = {
          position: ss.skillSetCode,
          dailyShifts: ss.quantity,
          dailyHrs: ss.quantity * this.workingHoursInSite,
         // monthlyShifts: monthlyShifts,

          monthlyHrs: this.workingHoursInSite * (ss.quantity * this.noOfDays),

          monthlyOffs: monthlyOffs,
          monthlyReplacements: monthlyShifts + monthlyOffs - (this.noOfDays * ss.quantity),
          monthlyShiftsSchedulled:ss.quantity*this.noOfDays,
          monthlyShiftsAssigned: monthlyShifts
        }
        this.shiftsReport.push(shift)


      });
    });

   
  }

  }













