import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';

@Component({
  selector: 'app-projects-sites-reports',
  templateUrl: './projects-sites-reports.component.html'
})
export class ProjectsSitesReportsComponent extends ParentOptMgtComponent implements OnInit {
  form: FormGroup;
  List: Array<any> = [];
  isLoading: boolean = false;
  company: any;
  projectSitesList: Array<any> = [];
  customersList: Array<any> = [];
 
  dateFrom: string = '';
  dateTo: string = '';



  options = [];
  fromDate: '';
  toDate: '';
  InputQuery: any;

  customerSelectionList: Array<any> = [{ text: "54500025", value:"54500025"}];
  customerCode: string = '';
  statusSelectionList: Array<any> = [{ text: "InProgress", value: "InProgress" }, { text: "Closed", value: "Closed" }, { text: "Suspended", value: "Suspended" }, { text: "InActive", value: "InActive" },{ text: "Completed", value: "Completed" }];
  statusCode: string = '';
  serviceSelectionList: Array<any> = [];
  serviceCode: string = '';
  citySelectionList: Array<any> = [];
  cityCode: string = '';
  curDate: Date = new Date();
  isArabic: boolean= false;




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
    this.loadServiceCodesList();


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
  loadServiceCodesList() {
  this.apiService.getall('Services/getSelectServiceList').subscribe((res: any) => {
      this.serviceSelectionList = res as Array<any>;

    this.serviceSelectionList.forEach(e => {
      e.lable = this.isArabic ? e.value + "-" + e.textTwo : e.value + "-" + e.text;
      });
    });

  }




  setForm() {
    
    this.customerCode = this.serviceCode = this.cityCode = this.statusCode = '';
    this.projectSitesList = [];
    this.customersList = [];
    this.InputQuery = { fromDate: this.fromDate, todate: this.toDate, customerCode: "", statusCode: "", cityCode: "", serviceCode: "" };

  }
  search() {

    this.projectSitesList = [];
    this.customersList = [];
    let fd: Date = new Date(this.fromDate);
    fd.setMinutes(fd.getMinutes() - fd.getTimezoneOffset());
    let td: Date = new Date(this.toDate);
    td.setMinutes(fd.getMinutes() - td.getTimezoneOffset());



    this.InputQuery = {
      fromDate: fd, todate: td,
      customerCode: this.customerCode == null ? "" : this.customerCode,
      statusCode: this.statusCode == null ? "" : this.statusCode,
      cityCode: this.cityCode == null ? "" : this.cityCode,
      serviceCode: this.serviceCode == null ? "" : this.serviceCode
     
    };
  
    if (this.fromDate && this.toDate) {
      this.isLoading = true;
     

      this.apiService.post('Reports/getProjectSitesReports', this.InputQuery).subscribe((res: any) => {
        
        if (res != null) {

          this.projectSitesList = res.projectSites as Array<any>;
          this.customersList = res.customers as Array<any>;
          this.company = res.company;

          if (this.projectSitesList.length > 0) {
            console.log(res);

            for (let i = 0; i < this.projectSitesList.length; i += 50) {
              let projectsSites: Array<any> = [];
              for (let j = i; j < i + 50 && j < this.projectSitesList.length; j++) {
                projectsSites.push(this.projectSitesList[j]);

               

              }

              this.apiService.post('Reports/getReportByProjectSites', projectsSites).subscribe((res2) => {
                let reports = res2 as Array<any>;
                for (let k = 0; k < reports.length; k++) {
                  this.projectSitesList[i + k] = reports[k];

                  if (i+k== this.projectSitesList.length - 1)
                    this.isLoading = false;
                }
                this.isLoading = false;
              });
            }
          }
          else {
            this.isLoading = false;
          }
        }
        else {
          this.isLoading = false;
        }
      });
    }
    else
      this.notifyService.showError("Select Dates");
  }


  openPrint() {

    if (this.projectSitesList.length > 0) {
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

 
  getTotal(customerCode:string):any {
    let Total: any = {estimationCost:0}
    this.projectSitesList.forEach(e => {
      if (e.customerCode == customerCode) {
        Total.estimationCost+=e.estimationCost
      }
    });
    return Total;
  }

  clearProjectsData() {
    this.customersList = [];
    this.projectSitesList = [];
    this.customersList = [];
    this.company = null;
  
  }
    }
