import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AuthorizeService } from 'src/app/api-authorization/AuthorizeService';
import { TblHRMTrnEmployeeContractInfoDto } from 'src/app/models/HumanResource/EmployeeContractInfoDto';
import {
  BaseEmployeePaySlipDto,
  EmployeePaySlipHeader,
  TblPRLTrnEmployeePayrollProcessDto,
} from 'src/app/models/Payroll/BaseEmployeePaySlipDto';
import { ApiService } from 'src/app/services/api.service';
import { PayrollComponentType } from 'src/app/services/utility.constants';
import { ParentpayrollmgtComponent } from 'src/app/sharedcomponent/parentpayrollmgt.component';

@Component({
  selector: 'app-accordion-item',
  templateUrl: './accordion-item.component.html',
  styles: [],
})
export class AccordionItemComponent
  extends ParentpayrollmgtComponent
  implements OnInit
{
  constructor(
    private authService: AuthorizeService,
    private apiService: ApiService
  ) {
    super(authService);
  }
  @Input() item!: TblHRMTrnEmployeeContractInfoDto;
  employeePaySlip!: BaseEmployeePaySlipDto;
  paySlipHeader!: EmployeePaySlipHeader;
  // paySlipDetails!: Array<TblPRLTrnEmployeePayrollProcessDto>;
  earningComponents: Array<TblPRLTrnEmployeePayrollProcessDto> = [];
  deductionComponents: Array<TblPRLTrnEmployeePayrollProcessDto> = [];
  @Output()
  onLastEmployeePayrollProcessed: EventEmitter<TblHRMTrnEmployeeContractInfoDto> =
    new EventEmitter<TblHRMTrnEmployeeContractInfoDto>();

  ngOnInit(): void {
    if (this.item.isPreRun) {
      this.RetrieveEmployeePayslip(this.item.employeeID);
    } else {
      this.ProcessEmployeePayroll(
        this.item.employeeID,
        this.item.isApproved!,
        this.item.isReleased!
      );
      //Update PayrollProcessFiltersLog
      if (this.item.id == this.item.lastEmployeeContractInfoId)
        this.onLastEmployeePayrollProcessed.emit(this.item);
    }
  }

  RetrieveEmployeePayslip(employeeID: number) {
    this.apiService.get('PayrollProcess', employeeID).subscribe((res) => {
      if (res) {
        this.employeePaySlip = res;
        this.paySlipHeader = this.employeePaySlip.paySlipHeader;

        if (this.employeePaySlip.paySlipDetails) {
          //Retrieve list of earnings
          this.earningComponents = this.employeePaySlip.paySlipDetails.filter(
            (e) =>
              e.payrollComponentType == PayrollComponentType.Earning ||
              e.payrollComponentType == PayrollComponentType.UnStructuredEarning
          );
          //Retrieve list of deductions
          this.deductionComponents = this.employeePaySlip.paySlipDetails.filter(
            (e) =>
              e.payrollComponentType == PayrollComponentType.Deduction ||
              e.payrollComponentType ==
                PayrollComponentType.UnStructuredDeduction
          );
        }
      }
    });
  }

  ProcessEmployeePayroll(
    employeeID: number,
    isApproved: boolean,
    isReleased: boolean
  ) {
    let queryParam = `employeeID=${encodeURIComponent(
      '' + employeeID
    )}&isApproved=${encodeURIComponent(
      '' + isApproved
    )}&isReleased=${encodeURIComponent('' + isReleased)}`;
    this.apiService
      .getQueryString(`PayrollProcess/ProcessEmployeePayroll?`, queryParam)
      .subscribe((res) => {
        if (res) {
          this.employeePaySlip = res;
          this.paySlipHeader = this.employeePaySlip.paySlipHeader;

          if (this.employeePaySlip.paySlipDetails) {
            //Retrieve list of earnings
            this.earningComponents = this.employeePaySlip.paySlipDetails.filter(
              (e) =>
                e.payrollComponentType == PayrollComponentType.Earning ||
                e.payrollComponentType ==
                  PayrollComponentType.UnStructuredEarning
            );
            //Retrieve list of deductions
            this.deductionComponents =
              this.employeePaySlip.paySlipDetails.filter(
                (e) =>
                  e.payrollComponentType == PayrollComponentType.Deduction ||
                  e.payrollComponentType ==
                    PayrollComponentType.UnStructuredDeduction
              );
          }
        }
      });
  }
}
