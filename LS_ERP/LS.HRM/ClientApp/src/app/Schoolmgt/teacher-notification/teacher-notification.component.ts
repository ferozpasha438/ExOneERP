import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';


@Component({
  selector: 'app-teacher-notification',
  templateUrl: './teacher-notification.component.html'
})
export class TeacherNotificationComponent implements OnInit {
  form!: FormGroup;
  id: number = 0;
  row: any;
  isArab: boolean = false;
  teacherCode: string = '';
  isShowPublish: boolean = false;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<TeacherNotificationComponent>,
    private notifyService: NotificationService) { }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.form = this.fb.group({
      'notificationTitle': ['', Validators.required],
      'notificationTitle_Ar': ['', Validators.required],
      'notificationMessage': ['', Validators.required],
      'notificationMessage_Ar': ['', Validators.required],
    });
    if (this.row) {
      this.teacherCode = this.row['teacherCode'];
      this.getLatestNotification();
    }
  }
  reset() {
    this.form.reset();
  }

  closeModel() {
    this.dialogRef.close();
  }
  publishNotification() {
    this.form.value['id'] = this.id;
    this.form.value['notificationType'] = 1;
    this.apiService.post('WebNotification/NotificationApproval', this.form.value)
      .subscribe(res => {
        this.utilService.OkMessage();
        this.reset();
        this.getLatestNotification();
        this.isShowPublish = true;
        this.dialogRef.close(true);
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });
  }
  getLatestNotification() {
    this.apiService.getall(`WebNotification/${this.teacherCode}/1`).subscribe(res => {
      if (res) {
        if (!res.isApproved) {
          this.isShowPublish = true;
          this.id = res.id;
          this.form.patchValue({
            'notificationTitle': res.notificationTitle,
            'notificationTitle_Ar': res.notificationTitle_Ar,
            'notificationMessage': res.notificationMessage,
            'notificationMessage_Ar': res.notificationMessage_Ar
          });
        }
      }
    });
  }
  submit() {
    if (this.form.valid) {
      this.form.value['id'] = this.id;
      this.form.value['notificationType'] = 1;
      this.form.value['code'] = this.teacherCode;
      this.apiService.post('WebNotification', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.reset();
          this.getLatestNotification();
          this.dialogRef.close(true);
        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
          });
    }
    else
      this.utilService.FillUpFields();
  }
}
