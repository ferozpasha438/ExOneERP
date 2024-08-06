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
import { formatDate } from '@angular/common';
@Component({
  selector: 'app-addupdate-examination-management',
  templateUrl: './addupdate-examination-management.component.html',
})
export class AddupdateExaminationManagementComponent implements OnInit {
  id: number = 0;
  row: any;
  form!: FormGroup;
  isArab: boolean = false;
  myControl = new FormControl('');
  options: string[] = ['One', 'Two', 'Three'];
  filteredOptions!: Observable<string[]>;
  isShown: boolean = false;
  gradeCodeList: Array<any> = [];
  branchCodeList: Array<any> = [];
  typesOfExamsList: Array<any> = [];
  usersList: Array<any> = [];
  isShowCompleteSection: boolean = false;
  gradeSubjectList: Array<any> = [];
  buttonstatus = false;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateExaminationManagementComponent>,
    private notifyService: NotificationService) {

  }
  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.form = this.fb.group({
      'branchCode': ['', Validators.required],
      'gradeCode': ['', Validators.required],
      'typeofExamination': ['', Validators.required],
      'startingDate': ['', Validators.required],
      'endingDate': ['', Validators.required],
      'remarks': [''],
      'preparedBy': ['', Validators.required],
      'dateOfCompletion': [''],
      'dateofResult': [''],
      'isCompleted': [false],
      'isResultDeclared': [false],
      'tableRows': this.fb.array([])
    });

    this.filteredOptions = this.myControl.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value || '')),
    );
    this.loadData();
    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.form.patchValue(this.row);
      this.editExaminationData();
    }
  }
  initiateForm(code: string): FormGroup {
    return this.fb.group({
      'subjectCode': [code, Validators.required],
      'startingDateTime': ['', Validators.required],
      'endingDateTime': ['', Validators.required]
    });
  }
  initiateEditForm(code: string,sdate:string,tdate:string): FormGroup {
    return this.fb.group({
      'subjectCode': [code, Validators.required],
      'startingDateTime': [sdate, Validators.required],
      'endingDateTime': [tdate, Validators.required]
    });
  }
  get getFormControls() {
    const control = this.form.get('tableRows') as FormArray;
    return control;
  }
  //addRow() {
  //  let control = this.form.get('tableRows') as FormArray;
  //  control.push(this.initiateForm());
  //  this.buttonstatus = true;
  //}
  //deleteRow(index: number) {
  //  const control = this.form.get('tableRows') as FormArray;
  //  control.removeAt(index);
  //  if (control.length == 0) {
  //    this.buttonstatus = false;
  //  }
  //}
  loadData() {
    this.apiService.getall('schoolBranches/getSchoolBranchList').subscribe(res => {
      this.branchCodeList = res;
    });
    this.apiService.getPagination('acedemicClassGrade', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res)
        this.gradeCodeList = res['items'];
    });
    this.apiService.getall('ExaminationManagement/GetAllExamTypesList').subscribe(res => {
      this.typesOfExamsList = res;
    });
  }
  loadUsersList() {
    const branchCode: string = this.form.value['branchCode'] as string;
    const gradeCode: string = this.form.value['gradeCode'] as string;
    if (branchCode != null && branchCode != '' && gradeCode != null && gradeCode != '') {
      this.apiService.getall(`ExaminationManagement/GetAllUsersList/${gradeCode}/${branchCode}`).subscribe(res => {
        this.usersList = res;
      });
    }
    if (gradeCode != null && gradeCode != '') {
      this.apiService.getall(`AcademicsSubjects/GetAllGradeSubjectList/${gradeCode}`).subscribe(res => {
        if (res) {
          this.gradeSubjectList = res;
          if (this.row) {

          } else {
            let control = this.form.get('tableRows') as FormArray;
            for (var i = 0; i < res.length; i++) {
              control.push(this.initiateForm(res[i].value));
            }
            if (res.length > 0) {
              this.isShown = true;
              this.buttonstatus = true;
            }
          }
        }
      });
    }
  }
  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();
    return this.options.filter(option => option.toLowerCase().includes(filterValue));
  }

  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      this.form.value['startingDate'] = this.utilService.selectedDate(this.form.controls['startingDate'].value);
      this.form.value['endingDate'] = this.utilService.selectedDate(this.form.controls['endingDate'].value);
      if (this.form.controls['dateOfCompletion'].value != null && this.form.controls['dateOfCompletion'].value != '')
        this.form.value['dateOfCompletion'] = this.utilService.selectedDate(this.form.controls['dateOfCompletion'].value);
      else
        this.form.value['dateOfCompletion'] = null;
      if (this.form.controls['dateofResult'].value != null && this.form.controls['dateofResult'].value != '')
        this.form.value['dateofResult'] = this.utilService.selectedDate(this.form.controls['dateofResult'].value);
      else
        this.form.value['dateofResult'] = null;
      //const rows = this.form.get('tableRows') as FormArray;
      //for (var i = 0; i < rows.controls.length; i++) {
      //  if (rows.controls[i].value['startingDateTime'] != null && rows.controls[i].value['startingDateTime'] != '')
      //    rows.controls[i].value['startingDateTime'] = this.utilService.selectedDate(rows.controls[i].value['startingDateTime']);
      //  else
      //    rows.controls[i].value['startingDateTime'] = null;
      //  if (rows.controls[i].value['endingDateTime'] != null && rows.controls[i].value['endingDateTime'] != '')
      //    rows.controls[i].value['endingDateTime'] = this.utilService.selectedDate(rows.controls[i].value['endingDateTime']);
      //  else
      //    rows.controls[i].value['endingDateTime'] = null;
      //}
      this.apiService.post('ExaminationManagement', this.form.value)
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
  editExaminationData() {
    this.apiService.getall(`ExaminationManagement/${this.id}`).subscribe(res => {
      if (res) {
        this.form.patchValue({
          'branchCode': res.branchCode,
          'dateOfCompletion': res.dateOfCompletion,
          'dateofResult': res.dateofResult,
          'endingDate': res.endingDate,
          'gradeCode': res.gradeCode,
          'isCompleted': res.isCompleted,
          'isResultDeclared': res.isResultDeclared,
          'preparedBy': res.preparedBy,
          'remarks': res.remarks,
          'startingDate': res.startingDate,
          'typeofExamination': res.typeofExamination,
          'tableRows': res.tableRows
        });
        let date1 = formatDate(new Date(), 'yyyy-MM-dd', 'en_US');
        let date2 = formatDate(res.endingDate, 'yyyy-MM-dd', 'en_US');
        if (date1 > date2) {
          this.isShowCompleteSection = true;
        }
        let control = this.form.get('tableRows') as FormArray;
        for (var i = 0; i < res.tableRows.length; i++) {
          control.push(this.initiateEditForm(res.tableRows[i].subjectCode, res.tableRows[i].startingDateTime, res.tableRows[i].endingDateTime));
        }
        this.loadUsersList();
        this.form.patchValue({
          'preparedBy': res.preparedBy
        });
        this.isShown = true;
        this.buttonstatus = true;
      }
    });
  }
  reset() {
    this.form.reset();
  }

  closeModel() {
    this.dialogRef.close();
  }

}


