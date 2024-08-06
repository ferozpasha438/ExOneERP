import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ParentSchoolMgtComponent } from '../../sharedcomponent/parentschoolmgt.component';

@Component({
  selector: 'app-semesterssubjectmappingwithgrade',
  templateUrl: './semesterssubjectmappingwithgrade.component.html',
  styleUrls: []
})
export class SemesterssubjectmappingwithgradeComponent extends ParentSchoolMgtComponent implements OnInit {

  code: string = '';
  semisterCode: string = '';

  form!: FormGroup;
  semCodeList: Array<any> = [];
  semSubjectsList: Array<any> = [];
  semEditSubjectsList: Array<any> = [];
  selecetedSemSubjectsCodeList: Array<any> = [];

  isArab: boolean = false;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<SemesterssubjectmappingwithgradeComponent>,
    private notifyService: NotificationService) {

    super(authService);
  }

  ngOnInit(): void {

    this.isArab = this.utilService.isArabic();
    this.loadData();
  }

  loadData() {
    this.apiService.getPagination('schoolSemister', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res)
        this.semCodeList = res['items'];
    });

    this.apiService.getPagination('academicsSubjects', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res) {
        this.semSubjectsList = res['items'];
        if (this.semSubjectsList.length > 0) {
          this.semEditSubjectsList = this.semSubjectsList.map(item => {
            return {
              isSelected: false,
              //sectionCode: item.sectionCode,
              subCodes: item.subCodes,
              subName: item.subName,
              subName2: item.subName2,
            };
          });
        }
      }
    });
  }

  //loadSubjects(event: any) {
  //  const semCode = event.target.value;
  //  this.apiService.getPagination('academicsSubjects', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
  //    if (res)
  //      this.semSubjectsList = res['items'];
  //  });
  //}

  getGradeSectionData() {
    if (this.code != null && this.code != ''
      && this.semisterCode != null && this.semisterCode != '') {
      this.apiService.getall(`schoolGradeSubjectMapping/getAllSubjectByGradeAndSemisterMapping/${this.code}/${this.semisterCode}`).subscribe(res => {
        if (res.length > 0) {
          for (var i = 0; i < res.length; i++) {
            if (this.semEditSubjectsList.length > 0) {
              var index: number = this.semEditSubjectsList.findIndex(item => item.subCodes === res[i].subCodes);
              let pItem = this.semEditSubjectsList[index];
              if (pItem != null) {
                pItem.isSelected = true;
              }
            }
          }
        }
      });
    }
  }
  addRemoveSubjects(event: any, subCodes: string) {
    if (event.checked) {
      this.selecetedSemSubjectsCodeList.push(subCodes)
    }
    else
      this.selecetedSemSubjectsCodeList = this.selecetedSemSubjectsCodeList.filter(sCode => sCode != subCodes);
  }

  submit() {

    if (this.selecetedSemSubjectsCodeList.length > 0 && this.semisterCode !== '') {
      this.apiService.post('schoolGradeSubjectMapping', { gradeCode: this.code, SemisterCode: this.semisterCode, SubCodes: this.selecetedSemSubjectsCodeList })
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
    this.selecetedSemSubjectsCodeList = [];
    this.semisterCode = '';
  }

  closeModel() {
    this.dialogRef.close();
  }
}
