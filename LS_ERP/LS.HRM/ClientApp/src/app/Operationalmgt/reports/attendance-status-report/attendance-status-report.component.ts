import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';
import { DatePipe } from '@angular/common'
@Component({
  selector: 'app-attendance-status-report',
  templateUrl: './attendance-status-report.component.html'
})
export class AttendanceStatusReportComponent extends ParentOptMgtComponent implements OnInit {
  form: FormGroup;
  List: Array<any> = [];
  isLoading: boolean = false;
  company: any;
  projectSitesList: Array<any> = [];
  customersList: Array<any> = [];

  result: any;


  options = [];
  date: '';
  InputQuery: any;

  customerSelectionList: Array<any> = [{ text: "54500025", value: "54500025" }];
  customerCode: string = '';
  statusSelectionList: Array<any> = [{ text: "Attendance_Drafted", value: "Attendance_Drafted" }, { text: "Attendance_Not_Drafted", value: "Attendance_Not_Drafted" }, { text: "Attendance_Posted", value: "Attendance_Posted" }, { text: "Attendance_Not_Posted", value: "Attendance_Not_Posted" }];
  statusCode: string = '';
  serviceSelectionList: Array<any> = [];
  serviceCode: string = '';
  citySelectionList: Array<any> = [];
  cityCode: string = '';
  curDate: Date = new Date();
  isArabic: boolean = false;

  totalItemsCount: number = 0;
  pageSize: number = 10;
  pageNumber: number = 0;


  constructor(private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private fb: FormBuilder,
    private notifyService: NotificationService, private translate: TranslateService, public datepipe: DatePipe) {
    super(authService);



  }

  ngOnInit(): void {
    this.isArabic = this.utilService.isArabic();
    this.loadInitialData();
    this.setForm();


  }
  loadInitialData() {
    this.loadCustomersList();
    this.loadCitiesList();


  }
  loadCustomersList() {
    this.apiService.getall('CustomerMaster/getSelectCustomerList').subscribe((res: any) => {
      this.customerSelectionList = res as Array<any>;

      this.customerSelectionList.forEach(e => {
        e.lable = this.isArabic ? e.value + "-" + e.textTwo : e.value + "-" + e.text;
      });
    });

  }
  loadCitiesList() {
    this.apiService.getall('City/getCitiesSelectList').subscribe((res: any) => {
      this.citySelectionList = res as Array<any>;

      this.citySelectionList.forEach(e => {
        e.lable = e.value + "-" + e.text;
      });
    });

  }
  

  

  setForm() {

    this.customerCode = this.serviceCode = this.cityCode = this.statusCode = '';
    this.projectSitesList = [];
    this.customersList = [];
    this.date = '';
    this.InputQuery = { date: '', customerCode: "", statusCode: "", cityCode: "", serviceCode: "" };
    this.result.columns = null;
  }
  search() {

    this.projectSitesList = [];
    this.customersList = [];
    

    if (this.date) {
      let fd: Date = new Date(this.date);
      fd.setMinutes(fd.getMinutes() - fd.getTimezoneOffset());



      this.InputQuery = {
        payrollStartDate: fd,
        customerCode: this.customerCode,
        attendanceStatus: this.statusCode,
        cityCode: this.cityCode,
        branchCode: this.cityCode,
        pageNumber: this.pageNumber,
        pageSize: this.pageSize
      };
      this.isLoading = true;
      this.apiService.post('Reports/getAttendanceStatusPayrollReport', this.InputQuery).subscribe((res: any) => {

        if (res != null) {

          this.result = res;
          this.company = res.company;
          this.totalItemsCount = res.totalItemsCount;
         
          this.loadCompleteReport(this.InputQuery);

        }
        else {
          this.isLoading = false;
        }
      });
    }
    else
      this.notifyService.showError("Select Dates");
  }


  loadCompleteReport(inputQuery: any) {
    this.InputQuery.pageNumber++;
    if (inputQuery.pageNumber > Math.floor(this.totalItemsCount / this.pageSize)) {
      this.isLoading = false;
    }
    else {
      this.apiService.post('Reports/getAttendanceStatusPayrollReport', this.InputQuery).subscribe((res: any) => {

        this.result.rows = this.result.rows.concat(res['rows']) as Array<any>;
        this.loadCompleteReport(inputQuery);
      });

    }

  }






  openPrint() {

    if (this.result.rows.length > 0) {
      if (!this.isLoading) {
        const printContent = document.getElementById("printcontainer") as HTMLElement;
        const WindowPrt: any = window.open('', '', 'left=0,top=0,width=2000,height=1000,toolbar=0,scrollbars=0,status=0');
        setTimeout(() => {
          WindowPrt.document.write(printContent.innerHTML);
          WindowPrt.document.close();
          WindowPrt.focus();
          WindowPrt.print();
          WindowPrt.close();
        }, 50);
      }
      else {
        this.notifyService.showError("Loading Data,Please Wait...");
      }
    }
    else {
      this.notifyService.showError("No_Data_Found");
    }

  }

  openDatePicker(dp: any) {
    dp.open();
  }

  getProjectsOfCustomer(CustomerCode: string): Array<any> {
    return this.projectSitesList.filter(e => e.customerCode == CustomerCode);
  }


  getTotal(customerCode: string): any {
    let Total: any = { inProgress: 0, closed: 0, completed: 0, suspended: 0, inActive: 0, total: 0 };

    this.projectSitesList.forEach(e => {
      if (e.customerCode == customerCode) {
        if (e.status == "Closed") {
          Total.inActive++;
          Total.closed++;
        }
        else if (e.status == "InProgress") {
          Total.inProgress++;
        }
        else if (e.status == "Suspended") {
          Total.inActive++;
          Total.suspended++;
        }
        else if (e.status == "Completed") {
          Total.completed++;
          Total.inActive++;
        }

        else if (e.status == "InActive") {
          Total.inActive++;
        }


        Total.total++;
      }


    });


    return Total;
  }

  clearProjectsData() {
    this.customersList = [];
    this.projectSitesList = [];
    this.customersList = [];
    this.result = null;
    this.company = null;

  }
  getBgColor(cell:any) {
    if (cell.isAttnPosted) {
      return `bgGreen`;
    }
    else if (cell.isAttnDrafted) {
      return `bgOrange`;
    }
    else {
      return `bgRed`;
    }
  }
  getStatusFlag(cell: any) {

    if (cell.isAttnPosted) {
      return `P`;
    }
    else if (cell.isAttnDrafted) {
      return `D`;
    }
    else {
      return `N`
    }


  }

  getToolTip(row:number,column: number) {
    let date = this.datepipe.transform(this.result.columns[column].attnDate, 'dd-MM-yyyy');
    return `${date}`
  }
}

