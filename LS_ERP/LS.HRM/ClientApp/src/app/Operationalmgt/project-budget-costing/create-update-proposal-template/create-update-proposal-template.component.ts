import { HttpClient } from '@angular/common/http';
import { Component, OnChanges, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { from, Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, mergeMap, startWith, switchMap } from 'rxjs/operators';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
//import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';
import { ValidationService } from '../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-create-update-proposal-template',
  templateUrl: './create-update-proposal-template.component.html'
})
export class CreateUpdateProposalTemplateComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
  inputData: any;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<CreateUpdateProposalTemplateComponent>) {
    super(authService);
  }

  ngOnInit(): void {
    this.setForm();
   




    this.id = this.inputData?.id;
    if (this.id > 0) {
      this.setEditForm();
    }
   
  }

  setEditForm() {

    this.apiService.get('proposalTemplates/getProposalTemplateById', this.id).subscribe(res => {
      if (res) {
        this.form.patchValue(res);
        this.form.patchValue({ 'id': res.id });

      }
    });

  }

  setForm() {
    this.form = this.fb.group({

      'titleOfService': ['', Validators.required],
      'coveringLetter': ['', Validators.required],
      'commercial': ['', Validators.required],
      'notes': ['', Validators.required],
      'issuingAuthority': ['', Validators.required],

      'titleOfServiceArb': ['', Validators.required],
      'coveringLetterArb': ['', Validators.required],
      'commercialArb': ['', Validators.required],
      'notesArb': ['', Validators.required],
      'issuingAuthorityArb': ['' ,Validators.required],
    });
  }

  submit() {
    this.form.value['id'] = this.id;
    this.form.value['projectCode'] = this.inputData.projectCode;
   this.form.value['siteCode'] = this.inputData.siteCode;
    this.form.value['customerCode'] = this.inputData.customerCode;

    if (this.form.valid) {
      this.apiService.post('proposalTemplates', this.form.value)
        .subscribe(res => {
          if (res) {

            this.utilService.OkMessage();

            this.dialogRef.close(true);

          }
        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
          });
    }
    else {
      this.notifyService.showError(this.translate.instant("Incomplete_Data"));
    }
  }
  closeModel() {
    this.dialogRef.close();
  }
}
