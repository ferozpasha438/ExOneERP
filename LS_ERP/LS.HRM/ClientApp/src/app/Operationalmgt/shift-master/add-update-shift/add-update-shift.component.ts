import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { UtilityService } from '../../../services/utility.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { NotificationService } from '../../../services/notification.service';
import { ApiService } from '../../../services/api.service';
import { TranslateService } from '@ngx-translate/core';
import { AddUpdateEmployeeComponent } from '../../employee/add-update-employee/add-update-employee.component';
import { DBOperation } from '../../../services/utility.constants';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';
import { MatDialogRef } from '@angular/material/dialog';
import { Time } from '@angular/common';

@Component({
  selector: 'app-add-update-shift',
  templateUrl: './add-update-shift.component.html'
})
export class AddUpdateShiftComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
  shiftId: number = 0;
  isDataLoading: boolean = false;
  readonly: string = "";
  isOff: boolean = false;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<AddUpdateShiftComponent>) {
    super(authService);
  }

  ngOnInit(): void {
    this.setForm();
    //this.loadBranchCodes();
    if (this.id > 0) {
      this.shiftId = this.id;
      this.setEditForm();
      this.readonly = "readonly";
    }
  }
  closeModel() {
    this.dialogRef.close();
  }
  setForm() {
    this.form = this.fb.group({
      //'': ['', Validators.required],
      'shiftId': [0],
      'shiftCode': ['', Validators.required],
      'shiftName_EN': ['', Validators.required],
      'shiftName_AR': ['', Validators.required],
      'inTime': ['', Validators.required],
      'outTime': ['', Validators.required],
      'breakTime': ['', Validators.required],
      'inGrace': [''],
      'outGrace': [''],
      'workingTime': [''],
      'netWorkingTime': [''],
      'isOff': [false]
      
    });
  }
  submit() {
    console.log(this.form.value);

    let validate = this.form.controls['isOff'].value && this.form.controls['shiftCode'].value != '' && this.form.controls['shiftName_EN'].value != '' && this.form.controls['shiftName_AR'].value != '';
  


    if (this.form.valid || validate ) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      this.form.value['shiftId'] = this.id;
      this.apiService.post('ShiftMaster', this.form.value)
        .subscribe(res => {
          if (res) {

            this.utilService.OkMessage();
            this.reset();
            this.dialogRef.close(true);

          }
        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
          });
    }
    else
      this.utilService.FillUpFields();

  }

  setEditForm() {
    this.apiService.get('ShiftMaster/getShiftMasterById', this.shiftId).subscribe(res => {
      if (res) {
        this.form.patchValue(res);
        this.form.patchValue({ 'shiftId': 0 });
        this.form.patchValue({ 'id': 0 });

      }
    });
  }








  reset() {
    this.form.controls['shiftCode'].setValue('');
    this.form.controls['shiftName_EN'].setValue('');
    this.form.controls['shiftName_AR'].setValue('');
    this.form.controls['inTime'].setValue('');
    this.form.controls['outTime'].setValue('');
    this.form.controls['breakTime'].setValue('');
    this.form.controls['inGrace'].setValue('');
    this.form.controls['outGrace'].setValue('');
    this.form.controls['workingTime'].setValue('');
    this.form.controls['netWorkingTime'].setValue('');
    
    this.form.controls['isOff'].setValue(false);
  }
  setDisable() {
    if (this.form.controls['isOff'].value) {

      this.form.controls['inTime'].disable({ onlySelf: true });
      this.form.controls['outTime'].disable({ onlySelf: true });
      this.form.controls['breakTime'].disable({ onlySelf: true });
      this.form.controls['inGrace'].disable({ onlySelf: true });
      this.form.controls['outGrace'].disable({ onlySelf: true });
      this.form.controls['workingTime'].disable({ onlySelf: true });
      this.form.controls['netWorkingTime'].disable({ onlySelf: true });

      this.form.value['inTime']="00:00";
      this.form.value['outTime']= "00:00";
      this.form.value['breakTime'] = 0;
      this.form.value['inGrace'] = 0;
      this.form.value['outGrace']= 0;
      this.form.value['workingTime'] = '';
      this.form.value['netWorkingTime'] = '';
    }
    else {

      this.form.controls['inTime'].enable({ onlySelf: true });
      this.form.controls['outTime'].enable({ onlySelf: true });
      this.form.controls['breakTime'].enable({ onlySelf: true });
      this.form.controls['inGrace'].enable({ onlySelf: true });
      this.form.controls['outGrace'].enable({ onlySelf: true });
      this.form.controls['workingTime'].enable({ onlySelf: true });
      this.form.controls['netWorkingTime'].enable({ onlySelf: true });



    }

  }


}
