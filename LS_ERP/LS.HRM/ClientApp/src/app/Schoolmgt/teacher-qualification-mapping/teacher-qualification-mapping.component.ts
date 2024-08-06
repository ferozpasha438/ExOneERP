import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';

@Component({
  selector: 'app-teacher-qualification-mapping',
  templateUrl: './teacher-qualification-mapping.component.html',
  styleUrls: []
})
export class TeacherQualificationMappingComponent implements OnInit {
  form!: FormGroup;
  id: number = 0;
  row: any;
  isArab: boolean = false;
  qualificationDetails: Array<any> = [];
  editsequence: number = 0;
  sequence: number = 1;
  teacherCode: string = '';
  qualification: string = '';
  institute: string = '';
  year: string = '';
  grade: string = '';
  percentage: string = '';
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<TeacherQualificationMappingComponent>,
    private notifyService: NotificationService) { }
  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.form = this.fb.group({
    });
    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.teacherCode = this.row['teacherCode'];
      this.loadDetailsForTeacherCode(this.teacherCode);
    }
  }
  loadDetailsForTeacherCode(teacherCode: string) {
    this.apiService.getall(`TeacherQualification/${teacherCode}`).subscribe(res => {
      if (res) {
        const details = res as Array<any>;
        details.forEach(item => {
          this.qualificationDetails.push({
            sequence: this.getSequence(),
            teacherCode: this.teacherCode,
            qualification: item.qualification,
            institute: item.institute,
            year: item.year,
            grade: item.grade,
            percentage: item.percentage
          });
        });
      }
    });
  }
  addQualificationItem() {
    if (this.qualification != '' && this.institute != '' &&
      this.year != '' && this.grade != '' && this.percentage != '') {
      if (this.editsequence > 0) {
        var index: number = this.qualificationDetails.findIndex(a => a.sequence === this.editsequence);
        let pItem = this.qualificationDetails[index];
        pItem.qualification = this.qualification;
        pItem.institute = this.institute;
        pItem.year = this.year;
        pItem.grade = this.grade;
        pItem.percentage = this.percentage;
        this.editsequence = 0;
      }
      else {
        this.qualificationDetails.push({
          sequence: this.getSequence(),
          teacherCode: this.teacherCode,
          qualification: this.qualification,
          institute: this.institute,
          year: this.year,
          grade: this.grade,
          percentage: this.percentage
        });
      }
      this.setToDefault();
    }
  }
  getSequence(): number { return this.sequence = this.sequence + 1 };

  deleteQualificationItem(item: any) {
    this.removeQualificationList(item.sequence);
  }
  removeQualificationList(sequence: number) {
    let index: number = this.qualificationDetails.findIndex(a => a.sequence === sequence);
    this.qualificationDetails.splice(index, 1);
  }
  editQualificationItem(item: any) {
    this.editsequence = item.sequence;
    this.qualification = item.qualification;
    this.institute = item.institute;
    this.year = item.year;
    this.grade = item.grade;
    this.percentage = item.percentage;
  }
  closeModel() {
    this.dialogRef.close();
  }
  setToDefault() {
    this.qualification = '';
    this.institute = '';
    this.year = '';
    this.grade = '';
    this.percentage = '';
  }
  reset() {
    this.form.reset();
    this.setToDefault();
  }
  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      if (this.qualificationDetails.length > 0) {
        this.form.value['teacherQualificationsList'] = this.qualificationDetails;
        this.apiService.post('TeacherQualification', this.form.value)
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
    else
      this.utilService.FillUpFields();
  }

}
