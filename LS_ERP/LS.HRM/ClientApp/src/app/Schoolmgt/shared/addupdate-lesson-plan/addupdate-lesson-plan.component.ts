import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { map, startWith } from 'rxjs/operators';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-addupdate-lesson-plan',
  templateUrl: './addupdate-lesson-plan.component.html',
})
export class AddupdateLessonPlanComponent implements OnInit {
  id: number = 0;
  row: any;
  form!: FormGroup;
  isArab: boolean = false;
  //myControl = new FormControl('');
  //options: string[] = ['One', 'Two', 'Three'];
  //filteredOptions!: Observable<string[]>;
  isShown: boolean = true;
  //isSecShown: boolean = false;
  isShowLessonPlanCode: boolean = false;
  branchCodeList: Array<any> = [];
  teacherList: Array<any> = [];
  gradeCodeList: Array<any> = [];
  subCodesList: Array<any> = [];
  sectionList: Array<any> = [];
  listNum: number = 0;
  buttonstatus = false;
  teacherName: string = '';

  branchCode: string = '';
  sectionCode: string = '';
  gradeCode: string = '';
  teacherCode: string = '';
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateLessonPlanComponent>,
    private notifyService: NotificationService) {

  }
  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.form = this.fb.group({
      'lessonPlanCode': [''],
      'branchCode': ['', Validators.required],
      'teacherCode': ['', Validators.required],
      'gradeCode': ['', Validators.required],
      'sectionCode': ['', Validators.required],
      'subCodes': ['', Validators.required],
      'estStartDate': ['', Validators.required],
      'estEndDate': ['', Validators.required],
      'numOfDays': ['', Validators.required],
      'numOfLessons': ['', Validators.required],
      'tableRows': this.fb.array([])
    });
    this.loadData();
    //this.filteredOptions = this.myControl.valueChanges.pipe(
    //  startWith(''),
    //  map(value => this._filter(value || '')),
    //);
    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.form.patchValue(this.row);
      this.editLessonInfoData();
    }
    this.isShown = true;
  }
  //toggleShow() {
  //  if (this.form.valid) {
  //    this.isShown = true;
  //    if (this.row) {

  //    } else {
  //      this.buttonstatus = true;
  //      this.addRow();
  //    }
  //  }
  //  else
  //    this.utilService.FillUpFields();
  //}
  initiateForm(): FormGroup {
    return this.fb.group({
      'chapter': ['', Validators.required],
      'lessonName': ['', Validators.required],
      'lessonName2': ['', Validators.required],
      'topics': ['', Validators.required],
      'topics2': ['', Validators.required],
      'numOfSessions': ['', Validators.required],
      'topicDate': ['', Validators.required],
      'startTime': ['', Validators.required],
      'endTime': ['', Validators.required]
    });
  }
  initiateEditForm(chapter: string, lessonName: string, lessonName2: string, topics: string,
    topics2: string, numOfSessions: string, topicDate: any, startTime: any, endTime: any): FormGroup {
    return this.fb.group({
      'chapter': [chapter, Validators.required],
      'lessonName': [lessonName, Validators.required],
      'lessonName2': [lessonName2, Validators.required],
      'topics': [topics, Validators.required],
      'topics2': [topics2, Validators.required],
      'numOfSessions': [numOfSessions, Validators.required],
      'topicDate': [topicDate, Validators.required],
      'startTime': [startTime, Validators.required],
      'endTime': [endTime, Validators.required]
    });
  }
  get getFormControls() {
    const control = this.form.get('tableRows') as FormArray;
    return control;
  }
  addRow() {
    if (this.form.valid) {
      let control = this.form.get('tableRows') as FormArray;
      control.push(this.initiateForm());
      this.buttonstatus = true;
    } else
      this.utilService.FillUpFields();
  }
  deleteRow(index: number) {
    const control = this.form.get('tableRows') as FormArray;
    control.removeAt(index);
    if (control.length == 0) {
      this.buttonstatus = false;
    }
  }
  //toggleShowsec() {
  //  this.isSecShown = !this.isSecShown;
  //}
  calcDays(event: any) {
    if (this.form.value['estStartDate'] != '' && this.form.value['estEndDate'] != '') {
      const estStartDate: Date = this.form.value['estStartDate'] as Date;
      const estEndDate: Date = this.form.value['estEndDate'] as Date;
      const diff = estEndDate.getTime() - estStartDate.getTime();
      this.form.patchValue({
        'numOfDays': Math.floor(diff / (1000 * 60 * 60 * 24)) + 1
      });
    }
  }
  loadData() {
    this.apiService.getall('schoolBranches/getSchoolBranchList').subscribe(res => {
      if (res)
        this.branchCodeList = res;
      this.teacherList = [];
      this.gradeCodeList = [];
      this.subCodesList = [];
    });
  }
  loadTeachers() {
    this.branchCode = this.form.value['branchCode'] as string;
    if (this.branchCode != null && this.branchCode != '') {
      this.apiService.getall(`TeacherMaster/GetTeachersByBranchcode/${this.branchCode}`).subscribe(res => {
        if (res)
          this.teacherList = res;
        this.gradeCodeList = [];
        this.subCodesList = [];
      });
    }
  }
  loadGrades() {
    this.teacherCode = this.form.value['teacherCode'] as string;
    if (this.teacherCode != null && this.teacherCode != '') {
      var index: number = this.teacherList.findIndex(a => a.teacherCode === this.teacherCode);
      let pItem = this.teacherList[index];
      this.teacherName = this.isArab ? pItem.teacherName2 : pItem.teacherName1
      this.apiService.getall(`TeacherMaster/GetBranchTeacherGrades/${this.teacherCode}`).subscribe(res => {
        if (res)
          this.gradeCodeList = res;
        this.subCodesList = [];
      });
    }
  }
  loadSectionSubjects() {
    this.sectionCode = this.form.value['sectionCode'] as string;
    this.gradeCode = this.form.value['gradeCode'] as string;
    this.teacherCode = this.form.value['teacherCode'] as string;
    if ((this.sectionCode === null || this.sectionCode === '')
      && this.gradeCode != null && this.gradeCode != '') {
      this.apiService.getall(`SchoolGradeSectionMapping/getAllSectionsByGradeCode/${this.gradeCode}`).subscribe(res => {
        if (res)
          this.sectionList = res;
      });
    }

    if (this.gradeCode != null && this.gradeCode != ''
      && this.sectionCode != null && this.sectionCode != ''
      && this.teacherCode != null && this.teacherCode != '') {
      this.apiService.getall(`TeacherMaster/GetTeacherGradeSubjects/${this.teacherCode}/${this.gradeCode}/${this.sectionCode}`).subscribe(res => {
        if (res)
          this.subCodesList = res;
      });
    }
  }
  //private _filter(value: string): string[] {
  //  const filterValue = value.toLowerCase();
  //  return this.teacherList.filter(option => option.toLowerCase().includes(filterValue));
  //}
  editLessonInfoData() {
    this.apiService.getall(`LessonPlan/GetLessonPlanInfoById/${this.id}`).subscribe(res => {
      if (res) {
        this.form.patchValue({
          'lessonPlanCode': res.lessonPlanCode,
          'branchCode': res.branchCode,
          'teacherCode': res.teacherCode,
          'gradeCode': res.gradeCode,
          'sectionCode': res.sectionCode,
          'subCodes': res.subCodes,
          'estStartDate': res.estStartDate,
          'estEndDate': res.estEndDate,
          'numOfDays': res.numOfDays,
          'numOfLessons': res.numOfLessons,
          'tableRows': res.tableRows
        });
        this.branchCode = res.branchCode;
        if (this.branchCode != null && this.branchCode != '') {
          this.apiService.getall(`TeacherMaster/GetTeachersByBranchcode/${this.branchCode}`).subscribe(res2 => {
            if (res2) {
              this.teacherList = res2;
              this.gradeCodeList = [];
              this.subCodesList = [];
              this.teacherCode = res.teacherCode;
              if (this.teacherCode != null && this.teacherCode != '') {
                var index: number = this.teacherList.findIndex(a => a.teacherCode === this.teacherCode);
                let pItem = this.teacherList[index];
                this.teacherName = this.isArab ? pItem.teacherName2 : pItem.teacherName1
                this.apiService.getall(`TeacherMaster/GetBranchTeacherGrades/${this.teacherCode}`).subscribe(res3 => {
                  if (res3) {
                    this.gradeCodeList = res3;
                    this.subCodesList = [];
                    this.gradeCode = res.gradeCode;
                    this.apiService.getall(`SchoolGradeSectionMapping/getAllSectionsByGradeCode/${this.gradeCode}`).subscribe(res4 => {
                      if (res4) {
                        this.sectionList = res4;
                        this.sectionCode = res.sectionCode;
                        this.apiService.getall(`TeacherMaster/GetTeacherGradeSubjects/${this.teacherCode}/${this.gradeCode}/${this.sectionCode}`).subscribe(res5 => {
                          if (res5)
                            this.subCodesList = res5;
                        });
                      }
                    });
                  }
                });
              }
            }
          });
        }
        let control = this.form.get('tableRows') as FormArray;
        for (var i = 0; i < res.tableRows.length; i++) {
          control.push(this.initiateEditForm(res.tableRows[i].chapter, res.tableRows[i].lessonName,
            res.tableRows[i].lessonName2, res.tableRows[i].topics, res.tableRows[i].topics2,
            res.tableRows[i].numOfSessions, res.tableRows[i].topicDate, res.tableRows[i].startTime,
            res.tableRows[i].endTime));
        }
        this.isShown = true;
        this.buttonstatus = true;
        this.form.patchValue({
          'teacherCode': res.teacherCode,
          'gradeCode': res.gradeCode,
          'sectionCode': res.sectionCode,
          'subCodes': res.subCodes
        });
      }
    });
  }
  submit() {
    if (this.form.valid) {
      this.form.value['id'] = this.id;
      this.form.value['estStartDate'] = this.utilService.selectedDate(this.form.controls['estStartDate'].value);
      this.form.value['estEndDate'] = this.utilService.selectedDate(this.form.controls['estEndDate'].value);
      const rows = this.form.get('tableRows') as FormArray;
      for (var i = 0; i < rows.controls.length; i++) {
        if (rows.controls[i].value['topicDate'] != null && rows.controls[i].value['topicDate'] != '')
          rows.controls[i].value['topicDate'] = this.utilService.selectedDate(rows.controls[i].value['topicDate']);
        else
          rows.controls[i].value['topicDate'] = null;
      }
      this.apiService.post('LessonPlan', this.form.value)
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

