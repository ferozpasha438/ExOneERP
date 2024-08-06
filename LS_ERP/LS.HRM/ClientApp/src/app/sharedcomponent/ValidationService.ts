import { Injectable } from '@angular/core';
import { ValidatorFn } from '@angular/forms';


interface BooleanFn {
  (): boolean;
}
@Injectable({ providedIn: 'root' })
export class ValidationService {
  constructor() { }

  conditionalValidator(predicate: BooleanFn,
    validator: ValidatorFn,
    errorNamespace?: string): ValidatorFn {
    return (formControl => {
      if (!formControl.parent) {
        return null;
      }
      let error = null;
      if (predicate()) {
        error = validator(formControl);
      }
      //if (errorNamespace && error) {
      //  const customError = {};
      //  customError[errorNamespace] = error;
      //  error = customError
      //}
      return error;
    })
  }

  getValidatorErrorMessage(validatorName: string, validatorValue?: any) {

    const language = localStorage.getItem('language') ?? 'en';

    const config: any = {
      'required': language === 'en' ? 'Required' : 'مطلوب',
      'invalidCreditCard': language == 'en' ? 'Is invalid credit card number' : 'رقم بطاقة ائتمان غير صالح',
      'invalidEmailAddress': language == 'en' ? 'Invalid email address' : 'عنوان بريد إلكتروني غير صالح',
      'email': language == 'en' ? 'Invalid email address' : 'عنوان بريد إلكتروني غير صالح',
      'invalidMobile': language == 'en' ? 'Invalid Mobile no' :'الجوال غير صالح لا',
      'invalidCode': language == 'en' ? 'Format : (xx-xx-xxxxx)' :'التنسيق : (xx-xx-xxxxxx)',
      'invalidNumber': language == 'en' ? 'Invalid Number' : 'رقم غير صالح',
      'invalidDecimal': language == 'en' ? 'Invalid Decimal Number' : 'رقم عشري غير صحيح',
      'invalidPassword': language == 'en' ? 'Invalid password. Password must be at least 6 characters long, and contain a number.' : 'كلمة مرور غير صالحة. يجب أن يكون طول كلمة المرور 6 أحرف على الأقل، وأن تحتوي على رقم.',
      'minlength': language == 'en' ? `Minimum length ${validatorValue.requiredLength}` : `الحد الأدنى للطول  ${validatorValue.requiredLength}`,
      'maxlength': language == 'en' ? `Max length ${validatorValue.requiredLength}` : `الحد الأقصى للطول  ${validatorValue.requiredLength}`,
      'confirmedValidator': language === 'en' ? 'password mismatch' : 'عدم تطابق كلمة المرور'
    };
    return config[validatorName];
  }
  //emailValidator(control: any) {
  //  if (control.value.match(/[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/)) {
  //    return null;
  //  } else {
  //    return { 'invalidEmailAddress': true };
  //  }
  //}


  mobileOptionalValidator(control: any) {
    if (control.value.match(/^(\+\d{1,3}[- ]?)?\d{10}$/)) {
      return null;
    }
    else if (!control.value) {
      return null;
    }
    else {
      return { 'invalidMobile': true };
    }
  }

  mobileValidator(control: any) {
    if (control.value.match(/^(\+\d{1,3}[- ]?)?\d{10}$/)) {
      return null;
    } else {
      return { 'invalidMobile': true };
    }
  }

  mobile9Or10Validator(control: any) {
    if (control.value.match(/^(\+\d{1,3}[- ]?)?\d{9}$/) || control.value.match(/^(\+\d{1,3}[- ]?)?\d{10}$/)) {
      return null;
    } else {
      return { 'invalidMobile': true };
    }
  }
  accountValidator(control: any) {
    if (control.value && control.value !== '') {
      if (control.value.match(/(\d{2})-(\d{2})-(\d{5})/)) {
        return null;
      } else {
        return { 'invalidCode': true };
      }
    }
    return null;
  }
  numberValidator(control: any) {
    if (control.value.match(/^[0-9]*$/)) {
      return null;
    } else {
      return { 'invalidNumber': true };
    }
  }
  decimalValidator(control: any) {
    if (control.value.match(/^\d+[.,]?\d{0,3}$/)) {   
      return null;
    } else {
      return { 'invalidDecimal': true };
    }
  }
}
