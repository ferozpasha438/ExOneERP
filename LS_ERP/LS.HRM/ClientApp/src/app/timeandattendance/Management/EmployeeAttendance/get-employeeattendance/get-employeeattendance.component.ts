import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { AuthorizeService } from 'src/app/api-authorization/AuthorizeService';
import { CustomSelectListItem } from 'src/app/models/MenuItemListDto';
import { ApiService } from 'src/app/services/api.service';
import { DBOperation } from 'src/app/services/utility.constants';
import { UtilityService } from 'src/app/services/utility.service';
import { ParenttnamgtComponent } from 'src/app/sharedcomponent/parenttnamgt.component';
import { ImportemployeeattendanceComponent } from '../importemployeeattendance/importemployeeattendance.component';
import { TranslateService } from '@ngx-translate/core';
import {
  AttendanceColumn,
  BaseEmployeeAttendanceDto,
  Employee,
} from 'src/app/models/TimeAndAttendance/BaseEmployeeAttendanceDto';
import { DeleteConfirmDialogComponent } from 'src/app/sharedcomponent/delete-confirm-dialog';
import { ConsolidateemployeeattendanceComponent } from '../consolidateemployeeattendance/consolidateemployeeattendance.component';

@Component({
  selector: 'app-get-employeeattendance',
  templateUrl: './get-employeeattendance.component.html',
  styles: [],
})
export class GetEmployeeattendanceComponent
  extends ParenttnamgtComponent
  implements OnInit
{
  constructor(
    private fb: FormBuilder,
    private authService: AuthorizeService,
    private apiService: ApiService,
    private utilService: UtilityService,
    public dialog: MatDialog,
    private translate: TranslateService
  ) {
    super(authService);
  }
  form!: FormGroup;
  payrollGroups: Array<CustomSelectListItem> = [];
  branches: Array<CustomSelectListItem> = [];
  data!: BaseEmployeeAttendanceDto;
  attendanceColumns: Array<AttendanceColumn> = [];
  employeeList: Array<Employee> = [];
  payrollGroupCode: string = '';
  branchCode: string = '';

  ngOnInit(): void {
    this.loadPayrollGroups();
    this.loadBranches('1');
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

  private openDialogManage(modalTitle: string, modalBtnTitle: string) {
    let dialogRef = this.utilService.openCrudDialog(
      this.dialog,
      ImportemployeeattendanceComponent,
      '35%',
      '36%'
    );
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
  }

  public importemployeeattendance() {
    this.openDialogManage(
      this.translate.instant('AddAttachment'),
      this.translate.instant('ImportFile')
    );
  }

  public GetEmployeeAttendance() {
    if (this.payrollGroupCode && this.branchCode) {
      let queryParam = `payrollGroupCode=${encodeURIComponent(
        '' + this.payrollGroupCode
      )}&branchCode=${encodeURIComponent('' + this.branchCode)}`;
      this.apiService
        .getQueryString(`EmployeeAttendance?`, queryParam)
        .subscribe((res) => {
          this.data = res;
          if (this.data) {
            this.attendanceColumns = [];
            this.employeeList = [];

            //Add employees along with their attendance
            this.data.employeeList.forEach((row) => {
              this.employeeList.push(row);
            });

            //Add attendance columns
            this.data.attendanceColumns.forEach((row) => {
              this.attendanceColumns.push(row);
            });
          }
        });
    }
  }

  public ApproveAttendance() {
    const dialogRef = this.utilService.openDeleteConfirmDialog(
      this.dialog,
      DeleteConfirmDialogComponent
    );
    (dialogRef.componentInstance as any).modalTitle =
    this.translate.instant('ConfirmApproveAttendance');
    dialogRef.afterClosed().subscribe(
      (canApprove) => {
        if (canApprove) {
          if (this.payrollGroupCode && this.branchCode) {
            let queryParam = `payrollGroupCode=${encodeURIComponent(
              '' + this.payrollGroupCode
            )}&branchCode=${encodeURIComponent('' + this.branchCode)}`;
            this.apiService
              .getQueryString(
                `EmployeeAttendance/ApproveEmployeeAttendance?`,
                queryParam
              )
              .subscribe(
                (res) => {
                  this.utilService.OkMessage();
                },
                (error) => {
                  this.utilService.ShowApiErrorMessage(error);
                }
              );
          }
        }
      },
      (error) => this.utilService.ShowApiErrorMessage(error)
    );
  }

  private openConsolidatedAttendanceDialog(
    payrollGroupCode: string = '',
    branchCode: string = '',
    modalTitle: string
  ) {
    let dialogRef = this.utilService.openCrudDialog(
      this.dialog,
      ConsolidateemployeeattendanceComponent
    );
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).payrollGroupCode = payrollGroupCode;
    (dialogRef.componentInstance as any).branchCode = branchCode;
  }

  public GetEmployeeConsolidatedAttendance() {
    if (this.payrollGroupCode && this.branchCode) {
      this.openConsolidatedAttendanceDialog(
        this.payrollGroupCode,
        this.branchCode,
        this.translate.instant('EmployeeConsolidatedAttendance'),
      );
    }
  }
}
