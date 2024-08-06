import { OnInit } from "@angular/core";
import { Input } from "@angular/core";
import { Component } from "@angular/core";
import { AuthorizeService } from "../api-authorization/AuthorizeService";

@Component({
  selector: 'submit-button',
  template: `
<ng-container *ngIf="IsSubmitting">  
<button type="button" class="btn btn-block waves-effect waves-light btn-rounded  btn-primary">{{title}}</button>
</ng-container>
<ng-container *ngIf="!IsSubmitting">
<button type="submit" class="btn btn-block waves-effect waves-light btn-rounded  btn-primary">{{title}}</button>  
</ng-container>
`})
export class SubmitLoaderComponent implements OnInit {
  @Input() public title = 'Save';
  IsSubmitting: boolean = false;

  constructor(private authService: AuthorizeService) {

  }
  ngOnInit(): void {
    this.authService.IsSubmitting().subscribe(isSubmitting => {
      this.IsSubmitting = isSubmitting;
    });
  }
}

@Component({
  selector: 'submitsave-button',
  template: `
<ng-container *ngIf="IsSubmitting">
<input type="button" class="btn waves-effect waves-light btn-rounded btn-primary btn-midblock" value="wait.." />
</ng-container>
<ng-container *ngIf="!IsSubmitting">
      <input type="submit" class="btn waves-effect waves-light btn-rounded btn-primary btn-midblock" value="{{title}}" />
</ng-container>
`})
export class SubmitSaveLoaderComponent implements OnInit {
  @Input() public title = 'SAVE';  
  IsSubmitting: boolean = false;

  constructor(private authService: AuthorizeService) {

  }
  ngOnInit(): void {
    this.authService.IsSubmitting().subscribe(isSubmitting => {
      this.IsSubmitting = isSubmitting;
    });
  }
}
