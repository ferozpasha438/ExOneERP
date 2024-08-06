import { Injectable } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, ValidatorFn } from '@angular/forms';

export default class Validation {

  static isCodeExist(controlName: string, isExistControl: string): ValidatorFn {
    return (controls: AbstractControl) => {
      let control = controls.get(controlName) as any;
      let control2 = controls.get(isExistControl) as any;
      if (control.value == '') {
        control.setErrors({ required: true });
        return { required: true };
      }
      else if (control2.value) {
        control.setErrors({ isExist: true });
        return { isExist: true };
      }
      else {
        return null;
      }
    }
  }
}
