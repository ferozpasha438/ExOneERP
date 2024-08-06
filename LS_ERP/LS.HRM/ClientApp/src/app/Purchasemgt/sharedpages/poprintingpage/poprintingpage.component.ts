import { ComponentFactoryResolver, Type, ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { AdItem, ComponentLoaderData, ComponentloaderDirective } from '../../../sharedcomponent/componentloader.directive';
import { PoprintingformatfourpageComponent } from '../poprintingformatfourpage/poprintingformatfourpage.component';
import { PoprintingformatonepageComponent } from '../poprintingformatonepage/poprintingformatonepage.component';
import { PoprintingformatthreepageComponent } from '../poprintingformatthreepage/poprintingformatthreepage.component';
import { PoprintingformattwopageComponent } from '../poprintingformattwopage/poprintingformattwopage.component';


@Component({
  selector: 'app-poprintingpage',
  templateUrl: './poprintingpage.component.html',
  styles: [
  ]
})
export class PoprintingpageComponent implements OnInit {
  id: number = 0;
  modalTitle: string = '';
  modalBtnTitle: string = '';
  @ViewChild(ComponentloaderDirective, { static: true }) appComponentloader!: ComponentloaderDirective;

  poList: AdItem[] = [];

  constructor(private componentFactoryResolver: ComponentFactoryResolver, public dialogRef: MatDialogRef<PoprintingpageComponent>

  ) {
  }

  ngOnInit(): void {
    this.fillComponents();
    this.initialLoading(1);
  }

  fillComponents() {
    const data = { from: this.modalBtnTitle, id: this.id }
    this.poList = [
      new AdItem(PoprintingformatonepageComponent, data),
      new AdItem(PoprintingformattwopageComponent, data),
      new AdItem(PoprintingformatthreepageComponent, data),
      //new AdItem(PoprintingformatfourpageComponent, data),
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
    componentRef.instance.data = componentItem.data;
//    componentRef.instance.data = this.id;
  }

  loadComponents(event: any) {
    this.initialLoading(event.target.value as number);

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
