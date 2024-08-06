import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthorizeService } from '../api-authorization/AuthorizeService';
import { CINServerMetaDataDto } from '../models/CINServerMetaDataDto';
import { GetSideMenuOptionListDto } from '../models/MenuItemListDto';
import { ApiService } from '../services/api.service';
import { NotificationService } from '../services/notification.service';
import { UtilityService } from '../services/utility.service';
import { ParentSystemSetupComponent } from '../sharedcomponent/parentsystemsetup.component';
import { default as data } from "../../assets/i18n/apiuri.json";

@Component({
  selector: 'app-login-component',
  templateUrl: './login.component.html'
})
export class LoginComponent extends ParentSystemSetupComponent implements OnInit {

  cinloginForm!: FormGroup;
  loginForm!: FormGroup;
  dbconnectionString: string = '';
  dbHRMconnectionString: string = '';
  apiEndPoint: string = '';
  //moduleCodes: string = '';
  //cinNumber: string = '';
  isCinForm: boolean = true;
  apiUri: string = data.financeurl;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router,// private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private notifyService: NotificationService) {
    super(authService);
    this.setForm();
  }


  ngOnInit(): void {
    //console.log(this.authService.isAuthenticated())

    //if (this.authService.isAuthenticated())
    //  window.location.href = "dashboard";
    //else

    localStorage.clear();
  }

  setForm() {
    this.cinloginForm = this.fb.group({
      //'cinNumber': ['CIN0005', Validators.required]
      'cinNumber': ['Saher', Validators.required]
    });

    this.loginForm = this.fb.group({
      'cinNumber': '',
      'userName': ['Admin', Validators.required],
      'password': ['admin@123', Validators.required]
    });
  }

  //setForm() {
  //  this.cinloginForm = this.fb.group({      
  //    'cinNumber': ['', Validators.required]      
  //  });

  //  this.loginForm = this.fb.group({
  //    'cinNumber': '',
  //    'userName': ['', Validators.required],
  //    'password': ['', Validators.required]     
  //  });
  //}

  cinlogin() {
    if (this.cinloginForm.valid) {
      this.authService.SetSubmitting(true);
      this.http.post(`${this.apiUri}/validatecin`, this.cinloginForm.value)
        .subscribe(data => {
          // console.log(data)
          let metaData = (data as CINServerMetaDataDto);
          this.dbconnectionString = metaData.dbConnectionString;
          this.dbHRMconnectionString = metaData.utlUrl;

          this.apiEndPoint = metaData.admUrl;

          localStorage.setItem('setupapi', metaData.admUrl);
          localStorage.setItem('apiEndpoint', metaData.admUrl);
          localStorage.setItem('oprEndPoint', metaData.opmUrl);
          localStorage.setItem('dbConnectionString', this.dbconnectionString);


          localStorage.setItem('dbHRMConnectionString', this.dbHRMconnectionString);

          localStorage.setItem('moduleCodes', metaData.moduleCodes);
          this.loginForm.controls['cinNumber'].setValue(metaData.cinNumber);
          localStorage.setItem('metaData', JSON.stringify(metaData));
          this.isCinForm = false;
        },
          error => {
            this.utilService.ShowApiErrorMessage(error);
          }
        );
      this.authService.SetSubmitting(false);
    }
    else
      this.utilService.FillUpFields();
  }

  ////login() {
  ////  if (this.loginForm.valid) {
  ////    //alert(ApiEndPoint);
  ////    //const headers = new HttpHeaders()
  ////    //  .set('ConnectionString', this.dbconnectionString);
  ////    this.http.post(`${this.apiEndPoint}/login`, this.loginForm.value)//, { 'headers': headers })
  ////      .subscribe(data => {
  ////        localStorage.setItem('userName', this.loginForm.controls['userName'].value)
  ////        localStorage.setItem('menuItems', JSON.stringify(data as Array<MenuItemListDto>));
  ////        this.authService.setAuthorize(true);
  ////        this.notifyService.showSuccess('Login Successful');
  ////        this.router.navigateByUrl('dashboard');
  ////        //window.location.href = "/dashboard";
  ////      },
  ////        error => {
  ////          this.utilService.ShowApiErrorMessage(error);
  ////        });
  ////  }
  ////}

  login() {
    if (this.loginForm.valid) {
      this.authService.SetSubmitting(true);
      this.http.post(`${this.apiEndPoint}/login`, this.loginForm.value)
        .subscribe((res: any) => {
          const menuItems = res['userSideMenuList'];
          const token = res['token'];
          const logoURL = res['logoURL'];
          this.notifyService.showSuccess('Login Successful');
          localStorage.setItem('accessToken', token);
          localStorage.setItem('logoURL', logoURL);

          localStorage.setItem('userName', this.loginForm.controls['userName'].value)
          localStorage.setItem('menuItems', JSON.stringify(menuItems as Array<GetSideMenuOptionListDto>));
          this.authService.setAuthorize(true);
          this.authService.SetSubmitting(false);
          //this.router.navigateByUrl('dashboard');
          window.location.href = "dashboard";
        },
          error => {
            this.authService.SetSubmitting(false);
            this.utilService.ShowApiErrorMessage(error);
          });
    }
    else
      this.utilService.FillUpFields();
  }
}
