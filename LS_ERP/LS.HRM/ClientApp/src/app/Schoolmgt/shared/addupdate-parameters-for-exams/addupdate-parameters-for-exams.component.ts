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
  selector: 'app-addupdate-parameters-for-exams',
  templateUrl: './addupdate-parameters-for-exams.component.html',

})
export class AddupdateParametersForExamsComponent implements OnInit {
  id: number = 0;
  row: any;
  form!: FormGroup;
  form2!: FormGroup;
  myControl = new FormControl('');
  options: string[] = ['One', 'Two', 'Three'];
  filteredOptions!: Observable<string[]>;
  isShown: boolean = false;
  isSecShown: boolean = false;
  isArab: boolean = false;
  gradeCode: string = '';
  gradeSubjectList: Array<any> = [];
  configRowsList: Array<any> = [];
  buttonstatus = false;
  subjectCode: string = '';
  subjectName: string = '';
  rid: number = -1;
  formArray!: FormArray;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateParametersForExamsComponent>,
    private notifyService: NotificationService) {

  }
  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.gradeCode = this.row['gradeCode'];
    }
    this.form = this.fb.group({
      'gradeCode': [this.gradeCode, Validators.required],
      'academicYear': ['', Validators.required],
      'isGradeRequired': [false],
      'noOfGrades': ['', Validators.required],
      'tableRows': this.fb.array([])
    });
    this.form2 = this.fb.group({
      'configTempTableRows': this.fb.array([])
    });
    

    this.filteredOptions = this.myControl.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value || '')),
    );
    this.loadData();
    if (this.row) {
      this.id = this.row.id;
      this.form.patchValue(this.row);
    }
  }
  initiateTempTableForm(): FormGroup {
    return this.fb.group({
      'maximumMarks': ['', Validators.required],
      'minimumMarks': ['', Validators.required],
      'qualifiyingGrade': ['', Validators.required]
    });
  }
  initiateForm(code: string): FormGroup {
    return this.fb.group({
      'subCodes': [code, Validators.required],
      'maximumMarks': ['', Validators.required],
      'qualifyingMarks': ['', Validators.required],
      'configRows': this.fb.array([])
    });
  }
  initiateEditForm(subCodes: string, maximumMarks: string, qualifyingMarks: string, configRows: Array<any>): FormGroup {
    return this.fb.group({
      'subCodes': [subCodes, Validators.required],
      'maximumMarks': [maximumMarks, Validators.required],
      'qualifyingMarks': [qualifyingMarks, Validators.required],
      'configRows': [configRows]
    });
  }
  get getFormControls() {
    const control = this.form.get('tableRows') as FormArray;
    return control;
  }
  get getTempTableFormControls() {
    const control = this.form2.get('configTempTableRows') as FormArray;
    return control;
  }
  initiateConfigForm(): FormGroup {
    return this.fb.group({
      'maximumMarks': ['', Validators.required],
      'minimumMarks': ['', Validators.required],
      'qualifiyingGrade': ['', Validators.required]
    });
  }
  initiateConfigEditForm(maximumMarks: string, minimumMarks: string, qualifiyingGrade: string): FormGroup {
    return this.fb.group({
      'maximumMarks': [maximumMarks, Validators.required],
      'minimumMarks': [minimumMarks, Validators.required],
      'qualifiyingGrade': [qualifiyingGrade, Validators.required]
    });
  }
  loadData() {
    this.apiService.getall('AcedemicClassGrade/GetAcademicYear').subscribe(res => {
      this.form.patchValue({
        'academicYear': res
      });
    });
    if (this.gradeCode != null && this.gradeCode != '') {
      this.apiService.getall(`AcademicsSubjects/GetAllGradeSubjectList/${this.gradeCode}`).subscribe(res => {
        if (res) {
          this.gradeSubjectList = res;
          let control = this.form.get('tableRows') as FormArray;
          for (var i = 0; i < res.length; i++) {
            control.push(this.initiateForm(res[i].value));
          }
          if (res.length > 0) {
            this.isShown = true;
            this.buttonstatus = true;
            this.editParametersForExamsData();
          }
        }
      });
    }
  }
  addGrades() {
    const rows = this.form.get('tableRows') as FormArray;
    if (rows.controls.length > 0 && this.rid > -1) {
      var configTempTableRows=this.form2.get('configTempTableRows') as FormArray;
      rows.controls[this.rid].value['configRows'] = configTempTableRows.value;
      for (var i = configTempTableRows.length - 1; i >= 0; i--) {
        configTempTableRows.removeAt(i);
      }
    }
    this.rid = -1;
    this.isSecShown = false;
  }
  
  toggleShow() {
    this.isShown = !this.isShown;
  }
  toggleShowsec(rowid: number) {
    const rows = this.form.get('tableRows') as FormArray;
    const noOfGrades = this.form.controls['noOfGrades'].value as number;
    if (rows.controls.length > 0 && noOfGrades > 0) {
      var tempConfigTempTableRows = this.form2.get('configTempTableRows') as FormArray;
      for (var i = tempConfigTempTableRows.length-1; i >= 0; i--) {
        tempConfigTempTableRows.removeAt(i);
      }
      this.rid = rowid;
      this.isSecShown = true;
      this.subjectCode = rows.controls[rowid].value['subCodes'];
      var index: number = this.gradeSubjectList.findIndex(a => a.value === this.subjectCode);
      this.subjectName = this.isArab ? this.gradeSubjectList[index].textTwo : this.gradeSubjectList[index].text;
      var configTempTableRows = this.form2.get('configTempTableRows') as FormArray;
      var actualConfigControl=rows.controls[rowid].value['configRows'] as FormArray;
      for (var i = 0; i < noOfGrades; i++) {
        if (actualConfigControl.length>i) {
          configTempTableRows.push(this.fb.group(rows.controls[rowid].value['configRows'][i]));
        } else {
          configTempTableRows.push(this.initiateConfigForm());
        }
      }
    }
    else
      this.utilService.FillUpFields();
  }
  editParametersForExamsData() {
    this.apiService.getall(`ParametersForExams/${this.gradeCode}`).subscribe(res => {
      if (res) {
        this.form.patchValue({
          'isGradeRequired': res.isGradeRequired,
          'noOfGrades': res.noOfGrades
        });
        const rows = this.form.get('tableRows') as FormArray;
        this.subjectCode = '';
        this.configRowsList = [];
        for (var i = 0; i < rows.length; i++) {
          this.configRowsList = [];
          this.subjectCode = rows.controls[i].value['subCodes'];
          var tableRowsData = res.tableRows as Array<any>;
          var index: number = tableRowsData.findIndex(x => x.subCodes === this.subjectCode);
          if (index >= 0) {
            rows.controls[i].patchValue({
              'maximumMarks': tableRowsData[index].maximumMarks,
              'qualifyingMarks': tableRowsData[index].qualifyingMarks,
              'configRows': tableRowsData[index].configRows
            });
            rows.controls[i].value['configRows'] = tableRowsData[index].configRows;
          }
        }
        this.subjectCode = '';
        this.configRowsList = [];
      }
    });
  }
  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();
    return this.options.filter(option => option.toLowerCase().includes(filterValue));
  }
  submit() {
    if (this.form.valid) {
      this.apiService.post('ParametersForExams/CreateParametersForExams', this.form.value)
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

