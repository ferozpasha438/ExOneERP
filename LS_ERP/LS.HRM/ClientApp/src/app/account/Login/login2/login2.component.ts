import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthorizeService } from '../../../api-authorization/AuthorizeService';
import { CINServerMetaDataDto } from '../../../models/CINServerMetaDataDto';
import { GetSideMenuOptionListDto } from '../../../models/MenuItemListDto';
import { ApiService } from '../../../services/api.service';
import { NotificationService } from '../../../services/notification.service';
import { UtilityService } from '../../../services/utility.service';
import { ParentSystemSetupComponent } from '../../../sharedcomponent/parentsystemsetup.component';
import { default as data } from "../../../../assets/i18n/apiuri.json";
import { default as dashboard } from "../../../../assets/i18n/siteConfig.json";
@Component({
  selector: 'app-login2',
  templateUrl: './login2.component.html',
  styleUrls: ['./login2.component.scss']
})
export class Login2Component extends ParentSystemSetupComponent implements OnInit {

  cinloginForm!: FormGroup;
  //loginForm!: FormGroup;
  dbconnectionString: string = '';
  dbHRMconnectionString: string = '';
  apiEndPoint: string = '';
  //moduleCodes: string = '';
  //cinNumber: string = '';
  isCinForm: boolean = true;
  apiUri: string = data.financeurl;
  lastcinnumber: string = '';
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router,// private apiService: ApiService,
    private authService: AuthorizeService, private utilService: UtilityService, private notifyService: NotificationService) {
    super(authService);

  }


  ngOnInit(): void {
    //console.log(this.authService.isAuthenticated())

    //if (this.authService.isAuthenticated())
    //  window.location.href = "dashboard";
    //else
    this.lastcinnumber = localStorage.getItem('lastcinnumber') ?? 'sahir';
    localStorage.clear();
    localStorage.setItem('lastcinnumber', this.lastcinnumber);
    this.setForm();
  }

  setForm() {
    //this.cinloginForm = this.fb.group({
    //  //'cinNumber': ['CIN0005', Validators.required]
    //  //'cinNumber': ['Saher', Validators.required]
    //  'cinNumber': ['erphrm', Validators.required]
    //});

    this.cinloginForm = this.fb.group({
      'cinNumber': [this.lastcinnumber, Validators.required],
      'userName': ['Admin', Validators.required],
      'password': ['admin@123', Validators.required]
      //'password': ['sh1234', Validators.required]

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
          this.cinloginForm.controls['cinNumber'].setValue(metaData.cinNumber);
          localStorage.setItem('metaData', JSON.stringify(metaData));
          this.login();
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

  login() {
    if (this.cinloginForm.valid) {
      this.authService.SetSubmitting(true);
      this.http.post(`${this.apiEndPoint}/login`, this.cinloginForm.value)
        .subscribe((res: any) => {
          const menuItems = res['userSideMenuList'];
          const token = res['token'];
          const logoURL = res['logoURL'];
          this.notifyService.showSuccess('Login Successful');
          localStorage.setItem('accessToken', token);
          localStorage.setItem('logoURL', logoURL);

          localStorage.setItem('userName', this.cinloginForm.controls['userName'].value)
          localStorage.setItem('menuItems', JSON.stringify(menuItems as Array<GetSideMenuOptionListDto>));
          this.authService.setAuthorize(true);
          this.authService.SetSubmitting(false);
          localStorage.setItem('lastcinnumber', this.cinloginForm.controls['cinNumber'].value);
          //this.router.navigateByUrl('dashboard');
          if (dashboard.dashBoardType == "default")
            window.location.href = "dashboard";
          else
            window.location.href = "dashboard/home1";
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
