import { HttpClient } from '@angular/common/http';
import { Component, OnChanges, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { from, Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, mergeMap, startWith, switchMap } from 'rxjs/operators';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
//import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';
import { ValidationService } from '../../../sharedcomponent/ValidationService';


@Component({
  selector: 'app-create-update-proposal-costings',
  templateUrl: './create-update-proposal-costings.component.html'
})
export class CreateUpdateProposalCostingsComponent  extends ParentOptMgtComponent implements OnInit {
  modalTitle: string;
  modalBtnTitle: string;
  dbops: DBOperation;
  form: FormGroup;
  id: number = 0;
  inputData: any;
  total = 0;
  editRowNumber: number = -1;
  editRowItem: any = {
    id: 0, qty: 0, itemEng: "", itemArab: "", price: 0, total: 0, projectBudgetEstimationId:0
  };
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private translate: TranslateService,
    private notifyService: NotificationService, private validationService: ValidationService, public dialogRef: MatDialogRef<CreateUpdateProposalCostingsComponent>) {
    super(authService);}

  ngOnInit(): void {

    this.form = this.fb.group({
    });
    if (this.inputData.proposalCosting.length == 0) {
      this.notifyService.showError(this.translate.instant("No_Costings_Found"));
      this.closeModel();
    }
    this.getTotal();

  }

 

  

  submit() {





    this.apiService.post('proposalCosting', this.inputData.proposalCosting)
      .subscribe(res => {
        if (res) {

          this.utilService.OkMessage();

          this.dialogRef.close(true);

        }
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
              });


        }



  closeModel() {
    this.dialogRef.close();
  }

  edit(row: number) {
    this.editRowNumber = row;
    this.editRowItem = this.inputData.proposalCosting[row];
   
  }
  delete(row: number) {
    if (this.inputData.proposalCosting.length>1) {
      this.inputData.proposalCosting.splice(row, 1);
      this.editRowNumber = -1;
      this.getTotal();
    }
  }
  translateToolTip(text: string) {
    return this.translate.instant(text);
  }

  getTotal() {
     this.total = 0;
    this.inputData.proposalCosting.forEach((e: any) => {
      e.total = e.price * e.qty;
      this.total +=e.total;
    });
  }
  add_new() {

    let newItem: any = {
      id: 0, qty: 0, itemEng: "", itemArab: "", price: 0, total: 0, projectBudgetEstimationId:this.inputData.proposalCosting[0].projectBudgetEstimationId
    };
    this.inputData.proposalCosting.push(newItem);
    this.editRowItem = newItem;
    this.editRowNumber = this.inputData.proposalCosting.length-1;
  }
  save(row: number) {

    if (this.editRowItem.qty > 0 && this.editRowItem.itemEng != "" && this.editRowItem.itemArab != "" && this.editRowItem.price >= 0) {
      this.inputData.proposalCosting[row] = this.editRowItem;

      this.editRowNumber = -1;
      this.getTotal();
      this.editRowItem= {
        id: 0, qty: 0, itemEng: "", itemArab: "", price: 0, total: 0, projectBudgetEstimationId: this.inputData.proposalCosting[0].projectBudgetEstimationId
      };
    }
  }
  cancelEdit() {

    console.log(this.inputData.proposalCosting);
    this.editRowNumber = -1;
  }
  updateTotal() {
    this.editRowItem.total = this.editRowItem.price * this.editRowItem.qty;
  }
}
