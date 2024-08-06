import { HttpClient } from '@angular/common/http';
import { I18nMeta } from '@angular/compiler/src/render3/view/i18n/meta';
import { Component, OnInit } from '@angular/core';
import { Data, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AuthorizeService } from '../api-authorization/AuthorizeService';
import { CINServerMetaDataDto } from '../models/CINServerMetaDataDto';
import { ApiService } from '../services/api.service';
import { NotificationService } from '../services/notification.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent implements OnInit {
  //limesBasket$: BehaviorSubject<boolean>;
  userName: string = 'User';
  isAuthenticated: boolean = false;
  SelectedLanguage: string = 'en';
  cinNumber: string = '';
  logoURL: string = '';
  notificationList: Array<any> = [];
  notificationCount: number = 0;
  notificationLink: string = '';

  constructor(private authService: AuthorizeService, private notifyService: NotificationService,
    private http: HttpClient, private router: Router, private translate: TranslateService, private apiService: ApiService)
  {
    translate.addLangs(['en', 'ar']);
    translate.setDefaultLang('en');
    translate.use('en');
  }
  ngOnInit(): void {
    let linkElement = this.htmlLinkElement()
    linkElement.setAttribute('href', 'assets/new-styles.css');
    let lan = localStorage.getItem('language');
    this.logoURL = localStorage.getItem('logoURL') as string;

    if (lan) {
      this.SelectedLanguage = lan;
      linkElement.setAttribute('href', this.getLanFile(lan));
      this.translate.use(lan);
      document.body.setAttribute('dir', lan === "ar" ? "rtl" : "ltr");
      document.body.setAttribute('lang', lan);
    }

    document.head.appendChild(linkElement);

    this.authService.getAuthorize().subscribe(isAuth => {
      this.isAuthenticated = isAuth;
    });
    this.userName = this.authService.getUserName();
    this.cinNumber = this.authService.getUser().cinNumber;
    this.isAuthenticated = this.authService.isAuthenticated();

    if (this.isAuthenticated) {

      ////let distance = 0;
      ////let isConfirmed = false;
      ////let isExtended = false;      
      ////let momentOfTime = new Date();
      ////let timeSpan = 30 * 60 * 1000;
      ////let countDownDate = momentOfTime.setTime(momentOfTime.getTime() + timeSpan);

      ////let timerId = setInterval(() => {
      ////  let now = new Date().getTime();
      ////  distance = countDownDate - now;
      ////  //console.log(distance);
      ////  if (distance < (27 * 60 * 1000)) {
      ////    if (isConfirmed === false) {
      ////      isExtended = confirm("Do you want to extend sesssion");
      ////      isConfirmed = true;

      ////      if (isExtended) {              
      ////        location.reload(true);
      ////      }            
      ////    }
      ////  }

      ////  if (distance < 0) {          
      ////    setTimeout(() => { clearInterval(timerId); this.LogOut(); }, 0);
      ////  }

      ////}, 900);

      this.getNotificationData();
    }
   
  }

  getNotificationData() {
    this.apiService.getSchoolUrl('WebNotification/GetWebTopNotifications')
      .subscribe(res => {
      if (res) {
        this.notificationList = res;
        this.notificationCount = res.length;
        this.notificationLink = 'school/allnotifications';
      }
    });
  }
  htmlLinkElement(): HTMLLinkElement {
    var linkElement = document.createElement('link');
    linkElement.setAttribute('rel', 'stylesheet');
    linkElement.setAttribute('type', 'text/css');
    linkElement.setAttribute('id', 'dynamicCss');


    return linkElement;
  }

  getLanFile = (lan: string): string => lan === 'ar' ? 'assets/new-styles-ar.css' : 'assets/new-styles.css';

  selectLan(lan: any) {
    const selectedLan = lan.target.value;
    localStorage.setItem('language', selectedLan);
    window.location.href = window.location.href;
  }

  LogOut() {
    let user = this.authService.getUser();
    localStorage.clear();
    localStorage.removeItem("accessToken");
    this.authService.setAuthorize(false);
    this.notifyService.showSuccess("Logout Successful");
    location.replace('');

    //this.authService.SetApiEndPoint(this.authService.GetSystemSetupApiEndPoint());

    this.http.post(`${this.authService.GetSystemSetupApiEndPoint()}/validatecin/logout`, { cinNumber: user.cinNumber })
      .subscribe(resonse => {
        let res = resonse as any;
        this.authService.setAuthorize(false);
        localStorage.clear();
        //this.router.navigateByUrl('/');
        this.notifyService.showSuccess(res.data);
        //window.location.href = "/";
        location.replace('');
      });
  }

  //se77lectLan(lan: any) {

  //var linkElement = document.createElement('link');
  //linkElement.setAttribute('rel', 'stylesheet');
  //linkElement.setAttribute('type', 'text/css');
  //// linkElement.setAttribute('id', 'dynamicCss');
  //linkElement.setAttribute('href', 'assets/new-styles.css');
  //document.head.appendChild(linkElement);


  //  const selectedLan = lan.target.value;
  //  localStorage.setItem('language', selectedLan);
  //  window.location.href = window.location.href;


  //  //this.translate.use(selectedLan);
  //  //var linkElement = this.htmlLinkElement();
  //  //var linkElement = document.createElement('link');
  //  //linkElement.setAttribute('rel', 'stylesheet');
  //  //linkElement.setAttribute('type', 'text/css');    

  //  //if (document.getElementById('dynamicCss') !== null) {
  //  //  document.getElementById('dynamicCss')?.setAttribute("href", this.getLanFile(selectedLan));
  //  //}
  //  //else {
  //  //  linkElement.setAttribute('href', this.getLanFile(selectedLan));
  //  //  document.head.appendChild(linkElement);
  //  //}
  //  //this.authService.setLanguageChange(selectedLan)
  //}


  //isExpanded = false;

  //collapse() {
  //  this.isExpanded = false;
  //}

  //toggle() {
  //  this.isExpanded = !this.isExpanded;
  //}

  //selectLan(lan: any) {
  //  this.translate.use(lan.target.value);
  //  var linkElement = document.createElement('link');
  //  linkElement.setAttribute('rel', 'stylesheet');
  //  linkElement.setAttribute('type', 'text/css');
  //  linkElement.setAttribute('id', 'dynamicCss');
  //  let arStyle = 'assets/new-styles-ar.css', enStyle = 'assets/new-styles.css';

  //  if (lan.target.value === 'ar') {
  //    linkElement.setAttribute('href', arStyle);
  //    //linkElement.setAttribute('href', '/css/new-styles-ar.css');
  //    console.log('ar')
  //  }
  //  else {
  //    //linkElement.setAttribute('href', '/css/new-styles-ar.css');
  //    linkElement.setAttribute('href', enStyle);
  //    console.log('en')
  //  }

  //  //  document.removeChild('dynamicCss')?.setAttribute('hre';
  //  if (document.getElementById('dynamicCss') !== null) {
  //    alert('p')
  //    document.getElementById('dynamicCss')?.setAttribute("href", lan.target.value === 'ar' ? arStyle : enStyle);
  //  }
  //  else
  //    document.head.appendChild(linkElement);

  //}
}
