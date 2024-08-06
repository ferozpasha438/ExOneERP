import { Injectable } from '@angular/core';
import { isNil } from 'lodash';
export class TreeviewEventParser {
}
TreeviewEventParser.decorators = [
    { type: Injectable }
];
export class DefaultTreeviewEventParser extends TreeviewEventParser {
    getSelectedChange(component) {
        const checkedItems = component.selection.checkedItems;
        if (!isNil(checkedItems)) {
            return checkedItems.map(item => item.value);
        }
        return [];
    }
}
DefaultTreeviewEventParser.decorators = [
    { type: Injectable }
];
export class DownlineTreeviewEventParser extends TreeviewEventParser {
    getSelectedChange(component) {
        const items = component.items;
        if (!isNil(items)) {
            let result = [];
            items.forEach(item => {
                const links = this.getLinks(item, null);
                if (!isNil(links)) {
                    result = result.concat(links);
                }
            });
            return result;
        }
        return [];
    }
    getLinks(item, parent) {
        if (!isNil(item.children)) {
            const link = {
                item,
                parent
            };
            let result = [];
            item.children.forEach(child => {
                const links = this.getLinks(child, link);
                if (!isNil(links)) {
                    result = result.concat(links);
                }
            });
            return result;
        }
        if (item.checked) {
            return [{
                    item,
                    parent
                }];
        }
        return null;
    }
}
DownlineTreeviewEventParser.decorators = [
    { type: Injectable }
];
export class OrderDownlineTreeviewEventParser extends TreeviewEventParser {
    constructor() {
        super(...arguments);
        this.currentDownlines = [];
        this.parser = new DownlineTreeviewEventParser();
    }
    getSelectedChange(component) {
        const newDownlines = this.parser.getSelectedChange(component);
        if (this.currentDownlines.length === 0) {
            this.currentDownlines = newDownlines;
        }
        else {
            const intersectDownlines = [];
            this.currentDownlines.forEach(downline => {
                let foundIndex = -1;
                const length = newDownlines.length;
                for (let i = 0; i < length; i++) {
                    if (downline.item.value === newDownlines[i].item.value) {
                        foundIndex = i;
                        break;
                    }
                }
                if (foundIndex !== -1) {
                    intersectDownlines.push(newDownlines[foundIndex]);
                    newDownlines.splice(foundIndex, 1);
                }
            });
            this.currentDownlines = intersectDownlines.concat(newDownlines);
        }
        return this.currentDownlines;
    }
}
OrderDownlineTreeviewEventParser.decorators = [
    { type: Injectable }
];
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidHJlZXZpZXctZXZlbnQtcGFyc2VyLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiLi4vLi4vLi4vLi4vLi4vcHJvamVjdHMvbmd4LXRyZWV2aWV3L3NyYy9saWIvaGVscGVycy90cmVldmlldy1ldmVudC1wYXJzZXIudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sUUFBUSxDQUFDO0FBSy9CLE1BQU0sT0FBZ0IsbUJBQW1COzs7WUFEeEMsVUFBVTs7QUFNWCxNQUFNLE9BQU8sMEJBQTJCLFNBQVEsbUJBQW1CO0lBQ2pFLGlCQUFpQixDQUFDLFNBQTRCO1FBQzVDLE1BQU0sWUFBWSxHQUFHLFNBQVMsQ0FBQyxTQUFTLENBQUMsWUFBWSxDQUFDO1FBQ3RELElBQUksQ0FBQyxLQUFLLENBQUMsWUFBWSxDQUFDLEVBQUU7WUFDeEIsT0FBTyxZQUFZLENBQUMsR0FBRyxDQUFDLElBQUksQ0FBQyxFQUFFLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO1NBQzdDO1FBRUQsT0FBTyxFQUFFLENBQUM7SUFDWixDQUFDOzs7WUFURixVQUFVOztBQWtCWCxNQUFNLE9BQU8sMkJBQTRCLFNBQVEsbUJBQW1CO0lBQ2xFLGlCQUFpQixDQUFDLFNBQTRCO1FBQzVDLE1BQU0sS0FBSyxHQUFHLFNBQVMsQ0FBQyxLQUFLLENBQUM7UUFDOUIsSUFBSSxDQUFDLEtBQUssQ0FBQyxLQUFLLENBQUMsRUFBRTtZQUNqQixJQUFJLE1BQU0sR0FBMkIsRUFBRSxDQUFDO1lBQ3hDLEtBQUssQ0FBQyxPQUFPLENBQUMsSUFBSSxDQUFDLEVBQUU7Z0JBQ25CLE1BQU0sS0FBSyxHQUFHLElBQUksQ0FBQyxRQUFRLENBQUMsSUFBSSxFQUFFLElBQUksQ0FBQyxDQUFDO2dCQUN4QyxJQUFJLENBQUMsS0FBSyxDQUFDLEtBQUssQ0FBQyxFQUFFO29CQUNqQixNQUFNLEdBQUcsTUFBTSxDQUFDLE1BQU0sQ0FBQyxLQUFLLENBQUMsQ0FBQztpQkFDL0I7WUFDSCxDQUFDLENBQUMsQ0FBQztZQUVILE9BQU8sTUFBTSxDQUFDO1NBQ2Y7UUFFRCxPQUFPLEVBQUUsQ0FBQztJQUNaLENBQUM7SUFFTyxRQUFRLENBQUMsSUFBa0IsRUFBRSxNQUE0QjtRQUMvRCxJQUFJLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsRUFBRTtZQUN6QixNQUFNLElBQUksR0FBRztnQkFDWCxJQUFJO2dCQUNKLE1BQU07YUFDUCxDQUFDO1lBQ0YsSUFBSSxNQUFNLEdBQTJCLEVBQUUsQ0FBQztZQUN4QyxJQUFJLENBQUMsUUFBUSxDQUFDLE9BQU8sQ0FBQyxLQUFLLENBQUMsRUFBRTtnQkFDNUIsTUFBTSxLQUFLLEdBQUcsSUFBSSxDQUFDLFFBQVEsQ0FBQyxLQUFLLEVBQUUsSUFBSSxDQUFDLENBQUM7Z0JBQ3pDLElBQUksQ0FBQyxLQUFLLENBQUMsS0FBSyxDQUFDLEVBQUU7b0JBQ2pCLE1BQU0sR0FBRyxNQUFNLENBQUMsTUFBTSxDQUFDLEtBQUssQ0FBQyxDQUFDO2lCQUMvQjtZQUNILENBQUMsQ0FBQyxDQUFDO1lBRUgsT0FBTyxNQUFNLENBQUM7U0FDZjtRQUVELElBQUksSUFBSSxDQUFDLE9BQU8sRUFBRTtZQUNoQixPQUFPLENBQUM7b0JBQ04sSUFBSTtvQkFDSixNQUFNO2lCQUNQLENBQUMsQ0FBQztTQUNKO1FBRUQsT0FBTyxJQUFJLENBQUM7SUFDZCxDQUFDOzs7WUE1Q0YsVUFBVTs7QUFnRFgsTUFBTSxPQUFPLGdDQUFpQyxTQUFRLG1CQUFtQjtJQUR6RTs7UUFFVSxxQkFBZ0IsR0FBMkIsRUFBRSxDQUFDO1FBQzlDLFdBQU0sR0FBRyxJQUFJLDJCQUEyQixFQUFFLENBQUM7SUE2QnJELENBQUM7SUEzQkMsaUJBQWlCLENBQUMsU0FBNEI7UUFDNUMsTUFBTSxZQUFZLEdBQTJCLElBQUksQ0FBQyxNQUFNLENBQUMsaUJBQWlCLENBQUMsU0FBUyxDQUFDLENBQUM7UUFDdEYsSUFBSSxJQUFJLENBQUMsZ0JBQWdCLENBQUMsTUFBTSxLQUFLLENBQUMsRUFBRTtZQUN0QyxJQUFJLENBQUMsZ0JBQWdCLEdBQUcsWUFBWSxDQUFDO1NBQ3RDO2FBQU07WUFDTCxNQUFNLGtCQUFrQixHQUEyQixFQUFFLENBQUM7WUFDdEQsSUFBSSxDQUFDLGdCQUFnQixDQUFDLE9BQU8sQ0FBQyxRQUFRLENBQUMsRUFBRTtnQkFDdkMsSUFBSSxVQUFVLEdBQUcsQ0FBQyxDQUFDLENBQUM7Z0JBQ3BCLE1BQU0sTUFBTSxHQUFHLFlBQVksQ0FBQyxNQUFNLENBQUM7Z0JBQ25DLEtBQUssSUFBSSxDQUFDLEdBQUcsQ0FBQyxFQUFFLENBQUMsR0FBRyxNQUFNLEVBQUUsQ0FBQyxFQUFFLEVBQUU7b0JBQy9CLElBQUksUUFBUSxDQUFDLElBQUksQ0FBQyxLQUFLLEtBQUssWUFBWSxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxLQUFLLEVBQUU7d0JBQ3RELFVBQVUsR0FBRyxDQUFDLENBQUM7d0JBQ2YsTUFBTTtxQkFDUDtpQkFDRjtnQkFFRCxJQUFJLFVBQVUsS0FBSyxDQUFDLENBQUMsRUFBRTtvQkFDckIsa0JBQWtCLENBQUMsSUFBSSxDQUFDLFlBQVksQ0FBQyxVQUFVLENBQUMsQ0FBQyxDQUFDO29CQUNsRCxZQUFZLENBQUMsTUFBTSxDQUFDLFVBQVUsRUFBRSxDQUFDLENBQUMsQ0FBQztpQkFDcEM7WUFDSCxDQUFDLENBQUMsQ0FBQztZQUVILElBQUksQ0FBQyxnQkFBZ0IsR0FBRyxrQkFBa0IsQ0FBQyxNQUFNLENBQUMsWUFBWSxDQUFDLENBQUM7U0FDakU7UUFFRCxPQUFPLElBQUksQ0FBQyxnQkFBZ0IsQ0FBQztJQUMvQixDQUFDOzs7WUEvQkYsVUFBVSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgaXNOaWwgfSBmcm9tICdsb2Rhc2gnO1xyXG5pbXBvcnQgeyBUcmVldmlld0l0ZW0gfSBmcm9tICcuLi9tb2RlbHMvdHJlZXZpZXctaXRlbSc7XHJcbmltcG9ydCB7IFRyZWV2aWV3Q29tcG9uZW50IH0gZnJvbSAnLi4vY29tcG9uZW50cy90cmVldmlldy90cmVldmlldy5jb21wb25lbnQnO1xyXG5cclxuQEluamVjdGFibGUoKVxyXG5leHBvcnQgYWJzdHJhY3QgY2xhc3MgVHJlZXZpZXdFdmVudFBhcnNlciB7XHJcbiAgYWJzdHJhY3QgZ2V0U2VsZWN0ZWRDaGFuZ2UoY29tcG9uZW50OiBUcmVldmlld0NvbXBvbmVudCk6IGFueVtdO1xyXG59XHJcblxyXG5ASW5qZWN0YWJsZSgpXHJcbmV4cG9ydCBjbGFzcyBEZWZhdWx0VHJlZXZpZXdFdmVudFBhcnNlciBleHRlbmRzIFRyZWV2aWV3RXZlbnRQYXJzZXIge1xyXG4gIGdldFNlbGVjdGVkQ2hhbmdlKGNvbXBvbmVudDogVHJlZXZpZXdDb21wb25lbnQpOiBhbnlbXSB7XHJcbiAgICBjb25zdCBjaGVja2VkSXRlbXMgPSBjb21wb25lbnQuc2VsZWN0aW9uLmNoZWNrZWRJdGVtcztcclxuICAgIGlmICghaXNOaWwoY2hlY2tlZEl0ZW1zKSkge1xyXG4gICAgICByZXR1cm4gY2hlY2tlZEl0ZW1zLm1hcChpdGVtID0+IGl0ZW0udmFsdWUpO1xyXG4gICAgfVxyXG5cclxuICAgIHJldHVybiBbXTtcclxuICB9XHJcbn1cclxuXHJcbmV4cG9ydCBpbnRlcmZhY2UgRG93bmxpbmVUcmVldmlld0l0ZW0ge1xyXG4gIGl0ZW06IFRyZWV2aWV3SXRlbTtcclxuICBwYXJlbnQ6IERvd25saW5lVHJlZXZpZXdJdGVtO1xyXG59XHJcblxyXG5ASW5qZWN0YWJsZSgpXHJcbmV4cG9ydCBjbGFzcyBEb3dubGluZVRyZWV2aWV3RXZlbnRQYXJzZXIgZXh0ZW5kcyBUcmVldmlld0V2ZW50UGFyc2VyIHtcclxuICBnZXRTZWxlY3RlZENoYW5nZShjb21wb25lbnQ6IFRyZWV2aWV3Q29tcG9uZW50KTogYW55W10ge1xyXG4gICAgY29uc3QgaXRlbXMgPSBjb21wb25lbnQuaXRlbXM7XHJcbiAgICBpZiAoIWlzTmlsKGl0ZW1zKSkge1xyXG4gICAgICBsZXQgcmVzdWx0OiBEb3dubGluZVRyZWV2aWV3SXRlbVtdID0gW107XHJcbiAgICAgIGl0ZW1zLmZvckVhY2goaXRlbSA9PiB7XHJcbiAgICAgICAgY29uc3QgbGlua3MgPSB0aGlzLmdldExpbmtzKGl0ZW0sIG51bGwpO1xyXG4gICAgICAgIGlmICghaXNOaWwobGlua3MpKSB7XHJcbiAgICAgICAgICByZXN1bHQgPSByZXN1bHQuY29uY2F0KGxpbmtzKTtcclxuICAgICAgICB9XHJcbiAgICAgIH0pO1xyXG5cclxuICAgICAgcmV0dXJuIHJlc3VsdDtcclxuICAgIH1cclxuXHJcbiAgICByZXR1cm4gW107XHJcbiAgfVxyXG5cclxuICBwcml2YXRlIGdldExpbmtzKGl0ZW06IFRyZWV2aWV3SXRlbSwgcGFyZW50OiBEb3dubGluZVRyZWV2aWV3SXRlbSk6IERvd25saW5lVHJlZXZpZXdJdGVtW10ge1xyXG4gICAgaWYgKCFpc05pbChpdGVtLmNoaWxkcmVuKSkge1xyXG4gICAgICBjb25zdCBsaW5rID0ge1xyXG4gICAgICAgIGl0ZW0sXHJcbiAgICAgICAgcGFyZW50XHJcbiAgICAgIH07XHJcbiAgICAgIGxldCByZXN1bHQ6IERvd25saW5lVHJlZXZpZXdJdGVtW10gPSBbXTtcclxuICAgICAgaXRlbS5jaGlsZHJlbi5mb3JFYWNoKGNoaWxkID0+IHtcclxuICAgICAgICBjb25zdCBsaW5rcyA9IHRoaXMuZ2V0TGlua3MoY2hpbGQsIGxpbmspO1xyXG4gICAgICAgIGlmICghaXNOaWwobGlua3MpKSB7XHJcbiAgICAgICAgICByZXN1bHQgPSByZXN1bHQuY29uY2F0KGxpbmtzKTtcclxuICAgICAgICB9XHJcbiAgICAgIH0pO1xyXG5cclxuICAgICAgcmV0dXJuIHJlc3VsdDtcclxuICAgIH1cclxuXHJcbiAgICBpZiAoaXRlbS5jaGVja2VkKSB7XHJcbiAgICAgIHJldHVybiBbe1xyXG4gICAgICAgIGl0ZW0sXHJcbiAgICAgICAgcGFyZW50XHJcbiAgICAgIH1dO1xyXG4gICAgfVxyXG5cclxuICAgIHJldHVybiBudWxsO1xyXG4gIH1cclxufVxyXG5cclxuQEluamVjdGFibGUoKVxyXG5leHBvcnQgY2xhc3MgT3JkZXJEb3dubGluZVRyZWV2aWV3RXZlbnRQYXJzZXIgZXh0ZW5kcyBUcmVldmlld0V2ZW50UGFyc2VyIHtcclxuICBwcml2YXRlIGN1cnJlbnREb3dubGluZXM6IERvd25saW5lVHJlZXZpZXdJdGVtW10gPSBbXTtcclxuICBwcml2YXRlIHBhcnNlciA9IG5ldyBEb3dubGluZVRyZWV2aWV3RXZlbnRQYXJzZXIoKTtcclxuXHJcbiAgZ2V0U2VsZWN0ZWRDaGFuZ2UoY29tcG9uZW50OiBUcmVldmlld0NvbXBvbmVudCk6IGFueVtdIHtcclxuICAgIGNvbnN0IG5ld0Rvd25saW5lczogRG93bmxpbmVUcmVldmlld0l0ZW1bXSA9IHRoaXMucGFyc2VyLmdldFNlbGVjdGVkQ2hhbmdlKGNvbXBvbmVudCk7XHJcbiAgICBpZiAodGhpcy5jdXJyZW50RG93bmxpbmVzLmxlbmd0aCA9PT0gMCkge1xyXG4gICAgICB0aGlzLmN1cnJlbnREb3dubGluZXMgPSBuZXdEb3dubGluZXM7XHJcbiAgICB9IGVsc2Uge1xyXG4gICAgICBjb25zdCBpbnRlcnNlY3REb3dubGluZXM6IERvd25saW5lVHJlZXZpZXdJdGVtW10gPSBbXTtcclxuICAgICAgdGhpcy5jdXJyZW50RG93bmxpbmVzLmZvckVhY2goZG93bmxpbmUgPT4ge1xyXG4gICAgICAgIGxldCBmb3VuZEluZGV4ID0gLTE7XHJcbiAgICAgICAgY29uc3QgbGVuZ3RoID0gbmV3RG93bmxpbmVzLmxlbmd0aDtcclxuICAgICAgICBmb3IgKGxldCBpID0gMDsgaSA8IGxlbmd0aDsgaSsrKSB7XHJcbiAgICAgICAgICBpZiAoZG93bmxpbmUuaXRlbS52YWx1ZSA9PT0gbmV3RG93bmxpbmVzW2ldLml0ZW0udmFsdWUpIHtcclxuICAgICAgICAgICAgZm91bmRJbmRleCA9IGk7XHJcbiAgICAgICAgICAgIGJyZWFrO1xyXG4gICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgaWYgKGZvdW5kSW5kZXggIT09IC0xKSB7XHJcbiAgICAgICAgICBpbnRlcnNlY3REb3dubGluZXMucHVzaChuZXdEb3dubGluZXNbZm91bmRJbmRleF0pO1xyXG4gICAgICAgICAgbmV3RG93bmxpbmVzLnNwbGljZShmb3VuZEluZGV4LCAxKTtcclxuICAgICAgICB9XHJcbiAgICAgIH0pO1xyXG5cclxuICAgICAgdGhpcy5jdXJyZW50RG93bmxpbmVzID0gaW50ZXJzZWN0RG93bmxpbmVzLmNvbmNhdChuZXdEb3dubGluZXMpO1xyXG4gICAgfVxyXG5cclxuICAgIHJldHVybiB0aGlzLmN1cnJlbnREb3dubGluZXM7XHJcbiAgfVxyXG59XHJcbiJdfQ==