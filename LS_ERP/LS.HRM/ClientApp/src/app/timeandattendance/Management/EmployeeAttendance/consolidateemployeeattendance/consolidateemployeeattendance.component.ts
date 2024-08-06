import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from 'src/app/api-authorization/AuthorizeService';
import {
  BaseConsolidatedAttendanceDto,
  TblTNATrnConsolidatedEmployeeAttendanceDto,
} from 'src/app/models/TimeAndAttendance/BaseConsolidatedAttendanceDto';
import { ApiService } from 'src/app/services/api.service';
import { UtilityService } from 'src/app/services/utility.service';
import { ParenttnamgtComponent } from 'src/app/sharedcomponent/parenttnamgt.component';

@Component({
  selector: 'app-consolidateemployeeattendance',
  templateUrl: './consolidateemployeeattendance.component.html',
  styles: [],
})
export class ConsolidateemployeeattendanceComponent
  extends ParenttnamgtComponent
  implements OnInit
{
  constructor(
    private authService: AuthorizeService,
    private apiService: ApiService,
    private utilService: UtilityService,
    private translate: TranslateService,
    public dialogRef: MatDialogRef<ConsolidateemployeeattendanceComponent>
  ) {
    super(authService);
  }
  searchValue: string = '';
  modalTitle!: string;
  payrollGroupCode!: string;
  branchCode!: string;
  baseConsolidatedAttendance!: BaseConsolidatedAttendanceDto;
  consolidatedAttendanceList: Array<TblTNATrnConsolidatedEmployeeAttendanceDto> =
    [];

  ngOnInit(): void {
    this.ConsolidateEmployeeAttendance();
  }

  ConsolidateEmployeeAttendance() {
    if (this.payrollGroupCode && this.branchCode) {
      let queryParam = `payrollGroupCode=${encodeURIComponent(
        '' + this.payrollGroupCode
      )}&branchCode=${encodeURIComponent('' + this.branchCode)}`;
      if (this.searchValue)
        queryParam =
          queryParam + `&employeeName=${encodeURIComponent('' + this.searchValue)}`;
      this.apiService
        .getQueryString(
          `EmployeeAttendance/ConsolidateEmployeeAttendance?`,
          queryParam
        )
        .subscribe((res) => {
          if (res) this.consolidatedAttendanceList = res;
        });
    }
  }

  closeModel() {
    this.dialogRef.close();
  }
}
