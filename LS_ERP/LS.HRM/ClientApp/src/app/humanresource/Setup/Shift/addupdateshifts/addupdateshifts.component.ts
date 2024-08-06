import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from 'src/app/api-authorization/AuthorizeService';
import { ApiService } from 'src/app/services/api.service';
import { NotificationService } from 'src/app/services/notification.service';
import { DBOperation } from 'src/app/services/utility.constants';
import { UtilityService } from 'src/app/services/utility.service';
import { ParentHrmAdminComponent } from 'src/app/sharedcomponent/ParentHrmAdmin.component';
import { ValidationService } from 'src/app/sharedcomponent/ValidationService';
import { AddupdatereligionsComponent } from '../../Religion/addupdatereligions/addupdatereligions.component';

@Component({
  selector: 'app-addupdateshifts',
  templateUrl: './addupdateshifts.component.html',
})
export class AddupdateshiftsComponent extends ParentHrmAdminComponent implements OnInit {

  modalTitle!: string;
  modalBtnTitle!: string;
  dbops!: DBOperation;
  form!: FormGroup;
  id: number = 0;
  isReadOnly: boolean = false;

  constructor(private fb: FormBuilder, private apiService: ApiService, private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdatereligionsComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {
    super(authService)
  }

  ngOnInit(): void {
    this.setForm();
    if (this.id > 0)
      this.setEditForm();
  }
  setForm() {
    this.form = this.fb.group(
      {
        'shiftCode': ['', Validators.required],
        'shiftNameEn': ['', Validators.required],
        'shiftNameAr': [''],
        'inTime': ['', Validators.required],
        'outTime': ['', Validators.required],
        'breakTime': [''],
        'inGrace': [''],
        'outGrace': [''],
        'isActive': [false],
      }
    );
    this.isReadOnly = false;
  }
  setEditForm() {
    this.apiService.get('Shift', this.id).subscribe(res => {
      if (res) {
        this.isReadOnly = true;
        res.inTime = this.utilService.formatTime(res.inTime);
        res.outTime = this.utilService.formatTime(res.outTime);
        res.breakTime = this.utilService.formatTime(res.breakTime);
        res.inGrace = this.utilService.formatTime(res.inGrace);
        res.outGrace = this.utilService.formatTime(res.outGrace);
        this.form.patchValue(res);
      }
    });
  }

  onTimeInputChange(event: Event) {
    const inputElement = event.target as HTMLInputElement;
    let inputValue = inputElement.value.trim();
    if (inputValue.length >= 5) {
      // Remove any non-numeric characters
      inputValue = inputValue.replace(/[^0-9:]/g, '');

      // Ensure the input is in HH:mm format
      const timeRegex = /^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$/;
      if (timeRegex.test(inputValue)) {
        inputElement.value = inputValue;
      } else {
        // Clear the input if the format is incorrect
        inputElement.value = inputValue.slice(0, inputValue.length - 1);
      }
    }
    else if (inputValue.length == 2)
      inputElement.value = inputValue + ":";
    else
      inputElement.value = inputValue;
  }

  closeModel() {
    this.dialogRef.close();
  }
  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      this.apiService.post('Shift', this.form.value)
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
    this.form.controls['shiftCode'].setValue('');
    this.form.controls['shiftNameEn'].setValue('');
    this.form.controls['shiftNameAr'].setValue('');
    this.form.controls['inTime'].setValue('');
    this.form.controls['outTime'].setValue('');
    this.form.controls['inGrace'].setValue('');
    this.form.controls['outGrace'].setValue('');
    this.form.controls['isActive'].setValue('');
  }
}
