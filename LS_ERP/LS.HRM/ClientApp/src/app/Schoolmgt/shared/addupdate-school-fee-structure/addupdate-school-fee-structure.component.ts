import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';



@Component({
  selector: 'app-addupdate-school-fee-structure',
  templateUrl: './addupdate-school-fee-structure.component.html',
  styleUrls: []
})
export class AddupdateSchoolFeeStructureComponent implements OnInit {
  id: number = 0;
  row: any;
  feeTypeList: Array<any> = [];
  termsCodeList: Array<any> = [];
  gradeCodeList: Array<any> = [];

  isArab: boolean = false;
  form!: FormGroup;
  termCode: string = '';
  feeCode: string = '';
  feeAmount: number = 0;
  feeStructureDetails: Array<any> = [];
  editsequence: number = 0;
  sequence: number = 1;

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateSchoolFeeStructureComponent>,
    private notifyService: NotificationService) {

  }
  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();

    this.form = this.fb.group({
      'feeStructCode': ['', Validators.required],
      'gradeCode': ['', Validators.required],
      'feeStructName': ['', Validators.required],
      'feeStructName2': ['', Validators.required],
      'applyLateFee': ['', Validators.required],
      'branchCode': ['', Validators.required],
      'lateFeeCode': ['', Validators.required],
      'actualFeePayable': ['', Validators.required],
      'remarks': ['', Validators.required],
      'isActive': [false, Validators.required]
    });

    this.loadData();

    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.form.patchValue(this.row);
      this.loadDetailsforCode(this.row.feeStructCode);
    }
  }

  loadDetailsforCode(feeStructCode: string) {
    this.apiService.getall(`schoolFeeStructure/getSchoolFeeDetailsByFeeStructCode/${feeStructCode}`).subscribe(res => {
      if (res) {
        const details = res['feeDetailList'] as Array<any>;
        details.forEach(item => {

          this.feeAmount = item.feeAmount;
          this.feeCode = item.feeCode;
          this.termCode = item.termCode;
          this.addInvoice();
        });
      }
    });

  }

  addInvoice() {
    if (this.feeAmount != 0) {
      if (this.editsequence > 0) {
        var index: number = this.feeStructureDetails.findIndex(a => a.sequence === this.editsequence);

        let pItem = this.feeStructureDetails[index];
        pItem.sequence = this.editsequence;
        pItem.feeAmount = this.feeAmount;
        pItem.feeCode = this.feeCode;
        pItem.termCode = this.termCode;

        this.editsequence = 0;
      }
      else {
        this.feeStructureDetails.push({
          sequence: this.getSequence(),
          feeAmount: this.feeAmount,
          feeCode: this.feeCode,
          termCode: this.termCode
        });
      }
      //this.setLabelPrices(this.total, this.vatAmount, '');
      //this.setGrandTotal();
      this.setToDefault();
    }
  }

  getSequence(): number { return this.sequence = this.sequence + 1 };

  deleteInvoiceItem(item: any) {
    this.removeInvoiceList(item.sequence);
    //  this.setGrandTotal();
  }
  removeInvoiceList(sequence: number) {
    let index: number = this.feeStructureDetails.findIndex(a => a.sequence === sequence);
    this.feeStructureDetails.splice(index, 1);
  }

  editInvoiceItem(item: any) {

    this.editsequence = item.sequence,
      this.feeAmount = item.feeAmount;
    this.feeCode = item.feeCode;
    this.termCode = item.termCode;
  }
  setToDefault() {
    this.feeAmount = 0;
    this.feeCode = this.termCode = '';
  }

  loadData() {
    this.apiService.getPagination('schoolFeeType', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res)
        this.feeTypeList = res['items'];
    });

    this.apiService.getPagination('schoolFeeTerms', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res)
        this.termsCodeList = res['items'];
    });

    this.apiService.getPagination('acedemicClassGrade', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res)
        this.gradeCodeList = res['items'];
    });

  }



  submit() {
    if (this.form.valid) {

      if (this.id > 0)
        this.form.value['id'] = this.id;

      if (this.feeStructureDetails.length > 0) {
        this.form.value['feeDetailList'] = this.feeStructureDetails;

        this.apiService.post('schoolFeeStructure', this.form.value)
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
    else
      this.utilService.FillUpFields();
  }


  reset() {
    this.form.reset();
    this.setToDefault();
  }

  closeModel() {
    this.dialogRef.close();
  }

}
