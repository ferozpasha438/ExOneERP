import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HumanresourceRoutingModule } from './humanresource-routing.module';
import { SharedModule } from '../sharedcomponent/shared.module';

//get components imports

import { GetpositionsComponent } from './Setup/Position/Getpositions/getpositions.component';
import { GetreligionsComponent } from './Setup/Religion/Getreligions/getreligions.component';
import { GetaddresstypesComponent } from './Setup/AddressType/Getaddresstypes/getaddresstypes.component';
import { GetbanksComponent } from './Setup/Bank/Getbanks/getbanks.component';
import { GetbankbranchesComponent } from './Setup/BankBranch/getbankbranches/getbankbranches.component';
import { GetdegreetypesComponent } from './Setup/DegreeType/getdegreetypes/getdegreetypes.component';
import { GetdependenttypesComponent } from './Setup/DependentType/getdependenttypes/getdependenttypes.component';
import { GetdocumenttypesComponent } from './Setup/DocumentType/getdocumenttypes/getdocumenttypes.component';
import { GetgendersComponent } from './Setup/Gender/getgenders/getgenders.component';
import { GetgroupsComponent } from './Setup/Group/getgroups/getgroups.component';
import { GetinsuranceclassesComponent } from './Setup/InsuranceClass/getinsuranceclasses/getinsuranceclasses.component';
import { GetinsuranceprovidersComponent } from './Setup/InsuranceProvider/getinsuranceproviders/getinsuranceproviders.component';
import { GetinsurancetypesComponent } from './Setup/InsuranceType/getinsurancetypes/getinsurancetypes.component';
import { GetlanguagesComponent } from './Setup/Language/getlanguages/getlanguages.component';
/*import { GetmartialstatusComponent } from './Setup/MartialStatus/getmartialstatus/getmartialstatus.component';*/
import { GetsubgroupsComponent } from './Setup/SubGroup/getsubgroups/getsubgroups.component';
import { GettitlesComponent } from './Setup/Title/gettitles/gettitles.component';
import { GetbloodgroupsComponent } from './Setup/BloodGroup/getbloodgroups/getbloodgroups.component';
import { GetcoursetypesComponent } from './Setup/CourseType/getcoursetypes/getcoursetypes.component';
import { GetqualificationsComponent } from './Setup/Qualification/getqualifications/getqualifications.component';
import { GetvisatypesComponent } from './Setup/VisaType/getvisatypes/getvisatypes.component';
import { GetEmployeeTypesComponent } from './Setup/EmployeeType/getemployeetypes/getemployeetypes.component';

//addupdates imports

import { AddupdatedegreetypesComponent } from './Setup/DegreeType/addupdatedegreetypes/addupdatedegreetypes.component';
import { AddupdateaddresstypesComponent } from './Setup/AddressType/addupdateaddresstypes/addupdateaddresstypes.component';
import { AddupdatereligionsComponent } from './Setup/Religion/addupdatereligions/addupdatereligions.component';
import { GetdocumentownersComponent } from './Setup/DocumentOwner/getdocumentowners/getdocumentowners.component';
import { AddupdatebankbranchesComponent } from './Setup/BankBranch/addupdatebankbranches/addupdatebankbranches.component';
import { AddupdatebanksComponent } from './Setup/Bank/addupdatebanks/addupdatebanks.component';
import { AddupdatetitlesComponent } from './Setup/Title/addupdatetitles/addupdatetitles.component';
import { AddupdatepositionsComponent } from './Setup/Position/addupdatepositions/addupdatepositions.component';
import { AddupdatesubgroupsComponent } from './Setup/SubGroup/addupdatesubgroups/addupdatesubgroups.component';
/*import { AddupdatemartialstatusComponent } from './Setup/MartialStatus/addupdatemartialstatus/addupdatemartialstatus.component';*/
import { AddupdatelanguagesComponent } from './Setup/Language/addupdatelanguages/addupdatelanguages.component';
import { AddupdateinsurancetypesComponent } from './Setup/InsuranceType/addupdateinsurancetypes/addupdateinsurancetypes.component';
import { AddupdateinsuranceprovidersComponent } from './Setup/InsuranceProvider/addupdateinsuranceproviders/addupdateinsuranceproviders.component';
import { AddupdateinsuranceclassesComponent } from './Setup/InsuranceClass/addupdateinsuranceclasses/addupdateinsuranceclasses.component';
import { AddupdategroupsComponent } from './Setup/Group/addupdategroups/addupdategroups.component';
import { AddupdategendersComponent } from './Setup/Gender/addupdategenders/addupdategenders.component';
import { AddupdatedocumenttypesComponent } from './Setup/DocumentType/addupdatedocumenttypes/addupdatedocumenttypes.component';
import { AddupdatedependenttypesComponent } from './Setup/DependentType/addupdatedependenttypes/addupdatedependenttypes.component';
import { AddupdatebloodgroupsComponent } from './Setup/BloodGroup/addupdatebloodgroups/addupdatebloodgroups.component';
import { AddupdatecoursetypesComponent } from './Setup/CourseType/addupdatecoursetypes/addupdatecoursetypes.component';
import { AddupdatequalificationsComponent } from './Setup/Qualification/addupdatequalifications/addupdatequalifications.component';
import { AddupdatevisatypesComponent } from './Setup/VisaType/addupdatevisatypes/addupdatevisatypes.component';
import { AddupdatedocumentownersComponent } from './Setup/DocumentOwner/addupdatedocumentowners/addupdatedocumentowners.component';
import { AddupdateemployeetypesComponent } from './Setup/EmployeeType/addupdateemployeetypes/addupdateemployeetypes.component';
import { GetemployeelistComponent } from './Employeemgt/Getemployeelist/getemployeelist.component';
import { GetemployeepersonalinfoComponent } from './Employeemgt/Employeepersonalinfo/getemployeepersonalinfo.component';
import { EmployeemanagementtabsComponent } from './Employeemgt/Sharedcomponent/employeemanagementtabs/employeemanagementtabs.component';
import { GetemployeeaddressComponent } from './Employeemgt/EmployeeAddress/EmployeeAddress/getemployeeaddress.component';
import { AddupdateaddressComponent } from './Employeemgt/EmployeeAddress/Addupdateaddress/addupdateaddress.component';
import { GetemployeecontactinfoComponent } from './Employeemgt/EmployeeContactInfo/EmployeeContact/getemployeecontactinfo.component';
import { AddupdatecontactinfoComponent } from './Employeemgt/EmployeeContactInfo/AddupdateContact/addupdatecontactinfo.component';
import { GetemployeeeducationinfoComponent } from './Employeemgt/EmployeeEducation/EmployeeEducation/getemployeeeducationinfo.component';
import { AddupdateeducationinfoComponent } from './Employeemgt/EmployeeEducation/Addupdateeducation/addupdateeducationinfo.component';
import { GetemployeevisainfoComponent } from './Employeemgt/EmployeeVisaInfo/EmployeeVisaInfo/getemployeevisainfo.component';
import { AddupdatevisainfoComponent } from './Employeemgt/EmployeeVisaInfo/AddupdateVisa/addupdatevisainfo.component';
import { GetemployeeinsuranceinfoComponent } from './Employeemgt/EmployeeInsuranceInfo/EmployeeInsuranceInfo/getemployeeinsuranceinfo.component';
import { AddupdateinsuranceinfoComponent } from './Employeemgt/EmployeeInsuranceInfo/AddupdateInsurance/addupdateinsuranceinfo.component';
import { GetemployeedocumentinfoComponent } from './Employeemgt/EmployeeDocumentInfo/EmployeeDocumentInfo/getemployeedocumentinfo.component';
import { AddupdatedocumentinfoComponent } from './Employeemgt/EmployeeDocumentInfo/AddupdateDocument/addupdatedocumentinfo.component';
import { GetemployeecontractinfoComponent } from './Employeemgt/EmployeeContractInfo/EmployeeContractInfo/getemployeecontractinfo.component';
import { GetnationalityComponent } from './Setup/Nationality/GetNationality/getnationality.component';
import { AddupdatenationalityComponent } from './Setup/Nationality/addupdatenationality/addupdatenationality.component';
import { GetemployeedependentinfoComponent } from './Employeemgt/EmployeeDependentInfo/EmployeeDependentInfo/getemployeedependentinfo.component';
import { AddupdatedependentinfoComponent } from './Employeemgt/EmployeeDependentInfo/AddupdateDependent/addupdatedependentinfo.component';
import { EmployeebasicinfoComponent } from './Employeemgt/Sharedcomponent/EmployeeBasicInfo/employeebasicinfo.component';
import { GetshiftsComponent } from './Setup/Shift/getshifts/getshifts.component';
import { AddupdateshiftsComponent } from './Setup/Shift/addupdateshifts/addupdateshifts.component';
import { GetholidaycalendarComponent } from './Setup/HolidayCalendar/getholidaycalendar/getholidaycalendar.component';
import { AddupdateholidaycalendarComponent } from './Setup/HolidayCalendar/addupdateholidaycalendar/addupdateholidaycalendar.component';
import { GetholidayComponent } from './Setup/Holiday/getholiday/getholiday.component';
import { AddupdateholidayComponent } from './Setup/Holiday/addupdateholiday/addupdateholiday.component';
import { GetshiftinfoComponent } from './Employeemgt/EmployeeShiftInfo/getshiftinfo/getshiftinfo.component';
import { AddupdatedivisonComponent } from './Setup/Division/addupdatedivison/addupdatedivison.component';
import { GetdivisonComponent } from './Setup/Division/getdivison/getdivison.component';
import { AddupdateemployeestatusComponent } from './Setup/EmployeeStatus/addupdateemployeestatus/addupdateemployeestatus.component';
import { GetemployeestatusComponent } from './Setup/EmployeeStatus/getemployeestatus/getemployeestatus.component';
import { GetdepartmentsComponent } from './Setup/Departments/getdepartments/getdepartments.component';
import { AddupdatedepartmentsComponent } from './Setup/Departments/addupdatedepartments/addupdatedepartments.component';
import { GetemployeepayrollstructureComponent } from './Employeemgt/EmployeePayrollStructure/getemployeepayrollstructure.component';
import { AddupdateleavetypesComponent } from './Setup/LeaveType/addupdateleavetypes/addupdateleavetypes.component';
import { GetleavetypesComponent } from './Setup/LeaveType/getleavetypes/getleavetypes.component';
import { AddupdateleavetemplatesComponent } from './Setup/LeaveTemplates/addupdateleavetemplates/addupdateleavetemplates.component';
import { GetleavetemplatesComponent } from './Setup/LeaveTemplates/getleavetemplates/getleavetemplates.component';
import { LeaverequestapprovalComponent } from './ServiceRequest/shared/leaverequestapproval/leaverequestapproval.component';
import { LeaverequestattachmentComponent } from './ServiceRequest/shared/leaverequestattachment/leaverequestattachment.component';
import { LeaverequestotherComponent } from './ServiceRequest/shared/leaverequestother/leaverequestother.component';
import { LeaverequestemployeeinfoComponent } from './ServiceRequest/shared/leaverequestemployeeinfo/leaverequestemployeeinfo.component';
import { VacationrequestComponent } from './ServiceRequest/vacationrequest/vacationrequest.component';
import { MyrequestComponent } from './ServiceRequest/myrequest/myrequest.component';
import { ServicerequestinfoComponent } from './ServiceRequest/servicerequestinfo/servicerequestinfo.component';
import { GetgradeComponent } from './Setup/Grade/getgrade/getgrade.component';
import { AddupdategradeComponent } from './Setup/Grade/addupdategrade/addupdategrade.component';
import { GetmaritalstatusComponent } from './Setup/MaritalStatus/getmaritalstatus/getmaritalstatus.component';
import { AddupdatemaritalstatusComponent } from './Setup/MaritalStatus/addupdatemaritalstatus/addupdatemaritalstatus.component';
import { LeaverequestemployeelistComponent } from './ServiceRequest/shared/leaverequestemployeelist/leaverequestemployeelist.component';
import { WaitingapprovalrequestComponent } from './ServiceRequest/waitingapprovalrequest/waitingapprovalrequest.component';
import { LeaverequestauditComponent } from './ServiceRequest/shared/leaverequestaudit/leaverequestaudit.component';
import { GetemployeeleaveComponent } from './Employeemgt/EmployeeLeaveInfo/EmployeeLeave/getemployeeleave.component';
import { MultiapprovalrequestComponent } from './ServiceRequest/shared/multiapprovalrequest/multiapprovalrequest.component';
import { GetvacationpolicyComponent } from './Setup/VacationPolicy/getvacationpolicy/getvacationpolicy.component';
import { AddupdatevacationpolicyComponent } from './Setup/VacationPolicy/addupdatevacationpolicy/addupdatevacationpolicy.component';
import { EmployeeexitreentryComponent } from './ServiceRequest/shared/employeeexitreentry/employeeexitreentry.component';
import { EmployeereportingbackComponent } from './ServiceRequest/shared/employeereportingback/employeereportingback.component';
import { LeaverequestComponent } from './ServiceRequest/leaverequest/leaverequest.component';
// import { GetgenderComponent } from './Setup/Grade/getgender/getgender.component';

@NgModule({
  declarations: [
    GetpositionsComponent,
    GetaddresstypesComponent,
    GetreligionsComponent,
    GetbanksComponent,
    GetbankbranchesComponent,
    GetdegreetypesComponent,
    GetdependenttypesComponent,
    GetdocumenttypesComponent,
    GetgendersComponent,
    GetgroupsComponent,
    GetinsuranceclassesComponent,
    GetinsuranceprovidersComponent,
    GetinsurancetypesComponent,
    GetlanguagesComponent,
    /*GetmartialstatusComponent,*/
    GetsubgroupsComponent,
    GettitlesComponent,
    GetdocumentownersComponent,
    GetvisatypesComponent,
    GetbloodgroupsComponent,
    GetcoursetypesComponent,
    GetqualificationsComponent,
    GetEmployeeTypesComponent,
    AddupdatedocumentownersComponent,
    AddupdatepositionsComponent,
    AddupdatereligionsComponent,
    AddupdateaddresstypesComponent,
    AddupdatebanksComponent,
    AddupdatebankbranchesComponent,
    AddupdatedegreetypesComponent,
    AddupdatedependenttypesComponent,
    AddupdatedocumenttypesComponent,
    AddupdategendersComponent,
    AddupdategroupsComponent,
    AddupdateinsuranceclassesComponent,
    AddupdateinsuranceprovidersComponent,
    AddupdateinsurancetypesComponent,
    AddupdatelanguagesComponent,
    /* AddupdatemartialstatusComponent,*/
    AddupdatesubgroupsComponent,
    AddupdatetitlesComponent,
    AddupdatebloodgroupsComponent,
    AddupdatecoursetypesComponent,
    AddupdatequalificationsComponent,
    AddupdatevisatypesComponent,
    AddupdateemployeetypesComponent,
    GetemployeelistComponent,
    GetemployeepersonalinfoComponent,
    EmployeemanagementtabsComponent,
    GetemployeeaddressComponent,
    AddupdateaddressComponent,
    GetemployeecontactinfoComponent,
    AddupdatecontactinfoComponent,
    GetemployeeeducationinfoComponent,
    AddupdateeducationinfoComponent,
    GetemployeevisainfoComponent,
    AddupdatevisainfoComponent,
    GetemployeeinsuranceinfoComponent,
    AddupdateinsuranceinfoComponent,
    GetemployeedocumentinfoComponent,
    AddupdatedocumentinfoComponent,
    GetemployeecontractinfoComponent,
    GetnationalityComponent,
    AddupdatenationalityComponent,
    GetemployeedependentinfoComponent,
    AddupdatedependentinfoComponent,
    EmployeebasicinfoComponent,
    GetshiftsComponent,
    AddupdateshiftsComponent,
    GetholidaycalendarComponent,
    AddupdateholidaycalendarComponent,
    GetholidayComponent,
    AddupdateholidayComponent,
    GetshiftinfoComponent,
    AddupdatedivisonComponent,
    GetdivisonComponent,
    AddupdateemployeestatusComponent,
    GetemployeestatusComponent,
    GetdepartmentsComponent,
    AddupdatedepartmentsComponent,
    GetemployeepayrollstructureComponent,
    AddupdateleavetypesComponent,
    GetleavetypesComponent,
    AddupdateleavetemplatesComponent,
    GetleavetemplatesComponent,
    LeaverequestapprovalComponent,
    LeaverequestattachmentComponent,
    LeaverequestotherComponent,
    LeaverequestemployeeinfoComponent,
    VacationrequestComponent,
    MyrequestComponent,
    ServicerequestinfoComponent,
    GetgradeComponent,
    AddupdategradeComponent,
    GetmaritalstatusComponent,
    AddupdatemaritalstatusComponent,
    LeaverequestemployeelistComponent,
    WaitingapprovalrequestComponent,
    LeaverequestauditComponent,
    GetemployeeleaveComponent,
    MultiapprovalrequestComponent,
    GetvacationpolicyComponent,
    AddupdatevacationpolicyComponent,
    EmployeeexitreentryComponent,
    EmployeereportingbackComponent,
    MultiapprovalrequestComponent,
    LeaverequestComponent,
  ],
  imports: [CommonModule, HumanresourceRoutingModule, SharedModule],
})
export class HumanresourceModule {}
