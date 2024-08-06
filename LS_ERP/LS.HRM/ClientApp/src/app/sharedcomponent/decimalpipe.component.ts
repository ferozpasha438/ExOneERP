import { Component, OnInit, Inject, LOCALE_ID } from '@angular/core';
import { formatNumber } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'decimalpipe'
})
export class DecimalPipe implements PipeTransform {

  transform(value: any, args: any[]) {
    //constructor(@Inject(LOCALE_ID) private locale: string) {    
    return formatNumber(value, "en", '1.2-2');

  }
}

@Pipe({
  name: 'decimalpipe2'
})
export class DecimalPipe2 implements PipeTransform {

  transform(value: any, args: any[]) {
    if (value < 0)
      return `- (${formatNumber((-1 * value), "en", '1.2-2') })`;

    return formatNumber(value, "en", '1.2-2');

  }
}
