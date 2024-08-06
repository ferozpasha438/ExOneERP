import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';
@Component({
  selector: 'app-count-of-skillsets',
  templateUrl: './count-of-skillsets.component.html'
})
export class CountOfSkillsetsComponent extends ParentOptMgtComponent implements OnInit {
  form: FormGroup;
  List: Array<any> = [];
  isLoading: boolean = false;
  company: any;
  projectSitesList: Array<any> = [];
  customersList: Array<any> = [];

  dateFrom: string = '';
  dateTo: string = '';



  options = [];
  InputQuery: any;

  customerSelectionList: Array<any> = [];
  siteSelectionList: Array<any> = [];
  customerCode: string = '';

  citySelectionList: Array<any> = [];
  cityCode: string = '';
  siteCode: string = '';
  curDate: Date = new Date();
  isArabic: boolean = false;




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
    this.loadSiteSelectionList();


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


  loadProjectSites() {
    this.apiService.getall('ProjectSites/getSelectProjectSitesList').subscribe((res: any) => {
      this.projectSitesList = res as Array<any>;

      this.projectSitesList.forEach(e => {
        e.lable = this.isArabic ? e.value + "-" + e.textTwo : e.value + "-" + e.text;
      });



    });
  }



  onSelectCustomer() {
    this.clearProjectsData();
    this.siteCode = "";

    this.loadSiteSelectionList();

   

  }



  loadSiteSelectionList() {
    if (this.customerCode != "" && this.customerCode !=null) {
      this.apiService.getall('ProjectSites/getSelectProjectSitesListByCustomerCode/' + this.customerCode).subscribe((res: any) => {
        this.siteSelectionList = res as Array<any>;

        this.siteSelectionList.forEach(e => {
          e.lable = this.isArabic ? e.value + "-" + e.textTwo : e.value + "-" + e.text;
        });
      });

    }
    else {

      this.apiService.getall('ProjectSites/getSelectProjectSitesList').subscribe((res: any) => {
        this.siteSelectionList = res as Array<any>;

        this.siteSelectionList.forEach(e => {
          e.lable = this.isArabic ? e.value + "-" + e.textTwo : e.value + "-" + e.text;
        });
      });

    }

  }

  setForm() {

    this.customerCode = this.cityCode;
    this.projectSitesList = [];
    this.customersList = [];
    this.InputQuery = { customerCode: "", siteCode: "", cityCode: "" };

  }
  search() {

    this.projectSitesList = [];
    this.customersList = [];




    this.InputQuery = {

      customerCode: this.customerCode == null ? "" : this.customerCode,
      cityCode: this.cityCode == null ? "" : this.cityCode,
      siteCode: this.siteCode == null ? "" : this.siteCode,


    };


    this.isLoading = true;


    this.apiService.post('Reports/getSkillsetsOnProjectsReports', this.InputQuery).subscribe((res: any) => {

      if (res != null) {

        this.projectSitesList = res.projectSites as Array<any>;
        this.customersList = res.customers as Array<any>;
        this.company = res.company;
        if (this.projectSitesList?.length == 0)
        {
          this.isLoading = false;
        }

        else {
         

          for (let i = 0; i < this.projectSitesList.length; i += 50) {
            let projectsSites: Array<any> = [];
            for (let j = i; j < i + 50 && j < this.projectSitesList.length; j++) {
              projectsSites.push(this.projectSitesList[j]);



            }

            this.apiService.post('Reports/getSkillsetsOnProjectsReportsByProjectSites', projectsSites).subscribe((res2) => {
              let reports = res2 as Array<any>;

              for (let k = 0; k < reports.length; k++) {
                this.projectSitesList[i + k] = reports[k];
                if (i + k >= this.projectSitesList.length - 1)
                  this.isLoading = false;
              }

            });





          }
        }
       



      }
      else {
        this.isLoading = false;
      }

    });


  }


  openPrint() {
    console.log(this.projectSitesList);
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




  clearProjectsData() {
    this.customersList = [];
    this.projectSitesList = [];
    this.customersList = [];
    this.company = null;
    this.loadSiteSelectionList();
  }
}
