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
  selector: 'app-addupdate-serviceunitmapp',
  templateUrl: './addupdate-serviceunitmapp.component.html'
})
export class AddupdateServiceunitmappComponent extends ParentOptMgtComponent implements OnInit {
  readonly: string = "";
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
  pricePerUnit: number;
  serviceCode: string;
  unitCode: string;
  serviceCodeList: Array<CustomSelectListItem> = [];
  unitCodeList: Array<CustomSelectListItem> = [];
  isDataLoading: boolean = false;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<AddupdateServiceunitmappComponent>) {
    super(authService);
  }

  ngOnInit(): void {
    this.setForm();
    this.loadUnitCodes();
    this.loadServiceCodes();
    if (this.id > 0) {
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
      //'stateone': [''],
      'serviceCode': ['', Validators.required],
      'unitCode': ['', Validators.required],
      'pricePerUnit': [, Validators.required],
      'isActive': [true]
    });

  }
  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      this.apiService.post('ServiceUnitMap',this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.dialogRef.close(true);
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
    this.apiService.get('ServiceUnitMap/getServiceUnitMapById', this.id).subscribe(res => {
      if (res) {
        this.form.patchValue(res);
        this.form.patchValue({ 'id': 0 });

      }
    });



  }






  loadServiceCodes() {
    this.apiService.getall('Services/getSelectServiceList').subscribe(res => {
      this.serviceCodeList = res;
    });
  }
  loadUnitCodes() {
    this.apiService.getall('Units/getSelectUnitList').subscribe(res => {
      this.unitCodeList = res;
    });
  }

  reset() {
    this.form.controls['serviceCode'].setValue('');
    this.form.controls['unitCode'].setValue('');
    this.form.controls['pricePerUnit'].setValue('');
    this.form.controls['isActive'].setValue(true);
  }




}
