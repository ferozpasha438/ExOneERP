import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GetemployeelistComponent } from './Employeemgt/Getemployeelist/getemployeelist.component';
import { GetaddresstypesComponent } from './Setup/AddressType/Getaddresstypes/getaddresstypes.component';
import { GetbanksComponent } from './Setup/Bank/Getbanks/getbanks.component';
import { GetbankbranchesComponent } from './Setup/BankBranch/getbankbranches/getbankbranches.component';
import { GetbloodgroupsComponent } from './Setup/BloodGroup/getbloodgroups/getbloodgroups.component';
import { GetcoursetypesComponent } from './Setup/CourseType/getcoursetypes/getcoursetypes.component';
import { GetdegreetypesComponent } from './Setup/DegreeType/getdegreetypes/getdegreetypes.component';
import { GetdependenttypesComponent } from './Setup/DependentType/getdependenttypes/getdependenttypes.component';
import { GetdocumentownersComponent } from './Setup/DocumentOwner/getdocumentowners/getdocumentowners.component';
import { GetdocumenttypesComponent } from './Setup/DocumentType/getdocumenttypes/getdocumenttypes.component';
import { GetEmployeeTypesComponent } from './Setup/EmployeeType/getemployeetypes/getemployeetypes.component';
import { GetgendersComponent } from './Setup/Gender/getgenders/getgenders.component';
import { GetgroupsComponent } from './Setup/Group/getgroups/getgroups.component';
import { GetinsuranceclassesComponent } from './Setup/InsuranceClass/getinsuranceclasses/getinsuranceclasses.component';
import { GetinsuranceprovidersComponent } from './Setup/InsuranceProvider/getinsuranceproviders/getinsuranceproviders.component';
import { GetinsurancetypesComponent } from './Setup/InsuranceType/getinsurancetypes/getinsurancetypes.component';
import { GetlanguagesComponent } from './Setup/Language/getlanguages/getlanguages.component';
import { GetnationalityComponent } from './Setup/Nationality/GetNationality/getnationality.component';
import { GetpositionsComponent } from './Setup/Position/Getpositions/getpositions.component';
import { GetqualificationsComponent } from './Setup/Qualification/getqualifications/getqualifications.component';
import { GetreligionsComponent } from './Setup/Religion/Getreligions/getreligions.component';
import { GetsubgroupsComponent } from './Setup/SubGroup/getsubgroups/getsubgroups.component';
import { GettitlesComponent } from './Setup/Title/gettitles/gettitles.component';
import { GetvisatypesComponent } from './Setup/VisaType/getvisatypes/getvisatypes.component';
import { GetshiftsComponent } from './Setup/Shift/getshifts/getshifts.component';
import { GetholidayComponent } from './Setup/Holiday/getholiday/getholiday.component';
import { GetholidaycalendarComponent } from './Setup/HolidayCalendar/getholidaycalendar/getholidaycalendar.component';
import { GetdivisonComponent } from './Setup/Division/getdivison/getdivison.component';
import { GetemployeestatusComponent } from './Setup/EmployeeStatus/getemployeestatus/getemployeestatus.component';
import { GetleavetypesComponent } from './Setup/LeaveType/getleavetypes/getleavetypes.component';
import { GetleavetemplatesComponent } from './Setup/LeaveTemplates/getleavetemplates/getleavetemplates.component';
import { VacationrequestComponent } from './ServiceRequest/vacationrequest/vacationrequest.component';
import { MyrequestComponent } from './ServiceRequest/myrequest/myrequest.component';
import { GetgradeComponent } from './Setup/Grade/getgrade/getgrade.component';
import { GetmaritalstatusComponent } from './Setup/MaritalStatus/getmaritalstatus/getmaritalstatus.component';
import { GetdepartmentsComponent } from './Setup/Departments/getdepartments/getdepartments.component';
import { WaitingapprovalrequestComponent } from './ServiceRequest/waitingapprovalrequest/waitingapprovalrequest.component';


const routes: Routes = [
  { path: 'getaddresstypes', component: GetaddresstypesComponent },
  { path: 'getbanks', component: GetbanksComponent },
  { path: 'getbankbranches', component: GetbankbranchesComponent },
  { path: 'getbloodgroups', component: GetbloodgroupsComponent },
  { path: 'getcoursetypes', component: GetcoursetypesComponent },
  { path: 'getdegreetypes', component: GetdegreetypesComponent },
  { path: 'getdependenttypes', component: GetdependenttypesComponent },
  { path: 'getdocumentowners', component: GetdocumentownersComponent },
  { path: 'getdocumenttypes', component: GetdocumenttypesComponent },
  { path: 'getemployeetypes', component: GetEmployeeTypesComponent },
  { path: 'getgenders', component: GetgendersComponent },
  { path: 'getgroups', component: GetgroupsComponent },
  { path: 'getinsuranceclasses', component: GetinsuranceclassesComponent },
  { path: 'getinsuranceproviders', component: GetinsuranceprovidersComponent },
  { path: 'getinsurancetypes', component: GetinsurancetypesComponent },
  { path: 'getlanguages', component: GetlanguagesComponent },
  { path: 'getpositions', component: GetpositionsComponent },
  { path: 'getqualifications', component: GetqualificationsComponent },
  { path: 'getreligions', component: GetreligionsComponent },
  { path: 'getsubgroups', component: GetsubgroupsComponent },
  { path: 'gettitles', component: GettitlesComponent },
  { path: 'getvisatypes', component: GetvisatypesComponent },
  { path: 'Getnationality', component: GetnationalityComponent },
  { path: 'Getemployeelist', component: GetemployeelistComponent },
  { path: 'getmyrequest', component: MyrequestComponent  },
  { path: 'getwaitingapprovalrequest', component: WaitingapprovalrequestComponent  },//GetleavetemplatesComponent 
  { path: 'getmartialstatus', component: GetmaritalstatusComponent },
  { path: 'getgrade', component: GetgradeComponent },
  { path: 'getleavetypes', component: GetleavetypesComponent },//GetleavetypesComponent
  { path: 'getleavetemplates', component: GetleavetemplatesComponent },//GetleavetemplatesComponent



  //time & attendance

  { path: 'getshifts', component: GetshiftsComponent },
  { path: 'getholiday', component: GetholidayComponent },
  { path: 'getholidaycalendar', component: GetholidaycalendarComponent },
  { path: 'getdivisions', component: GetdivisonComponent },
  { path: 'getemployeestatuses', component: GetemployeestatusComponent },


];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HumanresourceRoutingModule { }
