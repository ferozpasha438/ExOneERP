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
import { CustomSelectListItem } from 'src/app/models/MenuItemListDto';

@Component({
  selector: 'app-addupdatequalifications',
  templateUrl: './addupdatequalifications.component.html',
  styles: [
  ]
})
export class AddupdatequalificationsComponent extends ParentHrmAdminComponent implements OnInit {
  modalTitle!: string;
  modalBtnTitle!: string;
  dbops!: DBOperation;
  form!: FormGroup;
  id: number = 0;
  isReadOnly: boolean = false;
  degreeTypes: Array<CustomSelectListItem> = [];

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdatequalificationsComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {
    super(authService)
  };


  ngOnInit(): void {
    this.setForm();
    this.loadDegreeTypes();
    if (this.id > 0)
      this.setEditForm();
  }

  setForm() {
    this.form = this.fb.group(
      {
        'qualificationCode': ['', Validators.required],
        'qualificationNameEn': ['', Validators.required],
        'qualificationNameAr': [''],
        'isTechnicalQualification': [false],
        'degreeTypeCode': ['', Validators.required],
        'isActive': [false],
      }
    );
  }
  setEditForm() {
    this.apiService.get('Qualification', this.id).subscribe(res => {
      if (res) {
        this.isReadOnly=true;
        this.form.patchValue(res);
      }
    });
  }
  closeModel() {
    this.dialogRef.close();
  }

  loadDegreeTypes() {
    this.apiService.getall('DegreeType/GetDegreeTypeSelectListItem').subscribe(res => {
      this.degreeTypes = res;
    });
  }

  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      this.apiService.post('Qualification', this.form.value)
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
    this.form.controls['qualificationCode'].setValue('');
    this.form.controls['qualificationNameEn'].setValue('');
    this.form.controls['qualificationNameAr'].setValue('');
    this.form.controls['isTechnicalQualification'].setValue(false);
    this.form.controls['degreeTypeCode'].setValue('');
    this.form.controls['isActive'].setValue(false);
  }
}
