import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TestingsalespageComponent } from './testingsalespage/testingsalespage.component';

const routes: Routes = [
  { path: 'testingsalespage', component: TestingsalespageComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SalesmgtRoutingModule { }
