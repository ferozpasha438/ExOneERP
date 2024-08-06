import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';

@Component({
  selector: 'app-report-customervisits',
  templateUrl: './report-customervisits.component.html'
})
export class ReportCustomervisitsComponent extends ParentOptMgtComponent implements OnInit {
  form: FormGroup;
  List: Array<any> = [];
  isLoading: boolean = false;
  company: any;

  dateFrom: string = '';
  dateTo: string = '';



  options = [];
  fromDate: '';
  toDate: '';
  InputQuery: any;

  customerSelectionList: Array<any> = [];
  customerCode: string = '';
  statusSelectionList: Array<any> = [{ text: "Inprogress", value: "Inprogress" }, { text: "Closed", value: "Closed" }, { text: "Open", value: "Open" }];
  statusCode: string = '';
  reasonCode: string = '';
  projectCode: string = '';
  reasonCodesSelectionList: Array<any> = [];
  usersSelectionList: Array<any> = [];
  citySelectionList: Array<any> = [];
  cityCode: string = '';
  curDate: Date = new Date();
  isArabic: boolean = false;

  supervisorId = '';
  visitedBy = '';
  result: any;

  constructor(private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private fb: FormBuilder,
    private notifyService: NotificationService) {
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
    this.loadReasonCodesList();
    this.loadUsersList();

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
  loadReasonCodesList() {
    this.apiService.getall('ReasonCode/getSelectReasonCodeListForCustomerComplaint').subscribe((res: any) => {
      this.reasonCodesSelectionList = res as Array<any>;

      this.reasonCodesSelectionList.forEach(e => {
        e.lable = this.isArabic ? e.value + "-" + e.textTwo : e.value + "-" + e.text;
      });
    });

  }

  loadUsersList() {
    this.apiService.getall('Users/GetUserSelectionList').subscribe((res: any) => {
      this.usersSelectionList = res as Array<any>;

      this.usersSelectionList.forEach(e => {
        e.lable = this.isArabic ? e.value + "-" + e.textTwo : e.value + "-" + e.text;
      });
    });

  }



  setForm() {

    this.InputQuery = { fromDate: this.fromDate, todate: this.toDate, customerCode: "", reasonCode: "", branchCode: "", projectCode: "", supervisorId: "",visitedBy:"" };

  }
  search() {
    let fd: Date = new Date(this.fromDate);
    fd.setMinutes(fd.getMinutes() - fd.getTimezoneOffset());
    let td: Date = new Date(this.toDate);
    td.setMinutes(fd.getMinutes() - td.getTimezoneOffset());
    this.InputQuery = {
      fromDate: fd,
      todate: td,
      customerCode: this.customerCode == null ? "" : this.customerCode,
      statusCode: this.statusCode == null ? "" : this.statusCode,
      reasonCode: this.reasonCode == null ? "" : this.reasonCode,
      projectCode: this.projectCode == null ? "" : this.projectCode,
      branchCode: this.cityCode == null ? "" : this.cityCode,
      visitedBy: this.visitedBy == null ? "" : this.visitedBy,
      supervisorId: this.supervisorId == null ? "" : this.supervisorId,

    };

    if (this.fromDate && this.toDate) {
      this.isLoading = true;
      this.apiService.post('Reports/getCustomerVisitsReport', this.InputQuery).subscribe((res: any) => {
        this.result = res;
        this.company = res.company;
        this.isLoading = false;

      });
    }
    else
      this.notifyService.showError("Select Dates");
  }


  openPrint() {

    if (this.result.visits.length > 0) {
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


  clearResultData() {
    this.company = null;

  }



}
