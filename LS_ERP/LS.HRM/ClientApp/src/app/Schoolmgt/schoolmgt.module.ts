import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from '../sharedcomponent/shared.module';
import { SchoolmgtRoutingModule } from './schoolmgt-routing.module';
import { SchoolacademicbatchesComponent } from './schoolacademicbatches/schoolacademicbatches.component';
import { AddupdateschoolacademicbatchesComponent } from './shared/addupdateschoolacademicbatches/addupdateschoolacademicbatches.component';
import { SchoolacademicgradeComponent } from './schoolacademicgrade/schoolacademicgrade.component';
import { SchoolacademicsectionsComponent } from './schoolacademicsections/schoolacademicsections.component';
import { SchoolacademicsubjectsComponent } from './schoolacademicsubjects/schoolacademicsubjects.component';
import { GradesectionmappingComponent } from './gradesectionmapping/gradesectionmapping.component';
import { SemesterssubjectmappingwithgradeComponent } from './semesterssubjectmappingwithgrade/semesterssubjectmappingwithgrade.component';
import { AddupdateschoolacademicsubjectsComponent } from './shared/addupdateschoolacademicsubjects/addupdateschoolacademicsubjects.component';
import { AddupdateschoolacademicsectionsComponent } from './shared/addupdateschoolacademicsections/addupdateschoolacademicsections.component';
import { AddupdateschoolacademicgradeComponent } from './shared/addupdateschoolacademicgrade/addupdateschoolacademicgrade.component';
import { AddupdateschoolLanguagesComponent } from './shared/addupdateschool-languages/addupdateschool-languages.component';
import { AddupdateSchoolNationalityComponent } from './shared/addupdate-school-nationality/addupdate-school-nationality.component';
import { AddupdateSchoolPtCategoryComponent } from './shared/addupdate-school-pt-category/addupdate-school-pt-category.component';
import { AddupdateSchoolReligionComponent } from './shared/addupdate-school-religion/addupdate-school-religion.component';
import { AddupdteSchoolFeeTermsComponent } from './shared/addupdte-school-fee-terms/addupdte-school-fee-terms.component';
import { SchoolFeeTermsComponent } from './school-fee-terms/school-fee-terms.component';
import { SchoolLanguagesComponent } from './school-languages/school-languages.component';
import { SchoolNationalityComponent } from './school-nationality/school-nationality.component';
import { SchoolPtCategoryComponent } from './school-pt-category/school-pt-category.component';
import { SchoolReligionComponent } from './school-religion/school-religion.component';
import { TeacherClassMappingComponent } from './teacher-class-mapping/teacher-class-mapping.component';
import { TeacherLanguageMappingComponent } from './teacher-language-mapping/teacher-language-mapping.component';
import { TeacherMasterComponent } from './teacher-master/teacher-master.component';
import { TeacherQualificationMappingComponent } from './teacher-qualification-mapping/teacher-qualification-mapping.component';
import { TeacherSubjectMappingComponent } from './teacher-subject-mapping/teacher-subject-mapping.component';
import { AddupdateTeacherMasterComponent } from './shared/addupdate-teacher-master/addupdate-teacher-master.component';
import { NotificationComponent } from './notification/notification.component';
import { SchoolFeePaymentTypeComponent } from './school-fee-payment-type/school-fee-payment-type.component';
import { SchoolFeeStructureComponent } from './school-fee-structure/school-fee-structure.component';
import { SchoolFeeTypeComponent } from './school-fee-type/school-fee-type.component';
import { SchoolLeavesTypeComponent } from './school-leaves-type/school-leaves-type.component';
import { StudentRegistrationComponent } from './student-registration/student-registration.component';
import { ApproveNotificationComponent } from './shared/approve-notification/approve-notification.component';
import { AddupdteSchoolLeavesTypeComponent } from './shared/addupdte-school-leaves-type/addupdte-school-leaves-type.component';
import { AddupdateStudentRegistrationComponent } from './shared/addupdate-student-registration/addupdate-student-registration.component';
import { AddupdateSchoolFeeTypeComponent } from './shared/addupdate-school-fee-type/addupdate-school-fee-type.component';
import { AddupdateSchoolFeeStructureComponent } from './shared/addupdate-school-fee-structure/addupdate-school-fee-structure.component';
import { AddupdateSchoolFeePaymentTypeComponent } from './shared/addupdate-school-fee-payment-type/addupdate-school-fee-payment-type.component';
import { AddupdateNotificationComponent } from './shared/addupdate-notification/addupdate-notification.component';
import { AddupdateStudentMasterComponent } from './shared/addupdate-student-master/addupdate-student-master.component';
import { StudentApplyLeaveComponent } from './shared/student-apply-leave/student-apply-leave.component';
import { StudentAttendanceComponent } from './shared/student-attendance/student-attendance.component';
import { StudentAcademicsComponent } from './shared/student-academics/student-academics.component';
import { StudentFeeMasterComponent } from './shared/student-fee-master/student-fee-master.component';
import { StudentNoticeAndMessagingComponent } from './shared/student-notice-and-messaging/student-notice-and-messaging.component';
import { StudentMasterComponent } from './student-master/student-master.component';
import { SchoolAcademicSemestersComponent } from './school-academic-semesters/school-academic-semesters.component';
import { AddupdatateSchoolAcademicSemestersComponent } from './shared/addupdatate-school-academic-semesters/addupdatate-school-academic-semesters.component';
import { WebStudentRegistrationListComponent } from './shared/web-student-registration-list/web-student-registration-list.component';
import { StudentAddressComponent } from './shared/student-address/student-address.component';
import { StudentSiblingDataComponent } from './shared/student-sibling-data/student-sibling-data.component';


import { EventsComponent } from './events/events.component';
import { ExaminationManagementComponent } from './examination-management/examination-management.component';
import { HomeHorkComponent } from './home-hork/home-hork.component';
import { LessonPlanComponent } from './lesson-plan/lesson-plan.component';
import { ParametersForExamsComponent } from './parameters-for-exams/parameters-for-exams.component';
import { StudentResultComponent } from './student-result/student-result.component';
import { AcademicFeeTransactionComponent } from './academic-fee-transaction/academic-fee-transaction.component';
import { FeeDiscountTransactionComponent } from './fee-discount-transaction/fee-discount-transaction.component';
import { StudentReasoncodeComponent } from './student-reasoncode/student-reasoncode.component';
import { StudentRegisterComponent } from './student-register/student-register.component';

import { AddupdateEventsComponent } from './shared/addupdate-events/addupdate-events.component';
import { AddupdateExaminationManagementComponent } from './shared/addupdate-examination-management/addupdate-examination-management.component';
import { AddupdateHomeHorkComponent } from './shared/addupdate-home-hork/addupdate-home-hork.component';
import { AddupdateLessonPlanComponent } from './shared/addupdate-lesson-plan/addupdate-lesson-plan.component';
import { AddupdateParametersForExamsComponent } from './shared/addupdate-parameters-for-exams/addupdate-parameters-for-exams.component';
import { AddupdateStudentResultComponent } from './shared/addupdate-student-result/addupdate-student-result.component';
import { AddupdateAcademicFeeTransactionComponent } from './shared/addupdate-academic-fee-transaction/addupdate-academic-fee-transaction.component';
import { AddupdateFeeDiscountTransactionComponent } from './shared/addupdate-fee-discount-transaction/addupdate-fee-discount-transaction.component';
import { FeeinvoiceComponent } from './shared/feeinvoice/feeinvoice.component';
import { AddupdateStudentReasoncodeComponent } from './shared/addupdate-student-reasoncode/addupdate-student-reasoncode.component';
import { SchoolBranchesComponent } from './school-branches/school-branches.component';
import { AddupdateSchoolBranchComponent } from './shared/addupdate-school-branch/addupdate-school-branch.component';
import { CalendarComponent } from './calendar/calendar.component';
import { TeacherNotificationComponent } from './teacher-notification/teacher-notification.component';
import { AddUpdateHolidayComponent } from './shared/add-update-holiday/add-update-holiday.component';
import { AddUpdateStudentNotificationComponent } from './shared/add-update-student-notification/add-update-student-notification.component';
import { BranchCalendarComponent } from './shared/branch-calendar/branch-calendar.component';
import { SeeAllNotificationsComponent } from './see-all-notifications/see-all-notifications.component';
import { TermDuePaymentComponent } from './term-due-payment/term-due-payment.component';
import { StudentFeeHistoryComponent } from './shared/student-fee-history/student-fee-history.component';
import { AcademicYearFeeAnalysisComponent } from './academic-year-fee-analysis/academic-year-fee-analysis.component';
import { StudentListReportComponent } from './student-list-report/student-list-report.component';
import { FeeStructureSummaryComponent } from './fee-structure-summary/fee-structure-summary.component';
import { FeeStructureDetailsComponent } from './shared/fee-structure-details/fee-structure-details.component';
import { AddUpdateGradeDocumentComponent } from './shared/add-update-grade-document/add-update-grade-document.component';
import { SchoolinvoiceprintingComponent } from './shared/schoolinvoiceprinting/schoolinvoiceprinting.component';
import { AddupdateAcademicBulkFeeTransactionComponent } from './shared/addupdate-academic-bulkfee-transaction/addupdate-academic-bulkfee-transaction.component';



@NgModule({
  declarations: [
    SchoolacademicbatchesComponent,
    AddupdateschoolacademicbatchesComponent,
    SchoolacademicgradeComponent,
    SchoolacademicsectionsComponent,
    SchoolacademicsubjectsComponent,
    GradesectionmappingComponent,
    SemesterssubjectmappingwithgradeComponent,
    AddupdateschoolacademicsubjectsComponent,
    AddupdateschoolacademicsectionsComponent,
    AddupdateschoolacademicgradeComponent,
    AddupdateschoolLanguagesComponent,
    AddupdateSchoolNationalityComponent,
    AddupdateSchoolPtCategoryComponent,
    AddupdateSchoolReligionComponent,
    AddupdteSchoolFeeTermsComponent,
    SchoolFeeTermsComponent,
    SchoolLanguagesComponent,
    SchoolNationalityComponent,
    SchoolPtCategoryComponent,
    SchoolReligionComponent,
    TeacherClassMappingComponent,
    TeacherLanguageMappingComponent,
    TeacherMasterComponent,
    TeacherQualificationMappingComponent,
    TeacherSubjectMappingComponent,
    AddupdateTeacherMasterComponent,
    NotificationComponent,
    SchoolFeePaymentTypeComponent,
    SchoolFeeStructureComponent,
    SchoolFeeTypeComponent,
    SchoolLeavesTypeComponent,
    StudentRegistrationComponent,
    ApproveNotificationComponent,
    AddupdteSchoolLeavesTypeComponent,
    AddupdateStudentRegistrationComponent,
    AddupdateSchoolFeeTypeComponent,
    AddupdateSchoolFeeStructureComponent,
    AddupdateSchoolFeePaymentTypeComponent,
    AddupdateNotificationComponent,
    AddupdateStudentMasterComponent,
    StudentApplyLeaveComponent,
    StudentAttendanceComponent,
    StudentAcademicsComponent,
    StudentFeeMasterComponent,
    StudentNoticeAndMessagingComponent,
    StudentMasterComponent,
    SchoolAcademicSemestersComponent,
    AddupdatateSchoolAcademicSemestersComponent,
    WebStudentRegistrationListComponent,
    StudentAddressComponent,
    StudentSiblingDataComponent,
    EventsComponent,
    ExaminationManagementComponent,
    HomeHorkComponent,
    LessonPlanComponent,
    ParametersForExamsComponent,
    StudentResultComponent,
    StudentReasoncodeComponent,
    AddupdateEventsComponent,
    AddupdateExaminationManagementComponent,
    AddupdateHomeHorkComponent,
    AddupdateLessonPlanComponent,
    AddupdateParametersForExamsComponent,
    AddupdateStudentResultComponent,
    AcademicFeeTransactionComponent,
    FeeDiscountTransactionComponent,
    AddupdateAcademicFeeTransactionComponent,
    AddupdateAcademicBulkFeeTransactionComponent,
    AddupdateFeeDiscountTransactionComponent,
    FeeinvoiceComponent,
    AddupdateFeeDiscountTransactionComponent,
    AddupdateStudentReasoncodeComponent,
    StudentRegisterComponent,
    SchoolBranchesComponent,
    AddupdateSchoolBranchComponent,
    CalendarComponent,
    TeacherNotificationComponent,
    AddUpdateHolidayComponent,
    AddUpdateStudentNotificationComponent,
    BranchCalendarComponent,
    SeeAllNotificationsComponent,
    TermDuePaymentComponent,
    StudentFeeHistoryComponent,
    AcademicYearFeeAnalysisComponent,
    StudentListReportComponent,
    FeeStructureSummaryComponent,
    FeeStructureDetailsComponent,
    AddUpdateGradeDocumentComponent,
    SchoolinvoiceprintingComponent,
  ],
  imports: [
    SchoolmgtRoutingModule,
    SharedModule
    
  ],
  exports: [CommonModule],
})

export class SchoolmgtModule {
  
}
