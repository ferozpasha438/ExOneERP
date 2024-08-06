import { HttpClient } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { AuthorizeService } from "../api-authorization/AuthorizeService";
import { MultiFileUploadDto } from "../models/sharedDto";
import { ApiService } from "../services/api.service";
import { NotificationService } from "../services/notification.service";
import { UtilityService } from "../services/utility.service";
import { default as data } from "../../assets/i18n/apiuri.json";

@Component({
  selector: 'file-upload',
  templateUrl: './fileupload.component.html'
})
export class FileUploadComponent implements OnInit {
  moduleFile!: MultiFileUploadDto;
  uploadedFiles: Array<any> = [];
  fileUrl = '';
  isLoading: boolean = false;

  fileone: string = '';
  filetwo: string = '';
  filethree: string = '';
  filefour: string = '';
  filefive: string = '';

  fileUploadone!: File;
  fileUploadtwo!: File;
  fileUploadthree!: File;
  fileUploadfour!: File;
  fileUploadfive!: File;


  files: Array<File> = [];
  formData!: FormData;

  constructor(private fb: FormBuilder, private apiService: ApiService, private authService: AuthorizeService, private utilService: UtilityService,
    private notifyService: NotificationService, private http: HttpClient,
    public dialog: MatDialog, public dialogRef: MatDialogRef<FileUploadComponent>) {
  }
  ngOnInit(): void {
    this.loadFiles();
    this.fileUrl = this.authService.ApiEndPoint();
  }


  onSelectFile1(fileInput: any) {
    this.fileUploadone = <File>fileInput.target.files[0];
  }
  onSelectFile2(fileInput: any) {
    this.fileUploadtwo = <File>fileInput.target.files[0];
  }
  onSelectFile3(fileInput: any) {
    this.fileUploadthree = <File>fileInput.target.files[0];
  }
  onSelectFile4(fileInput: any) {
    this.fileUploadfour = <File>fileInput.target.files[0];
  }
  onSelectFile5(fileInput: any) {
    this.fileUploadfive = <File>fileInput.target.files[0];
  }

  loadFiles() {
    this.isLoading = true;
    this.apiService.getall(`fileUpload/getFilesByModulewiseId?sourceId=${this.moduleFile.sourceId}&action=${this.moduleFile.action}`)
      .subscribe(res => {
        this.isLoading = false;
        this.uploadedFiles = res;
      }, error => {
        this.isLoading = false;
      });
  }

  uploadFiles() {
    this.formData = new FormData();

    //this.moduleFile.action = "PO";
    //this.moduleFile.id = 222;
    //this.moduleFile.sourceId = "so_8898";
    //this.moduleFile.module = "PM";

    if (this.moduleFile)
      this.formData.append("modulefile", JSON.stringify(this.moduleFile));

    if (this.fileUploadone) {
      this.formData.append("fileone", this.fileUploadone, this.fileUploadone.name);
      this.formData.append("fileone", this.fileone);
    }

    if (this.fileUploadtwo) {
      this.formData.append("filetwo", this.fileUploadtwo, this.fileUploadtwo.name);
      this.formData.append("filetwo", this.filetwo);
    }

    if (this.fileUploadthree) {
      this.formData.append("filethree", this.fileUploadthree, this.fileUploadthree.name);
      this.formData.append("filethree", this.filethree);
    }

    if (this.fileUploadfour) {
      this.formData.append("filefour", this.fileUploadfour, this.fileUploadfour.name);
      this.formData.append("filefour", this.filefour);
    }

    if (this.fileUploadfive) {
      this.formData.append("filefive", this.fileUploadfive, this.fileUploadfive.name);
      this.formData.append("filefive", this.filefive);
    }

    //this.authService.SetApiEndPoint(data.purchaseapiurl);
    //this.http.post(`http://localhost:29081/api/fileUpload/testuploadfiles`, this.formData)
    this.apiService.post("fileUpload/uploadfiles", this.formData)
      //    this.apiService.post("fileUpload/createDocument", this.formData)
      .subscribe(res => {
        this.notifyService.showSuccess('Uploaded successfully');
        this.dialogRef.close(true);

      }, error => {
        this.notifyService.showError(error.error);
        //console.log(error);
      });


    //for (let i = 0; i < this.files.length; i++) {
    //  formData.append('filedata' + i, this.files[i], this.files[i].name);
    //  formData.append('data' + i, JSON.stringify(this.files[i].stream));
    //}




    // console.log(formData);

    //formData.append('files', this.selectedFiles); // add all the other properties
    // return this.http.post('http://somehost/someendpoint/fileupload/', formData);
  }

  closeModel() {
    this.dialogRef.close();
  }

}
