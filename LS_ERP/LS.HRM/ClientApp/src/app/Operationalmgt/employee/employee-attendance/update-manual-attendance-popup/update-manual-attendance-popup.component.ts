import { Time } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { Timestamp } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';

import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../../models/MenuItemListDto';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { DBOperation } from '../../../../services/utility.constants';
import { UtilityService } from '../../../../services/utility.service';
import { DeleteConfirmDialogComponent } from '../../../../sharedcomponent/delete-confirm-dialog';
import { ParentOptMgtComponent } from '../../../../sharedcomponent/parentoptmgt.component';

@Component({
  selector: 'app-update-manual-attendance-popup',
  templateUrl: './update-manual-attendance-popup.component.html'
})
export class UpdateManualAttendancePopupComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  inputData: any;
  inputCellData: any;
  editEmp: boolean = true;
  employee: any;
  altEmployee: any;
  shift: any = { shiftCode: '' };
  altShift: any = { shiftCode: '' };
  listOfShiftCodes: Array<any> = [];


  editInTime: number = 0;
  editOutTime: number = 0;

  empCodeControl = new FormControl('');
  filteredEmployeeNumbers: Observable<Array<CustomSelectListItem>>;
  employeeList: Array<any> = [];
  employeesForProjectSite: Array<any> = [];
  employeeNumber: string = '';
  isDataLoading: boolean = false;

  canShowAllEmployees: boolean = false;

  constructor(private translate: TranslateService,
    private notifyService: NotificationService, private utilService: UtilityService, private apiService: ApiService, public dialogRef: MatDialogRef<UpdateManualAttendancePopupComponent>, private authService: AuthorizeService, private fb: FormBuilder, public dialog: MatDialog) {
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

  ngOnInit(): void {
    console.log(this.inputData);

    if (this.inputData.altEmployeeNumber == "") {
      this.inputData.altEmployeeNumber = this.inputData.employeeNumber;
      this.inputData.isDefaultEmployee = true;
    }
    else {
      this.inputData.isDefaultEmployee = false;
    }

    this.loadShiftCodes();
    this.getEmployee(this.inputData.employeeNumber);
    if (this.inputData.altEmployeeNumber != "") {
      this.getAltEmployee(this.inputData.altEmployeeNumber);
    }
    this.getShift(this.inputData.shiftCode);
    if (this.inputData.altShiftCode != "") {
      this.getAltShift(this.inputData.altShiftCode);
    }

    this.setForm();
  }

  setForm() {

    this.form = this.fb.group({

    });
  }





  closeModel() {
    this.dialogRef.close();
  }
  submit() { }

  editEmployee() {
    this.editEmp = !this.editEmp;

  }

  getEmployee(employeeNumber: number) {

    this.apiService.get('Employee/getEmployeeByEmployeeNumber/', employeeNumber).subscribe(res => {
    //this.apiService.getall(`EmployeesToProjectSite/getEmployeeOfProjectSiteByEmpNumber/${this.inputData.projectCode}/${this.inputData.siteCode}/${employeeNumber}`).subscribe(res => {
      if (res) {
        this.employee = res;

      }
    });

  }
  getAltEmployee(employeeNumber: number) {

     this.apiService.get('Employee/getEmployeeByEmployeeNumber/', employeeNumber).subscribe(res => {
   // this.apiService.getall(`EmployeesToProjectSite/getEmployeeOfProjectSiteByEmpNumber/${this.inputData.projectCode}/${this.inputData.siteCode}/${employeeNumber}`).subscribe(res => {

      if (res) {
        this.altEmployee = res;

      }
    });

  }
  //getEmployee(employeeID: number){

  //  this.apiService.get('Employee/getEmployeeById/', employeeID).subscribe(res => {

  //    if (res) {
  //      this.employee = res;

  //    }
  //  });

  //}
  getShift(shiftCode: string) {

    if (shiftCode != '') {
      this.apiService.getall('ShiftMaster/getShiftMasterByShiftMasterCode/' + shiftCode).subscribe((res: any) => {

        if (res) {

          this.shift = res as any;

          if (this.inputData.id > 0) {
            this.shift.inTime = this.inputData.inTime.toString().substring(0, 5);
            this.shift.outTime = this.inputData.outTime.toString().substring(0, 5);

          }
        }
      });

    }

  }
  getAltShift(shiftCode: string) {
    if (shiftCode != '') {
      this.apiService.getall('ShiftMaster/getShiftMasterByShiftMasterCode/' + shiftCode).subscribe((res: any) => {

        if (res) {
          this.altShift = res as any;
          if (this.altShift.isOff) {

            this.altShift.shiftCode = '';
          }

        }
      });

    }
  }

  changeTime(element: any, event: any) {
    let in_HH = Number(this.shift.inTime.split(":")[0]);
    let out_HH = Number(this.shift.outTime.split(":")[0]);
    let in_MM = Number(this.shift.inTime.split(":")[1]);
    let out_MM = Number(this.shift.outTime.split(":")[1]);
    if (element == 'intime') {
      let totIn = in_HH * 60 + this.editInTime + in_MM;
      this.shift.inTime = this.minsToTimeString(totIn);
    }

    else {
      let totOut = out_HH * 60 + out_MM + this.editOutTime;

      this.shift.outTime = this.minsToTimeString(totOut);

    }
    this.editOutTime = 0;
    this.editInTime = 0;








  }
  minsToTimeString(value: number): string {
    let hours = value > 0 ? Math.floor(value / 60) % 24 : Math.floor((value + 1440) / 60) % 24;
    let minutes = value > 0 ? Math.floor(value % 60) : value + 60;
    if (minutes >= 60) {
      hours++;
      minutes = minutes - 60;


    }

    return this.padLeft(hours.toString(), "0", 2) + ":" + this.padLeft(minutes.toString(), "0", 2);
  }
  padLeft(text: string, padChar: string, size: number): string {
    return (String(padChar).repeat(size) + text).substr((size * -1), size);
  }

  onSelectShift(event: any) {

    this.getAltShift(event.target.value as string);
    this.getShift(event.target.value as string)


  }


  loadShiftCodes() {

    this.apiService.getall(`ShiftMaster/getShiftsByProjectAndSiteCode/${this.inputData.projectCode}/${this.inputData.siteCode}`).subscribe(res => {
      this.listOfShiftCodes = res;


    });
  }

  enterAttendance() {





    if (this.inputData.isDefShiftOff) {

      this.inputData.altShiftCode = this.altShift.shiftCode;



    }
    else {
      this.inputData.altShiftCode = "";

    }
    if (!this.inputData.isDefShiftOff && !this.inputData.isDefaultEmployee) {
      this.inputData.attendance = 'A';
      this.inputData.altEmployeeNumber = this.altEmployee.employeeNumber;

    }
    else if (this.inputData.isDefShiftOff) {
      this.inputData.attendance = 'P';
      this.inputData.altEmployeeNumber = this.altEmployee.employeeNumber;
    }
    else {
      this.inputData.altEmployeeNumber = "";
    }


    if (this.inputData.altEmployeeNumber == this.inputData.employeeNumber || this.inputData.altEmployeeNumber == "") {
      this.inputData.isDefaultEmployee = true;
    }

    this.inputData.inTime = this.shift.inTime;
    this.inputData.outTime = this.shift.outTime;



    if ((this.inputData.isDefShiftOff && this.inputData.altShiftCode != "") || (!this.inputData.isDefShiftOff && this.inputData.altShiftCode != this.inputData.shiftCode)) {

      let inTimeHrs = Number(this.inputData.inTime.substr(0, 2));
      let inTimeMins = Number(this.inputData.inTime.substr(3, 2));
      let outTimeHrs = Number(this.inputData.outTime.substr(0, 2));
      let outTimeMins = Number(this.inputData.outTime.substr(3, 2));

      let diff = this.inputData.nwTime.totalMinutes + this.inputData.daywiseConsolidateData.tch * 60 + this.inputData.daywiseConsolidateData.tch_min - this.inputData.daywiseConsolidateData.mins - this.inputData.daywiseConsolidateData.hours * 60;
      let nw = outTimeHrs * 60 + outTimeMins - inTimeHrs * 60 - inTimeMins;
      if (nw < 0) nw = nw + (24 * 60);
      if (diff < nw) {
        this.notifyService.showError(this.translate.instant("Daily_Shift_Hours_Exceeding"));
      }
      else {

        this.apiService.post('EmployeesAttendance/enterEmployeeAttendance', this.inputData)
          .subscribe(res => {

            this.utilService.OkMessage();
            this.dialogRef.close({ res: true, cellData: this.inputCellData, outputData: this.inputData });
          },
            error => {
              console.error(error);
              this.utilService.ShowApiErrorMessage(error);
            });

      }




    }
    else {
      this.notifyService.showError(this.translate.instant("Please") + " " + this.translate.instant("Select") + " " + this.translate.instant("Shift_Code"));

    }


  }


  filterEmployeeNumbers(val: string): Observable<Array<CustomSelectListItem>> {
    if (!this.canShowAllEmployees) {
      return this.apiService.getall(`EmployeesToProjectSite/getAutoFillEmployeeListForProjectSite/${this.inputData.projectCode}/${this.inputData.siteCode}/?search=${val}`)
        .pipe(
          map((response: Array<any>) => {
            const res = response as Array<any>;
            this.isDataLoading = false;
            return res;
          })
        )
    }
    else {
          return this.apiService.getall(`Employee/getAutoSelectEmployeeList/?search=${val}`)
        .pipe(
          map((response: Array<any>) => {
            const res = response as Array<any>;
            this.isDataLoading = false;
            return res;
          })
        )
    }
  
  }

  autoSelectionEmployeeNumber(event: any) {

    if (this.inputData.employeeNumber != event.option.value.text) {
      this.altEmployee.employeeNumber = event.option.value.text;
      this.altEmployee.employeeID = event.option.value.value;
      this.altEmployee.employeeName = event.option.value.textTwo;
      this.inputData.isDefaultEmployee = false;

    }
    else {
      this.altEmployee.employeeNumber = "";
      this.altEmployee.employeeName = "";
      this.inputData.isDefaultEmployee = true;

    }
    this.empCodeControl.setValue('');
    this.editEmp = true;


  }

  cancelAttendance() {
    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete && this.inputData.id > 0) {

        this.apiService.get('EmployeesAttendance/CancelAttendance', this.inputData.id).subscribe(res => {

          if (res.id == 0) {
            this.notifyService.showError(this.translate.instant("something went wrong"));
          }
         else if (res.id ==-1) {
            this.notifyService.showError(this.translate.instant("Attendance_Already_Posted"));
          }
          //this.apiService.delete('EmployeesAttendance/', this.inputData.id).subscribe(res => {
          else if(res.id==-100){            //removed roaster
            this.inputData.primaryEmployee = res;

            this.dialogRef.close({ res: true, cellData: this.inputCellData, outputData: this.inputData,isRoasterRemoved:true });
            this.utilService.OkMessage();
          }
          else if (res.id == -101) {            //removed roaster
            this.inputData.primaryEmployee = res;

            this.dialogRef.close({ res: true, cellData: this.inputCellData, outputData: this.inputData, r: this.inputCellData.r, c: this.inputCellData.c,isNotPrimaryAtt: true});
            this.utilService.OkMessage();
          }
          else {
            this.inputData.primaryEmployee = res;

            this.dialogRef.close({ res: true, cellData: this.inputCellData, outputData: this.inputData });
            this.utilService.OkMessage();
          }
        });
      }
    });
  }

  changeEmployeeFilter() {

    this.filteredEmployeeNumbers = this.empCodeControl.valueChanges.pipe(
      startWith(this.employeeNumber),
      debounceTime(this.utilService.autoDelay()),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isDataLoading = true;
        return this.filterEmployeeNumbers(val || '')
      })
    );
  }
}
