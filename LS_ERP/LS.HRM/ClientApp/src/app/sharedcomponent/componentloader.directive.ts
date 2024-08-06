import { Type, Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[loadingComponent]'
})
export class ComponentloaderDirective {

  constructor(public viewContainerRef: ViewContainerRef) { }

}

export interface ComponentLoaderData {
  data: any;
}

export class AdItem {
  constructor(public component: Type<any>, public data: any) { }
}
