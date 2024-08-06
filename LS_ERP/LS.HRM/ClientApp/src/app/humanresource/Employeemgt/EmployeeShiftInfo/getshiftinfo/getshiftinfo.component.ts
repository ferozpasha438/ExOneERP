import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from 'src/app/api-authorization/AuthorizeService';
import { CustomSelectListItem } from 'src/app/models/MenuItemListDto';
import { ApiService } from 'src/app/services/api.service';
import { UtilityService } from 'src/app/services/utility.service';
import { ParentHrmAdminComponent } from 'src/app/sharedcomponent/ParentHrmAdmin.component';
import { GetemployeepersonalinfoComponent } from '../../Employeepersonalinfo/getemployeepersonalinfo.component';

@Component({
  selector: 'app-getshiftinfo',
  templateUrl: './getshiftinfo.component.html',
})
export class GetshiftinfoComponent
  extends ParentHrmAdminComponent
  implements OnInit
{
  form!: FormGroup;
  @Input() employeeNumber!: string;
  shifts: Array<CustomSelectListItem> = [];
  employeeBasicInfo: any;
  id: number = 0;
  selectedEmployee: string = '';
  employees: Array<CustomSelectListItem> = [];

  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private authService: AuthorizeService,
    private utilService: UtilityService,
    public dialogRef: MatDialogRef<GetemployeepersonalinfoComponent>,
  ) {
    super(authService);
  }

  ngOnInit(): void {
    this.loadShifts();
    this.loadCopyFromEmployeesList();
    this.getEmployeeBasicInfo();
    this.setForm();
  }

  loadShifts() {
    this.apiService.getall('Shift/getShiftSelectListItem').subscribe((res) => {
      this.shifts = res;
    });
  }

  loadCopyFromEmployeesList() {
    this.apiService
      .getall('EmployeeShift/getCopyFromEmployeesList')
      .subscribe((res) => {
        this.employees = res;
      });
  }

  setForm() {
    this.form = this.fb.group({
      mondayShiftCode: ['', Validators.required],
      tuesdayShiftCode: ['', Validators.required],
      wednesdayShiftCode: ['', Validators.required],
      thursdayShiftCode: ['', Validators.required],
      fridayShiftCode: ['', Validators.required],
      saturdayShiftCode: ['', Validators.required],
      sundayShiftCode: ['', Validators.required],
      isActive: [false],
    });
    this.GetEmployeeShifts(Number(this.employeeNumber).toString(), false);
  }

  public GetEmployeeShifts(employeeID: string, isCopyFromEmployee: boolean) {
    this.apiService
      .getQueryString(
        `EmployeeShift/GetEmployeeShiftById?employeeID=`,
        employeeID
      )
      .subscribe((res) => {
        if (res) {
          this.form.patchValue(res);
          if (!isCopyFromEmployee) {
            if (res['id'] != null) this.id = Number(res['id']);
          }
        }
      });
  }

  private getEmployeeBasicInfo() {
    this.apiService
      .getQueryString(
        `PersonalInformation/GetEmployeePersonalInformationById?employeeNumber=`,
        this.employeeNumber
      )
      .subscribe((res) => {
        if (res) {
          res.allowImageUpload = false;
          this.employeeBasicInfo = res;
        }
      });
  }
  closeModel() {
    this.dialogRef.close();
  }

  submit() {
    if (this.form.valid) {
      if (this.id > 0) this.form.value['id'] = this.id;
      this.form.value['employeeID'] = Number(this.employeeNumber);
      this.apiService.post('EmployeeShift', this.form.value).subscribe(
        (res) => {
          this.utilService.OkMessage();
        },
        (error) => {
          this.utilService.ShowApiErrorMessage(error);
        }
      );
    } else this.utilService.FillUpFields();
  }

  reset() {
    this.form.controls['mondayShiftCode'].setValue('');
    this.form.controls['tuesdayShiftCode'].setValue('');
    this.form.controls['wednesdayShiftCode'].setValue('');
    this.form.controls['thursdayShiftCode'].setValue('');
    this.form.controls['fridayShiftCode'].setValue('');
    this.form.controls['saturdayShiftCode'].setValue('');
    this.form.controls['sundayShiftCode'].setValue('');
    this.form.controls['isActive'].setValue('');
  }
}
