import { DateTime } from "luxon";

export interface TblHRMTrnEmployeeContractInfoDto {
    id: number;
    employeeID: number;
    branchCode: string;
    gradeCode: string;
    positionCode: string;
    payrollGroupCode: string;
    holidayCalendarCode: string;
    vacationPolicyCode: string;
    employeeStatusCode: string;
    stopPayroll: boolean;
    lastDateOfDuty: DateTime;
  }