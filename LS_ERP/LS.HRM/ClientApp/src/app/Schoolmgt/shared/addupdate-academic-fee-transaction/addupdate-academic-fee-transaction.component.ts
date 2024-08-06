import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';

@Component({
  selector: 'app-addupdate-academic-fee-transaction',
  templateUrl: './addupdate-academic-fee-transaction.component.html',
  styleUrls: []
})
export class AddupdateAcademicFeeTransactionComponent implements OnInit {
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
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<AddupdateAcademicFeeTransactionComponent>,
    private notifyService: NotificationService) {

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
      this.apiService.getall('SchoolStudentMaster/GetStudentDetailsByStuAdmNum?stuAdmNum='+stuAdmNum).subscribe(res => {
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
      this.apiService.getall('SchoolStudentMaster/GetStudentFeeHeaderWithVatByStuAdmNum?stuAdmNum='+stuAdmNum).subscribe(res => {
        if (res) {
          this.feeHeaderList = res;
        }
      });
      
    }
  }
  //getTotalAmount(row:any,isSelected:boolean) {
  //  if (isSelected) {
  //    this.totalAmount = this.totalAmount + row.netFeeAmount;
  //    this.termCodes.push(row.termCode);
  //  } else {
  //    this.totalAmount = this.totalAmount - row.netFeeAmount;
  //    this.termCodes.forEach((item, index) => {
  //      if (item === row.termCode) this.termCodes.splice(index, 1);
  //    });
  //  }
  //  var discountPer = this.form.value['discountPercentage'] as number;
  //  var discount = this.form.value['discount'] as number;
  //  if (discountPer != null && discountPer != undefined && discountPer > 0) {
  //    discount = (this.totalAmount / 100) * discountPer;
  //  }
  //  else if (discount != null && discount != undefined && discount > 0) {
  //    discountPer = (discount / this.totalAmount) * 100;
  //  }
  //  this.netAmount = this.totalAmount - discount;
  //  this.form.patchValue({
  //    'discountPercentage': discountPer,
  //    'discount': discount
  //  });
  //}
  getTotalAmount(row: any, isSelected: boolean) {
    this.termCode = '';
    this.totalAmount = row.netFeeAmount;
    this.termCode=row.termCode;
    this.form.patchValue({
      'paidAmount': this.totalAmount,
      'remainingAmount': 0
    });
    this.isExactAmount = true;
    var discountPer = this.form.value['discountPercentage'] as number;
    var discount = this.form.value['discount'] as number;
    if (discountPer != null && discountPer != undefined && discountPer > 0) {
      discount = (this.totalAmount / 100) * discountPer;
    }
    else if (discount != null && discount != undefined && discount > 0) {
      discountPer = (discount / this.totalAmount) * 100;
    }
    this.netAmount = this.totalAmount - discount;
    this.form.patchValue({
      'discountPercentage': discountPer,
      'discount': discount
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
  submit() {
    if (this.form.valid) {
      if (this.id > 0)
        this.form.value['id'] = this.id;
      this.form.value['termCode'] = this.termCode;
      this.apiService.post('studentFeeTransaction/createFeeTransaction', this.form.value)
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
  }
  closeModel() {
    this.dialogRef.close();
  }
}
