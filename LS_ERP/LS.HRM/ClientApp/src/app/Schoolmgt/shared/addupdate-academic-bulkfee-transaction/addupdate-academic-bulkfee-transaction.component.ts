import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';

@Component({
  selector: 'app-addupdate-academic-bulkfee-transaction',
  templateUrl: './addupdate-academic-bulkfee-transaction.component.html',
  styleUrls: []
})
export class AddupdateAcademicBulkFeeTransactionComponent implements OnInit {
  id: number = 0;
  row: any;
  studentList: Array<any> = [];
  feeHeaderList: Array<any> = [];
  payTypes: Array<any> = [];
  termCode: string = '';
  totalAmount: number = 0.00;
  netAmount: number = 0.00;
  isArab: boolean = false;
  gradeCodeList: Array<any> = [];
  branchCodeList: Array<any> = [];
  sectionList: Array<any> = [];
  form!: FormGroup;
  isExactAmount: boolean = true;
  // Initialize an array to keep track of checkbox states
  checkboxStates: boolean[] = [];
  checkBoxIndex: number = 0;
  isFirstIndex: boolean = true;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateAcademicBulkFeeTransactionComponent>,
    private notifyService: NotificationService) {
    // Initialize the checkboxStates array with false values
    this.checkboxStates = this.feeHeaderList.map(() => false);
  }
  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.form = this.fb.group({
      "branchCode": ['', Validators.required],
      "gradeCode": ['', Validators.required],
      "sectionCode": ['', Validators.required],
      'admissionNumber': ['', Validators.required],
      'admissionDate': ['', Validators.required],
      'nameOfStudent': ['', Validators.required],
      'nameOfStudentInArabic': ['', Validators.required],
      'dueFeeAmount': ['', Validators.required],
      'paidAmount': [0, Validators.required],
      'remainingAmount': [0, Validators.required],
      'payCode': ['', Validators.required],
      'discountPercentage': [0, Validators.required],
      'discount': [0, Validators.required],
      'remarks': ['']
    });
    this.loadData();
    if (this.row) {
      this.id = parseInt(this.row['id']);
      this.form.patchValue(this.row);
    }
  }
  loadData() {
    //this.apiService.getall('SchoolStudentMaster/GetAllStudentsList').subscribe(res => {
    //  this.studentList = res;
    //});

    this.apiService.getall('schoolBranches/getSchoolBranchList').subscribe(res => {
      this.branchCodeList = res;
    });
    this.apiService.getPagination('acedemicClassGrade', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
      if (res)
        this.gradeCodeList = res['items'];
    });
  }
  loadRelatedItems() {
    const stuAdmNum: string = this.form.value['admissionNumber'] as string;
    if (stuAdmNum != null && stuAdmNum != '') {
      this.apiService.getall('SchoolStudentMaster/GetStudentDetailsByStuAdmNum?stuAdmNum=' + stuAdmNum).subscribe(res => {
        if (res) {
          this.form.patchValue({
            'admissionNumber': res.stuAdmNum,
            'admissionDate': res.admissionDate,
            'nameOfStudent': res.studentName,
            'nameOfStudentInArabic': res.studentNameAR,
            'dueFeeAmount': res.feeAmount
          });
          this.apiService.getall(`StudentPayType/GetAllBranchPayTypes/${res.branchCode}`).subscribe(res => {
            if (res)
              this.payTypes = res;
          });
        }
      });
      this.apiService.getall('SchoolStudentMaster/GetStudentFeeHeaderWithVatByStuAdmNum?stuAdmNum=' + stuAdmNum).subscribe(res => {
        if (res) {
          this.feeHeaderList = res;
        }
      });

    }
  }



  // shiva Check if the checkbox should be disabled
  //isCheckboxDisabled(index: number, netFeeAmount: number): boolean {
  //  if (index === 0) {
  //    return false; // The first checkbox is never disabled
  //  } else if (netFeeAmount > 0 && this.checkBoxIndex < index) {
  //    this.checkBoxIndex = index;
  //    return !this.checkboxStates[index - 1]; // Disable if the previous checkbox is not checked
  //  } else {
  //    return false;
  //  }
  //}


  // shiva Check if the checkbox should be disabled
  //getTotalAmount(row: any, isSelected: boolean, index: number) {
  //  this.checkboxStates[index] = isSelected;
  //  this.termCode = '';

  //  if (isSelected) {
  //    // Add the netFeeAmount to totalAmount when the checkbox is checked
  //    this.totalAmount += row.netFeeAmount;
  //  } else {
  //    // Subtract the netFeeAmount from totalAmount when the checkbox is unchecked
  //    this.totalAmount -= row.netFeeAmount;
  //  }

  //  this.termCode = row.termCode;

  //  this.form.patchValue({
  //    'paidAmount': this.totalAmount,
  //    'remainingAmount': 0
  //  });

  //  this.isExactAmount = true;

  //  let discountPer = this.form.value['discountPercentage'] as number;
  //  let discount = this.form.value['discount'] as number;

  //  if (discountPer != null && discountPer != undefined && discountPer > 0) {
  //    discount = (this.totalAmount / 100) * discountPer;
  //  } else if (discount != null && discount != undefined && discount > 0) {
  //    discountPer = (discount / this.totalAmount) * 100;
  //  }

  //  this.netAmount = this.totalAmount - discount;

  //  this.form.patchValue({
  //    'discountPercentage': discountPer,
  //    'discount': discount
  //  });
  //}




  ////test
  //isCheckboxDisabled(index: number, isFirst: boolean): boolean {
  //  if (index == 0 || isFirst) {
  //    this.isFirstIndex = false;
  //    return false; // The first checkbox is never disabled
  //  } else
  //    // Disable if the previous checkbox is not checked
  //    return !this.checkboxStates[index - 1];
  //}

 isCheckboxDisabled(index: number, netFeeAmount: number): boolean {
    if (index === 0) {
      return false; // The first checkbox is never disabled
    } else {
      return this.checkboxStates != undefined ? !this.checkboxStates[index - 1] : true; // Disable if the previous checkbox is not checked
    }
  }
  
  


  getTotalAmount(row: any, isSelected: boolean, index: number) {
    this.termCode = '';

    if (isSelected) {
      this.totalAmount += row.netFeeAmount;
      this.checkboxStates[index] = true;
    } else {
      this.totalAmount -= row.netFeeAmount;
      this.checkboxStates[index] = false;
      // Uncheck all subsequent checkboxes and deduct their amounts
      for (let i = index + 1; i < this.checkboxStates.length; i++) {
        if (this.checkboxStates[i]) {
          this.totalAmount -= this.feeHeaderList[i].netFeeAmount;
          this.checkboxStates[i] = false;
        }
      }
    }

    this.termCode = row.termCode;

    this.form.patchValue({
      paidAmount: this.totalAmount,
      remainingAmount: 0
    });

    this.isExactAmount = true;

    let discountPer = this.form.value['discountPercentage'] as number;
    let discount = this.form.value['discount'] as number;

    if (discountPer != null && discountPer != undefined && discountPer > 0) {
      discount = (this.totalAmount / 100) * discountPer;
    } else if (discount != null && discount != undefined && discount > 0) {
      discountPer = (discount / this.totalAmount) * 100;
    }

    this.netAmount = this.totalAmount - discount;

    this.form.patchValue({
      discountPercentage: discountPer,
      discount: discount
    });
  }


  changePaidAmount() {
    var paidAmount = this.form.value['paidAmount'] as number;
    if (paidAmount != null && paidAmount != undefined && paidAmount > 0) {
      this.form.patchValue({
        'remainingAmount': this.totalAmount - paidAmount
      });
      if (this.totalAmount === paidAmount) {
        this.isExactAmount = true;
        var discountPer = this.form.value['discountPercentage'] as number;
        var discount = this.form.value['discount'] as number;
        if (discountPer != null && discountPer != undefined && discountPer > 0) {
          discount = (paidAmount / 100) * discountPer;
        }
        else if (discount != null && discount != undefined && discount > 0) {
          discountPer = (discount / paidAmount) * 100;
        }
        this.netAmount = paidAmount - discount;
        this.form.patchValue({
          'discountPercentage': discountPer,
          'discount': discount
        });
      } else {
        this.isExactAmount = false;
        this.form.patchValue({
          'discountPercentage': 0,
          'discount': 0
        });
        this.netAmount = paidAmount;
      }

    }
  }
  changeDiscountPer() {
    var paidAmount = this.form.value['paidAmount'] as number;
    var discountPer = this.form.value['discountPercentage'] as number;
    if (discountPer != null && discountPer != undefined && discountPer > 0) {
      var discount = this.form.value['discount'] as number;
      discount = (paidAmount / 100) * discountPer;
      this.netAmount = paidAmount - discount;
      this.form.patchValue({
        'discountPercentage': discountPer,
        'discount': discount
      });
    }
  }
  changeDiscount() {
    var paidAmount = this.form.value['paidAmount'] as number;
    var discount = this.form.value['discount'] as number;
    if (discount != null && discount != undefined && discount > 0) {
      var discountPer = this.form.value['discountPercentage'] as number;
      discountPer = (discount / paidAmount) * 100;
      this.netAmount = paidAmount - discount;
      this.form.patchValue({
        'discountPercentage': discountPer,
        'discount': discount
      });
    }
  }
  //submit() {
  //  if (this.form.valid) {
  //    if (this.id > 0)
  //      this.form.value['id'] = this.id;
  //    this.form.value['termCode'] = this.termCode;
  //    this.apiService.post('studentFeeTransaction/createFeeTransaction', this.form.value)
  //      .subscribe(res => {
  //        this.utilService.OkMessage();
  //        this.reset();
  //        this.dialogRef.close(true);
  //      },
  //        error => {
  //          console.error(error);
  //          this.utilService.ShowApiErrorMessage(error);
  //        });
  //  }
  //  else
  //    this.utilService.FillUpFields();
  //}
  submit() {
    if (this.form.valid) {
      let selectedTermCodes = this.feeHeaderList
        .filter((item, index) => this.checkboxStates[index])
        .map(item => item.termCode);

      if (selectedTermCodes.length === 0) {
        this.utilService.FillUpFields(); // Or show an appropriate message
        return;
      }

      const formData = {
        ...this.form.value,
        id: this.id > 0 ? this.id : undefined,
        termCodes: selectedTermCodes
      };

      this.apiService.post('studentFeeTransaction/createBulkFeeTransaction', formData)
        .subscribe(res => {
          this.utilService.OkMessage();
          this.reset();
          this.dialogRef.close(true);
        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
          });
    } else {
      this.utilService.FillUpFields();
    }
  }

  loadStudentData(type: any) {
    const branchCode: string = this.form.value['branchCode'] as string;
    const gradeCode: string = this.form.value['gradeCode'] as string;
    const sectionCode: string = this.form.value['sectionCode'] as string;
    if (type === 2) {
      this.apiService.getall(`SchoolGradeSectionMapping/getAllSectionsByGradeCode/${gradeCode}`).subscribe(res => {
        if (res)
          this.sectionList = res;
      });
    }
    if (branchCode != null && branchCode != '' &&
      gradeCode != null && gradeCode != '' &&
      sectionCode != null && sectionCode != '') {
      this.apiService.getall(`StudentAttendanceRegister/StudentList/${branchCode}/${gradeCode}/${sectionCode}`).subscribe(res => {
        if (res) {
          this.studentList = res;
        }
      });
    }
  }
  reset() {
    this.form.reset();
    this.checkboxStates = [];
  }
  closeModel() {
    this.dialogRef.close();
  }
}
