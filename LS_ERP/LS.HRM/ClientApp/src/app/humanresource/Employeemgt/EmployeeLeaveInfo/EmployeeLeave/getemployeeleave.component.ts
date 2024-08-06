import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from 'src/app/api-authorization/AuthorizeService';
import { CustomSelectListItem } from 'src/app/models/MenuItemListDto';
import { ApiService } from 'src/app/services/api.service';
import { NotificationService } from 'src/app/services/notification.service';
import { UtilityService } from 'src/app/services/utility.service';
import { ParentHrmAdminComponent } from 'src/app/sharedcomponent/ParentHrmAdmin.component';
import { EmployeemanagementtabsComponent } from '../../Sharedcomponent/employeemanagementtabs/employeemanagementtabs.component';
import { TblHRMTrnEmployeeLeaveInformationDto } from 'src/app/models/HumanResource/EmployeeLeaveInformationDto';

@Component({
  selector: 'app-getemployeeleave',
  templateUrl: './getemployeeleave.component.html',
  styles: [],
})
export class GetemployeeleaveComponent
  extends ParentHrmAdminComponent
  implements OnInit
{
  form!: FormGroup;
  @Input() employeeNumber!: string;
  leaveTemplates: Array<CustomSelectListItem> = [];
  employeeBasicInfo: any;
  gradeCode!: string;
  positionCode!: string;
  isArab: boolean = false;
  employeeLeaves: Array<TblHRMTrnEmployeeLeaveInformationDto> = [];
  employeeName!: string;

  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private authService: AuthorizeService,
    private translate: TranslateService,
    private utilService: UtilityService,
    private notifyService: NotificationService,
    public dialogRef: MatDialogRef<EmployeemanagementtabsComponent>
  ) {
    super(authService);
  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.setForm();
    if (this.employeeNumber != '') this.setEditForm();
  }

  setForm() {
    this.form = this.fb.group({
      leaveTemplateCode: ['', Validators.required],
      employeeLeaves: this.fb.array([]),
    });
  }

  setEditForm() {
    this.GetEmployeePersonalInformation();
    this.GetEmployeeContractInformation();
  }

  get employeeLeavesFrmArray(): FormArray {
    return <FormArray>this.form.get('employeeLeaves');
  }

  editItem(res: any) {
    this.employeeLeavesFrmArray.push(this.fb.group(res));
  }

  GetEmployeePersonalInformation() {
    this.apiService
      .getQueryString(
        `PersonalInformation/GetEmployeePersonalInformationById?employeeNumber=`,
        this.employeeNumber
      )
      .subscribe((res) => {
        if (res) {
          res.allowImageUpload = false;
          this.employeeBasicInfo = res;
          this.employeeName = res['employeeName'];
        }
      });
  }

  GetEmployeeContractInformation() {
    let queryParam = `employeeID=${encodeURIComponent(
      '' + Number(this.employeeNumber)
    )}`;
    this.apiService
      .getQueryString(
        `EmployeeContract/GetEmployeeContractInformationById?`,
        queryParam
      )
      .subscribe((res) => {
        if (res) {
          if (res['gradeCode'] != null)
            this.gradeCode = res['gradeCode'] as string;
          if (res['positionCode'] != null)
            this.positionCode = res['positionCode'] as string;
          this.loadLeaveTemplates();
          this.GetEmployeeLeaveInformation();
        }
      });
  }

  loadLeaveTemplates() {
    let queryParam = `GradeCode=${encodeURIComponent(
      '' + this.gradeCode
    )}&PositionCode=${encodeURIComponent('' + this.positionCode)}`;
    this.apiService
      .getQueryString(
        `LeaveTemplate/GetLeaveTemplateSelectListItem?`,
        queryParam
      )
      .subscribe((res) => {
        this.leaveTemplates = res;
      });
  }

  closeModel() {
    this.dialogRef.close();
  }

  GetEmployeeLeaveInformation() {
    this.apiService
      .get('EmployeeLeave', Number(this.employeeNumber))
      .subscribe((res) => {
        if (res) {
          //Remove all items.
          this.employeeLeaves.splice(0,this.employeeLeaves.length);
          if (res['leaveTemplateCode'] == '')
            this.form.controls['leaveTemplateCode'].setValue('');
          else
            this.form.controls['leaveTemplateCode'].setValue(
              res['leaveTemplateCode']
            );
          let employeeLeaves = res[
            'employeeLeaves'
          ] as Array<TblHRMTrnEmployeeLeaveInformationDto>;

          employeeLeaves.forEach((item) => {
            let employeeLeaveMapping: TblHRMTrnEmployeeLeaveInformationDto =
              item as TblHRMTrnEmployeeLeaveInformationDto;
            employeeLeaveMapping.isUpdate = false;
            this.employeeLeaves.push(employeeLeaveMapping);
          });
        }
      });
  }

  ImportDetailsFromLeaveTemplate() {
    if (this.form.controls['leaveTemplateCode'].value) {
      let queryParam = `TemplateCode=${encodeURIComponent(
        '' + this.form.controls['leaveTemplateCode'].value
      )}`;
      this.apiService
        .getQueryString(
          'EmployeeLeave/GetEmployeeLeaveTemplateMappings?',
          queryParam
        )
        .subscribe((res) => {
          if (res) {
            //Remove all items.
            this.employeeLeaves.splice(0,this.employeeLeaves.length);

            let leaveTemplateMappings =
              res as Array<TblHRMTrnEmployeeLeaveInformationDto>;
            leaveTemplateMappings.forEach((item) => {
              let employeeLeaveMapping: TblHRMTrnEmployeeLeaveInformationDto =
                item as TblHRMTrnEmployeeLeaveInformationDto;
              employeeLeaveMapping.employeeID = Number(this.employeeNumber);
              employeeLeaveMapping.employeeName = this.employeeName;
              employeeLeaveMapping.isUpdate = false;
              this.employeeLeaves.push(employeeLeaveMapping);
            });
          }
        });
    }
  }

  submit() {
    if (this.form.valid) {
      if (this.employeeLeaves.length > 0) {
        this.employeeLeaves.forEach((item) => {
          this.editItem(item);
        });
      }
      this.apiService.post('EmployeeLeave', this.form.value).subscribe(
        (res) => {
          this.utilService.OkMessage();
          this.GetEmployeeLeaveInformation();
        },
        (error) => {
          this.utilService.ShowApiErrorMessage(error);
        }
      );
    } else this.utilService.FillUpFields();
  }

  EditemployeeLeaves(i: number) {
    this.employeeLeaves[i].isUpdate = true;
  }

  UpdateEmployeeLeave(i: number) {
    let assignedElement: any = document.getElementById(
      'assigned-' + i.toString()
    );
    let remarksElement: any = document.getElementById(
      'remarks-' + i.toString()
    );
    let assigned = assignedElement.value;
    let remarks = remarksElement.value;
    this.employeeLeaves[i].assigned = Number(assigned);
    this.employeeLeaves[i].remarks = remarks;
    this.employeeLeaves[i].isUpdate = false;
  }

  DeleteEmployeeLeave(i: number) {
    this.employeeLeaves.splice(i, 1);
  }

  Cancel(i: number) {
    this.employeeLeaves[i].isUpdate = false;
  }
}
