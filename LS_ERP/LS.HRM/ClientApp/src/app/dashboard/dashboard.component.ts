import { Component, OnInit } from '@angular/core';
import { GetSideMenuOptionListDto } from '../models/MenuItemListDto';
import { UtilityService } from '../services/utility.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styles: [
  ]
})
export class DashboardComponent implements OnInit {
  //menuItems: Array<GetSideMenuOptionListDto> = [];
  isArab: boolean = false;
  constructor(private utilService: UtilityService) { }

  ngOnInit(): void {
    this.isArab = this.utilService.isArabic();
    //console.log(localStorage.getItem('language'));
  //  this.menuItems = JSON.parse(localStorage.getItem('menuItems') as string) as Array<GetSideMenuOptionListDto>;
  //  console.log('the length of the menu items are for ' + this.menuItems.length)
  }

}
