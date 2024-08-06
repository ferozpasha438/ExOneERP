import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';


@Component({
  selector: 'app-teacher-class-mapping',
  templateUrl: './teacher-class-mapping.component.html'
})
export class TeacherClassMappingComponent implements OnInit {
  form!: FormGroup;
  id: number = 0;
  row: any;
  isArab: boolean = false;
  classDetails: Array<any> = [];
  editsequence: number = 0;
  sequence: number = 1;
  gradeCodeList: Array<any> = [];
  sectionCodeList: Array<any> = [];
  teacherCode: string = '';
  gradeCode: string = '';
  sectionCode: string = '';
  isMapped: boolean = false;

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<TeacherClassMappingComponent>,
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
      this.apiService.getall(`SchoolGradeSectionMapping/getAllSectionsByGradeCode/${this.gradeCode}`).subscribe(res => {
        if (res)
          this.sectionCodeList = res;
      });
    }
  }
  loadDetailsForTeacherCode(teacherCode: string) {
    this.apiService.getall(`TeacherClassMapping/${teacherCode}`).subscribe(res => {
      if (res) {
        const details = res as Array<any>;
        details.forEach(item => {
          this.classDetails.push({
            sequence: this.getSequence(),
            teacherCode: this.teacherCode,
            gradeCode: item.gradeCode,
            sectionCode: item.sectionCode,
            isMapped: item.isMapped
          });
        });
      }
    });
  }
  addClassItem() {
    if (this.gradeCode != '' && this.sectionCode != '') {
      if (this.editsequence > 0) {
        var index: number = this.classDetails.findIndex(a => a.teacherCode === this.teacherCode
          && a.gradeCode === this.gradeCode && a.sectionCode === this.sectionCode);
        var pIndex: number = this.classDetails.findIndex(a => a.sequence === this.editsequence);
        if (index < 0 || index == pIndex) {
          let pItem = this.classDetails[pIndex];
          pItem.gradeCode = this.gradeCode;
          pItem.sectionCode = this.sectionCode;
          pItem.isMapped = this.isMapped;
          this.editsequence = 0;
        } else {
          this.editsequence = 0;
          this.gradeCode = '';
          this.sectionCode = ''
          this.isMapped = false;
          this.notifyService.showError("Already added this grade and section");
        }
      }
      else {
        var index: number = this.classDetails.findIndex(a => a.teacherCode === this.teacherCode
          && a.gradeCode === this.gradeCode && a.sectionCode === this.sectionCode);
        if (index < 0) {
          this.classDetails.push({
            sequence: this.getSequence(),
            teacherCode: this.teacherCode,
            gradeCode: this.gradeCode,
            sectionCode: this.sectionCode,
            isMapped: this.isMapped
          });
        } else {
          this.gradeCode = '';
          this.sectionCode = ''
          this.isMapped = false;
          this.notifyService.showError("Already added this grade and section");
        }
      }
      this.setToDefault();
    }
  }
  getSequence(): number { return this.sequence = this.sequence + 1 };

  deleteClassItem(item: any) {
    this.removeClassList(item.sequence);
  }
  removeClassList(sequence: number) {
    let index: number = this.classDetails.findIndex(a => a.sequence === sequence);
    this.classDetails.splice(index, 1);
  }
  editClassItem(item: any) {
    this.editsequence = item.sequence;
    this.gradeCode = item.gradeCode;
    this.sectionCode = item.sectionCode;
    this.isMapped = item.isMapped;
  }
  closeModel() {
    this.dialogRef.close();
  }
  setToDefault() {
    this.gradeCode = '';
    this.sectionCode = '';
    this.isMapped = false;
  }
  reset() {
    this.form.reset();
    this.setToDefault();
  }
  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      if (this.classDetails.length > 0) {
        this.form.value['teacherClassesMappingList'] = this.classDetails;
        this.apiService.post('TeacherClassMapping', this.form.value)
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
