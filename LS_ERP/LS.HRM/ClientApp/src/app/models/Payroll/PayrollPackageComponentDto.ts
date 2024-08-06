export interface TblPRLTrnPayrollPackageComponentDto {
  id: number;
  packageCode: string;
  payrollComponentCode: string;
  payrollComponentName: string;
  isFormula: boolean;
  payValue: number;
  payrollComponentType: number;
  formulaQueryString: string;
}
