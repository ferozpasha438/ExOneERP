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
  selector: 'app-add-site-to-customer',
  templateUrl: './add-site-to-customer.component.html'
})
export class AddSiteToCustomerComponent extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
  customerCode: string;
  custCityCode: string;
  cityList: Array<CustomSelectListItem> = [];
  custCodeControl = new FormControl();
  filteredCustCodes: Observable<Array<CustomSelectListItem>>;
  isDataLoading: boolean = false;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<AddSiteToCustomerComponent>) {
    super(authService);

  }

  ngOnInit(): void {
    this.loadCities();

    this.setForm();
    if (this.id > 0) {
      this.setEditForm();
    }
  }
  closeModel() {
    this.dialogRef.close();
  }
  setForm() {
    this.form = this.fb.group({

      'siteCode': ['SITEXXXXXX'],
      'siteName': ['', Validators.required],
      'siteArbName': ['', Validators.required],
      'customerCode': ['', Validators.required],
      'siteAddress': ['', Validators.required],
      'siteCityCode': ['', Validators.required],
      'siteGeoLatitude': ['', Validators.required],
      'siteGeoLongitude': ['', Validators.required],
      'siteGeoGain': ['', Validators.required],
      'isActive': [true]
    });

  }
  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = 0;

      let custCode = this.custCodeControl.value as string;

         this.apiService.post('CustomerSite', this.form.value)
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
    this.apiService.get('CustomerMaster', this.id).subscribe(res => {
      this.form.value['id'] = 0;
      if (res) {
        
        this.form.controls['customerCode'].setValue(res.custCode);
       
      }


    });



  }


  
  loadCities() {
    this.apiService.getall('City/getCitiesSelectList').subscribe(res => {
      this.cityList = res;
    });
  }
}
