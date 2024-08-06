import { isBoolean, isNil, isString } from 'lodash';
import { TreeviewHelper } from '../helpers/treeview-helper';
export class TreeviewItem {
    constructor(item, autoCorrectChecked = false) {
        this.internalDisabled = false;
        this.internalChecked = true;
        this.internalCollapsed = false;
        if (isNil(item)) {
            throw new Error('Item must be defined');
        }
        if (isString(item.text)) {
            this.text = item.text;
        }
        else {
            throw new Error('A text of item must be string object');
        }
        this.value = item.value;
        if (isBoolean(item.checked)) {
            this.checked = item.checked;
        }
        if (isBoolean(item.collapsed)) {
            this.collapsed = item.collapsed;
        }
        if (isBoolean(item.disabled)) {
            this.disabled = item.disabled;
        }
        if (!isNil(item.children) && item.children.length > 0) {
            this.children = item.children.map(child => {
                if (this.disabled === true) {
                    child.disabled = true;
                }
                return new TreeviewItem(child);
            });
        }
        if (autoCorrectChecked) {
            this.correctChecked();
        }
    }
    get checked() {
        return this.internalChecked;
    }
    set checked(value) {
        if (!this.internalDisabled) {
            if (this.internalChecked !== value) {
                this.internalChecked = value;
            }
        }
    }
    get indeterminate() {
        return this.checked === undefined;
    }
    setCheckedRecursive(value) {
        if (!this.internalDisabled) {
            this.internalChecked = value;
            if (!isNil(this.internalChildren)) {
                this.internalChildren.forEach(child => child.setCheckedRecursive(value));
            }
        }
    }
    get disabled() {
        return this.internalDisabled;
    }
    set disabled(value) {
        if (this.internalDisabled !== value) {
            this.internalDisabled = value;
            if (!isNil(this.internalChildren)) {
                this.internalChildren.forEach(child => child.disabled = value);
            }
        }
    }
    get collapsed() {
        return this.internalCollapsed;
    }
    set collapsed(value) {
        if (this.internalCollapsed !== value) {
            this.internalCollapsed = value;
        }
    }
    setCollapsedRecursive(value) {
        this.internalCollapsed = value;
        if (!isNil(this.internalChildren)) {
            this.internalChildren.forEach(child => child.setCollapsedRecursive(value));
        }
    }
    get children() {
        return this.internalChildren;
    }
    set children(value) {
        if (this.internalChildren !== value) {
            if (!isNil(value) && value.length === 0) {
                throw new Error('Children must be not an empty array');
            }
            this.internalChildren = value;
            if (!isNil(this.internalChildren)) {
                let checked = null;
                this.internalChildren.forEach(child => {
                    if (checked === null) {
                        checked = child.checked;
                    }
                    else {
                        if (child.checked !== checked) {
                            checked = undefined;
                            return;
                        }
                    }
                });
                this.internalChecked = checked;
            }
        }
    }
    getSelection() {
        let checkedItems = [];
        let uncheckedItems = [];
        if (isNil(this.internalChildren)) {
            if (this.internalChecked) {
                checkedItems.push(this);
            }
            else {
                uncheckedItems.push(this);
            }
        }
        else {
            const selection = TreeviewHelper.concatSelection(this.internalChildren, checkedItems, uncheckedItems);
            checkedItems = selection.checked;
            uncheckedItems = selection.unchecked;
        }
        return {
            checkedItems,
            uncheckedItems
        };
    }
    correctChecked() {
        this.internalChecked = this.getCorrectChecked();
    }
    getCorrectChecked() {
        let checked = null;
        if (!isNil(this.internalChildren)) {
            for (const child of this.internalChildren) {
                child.internalChecked = child.getCorrectChecked();
                if (checked === null) {
                    checked = child.internalChecked;
                }
                else if (checked !== child.internalChecked) {
                    checked = undefined;
                    break;
                }
            }
        }
        else {
            checked = this.checked;
        }
        return checked;
    }
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidHJlZXZpZXctaXRlbS5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIi4uLy4uLy4uLy4uLy4uL3Byb2plY3RzL25neC10cmVldmlldy9zcmMvbGliL21vZGVscy90cmVldmlldy1pdGVtLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsS0FBSyxFQUFFLFFBQVEsRUFBRSxNQUFNLFFBQVEsQ0FBQztBQUNwRCxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sNEJBQTRCLENBQUM7QUFnQjVELE1BQU0sT0FBTyxZQUFZO0lBUXZCLFlBQVksSUFBYyxFQUFFLGtCQUFrQixHQUFHLEtBQUs7UUFQOUMscUJBQWdCLEdBQUcsS0FBSyxDQUFDO1FBQ3pCLG9CQUFlLEdBQUcsSUFBSSxDQUFDO1FBQ3ZCLHNCQUFpQixHQUFHLEtBQUssQ0FBQztRQU1oQyxJQUFJLEtBQUssQ0FBQyxJQUFJLENBQUMsRUFBRTtZQUNmLE1BQU0sSUFBSSxLQUFLLENBQUMsc0JBQXNCLENBQUMsQ0FBQztTQUN6QztRQUNELElBQUksUUFBUSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsRUFBRTtZQUN2QixJQUFJLENBQUMsSUFBSSxHQUFHLElBQUksQ0FBQyxJQUFJLENBQUM7U0FDdkI7YUFBTTtZQUNMLE1BQU0sSUFBSSxLQUFLLENBQUMsc0NBQXNDLENBQUMsQ0FBQztTQUN6RDtRQUNELElBQUksQ0FBQyxLQUFLLEdBQUcsSUFBSSxDQUFDLEtBQUssQ0FBQztRQUN4QixJQUFJLFNBQVMsQ0FBQyxJQUFJLENBQUMsT0FBTyxDQUFDLEVBQUU7WUFDM0IsSUFBSSxDQUFDLE9BQU8sR0FBRyxJQUFJLENBQUMsT0FBTyxDQUFDO1NBQzdCO1FBQ0QsSUFBSSxTQUFTLENBQUMsSUFBSSxDQUFDLFNBQVMsQ0FBQyxFQUFFO1lBQzdCLElBQUksQ0FBQyxTQUFTLEdBQUcsSUFBSSxDQUFDLFNBQVMsQ0FBQztTQUNqQztRQUNELElBQUksU0FBUyxDQUFD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LE1BQU0sU0FBUyxHQUFHLGNBQWMsQ0FBQyxlQUFlLENBQUMsSUFBSSxDQUFDLGdCQUFnQixFQUFFLFlBQVksRUFBRSxjQUFjLENBQUMsQ0FBQztZQUN0RyxZQUFZLEdBQUcsU0FBUyxDQUFDLE9BQU8sQ0FBQztZQUNqQyxjQUFjLEdBQUcsU0FBUyxDQUFDLFNBQVMsQ0FBQztTQUN0QztRQUVELE9BQU87WUFDTCxZQUFZO1lBQ1osY0FBYztTQUNmLENBQUM7SUFDSixDQUFDO0lBRUQsY0FBYztRQUNaLElBQUksQ0FBQyxlQUFlLEdBQUcsSUFBSSxDQUFDLGlCQUFpQixFQUFFLENBQUM7SUFDbEQsQ0FBQztJQUVPLGlCQUFpQjtRQUN2QixJQUFJLE9BQU8sR0FBWSxJQUFJLENBQUM7UUFDNUIsSUFBSSxDQUFDLEtBQUssQ0FBQyxJQUFJLENBQUMsZ0JBQWdCLENBQUMsRUFBRTtZQUNqQyxLQUFLLE1BQU0sS0FBSyxJQUFJLElBQUksQ0FBQyxnQkFBZ0IsRUFBRTtnQkFDekMsS0FBSyxDQUFDLGVBQWUsR0FBRyxLQUFLLENBQUMsaUJBQWlCLEVBQUUsQ0FBQztnQkFDbEQsSUFBSSxPQUFPLEtBQUssSUFBSSxFQUFFO29CQUNwQixPQUFPLEdBQUcsS0FBSyxDQUFDLGVBQWUsQ0FBQztpQkFDakM7cUJBQU0sSUFBSSxPQUFPLEtBQUssS0FBSyxDQUFDLGVBQWUsRUFBRTtvQkFDNUMsT0FBTyxHQUFHLFNBQVMsQ0FBQztvQkFDcEIsTUFBTTtpQkFDUDthQUNGO1NBQ0Y7YUFBTTtZQUNMLE9BQU8sR0FBRyxJQUFJLENBQUMsT0FBTyxDQUFDO1NBQ3hCO1FBRUQsT0FBTyxPQUFPLENBQUM7SUFDakIsQ0FBQztDQUNGIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgaXNCb29sZWFuLCBpc05pbCwgaXNTdHJpbmcgfSBmcm9tICdsb2Rhc2gnO1xyXG5pbXBvcnQgeyBUcmVldmlld0hlbHBlciB9IGZyb20gJy4uL2hlbHBlcnMvdHJlZXZpZXctaGVscGVyJztcclxuXHJcbmV4cG9ydCBpbnRlcmZhY2UgVHJlZXZpZXdTZWxlY3Rpb24ge1xyXG4gIGNoZWNrZWRJdGVtczogVHJlZXZpZXdJdGVtW107XHJcbiAgdW5jaGVja2VkSXRlbXM6IFRyZWV2aWV3SXRlbVtdO1xyXG59XHJcblxyXG5leHBvcnQgaW50ZXJmYWNlIFRyZWVJdGVtIHtcclxuICB0ZXh0OiBzdHJpbmc7XHJcbiAgdmFsdWU6IGFueTtcclxuICBkaXNhYmxlZD86IGJvb2xlYW47XHJcbiAgY2hlY2tlZD86IGJvb2xlYW47XHJcbiAgY29sbGFwc2VkPzogYm9vbGVhbjtcclxuICBjaGlsZHJlbj86IFRyZWVJdGVtW107XHJcbn1cclxuXHJcbmV4cG9ydCBjbGFzcyBUcmVldmlld0l0ZW0ge1xyXG4gIHByaXZhdGUgaW50ZXJuYWxEaXNhYmxlZCA9IGZhbHNlO1xyXG4gIHByaXZhdGUgaW50ZXJuYWxDaGVja2VkID0gdHJ1ZTtcclxuICBwcml2YXRlIGludGVybmFsQ29sbGFwc2VkID0gZmFsc2U7XHJcbiAgcHJpdmF0ZSBpbnRlcm5hbENoaWxkcmVuOiBUcmVldmlld0l0ZW1bXTtcclxuICB0ZXh0OiBzdHJpbmc7XHJcbiAgdmFsdWU6IGFueTtcclxuXHJcbiAgY29uc3RydWN0b3IoaXRlbTogVHJlZUl0ZW0sIGF1dG9Db3JyZWN0Q2hlY2tlZCA9IGZhbHNlKSB7XHJcbiAgICBpZiAoaXNOaWwoaXRlbSkpIHtcclxuICAgICAgdGhyb3cgbmV3IEVycm9yKCdJdGVtIG11c3QgYmUgZGVmaW5lZCcpO1xyXG4gICAgfVxyXG4gICAgaWYgKGlzU3RyaW5nKGl0ZW0udGV4dCkpIHtcclxuICAgICAgdGhpcy50ZXh0ID0gaXRlbS50ZXh0O1xyXG4gICAgfSBlbHNlIHtcclxuICAgICAgdGhyb3cgbmV3IEVycm9yKCdBIHRleHQgb2YgaXRlbSBtdXN0IGJlIHN0cmluZyBvYmplY3QnKTtcclxuICAgIH1cclxuICAgIHRoaXMudmFsdWUgPSBpdGVtLnZhbHVlO1xyXG4gICAgaWYgKGlzQm9vbGVhbihpdGVtLmNoZWNrZWQpKSB7XHJcbiAgICAgIHRoaXMuY2hlY2tlZCA9IGl0ZW0uY2hlY2tlZDtcclxuICAgIH1cclxuICAgIGlmIChpc0Jvb2xlYW4oaXRlbS5jb2xsYXBzZWQpKSB7XHJcbiAgICAgIHRoaXMuY29sbGFwc2VkID0gaXRlbS5jb2xsYXBzZWQ7XHJcbiAgICB9XHJcbiAgICBpZiAoaXNCb29sZWFuKGl0ZW0uZGlzYWJsZWQpKSB7XHJcbiAgICAgIHRoaXMuZGlzYWJsZWQgPSBpdGVtLmRpc2FibGVkO1xyXG4gICAgfVxyXG4gICAgaWYgKCFpc05pbChpdGVtLmNoaWxkcmVuKSAmJiBpdGVtLmNoaWxkcmVuLmxlbmd0aCA+IDApIHtcclxuICAgICAgdGhpcy5jaGlsZHJlbiA9IGl0ZW0uY2hpbGRyZW4ubWFwKGNoaWxkID0+IHtcclxuICAgICAgICBpZiAodGhpcy5kaXNhYmxlZCA9PT0gdHJ1ZSkge1xyXG4gICAgICAgICAgY2hpbGQuZGlzYWJsZWQgPSB0cnVlO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgcmV0dXJuIG5ldyBUcmVldmlld0l0ZW0oY2hpbGQpO1xyXG4gICAgICB9KTtcclxuICAgIH1cclxuXHJcbiAgICBpZiAoYXV0b0NvcnJlY3RDaGVja2VkKSB7XHJcbiAgICAgIHRoaXMuY29ycmVjdENoZWNrZWQoKTtcclxuICAgIH1cclxuICB9XHJcblxyXG4gIGdldCBjaGVja2VkKCk6IGJvb2xlYW4ge1xyXG4gICAgcmV0dXJuIHRoaXMuaW50ZXJuYWxDaGVja2VkO1xyXG4gIH1cclxuXHJcbiAgc2V0IGNoZWNrZWQodmFsdWU6IGJvb2xlYW4pIHtcclxuICAgIGlmICghdGhpcy5pbnRlcm5hbERpc2FibGVkKSB7XHJcbiAgICAgIGlmICh0aGlzLmludGVybmFsQ2hlY2tlZCAhPT0gdmFsdWUpIHtcclxuICAgICAgICB0aGlzLmludGVybmFsQ2hlY2tlZCA9IHZhbHVlO1xyXG4gICAgICB9XHJcbiAgICB9XHJcbiAgfVxyXG5cclxuICBnZXQgaW5kZXRlcm1pbmF0ZSgpOiBib29sZWFuIHtcclxuICAgIHJldHVybiB0aGlzLmNoZWNrZWQgPT09IHVuZGVmaW5lZDtcclxuICB9XHJcblxyXG4gIHNldENoZWNrZWRSZWN1cnNpdmUodmFsdWU6IGJvb2xlYW4pOiB2b2lkIHtcclxuICAgIGlmICghdGhpcy5pbnRlcm5hbERpc2FibGVkKSB7XHJcbiAgICAgIHRoaXMuaW50ZXJuYWxDaGVja2VkID0gdmFsdWU7XHJcbiAgICAgIGlmICghaXNOaWwodGhpcy5pbnRlcm5hbENoaWxkcmVuKSkge1xyXG4gICAgICAgIHRoaXMuaW50ZXJuYWxDaGlsZHJlbi5mb3JFYWNoKGNoaWxkID0+IGNoaWxkLnNldENoZWNrZWRSZWN1cnNpdmUodmFsdWUpKTtcclxuICAgICAgfVxyXG4gICAgfVxyXG4gIH1cclxuXHJcbiAgZ2V0IGRpc2FibGVkKCk6IGJvb2xlYW4ge1xyXG4gICAgcmV0dXJuIHRoaXMuaW50ZXJuYWxEaXNhYmxlZDtcclxuICB9XHJcblxyXG4gIHNldCBkaXNhYmxlZCh2YWx1ZTogYm9vbGVhbikge1xyXG4gICAgaWYgKHRoaXMuaW50ZXJuYWxEaXNhYmxlZCAhPT0gdmFsdWUpIHtcclxuICAgICAgdGhpcy5pbnRlcm5hbERpc2FibGVkID0gdmFsdWU7XHJcbiAgICAgIGlmICghaXNOaWwodGhpcy5pbnRlcm5hbENoaWxkcmVuKSkge1xyXG4gICAgICAgIHRoaXMuaW50ZXJuYWxDaGlsZHJlbi5mb3JFYWNoKGNoaWxkID0+IGNoaWxkLmRpc2FibGVkID0gdmFsdWUpO1xyXG4gICAgICB9XHJcbiAgICB9XHJcbiAgfVxyXG5cclxuICBnZXQgY29sbGFwc2VkKCk6IGJvb2xlYW4ge1xyXG4gICAgcmV0dXJuIHRoaXMuaW50ZXJuYWxDb2xsYXBzZWQ7XHJcbiAgfVxyXG5cclxuICBzZXQgY29sbGFwc2VkKHZhbHVlOiBib29sZWFuKSB7XHJcbiAgICBpZiAodGhpcy5pbnRlcm5hbENvbGxhcHNlZCAhPT0gdmFsdWUpIHtcclxuICAgICAgdGhpcy5pbnRlcm5hbENvbGxhcHNlZCA9IHZhbHVlO1xyXG4gICAgfVxyXG4gIH1cclxuXHJcbiAgc2V0Q29sbGFwc2VkUmVjdXJzaXZlKHZhbHVlOiBib29sZWFuKTogdm9pZCB7XHJcbiAgICB0aGlzLmludGVybmFsQ29sbGFwc2VkID0gdmFsdWU7XHJcbiAgICBpZiAoIWlzTmlsKHRoaXMuaW50ZXJuYWxDaGlsZHJlbikpIHtcclxuICAgICAgdGhpcy5pbnRlcm5hbENoaWxkcmVuLmZvckVhY2goY2hpbGQgPT4gY2hpbGQuc2V0Q29sbGFwc2VkUmVjdXJzaXZlKHZhbHVlKSk7XHJcbiAgICB9XHJcbiAgfVxyXG5cclxuICBnZXQgY2hpbGRyZW4oKTogVHJlZXZpZXdJdGVtW10ge1xyXG4gICAgcmV0dXJuIHRoaXMuaW50ZXJuYWxDaGlsZHJlbjtcclxuICB9XHJcblxyXG4gIHNldCBjaGlsZHJlbih2YWx1ZTogVHJlZXZpZXdJdGVtW10pIHtcclxuICAgIGlmICh0aGlzLmludGVybmFsQ2hpbGRyZW4gIT09IHZhbHVlKSB7XHJcbiAgICAgIGlmICghaXNOaWwodmFsdWUpICYmIHZhbHVlLmxlbmd0aCA9PT0gMCkge1xyXG4gICAgICAgIHRocm93IG5ldyBFcnJvcignQ2hpbGRyZW4gbXVzdCBiZSBub3QgYW4gZW1wdHkgYXJyYXknKTtcclxuICAgICAgfVxyXG4gICAgICB0aGlzLmludGVybmFsQ2hpbGRyZW4gPSB2YWx1ZTtcclxuICAgICAgaWYgKCFpc05pbCh0aGlzLmludGVybmFsQ2hpbGRyZW4pKSB7XHJcbiAgICAgICAgbGV0IGNoZWNrZWQgPSBudWxsO1xyXG4gICAgICAgIHRoaXMuaW50ZXJuYWxDaGlsZHJlbi5mb3JFYWNoKGNoaWxkID0+IHtcclxuICAgICAgICAgIGlmIChjaGVja2VkID09PSBudWxsKSB7XHJcbiAgICAgICAgICAgIGNoZWNrZWQgPSBjaGlsZC5jaGVja2VkO1xyXG4gICAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgaWYgKGNoaWxkLmNoZWNrZWQgIT09IGNoZWNrZWQpIHtcclxuICAgICAgICAgICAgICBjaGVja2VkID0gdW5kZWZpbmVkO1xyXG4gICAgICAgICAgICAgIHJldHVybjtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgICAgfVxyXG4gICAgICAgIH0pO1xyXG4gICAgICAgIHRoaXMuaW50ZXJuYWxDaGVja2VkID0gY2hlY2tlZDtcclxuICAgICAgfVxyXG4gICAgfVxyXG4gIH1cclxuXHJcbiAgZ2V0U2VsZWN0aW9uKCk6IFRyZWV2aWV3U2VsZWN0aW9uIHtcclxuICAgIGxldCBjaGVja2VkSXRlbXM6IFRyZWV2aWV3SXRlbVtdID0gW107XHJcbiAgICBsZXQgdW5jaGVja2VkSXRlbXM6IFRyZWV2aWV3SXRlbVtdID0gW107XHJcbiAgICBpZiAoaXNOaWwodGhpcy5pbnRlcm5hbENoaWxkcmVuKSkge1xyXG4gICAgICBpZiAodGhpcy5pbnRlcm5hbENoZWNrZWQpIHtcclxuICAgICAgICBjaGVja2VkSXRlbXMucHVzaCh0aGlzKTtcclxuICAgICAgfSBlbHNlIHtcclxuICAgICAgICB1bmNoZWNrZWRJdGVtcy5wdXNoKHRoaXMpO1xyXG4gICAgICB9XHJcbiAgICB9IGVsc2Uge1xyXG4gICAgICBjb25zdCBzZWxlY3Rpb24gPSBUcmVldmlld0hlbHBlci5jb25jYXRTZWxlY3Rpb24odGhpcy5pbnRlcm5hbENoaWxkcmVuLCBjaGVja2VkSXRlbXMsIHVuY2hlY2tlZEl0ZW1zKTtcclxuICAgICAgY2hlY2tlZEl0ZW1zID0gc2VsZWN0aW9uLmNoZWNrZWQ7XHJcbiAgICAgIHVuY2hlY2tlZEl0ZW1zID0gc2VsZWN0aW9uLnVuY2hlY2tlZDtcclxuICAgIH1cclxuXHJcbiAgICByZXR1cm4ge1xyXG4gICAgICBjaGVja2VkSXRlbXMsXHJcbiAgICAgIHVuY2hlY2tlZEl0ZW1zXHJcbiAgICB9O1xyXG4gIH1cclxuXHJcbiAgY29ycmVjdENoZWNrZWQoKTogdm9pZCB7XHJcbiAgICB0aGlzLmludGVybmFsQ2hlY2tlZCA9IHRoaXMuZ2V0Q29ycmVjdENoZWNrZWQoKTtcclxuICB9XHJcblxyXG4gIHByaXZhdGUgZ2V0Q29ycmVjdENoZWNrZWQoKTogYm9vbGVhbiB7XHJcbiAgICBsZXQgY2hlY2tlZDogYm9vbGVhbiA9IG51bGw7XHJcbiAgICBpZiAoIWlzTmlsKHRoaXMuaW50ZXJuYWxDaGlsZHJlbikpIHtcclxuICAgICAgZm9yIChjb25zdCBjaGlsZCBvZiB0aGlzLmludGVybmFsQ2hpbGRyZW4pIHtcclxuICAgICAgICBjaGlsZC5pbnRlcm5hbENoZWNrZWQgPSBjaGlsZC5nZXRDb3JyZWN0Q2hlY2tlZCgpO1xyXG4gICAgICAgIGlmIChjaGVja2VkID09PSBudWxsKSB7XHJcbiAgICAgICAgICBjaGVja2VkID0gY2hpbGQuaW50ZXJuYWxDaGVja2VkO1xyXG4gICAgICAgIH0gZWxzZSBpZiAoY2hlY2tlZCAhPT0gY2hpbGQuaW50ZXJuYWxDaGVja2VkKSB7XHJcbiAgICAgICAgICBjaGVja2VkID0gdW5kZWZpbmVkO1xyXG4gICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgfVxyXG4gICAgICB9XHJcbiAgICB9IGVsc2Uge1xyXG4gICAgICBjaGVja2VkID0gdGhpcy5jaGVja2VkO1xyXG4gICAgfVxyXG5cclxuICAgIHJldHVybiBjaGVja2VkO1xyXG4gIH1cclxufVxyXG4iXX0=