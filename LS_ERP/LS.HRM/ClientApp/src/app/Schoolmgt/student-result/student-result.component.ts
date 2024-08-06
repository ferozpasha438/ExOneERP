import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { UtilityService } from '../../services/utility.service';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { ParentSchoolMgtComponent } from '../../sharedcomponent/parentschoolmgt.component';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
//import { ApiService } from '../../services/api.service';
import { DBOperation } from '../../services/utility.constants';
import { ValidationService } from '../../sharedcomponent/ValidationService';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-student-result',
  templateUrl: './student-result.component.html',
})
export class StudentResultComponent extends ParentSchoolMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort!: MatSort;
  displayedColumns: string[] = ['registrationNumber', 'studentName', 'studentArabicName', 'gradeSection', 'Actions'];
  data!: MatTableDataSource<any>;
  totalItemsCount!: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;
  id: number = 0;
  form!: FormGroup;
  isArab: boolean = false;
  typesOfExamsList: Array<any> = [];
  gradeCodeList: Array<any> = [];
  branchCodeList: Array<any> = [];
  headerData: Array<any> = [];
  isShowTable: boolean = false;
  examHeaderID: number = 0;
  branchCode: string = '';
  gradeCode: string = '';
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private translate: TranslateService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService) {
    super(authService);
  }
  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.form = this.fb.group({
      "examinationTypeCode": ['', Validators.required],
      "branchCode": ['', Validators.required],
      "gradeCode": ['', Validators.required],
      'studentExamResultData': this.fb.array([])
    });
    this.loading();
  }
  initiateForm(): FormGroup {
    return this.fb.group({
      'studentName': ['', Validators.required],
      'studentName2': ['', Validators.required],
      'stuAdmCode': ['', Validators.required],
      'totalMarks': ['', Validators.required],
      'remarks': [''],
      'studentResults': this.fb.array([])
    });
  }
  initiateResultForm(): FormGroup {
    return this.fb.group({
      'subCodes': ['', Validators.required],
      'subjectName': ['', Validators.required],
      'subjectName2': ['', Validators.required],
      'maximumMarks': [''],
      'qualifyingMarks': [''],
      'subjectMarks': ['', Validators.required],
      'qualifyingGrade': ['']
    });
  }
  initiateEditForm(studentName: string, studentName2: string, stuAdmCode: string, totalMarks: number, remarks: string, studentResults: Array<any>): FormGroup {
    return this.fb.group({
      'studentName': [studentName, Validators.required],
      'studentName2': [studentName2, Validators.required],
      'stuAdmCode': [stuAdmCode, Validators.required],
      'totalMarks': [totalMarks, Validators.required],
      'remarks': [remarks],
      'studentResults': [studentResults]
    });
  }
  getFormControls() {
    return this.form.get('studentExamResultData') as FormArray;
  }
  studentSubjectMarksData(index: number): FormArray {
    return this.getFormControls().at(index).get('studentResults') as FormArray;
  }
  changeMarks(event: any, mainIndex: number, subIndex: number) {
    var totalMarks = 0;
    const rows = this.form.get('studentExamResultData') as FormArray;
    if (rows.controls.length > 0) {
      const rowData = rows.controls[mainIndex].value;
      if (rowData != null && rowData != '') {
        const subRows = rowData.studentResults;
        subRows[subIndex]['subjectMarks'] = event.target.value;
        for (let i = 0; i < subRows.length; i++) {
          totalMarks = totalMarks + parseFloat(subRows[i]['subjectMarks']);
        }
        rows.controls[mainIndex].value['totalMarks'] = totalMarks;
      }
    }
  }
  changeRemarks(event: any, mainIndex: number, subIndex: number) {
    const rows = this.form.get('studentExamResultData') as FormArray;
    if (rows.controls.length > 0) {
      rows.controls[mainIndex].value['remarks'] = event.target.value;
    }
  }
  loading() {
    this.apiService.getall('ExaminationManagement/GetAllExamTypesList').subscribe(res => {
      this.typesOfExamsList = res;
    });
    this.apiService.getall('schoolBranches/getSchoolBranchList').subscribe(res => {
      this.branchCodeList = res;
    });
    this.apiService.getPagination('acedemicClassGrade', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res)
        this.gradeCodeList = res['items'];
    });
  }
  submit() {
    if (this.form.valid) {
      this.branchCode = this.form.value['branchCode'] as string;
      this.gradeCode = this.form.value['gradeCode'] as string;
      this.form.value['ExamHeaderID'] = this.examHeaderID;
      this.apiService.post('ExamResult', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          //this.loading();
          let control = this.form.get('studentExamResultData') as FormArray;
          const controllength: number = control.controls.length;
          for (var i = controllength - 1; i >= 0; i--) {
            control.removeAt(i);
          }
          this.loadResultList();
        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
          });

    }
    else
      this.utilService.FillUpFields();
  }
  loadResultList() {
    const examinationTypeCode: string = this.form.value['examinationTypeCode'] as string;
    const branchCode: string = this.form.value['branchCode'] as string;
    const gradeCode: string = this.form.value['gradeCode'] as string;
    if (branchCode != null && branchCode != '' &&
      gradeCode != null && gradeCode != '' &&
      examinationTypeCode != null && examinationTypeCode != '') {
      this.apiService.getall(`ExamResult/StudentResultList/${branchCode}/${gradeCode}/${examinationTypeCode}`).subscribe(res => {
        if (res) {
          this.examHeaderID = res.examHeaderID;
          if (res.studentExamResultData != null) {
            let control = this.form.get('studentExamResultData') as FormArray;
            const controllength: number = control.controls.length;
            for (var i = controllength - 1; i >= 0; i--) {
              control.removeAt(i);
            }
            control = this.form.get('studentExamResultData') as FormArray;
            for (var i = 0; i < res.studentExamResultData.length; i++) {
              control.push(this.initiateEditForm(res.studentExamResultData[i].studentName, res.studentExamResultData[i].studentName2, res.studentExamResultData[i].stuAdmCode, res.studentExamResultData[i].totalMarks, res.studentExamResultData[i].remarks, res.studentExamResultData[i].studentResults));
            }
            if (res.studentExamResultData.length > 0) {
              this.headerData = res.studentExamResultData[0].studentResults;
              this.isShowTable = true;
            } else {
              this.isShowTable = false;
            }
          } else {
            let control = this.form.get('studentExamResultData') as FormArray;
            const controllength: number = control.controls.length;
            for (var i = controllength - 1; i >= 0; i--) {
              control.removeAt(i);
            }
            this.isShowTable = false;
          }
        }
      });
    }
  }

  reset() {
    this.form.reset();
  }

}

