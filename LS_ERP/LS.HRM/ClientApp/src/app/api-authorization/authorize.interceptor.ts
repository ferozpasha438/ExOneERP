import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { mergeMap } from 'rxjs/operators';
import { AuthorizeService } from './AuthorizeService';

@Injectable({
  providedIn: 'root'
})
export class AuthorizeInterceptor implements HttpInterceptor {
  constructor(private authService: AuthorizeService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return this.processRequestWithToken(req, next);
  }

  // Checks if there is an access_token available in the authorize service
  // and adds it to the request in case it's targeted at the same origin as the
  // single page application.
  private processRequestWithToken(req: HttpRequest<any>, next: HttpHandler) {
    let token = this.authService.getAccessToken();
    let dbConnectionString = this.authService.DbConnectionString();
    let dbHRMConnectionString = this.authService.DbHRMConnectionString();
    if (dbConnectionString && dbConnectionString !== '') {//&& this.isSameOriginUrl(req)) {
      //req = req.clone({
      //  setHeaders: {
      //    Authorization: `Bearer ${token}`,
      //    Accept-Language:''
      //  }

      req = req.clone({
        headers: new HttpHeaders({
          // 'Content-Type': 'application/json',
          'Accept-Language': this.authService.selectedLanguage(),
          'ConnectionString': this.authService.DbConnectionString(),          
         'HRMConnectionString': this.authService.DbHRMConnectionString(),
          'ModuleCodes': this.authService.ModuleCodes(),
          //'Auth-Token': 'jwtToken'
          'Authorization': `Bearer ${token}`,
        })
      });
      //alert('is calling from the ');
    }

    //console.log({
    //  // 'Content-Type': 'application/json',
    //  'Accept-Language': this.authService.selectedLanguage(),
    //  'ConnectionString': this.authService.DbConnectionString(),
    //  'ModuleCodes': this.authService.ModuleCodes(),
    //  //'Auth-Token': 'jwtToken'
    //  'Authorization': `Bearer ${token}`,
    //});
    return next.handle(req);
  }

  //private isSameOriginUrl(req: any) {
  //  // It's an absolute url with the same origin.
  //  if (req.url.startsWith(`${window.location.origin}/`)) {
  //    return true;
  //  }

  //  // It's a protocol relative url with the same origin.
  //  // For example: //www.example.com/api/Products
  //  if (req.url.startsWith(`//${window.location.host}/`)) {
  //    return true;
  //  }

  //  // It's a relative url like /api/Products
  //  if (/^\/[^\/].*/.test(req.url)) {
  //    return true;
  //  }

  //  // It's an absolute or protocol relative url that
  //  // doesn't have the same origin.
  //  return false;
  //}
}
