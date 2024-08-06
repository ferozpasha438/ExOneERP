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
  selector: 'app-productnewcategory',
  templateUrl: './productnewcategory.component.html',
  styles: [
  ]
})
export class ProductnewcategoryComponent extends ParentSystemSetupComponent implements OnInit {

  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
 /* isCompanyLoading: boolean = false;*/
  isReadOnly: boolean = false;
  //companyControl = new FormControl();
  //filteredOptions: Observable<Array<CustomSelectListItem>>;
  //cityList: Array<CustomSelectListItem> = [];
  catprefixLength: number = 0;
  prefixCatCode: boolean = false;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<ProductnewcategoryComponent>,
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
    this.loadInvtConfig();

    if (this.id > 0)
      this.setEditForm();
  }

  keyPressNumbers(event: any) {
    var charCode = (event.which) ? event.which : event.keyCode;
    // Only Numbers 0-9
    if (charCode != 46 && charCode > 31
      && (charCode < 48 || charCode > 57)) {
      event.preventDefault();
      return false;
    } else {
      return true;
    }
  }

  setForm() {
    this.form = this.fb.group({
      'itemCatCode': ['', Validators.required],
      'itemCatName': ['', Validators.required],
      'itemCatDesc': '',
      'itemCatPrefix': ''
      
    });
  }

  loadInvtConfig() {

    this.apiService.getall(`inventoryconfiguration/getInvtConfig`).subscribe(res => {
      if (res) {        
        this.catprefixLength = res['categoryLength'];
        this.prefixCatCode = res['prefixCatCode'] as boolean;
      }
    });
  }

  setEditForm() {
    this.apiService.get('producthierarchy', this.id).subscribe(res => {
      if (res) {
        this.form.patchValue(res);

      }
    })
  }
  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;

      if (this.catprefixLength == (this.form.controls['itemCatPrefix'].value as string).trim().length) {

        this.apiService.post('producthierarchy', this.form.value)
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
        this.notifyService.showError(`Category Prefix must be ${this.catprefixLength} characters`);

    }
    else
      this.utilService.FillUpFields();
  }
  reset() {
    this.form.controls['itemCatCode'].setValue('');
    this.form.controls['itemCatName'].setValue('');
    this.form.controls['itemCatDesc'].setValue('');
    this.form.controls['itemCatPrefix'].setValue('');

  }

  closeModel() {
    this.dialogRef.close();
  }
  onTextchange(Value: string) {
    if (Value != null) {
      this.apiService.getall(`producthierarchy/GetCategoryItem?CatCode=${Value}`).subscribe(res => {
        if (res) {
          this.isReadOnly = true;
          this.form.patchValue(res);
          this.id = res.id;
        }
      })
    }
  }
}
