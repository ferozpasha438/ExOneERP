import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../models/MenuItemListDto';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { DBOperation } from '../../services/utility.constants';
import { UtilityService } from '../../services/utility.service';
import { ParentSystemSetupComponent } from '../../sharedcomponent/parentsystemsetup.component';
import { ValidationService } from '../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-addupdatebranch',
  templateUrl: './addupdatebranch.component.html',
  styles: [
  ],
})
export class AddupdatebranchComponent extends ParentSystemSetupComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
  isCompanyLoading: boolean = false;

  companyControl = new FormControl();
  filteredOptions: Observable<Array<CustomSelectListItem>>;
  cityList: Array<CustomSelectListItem> = [];
  zoneList: Array<CustomSelectListItem> = [];

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdatebranchComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {
    super(authService);
    this.filteredOptions = this.companyControl.valueChanges.pipe(
      startWith(''),
      debounceTime(utilService.autoDelay()),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isCompanyLoading = true;
        return this.filter(val || '')
      })
    );
  }

  getAll() {
    this.apiService.getall('').subscribe(data => {

    });
  }


  ngOnInit(): void {
    this.setForm();
    this.loadCities();
    if (this.id > 0)
      this.setEditForm();
  }

  filter(val: string): Observable<Array<CustomSelectListItem>> {
    return this.apiService.getall(`company/getSelectCompanyList?search=${val.trim()}`)
      .pipe(
        map(response => {
          const res = response as Array<CustomSelectListItem>;
          this.isCompanyLoading = false;
          //if (res && res.length == 0)
          //  this.notifyService.showError("enter branch name")
          return res;
        })
      )
  }

  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      'branchCode': ['', Validators.required],
      'bankName': ['', Validators.required],
      'bankNameAr': ['', Validators.required],
      'branchName': ['', Validators.required],
      'branchAddress': ['', Validators.required],
      'branchAddressAr': [''],
      'phone': ['', Validators.compose([Validators.required, this.validationService.mobileValidator,
        //  this.validationService.numberValidator
      ])],
      'mobile': ['', Validators.compose([Validators.required, this.validationService.mobileValidator])],
      'city': ['', Validators.required],
      'zoneId': ['', Validators.required],
      'state': ['', Validators.required],
      'authorityName': ['', Validators.required],
      //'remarks': ['', Validators.required],
      'isActive': [false],
      'accountNumber': ['', Validators.required],
      'iban': '',
    });
  }

  setEditForm() {
    this.apiService.get('branch', this.id).subscribe(res => {
      if (res) {
        this.companyControl.setValue(res['companyName']);
        this.form.patchValue(res);

      }
    })


    //this.form = this.fb.group({
    //  'branchCode': ['', Validators.required],
    //  'branchName': ['', Validators.required],
    //  'branchAddress': ['', Validators.required],
    //  'phone': ['', Validators.compose([Validators.required, this.validationService.mobileValidator,
    //    //  this.validationService.numberValidator
    //  ])],
    //  'mobile': ['', Validators.compose([Validators.required, this.validationService.mobileValidator])],
    //  'city': ['', Validators.required],
    //  'state': ['', Validators.required],
    //  'authorityName': ['', Validators.required],
    //  //'remarks': ['', Validators.required],
    //  'isActive': [false]
    //});

  }

  getStateCountrybyCityCode1(event: any) {
    const id = event.target.value;
    this.apiService.getall('City/getStateCountrybyCityCode/' + id).subscribe(res => {
      this.form.patchValue({ 'state': res['stateName'] });
    });
  }

  loadCities() {

    this.apiService.getall('city/getSelectList').subscribe(res => {
      if (res) {
        this.cityList = res;
      }
    });

    this.apiService.getall('zone/getZoneSelectList').subscribe(res => {
      if (res) {
        this.zoneList = res;
      }
    });



  }

  checkBranchCode(evt: any) {
    const bCode = evt.target.value as string;
    if (bCode.trim() !== '') {
      this.apiService.getall(`branch/checkBranchCode?branchCode=${bCode}`).subscribe(res => {
        if (res) {
          this.form.controls['branchCode'].setValue('');
        }
      })
    }
  }

  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;

      let companyName = this.companyControl.value as string;
      if (this.utilService.hasValue(companyName)) {
        this.form.value['companyName'] = companyName;
        this.apiService.post('branch', this.form.value)
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
    else
      this.utilService.FillUpFields();

  }

  reset() {
    this.form.controls['branchCode'].setValue('');
    this.form.controls['branchName'].setValue('');
    this.form.controls['branchAddress'].setValue('');
  }

  closeModel() {
    this.dialogRef.close();
  }
}
