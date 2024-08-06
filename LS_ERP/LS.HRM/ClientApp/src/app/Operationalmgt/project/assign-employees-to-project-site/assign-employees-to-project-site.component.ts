import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { PaginationService } from '../../../sharedcomponent/pagination.service';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { DBOperation } from '../../../services/utility.constants';



@Component({
  selector: 'app-assign-employees-to-project-site',
  templateUrl: './assign-employees-to-project-site.component.html'
})
export class AssignEmployeesToProjectSiteComponent extends ParentOptMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['employeeNumber', 'employeeName', 'Actions'];
  data: MatTableDataSource<any>;
  totalItemsCount: number;
  searchValue: string = '';
  sortingOrder: string = 'employeeID desc';
  isLoading: boolean = false;
  form: FormGroup;
  employeesList: Array<any> = [];
  selectedEmployeesList: Array<any> = [];

  project: any;
  skillsetsListForSite: Array<any> = [];

  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  projectStartDate: Date;
  projectEndDate: Date;
  startYear: number;
  startMonth: number;
  startDay: number;
  endYear: number;
  endMonth: number;
  endDay: number;
  noOfDays: number;

 

  shiftsForSite: Array<any> = [];
 // siteCode: string = '';
  siteCodeList: Array<any> = [];
  tableData: Array<any> = [];
  footerData: Array<any> = [];
  monthsDataList: Array<any> = [];

  monthlyRoasterForSite: Array<any> = [];
  isRoasterGenerated: boolean = false;

  isArab: boolean = false;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, private authService: AuthorizeService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService, public dialogRef: MatDialogRef<AssignEmployeesToProjectSiteComponent>) {
    super(authService);
  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.initialLoading();
    this.calculateDates();
    this.setForm();
    this.getSitesList();

   
  }

  refresh() {
    this.searchValue = '';
    this.initialLoading();
  }

  initialLoading() {


    this.loadList(0, this.pageService.pageCount, "", this.sortingOrder);
    this.loadEmployees();
    if (this.project?.siteCode!=null) {
      this.getSkillsetsForSite(this.project.siteCode);
      this.getEmployeesOfProjectSite(this.project.siteCode);
    }

  }

  onSortOrder(sort: any) {
    this.totalItemsCount = 0;
    this.sortingOrder = sort.active + ' ' + sort.direction;
    this.loadList(0, this.pageService.pageCount, "", this.sortingOrder);
  }


  onPageSwitch(event: PageEvent) {
    console.log(event);
    this.pageService.change(event);
    this.loadList(event.pageIndex, event.pageSize, "", this.sortingOrder);
  }

  private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
    this.isLoading = true;
    this.apiService.getPagination('Employee/getEmployeesPagedList', this.utilService.getQueryString(page, pageCount, query, orderBy)).subscribe(result => {
      this.totalItemsCount = 0;




      this.data = new MatTableDataSource(result.items);

      this.totalItemsCount = result.totalCount;
      this.data.paginator = this.paginator;
      this.data.sort = this.sort;
      setTimeout(() => this.data.sort = this.sort, 2000);
      this.isLoading = false;
    }, error => this.utilService.ShowApiErrorMessage(error));
  }


  applyFilter(searchVal: any) {
    const search = searchVal;//.target.value as string;
    //if (search && search.length >= 3) {
    if (search) {
      this.searchValue = search;
      this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
    }
  }
  

 
  

  loadEmployees() {

    this.apiService.getall('Employee/getSelectEmployeeList').subscribe(result => {
      this.employeesList = result;

    }, error => this.utilService.ShowApiErrorMessage(error));
  }





  addEmployee(emp: any) {

    if (this.selectedEmployeesList.findIndex((e: any) => e.employeeID == emp.employeeID) < 0) {
      emp.id = 0;
      this.selectedEmployeesList.push(emp);
    }
  }
  removeEmployee(emp: any) {
    let index: number = this.selectedEmployeesList.findIndex((e: any) => e.employeeID == emp.employeeID);
    this.selectedEmployeesList.splice(index, 1);

  }

  isSelected(emp: any) {
    return (this.selectedEmployeesList.findIndex((e: any) => e.employeeNumber == emp.employeeNumber) >= 0)
  }

  

  setForm() {
    this.form = this.fb.group({
      'projectCode': [this.project.projectCode],
      'customerCode': [this.project.customerCode],
      'siteCode': [this.project?.siteCode!=null?this.project.siteCode:'', Validators.required],
      'startDate': [this.project.startDate.toString().substring(0, 10)],
      'endDate': [this.project.endDate.toString().substring(0, 10)],
      'noOfDays': [this.noOfDays]
      

    });

    if (this.project?.siteCode!=null) {
      this.form.controls['siteCode'].disable({ onlySelf: true });
    }

  }
  closeModel() {
    this.dialogRef.close();
  }
  submit() { }

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








  getSitesList() {
    this.apiService.getall(`customerSite/getSelectSiteListByProjectCode/${this.project.projectCode}`).subscribe(res => {
      this.siteCodeList = res;
      
    });

  }



  getSkillsetsForSite(siteCode: string) {
    if (siteCode != '')
      this.apiService.getall(`Skillset/getSkillsetsByProjectCodeAndSiteCode/${this.project.projectCode}/${siteCode}`).subscribe(res => {
        this.skillsetsListForSite = res;

        this.getShiftsForSite(siteCode);

      });
    else {
      this.shiftsForSite = [];
      this.skillsetsListForSite = [];
      this.loadTableData();

    }

  }
  getShiftsForSite(siteCode: string) {
    this.apiService.getall(`ShiftMaster/getShiftsByProjectAndSiteCode/${this.project.projectCode}/${siteCode}`).subscribe(res => {
      this.shiftsForSite = res;
      console.log(res);
      this.loadTableData();
    });
  }


  onSelectSiteCode(event: any) {

    this.getSkillsetsForSite(event.target.value);
    this.getEmployeesOfProjectSite(event.target.value);
  }







  loadTableData() {
    this.tableData = [];
    this.footerData = [];

    for (let m = 0; m < this.monthsDataList.length; m++) {

      let month = this.monthsDataList[m];
      for (let ss = 0; ss < this.skillsetsListForSite.length; ss++) {

        let tablerow: any = {
          sd: this.monthsDataList[m]?.sd,
          ed: this.monthsDataList[m]?.ed,
          startDate: this.monthsDataList[m]?.mStartDate,
          endDate: this.monthsDataList[m]?.mEndDate,
          noOfDays: this.monthsDataList[m].mNoOfDays,
          noOfShifts: this.shiftsForSite.length,
          skillSet: this.skillsetsListForSite[ss]?.skillSetCode,
          skillSetNameEng: this.skillsetsListForSite[ss]?.nameInEnglish,
          skillSetNameArb: this.skillsetsListForSite[ss]?.nameInArabic,
          quantity: this.skillsetsListForSite[ss].quantity,
          totShifts: /*noOfShiftsForSite **/ month.mNoOfDays * this.skillsetsListForSite[ss]?.quantity,
        };


        this.tableData.push(tablerow);


      }


    }


    this.loadFooterData();
    this.checkRoasterForSite();
  }

  loadFooterData() {

    for (let ss = 0; ss < this.skillsetsListForSite.length; ss++) {
      let footerrow: any = {
        skillSet: this.skillsetsListForSite[ss]?.skillSetCode,
        skillSetNameEng: this.skillsetsListForSite[ss]?.nameInEnglish,
        skillSetNameArb: this.skillsetsListForSite[ss]?.nameInArabic,
        quantity: this.skillsetsListForSite[ss]?.quantity,
      };
      this.footerData.push(footerrow);
      
    }


  }


  checkRoasterForSite() {

    this.apiService.getall(`MonthlyRoaster/isExistMonthlyRoasterForProjectSite/${this.project.projectCode}/${this.form.controls['siteCode'].value}`).subscribe(res => {
      this.isRoasterGenerated = res;
    });

  }


  

 
  getJvalue(i: number): number {

    return i % this.skillsetsListForSite.length;
  }
  assignEmployees() {
    let postData: Array<any> = [];
    
   
    this.selectedEmployeesList.forEach((e: any) => {


      let emp: any = {
        id:e.id,
        employeeNumber: e.employeeNumber,
        employeeID: e.employeeID,
        employeeName: e.employeeName,
        employeeNameAr: e.employeeName_AR,
        projectCode: this.project.projectCode,
        siteCode: this.form.controls['siteCode'].value

      }
      postData.push(emp);

    });
   // console.log(postData);

    if (this.selectedEmployeesList.length!=0) {
      this.apiService.post('EmployeesToProjectSite/assignEmployeesToProjectSite',postData)
        .subscribe(res => {
          this.utilService.OkMessage();

          this.checkRoasterForSite();
          this.dialogRef.close(true);
        },
          error => {
            console.log(error);
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
          });
    }
  }

  getEmployeesOfProjectSite(siteCode: string) {

    if (siteCode != '')
      this.apiService.getall(`EmployeesToProjectSite/getEmployeesOfProjectSite/${this.project.projectCode}/${siteCode}`).subscribe((res:Array<any>) => {
        this.selectedEmployeesList = res;
       
        this.getShiftsForSite(siteCode);

      });
    else {
      this.shiftsForSite = [];
      this.skillsetsListForSite = [];
      this.loadTableData();

    }

  }
  getEmployeeData(empNumber: string): any {
    /*console.log(this.employeesList);*/
    return this.employeesList.find((e: any) => e.value == empNumber) as any;
  }
  
}
