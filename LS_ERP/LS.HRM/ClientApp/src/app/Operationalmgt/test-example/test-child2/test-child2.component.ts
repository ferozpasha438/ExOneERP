import { EventEmitter, Input, Output } from '@angular/core';
import { Component, OnInit } from '@angular/core';
//import { Student } from '../test-example.component';

@Component({
  selector: 'app-test-child2',
  templateUrl: './test-child2.component.html'
})
export class TestChild2Component{
  @Input()
  studentMsg: string = '';

  @Input('stdLeader')
//  myStdLeader = {} as Student;

  @Output('addNumberEvent')
  addNumEvent = new EventEmitter<number>();

  childTitle = '---Child Two---';
  addNumMsg = 'Add Number'
  num1 = '';
  num2 = '';

  addNumber() {
    this.addNumEvent.emit(parseInt(this.num1) + parseInt(this.num2));
  }
  getVal(ob: EventTarget | null) {
    return (<HTMLInputElement>ob).value;
  }
}
