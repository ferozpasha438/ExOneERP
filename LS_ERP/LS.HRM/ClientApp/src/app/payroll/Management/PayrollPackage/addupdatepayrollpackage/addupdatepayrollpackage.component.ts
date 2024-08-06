import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormArray,
  AbstractControl,
} from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from 'src/app/api-authorization/AuthorizeService';
import { CustomSelectListItem } from 'src/app/models/MenuItemListDto';
import { TblPRLSysPayrollComponentDto } from 'src/app/models/Payroll/PayrollComponentDto';
import { TblPRLTrnPayrollPackageComponentDto } from 'src/app/models/Payroll/PayrollPackageComponentDto';
import { ApiService } from 'src/app/services/api.service';
import { NotificationService } from 'src/app/services/notification.service';
import {
  DBOperation,
  PayrollComponentType,
} from 'src/app/services/utility.constants';
import { UtilityService } from 'src/app/services/utility.service';
import { ValidationService } from 'src/app/sharedcomponent/ValidationService';
import { ParentpayrollmgtComponent } from 'src/app/sharedcomponent/parentpayrollmgt.component';

@Component({
  selector: 'app-addupdatepayrollpackage',
  templateUrl: './addupdatepayrollpackage.component.html',
  styles: [],
})
export class AddupdatepayrollpackageComponent
  extends ParentpayrollmgtComponent
  implements OnInit
{
  modalTitle!: string;
  modalBtnTitle!: string;
  dbops!: DBOperation;
  form!: FormGroup;
  id: number = 0;
  isReadOnly: boolean = false;
  positions: Array<CustomSelectListItem> = [];
  grades: Array<CustomSelectListItem> = [];
  payrollComponents: Array<CustomSelectListItem> = [];
  packageComponents: Array<TblPRLTrnPayrollPackageComponentDto> = [];
  grossSalary: number = 0;
  totalDeductions: number = 0;
  netPay: number = 0;
  isArab: boolean = false;
  ePayrollComponents: Array<TblPRLSysPayrollComponentDto> = [];
  dPayrollComponents: Array<TblPRLSysPayrollComponentDto> = [];

  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private authService: AuthorizeService,
    private utilService: UtilityService,
    public dialogRef: MatDialogRef<AddupdatepayrollpackageComponent>,
    private notifyService: NotificationService,
    private validationService: ValidationService,
    private translate: TranslateService
  ) {
    super(authService);
  }

  ngOnInit(): void {
    this.setForm();
    if (this.id > 0) this.setEditForm();
  }

  setForm() {
    this.form = this.fb.group({
      id: [0],
      packageCode: ['', Validators.required],
      packageNameEn: ['', Validators.required],
      packageNameAr: [''],
      gradeCode: [''],
      positionCode: [''],
      isActive: [false],
      basicSalary: ['', Validators.required],

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
    this.isReadOnly = false;
    this.loadGrades();
    this.loadPositions();
    this.loadpayrollComponents();
  }

  setEditForm() {
    this.apiService.get('PayrollPackage', this.id).subscribe((res) => {
      if (res) {
        this.isReadOnly = true;
        this.form.patchValue(res);
        let packageComponents = res[
          'packageComponents'
        ] as Array<TblPRLTrnPayrollPackageComponentDto>;
        packageComponents.forEach((item) => {
          this.packageComponents.push(item);
          this.calculateTotalPackage();
        });
      }
    });
  }

  get packageComponentsFrmArray(): FormArray {
    return <FormArray>this.form.get('packageComponents');
  }

  editItem(res: any) {
    this.packageComponentsFrmArray.push(this.fb.group(res));
  }

  removeItem(index: number) {
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

  closeModel() {
    this.dialogRef.close();
  }

  submit() {
    if (this.form.valid) {
      if (this.id > 0) this.form.controls['id'].setValue(this.id);
      if (this.packageComponents.length > 0) {
        this.packageComponents.forEach((item) => {
          this.editItem(item);
        });
      }
      this.apiService.post('PayrollPackage', this.form.value).subscribe(
        (res) => {
          this.utilService.OkMessage();
          this.reset();
          this.dialogRef.close(true);
        },
        (error) => {
          this.utilService.ShowApiErrorMessage(error);
        }
      );
    } else this.utilService.FillUpFields();
  }

  reset() {
    this.form.controls['packageCode'].setValue('');
    this.form.controls['packageNameEn'].setValue('');
    this.form.controls['packageNameAr'].setValue('');
    this.form.controls['gradeCode'].setValue('');
    this.form.controls['positionCode'].setValue('');
    this.form.controls['isActive'].setValue(false);
    this.form.controls['basicSalary'].setValue('');
    this.ResetAll('e');
    this.ResetAll('d');
  }

  loadpayrollComponents() {
    let queryParam = `isStructured=true`;
    this.apiService
      .getQueryString(`PayrollComponent/GetPayrollComponents?`, queryParam)
      .subscribe((res) => {
        if (res) {
          let payrollComponents: Array<TblPRLSysPayrollComponentDto> = res;
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

  loadPositions() {
    this.apiService
      .getall('Position/GetPositionSelectListItem')
      .subscribe((res) => {
        this.positions = res;
      });
  }

  loadGrades() {
    this.apiService.getall('Grade/GetGradeSelectListItem').subscribe((res) => {
      this.grades = res;
    });
  }

  AddBasicPayrollComponent(payrollComponentCode: string, event: any) {
    let payValue: number = event.target.value;

    //try to retrieve Basic Component.
    let payrollPackageComponent = this.packageComponents.find(
      (e) =>
        e.payrollComponentCode.toLocaleLowerCase() ===
        payrollComponentCode.toLocaleLowerCase()
    );
    //if Basic component is already existing
    if (payrollPackageComponent) {
      payrollPackageComponent.payValue = payValue;
      this.packageComponents.forEach((payrollPackageComponent) => {
        //if payvalue is calculated based on formula.
        if (payrollPackageComponent.isFormula) {
          payrollPackageComponent.payValue = this.CalculatePayValue(
            payrollPackageComponent.formulaQueryString
          );
        }
      });
      this.calculateTotalPackage();
    } else {
      let queryParam = `payrollComponentCode=${encodeURIComponent(
        '' + payrollComponentCode
      )}`;
      this.apiService
        .getQueryString(
          `PayrollComponent/GetPayrollComponentByCode?`,
          queryParam
        )
        .subscribe((res: any) => {
          if (res) {
            let item: any;
            let payrollComponentName = this.utilService.isArabic()
              ? res['payrollComponentNameAr']
              : res['payrollComponentNameEn'];
            item = {
              id: 0,
              packageCode: this.form.controls['packageCode'].value,
              payrollComponentCode: res['payrollComponentCode'],
              payrollComponentName: payrollComponentName,
              isFormula: res['isFormula'],
              payValue: payValue,
              payrollComponentType: res['payrollComponentType'],
              formulaQueryString: res['formulaQueryString'],
            };
            if (payValue) {
              this.packageComponents.push(item);
              this.calculateTotalPackage();
            }
          }
        });
    }
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
        this.form.controls['packageCode'].value
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

                if (isFormula)
                  payValue = this.CalculatePayValue(_formulaQueryString);
                else payValue = res['payValue'];
                this.ToggleIsFormula(payrollComponentType);
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

  AddPayrollComponent(payrollComponentType: string) {
    let payrollComponentCode: string =
      this.form.controls[payrollComponentType + 'PayrollComponentCode'].value;
    //Try to retrieve the component that's being added.
    let payrollPackageComponent = this.packageComponents.find(
      (e) =>
        e.payrollComponentCode.toLocaleLowerCase() ===
        payrollComponentCode.toLocaleLowerCase()
    );

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
        this.form.controls['packageCode'].value
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
                item = {
                  id: 0,
                  packageCode: this.form.controls['packageCode'].value,
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
                    this.form.controls[payrollComponentType + 'PayValue'].value,
                  payrollComponentType: res['payrollComponentType'],
                  formulaQueryString: res['formulaQueryString'],
                };

                //If PayValue is empty, do not add to the list.
                if (
                  this.form.controls[payrollComponentType + 'PayValue'].value
                ) {
                  this.packageComponents.push(item);
                  this.calculateTotalPackage();
                  this.ResetAll(payrollComponentType);
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

  ResetPayrollComponentsControls(payrollComponentType: string) {
    this.form.controls[payrollComponentType + 'IsFormula'].setValue(false);
    this.form.controls[payrollComponentType + 'FormulaQueryString'].setValue(
      ''
    );
    this.form.controls[payrollComponentType + 'PayValue'].setValue('');
    this.ToggleIsFormula(payrollComponentType);
  }

  ToggleIsFormula(payrollComponentType: string) {
    let isFormula: boolean =
      this.form.controls[payrollComponentType + 'IsFormula'].value;
    if (isFormula) {
      this.form.controls[payrollComponentType + 'PayValue'].disable();
    } else {
      this.form.controls[payrollComponentType + 'PayValue'].enable();
    }
    this.form.controls[payrollComponentType + 'FormulaQueryString'].disable();
  }

  ResetAll(payrollComponentType: string) {
    this.form.controls[payrollComponentType + 'PayrollComponentCode'].setValue(
      ''
    );
    this.ResetPayrollComponentsControls(payrollComponentType);
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
}
