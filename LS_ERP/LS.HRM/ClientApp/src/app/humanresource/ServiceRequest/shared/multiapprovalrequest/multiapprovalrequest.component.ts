import { Component, OnInit } from '@angular/core';
import { ValidationService } from '../../../../sharedcomponent/ValidationService';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ApiService } from '../../../../services/api.service';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { NotificationService } from '../../../../services/notification.service';
import { UtilityService } from '../../../../services/utility.service';
import { not } from '@angular/compiler/src/output/output_ast';

@Component({
  selector: 'app-multiapprovalrequest',
  templateUrl: './multiapprovalrequest.component.html',
  styles: [
  ]
})
export class MultiapprovalrequestComponent implements OnInit {
  actionType: number = 0;
  isRejectOrApprovig: boolean = false;
  data: any;
  remarks: string = '';
  constructor(private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<MultiapprovalrequestComponent>,
    private notifyService: NotificationService, private validationService: ValidationService, public dialog: MatDialog) {
  };

  ngOnInit(): void {
  }

  reject() {
    this.actionType = 3; //C# ProcessStage  enum
    this.rejectOrAppove();
  }
  appove() {
    this.actionType = 4; //C# ProcessStage  enum
    this.rejectOrAppove();
  }

  rejectOrAppove() {
    if (this.utilService.hasValue(this.remarks)) {
      this.isRejectOrApprovig = true;
      this.apiService.post('serviceRequest/approvalVacationRequestList', { ids: this.data, actionType: this.actionType, remarks: this.remarks }).subscribe(res => {
        this.isRejectOrApprovig = false;
        this.utilService.OkMessage();
        this.dialogRef.close(true);
      },
        error => {
          this.utilService.ShowApiErrorMessage(error);
        });
    }
    else
      this.utilService.FillUpFields();
  }
  closeModel() {
    this.dialogRef.close();
  }


}
