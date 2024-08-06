import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatCalendar, MatCalendarCellCssClasses } from '@angular/material/datepicker';
import { MatTableDataSource } from '@angular/material/table';
import { Moment } from 'moment';
import { default as data } from "../../../assets/i18n/siteConfig.json";
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { UtilityService } from '../../services/utility.service';
import { ApexAxisChartSeries, ApexDataLabels, ApexFill, ApexLegend, ApexPlotOptions, ApexStroke, ApexTooltip, ApexXAxis, ApexYAxis, ChartComponent } from "ng-apexcharts";

import { NotificationService } from '../../services/notification.service';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { TranslateService } from '@ngx-translate/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { OprServicesService } from '../opr-services.service';

import {
  ApexNonAxisChartSeries,
  ApexResponsive,
  ApexChart
} from "ng-apexcharts";
export type ChartOptions = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  responsive: ApexResponsive[];
  labels: any;
};
export type RadialChartOptions = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  labels: string[];
  plotOptions: ApexPlotOptions;
};

export type SemiRadialChartOptions = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  labels: string[];
  plotOptions: ApexPlotOptions;
  fill: ApexFill;
};

export type MonthlyChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  dataLabels: ApexDataLabels;
  plotOptions: ApexPlotOptions;
  yaxis: ApexYAxis;
  xaxis: ApexXAxis;
  fill: ApexFill;
  tooltip: ApexTooltip;
  stroke: ApexStroke;
  legend: ApexLegend;
};


@Component({
  selector: 'app-test-example',
  //styleUrls: ['./test-example.component.css'],
  templateUrl: './test-example.component.html'
 // providers: [DatePipe]
})

export class TestExampleComponent  implements OnInit {
 
  @ViewChild("chart") chart!: ChartComponent;
  public chartOptions1!: Partial<ChartOptions>;
  public chartOptions2!: Partial<ChartOptions>;
  public chartOptions3!: Partial<ChartOptions>;
  public chartOptions4!: Partial<ChartOptions>;
  public chartOptions5!: Partial<ChartOptions>;
  isArab: boolean = false;
  dashBoardType: string = '';
  @ViewChild('calendar') calendar!: MatCalendar<Moment>;

  selectedDate!: Moment;
  displayedColumns: string[] = [];
  //data!: MatTableDataSource<any>;
  totalItemsCount!: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  id: number = 0;
  form!: FormGroup;
  branchCodeList: Array<any> = [];
  isShowDiv: boolean = false;
  examHeaderID: number = 0;
  branchCode: string = '';
  minDate!: any;
  maxDate!: any;
  datesWeekends: Array<any> = [];
  datesHolidays: Array<any> = [];
  datesEvents: Array<any> = [];
  allDatesData: Array<any> = [];

  dashboardEvents: Array<any> = [];
  totalStudents: number = 0;
  studentsOnLeave: number = 0;
  feeDueStudents: number = 0;
  totalTeachers: number = 0;
  newRegistrations: number = 0;
  feeDueTotal: number = 0;
  todayAttData: number[] = [];
  monthAttData: number[] = [];
  yearAttData: number[] = [];
  dashboardStudents: Array<any> = [];
  isShowDonut: boolean = false;
  isShowCalandar: boolean = false;

  //#startregion Opr

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  isLoadSchoolDashBoard: boolean = false;
  attendanceData: MatTableDataSource<any>;
  interval: any;
  oprDashboard: any = { notReportedEmpCount: 1, totalEmpCount: 1, reportedEmpCount: 1, lateArrivalsCount: 0 };
  oprChartData1: Array<any> = [];
  oprChartData2: Array<any> = [];
  oprChartData3: Array<any> = [];
  oprChartData4: Array<any> = [];
  oprChartData5: Array<any> = [];
  pageNumber = 0;
  pageSize = 10;
  //filterOptions: Array<any> = [];// [{ key: "late", isSelected: false }];
  selectedfilterOptions: Array<any> = [];// [{ key: "late", isSelected: false }];
  input: any = {
    date: null,
    pageNumber: 0,
    pageSize: 10,
    branchCode: null,
    siteCode: null,
    projectCode: null,
    employeeNumber: null,
    filterOptions: null,
    dashBoardSubType: "operations",// operations,management
  };
  citySelectionList: Array<any> = [];
  projectsSelectionList: Array<any> = [];
  sitesSelectionList: Array<any> = [];
  employeesSelectionList: Array<any> = [];

  public radialChartOptions1!: Partial<RadialChartOptions>;
  public radialChartOptions2!: Partial<RadialChartOptions>;
  public radialChartOptions3!: Partial<RadialChartOptions>;
  public radialChartOptions4!: Partial<RadialChartOptions>;
  public radialChartOptions5!: Partial<RadialChartOptions>;
  //#endregion Opr

  //#startregion profm
  profmDashboard: any;
  public semiRadialChartOptions1!: Partial<SemiRadialChartOptions>;
  public semiRadialChartOptions2!: Partial<SemiRadialChartOptions>;
  public monthlyChartOptions!: Partial<MonthlyChartOptions>;

  //endregion profm


  constructor(private authService: AuthorizeService, private http: HttpClient,
    private apiService: ApiService, private utilService: UtilityService
    //#start region Opr
    , private oprService: OprServicesService, private notifyService: NotificationService, public pageService: PaginationService, private translate: TranslateService,

    //#end region Opr

    //#startregion profm
    
    //end region profm

  ) {
   this.semiRadialChartOptions1 = {
      series: [76],
      chart: {
        type: "radialBar",
        offsetY: -20,
        height: 300
      },
      plotOptions: {
        radialBar: {
          startAngle: -90,
          endAngle: 90,
          track: {
            background: "#e7e7e7",
            strokeWidth: "97%",
            margin: 5, // margin is in pixels
            dropShadow: {
              enabled: true,
              top: 2,
              left: 0,
              opacity: 0.31,
              blur: 2
            }
          },
          dataLabels: {
            name: {
              show: false
            },
            value: {
              offsetY: -2,
              fontSize: "22px"
            }
          }
        }
      },
      fill: {
        type: "gradient",
        gradient: {
          shade: "light",
          shadeIntensity: 0.4,
          inverseColors: false,
          opacityFrom: 1,
          opacityTo: 1,
          stops: [0, 50, 53, 91]
        }
      },
      labels: ["Average Results"]
    };
     this.semiRadialChartOptions2 = {
      series: [76],
      chart: {
        type: "radialBar",
        offsetY: -20,
        height:300
      },
      plotOptions: {
        radialBar: {
          startAngle: -90,
          endAngle: 90,
          track: {
            background: "#e7e7e7",
            strokeWidth: "97%",
            margin: 5, // margin is in pixels
            dropShadow: {
              enabled: true,
              top: 2,
              left: 0,
              opacity: 0.31,
              blur: 2
            }
          },
          dataLabels: {
            name: {
              show: false
            },
            value: {
              offsetY: -2,
              fontSize: "22px"
            }
          }
        }
      },
      fill: {
        type: "gradient",
        gradient: {
          shade: "light",
          shadeIntensity: 0.4,
          inverseColors: false,
          opacityFrom: 1,
          opacityTo: 1,
          stops: [0, 50, 53, 91]
        }
      },
      labels: ["Average Results"]
    };


    this.monthlyChartOptions = {
      series: [
        {
          name: "Net Profit",
          data: [44, 55, 57, 56, 61, 58]
        }
      ],
      chart: {
        type: "bar",
        height: 350
      },
      plotOptions: {
        bar: {
          horizontal: false,
          columnWidth: "55%",
         // endingShape: "rounded"
        }
      },
      dataLabels: {
        enabled: false
      },
      stroke: {
        show: true,
        width: 2,
        colors: ["transparent"]
      },
      xaxis: {
        categories: [
          "Feb",
          "Mar",
          "Apr",
          "May",
          "Jun",
          "Jul"
        
        ]
      },
      yaxis: {
        title: {
          text: "tickets"
        }
      },
      fill: {
        opacity: 1
      },
      tooltip: {
        y: {
          formatter: function (val) {
            return  val + " tickets";
          }
        }
      }
    };
  }
  ngOnInit(): void {


    










    let count = 0;
    this.isArab = this.utilService.isArabic();
    if (this.dashBoardType == '')
      this.dashBoardType = data.dashBoardType;

    if (this.dashBoardType == "school") {
      this.apiService.getSchoolUrl(`Branch/GetBranchDashBoard`)
        .subscribe(res => {
          if (res) {
            this.dashboardEvents = res.dashboardEvents;
            this.dashboardStudents = res.dashboardStudents;
            this.totalStudents = res.totalStudents;
            this.studentsOnLeave = res.studentsOnLeave;
            this.feeDueStudents = res.feeDueStudents;
            this.totalTeachers = res.totalTeachers;
            this.newRegistrations = res.newRegistrations;
            this.feeDueTotal = res.feeDueTotal;
            var attData = res.chartAttandanceData as Array<any>;
            var tAttData = attData.find(x => x.typeID == 1);
            this.todayAttData.push(tAttData.totalStudents == 0 ? 1 : tAttData.totalStudents);
            this.todayAttData.push(tAttData.presentStudents == 0 ? 1 : tAttData.presentStudents);
            this.todayAttData.push(tAttData.absentStudents == 0 ? 1 : tAttData.absentStudents);

            var mAttData = attData.find(x => x.typeID == 2);
            this.monthAttData.push(mAttData.totalStudents == 0 ? 1 : mAttData.totalStudents);
            this.monthAttData.push(mAttData.presentStudents == 0 ? 1 : mAttData.presentStudents);
            this.monthAttData.push(mAttData.absentStudents == 0 ? 1 : mAttData.absentStudents);

            var yAttData = attData.find(x => x.typeID == 3);
            this.yearAttData.push(yAttData.totalStudents == 0 ? 1 : yAttData.totalStudents);
            this.yearAttData.push(yAttData.presentStudents == 0 ? 1 : yAttData.presentStudents);
            this.yearAttData.push(yAttData.absentStudents == 0 ? 1 : yAttData.absentStudents);

            this.chartOptions1 = {
              series: this.todayAttData,
              chart: {
                type: "donut",
                height: 150,
                width: 280
              },
              labels: ["Total Students", "Present Students", "Absent Students"],
              responsive: [
                {
                  breakpoint: 480,
                  options: {
                    chart: {
                      width: 500,
                    },
                    legend: {
                      position: "bottom"
                    }
                  }
                }
              ]
            };
            this.chartOptions2 = {
              series: this.monthAttData,
              chart: {
                type: "donut",
                height: 150,
                width: 280
              },
              labels: ["Total Students", "Present Students", "Absent Students"],
              responsive: [
                {
                  breakpoint: 480,
                  options: {
                    chart: {
                      width: 500
                    },
                    legend: {
                      position: "bottom"
                    }
                  }
                }
              ]
            };
            this.chartOptions3 = {
              series: this.yearAttData,
              chart: {
                type: "donut",
                height: 150,
                width: 280
              },
              labels: ["Total Students", "Present Students", "Absent Students"],
              responsive: [
                {
                  breakpoint: 480,
                  options: {
                    chart: {
                      width: 500
                    },
                    legend: {
                      position: "bottom"
                    }
                  }
                }
              ]
            };
            this.isShowDonut = true;
            if (res.branchCode != null && res.branchCode != '') {
              this.apiService.getSchoolUrl(`Branch/GetBranchEventsHolidaysData/${res.branchCode}`)
                .subscribe(res => {
                  if (res) {
                    this.minDate = this.utilService.selectedDate(res.startDate);
                    this.maxDate = this.utilService.selectedDate(res.endDate);
                    for (var i = 0; i < res.eventsHolidaysDataList.length; i++) {
                      if (res.eventsHolidaysDataList[i].eventType === 1) {
                        this.datesWeekends.push(res.eventsHolidaysDataList[i].eventDate);
                      } else if (res.eventsHolidaysDataList[i].eventType === 2) {
                        this.datesHolidays.push(res.eventsHolidaysDataList[i].eventDate);
                        res.eventsHolidaysDataList[i].eventDate = this.utilService.selectedDate(res.eventsHolidaysDataList[i].eventDate);
                        this.allDatesData.push(res.eventsHolidaysDataList[i]);
                      } else if (res.eventsHolidaysDataList[i].eventType === 3) {
                        this.datesEvents.push(res.eventsHolidaysDataList[i].eventDate);
                        res.eventsHolidaysDataList[i].eventDate = this.utilService.selectedDate(res.eventsHolidaysDataList[i].eventDate);
                        this.allDatesData.push(res.eventsHolidaysDataList[i]);
                      }
                    }
                    this.isShowDiv = true;
                    this.isShowCalandar = true;
                  }
                },
                  error => {
                    this.utilService.ShowApiErrorMessage(error);
                  });
            }
          }
        },
          error => {
            this.utilService.ShowApiErrorMessage(error);
          });
    }
    else if (this.dashBoardType == "opr") {
      this.loadCitiesList();
      this.isArab = this.utilService.isArabic();
      if (this.input.dashBoardSubType == "operations") {
        this.displayedColumns = [/*'projectCode', */'projectName',/* 'siteCode',*/ 'siteName', 'employeeNumber', 'employeeName', 'shiftCode', 'inTime', 'outTime', 'arrive', 'late', 'isGeofenseOut', 'geofenseOutCount', 'overtime', 'isOnBreak', 'Actions'];
        this.interval = setInterval(() => {
          count++;
          if (count == 1) {
            this.loadFilterOptions();
          }
          else if (count == 2) {
            this.loadInitialData();
          }
        }, (count > 2) ? 0 : 1000);

      }
      else if (this.input.dashBoardSubType == "management") {
        this.displayedColumns = ['projectName', 'siteName', 'shiftCode', 'totalContracted', 'staffPresent', 'lateStaff', 'shortage', 'supportGaurds', 'projectStatus', 'Actions'];

        this.loadInitialData();


      }



    }
    else if (this.dashBoardType == "profm") {
      this.loadProfmDashBoardData();
    }
  }
  dateClass() {

    return (date: Date): MatCalendarCellCssClasses => {
      var highlightDate = this.datesWeekends
        .map(strDate => new Date(strDate))
        .some(d => d.getDate() === date.getDate()
          && d.getMonth() === date.getMonth()
          && d.getFullYear() === date.getFullYear());
      if (highlightDate) {
        return highlightDate ? 'special-date' : '';
      }
      else {
        highlightDate = this.datesHolidays
          .map(strDate => new Date(strDate))
          .some(d => d.getDate() === date.getDate()
            && d.getMonth() === date.getMonth()
            && d.getFullYear() === date.getFullYear());
        if (highlightDate) {
          return highlightDate ? 'special-holiday-date' : '';
        }
        else {
          highlightDate = this.datesEvents
            .map(strDate => new Date(strDate))
            .some(d => d.getDate() === date.getDate()
              && d.getMonth() === date.getMonth()
              && d.getFullYear() === date.getFullYear());
          return highlightDate ? 'special-event-date' : '';
        }
      }
    };
  }




  //#startregion Opr_Functions

  loadInitialData() {





    this.totalItemsCount = 0;
    this.loadList(0);

  }

  resetFilter() {
    this.totalItemsCount = 0;
    this.oprDashboard = null;
    this.input.branchCode = null;
    this.input.siteCode = null;
    this.input.projectCode = null;
    this.input.employeeNumber = null;
    this.input.pageNumber = 0;
    this.input.pageSize = 10;

    this.loadList(0);

  }

  private loadList(page: number) {
    this.isLoading = true;
    this.pageService.change({ pageIndex: page, pageSize: this.pageSize, previousPageIndex: page - 1, length: this.totalItemsCount });
    this.input.pageNumber = page;

    this.apiService.postOprUrl('OperationsDashboard/getOpeartionsDashboardByFilter', this.input).subscribe((db: any) => {
      if (db) {
        this.oprDashboard = db as any;
        this.oprDashboard.todayAttData = [] as Array<number>;
        this.oprDashboard.todayAttData.push(db.totalEmpCount);
        this.oprDashboard.todayAttData.push(db.reportedEmpCount);
        this.oprDashboard.todayAttData.push(db.lateArrivalsCount);
        this.oprDashboard.todayAttData.push(db.notReportedEmpCount);
        this.oprDashboard.todayAttData.push(db.shiftsNotAssignedCount);
        this.oprDashboard.todayAttData.push(db.leavesCount);
        this.totalItemsCount = db.totalItemsCount;
        this.projectsSelectionList = db.projectsSelectionList;
        this.sitesSelectionList = db.sitesSelectionList;

        if (this.input.dashBoardSubType == "operations") {





          this.attendanceData = new MatTableDataSource(db.employeeAttendance);


          this.employeesSelectionList = db.employeesSelectionList;
          setTimeout(() => {
            this.paginator.pageIndex = page as number;
            this.paginator.length = this.totalItemsCount;
          });

          this.chartOptions1 = {
            series: this.oprDashboard.todayAttData,
            chart: {
              type: "donut",
              height: 150,
              width: 280
            },
            labels: ["Total Employees", "Reported Employees", "Late Arrivals Count", "Not Reported Employees", "Shifts Not Assigned", "Employees On Leave"],
            responsive: [
              {
                breakpoint: 480,
                options: {
                  chart: {
                    width: 500,
                  },
                  legend: {
                    position: "bottom"
                  }
                }
              }
            ]
          };


          this.radialChartOptions1 = {
            series: [100], //this.oprChartData1,
            chart: {
              height: 120,
              type: "radialBar"
            },
            plotOptions: {
              radialBar: {

                hollow: {
                  size: "50%",
                },
                dataLabels: {

                  name: {
                    fontSize: '0px',
                  },
                  value: {
                    //  fontSize: '10px',
                    fontWeight: 'bold',
                    offsetY: -10,
                  },
                }
              }
            },
            labels: ['']
          };

          this.radialChartOptions2 = {
            series: [Math.round((db.reportedEmpCount / db.totalEmpCount) * 100)],
            chart: {
              height: 120,
              type: "radialBar"
            },
            plotOptions: {
              radialBar: {
                hollow: {
                  size: "50%"
                },
                dataLabels: {
                  name: {
                    fontSize: '0px',
                  },
                  value: {
                    // fontSize: '10px',
                    offsetY: -10,
                    fontWeight: 'bold',
                  },
                }
              }
            },
            labels: ['']
          };



          this.radialChartOptions3 = {
            series: [Math.round((db.lateArrivalsCount / db.totalEmpCount) * 100)],
            chart: {
              height: 120,
              type: "radialBar"
            },
            plotOptions: {
              radialBar: {
                hollow: {
                  size: "50%"
                },
                dataLabels: {
                  name: {
                    fontSize: '0px',
                  },
                  value: {
                    // fontSize: '10px',
                    offsetY: -10,
                    fontWeight: 'bold',
                  },
                }
              }
            },
            labels: ['']
          };


          this.radialChartOptions4 = {
            series: [Math.round((db.notReportedEmpCount / db.totalEmpCount) * 100)],

            chart: {
              height: 120,
              type: "radialBar",
            },
            plotOptions: {

              radialBar: {
                hollow: {
                  size: "50%",
                },
                dataLabels: {
                  name: {
                    fontSize: '0px',
                  },
                  value: {
                    //color:'red',
                    // fontSize: '10px',
                    offsetY: -10,
                    fontWeight: 'bold',
                  },
                }
              },
            },
            labels: [''],


          };












        }

        else if (this.input.dashBoardSubType == "management") {

          // this.oprDashboard.todayAttData = [] as Array<number>;

          this.attendanceData = new MatTableDataSource(db.rows);

          this.radialChartOptions1 = {
            series: [100], //this.oprChartData1,
            chart: {
              height: 120,
              type: "radialBar"
            },
            plotOptions: {
              radialBar: {
                hollow: {
                  size: "50%"
                },
                dataLabels: {
                  name: {
                    fontSize: '0px',
                  },
                  value: {
                    //fontSize: '10px',
                    offsetY: -10,
                    fontWeight: 'bold',
                  },
                }
              }
            },
            labels: [""]
          };

          this.radialChartOptions2 = {
            series: [Math.round((db.staffPresent / db.totalContracted) * 100)],
            chart: {
              height: 120,
              type: "radialBar"
            },
            plotOptions: {
              radialBar: {
                hollow: {
                  size: "50%"
                },
                dataLabels: {
                  name: {
                    fontSize: '0px',
                  },
                  value: {

                    offsetY: -10,
                    fontWeight: 'bold',
                  },
                }
              }
            },
            labels: [""]
          };



          this.radialChartOptions3 = {
            series: [Math.round((db.lateStaff / db.totalContracted) * 100)],
            chart: {
              height: 120,
              type: "radialBar"
            },
            plotOptions: {
              radialBar: {
                hollow: {
                  size: "50%"
                },
                dataLabels: {
                  name: {
                    fontSize: '0px',
                  },
                  value: {
                    //  fontSize: '10px',
                    offsetY: -10,
                    fontWeight: 'bold',
                  },
                }
              }
            },
            labels: ['']
          };


          this.radialChartOptions4 = {
            series: [Math.round((db.shortage / db.totalContracted) * 100)],
            chart: {
              height: 120,
              type: "radialBar"
            },
            plotOptions: {
              radialBar: {
                hollow: {
                  size: "50%"
                },
                dataLabels: {
                  name: {
                    fontSize: '0px',
                  },
                  value: {
                    // fontSize: '10px',
                    offsetY: -10,
                    fontWeight: 'bold',
                  },
                }
              }
            },
            labels: ['']
          };


          setTimeout(() => {
            this.paginator.pageIndex = page as number;
            this.paginator.length = this.totalItemsCount;
          });

        }
      }

    });

  }


  onSortOrder(sort: any) {

    this.pageService.change({ pageSize: this.pageSize, pageIndex: 0, previousPageIndex: -1, length: 0 });
    this.sortingOrder = sort.active + ' ' + sort.direction;
    this.input.sortingOrder = this.sortingOrder;
    this.loadList(0);

  }
  onPageSwitch(event: PageEvent) {
    this.pageService.change(event);
    this.pageSize = event.pageSize;
    this.input.pageSize = event.pageSize;
    this.loadList(event.pageIndex);
  }
  applyFilter() {
    //this.input.filterOptions = this.filterOptions;
    this.loadList(0);

  }
  translateToolTip(msg: string) {
    return `${this.translate.instant(msg)}`;

  }

  loadCitiesList() {
    //  this.apiService.getall('City/getCitiesSelectList').subscribe((res: any) => {
    this.apiService.getOprUrl('Branch/getBranchSelectListForUser').subscribe((res: any) => {
      this.citySelectionList = res as Array<any>;

      this.citySelectionList.forEach(e => {
        e.lable = e.value + "-" + e.text;
      });
    });

  }
  updateFilterOptions(index: number) {
    this.input.filterOptions[index].isSelected = !this.input.filterOptions[index].isSelected;
    //  this.filterOptions.sort((a,b) => b.isSelected-a.isSelected);
    this.pageService.change({ pageSize: this.pageSize, pageIndex: 0, previousPageIndex: -1, length: 0 });

    this.loadList(0);
  }
  loadFilterOptions() {
    let dashBoardSubType = this.input.dashBoardSubType;
    this.apiService.getOprUrl(`OperationsDashboard/getFilterOptions/${dashBoardSubType}`).subscribe((res: any) => {
      this.input.filterOptions = res as Array<any>;
    });

  }
  enterAutoAttendance() {
    this.isLoading = true;
    let notReportedEmployees = this.oprDashboard.employeeAttendance as Array<any>;
    notReportedEmployees.filter((e: any) => e.isReported == false);
    if (notReportedEmployees.length > 0) {
      this.apiService.postOprUrl('EmployeesAttendance/EnterAutoAttendanceForAllProjectSites', notReportedEmployees)
        .subscribe(res => {
          if (res) {

            this.utilService.OkMessage();
            this.loadInitialData();
          }
        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
          });

    }
    else {
      this.notifyService.showWarning("No_Updates_Found");
    }
  }

  convertStringToTime(time: string) {
    if (time == null || time == '') {
      return '';
    }
    else {
      let hrs: number = +(time.split(':', 2)[0]);
      let mins: number = +time.split(':', 2)[1];
      if (hrs == 0) {
        time = "12:" + String(mins).padStart(2, '0') + "AM";
      }
      else if (hrs >= 12) {

        time = String(hrs - 12).padStart(2, '0') + ":" + String(mins).padStart(2, '0') + "PM";
      }
      else {
        time = String(hrs).padStart(2, '0') + ":" + String(mins).padStart(2, '0') + "AM";
      }

      return time;
    }
  }



  onChangeFilterItem() {
    this.input.filterOptions.forEach((e: any) => {
      e.isSelected = false;
    });

    this.selectedfilterOptions.forEach(e => {

      let index = this.input.filterOptions.findIndex((f: any) => f.key == e.key);
      if (index >= 0) {
        this.input.filterOptions[index].isSelected = true;
      }

    });
    this.applyFilter();
  }

  changeDashboardSubtype()    //by dblclick()
  {
    this.resetInitialData();
    if (this.dashBoardType == "opr") {

      if (this.input?.dashBoardSubType == "operations") {
        this.input.dashBoardSubType = "management";
      }
      else if (this.input?.dashBoardSubType == "management") {
        this.input.dashBoardSubType = "operations";
      }
    }
    else {
      this.dashBoardType = "opr";
      this.input.dashBoardSubType = "operations";
    }
    this.ngOnInit();
  }
  chageDate()//by dblclick()
  {
    this.input.date = this.input.date == null ? new Date('2022-11-01') : null;
    this.ngOnInit();
  }


  resetInitialData() {
    this.oprDashboard = { notReportedEmpCount: 1, totalEmpCount: 1, reportedEmpCount: 1, lateArrivalsCount: 0 };
    this.oprChartData1 = [];
    this.oprChartData2 = [];
    this.oprChartData3 = [];
    this.oprChartData4 = [];
    this.oprChartData5 = [];
    this.pageNumber = 0;
    this.pageSize = 10;
    this.selectedfilterOptions = [];// [{ key: "late", isSelected: false }];


    this.input.pageNumber = 0
    this.input.pageSize = 10;
    this.input.branchCode = null;
    this.input.siteCode = null;
    this.input.projectCode = null;
    this.input.employeeNumber = null;
    this.input.filterOptions = null;


    this.citySelectionList = [];
    this.projectsSelectionList = [];
    this.sitesSelectionList = [];
    this.employeesSelectionList = [];
  }
  //#endregion Opr_Functions

  //#region proFm
  loadProfmDashBoardData() {

    this.http.get('http://localhost:13428/api/FomWebDashboard/getWebDashboardData').subscribe((res: any) => {
      this.profmDashboard = res;

      this.profmDashboard.totalData = [] as Array<number>;
      this.profmDashboard.totalData.push(res.totalTickets);
      this.profmDashboard.totalData.push(res.closedTickets);
      this.profmDashboard.totalData.push(res.pendingTickets);

      this.monthlyChartOptions = {
        series: [
          {
            name: "Tickets",
            data: res.monthlyTotalTickets
              //[44, 55, 57, 56, 61, 58]
          }
        ],
        chart: {
          type: "bar",
          height: 150
        },
        plotOptions: {
          bar: {
            horizontal: false,
            columnWidth: "55%",
            // endingShape: "rounded"
          }
        },
        dataLabels: {
          enabled: false
        },
        stroke: {
          show: true,
          width: 2,
          colors: ["transparent"]
        },
        xaxis: {
          categories: res.monthsNames
          //  [
          //  "Feb",
          //  "Mar",
          //  "Apr",
          //  "May",
          //  "Jun",
          //  "xxxx"

          //]
        },
        yaxis: {
          title: {
            text: "tickets"
          }
        },
        fill: {
          opacity: 1
        },
        tooltip: {
          y: {
            formatter: function (val) {
              return val + " tickets";
            }
          }
        }
      };
    

      this.radialChartOptions1 = {
        series: [100],
        chart: {
          height: 120,
          type: "radialBar"
        },
        plotOptions: {
          radialBar: {
            hollow: {
              size: "50%"
            },
            dataLabels: {
              name: {
                fontSize: '0px',
              },
              value: {
                //fontSize: '10px',
                offsetY: -10,
                fontWeight: 'bold',
              },
            }
          }
        },
        labels: [""]
      };
      this.radialChartOptions2 = {
        series: [Math.round((res.closedTickets / res.totalTickets) * 100)],
        chart: {
          height: 120,
          type: "radialBar"
        },
        plotOptions: {
          radialBar: {
            hollow: {
              size: "50%"
            },
            dataLabels: {
              name: {
                fontSize: '0px',
              },
              value: {
                //fontSize: '10px',
                offsetY: -10,
                fontWeight: 'bold',
              },
            }
          }
        },
        labels: [""]
      };

      this.radialChartOptions3 = {
        series: [Math.round((res.pendingTickets / res.totalTickets) * 100)],
        chart: {
          height: 120,
          type: "radialBar"
        },
        plotOptions: {
          radialBar: {
            hollow: {
              size: "50%"
            },
            dataLabels: {
              name: {
                fontSize: '0px',
              },
              value: {
                //fontSize: '10px',
                offsetY: -10,
                fontWeight: 'bold',
              },
            }
          }
        },
        labels: [""]
      };


      this.semiRadialChartOptions1.series=[Math.round((res.closedTickets / res.totalTickets) * 100)];
      this.semiRadialChartOptions2.series = [Math.round((res.last30DaysData?.closedTickets / res.last30DaysData?.totalTickets) * 100)];
  
  



    });
   
  }


  //#endregion proFm
}
