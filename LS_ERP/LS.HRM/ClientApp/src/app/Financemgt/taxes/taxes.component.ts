import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../models/MenuItemListDto';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';
import { DeleteConfirmDialogComponent } from '../../sharedcomponent/delete-confirm-dialog';
import { ParentSystemSetupComponent } from '../../sharedcomponent/parentsystemsetup.component';
import { ValidationService } from '../../sharedcomponent/ValidationService';
import { AddupdatetaxslabComponent } from '../sharedpages/addupdatetaxslab/addupdatetaxslab.component';



@Component({
  selector: 'app-taxes',
  templateUrl: './taxes.component.html',
  //styleUrls: ['./taxes.component.scss']  

})
export class TaxesComponentFinance extends ParentSystemSetupComponent implements OnInit {
  form: FormGroup;
  taxList: Array<any> = [];
  id: number = 0;
  isLoading: boolean = false;

  constructor(private fb: FormBuilder, private apiService: ApiService, private authService: AuthorizeService, private utilService: UtilityService,
    private validationService: ValidationService, private notifyService: NotificationService, public dialog: MatDialog
  ) { super(authService); }

  ngOnInit(): void {
    this.initialLoading();
  }
  initialLoading() {
    this.isLoading = true;
    this.apiService.getall('taxes/getAllTaxList').subscribe(res => {
      this.isLoading = false;
      if (res)
        this.taxList = res;
    });

  }

  private openDialogManage(id: number = 0) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, AddupdatetaxslabComponent);
    (dialogRef.componentInstance as any).id = id;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.initialLoading();
    });
  }



  public create() {
    this.openDialogManage();
  }

  public edit(id: number) {
    this.openDialogManage(id);
  }

  public delete(id: number) {
    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete && id > 0) {
        this.apiService.delete('', id).subscribe(res => {
          this.initialLoading();
          this.utilService.OkMessage();
        },
        );
      }
    },
      error => this.utilService.ShowApiErrorMessage(error));
  }



}
