import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
@Component({
  selector: 'app-addupdate-student-registration',
  templateUrl: './addupdate-student-registration.component.html',
  styleUrls: []
})
export class AddupdateStudentRegistrationComponent implements OnInit {
  form!: FormGroup;
  constructor(public dialogRef: MatDialogRef<AddupdateStudentRegistrationComponent>,private fb: FormBuilder) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      'registrationNumber': ['', Validators.required],
      'date': ['', Validators.required],
      'studentName': ['', Validators.required],
      'studentNameInArabic': ['', Validators.required],
      'dateOfBirth': ['', Validators.required],
      'age': ['', Validators.required],
      'gradeSeekingAdmissionIn': ['', Validators.required],
      'mediumOnLanguage': ['', Validators.required],
      'genderCode': ['', Validators.required],
      'identityCardNumber': ['', Validators.required],
      'nationality': ['', Validators.required],
      'religion': ['', Validators.required],
      'physicallyChallenged': ['', Validators.required],    
      'mentionedPhysicalChallenge': ['', Validators.required],
      'mentionedMedicalIssue': ['', Validators.required],
      'anyMedicalIssues': ['', Validators.required],

      'fatherName': ['', Validators.required],
      'motherName': ['', Validators.required],
      'registeredMobile': ['', Validators.required],
      'registeredEmail': ['', Validators.required],
      'city': ['', Validators.required],
      'remarksNotes': ['', Validators.required],
    });
  }
  closeModel() {
    this.dialogRef.close();
  }
  submit() {}

}
