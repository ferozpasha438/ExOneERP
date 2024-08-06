import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatDialog } from "@angular/material/dialog";
import { TooltipVisibility } from "@angular/material/tooltip";
import { TreeviewConfig, TreeviewItem } from "ngx-treeview";
import { AuthorizeService } from "../../api-authorization/AuthorizeService";
import { CustomSelectListItem } from "../../models/MenuItemListDto";
import { ApiService } from "../../services/api.service";
import { NotificationService } from "../../services/notification.service";
import { UtilityService } from "../../services/utility.service";
import { DeleteConfirmDialogComponent } from "../../sharedcomponent/delete-confirm-dialog";
import { ParentSystemSetupComponent } from "../../sharedcomponent/parentsystemsetup.component";
import { ConfirmedValidator } from "../../sharedcomponent/spinner.component";
import { ValidationService } from "../../sharedcomponent/ValidationService";

@Component({
  selector: 'app-loginandsecurity',
  templateUrl: './loginandsecurity.component.html',
  styles: [
    `.menuOptionsList {
          min-height: calc(200vh - 0px)!important;
             }
`
  ],
})
export class LoginandsecurityComponent extends ParentSystemSetupComponent implements OnInit {

  id: number = 0;
  isEdit: boolean = false;

  userList: Array<CustomSelectListItem> = [];
  branchList: Array<CustomSelectListItem> = [];
  authUserList: Array<any> = [];
  allAuthUserList: Array<any> = [];

  PermissionUserId: number = 0;

  //selectedNodesPartial: Array<TodoItemFlatNode, TodoItemFlatNode>;

  /** The new item's name */
  newItemName = '';

  // config = {
  //  isShowAllCheckBox: true,
  //  isShowFilter: false,
  //  isShowCollapseExpand: false,
  //  maxHeight: 500
  //}

  items: TreeviewItem[];
  backItems: TreeviewItem[];

  config: TreeviewConfig;

  values: string[] = [];

  form: FormGroup;
  canMultipleShow: boolean = false;

  constructor(private fb: FormBuilder, private apiService: ApiService, private authService: AuthorizeService,
    private notifyService: NotificationService, private validationService: ValidationService, private utilService: UtilityService, public dialog: MatDialog) {
    super(authService);
  }


  onSelectedChange(event: any) {
    console.log(event);
  }
  onFilterChange(value: string): void {
    // console.log('filter:', value);
  }


  ngOnInit(): void {
    this.setForm();
    this.loadAllUsers();
    this.loadAuthUsers();

    this.loadAllBranches();

    this.config = TreeviewConfig.create({
      hasAllCheckBox: true,
      hasFilter: true,
      hasCollapseExpand: false,
      decoupleChildFromParent: false,
      maxHeight: 800,
      //isShowAllCheckBox: true,
      //isShowFilter: false,
      //isShowCollapseExpand: false,

    });

    this.bindAllLinks();

  }


  setForm() {
    //let cDate: IMyDateModel = { isRange: false, singleDate: {}, dateRange: null };
    this.form = this.fb.group({
      'loginId': ['', Validators.required],
      'password': ['', Validators.required],
      'cpassword': ['', Validators.required],
      'userName': ['', Validators.required],
      'userEmail': ['', Validators.compose([Validators.required, Validators.email])],
      'userType': ['', Validators.required],
      'primaryBranch': ['', Validators.required],
      'swpireCardId': ['', Validators.required],
      'imagePath': [''],
      'branchCodes': [''],
      'isActive': ['', Validators.required],
    }, {
      validator: ConfirmedValidator('password', 'cpassword')
    });

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
      this.backItems = this.items;

      //console.log(this.items);
      //console.log(this.backItems);

    });
  }

  loadAllUsers() {


    this.apiService.getall('menuOption/getAllUsersList').subscribe(res => {
      this.userList = res;
    });
  }

  loadAuthUsers() {
    this.apiService.getall('sysLogin').subscribe(res => {
      if (res)
        this.authUserList = res;
      this.allAuthUserList = res;
    });

  }


  searchFilter(value: string) {
    let filterValueLower = value.toLowerCase();
    if (value.trim() === '') {
      this.authUserList = this.allAuthUserList;
    }
    else {
      this.authUserList = this.allAuthUserList.filter((item) =>
        item.loginId.includes(filterValueLower) || item.userName.includes(filterValueLower)
        || item.userEmail?.includes(filterValueLower) || item.swpireCardId?.includes(filterValueLower)
      );
    }
  }
 

  loadAllBranches() {
    this.apiService.getall('branch/getSelectBranchCodeList').subscribe(res => {
      this.branchList = res;
    });
  }

  addPermission() {
    // console.log(this.values);
    const permission_userid = this.PermissionUserId;// ;localStorage.getItem('permission_userid');
    if (permission_userid > 0) {
      if (this.values.length > 0) {

        this.apiService.post('menuOption', { Nodes: this.values, UserId: permission_userid }).subscribe(res => {
          this.utilService.OkMessage();
        },
          error => {
            this.utilService.ShowApiErrorMessage(error);
          });
      }
      else
        this.notifyService.showError('Select Options');
    }
    else
      this.notifyService.showError('Select User');

  }


  loadPermission(event: any) {
    this.apiService.getall(`menuOption/getUserWiseMenuCodes/${event.target.value}`).subscribe(res => {
      this.values = [];
      const list = res as Array<string>;
      const nonZerosList = list.filter(item => !item.match(/^[A-Z][A-Z]00*$/));
      const zerosList = list.filter(item => item.match(/^[A-Z][A-Z]00*$/));

      //this.items = this.items;    
      const button = document.querySelector('.btn-outline-secondary') as HTMLButtonElement;
      button.textContent = 'Select options';
      if (list.length > 0) {

        this.items.forEach(item => {
          item.checked = false;

          if (item.children != null) {
            item.children.forEach(child => {
              child.checked = false;
              item.correctChecked()
              if (child.children != null) {
                child.children.forEach(gChild => {
                  gChild.checked = false;
                  item.correctChecked();
                })
              }
            });
          }
        });

        this.items.forEach(item => {

          if (item.children != null) {
            item.children.forEach(child => {

              zerosList.forEach(zero => {
                if (zero.trim() == child.value.trim()) {
                  item.checked = true;
                  child.checked = true;
                  this.values.push(child.value);
                  item.correctChecked();
                  item.collapsed = true;
                }
              });

              if (child.children != null) {
                child.children.forEach(gChild => {
                  //console.log(gChild.value);
                  nonZerosList.forEach(nzero => {
                    if (nzero.trim() == gChild.value.trim()) {
                      item.checked = true;
                      gChild.checked = true;
                      item.correctChecked();
                      this.values.push(gChild.value);
                      item.collapsed = true;
                    }
                  })

                })
              }
            });
          }
        });

      }
      else {

        this.items.forEach(item => {
          item.checked = false;
          if (item.children != null) {
            item.children.forEach(child => {
              child.checked = false;

              if (child.children != null) {
                child.children.forEach(gChild => {
                  gChild.checked = false;
                  item.collapsed = true;
                })
              }
            });
          }
        });

      }

    });


  }

  checkUserLoginId(evt: any) {
    const bCode = evt.target.value as string;
    if (bCode.trim() !== '' && !this.isEdit) {
      this.apiService.getall(`sysLogin/checkUserLoginId?loginId=${bCode}`).subscribe(res => {
        if (res) {
          this.form.controls['loginId'].setValue('');
          this.notifyService.showError('Alerady exists');
        }
      })
    }
  }

  checkUserName(evt: any) {
    const bCode = evt.target.value as string;
    if (bCode.trim() !== '') {
      this.apiService.getall(`sysLogin/checkUserName?userName=${bCode}`).subscribe(res => {
        if (res) {
          this.form.controls['userName'].setValue('');
        }
      })
    }
  }

  submit() {

    if (this.form.valid) {

      if (this.id > 0)
        this.form.value['id'] = this.id;

      if (!this.canMultipleShow)
        this.form.value['branchCodes'] = null;

      this.apiService.post('sysLogin', this.form.value)
        .subscribe(res => {
          this.id = 0;
          this.utilService.OkMessage();
          this.loadAllUsers();
          this.loadAuthUsers();
          this.setDefaults();
          this.canMultipleShow = false;
        },
          error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
          });
    }
    else
      this.utilService.FillUpFields();
  }

  cancel() {
    this.id = 0;
    this.isEdit = false;
    this.setDefaults();

  }

  setDefaults() {
    this.form.patchValue({
      'loginId': '',
      'password': '',
      'cpassword': '',
      'userName': '',
      'userEmail': '',
      'userType': '',
      'branchCodes': '',
      'primaryBranch': '',
      'swpireCardId': '',
      'imagePath': '',
      'isActive': '',
    });
  }

  public edit(item: any) {
    this.id = parseInt(item['id']);
    this.isEdit = true;
    this.canMultipleShow = true;
    this.form.patchValue(item);
    this.form.patchValue({ 'cpassword': item['password'] });

  }

  public delete(id: number) {
    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete && id > 0) {
        this.apiService.delete('sysLogin', id).subscribe(res => {
          this.loadAuthUsers();
          this.utilService.OkMessage();
        },
        );
      }
    },
      error => this.utilService.ShowApiErrorMessage(error));
  }

  mapBranches() {
    this.canMultipleShow = !this.canMultipleShow;

  }

  ////loadPermissionOld(userId: any) {
  ////  const id = parseInt(userId.target.value);
  ////  // localStorage.setItem('permission_userid', id.toString());
  ////  if (id > 0) {
  ////    this.apiService.get('menuOption/getUserMenuSubLink', id).subscribe(links => {
  ////      let menuCodes = links as Array<string>; //['CC00', 'CE01', 'AA01', 'BA01', 'CA00']
  ////      console.log(menuCodes)
  ////      this.items.forEach(item => {

  ////        //if (menuCodes.includes(item.value as string))
  ////        //  item.checked = true;
  ////        //else
  ////        //  item.checked = false;

  ////        item.children.forEach(cItem => {
  ////          if (menuCodes.includes(cItem.value as string))
  ////            cItem.checked = true;
  ////          else
  ////            cItem.checked = false;

  ////          if (cItem.children && cItem.children.length > 0) {
  ////            cItem.children.forEach(ccItem => {
  ////              if (menuCodes.includes(ccItem.value as string))
  ////                ccItem.checked = true;
  ////              else
  ////                ccItem.checked = false;
  ////            });
  ////          }
  ////        });
  ////      });
  ////    });
  ////  }
  ////  else {

  ////  }
  ////}



}
