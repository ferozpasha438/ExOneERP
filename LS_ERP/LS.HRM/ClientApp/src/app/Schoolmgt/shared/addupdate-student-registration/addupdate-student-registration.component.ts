import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { UtilityService } from '../../../services/utility.service';
import { ApiService } from '../../../services/api.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-addupdate-student-registration',
  templateUrl: './addupdate-student-registration.component.html',
  styleUrls: []
})
export class AddupdateStudentRegistrationComponent implements OnInit {
  id: number = 0;
  genderList: Array<any> = [];
  gradeCodeList: Array<any> = [];
  languageList: Array<any> = [];
  nationalityList: Array<any> = [];
  religionList: Array<any> = [];
  isLoading: boolean = false;
  form!: FormGroup;
  constructor(private fb: FormBuilder, private utilService: UtilityService, private apiService: ApiService,
    private validationService: ValidationService, public dialogRef: MatDialogRef<AddupdateStudentRegistrationComponent>) { }

  ngOnInit(): void {
    this.loadData();
    this.form = this.fb.group({
      'regNum': ['', Validators.required],
      'regDate': ['', Validators.required],
      'fullName': ['', Validators.required],
      'nameAr': ['', Validators.required],
      'dateOfBirth': ['', Validators.required],
      'age': ['', Validators.required],
      'grade': ['', Validators.required],
      'langCode': ['', Validators.required],
      'genderName': ['', Validators.required],
      'idNumber': ['', Validators.required],
      'nationality': ['', Validators.required],
      'religionCode': ['', Validators.required],
      'physicalDisability': [false],
      'physicalDisabilityNotes': [''],
      'medicalIssue': [false],
      'medicalIssueNotes': [''],
      'fatherName': ['', Validators.required],
      'motherName': ['', Validators.required],
      'fatherPhoneNumber': ['', Validators.compose([Validators.required, this.validationService.mobileValidator])],
      'fatherEmail': ['', Validators.compose([Validators.required, Validators.email])],
      'city': ['', Validators.required],
      'remarks': [''],
    });

    
    if (this.id > 0) {            
      this.editStudentMastrData();
    }

  }

  loadData() {
    this.isLoading = true;
    const genderList$ = this.apiService.getPagination('schoolGender', this.utilService.getQueryString(0, 1000, '', ''));
    const gradeCodeList$ = this.apiService.getPagination('acedemicClassGrade', this.utilService.getQueryString(0, 1000, '', ''));
    const languageList$ = this.apiService.getPagination('schoolLanguages', this.utilService.getQueryString(0, 1000, '', ''));
    const nationalityList$ = this.apiService.getPagination('schoolNational', this.utilService.getQueryString(0, 1000, '', ''));
    const religionList$ = this.apiService.getPagination('schoolReligion', this.utilService.getQueryString(0, 1000, '', ''));

    forkJoin({
      genderList: genderList$,
      gradeCodeList: gradeCodeList$,
      languageList: languageList$,
      nationalityList: nationalityList$,
      religionList: religionList$,
    }).subscribe(result => {
      this.gradeCodeList = result.gradeCodeList['items'];;
      this.languageList = result.languageList['items'];;
      this.genderList = result.genderList['items'];;
      this.nationalityList = result.nationalityList['items'];;
      this.religionList = result.religionList['items'];;
      this.isLoading = false;
    });
  }

  //loadData0() {
  //  this.apiService.getPagination('schoolGender', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
  //    if (res)
  //      this.genderList = res['items'];
  //  });    
  //  this.apiService.getPagination('acedemicClassGrade', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
  //    if (res)
  //      this.gradeCodeList = res['items'];
  //  });
    
  //  this.apiService.getPagination('schoolLanguages', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
  //    if (res)
  //      this.languageList = res['items'];
  //  });
  //  this.apiService.getPagination('schoolNational', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
  //    if (res)
  //      this.nationalityList = res['items'];
  //  });
  //  this.apiService.getPagination('schoolReligion', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
  //    if (res)
  //      this.religionList = res['items'];
  //  });    
  //}

  calcAge(event: any) {
    const dateofBirth: Date = this.form.value['dateOfBirth'] as Date;
    const currentDate = new Date();
    const diff = currentDate.getTime() - dateofBirth.getTime();
    this.form.patchValue({
      'age': Math.floor(diff / (1000 * 60 * 60 * 24 * 365))
    });
  }
  editStudentMastrData() {
    this.apiService.getall(`WebStudentRegistration/GetSchoolStudentRegistrationById/${this.id}`).subscribe(res => {
      if (res) {
        this.form.patchValue(res);
      }
    });
  }

  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;

      this.form.value['regDate'] = this.utilService.selectedDate(this.form.controls['regDate'].value);
      this.form.value['dateOfBirth'] = this.utilService.selectedDate(this.form.controls['dateOfBirth'].value);

      this.apiService.post('WebStudentRegistration', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
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

  closeModel() {
    this.dialogRef.close();
  }
}
