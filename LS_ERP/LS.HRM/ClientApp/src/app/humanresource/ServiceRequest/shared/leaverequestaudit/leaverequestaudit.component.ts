import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-leaverequestaudit',
  templateUrl: './leaverequestaudit.component.html',
  styles: [
  ]
})
export class LeaverequestauditComponent implements OnInit {
  @Input() requestAudits: any;
  constructor() { }

  ngOnInit(): void {
  }

}
