import { error } from '@angular/compiler/src/util';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';
import { OprServicesService } from '../../opr-services.service';
import Validation from '../../Validators/custom.validators/custom.validators.component';

@Component({
  selector: 'app-add-update-skillset',
  templateUrl: './add-update-skillset.component.html'
})
export class AddUpdateSkillsetComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
  isDataLoading: boolean = false;
  readonly: string = "";

  constructor(public dialogRef: MatDialogRef<AddUpdateSkillsetComponent>, private notifyService: NotificationService, private authService: AuthorizeService, private fb: FormBuilder, private apiService: ApiService, private utilService: UtilityService, private opservice: OprServicesService) {
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
      'skillSetCode': ['SSTXXXXXX'],
      'skillType': ['', Validators.required],
      'nameInEnglish': ['', Validators.required],
      'nameInArabic': ['', Validators.required],
      'detailsOfSkillSet': ['', Validators.required],
      'skillSetType': ['', Validators.required],
      'prioryImportanceScale': ['', Validators.required],
      'minBufferResource': ['', Validators.required],
      'monthlyCtc': ['', Validators.required],
      'costToCompanyDay': ['', Validators.required],
      'monthlyOverheadCost': ['', Validators.required],
      'monthlyOtherOverHeads': ['', Validators.required],
      'totalSkillsetCost': ['', Validators.required],
      'totalSkillsetCostDay': ['', Validators.required],
      'servicePriceToCompany': ['', Validators.required],
      'minMarginRequired': ['', Validators.required],
      'overrideMarginIfRequired': [false],
      'isActive': [true],
      'responsibilitiesRoles': ['', Validators.required],
      'isExist': [false]
    },
      {
        validators: [Validation.isCodeExist('skillSetCode','isExist')]
      }
    );
  }
  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }
  closeModel() {
    this.dialogRef.close();
  }
  submit() {
    if (this.form.valid) {

      if (this.id > 0)
        this.form.value['id'] = this.id;
      this.apiService.post('Skillset', this.form.value)
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
    this.apiService.get('Skillset/getSkillsetById', this.id).subscribe(res => {
      if (res) {
        this.form.patchValue(res);
        this.form.patchValue({ 'id': 0 });

      }
    });
  }

  reset() {
    this.form.controls['skillSetCode'].setValue('');
    this.form.controls['skillType'].setValue('');
    this.form.controls['nameInEnglish'].setValue('');
    this.form.controls['detailsOfSkillSet'].setValue('');
    this.form.controls['skillSetType'].setValue('');
    this.form.controls['prioryImportanceScale'].setValue('');
    this.form.controls['minBufferResource'].setValue('');
    this.form.controls['monthlyCtc'].setValue('');
    this.form.controls['costToCompanyDay'].setValue('');
    this.form.controls['monthlyOverheadCost'].setValue('');
    this.form.controls['monthlyOtherOverHeads'].setValue('');
    this.form.controls['totalSkillsetCost'].setValue('');
    this.form.controls['totalSkillsetCostDay'].setValue('');
    this.form.controls['servicePriceToCompany'].setValue('');
    this.form.controls['minMarginRequired'].setValue('');
    this.form.controls['overrideMarginIfRequired'].setValue('');
    this.form.controls['overrideMarginIfRequired'].setValue(false);
    this.form.controls['isActive'].setValue(true);
    this.form.controls['responsibilitiesRoles'].setValue('');
  }



  verifyCode(event: any) {
    let code = event.target.value.toUpperCase();
   if(code!='')
      this.opservice.verifyCode('Skillset/isExistCode/'+ code).subscribe(res => {
        this.form.controls['isExist'].setValue(res);
        this.form.controls['skillSetCode'].setValue(code);
      });
  }


}
