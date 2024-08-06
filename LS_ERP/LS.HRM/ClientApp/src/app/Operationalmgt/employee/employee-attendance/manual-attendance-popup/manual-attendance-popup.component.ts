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
import { ParentOptMgtComponent } from '../../../../sharedcomponent/parentoptmgt.component';
import { ConfirmDialogWindowComponent } from '../../../confirm-dialog-window/confirm-dialog-window.component';
import { OprServicesService } from '../../../opr-services.service';
import { UpdateEmployeeShiftComponent } from './update-employee-shift/update-employee-shift.component';


@Component({
  selector: 'app-manual-attendance-popup',
  templateUrl: './manual-attendance-popup.component.html'
})
export class ManualAttendancePopupComponent extends ParentOptMgtComponent implements OnInit {
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
  inTime: string = "11:00";

  canShowAllEmployees: boolean = false;
  constructor(private translate: TranslateService,
    private notifyService: NotificationService, private utilService: UtilityService, public dialog: MatDialog, private apiService: ApiService, public dialogRef: MatDialogRef<ManualAttendancePopupComponent>, private authService: AuthorizeService, private fb: FormBuilder, private oprService: OprServicesService) {
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
    if (!this.inputData.isPrimarySite) {
      this.inputData.isDefaultEmployee = true;
    }


    this.loadShiftCodes();
    this.getEmployee(this.inputData.employeeNumber);
    if (this.inputData.altEmployeeNumber != "")
      this.getAltEmployee(this.inputData.altEmployeeNumber);
    this.getShift(this.inputData.shiftCode);
    if (this.inputData.altShiftCode != "")
      this.getAltShift(this.inputData.altShiftCode);

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
   // this.apiService.getall(`EmployeesToProjectSite/getEmployeeOfProjectSiteByEmpNumber/${this.inputData.projectCode}/${this.inputData.siteCode}/${employeeNumber}`).subscribe(res => {
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
            this.shift.inTime = res.inTime.toString().substring(0, 5);
            this.shift.outTime = res.outTime.toString().substring(0, 5);

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
      this.inputData.attendance = 'S';
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
      
      let diff = this.inputData.daywiseConsolidateData.tch * 60 + this.inputData.daywiseConsolidateData.tch_min- this.inputData.daywiseConsolidateData.mins - this.inputData.daywiseConsolidateData.hours * 60;
      let nw = outTimeHrs * 60 + outTimeMins - inTimeHrs * 60 - inTimeMins;
      if (nw < 0) nw = nw + (24 * 60);
      if (diff<nw ) {
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
        //return this.apiService.getall(`Employee/getAutoSelectEmployeeList/?search=${val}`)
        .pipe(
          map((response: Array<any>) => {
            const res = response as Array<any>;
            this.isDataLoading = false;
            return res;
          })
        )
    } else {
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

    if (this.inputData.employeeNumber != event.option.value.text || this.inputData.isDefShiftOff) {
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


  enterAbsent() {
    if (!this.inputData.isDefShiftOff && this.inputData.isDefaultEmployee) {

      this.inputData.attendance = "A";
      this.inputData.isDefaultEmployee = true;
      this.inputData.inTime = this.shift.inTime;
      this.inputData.outTime = this.shift.outTime;





      this.apiService.post('EmployeesAttendance/enterEmployeeAbsent', this.inputData)
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
  enterEmployeeLeave(leaveType: string) {

    let leaveDto: any = {
      employeeNumber: this.inputData.employeeNumber,
      attnDate: this.inputData.attnDate,
      al: false,
      el: false,
      ul: false,
      sl: false,
      stl: false,
      w: false,

      projectCode: this.inputData.projectCode,
      siteCode: this.inputData.siteCode,
      shiftCode: this.inputData.shiftCode
    };

    switch (leaveType) {
      case "AL": leaveDto.al = true; break;
      case "EL": leaveDto.el = true; break;
      case "UL": leaveDto.ul = true; break;
      case "SL": leaveDto.sl = true; break;
      case "STL": leaveDto.stl = true; break;
      case "W": leaveDto.w = true; break;
      default: console.log("Default");

    }
    this.openConfirmationDialog(DBOperation.create, 'Are_You_Sure?', leaveType, ConfirmDialogWindowComponent, this.inputCellData, this.inputData, leaveDto, "leave");
  }
  private openConfirmationDialog(dbops: DBOperation, modalTitle: string, leaveType: string, Component: any, cellData: any, outputData: any, leaveData: any, confirmType: string) {
    let dialogRef = this.oprService.confirmationDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).leaveType = leaveType;
    (dialogRef.componentInstance as any).cellData = cellData;
    (dialogRef.componentInstance as any).outputData = outputData;
    (dialogRef.componentInstance as any).leaveData = leaveData;
    (dialogRef.componentInstance as any).confirmType = confirmType;


    dialogRef.afterClosed().subscribe(res => {
      if ((res && res == true) || res.res) {



        if (confirmType == "leave")  // for leave and withdraw
        {
          this.apiService.post('EmployeeLeaves/enterEmployeeLeave', leaveData)
            .subscribe(res => {
              console.log(res);

              this.utilService.OkMessage();
              this.dialogRef.close({ res: true, cellData: this.inputCellData, outputData: this.inputData, leaveData: leaveData ,isConfirmedLeave:true});
            },
              error => {
                console.error(error);
                this.utilService.ShowApiErrorMessage(error);
              });

        }
        else if (confirmType == "transResign")  // for transfer or resign
        {
          leaveData.remarks = res.remarks;
          this.apiService.post('EmployeeTransResign/enterEmployeeTransResign', leaveData)
            .subscribe((res: any) => {


              this.utilService.OkMessage();

              this.dialogRef.close({ res: true, cellData: this.inputCellData, outputData: this.inputData, leaveData: leaveData, confirmType });
            },
              error => {
                console.error(error);
                this.utilService.ShowApiErrorMessage(error);
              });

        }
        else if (confirmType == "cancelLeave")  // for transfer or resign
        {
          console.log(leaveData);
          this.apiService.get('EmployeeLeaves/cancelLeave', leaveData.id)
            // this.apiService.delete('EmployeeLeaves',leaveData.id)
            .subscribe((res: any) => {


              this.utilService.OkMessage();

              this.dialogRef.close({ res: true, cellData: this.inputCellData, outputData: this.inputData, leaveData: leaveData, confirmType });
            },
              error => {
                console.error(error);
                this.utilService.ShowApiErrorMessage(error);
              });

        }
         else if (confirmType == "cancelEmployeeTRX")  // for cancel transfer or resign or Termination
        {
          this.apiService.get('EmployeeTransResign/cancelEmployeeTRX', leaveData.id)
            // this.apiService.delete('EmployeeLeaves',leaveData.id)
            .subscribe((res: any) => {


              this.utilService.OkMessage();

              this.dialogRef.close({ res: true, cellData: this.inputCellData, outputData: this.inputData, leaveData: leaveData, confirmType });
            },
              error => {
                console.error(error);
                this.utilService.ShowApiErrorMessage(error);
              });

        }

      }
    });
  }



  private openUpdateShiftWindow(inputData: any, Component: any) {

    let dialogRef = this.oprService.openAutoWidthDialog(this.dialog, Component);

    (dialogRef.componentInstance as any).inputData = inputData;


    dialogRef.afterClosed().subscribe(res => {
      //if (res.res == true && res.isShiftUpdated == true)
      //{
      //  this.dialogRef.close({ res: true, isUpdatedShift: true, r: this.inputData.r, c: this.inputData.c, updatedShift: res.updatedShift });
      //}
      //else
      //{
      //  this.dialogRef.close(false);
      //}

      if (res?.result > 0) {
        this.dialogRef.close({ res: true, isUpdatedShift: true, r: this.inputData.r, c: this.inputData.c, updatedShift: res.updatedShift });
        this.notifyService.showSuccess("Success");
      }
      else if (res == false) {
      }
      else {
        this.notifyService.showError("Failed");
        this.dialogRef.close(false);
      }
    });

  }










  enterEmployeeTransResign(input: string) {
    let transResignDto: any = {
      employeeNumber: this.inputData.employeeNumber,
      attnDate: this.inputData.attnDate,
      projectCode: this.inputData.projectCode,
      siteCode: this.inputData.siteCode,
      r: false,
      tr: false,
    };

    switch (input) {
      case "R": transResignDto.r = true; break;
      case "TR": transResignDto.tr = true; break;
      case "X": transResignDto.r = false; transResignDto.tr = false; break;
      default: console.log("Default");

    }
    this.openConfirmationDialog(DBOperation.create, 'Are_You_Sure?', input, ConfirmDialogWindowComponent, this.inputCellData, this.inputData, transResignDto, "transResign");

  }

  cancelEmployeeLeave(id: number) {
    let leaveDto: any = { id: id };
    this.openConfirmationDialog(DBOperation.create, 'Are_You_Sure?', "", ConfirmDialogWindowComponent, this.inputCellData, this.inputData, leaveDto, "cancelLeave");


  }
  cancelEmployeeTRX(id: number) {
    let leaveDto: any = { id: id };
    this.openConfirmationDialog(DBOperation.create, 'Are_You_Sure?', "", ConfirmDialogWindowComponent, this.inputCellData, this.inputData, leaveDto, "cancelEmployeeTRX");


  }


  updateShift() {
    this.inputData.c = this.inputData.attnDate.toString().split("-")[2] as number-1;
    this.openUpdateShiftWindow(this.inputData, UpdateEmployeeShiftComponent);

  }

  translateToolTip(msg: string) {

    return `${this.translate.instant(msg)}`;

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
  
