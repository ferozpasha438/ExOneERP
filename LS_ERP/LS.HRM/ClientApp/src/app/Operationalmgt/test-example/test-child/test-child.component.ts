import { HttpClient } from '@angular/common/http';
import { EventEmitter, Input, Output } from '@angular/core';
import { Component, OnChanges, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { from, Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, mergeMap, startWith, switchMap } from 'rxjs/operators';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
//import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { DBOperation } from '../../../services/utility.constants';
import { UtilityService } from '../../../services/utility.service';
import { ParentOptMgtComponent } from '../../../sharedcomponent/parentoptmgt.component';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { Student } from '../../models/enquiries-dto.model';









@Component({
  selector: 'app-test-child',
  templateUrl: './test-child.component.html'
})

export class TestChildComponent {

  @Input()
  ctMsg: string = '';

  @Input('ctArray')
  myctArray = {} as Array<string>;

  @Input('studentAddMsg')
  addMsg: string = '';

  @Output('addStudentEvent')
  addStdEvent = new EventEmitter<Student>();

  @Output()
  sendMsgEvent = new EventEmitter<string>();

  student = new Student('', '');
  childTitle = '---Child One---';
  message = 'Send Message'
  msg = '';

  addStudent() {
    this.addStdEvent.emit(this.student);
  }
  sendMsg() {
    this.sendMsgEvent.emit(this.msg);
  }
  getVal(ob: EventTarget | null) {
    return (<HTMLInputElement>ob).value;
  }
}
