import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ParentOptMgtComponent } from '../../../../sharedcomponent/parentoptmgt.component';
import { CustomSelectListItem } from '../../../../models/MenuItemListDto';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { DatePipe } from '@angular/common';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { UtilityService } from '../../../../services/utility.service';
import { ApiService } from '../../../../services/api.service';
import { DBOperation } from '../../../../services/utility.constants';
import { NotificationService } from '../../../../services/notification.service';



@Component({
  selector: 'app-pv-emp-to-res-map',
  templateUrl: './pv-emp-to-res-map.component.html'
})
export class PvEmpToResMapComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number;
  isDataLoading: boolean = false;
  mappingsList: Array<any> = [];
  skillSetsList: Array<CustomSelectListItem> = [];
  isArabic: boolean = false;


  editRow: number = -1;
  empCodeControl = new FormControl('');
  filteredEmployeeNumbers: Observable<Array<CustomSelectListItem>>;
  employeeList: Array<any> = [];
  employeesForProjectSite: Array<any> = [];
  employeeNumber: string = '';

  offDay: string = "-1";
  shiftCodesForProjectSite: Array<CustomSelectListItem> = [];
  isUpdating: boolean = false;
  constructor(private notifyService: NotificationService,public datepipe: DatePipe, private translate: TranslateService, private fb: FormBuilder, private authService: AuthorizeService, private utilService: UtilityService, private apiService: ApiService, public dialogRef: MatDialogRef<PvEmpToResMapComponent>) {

    super(authService);
    this.filteredEmployeeNumbers = this.empCodeControl.valueChanges.pipe(
      // startWith(this.employeeNumber),
      startWith(''),
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
    this.isDataLoading = true;
    this.setForm();
    this.loadSkillSetsList();
    this.apiService.get('PvAddResource/getPvAddResourceEmpToResMapsByReqIdById', this.id).subscribe(res => {
      if (res != null) {
        this.mappingsList = res as Array<any>;
        this.loadEmployeesForProjectSite();
        this.loadShiftCodesForProjectSite();
      }
    });
  

  }


  setForm() {
    this.form = this.fb.group({

    });
   

  }
  submit() {
    if (!this.isUpdating) {

      if (this.mappingsList.findIndex(e => e.employeeNumber == "" || e.defShift == "") >= 0) {
        this.notifyService.showInfo(this.translate.instant("Empty EMployee_Number Or Shift Code"));
      }
      else {
        this.isUpdating = true;
        this.apiService.post('PvAddResource/CreateUpdatePvAddResourceEmployeeToResourceMap', { mappingsList: this.mappingsList }).subscribe(res => {
          this.isUpdating = false;
          if (res) {

            this.utilService.OkMessage();

            this.dialogRef.close(true);

          }
        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
            this.isUpdating = false;
          });
      }
    }
    else {
      this.notifyService.showError(this.translate.instant("Please Wait..."));
    }

  }
  closeModel() {
    this.dialogRef.close();

  }

  ToDateString(date: any) {

    if (date != null)
      return this.datepipe.transform(date.toString(), 'yyyy-MM-dd')?.toString();
    else
      return "";
  }
  getSkillSet(code: string) {
    let ss: any = this.skillSetsList.find(e => e.value == code);
    if (this.isArabic) {
      ss.text = ss.textTwo;
    }
    return ss;
  }


  loadSkillSetsList() {
    this.apiService.getall('Skillset/GetSelectSkillsetList/').subscribe(res => {

      this.skillSetsList = res as Array<any>;

    });

  }

  filterEmployeeNumbers(val: string): Observable<Array<CustomSelectListItem>> {
    return this.apiService.getall(`EmployeesToProjectSite/getAutoFillEmployeeListForProjectSite/${this.mappingsList[0].projectCode}/${this.mappingsList[0].siteCode}/?search=${val}`)
      .pipe(
        map(response => {
          const res = response as Array<any>;
          this.isDataLoading = false;
          return res;
        })
      )
  }

  editMap(row: number) {
    if (this.editRow == -1) {
      this.editRow = row;
      this.empCodeControl.setValue(this.mappingsList[this.editRow].employeeNumber);

    }
    else
      this.editRow = -1;
  }


  loadEmployees() {

    this.apiService.getall('Employee/getSelectEmployeeList').subscribe(result => {
      this.employeeList = result;

    }, error => this.utilService.ShowApiErrorMessage(error));
  }
  loadEmployeesForProjectSite() {

    this.apiService.getall(`EmployeesToProjectSite/getEmployeesOfProjectSite/${this.mappingsList[0].projectCode}/${this.mappingsList[0].siteCode}`).subscribe(result => {


      this.employeesForProjectSite = result as Array<any>;
      if (this.employeesForProjectSite.length == 0)
        this.notifyService.showError(this.translate.instant("Employees_Not_Assigned_To_Site"));

    }, error => this.utilService.ShowApiErrorMessage(error));
  }



  autoSelectionEmployeeNumber(event: any) {

    if (this.mappingsList.findIndex(e => e.employeeNumber == event.option.value.text) < 0) {

      this.mappingsList[this.editRow].employeeID = event.option.value.value;

      this.mappingsList[this.editRow].employeeNumber = event.option.value.text;
      this.mappingsList[this.editRow].empName = event.option.value.textTwo;
      this.mappingsList[this.editRow].empNameAr = event.option.value.textAr;

      this.empCodeControl.setValue(event.option.value.text);

    }
    else {
      this.empCodeControl.setValue("");
      this.mappingsList[this.editRow].employeeNumber = "";
    }






   // this.editRow = -1;

  }

  canSave(): boolean {
    return this.mappingsList.findIndex(e => e.employeeNumber == "") < 0;
  }

  getEmployee(empNumber: string): any {

    if (empNumber == "")
      return { text:""};
    let emp: any = this.employeesForProjectSite.find(e => e.employeeNumber == empNumber);
    emp.text = this.isArabic ? emp.employeeNameAr:emp.employeeName;
    return emp;
  }

  getOffDay(d:any):string {
    switch (d.toString()) {
      case "0": return !this.isArabic ? "Sunday" : "SundayAr"; break;
      case "1": return !this.isArabic ? "Monday" : "MondayAr"; break;
      case "2": return !this.isArabic ? "Tuesday" : "TuesdayAr"; break;
      case "3": return !this.isArabic ? "Wednesday" : "WednesdayAr"; break;
      case "4": return !this.isArabic ? "Thursday" : "ThursdayAr"; break;
      case "5": return !this.isArabic ? "Friday" : "FridayAr"; break;
      case "6": return !this.isArabic ? "Saturday" : "SaturdayAr"; break;
      default: return !this.isArabic ? "No Off Day" :"No Off Day Ar"


    }

  }


  loadShiftCodesForProjectSite() {
    this.apiService.getall(`ShiftMaster/getShiftsByProjectAndSiteCode/${this.mappingsList[0].projectCode}/${this.mappingsList[0].siteCode}`).subscribe(result => {
      this.shiftCodesForProjectSite = result;

    }, error => this.utilService.ShowApiErrorMessage(error));

  }
}
