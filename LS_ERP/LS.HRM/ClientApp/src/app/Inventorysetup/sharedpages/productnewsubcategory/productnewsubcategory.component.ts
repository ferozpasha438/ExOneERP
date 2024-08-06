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
import { MatDialog } from '@angular/material/dialog';
import { ParentSystemSetupComponent } from '../../../sharedcomponent/parentsystemsetup.component';
@Component({
  selector: 'app-productnewsubcategory',
  templateUrl: './productnewsubcategory.component.html',
})
export class ProductnewsubcategoryComponent extends ParentSystemSetupComponent implements OnInit {
  listOfCategories: Array<any> = [];
  fInSysGenAcCode: boolean = false;
  accountTypeId: string = '';
  accountTypeCode: string = '';
  //modalTitle: string;
  //modalBtnTitle: string;
  //dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
/*  isCompanyLoading: boolean = false;*/
  isReadOnly: boolean = false;

  /*companyControl = new FormControl();*/
 /* filteredOptions: Observable<Array<CustomSelectListItem>>;*/
  CategoryCodeList: Array<CustomSelectListItem> = [];

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<ProductnewsubcategoryComponent>,
    private notifyService: NotificationService, private validationService: ValidationService,public dialog: MatDialog) {
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
    this.loadCategoryCode();
    this.loadCategoryTypeList();
    if (this.id > 0)
      this.setEditForm();
  }
  loadCategoryTypeList() {
    this.apiService.getall('producthierarchy/getCategoryTypeList').subscribe(res => {
      if (res) {
        this.listOfCategories = res['list'];
        //this.fInSysGenAcCode = res['fInSysGenAcCode'] as boolean;
        //this.listOfCategories =res[0].text;
      }
    })
  }
  loadCategoryCode() {
    this.apiService.getall('producthierarchy/GetCategorySelectList').subscribe(res => {
      if (res) {
        this.CategoryCodeList = res;
      }
    })
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
      'subCatKey': ['', Validators.required],
      'itemSubCatCode': ['', Validators.required],
      'itemCatCode': ['', Validators.required],
      'itemCatName': ['', Validators.required],
      'itemSubCatName': ['', Validators.required],
      'itemSubCatDesc': ''
      /*'isActive': [true]*/
    });
  }

  setEditForm() {
    this.apiService.get('SubCategory', this.id).subscribe(res => {
      if (res) {
        this.isReadOnly = true;
        this.form.patchValue(res);
        this.form.controls['itemCatName'].setValue(res.itemCatCode);
      }
    })
  }
  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;

      this.apiService.post('SubCategory', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.reset();
          this.dialogRef.close(true);
          /*this.loadCategoryTypeList();*/
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
    this.form.controls['subCatKey'].setValue('');
    this.form.controls['itemSubCatCode'].setValue('');
    this.form.controls['itemCatCode'].setValue('');
    this.form.controls['itemCatName'].setValue('');
    this.form.controls['itemSubCatName'].setValue('');
    this.form.controls['itemSubCatDesc'].setValue('');

  }

  closeModel() {
    this.dialogRef.close();
  }
  onCCChange(Value: string) {
    console.log(Value);
    this.form.controls['itemCatName'].setValue(Value);
  }
  onCNChange(Value: string) {
    console.log(Value);
    this.form.controls['itemCatCode'].setValue(Value);
  }
  onTextchange(Value: string) {
    if (Value != null) {
      this.apiService.getall(`SubCategory/GetSubCategory?Subcode=${Value}`).subscribe(res => {
        if (res) {
          this.isReadOnly = true;
          this.onCCChange(res.itemCatCode)
          this.form.patchValue(res);
          this.id = res.id;
        }
      })
    }
  }
  private openDialogManage<T>(component: T, accountTypeId: string = '', accountTypeCode: string = '', fInSysGenAcCode: boolean = false, dbops: DBOperation = DBOperation.create, modalTitle: string = '', modalBtnTitle: string = '') {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, component);
    (dialogRef.componentInstance as any).accountTypeCode = accountTypeCode;
    (dialogRef.componentInstance as any).accountTypeId = accountTypeId;
    (dialogRef.componentInstance as any).fInSysGenAcCode = fInSysGenAcCode;
    //(dialogRef.componentInstance as any).modalTitle = modalTitle;
    //(dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    //(dialogRef.componentInstance as any).id = id;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true) {
        this.loadCategoryTypeList();
        this.utilService.OkMessage();
      }
    });
  }
  createSubCategory(CatID: string) {
  /*  this.form.patchValue({ 'itemCatCode': CatID })*/
    this.form.controls['itemCatCode'].setValue(CatID);
    this.form.controls['itemCatName'].setValue(CatID);

  }

}
