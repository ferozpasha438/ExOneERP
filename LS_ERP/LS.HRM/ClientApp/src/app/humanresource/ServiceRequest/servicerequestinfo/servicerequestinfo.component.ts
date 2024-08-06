import { ComponentFactoryResolver, Type, ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { AdItem, ComponentLoaderData, ComponentloaderDirective } from '../../../sharedcomponent/componentloader.directive';
import { VacationrequestComponent } from '../vacationrequest/vacationrequest.component';
import { UtilityService } from '../../../services/utility.service';
import { TranslateService } from '@ngx-translate/core';
import { MyrequestComponent } from '../myrequest/myrequest.component';
import { LeaverequestattachmentComponent } from '../shared/leaverequestattachment/leaverequestattachment.component';
import { LeaverequestapprovalComponent } from '../shared/leaverequestapproval/leaverequestapproval.component';

@Component({
  selector: 'app-servicerequestinfo',
  templateUrl: './servicerequestinfo.component.html',
  styles: [
  ]
})
export class ServicerequestinfoComponent implements OnInit {
  id: number = 0;
  modalTitle: string = '';
  serviceRequestRefNo: string = '';
  isFromAppoval: boolean = false;
  modalBtnTitle: string = '';
  // employeeNumber: string = '00004';
  employeeId: number = 4;
  btnClicked: string = '';
  btnList: Array<any> = [];
  isArab: boolean = false;

  @ViewChild(ComponentloaderDirective, { static: true }) appComponentloader!: ComponentloaderDirective;

  poList: AdItem[] = [];

  constructor(private utilService: UtilityService, private translate: TranslateService, private componentFactoryResolver: ComponentFactoryResolver, public dialogRef: MatDialogRef<ServicerequestinfoComponent>

  ) {
  }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    this.fillBtnList();
    this.fillComponents();
    this.initialLoading(1);
  }
  fillBtnList() {
    this.btnList = [
      { id: 1, text: this.translate.instant('Vaction_request'), isBtnClicked: true, serviceRequestTypeCode: 'VAC' },
      { id: 2, text: this.translate.instant('Ticket_request'), isBtnClicked: false, serviceRequestTypeCode: 'TAC' },
      { id: 3, text: this.translate.instant('Travel_request'), isBtnClicked: false, serviceRequestTypeCode: 'TAC' },
      { id: 4, text: this.translate.instant('Loan_request'), isBtnClicked: false, serviceRequestTypeCode: 'LAC' },
    ]
  }

  fillComponents() {
    const data = { serviceRequestTypeCode: '', id: this.id, serviceRequestRefNo: this.serviceRequestRefNo, isFromAppoval: this.isFromAppoval }
    this.poList = [
      new AdItem(VacationrequestComponent, data),
      new AdItem(MyrequestComponent, data),
      new AdItem(LeaverequestattachmentComponent, data),
      new AdItem(LeaverequestapprovalComponent, data),

    ];
  }

  initialLoading(index: number) {
    const viewContainerRef = this.appComponentloader.viewContainerRef;
    viewContainerRef.clear();
    const componentItem = this.poList[index - 1];
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory<ComponentLoaderData>(
      componentItem.component
    );
    const componentRef = viewContainerRef.createComponent<ComponentLoaderData>(componentFactory);
    componentItem.data.serviceRequestTypeCode = this.btnList.find(e => e.id == index).serviceRequestTypeCode
    componentRef.instance.data = componentItem.data;

    //    componentRef.instance.data = this.id;
  }

  loadComponents(item: any) {
    this.initialLoading(item.id);
    this.setBtnStyles(item);
  }
  setBtnStyles(item: any) {
    this.btnList.forEach(item => item.isBtnClicked = false);
    item.isBtnClicked = true;
  }

  printInvoice() {
    const printContent = document.getElementById("printcontainer") as HTMLElement;
    const WindowPrt: any = window.open('', '', 'left=0,top=0,width=2000,height=1000,toolbar=0,scrollbars=0,status=0');
    setTimeout(() => {
      WindowPrt.document.write(printContent.innerHTML);
      WindowPrt.document.close();
      WindowPrt.focus();
      WindowPrt.print();
      WindowPrt.close();
    }, 50);
  }

  closeModel() {
    this.dialogRef.close();
  }


}
