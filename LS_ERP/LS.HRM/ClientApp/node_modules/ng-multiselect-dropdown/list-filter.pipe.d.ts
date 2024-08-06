import { PipeTransform } from '@angular/core';
import { ListItem } from './multiselect.model';
import * as i0 from "@angular/core";
export declare class ListFilterPipe implements PipeTransform {
    transform(items: ListItem[], filter: ListItem): ListItem[];
    applyFilter(item: ListItem, filter: ListItem): boolean;
    static ɵfac: i0.ɵɵFactoryDeclaration<ListFilterPipe, never>;
    static ɵpipe: i0.ɵɵPipeDeclaration<ListFilterPipe, "multiSelectFilter", false>;
}
