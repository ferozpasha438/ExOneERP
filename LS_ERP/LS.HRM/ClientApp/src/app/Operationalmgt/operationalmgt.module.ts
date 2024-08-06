import { CommonModule, DatePipe } from '@angular/common';
import { NgModule } from '@angular/core';
 import { SharedModule } from '../sharedcomponent/shared.module';
import { OpcustomerComponent } from './opcustomer/opcustomer.component';
import { OperationalmgtRoutingModule } from './operationalmgt-routing.module';
//import { AddupdatecustomerComponent } from './sharedpages/addupdatecustomer/addupdatecustomer.component';
import { AddupdatesitesComponent } from './sharedpages/addupdatesites/addupdatesites.component';
import { CustomerSitesComponent } from './customer-sites/customer-sites.component';
import { ServiceCodeComponent } from './service-code/service-code.component';
import { AddupdateServiceCodeComponent } from './sharedpages/addupdate-service-code/addupdate-service-code.component';
import { ServicesEnquiryComponent } from './services-enquiry/services-enquiry.component';
import { AddupdateServicesEnquiryComponent } from './sharedpages/addupdate-services-enquiry/addupdate-services-enquiry.component';
import { SurveyFormComponent } from './survey-form/survey-form.component';
import { AddupdateSurveyFormComponent } from './sharedpages/addupdate-survey-form/addupdate-survey-form.component';
import { SurveyFormElementsComponent } from './survey-form-elements/survey-form-elements.component';
import { AddupdateSurveyFormElementsComponent } from './sharedpages/addupdate-survey-form-elements/addupdate-survey-form-elements.component';
import { SurveyorComponent } from './surveyor/surveyor.component';
import { AddupdateSurveyorComponent } from './sharedpages/addupdate-surveyor/addupdate-surveyor.component';
import { UnitCodeForServicesComponent } from './unit-code-for-services/unit-code-for-services.component';
import { AddupdateUnitCodeForServicesComponent } from './sharedpages/addupdate-unit-code-for-services/addupdate-unit-code-for-services.component';
import { AddupdatecustomerComponent } from './sharedpages/addupdatecustomer/addupdatecustomer.component';
import { AddupdateServiceunitmappComponent } from './sharedpages/addupdate-serviceunitmapp/addupdate-serviceunitmapp.component';
import { ServiceUnitMappingsComponent } from './service-unit-mappings/service-unit-mappings.component';
import { AddSiteToCustomerComponent } from './sharedpages/add-site-to-customer/add-site-to-customer.component';
import { AddupdateEnquiryFormComponent } from './sharedpages/addupdate-enquiry-form/addupdate-enquiry-form.component';
import { ServiceEnquiryFormsComponent } from './service-enquiry-forms/service-enquiry-forms.component';
import { AddSurveyorToEnquiryComponent } from './sharedpages/add-surveyor-to-enquiry/add-surveyor-to-enquiry.component';
import { ViewEnquiriesByEnquiryNumberComponent } from './view-enquiries-by-enquiry-number/view-enquiries-by-enquiry-number.component';
import { PrintEnquiriesByEnquiryNumberComponent } from './print-enquiries-by-enquiry-number/print-enquiries-by-enquiry-number.component';
import { AttachSurveyFormToServiceComponent } from './sharedpages/attach-survey-form-to-service/attach-survey-form-to-service.component';
import { EnquiriesBySurveyorCodeComponent } from './enquiries-by-surveyor-code/enquiries-by-surveyor-code.component';
import { PrintSurveyFormComponent } from './print-survey-form/print-survey-form.component';
import { ViewSurveyFormTemplateComponent } from './view-survey-form-template/view-survey-form-template.component';
import { SitesListByCustomerCodeComponent } from './sites-list-by-customer-code/sites-list-by-customer-code.component';
import { EditableSurveyFormComponent } from './editable-survey-form/editable-survey-form.component';
import { EmployeeComponent } from './employee/employee.component';
import { AddUpdateEmployeeComponent } from './employee/add-update-employee/add-update-employee.component';
import { ProjectComponent } from './project/project.component';
//import { AddUpdateProjectComponent } from './project/add-update-project/add-update-project.component';
import { ShiftMasterComponent } from './shift-master/shift-master.component';
import { AddUpdateShiftComponent } from './shift-master/add-update-shift/add-update-shift.component';
import { PayrollGroupComponent } from './payroll-group/payroll-group.component';
import { AddUpdatePayrollGroupComponent } from './payroll-group/add-update-payroll-group/add-update-payroll-group.component';
import { ShiftsToSiteMapComponent } from './shift-master/shifts-to-site-map/shifts-to-site-map.component';
import { MonthlyRoasterComponent } from './monthly-roaster/monthly-roaster.component';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatTooltipModule } from '@angular/material/tooltip';
import { TestExampleComponent } from './test-example/test-example.component';
import { TestChildComponent } from './test-example/test-child/test-child.component';
import { AttendanceFromMonthlyRoasterComponent } from './attendance-from-monthly-roaster/attendance-from-monthly-roaster.component';
import { SkillsetComponent } from './skillset/skillset.component';
import { AddUpdateSkillsetComponent } from './skillset/add-update-skillset/add-update-skillset.component';
import { MaterialequipmentComponent } from './materialequipment/materialequipment.component';
import { AddUpdateMaterialequipmentComponent } from './materialequipment/add-update-materialequipment/add-update-materialequipment.component';
import { OperationexpenseheadsComponent } from './operationexpenseheads/operationexpenseheads.component';
import { AddUpdateOperationexpenseheadsComponent } from './operationexpenseheads/add-update-operationexpenseheads/add-update-operationexpenseheads.component';
import { TestChild2Component } from './test-example/test-child2/test-child2.component';
import { LogisticsandvehicleComponent } from './logisticsandvehicle/logisticsandvehicle.component';
import { AddUpdatelogisticsandvehicleComponent } from './logisticsandvehicle/add-updatelogisticsandvehicle/add-updatelogisticsandvehicle.component';
import { ShiftplanForProjectComponent } from './shift-master/shiftplan-for-project/shiftplan-for-project.component';
import { SkillsetPlanForProjectComponent } from './skillset/skillset-plan-for-project/skillset-plan-for-project.component';
import { ConvertToProjectComponent } from './convert-to-project/convert-to-project.component';
import { CalendarDaysComponent } from './project/calendar-days/calendar-days.component';
import { MonthylRoasterForProjectComponent } from './monthyl-roaster-for-project/monthyl-roaster-for-project.component';
import { ProjectBudgetCostingComponent } from './project-budget-costing/project-budget-costing.component';
import { ResourceCostingComponent } from './project-budget-costing/resource-costing/resource-costing.component';
import { LogisticsCostingComponent } from './project-budget-costing/logistics-costing/logistics-costing.component';
import { MaterialCostingComponent } from './project-budget-costing/material-costing/material-costing.component';
import { FinancialExpenseCostingComponent } from './project-budget-costing/financial-expense-costing/financial-expense-costing.component';
import { CostEstimationForProjectComponent } from './project-budget-costing/cost-estimation-for-project/cost-estimation-for-project.component';
import { EstimationConsolReportComponent } from './project-budget-costing/estimation-consol-report/estimation-consol-report.component';
import { EstimationSummaryReportComponent } from './project-budget-costing/estimation-summary-report/estimation-summary-report.component';
import { ApprovalServiceEnquiryComponent } from './view-enquiries-by-enquiry-number/approval-service-enquiry/approval-service-enquiry.component';
import { AuthoritiesComponent } from './authorities/authorities.component';
import { ApprovalDialogWindowComponent } from './approval-dialog-window/approval-dialog-window.component';
import { ConvertToContractComponent } from './project-budget-costing/convert-to-contract/convert-to-contract.component';
import { ViewProjectContractComponent } from './project-budget-costing/view-project-contract/view-project-contract.component';
import { AddUpdateAuthoritiesComponent } from './authorities/add-update-authorities/add-update-authorities.component';
import { PrintProposalComponent } from './project-budget-costing/print-proposal/print-proposal.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { PrintEstimationComponent } from './project-budget-costing/estimation-consol-report/print-estimation/print-estimation.component';
import { AssignEmployeesToProjectSiteComponent } from './project/assign-employees-to-project-site/assign-employees-to-project-site.component';
import { MappingEmpToResourceForProjectSiteComponent } from './project/mapping-emp-to-resource-for-project-site/mapping-emp-to-resource-for-project-site.component';
import { EmployeeAttendanceComponent } from './employee/employee-attendance/employee-attendance.component';
import { ManualAttendancePopupComponent } from './employee/employee-attendance/manual-attendance-popup/manual-attendance-popup.component';
import { UpdateManualAttendancePopupComponent } from './employee/employee-attendance/update-manual-attendance-popup/update-manual-attendance-popup.component';
import { ExistingProjectsComponent } from './existing-projects/existing-projects.component';
import { AddExistingProjectComponent } from './existing-projects/add-existing-project/add-existing-project.component';
import { ConfirmDialogWindowComponent } from './confirm-dialog-window/confirm-dialog-window.component';
import { AddResourceComponent } from './ProgectManagement/add-resource/add-resource.component';
import { RemoveResourceComponent } from './ProgectManagement/remove-resource/remove-resource.component';
import { TransferEmployeeComponent } from './ProgectManagement/transfer-employee/transfer-employee.component';
import { CreateUpdateAddResourceComponent } from './ProgectManagement/add-resource/create-update-add-resource/create-update-add-resource.component';
import { ViewPvAddResRequestComponent } from './ProgectManagement/add-resource/view-pv-add-res-request/view-pv-add-res-request.component';
import { CreateUpdateReqRemoveResourceComponent } from './ProgectManagement/remove-resource/create-update-req-remove-resource/create-update-req-remove-resource.component';
import { CreateUpdateTransferResourceReqComponent } from './ProgectManagement/transfer-employee/create-update-transfer-resource-req/create-update-transfer-resource-req.component';
import { ReplaceEmployeeComponent } from './ProgectManagement/replace-employee/replace-employee.component';
import { CreateUpdateReqReplaceEmployeeComponent } from './ProgectManagement/replace-employee/create-update-req-replace-employee/create-update-req-replace-employee.component';
import { ViewReqReplaceEmployeeComponent } from './ProgectManagement/replace-employee/view-req-replace-employee/view-req-replace-employee.component';
import { ViewReqRemoveResourceComponent } from './ProgectManagement/remove-resource/view-req-remove-resource/view-req-remove-resource.component';
import { PvEmpToResMapComponent } from './ProgectManagement/add-resource/pv-emp-to-res-map/pv-emp-to-res-map.component';
import { ProjectSitesComponent } from './project/project-sites/project-sites.component';
import { PvOpenCloseReqsComponent } from './ProgectManagement/pv-open-close-reqs/pv-open-close-reqs.component';
import { CreateUpdatePvOpenCloseReqComponent } from './ProgectManagement/pv-open-close-reqs/create-update-pv-open-close-req/create-update-pv-open-close-req.component';
import { ReportsComponent } from './reports/reports.component';
import { ProjectsSitesReportsComponent } from './reports/projects-sites-reports/projects-sites-reports.component';
import { ProjectsCountMatrixComponent } from './reports/projects-count-matrix/projects-count-matrix.component';
import { ResourcesOnProjectComponent } from './reports/resources-on-project/resources-on-project.component';
import { CountOfSkillsetsComponent } from './reports/count-of-skillsets/count-of-skillsets.component';
import { AllRequestsComponent } from './ProgectManagement/all-requests/all-requests.component';
import { CreateUpdateProposalTemplateComponent } from './project-budget-costing/create-update-proposal-template/create-update-proposal-template.component';
import { CreateUpdateProposalCostingsComponent } from './project-budget-costing/create-update-proposal-costings/create-update-proposal-costings.component';
import { UpdateEmployeeShiftComponent } from './employee/employee-attendance/manual-attendance-popup/update-employee-shift/update-employee-shift.component';
import { ContractFormComponent } from './project/contract-form/contract-form.component';
import { ContractFormTemplatesComponent } from './contract-form-templates/contract-form-templates.component';
import { ContractFormElementsComponent } from './contract-form-templates/contract-form-elements/contract-form-elements.component';
import { CreateUpdateContractFormComponent } from './project/contract-form/create-update-contract-form/create-update-contract-form.component';
import { CreateUpdateContractFormTemplateComponent } from './contract-form-templates/create-update-contract-form-template/create-update-contract-form-template.component';
import { CreateUpdateContractFormElementsComponent } from './contract-form-templates/contract-form-elements/create-update-contract-form-elements/create-update-contract-form-elements.component';
import { PostattendancewithdateComponent } from './employee/employee-attendance/postattendancewithdate/postattendancewithdate.component';
import { TransferWithReplacementComponent } from './ProgectManagement/transfer-with-replacement/transfer-with-replacement.component';
import { CreateUpdateTransferWithReplacementComponent } from './ProgectManagement/transfer-with-replacement/create-update-transfer-with-replacement/create-update-transfer-with-replacement.component';
import { SwapEmployeesComponent } from './ProgectManagement/swap-employees/swap-employees.component';
import { CreateUpdateSwapEmployeesComponent } from './ProgectManagement/swap-employees/create-update-swap-employees/create-update-swap-employees.component';
import { EmployeePrimarysitelogsComponent } from './employee/employee-primarysitelogs/employee-primarysitelogs.component';
import { EmployeeprimarysitelogsComponent } from './employee/employee-primarysitelogs/employeeprimarysitelogs/employeeprimarysitelogs.component';
import { AddupdateEmployeeprimarysitelogComponent } from './employee/employee-primarysitelogs/employeeprimarysitelogs/addupdate-employeeprimarysitelog/addupdate-employeeprimarysitelog.component';
import { ClearattendancewithdateComponent } from './employee/employee-attendance/clearattendancewithdate/clearattendancewithdate.component';
import { PvRequestsPaginationByProjectSiteComponent } from './ProgectManagement/pv-requests-pagination-by-project-site/pv-requests-pagination-by-project-site.component';
import { AttendancePayrollReportComponent } from './employee/employee-attendance/attendance-payroll-report/attendance-payroll-report.component';
import { ReasoncodesComponent } from './reasoncodes/reasoncodes.component';
import { AddupdateReasoncodeComponent } from './sharedpages/addupdate-reasoncode/addupdate-reasoncode.component';
import { CustomerVisitFormsComponent } from './customer-visit-forms/customer-visit-forms.component';
import { AddupdateCustomerVistFormComponent } from './customer-visit-forms/addupdate-customer-vist-form/addupdate-customer-vist-form.component';
import { CustomerComplaintsComponent } from './customer-complaints/customer-complaints.component';
import { AddupdateCustomerComplaintComponent } from './customer-complaints/addupdate-customer-complaint/addupdate-customer-complaint.component';
import { ReportCustomercomplaintsComponent } from './reports/report-customercomplaints/report-customercomplaints.component';
import { ReportCustomervisitsComponent } from './reports/report-customervisits/report-customervisits.component';
import { UploadAdendumComponent } from './upload-adendum/upload-adendum.component';
import { AddendumFormComponent } from './project/project-sites/addendum-form/addendum-form.component';
import { UploadFileComponent } from './upload-file/upload-file.component';
import { AttendanceStatusReportComponent }
from './reports/attendance-status-report/attendance-status-report.component';
import { NgxPrintModule } from 'ngx-print';
import { AttendancePayrollReport2Component } from './employee/employee-attendance/attendance-payroll-report/attendance-payroll-report2.component';
import { Project2Component } from './project/project2.component';
import { ProjectSites2Component } from './project/project-sites/project-sites2.component';
import { ConfirmDialogWindowGeneralComponent } from './confirm-dialog-window/confirm-dialog-window-general.component';
import { NgApexchartsModule } from 'ng-apexcharts';
import { UpdatestandarddevationComponent } from './sharedpages/addupdatesites/updatestandarddevation.component';



@NgModule({
  declarations: [
    OpcustomerComponent,
   AddupdatecustomerComponent,
    AddupdatesitesComponent,
    CustomerSitesComponent,
    ServiceCodeComponent,
    AddupdateServiceCodeComponent,
    ServicesEnquiryComponent,
    AddupdateServicesEnquiryComponent,
    SurveyFormComponent,
    AddupdateSurveyFormComponent,
    SurveyFormElementsComponent,
    AddupdateSurveyFormElementsComponent,
    SurveyorComponent,
    AddupdateSurveyorComponent,
    UnitCodeForServicesComponent,
    AddupdateUnitCodeForServicesComponent,
    AddupdateServiceunitmappComponent,
    ServiceUnitMappingsComponent,
    AddSiteToCustomerComponent,
    AddupdateEnquiryFormComponent,
    ServiceEnquiryFormsComponent,
    AddSurveyorToEnquiryComponent,
    ViewEnquiriesByEnquiryNumberComponent,
    PrintEnquiriesByEnquiryNumberComponent,
    AttachSurveyFormToServiceComponent,
    EnquiriesBySurveyorCodeComponent,
    PrintSurveyFormComponent,
    ViewSurveyFormTemplateComponent,
    SitesListByCustomerCodeComponent,
    EditableSurveyFormComponent,
    EmployeeComponent,
    AddUpdateEmployeeComponent,
    ProjectComponent,
    Project2Component,
    //AddUpdateProjectComponent,
    ShiftMasterComponent,
    AddUpdateShiftComponent,
    PayrollGroupComponent,
    AddUpdatePayrollGroupComponent,
    ShiftsToSiteMapComponent,
    MonthlyRoasterComponent,
    //TestExampleComponent,
    TestChildComponent,
    AttendanceFromMonthlyRoasterComponent,
    SkillsetComponent,
    AddUpdateSkillsetComponent,
    MaterialequipmentComponent,
    AddUpdateMaterialequipmentComponent,
    OperationexpenseheadsComponent,
    AddUpdateOperationexpenseheadsComponent,
    TestChild2Component,
    LogisticsandvehicleComponent,
    AddUpdatelogisticsandvehicleComponent,
    ShiftplanForProjectComponent,
    SkillsetPlanForProjectComponent,
    ConvertToProjectComponent,
    CalendarDaysComponent,
    MonthylRoasterForProjectComponent,
    ProjectBudgetCostingComponent,
    ResourceCostingComponent,
    LogisticsCostingComponent,
    MaterialCostingComponent,
    FinancialExpenseCostingComponent,
    CostEstimationForProjectComponent,
    EstimationConsolReportComponent,
    EstimationSummaryReportComponent,
    ApprovalServiceEnquiryComponent,
    AuthoritiesComponent,
    ApprovalDialogWindowComponent,
    ConvertToContractComponent,
    ViewProjectContractComponent,
    AddUpdateAuthoritiesComponent,
    PrintProposalComponent,
    PrintEstimationComponent,
    AssignEmployeesToProjectSiteComponent,
    MappingEmpToResourceForProjectSiteComponent,
    EmployeeAttendanceComponent,
    ManualAttendancePopupComponent,
    UpdateManualAttendancePopupComponent,
    ExistingProjectsComponent,
    AddExistingProjectComponent,
    ConfirmDialogWindowComponent,
    ConfirmDialogWindowGeneralComponent,
    AddResourceComponent,
    RemoveResourceComponent,
    TransferEmployeeComponent,
    CreateUpdateAddResourceComponent,
    ViewPvAddResRequestComponent,
    CreateUpdateReqRemoveResourceComponent,
    CreateUpdateTransferResourceReqComponent,
    ReplaceEmployeeComponent,
    CreateUpdateReqReplaceEmployeeComponent,
    ViewReqReplaceEmployeeComponent,
    ViewReqRemoveResourceComponent,
    PvEmpToResMapComponent,
    ProjectSitesComponent,
    ProjectSites2Component,
    PvOpenCloseReqsComponent,
    CreateUpdatePvOpenCloseReqComponent,
    ReportsComponent,
    ProjectsSitesReportsComponent,
    ProjectsCountMatrixComponent,
    ResourcesOnProjectComponent,
    CountOfSkillsetsComponent,
    AllRequestsComponent,
    CreateUpdateProposalTemplateComponent,
    CreateUpdateProposalCostingsComponent,
    UpdateEmployeeShiftComponent,
    ContractFormComponent,
    ContractFormTemplatesComponent,
    ContractFormElementsComponent,
    CreateUpdateContractFormComponent,
    CreateUpdateContractFormTemplateComponent,
    CreateUpdateContractFormElementsComponent,
    PostattendancewithdateComponent,
    TransferWithReplacementComponent,
    CreateUpdateTransferWithReplacementComponent,
    SwapEmployeesComponent,
    CreateUpdateSwapEmployeesComponent,
    EmployeePrimarysitelogsComponent,
    EmployeeprimarysitelogsComponent,
    AddupdateEmployeeprimarysitelogComponent,
    ClearattendancewithdateComponent,
    PvRequestsPaginationByProjectSiteComponent,
    AttendancePayrollReportComponent,
    AttendancePayrollReport2Component,
    ReasoncodesComponent,
    AddupdateReasoncodeComponent,
    CustomerVisitFormsComponent,
    AddupdateCustomerVistFormComponent,
    CustomerComplaintsComponent,
    AddupdateCustomerComplaintComponent,
    ReportCustomercomplaintsComponent,
    ReportCustomervisitsComponent,
    UploadAdendumComponent,
    AddendumFormComponent,
    UploadFileComponent,
    AttendanceStatusReportComponent,
    UpdatestandarddevationComponent,
    //TempProjectComponent,
    //TempPagesComponent,
   // TestExampleComponent
   ],
  imports: [   
    OperationalmgtRoutingModule,
    SharedModule,
   ],
  providers: [DatePipe],
  exports: [CommonModule],
})
export class OperationalmgtModule { }
