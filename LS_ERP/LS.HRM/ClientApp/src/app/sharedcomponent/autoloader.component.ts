import { Component, Input } from "@angular/core";

@Component({
  selector: 'auto-loader',
  template: `
<div *ngIf="isLoading"
                   style="display: flex; justify-content: center; align-items: center; background: white;">
  <mat-progress-spinner color="primary" strokeWidth="3" diameter="20"
                                      mode="indeterminate">
                </mat-progress-spinner></div>
`})
export class AutoLoaderComponent {
  @Input() public isLoading: boolean = false;
}
