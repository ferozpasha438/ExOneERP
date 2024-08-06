import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';

@Component({
  selector: 'app-addupdate-vehichle-master',
  templateUrl: './addupdate-vehicle-master.component.html',
  styleUrls: []  
})
export class AddupdateVehicleMasterComponent implements OnInit {
  id: number = 0;
  row: any;
  vehicleCompanyList: Array<any> = [];
  vehicleTypeList: Array<any> = [];
  vehicleBrandList: Array<any> = [];
 
  form!: FormGroup;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateVehicleMasterComponent>,
    private notifyService: NotificationService) {

  }

  ngOnInit(): void {
    this.form = this.fb.group({
      'registrationNumber': ['', Validators.required],
      'vehicleCompany': ['', Validators.required],
      'vehicleType': ['', Validators.required],
      'brand': ['', Validators.required],
      'chassisNumber': ['', Validators.required],
      'seatingCapacity': ['', Validators.required],
      'registeredRTORegion': ['', Validators.required],
      'registrationAuthority': ['', Validators.required],
      'vehicleValidityTill': ['', Validators.required],
      'vehicleCondition': ['', Validators.required],
      'vehicleOwnership': ['', Validators.required],
      'procurementLeasedOn': ['', Validators.required],
      'currentBookValue': ['', Validators.required],
      'annualLeaseValue': ['', Validators.required],
      'salvageBookValue': ['', Validators.required],
      'leaseEndDate': ['', Validators.required],
      'vehicleOwnerEnglish': ['', Validators.required],
      'vehicleOwnerArabic': ['', Validators.required],
      'meterReadingOnProcurement': ['', Validators.required],
      'currentMeterReading': ['', Validators.required],
      'vehicleNextMaintenanceDate': ['', Validators.required],
      'estimatedMileagePerKM': ['', Validators.required],
      'vehicleLastMaintenanceDate': ['', Validators.required],
      'estimatedServiceYear': ['', Validators.required],
      'fuelType': ['', Validators.required],
      'fuelTankCapacityInLitters': ['', Validators.required],
      'isActive': [true, Validators.required],
      'isVehicleGoodsCarrier': [true, Validators.required]
    });

    this.loadData();

    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.form.patchValue(this.row);
    }
  }

  
 
  submit() {

    if (this.form.valid) {

      if (this.id > 0)
        this.form.value['id'] = this.id;


      this.apiService.post('vehicleMaster', this.form.value)
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


  loadData() {


    this.apiService.getPagination('vehicleCompany', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res)
        this.vehicleCompanyList = res['items'];
    });


    this.apiService.getPagination('vehicleType', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res)
        this.vehicleTypeList = res['items'];
    });

    //this.apiService.getPagination('brand', this.utilService.getCalendarQueryString(0, 1000, '', '')).subscribe(res => {
    //  if (res)
    //    this.vehicleBrandList = res['items'];
    //});

    this.apiService.getPagination('brand', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res)
        this.vehicleBrandList = res['items'];
    });

  }

  reset() {
    this.form.reset();
  }


  closeModel() {
    this.dialogRef.close();
  }

}
