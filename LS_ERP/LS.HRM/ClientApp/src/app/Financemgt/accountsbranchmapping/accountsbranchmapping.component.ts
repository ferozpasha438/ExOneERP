import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatDialog } from '@angular/material/dialog';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../models/MenuItemListDto';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { ParentSystemSetupComponent } from '../../sharedcomponent/parentsystemsetup.component';

@Component({
  selector: 'app-accountsbranchmapping',
  templateUrl: './accountsbranchmapping.component.html',
  //styleUrls: ['./accountsbranchmapping.component.scss']  

})
export class AccountsbranchmappingComponent extends ParentSystemSetupComponent implements OnInit {
  form: FormGroup;
  branchCodeControl = new FormControl();
  filteredOptions: Observable<Array<CustomSelectListItem>>;
  acCodeList: Array<any> = [];
  allAcCodeList: Array<any> = [];
  isBranchLoading: boolean = false;
  isChecked: boolean = false;
  isMainChecked: boolean = false;
  branchCodeError: string = '';
  isLoading: boolean = false;

  constructor(private fb: FormBuilder, private apiService: ApiService, private authService: AuthorizeService,
    private utilService: UtilityService, private notifyService: NotificationService, public dialog: MatDialog,
    public pageService: PaginationService) {
    super(authService);

    this.filteredOptions = this.branchCodeControl.valueChanges.pipe(
      startWith(''),
      debounceTime(400),
      distinctUntilChanged(),
      switchMap((val: string) => {
        if (val.trim() !== '')
          this.isBranchLoading = true;
        return this.filter(val || '')
      })
    );
  }

  ngOnInit(): void {
    this.loadAcCodes();
  }

  filter(val: string): Observable<Array<CustomSelectListItem>> {
    return this.apiService.getall(`branch/getSelectSysBranchList?search=${val.trim()}`)
      .pipe(
        map(response => {
          const res = response as Array<CustomSelectListItem>;
          //if (res && res.length == 0)
          //  this.notifyService.showError("enter branch name")
          this.isBranchLoading = false;
          return res;
        })
      )
  }

  loadAcCodes() {
    this.isLoading = true;
    this.apiService.getall(`accountsbranchmapping/getBranchAccountMappingList`).subscribe(res => {
      this.isLoading = false;
      if (res) {
        this.acCodeList = res;
        this.allAcCodeList = res;
      }
    });
  }

  searchFilter(value: string) {
    let filterValueLower = value.toLowerCase();
    if (value.trim() === '') {
      this.acCodeList = this.allAcCodeList;
    }
    else {      
      this.acCodeList = this.allAcCodeList.filter((item) =>
        item.finAcDesc?.toLowerCase().includes(filterValueLower) || item.finAcCode.includes(filterValueLower)
      );
    }
  }

  listOfAcCodes: Array<string> = [];
  selectMapping(event: MatSlideToggleChange, id: string) {
    const isChecked = event.checked;
    if (isChecked) {
      this.listOfAcCodes.push(id);
    }
    else {
      let index: number = this.listOfAcCodes.findIndex(a => a === id);
      this.listOfAcCodes.splice(index, 1);
    }
  }


  checkAll(evt: MatSlideToggleChange) {
    this.isMainChecked = evt.checked;
    if (evt.checked) {
      this.acCodeList.forEach(item => {
        this.listOfAcCodes.push(item.finAcCode);
      });
      this.isChecked = true;
      // this.setCheckBoxItems(true);
    }
    else {
      this.listOfAcCodes = [];
      this.isChecked = false;
    }

  }

  blurBranchCode(event: any) {
    this.apiService.getall(`branch/checkBranchCode?branchCode=${event.target.value}`).subscribe(res => {
      if (!res) {
        //if (!this.isOptionSelected) {
        //this.isOptionSelected = false;
        this.branchCodeControl.setValue('');
        this.branchCodeError = "Select BranchCode";
        this.form.patchValue({ 'finBranchName': '', 'finBranchAddress': '', });
        //}
      }
    });
  }

  branchCodeSelected(event: MatAutocompleteSelectedEvent) {
   // this.branchCodeControl.setValue(event.option.value);
    this.branchCodeError = '';
    this.isMainChecked = false;
    this.isChecked = false;

  }
  //setCheckBoxItems(isChecked: boolean) {
  //  var ele = document.getElementsByName('checkItem') as any;
  //  for (var i = 0; i < ele.length; i++) {
  //    if (ele[i].type == 'checkbox')
  //      ele[i].checked = isChecked;
  //  }
  //}

  submit() {

    if (this.listOfAcCodes.length > 0) {
      let finBranchCode = this.branchCodeControl.value as string;
      if (this.utilService.hasValue(finBranchCode)) {
        this.apiService.post('accountsbranchmapping', { 'finBranchCode': finBranchCode, acCodeList: this.listOfAcCodes })
          .subscribe(res => {
            this.utilService.OkMessage();
            this.listOfAcCodes = [];
            // this.setCheckBoxItems(false);
            this.isChecked = false;
            this.isMainChecked = false;
            // this.form.reset();
          },
            error => {
              this.utilService.ShowApiErrorMessage(error);
            });

      }
      else
        this.utilService.FillUpFields();
    }
    else
      this.utilService.FillUpFields();


  }
}
