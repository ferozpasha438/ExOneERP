import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable, throwError } from 'rxjs';
import { AuthorizeService } from '../api-authorization/AuthorizeService';
import { catchError, retry } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})

export class OprServicesService {
  apiURL: string = ''
  constructor(private http: HttpClient, private authService: AuthorizeService) { }

  private enquiryNumber: any;

  set EnquiryNumber(enquiryNumber) {
    this.enquiryNumber = enquiryNumber;
  }

  get EnquiryNumber() {
    return this.enquiryNumber;
  }

  private enquiryID: any;
  set EnquiryID(enquiryID) {
    this.enquiryID = enquiryID;
  }


  get EnquiryID() {
    return this.enquiryID;
  }

  private input: any;
  set Input(input) {
    this.input = input;
  }

  get Input() {
    return this.input;
  }

  private surveyorCode: any;
  set SurveyorCode(surveyorCode) {
    this.surveyorCode = surveyorCode;
  }


  get SurveyorCode() {
    return this.surveyorCode;
  }










  /*to verify whether code exist or not */
  verifyCode(url: string): Observable<any> {
    this.apiURL = this.authService.ApiEndPoint();
    return this.http.get<any>(`${this.apiURL}/${url}`);
  }







  openApprovalDialog(dialog: MatDialog, component: any) {
    const dialogRef = dialog.open(component, {
      disableClose: true,
    //  direction: this.isArabic() ? "rtl" : "ltr"
    });
    return dialogRef;
  }
   openAutoWidthDialog(dialog: MatDialog, component: any) {
    const dialogRef = dialog.open(component, {
      disableClose: true,
      width: "auto",
    //  direction: this.isArabic() ? "rtl" : "ltr"
    });
    return dialogRef;
  }
  openAutoWidthNHeightDialog(dialog: MatDialog, component: any) {
    const dialogRef = dialog.open(component, {
      disableClose: true,
      width: "auto",
      height: "auto",
     // direction: this.isArabic() ? "rtl" : "ltr"
    });
    return dialogRef;
  }
  openAutoHeightWidthDialog(dialog: MatDialog, component: any) {
    const dialogRef = dialog.open(component, {
      disableClose: true,
      width: "100%",
      height: "100%",
      direction: this.isArabic() ? "rtl" : "ltr"
    });
    return dialogRef;
  }
  openFullWidthDialog(dialog: MatDialog, component: any) {
    const dialogRef = dialog.open(component, {
      disableClose: true,
      width: "100%",
      height: "auto",
      direction: this.isArabic() ? "rtl" : "ltr"
    });
    return dialogRef;
  }
  confirmationDialog(dialog: MatDialog, component: any) {
    const dialogRef = dialog.open(component, {
      disableClose: true,
      width: "auto",
      height: "auto",
     // direction: this.isArabic() ? "rtl" : "ltr"
      
    });
    return dialogRef;
  }

  fullWindow(dialog: MatDialog, component: any) {
    const dialogRef = dialog.open(component, {
      disableClose: true,
      width: "100%",
      height: "100%",
      direction: this.isArabic() ? "rtl" : "ltr"
    });
    return dialogRef;
  }


  

  //openFullCrudDialog(dialog: MatDialog, component: any, width: string = '80%') {
  //  const widthNumber = width.includes('%') ? width.split('%')[0] : width;
  //  const dialogRef = dialog.open(component, {
  //    disableClose: true,
  //    width: `${widthNumber}%`,
  //    maxWidth: `${widthNumber}vw`,
  //    height: '90%',
  //    direction: this.isArabic() ? "rtl" : "ltr"
  //  });
  //  return dialogRef;
  //}
  isArabic() {
    return this.selectedLanguage() == 'ar';
  }
  getByObj(url: string, objectItem: any):Object {
    this.apiURL = this.authService.GetOprApiEndPoint();
    return this.http.get(`${this.apiURL}/${url}`, objectItem);
  }
  selectedLanguage() {
    return localStorage.getItem('language') ?? 'en-US'
  }


  getPaginationWithFilter(url: string, queryParams: string, filterInput: any): Observable <any> {
    this.apiURL = this.authService.GetOprApiEndPoint();
    return this.http.post(`${this.apiURL}/${url}?${queryParams}`, filterInput).pipe(
      retry(1),
      catchError(this.handleError)
    );
  }
  getWithFilter(url: string, filterInput: any): Observable<any> {
      this.apiURL = this.authService.GetOprApiEndPoint();
    return this.http.post(`${this.apiURL}/${url}`, filterInput).pipe(
      retry(1),
      catchError(this.handleError)
    );
  }

  handleError(error: any) {
    return throwError(error);
  }
}
