import { Component, Input, OnInit } from '@angular/core';
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

  ngOnInit(): void {
    this.RetrieveEmployeePayslip(this.item.employeeID);
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
              e.payrollComponentType == PayrollComponentType.UnStructuredDeduction
          );
        }
      }
    });
  }
}
