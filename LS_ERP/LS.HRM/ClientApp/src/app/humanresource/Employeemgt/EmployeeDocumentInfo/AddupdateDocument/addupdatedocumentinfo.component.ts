import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../../services/api.service';
import { NotificationService } from '../../../../services/notification.service';
import { DBOperation } from '../../../../services/utility.constants';
import { UtilityService } from '../../../../services/utility.service';
import { ParentHrmAdminComponent } from '../../../../sharedcomponent/ParentHrmAdmin.component';
import { ValidationService } from '../../../../sharedcomponent/ValidationService';
import { CustomSelectListItem } from 'src/app/models/MenuItemListDto';

@Component({
  selector: 'app-addupdatedocumentinfo',
  templateUrl: './addupdatedocumentinfo.component.html',
  styles: [
  ]
})
export class AddupdatedocumentinfoComponent extends ParentHrmAdminComponent implements OnInit {
  modalTitle!: string;
  modalBtnTitle!: string;
  dbops!: DBOperation;
  form!: FormGroup;
  id: number = 0;
  employeeNumber!: string;
  documentTypes: Array<CustomSelectListItem> = [];
  documentUrl: string | ArrayBuffer | null = null;
  formData!: FormData;
  files: Array<File> = [];
  fileUrl = '';
  fileName = '';
  name = '';

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdatedocumentinfoComponent>,
    private notifyService: NotificationService, private validationService: ValidationService) {
    super(authService)
  };

  ngOnInit(): void {
    this.fileUrl = this.authService.ApiEndPoint();
    this.loadDocumentTypes();
    this.setForm();
    if (this.id > 0)
      this.setEditForm();
  }

  loadDocumentTypes() {
    this.apiService.getall('DocumentType/GetDocumentTypeSelectListItem').subscribe(res => {
      this.documentTypes = res;
    });
  }

  setForm() {
    this.form = this.fb.group(
      {
        'employeenumber': [this.employeeNumber],
        'documentTypeCode': ['', Validators.required],
        'isVerified': [false],
        'documentNumber': [''],
        'name': [''],
        'fileName': [''],
        'isActive': [true],
        'files':['']
      }
    );
  }

  onFileChanged(event: any) {
    let reader = new FileReader();
    if (event.target.files && event.target.files.length > 0) {
      for (let i = 0; i < event.target.files.length; i++) {
        this.files.push(event.target.files[i]);
      }
      // let file = event.target.files[0];
      // reader.readAsDataURL(file);
      // reader.onload = () => {
      //   this.documentUrl = reader.result;
      //   this.form.patchValue({
      //     'Documents': file,
      //   });
      // };
    }
  };

  setEditForm() {
    let queryParam = `id=${encodeURIComponent("" + this.id)}&employeeID=${encodeURIComponent("" + Number(this.employeeNumber))}&deleteDocument=false`;
    this.apiService.getQueryString(`EmployeeDocument/GetEmployeeDocumentById?`, queryParam).subscribe(res => {
      if (res) {
        this.form.patchValue(res);
        this.fileName = this.form.controls['fileName'].value;
        this.name = this.form.controls['name'].value;
      }
    });
  }
  
  closeModel() {
    this.dialogRef.close();
  }

  submit() {
    if (this.form.valid) {
      this.formData = new FormData();

      if (this.id > 0)
        this.formData.append('id', this.id.toString());

      this.formData.append('employeeID', Number(this.employeeNumber).toString());
      this.formData.append('documentTypeCode', this.form.controls['documentTypeCode'].value);
      this.formData.append('isVerified', this.form.controls['isVerified'].value);
      this.formData.append('documentNumber', this.form.controls['documentNumber'].value);
      this.formData.append('isActive', this.form.controls['isActive'].value);

      for (let i = 0; i < this.files.length; i++) {
          this.formData.append('files', this.files[i], this.files[i].name);
      }

      this.apiService.post('EmployeeDocument', this.formData)
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

  reset() {
    this.form.controls['employeenumber'].setValue(this.employeeNumber);
    this.form.controls['documentTypeCode'].setValue('');
    this.form.controls['isVerified'].setValue(false);
    this.form.controls['documentNumber'].setValue('');
    this.form.controls['name'].setValue('');
    this.form.controls['fileName'].setValue('');
    this.form.controls['isActive'].setValue(true);
    this.form.controls['files'].setValue('');
  }
}
