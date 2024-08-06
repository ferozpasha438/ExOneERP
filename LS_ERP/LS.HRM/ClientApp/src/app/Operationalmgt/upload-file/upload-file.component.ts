import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';
import { ParentOptMgtComponent } from '../../sharedcomponent/parentoptmgt.component';
import { ValidationService } from '../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-upload-file',
  templateUrl: './upload-file.component.html'
})
export class UploadFileComponent extends ParentOptMgtComponent implements OnInit {
  projectSite: any;
  project: any;


  requestData: any;
  filePath: string="";
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<UploadFileComponent>) {
    super(authService);
  }

  ngOnInit(): void {
    if (this.projectSite != null) {
      this.requestData = this.projectSite;
      this.filePath = this.authService.ApiEndPoint().replace("api", "") + 'Uploads/ContractForms/';
    }
    else if (this.project!=null) {
      this.requestData = this.project;
      this.requestData.siteCode = "";
      this.filePath = this.authService.ApiEndPoint().replace("api", "") + 'Uploads/ContractForms/';
    }
  }
  submit() {

  }
  closeModel() {
    this.dialogRef.close();
  }


  onFileChanged(event: any, type: number) {
    let reader = new FileReader();
    if (event.target.files && event.target.files.length > 0) {
      let file = event.target.files[0];
      reader.readAsDataURL(file);
      reader.onload = () => {
        if (type === 1) {
          this.requestData.fileIForm = file,
            this.requestData.fileName = reader.result

        }
      };
    }
  }


  uploadFile() {

    

    if (this.requestData.fileIForm != null) {
      const formData = new FormData();
      formData.append("id", this.requestData.id);
      formData.append("fileIForm", this.requestData.fileIForm);
      formData.append("fileName", this.filePath);
      formData.append("isAdendum", this.requestData.isAdendum??false);


      this.apiService.post('projectSites/uploadContractForm', formData).subscribe(res => {
        if (res) {

          this.utilService.OkMessage();

          this.dialogRef.close(true);

        }
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });

    }
    else {
      this.utilService.ShowApiErrorMessage(this.translate.instant("Select_File"))
    }
  }

}

