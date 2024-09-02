import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from 'src/app/api-authorization/AuthorizeService';
import { CustomSelectListItem } from 'src/app/models/MenuItemListDto';
import { ApiService } from 'src/app/services/api.service';
import { NotificationService } from 'src/app/services/notification.service';
import { DBOperation } from 'src/app/services/utility.constants';
import { UtilityService } from 'src/app/services/utility.service';
import { ParentHrmAdminComponent } from 'src/app/sharedcomponent/ParentHrmAdmin.component';
import { ValidationService } from 'src/app/sharedcomponent/ValidationService';

@Component({
  selector: 'app-addupdatevacationpolicy',
  templateUrl: './addupdatevacationpolicy.component.html',
  styles: [],
})
export class AddupdatevacationpolicyComponent
  extends ParentHrmAdminComponent
  implements OnInit
{
  modalTitle!: string;
  modalBtnTitle!: string;
  dbops!: DBOperation;
  form!: FormGroup;
  id: number = 0;
  isReadOnly: boolean = false;
  positions: Array<CustomSelectListItem> = [];
  grades: Array<CustomSelectListItem> = [];

  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private authService: AuthorizeService,
    private utilService: UtilityService,
    public dialogRef: MatDialogRef<AddupdatevacationpolicyComponent>,
    private notifyService: NotificationService,
    private validationService: ValidationService
  ) {
    super(authService);
  }

  ngOnInit(): void {
    this.setForm();
    if (this.id > 0) this.setEditForm();
  }

  setForm() {
    this.form = this.fb.group({
      vacationPolicyCode: ['', Validators.required],
      vacationPolicyNameEn: ['', Validators.required],
      vacationPolicyNameAr: [''],
      gradeCode: [''],
      positionCode: [''],
      annualVacationDays: [0, Validators.required],
      maximumDaysAllowed: [0, Validators.required],
      vacationDurationInMonths: [0, Validators.required],
      isAirTicketAllowed: [false],
      airTicketDurationInMonths: [0],
      isFamilyTicketAllowed: [false],
      familyAirTicketDuration: [0],
      isExitReEntryRequired: [false],
      isAdvanceVacationPayAllowed: [false],
      isVacationExtensionAllowed: [false],
      isActive: [false],
    });
    this.isReadOnly = false;
    this.loadGrades();
    this.loadPositions();
  }

  setEditForm() {
    this.apiService.get('VacationPolicy', this.id).subscribe((res) => {
      if (res) {
        this.isReadOnly = true;
        this.form.patchValue(res);
      }
    });
  }

  closeModel() {
    this.dialogRef.close();
  }

  submit() {
    if (this.form.valid) {
      if (this.id > 0) this.form.value['id'] = this.id;
      this.apiService.post('VacationPolicy', this.form.value).subscribe(
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

  reset() {
    this.form.controls['vacationPolicyCode'].setValue('');
    this.form.controls['vacationPolicyNameEn'].setValue('');
    this.form.controls['vacationPolicyNameAr'].setValue('');
    this.form.controls['gradeCode'].setValue('');
    this.form.controls['positionCode'].setValue('');
    this.form.controls['annualVacationDays'].setValue(0);
    this.form.controls['maximumDaysAllowed'].setValue(0);
    this.form.controls['vacationDurationInMonths'].setValue(0);
    this.form.controls['isAirTicketAllowed'].setValue(false);
    this.form.controls['airTicketDurationInMonths'].setValue(0);
    this.form.controls['isFamilyTicketAllowed'].setValue(false);
    this.form.controls['familyAirTicketDuration'].setValue(0);
    this.form.controls['isExitReEntryRequired'].setValue(false);
    this.form.controls['isAdvanceVacationPayAllowed'].setValue(false);
    this.form.controls['isVacationExtensionAllowed'].setValue(false);
    this.form.controls['isActive'].setValue(false);
  }

  loadPositions() {
    this.apiService
      .getall('Position/GetPositionSelectListItem')
      .subscribe((res) => {
        this.positions = res;
      });
  }

  loadGrades() {
    this.apiService.getall('Grade/GetGradeSelectListItem').subscribe((res) => {
      this.grades = res;
    });
  }
}
