import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { DBOperation } from '../../../../services/utility.constants';
import { UtilityService } from '../../../../services/utility.service';
import { ParentHrmAdminComponent } from '../../../../sharedcomponent/ParentHrmAdmin.component';
import { ValidationService } from '../../../../sharedcomponent/ValidationService';
import { CustomSelectListItem } from 'src/app/models/MenuItemListDto';

@Component({
  selector: 'app-addupdateeducationinfo',
  templateUrl: './addupdateeducationinfo.component.html',
  styles: [
  ]
})
export class AddupdateeducationinfoComponent extends ParentHrmAdminComponent implements OnInit {
  modalTitle!: string;
  modalBtnTitle!: string;
  dbops!: DBOperation;
  form!: FormGroup;
  id: number = 0;
  employeeNumber!: string;
  degreeTypes: Array<CustomSelectListItem> = [];
  courseTypes: Array<CustomSelectListItem> = [];
  qualifications: Array<CustomSelectListItem> = [];
  countries: Array<CustomSelectListItem> = [];
  isTechnicalQualification!: boolean;
  degreeTypeCode!: string;

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateeducationinfoComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {
    super(authService)
  };

  ngOnInit(): void {
    this.loadDegreeTypes();
    this.loadCourseTypes();
    this.loadCountries();
    this.setForm();
    if (this.id > 0)
      this.setEditForm();
  }

  loadDegreeTypes() {
    this.apiService.getall('DegreeType/GetDegreeTypeSelectListItem').subscribe(res => {
      this.degreeTypes = res;
    });
  }

  loadQualifications() {
    this.isTechnicalQualification = this.form.get('isTechnicalQualification')?.value;
    this.degreeTypeCode = this.form.get('degreeTypeCode')?.value;

    let queryParam = `isTechnicalQualification=${encodeURIComponent("" + this.isTechnicalQualification)}&degreeTypeCode=${encodeURIComponent("" + this.degreeTypeCode)}`;
    this.apiService.getQueryString(`Qualification/GetQualificationSelectListItem?`, queryParam).subscribe(res => {
      this.qualifications = res;
    });
  }

  loadCourseTypes() {
    this.apiService.getall('CourseType/GetCourseTypeSelectListItem').subscribe(res => {
      this.courseTypes = res;
    });
  }

  loadCountries() {
    this.apiService.getall('Country/getCountrySelectListItem').subscribe(res => {
      this.countries = res;
    });
  }

  setForm() {
    this.form = this.fb.group(
      {
        'employeenumber': [this.employeeNumber],
        'degreeTypeCode': ['', Validators.required],
        'isTechnicalQualification': [false],
        'qualificationCode': ['', Validators.required],
        'courseTypeCode': [''],
        'dateOfCertification': ['', Validators.required],
        'collegeOrUniversity': ['', Validators.required],
        'countryCode': ['', Validators.required],
        'remarks': ['', Validators.required],
        'isActive': [true],
      }
    );
  }

  setEditForm() {
    let queryParam = `id=${encodeURIComponent("" + this.id)}&employeeID=${encodeURIComponent("" + Number(this.employeeNumber))}`;
    this.apiService.getQueryString(`EmployeeQualification/GetEmployeeQualificationById?`, queryParam).subscribe(res => {
      if (res) {
        this.form.patchValue(res);
        if(res['degreeTypeCode'] != null && res['isTechnicalQualification'] != null)
        this.loadQualifications();
      }
    });
  }

  closeModel() {
    this.dialogRef.close();
  }

  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      this.form.value['employeeID'] = Number(this.employeeNumber);
      this.form.value['dateOfCertification'] = this.utilService.selectedDateTime(this.form.controls['dateOfCertification'].value);
      this.apiService.post('EmployeeQualification', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.reset();
          this.dialogRef.close(true);
        },
          error => {
            this.utilService.ShowApiErrorMessage(error);
          });
    }
    else
      this.utilService.FillUpFields();
  }

  reset() {
    this.form.controls['employeenumber'].setValue(this.employeeNumber);
    this.form.controls['degreeTypeCode'].setValue('');
    this.form.controls['isTechnicalQualification'].setValue(false);
    this.form.controls['qualificationCode'].setValue('');
    this.form.controls['courseTypeCode'].setValue('');
    this.form.controls['dateOfCertification'].setValue('');
    this.form.controls['collegeOrUniversity'].setValue('');
    this.form.controls['countryCode'].setValue('');
    this.form.controls['remarks'].setValue('');
    this.form.controls['isActive'].setValue(true);
  }
}
