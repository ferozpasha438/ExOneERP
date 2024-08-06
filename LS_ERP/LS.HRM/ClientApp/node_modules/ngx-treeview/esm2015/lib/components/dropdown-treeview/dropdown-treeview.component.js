import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { TreeviewI18n } from '../../models/treeview-i18n';
import { TreeviewConfig } from '../../models/treeview-config';
import { TreeviewComponent } from '../treeview/treeview.component';
export class DropdownTreeviewComponent {
    constructor(i18n, defaultConfig) {
        this.i18n = i18n;
        this.defaultConfig = defaultConfig;
        this.buttonClass = 'btn-outline-secondary';
        this.selectedChange = new EventEmitter(true);
        this.filterChange = new EventEmitter();
        this.config = this.defaultConfig;
    }
    onSelectedChange(values) {
        this.buttonLabel = this.i18n.getText(this.treeviewComponent.selection);
        this.selectedChange.emit(values);
    }
    onFilterChange(text) {
        this.filterChange.emit(text);
    }
}
DropdownTreeviewComponent.decorators = [
    { type: Component, args: [{
                selector: 'ngx-dropdown-treeview',
                template: "<div class=\"dropdown\" ngxDropdown>\r\n  <button class=\"btn\" [ngClass]=\"buttonClass\" type=\"button\" role=\"button\" ngxDropdownToggle>\r\n    {{buttonLabel}}\r\n  </button>\r\n  <div ngxDropdownMenu aria-labelledby=\"dropdownMenu\" (click)=\"$event.stopPropagation()\">\r\n    <div class=\"dropdown-container\">\r\n      <ngx-treeview [config]=\"config\" [headerTemplate]=\"headerTemplate\" [items]=\"items\" [itemTemplate]=\"itemTemplate\"\r\n        (selectedChange)=\"onSelectedChange($event)\" (filterChange)=\"onFilterChange($event)\">\r\n      </ngx-treeview>\r\n    </div>\r\n  </div>\r\n</div>\r\n",
                styles: [".dropdown{display:inline-block;width:100%}.dropdown button{margin-right:.9rem;overflow:hidden;padding-right:30px;text-align:left;text-overflow:ellipsis;width:100%}.dropdown button:after{margin-top:.6rem;position:absolute;right:.6rem}.dropdown .dropdown-menu .dropdown-container{padding:0 .6rem}"]
            },] }
];
DropdownTreeviewComponent.ctorParameters = () => [
    { type: TreeviewI18n },
    { type: TreeviewConfig }
];
DropdownTreeviewComponent.propDecorators = {
    buttonClass: [{ type: Input }],
    headerTemplate: [{ type: Input }],
    itemTemplate: [{ type: Input }],
    items: [{ type: Input }],
    config: [{ type: Input }],
    selectedChange: [{ type: Output }],
    filterChange: [{ type: Output }],
    treeviewComponent: [{ type: ViewChild, args: [TreeviewComponent, { static: false },] }]
};
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZHJvcGRvd24tdHJlZXZpZXcuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiLi4vLi4vLi4vLi4vLi4vLi4vcHJvamVjdHMvbmd4LXRyZWV2aWV3L3NyYy9saWIvY29tcG9uZW50cy9kcm9wZG93bi10cmVldmlldy9kcm9wZG93bi10cmVldmlldy5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxZQUFZLEVBQUUsS0FBSyxFQUFFLE1BQU0sRUFBRSxTQUFTLEVBQWUsTUFBTSxlQUFlLENBQUM7QUFDL0YsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLDRCQUE0QixDQUFDO0FBRTFELE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSw4QkFBOEIsQ0FBQztBQUM5RCxPQUFPLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSxnQ0FBZ0MsQ0FBQztBQVNuRSxNQUFNLE9BQU8seUJBQXlCO0lBV3BDLFlBQ1MsSUFBa0IsRUFDakIsYUFBNkI7UUFEOUIsU0FBSSxHQUFKLElBQUksQ0FBYztRQUNqQixrQkFBYSxHQUFiLGFBQWEsQ0FBZ0I7UUFaOUIsZ0JBQVcsR0FBRyx1QkFBdUIsQ0FBQztRQUtyQyxtQkFBYyxHQUFHLElBQUksWUFBWSxDQUFRLElBQUksQ0FBQyxDQUFDO1FBQy9DLGlCQUFZLEdBQUcsSUFBSSxZQUFZLEVBQVUsQ0FBQztRQVFsRCxJQUFJLENBQUMsTUFBTSxHQUFHLElBQUksQ0FBQyxhQUFhLENBQUM7SUFDbkMsQ0FBQztJQUVELGdCQUFnQixDQUFDLE1BQWE7UUFDNUIsSUFBSSxDQUFDLFdBQVcsR0FBRyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBQyxJQUFJLENBQUMsaUJBQWlCLENBQUMsU0FBUyxDQUFDLENBQUM7UUFDdkUsSUFBSSxDQUFDLGNBQWMsQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLENBQUM7SUFDbkMsQ0FBQztJQUVELGNBQWMsQ0FBQyxJQUFZO1FBQ3pCLElBQUksQ0FBQyxZQUFZLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxDQUFDO0lBQy9CLENBQUM7OztZQTlCRixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLHVCQUF1QjtnQkFDakMsK21CQUFpRDs7YUFFbEQ7OztZQVhRLFlBQVk7WUFFWixjQUFjOzs7MEJBV3BCLEtBQUs7NkJBQ0wsS0FBSzsyQkFDTCxLQUFLO29CQUNMLEtBQUs7cUJBQ0wsS0FBSzs2QkFDTCxNQUFNOzJCQUNOLE1BQU07Z0NBQ04sU0FBUyxTQUFDLGlCQUFpQixFQUFFLEVBQUUsTUFBTSxFQUFFLEtBQUssRUFBRSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgRXZlbnRFbWl0dGVyLCBJbnB1dCwgT3V0cHV0LCBWaWV3Q2hpbGQsIFRlbXBsYXRlUmVmIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFRyZWV2aWV3STE4biB9IGZyb20gJy4uLy4uL21vZGVscy90cmVldmlldy1pMThuJztcclxuaW1wb3J0IHsgVHJlZXZpZXdJdGVtIH0gZnJvbSAnLi4vLi4vbW9kZWxzL3RyZWV2aWV3LWl0ZW0nO1xyXG5pbXBvcnQgeyBUcmVldmlld0NvbmZpZyB9IGZyb20gJy4uLy4uL21vZGVscy90cmVldmlldy1jb25maWcnO1xyXG5pbXBvcnQgeyBUcmVldmlld0NvbXBvbmVudCB9IGZyb20gJy4uL3RyZWV2aWV3L3RyZWV2aWV3LmNvbXBvbmVudCc7XHJcbmltcG9ydCB7IFRyZWV2aWV3SGVhZGVyVGVtcGxhdGVDb250ZXh0IH0gZnJvbSAnLi4vLi4vbW9kZWxzL3RyZWV2aWV3LWhlYWRlci10ZW1wbGF0ZS1jb250ZXh0JztcclxuaW1wb3J0IHsgVHJlZXZpZXdJdGVtVGVtcGxhdGVDb250ZXh0IH0gZnJvbSAnLi4vLi4vbW9kZWxzL3RyZWV2aWV3LWl0ZW0tdGVtcGxhdGUtY29udGV4dCc7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ25neC1kcm9wZG93bi10cmVldmlldycsXHJcbiAgdGVtcGxhdGVVcmw6ICcuL2Ryb3Bkb3duLXRyZWV2aWV3LmNvbXBvbmVudC5odG1sJyxcclxuICBzdHlsZVVybHM6IFsnLi9kcm9wZG93bi10cmVldmlldy5jb21wb25lbnQuc2NzcyddXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBEcm9wZG93blRyZWV2aWV3Q29tcG9uZW50IHtcclxuICBASW5wdXQoKSBidXR0b25DbGFzcyA9ICdidG4tb3V0bGluZS1zZWNvbmRhcnknO1xyXG4gIEBJbnB1dCgpIGhlYWRlclRlbXBsYXRlOiBUZW1wbGF0ZVJlZjxUcmVldmlld0hlYWRlclRlbXBsYXRlQ29udGV4dD47XHJcbiAgQElucHV0KCkgaXRlbVRlbXBsYXRlOiBUZW1wbGF0ZVJlZjxUcmVldmlld0l0ZW1UZW1wbGF0ZUNvbnRleHQ+O1xyXG4gIEBJbnB1dCgpIGl0ZW1zOiBUcmVldmlld0l0ZW1bXTtcclxuICBASW5wdXQoKSBjb25maWc6IFRyZWV2aWV3Q29uZmlnO1xyXG4gIEBPdXRwdXQoKSBzZWxlY3RlZENoYW5nZSA9IG5ldyBFdmVudEVtaXR0ZXI8YW55W10+KHRydWUpO1xyXG4gIEBPdXRwdXQoKSBmaWx0ZXJDaGFuZ2UgPSBuZXcgRXZlbnRFbWl0dGVyPHN0cmluZz4oKTtcclxuICBAVmlld0NoaWxkKFRyZWV2aWV3Q29tcG9uZW50LCB7IHN0YXRpYzogZmFsc2UgfSkgdHJlZXZpZXdDb21wb25lbnQ6IFRyZWV2aWV3Q29tcG9uZW50O1xyXG4gIGJ1dHRvbkxhYmVsOiBzdHJpbmc7XHJcblxyXG4gIGNvbnN0cnVjdG9yKFxyXG4gICAgcHVibGljIGkxOG46IFRyZWV2aWV3STE4bixcclxuICAgIHByaXZhdGUgZGVmYXVsdENvbmZpZzogVHJlZXZpZXdDb25maWdcclxuICApIHtcclxuICAgIHRoaXMuY29uZmlnID0gdGhpcy5kZWZhdWx0Q29uZmlnO1xyXG4gIH1cclxuXHJcbiAgb25TZWxlY3RlZENoYW5nZSh2YWx1ZXM6IGFueVtdKTogdm9pZCB7XHJcbiAgICB0aGlzLmJ1dHRvbkxhYmVsID0gdGhpcy5pMThuLmdldFRleHQodGhpcy50cmVldmlld0NvbXBvbmVudC5zZWxlY3Rpb24pO1xyXG4gICAgdGhpcy5zZWxlY3RlZENoYW5nZS5lbWl0KHZhbHVlcyk7XHJcbiAgfVxyXG5cclxuICBvbkZpbHRlckNoYW5nZSh0ZXh0OiBzdHJpbmcpOiB2b2lkIHtcclxuICAgIHRoaXMuZmlsdGVyQ2hhbmdlLmVtaXQodGV4dCk7XHJcbiAgfVxyXG59XHJcbiJdfQ==