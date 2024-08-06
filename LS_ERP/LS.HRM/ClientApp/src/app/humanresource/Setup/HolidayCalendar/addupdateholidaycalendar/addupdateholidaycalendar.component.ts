import { Component, OnInit, ViewChild } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
  FormArray,
} from '@angular/forms';
import { MatDatepicker } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { AuthorizeService } from 'src/app/api-authorization/AuthorizeService';
import { CheckBoxSelectListItem } from 'src/app/models/MenuItemListDto';
import { ApiService } from 'src/app/services/api.service';
import { NotificationService } from 'src/app/services/notification.service';
import { DBOperation } from 'src/app/services/utility.constants';
import { UtilityService } from 'src/app/services/utility.service';
import { ParentHrmAdminComponent } from 'src/app/sharedcomponent/ParentHrmAdmin.component';
import { ValidationService } from 'src/app/sharedcomponent/ValidationService';

@Component({
  selector: 'app-addupdateholidaycalendar',
  templateUrl: './addupdateholidaycalendar.component.html',
})
export class AddupdateholidaycalendarComponent
  extends ParentHrmAdminComponent
  implements OnInit
{
  @ViewChild(MatDatepicker) picker: any;
  modalTitle!: string;
  modalBtnTitle!: string;
  dbops!: DBOperation;
  form!: FormGroup;
  id: number = 0;
  date = new FormControl();
  holidays: Array<any> = [];
  holidayCalendarMappings: Array<any> = [];
  isReadOnly: boolean = false;
  isArab: boolean = false;

  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private authService: AuthorizeService,
    private utilService: UtilityService,
    public dialogRef: MatDialogRef<AddupdateholidaycalendarComponent>,
    private notifyService: NotificationService,
    private validationService: ValidationService
  ) {
    super(authService);
  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.setForm();
    if (this.id > 0) this.setEditForm();
  }

  setForm() {
    this.form = this.fb.group({
      holidayCalendarCode: ['', Validators.required],
      holidayCalendarNameEn: ['', Validators.required],
      holidayCalendarNameAr: [''],
      year: [new Date().getFullYear(), Validators.required],
      remarks: [''],
      isActive: [false],
      holidayCalendarMappings: [],
    });
    this.isReadOnly = false;
    this.loadHolidays(this.form.value['year'].value);
  }

  setEditForm() {
    this.apiService.get('holidayCalendar', this.id).subscribe((res) => {
      if (res) {
        let holidayCalendarMappings = res[
          'holidayCalendarMappings'
        ] as Array<any>;
        holidayCalendarMappings.forEach((item) => {
          this.holidayCalendarMappings.push(item);
        });
        this.isReadOnly = true;
        this.form.patchValue(res);
        this.loadHolidays(this.form.value['year'].value);
      }
    });
  }

  loadHolidays(calendarYear: number) {
    this.apiService
      .getQueryString(
        `Holiday/getHolidaySelectListItem?year=`,
        calendarYear > 0
          ? calendarYear.toString()
          : (this.form.value['year'] as number).toString()
      )
      .subscribe((res) => {
        let holidays = res as Array<any>;
        if (this.holidayCalendarMappings.length > 0) {
          holidays.forEach((item) => {
            this.holidayCalendarMappings.some((e) => {
              if (e.holidayCode === item.holidayCode) item.checked = true;
            });
          });
        }
        this.holidays = holidays;
      });
  }

  public toggle(event: MatSlideToggleChange, holidayCode: string) {
    if (event.checked) {
      this.holidayCalendarMappings.push({
        holidayCalendarCode: this.form.controls.holidayCalendarCode?.value,
        holidayCode: holidayCode,
        id: 0,
        checked: false,
      });
    } else {
      this.holidayCalendarMappings.some((e, index) => {
        if (e.holidayCode === holidayCode) {
          this.holidayCalendarMappings.splice(index, 1);
        }
      });
    }
  }

  closeModel() {
    this.dialogRef.close();
  }

  submit() {
    if (this.form.valid) {
      if (this.holidayCalendarMappings.length > 0) {
        if (this.id > 0) this.form.value['id'] = this.id;
        this.form.value['holidayCalendarMappings'] =
          this.holidayCalendarMappings;
        this.apiService.post('HolidayCalendar', this.form.value).subscribe(
          (res) => {
            this.utilService.OkMessage();
            this.reset();
            this.dialogRef.close(true);
          },
          (error) => {
            this.utilService.ShowApiErrorMessage(error);
          }
        );
      } else this.notifyService.showError('SelectAtLeastOneHoliday');
    } else this.utilService.FillUpFields();
  }

  reset() {
    this.form.controls['holidayCalendarCode'].setValue('');
    this.form.controls['holidayCalendarNameEn'].setValue('');
    this.form.controls['holidayCalendarNameAr'].setValue('');
    this.form.controls['year'].setValue('');
    this.form.controls['remarks'].setValue('');
    this.form.controls['isActive'].setValue('');
    this.form.controls['holidayCalendarMappings'].setValue('');
  }
}
