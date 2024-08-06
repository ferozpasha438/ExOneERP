export interface TreeviewSelection {
    checkedItems: TreeviewItem[];
    uncheckedItems: TreeviewItem[];
}
export interface TreeItem {
    text: string;
    value: any;
    disabled?: boolean;
    checked?: boolean;
    collapsed?: boolean;
    children?: TreeItem[];
}
export declare class TreeviewItem {
    private internalDisabled;
    private internalChecked;
    private internalCollapsed;
    private internalChildren;
    text: string;
    value: any;
    constructor(item: TreeItem, autoCorrectChecked?: boolean);
    get checked(): boolean;
    set checked(value: boolean);
    get indeterminate(): boolean;
    setCheckedRecursive(value: boolean): void;
    get disabled(): boolean;
    set disabled(value: boolean);
    get collapsed(): boolean;
    set collapsed(value: boolean);
    setCollapsedRecursive(value: boolean): void;
    get children(): TreeviewItem[];
    set children(value: TreeviewItem[]);
    getSelection(): TreeviewSelection;
    correctChecked(): void;
    private getCorrectChecked;
}
