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
  selector: 'app-student-register',
  templateUrl: './student-register.component.html',
})
export class StudentRegisterComponent extends ParentSchoolMgtComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort!: MatSort;
  data!: MatTableDataSource<any>;
  totalItemsCount!: number;
  searchValue: string = '';
  sortingOrder: string = 'id desc';
  isLoading: boolean = false;

  form!: FormGroup;
  isArab: boolean = false;
  gradeCodeList: Array<any> = [];
  branchCodeList: Array<any> = [];
  sectionList: Array<any> = [];
  isShowTable: boolean = false;
  isShowAttendanceClose: boolean = false;

  id: number = 0;
  attnDate: string = '';
  teacherCode: string = 'TEACH222201210';
  isOpen: boolean = true;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private translate: TranslateService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService) {
    super(authService);
  }
  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.form = this.fb.group({
      "branchCode": ['', Validators.required],
      "gradeCode": ['', Validators.required],
      "sectionCode": ['', Validators.required],
      'studentAttendanceDataList': this.fb.array([])
    });
    this.loading();
  }
  initiateForm(): FormGroup {
    return this.fb.group({
      'studentName': ['', Validators.required],
      'studentName2': ['', Validators.required],
      'studentAdmNumber': ['', Validators.required],
      'inTime': ['', Validators.required],
      'outTime': ['', Validators.required],
      'attnFlag': ['', Validators.required],
      'isPresent': [''],
      'isLeave': [''],
      'remarks': ['']
    });
  }
  initiateEditForm(studentName: string, studentName2: string, studentAdmNumber: string, inTime: any, outTime: any, attnFlag: any, isPresent: boolean, isLeave: boolean, remarks: string): FormGroup {
    return this.fb.group({
      'studentName': [studentName, Validators.required],
      'studentName2': [studentName2, Validators.required],
      'studentAdmNumber': [studentAdmNumber, Validators.required],
      'inTime': [inTime],
      'outTime': [outTime],
      'attnFlag': [attnFlag, Validators.required],
      'isPresent': [isPresent],
      'isLeave': [isLeave],
      'remarks': [remarks]
    });
  }
  getFormControls() {
    const control = this.form.get('studentAttendanceDataList') as FormArray;
    return control;
  }
  changeRegisterData(event: any, mainIndex: number) {
    if (event.target.value.toString().toUpperCase() === "P" || event.target.value.toString().toUpperCase() === "A") {
      const rows = this.form.get('studentAttendanceDataList') as FormArray;
      if (rows.controls.length > 0) {
        const rowData = rows.controls[mainIndex].value;
        if (rowData != null && rowData != '') {
          rowData['attnFlag'] = event.target.value.toString().toUpperCase();
          event.target.value = event.target.value.toString().toUpperCase();
        }
      }
    } else {
      const rows = this.form.get('studentAttendanceDataList') as FormArray;
      if (rows.controls.length > 0) {
        const rowData = rows.controls[mainIndex].value;
        if (rowData != null && rowData != '') {
          event.target.value = rowData['attnFlag'];
        }
      }
    }
  }
  changeRemarks(event: any, mainIndex: number) {
    const rows = this.form.get('studentAttendanceDataList') as FormArray;
    if (rows.controls.length > 0) {
      const rowData = rows.controls[mainIndex].value;
      if (rowData != null && rowData != '') {
        rowData['remarks'] = event.target.value;
      }
    }
  }
  loading() {
    this.apiService.getall('schoolBranches/getSchoolBranchList').subscribe(res => {
      this.branchCodeList = res;
    });
    this.apiService.getPagination('acedemicClassGrade', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res)
        this.gradeCodeList = res['items'];
    });
  }
  closeAttendance() {
    if (this.form.valid) {
      this.form.value['id'] = this.id;
      this.form.value['attnDate'] = this.attnDate;
      this.form.value['teacherCode'] = this.teacherCode;
      this.form.value['isOpen'] = this.isOpen;
      this.apiService.post('StudentAttendanceRegister/CloseAttendance', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          let control = this.form.get('studentAttendanceDataList') as FormArray;
          const controllength: number = control.controls.length;
          for (var i = controllength - 1; i >= 0; i--) {
            control.removeAt(i);
          }
          this.loadTodayStudentRegisterData(0);
        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
          });

    }
    else
      this.utilService.FillUpFields();
  }
  submit() {
    if (this.form.valid) {
      this.form.value['id'] = this.id;
      this.form.value['attnDate'] = this.attnDate;
      this.form.value['teacherCode'] = this.teacherCode;
      this.form.value['isOpen'] = this.isOpen;
      this.apiService.post('StudentAttendanceRegister', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          let control = this.form.get('studentAttendanceDataList') as FormArray;
          const controllength: number = control.controls.length;
          for (var i = controllength - 1; i >= 0; i--) {
            control.removeAt(i);
          }
          this.loadTodayStudentRegisterData(0);
        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
          });

    }
    else
      this.utilService.FillUpFields();
  }
  loadTodayStudentRegisterData(type: any) {
    const branchCode: string = this.form.value['branchCode'] as string;
    const gradeCode: string = this.form.value['gradeCode'] as string;
    const sectionCode: string = this.form.value['sectionCode'] as string;
    if (type === 2) {
      this.apiService.getall(`SchoolGradeSectionMapping/getAllSectionsByGradeCode/${gradeCode}`).subscribe(res => {
        if (res)
          this.sectionList = res;
      });
    }
    if (branchCode != null && branchCode != '' &&
      gradeCode != null && gradeCode != '' &&
      sectionCode != null && sectionCode != '') {
      this.apiService.getall(`StudentAttendanceRegister/StudentAttendanceRegisterList/${branchCode}/${gradeCode}/${sectionCode}`).subscribe(res => {
        if (res) {
          this.id = res.id;
          this.attnDate = res.attnDate;
          this.isOpen = res.isOpen;
          if (res.id > 0 && res.isOpen === true) {
            this.isShowAttendanceClose = true;
          }
          if (res.studentAttendanceDataList != null) {
            let control = this.form.get('studentAttendanceDataList') as FormArray;
            const controllength: number = control.controls.length;
            for (var i = controllength - 1; i >= 0; i--) {
              control.removeAt(i);
            }
            control = this.form.get('studentAttendanceDataList') as FormArray;
            for (var i = 0; i < res.studentAttendanceDataList.length; i++) {
              control.push(this.initiateEditForm(res.studentAttendanceDataList[i].studentName,
                res.studentAttendanceDataList[i].studentName2,
                res.studentAttendanceDataList[i].studentAdmNumber,
                res.studentAttendanceDataList[i].inTime,
                res.studentAttendanceDataList[i].outTime,
                res.studentAttendanceDataList[i].attnFlag,
                res.studentAttendanceDataList[i].isPresent,
                res.studentAttendanceDataList[i].isLeave,
                res.studentAttendanceDataList[i].remarks));
            }
          } else {
            let control = this.form.get('studentAttendanceDataList') as FormArray;
            const controllength: number = control.controls.length;
            for (var i = controllength - 1; i >= 0; i--) {
              control.removeAt(i);
            }
          }
          if (res.studentAttendanceDataList.length > 0)
            this.isShowTable = true;
        }
      });
    }
  }

  reset() {
    this.form.reset();
  }

}

