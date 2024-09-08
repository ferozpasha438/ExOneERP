import { Component, Input, OnInit } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { ParentHrmAdminComponent } from '../../../sharedcomponent/ParentHrmAdmin.component';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { CustomSelectListItem } from 'src/app/models/MenuItemListDto';
import { EmployeeBasicInfoDto } from 'src/app/models/HumanResource/EmployeeBasicInfo';
import { default as constants } from '../../../../assets/i18n/constants.json';

@Component({
  selector: 'app-getemployeepersonalinfo',
  templateUrl: './getemployeepersonalinfo.component.html',
  styles: [],
})
export class GetemployeepersonalinfoComponent
  extends ParentHrmAdminComponent
  implements OnInit
{
  form!: FormGroup;
  @Input() employeeNumber!: string;
  nationalities: Array<CustomSelectListItem> = [];
  religions: Array<CustomSelectListItem> = [];
  employeeTypes: Array<CustomSelectListItem> = [];
  bloodGroups: Array<CustomSelectListItem> = [];
  genders: Array<CustomSelectListItem> = [];
  maritalStatuses: Array<CustomSelectListItem> = [];
  titles: Array<CustomSelectListItem> = [];
  groups: Array<CustomSelectListItem> = [];
  subGroups: Array<CustomSelectListItem> = [];
  languages: Array<CustomSelectListItem> = [];
  employeeBasicInfo!: EmployeeBasicInfoDto;
  formData!: FormData;
  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private authService: AuthorizeService,
    private utilService: UtilityService,
    public dialogRef: MatDialogRef<GetemployeepersonalinfoComponent>,
    private notifyService: NotificationService,
    private validationService: ValidationService
  ) {
    super(authService);
  }

  ngOnInit(): void {
    this.setForm();
    this.loadNationalities();
    this.loadReligions();
    this.loadEmployeeTypes();
    this.loadBloodGroups();
    this.loadGenders();
    this.loadMaritalStatuses();
    this.loadTitles();
    this.loadLanguages();
    this.employeeBasicInfo = {
      employeeImageUrl: 'assets/images/profile.jpg',
      allowImageUpload: true,
    };
    if (this.employeeNumber != '') this.setEditForm();
  }

  setForm() {
    this.form = this.fb.group({
      primaryNumber: ['', Validators.required],
      employeeName: [''],
      employeeNumber: [''],
      firstNameEn: ['', Validators.required],
      firstNameAr: [''],
      lastNameEn: ['', Validators.required],
      lastNameAr: [''],
      nickNameEn: [''],
      nickNameAr: [''],
      dateOfBirth: ['', Validators.required],
      idNumber1: [''],
      idNumber2: [''],
      fatherNameEn: [''],
      fatherNameAr: [''],
      motherNameEn: [''],
      motherNameAr: [''],
      isActive: [false],
      isPhysicallyChallenged: [false],
      countryCode: ['', Validators.required],
      religionCode: ['', Validators.required],
      employeeTypeCode: ['', Validators.required],
      bloodGroupCode: ['', Validators.required],
      genderCode: ['', Validators.required],
      maritalStatusCode: ['', Validators.required],
      titleCode: ['', Validators.required],
      groupCode: [null],
      subGroupCode: [null],
      marriageDate: [null],
      pHDescription: [''],
      employeeLanguages: this.fb.array([this.CreateemployeeLanguages()]),
    });
  }

  CreateemployeeLanguages(res?: any): FormGroup {
    if (res) {
      return this.fb.group(res);
    }

    return this.fb.group({
      employeeId: [0],
      languageCode: ['', Validators.required],
      canRead: [false],
      canWrite: [false],
      canSpeak: [false],
      id: [0],
    });
  }

  get employeeLanguages(): FormArray {
    return <FormArray>this.form.get('employeeLanguages');
  }
  addItem() {
    this.employeeLanguages.push(this.CreateemployeeLanguages());
  }
  editItem(res: any) {
    this.employeeLanguages.push(this.CreateemployeeLanguages(res));
  }
  removeItem(item: number) {
    this.employeeLanguages.removeAt(item);
  }
  setEditForm() {
    this.apiService
      .getQueryString(
        `PersonalInformation/GetEmployeePersonalInformationById?employeeNumber=`,
        this.employeeNumber
      )
      .subscribe((res) => {
        if (res) {
          this.form.patchValue(res);
          res.allowImageUpload = true;
          this.employeeBasicInfo = res;

          if (
            !(this.employeeBasicInfo.employeeImageUrl as string)?.includes(
              constants.employeeProfile
            )
          )
            this.employeeBasicInfo.employeeImageUrl = `${this.authService
              .ApiEndPoint()
              .replace('api', '')}${this.employeeBasicInfo.employeeImageUrl}`;

          if (res['religionCode'] != null)
            this.loadGroups(res['religionCode'] as string);
          if (res['groupCode'] != null)
            this.loadSubGroups(res['groupCode'] as string);
          this.employeeLanguages.clear();
          let employeeLanguages = res['employeeLanguages'] as Array<any>;
          employeeLanguages.forEach((item) => {
            this.editItem(item);
          });
        }
      });
  }
  closeModel() {
    this.dialogRef.close();
  }

  loadNationalities() {
    this.apiService
      .getall('Country/getCountrySelectListItem')
      .subscribe((res) => {
        this.nationalities = res;
      });
  }
  loadReligions() {
    this.apiService
      .getall('Religion/getReligionSelectListItem')
      .subscribe((res) => {
        this.religions = res;
      });
  }
  loadEmployeeTypes() {
    this.apiService
      .getall('EmployeeType/getEmployeeTypeSelectListItem')
      .subscribe((res) => {
        this.employeeTypes = res;
      });
  }
  loadBloodGroups() {
    this.apiService
      .getall('BloodGroup/getBloodGroupSelectListItem')
      .subscribe((res) => {
        this.bloodGroups = res;
      });
  }
  loadGenders() {
    this.apiService
      .getall('Gender/getGenderSelectListItem')
      .subscribe((res) => {
        this.genders = res;
      });
  }
  loadMaritalStatuses() {
    this.apiService
      .getall('MaritalStatus/getMaritalStatusSelectListItem')
      .subscribe((res) => {
        this.maritalStatuses = res;
      });
  }
  loadTitles() {
    this.apiService.getall('Title/getTitleSelectListItem').subscribe((res) => {
      this.titles = res;
    });
  }

  loadGroups(value: string) {
    const religionCode = value;
    this.apiService
      .getQueryString(
        `Group/getGroupSelectListItem?religionCode=`,
        religionCode
      )
      .subscribe((res) => {
        if (res) {
          this.groups = res;
        }
      });
  }

  loadSubGroups(value: string) {
    const groupCode = value;
    this.apiService
      .getQueryString(
        `SubGroup/getSubGroupSelectListItem?groupCode=`,
        groupCode
      )
      .subscribe((res) => {
        if (res) {
          this.subGroups = res;
        }
      });
  }

  loadLanguages() {
    this.apiService
      .getall('Language/getLanguageSelectListItem')
      .subscribe((res) => {
        this.languages = res;
      });
  }

  // submit() {
  //   console.log(this.employeeLanguages.value);
  //   if (this.form.valid) {
  //     this.formData = new FormData();

  //     if (this.employeeNumber != '')
  //       this.formData.append('employeeNumber', this.employeeNumber);
  //     this.formData.append('primaryNumber', this.form.value.primaryNumber);
  //     this.formData.append('iDNumber1', this.form.value.iDNumber1);
  //     this.formData.append('iDNumber2', this.form.value.iDNumber2);
  //     this.formData.append('firstNameEn', this.form.value.firstNameEn);
  //     this.formData.append('lastNameEn', this.form.value.lastNameEn);
  //     this.formData.append('firstNameAr', this.form.value.firstNameAr);
  //     this.formData.append('lastNameAr', this.form.value.lastNameAr);
  //     this.formData.append('nickNameEn', this.form.value.nickNameEn);
  //     this.formData.append('nickNameAr', this.form.value.nickNameAr);
  //     this.formData.append('fatherNameEn', this.form.value.fatherNameEn);
  //     this.formData.append('motherNameEn', this.form.value.motherNameEn);
  //     this.formData.append('fatherNameAr', this.form.value.fatherNameAr);
  //     this.formData.append('motherNameAr', this.form.value.motherNameAr);
  //     this.formData.append('countryCode', this.form.value.countryCode);
  //     this.formData.append('religionCode', this.form.value.religionCode);
  //     this.formData.append(
  //       'employeeTypeCode',
  //       this.form.value.employeeTypeCode
  //     );
  //     this.formData.append('bloodGroupCode', this.form.value.bloodGroupCode);
  //     this.formData.append('genderCode', this.form.value.genderCode);
  //     this.formData.append(
  //       'maritalStatusCode',
  //       this.form.value.maritalStatusCode
  //     );
  //     this.formData.append('titleCode', this.form.value.titleCode);
  //     this.formData.append('groupCode', this.form.value.groupCode);
  //     this.formData.append('subGroupCode', this.form.value.subGroupCode);
  //     this.formData.append(
  //       'dateOfBirth',
  //       this.utilService.selectedDateTime(this.form.value.dateOfBirth)
  //     );
  //     this.formData.append(
  //       'marriageDate',
  //       this.utilService.selectedDateTime(this.form.value.marriageDate)
  //     );
  //     this.formData.append(
  //       'isPhysicallyChallenged',
  //       this.form.value.isPhysicallyChallenged
  //     );
  //     this.formData.append('pHDescription', this.form.value.pHDescription);
  //     this.formData.append(
  //       'employeeLanguages',
  //       this.form.value.employeeLanguages
  //     );
  //     this.formData.append('isActive', this.form.value.isActive);

  //     if (this.employeeBasicInfo.file) {
  //       this.formData.append(
  //         'file',
  //         this.employeeBasicInfo.file,
  //         this.employeeBasicInfo.file.name
  //       );
  //     }

  //     this.apiService.post('PersonalInformation', this.formData).subscribe(
  //       (res) => {
  //         this.utilService.OkMessage();
  //         this.reset();
  //         this.dialogRef.close(true);
  //       },
  //       (error) => {
  //         this.utilService.ShowApiErrorMessage(error);
  //       }
  //     );
  //   } else this.utilService.FillUpFields();
  // }

  submit() {
    console.log(this.employeeLanguages.value);
    if (this.form.valid) {
      this.formData = new FormData();
      this.formData.append(
        'input',
        JSON.stringify({
          employeeNumber: this.employeeNumber != '' ? this.employeeNumber : '',
          primaryNumber: this.form.value.primaryNumber,
          iDNumber1: this.form.value.iDNumber1,
          iDNumber2: this.form.value.iDNumber2,
          firstNameEn: this.form.value.firstNameEn,
          lastNameEn: this.form.value.lastNameEn,
          firstNameAr: this.form.value.firstNameAr,
          lastNameAr: this.form.value.lastNameAr,
          nickNameEn: this.form.value.nickNameEn,
          nickNameAr: this.form.value.nickNameAr,
          fatherNameEn: this.form.value.fatherNameEn,
          motherNameEn: this.form.value.motherNameEn,
          fatherNameAr: this.form.value.fatherNameAr,
          motherNameAr: this.form.value.motherNameAr,
          countryCode: this.form.value.countryCode,
          religionCode: this.form.value.religionCode,
          employeeTypeCode: this.form.value.employeeTypeCode,
          bloodGroupCode: this.form.value.bloodGroupCode,
          genderCode: this.form.value.genderCode,
          maritalStatusCode: this.form.value.maritalStatusCode,
          titleCode: this.form.value.titleCode,
          groupCode: this.form.value.groupCode,
          subGroupCode: this.form.value.subGroupCode,
          dateOfBirth: this.utilService.selectedDateTime(
            this.form.value.dateOfBirth
          ),
          marriageDate: this.utilService.selectedDateTime(
            this.form.value.marriageDate
          ),
          isPhysicallyChallenged: this.form.value.isPhysicallyChallenged,
          pHDescription: this.form.value.pHDescription,
          employeeLanguages: this.form.value.employeeLanguages,
          isActive: this.form.value.isActive,
        })
      );

      if (this.employeeBasicInfo.file) {
        this.formData.append(
          'file',
          this.employeeBasicInfo.file,
          this.employeeBasicInfo.file.name
        );
      }

      this.apiService.post('PersonalInformation', this.formData).subscribe(
        (res) => {
          this.utilService.OkMessage();
          this.reset();
          this.dialogRef.close(true);
        },
        (error) => {
          this.utilService.ShowApiErrorMessage(error);
        }
      );
    } else this.utilService.FillUpFields();
  }

  addForm() {
    this.form.value['employeeLanguages'].push({
      employeeId: Number(this.employeeNumber),
      languageCode: '',
      canRead: false,
      canWrite: false,
      canSpeak: false,
      id: 0,
    });
  }
  dltForm(i: any) {
    this.form.value['employeeLanguages'].splice(i, 1);
  }

  reset() {
    this.form.controls['primaryNumber'].setValue('');
    this.form.controls['iDNumber1'].setValue('');
    this.form.controls['iDNumber2'].setValue('');
    this.form.controls['employeeNumber'].setValue('');
    this.form.controls['firstNameEn'].setValue('');
    this.form.controls['lastNameEn'].setValue('');
    this.form.controls['firstNameAr'].setValue('');
    this.form.controls['lastNameAr'].setValue('');
    this.form.controls['nickNameEn'].setValue('');
    this.form.controls['nickNameAr'].setValue('');
    this.form.controls['fatherNameEn'].setValue('');
    this.form.controls['motherNameEn'].setValue('');
    this.form.controls['fatherNameAr'].setValue('');
    this.form.controls['motherNameAr'].setValue('');
    this.form.controls['countryCode'].setValue('');
    this.form.controls['religionCode'].setValue('');
    this.form.controls['employeeTypeCode'].setValue('');
    this.form.controls['bloodGroupCode'].setValue('');
    this.form.controls['genderCode'].setValue('');
    this.form.controls['maritalStatusCode'].setValue('');
    this.form.controls['titleCode'].setValue('');
    this.form.controls['groupCode'].setValue('');
    this.form.controls['subGroupCode'].setValue('');
    this.form.controls['dateOfBirth'].setValue('');
    this.form.controls['marriageDate'].setValue('');
    this.form.controls['isPhysicallyChallenged'].setValue('');
    this.form.controls['pHDescription'].setValue('');
    this.form.controls['isActive'].setValue('');
  }

  employeeBasicInfoChange(employeeBasicInfo: EmployeeBasicInfoDto) {
    this.employeeBasicInfo = employeeBasicInfo;
  }
}
