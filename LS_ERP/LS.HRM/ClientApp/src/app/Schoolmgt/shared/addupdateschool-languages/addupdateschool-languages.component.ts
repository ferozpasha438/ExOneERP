import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
@Component({
  selector: 'app-addupdateschool-languages',
  templateUrl: './addupdateschool-languages.component.html',
  styleUrls: []
})
export class AddupdateschoolLanguagesComponent implements OnInit {
  id: number = 0;
  row: any;

  form!: FormGroup;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateschoolLanguagesComponent>,
    private notifyService: NotificationService) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      'langCode': ['', Validators.required],
      'langName': ['', Validators.required],
      'langName2': ['', Validators.required],
      'isActive': [false, Validators.required]
    });
    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.form.patchValue(this.row);
    }
  }


  submit() {
    
  }


  reset() {
    this.form.reset();
  }


  closeModel() {
    this.dialogRef.close();
  }
}
