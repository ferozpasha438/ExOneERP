import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { UtilityService } from '../../services/utility.service';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { ParentSchoolMgtComponent } from '../../sharedcomponent/parentschoolmgt.component';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
//import { ApiService } from '../../services/api.service';
import { DBOperation } from '../../services/utility.constants';
import { ValidationService } from '../../sharedcomponent/ValidationService';
import { TranslateService } from '@ngx-translate/core';
import { MatCalendar, MatCalendarCellCssClasses } from '@angular/material/datepicker';
import { Moment } from 'moment';

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html'
})
export class CalendarComponent extends ParentSchoolMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort!: MatSort;
  @ViewChild('calendar') calendar!: MatCalendar<Moment>;
  selectedDate!: Moment;
  displayedColumns: string[] = ['Actions'];
  data!: MatTableDataSource<any>;
  totalItemsCount!: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  id: number = 0;
  form!: FormGroup;
  isArab: boolean = false;
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
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private translate: TranslateService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService) {
    super(authService);
  }
  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.form = this.fb.group({
      "branchCode": ['', Validators.required]
    });
    this.loading();
  }
  loading() {
    this.apiService.getall('schoolBranches/getSchoolBranchList').subscribe(res => {
      this.branchCodeList = res;
    });
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

  loadResultList() {
    const branchCode: string = this.form.value['branchCode'] as string;
    if (branchCode != null && branchCode != '') {
      this.apiService.getall(`Branch/GetBranchEventsHolidaysData/${branchCode}`)
        .subscribe(res => {
          if (res) {
            this.minDate = this.utilService.selectedDate(res.startDate);
            this.maxDate = this.utilService.selectedDate(res.endDate);
            for (var i = 0; i < res.eventsHolidaysDataList.length; i++) {
              if (res.eventsHolidaysDataList[i].eventType===1) {
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
          }
        },
          error => {
            this.utilService.ShowApiErrorMessage(error);
          });
    }
  }
  submit() {
  }
  reset() {
    this.form.reset();
  }

}


