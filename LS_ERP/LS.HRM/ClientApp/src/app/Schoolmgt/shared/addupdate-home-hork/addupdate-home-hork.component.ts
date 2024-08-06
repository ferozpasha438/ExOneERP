import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';

@Component({
  selector: 'app-addupdate-home-hork',
  templateUrl: './addupdate-home-hork.component.html',
})
export class AddupdateHomeHorkComponent implements OnInit {
  id: number = 0;
  row: any;
  gradeCodeList: Array<any> = [];
  subjectCodeList: Array<any> = [];
  teacherList: Array<any> = [];
  form!: FormGroup;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateHomeHorkComponent>,
    private notifyService: NotificationService) {

  }
  ngOnInit(): void {
    this.form = this.fb.group({
      'homeworkDate': ['', Validators.required],
      'teacherCode': ['', Validators.required],
      'gradeCode': ['', Validators.required],
      'subCodes': ['', Validators.required],
      'homeWorkDescription': ['', Validators.required],
      'homeWorkDescription_Ar': ['', Validators.required],
      'remarks': [''],
      'isActive': [false, Validators.required],
    });
    this.loadData();
  }
  loadData() {
    this.apiService.getall('TeacherMaster/GetTeachersList').subscribe(res => {
      if (res) {
        this.teacherList = res;
        if (this.row) {
          this.id = parseInt(this.row['id']);
          this.form.patchValue(this.row);
          this.loadGrades();
        }
      }
    });
  }
  loadGrades() {
    const teacherCode: string = this.form.value['teacherCode'] as string;
    if (teacherCode != null && teacherCode != '') {
      this.apiService.getall(`TeacherClassMapping/GetTeacherGradesByTeacherCode/${teacherCode}`).subscribe(res => {
        if (res) {
          this.gradeCodeList = res;
          if (this.row) {
            this.id = parseInt(this.row['id']);
            this.form.patchValue(this.row);
            this.loadRelatedItems();
          }
        }
      });
    }
  }
  loadRelatedItems() {
    const teacherCode: string = this.form.value['teacherCode'] as string;
    const gradeCode: string = this.form.value['gradeCode'] as string;
    if (teacherCode != null && teacherCode != '' && gradeCode != null && gradeCode != '') {
      this.apiService.getall(`TeacherSubjectsMapping/GetTeacherSubjectsByGradeCode/${teacherCode}/${gradeCode}`).subscribe(res => {
        if (res) {
          this.subjectCodeList = res;
          if (this.row) {
            this.id = parseInt(this.row['id']);
            this.form.patchValue(this.row);
          }
        }
          
      });
    }
  }

submit() {
  if (this.form.valid) {
    if (this.id > 0)
      this.form.value['id'] = this.id;
    this.form.value['homeworkDate'] = this.utilService.selectedDate(this.form.controls['homeworkDate'].value);
    this.apiService.post('StudentHomeWork', this.form.value)
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
