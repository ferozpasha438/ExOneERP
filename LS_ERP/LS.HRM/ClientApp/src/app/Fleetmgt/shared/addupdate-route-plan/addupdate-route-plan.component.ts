import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';

@Component({
  selector: 'app-addupdate-route-plan',
  templateUrl: './addupdate-route-plan.component.html',
  styleUrls: []  
})
export class AddupdateRoutePlanComponent implements OnInit {
  id: number = 0;
  row: any;
  cityList: Array<any> = [];
  areaList: Array<any> = [];
  routePlanDetails: Array<any> = [];
  routeCode: string = '';
  city: string = '';
  flag: string = '';
  editsequence: number = 0;
  sequence: number = 1;
  form!: FormGroup;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateRoutePlanComponent>,
    private notifyService: NotificationService) {

  }

  ngOnInit(): void {
    this.form = this.fb.group({
      'routePlanCode': ['', Validators.required],
      'routeNameEn': ['', Validators.required],
      'routeNameAr': ['', Validators.required],
      'routeCode': ['', Validators.required],
      'city': ['', Validators.required],
      'flag': ['', Validators.required],
      'isActive': [true, Validators.required]
    });
   this.loadData();
    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.form.patchValue(this.row);
      this.loadDetailsforCode(this.row['id']);
    }
  }

  loadDetailsforCode(id: number) {
    this.apiService.getall(`routePlan/GetRoutePlanInfoById/${id}`).subscribe(res => {
      if (res) {
        this.routePlanDetails = res['routePlanDetailsRows'] as Array<any>;
        this.routePlanDetails.forEach(item => {
          //this.feeAmount = item.feeAmount;
          //this.feeCode = item.feeCode;
          //this.termCode = item.termCode;
          //this.addInvoice();
          this.routeCode = item.routeCode;
          this.flag = item.flag;
        });
        this.addRoutePlan();
      }
    });

  }
 
  submit() {

    //if (this.form.valid) {

      if (this.id > 0)
        this.form.value['id'] = this.id;

      if (this.routePlanDetails.length > 0) {
        this.form.value['routePlanDetailsRows'] = this.routePlanDetails;

        this.apiService.post('routePlan', this.form.value)
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
      
    }
   
 // }

  setToDefault() {
    this.routeCode = '';
    this.city = '';
    this.flag = '';
    
  }

  addRoutePlan() {
    
    if (this.editsequence > 0) {
      var index: number = this.routePlanDetails.findIndex(a => a.sequence === this.editsequence);

      let pItem = this.routePlanDetails[index];
     
      pItem.sequence = this.editsequence;
     // pItem.city = this.city;
      pItem.routeCode = this.routeCode;
      pItem.flag = this.flag;
        

        this.editsequence = 0;
      }
      else {
      this.routePlanDetails.push({
        sequence: this.getSequence(),
       // city: this.city,
        routeCode: this.routeCode,
        flag: this.flag
          
        });
      }
      //this.setLabelPrices(this.total, this.vatAmount, '');
      //this.setGrandTotal();
      this.setToDefault();
    }


  getSequence(): number { return this.sequence = this.sequence + 1 };

  loadData() {
    //this.apiService.getall('routeMaster/getCitySelectList').subscribe(res => {
    //  this.cityList = res;
    // });
    //this.apiService.getPagination('routeMaster', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
    //    if (res)
    //      this.areaList = res['items'];
    //});

      this.apiService.getall('routeMaster/getCitySelectList').subscribe(res => {
        this.cityList = res;
      });
  
  }

  loadRelatedItems() {
    if (this.city != null && this.city != '') {
      this.apiService.getall(`routeMaster/getRouteListByCity/${this.city}`).subscribe(res => {
        if (res)
          this.areaList = res;
      });
    }
  }

  deleteItem(item: any) {
    //this.removeInvoiceList(item.sequence);
    this.removeList(item.sequence);
  }

  removeList(sequence: number) {
    let index: number = this.routePlanDetails.findIndex(a => a.sequence === sequence);
    this.routePlanDetails.splice(index, 1);
  }


  reset() {
    this.form.reset();
    this.setToDefault();
  }


  closeModel() {
    this.dialogRef.close();
  }

}
