import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatDialog } from "@angular/material/dialog";
import { AuthorizeService } from "../../api-authorization/AuthorizeService";
import { ApiService } from "../../services/api.service";
import { NotificationService } from "../../services/notification.service";
import { UtilityService } from "../../services/utility.service";
import { ParentSystemSetupComponent } from "../../sharedcomponent/parentsystemsetup.component";
import { ValidationService } from "../../sharedcomponent/ValidationService";


@Component({
  selector: 'app-segmentsetup',
  templateUrl: './segmentsetup.component.html',
  styles: [
  ]
})
export class SegmentsetupComponent extends ParentSystemSetupComponent implements OnInit {

  list: Array<any>;
  allList: Array<any>;
  form: FormGroup;
  id: number = 0;
  isEdit: boolean = false;
  isLoading: boolean = false;
  constructor(private fb: FormBuilder, private apiService: ApiService, private authService: AuthorizeService,
    private notifyService: NotificationService, private validationService: ValidationService, private utilService: UtilityService, public dialog: MatDialog) {
    super(authService);
  }

  ngOnInit(): void {
    this.setForm();
    this.loadData();
  }


  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      'seg2Code': ['', Validators.required],
      'seg2Name': ['', Validators.required],
      'seg2Name2': ['', Validators.required],
      'isActive': false,
    });

  }


  loadData() {

    this.isLoading = true;
    this.apiService.getall('segmentSetup').subscribe(res => {
      this.isLoading = false;
      this.list = res;
      this.allList = res;
    });
  }

  searchFilter(value: string) {
    let filterValueLower = value.toLowerCase();
    if (value.trim() === '') {
      this.list = this.allList;
    }
    else {
      this.list = this.allList.filter((item) =>
        item.seg2Code?.toLowerCase().includes(filterValueLower) || item.seg2Name?.toLowerCase().includes(filterValueLower)
        || item.seg2Name2?.toLowerCase().includes(filterValueLower)
      );
    }
  }

  public edit(item: any) {
    this.id = parseInt(item['id']);
    this.isEdit = true;
    this.form.patchValue(item);

  }


  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;

      this.apiService.post('segmentSetup', this.form.value)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.cancel();
          this.loadData();
        },
          error => {
            this.utilService.ShowApiErrorMessage(error);
          });
    }
    else {      
      this.utilService.setMarkAllAsTouched(this.form);    
      //this.form.markAllAsTouched();
    }
  }
  cancel() {
    this.id = 0;
    this.isEdit = false;
    this.reset();

  }

  reset() {
    this.form.reset();
    //this.form.controls['branchCode'].setValue('');
    //this.form.controls['branchName'].setValue('');
    //this.form.controls['branchAddress'].setValue('');
  }

}
