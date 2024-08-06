import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import {map, startWith} from 'rxjs/operators';
import {FormControl} from '@angular/forms';
import {Observable} from 'rxjs';

@Component({
  selector: 'app-addupdate-student-result',
  templateUrl: './addupdate-student-result.component.html', 
})
export class AddupdateStudentResultComponent implements OnInit {
  id: number = 0;
  row: any;
  form!: FormGroup;
  myControl = new FormControl('');
  options: string[] = ['One', 'Two', 'Three'];
  filteredOptions!: Observable<string[]>;
  isShown: boolean = false ;
  isSecShown: boolean = false ;

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateStudentResultComponent>,
    private notifyService: NotificationService) {

  }
  ngOnInit(): void {
    this.form = this.fb.group({
      'subject': ['', Validators.required],
      'maximumMarks': ['', Validators.required],
      'qualifyingMarks': ['', Validators.required],
      'marksObtained': ['', Validators.required],     
      'grade': ['', Validators.required],          
        
          
    });

    this.filteredOptions = this.myControl.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value || '')),
    );
    
    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.form.patchValue(this.row);
    }
  }
 


  toggleShow() {
    this.isShown = ! this.isShown;    
    }
  toggleShowsec() {
    this.isSecShown = ! this.isSecShown;    
    }
  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.options.filter(option => option.toLowerCase().includes(filterValue));
  }

submit() {
  if (this.form.valid) {

    if (this.id > 0)
      this.form.value['id'] = this.id;      

    this.apiService.post('acedemicClassGrade', this.form.value)
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
  this.form.reset();
}

closeModel() {
  this.dialogRef.close();
}

}


