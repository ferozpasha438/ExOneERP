import { Directive } from '@angular/core';
import { DropdownDirective } from './dropdown.directive';
export class DropdownMenuDirective {
    constructor(dropdown) {
        this.dropdown = dropdown;
    }
}
DropdownMenuDirective.decorators = [
    { type: Directive, args: [{
                selector: '[ngxDropdownMenu]',
                host: {
                    '[class.dropdown-menu]': 'true',
                    '[class.show]': 'dropdown.isOpen'
                }
            },] }
];
DropdownMenuDirective.ctorParameters = () => [
    { type: DropdownDirective }
];
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZHJvcGRvd24tbWVudS5kaXJlY3RpdmUuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyIuLi8uLi8uLi8uLi8uLi9wcm9qZWN0cy9uZ3gtdHJlZXZpZXcvc3JjL2xpYi9kaXJlY3RpdmVzL2Ryb3Bkb3duLW1lbnUuZGlyZWN0aXZlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQWUsTUFBTSxlQUFlLENBQUM7QUFDdkQsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFTekQsTUFBTSxPQUFPLHFCQUFxQjtJQUNoQyxZQUNTLFFBQTJCO1FBQTNCLGFBQVEsR0FBUixRQUFRLENBQW1CO0lBQ2hDLENBQUM7OztZQVZOLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsbUJBQW1CO2dCQUM3QixJQUFJLEVBQUU7b0JBQ0osdUJBQXVCLEVBQUUsTUFBTTtvQkFDL0IsY0FBYyxFQUFFLGlCQUFpQjtpQkFDbEM7YUFDRjs7O1lBUlEsaUJBQWlCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgRGlyZWN0aXZlLCBIb3N0QmluZGluZyB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBEcm9wZG93bkRpcmVjdGl2ZSB9IGZyb20gJy4vZHJvcGRvd24uZGlyZWN0aXZlJztcclxuXHJcbkBEaXJlY3RpdmUoe1xyXG4gIHNlbGVjdG9yOiAnW25neERyb3Bkb3duTWVudV0nLFxyXG4gIGhvc3Q6IHtcclxuICAgICdbY2xhc3MuZHJvcGRvd24tbWVudV0nOiAndHJ1ZScsXHJcbiAgICAnW2NsYXNzLnNob3ddJzogJ2Ryb3Bkb3duLmlzT3BlbidcclxuICB9XHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBEcm9wZG93bk1lbnVEaXJlY3RpdmUge1xyXG4gIGNvbnN0cnVjdG9yKFxyXG4gICAgcHVibGljIGRyb3Bkb3duOiBEcm9wZG93bkRpcmVjdGl2ZVxyXG4gICkgeyB9XHJcbn1cclxuIl19