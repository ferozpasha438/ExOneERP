import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GradesectionmappingComponent } from './gradesectionmapping/gradesectionmapping.component';
import { NotificationComponent } from './notification/notification.component';
import { SchoolAcademicSemestersComponent } from './school-academic-semesters/school-academic-semesters.component';
import { SchoolFeePaymentTypeComponent } from './school-fee-payment-type/school-fee-payment-type.component';
import { SchoolFeeStructureComponent } from './school-fee-structure/school-fee-structure.component';
import { SchoolFeeTermsComponent } from './school-fee-terms/school-fee-terms.component';
import { SchoolFeeTypeComponent } from './school-fee-type/school-fee-type.component';
import { SchoolLanguagesComponent } from './school-languages/school-languages.component';
import { SchoolLeavesTypeComponent } from './school-leaves-type/school-leaves-type.component';
import { SchoolNationalityComponent } from './school-nationality/school-nationality.component';
import { SchoolPtCategoryComponent } from './school-pt-category/school-pt-category.component';
import { SchoolReligionComponent } from './school-religion/school-religion.component';
import { SchoolacademicbatchesComponent } from './schoolacademicbatches/schoolacademicbatches.component';
import { SchoolacademicgradeComponent } from './schoolacademicgrade/schoolacademicgrade.component';
import { SchoolacademicsectionsComponent } from './schoolacademicsections/schoolacademicsections.component';
import { SchoolacademicsubjectsComponent } from './schoolacademicsubjects/schoolacademicsubjects.component';
import { SemesterssubjectmappingwithgradeComponent } from './semesterssubjectmappingwithgrade/semesterssubjectmappingwithgrade.component';
import { StudentMasterComponent } from './student-master/student-master.component';
import { StudentRegistrationComponent } from './student-registration/student-registration.component';
import { TeacherMasterComponent } from './teacher-master/teacher-master.component';

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
import { SchoolBranchesComponent } from './school-branches/school-branches.component';
import { CalendarComponent } from './calendar/calendar.component';
import { SeeAllNotificationsComponent } from './see-all-notifications/see-all-notifications.component';
import { TermDuePaymentComponent } from './term-due-payment/term-due-payment.component';
import { AcademicYearFeeAnalysisComponent } from './academic-year-fee-analysis/academic-year-fee-analysis.component';
import { StudentListReportComponent } from './student-list-report/student-list-report.component';
import { FeeStructureSummaryComponent } from './fee-structure-summary/fee-structure-summary.component';

const routes: Routes = [/*{ path: '', component: PurchasesetupComponent }*/
  { path: 'schoolacademicbatches', component: SchoolacademicbatchesComponent },
  //  { path: 'gradesectionmapping', component: GradesectionmappingComponent },
  { path: 'schoolacademicgrade', component: SchoolacademicgradeComponent },
  { path: 'schoolacademicsections', component: SchoolacademicsectionsComponent },
  { path: 'schoolacademicsubjects', component: SchoolacademicsubjectsComponent },
  { path: 'schoolacademicsemesters', component: SchoolAcademicSemestersComponent },

  { path: 'schoolFeeTerms', component: SchoolFeeTermsComponent },
  { path: 'schoolLanguages', component: SchoolLanguagesComponent },
  { path: 'schoolNationality', component: SchoolNationalityComponent },
  { path: 'schoolPtCategory', component: SchoolPtCategoryComponent },
  { path: 'schoolReligion', component: SchoolReligionComponent },
  { path: 'schoolBranches', component: SchoolBranchesComponent },

  ////registration
  { path: 'notification', component: NotificationComponent },
  { path: 'feePaymentType', component: SchoolFeePaymentTypeComponent },
  { path: 'feeStructure', component: SchoolFeeStructureComponent },
  { path: 'feeType', component: SchoolFeeTypeComponent },
  { path: 'leavesType', component: SchoolLeavesTypeComponent },
  { path: 'registration', component: StudentRegistrationComponent },

  { path: 'allnotifications', component: SeeAllNotificationsComponent },

  ////students
  { path: 'studentmaster', component: StudentMasterComponent },
  { path: 'studentresult', component: StudentResultComponent },
  { path: 'parametersforexams', component: ParametersForExamsComponent },
  { path: 'lessonplan', component: LessonPlanComponent },
  { path: 'homework', component: HomeHorkComponent },
  { path: 'examination', component: ExaminationManagementComponent },
  { path: 'events', component: EventsComponent },
  { path: 'academicfee', component: AcademicFeeTransactionComponent },
  { path: 'feediscount', component: FeeDiscountTransactionComponent },
  { path: 'studentReasoncode', component: StudentReasoncodeComponent },
  { path: 'calendar', component: CalendarComponent },
 
  ////teachers
  { path: 'teachermaster', component: TeacherMasterComponent },
  { path: 'studentAttendanceRegister', component: StudentRegisterComponent },

  //// Reports
  { path: 'termduepayment', component: TermDuePaymentComponent },
  { path: 'academic-year-fee-analysis', component: AcademicYearFeeAnalysisComponent },
  { path: 'student-fee-list', component: StudentListReportComponent },
  { path: 'fee-structure-summary', component: FeeStructureSummaryComponent }


];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SchoolmgtRoutingModule { }
