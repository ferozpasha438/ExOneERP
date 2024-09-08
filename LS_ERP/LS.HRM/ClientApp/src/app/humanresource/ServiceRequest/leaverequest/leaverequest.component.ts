import { Component, Input, OnInit } from '@angular/core';
import { ParentHrmAdminComponent } from '../../../sharedcomponent/ParentHrmAdmin.component';
import { FormBuilder } from '@angular/forms';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { NotificationService } from '../../../services/notification.service';
import { ApiService } from '../../../services/api.service';
import { UtilityService } from '../../../services/utility.service';
import { ValidationService } from '../../../sharedcomponent/ValidationService';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { CustomSelectListItem } from '../../../models/MenuItemListDto';
import * as moment from 'moment/moment';
import { StickyDirection } from '@angular/cdk/table';
import { DeleteConfirmDialogComponent } from '../../../sharedcomponent/delete-confirm-dialog';
@Component({
  selector: 'app-leaverequest',
  templateUrl: './leaverequest.component.html',
  styles: [
  ]
})
export class LeaverequestComponent extends ParentHrmAdminComponent implements OnInit {
  remarks: string = '';
  //employeeNumber: string = '';
  employeeId: string = '';
  employeeBasicInfo: any;
  empSelectInfo: any;
  leaveTypeSelectListItems: Array<any> = [];

  leaveTypeCode: string = '';
  noOfDays: number = 0;
  fromDate: string = '';
  toDate: string = '';

  requestInfoList: Array<any> = [];
  @Input() data: any;
  editSeq: number = 1;
  isEditing: boolean = false;
  minDate = new Date();
  fromDay!: Date;
  toDay!: Date;
  avalied: number = 0;
  assigned: number = 0;

  formData!: FormData;
  isSaveOrSubmittig: boolean = false;
  isRejectOrApprovig: boolean = false;
  actionType: number = 0;
  fileInfo: any;
  hasDocument: boolean = false;
  isLoading: boolean = false;
  isFromAppoval: boolean = false;
  audits: any;
  canDisable: boolean = false;
  canDisableApproval: boolean = false;
  constructor(private fb: FormBuilder, private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, public dialogRef: MatDialogRef<LeaverequestComponent>,
    private notifyService: NotificationService, private validationService: ValidationService, public dialog: MatDialog) {
    super(authService)
  };


  ngOnInit(): void {
    //this.employeeNumber = this.data.employeeNumber
    this.isFromAppoval = this.data.isFromAppoval;
    console.log(this.data);
    if (this.data.id > 0)
      this.editRequest();
  }
  getSelectedEmpInfo(empIem: any) {
    this.empSelectInfo = empIem;
    this.loadEmpInfo();
  }
  getRemarks(remark: string) {
    this.remarks = remark;
  }
  attachmentEvent(file: any) {
    //console.log(files);
    this.fileInfo = file;
    //console.log(files.files);
    this.hasDocument = file.hasDocument;
    if (file.files)
      this.notifyService.showSuccess('document captured');

  }
  editRequest() {
    this.isLoading = true;
    this.apiService.getall(`serviceRequest/getMyServiceRequestById/${this.data.id}?isFromApproval=${this.isFromAppoval}`).subscribe(res => {
      this.empSelectInfo = res['employeeInfo'];
      this.remarks = res['remarks'];
      this.actionType = res['actionType'] as number;
      this.canDisable = this.actionType == 2 || this.actionType == 4;
      this.canDisableApproval = res['isApproved'] as boolean;
      this.audits = res['audits'];

      let requestInfoItems = res['list'] as Array<any>;
      requestInfoItems.forEach(item => {
        item.editSeq = this.editSeq;
        this.requestInfoList.push(item);
        this.editSeq++;
      });

      this.fileInfo = {
        documentName: res['documentName'],
        documentType: res['documentType'],
        fileName: res['fileName']
      };
      this.hasDocument = true;
      this.loadEmpInfo();
      this.isLoading = false;
    });
  }

  loadEmpInfo() {
    this.apiService.getQueryString(`leaveType/getLeaveTypeSelectListItem`, `?employeeId=${this.empSelectInfo.intValue}&requestType=leaveRequest`).subscribe(res => {
      this.leaveTypeSelectListItems = res;
    });

    this.apiService.getQueryString(`PersonalInformation/GetEmployeePersonalInformationById?employeeNumber=`, this.empSelectInfo.value).subscribe(res => {
      if (res) {
        res.allowImageUpload = true;
        this.employeeBasicInfo = res;
      }
    })
  }

  fromDateEvent(evt: any) {
    //console.log('F', evt.target.value);
    this.fromDay = this.utilService.getDate(evt.target.value);
    this.fromDate = evt.target.value;
    if (this.toDay)
      this.setNoOfDays();
  }

  toDateEvent(evt: any) {
    //console.log('T', evt.target.value);
    this.toDate = evt.target.value;
    this.toDay = this.utilService.getDate(evt.target.value);
    if (this.fromDay)
      this.setNoOfDays();
  }
  setNoOfDays() {
    this.noOfDays = this.utilService.totalDaysFromTwoDates(this.toDay, this.fromDay);
  }
  selectLeaveType(evt: any) {
    let leaveObj = this.leaveTypeSelectListItems.find(e => e.value == evt.target.value);
    if (leaveObj) {
      this.leaveTypeCode = evt.target.value;
      this.avalied = leaveObj.availed;
      this.assigned = leaveObj.assigned;
    }
    else {
      this.avalied = 0;
      this.assigned = 0;
      this.leaveTypeCode = '';
    }

  }

  checkDatesSeqMissing(toDateField: Date) {

    //myDate.setDate(myDate.getDate() + parseInt(days));

    this.minDate = new Date(toDateField.setDate(toDateField.getDate() + 1));
    //this.minDate = toDateField.setDate(toDateField.getDate() + 1);
    //if (this.requestInfoList.length > 0) {
    //  this.requestInfoList.find(item => {
    //    console.log(this.utilService.selectedDate(item.toDate), this.utilService.selectedDate(fromDate), item.toDate.getDate() == fromDate.getDate());

    //  })
    //}
    // return true;
  }

  addItem() {
    // console.log(requestInfoItem);
    //this.requestInfo = requestInfoItem;
    let noOfDaysCount = this.noOfDays;
    if (this.leaveTypeCode && this.fromDate && this.toDate) {
      if (noOfDaysCount < 1) {
        this.notifyService.showError('noOfDays should not zero or negative');
        return;
      }
      const remaining = (this.assigned - this.avalied);
      if (noOfDaysCount > remaining) {
        this.notifyService.showError('noOfDays should not more than ' + remaining);
        return;
      }



      if (this.isEditing) {
        const index = this.requestInfoList.findIndex(e => e.editSeq == this.editSeq);
        let editIem = this.requestInfoList[index];

        editIem.leaveTypeCode = this.leaveTypeCode;
        editIem.fromDate = this.fromDate;
        editIem.toDate = this.toDate;
        editIem.noOfDays = this.noOfDays;
      }
      else {

        if (this.requestInfoList.length > 0) {
          const alrLeaveType = this.requestInfoList.find(e => e.leaveTypeCode == this.leaveTypeCode);
          if (alrLeaveType) {
            this.notifyService.showError('duplicate leave ( ' + this.leaveTypeCode + ' ) edit it');
            return;
          }
        }

        this.checkDatesSeqMissing(new Date(this.toDate));
        this.editSeq = this.editSeq;
        this.requestInfoList.push({ leaveTypeCode: this.leaveTypeCode, editSeq: this.editSeq, noOfDays: this.noOfDays, fromDate: this.fromDate, toDate: this.toDate });
        this.editSeq++;

      }

      this.reset();
    }
    else
      this.utilService.FillUpFields();
  }
  reset() {

    this.leaveTypeCode = ''
    //this.editSeq = 0;
    this.noOfDays = 0;
    this.fromDate = '';
    this.toDate = '';

    this.assigned = 0
    this.avalied = 0

    this.isEditing = false;
  }

  edit(item: any) {
    this.isEditing = true;

    this.leaveTypeCode = item.leaveTypeCode;
    this.editSeq = item.editSeq;
    this.noOfDays = item.noOfDays;
    this.fromDate = item.fromDate;
    this.toDate = item.toDate;

    this.fromDay = this.utilService.getDate(item.fromDate);
    this.toDay = this.utilService.getDate(item.toDate);

    let leaveObj = this.leaveTypeSelectListItems.find(e => e.value == item.leaveTypeCode);
    this.avalied = leaveObj.availed;
    this.assigned = leaveObj.assigned;


  }

  save() {
    this.actionType = 1; //C# ProcessStage enum
    this.saveOrSubmit();
  }
  submit() {
    this.actionType = 2; //C# ProcessStage enum 
    this.saveOrSubmit();

  }
  saveOrSubmit() {
    // console.log(this.fileInfo, this.fileInfo.files);
    if (this.requestInfoList && this.requestInfoList.length > 0 && this.empSelectInfo) {
      if (this.hasDocument) {//this.fileInfo && this.fileInfo.files) {
        this.requestInfoList.forEach(item => {
          item.fromDate = this.utilService.selectedDateTime(item.fromDate);
          item.toDate = this.utilService.selectedDateTime(item.toDate);
        })
        this.isSaveOrSubmittig = true;
        this.formData = new FormData();
        this.formData.append("input", JSON.stringify({ id: (this.data.id > 0) ? this.data.id : 0, actionType: this.actionType, serviceRequestTypeCode: this.data.serviceRequestTypeCode, documentName: this.fileInfo.DocumentName, documentType: this.fileInfo.DocumentType, list: this.requestInfoList, employeeInfo: this.empSelectInfo, remarks: this.remarks }));

        if (this.fileInfo.files) {
          const fileUpload = <File>this.fileInfo.files;
          this.formData.append("file", fileUpload, fileUpload.name);
        }

        this.apiService.post("serviceRequest", this.formData)
          .subscribe(res => {
            this.isSaveOrSubmittig = false;
            this.notifyService.showSuccess('saved successfully');
            this.dialogRef.close(true);
          }, error => {
            this.isSaveOrSubmittig = false;
            this.notifyService.showError(error.error);
          });
      }
      else
        this.notifyService.showError('document is empty');
    }
    else
      this.utilService.FillUpFields();
  }

  reject() {
    this.actionType = 3; //C# ProcessStage  enum
    this.rejectOrAppove();
  }
  appove() {
    this.actionType = 4; //C# ProcessStage  enum
    this.rejectOrAppove();
  }

  rejectOrAppove() {
    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canTakeAction => {
      if (canTakeAction) {
        this.isRejectOrApprovig = true;
        this.apiService.post('serviceRequest/rejectApproveVacationRequest', { id: (this.data.id > 0) ? this.data.id : 0, actionType: this.actionType, remarks: this.remarks }).subscribe(res => {
          this.isRejectOrApprovig = false;
          this.utilService.OkMessage();
          this.dialogRef.close(true);
        },
          error => {
            this.utilService.ShowApiErrorMessage(error);
          });
      }
    });
  }

  closeModel() {
    this.dialogRef.close();
  }
}
