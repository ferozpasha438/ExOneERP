import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthorizeService } from '../../api-authorization/AuthorizeService';
import { ApiService } from '../../services/api.service';
import { NotificationService } from '../../services/notification.service';
import { UtilityService } from '../../services/utility.service';
import { ValidationService } from '../../sharedcomponent/ValidationService';
import { PaginationService } from '../../sharedcomponent/pagination.service';
import { DBOperation } from '../../services/utility.constants';
import { DeleteConfirmDialogComponent } from '../../sharedcomponent/delete-confirm-dialog';
import { AddupdateSurveyorComponent } from '../sharedpages/addupdate-surveyor/addupdate-surveyor.component';
import { AddupdateServicesEnquiryComponent } from '../sharedpages/addupdate-services-enquiry/addupdate-services-enquiry.component';
import { AddupdateEnquiryFormComponent } from '../sharedpages/addupdate-enquiry-form/addupdate-enquiry-form.component';
import { AddSurveyorToEnquiryComponent } from '../sharedpages/add-surveyor-to-enquiry/add-surveyor-to-enquiry.component';
import { ActivatedRoute } from '@angular/router';
import { ParentOptMgtComponent } from '../../sharedcomponent/parentoptmgt.component';
import { OprServicesService } from '../opr-services.service';

@Component({
  selector: 'app-print-enquiries-by-enquiry-number',
  templateUrl: './print-enquiries-by-enquiry-number.component.html'
})
export class PrintEnquiriesByEnquiryNumberComponent
  extends ParentOptMgtComponent  implements OnInit {

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['enquiryID', 'siteCode', 'serviceCode', 'unitCode', 'pricePerUnit', 'serviceQuantity', 'estimatedPrice'/*, 'statusEnquiry'*/];
  data: MatTableDataSource<any>;
  totalItemsCount: number;
  searchValue: string = '';
  sortingOrder: string = 'enquiryID desc';
  isLoading: boolean = false;
  form: FormGroup;
  enquiryNumber: string;
  customerCode: string;
  dateOfEnquiry: number;
  estimateClosingDate: number;
  custAddress: string;
  saleRep: string;
  userName: string;
  remarks: string;
  totalEstPrice: number;
  id: number;
  enquiries: Array<any> = [];
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private apiService: ApiService, private authService: AuthorizeService,
    private notifyService: NotificationService, private utilService: UtilityService, private validationService: ValidationService, public dialog: MatDialog,
    public pageService: PaginationService, private route: ActivatedRoute, private oprService: OprServicesService, public dialogRef: MatDialogRef<PrintEnquiriesByEnquiryNumberComponent>) {
    super(authService);
  }

  ngOnInit(): void {
    
    this.LoadEnquiries();
    this.LoadFormHeaderData();
  }

  refresh() {
    this.searchValue = '';
   
  }



  //onSortOrder(sort: any) {
  //  this.totalItemsCount = 0;
  //  this.sortingOrder = sort.active + ' ' + sort.direction;
  //  this.loadList(0, this.pageService.pageCount, "", this.sortingOrder);
  //}


  //onPageSwitch(event: PageEvent) {
  //  this.pageService.change(event);
  //  this.loadList(event.pageIndex, event.pageSize, "", this.sortingOrder);
  //}

  //private loadList(page: number | undefined, pageCount: number | undefined, query: string | null | undefined, orderBy: string | null | undefined) {
  //  this.isLoading = true;
  //  this.apiService.getPagination('ServiceEnquiries/getSevriceEnquiriesPagedList', this.utilService.getQueryString(page, pageCount, this.enquiryNumber, orderBy)).subscribe(result => {
  //    this.totalItemsCount = 0;

  //    this.data = new MatTableDataSource(result.items);
  //    this.totalItemsCount = result.totalCount;
  //    this.data.paginator = this.paginator;
  //    this.data.sort = this.sort;

  //    setTimeout(() => this.data.sort = this.sort, 2000);
  //    this.isLoading = false;
  //  }, error => this.utilService.ShowApiErrorMessage(error));
  //}


  LoadFormHeaderData() {
    this.isLoading = true;
    this.apiService.getall('ServiceEnquiryForm/getEnquiryFormByEnquiryNumber/'+this.enquiryNumber).subscribe(result => {

      this.customerCode = result.customerCode;
      this.dateOfEnquiry = result.dateOfEnquiry;
      this.estimateClosingDate = result.estimateClosingDate;
      this.custAddress = result.custAddress;
      this.saleRep = result.saleRep;
      this.userName = result.userName;
      this.remarks = result.remarks;
      this.totalEstPrice = result.totalEstPrice;
      if (this.customerCode != '') {

        this.isLoading = true;
        this.apiService.getall('CustomerMaster/getCustomerByCustomerCode/' + result['customerCode']).subscribe(res => {
          if (res != null) {
            this.custAddress = res['custAddress1'];
            this.saleRep = res['custSalesRep'];

            this.isLoading = false;
          }

          else {
            this.custAddress = '';
            this.saleRep = '';

            this.isLoading = false;

          }
        });

      }

      this.isLoading = false;
    }, error => this.utilService.ShowApiErrorMessage(error));
  }
  //applyFilter(searchVal: any) {
  //  const search = searchVal;//.target.value as string;
  //  //if (search && search.length >= 3) {
  //  if (search) {
  //    this.searchValue = search;
  //    this.loadList(0, this.pageService.pageCount, this.searchValue, this.sortingOrder);
  //  }
  //}
  private openDialogManage(id: number = 0, dbops: DBOperation, modalTitle: string, modalBtnTitle: string, component: any) {
    let dialogRef = this.utilService.openCrudDialog(this.dialog, component);
    (dialogRef.componentInstance as any).dbops = dbops;
    (dialogRef.componentInstance as any).modalTitle = modalTitle;
    (dialogRef.componentInstance as any).modalBtnTitle = modalBtnTitle;
    (dialogRef.componentInstance as any).id = id;

    dialogRef.afterClosed().subscribe(res => {
      if (res && res === true)
        this.ngOnInit();
    });
  }

  public create() {
    this.openDialogManage(0, DBOperation.create, 'Adding_New_Enquiry', 'Add', AddupdateEnquiryFormComponent);
  }

  public edit(id: number) {
    this.openDialogManage(id, DBOperation.update, 'Updating_Enquiry', 'Update', AddupdateEnquiryFormComponent);
  }

  public addSuveyorToEnquiry(enquiryID: number) {
    this.openDialogManage(enquiryID, DBOperation.create, 'Adding_Surveyor_To_Enquiry', 'Save', AddSurveyorToEnquiryComponent);

  }


  public delete(id: number) {
    const dialogRef = this.utilService.openDeleteConfirmDialog(this.dialog, DeleteConfirmDialogComponent);
    dialogRef.afterClosed().subscribe(canDelete => {
      if (canDelete && id > 0) {
        this.apiService.delete('ServiceEnquiries', id).subscribe(res => {
          this.utilService.OkMessage();
          this.ngOnInit();
        },
        );
      }
    },
      error => this.utilService.ShowApiErrorMessage(error));
  }
  submit() {

  }



  cahngeEnquiryStatus(enquiryID: number, statusEnquiry: string) {
    this.apiService.post('ServiceEnquiries/changeEnquiryStatus?enquiryID=' + enquiryID + '&statusEnquiry=' + statusEnquiry, null)
      .subscribe(res => {
        this.utilService.OkMessage();
        this.ngOnInit();
      },
        error => {
          console.error(error);
          this.utilService.ShowApiErrorMessage(error);
        });


  }
  printComponent(prinatble: any) {


    const printContent = document.getElementById(prinatble) as HTMLElement;
    const WindowPrt: any = window.open('', '', 'left=0,top=0,width=2000,height=1000,toolbar=0,scrollbars=0,status=0');
    setTimeout(() => {
      WindowPrt.document.write(printContent.innerHTML);
      WindowPrt.document.close();
      WindowPrt.focus();
      WindowPrt.print();
      WindowPrt.close();
    }, 50);







  }
  goBack() {
    window.history.back();

  }
  closeModel() {
    this.dialogRef.close();
  }

  printEnquiryForm() {
    this.openPrint();
  }

  openPrint() {
    const printContent = document.getElementById("printcontainer") as HTMLElement;
    const WindowPrt: any = window.open('', '', 'left=0,top=0,width=2000,height=1000,toolbar=0,scrollbars=0,status=0');
    setTimeout(() => {
      WindowPrt.document.write(printContent.innerHTML);
      WindowPrt.document.close();
      WindowPrt.focus();
      WindowPrt.print();
      WindowPrt.close();
    }, 50);

  }
  LoadEnquiries() {

    this.isLoading = true;
    this.apiService.getall(`ServiceEnquiries/getSevriceEnquiriesByEnquiryNumber/${this.enquiryNumber}`,).subscribe(result => {

      this.enquiries = result as Array<any>;
    }, error => this.utilService.ShowApiErrorMessage(error));
    this.isLoading = false;
  }

  }
