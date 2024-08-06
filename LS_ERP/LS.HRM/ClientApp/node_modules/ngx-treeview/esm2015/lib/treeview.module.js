import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { DropdownDirective } from './directives/dropdown.directive';
import { DropdownMenuDirective } from './directives/dropdown-menu.directive';
import { DropdownToggleDirective } from './directives/dropdown-toggle.directive';
import { DropdownTreeviewComponent } from './components/dropdown-treeview/dropdown-treeview.component';
import { TreeviewComponent } from './components/treeview/treeview.component';
import { TreeviewItemComponent } from './components/treeview-item/treeview-item.component';
import { TreeviewPipe } from './pipes/treeview.pipe';
import { TreeviewI18n, DefaultTreeviewI18n } from './models/treeview-i18n';
import { TreeviewConfig } from './models/treeview-config';
import { TreeviewEventParser, DefaultTreeviewEventParser } from './helpers/treeview-event-parser';
export class TreeviewModule {
    static forRoot() {
        return {
            ngModule: TreeviewModule,
            providers: [
                TreeviewConfig,
                { provide: TreeviewI18n, useClass: DefaultTreeviewI18n },
                { provide: TreeviewEventParser, useClass: DefaultTreeviewEventParser }
            ]
        };
    }
}
TreeviewModule.decorators = [
    { type: NgModule, args: [{
                imports: [
                    FormsModule,
                    CommonModule
                ],
                declarations: [
                    TreeviewComponent,
                    TreeviewItemComponent,
                    TreeviewPipe,
                    DropdownDirective,
                    DropdownMenuDirective,
                    DropdownToggleDirective,
                    DropdownTreeviewComponent
                ], exports: [
                    TreeviewComponent,
                    TreeviewPipe,
                    DropdownTreeviewComponent
                ]
            },] }
];
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidHJlZXZpZXcubW9kdWxlLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiLi4vLi4vLi4vLi4vcHJvamVjdHMvbmd4LXRyZWV2aWV3L3NyYy9saWIvdHJlZXZpZXcubW9kdWxlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBLE9BQU8sRUFBRSxRQUFRLEVBQXVCLE1BQU0sZUFBZSxDQUFDO0FBQzlELE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUM3QyxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFDL0MsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0saUNBQWlDLENBQUM7QUFDcEUsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sc0NBQXNDLENBQUM7QUFDN0UsT0FBTyxFQUFFLHVCQUF1QixFQUFFLE1BQU0sd0NBQXdDLENBQUM7QUFDakYsT0FBTyxFQUFFLHlCQUF5QixFQUFFLE1BQU0sNERBQTRELENBQUM7QUFDdkcsT0FBTyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sMENBQTBDLENBQUM7QUFDN0UsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sb0RBQW9ELENBQUM7QUFDM0YsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLHVCQUF1QixDQUFDO0FBQ3JELE9BQU8sRUFBRSxZQUFZLEVBQUUsbUJBQW1CLEVBQUUsTUFBTSx3QkFBd0IsQ0FBQztBQUMzRSxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sMEJBQTBCLENBQUM7QUFDMUQsT0FBTyxFQUFFLG1CQUFtQixFQUFFLDBCQUEwQixFQUFFLE1BQU0saUNBQWlDLENBQUM7QUFxQmxHLE1BQU0sT0FBTyxjQUFjO0lBQ3pCLE1BQU0sQ0FBQyxPQUFPO1FBQ1osT0FBTztZQUNMLFFBQVEsRUFBRSxjQUFjO1lBQ3hCLFNBQVMsRUFBRTtnQkFDVCxjQUFjO2dCQUNkLEVBQUUsT0FBTyxFQUFFLFlBQVksRUFBRSxRQUFRLEVBQUUsbUJBQW1CLEVBQUU7Z0JBQ3hELEVBQUUsT0FBTyxFQUFFLG1CQUFtQixFQUFFLFFBQVEsRUFBRSwwQkFBMEIsRUFBRTthQUN2RTtTQUNGLENBQUM7SUFDSixDQUFDOzs7WUE3QkYsUUFBUSxTQUFDO2dCQUNSLE9BQU8sRUFBRTtvQkFDUCxXQUFXO29CQUNYLFlBQVk7aUJBQ2I7Z0JBQ0QsWUFBWSxFQUFFO29CQUNaLGlCQUFpQjtvQkFDakIscUJBQXFCO29CQUNyQixZQUFZO29CQUNaLGlCQUFpQjtvQkFDakIscUJBQXFCO29CQUNyQix1QkFBdUI7b0JBQ3ZCLHlCQUF5QjtpQkFDMUIsRUFBRSxPQUFPLEVBQUU7b0JBQ1YsaUJBQWlCO29CQUNqQixZQUFZO29CQUNaLHlCQUF5QjtpQkFDMUI7YUFDRiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IE5nTW9kdWxlLCBNb2R1bGVXaXRoUHJvdmlkZXJzIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IEZvcm1zTW9kdWxlIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xyXG5pbXBvcnQgeyBDb21tb25Nb2R1bGUgfSBmcm9tICdAYW5ndWxhci9jb21tb24nO1xyXG5pbXBvcnQgeyBEcm9wZG93bkRpcmVjdGl2ZSB9IGZyb20gJy4vZGlyZWN0aXZlcy9kcm9wZG93bi5kaXJlY3RpdmUnO1xyXG5pbXBvcnQgeyBEcm9wZG93bk1lbnVEaXJlY3RpdmUgfSBmcm9tICcuL2RpcmVjdGl2ZXMvZHJvcGRvd24tbWVudS5kaXJlY3RpdmUnO1xyXG5pbXBvcnQgeyBEcm9wZG93blRvZ2dsZURpcmVjdGl2ZSB9IGZyb20gJy4vZGlyZWN0aXZlcy9kcm9wZG93bi10b2dnbGUuZGlyZWN0aXZlJztcclxuaW1wb3J0IHsgRHJvcGRvd25UcmVldmlld0NvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy9kcm9wZG93bi10cmVldmlldy9kcm9wZG93bi10cmVldmlldy5jb21wb25lbnQnO1xyXG5pbXBvcnQgeyBUcmVldmlld0NvbXBvbmVudCB9IGZyb20gJy4vY29tcG9uZW50cy90cmVldmlldy90cmVldmlldy5jb21wb25lbnQnO1xyXG5pbXBvcnQgeyBUcmVldmlld0l0ZW1Db21wb25lbnQgfSBmcm9tICcuL2NvbXBvbmVudHMvdHJlZXZpZXctaXRlbS90cmVldmlldy1pdGVtLmNvbXBvbmVudCc7XHJcbmltcG9ydCB7IFRyZWV2aWV3UGlwZSB9IGZyb20gJy4vcGlwZXMvdHJlZXZpZXcucGlwZSc7XHJcbmltcG9ydCB7IFRyZWV2aWV3STE4biwgRGVmYXVsdFRyZWV2aWV3STE4biB9IGZyb20gJy4vbW9kZWxzL3RyZWV2aWV3LWkxOG4nO1xyXG5pbXBvcnQgeyBUcmVldmlld0NvbmZpZyB9IGZyb20gJy4vbW9kZWxzL3RyZWV2aWV3LWNvbmZpZyc7XHJcbmltcG9ydCB7IFRyZWV2aWV3RXZlbnRQYXJzZXIsIERlZmF1bHRUcmVldmlld0V2ZW50UGFyc2VyIH0gZnJvbSAnLi9oZWxwZXJzL3RyZWV2aWV3LWV2ZW50LXBhcnNlcic7XHJcblxyXG5ATmdNb2R1bGUoe1xyXG4gIGltcG9ydHM6IFtcclxuICAgIEZvcm1zTW9kdWxlLFxyXG4gICAgQ29tbW9uTW9kdWxlXHJcbiAgXSxcclxuICBkZWNsYXJhdGlvbnM6IFtcclxuICAgIFRyZWV2aWV3Q29tcG9uZW50LFxyXG4gICAgVHJlZXZpZXdJdGVtQ29tcG9uZW50LFxyXG4gICAgVHJlZXZpZXdQaXBlLFxyXG4gICAgRHJvcGRvd25EaXJlY3RpdmUsXHJcbiAgICBEcm9wZG93bk1lbnVEaXJlY3RpdmUsXHJcbiAgICBEcm9wZG93blRvZ2dsZURpcmVjdGl2ZSxcclxuICAgIERyb3Bkb3duVHJlZXZpZXdDb21wb25lbnRcclxuICBdLCBleHBvcnRzOiBbXHJcbiAgICBUcmVldmlld0NvbXBvbmVudCxcclxuICAgIFRyZWV2aWV3UGlwZSxcclxuICAgIERyb3Bkb3duVHJlZXZpZXdDb21wb25lbnRcclxuICBdXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBUcmVldmlld01vZHVsZSB7XHJcbiAgc3RhdGljIGZvclJvb3QoKTogTW9kdWxlV2l0aFByb3ZpZGVyczxUcmVldmlld01vZHVsZT4ge1xyXG4gICAgcmV0dXJuIHtcclxuICAgICAgbmdNb2R1bGU6IFRyZWV2aWV3TW9kdWxlLFxyXG4gICAgICBwcm92aWRlcnM6IFtcclxuICAgICAgICBUcmVldmlld0NvbmZpZyxcclxuICAgICAgICB7IHByb3ZpZGU6IFRyZWV2aWV3STE4biwgdXNlQ2xhc3M6IERlZmF1bHRUcmVldmlld0kxOG4gfSxcclxuICAgICAgICB7IHByb3ZpZGU6IFRyZWV2aWV3RXZlbnRQYXJzZXIsIHVzZUNsYXNzOiBEZWZhdWx0VHJlZXZpZXdFdmVudFBhcnNlciB9XHJcbiAgICAgIF1cclxuICAgIH07XHJcbiAgfVxyXG59XHJcbiJdfQ==