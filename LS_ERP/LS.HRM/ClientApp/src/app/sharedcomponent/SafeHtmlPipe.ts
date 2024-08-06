import { formatNumber } from "@angular/common";
import { Pipe, PipeTransform } from "@angular/core";
import { DomSanitizer } from "@angular/platform-browser";

@Pipe({ name: "safeHtml" })
export class SafeHtmlPipe implements PipeTransform {
  constructor(private sanitizer: DomSanitizer) { }

  transform(value: any) {
    //if (value && typeof value === "string" && value.indexOf('http') > 0)
    //  return this.sanitizer.bypassSecurityTrustUrl(value);
    return this.sanitizer.bypassSecurityTrustHtml(value);
  }
}



@Pipe({ name: "leadingZeros" })
export class LeadingZerosPipe implements PipeTransform {
  constructor(private sanitizer: DomSanitizer) { }

  transform(value: any) {
    if (value !== undefined && value !== null) {
      if (parseFloat(value) >= 100)
        return formatNumber(value, 'en', '3.2-5');

      return formatNumber(value, 'en', '1.2-5');
    }
    return value;
  }
}
