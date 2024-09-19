import { DateTime } from 'luxon';

export interface BaseEmployeePaySlipDto {
  paySlipHeader: EmployeePaySlipHeader;
  paySlipDetails: Array<TblPRLTrnEmployeePayrollProcessDto>;
  statusMessage: string;
}

export interface EmployeePaySlipHeader {
  employeeID: number;
  employeeName: string;
  employeeNumber: string;
  branchName: string;
  gradeName: string;
  positionName: string;
  payrollMonth: string;
  calandarDays: number;
  totalOffDays: number;
  totalHolidays: number;
  totalLeaves: number;
  totalAbsents:number;
  netWorkingDays: number;
  earnings: number;
  deductions: number;
  netpay: number;
}

export interface TblPRLTrnEmployeePayrollProcessDto {
  id: number;
  employeeID: number;
  payrollMonth: string;
  payrollComponentCode: string;
  payrollComponentName: string;
  isFormula: boolean;
  formulaQueryString: string;
  payrollComponentType: number;
  payValue: number;
  isApproved: boolean;
  isReleased: boolean;
}
