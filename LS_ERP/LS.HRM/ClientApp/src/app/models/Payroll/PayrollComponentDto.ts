import { DateTime } from "luxon";

export interface TblPRLSysPayrollComponentDto {
    id: number;
    payrollComponentCode: string;
    payrollComponentNameEn: string;
    payrollComponentNameAr: string;
    payrollComponentType: number;
    isFormula: boolean;
    isUsedForOtherPayrollComponent: boolean;
    isApplicableForDeduction: boolean;
    formulaQueryString: string;
    payrollComponentTypeName: string;
    created: DateTime;
    createdBy: number;
    modified: DateTime;
    modifiedBy: number;
    isActive: boolean;
    payrollComponentName: string;
  }