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
  selector: 'app-addupdateaccountbranch',
  templateUrl: './addupdateaccountbranch.component.html',
  styles: [
  ],

})
export class AddupdateaccountbranchComponent implements OnInit {

  modalTitle!: string;
  modalBtnTitle!: string;
  dbops!: DBOperation;
  form!: FormGroup;
  id: number = 0;
  isBranchLoading: boolean = false;
  branchCodeError: string = '';

  branchCodeControl = new FormControl();
  options = [];
  filteredOptions: Observable<Array<CustomSelectListItem>>;
  userList: Array<CustomSelectListItem> = [];


  constructor(private fb: FormBuilder, private apiService: ApiService, private utilService: UtilityService,
    private validationService: ValidationService, public dialog: MatDialog, private notifyService: NotificationService,
    public dialogRef: MatDialogRef<AddupdateaccountbranchComponent>) {
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
    if (this.id > 0)
      this.setEditForm();
  }

  getUsersByBranchCode(branchCode: any) {
    this.apiService.getall(`menuOption/getUsersByBranchCode?branchCode=${branchCode}`).subscribe(res => {
      this.userList = res;
    });
  }

  filter(val: string): Observable<Array<CustomSelectListItem>> {
    return this.apiService.getall(`branch/getSelectSysBranchList?search=${val.trim()}`)
      .pipe(
        map(response => {
          const res = response as Array<CustomSelectListItem>;
          //if (res && res.length == 0)
          //  this.notifyService.showError("enter branch name")
          this.isBranchLoading = false;
          return res;
        })
      )

    //let emptyList: Array<CustomSelectListItem> = [];
    //return new Observable((observer) => {
    //  observer.next(emptyList);
    //  observer.complete();
    //});

    //return Observable.create(emptyList);
  }

  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      // 'finBranchCode': ['', Validators.required],
      'finBranchName': ['', Validators.required],
      'finBranchPrefix': ['', Validators.required],
      'finBranchAddress': ['', Validators.required],
      'finBranchDesc': ['', Validators.required],
      'finBranchType': ['', Validators.required],
      'finBranchIsActive': [false],
      authList: this.fb.array([this.createAuthority()])//, Validators.required)
    })
  }

  createAuthority(res?: any): FormGroup {
    if (res) {
      return this.fb.group(res);
    }
    return this.fb.group({
      'appAuth': ['', Validators.required],
      'appLevel': ['', Validators.required],
      'appAuthJV': [false],
      'appAuthBV': [false],
      'appAuthCV': [false],
      'appAuthAP': [false],
      'appAuthAR': [false],
      'appAuthPurcRequest': [false],
      'appAuthPurcReturn': [false],
      'appAuthPurcOrder': [false],
      'appAuthAdj': [false],
      'appAuthIssue': [false],
      'appAuthRect': [false],
      'appAuthTrans': [false],

    })
  }
  get authList(): FormArray {
    return <FormArray>this.form.get('authList');
  }
  addItem() {
    this.authList.push(this.createAuthority());
  }
  editItem(res: any) {
    this.authList.push(this.createAuthority(res));
  }
  removeItem(item: number) {
    this.authList.removeAt(item);
  }

  blurBranchCode(event: any) {
    this.apiService.getall(`branch/checkBranchCode?branchCode=${event.target.value}`).subscribe(res => {
      if (!res) {
        //if (!this.isOptionSelected) {
        //this.isOptionSelected = false;
        this.branchCodeControl.setValue('');
        this.branchCodeError = "Select BranchCode";
        this.form.patchValue({ 'finBranchName': '', 'finBranchAddress': '', });
        //}
      }
    });
  }

  selectedOption(event: MatAutocompleteSelectedEvent) {
   // this.branchCodeControl.setValue(event.option.value);
    this.branchCodeError = '';
    this.getUsersByBranchCode(event.option.value);
    this.apiService.getall(`branch/getBranchByBranchCode?branchCode=${event.option.value}`).subscribe(res => {
      if (res) {
        this.form.patchValue({ 'finBranchName': res['text'], 'finBranchAddress': res['value'] });

      }
    }, error => {
      this.form.patchValue({ 'finBranchName': '', 'finBranchAddress': '', });
      // this.branchCodeControl.setValue('');
    })
  }

  branchcodeBlur(event: any) {
    this.getUsersByBranchCode(event.target.value);

    this.apiService.getall(`branch/getBranchByBranchCode?branchCode=${event.target.value}`).subscribe(res => {
      if (res) {
        this.form.patchValue({ 'finBranchName': res['text'], 'finBranchAddress': res['value'] });
      }
    }, error => {
      this.form.patchValue({ 'finBranchName': '', 'finBranchAddress': '', });
      this.branchCodeControl.setValue('');
    })

  }


  setEditForm() {
    this.apiService.get('accountsbranches', this.id).subscribe(res => {
      if (res) {
        this.getUsersByBranchCode(res["finBranchCode"]);
        this.form.patchValue(res);
        this.branchCodeControl.setValue(res["finBranchCode"]);

        this.authList.clear();
        let authList = res['authList'] as Array<any>;
        authList.forEach(item => {
          this.editItem(item);
        });
      }
    })
  }

  submit() {
    if (this.form.valid) {
      let finBranchCode = this.branchCodeControl.value as string;
      if (finBranchCode && finBranchCode.trim() !== '') {
        if (this.id > 0)
          this.form.value['id'] = this.id;

        this.form.value['finBranchCode'] = finBranchCode;
        //this.form.value['finBranchCode'] = finBranchCode.substring(finBranchCode.lastIndexOf('-') + 1);
        this.apiService.post('accountsbranches', this.form.value)
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
    this.form.controls['finBranchName'].setValue('');
    this.form.controls['finBranchPrefix'].setValue('');
    this.form.controls['finBranchAddress'].setValue('');
  }


  closeModel() {
    this.dialogRef.close();
  }
}
