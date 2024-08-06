import { DateTime } from 'luxon';

export interface TblPRLTrnEmployeePayrollStructureDto {
  id: number;
  employeeID: number;
  packageCode: string;
  payrollComponentCode: string;
  payrollComponentName: string;
  isFormula: boolean;
  payValue: number;
  payrollComponentType: number;
  formulaQueryString: string;
  isUsedInPayroll: boolean;
}
