import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';
import { OprServicesService } from '../../opr-services.service';
import Validation from '../../Validators/custom.validators/custom.validators.component';
@Component({
  selector: 'app-add-updatelogisticsandvehicle',
  templateUrl: './add-updatelogisticsandvehicle.component.html'
})
export class AddUpdatelogisticsandvehicleComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
  isDataLoading: boolean = false;
  readonly: string = "";
  constructor(public dialogRef: MatDialogRef<AddUpdatelogisticsandvehicleComponent>, private authService: AuthorizeService, private fb: FormBuilder, private apiService: ApiService, private utilService: UtilityService, private opservice: OprServicesService) {
    super(authService);
  }


  ngOnInit(): void {
    this.setForm();
    if (this.id > 0) {
      this.setEditForm();
      this.readonly = "readonly";
    }
  }


  setForm() {
    this.form = this.fb.group({
      //'': ['', Validators.required],
      'vehicleNumber': ['', Validators.required],
      'vehicleNameInEnglish': ['', Validators.required],
      'vehicleNameInArabic': ['', Validators.required],
      'dailyFuelCost': ['', Validators.required],
      'dailyMiscCost': ['', Validators.required],
      'estimatedDailyMaintenanceCost': ['', Validators.required],
      'otherDailyOperationCost': ['', Validators.required],
      'totalDailyServiceCost': [0],
      'dailyServicePrice': ['', Validators.required],
      'valueofVehicle': ['', Validators.required],
      'vehicletype': ['', Validators.required],
      'minMargin': ['', Validators.required],
      'isActive': [true, Validators.required],
      'remarks': [''],
      'isExist': [false]

    },
      {
        validators: [Validation.isCodeExist('vehicleNumber','isExist')]
      });
  }
  closeModel() {
    this.dialogRef.close();
  }
  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      this.apiService.post('Logisticsandvehicle', this.form.value)
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
    this.apiService.get('Logisticsandvehicle/getLogisticsandvehicleById', this.id).subscribe(res => {
      if (res) {
        this.form.patchValue(res);
        this.form.patchValue({ 'id': 0 });

      }
    });
  }
  reset() {
    this.form.reset();
  }






  verifyCode(event: any) {
    let code = event.target.value.toUpperCase();
    if (code != '')
      this.opservice.verifyCode('Logisticsandvehicle/isExistCode/'+code).subscribe(res => {
        this.form.controls['isExist'].setValue(res);
        this.form.controls['vehicleNumber'].setValue(code);
      });
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }
}
