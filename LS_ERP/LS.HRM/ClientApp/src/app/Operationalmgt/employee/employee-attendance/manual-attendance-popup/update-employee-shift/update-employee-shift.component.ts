import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../../../services/api.service';
import { NotificationService } from '../../../../../services/notification.service';
import { UtilityService } from '../../../../../services/utility.service';
import { ParentOptMgtComponent } from '../../../../../sharedcomponent/parentoptmgt.component';
import { OprServicesService } from '../../../../opr-services.service';
import { ManualAttendancePopupComponent } from '../manual-attendance-popup.component';

@Component({
  selector: 'app-update-employee-shift',
  templateUrl: './update-employee-shift.component.html'
})
export class UpdateEmployeeShiftComponent extends ParentOptMgtComponent implements OnInit {
  inputData: any;
  inputShift: string = '';
  updatedShift: string = '';

  employeeData: any;
  isArabic: boolean = false;
  constructor(private translate: TranslateService,
    private notifyService: NotificationService, private utilService: UtilityService, public dialog: MatDialog, private apiService: ApiService, public dialogRef: MatDialogRef<UpdateEmployeeShiftComponent>, private authService: AuthorizeService, private fb: FormBuilder, private oprService: OprServicesService) {

    super(authService);
  }

  ngOnInit(): void {

   this.isArabic= this.utilService.isArabic();
    this.inputShift = this.inputData?.shiftCode;
    this.updatedShift = this.inputData?.shiftCode;
    this.getEmployee(this.inputData.roasterData.employeeNumber);
    console.log(this.inputData);
  
  }
  getEmployee(empNumber: string) {
    this.apiService.getall(`Employee/getEmployeeByEmployeeNumber/${empNumber}`).subscribe(res => {

      if (this.isArabic) {
        res.name = res.employeeName_AR;
      }
      else {
        res.name = res.employeeName;
      }
      this.employeeData=res;
    });

  }

  closeModel() {

   this.dialogRef.close(false);

   
  }


  save() {
    if (this.updatedShift != '' && this.updatedShift != this.inputShift) {

      let UpdateShiftCodeForDayDto: any = {
        day: this.inputData.c,
        roasterId: this.inputData.roasterData.id,
        shiftCode: this.updatedShift
      }




      this.apiService.post('monthlyRoaster/updateShiftCodeForDay', UpdateShiftCodeForDayDto).subscribe((res: any) => {
        if (res > 0) {
          this.dialogRef.close({ result: res, res: true, r: this.inputData.r, c: this.inputData.c, isShiftUpdated: true, updatedShift: this.updatedShift });
        }
        else {
          this.dialogRef.close({ result: res, res: false});
        }
                     
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
          this.dialogRef.close({ result: -4, res: false});

        });
    }

  }
}
