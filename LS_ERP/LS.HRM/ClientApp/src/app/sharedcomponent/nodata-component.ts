import { Component, Input } from "@angular/core";

@Component({
  selector: 'no-apidata',
  template: `
 <div style="display: flex; justify-content: center; align-items: center; background: white;" class="text-danger">
   <mat-icon>priority_high</mat-icon> No Records
 </div>
`})
export class NoDataComponent {

}
