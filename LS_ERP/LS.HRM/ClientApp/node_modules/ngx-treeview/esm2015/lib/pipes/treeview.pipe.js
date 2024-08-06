import { Pipe } from '@angular/core';
import { isNil } from 'lodash';
import { TreeviewItem } from '../models/treeview-item';
export class TreeviewPipe {
    transform(objects, textField) {
        if (isNil(objects)) {
            return undefined;
        }
        return objects.map(object => new TreeviewItem({ text: object[textField], value: object }));
    }
}
TreeviewPipe.decorators = [
    { type: Pipe, args: [{
                name: 'ngxTreeview'
            },] }
];
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidHJlZXZpZXcucGlwZS5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIi4uLy4uLy4uLy4uLy4uL3Byb2plY3RzL25neC10cmVldmlldy9zcmMvbGliL3BpcGVzL3RyZWV2aWV3LnBpcGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUEsT0FBTyxFQUFFLElBQUksRUFBaUIsTUFBTSxlQUFlLENBQUM7QUFDcEQsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLFFBQVEsQ0FBQztBQUMvQixPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0seUJBQXlCLENBQUM7QUFLdkQsTUFBTSxPQUFPLFlBQVk7SUFDdkIsU0FBUyxDQUFDLE9BQWMsRUFBRSxTQUFpQjtRQUN6QyxJQUFJLEtBQUssQ0FBQyxPQUFPLENBQUMsRUFBRTtZQUNsQixPQUFPLFNBQVMsQ0FBQztTQUNsQjtRQUVELE9BQU8sT0FBTyxDQUFDLEdBQUcsQ0FBQyxNQUFNLENBQUMsRUFBRSxDQUFDLElBQUksWUFBWSxDQUFDLEVBQUUsSUFBSSxFQUFFLE1BQU0sQ0FBQyxTQUFTLENBQUMsRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFFLENBQUMsQ0FBQyxDQUFDO0lBQzdGLENBQUM7OztZQVZGLElBQUksU0FBQztnQkFDSixJQUFJLEVBQUUsYUFBYTthQUNwQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFBpcGUsIFBpcGVUcmFuc2Zvcm0gfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgaXNOaWwgfSBmcm9tICdsb2Rhc2gnO1xyXG5pbXBvcnQgeyBUcmVldmlld0l0ZW0gfSBmcm9tICcuLi9tb2RlbHMvdHJlZXZpZXctaXRlbSc7XHJcblxyXG5AUGlwZSh7XHJcbiAgbmFtZTogJ25neFRyZWV2aWV3J1xyXG59KVxyXG5leHBvcnQgY2xhc3MgVHJlZXZpZXdQaXBlIGltcGxlbWVudHMgUGlwZVRyYW5zZm9ybSB7XHJcbiAgdHJhbnNmb3JtKG9iamVjdHM6IGFueVtdLCB0ZXh0RmllbGQ6IHN0cmluZyk6IFRyZWV2aWV3SXRlbVtdIHtcclxuICAgIGlmIChpc05pbChvYmplY3RzKSkge1xyXG4gICAgICByZXR1cm4gdW5kZWZpbmVkO1xyXG4gICAgfVxyXG5cclxuICAgIHJldHVybiBvYmplY3RzLm1hcChvYmplY3QgPT4gbmV3IFRyZWV2aWV3SXRlbSh7IHRleHQ6IG9iamVjdFt0ZXh0RmllbGRdLCB2YWx1ZTogb2JqZWN0IH0pKTtcclxuICB9XHJcbn1cclxuIl19