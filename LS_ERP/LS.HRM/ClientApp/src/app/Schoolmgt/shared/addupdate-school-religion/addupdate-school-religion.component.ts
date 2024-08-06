import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
@Component({
  selector: 'app-addupdate-school-religion',
  templateUrl: './addupdate-school-religion.component.html',
  styleUrls: []
})
export class AddupdateSchoolReligionComponent implements OnInit {
  form!: FormGroup;
  constructor(public dialogRef: MatDialogRef<AddupdateSchoolReligionComponent>,private fb: FormBuilder) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      'regCode': ['', Validators.required],
      'regName': ['', Validators.required],
      'regName2': ['', Validators.required],
      'isActive': [false, Validators.required]
    });
  }
  closeModel() {
    this.dialogRef.close();
  }
  submit() {}
}
