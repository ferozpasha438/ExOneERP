import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { AuthorizeService } from '../api-authorization/AuthorizeService';

@Injectable({
  providedIn: 'root'
})

export class SndServicesService {
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
    });
    return dialogRef;
  }
   openAutoWidthDialog(dialog: MatDialog, component: any) {
    const dialogRef = dialog.open(component, {
      disableClose: true,
      width: "75%",
      height:"auto"
    });
    return dialogRef;
  }
  confirmationDialog(dialog: MatDialog, component: any) {
    const dialogRef = dialog.open(component, {
      disableClose: true,
      width: "auto",
      height:"auto"
      
    });
    return dialogRef;
  }




  fullWindow(dialog: MatDialog, component: any) {
    const dialogRef = dialog.open(component, {
      disableClose: true,
      width: "100%",
      height:"100%"

    });
    return dialogRef;
  }



  getByObj(url: string, objectItem: any):Object {
    this.apiURL = this.authService.ApiEndPoint();
    return this.http.get(`${this.apiURL}/${url}`, objectItem);
  }

}
