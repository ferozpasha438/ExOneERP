import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatSlideToggleChange, MatSlideToggleModule } from '@angular/material/slide-toggle';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';
import { ValidationService } from '../../sharedcomponent/ValidationService';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { ParentSystemSetupComponent } from '../../sharedcomponent/parentsystemsetup.component';
import { DBOperation } from '../../services/utility.constants';
import { AddingtopologyComponent } from '../../SystemSetup/sharedpages/addingtopology/addingtopology.component';
@Component({
  selector: 'app-financialsetup',
  templateUrl: './financialsetup.component.html',
  //styleUrls: ['./financialsetup.component.scss']  
  
})
export class FinancialsetupComponent extends ParentSystemSetupComponent implements OnInit {
  financialsetupform!: FormGroup;
  isLoading!: boolean;
  canShow: boolean = true;
  IsMaskingChecked: boolean = false;

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialog: MatDialog,
    private notifyService: NotificationService, private validationService: ValidationService, public pageService: PaginationService) {
    super(authService);
  }

  ngOnInit(): void {
    this.setForm();
    this.loadSetUp();
  }
  //fInSysGenAcCode
  loadSetUp() {
    this.isLoading = true;
    this.apiService.get('financialsetup', 0)
      .subscribe(data => {
        this.isLoading = false;
        if (data && parseInt(data['id']) > 0) {
          this.canShow = false;

          this.financialsetupform.patchValue(data);
          this.financialsetupform.controls['fyYear'].setValue(new Date(data.fyClosingDate).getFullYear());
          //this.financialsetupform.setValue({
          //  'fyOpenDate': data.fyOpenDate,
          //  'fyClosingDate': data.fyClosingDate,
          //  'fyYear': data.fyYear,
          //  'finAcCatLen': data.finAcCatLen,
          //  'finAcSubCatLen': data.finAcSubCatLen,
          //  'finAcLen': data.finAcLen,
          //  'finAllowNextYearTran': data.finAllowNextYearTran,
          //  'finTranDateAsPostDate': data.finTranDateAsPostDate,
          //  'fInSysGenAcCode': data.fInSysGenAcCode
          //});
        }
        else
          this.IsMaskingChecked = true;

      });
  }

  setForm() {
  
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.financialsetupform = this.fb.group({
      'fyOpenDate': ['', Validators.required],
      'fyClosingDate': ['', Validators.required],
      'fyYear': ['', Validators.required],
      'finAcCatLen': ['', Validators.required],
      'finAcSubCatLen': ['', Validators.required],
      'finAcLen': ['', Validators.required],
      'finAllowNextYearTran': false,
      'finTranDateAsPostDate': false,
      'fInSysGenAcCode': false,
      'paymentMethod': ['', Validators.required],
      'numOfSeg': '',
      'userCostSeg': false,
      'minCutOffShortAmt': ['', Validators.required],
      'maxCutOffOverAmr': ['', Validators.required],
      'arDistFlag': false,
      //'finBranchPrefixLen': ['', Validators.required],
      //'finAcFormat': ['', Validators.required],
    });
  }

  setfyYear(event: any) {
    this.financialsetupform.controls['fyYear'].setValue(new Date(event).getFullYear());
  }
  setFinYear() {
    const fyOpenDate = this.financialsetupform.controls['fyOpenDate'];
    console.log(fyOpenDate);
  }

  financialsetupSubmit() {
    if (this.financialsetupform.valid) {

      this.financialsetupform.value['fyOpenDate'] = this.utilService.selectedDate(this.financialsetupform.controls['fyOpenDate'].value);
      this.financialsetupform.value['fyClosingDate'] = this.utilService.selectedDate(this.financialsetupform.controls['fyClosingDate'].value);           

      this.authService.SetSubmitting(true);
      this.apiService.post('financialsetup', this.financialsetupform.value)
        .subscribe(data => {
          this.authService.SetSubmitting(false);
          this.utilService.OkMessage();
        },
          error => {
            this.utilService.ShowApiErrorMessage(error);
            this.authService.SetSubmitting(false);
          });
    }
    else
      this.utilService.FillUpFields();
  }

  private openDialogManage(id: number = 0) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, AddingtopologyComponent);   
    (dialogRef.componentInstance as any).id = id;

    dialogRef.afterClosed().subscribe(res => {
              
    });
  }
  MaskingChecked(event: MatSlideToggleChange) {
    this.IsMaskingChecked = !event.checked;
  }

  openAccountCode() {
    this.openDialogManage(0);
  }

}
