import { Injectable } from '@angular/core';
import { BehaviorSubject, concat, from, observable, Observable } from 'rxjs';
import { filter, map, mergeMap, take, tap } from 'rxjs/operators';
import { CINServerMetaDataDto } from '../models/CINServerMetaDataDto';
import { default as data } from "../../assets/i18n/apiuri.json";

@Injectable({
  providedIn: 'root'
})
export class AuthorizeService {
  // By default pop ups are disabled because they don't work properly on Edge.
  // If you want to enable pop up authentication simply set this flag to false. 
  //private languageSubject: BehaviorSubject<string> = new BehaviorSubject<string>('');
  private userSubject: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private isSubmittingSubject: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  //public setLanguageChange(language: string) {
  //  this.languageSubject.next(language);
  //}

  public setAuthorize(isAuth: boolean) {
    this.userSubject.next(isAuth);
  }

  public isAuthenticated(): boolean {
    return this.getUserName() !== '' ? true : false;
  }

  //public getLanguageChange(): BehaviorSubject<string> {
  //  return this.languageSubject;
  //}

  public getAuthorize(): BehaviorSubject<boolean> {
    return this.userSubject;
  }
  public getUserName(): string {
    return localStorage.getItem('userName') ?? '';
  }

  public getUser(): CINServerMetaDataDto {
    const user = localStorage.getItem('metaData') as string;
    return JSON.parse(user) as CINServerMetaDataDto;
  }

  public getAccessToken(): string {
    return localStorage.getItem('accessToken') ?? '';
    //return from('')
    //  .pipe(mergeMap(() => from(Observable<string>(''))),
    //    map(user => ''));

    //return from(this.ensureUserManagerInitialized())
    //  .pipe(mergeMap(() => from(this.userManager.getUser())),
    //    map(user => user && user.access_token));
  }


  public SetApiEndPoint(apiEndpoint: string) {
    //localStorage.removeItem('apiEndpoint');
    localStorage.setItem('apiEndpoint', apiEndpoint);
  };
  public ApiEndPoint(): string {
    return localStorage.getItem('apiEndpoint') ?? ''
  };
  public GetSystemSetupApiEndPoint(): string {
    return localStorage.getItem('setupapi') ?? ''
  };
  public GetSchoolApiEndPoint(): string {
    return data.schoolapiurl??''
  };
  public GetOprApiEndPoint(): string {
    return localStorage.getItem('oprEndPoint') ?? ''
  };

  public DbConnectionString(): string {
    return localStorage.getItem('dbConnectionString') ?? ''
  };
public DbHRMConnectionString(): string {
    return localStorage.getItem('dbHRMConnectionString') ?? ''
  };
  public ModuleCodes(): string {
    return localStorage.getItem('moduleCodes') ?? ''
  };
 
  public SetSubmitting(isSubmitting: boolean) {
    this.isSubmittingSubject.next(isSubmitting);
  }
  public IsSubmitting(): BehaviorSubject<boolean> {
    return this.isSubmittingSubject;
  }

  selectedLanguage() {
    return localStorage.getItem('language') ?? 'en-US'
  }

  isArabic() {
    return this.selectedLanguage() == 'ar';
  }

  Lang = (): string => this.selectedLanguage();

  //public isAuthenticated(): Observable<boolean> {
  //  return this.getUser().pipe(map(u => !!u));
  //}

  //public getUser(): Observable<IUser | null> {
  //  return concat(
  //    this.userSubject.pipe(take(1), filter(u => !!u)),
  //    this.getUserFromStorage().pipe(filter(u => !!u), tap(u => this.userSubject.next(u))),
  //    this.userSubject.asObservable());
  //}

  //public getAccessToken(): Observable<string> {
  //  return from(this.ensureUserManagerInitialized())
  //    .pipe(mergeMap(() => from(this.userManager.getUser())),
  //      map(user => user && user.access_token));
  //}

  // We try to authenticate the user in three different ways:
  // 1) We try to see if we can authenticate the user silently. This happens
  //    when the user is already logged in on the IdP and is done using a hidden iframe
  //    on the client.
  // 2) We try to authenticate the user using a PopUp Window. This might fail if there is a
  //    Pop-Up blocker or the user has disabled PopUps.
  // 3) If the two methods above fail, we redirect the browser to the IdP to perform a traditional
  //    redirect flow.

  public async signOut(state: any) {

  }

  //private getUserFromStorage(): Observable<IUser> {
  //  return from(this.ensureUserManagerInitialized())
  //    .pipe(
  //      mergeMap(() => this.userManager.getUser()),
  //      map(u => u && u.profile));
  //}
}
