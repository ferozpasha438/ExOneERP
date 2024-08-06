import { DateTime } from 'luxon';

export interface BaseEmployeeAttendanceDto {
  employeeList: Employee[];
  attendanceColumns: AttendanceColumn[];
}

export interface AttendanceColumn {
  attendanceDate: DateTime;
  attendanceDay: string;
}

export interface Employee {
  employeeID: number;
  employeeName: string;
  branchCode: string;
  branchName: string;
  payrollGroupCode: string;
  payrollGroupName: string;
  attendanceRows: TblTNATrnEmployeeAttendanceDto[];
  holidayCalendarCode: string;
  isRoasterApplicable: boolean;
}

export interface TblTNATrnEmployeeAttendanceDto {
  id: number;
  employeeID: number;
  date: DateTime;
  inTime: string;
  outTime: string;
  attnFlag: string;
  estimatedInTime: string;
  estimatedOutTime: string;
  estimatedNetWorkingTime: string;
  lateHours: string;
  overTimeHours: number;
  netWorkingTime: number;
  shiftCode: string;
  isLate: boolean;
  isSpecialDay: boolean;
  isPunchedOutNextDay: boolean;
  shiftNumber: boolean;
  isApproved: boolean;
  created: string;
  createdBy: number;
  modified: string;
  modifiedBy: number;
  isActive: boolean;
}
