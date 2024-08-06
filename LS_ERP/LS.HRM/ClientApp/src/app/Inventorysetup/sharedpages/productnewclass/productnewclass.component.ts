import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { ParentSystemSetupComponent } from '../../../sharedcomponent/parentsystemsetup.component';
@Component({
  selector: 'app-productnewclass',
  templateUrl: './productnewclass.component.html',
  
})
export class ProductnewclassComponent extends ParentSystemSetupComponent implements OnInit {

  //modalTitle: string;
  //modalBtnTitle: string;
  //dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
/*  isCompanyLoading: boolean = false;*/
  isReadOnly: boolean = false;

  //companyControl = new FormControl();
  //filteredOptions: Observable<Array<CustomSelectListItem>>;
  //CategoryCodeList: Array<CustomSelectListItem> = [];

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<ProductnewclassComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {
    //this.filteredOptions = this.companyControl.valueChanges.pipe(
    //  startWith(''),
    //  debounceTime(utilService.autoDelay()),
    //  distinctUntilChanged(),
    //  switchMap((val: string) => {
    //    if (val.trim() !== '')
    //      this.isCompanyLoading = true;
    //    return this.filter(val || '')
    //  })
    //);
    super(authService);
  }

  //getAll() {
  //  this.apiService.getall('').subscribe(data => {

  //  });
  //}


  ngOnInit(): void {
    this.setForm();
    if (this.id > 0)
      this.setEditForm();
  }

 
  //filter(val: string): Observable<Array<CustomSelectListItem>> {
  //  return this.apiService.getall(`company/getSelectCompanyList?search=${val}`)
  //    .pipe(
  //      map(response => {
  //        const res = response as Array<CustomSelectListItem>;
  //        this.isCompanyLoading = false;
  //        return res;
  //      })
  //    )
  //}

  setForm() {
    this.form = this.fb.group({
      'itemClassCode': ['', Validators.required],
      'itemClassName': ['', Validators.required],
      'itemClassDesce': ''
     /* 'isActive': [true]*/
    });
  }

  setEditForm() {
    this.apiService.get('Class', this.id).subscribe(res => {
      if (res) {
        this.isReadOnly = true;
        this.form.patchValue(res);
      }
    })
  }
  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;

      this.apiService.post('Class', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.reset();
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
  reset() {
    this.form.controls['itemClassCode'].setValue('');
    this.form.controls['itemClassName'].setValue('');
    this.form.controls['itemClassDesce'].setValue('');
  }

  closeModel() {
    this.dialogRef.close();
  }
  onTextchange(Value: string) {
    if (Value != null) {
      this.apiService.getall(`Class/GetClassItems?ClassCode=${Value}`).subscribe(res => {
        if (res) {
          this.isReadOnly = true;
          this.form.patchValue(res);
          this.id = res.id;
        }
      })
    }
  }
}
