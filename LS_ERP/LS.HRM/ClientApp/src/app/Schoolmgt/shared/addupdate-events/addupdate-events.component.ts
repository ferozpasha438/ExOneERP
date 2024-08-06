import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import {map, startWith} from 'rxjs/operators';
import {FormControl} from '@angular/forms';
import {Observable} from 'rxjs';

@Component({
  selector: 'app-addupdate-events',
  templateUrl: './addupdate-events.component.html',  
})
export class AddupdateEventsComponent implements OnInit {
  id: number = 0;
  row: any;
  form!: FormGroup;
  myControl = new FormControl('');
 // options: string[] = ['One', 'Two', 'Three'];
  filteredOptions!: Observable<string[]>;
  isShown: boolean = false ;
  isSecShown: boolean = false ;
  isApprovalLogin: boolean = false;
  isShowSave: boolean = true;

  branchCodeList: Array<any> = [];
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateEventsComponent>,
    private notifyService: NotificationService) {

  }
  ngOnInit(): void {
    this.loadBranches();
    this.setFrom();
    if (this.id > 0) {
      this.setEditForm();
    }
  }
  setEditForm() {
    this.apiService.get('SchoolScheduleEvents/getScheduleEventById', this.id).subscribe(res => {
      if (res) {
        if (res.isApproved) {
          this.isShowSave = false;
        } else {
          this.apiService.get('TeacherMaster/IsApprovalLoginTeacher', 4).subscribe(res => {
            if (res) {
              this.isApprovalLogin = res;
            }
          });
        }
        this.form.patchValue(res);
        this.form.patchValue({ 'id': 0 });
      }
    });
  }
  setFrom() {
    this.form = this.fb.group({
      'hDate': ['', Validators.required],
      'branchCode': ['', Validators.required],
      'eventName': ['', Validators.required],
      'eventNameAr': ['', Validators.required],
      'eventDescription': ['', Validators.required],
      'eventDescriptionAr': ['', Validators.required],
      'fromTime': ['', Validators.required],
      'toTime': ['', Validators.required],
      'notesOnEvent': ['', Validators.required],
      'isActive': [false, Validators.required],
    });
  }
  loadBranches() {
    this.apiService.getall('schoolBranches/getSchoolBranchList').subscribe(res => {
      this.branchCodeList = res;
    });
  }
  toggleShow() {
    this.isShown = ! this.isShown;    
    }
  toggleShowsec() {
    this.isSecShown = ! this.isSecShown;    
    }
  approveEvent() {
    this.form.value['id'] = this.id;
    this.apiService.post('SchoolScheduleEvents/EventApproval', this.form.value)
      .subscribe(res => {
        this.utilService.OkMessage();
        this.dialogRef.close(true);
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });
  }
  submit() {
    if (this.form.valid) {
      //let td: Date = new Date(this.form.controls['hDate'].value);
      //td.setMinutes(td.getMinutes() - td.getTimezoneOffset());
      //this.form.controls['hDate'].setValue(td);
    if (this.id > 0)
      this.form.value['id'] = this.id;      
    this.apiService.post('SchoolScheduleEvents', this.form.value)
      .subscribe(res => {
        this.utilService.OkMessage();
        this.reset();
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


reset() {
  this.form.reset();
}

closeModel() {
  this.dialogRef.close();
}

}


