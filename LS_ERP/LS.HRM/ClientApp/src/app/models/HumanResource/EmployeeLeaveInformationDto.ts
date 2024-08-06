import { DateTime } from 'luxon';

export interface TblHRMTrnEmployeeLeaveInformationDto {
  id: number;
  employeeID: number;
  employeeName: string;
  templateCode: string;
  // templateName: string;
  leaveTypeCode: string;
  leaveTypeName: string;
  assigned: number;
  availed: number;
  tranDate: DateTime;
  remarks: string;
  isUpdate: boolean;
  type: number;
}
