import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { EmployeeBasicInfoDto } from 'src/app/models/HumanResource/EmployeeBasicInfo';

@Component({
  selector: 'app-employeebasicinfo',
  templateUrl: './employeebasicinfo.component.html',
  styles: [],
})
export class EmployeebasicinfoComponent implements OnInit {
  @Input() employeeBasicInfo!: EmployeeBasicInfoDto;
  @Output() employeeBasicInfoChange: EventEmitter<EmployeeBasicInfoDto> =
    new EventEmitter<EmployeeBasicInfoDto>();

  constructor() {}

  ngOnInit(): void {}

  onFileChanged(event: any) {
    let reader = new FileReader();
    if (event.target.files && event.target.files.length > 0) {
      let file = event.target.files[0];
      reader.readAsDataURL(file);
      reader.onload = () => {
        this.employeeBasicInfo.file = file;
        this.employeeBasicInfo.employeeImageUrl = reader.result;
        this.employeeBasicInfoChange.emit(this.employeeBasicInfo);
      };
    }
  }
}
