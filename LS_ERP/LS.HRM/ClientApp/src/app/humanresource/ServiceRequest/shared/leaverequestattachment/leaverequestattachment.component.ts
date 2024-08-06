import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CustomSelectListItem } from '../../../../models/MenuItemListDto';
import { ApiService } from '../../../../services/api.service';
import { AuthorizeService } from '../../../../api-authorization/AuthorizeService';
import { UtilityService } from '../../../../services/utility.service';
import { NotificationService } from '../../../../services/notification.service';

@Component({
  selector: 'app-leaverequestattachment',
  templateUrl: './leaverequestattachment.component.html',
  styles: [
  ]
})
export class LeaverequestattachmentComponent implements OnInit {
  uploadedFiles: Array<any> = [];
  fileUploads: Array<File> = [];
  DocumentName: string = '';
  DocumentType: string = '';
  FileName: string = '';
  isCaptured: boolean = false;
  DocumentTypes: Array<CustomSelectListItem> = [];
  hasDocument: boolean = false;
  @Input() document: any;
  @Output() attachmentEvent = new EventEmitter<any>();
  constructor(private apiService: ApiService, private authService: AuthorizeService, private notifyService: NotificationService) {
  };

  ngOnInit(): void {
    this.isCaptured = false;
    this.apiService.getall(`documentType/GetDocumentTypeSelectListItem`).subscribe(res => {
      this.DocumentTypes = res;
    });
    if (this.document) {
      this.hasDocument = true;
      this.DocumentName = this.document.documentName;
      this.DocumentType = this.document.documentType;
      this.FileName = `${this.authService.ApiEndPoint().replace('api', '')}${this.document.fileName}`;
      this.isCaptured = true;
    }
  }

  onSelectFile(fileInput: any) {
    this.fileUploads = fileInput.target.files;
    this.isCaptured = false;
    this.hasDocument = true;
    this.FileName = this.fileUploads[0].name;
  }
  save() {
    if (this.DocumentName && this.DocumentType && this.fileUploads.length > 0) {
      this.isCaptured = true;
      this.attachmentEvent.emit({ hasDocument: this.hasDocument, DocumentName: this.DocumentName, DocumentType: this.DocumentType, files: this.fileUploads[0] });
    }
    else {
      this.notifyService.showError('document is empty');
    }
  }
  delete() {
    this.DocumentName = '';
    this.DocumentType = '';
    this.FileName = '';
    this.fileUploads = [];
    this.hasDocument = false;
    (document.getElementById('document_file') as any).value = null;
    this.isCaptured = false;
    this.attachmentEvent.emit({ hasDocument: this.hasDocument, DocumentName: this.DocumentName, DocumentType: this.DocumentType, files: this.fileUploads[0] });
  }
}
