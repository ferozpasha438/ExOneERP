import { DateTime } from 'luxon';

export interface EmployeeBasicInfoDto {
  employeeImageUrl?: string | ArrayBuffer | null;
  allowImageUpload?: boolean;
  employeeName?: string;
  employeeNumber?: string;
  file?: File;
}
