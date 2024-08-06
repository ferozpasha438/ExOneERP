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
  selector: 'app-add-update-materialequipment',
  templateUrl: './add-update-materialequipment.component.html'
})
export class AddUpdateMaterialequipmentComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
  isDataLoading: boolean = false;
  readonly: string = "";
  constructor(public dialogRef: MatDialogRef<AddUpdateMaterialequipmentComponent>, private authService: AuthorizeService, private fb: FormBuilder, private apiService: ApiService, private utilService: UtilityService, private opservice: OprServicesService) {
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
     // '': ['', Validators.required],
      'isExist': [false],
      'code': ['MEXXXXXX', Validators.required],
      'category': ['', Validators.required],
      'nameInEnglish': ['', Validators.required],
      'nameInArabic': ['', Validators.required],
      'type': ['', Validators.required],
      'estimatedCostValue': ['', Validators.required],
      'isDepreciationApplicable': [false, Validators.required],
      'minimumCostPerUsage': ['', Validators.required],
      'depreciationPerYear': ['', Validators.required],
      'usageLifetermsYear': ['', Validators.required],
      'isActive': [true, Validators.required],
      'remarks': [''],
    },
      {
        validators: [Validation.isCodeExist('code', 'isExist')]
      });
  }
  closeModel() {
    this.dialogRef.close();
  }
  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      this.apiService.post('Materialequipment', this.form.value)
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
    this.apiService.get('Materialequipment/getMaterialequipmentById', this.id).subscribe(res => {
      if (res) {
        this.form.patchValue(res);
        this.form.patchValue({ 'id': 0 });

      }
    });
  }
  reset() {
    this.form.controls['code'].setValue('');
    this.form.controls['category'].setValue('');
    this.form.controls['nameInEnglish'].setValue('');
    this.form.controls['nameInArabic'].setValue('');
    this.form.controls['type'].setValue('');
    this.form.controls['estimatedCostValue'].setValue('');
    this.form.controls['isDepreciationApplicable'].setValue(false);
    this.form.controls['minimumCostPerUsage'].setValue('');
    this.form.controls['depreciationPerYear'].setValue('');
    this.form.controls['usageLifetermsYear'].setValue('');
    this.form.controls['isActive'].setValue(true);
    this.form.controls['remarks'].setValue('');
  }
  verifyCode(event: any) {
    let code = event.target.value.toUpperCase();
    if (code != '')
      this.opservice.verifyCode('Materialequipment/isExistCode/' + code).subscribe(res => {
        this.form.controls['isExist'].setValue(res);
        this.form.controls['code'].setValue(code);
      });
  }
  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }
}
