import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AttendanceFromMonthlyRoasterComponent } from './attendance-from-monthly-roaster/attendance-from-monthly-roaster.component';
import { AuthoritiesComponent } from './authorities/authorities.component';
import { ContractFormElementsComponent } from './contract-form-templates/contract-form-elements/contract-form-elements.component';
import { ContractFormTemplatesComponent } from './contract-form-templates/contract-form-templates.component';
import { CustomerComplaintsComponent } from './customer-complaints/customer-complaints.component';
import { CustomerSitesComponent } from './customer-sites/customer-sites.component';
import { CustomerVisitFormsComponent } from './customer-visit-forms/customer-visit-forms.component';
import { EditableSurveyFormComponent } from './editable-survey-form/editable-survey-form.component';
import { AttendancePayrollReportComponent } from './employee/employee-attendance/attendance-payroll-report/attendance-payroll-report.component';
import { AttendancePayrollReport2Component } from './employee/employee-attendance/attendance-payroll-report/attendance-payroll-report2.component';
import { EmployeePrimarysitelogsComponent } from './employee/employee-primarysitelogs/employee-primarysitelogs.component';
import { EmployeeComponent } from './employee/employee.component';
import { EnquiriesBySurveyorCodeComponent } from './enquiries-by-surveyor-code/enquiries-by-surveyor-code.component';
import { ExistingProjectsComponent } from './existing-projects/existing-projects.component';
import { LogisticsandvehicleComponent } from './logisticsandvehicle/logisticsandvehicle.component';
import { MaterialequipmentComponent } from './materialequipment/materialequipment.component';
import { MonthlyRoasterComponent } from './monthly-roaster/monthly-roaster.component';
import { MonthylRoasterForProjectComponent } from './monthyl-roaster-for-project/monthyl-roaster-for-project.component';

import { OpcustomerComponent } from './opcustomer/opcustomer.component';
import { OperationexpenseheadsComponent } from './operationexpenseheads/operationexpenseheads.component';
import { OprDashBoardComponent } from './opr-dash-board/opr-dash-board.component';
import { PayrollGroupComponent } from './payroll-group/payroll-group.component';
import { PrintEnquiriesByEnquiryNumberComponent } from './print-enquiries-by-enquiry-number/print-enquiries-by-enquiry-number.component';
import { PrintSurveyFormComponent } from './print-survey-form/print-survey-form.component';
import { AddResourceComponent } from './ProgectManagement/add-resource/add-resource.component';
import { CreateUpdateAddResourceComponent } from './ProgectManagement/add-resource/create-update-add-resource/create-update-add-resource.component';
import { AllRequestsComponent } from './ProgectManagement/all-requests/all-requests.component';
import { PvOpenCloseReqsComponent } from './ProgectManagement/pv-open-close-reqs/pv-open-close-reqs.component';
import { RemoveResourceComponent } from './ProgectManagement/remove-resource/remove-resource.component';
import { ReplaceEmployeeComponent } from './ProgectManagement/replace-employee/replace-employee.component';
import { SwapEmployeesComponent } from './ProgectManagement/swap-employees/swap-employees.component';
import { TransferEmployeeComponent } from './ProgectManagement/transfer-employee/transfer-employee.component';
import { TransferWithReplacementComponent } from './ProgectManagement/transfer-with-replacement/transfer-with-replacement.component';
import { ProjectBudgetCostingComponent } from './project-budget-costing/project-budget-costing.component';
import { ProjectComponent } from './project/project.component';
import { Project2Component } from './project/project2.component';
import { ReasoncodesComponent } from './reasoncodes/reasoncodes.component';
import { AttendanceStatusReportComponent } from './reports/attendance-status-report/attendance-status-report.component';
import { CountOfSkillsetsComponent } from './reports/count-of-skillsets/count-of-skillsets.component';
import { ProjectsCountMatrixComponent } from './reports/projects-count-matrix/projects-count-matrix.component';
import { ProjectsSitesReportsComponent } from './reports/projects-sites-reports/projects-sites-reports.component';
import { ReportCustomercomplaintsComponent } from './reports/report-customercomplaints/report-customercomplaints.component';
import { ReportCustomervisitsComponent } from './reports/report-customervisits/report-customervisits.component';
import { ResourcesOnProjectComponent } from './reports/resources-on-project/resources-on-project.component';
import { ServiceCodeComponent } from './service-code/service-code.component';
import { ServiceEnquiryFormsComponent } from './service-enquiry-forms/service-enquiry-forms.component';
import { ServiceUnitMappingsComponent } from './service-unit-mappings/service-unit-mappings.component';
import { ServicesEnquiryComponent } from './services-enquiry/services-enquiry.component';
import { ShiftMasterComponent } from './shift-master/shift-master.component';
import { ShiftsToSiteMapComponent } from './shift-master/shifts-to-site-map/shifts-to-site-map.component';
import { SitesListByCustomerCodeComponent } from './sites-list-by-customer-code/sites-list-by-customer-code.component';
import { SkillsetComponent } from './skillset/skillset.component';
import { SurveyFormElementsComponent } from './survey-form-elements/survey-form-elements.component';
import { SurveyFormComponent } from './survey-form/survey-form.component';
import { SurveyorComponent } from './surveyor/surveyor.component';
import { TestExampleComponent } from './test-example/test-example.component';
import { UnitCodeForServicesComponent } from './unit-code-for-services/unit-code-for-services.component';
import { ViewEnquiriesByEnquiryNumberComponent } from './view-enquiries-by-enquiry-number/view-enquiries-by-enquiry-number.component';
import { ViewSurveyFormTemplateComponent } from './view-survey-form-template/view-survey-form-template.component';
 

const routes: Routes = [
  { path: 'opCustomer', component: OpcustomerComponent },
  { path: 'customerSites', component: CustomerSitesComponent },
  { path: 'customerSites/viewSites', component: SitesListByCustomerCodeComponent},
  { path: 'serviceCode', component: ServiceCodeComponent },
  { path: 'servicesEnquiry', component: ServicesEnquiryComponent },
  { path: 'surveyForm', component: SurveyFormComponent },
  { path: 'surveyForm/ViewSurveyFormTemplate', component: ViewSurveyFormTemplateComponent },
  { path: 'surveyFormElements', component: SurveyFormElementsComponent },
  { path: 'surveyor', component: SurveyorComponent },
  { path: 'unitCodeForServices', component: UnitCodeForServicesComponent },
  { path: 'serviceUnitMappings', component: ServiceUnitMappingsComponent },
  { path: 'serviceEnquiryForms', component: ServiceEnquiryFormsComponent },
  { path: 'serviceEnquiryForms/viewEnquiries', component: ViewEnquiriesByEnquiryNumberComponent },
  { path: 'serviceEnquiryForms/viewEnquiries/PrintSurveyForm', component: PrintSurveyFormComponent },
  { path: 'serviceEnquiryForms/viewEnquiries/EditableSurveyForm', component: EditableSurveyFormComponent },
  { path: 'serviceEnquiryForms/printEnquiryForm', component: PrintEnquiriesByEnquiryNumberComponent },
  { path: 'surveyor/viewEnquiriesBySurveyorCode', component: EnquiriesBySurveyorCodeComponent},
  { path: 'surveyor/viewEnquiriesBySurveyorCode/PrintSurveyForm', component: PrintSurveyFormComponent},
  { path: 'surveyor/viewEnquiriesBySurveyorCode/EditableSurveyForm', component: EditableSurveyFormComponent },
  { path: 'employee', component: EmployeeComponent },
  { path: 'employeePrimarySiteLogs', component: EmployeePrimarysitelogsComponent },
  { path: 'projects', component: ProjectComponent },
  { path: 'projects2', component: Project2Component },
  { path: 'shiftMaster', component: ShiftMasterComponent },
  { path: 'payrollGroup', component: PayrollGroupComponent },
  { path: 'shiftToSiteMap', component: ShiftsToSiteMapComponent },
  { path: 'monthlyRoaster', component: MonthlyRoasterComponent },
  { path: 'attendance', component: AttendanceFromMonthlyRoasterComponent },
  { path: 'skillset', component: SkillsetComponent },
  { path: 'materialequipment', component: MaterialequipmentComponent },
  { path: 'operationexpensehead', component: OperationexpenseheadsComponent },
  { path: 'logisticsandvehicle', component: LogisticsandvehicleComponent },
 /* { path: 'monthlyRoasterForProject', component: MonthylRoasterForProjectComponent },*/
  { path: 'projectBudgetCosting', component: ProjectBudgetCostingComponent},
  { path: 'authorities', component: AuthoritiesComponent},
  { path: 'test', component: TestExampleComponent},
  { path: 'oprDashBoard', component: OprDashBoardComponent },
  { path: 'existingProjects', component: ExistingProjectsComponent },
  { path: 'addResourceRequests', component: AddResourceComponent },
  { path: 'transferEmployeeRequests', component: TransferEmployeeComponent },
  { path: 'removeResourceRequests', component: RemoveResourceComponent },
  { path: 'replaceEmployeeRequests', component: ReplaceEmployeeComponent },
  { path: 'pvOpenCloseRequests', component: PvOpenCloseReqsComponent },
  { path: 'reportsProjectSitesList', component: ProjectsSitesReportsComponent },
  { path: 'reportsProjectsCountMatrix', component: ProjectsCountMatrixComponent },
  { path: 'reportsResourcesOnProject', component: ResourcesOnProjectComponent },
  { path: 'reportsCountOfSkillsets', component: CountOfSkillsetsComponent },
  { path: 'reportsAttendanceStatus', component: AttendanceStatusReportComponent },
  { path: 'reportCustomerComplaints', component: ReportCustomercomplaintsComponent },
  { path: 'reportCustomerVisits', component: ReportCustomervisitsComponent },
  { path: 'requests', component: AllRequestsComponent },
  { path: 'contractFormTemplates', component: ContractFormTemplatesComponent },
  { path: 'contractFormTemplateElements', component: ContractFormElementsComponent },
  { path: 'tranferWithReplacementReqs', component: TransferWithReplacementComponent },
  { path: 'swapEmployeesReqs', component: SwapEmployeesComponent },

  { path: 'reasonCodes', component: ReasoncodesComponent },
 
  { path: 'test', component: TestExampleComponent }
  ];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],

})
export class OperationalmgtRoutingModule {
}
