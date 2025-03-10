import { Component, OnChanges, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { from, Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, mergeMap, startWith, switchMap } from 'rxjs/operators';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';

@Component({
  selector: 'app-addupdate-school-branch',
  templateUrl: './addupdate-school-branch.component.html',
  styleUrls: []
})
export class AddupdateSchoolBranchComponent implements OnInit {
  modalTitle!: string;
  modalBtnTitle!: string;
  dbops!: DBOperation;
  form!: FormGroup;
  id: number = 0;
  isBranchLoading: boolean = false;
  branchCodeControl = new FormControl();
  options = [];
  filteredOptions: Observable<Array<CustomSelectListItem>>;
  userList: Array<any> = [];
  branchCode: string = '';
  row: any;
  moderatorList: Array<any> = [];

  constructor(private fb: FormBuilder, private apiService: ApiService, private utilService: UtilityService,
    private validationService: ValidationService, public dialog: MatDialog, private notifyService: NotificationService,
    public dialogRef: MatDialogRef<AddupdateSchoolBranchComponent>) {
    this.filteredOptions = this.branchCodeControl.valueChanges.pipe(
      startWith(''),
      debounceTime(utilService.autoDelay()),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isBranchLoading = true;
        return this.filter(val || '')
      })
    )
  }
  ngOnInit(): void {
    this.setForm();
    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.branchCode = this.row['branchCode'];
      this.getModerators();
      this.setEditForm();
    }
  }
  getModerators() {
    this.apiService.getall(`TeacherMaster/GetModerators`).subscribe(res => {
      this.moderatorList = res as Array<any>;
    });
  }

  getTeachersByBranchCode(branchCode: any) {
    this.apiService.getall(`TeacherMaster/GetTeachersByBranchcode/${branchCode}`).subscribe(res => {
      this.userList = res as Array<any>;
    });
  }
  filter(val: string): Observable<Array<CustomSelectListItem>> {
    return this.apiService.getall(`branch/getSelectSysBranchList?search=${val.trim()}`)
      .pipe(
        map(response => {
          const res = response as Array<CustomSelectListItem>;
          this.isBranchLoading = false;
          return res;
        })
      )
  }

  setForm() {
    this.form = this.fb.group({
      'branchName': ['', Validators.required],
      'branchNameAr': ['', Validators.required],
      'startAcademicDate': ['', Validators.required],
      'endAcademicDate': ['', Validators.required],
      'branchPrefix': ['', Validators.required],
      'address': ['', Validators.required],
      'city': ['', Validators.required],
      'mobile': ['', Validators.compose([Validators.required, this.validationService.mobileValidator])],
      'phone': ['', Validators.compose([Validators.required, this.validationService.mobileValidator])],
      'email': ['', Validators.required],
      'weekOff1': ['', Validators.required],
      'weekOff2': [''],
      'geoLat': ['', Validators.required],
      'geoLong': ['', Validators.required],
      'currencyCode': [''],
      'privacyPolicyUrl': [''],
      'startWeekDay': ['', Validators.required],
      'website': [''],
      'branchNotification_Moderator': [''],
      'default_InTime': ['', Validators.required],
      'default_OutTime': ['', Validators.required],
      'schoolBranchesAuthorityList': this.fb.array([this.createAuthority()])
    })
  }
  createAuthority(res?: any): FormGroup {
    if (res) {
      return this.fb.group(res);
    }
    return this.fb.group({
      'id': 0,
      'teacherCode': [''],
      'level': [0],
      'isApproveLeave': [false],
      'isApproveDisciPlinaryAction': [false],
      'isApproveNotification': [false],
      'isApproveEvent': [false]
    })
  }
  get schoolBranchesAuthorityList(): FormArray {
    return <FormArray>this.form.get('schoolBranchesAuthorityList');
  }
  addItem() {
    this.schoolBranchesAuthorityList.push(this.createAuthority());
  }
  editItem(res: any) {
    this.schoolBranchesAuthorityList.push(this.createAuthority(res));
  }
  removeItem(item: number) {
    this.schoolBranchesAuthorityList.removeAt(item);
  }
  selectedOption(event: MatAutocompleteSelectedEvent) {
    this.branchCode = event.option.value;
    this.getTeachersByBranchCode(event.option.value);
    this.getBranchData(event.option.value);
    this.setEditForm();
  }
  getBranchData(branchCode: string) {
    this.apiService.getall(`branch/getBranchDataByBranchCode?branchCode=${branchCode}`).subscribe(res => {
      if (res) {
        this.form.patchValue({
          'branchName': res['branchName'], 'address': res['branchAddress'], 'city': res['city'], 'mobile': res['mobile'], 'phone': res['phone'],
          'geoLat': res['geoLocLatitude'],
          'geoLong': res['geoLocLongitude']
        });
      }
    }, error => {
      this.form.patchValue({
        'branchName': '', 'address': '', 'city': '', 'mobile': '', 'phone': '',
        'geoLat': '',
        'geoLong': ''
      });
    });
  }

  branchcodeBlur(event: any) {
    // this.getUsersByBranchCode(event.target.value);
    this.apiService.getall(`branch/getBranchDataByBranchCode?branchCode=${event.target.value}`).subscribe(res => {
      if (res) {
        this.form.patchValue({
          'branchName': res['branchName'], 'address': res['branchAddress'], 'city': res['city'], 'mobile': res['mobile'], 'phone': res['phone']
          , 'geoLat': res['geoLocLatitude'],
          'geoLong': res['geoLocLongitude']
        });
      }
    }, error => {
      this.form.patchValue({
        'branchName': '', 'address': '', 'city': '', 'mobile': '', 'phone': '',
        'geoLat': '',
        'geoLong': ''
      });
      this.branchCodeControl.setValue('');
    })
  }
  setEditForm() {
    this.apiService.getall(`branch/${this.branchCode}`).subscribe(res => {
      if (res) {
        if (res.branchCode != null) {
          this.id = res.id;
          this.getTeachersByBranchCode(res.branchCode);
          //this.getBranchData(res.branchCode);
          //this.form.patchValue(res);
          this.form.patchValue({
            'branchName': res.branchName,
            'branchNameAr': res.branchNameAr,
            'startAcademicDate': res.startAcademicDate,
            'endAcademicDate': res.endAcademicDate,
            'branchPrefix': res.branchPrefix,
            'address': res.address,
            'city': res.city,
            'mobile': res.mobile,
            'phone': res.phone,
            'email': res.email,
            'weekOff1': res.weekOff1,
            'weekOff2': res.weekOff2,
            'geoLat': res.geoLat,
            'geoLong': res.geoLong,
            'currencyCode': res.currencyCode,
            'privacyPolicyUrl': res.privacyPolicyUrl,
            'startWeekDay': res.startWeekDay,
            'website': res.Website,
            'branchNotification_Moderator': res.branchNotification_Moderator,
            'default_InTime': res.default_InTime,
            'default_OutTime': res.default_OutTime
          });
          this.branchCodeControl.setValue(res.branchCode);
          this.schoolBranchesAuthorityList.clear();
          let schoolBranchesAuthorityList = res.schoolBranchesAuthorityList as Array<any>;
          schoolBranchesAuthorityList.forEach(item => {
            this.editItem(item);
          });
        }
      }
    })
  }
  submit() {
    if (this.form.valid) {
      let branchCode = this.branchCodeControl.value as string;
      if (branchCode && branchCode.trim() !== '') {
        this.form.value['id'] = this.id;
        this.form.value['branchCode'] = branchCode;
        this.apiService.post('Branch', this.form.value)
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
    this.form.controls['branchName'].setValue('');
    this.form.controls['branchNameAr'].setValue('');
    this.form.controls['branchPrefix'].setValue('');
    this.form.controls['address'].setValue('');
    this.form.controls['phone'].setValue('');
    this.form.controls['city'].setValue('');
    this.form.controls['mobile'].setValue('');
  }
  closeModel() {
    this.dialogRef.close();
  }
}
