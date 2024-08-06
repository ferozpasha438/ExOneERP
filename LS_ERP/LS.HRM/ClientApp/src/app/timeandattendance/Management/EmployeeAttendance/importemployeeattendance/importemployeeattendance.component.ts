import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from 'src/app/api-authorization/AuthorizeService';
import { ApiService } from 'src/app/services/api.service';
import { UtilityService } from 'src/app/services/utility.service';
import { ParenttnamgtComponent } from 'src/app/sharedcomponent/parenttnamgt.component';

@Component({
  selector: 'app-importemployeeattendance',
  templateUrl: './importemployeeattendance.component.html',
  styles: [],
})
export class ImportemployeeattendanceComponent
  extends ParenttnamgtComponent
  implements OnInit
{
  modalTitle!: string;
  modalBtnTitle!: string;
  form!: FormGroup;
  formData!: FormData;
  files: Array<File> = [];

  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private authService: AuthorizeService,
    private utilService: UtilityService,
    public dialogRef: MatDialogRef<ImportemployeeattendanceComponent>
  ) {
    super(authService);
  }

  ngOnInit(): void {
    this.setForm();
  }

  setForm() {
    this.form = this.fb.group({
      files: [''],
    });
  }

  onFileChanged(event: any) {
    let reader = new FileReader();
    if (event.target.files && event.target.files.length > 0) {
      for (let i = 0; i < event.target.files.length; i++) {
        this.files.push(event.target.files[i]);
      }
    }
  }

  closeModel() {
    this.dialogRef.close();
  }

  submit() {
    if (this.form.valid) {
      this.formData = new FormData();
      if (this.files.length > 0) {
        for (let i = 0; i < this.files.length; i++) {
          this.formData.append('file', this.files[i], this.files[i].name);
        }
        this.apiService
          .post('EmployeeAttendance/ImportEmployeeAttendance', this.formData)
          .subscribe(
            (res) => {
              this.utilService.OkMessage();
              this.reset();
              this.dialogRef.close(true);
            },
            (error) => {
              this.utilService.ShowApiErrorMessage(error);
            }
          );
      }
    } else this.utilService.FillUpFields();
  }

  reset() {
    this.form.controls['files'].setValue('');
  }
}
