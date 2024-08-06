import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from 'src/app/api-authorization/AuthorizeService';
import { CustomSelectListItem } from 'src/app/models/MenuItemListDto';
import { TblPRLTrnEmployeePayrollStructureDto } from 'src/app/models/Payroll/EmployeePayrollStructureDto';
import { TblPRLTrnPayrollPackageComponentDto } from 'src/app/models/Payroll/PayrollPackageComponentDto';
import { ApiService } from 'src/app/services/api.service';
import { NotificationService } from 'src/app/services/notification.service';
import { PayrollComponentType } from 'src/app/services/utility.constants';
import { UtilityService } from 'src/app/services/utility.service';
import { ParentpayrollmgtComponent } from 'src/app/sharedcomponent/parentpayrollmgt.component';
import { EmployeemanagementtabsComponent } from '../Sharedcomponent/employeemanagementtabs/employeemanagementtabs.component';
import { TblPRLSysPayrollComponentDto } from 'src/app/models/Payroll/PayrollComponentDto';

@Component({
  selector: 'app-getemployeepayrollstructure',
  templateUrl: './getemployeepayrollstructure.component.html',
  styles: [],
})
export class GetemployeepayrollstructureComponent
  extends ParentpayrollmgtComponent
  implements OnInit
{
  form!: FormGroup;
  @Input() employeeNumber!: string;
  ePayrollComponents: Array<TblPRLSysPayrollComponentDto> = [];
  dPayrollComponents: Array<TblPRLSysPayrollComponentDto> = [];
  payrollPackages: Array<CustomSelectListItem> = [];
  employeeBasicInfo: any;
  gradeCode!: string;
  positionCode!: string;
  packageComponents: Array<TblPRLTrnEmployeePayrollStructureDto> = [];
  grossSalary: number = 0;
  totalDeductions: number = 0;
  netPay: number = 0;
  isArab: boolean = false;
  btnAddTitle: string = this.translate.instant('AddPayrollComponent');

  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private authService: AuthorizeService,
    private translate: TranslateService,
    private utilService: UtilityService,
    private notifyService: NotificationService,
    public dialogRef: MatDialogRef<EmployeemanagementtabsComponent>
  ) {
    super(authService);
  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.setForm();
    if (this.employeeNumber != '') this.setEditForm();
  }

  setForm() {
    this.form = this.fb.group({
      id: ['', Validators.required],
      ePayrollComponentCode: [{ value: '', disabled: false }],
      eIsFormula: [{ value: false, disabled: true }],
      eFormulaQueryString: [''],
      ePayValue: [''],
      dPayrollComponentCode: [{ value: '', disabled: false }],
      dIsFormula: [{ value: false, disabled: true }],
      dFormulaQueryString: [''],
      dPayValue: [''],
      packageComponents: this.fb.array([]),
    });
    this.loadpayrollComponents();
  }

  setEditForm() {
    this.GetEmployeePersonalInformation();
    this.GetEmployeeContractInformation();
  }

  GetEmployeePersonalInformation() {
    this.apiService
      .getQueryString(
        `PersonalInformation/GetEmployeePersonalInformationById?employeeNumber=`,
        this.employeeNumber
      )
      .subscribe((res) => {
        if (res) {
          res.allowImageUpload = false;
          this.employeeBasicInfo = res;
        }
      });
  }

  GetEmployeeContractInformation() {
    let queryParam = `employeeID=${encodeURIComponent(
      '' + Number(this.employeeNumber)
    )}`;
    this.apiService
      .getQueryString(
        `EmployeeContract/GetEmployeeContractInformationById?`,
        queryParam
      )
      .subscribe((res) => {
        if (res) {
          if (res['gradeCode'] != null)
            this.gradeCode = res['gradeCode'] as string;
          if (res['positionCode'] != null)
            this.positionCode = res['positionCode'] as string;
          this.loadpayrollPackages();
          this.GetEmployeePayrollPackageDetails();
        }
      });
  }

  loadpayrollComponents() {
    let queryParam = `isStructured=true`;
    this.apiService
      .getQueryString(`PayrollComponent/GetPayrollComponents?`, queryParam)
      .subscribe((res) => {
        if (res) {
          let payrollComponents: Array<TblPRLSysPayrollComponentDto> =
            res;
          payrollComponents.forEach(
            (e) =>
              (e.payrollComponentName = this.isArab
                ? e.payrollComponentNameAr
                : e.payrollComponentNameEn)
          );
          this.ePayrollComponents = payrollComponents.filter(
            (e) => e.payrollComponentType == PayrollComponentType.Earning
          );
          this.dPayrollComponents = payrollComponents.filter(
            (e) => e.payrollComponentType == PayrollComponentType.Deduction
          );
        }
      });
    }

  get packageComponentsFrmArray(): FormArray {
    return <FormArray>this.form.get('packageComponents');
  }

  editItem(res: any) {
    this.packageComponentsFrmArray.push(this.fb.group(res));
  }

  loadpayrollPackages() {
    let queryParam = `GradeCode=${encodeURIComponent(
      '' + this.gradeCode
    )}&PositionCode=${encodeURIComponent('' + this.positionCode)}`;
    this.apiService
      .getQueryString(
        `PayrollPackage/GetPayrollPackageSelectListItem?`,
        queryParam
      )
      .subscribe((res) => {
        this.payrollPackages = res;
      });
  }

  GetEmployeePayrollPackageDetails() {
    this.apiService
      .get('EmployeePayrollStructured', Number(this.employeeNumber))
      .subscribe((res) => {
        if (res) {
          if ((res['id'] as number) == 0) this.form.controls['id'].setValue('');
          else this.form.controls['id'].setValue(res['id']);

          let packageComponents = res[
            'packageComponents'
          ] as Array<TblPRLTrnEmployeePayrollStructureDto>;
          packageComponents.forEach((item) => {
            this.packageComponents.push(item);
            this.calculateTotalPackage();
          });
        }
      });
  }

  ImportPayrollPackageDetails() {
    if (this.form.controls['id'].value) {
      this.apiService
        .get('PayrollPackage', this.form.controls['id'].value)
        .subscribe((res) => {
          if (res) {
            let packageComponents = res[
              'packageComponents'
            ] as Array<TblPRLTrnPayrollPackageComponentDto>;
            packageComponents.forEach((item) => {
              let packageComponent: TblPRLTrnEmployeePayrollStructureDto =
                item as TblPRLTrnEmployeePayrollStructureDto;
              packageComponent.id = this.form.controls['id'].value;
              packageComponent.employeeID = Number(this.employeeNumber);
              packageComponent.isUsedInPayroll = false;
              this.packageComponents.push(packageComponent);
              this.calculateTotalPackage();
            });
          }
        });
    }
  }

  AddPayrollComponent(payrollComponentType: string) {
    let payrollComponentCode: string =
      this.form.controls[payrollComponentType + 'PayrollComponentCode'].value;
    //Try to retrieve the component that's being added.
    let payrollPackageComponent = this.packageComponents.find(
      (e) =>
        e.payrollComponentCode.toLocaleLowerCase() ===
        payrollComponentCode.toLocaleLowerCase()
    );

    //Add Payroll component.
    if (
      !this.form.controls[payrollComponentType + 'PayrollComponentCode']
        .disabled
    ) {
      //Check for Basic component's existence
      //try to retrieve Basic Component.
      let basicSalaryComponent = this.packageComponents.filter(
        (e) =>
          e.payrollComponentCode.toLocaleLowerCase() ===
          'BASIC'.toLocaleLowerCase()
      );
      //Check if Basic Salary is existing.
      if (basicSalaryComponent.length == 1) {
        if (
          this.form.controls[payrollComponentType + 'PayrollComponentCode']
            .value &&
          this.form.controls['id'].value
        ) {
          //Check if its already existing.
          if (!payrollPackageComponent) {
            let queryParam = `payrollComponentCode=${encodeURIComponent(
              '' + payrollComponentCode
            )}`;
            this.apiService
              .getQueryString(
                `PayrollComponent/GetPayrollComponentByCode?`,
                queryParam
              )
              .subscribe((res) => {
                if (res) {
                  let item: any;
                  let formulaQueryString: string = '';
                  if (
                    this.form.controls[payrollComponentType + 'IsFormula'].value
                  )
                    formulaQueryString =
                      this.form.controls[
                        payrollComponentType + 'FormulaQueryString'
                      ].value;
                  item = {
                    id: this.form.controls['id'].value,
                    employeeID: Number(this.employeeNumber),
                    payrollComponentCode:
                      this.form.controls[
                        payrollComponentType + 'PayrollComponentCode'
                      ].value,
                    payrollComponentName: this.isArab
                      ? res['payrollComponentNameAr']
                      : res['payrollComponentNameEn'],
                    isFormula:
                      this.form.controls[payrollComponentType + 'IsFormula']
                        .value,
                    payValue:
                      this.form.controls[payrollComponentType + 'PayValue']
                        .value,
                    payrollComponentType: res['payrollComponentType'],
                    formulaQueryString: formulaQueryString,
                  };

                  //If PayValue is empty, do not add to the list.
                  if (
                    this.form.controls[payrollComponentType + 'PayValue'].value
                  ) {
                    this.packageComponents.push(item);
                    this.calculateTotalPackage();
                    this.form.controls[
                      payrollComponentType + 'PayrollComponentCode'
                    ].setValue('');
                    this.ResetPayrollComponentsControls(payrollComponentType);
                  }
                }
              });
          } else {
            this.notifyService.showError(
              this.translate.instant('PayrollComponentAlreadyExisting'),
              'Error'
            );
          }
        } else this.utilService.FillUpFields();
      } else {
        this.notifyService.showError(
          this.translate.instant('AddBasicComponent'),
          'Error'
        );
      }
    } else {
      if (payrollPackageComponent) {
        if (this.form.controls[payrollComponentType + 'IsFormula'].value)
          payrollPackageComponent.formulaQueryString =
            this.form.controls[
              payrollComponentType + 'FormulaQueryString'
            ].value;
        payrollPackageComponent.payValue =
          this.form.controls[payrollComponentType + 'PayValue'].value;
        //If PayValue is empty, do not add to the list.
        if (this.form.controls[payrollComponentType + 'PayValue'].value) {
          //Remove the existing item from array.
          let index = this.packageComponents.findIndex(
            (e) =>
              e.payrollComponentCode.toLocaleLowerCase() ===
              payrollComponentCode.toLocaleLowerCase()
          );
          this.packageComponents.splice(index, 1);

          //Push the modified item
          this.packageComponents.push(payrollPackageComponent);

          //Re-Calculate the payvalue for formula based components.
          this.packageComponents.forEach((e) => {
            //if payvalue is calculated based on formula.
            if (e.isFormula) {
              e.payValue = this.CalculatePayValue(e.formulaQueryString);
            }
          });

          //Re-Calculate the Total Package.
          this.calculateTotalPackage();
          this.ResetAll(payrollComponentType);
        }
      }
    }
  }

  UpdatePayrollComponent(
    payrollComponentCode: string,
    payrollComponentType: string
  ) {
    //Try to retrieve the component that's being added.
    let payrollPackageComponent = this.packageComponents.find(
      (e) =>
        e.payrollComponentCode.toLocaleLowerCase() ===
        payrollComponentCode.toLocaleLowerCase()
    );

    //If exists, populate the values in controls.
    if (payrollPackageComponent) {
      this.form.controls[
        payrollComponentType + 'PayrollComponentCode'
      ].setValue(payrollPackageComponent.payrollComponentCode);
      this.form.controls[
        payrollComponentType + 'PayrollComponentCode'
      ].disable();
      this.form.controls[payrollComponentType + 'IsFormula'].setValue(
        payrollPackageComponent.isFormula
      );
      this.form.controls[payrollComponentType + 'FormulaQueryString'].setValue(
        payrollPackageComponent.formulaQueryString
      );
      this.form.controls[payrollComponentType + 'PayValue'].setValue(
        payrollPackageComponent.payValue
      );
      this.btnAddTitle = this.translate.instant('UpdatePayrollComponent');
      if (!payrollPackageComponent.isFormula)
        this.form.controls[payrollComponentType + 'PayValue'].enable();
      else this.form.controls[payrollComponentType + 'PayValue'].disable();
    }
  }

  RemovePayrollComponent(index: number) {
    let basicSalaryIndex = this.packageComponents.findIndex(
      (e) =>
        e.payrollComponentCode.toLocaleLowerCase() ===
        'BASIC'.toLocaleLowerCase()
    );
    if (basicSalaryIndex != index) {
      this.packageComponents.splice(index, 1);
      this.calculateTotalPackage();
    } else
      this.notifyService.showError(
        this.translate.instant('CannotDeleteBasicSalary'),
        'Error'
      );
  }

  OnPayrollComponentChange(payrollComponentType: string) {
    //Check for Basic component's existence
    //try to retrieve Basic Component.
    let basicSalaryComponent = this.packageComponents.filter(
      (e) =>
        e.payrollComponentCode.toLocaleLowerCase() ===
        'BASIC'.toLocaleLowerCase()
    );
    //Check if Basic Salary is existing.
    if (basicSalaryComponent.length == 1) {
      if (
        this.form.controls[payrollComponentType + 'PayrollComponentCode']
          .value &&
        this.form.controls['id'].value
      ) {
        let payrollComponentCode: string =
          this.form.controls[payrollComponentType + 'PayrollComponentCode']
            .value;
        //Try to retrieve the component that's being added.
        let payrollPackageComponent = this.packageComponents.find(
          (e) =>
            e.payrollComponentCode.toLocaleLowerCase() ===
            payrollComponentCode.toLocaleLowerCase()
        );
        this.ResetPayrollComponentsControls(payrollComponentType);
        //Check if its already existing.
        if (!payrollPackageComponent) {
          let payValue: number;
          let queryParam = `payrollComponentCode=${encodeURIComponent(
            '' + payrollComponentCode
          )}`;
          this.apiService
            .getQueryString(
              `PayrollComponent/GetPayrollComponentByCode?`,
              queryParam
            )
            .subscribe((res) => {
              if (res) {
                let isFormula: boolean = res['isFormula'] as boolean;
                let _formulaQueryString: string = res['formulaQueryString'];
                this.form.controls[payrollComponentType + 'IsFormula'].setValue(
                  isFormula
                );
                this.form.controls[
                  payrollComponentType + 'FormulaQueryString'
                ].setValue(_formulaQueryString);
                if (isFormula) {
                  payValue = this.CalculatePayValue(_formulaQueryString);
                } else payValue = res['payValue'];
                this.form.controls[payrollComponentType + 'PayValue'].setValue(
                  payValue
                );
              }
            });
        } else {
          this.notifyService.showError(
            this.translate.instant('PayrollComponentAlreadyExisting'),
            'Error'
          );
        }
      } else this.utilService.FillUpFields();
    } else {
      this.notifyService.showError(
        this.translate.instant('AddBasicComponent'),
        'Error'
      );
    }
  }

  ResetAll(payrollComponentType: string) {
    this.form.controls[payrollComponentType + 'PayrollComponentCode'].setValue(
      ''
    );
    this.form.controls[payrollComponentType + 'PayrollComponentCode'].enable();
    this.ResetPayrollComponentsControls(payrollComponentType);
  }

  ResetPayrollComponentsControls(payrollComponentType: string) {
    this.form.controls[payrollComponentType + 'IsFormula'].setValue(false);
    this.form.controls[payrollComponentType + 'FormulaQueryString'].setValue(
      ''
    );
    this.form.controls[payrollComponentType + 'PayValue'].setValue('');
    this.btnAddTitle = this.translate.instant('AddPayrollComponent');
  }

  OnFormulaQueryStringChange(event: any, payrollComponentType: string) {
    let formulaQueryString: string = event.target.value;
    let payValue: number = 0;
    payValue = this.CalculatePayValue(formulaQueryString);
    this.form.controls[payrollComponentType + 'PayValue'].setValue(payValue);
  }

  CalculatePayValue(formulaeQueryString: string) {
    let _payValue: number = 0;
    //formulaQueryString  will be in the format of "Basic*(10/100);HRA*(5/100)"
    if (formulaeQueryString) {
      let formulae: string[] = formulaeQueryString.split(';');
      if (formulae.length > 0) {
        formulae.forEach((formula) => {
          let expression: string[] = formula.split('*');
          if (expression.length == 2) {
            if (expression[1].includes('('))
              expression[1] = expression[1].replace('(', '');
            if (expression[1].includes(')'))
              expression[1] = expression[1].replace(')', '');
            let _fraction: string[] = expression[1].split('/');
            let _percentage: number = 0;
            if (_fraction.length == 2) _percentage = Number(_fraction[0]);
            let _payrollComponentCode: string = expression[0];
            if (!formulaeQueryString.includes('gross')) {
              // let payrollPackageComponent =
              //   this.packageComponents.controls.find(
              //     (group) =>
              //       (group as FormGroup).get('payrollComponentCode')?.value ===
              //       _payrollComponentCode
              //   ) as FormGroup | undefined;
              try {
                let payrollPackageComponent = this.packageComponents.find(
                  (e) =>
                    e.payrollComponentCode.toLocaleLowerCase() ===
                    _payrollComponentCode.toLocaleLowerCase()
                );
                if (payrollPackageComponent) {
                  if (payrollPackageComponent.isFormula) {
                    //Calculates payvalue for all the earning or deduction payroll components those are based on 'BASIC' salary.
                    _payValue =
                      _payValue +
                      (_percentage / 100) *
                        this.CalculatePayValue(
                          payrollPackageComponent.formulaQueryString
                        );
                  } else
                    _payValue =
                      _payValue +
                      (_percentage / 100) * payrollPackageComponent.payValue;
                }
              } catch (e) {
                console.log((e as Error).message);
              }
            } else {
              _payValue = _payValue + (_percentage / 100) * this.grossSalary;
            }
          }
        });
      }
    }
    return _payValue;
  }

  calculateTotalPackage() {
    try {
      this.grossSalary = this.packageComponents
        .filter((e) => e.payrollComponentType == PayrollComponentType.Earning)
        .map((a) => a.payValue)
        .reduce(function (a, b) {
          return Number(a) + Number(b);
        });

      if (
        this.packageComponents.filter(
          (e) => e.payrollComponentType == PayrollComponentType.Deduction
        ).length > 0
      ) {
        this.totalDeductions = this.packageComponents
          .filter(
            (e) => e.payrollComponentType == PayrollComponentType.Deduction
          )
          .map((a) => a.payValue)
          .reduce(function (a, b) {
            return Number(a) + Number(b);
          });
      } else this.totalDeductions = 0;

      this.netPay = this.grossSalary - this.totalDeductions;
      console.log(this.grossSalary);
      console.log(this.totalDeductions);
      console.log(this.netPay);
    } catch (e) {
      console.log((e as Error).message);
    }
  }

  closeModel() {
    this.dialogRef.close();
  }

  submit() {
    if (this.form.valid) {
      if (this.packageComponents.length > 0) {
        this.packageComponents.forEach((item) => {
          this.editItem(item);
        });
      }
      this.apiService
        .post('EmployeePayrollStructured', this.form.value)
        .subscribe(
          (res) => {
            this.utilService.OkMessage();
            this.ResetAll('e');
            this.ResetAll('d');
          },
          (error) => {
            this.utilService.ShowApiErrorMessage(error);
          }
        );
    } else this.utilService.FillUpFields();
  }
}
