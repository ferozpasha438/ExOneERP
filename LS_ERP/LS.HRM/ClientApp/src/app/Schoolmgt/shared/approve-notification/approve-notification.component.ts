import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-approve-notification',
  templateUrl: './approve-notification.component.html',
  styleUrls: []
})
export class ApproveNotificationComponent implements OnInit {
  form!: FormGroup;
  constructor(public dialogRef: MatDialogRef<ApproveNotificationComponent>,private fb: FormBuilder) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      'notificationCriteria': ['', Validators.required],
      'notificationMessage': ['', Validators.required],
      'draftedBUser': ['', Validators.required],
      'draftedDate': ['', Validators.required],
      'isapproved': [false, Validators.required],
      'approvalRemarks': ['', Validators.required],
      'approveDate': ['', Validators.required],
      'approvedBy': ['', Validators.required],     
    });
  }
  closeModel() {
    this.dialogRef.close();
  }
  submit() {}

}
