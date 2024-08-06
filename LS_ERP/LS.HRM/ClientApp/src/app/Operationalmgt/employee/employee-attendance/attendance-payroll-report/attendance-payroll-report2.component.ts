import { HttpClient } from '@angular/common/http';
import { Component, OnChanges, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ValidationService } from '../../../../sharedcomponent/ValidationService';
import { NotificationService } from '../../../../services/notification.service';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { UtilityService } from '../../../../services/utility.service';
import { ApiService } from '../../../../services/api.service';
import { ParentOptMgtComponent } from '../../../../sharedcomponent/parentoptmgt.component';
import * as XLSX from "xlsx";
import { DatePipe } from '@angular/common';
import { OprServicesService } from '../../../opr-services.service';
import { DBOperation } from '../../../../services/utility.constants';
import { ManualAttendancePopupComponent } from '../manual-attendance-popup/manual-attendance-popup.component';
import { UpdateManualAttendancePopupComponent } from '../update-manual-attendance-popup/update-manual-attendance-popup.component';
import { ClearattendancewithdateComponent } from '../clearattendancewithdate/clearattendancewithdate.component';
import { PostattendancewithdateComponent } from '../postattendancewithdate/postattendancewithdate.component';
import { DeleteConfirmDialogComponent } from '../../../../sharedcomponent/delete-confirm-dialog';
import { ConfirmDialogWindowComponent } from '../../../confirm-dialog-window/confirm-dialog-window.component';
import { ConfirmDialogWindowGeneralComponent } from '../../../confirm-dialog-window/confirm-dialog-window-general.component';
import { PvEmpToResMapComponent } from '../../../ProgectManagement/add-resource/pv-emp-to-res-map/pv-emp-to-res-map.component';
import { CreateUpdateReqRemoveResourceComponent } from '../../../ProgectManagement/remove-resource/create-update-req-remove-resource/create-update-req-remove-resource.component';
import { CreateUpdateTransferResourceReqComponent } from '../../../ProgectManagement/transfer-employee/create-update-transfer-resource-req/create-update-transfer-resource-req.component';
import { CreateUpdateTransferWithReplacementComponent } from '../../../ProgectManagement/transfer-with-replacement/create-update-transfer-with-replacement/create-update-transfer-with-replacement.component';
import { SwapEmployeesComponent } from '../../../ProgectManagement/swap-employees/swap-employees.component';
import { CreateUpdateReqReplaceEmployeeComponent } from '../../../ProgectManagement/replace-employee/create-update-req-replace-employee/create-update-req-replace-employee.component';


@Component({
  selector: 'app-attendance-payroll-report2',
  templateUrl: './attendance-payroll-report2.component.html'
})
export class AttendancePayrollReport2Component extends ParentOptMgtComponent implements OnInit {
  result: any = { rows: [], columns: [] };
  filteredRows: Array < any >= [];
  
  rows: Array<any> = [];
  isLoading: boolean = false;
  isArabic: boolean = false;

  employeesGroup: Array<any> = [];

  projectCode: string = '';
  siteCode: string = '';
  fromDate: Date = new Date();
  toDate: Date = new Date();
  minDate: Date = new Date();
  maxDate: Date = new Date();

  projectSelectionList: Array<any> = [];
  siteSelectionList: Array<any> = [];
  //form: FormGroup;

  readonly: boolean = false;
  isFromProjectSiteActions: boolean = true;

  projectData: any;
  siteData: any;
  listOfShiftCodes2: Array<any> = [];
  reportType: string = "TimeSheet"; // "Attendance";
  filterType: string = "employee";
  highlightPostedAtt: boolean = false;
  highlightPendingAtt: boolean = false;

  alternativeAttedanceMapping: any = { r1: -1, r2: -1, c: -1 };
  columnGrandTotal: any = {
    contractualShiftTime: 0,
    attendedTime: 0,

  };
  rowGrandTotal: any = { contractualShiftTime: 0, wdc: 10, AL: 0, UL: 0, SL: 0, EL:0,STL:0,Ab:0,OT:0,lateHrs:0,W:0,OF:0};
  pageNumber: number = 0;
  pageSize: number = 1;
  totalRowsCount: number = 0;
  totalEmployeesCount: number = 0;



  tempAltEmployeeNumber: string = "";

  filter: any = { employeeNumber: "", shiftCode: "" };
  searchKey: string = "";
  dataProcessedTime: Date = new Date();
  isDataProcessed: boolean = false;
  time: number = 0;
  interval: any;
  waitingTime: number = 60;   // should enter data within 10 seconds
  activeColumn: number = -1;
  inputData: any;
  shiftTypeNames: Array<any> = [{ number: 0, nameEn: "OFF", nameAr: "عطلةss" },
  { number: 1, nameEn: "Shift 1", nameAr: "الوردية الأولى" },
  { number: 2, nameEn: "Shift 2", nameAr: "الوردية الثانية" },
  { number: 3, nameEn: "Shift 3", nameAr: "الوردية الثالثة" },
    { number: 4, nameEn: "Shift 4", nameAr: "الوردية الثالثة" }];
  roasterReport: any = { rows: [] };
  constructor(public datePipe: DatePipe, public dialog: MatDialog, private oprService: OprServicesService, private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<AttendancePayrollReport2Component>) {
    super(authService);
  }

  ngOnInit(): void {
    this.isArabic = this.utilService.isArabic();
    if (this.isFromProjectSiteActions) {
      this.maxDate = this.inputData.oldestRoasterStartDate;
      this.setForm();
      
    }
  }

  setForm() {
  //  this.form = this.fb.group({});

    if (this.isFromProjectSiteActions) {
      this.readonly = true;
      this.getProjectData();
      this.getSiteData();
      this.loadShiftCodes();
    }

  }
  submit() { }
  loadProjectSelectionList() {
    this.apiService.getall('project/getSelectProjectList2').subscribe(res => {
      this.projectSelectionList = res as Array<any>;
    });

  }
  loadSiteSelectionListForProject() {
    this.apiService.getall(`projectSites/getSelectProjectSitesListByProjectCode/${this.projectCode}`).subscribe(res => {
      this.siteSelectionList = res as Array<any>;

    });


  }


  getProjectData() {
    this.apiService.getall(`project/getProjectByProjectCode/${this.projectCode}`).subscribe(res => {
      this.projectData = res as any;

    });
  }


  getSiteData() {
    this.apiService.getall(`ProjectSites/getProjectSiteByProjectAndSiteCode/${this.projectCode}/${this.siteCode}`).subscribe(res => {
      this.siteData = res as any;

    });
  }





  getReport() {
    this.alternativeAttedanceMapping = { r1: -1, r2: -1, c: -1 };
        let day: number = this.fromDate.getDate();

    if (day > 28 /*|| (day != 1 && day < 6)*/) {
      this.notifyService.showInfo("Invalid Payroll Date: Select Date between 1-28");

    }
    else {
      let fd: Date = new Date(this.fromDate);
      fd.setMinutes(fd.getMinutes() - fd.getTimezoneOffset());
      this.fromDate = fd;
      this.getReports();


    }

  }

getRoasterReport(input:any){
  this.apiService.post('Reports/getRoasterPayrollReport', input).subscribe((res: any) => {
    this.roasterReport = res;

  });

}
  calculateRowGrandTotal() {
    this.rowGrandTotal = { contractualShiftTime: 0, wdc: 0, AL: 0, UL: 0, SL: 0, EL: 0, STL: 0, Ab: 0, OT: 0, lateHrs: 0, W: 0, OF: 0, attendedShiftsTime:0};


    for (let r = 0; r < this.result?.rows.length; r++) {
      if (r == 0 || this.result.rows[r].employeeNumber != this.result.rows[r - 1].employeeNumber) {
        this.rowGrandTotal.wdc += this.result.rows[r].wdc;
        this.rowGrandTotal.attendedShiftsTime += this.result.rows[r].attendedShiftsTime;


        this.rowGrandTotal.contractualShiftTime += this.result.rows[r].contractualShiftTime;

        this.rowGrandTotal.allottedShiftTime += this.result.rows[r].allottedShiftTime;
        this.rowGrandTotal.AL += this.result.rows[r].AL;
        this.rowGrandTotal.UL += this.result.rows[r].UL;
        this.rowGrandTotal.SL += this.result.rows[r].SL;
        this.rowGrandTotal.EL += this.result.rows[r].EL;
        this.rowGrandTotal.STL += this.result.rows[r].STL;
        this.rowGrandTotal.Ab += this.result.rows[r].Ab;
        this.rowGrandTotal.OT += (this.result.rows[r].OT);
        this.rowGrandTotal.lateHrs += this.result.rows[r].lateHrs;
        this.rowGrandTotal.W += this.result.rows[r].W;
        this.rowGrandTotal.OF += ((this.result.rows[r].contractualShiftTime - this.result.rows[r].allottedShiftTime) / (this.result.siteWorkingTime));

      }
    }
  }
  getReports() {
    if (this.projectCode != '' && this.siteCode != '') {
      let input: any = {
        projectCode: this.projectCode, siteCode: this.siteCode, payrollStartDate: this.fromDate,
        pageNumber: this.pageNumber,
        pageSize: this.pageSize,
        employeeNumber:""
      };
      this.isLoading = true;
      this.apiService.post('Reports/getAttendancePayrollReport', input).subscribe((res: any) => {
        this.result = res as any;
        if (res?.rows?.length > 0) {
          if (res?.isExistEmptyShifts) {
            this.notifyService.showWarning("Incomplete_Shift_Locks");
          }
          this.fromDate = res?.columns[0].attnDate;
          this.toDate = res?.columns[res?.columns?.length - 1].attnDate;
          this.totalEmployeesCount = res.totalEmployeesCount;
          this.loadRemainingData(input);
          this.getRoasterReport(input);
          this.RefreshOffs();

        }
        else {
          this.notifyService.showInfo("No Roasters Found");
          this.isLoading = false;
        }
      }, error => {
        this.isLoading = false;
        this.notifyService.showError(error.error.message);
      });
    }
  }


  loadRemainingData(inputQuery: any) {
    inputQuery.pageNumber++;
    if (inputQuery.pageNumber > Math.floor(this.totalEmployeesCount / this.pageSize)) {
      this.employeesGroup = [];
      this.isLoading = false;
      this.generateReportAndCalculations();
      this.startTimer();
    }
    else {
      this.apiService.post('Reports/getAttendancePayrollReport', inputQuery).subscribe((res: any) => {
        this.result.rows = this.result.rows.concat(res['rows']) as Array<any>;
        this.loadRemainingData(inputQuery);
      });
    }
  }
  filterRows(rows: Array<any>) {
    return rows.filter(e=>e.employeeNumber);
  }

  generateReportAndCalculations() {
     this.employeesGroup = [];
    this.result.rows.sort((a:any, b:any) => (a.employeeNumber < b.employeeNumber ? -1 : 1));
    this.result.rows.forEach((e: any) => {
      if (e.uniqueShiftsCount > 0 && this.employeesGroup.indexOf((x: any) => x.employeeNumber == e.employeeNumber)<0) {

        this.employeesGroup.push({
          employeeNumber: e.employeeNumber
        });
      }
    });
    this.calculateColumnTotals(this.result);
    this.calculateRowTotals(this.result);
    this.updateActiveColumn();
    this.calculateRowGrandTotal();
    
  }
  updateActiveColumn() {
    this.activeColumn = -1;
    for (let c = 0; c < this.result.columns.length; c++) {
      if ((this.result.rows.findIndex((m: any) => m.attendanceMatrix[c].attndId > 0) < 0) && (!this.result.columns[c].isHoliday))
      {

        if ((this.result.rows.findIndex((m: any) => m.shiftCode != "O" && m.attendanceMatrix[c].attnFlag=="-") >= 0))
        {
          this.activeColumn = c;
          break;
        }
        

        
      }
    }
  }
  calculateRowTotals(res: any) {

    for (let r2 = 0; r2 < res?.rows.length; r2++) {
      let employeeRows = res?.rows.filter((e: any) => e.employeeNumber == res?.rows[r2].employeeNumber) as Array<any>;
      let wdc = 0;
      let contractualShiftTime = 0;
      let allottedShiftTime = 0;
      let attendedShiftsTime = 0;
      let AL = 0, EL = 0, SL = 0, STL = 0, UL = 0;
      let Ab = 0, W = 0, OT = 0, lateHrs = 0;
      for (let r = 0; r < employeeRows.length; r++) {
        for (let c = 0; c < res?.columns.length; c++) {
          contractualShiftTime += employeeRows[r].attendanceMatrix[c].contractualShiftTime;
          allottedShiftTime += employeeRows[r].attendanceMatrix[c].allottedShiftTime;
          attendedShiftsTime += employeeRows[r].attendanceMatrix[c]?.attendedTime;
          if (employeeRows[r].attendanceMatrix[c].allottedShiftTime > 0) {
            wdc++;
          }
          if (employeeRows[r].attendanceMatrix[c]?.isOnLeave == true) {   //leave or withdrawal
            if (employeeRows[r].attendanceMatrix[c].attnFlag == "AL") {
              AL++;
            }
            else if (employeeRows[r].attendanceMatrix[c].attnFlag == "UL") {
              UL++;
            }
            else if (employeeRows[r].attendanceMatrix[c].attnFlag == "CL") {
              SL++;
            }
            else if (employeeRows[r].attendanceMatrix[c].attnFlag == "STL") {
              STL++;
            }
            else if (employeeRows[r].attendanceMatrix[c].attnFlag == "EL") {
              EL++;
            }
            else if (employeeRows[r].attendanceMatrix[c].attnFlag == "W") {
              W++;
            }
          }
          else if (employeeRows[r].attendanceMatrix[c]?.attnFlag == "S" || employeeRows[r].attendanceMatrix[c]?.attnFlag == "A") {
            Ab++;
          }
          else if (employeeRows[r].attendanceMatrix[c]?.refIdForAlt > 0) {
            OT += employeeRows[r].attendanceMatrix[c]?.attendedTime ?? 0;
          }
          else if (employeeRows[r]?.attendanceMatrix[c]?.refIdForAlt == 0 && (employeeRows[r]?.attendanceMatrix[c]?.allottedShiftTime < employeeRows[r]?.attendanceMatrix[c]?.attendedTime)) {
            OT += (employeeRows[r]?.attendanceMatrix[c]?.attendedTime - employeeRows[r].attendanceMatrix[c]?.allottedShiftTime);
          }

          if (employeeRows[r].attendanceMatrix[c]?.attndId > 0 && employeeRows[r].attendanceMatrix[c]?.attnFlag != "S" && employeeRows[r].attendanceMatrix[c]?.attnFlag != "A" && employeeRows[r].attendanceMatrix[c]?.allottedShiftTime > employeeRows[r]?.attendanceMatrix[c]?.attendedTime) {

            lateHrs += (employeeRows[r]?.attendanceMatrix[c]?.allottedShiftTime - employeeRows[r]?.attendanceMatrix[c]?.attendedTime);
          }
        }
      }
      res.rows[r2].contractualShiftTime = contractualShiftTime;
      res.rows[r2].wdc = wdc;
      res.rows[r2].allottedShiftTime = allottedShiftTime;
      res.rows[r2].attendedShiftsTime = attendedShiftsTime;
      res.rows[r2].AL = AL;
      res.rows[r2].UL = UL;
      res.rows[r2].SL = SL;
      res.rows[r2].EL = EL;
      res.rows[r2].STL = STL;
      res.rows[r2].Ab = Ab;
      res.rows[r2].OT = OT / 60;
      res.rows[r2].lateHrs = lateHrs / 60;
      res.rows[r2].W = W;
    }
  }
  calculateColumnTotals(res: any) {
    this.columnGrandTotal.contractualShiftTime = 0;
    this.columnGrandTotal.attendedTime = 0;
    for (let c = 0; c < res?.columns.length; c++) {
      let allottedShiftTime: number = 0;
      let attendedTime: number = 0;
      let contractualShiftTime: number = 0;
      for (let r = 0; r < res?.rows.length; r++) {
        allottedShiftTime += res?.rows[r].attendanceMatrix[c]?.allottedShiftTime;
        attendedTime += res?.rows[r].attendanceMatrix[c]?.attendedTime;
        contractualShiftTime += res?.rows[r].attendanceMatrix[c]?.contractualShiftTime;
      }
      res.columns[c].allottedShiftTime = allottedShiftTime / 60;
      res.columns[c].attendedTime = attendedTime / 60;
      res.columns[c].contractualShiftTime = contractualShiftTime / 60;

      this.columnGrandTotal.contractualShiftTime += res.columns[c].contractualShiftTime;
      this.columnGrandTotal.attendedTime += res.columns[c].attendedTime;
    }
  }

  getEmpRow(r: number): number {
    return this.employeesGroup.findIndex((e: any) => e.employeeNumber == this.result.rows[r].employeeNumber);
  }

  getEmpBackGround(r: number, i: number): string {
    let row = this.getEmpRow(r) + i;
    if (row % 2 == 0)
      return `bgColorEven`;
    else
      return `bgColorOdd`;
  }

  getCellBackGround(row: number, col: number): string {
    if (this.activeColumn == col)
      return `bgColorActiveColumn`;
    if (this.alternativeAttedanceMapping.r1 == row && this.alternativeAttedanceMapping.c == col) {
      return `bgColorPrimaryAttnd`;
    }
    else if (this.alternativeAttedanceMapping.r2 == row && this.alternativeAttedanceMapping.c == col) {
      return `bgColorAltAttnd`;
    }
    
    else {
      let emprow = this.getEmpRow(row);

      if (this.result.rows[row].attendanceMatrix[col]?.isPosted && (this.highlightPostedAtt)) {
        return `bgColorPosted`;
      }
      else if (this.result.rows[row].attendanceMatrix[col].attnFlag == 'A') {
        return `bgColorAbsent`
      }
      else if (this.result.rows[row].attendanceMatrix[col].attnFlag == 'OT' || this.result.rows[row].attendanceMatrix[col].refIdForAlt > 0) {
        return `bgColorOT`
      }
      

      else if (this.result.rows[row].attendanceMatrix[col]?.isTransfered && this.result.rows[row].attendanceMatrix[col].attnFlag == "-") {
        return `bgColorTransfer`
      }
      else if (this.result.rows[row].attendanceMatrix[col]?.isTerminated && this.result.rows[row].attendanceMatrix[col].attnFlag == "-") {
        return `bgColorTerminated`
      }
      else if (this.result.rows[row].attendanceMatrix[col]?.isResigned && this.result.rows[row].attendanceMatrix[col].attnFlag == "-") {
        return `bgColorResign`
      }
      else if (this.result.rows[row].attendanceMatrix[col]?.isOnLeave && this.result.rows[row].attendanceMatrix[col].altAttId == 0) {
        return `bgColorLeave`
      }
      else if (this.result.rows[row].attendanceMatrix[col]?.attnFlag == '-') {
        if (this.highlightPendingAtt && !this.result.columns[col]?.isHoliday && this.result.rows[row].shiftCode!="O")
          return `bgColorAttendancePending`;
      }
      if (this.result.rows[row].attendanceMatrix[col].attnFlag == 'S' || this.result.rows[row].attendanceMatrix[col].attnFlag == 'O') {
        return `bgColorSwap`
      }
     if (this.result.rows[row].attendanceMatrix[col].altAttId > 0)
        return `bgColorSwap`

      if (this.result.rows[row].attendanceMatrix[col].attnFlag == "" || (this.result.columns[col].isHoliday && this.result.rows[row].attendanceMatrix[col].attnFlag=="-")) {
        return `bgColorNoShift`
      }

       if (emprow % 2 == 0)
        return `bgColorEven`;
      else
        return `bgColorOdd`;
    }
  }

  openDatePicker(dp: any) {
    dp.open();
  }


  closeModel() {
    this.dialogRef.close();
  }

  exportexcel(): void {
    let element = document.getElementById("excel_table");
    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(element);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Sheet1");
    XLSX.writeFile(wb, this.reportType + "_" + this.projectCode + "_" + this.siteCode + ".xlsx");
  }
  FindAlternativeAttedanceMapping(row: number, col: number) {
    this.alternativeAttedanceMapping = { r1: -1, r2: -1, c: col }
    let selectedCell: any = this.result.rows[row]?.attendanceMatrix[col];
    if (selectedCell.attndId > 0) {
      if (selectedCell.altAttId > 0) {
        this.alternativeAttedanceMapping.r1 = row;
        let altCellRow = this.result.rows.findIndex((e: any) => e.attendanceMatrix[col].attndId == selectedCell.altAttId);
        this.alternativeAttedanceMapping.r2 = altCellRow;
      }
      else if (selectedCell.refIdForAlt > 0) {
        let refCellRow = this.result.rows.findIndex((e: any) => e.attendanceMatrix[col].attndId == selectedCell.refIdForAlt);
        this.alternativeAttedanceMapping.r1 = refCellRow;
        this.alternativeAttedanceMapping.r2 = row;
      }
    } else { this.alternativeAttedanceMapping = { r1: -1, r2: -1, c: -1 }; }
  }

  getToolTip(row: number, col: number, Head: any): string {
    var datePipe = new DatePipe("en-US");
    let date = col == -1 ? "" : datePipe.transform(this.result.columns[col]?.attnDate, 'dd/MM/yyyy');
    let empName = this.isArabic ? this.result.rows[row].employeeNameAr : this.result.rows[row].employeeName;
    let string =
      date = col == -1 ? `${Head}  : 
Employee Number : ${this.result.rows[row].employeeNumber}.
Employee Name : ${empName}.
`
        : `Employee Number : ${this.result.rows[row].employeeNumber}.
Employee Name : ${empName}.
Shift : ${this.result.rows[row].shiftCode}.
Date : ${date}.
Attendance ID: ${this.result.rows[row].attendanceMatrix[col]?.attndId}.
Alternate AttID: ${this.result.rows[row].attendanceMatrix[col].altAttId}`;
    return string;
  }
  translateToolTip(str:string) {
    return this.translate.instant(str);
  }
  openPrint() {
    const printContent = document.getElementById("excel_table") as HTMLElement;
    const WindowPrt: any = window.open('', '', 'left=0,top=0,width=2000,height=1000,toolbar=0,scrollbars=0,status=0');
    setTimeout(() => {
      WindowPrt.document.write(printContent.innerHTML);
      WindowPrt.document.close();
      WindowPrt.focus();
      WindowPrt.print();
      WindowPrt.close();
    }, 50);

  }

 

  loadShiftCodes() {
    this.apiService.getall(`ShiftMaster/getShiftsByProjectAndSiteCode2/${this.projectCode}/${this.siteCode}`).subscribe(res2 => {
      this.listOfShiftCodes2 = res2 as Array<any>;


      this.listOfShiftCodes2.sort((a: any, b: any) => (a.shiftCode == "O" ? -1 : a.inTime < b.inTime ? -1 : 1));
      const isOffExist: boolean = this.listOfShiftCodes2.findIndex(e => e.shiftCode == "O") >= 0;
      for (let i = 0; i < this.listOfShiftCodes2.length; i++) {
        this.listOfShiftCodes2[i].shiftNumberNameEN = isOffExist ? this.shiftTypeNames[i].nameEn : this.shiftTypeNames[i + 1].nameEn;
        this.listOfShiftCodes2[i].shiftNumberNameAR = isOffExist ? this.shiftTypeNames[i].nameAr : this.shiftTypeNames[i + 1].nameAr;
        this.listOfShiftCodes2[i].shiftNumber = isOffExist ? i:i+1;
      }

      

    });
  }

  singClick(row: number, col: number) {
    this.FindAlternativeAttedanceMapping(row, col);
    if (this.result.rows[row].attendanceMatrix[col].attnFlag != "" && this.result.rows[row].attendanceMatrix[col].attndId == 0) {
      this.manualAttendance(row, col);

    }
  }
  onRightClick(r: number, c: number) {
     if (this.result.rows[r].attendanceMatrix[c].attndId > 0 && this.result.rows[r].attendanceMatrix[c].refIdForAlt==0) {
      this.updateManualAttendance(r, c);
    }
     else if (this.result.rows[r].attendanceMatrix[c].attnFlag == "") {
       this.openPvRequestWindow(r, c);
    }
    return false;
  }
  manualAttendance(row: number, col: number) {
    let empRow = this.getEmpRow(row);

    var datePipe = new DatePipe("en-US");
   // let dateStr = datePipe.transform(this.result.columns[col]?.attnDate, 'dd-MM-yyyy') as string;
    let dateStr2 = datePipe.transform(this.result.columns[col]?.attnDate, 'yyyy-MM-dd') as string;
    let inputQuery: any = {
      employeeNumber: this.employeesGroup[empRow].employeeNumber,
      projectCode: this.projectCode,
      siteCode: this.siteCode,
      date: this.result.columns[col].attnDate
    };
    this.apiService.post('monthlyRoaster/getSingleRoasterForEmployee', inputQuery).subscribe((roaster: any) => {
      this.apiService.getall(`EmployeesAttendance/getSingleEmployeeAttendance/${this.projectCode}/${this.siteCode}/${this.employeesGroup[empRow].employeeNumber}/${dateStr2}/${this.result.rows[row].shiftCode}`).subscribe((singleAttendance: any) => {




        let inputCellData: any = {
          r: row,
          c: col,
          totRows: this.employeesGroup.length,
        };




        let isOff = this.result.rows[row].shiftCode == "O";
        let inputData: any = {
          id: singleAttendance.id,
          projectCode: this.projectCode,
          siteCode: this.siteCode,
          attnDate: dateStr2,
          shiftCode: this.result.rows[row].shiftCode,
          employeeNumber: this.result.rows[row].employeeNumber,
          employeeID: singleAttendance?.employeeID??0,
          isDefShiftOff: isOff,
          isActive: true,
          inTime: this.listOfShiftCodes2.find((e: any) => e.shiftCode == this.result.rows[row].shiftCode)?.inTime,
          outTime: this.listOfShiftCodes2.find((e: any) => e.shiftCode == this.result.rows[row].shiftCode)?.outTime,
          isDefaultEmployee: !(this.listOfShiftCodes2.find((e: any) => e.shiftCode == this.result.rows[row].shiftCode)?.isOff),
          isPrimarySite: true,
          isPosted: singleAttendance?.isPosted,
          altEmployeeNumber: this.result.rows[row].employeeNumber,
          altShiftCode: this.result.rows[row].shiftCode,
          attendance: 'P',
          refIdForAlt: 0,

          leavesData: singleAttendance?.leavesData,
          transORresignData: singleAttendance?.transORresignData,
          isOnLeave: singleAttendance?.isOnLeave,
          isWithDrawn: singleAttendance?.isWithDrawn,
          isResigned: singleAttendance?.isResigned,
          isTransfered: singleAttendance?.isTransfered,
          isTerminated: singleAttendance?.isTerminated,
          daywiseConsolidateData: this.getDayWiseConsolidateData(row, col),

          roasterData: roaster, r: row, c: col, shiftCodesSelectionList: this.listOfShiftCodes2          //for update shift in manual attendance popup
        };

        this.openManualAttendanceDialogWindow(inputCellData, inputData, DBOperation.create, 'Manual_Attendance', 'Save', ManualAttendancePopupComponent);

      });
    });
  }
  private openManualAttendanceDialogWindow(inputCellData: any, inputData: any, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {



    if (!this.isLoading) {
      this.isLoading = true;
      let dialogRef = this.oprService.openAutoWidthDialog(this.dialog, Component);
      (dialogRef.componentInstance as any).dbops = dbops;
      (dialogRef.componentInstance as any).modalTitle = modalTitle;
      (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
      (dialogRef.componentInstance as any).inputData = inputData;
      (dialogRef.componentInstance as any).inputCellData = inputCellData;
      dialogRef.afterClosed().subscribe(outputFromManualAtt => {
        this.isLoading = false;
        if (outputFromManualAtt?.isUpdatedShift) {
          inputData.altEmployeeNumber = "";
          this.removeAndUpdateEmployeeRows(inputData);
        }
        else if (outputFromManualAtt?.res == true) {
          
          this.removeAndUpdateEmployeeRows(outputFromManualAtt?.outputData);
        }
      },error => {
          this.isLoading = false;
        }
      );
    }
  }
  removeAndUpdateEmployeeRows(output: any) {
    this.alternativeAttedanceMapping = { r1: -1, r2: -1, c: -1 };
    let employeeNumber = output?.employeeNumber as string;
    let altEmployeeNumber = output?.altEmployeeNumber as string;
    if (employeeNumber != "") {
      this.employeesGroup = this.employeesGroup.filter(e => e.employeeNumber != employeeNumber);
      this.result.rows = this.result.rows.filter((e: any) => e.employeeNumber != employeeNumber);
      this.insertNewEmployee(employeeNumber);
    }
    if (altEmployeeNumber != "" && employeeNumber != altEmployeeNumber) {
      this.employeesGroup = this.employeesGroup.filter(e => e.employeeNumber != altEmployeeNumber);
      this.result.rows = this.result.rows.filter((e: any) => e.employeeNumber != altEmployeeNumber);
      this.insertNewEmployee(altEmployeeNumber);
    }

    
  }

  

insertNewEmployee(employeeNumber:string){
  let input: any = {
    projectCode: this.projectCode, siteCode: this.siteCode, payrollStartDate: this.fromDate,
    pageNumber: 0,
    pageSize: 1,
    employeeNumber: employeeNumber
  };
 
  this.apiService.post('Reports/getAttendancePayrollReport', input).subscribe((res: any) => {
    this.tempAltEmployeeNumber = "";
    if (res['rows'].length > 0) {
      this.result.rows = this.result.rows.concat(res['rows']);
      
      this.generateReportAndCalculations();
    }
  });
}

  private openUpdateAttendanceDialogWindow(inputCellData: any, inputData: any, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {
        if (!this.isLoading) {
      this.isLoading = true;
      let dialogRef = this.oprService.openAutoWidthDialog(this.dialog, Component);
      (dialogRef.componentInstance as any).dbops = dbops;
      (dialogRef.componentInstance as any).modalTitle = modalTitle;
      (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
      (dialogRef.componentInstance as any).inputData = inputData;
      (dialogRef.componentInstance as any).inputCellData = inputCellData;
      dialogRef.afterClosed().subscribe(outputFromUpdateManualAtt => {
        this.isLoading = false;
       
        if (outputFromUpdateManualAtt?.res == true) {

          if (this.tempAltEmployeeNumber != "" && outputFromUpdateManualAtt.outputData.altEmployeeNumber == "") {
            outputFromUpdateManualAtt.outputData.altEmployeeNumber = this.tempAltEmployeeNumber;
            this.tempAltEmployeeNumber = "";
          }
          else if (this.tempAltEmployeeNumber != "" && outputFromUpdateManualAtt.outputData.employeeNumber == "") {
            outputFromUpdateManualAtt.outputData.altEmployeeNumber = this.tempAltEmployeeNumber;
            this.tempAltEmployeeNumber = "";
          }
          this.removeAndUpdateEmployeeRows(outputFromUpdateManualAtt.outputData);
        }
      }, error => {
        this.isLoading = false;
      }
      );
    }
  }
  minsToStrHrsMins(Mins: number) {
    let H = Math.floor(Mins/60);
    let M = Mins%60;
    let Mstr = M.toString().padStart( 2,"0");
    return `${H}:${Mstr}`
  }
  hrsToStrHrsMins(Hrs: number) {
    let H = Math.round(Hrs);
    let M = Math.round((Hrs - H) * 60);
    let Mstr = M.toString().padStart( 2,"0");
    return `${H}:${Mstr}`
  }


  updateManualAttendance(r: number, c: number) {

    var datePipe = new DatePipe("en-US");
    let empRow = this.getEmpRow(r);
    
    let date: Date = this.result.columns[c]?.attnDate as Date;
    let dateStr = datePipe.transform(this.result.columns[c]?.attnDate, 'yyyy-MM-dd') as string;
      let curDate: Date = new Date();
      if (curDate <= date) {

        this.notifyService.showError(this.translate.instant("Can_not_Enter_Attendance_For_Future_Date"));
      }
      else {
        let inputCellData: any = {
          r: r,
          c: c,
          totRows: this.employeesGroup.length,

        };

        this.apiService.getall(`EmployeesAttendance/getSingleEmployeeAttendance/${this.projectCode}/${this.siteCode}/${this.employeesGroup[empRow].employeeNumber}/${dateStr}/${this.result.rows[r].shiftCode}`).subscribe((singleAttendance: any) => {
          this.tempAltEmployeeNumber = singleAttendance.altEmployeeNumber??"";
          singleAttendance.daywiseConsolidateData = this.getDayWiseConsolidateData(r, c);
          
          this.openUpdateAttendanceDialogWindow(inputCellData,singleAttendance as any, DBOperation.create, 'Update_Manual_Attendance', 'Save', UpdateManualAttendancePopupComponent);

        });
      

      }

  }
  getDayWiseConsolidateData(r: number, c: number) {
   return {
      tch: this.result?.columns[c]?.contractualShiftTime,
        tch_min: (this.result?.columns[c]?.contractualShiftTime * 60) % 60,
          tcs: (this.result?.columns[c]?.contractualShiftTime * 60) / (this.result?.siteWorkingTime),
            ot_count: 0,
              mins: ((this.result?.columns[c]?.attendedTime * 60) % 60),
                hours: Math.floor(this.result?.columns[c]?.attendedTime)
    } as any
  }
  isShiftFilterApplied(): boolean {
    if (this.filterType=="shiftCode" &&this.searchKey!="") {
      return true;
    }
    else
    return false;
  }

  applyFilter() {
    if (this.filterType == "employee") {
      this.filter.employeeNumber = this.searchKey;
      this.filter.shiftCode = ""
    }
    else if (this.filterType == "shiftCode") {
      this.filter.shiftCode = this.searchKey;
      this.filter.employeeNumber = "";
    }
  }

  canShowRow(row: any): boolean {
    if (this.searchKey == "") {
      return true;
    }
    else if (this.filterType == "shiftCode" && row?.shiftCode == this.searchKey) {
      return true;
    }
    else if (this.filterType == "employee" && (row?.employeeNumber.includes(this.searchKey) || (!this.isArabic ? row?.employeeName.toLowerCase().includes(this.searchKey.toLowerCase()) : row?.employeeNameAr.includes(this.searchKey))) ){
      return true;
    }

   
      return false;
  }



  startTimer() {
    this.dataProcessedTime = new Date();
    this.interval = setInterval(() => {

      let cdate: Date = new Date();
      let ps: number = this.dataProcessedTime.getHours() * 60 * 60 + this.dataProcessedTime.getMinutes() * 60 + this.dataProcessedTime.getSeconds();
      let cs: number = cdate.getHours() * 60 * 60 + cdate.getMinutes() * 60 + cdate.getSeconds();
      this.time = (cs - ps);
      if (this.time <this.waitingTime) {
        this.isDataProcessed = true;

      } else {
        this.isDataProcessed = false;
        this.pauseTimer();
      }
    },1000)
  }
  pauseTimer() {
    clearInterval(this.interval);
  }

  reloadData() {
    this.getReports();
  }


  clearAttendance() {
    if (!this.isLoading) {
      let dialogRef = this.oprService.openAutoWidthDialog(this.dialog, ClearattendancewithdateComponent);
      (dialogRef.componentInstance as any).dbops = DBOperation.create;
      (dialogRef.componentInstance as any).modalTitle = 'Clear_Attendance';
      (dialogRef.componentInstance as any).modalBtnTitle = 'Save';
      (dialogRef.componentInstance as any).projectCode = this.projectCode;
      (dialogRef.componentInstance as any).siteCode = this.siteCode;
      (dialogRef.componentInstance as any).maxDate = this.toDate;
      (dialogRef.componentInstance as any).minDate = this.fromDate;

      dialogRef.afterClosed().subscribe(res => {
        this.isLoading = false;
        if (res.res == true || res) {
          this.getReports();
        }
      });
    }
    }



   postAttendance() {
         if (!this.isLoading) {
      let dialogRef = this.oprService.openAutoWidthDialog(this.dialog, PostattendancewithdateComponent);
      (dialogRef.componentInstance as any).dbops = DBOperation.create;
      (dialogRef.componentInstance as any).modalTitle = 'Post_Attendance';
      (dialogRef.componentInstance as any).modalBtnTitle = 'Save';
      (dialogRef.componentInstance as any).maxPostDate = this.toDate;
      (dialogRef.componentInstance as any).minPostDate = this.fromDate;
      (dialogRef.componentInstance as any).projectCode = this.projectCode;
      (dialogRef.componentInstance as any).siteCode = this.siteCode;


           dialogRef.afterClosed().subscribe(res => {
             this.isLoading = false;
          if (res == true) {
            this.getReports();
        }
        else {
        }
      });
    }
  }

  autoAttendance(col: number) {
    const dialogRef = this.oprService.openApprovalDialog(this.dialog, ConfirmDialogWindowGeneralComponent);
    (dialogRef.componentInstance as any).dbops = DBOperation.create;
    (dialogRef.componentInstance as any).modalTitle = "Auto_Attendance";
    (dialogRef.componentInstance as any).confirmType = "general";
    (dialogRef.componentInstance as any).titleQuestion = this.translate.instant("Are_You_Sure?");
    (dialogRef.componentInstance as any).subTitleQuestion = this.translate.instant(col == -1 ? "Do_you_want_to_enter_default_attendance_for_whole_month?" :"Do_you_want_to_enter_default_attendance_for_selected_day?");
    dialogRef.afterClosed().subscribe(res => {
      if (res == true) {
        this.isLoading = true;

        this.enterAutoAttendance(col);
      }

    });
  }


  enterAutoAttendance(col: number) {

    let autoAtt: Array<any> = [];
    let s = col;
    let e = col;
    if (col == -1)      // for entire month
    {
      s = 0;
      e = this.result.columns.length -1;
    }
    for (let c = s; c <= e; c++)
    {
      let isAttendanceExist = this.result.rows.findIndex((e:any) => e.attendanceMatrix[c].attndId >= 0);
      if ((!this.result.columns[c].isHoliday||s==e) && !isAttendanceExist) {
        for (let r = 0; r < this.result.rows.length; r++) {
          if ((!this.result.rows[r].attendanceMatrix[c].isResigned && !this.result.rows[r].attendanceMatrix[c].isTransfered && !this.result.rows[r].attendanceMatrix[c].isTerminated)&&this.result.rows[r].attendanceMatrix[c].attnFlag == "-" && this.result.rows[r].shiftCode != "O") {
            autoAtt.push({
              id: 0,
              projectCode: this.projectCode,
              siteCode: this.siteCode,
              attnDate: this.result.columns[c].attnDate,
              shiftCode: this.result.rows[r].shiftCode,
              employeeNumber: this.result.rows[r].employeeNumber,
              isDefShiftOff: false,
              isActive: true,
              inTime: this.listOfShiftCodes2.find((e: any) => e.shiftCode == this.result.rows[r].shiftCode)?.inTime,
              outTime: this.listOfShiftCodes2.find((e: any) => e.shiftCode == this.result.rows[r].shiftCode)?.outTime,
              isDefaultEmployee: true,
              isPrimarySite: true,
              isPosted: false,
              altEmployeeNumber: "",
              altShiftCode: "",
              attendance: 'P',
              refIdForAlt: 0,
              shiftNumber: 1,
            });
          }
        }
      }
    }

    this.interval = setInterval(() => {


    }, 1000);

    if (autoAtt.length == 0) {
      this.isLoading = false;
      this.notifyService.showWarning(this.translate.instant("No_Updations_Found"));
      return;
    }
    else {
      this.apiService.post('EmployeesAttendance/enterAutoAttendance', autoAtt)
        .subscribe(res => {
          this.isLoading = false;
          this.getReports();
        },
          error => {
            this.isLoading = false;
            this.notifyService.showError(this.translate.instant(error?.error?.message));
          });
    }
  }
  getAttendanceFlag(r: number, c: number) {
    let res = this.result.rows[r].attendanceMatrix[c].attnFlag;

    if (this.result.rows[r].attendanceMatrix[c]?.isTransfered && this.result.rows[r].attendanceMatrix[c].attnFlag != "") {
      res = "TR";
    }
    else if (this.result.rows[r].attendanceMatrix[c]?.isResigned && this.result.rows[r].attendanceMatrix[c].attnFlag != "") {
      res = "R";
    }
    else if (this.result.rows[r].attendanceMatrix[c]?.isTerminated && this.result.rows[r].attendanceMatrix[c].attnFlag != "") {
      res = "X";
    }
    else if (this.result.rows[r].attendanceMatrix[c].attnFlag == "S" && !this.result.rows[r].attendanceMatrix[c]?.isOnLeave && this.result.rows[r].shiftCode != "O") {
      res = "A";
    }
    else if (this.result.rows[r].shiftCode == "O" && this.result.rows[r].attendanceMatrix[c].attnFlag == "-") {
      res = 'O';
    }
    else if (this.result.columns[c]?.isHoliday && this.result.rows[r].attendanceMatrix[c].attnFlag == "-") {
      res = 'H';
    }
    
    if (this.reportType == "TimeSheet") {
      if (this.result.rows[r].attendanceMatrix[c]?.attendedTime>0)
        res = this.minsToStrHrsMins(this.result.rows[r].attendanceMatrix[c]?.attendedTime);
      
      
    }
    
      return res;
  }

  getDayBackground(c: number) {
    if (this.result.columns[c]?.isHoliday) {
      return `bgColorDateHeaderHoliday`;
    }
    return;
  }

  getDayTooltip(dt: any) {
    return `${dt?.isHoliday ? this.isArabic ? dt.holidayInf.calanderName_AR : dt.holidayInf.calanderName_EN:''}`
  }
  
  getShiftBgColor(shiftCode: string) {
    let shift = this.listOfShiftCodes2.find(e => e.shiftCode == shiftCode);
    let shiftNumber = shift==null ? 0 :shift.shiftNumber;

    return `bgColorShift_${shiftNumber}`;
  }
  getShiftNumberName(shiftCode: string) {
    let shift = this.listOfShiftCodes2.find(e => e.shiftCode == shiftCode);
    return  shift == null ? "-" : this.isArabic ? shift.shiftNumberNameAR : shift.shiftNumberNameEN;

  }


  getWeekDay(date: Date) {
    let dt = new Date(date);
    let day_of_week = dt.getDay();
    switch (day_of_week) {
      case 0: return "Su";
      case 1: return "M";
      case 2: return "T";
      case 3: return "W";
      case 4: return "Th";
      case 5: return "F";
      case 6: return "S";
      default: return "";
    }
  }

  openPvRequestWindow(r: number, c: number) {
    let dateStr = this.datePipe.transform(this.result.columns[c].attnDate, 'yyyy-MM-dd');

    let input: any = {
      projectCode: this.projectCode,
      siteCode: this.siteCode,
      employeeNumber: this.result.rows[r].employeeNumber,
      date: dateStr
    };

    this.apiService.post('PvAllRequests/getRecentApprovedPvRequestData', input).subscribe((res: any) => {
      if (res?.reqId == 0 || res == null) {
        this.notifyService.showInfo("Employee Not Available");
        return true;
      }
      else {
        if (res?.requestType == 1) {         //EnumPvRequestType

          this.openPvRequestDialogManage(res?.reqId, DBOperation.create, 'Request_For_Add_Resource', 'View', PvEmpToResMapComponent /*ViewPvAddResRequestComponent*/);

        }
        else if (res?.requestType == 2) {
          this.openPvRequestDialogManage(res?.reqId, DBOperation.create, 'Request_For_Remove_Resource', 'View', CreateUpdateReqRemoveResourceComponent);

        }
        else if (res?.requestType == 3) {
          this.openPvRequestDialogManage(res?.reqId, DBOperation.create, 'Request_For_Transfer_Employee', 'View', CreateUpdateTransferResourceReqComponent);

        }
        else if (res?.requestType == 4) {
          this.openPvRequestDialogManage(res?.reqId, DBOperation.create, 'Request_For_Transfer_With_Replacement', 'View', CreateUpdateTransferWithReplacementComponent);

        }
        else if (res?.requestType == 5) {
          this.openPvRequestDialogManage(res?.reqId, DBOperation.create, 'Request_For_Swap_Employees', 'View', SwapEmployeesComponent);

        }
        else if (res?.requestType == 6) {
          this.openPvRequestDialogManage(res?.reqId, DBOperation.create, 'Request_For_Replace_Resource', 'View', CreateUpdateReqReplaceEmployeeComponent);

        }
        return false;
      }
    });
  }

  private openPvRequestDialogManage(id: number, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, Component: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, Component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;

    dialogRef.afterClosed().subscribe(res => {

    });
  }

  RefreshOffs() {
    let dayStr:any = this.datePipe.transform(this.fromDate, 'yyyy-MM-dd')?.substr(8,2);
    let isSingleMonth: boolean= dayStr=="01";

    let dto: any = {
      siteCode: this.siteCode,
      month: (this.datePipe.transform(this.fromDate, 'yyyy-MM-dd')?.substr(5,2)),
      year: this.datePipe.transform(this.fromDate, 'yyyy-MM-dd')?.substr(0,4),
    };
    this.apiService.post('OpUtils/RefreshOffs', dto).subscribe(e => {

    });
    if (!isSingleMonth) {
      let dto: any = {
        siteCode: this.siteCode,
        month: (this.datePipe.transform(this.toDate, 'yyyy-MM-dd')?.substr(5, 2)),
        year: this.datePipe.transform(this.toDate, 'yyyy-MM-dd')?.substr(0, 4),
      };
      this.apiService.post('OpUtils/RefreshOffs', dto).subscribe(e => {

      });
    }

  }

}
