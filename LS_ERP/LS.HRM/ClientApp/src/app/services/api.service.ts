import { HttpClient, HttpEvent, HttpHeaders } from '@angular/common/http';
import { Injectable } from "@angular/core";
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { AuthorizeService } from '../api-authorization/AuthorizeService';


@Injectable({
  providedIn: 'root'
})
export class ApiService {
  // Define API
  apiURL: string = ''
  constructor(private http: HttpClient, private authService: AuthorizeService) {
    //this.apiURL = this.authService.ApiEndPoint();
    //authService.SetApiEndPoint(apiendPoint);
    //alert(authService.ApiEndPoint());
  }

  // Http Options
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept-Language': 'en-US',
      'ConnectionString': this.authService.DbConnectionString(),
      'HRMConnectionString': this.authService.DbHRMConnectionString(),
      //'Auth-Token': 'jwtToken'
      'Authorization': `Bearer ${this.authService.getAccessToken()}`,
    })
  }

  // HttpClient API get() method => Fetch anys list
  getall(url: string): Observable<any> {
    this.apiURL = this.authService.ApiEndPoint();
    return this.http.get<any>(`${this.apiURL}/${url}`)
      .pipe(
        retry(1),
        catchError(this.handleError)
      )
  }
  getPagination(url: string, queryParams: string): Observable<any> {
    this.apiURL = this.authService.ApiEndPoint();
    return this.http.get<any>(`${this.apiURL}/${url}?${queryParams}`)
      .pipe(
        retry(1),
        catchError(this.handleError)
      )
  }

  // HttpClient API get() method => Fetch any
  get(url: string, id: number): Observable<any> {
    this.apiURL = this.authService.ApiEndPoint();
    return this.http.get<any>(`${this.apiURL}/${url}/${id}`)
      .pipe(
        retry(1),
        catchError(this.handleError)
      )
  }
  getFinanceUrl(url: string, id: number): Observable<any> {
    this.apiURL = this.authService.getUser()?.finUrl;
    return this.http.get<any>(`${this.apiURL}/${url}/${id}`)
      .pipe(
        retry(1),
        catchError(this.handleError)
      )
  }
  getSchoolUrl(url: string): Observable<any> {
    this.apiURL = this.authService.GetSchoolApiEndPoint();
    return this.http.get<any>(`${this.apiURL}/${url}`)
      .pipe(
        retry(1),
        catchError(this.handleError)
      )
  }
  getOprUrl(url: string): Observable<any> {
    this.apiURL = this.authService.GetOprApiEndPoint();
    return this.http.get<any>(`${this.apiURL}/${url}`)
      .pipe(
        retry(1),
        catchError(this.handleError)
      )
  }
  postOprUrl(url: string,filter:any): Observable<any> {
    this.apiURL = this.authService.GetOprApiEndPoint();
    return this.http.post<any>(`${this.apiURL}/${url}`,filter)
      .pipe(
        retry(1),
        catchError(this.handleError)
      )
  }
  getQueryString(url: string, queryParams: string): Observable<any> {
    this.apiURL = this.authService.ApiEndPoint();
    return this.http.get<any>(`${this.apiURL}/${url}${queryParams}`)
      .pipe(
        retry(1),
        catchError(this.handleError)
      )
  }
  // HttpClient API post() method => Create any
  post(url: string, objectItem: any): Observable<Object> {
    this.apiURL = this.authService.ApiEndPoint();
    return this.http.post(`${this.apiURL}/${url}`, objectItem);
  }
  // HttpClient API post() method => Create any
  postData(url: string, objectItem: any): Observable<any> {
    this.apiURL = this.authService.ApiEndPoint();
    return this.http.post <any>(`${this.apiURL}/${url}`, objectItem);
  }

  // HttpClient API put() method => Update any
  put(url: string, id: number, objectItem: any): Observable<Object> {
    this.apiURL = this.authService.ApiEndPoint();
    return this.http.put(`${this.apiURL}/${url}/${id}`, objectItem);
  }

  // HttpClient API delete() method => Delete any
  delete(url: string, id: number): Observable<Object> {
    this.apiURL = this.authService.ApiEndPoint();
    return this.http.delete(`${this.apiURL}/${url}/${id}`);
  }
  // Error handling 
  handleError(error: any) {
    return throwError(error);
  }
  mobpost(url: string, objectItem: any): Observable<Object> {
    this.apiURL = "http://localhost:56282/api";
    return this.http.post(`${this.apiURL}/${url}`, objectItem);
  }

}

