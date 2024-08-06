import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { DBOperation } from '../../services/utility.constants';
import { UtilityService } from '../../services/utility.service';
import { ParentSystemSetupComponent } from '../../sharedcomponent/parentsystemsetup.component';
import { ValidationService } from '../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-addupdatecity',
  templateUrl: './addupdatecity.component.html',
  styles: [
  ],
})
export class AddupdatecityComponent extends ParentSystemSetupComponent implements OnInit {
  form: FormGroup;;
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  id: number;

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdatecityComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {
    super(authService);
  }

  ngOnInit(): void {
    this.setForm();
  }

  setForm() {
    this.form = this.fb.group({
      'code': ['', Validators.required],
      'name': ['', Validators.required],
      //'stateCode': ['', Validators.required],
      'countryCode': ['', Validators.required],
    });
  }


  submit() {
    if (this.form.valid) {
      this.apiService.post('city', this.form.value)
        .subscribe(res => {
          this.reset();
          this.dialogRef.close();
          this.utilService.OkMessage();
        },
          error => {
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

}
