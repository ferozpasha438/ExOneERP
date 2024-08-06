import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { FormGroup } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';
import { MonthylRoasterForProjectComponent } from '../../monthyl-roaster-for-project/monthyl-roaster-for-project.component';

@Component({
  selector: 'app-calendar-days',
  templateUrl: './calendar-days.component.html'
})
export class CalendarDaysComponent extends ParentOptMgtComponent implements OnInit {
  project: any;
  form: FormGroup;
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
  noOfDays: number=0;
  shiftsForSite: Array<any>=[];
 // siteCode: string ='';
  siteCodeList: Array<any> = [];
  tableData: Array<any> = [];
  footerData: Array<any> = [];
  monthsDataList: Array<any> = [];
  skillsetsListForSite: Array<any> = [];
  monthlyRoasterForSite: Array<any> = [];
  monthlyRoasterForSiteMonth: Array<any> = [];

  isRoasterGenerated: boolean = true;
  isMonthlyRoasterGenerated: Array<boolean>=[];
  empToResMapData: Array<any> = [];
  employeeList: Array<any> = [];

  isLoading: boolean = false;
  isArab: boolean = false;
  interval: any;
  constructor(private translate: TranslateService,
    private notifyService: NotificationService,public dialog: MatDialog,private utilService: UtilityService,private apiService: ApiService,private authService: AuthorizeService, private fb: FormBuilder, public dialogRef: MatDialogRef<CalendarDaysComponent>) {
    super(authService);
  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.isLoading = true;
    //this.calculateDates();

 
    
    //this.getSitesList();
    
    //this.loadEmployees();
    
    this.setForm();
    var count = 0;
    this.interval = setInterval(() => {
      count++;
      if (count == 1) {
        this.calculateDates();


      }
      else if (count == 2) {
        this.getSitesList();
      }
     
      else if (count == 3) {
        this.loadEmployees();

      }
      else if (count == 4) {
        this.loadTableData();

      }
      else if (count == 5) {
        this.loadFooterData();

      }
      else if (count == 6) {
        this.getSkillsetsForSite(this.project.siteCode);

      }


    }, count > 6 ? 0 : 1000);
   


   
  }

  setForm() {
    this.form = this.fb.group({
      'projectCode': [this.project.projectCode],
      'customerCode': [this.project.customerCode],
      'siteCode': [this.project?.siteCode != null ? this.project.siteCode : '', Validators.required],
      'startDate': [this.project.startDate.toString().substring(0,10)],
      'endDate': [this.project.endDate.toString().substring(0, 10)],
      'noOfDays': [this.noOfDays],
      'dto': [this.monthlyRoasterForSite.slice()],
    
    });

    if (this.project.siteCode != null) {
      this.form.controls['siteCode'].disable({ onlySelf: true });
      this.getSkillsetsForSite(this.project.siteCode);
    }
   
  }
  closeModel() {
    this.dialogRef.close();
  }
  submit() { }

  calculateDates() {
    this.monthsDataList = [];
    this.isMonthlyRoasterGenerated = [];
    let startDate = new Date(this.project.startDate);
    let endDate = new Date(this.project.endDate);
    this.noOfDays = (endDate.getTime() - startDate.getTime()) / (1000 * 60 * 60 * 24)+1;
    for (let y = startDate.getFullYear(); y <= endDate.getFullYear(); y++) {
      let sm = y == startDate.getFullYear() ? startDate.getMonth() : 1;
      let em = y == endDate.getFullYear() ? endDate.getMonth() : 12;
     
      for (let m = sm; m <= em; m++) {
        let sd = (m == sm && y == startDate.getFullYear()) ? startDate.getDate() : 1;
        let ed = (m == em && y == endDate.getFullYear()) ? endDate.getDate() : new Date(y, m + 1, 0).getDate();
        let noOfDays = (new Date(y, m, ed).getTime() - new Date(y, m, sd).getTime()) / (1000 * 60 * 60 * 24) + 1;
       
        let monthData: any = {
          sd: sd,
          ed:ed,
          mStartDate: new Date(y, m, sd).toDateString(),
          mEndDate: new Date(y, m, ed).toDateString(),
          mNoOfDays: noOfDays,
          month: m+1,
          year:y
        };
        this.monthsDataList.push(monthData);
        this.isMonthlyRoasterGenerated.push(true);
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
        console.log(res);
        this.getShiftsForSite(siteCode);
       
      });
    else {
      this.shiftsForSite = [];
      this.skillsetsListForSite = [];
      this.loadTableData();

    }

  }
  getShiftsForSite(siteCode:string) {
    this.apiService.getall(`ShiftMaster/getShiftsByProjectAndSiteCode/${this.project.projectCode}/${siteCode}`).subscribe(res => {
      this.shiftsForSite = res;
      this.loadEmpToResourceMap(siteCode);
     
    });
  }


  onSelectSiteCode(event: any) {
    this.getSkillsetsForSite(event.target.value);
    
  }







  loadTableData() {
    this.tableData = [];
    this.footerData = [];
    
    for (let m = 0; m < this.monthsDataList.length; m++) {
      
      let month = this.monthsDataList[m];
      for (let ss = 0; ss < this.skillsetsListForSite.length; ss++) {

        let tablerow: any = {
          sd: this.monthsDataList[m].sd,
          ed: this.monthsDataList[m].ed,
          startDate: this.monthsDataList[m].mStartDate,
          endDate: this.monthsDataList[m].mEndDate,
          noOfDays: this.monthsDataList[m].mNoOfDays,
          noOfShifts: this.shiftsForSite?.length,
          skillSet: this.skillsetsListForSite[ss].skillSetCode,
          quantity: this.skillsetsListForSite[ss].quantity,
          totShifts: /*noOfShiftsForSite **/ month.mNoOfDays * this.skillsetsListForSite[ss].quantity,

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
        skillSet: this.skillsetsListForSite[ss].skillSetCode,
        quantity: this.skillsetsListForSite[ss].quantity,
      };
      this.footerData.push(footerrow);
    }


  }





  checkRoasterForSite(){

    this.apiService.getall(`MonthlyRoaster/isExistMonthlyRoasterForProjectSite/${this.project.projectCode}/${this.form.controls['siteCode'].value}`).subscribe(res => {
      this.isRoasterGenerated = res;
      
      //for (let m = 0; m < this.monthsDataList.length; m++) {
      //  this.checkMonthlyRoasterForSite(m);
      //}

    });



    
  }
  checkMonthlyRoasterForSite(i: number) {
    let month = this.monthsDataList[i].month;
    let year = this.monthsDataList[i].year;

    this.apiService.getall(`MonthlyRoaster/isExistMonthlyRoasterForProjectSiteMonth/${this.project.projectCode}/${this.form.controls['siteCode'].value}/${month}/${year}`).subscribe(res => {
      this.isMonthlyRoasterGenerated[i] = res;



    });
  }








  generateRoasterForSite() {

    if (!this.isLoading) {
      this.isLoading = true;

      this.monthlyRoasterForSite = [];

      for (let r = 0; r < this.tableData.length; r++) {

        let shiftCodesForMonth: Array<any> = [];

        let sd = new Date(new Date(this.tableData[r].startDate).toISOString().slice(0, 10));
        let ed = new Date(new Date(this.tableData[r].endDate).toISOString().slice(0, 10));


        let sday = new Date(sd.getFullYear(), sd.getMonth(), sd.getDate() + 2).toISOString().slice(8, 10);
        let eday = new Date(sd.getFullYear(), ed.getMonth(), ed.getDate() + 2).toISOString().slice(8, 10);


        for (var j = 1; j <= 31; j++) {



          if (j < parseInt(sday) || j > parseInt(eday))
            shiftCodesForMonth.push('x');
          else
            shiftCodesForMonth.push('');
        }



        for (let i = 1; i <= this.tableData[r].quantity; i++) {

          let msd:Date = new Date(this.tableData[r].startDate);
          msd.setMinutes(msd.getMinutes() - msd.getTimezoneOffset());
          let esd: Date = new Date(this.tableData[r].endDate);
          esd.setMinutes(esd.getMinutes() - esd.getTimezoneOffset());

          let row: any = {
            id: 0,
            customerCode: this.project.customerCode,
            siteCode: this.form.controls['siteCode'].value,
            projectCode: this.project.projectCode,
            month: new Date(this.tableData[r].startDate).getMonth() + 1,
            year: new Date(this.tableData[r].endDate).getFullYear(),

            skillsetName: this.skillsetsListForSite.find((x: any) => x.skillSetCode == this.tableData[r].skillSet).nameInEnglish,
         
            monthStartDate: msd,
            monthEndDate:esd,
            shiftCodesForMonth: shiftCodesForMonth.slice(),
            employeeID: 0,
            employeeNumber: '',
            mapId: 0,
            skillsetCode: '',
          };
          this.monthlyRoasterForSite.push(row);
        }

      }


      let i = 0;
      for (let m: number = 0; m < this.monthsDataList.length; m++) {

        for (let k: number = 0; k < this.empToResMapData.length; k++) {
          this.monthlyRoasterForSite[i].employeeID = this.empToResMapData[k].employeeID;
          this.monthlyRoasterForSite[i].employeeNumber = this.empToResMapData[k].empNumber;
          this.monthlyRoasterForSite[i].mapId = this.empToResMapData[k].mapId;
          this.monthlyRoasterForSite[i].skillsetCode = this.empToResMapData[k].skillSet;
          i++;
        }




      }

      this.apiService.post('MonthlyRoaster/generateMonthlyRoastersForSite', this.monthlyRoasterForSite.slice())
        .subscribe(res => {
          this.utilService.OkMessage();

          this.checkRoasterForSite();
          this.dialogRef.close(true);
          this.isLoading = false;
        },
          error => {
            console.log(error);
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
            this.isLoading = false;

          });

    }
    else {
      this.notifyService.showInfo(this.translate.instant("Please_Wait"));
    }
  }

  getSkillSet(code:string) {
    return this.skillsetsListForSite.find((x: any) => x.skillSetCode == code);

  }


  generateMonthlyRoasterForSite(month:number) {
    if (!this.isLoading) {
      this.isLoading = true;

      this.monthlyRoasterForSite = [];

      for (let r = month * this.skillsetsListForSite.length; r < (month + 1) * this.skillsetsListForSite.length; r++) {

        let shiftCodesForMonth: Array<any> = [];

        let sd = new Date(new Date(this.tableData[r].startDate).toISOString().slice(0, 10));
        let ed = new Date(new Date(this.tableData[r].endDate).toISOString().slice(0, 10));


        let sday = new Date(sd.getFullYear(), sd.getMonth(), sd.getDate() + 2).toISOString().slice(8, 10);
        let eday = new Date(sd.getFullYear(), ed.getMonth(), ed.getDate() + 2).toISOString().slice(8, 10);


        for (var j = 1; j <= 31; j++) {



          if (j < parseInt(sday) || j > parseInt(eday))
            shiftCodesForMonth.push('x');
          else
            shiftCodesForMonth.push('');
        }



        for (let i = 1; i <= this.tableData[r].quantity; i++) {

          let row: any = {
            id: 0,
            customerCode: this.project.customerCode,
            siteCode: this.form.controls['siteCode'].value,
            projectCode: this.project.projectCode,
            month: new Date(this.tableData[r].startDate).getMonth() + 1,
            year: new Date(this.tableData[r].endDate).getFullYear(),

            skillsetName: this.skillsetsListForSite.find((x: any) => x.skillSetCode == this.tableData[r].skillSet).nameInEnglish,
           
            monthStartDate: new Date(sd.getFullYear(), sd.getMonth(), sd.getDate() + 2),
            monthEndDate: new Date(sd.getFullYear(), ed.getMonth(), ed.getDate() + 2),
            shiftCodesForMonth: shiftCodesForMonth.slice(),
            employeeID: 0,
            employeeNumber: '',
            mapId: 0,
            skillsetCode: '',
          };
          this.monthlyRoasterForSite.push(row);
        }

      }


      let i = 0;
 

      for (let k: number = 0; k < this.empToResMapData.length; k++) {

          this.monthlyRoasterForSite[i].employeeID = this.empToResMapData[k].employeeID;
          this.monthlyRoasterForSite[i].employeeNumber = this.empToResMapData[k].empNumber;
          this.monthlyRoasterForSite[i].mapId = this.empToResMapData[k].mapId;
          this.monthlyRoasterForSite[i].skillsetCode = this.empToResMapData[k].skillSet;

          i++;
        
        }




      




      this.isLoading = true;;
      this.apiService.post('MonthlyRoaster/generateMonthlyRoastersForSite', this.monthlyRoasterForSite.slice())
        .subscribe(res => {
          this.utilService.OkMessage();

          this.checkRoasterForSite();
          this.dialogRef.close(true);
          this.isLoading = false;
        },
          error => {
            console.log(error);
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
            this.isLoading = false;

          });

    }
    else {
      this.notifyService.showInfo(this.translate.instant("Please_Wait"));
    }
  }






  getJvalue(i: number): number {
  
    return i % this.skillsetsListForSite.length;
  }


  loadEmpToResourceMap(siteCode: string) {
    this.isLoading = true;
    this.apiService.getall(`EmployeesToResorceMap/getEmployeesToResourcesMapForProjectSite/${this.project.projectCode}/${siteCode}`).subscribe(res => {
      this.empToResMapData = res as Array<any>;
      this.isLoading = false;
      // console.log(res);
      for (var i = 0; i < this.empToResMapData.length; i++) {
        let map: any = this.empToResMapData[i];
        //map.empNumber = this.employeeList.find((y: any) => y.textTwo == map.employeeID).value;
        //map.empName = this.employeeList.find((y: any) => y.textTwo == map.employeeID).text;
        map.empNumber = this.employeeList.find((y: any) => y.value == map.employeeNumber).value;
        map.empName = this.employeeList.find((y: any) => y.value == map.employeeNumber).text;
        map.empNameAr = this.employeeList.find((y: any) => y.value == map.employeeNumber).textAr;

        map.skillSetNameEng = this.skillsetsListForSite.find((x: any) => x.skillSetCode == map.skillSet).nameInEnglish;
        map.skillSetNameArb = this.skillsetsListForSite.find((x: any) => x.skillSetCode == map.skillSet).nameInArabic;

      }

      if (this.empToResMapData.length == 0) {

        this.notifyService.showError(this.translate.instant("Employees_Not_Mapped"));
      }
      else {  this.loadTableData();
    }
    });

   
  }

  loadEmployees() {
    this.isLoading = true;
    this.apiService.getall('Employee/getSelectEmployeeList').subscribe(result => {
      this.isLoading = false;
      this.employeeList = result as Array<any>;

    }, error => {
      this.utilService.ShowApiErrorMessage(error);
      this.isLoading = false;
    });
  }




}
