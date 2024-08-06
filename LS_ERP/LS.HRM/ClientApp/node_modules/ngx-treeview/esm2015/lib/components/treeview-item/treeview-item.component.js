import { Component, EventEmitter, Input, Output } from '@angular/core';
import { isNil } from 'lodash';
import { TreeviewConfig } from '../../models/treeview-config';
export class TreeviewItemComponent {
    constructor(defaultConfig) {
        this.defaultConfig = defaultConfig;
        this.checkedChange = new EventEmitter();
        this.onCollapseExpand = () => {
            this.item.collapsed = !this.item.collapsed;
        };
        this.onCheckedChange = () => {
            const checked = this.item.checked;
            if (!isNil(this.item.children) && !this.config.decoupleChildFromParent) {
                this.item.children.forEach(child => child.setCheckedRecursive(checked));
            }
            this.checkedChange.emit(checked);
        };
        this.config = this.defaultConfig;
    }
    onChildCheckedChange(child, checked) {
        if (!this.config.decoupleChildFromParent) {
            let itemChecked = null;
            for (const childItem of this.item.children) {
                if (itemChecked === null) {
                    itemChecked = childItem.checked;
                }
                else if (itemChecked !== childItem.checked) {
                    itemChecked = undefined;
                    break;
                }
            }
            if (itemChecked === null) {
                itemChecked = false;
            }
            if (this.item.checked !== itemChecked) {
                this.item.checked = itemChecked;
            }
        }
        this.checkedChange.emit(checked);
    }
}
TreeviewItemComponent.decorators = [
    { type: Component, args: [{
                selector: 'ngx-treeview-item',
                template: "<div *ngIf=\"item\" class=\"treeview-item\">\r\n  <ng-template [ngTemplateOutlet]=\"template\"\r\n    [ngTemplateOutletContext]=\"{item: item, onCollapseExpand: onCollapseExpand, onCheckedChange: onCheckedChange}\">\r\n  </ng-template>\r\n  <div *ngIf=\"!item.collapsed\">\r\n    <ngx-treeview-item [config]=\"config\" *ngFor=\"let child of item.children\" [item]=\"child\" [template]=\"template\"\r\n      (checkedChange)=\"onChildCheckedChange(child, $event)\">\r\n    </ngx-treeview-item>\r\n  </div>\r\n</div>\r\n",
                styles: [":host{display:block}:host .treeview-item{white-space:nowrap}:host .treeview-item .treeview-item{margin-left:2rem}"]
            },] }
];
TreeviewItemComponent.ctorParameters = () => [
    { type: TreeviewConfig }
];
TreeviewItemComponent.propDecorators = {
    config: [{ type: Input }],
    template: [{ type: Input }],
    item: [{ type: Input }],
    checkedChange: [{ type: Output }]
};
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidHJlZXZpZXctaXRlbS5jb21wb25lbnQuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyIuLi8uLi8uLi8uLi8uLi8uLi9wcm9qZWN0cy9uZ3gtdHJlZXZpZXcvc3JjL2xpYi9jb21wb25lbnRzL3RyZWV2aWV3LWl0ZW0vdHJlZXZpZXctaXRlbS5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxZQUFZLEVBQUUsS0FBSyxFQUFFLE1BQU0sRUFBZSxNQUFNLGVBQWUsQ0FBQztBQUNwRixPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sUUFBUSxDQUFDO0FBRS9CLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSw4QkFBOEIsQ0FBQztBQVE5RCxNQUFNLE9BQU8scUJBQXFCO0lBTWhDLFlBQ1UsYUFBNkI7UUFBN0Isa0JBQWEsR0FBYixhQUFhLENBQWdCO1FBSDdCLGtCQUFhLEdBQUcsSUFBSSxZQUFZLEVBQVcsQ0FBQztRQVF0RCxxQkFBZ0IsR0FBRyxHQUFHLEVBQUU7WUFDdEIsSUFBSSxDQUFDLElBQUksQ0FBQyxTQUFTLEdBQUcsQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLFNBQVMsQ0FBQztRQUM3QyxDQUFDLENBQUE7UUFFRCxvQkFBZSxHQUFHLEdBQUcsRUFBRTtZQUNyQixNQUFNLE9BQU8sR0FBRyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBQztZQUNsQyxJQUFJLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLHVCQUF1QixFQUFFO2dCQUN0RSxJQUFJLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxPQUFPLENBQUMsS0FBSyxDQUFDLEVBQUUsQ0FBQyxLQUFLLENBQUMsbUJBQW1CLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQzthQUN6RTtZQUNELElBQUksQ0FBQyxhQUFhLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBQyxDQUFDO1FBQ25DLENBQUMsQ0FBQTtRQWJDLElBQUksQ0FBQyxNQUFNLEdBQUcsSUFBSSxDQUFDLGFBQWEsQ0FBQztJQUNuQyxDQUFDO0lBY0Qsb0JBQW9CLENBQUMsS0FBbUIsRUFBRSxPQUFnQjtRQUN4RCxJQUFJLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyx1QkFBdUIsRUFBRTtZQUN4QyxJQUFJLFdBQVcsR0FBWSxJQUFJLENBQUM7WUFDaEMsS0FBSyxNQUFNLFNBQVMsSUFBSSxJQUFJLENBQUMsSUFBSSxDQUFDLFFBQVEsRUFBRTtnQkFDMUMsSUFBSSxXQUFXLEtBQUssSUFBSSxFQUFFO29CQUN4QixXQUFXLEdBQUcsU0FBUyxDQUFDLE9BQU8sQ0FBQztpQkFDakM7cUJBQU0sSUFBSSxXQUFXLEtBQUssU0FBUyxDQUFDLE9BQU8sRUFBRTtvQkFDNUMsV0FBVyxHQUFHLFNBQVMsQ0FBQztvQkFDeEIsTUFBTTtpQkFDUDthQUNGO1lBRUQsSUFBSSxXQUFXLEtBQUssSUFBSSxFQUFFO2dCQUN4QixXQUFXLEdBQUcsS0FBSyxDQUFDO2FBQ3JCO1lBRUQsSUFBSSxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sS0FBSyxXQUFXLEVBQUU7Z0JBQ3JDLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTyxHQUFHLFdBQVcsQ0FBQzthQUNqQztTQUVGO1FBRUQsSUFBSSxDQUFDLGFBQWEsQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFDLENBQUM7SUFDbkMsQ0FBQzs7O1lBcERGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsbUJBQW1CO2dCQUM3QixpaEJBQTZDOzthQUU5Qzs7O1lBUFEsY0FBYzs7O3FCQVNwQixLQUFLO3VCQUNMLEtBQUs7bUJBQ0wsS0FBSzs0QkFDTCxNQUFNIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBFdmVudEVtaXR0ZXIsIElucHV0LCBPdXRwdXQsIFRlbXBsYXRlUmVmIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IGlzTmlsIH0gZnJvbSAnbG9kYXNoJztcclxuaW1wb3J0IHsgVHJlZXZpZXdJdGVtIH0gZnJvbSAnLi4vLi4vbW9kZWxzL3RyZWV2aWV3LWl0ZW0nO1xyXG5pbXBvcnQgeyBUcmVldmlld0NvbmZpZyB9IGZyb20gJy4uLy4uL21vZGVscy90cmVldmlldy1jb25maWcnO1xyXG5pbXBvcnQgeyBUcmVldmlld0l0ZW1UZW1wbGF0ZUNvbnRleHQgfSBmcm9tICcuLi8uLi9tb2RlbHMvdHJlZXZpZXctaXRlbS10ZW1wbGF0ZS1jb250ZXh0JztcclxuXHJcbkBDb21wb25lbnQoe1xyXG4gIHNlbGVjdG9yOiAnbmd4LXRyZWV2aWV3LWl0ZW0nLFxyXG4gIHRlbXBsYXRlVXJsOiAnLi90cmVldmlldy1pdGVtLmNvbXBvbmVudC5odG1sJyxcclxuICBzdHlsZVVybHM6IFsnLi90cmVldmlldy1pdGVtLmNvbXBvbmVudC5zY3NzJ11cclxufSlcclxuZXhwb3J0IGNsYXNzIFRyZWV2aWV3SXRlbUNvbXBvbmVudCB7XHJcbiAgQElucHV0KCkgY29uZmlnOiBUcmVldmlld0NvbmZpZztcclxuICBASW5wdXQoKSB0ZW1wbGF0ZTogVGVtcGxhdGVSZWY8VHJlZXZpZXdJdGVtVGVtcGxhdGVDb250ZXh0PjtcclxuICBASW5wdXQoKSBpdGVtOiBUcmVldmlld0l0ZW07XHJcbiAgQE91dHB1dCgpIGNoZWNrZWRDaGFuZ2UgPSBuZXcgRXZlbnRFbWl0dGVyPGJvb2xlYW4+KCk7XHJcblxyXG4gIGNvbnN0cnVjdG9yKFxyXG4gICAgcHJpdmF0ZSBkZWZhdWx0Q29uZmlnOiBUcmVldmlld0NvbmZpZ1xyXG4gICkge1xyXG4gICAgdGhpcy5jb25maWcgPSB0aGlzLmRlZmF1bHRDb25maWc7XHJcbiAgfVxyXG5cclxuICBvbkNvbGxhcHNlRXhwYW5kID0gKCkgPT4ge1xyXG4gICAgdGhpcy5pdGVtLmNvbGxhcHNlZCA9ICF0aGlzLml0ZW0uY29sbGFwc2VkO1xyXG4gIH1cclxuXHJcbiAgb25DaGVja2VkQ2hhbmdlID0gKCkgPT4ge1xyXG4gICAgY29uc3QgY2hlY2tlZCA9IHRoaXMuaXRlbS5jaGVja2VkO1xyXG4gICAgaWYgKCFpc05pbCh0aGlzLml0ZW0uY2hpbGRyZW4pICYmICF0aGlzLmNvbmZpZy5kZWNvdXBsZUNoaWxkRnJvbVBhcmVudCkge1xyXG4gICAgICB0aGlzLml0ZW0uY2hpbGRyZW4uZm9yRWFjaChjaGlsZCA9PiBjaGlsZC5zZXRDaGVja2VkUmVjdXJzaXZlKGNoZWNrZWQpKTtcclxuICAgIH1cclxuICAgIHRoaXMuY2hlY2tlZENoYW5nZS5lbWl0KGNoZWNrZWQpO1xyXG4gIH1cclxuXHJcbiAgb25DaGlsZENoZWNrZWRDaGFuZ2UoY2hpbGQ6IFRyZWV2aWV3SXRlbSwgY2hlY2tlZDogYm9vbGVhbik6IHZvaWQge1xyXG4gICAgaWYgKCF0aGlzLmNvbmZpZy5kZWNvdXBsZUNoaWxkRnJvbVBhcmVudCkge1xyXG4gICAgICBsZXQgaXRlbUNoZWNrZWQ6IGJvb2xlYW4gPSBudWxsO1xyXG4gICAgICBmb3IgKGNvbnN0IGNoaWxkSXRlbSBvZiB0aGlzLml0ZW0uY2hpbGRyZW4pIHtcclxuICAgICAgICBpZiAoaXRlbUNoZWNrZWQgPT09IG51bGwpIHtcclxuICAgICAgICAgIGl0ZW1DaGVja2VkID0gY2hpbGRJdGVtLmNoZWNrZWQ7XHJcbiAgICAgICAgfSBlbHNlIGlmIChpdGVtQ2hlY2tlZCAhPT0gY2hpbGRJdGVtLmNoZWNrZWQpIHtcclxuICAgICAgICAgIGl0ZW1DaGVja2VkID0gdW5kZWZpbmVkO1xyXG4gICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgfVxyXG4gICAgICB9XHJcblxyXG4gICAgICBpZiAoaXRlbUNoZWNrZWQgPT09IG51bGwpIHtcclxuICAgICAgICBpdGVtQ2hlY2tlZCA9IGZhbHNlO1xyXG4gICAgICB9XHJcblxyXG4gICAgICBpZiAodGhpcy5pdGVtLmNoZWNrZWQgIT09IGl0ZW1DaGVja2VkKSB7XHJcbiAgICAgICAgdGhpcy5pdGVtLmNoZWNrZWQgPSBpdGVtQ2hlY2tlZDtcclxuICAgICAgfVxyXG5cclxuICAgIH1cclxuXHJcbiAgICB0aGlzLmNoZWNrZWRDaGFuZ2UuZW1pdChjaGVja2VkKTtcclxuICB9XHJcbn1cclxuIl19