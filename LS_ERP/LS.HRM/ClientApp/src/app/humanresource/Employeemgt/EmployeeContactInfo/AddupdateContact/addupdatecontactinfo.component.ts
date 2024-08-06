import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { DBOperation } from '../../../../services/utility.constants';
import { UtilityService } from '../../../../services/utility.service';
import { ParentHrmAdminComponent } from '../../../../sharedcomponent/ParentHrmAdmin.component';
import { ValidationService } from '../../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-addupdatecontactinfo',
  templateUrl: './addupdatecontactinfo.component.html',
  styles: [
  ]
})
export class AddupdatecontactinfoComponent extends ParentHrmAdminComponent implements OnInit {

  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdatecontactinfoComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {
    super(authService)
  };

  ngOnInit(): void {
    this.setForm();
    if (this.id > 0)
      this.setEditForm();
  }

  setForm() {
    this.form = this.fb.group(
      {
        'employeenumber': ['', Validators.required],
        'contactNumber': ['', Validators.required],
        'contactpersonName': ['', Validators.required],
        'remarks': ['', Validators.required],
        'contactAddress': ['', Validators.required],
        'isEmergency': ['', Validators.required],
        'relation': ['', Validators.required],
        'isFamily': ['', Validators.required],
        
      }
    );
  }
  setEditForm() {
    this.apiService.get('ContactInfo', this.id).subscribe(res => {
      if (res) {
        this.form.patchValue(res);
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
      this.apiService.post('ContactInfo', this.form.value)
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
    this.form.controls['employeenumber'].setValue('');
    this.form.controls['contactNumber'].setValue('');
    this.form.controls['contactpersonName'].setValue('');
    this.form.controls['remarks'].setValue('');
    this.form.controls['contactAddress'].setValue('');
    this.form.controls['isEmergency'].setValue('');
    this.form.controls['relation'].setValue('');
    this.form.controls['isFamily'].setValue('');
  }
}
