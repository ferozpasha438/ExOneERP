import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-employeebasicinfo',
  templateUrl: './employeebasicinfo.component.html',
  styles: [
  ]
})
export class EmployeebasicinfoComponent implements OnInit {
  @Input() employeeBasicInfo!: any;

  constructor() { }

  ngOnInit(): void {
  }

  onFileChanged(event: any) {
    let reader = new FileReader();
    if (event.target.files && event.target.files.length > 0) {
      let file = event.target.files[0];
      reader.readAsDataURL(file);
      reader.onload = () => {
        this.employeeBasicInfo.employeeImageUrl = reader.result;
      };
    }
  };
}
