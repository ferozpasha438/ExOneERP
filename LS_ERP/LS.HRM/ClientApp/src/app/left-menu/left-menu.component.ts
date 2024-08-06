import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { ApiService } from '../services/api.service';
import { GetSideMenuOptionListDto } from "../models/MenuItemListDto";
import { AuthorizeService } from '../api-authorization/AuthorizeService';
import { UtilityService } from '../services/utility.service';
import { ParentSystemSetupComponent } from '../sharedcomponent/parentsystemsetup.component';
import { HttpClient, HttpHeaders } from '@angular/common/http';


@Component({
  selector: 'app-left-menu',
  templateUrl: './left-menu.component.html',
  styleUrls: ['./left-menu.component.css'],

})
export class LeftMenuComponent extends ParentSystemSetupComponent implements OnInit {

  language: string = 'en';
  menuItems: Array<GetSideMenuOptionListDto> = [];
  constructor(private authService: AuthorizeService, private http: HttpClient,
    private apiService: ApiService, private utilService: UtilityService) {
    super(authService);
  }

  ngOnInit(): void {

    this.http.get(`${this.authService.GetSystemSetupApiEndPoint()}/base/isAuthenticated`).subscribe(lan => {
    }, error => this.utilService.ShowApiErrorMessage(error));

    this.loadUserMenuLinks();
  }
  loadUserMenuLinks() {
    const selectedLan = this.authService.selectedLanguage();
    this.language = selectedLan;
    //if (selectedLan !== 'ar') {
    //  var menuItems = JSON.parse(localStorage.getItem('menuItems') as string) as Array<GetSideMenuOptionListDto>;
    //  menuItems.forEach(item => {
    //    item.
    //  });

    //}
    //else

    this.menuItems = JSON.parse(localStorage.getItem('menuItems') as string) as Array<GetSideMenuOptionListDto>;

    //this.apiService.getall('menuOption/getSideMenuOptionList').subscribe(res => {
    //  this.menuItems = res;
    //});
  }

  expander(id: any) {
    const svgIconId = document.getElementById('svgIconId') as HTMLElement;
    if (svgIconId && svgIconId.tagName === "I") {
      const element = document.getElementById(id)?.getAttribute('class');

      //let list = document.getElementsByClassName('subLevel');// as HTMLCollectionOf<Element>;
      //for (var i = 0; i < list.length; i++) {
      //  const ulElement = document.getElementById(list[i].id);
      //  if (ulElement)// && list[i].id !== 'ul_' + id)
      //    ulElement.style.display = "none";
      //  //document.getElementById(list[i].id)?.setAttribute('class', 'collapse  subLevel first-level base-level-line li');
      //}

      if (element?.indexOf('active') !== -1) {
        //document.getElementById(id)?.removeAttribute('class');
        //document.getElementById('ul_' + id)?.removeAttribute('class');

        //const currentUl = document.getElementById('ul_' + id);
        //if (currentUl)
        //  currentUl.style.display = "none";
        document.getElementById(id)?.setAttribute('class', 'sidebar-link has-arrow defaultColor');
        document.getElementById('ul_' + id)?.setAttribute('class', 'collapse  subLevel first-level base-level-line li');
      }
      else {
        //const currentUl = document.getElementById('ul_' + id);
        //if (currentUl)
        //  currentUl.style.display = "block";
        document.getElementById(id)?.setAttribute('class', 'sidebar-link has-arrow defaultColor active');
        document.getElementById('ul_' + id)?.setAttribute('class', 'collapse  subLevel first-level base-level-line li in');
      }
    }
  }


  moduleExpander(className: any) {
    let x = document.getElementsByClassName(className);
    var i;
    let yes_has = x[0].getAttribute('class')?.split(' ')[0];
    for (i = 0; i < x.length; i++) {
      if (yes_has === 'yes_has')
        x[i].setAttribute("class", "d-none sidebar-item " + className);
      else
        x[i].setAttribute("class", "yes_has sidebar-item " + className);
    }
  }

}
