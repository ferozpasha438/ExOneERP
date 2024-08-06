export interface BaseConsolidatedAttendanceDto {
  consolidatedAttendanceList: TblTNATrnConsolidatedEmployeeAttendanceDto[];
}

export interface TblTNATrnConsolidatedEmployeeAttendanceDto {
  id: number;
  employeeID: number;
  employeeName: string;
  branchCode: string;
  branchName: string;
  totalDays: number;
  totalPresentDays: number;
  totalOffDays: number;
  totalLeaves: number;
  totalHolidays: number;
  totalAbsents: number;
  netWorkingDays: number;
  totalLateDays: number;
  totalLateHours: number;
  normalOTHours: number;
  specialOTHours: number;
  shiftNumber: boolean;
}
