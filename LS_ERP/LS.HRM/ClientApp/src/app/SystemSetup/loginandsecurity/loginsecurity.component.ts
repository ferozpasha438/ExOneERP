import { SelectionModel } from '@angular/cdk/collections';
import { FlatTreeControl } from '@angular/cdk/tree';
import { AfterViewInit } from '@angular/core';
import { OnInit } from '@angular/core';
import { Component, Injectable } from '@angular/core';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { TreeviewConfig, TreeviewItem } from 'ngx-treeview';

import { BehaviorSubject } from 'rxjs';
import { CustomSelectListItem } from '../../models/MenuItemListDto';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';

/**
 * Node for to-do item
 */
export class TodoItemNode {
  children: TodoItemNode[];
  item: string;
}

/** Flat to-do item node with expandable and level information */
export class TodoItemFlatNode {
  item: string;
  level: number;
  expandable: boolean;
}


/**
 * Checklist database, it can build a tree structured Json object.
 * Each node in Json object represents a to-do item or a category.
 * If a node is a category, it has children items and new items can be added under the category.
 */
@Injectable()
export class ChecklistDatabase {
  dataChange = new BehaviorSubject<TodoItemNode[]>([]);

  get data(): TodoItemNode[] { return this.dataChange.value; }

  constructor(private apiService: ApiService) {
    this.initialize();
  }

  initialize() {
    // Build the tree nodes from Json object. The result is a list of `TodoItemNode` with nested
    //     file node as children.
    let Paths: Object = {};
    let data: TodoItemNode[] = [];
    this.apiService.getall('menuOption').subscribe(res => {
      Paths = res;
      data = this.buildFileTree(Paths, 0);
      this.dataChange.next(data);
    });

    //const data = this.buildFileTree(Paths, 0);

    // Notify the change.
  }

  ////getData(): Object {
  ////  let Paths: Object = {};

  ////  this.apiService.getall('menuOption').subscribe(res => {
  ////    Paths = res;
  ////  });

  ////  //const Groceries: any = {
  ////  //  'ADMINISTRATION': {
  ////  //    'System':
  ////  //      ['Company Setup', 'Branches', 'Login and Security', 'Currency', 'Cities', 'Sequence Generator', 'Taxes'],
  ////  //  },
  ////  //  'FINANCEMANAGEMENT': {
  ////  //    'Finance':
  ////  //      ['F One', 'F Two', 'F Three'],
  ////  //  },
  ////  //};
  ////  return Paths;
  ////}

  /**
   * Build the file structure tree. The `value` is the Json object, or a sub-tree of a Json object.
   * The return value is the list of `TodoItemNode`.
   */
  buildFileTree(obj: { [key: string]: any }, level: number): TodoItemNode[] {
    return Object.keys(obj).reduce<TodoItemNode[]>((accumulator, key) => {
      const value = obj[key];
      const node = new TodoItemNode();
      node.item = key;

      if (value != null) {
        if (typeof value === 'object') {
          node.children = this.buildFileTree(value, level + 1);
        } else {
          node.item = value;
        }
      }

      return accumulator.concat(node);
    }, []);
  }

  /** Add an item to to-do list */
  insertItem(parent: TodoItemNode, name: string) {
    if (parent.children) {
      parent.children.push({ item: name } as TodoItemNode);
      this.dataChange.next(this.data);
    }
  }

  updateItem(node: TodoItemNode, name: string) {
    node.item = name;
    this.dataChange.next(this.data);
  }
}



@Component({
  selector: 'app-loginsecurity',
  templateUrl: './loginsecurity.component.html',
  styles: [
  ],
  providers: [ChecklistDatabase]
})
export class LoginsecurityComponent implements AfterViewInit, OnInit {

  selectedNodes: TodoItemFlatNode[] = [];
  userList: Array<CustomSelectListItem> = [];
  PermissionUserId: number = 0;

  //selectedNodesPartial: Array<TodoItemFlatNode, TodoItemFlatNode>;

  /** Map from flat node to nested node. This helps us finding the nested node to be modified */
  flatNodeMap = new Map<TodoItemFlatNode, TodoItemNode>();

  /** Map from nested node to flattened node. This helps us to keep the same object for selection */
  nestedNodeMap = new Map<TodoItemNode, TodoItemFlatNode>();

  /** A selected parent node to be inserted */
  selectedParent: TodoItemFlatNode | null = null;

  /** The new item's name */
  newItemName = '';

  treeControl: FlatTreeControl<TodoItemFlatNode>;

  treeFlattener: MatTreeFlattener<TodoItemNode, TodoItemFlatNode>;

  dataSource: MatTreeFlatDataSource<TodoItemNode, TodoItemFlatNode>;

  /** The selection for checklist */
  checklistSelection = new SelectionModel<TodoItemFlatNode>(true /* multiple */);

  // config = {
  //  isShowAllCheckBox: true,
  //  isShowFilter: false,
  //  isShowCollapseExpand: false,
  //  maxHeight: 500
  //}

  items: TreeviewItem[];

  config: TreeviewConfig;



  values: number[] = [];

  constructor(private _database: ChecklistDatabase, private apiService: ApiService, private notifyService: NotificationService, private utilService: UtilityService) {
    this.treeFlattener = new MatTreeFlattener(this.transformer, this.getLevel,
      this.isExpandable, this.getChildren);
    this.treeControl = new FlatTreeControl<TodoItemFlatNode>(this.getLevel, this.isExpandable);
    this.dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

    _database.dataChange.subscribe(data => {
      this.dataSource.data = data;
    });
  }



  onSelectedChange(event: any) {
    console.log(event);
  }
  onFilterChange(value: string): void {
    console.log('filter:', value);
  }

  getLevel = (node: TodoItemFlatNode) => node.level;

  isExpandable = (node: TodoItemFlatNode) => node.expandable;

  getChildren = (node: TodoItemNode): TodoItemNode[] => node.children;

  hasChild = (_: number, _nodeData: TodoItemFlatNode) => _nodeData.expandable;

  hasNoContent = (_: number, _nodeData: TodoItemFlatNode) => _nodeData.item === '';

  /**
   * Transformer to convert nested node to flat node. Record the nodes in maps for later use.
   */

  transformer = (node: TodoItemNode, level: number) => {
    const existingNode = this.nestedNodeMap.get(node);
    const flatNode = existingNode && existingNode.item === node.item
      ? existingNode
      : new TodoItemFlatNode();
    flatNode.item = node.item;
    flatNode.level = level;
    flatNode.expandable = !!node.children?.length;
    this.flatNodeMap.set(flatNode, node);
    this.nestedNodeMap.set(node, flatNode);
    return flatNode;
  }

  /** Whether all the descendants of the node are selected. */
  descendantsAllSelected(node: TodoItemFlatNode): boolean {
    const descendants = this.treeControl.getDescendants(node);
    const descAllSelected = descendants.length > 0 && descendants.every(child => {
      return this.checklistSelection.isSelected(child);
    });
    return descAllSelected;
  }

  /** Whether part of the descendants are selected */
  descendantsPartiallySelected(node: TodoItemFlatNode): boolean {
    const descendants = this.treeControl.getDescendants(node);
    const result = descendants.some(child => this.checklistSelection.isSelected(child));
    return result && !this.descendantsAllSelected(node);
  }

  /** Toggle the to-do item selection. Select/deselect all the descendants node */
  todoItemSelectionToggle(node: TodoItemFlatNode): void {
    this.checklistSelection.toggle(node);
    const descendants = this.treeControl.getDescendants(node);
    this.checklistSelection.isSelected(node)
      ? this.checklistSelection.select(...descendants)
      : this.checklistSelection.deselect(...descendants);

    // Force update for the parent
    descendants.forEach(child => this.checklistSelection.isSelected(child));
    this.checkAllParentsSelection(node);

    const partialSelection = this.treeControl.dataNodes.filter(x =>
      this.descendantsPartiallySelected(x));
    //console.log(this.checklistSelection.selected + ' ' + partialSelection);

    //this.checklistSelection.selected.forEach(e => e.item)
    //partialSelection.forEach(e => e.item);

    this.selectedNodes = this.checklistSelection.selected;
  }

  /** Toggle a leaf to-do item selection. Check all the parents to see if they changed */
  todoLeafItemSelectionToggle(node: TodoItemFlatNode): void {
    this.checklistSelection.toggle(node);
    this.checkAllParentsSelection(node);

    const partialSelection = this.treeControl.dataNodes.filter(x =>
      this.descendantsPartiallySelected(x));

    // console.log(this.checklistSelection.selected + ' ' + partialSelection);
    this.selectedNodes = this.checklistSelection.selected;
  }

  /* Checks all the parents when a leaf node is selected/unselected */
  checkAllParentsSelection(node: TodoItemFlatNode): void {
    let parent: TodoItemFlatNode | null = this.getParentNode(node);
    while (parent) {
      this.checkRootNodeSelection(parent);
      parent = this.getParentNode(parent);
    }
  }

  /** Check root node checked state and change it accordingly */
  checkRootNodeSelection(node: TodoItemFlatNode): void {
    const nodeSelected = this.checklistSelection.isSelected(node);
    const descendants = this.treeControl.getDescendants(node);
    const descAllSelected = descendants.length > 0 && descendants.every(child => {
      return this.checklistSelection.isSelected(child);
    });
    if (nodeSelected && !descAllSelected) {
      this.checklistSelection.deselect(node);
    } else if (!nodeSelected && descAllSelected) {
      this.checklistSelection.select(node);
    }
  }

  /* Get the parent node of a node */
  getParentNode(node: TodoItemFlatNode): TodoItemFlatNode | null {
    const currentLevel = this.getLevel(node);

    if (currentLevel < 1) {
      return null;
    }

    const startIndex = this.treeControl.dataNodes.indexOf(node) - 1;

    for (let i = startIndex; i >= 0; i--) {
      const currentNode = this.treeControl.dataNodes[i];

      if (this.getLevel(currentNode) < currentLevel) {
        return currentNode;
      }
    }
    return null;
  }

  ngAfterViewInit() {
  }

  //preSelectData(userId: any): Array<string> {
  //  //['Company Setup', 'Branches', 'Login and Security', 'Currency', 'F Three'];// 'Cities', 'Sequence Generator', 'Taxes'];
  //  let links: Array<string> = [];
  //  this.apiService.get('menuOption/getUserMenuSubLink', userId).subscribe(res => {
  //    links = res;
  //  });
  //  return links;
  //}

  ngOnInit(): void {
    this.loadAllUsers();

    this.config = TreeviewConfig.create({
      hasAllCheckBox: true,
      hasFilter: true,
      hasCollapseExpand: false,
      decoupleChildFromParent: false,
      maxHeight: 1600,
      //isShowAllCheckBox: true,
      //isShowFilter: false,
      //isShowCollapseExpand: false,

    });

    //this.items = this.getAllLinks();
    this.bindAllLinks();

  }
  bindAllLinks() {
    const treeviewItems: TreeviewItem[] = [];
    this.apiService.getall('menuOption/getNgLinks').subscribe(res => {
      //this.items = res as TreeviewItem[]
      let tVList = res as Array<any>;
      tVList.forEach(item => {
        const tvItem = new TreeviewItem(item)
        treeviewItems.push(tvItem);
      });
      this.items = treeviewItems;
    });
  }

  getAllLinks(): TreeviewItem[] {

    let links1 = new TreeviewItem({
      text: 'IT', value: 9, children: [
        //{
        //  text: 'Programming', value: 91, children: [{
        //    text: 'Frontend', value: 911, children: [
        //      { text: 'Angular 1', value: 9111 },
        //      { text: 'Angular 2', value: 9112 },
        //      { text: 'ReactJS', value: 9113 }
        //    ]
        //  }, {
        //    text: 'Backend', value: 912, children: [
        //      { text: 'C#', value: 9121 },
        //      { text: 'Java', value: 9122 },
        //      { text: 'Python', value: 9123, checked: false }
        //    ]
        //  }]
        //},
        {
          text: 'Networking', value: 92, children: [
            { text: 'Internet', value: 921, checked: false },
            { text: 'Security', value: 922, checked: false }
          ]
        }
      ], checked: false, collapsed: true, disabled: false
    });
    let links2 = new TreeviewItem({
      text: 'Networking', value: 9, children: [
        {
          text: 'Numbers', value: 92, children: [
            { text: 'One', value: 921, checked: false },
            { text: 'Two', value: 922, checked: false }
          ]
        },
        {
          text: 'More', value: '111', checked: false
        }
      ], checked: false, collapsed: true, disabled: false
    });
    return [links1, links2];
  }

  loadAllUsers() {
    this.apiService.getall('menuOption/getPermissionUsers').subscribe(res => {
      this.userList = res;
    });
  }

  //loadAllPermissions(userId: any) {
  //  let links = this.preSelectData(userId);
  //  console.log(links)
  //  if (links) {
  //  console.log(links)
  //    for (let i = 0; i < this.treeControl.dataNodes.length; i++) {
  //      let noteItem = this.treeControl.dataNodes[i].item;
  //      //console.log(noteItem + ' ' + key);

  //      if (links.includes(noteItem)) {
  //        //console.log('This is From Text' + this.preSelectData()[key]);
  //        this.todoItemSelectionToggle(this.treeControl.dataNodes[i]);
  //        this.treeControl.expand(this.treeControl.dataNodes[i])

  //        //this.treeControl.expandAll();//this.treeControl.dataNodes[i])

  //        //if (this.treeControl.dataNodes[i].item == 'Groceries For the Test') {
  //        //  this.todoItemSelectionToggle(this.treeControl.dataNodes[i]);
  //        //  this.treeControl.expand(this.treeControl.dataNodes[i])
  //        //}
  //        //if (this.treeControl.dataNodes[i].item == 'Second Level Menu') {
  //        //  this.treeControl.expand(this.treeControl.dataNodes[i])
  //        //}
  //        //if (this.treeControl.dataNodes[i].item == 'Berries') {
  //        //  this.treeControl.expand(this.treeControl.dataNodes[i])
  //        //}
  //      }
  //    }
  //  }

  //}

  addPermission() {
    const permission_userid = this.PermissionUserId;// ;localStorage.getItem('permission_userid');
    if (permission_userid > 0) {
      const links = this.selectedNodes.filter(node => node.expandable == false);
      this.apiService.post('menuOption', { Nodes: links, UserId: permission_userid }).subscribe(res => {
        //        localStorage.removeItem('permission_userid')
        this.utilService.OkMessage();
      },
        error => {
          this.utilService.ShowApiErrorMessage(error);
        });
      //console.log(this.selectedNodes.filter(node => node.expandable == false));
    }
    else
      this.notifyService.showError('Select User');
  }

  loadPermission(userId: any) {
    const id = parseInt(userId.target.value);
    // localStorage.setItem('permission_userid', id.toString());
    if (id > 0) {
      for (let i = 0; i < this.treeControl.dataNodes.length; i++) {
        this.checklistSelection.deselect(this.treeControl.dataNodes[i]);
      }
      this.apiService.get('menuOption/getUserMenuSubLink', id).subscribe(links => {

        for (let i = 0; i < this.treeControl.dataNodes.length; i++) {
          let noteItem = this.treeControl.dataNodes[i].item;

          if (links.includes(noteItem)) {
            this.todoItemSelectionToggle(this.treeControl.dataNodes[i]);
            this.treeControl.expand(this.treeControl.dataNodes[i])

            this.treeControl.expandAll();//(this.treeControl.dataNodes[i])
          }
        }
      });
    }
    else {
      for (let i = 0; i < this.treeControl.dataNodes.length; i++) {
        this.checklistSelection.deselect(this.treeControl.dataNodes[i]);
        //this.treeControl.collapseAll();
      }
    }
  }

  /** Select the category so we can insert the new item. */
  addNewItem(node: TodoItemFlatNode) {
    const parentNode = this.flatNodeMap.get(node);
    this._database.insertItem(parentNode!, '');
    this.treeControl.expand(node);
  }

  /** Save the node to database */
  saveNode(node: TodoItemFlatNode, itemValue: string) {
    const nestedNode = this.flatNodeMap.get(node);
    this._database.updateItem(nestedNode!, itemValue);
  }

}
