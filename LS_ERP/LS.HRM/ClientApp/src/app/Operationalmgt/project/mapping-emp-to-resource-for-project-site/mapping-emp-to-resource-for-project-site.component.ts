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
import { delay } from "rxjs/operators";
@Component({
  selector: 'app-mapping-emp-to-resource-for-project-site',
  templateUrl: './mapping-emp-to-resource-for-project-site.component.html'
})
export class MappingEmpToResourceForProjectSiteComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  project: any;
  skillsetData: Array<any> = [];
  empToResMapData: Array<any> = [];
  skillsetsListForSite: Array<any> = [];
 
  monthsDataList: Array<any> = [];
  form: FormGroup;
  shiftsForSite: Array<any> = [];
  projectStartDate: Date;
  projectEndDate: Date;
  startYear: number;
  startMonth: number;
  startDay: number;
  endYear: number;
  endMonth: number;
  endDay: number;
  noOfDays: number;
  siteCodeList: Array<any> = [];
  editRow: number = -1;


  empCodeControl = new FormControl('');
  filteredEmployeeNumbers: Observable<Array<CustomSelectListItem>>;
  employeeList: Array<any> = [];
  employeesForProjectSite: Array<any> = [];
  employeeNumber: string = '';
  isDataLoading: boolean = false;

  isArab: boolean = false;

  interval: any;

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<MappingEmpToResourceForProjectSiteComponent>) {
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
    this.isArab = this.utilService.isArabic();
      this.setForm();
    var count = 0;
    this.interval = setInterval(() => {
      count++;
      if (count == 1) {
        this.calculateDates();
       
      }
      else if (count == 2) {
        this.loadEmployees();

      }
      else if (count == 3) {
        this.getSitesList();

      }
        else if (count == 4) {
        this.getSkillsetsForSite(this.project.siteCode);

      }


    }, count>4?0:1000);
    


  
    
    


    
   
  }
  closeModel() {
    this.dialogRef.close();
  }

  setForm() {
    this.form = this.fb.group({
      'projectCode': [this.project.projectCode],
      'customerCode': [this.project.customerCode],
      'siteCode': [this.project?.siteCode != null ? this.project.siteCode : '', Validators.required],
      'startDate': [this.project.startDate.toString().substring(0, 10)],
      'endDate': [this.project.endDate.toString().substring(0, 10)],
      'noOfDays': [this.noOfDays],






     // 'empNumber': [''],




    });

    if (this.project?.siteCode != null) {

      this.form.controls['siteCode'].disable({ onlySelf: true });
      this.loadEmployeesForProjectSite(this.project.siteCode);
      this.getSkillsetsForSite(this.project.siteCode);
      this.loadEmpToResourceMap(this.project.siteCode);
    }
      }

  getSitesList() {
    this.apiService.getall(`customerSite/getSelectSiteListByProjectCode/${this.project.projectCode}`).subscribe(res => {
      this.siteCodeList = res;
    });

  }
  loadEmployees() {

    this.apiService.getall('Employee/getSelectEmployeeList').subscribe(result => {
      this.employeeList = result;

    }, error => this.utilService.ShowApiErrorMessage(error));
  }
  loadEmployeesForProjectSite(siteCode:string) {

    this.apiService.getall(`EmployeesToProjectSite/getEmployeesOfProjectSite/${this.project.projectCode}/${siteCode}`).subscribe(result => {
    
  
      this.employeesForProjectSite = result as Array<any>;
     
      if (this.employeesForProjectSite.length == 0)
        this.notifyService.showError(this.translate.instant("Employees_Not_Assigned_To_Site"));
      else
        this.loadEmpToResourceMap(siteCode);
    }, error => this.utilService.ShowApiErrorMessage(error));
  }
  calculateDates() {
    this.monthsDataList = [];
    let startDate = new Date(this.project.startDate);
    let endDate = new Date(this.project.endDate);
    this.noOfDays = (endDate.getTime() - startDate.getTime()) / (1000 * 60 * 60 * 24) + 1;
    for (let y = startDate.getFullYear(); y <= endDate.getFullYear(); y++) {
      let sm = y == startDate.getFullYear() ? startDate.getMonth() : 1;
      let em = y == endDate.getFullYear() ? endDate.getMonth() : 12;

      for (let m = sm; m <= em; m++) {
        let sd = (m == sm && y == startDate.getFullYear()) ? startDate.getDate() : 1;
        let ed = (m == em && y == endDate.getFullYear()) ? endDate.getDate() : new Date(y, m + 1, 0).getDate();
        let noOfDays = (new Date(y, m, ed).getTime() - new Date(y, m, sd).getTime()) / (1000 * 60 * 60 * 24) + 1;

        let monthData: any = {
          sd: sd,
          ed: ed,
          mStartDate: new Date(y, m, sd).toDateString(),
          mEndDate: new Date(y, m, ed).toDateString(),
          mNoOfDays: noOfDays

        };
        this.monthsDataList.push(monthData);
      }
    }
  }
  loadskillsetData() {
    this.skillsetData = [];
    this.empToResMapData = [];
    for (let ss = 0; ss < this.skillsetsListForSite.length; ss++) {
      let row: any = {
        skillSet: this.skillsetsListForSite[ss].skillSetCode,
        skillSetNameEng: this.skillsetsListForSite[ss].nameInEnglish,
        skillSetNameArb: this.skillsetsListForSite[ss].nameInArabic,
        quantity: this.skillsetsListForSite[ss].quantity,
      };
      this.skillsetData.push(row);
    }


  }

  loadEmpToResourceMap(siteCode:string) {


    this.isDataLoading = true;
   

    this.apiService.getall(`EmployeesToResorceMap/getEmployeesToResourcesMapForProjectSite/${this.project.projectCode}/${siteCode}`).subscribe(res => {
      this.empToResMapData = res as Array<any>;
      console.log(res);
      this.isDataLoading = false;
      console.log(this.skillsetsListForSite);

      for (var i = 0; i < this.empToResMapData.length; i++){
        let map: any = this.empToResMapData[i];
        //map.empNumber = this.employeeList.find((y: any) => y.textTwo == map.employeeID).value;

        map.empNumber = this.employeeList.find((y: any) => y?.value == map?.employeeNumber)?.value;
        map.empName = this.employeeList.find((y: any) => y?.value == map?.employeeNumber)?.text;
        map.empNameAr = this.employeeList.find((y: any) => y?.value == map?.employeeNumber)?.textAr;
        map.skillSetNameEng = this.skillsetsListForSite.find((x: any) => x.skillSetCode == map?.skillSet)?.nameInEnglish;
        map.skillSetNameArb = this.skillsetsListForSite.find((x: any) => x.skillSetCode == map?.skillSet)?.nameInArabic;

      }

      if (this.empToResMapData.length == 0) {
        for (let ss = 0; ss < this.skillsetsListForSite.length; ss++) {
          for (let i = 1; i <= this.skillsetsListForSite[ss].quantity; i++) {

            let row: any = {
              projectCode: this.project.projectCode,
              siteCode: this.form.controls['siteCode'].value,
              skillSet: this.skillsetsListForSite[ss].skillSetCode,
              skillSetNameEng: this.skillsetsListForSite[ss]?.nameInEnglish,
              skillSetNameArb: this.skillsetsListForSite[ss]?.nameInArabic,
              employeeNumber:"",
              employeeID: 0,
              mapId: 0
            };
            this.empToResMapData.push(row);
          }
        }
      }

    
    },
      error => {

        this.isDataLoading = false;
      });
  



 
  }
 
  getJvalue(i: number): number {

    return i % this.skillsetsListForSite.length;
  }
  getSkillsetsForSite(siteCode: string) {
    this.isDataLoading = true;
    if (siteCode != '')
      this.apiService.getall(`Skillset/getSkillsetsByProjectCodeAndSiteCode/${this.project.projectCode}/${siteCode}`).subscribe(res => {
        this.skillsetsListForSite = res;

        //this.getShiftsForSite(siteCode);
        this.loadEmployeesForProjectSite(siteCode);
        this.isDataLoading = false;
      },
        error => {
          this.isDataLoading = false;
        });
    else {
      this.isDataLoading =false;
      this.shiftsForSite = [];
      this.skillsetsListForSite = [];
   
      //this.loadskillsetData();
      //this.loadEmpToResourceMap();
    }
   
  }
  getShiftsForSite(siteCode: string) {
    this.isDataLoading = true;
    this.apiService.getall(`ShiftMaster/getShiftsToSiteBySiteCode/${siteCode}`).subscribe(res => {
      this.shiftsForSite = res;
      this.isDataLoading = false;
      //this.loadskillsetData();
    //  this.loadEmpToResourceMap();
    });
  }
  
  onSelectSiteCode(event: any) {
    this.loadEmployeesForProjectSite(event.target.value);
    delay(1000);
    this.getSkillsetsForSite(event.target.value);
   
   // this.loadEmpToResourceMap(event.target.value);
  }

  filterEmployeeNumbers(val: string): Observable<Array<CustomSelectListItem>> {
    return this.apiService.getall(`EmployeesToProjectSite/getAutoFillEmployeeListForProjectSite/${this.project.projectCode}/${this.form.controls['siteCode'].value}/?search=${val}`)
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
      this.empCodeControl.setValue(this.empToResMapData[this.editRow].employeeNumber);

    }
    else
      this.editRow = -1;
   
    //this.employeeNumber = this.empToResMapData[this.editRow].employeeNumber;
    
   


  }
  autoSelectionEmployeeNumber(event: any) {
    


    //if (this.empToResMapData.findIndex(e => e.employeeID == event.option.value.value) < 0) {
    if (this.empToResMapData.findIndex(e => e.employeeNumber == event.option.value.text) < 0) {

      this.empToResMapData[this.editRow].employeeID = event.option.value.value;
      this.empToResMapData[this.editRow].employeeNumber = event.option.value.text;
      this.empToResMapData[this.editRow].empName = event.option.value.textTwo;
      this.empToResMapData[this.editRow].empNameAr = event.option.value.textAr;
    }
    

      


      

   
    this.editRow = -1;

  }

  submit() {



    if (this.empToResMapData.length != 0 && !this.isDataLoading) {
      //let postDataList: Array<any> = [];
      //this.empToResMapData.forEach((e: any) => {
      //  let item: any = {
      //    mapId: e.mapId,
      //    projectCode: e.projectCode,
      //    siteCode: e.siteCode,
      //    skillSet: e.skillSet,
      //    employeeID: Number(e.employeeID),
      //    employeeNumber: e.empNumber

      //  }

      //  postDataList.push(item);


      //});
      this.isDataLoading = true;

      this.apiService.post('EmployeesToResorceMap/mapEmployeesToResources', this.empToResMapData)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.isDataLoading = false;

          this.dialogRef.close(true);
        },
          error => {
            console.log(error);
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
            this.isDataLoading = false;
          });




    }
    else if (this.isDataLoading) {
      this.notifyService.showInfo("Please Wait...");


    }
    else {

      this.notifyService.showWarning("No_mapping_Data_Founds");

    }

  }
  canSave():boolean {
    return this.empToResMapData.findIndex(e => e.employeeNumber == "") < 0;
  }
}
