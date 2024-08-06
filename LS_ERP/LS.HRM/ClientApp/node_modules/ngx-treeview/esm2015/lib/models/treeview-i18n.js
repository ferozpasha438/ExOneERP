import { Injectable } from '@angular/core';
export class TreeviewI18n {
}
TreeviewI18n.decorators = [
    { type: Injectable }
];
export class DefaultTreeviewI18n extends TreeviewI18n {
    getText(selection) {
        if (selection.uncheckedItems.length === 0) {
            if (selection.checkedItems.length > 0) {
                return this.getAllCheckboxText();
            }
            else {
                return '';
            }
        }
        switch (selection.checkedItems.length) {
            case 0:
                return 'Select options';
            case 1:
                return selection.checkedItems[0].text;
            default:
                return `${selection.checkedItems.length} options selected`;
        }
    }
    getAllCheckboxText() {
        return 'All';
    }
    getFilterPlaceholder() {
        return 'Filter';
    }
    getFilterNoItemsFoundText() {
        return 'No items found';
    }
    getTooltipCollapseExpandText(isCollapse) {
        return isCollapse ? 'Expand' : 'Collapse';
    }
}
DefaultTreeviewI18n.decorators = [
    { type: Injectable }
];
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidHJlZXZpZXctaTE4bi5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIi4uLy4uLy4uLy4uLy4uL3Byb2plY3RzL25neC10cmVldmlldy9zcmMvbGliL21vZGVscy90cmVldmlldy1pMThuLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFJM0MsTUFBTSxPQUFnQixZQUFZOzs7WUFEakMsVUFBVTs7QUFVWCxNQUFNLE9BQU8sbUJBQW9CLFNBQVEsWUFBWTtJQUNuRCxPQUFPLENBQUMsU0FBNEI7UUFDbEMsSUFBSSxTQUFTLENBQUMsY0FBYyxDQUFDLE1BQU0sS0FBSyxDQUFDLEVBQUU7WUFDekMsSUFBSSxTQUFTLENBQUMsWUFBWSxDQUFDLE1BQU0sR0FBRyxDQUFDLEVBQUU7Z0JBQ3JDLE9BQU8sSUFBSSxDQUFDLGtCQUFrQixFQUFFLENBQUM7YUFDbEM7aUJBQU07Z0JBQ0wsT0FBTyxFQUFFLENBQUM7YUFDWDtTQUNGO1FBRUQsUUFBUSxTQUFTLENBQUMsWUFBWSxDQUFDLE1BQU0sRUFBRTtZQUNyQyxLQUFLLENBQUM7Z0JBQ0osT0FBTyxnQkFBZ0IsQ0FBQztZQUMxQixLQUFLLENBQUM7Z0JBQ0osT0FBTyxTQUFTLENBQUMsWUFBWSxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQztZQUN4QztnQkFDRSxPQUFPLEdBQUcsU0FBUyxDQUFDLFlBQVksQ0FBQyxNQUFNLG1CQUFtQixDQUFDO1NBQzlEO0lBQ0gsQ0FBQztJQUVELGtCQUFrQjtRQUNoQixPQUFPLEtBQUssQ0FBQztJQUNmLENBQUM7SUFFRCxvQkFBb0I7UUFDbEIsT0FBTyxRQUFRLENBQUM7SUFDbEIsQ0FBQztJQUVELHlCQUF5QjtRQUN2QixPQUFPLGdCQUFnQixDQUFDO0lBQzFCLENBQUM7SUFFRCw0QkFBNEIsQ0FBQyxVQUFtQjtRQUM5QyxPQUFPLFVBQVUsQ0FBQyxDQUFDLENBQUMsUUFBUSxDQUFDLENBQUMsQ0FBQyxVQUFVLENBQUM7SUFDNUMsQ0FBQzs7O1lBbkNGLFVBQVUiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFRyZWV2aWV3U2VsZWN0aW9uIH0gZnJvbSAnLi90cmVldmlldy1pdGVtJztcclxuXHJcbkBJbmplY3RhYmxlKClcclxuZXhwb3J0IGFic3RyYWN0IGNsYXNzIFRyZWV2aWV3STE4biB7XHJcbiAgYWJzdHJhY3QgZ2V0VGV4dChzZWxlY3Rpb246IFRyZWV2aWV3U2VsZWN0aW9uKTogc3RyaW5nO1xyXG4gIGFic3RyYWN0IGdldEFsbENoZWNrYm94VGV4dCgpOiBzdHJpbmc7XHJcbiAgYWJzdHJhY3QgZ2V0RmlsdGVyUGxhY2Vob2xkZXIoKTogc3RyaW5nO1xyXG4gIGFic3RyYWN0IGdldEZpbHRlck5vSXRlbXNGb3VuZFRleHQoKTogc3RyaW5nO1xyXG4gIGFic3RyYWN0IGdldFRvb2x0aXBDb2xsYXBzZUV4cGFuZFRleHQoaXNDb2xsYXBzZTogYm9vbGVhbik6IHN0cmluZztcclxufVxyXG5cclxuQEluamVjdGFibGUoKVxyXG5leHBvcnQgY2xhc3MgRGVmYXVsdFRyZWV2aWV3STE4biBleHRlbmRzIFRyZWV2aWV3STE4biB7XHJcbiAgZ2V0VGV4dChzZWxlY3Rpb246IFRyZWV2aWV3U2VsZWN0aW9uKTogc3RyaW5nIHtcclxuICAgIGlmIChzZWxlY3Rpb24udW5jaGVja2VkSXRlbXMubGVuZ3RoID09PSAwKSB7XHJcbiAgICAgIGlmIChzZWxlY3Rpb24uY2hlY2tlZEl0ZW1zLmxlbmd0aCA+IDApIHtcclxuICAgICAgICByZXR1cm4gdGhpcy5nZXRBbGxDaGVja2JveFRleHQoKTtcclxuICAgICAgfSBlbHNlIHtcclxuICAgICAgICByZXR1cm4gJyc7XHJcbiAgICAgIH1cclxuICAgIH1cclxuXHJcbiAgICBzd2l0Y2ggKHNlbGVjdGlvbi5jaGVja2VkSXRlbXMubGVuZ3RoKSB7XHJcbiAgICAgIGNhc2UgMDpcclxuICAgICAgICByZXR1cm4gJ1NlbGVjdCBvcHRpb25zJztcclxuICAgICAgY2FzZSAxOlxyXG4gICAgICAgIHJldHVybiBzZWxlY3Rpb24uY2hlY2tlZEl0ZW1zWzBdLnRleHQ7XHJcbiAgICAgIGRlZmF1bHQ6XHJcbiAgICAgICAgcmV0dXJuIGAke3NlbGVjdGlvbi5jaGVja2VkSXRlbXMubGVuZ3RofSBvcHRpb25zIHNlbGVjdGVkYDtcclxuICAgIH1cclxuICB9XHJcblxyXG4gIGdldEFsbENoZWNrYm94VGV4dCgpOiBzdHJpbmcge1xyXG4gICAgcmV0dXJuICdBbGwnO1xyXG4gIH1cclxuXHJcbiAgZ2V0RmlsdGVyUGxhY2Vob2xkZXIoKTogc3RyaW5nIHtcclxuICAgIHJldHVybiAnRmlsdGVyJztcclxuICB9XHJcblxyXG4gIGdldEZpbHRlck5vSXRlbXNGb3VuZFRleHQoKTogc3RyaW5nIHtcclxuICAgIHJldHVybiAnTm8gaXRlbXMgZm91bmQnO1xyXG4gIH1cclxuXHJcbiAgZ2V0VG9vbHRpcENvbGxhcHNlRXhwYW5kVGV4dChpc0NvbGxhcHNlOiBib29sZWFuKTogc3RyaW5nIHtcclxuICAgIHJldHVybiBpc0NvbGxhcHNlID8gJ0V4cGFuZCcgOiAnQ29sbGFwc2UnO1xyXG4gIH1cclxufVxyXG4iXX0=