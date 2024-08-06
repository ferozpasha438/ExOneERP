import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

@Component({
  selector: 'app-leaverequestother',
  templateUrl: './leaverequestother.component.html',
  styles: [
  ]
})
export class LeaverequestotherComponent implements OnInit {
  @Input() remarks: string = '';
  @Output() remarksEvent = new EventEmitter<string>();
  constructor() { }

  ngOnInit(): void {
  }
  addRemarks(value: string) {    
    this.remarksEvent.emit(value);
  }
}
