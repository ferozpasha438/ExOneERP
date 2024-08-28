import { Component, Input, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { MatTabChangeEvent } from '@angular/material/tabs';

@Component({
  selector: 'app-employeemanagementtabs',
  templateUrl: './employeemanagementtabs.component.html',
})
export class EmployeemanagementtabsComponent implements OnInit {

  @Input() employeeNumber!: string;
  isTabDisabled: boolean = false;
  index: number = 0;

  constructor(public dialogRef: MatDialogRef<EmployeemanagementtabsComponent>,) { }

  ngOnInit(): void {
    this.isTabDisabled = (this.employeeNumber == "");
    this.index = 0;
  }

  onChange(event: MatTabChangeEvent) {
    const tab = event.tab.textLabel;
    this.index = event.index;
    console.log(tab);
    console.log(this.employeeNumber);
    console.log(event.index);
    if(tab===" Tab 1")
     {
       console.log("function want to implement");
     }
  }

  closeModel() {
    this.dialogRef.close(true);
  }
}

