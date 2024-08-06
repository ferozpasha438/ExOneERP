import { Component, Input } from "@angular/core";
import { FormGroup, FormControl, AbstractControl } from "@angular/forms";
import { ValidationService } from "./ValidationService";
@Component({
  selector: "validation-message",
  template: `<div class="text-danger" *ngIf="errorMessage !== null">{{errorMessage}}</div>`,
  //styleUrls: ["validation-messages.component.scss"],
})
export class ValidationMessagesComponent {

  @Input() control: AbstractControl;  // While using this component, need to pass relative control as input.  
  constructor(private validationService: ValidationService) { }

  get errorMessage() {
    for (const propertyName in this.control.errors) {
      if (this.control && this.control.errors.hasOwnProperty(propertyName) && this.control.touched) {
        return this.validationService.getValidatorErrorMessage(
          propertyName,
          this.control.errors[propertyName]
        );
      }
    }
    return null;
  }
}
