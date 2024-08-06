import { Time } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { Timestamp } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../models/MenuItemListDto';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { DBOperation } from '../../services/utility.constants';
import { UtilityService } from '../../services/utility.service';
import { ParentOptMgtComponent } from '../../sharedcomponent/parentoptmgt.component';
import { ManualAttendancePopupComponent } from '../employee/employee-attendance/manual-attendance-popup/manual-attendance-popup.component';
import { OprServicesService } from '../opr-services.service';



@Component({
  selector: 'app-confirm-dialog-window-general',
  templateUrl: './confirm-dialog-window-general.component.html'
})
export class ConfirmDialogWindowGeneralComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  inputData: any;
  inputCellData: any;
  leaveData: any;
  confirmType: string = '';
  leaveType: string = '';
  remarks: string = '';
  noOfUpdates: number;

  titleQuestion: string = this.translate.instant("Are_You_Sure?");
  subTitleQuestion: string = '';
  constructor(private translate: TranslateService,
    private notifyService: NotificationService, private utilService: UtilityService, public dialog: MatDialog, private apiService: ApiService, public dialogRef: MatDialogRef<ManualAttendancePopupComponent>, private authService: AuthorizeService, private fb: FormBuilder, private oprService: OprServicesService) {
    super(authService);
  }

  ngOnInit(): void {
        this.setForm();
  }

  setForm() {

    this.form = this.fb.group({

    });
  }





  closeModel(res: any) {
    if (res == 'true' && ((this.confirmType == "transResign" && this.leaveType == 'TR' && this.remarks != "") || (this.confirmType == "transResign" && this.leaveType == 'R') || this.confirmType == "leave"))
      this.dialogRef.close({ res: true, remarks: this.remarks });
    else if (res == 'true' && this.confirmType == "transResign" && this.leaveType =='TR')
    {
      this.notifyService.showError(this.translate.instant("Enter_Remarks"));
    }
    else if (res == 'true' && this.confirmType == "cancelLeave") {
      this.dialogRef.close(true);
    }
    else if (res == 'true' && this.confirmType == "postAttendance") {
      this.dialogRef.close(true);
    }
    else if (res == 'true' && this.confirmType == "general")
    {
      this.dialogRef.close(true);
    }
    else if (res == 'true')
    {
      this.dialogRef.close(true);
    }
    else
      this.dialogRef.close(false);
  }
  
}
