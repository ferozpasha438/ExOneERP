import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';

@Component({
  selector: 'app-teacher-language-mapping',
  templateUrl: './teacher-language-mapping.component.html',
  styleUrls: []
})
export class TeacherLanguageMappingComponent implements OnInit {
  form!: FormGroup;
  id: number = 0;
  row: any;
  isArab: boolean = false;
  languageDetails: Array<any> = [];
  editsequence: number = 0;
  sequence: number = 1;
  languageList: Array<any> = [];
  teacherCode: string = '';
  languageCode: string = ''
  read: number = 0;
  write: number = 0;
  speak: number = 0;

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<TeacherLanguageMappingComponent>,
    private notifyService: NotificationService) { }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.form = this.fb.group({
    });
    this.loadData();
    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.teacherCode = this.row['teacherCode'];
      this.loadDetailsForTeacherCode(this.teacherCode);
    }
  }
  loadData() {
    this.apiService.getPagination('SchoolLanguages', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res)
        this.languageList = res['items'];
    });
  }
  loadDetailsForTeacherCode(teacherCode: string) {
    this.apiService.getall(`TeacherLanguages/${teacherCode}`).subscribe(res => {
      if (res) {
        const details = res as Array<any>;
        details.forEach(item => {
          this.languageDetails.push({
            sequence: this.getSequence(),
            teacherCode: this.teacherCode,
            languageCode: item.languageCode,
            read: item.read,
            write: item.write,
            speak: item.speak
          });
        });
      }
    });
  }
  addLanguageItem() {
    if (this.languageCode!='') {
      if (this.editsequence > 0) {
        var index: number = this.languageDetails.findIndex(a => a.sequence === this.editsequence);
        let pItem = this.languageDetails[index];
        pItem.languageCode = this.languageCode;
        pItem.read = this.read;
        pItem.write = this.write;
        pItem.speak = this.speak;
        this.editsequence = 0;
      }
      else {
        this.languageDetails.push({
          sequence: this.getSequence(),
          teacherCode: this.teacherCode,
          languageCode: this.languageCode,
          read: this.read,
          write: this.write,
          speak: this.speak
        });
      }
      this.setToDefault();
    }    
  }
  getSequence(): number { return this.sequence = this.sequence + 1 };

  deleteLanguageItem(item: any) {
    this.removeLanguageList(item.sequence);
  }
  removeLanguageList(sequence: number) {
    let index: number = this.languageDetails.findIndex(a => a.sequence === sequence);
    this.languageDetails.splice(index, 1);
  }
  editLanguageItem(item: any) {
    this.editsequence = item.sequence;
    this.languageCode = item.languageCode;
    this.read = item.read;
    this.write = item.write;
    this.speak = item.speak;
  }
  closeModel() {
    this.dialogRef.close();
  }
  setToDefault() {
    this.languageCode = '';
    this.read = 0;
    this.write = 0;
    this.speak =0;
  }
  reset() {
    this.form.reset();
    this.setToDefault();
  }
  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      if (this.languageDetails.length > 0) {
        this.form.value['teacherLanguagesList'] = this.languageDetails;
        this.apiService.post('TeacherLanguages', this.form.value)
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
    else
      this.utilService.FillUpFields();
  }
}
