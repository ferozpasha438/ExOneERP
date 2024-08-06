import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ParentSchoolMgtComponent } from '../../sharedcomponent/parentschoolmgt.component';


@Component({
  selector: 'app-gradesectionmapping',
  templateUrl: './gradesectionmapping.component.html',
  styleUrls: []
})
export class GradesectionmappingComponent extends ParentSchoolMgtComponent implements OnInit {

  code: string = '';
  semCodeList: Array<any> = [];
  form!: FormGroup;
  isArab: boolean = false;
  semCodeGradeList: Array<any> = [];


  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<GradesectionmappingComponent>,
    private notifyService: NotificationService) {

    super(authService);
  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    //this.form = this.fb.group({
    //  'gradeCode': [this.code, Validators.required],
    //  'max': ['', Validators.required],
    //  'min': ['', Validators.required],
    //  'avg': ['', Validators.required]
    //});

    this.loadData();
  }

  loadData() {
    this.apiService.getPagination('schoolSections', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res) {
        this.semCodeList = res['items'];
        if (this.semCodeList.length > 0) {
          this.semCodeGradeList = this.semCodeList.map(item => {
            return {
              isSelected: false,
              sectionCode: item.sectionCode,
              fileName: item.fileName,
              maxStrength: 0,
              minStrength: 0,
              avgStrength: 0,
              uploadFile:''
            };
          });

          if (this.semCodeList.length > 0) {
            this.apiService.getall(`schoolGradeSectionMapping/getAllSectionsByGradeMapping/${this.code}`).subscribe(res => {
              if (res) {
                const editList = res as Array<any>;
                if (editList.length > 0) {
                  this.semCodeGradeList.forEach((item, index) => {
                    const editItem = editList.find(editItem => editItem.sectionCode == item.sectionCode);
                    if (editItem) {
                      editItem.isSelected = true;
                      this.semCodeGradeList.splice(index, 1, editItem);
                    }
                  });
                  //this.semCodeGradeList = this.semCodeGradeList.map(item => {
                  //  const editIndex = editList.findIndex(editItem => {
                  //    this.semCodeGradeList.splice(editIndex, 1, editItem);
                  //  });

                  //  return {
                  //    isSelected: false,
                  //    sectionCode: item.sectionCode,
                  //    maxStrength: 0,
                  //    minStrength: 0,
                  //    avgStrength: 0
                  //  };
                  //});
                }
              }
            });
          }

        }
      }
    });
  }
  closeModel() {
    this.dialogRef.close();
  }
  submit() {
    const newList = this.semCodeGradeList.filter(item => item.isSelected);
    const validList = newList.filter(item => item.maxStrength > 0 && item.minStrength > 0 && item.avgStrength > 0)
    var formData = new FormData();
    if (newList.length == validList.length) {
      var sectionCodes = '';
      const notSelectedList = this.semCodeGradeList.filter(item => item.isSelected==false);
      if (notSelectedList.length>0) {
        for (var i = 0; i < notSelectedList.length; i++) {
          if (i == 0) {
            sectionCodes = notSelectedList[i].sectionCode;
          } else {
            sectionCodes = sectionCodes + "," + notSelectedList[i].sectionCode;
          }
        }
      }
      for (var i = 0; i < newList.length; i++) {
        formData = new FormData();
        formData.append("gradeCode", this.code);
        formData.append("sectionCode", newList[i].sectionCode);
        formData.append("maxStrength", newList[i].maxStrength);
        formData.append("minStrength", newList[i].minStrength);
        formData.append("avgStrength", newList[i].avgStrength);
        formData.append("uploadFileName", this.authService.ApiEndPoint().replace("api", "") + 'Signaturefiles/' + this.code + '/');
        formData.append("uploadFile", newList[i].uploadFile);
        formData.append("page", i.toString());
        formData.append("sectionCodes", sectionCodes);
        this.apiService.post('schoolGradeSectionMapping/CreateMapping', formData)
          .subscribe(res => {
            if (i === newList.length) {
              this.utilService.OkMessage();
              this.reset();
              this.dialogRef.close(true);
            }
          },
            error => {
              console.error(error);
              this.utilService.ShowApiErrorMessage(error);
            });
      }
    }
    else
      this.utilService.FillUpFields();
  }

  reset() {
    this.semCodeGradeList = this.semCodeList.map(item => {
      return {
        isSelected: false,
        sectionCode: item.sectionCode,
        fileName: item.fileName,
        maxStrength: 0,
        minStrength: 0,
        avgStrength: 0,
        uploadFile: ''
      };
    });
  }

  onFileChanged(event: any, selectedItem:any) {
    let reader = new FileReader();
    if (event.target.files && event.target.files.length > 0) {
      let file = event.target.files[0];
      //if (file.type !== "image/png" &&
      //  this.selectedFile.type !== "image/jpg" &&
      //  this.selectedFile.type !== "image/jpeg" &&
      //  this.selectedFile.type !== "application/pdf")
      const newItem = this.semCodeGradeList.find(item => item.sectionCode == selectedItem.sectionCode);
      if (file.type !== "application/pdf") {
        this.notifyService.showError("File type must be pdf", "Error");
        newItem.uploadFile = '';
        return;
      } else {
        reader.readAsDataURL(file);
        reader.onload = () => {
          newItem.uploadFile = file;
        };
      }
    }
  }

}
