import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';

@Component({
  selector: 'app-add-update-grade-document',
  templateUrl: './add-update-grade-document.component.html',
  styleUrls: []
})
export class AddUpdateGradeDocumentComponent implements OnInit {

  id: number = 0;
  row: any;
  form!: FormGroup;
  code: string = '';
  fileName: string = '';
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddUpdateGradeDocumentComponent>,
    private notifyService: NotificationService) {

  }

  ngOnInit(): void {
    this.form = this.fb.group({
      'uploadFileName': [''],
      'uploadFile': ['', Validators.required]
    });

    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.code = this.row['gradeCode'];
      this.fileName = this.row['fileName'];
      this.form.patchValue(this.row);
    }
  }
  onFileChanged(event: any) {
    let reader = new FileReader();
    if (event.target.files && event.target.files.length > 0) {
      let file = event.target.files[0];
      //if (file.type !== "image/png" &&
      //  this.selectedFile.type !== "image/jpg" &&
      //  this.selectedFile.type !== "image/jpeg" &&
      //  this.selectedFile.type !== "application/pdf")
      if (file.type !== "application/pdf") {
        this.notifyService.showError("File type must be pdf", "Error");
        this.form.patchValue({
          'uploadFile': ''
        });
        return;
      } else {
        reader.readAsDataURL(file);
        reader.onload = () => {
          this.form.patchValue({
            'uploadFile': file
          });
        };
      }      
    }
  }
  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      const formData = new FormData();
      formData.append("gradeCode", this.code);
      formData.append("uploadFileName", this.authService.ApiEndPoint().replace("api", "") + 'Signaturefiles/' + this.code+'/');
      formData.append("uploadFile", this.form.controls['uploadFile'].value);
      this.apiService.post('AcedemicClassGrade/UploadDocument', formData)
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
      this.notifyService.showError("File type must be pdf", "Error");
      //this.utilService.FillUpFields();
  }
  reset() {
    this.form.reset();
  }
  closeModel() {
    this.dialogRef.close();
  }
}
