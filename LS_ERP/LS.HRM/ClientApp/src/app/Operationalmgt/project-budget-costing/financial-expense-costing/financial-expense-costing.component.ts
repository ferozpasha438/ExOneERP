import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { UtilityService } from '../../../services/utility.service';
import { TranslateService } from '@ngx-translate/core';
import { ApiService } from '../../../services/api.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { NotificationService } from '../../../services/notification.service';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, mergeMap, startWith, switchMap } from 'rxjs/operators';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';



export interface TblSndDefSiteMasterDto {
  siteCode: string;
  siteName: string;
  siteArbName: string;

}

export interface ProjectBudgetCosting {
  customerCode: string;
  projectBudgetCostingId: number;
  projectBudgetEstimationId: number;
  projectCode: string;
  siteCode: string;
  financialExpenseCostingsList: Array<FinancialExpenseCosting>;
  serviceType: string;
  status: number;

}
export interface FinancialExpenseCosting {
  costPerUnit: number;
  projectBudgetCostingId: number;
  financialExpenseCostingId: number;
  financialExpenseCode: string;
  siteCode: string;
}



@Component({
  selector: 'app-financial-expense-costing',
  templateUrl: './financial-expense-costing.component.html'
})
export class FinancialExpenseCostingComponent extends ParentOptMgtComponent implements OnInit {
  project: any;
  form: FormGroup;
  projectBudgetCosting: ProjectBudgetCosting;

 
  financialExpenseCostingList: Array<any> = [];
  grandTotal: number = 0;
  totalForFinancialExpenseUnit: number = -1;
  
  financialExpenseCode: string = '';

  activeFinancialExpenseIndex: number = -1;
  siteCodeSelectList: Array<TblSndDefSiteMasterDto> = [];
  financialExpenseSelectionList: Array<any> = [];

  isDataLoading: boolean = false;
  

  filteredFinancialExpenses: Observable<Array<any>>;
  financialExpenseCodeControl = new FormControl('');
  isArab: boolean = false;
  filterFinancialExpenses(val: string): Observable<Array<any>> {
    return this.apiService.getall(`OperationExpenseHead/getAutoSelectListForFinancialExpense?search=${val}`)
      .pipe(
        map(response => {
          const res = response as Array<any>;
          this.isDataLoading = false;
          
          return res;
        })
      )
  }

















  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<FinancialExpenseCostingComponent>) {
    super(authService);

    this.filteredFinancialExpenses = this.financialExpenseCodeControl.valueChanges.pipe(
      startWith(this.financialExpenseCode),
      debounceTime(utilService.autoDelay()),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isDataLoading = true;
        return this.filterFinancialExpenses(val || '')
      })
    );

  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.setForm();
    this.getFinancialExpenseSelectionList();
  }
  getFinancialExpenseSelectionList() {
    this.apiService.getall(`OperationExpenseHead/getOperationExpenseHeadsForFinancialExpence`).subscribe(res => {

      this.financialExpenseCostingList = res;
      


    });

  }


  setForm() {

    this.form = this.fb.group({
      'projectCode': [this.project.projectCode],
      'customerCode': [this.project.customerCode],
      'siteCode': [this.project?.siteCode == null ? '' : this.project.siteCode, Validators.required],
      
      'financialExpenseCode': [''],
      'costPerUnit': ['']

    });

    this.apiService.getall(`customerSite/getSelectSiteListByProjectCode/${this.project.projectCode}`).subscribe(res => {

      if (res != null) {
        this.siteCodeSelectList = res;
        if (this.project?.siteCode != null) {
          this.loadInitialData();
          this.form.controls['siteCode'].disable({ onlySelf: true });
        }
      }

      else
        this.siteCodeSelectList = [];


    });


  }




  closeModel() {
    this.dialogRef.close();
  }
  submit() {
    //if (this.projectBudgetCosting.status == 3) {

    //  this.notifyService.showError('Estimation is in Progress');

    //}
    //else
      // if (this.form.valid && this.grandTotal != 0) {
      if (this.projectBudgetCosting.financialExpenseCostingsList.length!=0) {


        this.apiService.post('projectBudgetCosting/createProjectFinancialExpenseCostingForSite', this.projectBudgetCosting)
          .subscribe(res => {
            if (res) {

              this.utilService.OkMessage();

              this.dialogRef.close(true);

            }
          },
            error => {

              console.log(error)
              console.error(error);
              this.utilService.ShowApiErrorMessage(error);
            });
        
      }
      else {


        this.utilService.FillUpFields();

      }

  }


  loadInitialData() {
    this.apiService.getall(`ProjectBudgetCosting/getProjectFinancialExpenseCostingForSite/${this.project.customerCode}/${this.project.projectCode}/${this.form.controls['siteCode'].value}`).subscribe(res => {
      this.projectBudgetCosting = res;


      if (this.projectBudgetCosting.status == 0) {

        this.createNewProjectBudgetCosting();


      }
      else if (this.projectBudgetCosting.status != 3) {
       
      }
      this.findTotalsCosts();
    });
    

  }
  createNewProjectBudgetCosting() {

    this.projectBudgetCosting.customerCode = this.project.customerCode;
    this.projectBudgetCosting.projectCode = this.project.projectCode;
    this.projectBudgetCosting.siteCode = this.form.controls['siteCode'].value;

    this.projectBudgetCosting.serviceType = "MATERIALEQUIPMENT";
    this.createNewFinancialExpenseBudgetCostingList();
  }
  createNewFinancialExpenseBudgetCostingList() {


    this.projectBudgetCosting.financialExpenseCostingsList = [];


  }

  

  findTotalsCosts() {

    this.grandTotal = 0;

    if (this.projectBudgetCosting.financialExpenseCostingsList.length != 0) {
      this.projectBudgetCosting.financialExpenseCostingsList.forEach((plc:any) => {
       
        this.grandTotal = this.grandTotal + plc.costPerUnit;
      });

    }
  }





  onSelectSiteCode(event: any) {
   // this.projectBudgetCosting = null;


    this.totalForFinancialExpenseUnit = 0;
    this.form.controls['siteCode'].setValue(event.target.value);
    this.loadInitialData();
    this.activeFinancialExpenseIndex = -1;
  }




 
  totalPerFinancialExpense(lsc: any): number {
    return lsc.costPerUnit;

  }


  onSelectionFinancialExpense(event: any) {

    let name = !this.isArab ? event.option.value.costNameInEnglish : event.option.value.costNameInArabic;
    
    this.form.controls['financialExpenseCode'].setValue(name);
    this.financialExpenseCodeControl.setValue(name);
    this.financialExpenseCode = event.option.value.costHead;

    let index = this.projectBudgetCosting.financialExpenseCostingsList.findIndex(e => e.financialExpenseCode == event.option.value.costHead);
    if (index >= 0) {
      this.form.controls['costPerUnit'].setValue(this.projectBudgetCosting.financialExpenseCostingsList[index].costPerUnit)
    }
    else {
      this.form.controls['costPerUnit'].setValue('');
    }
  }



  addUpdateFinancialExpense() {

    


    if (this.form.controls['financialExpenseCode'].value != '' && this.financialExpenseCodeControl.value != '' && this.form.controls['costPerUnit'].value!='' ) {
      
      let financialExpense: any = {
        costPerUnit: this.form.controls['costPerUnit'].value,
       
        projectBudgetCostingId: this.projectBudgetCosting.projectBudgetCostingId,

        financialExpenseCostingId: 0,
       
        financialExpenseCode: this.financialExpenseCode,

        


      };


      let index = this.projectBudgetCosting.financialExpenseCostingsList.findIndex(e => e.financialExpenseCode == this.financialExpenseCode);
      if (index >= 0) {
        this.projectBudgetCosting.financialExpenseCostingsList[index].costPerUnit= this.form.controls['costPerUnit'].value;
      }
      else {
        this.projectBudgetCosting.financialExpenseCostingsList.push(financialExpense);
      }





      this.findTotalsCosts();

    }
    else {
           this.notifyService.showError(`${this.translate.instant('Incomplete Data')}`);
    }
    this.financialExpenseCode = '';
    this.financialExpenseCodeControl.setValue('');
    this.form.controls['costPerUnit'].setValue('');
  }





  removeFinExpense(index:number) {
    this.projectBudgetCosting.financialExpenseCostingsList.splice(index, 1);
    this.findTotalsCosts();
  }


  getFinExpence(code: string) {
      return this.financialExpenseCostingList.find(e => e.costHead == code);


  }

  skippEstimationType() {
    let input: any = {
      projectCode: this.project.projectCode,
      siteCode: this.project.siteCode,
      type: "financeExpence"
    };

    this.apiService.post('projectBudgetCosting/skippEstimationType', input)
      .subscribe(res => {
        if (res) {

          this.utilService.OkMessage();

          this.dialogRef.close(true);

        }
      },
        error => {

          console.log(error)
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });

  }
}

