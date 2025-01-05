import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatCalendar, MatCalendarCellCssClasses } from '@angular/material/datepicker';
import { MatTableDataSource } from '@angular/material/table';
import { Moment } from 'moment';
import { default as data } from "../../assets/i18n/siteConfig.json";
import { AuthorizeService } from '../api-authorization/AuthorizeService';
import { ApiService } from '../services/api.service';
import { UtilityService } from '../services/utility.service';
import { ApexPlotOptions, ChartComponent } from "ng-apexcharts";

import {
  ApexNonAxisChartSeries,
  ApexResponsive,
  ApexChart
} from "ng-apexcharts";

//Defult
import {
  ApexAxisChartSeries,
  ApexDataLabels,
  ApexXAxis,
  ApexLegend,
  ApexFill, ApexYAxis,
  ApexStates,
  ApexGrid,
  ApexTitleSubtitle, ApexStroke,
} from "ng-apexcharts";

import { NotificationService } from '../services/notification.service';
import { PaginationService } from '../sharedcomponent/pagination.service';
import { TranslateService } from '@ngx-translate/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';

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

export type ChartOptionsDefault = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  dataLabels: ApexDataLabels;
  plotOptions: ApexPlotOptions;
  responsive: ApexResponsive[];
  xaxis: ApexXAxis;
  legend: ApexLegend;
  fill: ApexFill;
};

export type ChartOptionsDefault1 = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  dataLabels: ApexDataLabels;
  plotOptions: ApexPlotOptions;
  yaxis: ApexYAxis;
  xaxis: ApexXAxis;
};
type ApexXAxisDefault = {
  type?: "category" | "datetime" | "numeric";
  categories?: any;
  labels?: {
    style?: {
      colors?: string | string[];
      fontSize?: string;
    };
  };
};
var colorsDefault = [
  "#008FFB",
  "#00E396",
  "#FEB019",
  "#FF4560",
  "#775DD0",
  "#00D9E9",
  "#FF66C3"
];
export type ChartOptionsDefault2 = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  dataLabels: ApexDataLabels;
  plotOptions: ApexPlotOptions;
  yaxis: ApexYAxis;
  xaxis: ApexXAxis;
  grid: ApexGrid;
  subtitle: ApexTitleSubtitle;
  colors: string[];
  states: ApexStates;
  title: ApexTitleSubtitle;
  legend: ApexLegend;
  tooltip: any; //ApexTooltip;
};

declare global {
  interface Window {
    Apex: any;
  }
}
window.Apex = {
  chart: {
    toolbar: {
      show: false
    }
  },
  tooltip: {
    shared: false
  },
  legend: {
    show: false
  }
};

const arrayData = [
  {
    y: 400,
    quarters: [
      {
        x: "Q1",
        y: 120
      },
      {
        x: "Q2",
        y: 90
      },
      {
        x: "Q3",
        y: 100
      },
      {
        x: "Q4",
        y: 90
      }
    ]
  },
  {
    y: 430,
    quarters: [
      {
        x: "Q1",
        y: 120
      },
      {
        x: "Q2",
        y: 110
      },
      {
        x: "Q3",
        y: 90
      },
      {
        x: "Q4",
        y: 110
      }
    ]
  },
  {
    y: 448,
    quarters: [
      {
        x: "Q1",
        y: 70
      },
      {
        x: "Q2",
        y: 100
      },
      {
        x: "Q3",
        y: 140
      },
      {
        x: "Q4",
        y: 138
      }
    ]
  },
  {
    y: 470,
    quarters: [
      {
        x: "Q1",
        y: 150
      },
      {
        x: "Q2",
        y: 60
      },
      {
        x: "Q3",
        y: 190
      },
      {
        x: "Q4",
        y: 70
      }
    ]
  },
  {
    y: 540,
    quarters: [
      {
        x: "Q1",
        y: 120
      },
      {
        x: "Q2",
        y: 120
      },
      {
        x: "Q3",
        y: 130
      },
      {
        x: "Q4",
        y: 170
      }
    ]
  },
  {
    y: 580,
    quarters: [
      {
        x: "Q1",
        y: 170
      },
      {
        x: "Q2",
        y: 130
      },
      {
        x: "Q3",
        y: 120
      },
      {
        x: "Q4",
        y: 160
      }
    ]
  }
];

export type ChartOptionsDefault3 = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  responsive: ApexResponsive[];
  labels: any;
  stroke: ApexStroke;
  fill: ApexFill;
};

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  @ViewChild("chart") chart!: ChartComponent;
  public chartOptions1!: Partial<ChartOptions>;
  public chartOptions2!: Partial<ChartOptions>;
  public chartOptions3!: Partial<ChartOptions>;
  public chartOptions4!: Partial<ChartOptions>;
  public chartOptions5!: Partial<ChartOptions>;
  public chartOptionsDefault!: Partial<ChartOptionsDefault>;
  public chartOptionsDefault1!: Partial<ChartOptionsDefault1>;
  public chartOptionsDefault2!: Partial<ChartOptionsDefault2>;
  public chartQuarterOptionsDefault2!: Partial<ChartOptionsDefault2>;
  public chartOptionsDefault3!: Partial<ChartOptionsDefault3>;

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


  constructor(private authService: AuthorizeService, private http: HttpClient,
    private apiService: ApiService, private utilService: UtilityService,
    //#start region Opr
    private notifyService: NotificationService, public pageService: PaginationService, private translate: TranslateService,

    //#end region Opr

  ) {
  }

  //Default
  public makeData(): any {
    var dataSet = this.shuffleArray(arrayData);

    var dataYearSeries = [
      {
        x: "2019",
        y: dataSet[0].y,
        color: colorsDefault[0],
        quarters: dataSet[0].quarters
      },
      {
        x: "2020",
        y: dataSet[1].y,
        color: colorsDefault[1],
        quarters: dataSet[1].quarters
      },
      {
        x: "2021",
        y: dataSet[2].y,
        color: colorsDefault[2],
        quarters: dataSet[2].quarters
      },
      {
        x: "2022",
        y: dataSet[3].y,
        color: colorsDefault[3],
        quarters: dataSet[3].quarters
      },
      {
        x: "2023",
        y: dataSet[4].y,
        color: colorsDefault[4],
        quarters: dataSet[4].quarters
      },
      {
        x: "2024",
        y: dataSet[5].y,
        color: colorsDefault[5],
        quarters: dataSet[5].quarters
      }
    ];

    return dataYearSeries;
  }

  public shuffleArray(array: any) {
    for (var i = array.length - 1; i > 0; i--) {
      var j = Math.floor(Math.random() * (i + 1));
      var temp = array[i];
      array[i] = array[j];
      array[j] = temp;
    }
    return array;
  }

  public updateQuarterChart(sourceChart: any, destChartIDToUpdate: any) {
    var series = [];
    var seriesIndex = 0;
    var colors = [];

    if (sourceChart.w.globals.selectedDataPoints[0]) {
      var selectedPoints = sourceChart.w.globals.selectedDataPoints;
      for (var i = 0; i < selectedPoints[seriesIndex].length; i++) {
        var selectedIndex = selectedPoints[seriesIndex][i];
        var yearSeries = sourceChart.w.config.series[seriesIndex];
        series.push({
          name: yearSeries.data[selectedIndex].x,
          data: yearSeries.data[selectedIndex].quarters
        });
        colors.push(yearSeries.data[selectedIndex].color);
      }

      if (series.length === 0)
        series = [
          {
            data: []
          }
        ];

      return window.ApexCharts.exec(destChartIDToUpdate, "updateOptions", {
        series: series,
        colors: colors,
        fill: {
          colors: colors
        }
      });
    }
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
    else if (this.dashBoardType == "default") {

      this.chartOptionsDefault = {
        series: [
          {
            name: "PRODUCT A",
            data: [44, 55, 41, 67, 22, 43]
          },
          {
            name: "PRODUCT B",
            data: [13, 23, 20, 8, 13, 27]
          },
          {
            name: "PRODUCT C",
            data: [11, 17, 15, 15, 21, 14]
          },
          {
            name: "PRODUCT D",
            data: [21, 7, 25, 13, 22, 8]
          }
        ],
        chart: {
          type: "bar",
          height: 350,
          stacked: true,
          toolbar: {
            show: true
          },
          zoom: {
            enabled: true
          }
        },
        responsive: [
          {
            breakpoint: 480,
            options: {
              legend: {
                position: "bottom",
                offsetX: -10,
                offsetY: 0
              }
            }
          }
        ],
        plotOptions: {
          bar: {
            horizontal: false
          }
        },
        xaxis: {
          type: "category",
          categories: [
            "01/2011",
            "02/2011",
            "03/2011",
            "04/2011",
            "05/2011",
            "06/2011"
          ]
        },
        legend: {
          position: "right",
          offsetY: 40
        },
        fill: {
          opacity: 1
        }
      };


      this.chartOptionsDefault1 = {
        series: [
          {
            name: "Cash Flow",
            data: [
              1.45,
              5.42,
              5.9,
              -0.42,
              -12.6,
              -18.1,
              -18.2,
              -14.16,
              -11.1,
              -6.09,
              0.34,
              3.88,
              13.07,
              5.8,
              2,
              7.37,
              8.1,
              13.57,
              15.75,
              17.1,
              19.8,
              -27.03,
              -54.4,
              -47.2,
              -43.3,
              -18.6,
              -48.6,
              -41.1,
              -39.6,
              -37.6,
              -29.4,
              -21.4,
              -2.4
            ]
          }
        ],
        chart: {
          type: "bar",
          height: 350
        },
        plotOptions: {
          bar: {
            colors: {
              ranges: [
                {
                  from: -100,
                  to: -46,
                  color: "#F15B46"
                },
                {
                  from: -45,
                  to: 0,
                  color: "#FEB019"
                }
              ]
            },
            columnWidth: "80%"
          }
        },
        dataLabels: {
          enabled: false
        },
        yaxis: {
          title: {
            text: "Growth"
          },
          labels: {
            formatter: function (y) {
              return y.toFixed(0) + "%";
            }
          }
        },
        xaxis: {
          type: "datetime",
          categories: [
            "2011-01-01",
            "2011-02-01",
            "2011-03-01",
            "2011-04-01",
            "2011-05-01",
            "2011-06-01",
            "2011-07-01",
            "2011-08-01",
            "2011-09-01",
            "2011-10-01",
            "2011-11-01",
            "2011-12-01",
            "2012-01-01",
            "2012-02-01",
            "2012-03-01",
            "2012-04-01",
            "2012-05-01",
            "2012-06-01",
            "2012-07-01",
            "2012-08-01",
            "2012-09-01",
            "2012-10-01",
            "2012-11-01",
            "2012-12-01",
            "2013-01-01",
            "2013-02-01",
            "2013-03-01",
            "2013-04-01",
            "2013-05-01",
            "2013-06-01",
            "2013-07-01",
            "2013-08-01",
            "2013-09-01"
          ],
          labels: {
            rotate: -90
          }
        }
      };

      this.chartOptionsDefault2 = {
        series: [
          {
            name: "year",
            data: this.makeData()
          }
        ],
        chart: {
          id: "barYear",
          height: 400,
          width: "100%",
          type: "bar",
          events: {
            dataPointSelection: (e, chart, opts) => {
              var quarterChartEl = document.querySelector("#chart-quarter") as HTMLDivElement;
              var yearChartEl = document.querySelector("#chart-year") as HTMLDivElement;

              if (opts.selectedDataPoints[0].length === 1) {
                if (quarterChartEl.classList.contains("active")) {
                  this.updateQuarterChart(chart, "barQuarter");
                } else {
                  yearChartEl.classList.add("chart-quarter-activated");
                  quarterChartEl.classList.add("active");
                  this.updateQuarterChart(chart, "barQuarter");
                }
              } else {
                this.updateQuarterChart(chart, "barQuarter");
              }

              if (opts.selectedDataPoints[0].length === 0) {
                yearChartEl.classList.remove("chart-quarter-activated");
                quarterChartEl.classList.remove("active");
              }
            },
            updated: (chart) => {
              this.updateQuarterChart(chart, "barQuarter");
            }
          }
        },
        plotOptions: {
          bar: {
            distributed: true,
            horizontal: true,
            barHeight: "75%",
            dataLabels: {
              position: "bottom"
            }
          }
        },
        dataLabels: {
          enabled: true,
          textAnchor: "start",
          style: {
            colors: ["#fff"]
          },
          formatter: function (val, opt) {
            return opt.w.globals.labels[opt.dataPointIndex];
          },
          offsetX: 0,
          dropShadow: {
            enabled: true
          }
        },

        colors: colorsDefault,

        states: {
          normal: {
            filter: {
              type: "desaturate"
            }
          },
          active: {
            allowMultipleDataPointsSelection: true,
            filter: {
              type: "darken",
              value: 1
            }
          }
        },
        tooltip: {
          x: {
            show: false
          },
          y: {
            title: {
              formatter: function (val: any, opts: any) {
                return opts.w.globals.labels[opts.dataPointIndex];
              }
            }
          }
        },
        title: {
          text: "Yearly Results",
          offsetX: 15
        },
        subtitle: {
          text: "(Click on bar to see details)",
          offsetX: 15
        },
        yaxis: {
          labels: {
            show: false
          }
        }
      };

      this.chartQuarterOptionsDefault2 = {
        series: [
          {
            name: "quarter",
            data: []
          }
        ],
        chart: {
          id: "barQuarter",
          height: 400,
          width: "100%",
          type: "bar",
          stacked: true
        },
        plotOptions: {
          bar: {
            columnWidth: "50%",
            horizontal: false
          }
        },
        legend: {
          show: false
        },
        grid: {
          yaxis: {
            lines: {
              show: false
            }
          },
          xaxis: {
            lines: {
              show: true
            }
          }
        },
        yaxis: {
          labels: {
            show: false
          }
        },
        title: {
          text: "Quarterly Results",
          offsetX: 10
        },
        tooltip: {
          x: {
            formatter: function (val: any, opts: any) {
              return opts.w.globals.seriesNames[opts.seriesIndex];
            }
          },
          y: {
            title: {
              formatter: function (val: any, opts: any) {
                return opts.w.globals.labels[opts.dataPointIndex];
              }
            }
          }
        }
      };

      this.chartOptionsDefault3 = {
        series: [14, 23, 21, 17, 15, 10, 12, 17, 21],
        chart: {
          type: "polarArea"
        },
        stroke: {
          colors: ["#fff"]
        },
        fill: {
          opacity: 0.8
        },
        responsive: [
          {
            breakpoint: 480,
            options: {
              chart: {
                width: 200
              },
              legend: {
                position: "bottom"
              }
            }
          }
        ]
      };

      let res = {
        "branchCode": "Jeddah",
        "totalStudents": 28,
        "studentsOnLeave": 0,
        "feeDueStudents": 31,
        "totalTeachers": 3,
        "newRegistrations": 0,
        "feeDueTotal": 790300.00,
        "chartAttandanceData": [
          {
            "typeID": 1,
            "totalStudents": 28,
            "presentStudents": 0,
            "leaveStudents": 0,
            "absentStudents": 28
          },
          {
            "typeID": 2,
            "totalStudents": 532,
            "presentStudents": 0,
            "leaveStudents": 0,
            "absentStudents": 532
          },
          {
            "typeID": 3,
            "totalStudents": 532,
            "presentStudents": 43,
            "leaveStudents": 0,
            "absentStudents": 489
          }
        ],
        "dashboardEvents": [],
        "dashboardStudents": [
          {
            "studentName": "Lateen Waleed S Alqahtani",
            "studentName2": "لتين وليد سعيد القحطاني",
            "stuAdmNum": "22STU2",
            "gradeCode": "KG2",
            "grade": "KG2",
            "grade2": "KG2_Ar"
          }
        ]
      };

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
            height: 250,
            width: 280
          },
          labels: ["Total Customers", "Absent Customers", "Present Customers",],
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
            height: 250,
            width: 280
          },
          labels: ["Total Customers", "Absent Customers", "Present Customers"],
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
            height: 250,
            width: 280
          },
          labels: ["Total Customers", "Absent Customers", "Present Customers"],
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
          let res1 = {
            "branchCode": "Jeddah",
            "startDate": "2022-03-14T00:00:00",
            "endDate": "2023-12-31T00:00:00",
            "eventsHolidaysDataList": [
              {
                "eventDate": "2022-03-18T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-03-19T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-03-25T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-03-26T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-04-01T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-04-02T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-04-08T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-04-09T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-04-15T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-04-16T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-04-22T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-04-23T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-04-29T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-04-30T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-05-06T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-05-07T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-05-13T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-05-14T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-05-20T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-05-21T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-05-27T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-05-28T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-06-03T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-06-04T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-06-10T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-06-11T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-06-17T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-06-18T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-06-24T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-06-25T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-07-01T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-07-02T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-07-08T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-07-09T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-07-15T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-07-16T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-07-22T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-07-23T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-07-29T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-07-30T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-08-05T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-08-06T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-08-12T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-08-13T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-08-19T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-08-20T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-08-26T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-08-27T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-09-02T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-09-02T00:00:00",
                "eventName": "Eid",
                "eventNameAr": "Eid_ar",
                "eventType": 2
              },
              {
                "eventDate": "2022-09-03T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-09-09T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-09-09T00:00:00",
                "eventName": "Ganesh",
                "eventNameAr": "Ganesh_ar",
                "eventType": 2
              },
              {
                "eventDate": "2022-09-10T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-09-16T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-09-17T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-09-23T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-09-24T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-09-30T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-10-01T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-10-07T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-10-08T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-10-14T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-10-15T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-10-21T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-10-22T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-10-28T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-10-29T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-11-04T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-11-05T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-11-11T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-11-12T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-11-18T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-11-19T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-11-25T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-11-26T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-11-28T00:00:00",
                "eventName": "Singing competition",
                "eventNameAr": "مسابقة Singing",
                "eventType": 3
              },
              {
                "eventDate": "2022-12-02T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-12-03T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-12-05T00:00:00",
                "eventName": "Gulf Current Affairs",
                "eventNameAr": "شؤون الخليج الجارية",
                "eventType": 3
              },
              {
                "eventDate": "2022-12-08T00:00:00",
                "eventName": "G.K Competition",
                "eventNameAr": "مسابقة G.K",
                "eventType": 3
              },
              {
                "eventDate": "2022-12-09T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-12-10T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-12-16T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-12-17T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-12-23T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-12-24T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-12-25T00:00:00",
                "eventName": "Student - Teacher Relationship",
                "eventNameAr": "العلاقة بين الطالب والمعلم",
                "eventType": 3
              },
              {
                "eventDate": "2022-12-30T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2022-12-30T00:00:00",
                "eventName": "School Annual Day Function",
                "eventNameAr": "وظيفة اليوم السنوي للمدرسة",
                "eventType": 3
              },
              {
                "eventDate": "2022-12-31T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-01-06T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-01-07T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-01-13T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-01-14T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-01-20T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-01-21T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-01-27T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-01-28T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-02-01T00:00:00",
                "eventName": "G.K Competition",
                "eventNameAr": "مسابقة G.K",
                "eventType": 3
              },
              {
                "eventDate": "2023-02-02T00:00:00",
                "eventName": "Parent Teachers Meeting",
                "eventNameAr": "اجتماع أولياء الأمور",
                "eventType": 3
              },
              {
                "eventDate": "2023-02-03T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-02-04T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-02-10T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-02-11T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-02-17T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-02-18T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-02-24T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-02-25T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-03-03T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-03-04T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-03-10T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-03-11T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-03-17T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-03-18T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-03-24T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-03-25T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-03-31T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-04-01T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-04-07T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-04-08T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-04-14T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-04-15T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-04-21T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-04-22T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-04-28T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-04-29T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-05-05T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-05-06T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-05-12T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-05-13T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-05-19T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-05-20T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-05-26T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-05-27T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-06-02T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-06-02T00:00:00",
                "eventName": "Parent Teachers Meeting",
                "eventNameAr": "اجتماع أولياء الأمور",
                "eventType": 3
              },
              {
                "eventDate": "2023-06-03T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-06-09T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-06-10T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-06-10T00:00:00",
                "eventName": "G.K Competition",
                "eventNameAr": "شؤون الخليج الجارية",
                "eventType": 3
              },
              {
                "eventDate": "2023-06-15T00:00:00",
                "eventName": "Singing competition",
                "eventNameAr": "مسابقة Singing",
                "eventType": 3
              },
              {
                "eventDate": "2023-06-16T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-06-17T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-06-23T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-06-24T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-06-28T00:00:00",
                "eventName": "Arafat Day",
                "eventNameAr": "Arafat Day_AR",
                "eventType": 2
              },
              {
                "eventDate": "2023-06-29T00:00:00",
                "eventName": "Eid al-Adha",
                "eventNameAr": "Eid al-Adha_AR",
                "eventType": 2
              },
              {
                "eventDate": "2023-06-30T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-07-01T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-07-07T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-07-08T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-07-14T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-07-15T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-07-21T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-07-22T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-07-28T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-07-29T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-08-04T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-08-05T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-08-11T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-08-12T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-08-15T00:00:00",
                "eventName": "Independence Day",
                "eventNameAr": "Independence Day",
                "eventType": 3
              },
              {
                "eventDate": "2023-08-18T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-08-19T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-08-25T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-08-26T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-09-01T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-09-02T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-09-08T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-09-09T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-09-15T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-09-16T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-09-22T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-09-23T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-09-29T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-09-30T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-10-06T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-10-07T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-10-13T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-10-14T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-10-20T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-10-21T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-10-27T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-10-28T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-11-03T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-11-04T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-11-10T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-11-11T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-11-17T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-11-18T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-11-24T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-11-25T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-12-01T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-12-02T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-12-08T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-12-09T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-12-15T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-12-16T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-12-22T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-12-23T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-12-29T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              },
              {
                "eventDate": "2023-12-30T00:00:00",
                "eventName": "Weekend",
                "eventNameAr": null,
                "eventType": 1
              }
            ]
          };

          if (res1) {
            this.minDate = this.utilService.selectedDate(res1.startDate);
            this.maxDate = this.utilService.selectedDate(res1.endDate);
            for (var i = 0; i < res1.eventsHolidaysDataList.length; i++) {
              if (res1.eventsHolidaysDataList[i].eventType === 1) {
                this.datesWeekends.push(res1.eventsHolidaysDataList[i].eventDate);
              } else if (res1.eventsHolidaysDataList[i].eventType === 2) {
                this.datesHolidays.push(res1.eventsHolidaysDataList[i].eventDate);
                res1.eventsHolidaysDataList[i].eventDate = this.utilService.selectedDate(res1.eventsHolidaysDataList[i].eventDate);
                this.allDatesData.push(res1.eventsHolidaysDataList[i]);
              } else if (res1.eventsHolidaysDataList[i].eventType === 3) {
                this.datesEvents.push(res1.eventsHolidaysDataList[i].eventDate);
                res1.eventsHolidaysDataList[i].eventDate = this.utilService.selectedDate(res1.eventsHolidaysDataList[i].eventDate);
                this.allDatesData.push(res1.eventsHolidaysDataList[i]);
              }
            }
            this.isShowDiv = true;
            this.isShowCalandar = true;
          }

        }
      }
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

}
