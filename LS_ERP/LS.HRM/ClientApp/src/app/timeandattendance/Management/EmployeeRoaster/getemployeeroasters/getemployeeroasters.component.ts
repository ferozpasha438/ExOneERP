import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from 'src/app/api-authorization/AuthorizeService';
import { CustomSelectListItem } from 'src/app/models/MenuItemListDto';
import {
  BaseEmployeeRoasterDto,
  RoasterColumn,
  RoasterRow,
  TblTNATrnEmployeeRoasterDto,
} from 'src/app/models/TimeAndAttendance/BaseEmployeeRoasterDto';
import { ApiService } from 'src/app/services/api.service';
import { NotificationService } from 'src/app/services/notification.service';
import { UtilityService } from 'src/app/services/utility.service';
import { ParenttnamgtComponent } from 'src/app/sharedcomponent/parenttnamgt.component';

@Component({
  selector: 'app-getemployeeroasters',
  templateUrl: './getemployeeroasters.component.html',
  styles: [],
})
export class GetemployeeroastersComponent
  extends ParenttnamgtComponent
  implements OnInit
{
  constructor(
    private fb: FormBuilder,
    private authService: AuthorizeService,
    private apiService: ApiService,
    private utilService: UtilityService,
    private notifyService: NotificationService,
    private translate: TranslateService
  ) {
    super(authService);
  }
  form!: FormGroup;
  payrollGroups: Array<CustomSelectListItem> = [];
  branches: Array<CustomSelectListItem> = [];
  shifts: Array<CustomSelectListItem> = [];
  payrollPeriods: Array<CustomSelectListItem> = [];
  branchCode: string = '';
  payrollGroupCode: string = '';
  payrollPeriodCode: string = '';
  data!: BaseEmployeeRoasterDto;
  roasterColumns: Array<RoasterColumn> = [];
  isSaveEnabled: boolean = false;

  ngOnInit(): void {
    this.setForm();
    this.loadPayrollGroups();
    this.loadBranches('1');
    this.loadPayrollPeriod();
    this.loadShifts();
  }

  setForm() {
    this.form = this.fb.group({
      employeeRoasters: this.fb.array([this.addEmployeeRoastersRow()]),
      roasterColumns: this.fb.array([this.addRoasterColumn()]),
    });
  }

  get formEmployeeRoasterArr() {
    return this.form.get('employeeRoasters') as FormArray;
  }

  get formRoasterColumnsArr() {
    return this.form.get('roasterColumns') as FormArray;
  }

  formRoasterRowsArr(i: number): FormArray {
    return this.formEmployeeRoasterArr.at(i).get('roasterRows')
      ?.value as FormArray;
  }

  loadPayrollGroups() {
    this.apiService
      .getall('PayrollGroup/GetPayrollGroupSelectListItem')
      .subscribe((res) => {
        this.payrollGroups = res;
      });
  }

  loadBranches(value: string) {
    this.apiService
      .getQueryString(`Branch/GetBranchesByCompany?id=`, value)
      .subscribe((res) => {
        this.branches = res;
      });
  }

  loadShifts() {
    this.apiService.getall('Shift/getShiftSelectListItem').subscribe((res) => {
      this.shifts = res;
    });
  }

  loadPayrollPeriod() {
    this.apiService
      .getall('PayrollPeriod/GetPayrollPeriodSelectListItem')
      .subscribe(
        (res) => {
          this.payrollPeriods = res;
        },
        (error) => {
          this.utilService.ShowApiErrorMessage(error);
        }
      );
  }

  GetEmployeeRoaster() {
    if (this.payrollGroupCode && this.branchCode && this.payrollPeriodCode) {
      let queryParam = `payrollGroupCode=${encodeURIComponent(
        '' + this.payrollGroupCode
      )}&branchCode=${encodeURIComponent('' + this.branchCode)}
      &payrollPeriodCode=${encodeURIComponent('' + this.payrollPeriodCode)}`;
      this.apiService
        .getQueryString(`EmployeeRoaster?`, queryParam)
        .subscribe((res) => {
          this.data = res;
          this.isSaveEnabled = false;
          if (this.data) {
            this.formEmployeeRoasterArr.clear();
            this.formRoasterColumnsArr.clear();
            this.roasterColumns = [];
            //Add employees along with their Roasters
            this.data.employeeRoasters.forEach((row) => {
              this.formEmployeeRoasterArr.push(
                this.addEmployeeRoastersRow(row)
              );
            });

            //Add columns
            this.data.roasterColumns.forEach((row) => {
              this.roasterColumns.push(row);
              this.formRoasterColumnsArr.push(this.addRoasterColumn(row));
            });

            //Show Notification if roasters does not exists.
            if (this.data.employeeRoasters.length == 0)
              this.notifyService.showError(
                this.translate.instant('RoasterDoesNotExistsForEmployees')
              );
          }
        });
    } else
      this.notifyService.showError(this.translate.instant('SelectFilters'));
  }

  addRoasterRows(roasterRows?: RoasterRow[]): FormArray {
    if (roasterRows)
      return new FormArray(roasterRows.map((x) => this.addRoasterRow(x)));
    else return new FormArray([this.addRoasterRow()]);
  }

  updateEmployeeRoaster(i: number, j: number, event: any) {
    let element: any;
    this.isSaveEnabled = true;
    for (let index = j; index <= this.roasterColumns.length; index++) {
      element = document.getElementsByName(
        'select_' + i.toString() + '_' + index.toString()
      )[0];
      if (element.value != 'O') {
        element.value = event.target.value;
        (<FormArray>this.form.get('employeeRoasters'))
          .at(i)
          .get('roasterRows')
          ?.value.at(index)
          .get('shiftCode')
          .setValue(event.target.value);
      } else {
        if (element.value != 'O' || event.target.value === 'O') {
          (<FormArray>this.form.get('employeeRoasters'))
            .at(i)
            .get('roasterRows')
            ?.value.at(index)
            .get('shiftCode')
            .setValue(event.target.value);
        }
        break;
      }
    }
  }

  addRoasterRow(obj?: RoasterRow) {
    if (obj)
      return this.fb.group({
        roasterDate: [obj.roasterDate],
        shiftCode: [obj.shiftCode],
      });
    else
      return this.fb.group({
        roasterDate: [''],
        shiftCode: [''],
      });
  }

  addEmployeeRoastersRow(obj?: TblTNATrnEmployeeRoasterDto) {
    if (obj)
      return this.fb.group({
        id: [obj.id],
        employeeID: [obj.employeeID],
        employeeName: [obj.employeeName],
        branchCode: [obj.branchCode],
        branchName: [obj.branchName],
        payrollGroupCode: [obj.payrollGroupCode],
        PayrollGroupName: [obj.PayrollGroupName],
        roasterRows: [this.addRoasterRows(obj.roasterRows)],
        isShiftApplicable: [obj.isShiftApplicable],
        created: [obj.created],
        createdBy: [obj.createdBy],
        modified: [obj.modified],
        modifiedBy: [obj.modifiedBy],
        isActive: [obj.isActive],
      });
    else
      return this.fb.group({
        id: [''],
        employeeID: [''],
        employeeName: [''],
        branchCode: [''],
        branchName: [''],
        payrollGroupCode: [''],
        PayrollGroupName: [''],
        roasterRows: [this.addRoasterRows()],
        isShiftApplicable: [false],
        created: [''],
        createdBy: [''],
        modified: [''],
        modifiedBy: [''],
        isActive: [true],
      });
  }

  addRoasterColumn(obj?: RoasterColumn) {
    if (obj)
      return this.fb.group({
        roasterDate: [obj.roasterDate],
        roasterDay: [obj.roasterDay],
      });
    else
      return this.fb.group({
        roasterDate: [''],
        roasterDay: [''],
      });
  }

  submit() {
    if (this.form.valid) {
      this.data = this.form.value;
      this.data.employeeRoasters.forEach((e, i) => {
        e.roasterRows = (<FormArray>this.form.get('employeeRoasters'))
          .at(i)
          .get('roasterRows')?.value.value;
      });
      this.apiService.post('EmployeeRoaster', this.data).subscribe(
        (res) => {
          this.utilService.OkMessage();
        },
        (error) => {
          this.utilService.ShowApiErrorMessage(error);
        }
      );
    } else this.utilService.FillUpFields();
  }

  GenerateEmployeeRoaster() {
    if (this.payrollGroupCode && this.branchCode && this.payrollPeriodCode) {
      let queryParam = `payrollGroupCode=${encodeURIComponent(
        '' + this.payrollGroupCode
      )}&branchCode=${encodeURIComponent('' + this.branchCode)}
      &payrollPeriodCode=${encodeURIComponent('' + this.payrollPeriodCode)}`;
      this.apiService
        .getQueryString(`EmployeeRoaster/GenerateEmployeesRoaster?`, queryParam)
        .subscribe(
          (res) => {
            if (res.id == 1) {
              this.utilService.OkMessage();
              this.GetEmployeeRoaster();
            } else
              this.notifyService.showError(this.translate.instant(res.message));
          },
          (error) => {
            this.utilService.ShowApiErrorMessage(error);
          }
        );
    } else
      this.notifyService.showError(this.translate.instant('SelectAllFilters'));
  }
}
