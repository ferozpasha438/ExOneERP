import { Component, Input } from "@angular/core";
import { FormGroup } from "@angular/forms";

@Component({
  selector: 'spinner-loader',
  template: `
 <div *ngIf="isLoading"
                   style="display: flex; justify-content: center; align-items: center; background: white;">
                <mat-progress-spinner color="primary" strokeWidth="3" diameter="30"
                                      mode="indeterminate">
                </mat-progress-spinner>
              </div>
`})
export class SpinnerLoaderComponent {
  @Input() public isLoading: boolean = false;
}



export function ConfirmedValidator(controlName: string, matchingControlName: string) {
  return (formGroup: FormGroup) => {
    const control = formGroup.controls[controlName];
    const matchingControl = formGroup.controls[matchingControlName];
    if (matchingControl.errors && !matchingControl.errors.confirmedValidator) {
      return;
    }
    if (control.value !== matchingControl.value) {
      matchingControl.setErrors({ confirmedValidator: true });
    } else {
      matchingControl.setErrors(null);
    }
  }
}
