import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';

@Component({
  selector: 'app-teacher-subject-mapping',
  templateUrl: './teacher-subject-mapping.component.html',
  styleUrls: []
})
export class TeacherSubjectMappingComponent implements OnInit {
  form!: FormGroup;
  id: number = 0;
  row: any;
  isArab: boolean = false;
  subjectDetails: Array<any> = [];
  editsequence: number = 0;
  sequence: number = 1;
  gradeCodeList: Array<any> = [];
  subjectCodeList: Array<any> = [];
  teacherCode: string = '';
  gradeCode: string = '';
  subjectCode: string = ''
  teachingSkillLevel: number = 0;
  adminSkillLevel: number = 0;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<TeacherSubjectMappingComponent>,
    private notifyService: NotificationService) { }
  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.form = this.fb.group({
    });
    this.loadData();
    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.teacherCode = this.row['teacherCode'];
      this.loadDetailsForTeacherCode(this.teacherCode);
    }
  }
  loadData() {
    this.apiService.getPagination('acedemicClassGrade', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res)
        this.gradeCodeList = res['items'];
    });
  }
  loadRelatedItems() {
    if (this.gradeCode != null && this.gradeCode != '') {
      this.apiService.getall(`SchoolGradeSubjectMapping/getAllSubjectsByGradeCode/${this.gradeCode}`).subscribe(res => {
        if (res)
          this.subjectCodeList = res;
      });
    }
  }
  loadDetailsForTeacherCode(teacherCode: string) {
    this.apiService.getall(`TeacherSubjectsMapping/${teacherCode}`).subscribe(res => {
      if (res) {
        const details = res as Array<any>;
        details.forEach(item => {
          this.subjectDetails.push({
            sequence: this.getSequence(),
            teacherCode: this.teacherCode,
            gradeCode: item.gradeCode,
            subjectCode: item.subjectCode,
            teachingSkillLevel: item.teachingSkillLevel,
            adminSkillLevel: item.adminSkillLevel
          });
        });
      }
    });
  }
  addSubjectItem() {
    if (this.gradeCode != '' && this.subjectCode != '') {
      if (this.editsequence > 0) {
        var index: number = this.subjectDetails.findIndex(a => a.sequence === this.editsequence);
        let pItem = this.subjectDetails[index];
        pItem.gradeCode = this.gradeCode;
        pItem.subjectCode = this.subjectCode;
        pItem.teachingSkillLevel = this.teachingSkillLevel;
        pItem.adminSkillLevel = this.adminSkillLevel;
        this.editsequence = 0;
      }
      else {
        this.subjectDetails.push({
          sequence: this.getSequence(),
          teacherCode: this.teacherCode,
          gradeCode: this.gradeCode,
          subjectCode: this.subjectCode,
          teachingSkillLevel: this.teachingSkillLevel,
          adminSkillLevel: this.adminSkillLevel
        });
      }
      this.setToDefault();
    }
  }
  getSequence(): number { return this.sequence = this.sequence + 1 };

  deleteSubjectItem(item: any) {
    this.removeSubjectList(item.sequence);
  }
  removeSubjectList(sequence: number) {
    let index: number = this.subjectDetails.findIndex(a => a.sequence === sequence);
    this.subjectDetails.splice(index, 1);
  }
  editSubjectItem(item: any) {
    this.editsequence = item.sequence;
    this.gradeCode = item.gradeCode;
    this.teachingSkillLevel = item.teachingSkillLevel;
    this.adminSkillLevel = item.adminSkillLevel;
    if (this.gradeCode != null && this.gradeCode != '') {
      this.apiService.getall(`SchoolGradeSubjectMapping/getAllSubjectsByGradeCode/${this.gradeCode}`).subscribe(res => {
        if (res) {
          this.subjectCodeList = res;
          this.subjectCode = item.subjectCode;
        }
      });
    }
  }
  closeModel() {
    this.dialogRef.close();
  }
  setToDefault() {
    this.gradeCode = '';
    this.subjectCode = '';
    this.teachingSkillLevel = 0;
    this.adminSkillLevel = 0;
  }
  reset() {
    this.form.reset();
    this.setToDefault();
  }
  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      if (this.subjectDetails.length > 0) {
        this.form.value['teacherSubjectsMappingList'] = this.subjectDetails;
        this.apiService.post('TeacherSubjectsMapping', this.form.value)
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
