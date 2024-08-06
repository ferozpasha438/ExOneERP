import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { UtilityService } from '../../../services/utility.service';
import { PaginationService } from '../../../sharedcomponent/pagination.service';
import { ParentSchoolMgtComponent } from '../../../sharedcomponent/parentschoolmgt.component';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
//import { ApiService } from '../../services/api.service';
import { DBOperation } from '../../../services/utility.constants';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { TranslateService } from '@ngx-translate/core';
import { MatCalendar, MatCalendarCellCssClasses } from '@angular/material/datepicker';
import { Moment } from 'moment';

@Component({
  selector: 'app-branch-calendar',
  templateUrl: './branch-calendar.component.html',
})
export class BranchCalendarComponent extends ParentSchoolMgtComponent implements OnInit {
  selectedDate!: Moment;
  isArab: boolean = false;
  branchCode: string = '';
  minDate!: any;
  maxDate!: any;
  datesWeekends: Array<any> = [];
  datesHolidays: Array<any> = [];
  datesEvents: Array<any> = [];
  allDatesData: Array<any> = [];
  isShowDiv: boolean = false;
  row: any;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<BranchCalendarComponent>,
    private notifyService: NotificationService) {
    super(authService);
  }
  ngOnInit(): void {
    this.isShowDiv = false;
    this.isArab = this.utilService.isArabic();
    if (this.row) {
      this.branchCode = this.row['branchCode'];
    }
    this.loadResultList();
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
    if (this.branchCode != null && this.branchCode != '') {
      this.apiService.getall(`Branch/GetBranchEventsHolidaysData/${this.branchCode}`)
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
          }
        },
          error => {
            this.utilService.ShowApiErrorMessage(error);
          });
    }
  }
  submit() {
  }
  closeModel() {
    this.dialogRef.close();
  }
}


