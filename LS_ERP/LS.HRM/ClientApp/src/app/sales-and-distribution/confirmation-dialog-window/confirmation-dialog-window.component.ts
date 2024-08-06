import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { DBOperation } from '../../services/utility.constants';
import { UtilityService } from '../../services/utility.service';
import { ParentSalesMgtComponent } from '../../sharedcomponent/parentsalesmgt.component';
import { SndServicesService } from '../snd-services.service';

@Component({
  selector: 'app-confirmation-dialog-window',
  templateUrl: './confirmation-dialog-window.component.html'
})
export class ConfirmationDialogWindowComponent extends ParentSalesMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
 
  confirmType: string = '';
 
  remarks: string = '';
  noOfUpdates: number;
  constructor(private translate: TranslateService,
    private notifyService: NotificationService, private utilService: UtilityService, public dialogRef: MatDialogRef<ConfirmationDialogWindowComponent>, public dialog: MatDialog, private apiService: ApiService, private authService: AuthorizeService, private fb: FormBuilder, private sndService: SndServicesService) {
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
    this.dialogRef.close(res);
    }
   
}
