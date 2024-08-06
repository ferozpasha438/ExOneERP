import { DateTime } from 'luxon';

export interface RoasterColumn {
  roasterDate: DateTime;
  roasterDay: string;
}

export interface RoasterRow {
  roasterDate: DateTime;
  shiftCode: string;
}

export interface TblTNATrnEmployeeRoasterDto {
  id: number;
  employeeID: number;
  employeeName: string;
  branchCode: string;
  branchName: string;
  payrollGroupCode: string;
  PayrollGroupName: string;
  roasterRows: RoasterRow[];
  isShiftApplicable: boolean;
  created: string;
  createdBy: number;
  modified: string;
  modifiedBy: number;
  isActive: boolean;
}

export interface BaseEmployeeRoasterDto {
  employeeRoasters: TblTNATrnEmployeeRoasterDto[];
  roasterColumns: RoasterColumn[];
}
