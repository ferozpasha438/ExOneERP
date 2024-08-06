import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';

@Component({
  selector: 'app-addupdate-student-master',
  templateUrl: './addupdate-student-master.component.html',
  styleUrls: []
})
export class AddupdateStudentMasterComponent implements OnInit {
  id: number = 0;
  row: any;
  form!: FormGroup;
  isArab: boolean = false;
  genderList: Array<any> = [];
  gradeCodeList: Array<any> = [];
  branchCodeList: Array<any> = [];
  schoolPETCategoryList: Array<any> = [];
  sectionList: Array<any> = [];
  languageList: Array<any> = [];
  nationalityList: Array<any> = [];
  religionList: Array<any> = [];
  feeStructureList: Array<any> = [];
  cityList: Array<any> = [];
  fatherSignatureUrl: string | ArrayBuffer | null = null;
  motherSignatureUrl: string | ArrayBuffer | null = null;
  isShowTransportControls: boolean = false;
  isShowStuAdmNum: boolean = false;
  isShowSpecialAssistNotes: boolean = false;
  isShowPhysicalDisabilityNotes: boolean = false;
  isShowStudentProfile: boolean = false;
  studentImageUrl: string | ArrayBuffer | null = null;
  studentProfileImageUrl: string | ArrayBuffer | null = null;
  studentName: string = '';
  joiningDate: string = '';
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateStudentMasterComponent>,
    private notifyService: NotificationService) {
  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    
    this.form = this.fb.group({
      'stuAdmNum': [''],
      'stuAdmDate': ['', Validators.required],
      'stuName': ['', Validators.required],
      'stuName2': ['', Validators.required],
      'dateofBirth': ['', Validators.required],
      'alias': ['', Validators.required],
      'genderCode': ['', Validators.required],
      'age': ['', Validators.required],
      'branchCode': ['', Validators.required],
      'gradeCode': ['', Validators.required],
      'ptGroupCode': ['', Validators.required],
      'gradeSectionCode': ['', Validators.required],
      'langCode': ['', Validators.required],
      'natCode': ['', Validators.required],
      'religionCode': ['', Validators.required],
      'stuIDNumber': ['', Validators.required],
      'idNumber': ['', Validators.required],
      'motherToungue': ['', Validators.required],
      'registeredPhone': ['', Validators.required],
      'registeredEmail': ['', Validators.required],
      'isActive': [true],
      'studentImage': [''],
      'studentImageFileName': [''],
      'feeStructCode': ['', Validators.required],
      'isTaxApplicable': [false],
      'totFeeAmount': [''],
      'paidFees': [''],
      'netFeeAmount': [''],
      'transportationRequired': [false],
      'pickNDropZone': [''],
      'transportationFee': [''],
      'vehicleTransport': [''],
      'buildingName': ['', Validators.required],
      'pAddress1': ['', Validators.required],
      'city': ['', Validators.required],
      'phone': ['', Validators.required],
      'zipCode': ['', Validators.required],
      'mobile': ['', Validators.required],
      'fatherName': ['', Validators.required],
      'fatherMobile': ['', Validators.required],
      'fatherEmail': ['', Validators.required],
      'fatherOccupation': ['', Validators.required],
      'fatherDesignation': ['', Validators.required],
      'fatherSignature': [''],
      'fatherSignatureFileName': [''],
      'motherName': ['', Validators.required],
      'motherMobile': ['', Validators.required],
      'motherEmail': ['', Validators.required],
      'motherOccupation': ['', Validators.required],
      'motherDesignation': ['', Validators.required],
      'motherSignature': [''],
      'motherSignatureFileName': [''],
      'bloodGroup': [''],
      'height': [''],
      'weight': [''],
      'specialAssistance': [false],
      'specialAssistanceNotes': [''],
      'physicalDisability': [false],
      'physicalDisabilityNotes': [''],
      'academicsScale': ['0'],
      'attentivenessScale': ['0'],
      'homeWorkScale': ['0'],
      'projectWorkScale': ['0'],
      'sportsPhysicalScale': ['0'],
      'diciplineAttitude': ['0']
    });
    this.loadData();
    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.form.patchValue(this.row);
      this.editStudentMastrData();
    }

  }

  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      this.form.value['stuAdmDate'] = this.utilService.selectedDate(this.form.controls['stuAdmDate'].value);
      this.form.value['dateofBirth'] = this.utilService.selectedDate(this.form.controls['dateofBirth'].value);

      const formData = new FormData();
      formData.append("id", this.id.toString());
      formData.append("stuAdmNum", this.form.controls['stuAdmNum'].value);
      formData.append("stuAdmDate", this.utilService.selectedDate(this.form.controls['stuAdmDate'].value));
      formData.append("stuName", this.form.controls['stuName'].value);
      formData.append("stuName2", this.form.controls['stuName2'].value);
      formData.append("dateofBirth", this.utilService.selectedDate(this.form.controls['dateofBirth'].value));
      formData.append("alias", this.form.controls['alias'].value);
      formData.append("genderCode", this.form.controls['genderCode'].value);
      formData.append("age", this.form.controls['age'].value);
      formData.append("branchCode", this.form.controls['branchCode'].value);
      formData.append("gradeCode", this.form.controls['gradeCode'].value);
      formData.append("ptGroupCode", this.form.controls['ptGroupCode'].value);
      formData.append("gradeSectionCode", this.form.controls['gradeSectionCode'].value);
      formData.append("langCode", this.form.controls['langCode'].value);
      formData.append("natCode", this.form.controls['natCode'].value);
      formData.append("religionCode", this.form.controls['religionCode'].value);
      formData.append("stuIDNumber", this.form.controls['stuIDNumber'].value);
      formData.append("idNumber", this.form.controls['idNumber'].value);
      formData.append("motherToungue", this.form.controls['motherToungue'].value);
      formData.append("registeredPhone", this.form.controls['registeredPhone'].value);
      formData.append("registeredEmail", this.form.controls['registeredEmail'].value);
      formData.append("isActive", this.form.controls['isActive'].value);
      formData.append("studentImageFileName", this.authService.ApiEndPoint().replace("api", "") + 'Signaturefiles/');
      formData.append("studentImage", this.form.controls['studentImage'].value);
      formData.append("feeStructCode", this.form.controls['feeStructCode'].value);
      formData.append("taxApplicable", this.form.controls['isTaxApplicable'].value);
      formData.append("totFeeAmount", this.form.controls['totFeeAmount'].value);
      formData.append("paidFees", this.form.controls['paidFees'].value);
      formData.append("netFeeAmount", this.form.controls['netFeeAmount'].value);
      formData.append("transportationRequired", this.form.controls['transportationRequired'].value);
      formData.append("pickNDropZone", this.form.controls['pickNDropZone'].value);
      formData.append("transportationFee", this.form.controls['transportationFee'].value);
      formData.append("vehicleTransport", this.form.controls['vehicleTransport'].value);
      formData.append("buildingName", this.form.controls['buildingName'].value);
      formData.append("pAddress1", this.form.controls['pAddress1'].value);
      formData.append("city", this.form.controls['city'].value);
      formData.append("phone", this.form.controls['phone'].value);
      formData.append("zipCode", this.form.controls['zipCode'].value);
      formData.append("mobile", this.form.controls['mobile'].value);
      formData.append("fatherName", this.form.controls['fatherName'].value);
      formData.append("fatherMobile", this.form.controls['fatherMobile'].value);
      formData.append("fatherEmail", this.form.controls['fatherEmail'].value);
      formData.append("fatherOccupation", this.form.controls['fatherOccupation'].value);
      formData.append("fatherDesignation", this.form.controls['fatherDesignation'].value);
      formData.append("fatherSignatureFileName", this.authService.ApiEndPoint().replace("api", "") + 'Signaturefiles/');
      formData.append("fatherSignature", this.form.controls['fatherSignature'].value);
      formData.append("motherName", this.form.controls['motherName'].value);
      formData.append("motherMobile", this.form.controls['motherMobile'].value);
      formData.append("motherEmail", this.form.controls['motherEmail'].value);
      formData.append("motherOccupation", this.form.controls['motherOccupation'].value);
      formData.append("motherDesignation", this.form.controls['motherDesignation'].value);
      formData.append("motherSignatureFileName", this.authService.ApiEndPoint().replace("api", "") + 'Signaturefiles/');
      formData.append("motherSignature", this.form.controls['motherSignature'].value);
      formData.append("bloodGroup", this.form.controls['bloodGroup'].value);
      formData.append("height", this.form.controls['height'].value == "" ? "0" : this.form.controls['height'].value);
      formData.append("weight", this.form.controls['weight'].value == "" ? "0" : this.form.controls['weight'].value);
      formData.append("specialAssistance", this.form.controls['specialAssistance'].value);
      formData.append("specialAssistanceNotes", this.form.controls['specialAssistanceNotes'].value);
      formData.append("physicalDisability", this.form.controls['physicalDisability'].value);
      formData.append("physicalDisabilityNotes", this.form.controls['physicalDisabilityNotes'].value);
      formData.append("academicsScale", this.form.controls['academicsScale'].value);
      formData.append("attentivenessScale", this.form.controls['attentivenessScale'].value);
      formData.append("homeWorkScale", this.form.controls['homeWorkScale'].value);
      formData.append("projectWorkScale", this.form.controls['projectWorkScale'].value);
      formData.append("sportsPhysicalScale", this.form.controls['sportsPhysicalScale'].value);
      formData.append("diciplineAttitude", this.form.controls['diciplineAttitude'].value);
      this.apiService.post('SchoolStudentMaster/SaveAllStudentMasterData', formData)
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
    this.form.reset();
  }
  closeModel() {
    this.dialogRef.close();
  }
  loadData() {
    this.apiService.getPagination('schoolGender', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res)
        this.genderList = res['items'];
    });
    this.apiService.getall('schoolBranches/getSchoolBranchList').subscribe(res => {
      this.branchCodeList = res;
    });
    this.apiService.getPagination('acedemicClassGrade', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res)
        this.gradeCodeList = res['items'];
    });
    this.apiService.getPagination('schoolPETCategory', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res)
        this.schoolPETCategoryList = res['items'];
    });
    this.apiService.getPagination('schoolLanguages', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res)
        this.languageList = res['items'];
    });
    this.apiService.getPagination('schoolNational', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res)
        this.nationalityList = res['items'];
    });
    this.apiService.getPagination('schoolReligion', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res)
        this.religionList = res['items'];
    });
    this.apiService.getall('City/getCitiesSelectList').subscribe(res => {
      this.cityList = res;
    });
  }
  loadRelatedItems() {
    const gradeCode: string = this.form.value['gradeCode'] as string;
    const branchCode: string = this.form.value['branchCode'] as string;
    if (gradeCode != null && gradeCode != '') {
      this.apiService.getall(`SchoolGradeSectionMapping/getAllSectionsByGradeCode/${gradeCode}`).subscribe(res => {
        if (res)
          this.sectionList = res;
      });
    }
    if (gradeCode != null && gradeCode != '' && branchCode != null && branchCode != '') {
      this.apiService.getall(`schoolFeeStructure/GetFeeStructureCodesByGradeANDBranch/${gradeCode}/${branchCode}`).subscribe(res => {
        if (res)
          this.feeStructureList = res;
      });
    }
  }
  loadFeeDetails() {
    const feeStructCode: string = this.form.value['feeStructCode'] as string;
    if (feeStructCode != null && feeStructCode != '') {
      this.apiService.getall(`schoolFeeStructure/getTotalTermFeeByFeeStructCode/${feeStructCode}`).subscribe(res => {
        if (res) {
          this.form.patchValue({ 'totFeeAmount': res.feeAmount, 'paidFees': '0', 'netFeeAmount': res.feeAmount });
        }
      });
    }
  }
  calcAge(event:any) {
    const dateofBirth: Date = this.form.value['dateofBirth'] as Date;
    const currentDate = new Date();
    const diff = currentDate.getTime() - dateofBirth.getTime();
    this.form.patchValue({
      'age': Math.floor(diff / (1000 * 60 * 60 * 24*365))
    });
  }
  onFileChanged(event: any, type: number) {
    let reader = new FileReader();
    if (event.target.files && event.target.files.length > 0) {
      let file = event.target.files[0];
      reader.readAsDataURL(file);
      reader.onload = () => {
        if (type === 1) {
          this.fatherSignatureUrl = reader.result;
          this.form.patchValue({
            'fatherSignature': file,
          });
        } else if (type === 2) {
          this.motherSignatureUrl = reader.result;
          this.form.patchValue({
            'motherSignature': file,
          });
        } else if (type === 3){
          this.studentImageUrl = reader.result;
          this.form.patchValue({
            'studentImage': file,
          });
        }
      };
    }
  }
  applyTranportChanges(isChecked:boolean) {
    if (isChecked) {
      this.isShowTransportControls = true;
      this.form.controls['pickNDropZone'].setValidators([Validators.required]);
      this.form.controls['pickNDropZone'].updateValueAndValidity();
      this.form.controls['transportationFee'].setValidators([Validators.required]);
      this.form.controls['transportationFee'].updateValueAndValidity();
      this.form.controls['vehicleTransport'].setValidators([Validators.required]);
      this.form.controls['vehicleTransport'].updateValueAndValidity();

    } else {
      this.isShowTransportControls = false;
      this.form.controls['pickNDropZone'].clearValidators();
      this.form.controls['pickNDropZone'].updateValueAndValidity();
      this.form.controls['transportationFee'].clearValidators();
      this.form.controls['transportationFee'].updateValueAndValidity();
      this.form.controls['vehicleTransport'].clearValidators();
      this.form.controls['vehicleTransport'].updateValueAndValidity();
    }
  }
  changeTransportValue(isChecked:boolean) {
    this.applyTranportChanges(isChecked);
  }
  applySpecialAssistanceChanges(isChecked: boolean) {
    if (isChecked) {
      this.isShowSpecialAssistNotes = true;
      this.form.controls['specialAssistanceNotes'].setValidators([Validators.required]);
      this.form.controls['specialAssistanceNotes'].updateValueAndValidity();
    } else {
      this.isShowSpecialAssistNotes = false;
      this.form.controls['specialAssistanceNotes'].clearValidators();
      this.form.controls['specialAssistanceNotes'].updateValueAndValidity();
    }
  }
  checkSpecialAssistance(isChecked:boolean) {
    this.applySpecialAssistanceChanges(isChecked);
  }
  applyPhysicalDisabilityChanges(isChecked: boolean) {
    if (isChecked) {
      this.isShowPhysicalDisabilityNotes = true;
      this.form.controls['physicalDisabilityNotes'].setValidators([Validators.required]);
      this.form.controls['physicalDisabilityNotes'].updateValueAndValidity();
    } else {
      this.isShowPhysicalDisabilityNotes = false;
      this.form.controls['physicalDisabilityNotes'].clearValidators();
      this.form.controls['physicalDisabilityNotes'].updateValueAndValidity();
    }
  }
  checkPhysicalDisability(isChecked: boolean) {
    this.applyPhysicalDisabilityChanges(isChecked);
  }
  editStudentMastrData() {
    this.apiService.getall(`SchoolStudentMaster/GetStudentMasterDataById/${this.id}`).subscribe(res => {
      if (res) {
        this.form.patchValue({
          'stuAdmNum': res.stuAdmNum,
          'stuAdmDate': res.stuAdmDate,
          'stuName': res.stuName,
          'stuName2': res.stuName2,
          'dateofBirth': res.dateofBirth,
          'alias': res.alias,
          'genderCode': res.genderCode,
          'age': res.age,
          'branchCode': res.branchCode,
          'gradeCode': res.gradeCode,
          'ptGroupCode': res.ptGroupCode,
          'gradeSectionCode': res.gradeSectionCode,
          'langCode': res.langCode,
          'natCode': res.natCode,
          'religionCode': res.religionCode,
          'stuIDNumber': res.stuIDNumber,
          'idNumber': res.idNumber,
          'motherToungue': res.motherToungue,
          'registeredPhone': res.registeredPhone,
          'registeredEmail': res.registeredEmail,
          'isActive': res.isActive,
          'feeStructCode': res.feeStructCode,
          'isTaxApplicable': res.taxApplicable,
          'totFeeAmount': res.totFeeAmount,
          'paidFees': res.paidFees,
          'netFeeAmount': res.netFeeAmount,
          'transportationRequired': res.transportationRequired,
          'pickNDropZone': res.pickNDropZone,
          'transportationFee': res.transportationFee,
          'vehicleTransport': res.vehicleTransport,
          'pAddress1': res.pAddress1,
          'buildingName': res.buildingName,
          'city': res.city,
          'phone': res.phone,
          'zipCode': res.zipCode,
          'mobile': res.mobile,
          'fatherName': res.fatherName,
          'fatherMobile': res.fatherMobile,
          'fatherEmail': res.fatherEmail,
          'fatherOccupation': res.fatherOccupation,
          'fatherDesignation': res.fatherDesignation,
          'fatherSignatureFileName': res.fatherSignatureFileName,
          'motherName': res.motherName,
          'motherMobile': res.motherMobile,
          'motherEmail': res.motherEmail,
          'motherOccupation': res.motherOccupation,
          'motherDesignation': res.motherDesignation,
          'motherSignatureFileName': res.motherSignatureFileName,
          'bloodGroup': res.bloodGroup,
          'height': res.height,
          'weight': res.weight,
          'specialAssistance': res.specialAssistance,
          'specialAssistanceNotes': res.specialAssistanceNotes,
          'physicalDisability': res.physicalDisability,
          'physicalDisabilityNotes': res.physicalDisabilityNotes,
          'academicsScale': res.academicsScale,
          'attentivenessScale': res.attentivenessScale,
          'homeWorkScale': res.homeWorkScale,
          'projectWorkScale': res.projectWorkScale,
          'sportsPhysicalScale': res.sportsPhysicalScale,
          'diciplineAttitude': res.diciplineAttitude
        });
        this.isShowStuAdmNum = true;
        this.fatherSignatureUrl = res.fatherSignatureFileName;
        this.motherSignatureUrl = res.motherSignatureFileName;
        this.loadRelatedItems();
        this.form.patchValue({
          'gradeSectionCode': res.gradeSectionCode,
          'feeStructCode': res.feeStructCode
        });
        this.applyTranportChanges(res.transportationRequired);
        this.applySpecialAssistanceChanges(res.specialAssistance);
        this.applyPhysicalDisabilityChanges(res.physicalDisability);

        this.isShowStudentProfile = true;
        this.studentProfileImageUrl = res.studentImageFileName;
        this.studentImageUrl = res.studentImageFileName;
        this.joiningDate = res.stuAdmDate;
        this.studentName = this.isArab ? res.stuName2 : res.stuName;
      }
    });
    
   
  }
}
