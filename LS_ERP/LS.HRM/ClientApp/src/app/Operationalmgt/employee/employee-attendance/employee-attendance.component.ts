import { HttpClient } from '@angular/common/http';
import { Component, OnChanges, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { from, Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, mergeMap, startWith, switchMap, timestamp } from 'rxjs/operators';

import * as moment from "moment";
import { Moment } from "moment";
import { MatFormFieldModule } from '@angular/material/form-field';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';
import { LanCustomSelectListItem } from '../../../models/MenuItemListDto';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { NotificationService } from '../../../services/notification.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { UtilityService } from '../../../services/utility.service';
import { ApiService } from '../../../services/api.service';
import { roaster } from '../../models/roaster.model';
import { DBOperation } from '../../../services/utility.constants';
import { OprServicesService } from '../../opr-services.service';
import { ManualAttendancePopupComponent } from './manual-attendance-popup/manual-attendance-popup.component';
import { Timestamp } from 'rxjs/internal/operators/timestamp';
import { UpdateManualAttendancePopupComponent } from './update-manual-attendance-popup/update-manual-attendance-popup.component';
import { ConfirmDialogWindowComponent } from '../../confirm-dialog-window/confirm-dialog-window.component';
import * as XLSX from "xlsx";
import { UpdateEmployeeShiftComponent } from './manual-attendance-popup/update-employee-shift/update-employee-shift.component';
import { PostattendancewithdateComponent } from './postattendancewithdate/postattendancewithdate.component';
import { ClearattendancewithdateComponent } from './clearattendancewithdate/clearattendancewithdate.component';
import { SwapEmployeesComponent } from '../../ProgectManagement/swap-employees/swap-employees.component';
import { CreateUpdateTransferWithReplacementComponent } from '../../ProgectManagement/transfer-with-replacement/create-update-transfer-with-replacement/create-update-transfer-with-replacement.component';
import { CreateUpdateTransferResourceReqComponent } from '../../ProgectManagement/transfer-employee/create-update-transfer-resource-req/create-update-transfer-resource-req.component';
import { CreateUpdateReqRemoveResourceComponent } from '../../ProgectManagement/remove-resource/create-update-req-remove-resource/create-update-req-remove-resource.component';
import { PvEmpToResMapComponent } from '../../ProgectManagement/add-resource/pv-emp-to-res-map/pv-emp-to-res-map.component';
import { CreateUpdateReqReplaceEmployeeComponent } from '../../ProgectManagement/replace-employee/create-update-req-replace-employee/create-update-req-replace-employee.component';

@Component({
  selector: 'app-employee-attendance',
  templateUrl: './employee-attendance.component.html'
})
export class EmployeeAttendanceComponent extends ParentOptMgtComponent implements OnInit {

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

  //shiftsFormControls = new FormArray([new FormControl('')]);

  noOfDays: number = 0;
  saveEnable: boolean = false;
  editEnable: boolean = false;
  monthYear: string = '';
  skillsetCode: string = '';
  skillsetName: string = '';
  shiftCodesForMonth: Array<string> = [];

  inActiveDays: number = 0;
  siteCodeList: Array<any> = [];

  filteredCustCodes: Observable<Array<LanCustomSelectListItem>>;
  filteredskillsetCodes: Observable<Array<LanCustomSelectListItem>>;
  customerCode: string = '';
  listOfShiftCodes: Array<any> = [];
  listOfShiftCodes2: Array<any> = [];


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
  roasterReport: Array<any> = [];





  empCodeControl = new FormControl('');
  filteredEmployeeNumbers: Observable<Array<LanCustomSelectListItem>>;
  employeeList: Array<LanCustomSelectListItem> = [];
  employeeNumber: string = '';

  minDate: Date;
  maxDate: Date;
  maxPostDate: Date;
  minPostDate: Date;


  employeeID: number;
  mapId: number;


  attendanceData: any[][];
  workingHoursInSite: number = 0;

  autoAttendanceData: Array<any> = [];
  //canEnterAttendance: boolean=false;
  activeColumn: number = -1;

  updatedAttendance: Array<any> = [];
  updatedAttendance2: Array<any> = [];
  notPostedAtndncExist: boolean = false;
  isLoading: boolean = false;
  employeeConsolidateData: Array<any> = [];
  daywiseConsolidateData: Array<any> = [];
  colours: Array<string> = ["bg_Shift0", "bg_Shift1", "bg_Shift2", "bg_Shift3","bg_Shift4"];
  shiftTypeNames: Array<any> = [{ number: 0, nameEn: "-", nameAr: "-" },
    { number: 1, nameEn: "Shift 1", nameAr: "الوردية الأولى" },
    { number: 2, nameEn: "Shift 2", nameAr: "الوردية الثانية" },
    { number: 3, nameEn: "Shift 3", nameAr: "الوردية الثالثة" },
    { number: 4, nameEn: "Shift 4", nameAr: "الوردية الثالثة" }];
  showMins: boolean = true;
  siteData: any;
  totalData: any = {
    total: 0,
    offs: 0,
    totMins: 0,
    nwd: 0,
    wd: 0,
    wh: 0,
    lateHrs: 0,
    lateMins: 0,
    overTime: 0,
    overTimeMins: 0,
    notAssigned: 0,
    actualShifts: 0,
    al_count: 0,
    ul_count: 0,
    el_count: 0,
    sl_count: 0,
    stl_count: 0,
    w_count: 0,
    penalty: 0
  };
  totShiftReport: any = {
    dailyShifts: 0,
    dailyHrs: 0,
    monthlyHrs: 0,
    monthlyOffs: 0,
    monthlyReplacements: 0,
    monthlyShiftsSchedulled: 0,
    monthlyShiftsAssigned: 0
  };
  totRoastersReport: any = {
    contractualShifts: 0,
    offs: 0,
    qty:0
  }






  inputArray: Array<any> = [];
  inputListForDay: Array<any> = [];





  filterEmployeeNumbers(val: string): Observable<Array<LanCustomSelectListItem>> {
    return this.apiService.getall(`Employee/getAutoFillEmployeeList?search=${val}`)
      .pipe(
        map(response => {
          const res = response as Array<LanCustomSelectListItem>;
          this.isDataLoading = false;
          return res;
        })
      )
  }

  isArab: boolean=false;
  constructor(public dialog: MatDialog, private oprService: OprServicesService, public dialogRef: MatDialogRef<EmployeeAttendanceComponent>, private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
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

    //this.apiService.getall(`ShiftMaster/getShiftsByProjectAndSiteCode/${this.project.projectCode}/${this.siteCode}`).subscribe(res => {
    //  this.listOfShiftCodes = res;
    //});
      this.apiService.getall(`ShiftMaster/getShiftsByProjectAndSiteCode2/${this.project.projectCode}/${this.siteCode}`).subscribe(res2 => {
        this.listOfShiftCodes2 = res2 as Array<any>;
       // this.getShiftsReport();
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
      this.form.controls['siteCode'].disable({ onlySelf: true });
      this.siteCode = this.project.siteCode;
      //this.minDate = this.project.startDate;
      this.minDate = this.project.oldestRoasterStartDate;
      this.maxDate = this.project.endDate;

      this.form.controls['startDate'].setValue(this.project.startDate.toString().substring(0,10));
      this.form.controls['endDate'].setValue(this.project.endDate.toString().substring(0, 10));
      let event: any = {
        target: { value: this.project.siteCode }
      };
      this.onSelectSiteCode(event);





    }
  }



  getMaxPostDate() {
    this.minPostDate = new Date(this.year, this.month-1, 1);
    let selectedMonth = new Date(this.year, this.month,1);
    selectedMonth.setMinutes(selectedMonth.getMinutes() - selectedMonth.getTimezoneOffset());
    let monthLastDate = new Date(selectedMonth.getFullYear(), selectedMonth.getMonth(), 0, 23, 59, 59, 999);

    this.maxPostDate = monthLastDate > this.maxDate ? this.maxDate : monthLastDate;

  }
  submit() {


  }
  getSkillset(code: string): any {
    return this.skillsetSelectionList.find(e => e.value == code);

  }
  loadSkillsetSelectionList() {

    this.apiService.getall(`Skillset/getSelectSkillsetList`).subscribe(res => {
      this.skillsetSelectionList = res;
      
    });

  }


  getEmployee(code: string): any {


    return this.employeeList.find(e => e.value == code);

  }





  onSelectSiteCode(event: any) {
    if (event.target.value != '') {
      let siteCode = event.target.value;
      this.form.controls['monthYear'].setValue('');
      this.month = 0;
      this.year = 0;
      this.apiService.getall(`ProjectSites/getProjectSiteByProjectAndSiteCode/${this.project.projectCode}/${siteCode}`).subscribe(res => {

        this.project = res;
        this.workingHoursInSite = res.siteWorkingHours ?? 0;

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

    

      this.apiService.getall(`MonthlyRoaster/getMonthlyRoasterForSite/${this.project.customerCode}/${this.project.projectCode}/${this.siteCode}/${this.month}/${this.year}`).subscribe(res => {
        this.listOfRoasters = res;
        if (res.length != 0) {

       


          if (this.listOfRoasters.length == 0)
            this.notifyService.showError(this.translate.instant("Roaster_Not_Generated"));
          else {
            this.isLoading = true;
            
            this.getAllAttendance();
            this.getActiveColumn();
          }

          this.sequence = this.listOfRoasters.length + 1;
        }
        else {
          this.notifyService.showError(this.translate.instant("Roaster_Not_Generated"));
          this.isLoading = false;
        }

      });

      this.loadWeekDays();
     
      this.loadShiftCodes();
  

    }
    else {
      this.listOfRoasters = [];
      this.listOfShiftCodes = [];

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



  openDatePicker(dp: any) {
    dp.open();
  }

  closeDatePicker(eventData: any, dp?: any) {

    let picker = moment as any;
    let pickerDate = picker(eventData).format('YYYY-MM');

    this.selectedMonth = eventData.toString().substring(4, 7) + '-' + eventData.toString().substring(11, 15);



    this.month = parseInt(pickerDate.toString().substring(5, 7));
    this.year = Number(pickerDate.toString().substring(0, 4));
    this.getMaxPostDate();


    if (this.project.customerCode != '' && this.siteCode != '') {



      this.isLoading = true;
      this.loadData();
      this.RefreshOffs();
      this.utilService.autoDelay();


    }
    dp.close();

  }



  RefreshOffs() {

   

      let dto: any = {
       // projectCode: this.project.projectCode,
        siteCode: this.project.siteCode,
        month: this.month,
        year: this.year,
        //fromDate: '2020-09-01',
       // toDate: null,
      //  employeeNumber:null,
      };

      this.apiService.post('OpUtils/RefreshOffs', dto).subscribe(res1 => {

      });

   


  }




  RoastersCount(): number {

    return this.listOfRoasters.length;
  }







  getShiftInfo(r: number, c: number) {

    return `Shift Name: ${this.listOfRoasters[r]?.shiftCodesForMonth[c]}`
  }


  closeModel() {

    this.dialogRef.close();

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
    this.apiService.getall('Employee/getSelectEmployeeList2').subscribe(res => {
      this.employeeList = res;
    });
  }





  private openDialogWindow(inputCellData: any, inputData: any, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {



    if (!this.isLoading) {
      let dialogRef = this.oprService.openAutoWidthDialog(this.dialog, Component);
      (dialogRef.componentInstance as any).dbops = dbops;
      (dialogRef.componentInstance as any).modalTitle = modalTitle;
      (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
      (dialogRef.componentInstance as any).inputData = inputData;
      (dialogRef.componentInstance as any).inputCellData = inputCellData;


      dialogRef.afterClosed().subscribe(outputFromManualAtt => {


        if (outputFromManualAtt?.res == true && outputFromManualAtt?.isNotPrimaryAtt == true) {
         
          let c = outputFromManualAtt.c;

          this.apiService.getall(`MonthlyRoaster/getMonthlyRoasterForSite/${this.project.customerCode}/${this.project.projectCode}/${this.siteCode}/${this.month}/${this.year}`).subscribe(roastersRes => {
            this.listOfRoasters = roastersRes;
            for (let r = 0; r < this.listOfRoasters.length; r++) {

              let date: string = this.year + "-" + this.month.toString().padStart(2, "0") + "-" + (c + 1).toString().padStart(2, "0");
              
              this.apiService.getall(`EmployeesAttendance/getSingleEmployeeAttendance/${this.project.projectCode}/${this.siteCode}/${this.listOfRoasters[r]?.employeeNumber}/${date}/${this.listOfRoasters[r]?.shiftCodesForMonth[c]}`).subscribe((res: any) => {
                this.attendanceData[r][c] = res;
              });
            }

          });

        }

      else  if (outputFromManualAtt?.res == true && outputFromManualAtt?.isRoasterRemoved == true) {
          this.loadData();            //removed Roaster While Cancel attendance
        }
        

        else if (outputFromManualAtt?.res == true && outputFromManualAtt?.isUpdatedShift == true) {



          let r = outputFromManualAtt.r;
          let c = outputFromManualAtt.c;
          let date: string = this.year + "-" + this.month.toString().padStart(2, "0") + "-" + (c + 1).toString().padStart(2, "0");
          this.listOfRoasters[r].shiftCodesForMonth[c] = outputFromManualAtt.updatedShift;        
          this.apiService.getall(`EmployeesAttendance/getSingleEmployeeAttendance/${this.project.projectCode}/${this.siteCode}/${this.listOfRoasters[r]?.employeeNumber}/${date}/${this.listOfRoasters[r]?.shiftCodesForMonth[c]}`).subscribe((res: any) => {
            this.attendanceData[r][c] = res;
          });



        }






       else if (outputFromManualAtt && outputFromManualAtt.res == true) {
          let r = outputFromManualAtt.cellData?.r;
          let c = outputFromManualAtt.cellData?.c;
          let r2 = outputFromManualAtt.cellData?.r;


         
          this.apiService.getall(`MonthlyRoaster/getMonthlyRoasterForSite/${this.project.customerCode}/${this.project.projectCode}/${this.siteCode}/${this.month}/${this.year}`).subscribe(roastersRes => {
            this.listOfRoasters = roastersRes;
           
            if (roastersRes.length == 0)
              this.isLoading = false;
            let date: string = this.year + "-" + this.month.toString().padStart(2, "0") + "-" + (c + 1).toString().padStart(2, "0");
            this.apiService.getall(`EmployeesAttendance/getSingleEmployeeAttendance/${this.project.projectCode}/${this.siteCode}/${this.listOfRoasters[r]?.employeeNumber}/${date}/${this.listOfRoasters[r]?.shiftCodesForMonth[c]}`).subscribe((defEmpAtt: any) => {

              this.attendanceData[r][c] = defEmpAtt;

              if (!defEmpAtt.isPosted && defEmpAtt.id > 0 && (defEmpAtt.employeeNumber == defEmpAtt.altEmployeeNumber || defEmpAtt.altEmployeeNumber == "" || defEmpAtt.altEmployeeNumber == null) && (defEmpAtt.attendance == "P" || defEmpAtt.attendance == "OT")) {
                this.notPostedAtndncExist = false;
                this.updatedAttendance.push(defEmpAtt);
              }
              else
                if (defEmpAtt.altEmployeeNumber != "" && defEmpAtt.id > 0) {

                  this.apiService.get('EmployeesAttendance/getAltAttendanceById', defEmpAtt.id).subscribe(res3 => {
                    if (res3) {

                      if (!res3.isPosted && res3.id > 0 && (res3.employeeNumber == res3.altEmployeeNumber || res3.altEmployeeNumber == "" || res3.altEmployeeNumber == null) && (res3.attendance == "P" || res3.attendance == "OT")) {
                        this.notPostedAtndncExist = false;
                        this.updatedAttendance.push(res3);
                      }

                    }
                  });
                }
              let primEmpRow: number = -1;
            

                r2 = this.listOfRoasters.findIndex(e => e.employeeNumber == outputFromManualAtt.outputData.altEmployeeNumber);
                r = this.listOfRoasters.findIndex(e => e.employeeNumber == outputFromManualAtt.outputData.employeeNumber);
               
                if (outputFromManualAtt.outputData.primaryEmployee != null) {
                  primEmpRow  = this.listOfRoasters.findIndex(e => e.employeeNumber == outputFromManualAtt.outputData.primaryEmployee.employeeNumber);
              }
                if (this.listOfRoasters.length > outputFromManualAtt.cellData?.totRows) {
                  this.attendanceData[r2] = [];

                  for (let c = 0; c < this.noOfDays; c++) {

                    this.attendanceData[r2].push({ id: -1 });
                  }

                  this.loadData();
                }
                else if (primEmpRow >= 0) {

                  let date: string = this.year + "-" + this.month.toString().padStart(2, "0") + "-" + (c + 1).toString().padStart(2, "0");
                  this.apiService.getall(`EmployeesAttendance/getSingleEmployeeAttendance/${this.project.projectCode}/${this.siteCode}/${this.listOfRoasters[primEmpRow].employeeNumber}/${date}/${this.listOfRoasters[primEmpRow].shiftCodesForMonth[c]}`).subscribe((res4: any) => {
                    this.attendanceData[primEmpRow][c] = res4;
                  });
                }



                let date: string = this.year + "-" + this.month.toString().padStart(2, "0") + "-" + (c + 1).toString().padStart(2, "0");
                this.apiService.getall(`EmployeesAttendance/getSingleEmployeeAttendance/${this.project.projectCode}/${this.siteCode}/${this.listOfRoasters[r2].employeeNumber}/${date}/${this.listOfRoasters[r2].shiftCodesForMonth[c]}`).subscribe((res2: any) => {
                  this.attendanceData[r2][c] = res2;
                });

                this.apiService.getall(`EmployeesAttendance/getSingleEmployeeAttendance/${this.project.projectCode}/${this.siteCode}/${this.listOfRoasters[r].employeeNumber}/${date}/${this.listOfRoasters[r].shiftCodesForMonth[c]}`).subscribe((res3: any) => {
                  this.attendanceData[r][c] = res3;
                });
          




              for (let r = 0; r < this.attendanceData.length; r++) {

                for (let c = 0; c < this.attendanceData[r].length; c++) {

                  if (this.activeColumn == -1 && this.attendanceData[r][c].id == 0 && !this.attendanceData[r][c].isDefShiftOff && (!this.attendanceData[r][c]?.isOnLeave && !this.attendanceData[r][c]?.isWithDrawn && !this.attendanceData[r][c]?.isTransfered && !this.attendanceData[r][c]?.isResigned)) {
                    this.activeColumn = c;


                    break;
                  }
                  else if (this.activeColumn > c && this.attendanceData[r][c].id == 0 && !this.attendanceData[r][c].isDefShiftOff && (!this.attendanceData[r][c]?.isOnLeave && !this.attendanceData[r][c]?.isWithDrawn && !this.attendanceData[r][c]?.isTransfered && !this.attendanceData[r][c]?.isResigned)) {
                    this.activeColumn = c;

                    break;
                  }
                }
              }
              let rt: number = -1;
              for (let r = 0; r < this.attendanceData.length; r++) {
                if (this.attendanceData[r][this.activeColumn].id == 0 && !this.attendanceData[r][this.activeColumn].isDefShiftOff && (!this.attendanceData[r][this.activeColumn]?.isOnLeave && !this.attendanceData[r][this.activeColumn]?.isWithDrawn && !this.attendanceData[r][this.activeColumn]?.isTransfered && !this.attendanceData[r][this.activeColumn]?.isResigned)) {
                  rt = r;
                }

              }
              if (rt == -1) {
                this.activeColumn++;
              }
            });
            for (let r = 0; r < this.attendanceData.length; r++) {

              for (let c = 0; c < this.attendanceData[r].length; c++) {

                if (this.activeColumn == -1 && this.attendanceData[r][c].id == 0 && !this.attendanceData[r][c].isDefShiftOff && (!this.attendanceData[r][c]?.isOnLeave && !this.attendanceData[r][c]?.isWithDrawn && !this.attendanceData[r][c]?.isTransfered && !this.attendanceData[r][c]?.isResigned)) {
                  this.activeColumn = c;


                  break;
                }
                else if (this.activeColumn > c && this.attendanceData[r][c].id == 0 && !this.attendanceData[r][c].isDefShiftOff && (!this.attendanceData[r][c]?.isOnLeave && !this.attendanceData[r][c]?.isWithDrawn && !this.attendanceData[r][c]?.isTransfered && !this.attendanceData[r][c]?.isResigned)) {
                  this.activeColumn = c;

                  break;
                }
              }
            }
            let rt: number = -1;
            for (let r = 0; r < this.attendanceData.length; r++) {
              if (this.attendanceData[r][this.activeColumn].id == 0 && !this.attendanceData[r][this.activeColumn].isDefShiftOff && (!this.attendanceData[r][c]?.isOnLeave && !this.attendanceData[r][c]?.isWithDrawn && !this.attendanceData[r][c]?.isTransfered && !this.attendanceData[r][c]?.isResigned)) {
                rt = r;
              }

            }
            if (rt == -1) {
              this.activeColumn++;
            }

          });

        }

        if (outputFromManualAtt?.confirmType == "transResign") {
          this.loadData();
        }

      });
    }


  }
   private openPostAttendanceWindow(attendanceData: Array<any>, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {



    if (!this.isLoading) {
      let dialogRef = this.oprService.openAutoWidthDialog(this.dialog, Component);
      (dialogRef.componentInstance as any).dbops = dbops;
      (dialogRef.componentInstance as any).modalTitle = modalTitle;
      (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
      (dialogRef.componentInstance as any).inputAttendanceData = attendanceData.slice();
      (dialogRef.componentInstance as any).maxPostDate = this.maxPostDate;
      (dialogRef.componentInstance as any).minPostDate = this.minPostDate;
      (dialogRef.componentInstance as any).projectCode = this.project.projectCode;
      (dialogRef.componentInstance as any).siteCode = this.project.siteCode;


      dialogRef.afterClosed().subscribe(outputFromManualAtt => {


        if (outputFromManualAtt.res == true) {
          this.notifyService.showSuccess(this.translate.instant("Success"))
          this.loadData();            //removed Roaster While Cancel attendance
        }

        else {

          this.loadData();
        }
    

        

      });
    }


  }
   private openClearAttendanceWindow(attendanceData: Array<any>, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {



    if (!this.isLoading) {
      let dialogRef = this.oprService.openAutoWidthDialog(this.dialog, Component);
      (dialogRef.componentInstance as any).dbops = dbops;
      (dialogRef.componentInstance as any).modalTitle = modalTitle;
      (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
      (dialogRef.componentInstance as any).inputAttendanceData = attendanceData.slice();
      (dialogRef.componentInstance as any).maxDate = this.maxPostDate;
      (dialogRef.componentInstance as any).minDate = this.minPostDate;
      (dialogRef.componentInstance as any).projectCode = this.project.projectCode;
      (dialogRef.componentInstance as any).siteCode = this.project.siteCode;


      dialogRef.afterClosed().subscribe(outputFromManualAtt => {


        if (outputFromManualAtt.res == true) {
          this.notifyService.showSuccess(this.translate.instant("Success"))
          this.loadData();            //removed Roaster While Cancel attendance
        }

        else {

          this.loadData();
        }
    

        

      });
    }


  }

  getAttendance() {
    this.isLoading = true;
    this.notPostedAtndncExist = true;
    this.attendanceData = [];

    this.employeeConsolidateData = [];
    this.daywiseConsolidateData = [];





    this.updatedAttendance = [];

    for (let r = 0; r < this.listOfRoasters.length; r++) {

      this.attendanceData[r] = [];




      let consData: any = {
        total: 0,
       
        totMins: 0,
        off: 0,
        wd: 0,
        a: 0,
        nwd: 0,
        wh: 0,
        lateDays: 0,
        lateHrs: 0,
        lateMins: 0,
        overTime: 0,
        overTimeMins: 0,
        notAssigned: 0,
        actualShifts: 0,
        contractualShifts:0,
        al_count: 0,
        ul_count: 0,
        el_count: 0,
        sl_count: 0,
        w_count: 0,
        ot_count: 0

      };
      this.employeeConsolidateData.push(consData);
      for (let c = 0; c < this.noOfDays; c++) {
        let daywiseConsData: any = {
          tch: 0,
          tch_min: 0,
          tcs: 0,
          ot_count: 0,


        };
        this.daywiseConsolidateData.push(daywiseConsData);

        let date: string = this.year + "-" + this.month.toString().padStart(2, "0") + "-" + (c + 1).toString().padStart(2, "0");
      

        if (this.listOfRoasters[r]?.shiftCodesForMonth[c] != "")
          this.apiService.getall(`EmployeesAttendance/getSingleEmployeeAttendance/${this.project.projectCode}/${this.siteCode}/${this.listOfRoasters[r]?.employeeNumber}/${date}/${this.listOfRoasters[r]?.shiftCodesForMonth[c]}`).subscribe((res: any) => {
            this.attendanceData[r][c] = res;
            if (!res.isPosted && res.id > 0 && (res.employeeNumber == res.altEmployeeNumber || res.altEmployeeNumber == "" || res.altEmployeeNumber == null) && (res.attendance == "P" || res.attendance == "OT")) {
              this.notPostedAtndncExist = false;
              this.updatedAttendance.push(res);

            }
            else
              if (res.altEmployeeNumber != "" && res.id > 0) {

                this.apiService.get('EmployeesAttendance/getAltAttendanceById', res.id).subscribe(res3 => {
                  if (res3) {

                    if (!res3.isPosted && res3?.id > 0 && (res3?.employeeNumber == res3?.altEmployeeNumber || res3?.altEmployeeNumber == "" || res3?.altEmployeeNumber == null) && (res3?.attendance == "P" || res3.attendance == "OT")) {
                      this.notPostedAtndncExist = false;
                      if (this.updatedAttendance.findIndex(e => e.id == res3.id) < 0)
                        this.updatedAttendance.push(res3);
                    }
                  }
                });
              }

            if (this.attendanceData.length == this.listOfRoasters.length && this.attendanceData.length!=0) {
              if (this.attendanceData[this.listOfRoasters.length - 1].length == this.noOfDays) {
               
                this.getActiveColumn();
                this.isLoading = false;

            
              }
            }



          });
      }

    }


  }



  confirmPostAttendance() {

    if (this.updatedAttendance.length == 0) {
      this.notifyService.showInfo(this.translate.instant("No_Updates"));
    }
    else {
      //this.openConfirmationDialog(DBOperation.create, 'Are_You_Sure?', ConfirmDialogWindowComponent, "postAttendance", "postAttendance", 0);

      this.openPostAttendanceWindow(this.updatedAttendance.slice(), DBOperation.create, 'Post_Attendance', 'Save', PostattendancewithdateComponent);


    }

  }








  postAttendance() {
    

   



    if (!this.isLoading) {


      this.isLoading = true;
      this.apiService.post('PostingMonthlyAttendance/postEmployeeAttendance', this.updatedAttendance)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.isLoading = false;
          this.getAllAttendance();


        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
            this.isLoading = false;
          });
    }
  }

  getCellBackGroud(r: number, c: number): string {

    let shiftIndex = this.listOfShiftCodes2.findIndex(e => e.shiftCode == this.listOfRoasters[r]?.shiftCodesForMonth[c]);
    let isOff = this.listOfShiftCodes2.findIndex(e => e.shiftCode == this.listOfRoasters[r]?.shiftCodesForMonth[c] && e.isOff) >= 0;
    let date: Date = new Date(this.year + "-" + this.month.toString().padStart(2, "0") + "-" + (c + 1).toString().padStart(2, "0"));
    let curDate: Date = new Date();

    if (curDate >= date && this.listOfRoasters[r]?.shiftCodesForMonth[c] != 'x' && this.activeColumn == c) {
      if (this.attendanceData[r][c]!=null && (this.attendanceData[r][c]?.isAbsent || (this.attendanceData[r][c]?.nwTime != null && this.attendanceData[r][c]?.nwTime.hours == 0 && this.attendanceData[r][c]?.nwTime.minutes == 0 && this.attendanceData[r][c].id > 0))) {
        return `bg_red`;
      }
      else {
        if (shiftIndex != -1 && this.attendanceData[r][c].isPosted && (this.attendanceData[r][c].attendance == "P" || this.attendanceData[r][c].attendance == "OT")) {
          return `bg_Posted`;
        }
        else if (this.attendanceData[r][c].id == 0)
          return `bg_Active`;
        else
          return `bg_white`;
      }
    }
    else
      if (curDate >= date && this.listOfRoasters[r]?.shiftCodesForMonth[c] != 'x') {

        if (shiftIndex != -1 && this.attendanceData[r][c]?.isPosted && (this.attendanceData[r][c]?.attendance == "P" || this.attendanceData[r][c]?.attendance=="OT") ) {
          return `bg_Posted`;
        }

        else if (shiftIndex != -1 && !isOff) {
          if (this.attendanceData[r][c]?.isAbsent || (this.attendanceData[r][c]?.nwTime != null && this.attendanceData[r][c]?.nwTime.hours == 0 && this.attendanceData[r][c]?.nwTime.minutes == 0 && this.attendanceData[r][c].id>0)) {
            return `bg_red`;
          }

        
          return `bg_white`;;
        }
        else if (isOff)
          return `bg_offShift`;
        else
          return `bg_white`;

      }
      else if (this.listOfRoasters[r]?.shiftCodesForMonth[c] != 'x')
        return `bg_gray`;

      else return `bg_black`;



  }
  getDefShiftBackGround(r: number): string {
    let c = 0;


  
    while (c < this.noOfDays) {
      let isOff = this.listOfShiftCodes2.findIndex(e => e.shiftCode == this.listOfRoasters[r]?.shiftCodesForMonth[c] && e.isOff) >= 0;
      if (this.listOfRoasters[r]?.shiftCodesForMonth[c] != "x" && this.listOfRoasters[r]?.shiftCodesForMonth[c] != "" && !isOff) {
        break;
      }
      c++;
    }

    if (c < this.noOfDays) {


      let shiftIndex = this.listOfShiftCodes2.findIndex(e => e.shiftCode == this.listOfRoasters[r]?.shiftCodesForMonth[c]);

      if (this.attendanceData[r][c]?.isPrimarySite)
        return this.getShiftColour(shiftIndex);
      else

        return `bg_black`;

    }
    else
      return `bg_black`;
  }

  getDefShiftTypeName(r: number): any {
    let c = 0;

    let code = this.getDefShiftCode(r);
  
    let shiftIndex = this.listOfShiftCodes2.findIndex(e => e.shiftCode == code);

      return this.getShiftTypeNumber(shiftIndex);
  
     
  }

  getDefShiftCode(r: number): string {
    let c = 0;
 

    
    let flag = true;
    while (flag && c < this.noOfDays) {
      let shiftCode = this.listOfRoasters[r]?.shiftCodesForMonth[c];
      let isOff = this.listOfShiftCodes2.findIndex(e => e.shiftCode == shiftCode && e.isOff) >= 0;
      let isCodeExist = this.listOfShiftCodes2.findIndex(e => e.shiftCode == shiftCode) >= 0;
      if (!isOff && isCodeExist) {

        flag = false;
        break;
      }
      c++;
    }


    if (c < this.noOfDays && this.attendanceData[r][c]?.isPrimarySite)
      return this.listOfRoasters[r]?.shiftCodesForMonth[c];
    else
      return "-";
  }







  getShiftColour(i: number) {

    let isOff = this.listOfShiftCodes2.findIndex(e => e.shiftCode == this.listOfShiftCodes2[i].shiftCode && e.isOff) >= 0;
    if (isOff)
      return `bg_offShift`;
    else
      return this.colours[i];
  }



  public manualAttendance(r: number, c: number) {


    let date: Date = new Date(this.year + "-" + this.month.toString().padStart(2, "0") + "-" + (c + 1).toString().padStart(2, "0"));
    let curDate: Date = new Date();
    if (curDate <= date)
      this.notifyService.showError(this.translate.instant("Can_not_Enter_Attendance_For_Future_Date"));
    else if (this.listOfRoasters[r]?.shiftCodesForMonth[c] == "x") {
      if (new Date(this.listOfRoasters[r].monthStartDate) <= date && new Date(this.listOfRoasters[r].monthEndDate) >= date) {
        let input: any = {
          projectCode : this.listOfRoasters[r].projectCode,
           siteCode : this.listOfRoasters[r].siteCode,
          employeeNumber: this.listOfRoasters[r].employeeNumber,
          date: this.year.toString() + "-" + this.month.toString().padStart(2, "0") + "-" + (c + 1).toString().padStart(2, "0")
        };

        this.apiService.post('PvAllRequests/getRecentApprovedPvRequestData',input).subscribe((res:any) => {
         if(res?.reqId==0 || res==null)
                    this.notifyService.showInfo("Employee Not Available");
         else {
           if (res?.requestType == 1) {         //EnumPvRequestType

             this.openDialogManage(res?.reqId, DBOperation.create, 'Request_For_Add_Resource', 'View', PvEmpToResMapComponent /*ViewPvAddResRequestComponent*/);

           }
           else if (res?.requestType == 2) {
             this.openDialogManage(res?.reqId, DBOperation.create, 'Request_For_Remove_Resource', 'View', CreateUpdateReqRemoveResourceComponent);

           }
           else if (res?.requestType == 3) {
             this.openDialogManage(res?.reqId, DBOperation.create, 'Request_For_Transfer_Employee', 'View', CreateUpdateTransferResourceReqComponent);

           }
           else if (res?.requestType == 4) {
             this.openDialogManage(res?.reqId, DBOperation.create, 'Request_For_Transfer_With_Replacement', 'View', CreateUpdateTransferWithReplacementComponent);

           }
           else if (res?.requestType == 5) {
             this.openDialogManage(res?.reqId, DBOperation.create, 'Request_For_Swap_Employees', 'View', SwapEmployeesComponent);

           }
           else if (res?.requestType == 6) {
             this.openDialogManage(res?.reqId, DBOperation.create, 'Request_For_Replace_Resource', 'View', CreateUpdateReqReplaceEmployeeComponent);

           }

          }
        });

     //   this.notifyService.showError(this.translate.instant("Eployee_Not_Available"));

      }
      else {
        this.notifyService.showError(this.translate.instant("Invalid_Date_Selection"));
      }

    }
    else if (this.listOfRoasters[r]?.shiftCodesForMonth[c] == "" || this.attendanceData[r][c].id == -2)
      this.notifyService.showError(this.translate.instant("Shifts_Not_Assigned"));
    else if (this.attendanceData.length != 0) {


      if (this.attendanceData[r][c].id >= 0) {
        let inputCellData: any = {
          r: r,
          c: c,
          totRows: this.listOfRoasters.length,
        };
      


       
        let isOff = this.listOfShiftCodes2.findIndex((e: any) => e.shiftCode == this.listOfRoasters[r]?.shiftCodesForMonth[c] && e.isOff) >= 0;
        let inputData: any = {
          id: this.attendanceData[r][c].id,
          projectCode: this.project.projectCode,
          siteCode: this.siteCode,
          attnDate: this.year + "-" + this.month.toString().padStart(2, "0") + "-" + (c + 1).toString().padStart(2, "0"),
          shiftCode: this.listOfRoasters[r]?.shiftCodesForMonth[c],
          employeeNumber: this.listOfRoasters[r].employeeNumber,
          employeeID: this.listOfRoasters[r].employeeID,
          isDefShiftOff: isOff,
          isActive: true,
          inTime: this.listOfShiftCodes2.find((e: any) => e.shiftCode == this.listOfRoasters[r]?.shiftCodesForMonth[c])?.inTime,
          outTime: this.listOfShiftCodes2.find((e: any) => e.shiftCode == this.listOfRoasters[r]?.shiftCodesForMonth[c])?.outTime,
          isDefaultEmployee: !(this.listOfShiftCodes2.find((e: any) => e.shiftCode == this.listOfRoasters[r]?.shiftCodesForMonth[c])?.isOff),
          isPrimarySite: this.listOfRoasters[r]?.isPrimaryResource,
          isPosted: this.attendanceData[r][c]?.isPosted,
          altEmployeeNumber: this.listOfRoasters[r].employeeNumber,
          altShiftCode: this.listOfRoasters[r]?.shiftCodesForMonth[c],
          attendance: this.attendanceData[r][c - 1]?.id > 0 ? this.attendanceData[r][c - 1].attendance : 'P',
          refIdForAlt: 0,

          leavesData: this.attendanceData[r][c].leavesData,
          transORresignData: this.attendanceData[r][c].transORresignData,
          isOnLeave: this.attendanceData[r][c].isOnLeave,
          isWithDrawn: this.attendanceData[r][c].isWithDrawn,
          isResigned: this.attendanceData[r][c].isResigned,
          isTransfered: this.attendanceData[r][c].isTransfered,

          daywiseConsolidateData: this.daywiseConsolidateData[c],


          roasterData: this.listOfRoasters[r], r: r,c:c, shiftCodesSelectionList: this.listOfShiftCodes2          //for update shift in manual attendance popup
        };

        this.openDialogWindow(inputCellData, inputData, DBOperation.create, 'Manual_Attendance', 'Save', ManualAttendancePopupComponent);

      }
      else if (this.attendanceData[r][c - 1].id == 0) {
        this.notifyService.showError(this.translate.instant("Enter_Previous_Day_Attendance_First"));

      }


    }


    
  }
  private openDialogManage(id: number, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;

    dialogRef.afterClosed().subscribe(res => {
     
    });
  }
  getActiveColumn() {
    for (let r = 0; r < this.attendanceData.length; r++) {

      for (let c = 0; c < this.attendanceData[r].length; c++) {

        if (this.activeColumn == -1 && this.attendanceData[r][c]?.id == 0 && !this.attendanceData[r][c].isDefShiftOff && (!this.attendanceData[r][c]?.isOnLeave && !this.attendanceData[r][c]?.isWithDrawn && !this.attendanceData[r][c]?.isTransfered && !this.attendanceData[r][c]?.isResigned) && this.listOfRoasters[r]?.shiftCodesForMonth[c] != "x") {
          this.activeColumn = c;


          break;
        }
        else if (this.activeColumn > c && this.attendanceData[r][c]?.id == 0 && !this.attendanceData[r][c].isDefShiftOff && (!this.attendanceData[r][c]?.isOnLeave && !this.attendanceData[r][c]?.isWithDrawn && !this.attendanceData[r][c]?.isTransfered && !this.attendanceData[r][c]?.isResigned) &&  this.listOfRoasters[r]?.shiftCodesForMonth[c] != "x") {
          this.activeColumn = c;

          break;
        }
     
      }
    }


  }

  updateManualAttendance(r: number, c: number) {

    if (this.listOfRoasters.length > 0) {
      let date: Date = new Date(this.year + "-" + this.month.toString().padStart(2, "0") + "-" + (c + 1).toString().padStart(2, "0"));
      let curDate: Date = new Date();
      if (curDate <= date) {

        this.notifyService.showError(this.translate.instant("Can_not_Enter_Attendance_For_Future_Date"));
      }
      else if (this.attendanceData.length > 0 && this.attendanceData[r][c]?.id > 0 /*&& this.attendanceData[r][c]?.isPrimarySite*/) {

      //  if (!this.attendanceData[r][c].isPosted) {


          let inputCellData: any = {
            r: r,
            c: c,
            totRows: this.listOfRoasters.length,

          };

          this.attendanceData[r][c].daywiseConsolidateData = this.daywiseConsolidateData[c];
          this.openDialogWindow(inputCellData, this.attendanceData[r][c], DBOperation.create, 'Update_Manual_Attendance', 'Save', UpdateManualAttendancePopupComponent);


      //  }
        //else {

        //  this.notifyService.showError(this.translate.instant("Attendance_Already_Posted"));

        //}

      }


    }

  }











  onRightClick(r: number, c: number) {
    this.updateManualAttendance(r, c);
    return false;

  }


  translateToolTip(msg: string) {
    return `${this.translate.instant(msg)}`;

  }


  
  confirmAutoAttendancePerDay(day: number) {
    this.openConfirmationDialog(DBOperation.create, 'Are_You_Sure?', ConfirmDialogWindowComponent, "general", "AutoAttendancePerDay", day);


  }


  AutoAttendancePerDay(day: number) {
    let autoAtt: Array<any> = [];
    if (this.workingHoursInSite == null || this.workingHoursInSite <=0) {
      this.notifyService.showInfo("No.Of_Working_Hours_For_Site_Not_Defined");
      return;
    }

    
    let whPerDay: number = 0;


    if (this.listOfRoasters.length != 0) {

      let date: Date = new Date(this.year + "-" + this.month.toString().padStart(2, "0") + "-" + (day + 1).toString().padStart(2, "0"));
      let curDate: Date = new Date();


      if (curDate >= date) {
        for (let j = 0; j < this.listOfRoasters.length; j++) {


          if (this.attendanceData[j][day].id == 0 && !this.attendanceData[j][day].isDefShiftOff && (!this.attendanceData[j][day]?.isOnLeave && !this.attendanceData[j][day]?.isWithDrawn && !this.attendanceData[j][day]?.isTransfered && !this.attendanceData[j][day]?.isResigned && this.listOfRoasters[j].isPrimaryResource)) {
            let inputData: any = {
              id: 0,
              projectCode: this.project.projectCode,
              siteCode: this.siteCode,
              attnDate: this.year + "-" + this.month.toString().padStart(2, "0") + "-" + (day + 1).toString().padStart(2, "0"),
              shiftCode: this.listOfRoasters[j]?.shiftCodesForMonth[day],
              employeeNumber: this.listOfRoasters[j].employeeNumber,
            
              isDefShiftOff: false,
              isActive: true,
              inTime: this.listOfShiftCodes2.find((e: any) => e.shiftCode == this.listOfRoasters[j]?.shiftCodesForMonth[day])?.inTime,
              outTime: this.listOfShiftCodes2.find((e: any) => e.shiftCode == this.listOfRoasters[j]?.shiftCodesForMonth[day])?.outTime,
              workingTime: this.listOfShiftCodes2.find((e: any) => e.shiftCode == this.listOfRoasters[j]?.shiftCodesForMonth[day])?.workingTime,
              
              isDefaultEmployee: true,
              isPrimarySite: this.listOfRoasters[j]?.isPrimaryResource,
              isPosted: false,
              altEmployeeNumber: "",
              altShiftCode: "",
              attendance: 'P',
              refIdForAlt: 0,
              shiftNumber: 1,

            };
            let wh: number =  +inputData.workingTime.substring(0,2);
            whPerDay += wh;
            autoAtt.push(inputData);
           
          }

        }
      }
    }
    if (autoAtt.length > 0) {
      let defInTime = this.daywiseConsolidateData[day]?.tch - this.daywiseConsolidateData[day]?.hours;


     // if (defInTime < autoAtt.length * this.workingHoursInSite || (autoAtt.length * this.workingHoursInSite == defInTime && this.daywiseConsolidateData[day].mins > 0)) {
      if (defInTime < whPerDay || (whPerDay == defInTime && this.daywiseConsolidateData[day].mins > 0)) {

        this.notifyService.showError(this.translate.instant("Daily_Shift_Hours_Exceeding"));

      }
      else if (this.daywiseConsolidateData[day]?.varInShift < autoAtt.length) {
        this.notifyService.showError(this.translate.instant("Daily_Contract_Shifts_Exceeding"));
      }

      else {
        this.apiService.post('EmployeesAttendance/enterAutoAttendance', autoAtt)
          .subscribe(res => {

          
            this.getSelectedColumnAttendance(day);
            this.utilService.OkMessage();
            this.activeColumn++;
            
          },
            error => {
              console.error(error);
              this.utilService.ShowApiErrorMessage(error);
            });
      }

    }
    else {



      let rt: number = -1;
      while (rt == -1) {
        for (let r = 0; r < this.attendanceData.length; r++) {

          if (this.attendanceData[r][this.activeColumn]?.id == 0 && !this.attendanceData[r][this.activeColumn]?.isDefShiftOff && (!this.attendanceData[r][this.activeColumn]?.isOnLeave && !this.attendanceData[r][this.activeColumn]?.isWithDrawn && !this.attendanceData[r][this.activeColumn]?.isTransfered && !this.attendanceData[r][this.activeColumn]?.isResigned)) {
            rt = r;
          }

        }
        if (rt == -1) {
          this.activeColumn++;
         

        }
        if (this.listOfDays.length < this.activeColumn) {
          rt = this.activeColumn;
          break;
        }
          

      }
      this.getSelectedColumnAttendance(day);
     
    }




  }



  getSelectedColumnAttendance(day: number) {

    this.isLoading = true;
    this.inputListForDay = [];
    for (let j = 0; j < this.listOfRoasters.length; j++) {

      let date: string = this.year + "-" + this.month.toString().padStart(2, "0") + "-" + (day + 1).toString().padStart(2, "0");
     

      let input: any = {

        projectCode: this.project.projectCode,
        siteCode: this.siteCode,
        employeeNumber: this.listOfRoasters[j]?.employeeNumber,
        attnDate: date,
        shiftCode: this.listOfRoasters[j]?.shiftCodesForMonth[day],

      };
      this.inputListForDay.push(input);
      

    }

    

      let dto: any = { inputList: this.inputListForDay.splice(0) };
      this.apiService.post('EmployeesAttendance/getAllEmployeeAttendance', dto).subscribe(res => {

        let output: Array<any> = res as Array<any>;
        for (let k = 0; k < output.length; k++) {

          this.attendanceData[k][day] = output[k];
        }
        this.isLoading = false;
       
    });

    this.getActiveColumn();
    
   

    



  }


  private openConfirmationDialog(dbops: DBOperation, modalTitle: string, Component: any, confirmType: string,operation:string,day:number) {
    let dialogRef = this.oprService.confirmationDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).confirmType = confirmType;
    (dialogRef.componentInstance as any).noOfUpdates = this.updatedAttendance.length;
    


    dialogRef.afterClosed().subscribe(res => {
      if ((res && res == true) || res.res) {



        if (operation == "AutoAttendanceForMonth")  // for leave and withdraw
        {
          this.AutoAttendanceForMonth();

        }
        else if (operation == "AutoAttendancePerDay")
        {
          this.AutoAttendancePerDay(day);

        }
        else if (operation == "postAttendance")
        {
          this.postAttendance();

        }
       

      }
    });
  }


  confirmAutoAttendanceForMonth() {

    this.openConfirmationDialog(DBOperation.create, 'Are_You_Sure?', ConfirmDialogWindowComponent, "general", "AutoAttendanceForMonth", 0);
  }

  AutoAttendanceForMonth() {
    let autoAtt: Array<any> = [];
    for (let r = 0; r < this.listOfRoasters.length; r++) {

      for (let c = 0; c < this.noOfDays; c++) {
       let dayAttExist = false;
        for (let e = 0; e < this.listOfRoasters.length; e++) {
          if (this.attendanceData[e][c].id > 0) {
            dayAttExist = true;
            break;
          }
        }
        let date: string = this.year + "-" + this.month.toString().padStart(2, "0") + "-" + (c + 1).toString().padStart(2, "0");
        if (this.attendanceData[r][c].id == 0 && !this.attendanceData[r][c].isDefShiftOff && (!this.attendanceData[r][c]?.isOnLeave && !this.attendanceData[r][c]?.isWithDrawn && !this.attendanceData[r][c]?.isTransfered && !this.attendanceData[r][c]?.isResigned) && this.listOfRoasters[r].isPrimaryResource
          && !dayAttExist) {
          let inputData: any = {
            id: 0,
            projectCode: this.project.projectCode,
            siteCode: this.siteCode,
            attnDate: date,
            shiftCode: this.listOfRoasters[r]?.shiftCodesForMonth[c],
            employeeNumber: this.listOfRoasters[r].employeeNumber,
          
            isDefShiftOff: false,
            isActive: true,
            inTime: this.listOfShiftCodes2.find((e: any) => e.shiftCode == this.listOfRoasters[r]?.shiftCodesForMonth[c])?.inTime,
            outTime: this.listOfShiftCodes2.find((e: any) => e.shiftCode == this.listOfRoasters[r]?.shiftCodesForMonth[c])?.outTime,
            isDefaultEmployee: true,
            isPrimarySite: this.listOfRoasters[r] ?.isPrimaryResource,
            isPosted: false,
            altEmployeeNumber: "",
            altShiftCode: "",
            attendance: 'P',
            refIdForAlt: 0,
            shiftNumber: 1,

          };
          autoAtt.push(inputData);
        }
      }
    }
    if (autoAtt.length > 0) {

      let defInTime = this.Totals()?.wh - this.Totals()?.total;

      if (defInTime < autoAtt.length * this.workingHoursInSite || (autoAtt.length * this.workingHoursInSite == defInTime && this.Totals()?.totMins > 0)) {

        this.notifyService.showError(this.translate.instant("Monthly_Shift_Hours_Exceeding"));

      }
      else if (this.Totals()?.varInShift < autoAtt.length) {
        this.notifyService.showError(this.translate.instant("Monthly_Contract_Shifts_Exceeding"));
      }
      else {
        this.isLoading = true;
        this.apiService.post('EmployeesAttendance/enterAutoAttendance', autoAtt)
          .subscribe(res => {


            this.utilService.OkMessage();
            this.loadData();

          },
            error => {
              console.error(error);
              this.utilService.ShowApiErrorMessage(error);
            });

      }







    }
    else {
      let rt: number = -1;
      while (rt != -1) {
        for (let r = 0; r < this.attendanceData.length; r++) {

          if (this.attendanceData[r][this.activeColumn].id == 0) {
            rt = r;
          }

        }
        if (rt == -1) {
          this.activeColumn++;
        }
      }
      this.notifyService.showError(this.translate.instant("Default_Attedance_Already_Marked"));
      this.isLoading = false;
    }
  }

  DayWiseShiftCount(col: number): any {

    let hours = 0;
    let mins = 0;
    let tch = 0;
    let tcs = 0;
    let taws = 0;
    let ot_count = 0;


    let awh = 0;
    for (let i = 0; i < this.attendanceData.length; i++) {

      hours = this.attendanceData[i][col]?.nwTime==null? hours: hours + this.attendanceData[i][col]?.nwTime.hours;
      mins = this.attendanceData[i][col]?.nwTime == null? mins:mins + this.attendanceData[i][col]?.nwTime.minutes;
      tcs = this.attendanceData[i][col]?.isPrimarySite && this.listOfRoasters[i]?.shiftCodesForMonth[col] !="x" ? tcs + 1 : tcs;
      tch = tcs * this.workingHoursInSite;
      taws = this.attendanceData[i][col]?.nwTime == null ?taws: taws + this.attendanceData[i][col]?.shiftsCount;
      ot_count = ot_count + this.attendanceData[i][col]?.otCount;


    }
    hours = hours + ((mins - (mins % 60)) / 60);
    mins = mins % 60;
    this.daywiseConsolidateData[col].tch = tch;

    this.daywiseConsolidateData[col].tcs = tcs;
    this.daywiseConsolidateData[col].taws = taws;
    this.daywiseConsolidateData[col].varInShift = tcs - taws;
    this.daywiseConsolidateData[col].hours = hours;
    this.daywiseConsolidateData[col].mins = mins;
    this.daywiseConsolidateData[col].ot_count = ot_count;



    return this.daywiseConsolidateData[col];
  }
  EmployeeWiseShiftCount(row: number): any {
    let tot = 0;
    let totMins = 0;
    let offs = 0;
    let ab = 0;
    let nwd = 0;
    let late_days = 0;
    let late_hrs = 0;
    let late_mins = 0;
    let over_time = 0;
    let over_timeMins = 0;
    let not_assigned = 0;
    let actualShifts = 0;
    let contractualShifts = 0;
    let al_count = 0;
    let ul_count = 0;
    let el_count = 0;
    let sl_count = 0;
    let stl_count = 0;
    let w_count = 0;
    let ot_count = 0;


    for (let i = 0; i < this.attendanceData[row].length; i++) {

      tot = tot + this.attendanceData[row][i]?.nwTime.hours;
      totMins = totMins + this.attendanceData[row][i]?.nwTime.minutes;
      offs = this.attendanceData[row][i]?.isDefShiftOff ? offs + 1 : offs;
      ab = ((this.attendanceData[row][i]?.attendance == "A" || this.attendanceData[row][i]?.attendance == "S") && !this.attendanceData[row][i]?.isOnLeave) && !(this.attendanceData[row][i]?.isDefShiftOff) ? ab + 1 : ab;
      nwd = this.attendanceData[row][i]?.isWorkedOnday ? nwd + 1 : nwd;
      late_days = late_days + this.attendanceData[row][i]?.lateDays;  /*this.attendanceData[row][i]?.nwTime.hours != 0 && !this.attendanceData[row][i]?.isDefShiftOff && this.attendanceData[row][i]?.nwTime.hours <  this.workingHoursInSite ? late_days + 1 : late_days;*/

      late_hrs = late_hrs + this.attendanceData[row][i]?.lateHrs.hours;          /*this.attendanceData[row][i]?.nwTime.hours != 0 && !this.attendanceData[row][i]?.isDefShiftOff && this.attendanceData[row][i]?.nwTime.hours <  this.workingHoursInSite ? late_hrs + ( this.workingHoursInSite - this.attendanceData[row][i]?.nwTime.hours) : this.employeeConsolidateData[row].late_hrs = late_hrs;*/
      late_mins = late_mins + this.attendanceData[row][i]?.lateHrs.minutes;
      over_time = over_time + this.attendanceData[row][i]?.overTime.hours;
      over_timeMins = over_timeMins + this.attendanceData[row][i]?.overTime.minutes;
      not_assigned = this.attendanceData[row][i]?.isAbsent ? not_assigned + 1 : not_assigned;
      actualShifts = actualShifts + this.attendanceData[row][i]?.shiftsCount;
      contractualShifts = contractualShifts + this.attendanceData[row][i]?.contractualShifts;
      al_count = this.attendanceData[row][i]?.leavesData.al ? al_count + 1 : al_count;
      ul_count = this.attendanceData[row][i]?.leavesData.ul ? ul_count + 1 : ul_count;
      el_count = this.attendanceData[row][i]?.leavesData.el ? el_count + 1 : el_count;
      sl_count = this.attendanceData[row][i]?.leavesData.sl ? sl_count + 1 : sl_count;
      stl_count = this.attendanceData[row][i]?.leavesData.stl ? stl_count + 1 : stl_count;
      w_count = this.attendanceData[row][i]?.leavesData.w ? w_count + 1 : w_count;
      ot_count = ot_count + this.attendanceData[row][i]?.otCount;

    }


    this.employeeConsolidateData[row].total = tot + ((totMins - (totMins % 60)) / 60);
    this.employeeConsolidateData[row].totMins = totMins % 60;
    this.employeeConsolidateData[row].off = offs;
    this.employeeConsolidateData[row].wd = this.noOfDays;
    this.employeeConsolidateData[row].nwd = nwd;
    this.employeeConsolidateData[row].a = ab;
    this.employeeConsolidateData[row].wh = this.noOfDays * this.workingHoursInSite;
    this.employeeConsolidateData[row].lateDays = late_days;
    this.employeeConsolidateData[row].lateHrs = late_hrs + ((late_mins - (late_mins % 60)) / 60);
    this.employeeConsolidateData[row].lateMins = late_mins % 60;
    this.employeeConsolidateData[row].overTime = over_time + ((over_timeMins - (over_timeMins % 60)) / 60);
    this.employeeConsolidateData[row].overTimeMins = over_timeMins % 60;
    this.employeeConsolidateData[row].notAssigned = not_assigned;
    this.employeeConsolidateData[row].actualShifts = actualShifts;
    this.employeeConsolidateData[row].contractualShifts = contractualShifts;
    this.employeeConsolidateData[row].al_count = al_count;
    this.employeeConsolidateData[row].ul_count = ul_count;
    this.employeeConsolidateData[row].el_count = el_count;
    this.employeeConsolidateData[row].sl_count = sl_count;
    this.employeeConsolidateData[row].stl_count = stl_count;
    this.employeeConsolidateData[row].w_count = w_count;
    this.employeeConsolidateData[row].penalty = ab * 2;
    this.employeeConsolidateData[row].ot_count = ot_count;


    return this.employeeConsolidateData[row];
  }


  Totals(): any {



    let total = 0;
    let totMins = 0;
    let offs = 0;
    let nwd = 0;
    let wd = 0;
    let wh = 0;
    let late_hrs = 0;
    let late_mins = 0;
    let overTime = 0;
    let overTimeMins = 0;
    let not_assigned = 0;
    let actualShifts = 0;
    let contractualShifts = 0;
    let al_count = 0;
    let ul_count = 0;
    let el_count = 0;
    let sl_count = 0;
    let stl_count = 0;
    let w_count = 0;
    let penalty = 0;
    let ot_count = 0;
    let a = 0;


    for (let i = 0; i < this.attendanceData.length; i++) {


    
      let index = 0;

      for (let j = 0; this.listOfRoasters[i]?.shiftCodesForMonth.length; j++) {
        if (this.listOfRoasters[i]?.shiftCodesForMonth[j] != "" && this.listOfRoasters[i]?.shiftCodesForMonth[j] != "x") {
          index = j;
          break;
        }
      }

      
     
      wd = this.attendanceData[i][index] == null ? wd : wd + this.employeeConsolidateData[i]?.wd;
      wh = this.attendanceData[i][index] == null ?wh: wh + this.employeeConsolidateData[i]?.wh;

      if (this.employeeConsolidateData[i] == null) {

        total = total;
        offs = offs;
        totMins = totMins;
        nwd = nwd;

        late_hrs = late_hrs;
        late_mins = late_mins;
        overTime = overTime;
        overTimeMins = overTimeMins;
        not_assigned = not_assigned;
        actualShifts = actualShifts;
       
        al_count = al_count;
        ul_count = ul_count;
        el_count = el_count;
        sl_count = sl_count;
        stl_count = stl_count;
        w_count = w_count;
        penalty = penalty;
        a = a;

        ot_count = ot_count;

      }
      else {
        total = total + this.employeeConsolidateData[i]?.total;
        offs = offs + this.employeeConsolidateData[i]?.off;
        totMins = totMins + this.employeeConsolidateData[i]?.totMins;
        nwd = nwd + this.employeeConsolidateData[i]?.nwd;
        late_hrs = late_hrs + this.employeeConsolidateData[i]?.lateHrs;
        late_mins = late_mins + this.employeeConsolidateData[i]?.lateMins;
        overTime = overTime + this.employeeConsolidateData[i]?.overTime;
        overTimeMins = overTimeMins + this.employeeConsolidateData[i]?.overTimeMins;
        not_assigned = not_assigned + this.employeeConsolidateData[i]?.notAssigned;
        actualShifts = actualShifts + this.employeeConsolidateData[i]?.actualShifts;
       
       al_count = al_count + this.employeeConsolidateData[i]?.al_count;
        ul_count = ul_count + this.employeeConsolidateData[i]?.ul_count;
        el_count = el_count + this.employeeConsolidateData[i]?.el_count;
        sl_count = sl_count + this.employeeConsolidateData[i]?.sl_count;
        stl_count = stl_count + this.employeeConsolidateData[i]?.stl_count;
        w_count = w_count + this.employeeConsolidateData[i]?.w_count;
        penalty = penalty + this.employeeConsolidateData[i]?.penalty;
        a = a + this.employeeConsolidateData[i]?.a;

        ot_count = ot_count + this.employeeConsolidateData[i]?.ot_count;
      }




    }


    let tawh = this.diffTime(total, totMins, overTime + ((overTimeMins - (overTimeMins % 60)) / 60), overTimeMins % 60).hrs;
    let tawm = this.diffTime(total, totMins, overTime + ((overTimeMins - (overTimeMins % 60)) / 60), overTimeMins % 60).mins;

    let tawhWL = this.sumTime(tawh, tawm, late_hrs, late_mins).hrs;
    let tawmWL = this.sumTime(tawh, tawm, late_hrs, late_mins).mins;
    let splOt = this.multiplyTime(overTime + ((overTimeMins - (overTimeMins % 60)) / 60), overTimeMins % 60, 1.5);
    let splOtHalf = this.multiplyTime(overTime + ((overTimeMins - (overTimeMins % 60)) / 60), overTimeMins % 60, 0.5);
  
    let tAWOt = this.sumTime(total + ((totMins - (totMins % 60)) / 60), totMins % 60, splOtHalf.hrs, splOtHalf.mins);

    for (let i = 0; i < this.daywiseConsolidateData.length; i++) {

      contractualShifts += this.daywiseConsolidateData[i].tcs;
    }






    let totalData: any = {
      total: total + ((totMins - (totMins % 60)) / 60),
      offs: offs,
      totMins: totMins % 60,
      nwd: nwd,
      wd: wd,
      wh: wh,
      lateHrs: late_hrs + ((late_mins - (late_mins % 60)) / 60),
      lateMins: late_mins % 60,
      overTime: overTime + ((overTimeMins - (overTimeMins % 60)) / 60),
      overTimeMins: overTimeMins % 60,
      notAssigned: not_assigned,
      actualShifts: actualShifts,
      contractualShifts: contractualShifts,
      al_count: al_count,
      ul_count: ul_count,
      el_count: el_count,
      sl_count: sl_count,
      stl_count: stl_count,
      w_count: w_count,
      penalty: penalty,
      a: a,
      varInShifts: contractualShifts - actualShifts,
      ot_count: ot_count,
      tawhWoOt: tawhWL,
      tawmWoOt: tawmWL,
      diffHrs: this.diffTime(wh, 0, total + ((totMins - (totMins % 60)) / 60), totMins % 60).hrs,
      diffMins: this.diffTime(wh, 0, total + ((totMins - (totMins % 60)) / 60), totMins % 60).mins,


      spl_ot: splOt,
      

      tAWOt: tAWOt,
      
    

    };

   
   
    if (this.attendanceData.length == this.listOfRoasters.length && this.listOfRoasters.length != 0) {
      if (this.attendanceData[this.attendanceData.length - 1]?.length >= this.noOfDays - 1)
        this.isDataLoading = false;
    }

    








    return totalData;

  }

  showTime(hrs: any, mns: any): string {
    if (this.showMins) {
      return hrs.toString() + '.' + mns.toString();
    }
    else {
      return hrs.toString();
    }



  }
  diffTime(h1: number, m1: number, h2: number, m2: number): any {
    let resTime: any = {
      hrs: m1 - m2 < 0 ? h1 - h2 - 1 : h1 - h2,

      mins: m1 - m2 < 0 ? m1 - m2 + 60 : m1 - m2
    };

    return resTime;
  }
  sumTime(h1: number, m1: number, h2: number, m2: number): any {
    let hrs = h1 + h2;
    let mins = m1 + m2;
    if (mins >= 60) {
      hrs = hrs + (mins % 60);
      mins = mins % 60;
      

    }

    let resTime: any = {
      hrs: hrs,

      mins: mins
    };

    return resTime;
  }









  getSiteData(siteCode: string): any {

    return this.siteCodeList.find(e => e.siteCode == siteCode);

  }



  getAttendanceCellData(r: number, c: number): string {

    if (this.attendanceData[r][c]?.isOnLeave && !this.attendanceData[r][c]?.isWithDrawn) {

      if (this.attendanceData[r][c].leavesData.al) return "AL";
      else if (this.attendanceData[r][c].leavesData.ul) return "UL";
      else if (this.attendanceData[r][c].leavesData.sl) return "SL";
      else if (this.attendanceData[r][c].leavesData.el) return "EL";
      else if (this.attendanceData[r][c].leavesData.stl) return "STL";

    }
    else if (this.attendanceData[r][c]?.isOnLeave && !this.attendanceData[r][c]?.isWithDrawn) {
      return "W";

    }
    else if (this.attendanceData[r][c]?.isTransfered) {
      return "TR";

    }
    else if (this.attendanceData[r][c]?.isResigned) {
      return "R";

    }


    else if (this.attendanceData[r][c]?.id == -1) {
      return "";
    }
    else if (this.attendanceData[r][c]?.isAbsent) {
      return "A";
    }
    else if (!this.attendanceData[r][c]?.isOnLeave && !this.attendanceData[r][c]?.isWithDrawn) {
      if (this.attendanceData[r][c]?.isWorkedOnday) {
        return this.showTime(this.attendanceData[r][c]?.nwTime.hours, this.attendanceData[r][c]?.nwTime.minutes)
      }

      else if (this.attendanceData[r][c]?.id == 0 && !this.attendanceData[r][c]?.isDefShiftOff) {
        return "-";
      }
      else if (this.attendanceData[r][c]?.id == 0 && this.attendanceData[r][c]?.isDefShiftOff && !this.attendanceData[r][c]?.isWorkedOnday) {
        return "OF";
      }
      else if (this.attendanceData[r][c]?.id > 0 && this.attendanceData[r][c]?.isDefShiftOff && !this.attendanceData[r][c]?.isWorkedOnday) {
        return "O";
      }
      else if (this.attendanceData[r][c]?.id > 0 && this.attendanceData[r][c]?.isDefShiftOff && this.attendanceData[r][c]?.isWorkedOnday) {
        return "OT";
      }
      else if (this.attendanceData[r][c]?.id > 0 && (this.attendanceData[r][c]?.isDefShiftOff && (this.attendanceData[r][c]?.nwTime.hours != 0||this.attendanceData[r][c]?.nwTime.minutes != 0))) {
        return this.attendanceData[r][c]?.nwTime.hours.toSting() + "." + this.attendanceData[r][c]?.nwTime.minutes.toString();

      }
      else if (this.attendanceData[r][c]?.id > 0 && !this.attendanceData[r][c]?.isDefShiftOff) {

        if (this.attendanceData[r][c]?.nwTime.hours != 0 || this.attendanceData[r][c]?.nwTime.minutes != 0)

          return this.attendanceData[r][c]?.nwTime.hours.toSting() + "." + this.attendanceData[r][c]?.nwTime.minutes.toString();

        else
          return "A";
      }
    }
    else if (this.attendanceData[r][c]?.isWithDrawn) {
      return "W";

    }


    return "";
  }



  





  


  









  calculateStyles(r: number, c: number): string {
    if (this.attendanceData[r][c].isDefShiftOff) {
      return "background-color: red";
    } else {
      return "background-color: blue";
    }

  }

  multiplyTime(h: number, m: number, factor: number): any {
    return {
      hrs: m * factor > 60 ? h * 1.5 + (m * factor / 60) : h * factor,
      mins: m * factor % 60
    };


  }
  devideTime(h: number, m: number, factor: number): any {

    return {
      hrs: (h - (h % factor)) / factor,
      mins: (((h % factor) + m) - ((h % factor) + m) % factor) / factor
    };


  }
  convertTimeToShifts(h: number, m: number, factor: number): number {
    let res = (((h * 60 * 60) + m * 60)) / (factor * 60 * 60);

    return parseFloat(Number(res.toString()).toFixed(1));
  }


  





 getAllAttendance() {

    
    this.isLoading = true;
    this.notPostedAtndncExist = true;
    this.attendanceData = [];

    this.employeeConsolidateData = [];
    this.daywiseConsolidateData = [];





    this.updatedAttendance = [];

    for (let r = 0; r < this.listOfRoasters.length; r++) {

      this.attendanceData[r] = [];




      let consData: any = {
        total: 0,
        totMins: 0,
        off: 0,
        wd: 0,
        a: 0,
        nwd: 0,
        wh: 0,
        lateDays: 0,
        lateHrs: 0,
        lateMins: 0,
        overTime: 0,
        overTimeMins: 0,
        notAssigned: 0,
        actualShifts: 0,
        al_count: 0,
        ul_count: 0,
        el_count: 0,
        sl_count: 0,
        w_count: 0,
        ot_count: 0

      };
      this.employeeConsolidateData.push(consData);
      for (let c = 0; c < this.noOfDays; c++) {

        let daywiseConsData: any = {
          tch: 0,
          tch_min: 0,
          tcs: 0,
          ot_count: 0,


        };
        this.daywiseConsolidateData.push(daywiseConsData);

        let date: string = this.year + "-" + this.month.toString().padStart(2, "0") + "-" + (c + 1).toString().padStart(2, "0");
      



        let input: any = {

          projectCode: this.project.projectCode,
          siteCode: this.siteCode,
          employeeNumber: this.listOfRoasters[r]?.employeeNumber,
          attnDate: date,
          shiftCode: this.listOfRoasters[r]?.shiftCodesForMonth[c],

        };
   

        this.inputArray.push(input);






      }

      let dto: any = { inputList: this.inputArray.splice(0) };
    
      this.apiService.post('EmployeesAttendance/getAllEmployeeAttendance', dto).subscribe(res1 => {
     

       
        let res = res1 as Array<any>;



        let k = 0;


        for (let c = 0; c < this.noOfDays; c++) {


          this.attendanceData[r][c] = res[k];
        
          if (!res[k]?.isPosted && res[k]?.id > 0 && (res[k]?.employeeNumber == res[k]?.altEmployeeNumber || res[k]?.altEmployeeNumber == "" || res[k]?.altEmployeeNumber == null) && (res[k]?.attendance == "P" || res[k]?.attendance == "OT"))
          {
            this.notPostedAtndncExist = false;
            this.updatedAttendance.push(res[k]);

          }
          else
            if (res[k]?.altEmployeeNumber != "" && res[k]?.id > 0)
            {

              this.apiService.get('EmployeesAttendance/getAltAttendanceById', res[k].id).subscribe(res3 => {
                if (res3) {

                  if (!res3.isPosted && res3?.id > 0 && (res3?.employeeNumber == res3?.altEmployeeNumber || res3?.altEmployeeNumber == "" || res3?.altEmployeeNumber == null) && (res3?.attendance == "P" || res3.attendance == "OT")) {
                    this.notPostedAtndncExist = false;
                    if (this.updatedAttendance.findIndex(e => e.id == res3.id) < 0)
                      this.updatedAttendance.push(res3);
                  }
                }
              });

            }
          if (r == this.listOfRoasters.length - 1 || this.listOfRoasters.length==0) {
            this.getActiveColumn();
            this.isLoading = false;
          }
          k++;
        }






      });

    



    }

   


 
  }


  getShiftTypeNumber(i: number): any {

    if (i == -1) {

     return { number: -1, nameEn: "-", nameAr: "-" };
    }
   
    let number = 0;
 
    let isOffExist = this.listOfShiftCodes2.findIndex(e => e.shiftCode =="O")>=0;
    let timeHrs = Number(this.listOfShiftCodes2[i]?.inTime.substring(0, 2));
    let timeMins = Number(this.listOfShiftCodes2[i]?.inTime.substring(3, 2));
   
      for (let t = 0; t < this.listOfShiftCodes2.length; t++) {
        let timeHr = Number(this.listOfShiftCodes2[t]?.inTime.substring(0, 2));
        let timeMin = Number(this.listOfShiftCodes2[t]?.inTime.substring(0, 2));
        if (timeHrs > timeHr || (timeHrs == timeHr && timeMins > timeMin))
          number++;

       


      
    }
    if (isOffExist) {
      return this.shiftTypeNames[number];
    }
    else {
   
        return this.shiftTypeNames[number+1];
    }
   
    
  }

 
  


  
  exportexcel(): void {
    let element = document.getElementById("excel_table");
    let element2 = document.getElementById("excel_table2");
    let element3 = document.getElementById("excel_table3");
    let element4 = document.getElementById("excel_table4");
    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(element);
    const ws2: XLSX.WorkSheet = XLSX.utils.table_to_sheet(element2);
    const ws3: XLSX.WorkSheet = XLSX.utils.table_to_sheet(element3);
    const ws4: XLSX.WorkSheet = XLSX.utils.table_to_sheet(element4);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Sheet1");
    XLSX.utils.book_append_sheet(wb, ws2, "Sheet2");
    XLSX.utils.book_append_sheet(wb, ws3, "Sheet3");
    XLSX.utils.book_append_sheet(wb, ws4, "Sheet4");
    XLSX.writeFile(wb, "Attendance_" + this.project.projectCode + "_" + this.siteCode + "_" + this.month + "_" + this.year + ".xlsx");



  }





   getRoasterReport() :boolean{
     this.roasterReport = [];
     this.totRoastersReport = {contractualShifts:0,offs:0 ,qty:0};



    if (this.listOfRoasters.length != 0) {
      let primaryRoasters: Array<any> = this.listOfRoasters.filter(e => e.isPrimaryResource);
      if (this.attendanceData.length >= primaryRoasters.length)
        if (this.attendanceData[this.attendanceData.length - 1].length == this.noOfDays) {

          primaryRoasters.forEach(pr => {
            let index = this.roasterReport.findIndex(rp => rp.skillsetCode == pr.skillsetCode);

            let contractualShifts = 0;
            let offs = 0;
            pr.shiftCodesForMonth.forEach((shiftCode: any) => {
              let isOff: boolean = this.listOfShiftCodes2.findIndex(e => e.shiftCode == shiftCode && e.isOff) >= 0;
              if (this.listOfShiftCodes2.findIndex((e: any) => shiftCode != 'x' && e.shiftCode == shiftCode) >= 0 && isOff) {

                offs++;

              }
              else if (this.listOfShiftCodes2.findIndex((e: any) => e.shiftCode == shiftCode) >= 0 && !isOff && shiftCode != 'x' && shiftCode != '') {

                contractualShifts++;
              }


            });


            let row: any = {
              skillsetCode: pr.skillsetCode,
              contractualShifts: index >= 0 ? this.roasterReport[index].contractualShifts + contractualShifts : contractualShifts,
              offs: index >= 0 ? this.roasterReport[index].offs + offs : offs,
              qty: index >= 0 ? this.roasterReport[index].qty + 1 : 1
            };

            if (index >= 0) {
              this.roasterReport[index] = row;
            }
            else {
              this.roasterReport.push(row);
            }


            
            this.totRoastersReport.contractualShifts += contractualShifts;
            this.totRoastersReport.offs +=offs;
            this.totRoastersReport.qty +=1;
          });
          
        }
      return true;
    }
     return false;

  }


  confirmClearAttendance() {
    if (this.updatedAttendance.length == 0) {
      this.notifyService.showInfo(this.translate.instant("No_Updates"));
    }
    else {
      this.openClearAttendanceWindow(this.updatedAttendance.slice(), DBOperation.create, 'Clear_Attendance', 'Save', ClearattendancewithdateComponent);


    }

  }


}
