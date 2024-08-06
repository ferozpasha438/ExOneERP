import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';

@Component({
  selector: 'app-addupdate-teacher-master',
  templateUrl: './addupdate-teacher-master.component.html',
  styleUrls: []
})
export class AddupdateTeacherMasterComponent implements OnInit {
  id: number = 0;
  row: any;
  isShowTeacherCode: boolean = false;
  form!: FormGroup;
  cityList: Array<any> = [];
  genderList: Array<any> = [];
  branchCodeList: Array<any> = [];
  nationalityList: Array<any> = [];
  isArab: boolean = false;
  isShowTeacherProfile: boolean = false;
  thumbNailimageUrl: string | ArrayBuffer | null = null;
  teacherImageUrl: string | ArrayBuffer | null = null;
  teacherName: string = '';
  joiningDate: string = '';
  teacherProfileImageUrl: string = '';
  isVerifiedUserName: boolean = false;
  isShowSpouseName: boolean = true;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateTeacherMasterComponent>,
    private notifyService: NotificationService) {
  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.form = this.fb.group({
      'teacherCode': [''],
      'teacherShortName': ['', Validators.required],
      'teacherName1': ['', Validators.required],
      'teacherName2': ['', Validators.required],
      'fatherName': ['', Validators.required],
      'maritalStatus': ['', Validators.required],
      'pAddress': ['', Validators.required],
      'spouseName': ['', Validators.required],
      'pcity': ['', Validators.required],
      'pphone1': ['', Validators.required],
      'sphone2': ['', Validators.required],
      'teacherEmail': ['', Validators.required],
      'pmobile1': ['', Validators.required],
      'gender': ['', Validators.required],
      'hiringType': ['', Validators.required],
      'nationalityCode': ['', Validators.required],
      'dateJoin': ['', Validators.required],
      'nationalityID': ['', Validators.required],
      'totalExperience': ['', Validators.required],
      'passport': ['', Validators.required],
      'highestQualification': ['', Validators.required],
      'technologyCompetence': ['', Validators.required],
      'comminicationSkills': ['', Validators.required],
      'teachingSkills': ['', Validators.required],
      'subjectknowledge': ['', Validators.required],
      'administrativeSkills': ['', Validators.required],
      'disciplineSkills': ['', Validators.required],
      'thumbNailimagePath': [''],
      'thumbNailimageFileName': [''],
      'fullImageParh': [''],
      'fullImageFileName': [''],
      'primaryBranchCode': ['', Validators.required],
      'aboutTeacher': ['', Validators.required],
      'username': ['', Validators.required],
      'password': ['', Validators.required]
    });
    this.loadData();
    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.form.patchValue(this.row);
      this.editTeacherMasterData();
    }
  }

  loadData() {
    this.apiService.getall('schoolBranches/getSchoolBranchList').subscribe(res => {
      this.branchCodeList = res;
    });
    this.apiService.getPagination('schoolGender', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res)
        this.genderList = res['items'];
    });
    this.apiService.getall('City/getCitiesSelectList').subscribe(res => {
      this.cityList = res;
    });
    this.apiService.getPagination('schoolNational', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res)
        this.nationalityList = res['items'];
    });
  }
  onFileChanged(event: any, type: number) {
    let reader = new FileReader();
    if (event.target.files && event.target.files.length > 0) {
      let file = event.target.files[0];
      reader.readAsDataURL(file);
      reader.onload = () => {
        if (type === 1) {
          this.thumbNailimageUrl = reader.result;
          this.form.patchValue({
            'thumbNailimageFileName': file,
          });
        } else if (type === 2) {
          this.teacherImageUrl = reader.result;
          this.form.patchValue({
            'fullImageFileName': file,
          });
        }
      };
    }
  }
  closeModel() {
    this.dialogRef.close();
  }
  reset() {
    this.form.reset();
  }
  validateUsername() {
    const username: string = this.form.value['username'] as string;
    if (username != null && username != '') {
      this.apiService.getall(`TeacherMaster/ValidateUserName/${username}`).subscribe(res => {
        if (res)
          this.isVerifiedUserName = res;
      });
    }
  }
  submit() {
    if (this.form.valid) {
      if (this.isVerifiedUserName == false) {
        //Alert please verify username
        this.utilService.ValidateMessage();
      } else {
        if (this.id > 0)
          this.form.value['id'] = this.id;
        this.form.value['dateJoin'] = this.utilService.selectedDate(this.form.controls['dateJoin'].value);

        const formData = new FormData();
        formData.append("id", this.id.toString());
        formData.append("teacherCode", this.form.controls['teacherCode'].value);
        formData.append("teacherName1", this.form.controls['teacherName1'].value);
        formData.append("teacherName2", this.form.controls['teacherName2'].value);
        formData.append("teacherShortName", this.form.controls['teacherShortName'].value);
        formData.append("fatherName", this.form.controls['fatherName'].value);
        formData.append("maritalStatus", this.form.controls['maritalStatus'].value);
        formData.append("pAddress", this.form.controls['pAddress'].value);
        formData.append("spouseName", this.form.controls['spouseName'].value);
        formData.append("pcity", this.form.controls['pcity'].value);
        formData.append("pphone1", this.form.controls['pphone1'].value);
        formData.append("sphone2", this.form.controls['sphone2'].value);
        formData.append("teacherEmail", this.form.controls['teacherEmail'].value);
        formData.append("pmobile1", this.form.controls['pmobile1'].value);
        formData.append("thumbNailimageFileName", this.form.controls['thumbNailimageFileName'].value);
        formData.append("thumbNailimagePath", this.authService.ApiEndPoint().replace("api", "") + 'Teacherfiles/');
        formData.append("fullImageFileName", this.form.controls['fullImageFileName'].value);
        formData.append("fullImageParh", this.authService.ApiEndPoint().replace("api", "") + 'Teacherfiles/');
        formData.append("gender", this.form.controls['gender'].value);
        formData.append("hiringType", this.form.controls['hiringType'].value);
        formData.append("nationalityCode", this.form.controls['nationalityCode'].value);
        formData.append("dateJoin", this.utilService.selectedDate(this.form.controls['dateJoin'].value));
        formData.append("nationalityID", this.form.controls['nationalityID'].value);
        formData.append("totalExperience", this.form.controls['totalExperience'].value);
        formData.append("passport", this.form.controls['passport'].value);
        formData.append("highestQualification", this.form.controls['highestQualification'].value);
        formData.append("technologyCompetence", this.form.controls['technologyCompetence'].value);
        formData.append("comminicationSkills", this.form.controls['comminicationSkills'].value);
        formData.append("teachingSkills", this.form.controls['teachingSkills'].value);
        formData.append("subjectknowledge", this.form.controls['subjectknowledge'].value);
        formData.append("administrativeSkills", this.form.controls['administrativeSkills'].value);
        formData.append("disciplineSkills", this.form.controls['disciplineSkills'].value);
        formData.append("primaryBranchCode", this.form.controls['primaryBranchCode'].value);
        formData.append("aboutTeacher", this.form.controls['aboutTeacher'].value);
        formData.append("username", this.form.controls['username'].value);
        formData.append("password", this.form.controls['password'].value);
        this.apiService.post('TeacherMaster', formData)
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
    }
    else
      this.utilService.FillUpFields();
  }
  checkMaritalStatus() {
    const maritalStatus: string = this.form.value['maritalStatus'] as string;
    if (maritalStatus == "Single") {
      this.isShowSpouseName = false;
      this.form.controls['spouseName'].clearValidators();
      this.form.controls['spouseName'].updateValueAndValidity();
    } else {
      this.isShowSpouseName = true;
      this.form.controls['spouseName'].setValidators([Validators.required]);
      this.form.controls['spouseName'].updateValueAndValidity();
    }
  }
  editTeacherMasterData() {
    this.apiService.getall(`TeacherMaster/GetTeacherMasterDataById/${this.id}`).subscribe(res => {
      if (res) {
        this.form.patchValue({
          'teacherCode': res.teacherCode,
          'teacherName1': res.teacherName1,
          'teacherName2': res.teacherName2,
          'teacherShortName': res.teacherShortName,
          'fatherName': res.fatherName,
          'maritalStatus': res.maritalStatus,
          'pAddress': res.pAddress,
          'spouseName': res.spouseName,
          'pcity': res.pcity,
          'pphone1': res.pPhone1,
          'sphone2': res.sPhone2,
          'teacherEmail': res.teacherEmail,
          'pmobile1': res.pMobile1,
          'mobile': res.mobile,
          'gender': res.gender,
          'hiringType': res.hiringType,
          'nationalityCode': res.nationalityCode,
          'dateJoin': res.dateJoin,
          'nationalityID': res.nationalityID,
          'totalExperience': res.totalExperience,
          'passport': res.passport,
          'highestQualification': res.highestQualification,
          'technologyCompetence': res.technologyCompetence,
          'comminicationSkills': res.comminicationSkills,
          'teachingSkills': res.teachingSkills,
          'subjectknowledge': res.subjectknowledge,
          'administrativeSkills': res.administrativeSkills,
          'disciplineSkills': res.disciplineSkills,
          'primaryBranchCode': res.primaryBranchCode,
          'aboutTeacher': res.aboutTeacher,
          'username': res.username
        });
        this.checkMaritalStatus();
        this.isShowTeacherProfile = true;
        this.isShowTeacherCode = true;
        this.teacherProfileImageUrl = res.thumbNailImagePath;
        this.thumbNailimageUrl = res.thumbNailImagePath;
        this.teacherImageUrl = res.fullImageParh;
        this.joiningDate = res.dateJoin;
        this.teacherName = this.isArab ? res.teacherName2 : res.teacherName1;
        if (res.username != null && res.username != "") {
          this.isVerifiedUserName = true;
          this.form.controls['username'].clearValidators();
          this.form.controls['username'].updateValueAndValidity();
          this.form.controls['password'].clearValidators();
          this.form.controls['password'].updateValueAndValidity();
        }
      }
    });


  }

}
