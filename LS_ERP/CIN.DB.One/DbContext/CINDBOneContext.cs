using CIN.Domain;
using CIN.Domain.FinanceMgt;
using CIN.Domain.FleetMgt;
using CIN.Domain.GeneralLedger;
using CIN.Domain.GeneralLedger.BankVoucher;
using CIN.Domain.GeneralLedger.CashVoucher;
using CIN.Domain.GeneralLedger.Distribution;
using CIN.Domain.GeneralLedger.Ledger;
using CIN.Domain.HumanResource.EmployeeMgt;
using CIN.Domain.HumanResource.Setup;
using CIN.Domain.InventoryMgt;
using CIN.Domain.InventorySetup;
using CIN.Domain.InvoiceSetup;
using CIN.Domain.OpeartionsMgt;
using CIN.Domain.PurchaseMgt;
using CIN.Domain.PurchaseSetup;
using CIN.Domain.SalesSetup;
using CIN.Domain.SchoolMgt;
using CIN.Domain.SND;
using CIN.Domain.SndQuotationSetup;
using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;
using CIN.Domain.MobileMgt;
using CIN.Domain.TimeAndAttendance.Setup;
using CIN.Domain.TimeAndAttendance.Management;
using CIN.Domain.Payroll.Setup;
using CIN.Domain.Payroll.Management;
using CIN.Domain.HumanResource.ServiceRequest;

namespace CIN.DB
{
    public class CINDBOneContext : DbContext
    {
        public CINDBOneContext()
        {

        }

        public CINDBOneContext(DbContextOptions<CINDBOneContext> options)
            : base(options)
        {

        }

        #region GeneralLedger

        #region FinDistribution
        public DbSet<TblFinTrnDistribution> FinDistributions { get; set; }
        public DbSet<TblFinTrnAccountsLedger> AccountsLedgers { get; set; }
        public DbSet<TblFinTrnTrialBalance> TrialBalances { get; set; }

        #endregion


        #region JournalVoucher

        public DbSet<TblFinTrnJournalVoucher> JournalVouchers { get; set; }
        public DbSet<TblFinTrnJournalVoucherItem> JournalVoucherItems { get; set; }
        public DbSet<TblFinTrnJournalVoucherApproval> JournalVoucherApprovals { get; set; }
        public DbSet<TblFinTrnJournalVoucherStatement> JournalVoucherStatements { get; set; }

        #endregion

        #region BankVoucher

        public DbSet<TblFinTrnBankVoucher> BankVouchers { get; set; }
        public DbSet<TblFinTrnBankVoucherItem> BankVoucherItems { get; set; }
        public DbSet<TblFinTrnBankVoucherApproval> BankVoucherApprovals { get; set; }
        public DbSet<TblFinTrnBankVoucherStatement> BankVoucherStatements { get; set; }

        #endregion

        #region CashVoucher

        public DbSet<TblFinTrnCashVoucher> CashVouchers { get; set; }
        public DbSet<TblFinTrnCashVoucherItem> CashVoucherItems { get; set; }
        public DbSet<TblFinTrnCashVoucherApproval> CashVoucherApprovals { get; set; }
        public DbSet<TblFinTrnCashVoucherStatement> CashVoucherStatements { get; set; }

        #endregion


        #endregion

        #region System Setup

        public DbSet<TblErpSysCountryCode> CountryCodes { get; set; }
        public DbSet<TblErpSysStateCode> StateCodes { get; set; }
        public DbSet<TblErpSysCityCode> CityCodes { get; set; }
        public DbSet<TblErpSysCurrencyCode> CurrencyCodes { get; set; }
        public DbSet<TblErpSysCompany> Companies { get; set; }
        public DbSet<TblErpSysCompanyBranch> CompanyBranches { get; set; }
        public DbSet<TblErpSysSystemTax> SystemTaxes { get; set; }
        public DbSet<TblErpSysTransactionCode> TransactionCodes { get; set; }
        public DbSet<TblErpSysTransactionSequence> TransactionSequences { get; set; }
        public DbSet<TblErpSysUserBranch> UserBranches { get; set; }
        public DbSet<TblErpSysUserType> UserTypes { get; set; }
        public DbSet<TblErpSysLogin> SystemLogins { get; set; }
        public DbSet<TblErpSysMenuLoginId> MenuLoginIds { get; set; }
        public DbSet<TblErpSysMenuOption> MenuOptions { get; set; }
        public DbSet<TblErpSysAcCodeSegment> AcCodeSegments { get; set; }
        public DbSet<TblErpSysZoneSetting> ZoneSettings { get; set; }
        public DbSet<TblErpSysUserSiteMapping> UserSiteMappings { get; set; }
        public DbSet<TblErpSysIncidentReport> IncidentReports { get; set; }
        public DbSet<TblFinSysSegmentSetup> SegmentSetups { get; set; }
        public DbSet<TblFinSysSegmentTwoSetup> SegmentTwoSetups { get; set; }
        public DbSet<TblFinSysBatchSetup> BatchSetups { get; set; }
        public DbSet<TblFinSysCostAllocationSetup> CostAllocationSetups { get; set; }
        public DbSet<TblErpSysFileUpload> FileUploads { get; set; }

        #endregion

        #region Finance Management
        public DbSet<TblFinDefAccountBranches> FinAccountBranches { get; set; }
        public DbSet<TblFinDefAccountBranchMapping> FinAccountBranchMappings { get; set; }
        public DbSet<TblFinDefBranchesAuthority> FinBranchesAuthorities { get; set; }
        public DbSet<TblFinDefAccountCategory> FinAccountCategories { get; set; }
        public DbSet<TblFinDefAccountlPaycodes> FinAccountlPaycodes { get; set; }
        public DbSet<TblFinDefAccountSubCategory> FinAccountSubCategories { get; set; }
        public DbSet<TblFinDefBankCheckLeaves> FinBankCheckLeaves { get; set; }
        public DbSet<TblFinDefBranchesMainAccounts> FinBranchesMainAccounts { get; set; }
        public DbSet<TblFinDefMainAccounts> FinMainAccounts { get; set; }
        public DbSet<TblFinSysAccountType> FinSysAccountTypes { get; set; }
        public DbSet<TblFinSysFinanialSetup> FinSysFinanialSetups { get; set; }
        public DbSet<TblFinDefCenters> FinDefCenters { get; set; }
        #endregion

        #region InventorySetup
        public DbSet<TblInvDefCategory> InvCategories { get; set; }
        public DbSet<TblInvDefClass> InvClasses { get; set; }
        public DbSet<TblInvDefDistributionGroup> InvDistributionGroups { get; set; }
        public DbSet<TblInventoryDefDistributionGroup> InvPoDistributionGroups { get; set; }
        public DbSet<TblInvDefInventoryConfig> InvInventoryConfigs { get; set; }
        public DbSet<TblInvDefSubCategory> InvSubCategories { get; set; }
        public DbSet<TblInvDefSubClass> InvSubClasses { get; set; }
        public DbSet<TblInvDefUOM> InvUoms { get; set; }
        public DbSet<TblInvDefWarehouse> InvWarehouses { get; set; }

        public DbSet<TblInvDefWarehouseTest> TblInvDefWarehouseTest { get; set; }
        public DbSet<TblErpInvItemMaster> InvItemMaster { get; set; }
        public DbSet<TblErpInvItemInventory> InvItemInventory { get; set; }
        public DbSet<TblErpInvItemExpiryBatch> InvItemExpiryBatches { get; set; }
        public DbSet<TblErpInvItemSerialBatch> InvItemSerialBatches { get; set; }
        public DbSet<TblErpInvItemSpecification> InvItemSpecifications { get; set; }

        public DbSet<TblErpInvItemsBarcode> InvItemsBarcode { get; set; }
        public DbSet<TblErpInvItemsUOM> InvItemsUOM { get; set; }
        public DbSet<TblErpInvItemInventoryHistory> InvItemInventoryHistory { get; set; }
        public DbSet<TblErpInvItemNotes> InvItemNotes { get; set; }

        public DbSet<TblErpInvItemtype> TblInvDefType { get; set; }
        public DbSet<TblErpInvItemtracking> TblInvDefTracking { get; set; }

        #endregion

        #region InventoryMgt
        public DbSet<TblIMTransactionHeader> IMTransactionHeader { get; set; }
        public DbSet<TblIMTransactionDetails> IMTransactionDetails { get; set; }

        public DbSet<TblIMReceiptsTransactionHeader> IMReceiptsTransactionHeader { get; set; }
        public DbSet<TblIMReceiptsTransactionDetails> IMReceiptsTransactionDetails { get; set; }

        public DbSet<TblIMAdjustmentsTransactionHeader> IMAdjustmentsTransactionHeader { get; set; }
        public DbSet<TblIMAdjustmentsTransactionDetails> IMAdjustmentsTransactionDetails { get; set; }


        public DbSet<TblIMTransferTransactionHeader> IMTransferTransactionHeader { get; set; }
        public DbSet<TblIMTransferTransactionDetails> IMTransferTransactionDetails { get; set; }


        public DbSet<TblIMStockReconciliationTransactionHeader> IMStockReconciliationTransactionHeader { get; set; }
        public DbSet<TblIMStockReconciliationTransactionDetails> IMStockReconciliationTransactionDetails { get; set; }

        public DbSet<Adjustments> Adjs { get; set; }

        #endregion

        #region PurchaseSetup
        public DbSet<TblSndDefVendorMaster> VendorMasters { get; set; }

        public DbSet<TblPopDefVendorCategory> PopVendorCategories { get; set; }
        public DbSet<TblPopDefVendorPOTermsCode> PopVendorPOTermsCodes { get; set; }
        public DbSet<TblPopDefVendorShipment> PopVendorShipments { get; set; }

        #endregion

        #region PurchaseMgt          
        public DbSet<TblPopTrnPurchaseOrderHeader> purchaseOrderHeaders { get; set; }

        public DbSet<TblPopTrnPurchaseOrderDetails> purchaseOrderDetails { get; set; }

        public DbSet<TblPopTrnPurchaseReturnHeader> purchaseReturnHeader { get; set; }

        public DbSet<TblPopTrnPurchaseReturnDetails> purchaseReturnDetails { get; set; }
        public DbSet<TblPurAuthorities> PurAuthorities { get; set; }
        public DbSet<TblPurTrnApprovals> TblPurTrnApprovalsList { get; set; }
        public DbSet<TblTranPurcInvoice> TranPurcInvoices { get; set; }
        public DbSet<TblTranPurcInvoiceItem> TranPurcInvoiceItems { get; set; }

        public DbSet<TblPopTrnGRNHeader> GRNHeaders { get; set; }

        public DbSet<TblPopTrnGRNDetails> GRNDetails { get; set; }

        #endregion

        #region SalesSetup
        public DbSet<TblSndDefCustomerCategory> SndCustomerCategories { get; set; }
        public DbSet<TblSndDefSalesShipment> SndSalesShipments { get; set; }
        public DbSet<TblSndDefSalesTermsCode> SndSalesTermsCodes { get; set; }
        public DbSet<TblInvDefSalesConfig> SalesConfigs { get; set; }
        public DbSet<TblInvDefPurchaseConfig> PurchaseConfigs { get; set; }

        #endregion

        #region InvoiceSetup

        public DbSet<TblSequenceNumberSetting> Sequences { get; set; }
        public DbSet<TblTranDefTax> TranTaxes { get; set; }
        public DbSet<TblTranDefProduct> TranProducts { get; set; }
        public DbSet<TblTranDefProductType> TranProductTypes { get; set; }
        public DbSet<TblTranDefUnitType> TranUnitTypes { get; set; }



        public DbSet<TblTranInvoice> TranInvoices { get; set; }
        public DbSet<TblTranInvoiceItem> TranInvoiceItems { get; set; }









        public DbSet<TblFinTrnCustomerApproval> TrnCustomerApprovals { get; set; }
        public DbSet<TblFinTrnCustomerInvoice> TrnCustomerInvoices { get; set; }
        public DbSet<TblFinTrnCustomerStatement> TrnCustomerStatements { get; set; }
        public DbSet<TblFinTrnCustomerPayment> TrnCustomerPayments { get; set; }




        public DbSet<TblTranVenInvoice> TranVenInvoices { get; set; }
        public DbSet<TblTranVenInvoiceItem> TranVenInvoiceItems { get; set; }
        public DbSet<TblFinTrnVendorApproval> TrnVendorApprovals { get; set; }
        public DbSet<TblFinTrnVendorInvoice> TrnVendorInvoices { get; set; }
        public DbSet<TblFinTrnVendorStatement> TrnVendorStatements { get; set; }
        public DbSet<TblFinTrnVendorPayment> TrnVendorPayments { get; set; }



        public DbSet<TblFinTrnOpmCustomerPaymentHeader> OpmCustPaymentHeaders { get; set; }
        public DbSet<TblFinTrnOpmCustomerPayment> OpmCustomerPayments { get; set; }
        public DbSet<TblFinTrnOpmVendorPaymentHeader> OpmVendPaymentHeaders { get; set; }
        public DbSet<TblFinTrnOpmVendorPayment> OpmVendorPayments { get; set; }
        public DbSet<TblFinTrnOverShortAmount> OverShortAmounts { get; set; }
        public DbSet<TblFinTrnAdvanceWallet> AdvanceWallets { get; set; }
        public DbSet<TblFinTrnCustomerWallet> CustomerWallets { get; set; }


        public DbSet<TblFinTrnBankReconciliation> BankReconciliations { get; set; }
        public DbSet<TblFinTrnBankReconciliationItem> BankReconciliationItems { get; set; }


        #endregion

        #region OperationRegion
        public DbSet<TblSndDefCustomerMaster> OprCustomers { get; set; }
        public DbSet<TblSndDefUnitMaster> OprUnits { get; set; }
        public DbSet<TblSndDefSiteMaster> OprSites { get; set; }
        public DbSet<TblSndDefServiceMaster> OprServices { get; set; }
        public DbSet<TblSndDefServiceUnitMap> OprServiceUnitMaps { get; set; }
        public DbSet<TblSndDefServiceEnquiries> OprEnquiries { get; set; }
        public DbSet<TblSndDefServiceEnquiryHeader> OprEnquiryHeaders { get; set; }
        public DbSet<TblSndDefSurveyFormElement> OprSurveyFormElements { get; set; }
        public DbSet<TblSndDefSurveyFormHead> OprSurveyFormHeads { get; set; }
        public DbSet<TblSndDefSurveyFormElementsMapp> OprSurveyFormElementsMapp { get; set; }
        public DbSet<TblSndDefSurveyor> OprSurveyors { get; set; }
        public DbSet<TblSndDefSurveyFormDataEntry> OprSurveyFormDataEntries { get; set; }


        /*Phase-2*/
        public DbSet<OP_HRM_TEMP_Project> OP_HRM_TEMP_Projects { get; set; }



        /*phase-2 Reconstruct*/
        public DbSet<HRM_DEF_EmployeeShift> HRM_DEF_EmployeeShifts { get; set; }
        public DbSet<HRM_DEF_PayrollGroup> HRM_DEF_PayrollGroups { get; set; }
        //public DbSet<HRM_TRAN_Employee> HRM_TRAN_Employees { get; set; }
        // public DbSet<HRM_DEF_EmployeeShiftMaster> HRM_DEF_EmployeeShiftMasters { get; set; }
        //  public DbSet<TblOpShiftSiteMap> TblOpShiftSiteMaps { get; set; }
        public DbSet<TblOpMonthlyRoaster> TblOpMonthlyRoasters { get; set; }
        public DbSet<TblOpSkillset> TblOpSkillsets { get; set; }
        public DbSet<TblOpOperationExpenseHead> TblOpOperationExpenseHeads { get; set; }
        public DbSet<TblOpMaterialEquipment> TblOpMaterialEquipments { get; set; }
        public DbSet<TblOpLogisticsandvehicle> TblOpLogisticsandvehicles { get; set; }
        public DbSet<TblOpShiftsPlanForProject> TblOpShiftsPlanForProjects { get; set; }
        public DbSet<TblOpSkillsetPlanForProject> TblOpSkillsetPlanForProjects { get; set; }
        public DbSet<TblOpMonthlyRoasterForSite> TblOpMonthlyRoasterForSites { get; set; }
        public DbSet<TblOpProjectBudgetCosting> TblOpProjectBudgetCostings { get; set; }
        public DbSet<TblOpProjectResourceCosting> TblOpProjectResourceCosting { get; set; }
        public DbSet<TblOpProjectResourceSubCosting> TblOpProjectResourceSubCostings { get; set; }
        public DbSet<TblOpProjectLogisticsCosting> TblOpProjectLogisticsCostings { get; set; }
        public DbSet<TblOpProjectLogisticsSubCosting> TblOpProjectLogisticsSubCostings { get; set; }
        public DbSet<TblOpProjectBudgetEstimation> TblOpProjectBudgetEstimations { get; set; }
        public DbSet<TblOpProjectMaterialEquipmentCosting> TblOpProjectMaterialEquipmentCostings { get; set; }
        public DbSet<TblOpProjectMaterialEquipmentSubCosting> TblOpProjectMaterialEquipmentSubCostings { get; set; }
        public DbSet<TblOpProjectFinancialExpenseCosting> TblOpProjectFinancialExpenseCostings { get; set; }
        public DbSet<TblOpAuthorities> TblOpAuthoritiesList { get; set; }
        public DbSet<TblOprTrnApprovals> TblOprTrnApprovalsList { get; set; }
        public DbSet<TblOpContractTermsMapToProject> TblOpContractTermsMapToProjectList { get; set; }
        public DbSet<TblOpPaymentTermsMapToProject> TblOpPaymentTermsMapToProjectList { get; set; }
        public DbSet<TblOpEmployeesToProjectSite> TblOpEmployeesToProjectSiteList { get; set; }
        public DbSet<TblOpEmployeeToResourceMap> TblOpEmployeeToResourceMapList { get; set; }
        public DbSet<TblOpEmployeeAttendance> EmployeeAttendance { get; set; }
        public DbSet<TblOpEmployeeLeaves> EmployeeLeaves { get; set; }
        public DbSet<TblOpEmployeeTransResign> EmployeeTransResign { get; set; }
        public DbSet<TblOpPvAddResourceReqHead> PvAddResorceHeads { get; set; }
        public DbSet<TblOpPvAddResource> PvAddResorces { get; set; }
        public DbSet<TblOpPvAddResourceEmployeeToResourceMap> TblOpPvAddResourceEmployeeToResourceMaps { get; set; }

        public DbSet<TblOpPvRemoveResourceReq> TblOpPvRemoveResourceReqs { get; set; }
        public DbSet<TblOpPvTransferResourceReq> TblOpPvTransferResourceReqs { get; set; }
        public DbSet<TblOpPvTransferWithReplacementReq> TblOpPvTransferWithReplacementReqs { get; set; }
        public DbSet<TblOpPvSwapEmployeesReq> TblOpPvSwapEmployeesReqs { get; set; }
        public DbSet<TblOpPvReplaceResourceReq> TblOpPvReplaceResourceReqs { get; set; }
        public DbSet<TblOpProjectSites> TblOpProjectSites { get; set; }
        public DbSet<TblOpPvOpenCloseReq> TblOpPvOpenCloseReqs { get; set; }
        public DbSet<TblOpProposalTemplate> TblOpProposalTemplates { get; set; }
        public DbSet<TblOpProposalCosting> TblOpProposalCostings { get; set; }
        //contract form
        public DbSet<TblOpContractTemplate> TblOpContractTemplates { get; set; }
        public DbSet<TblOpContractClause> TblOpContractClauses { get; set; }
        public DbSet<TblOpContractTemplateToContractClauseMap> tblOpContractTemplateToContractClauseMaps { get; set; }
        public DbSet<TblOpContractFormHead> TblOpContractFormHeads { get; set; }
        public DbSet<TblOpContractClausesToContractFormMap> TblOpContractClausesToContractFormMap { get; set; }

        public DbSet<TblOpReasonCode> OprReasonCodes { get; set; }
        public DbSet<TblOpCustomerVisitForm> TblOpCustomerVisitForms { get; set; }
        public DbSet<TblOpCustomerComplaint> TblOpCustomerComplaints { get; set; }
        #endregion

        #region Sales And Distributions 

        public DbSet<TblSndTranInvoice> SndTranInvoice { get; set; }
        public DbSet<TblSndTranInvoiceItem> SndTranInvoiceItem { get; set; }

        public DbSet<TblSndTranQuotation> SndTranQuotations { get; set; }
        public DbSet<TblSndTranQuotationItem> SndTranQuotationItems { get; set; }

        public DbSet<TblSndAuthorities> TblSndAuthoritiesList { get; set; }
        public DbSet<TblSndTrnApprovals> TblSndTrnApprovalsList { get; set; }
        public DbSet<TblSndTranInvoicePaymentSettlements> TblSndTranInvoicePaymentSettlementsList { get; set; }


        public DbSet<TblSndDeliveryNoteHeader> DeliveryNoteHeaders { get; set; }
        public DbSet<TblSndDeliveryNoteLine> DeliveryNoteLines { get; set; }

        //public DbSet<TblSndInventoryHistory> SndInventoryHistory { get; set; }





        #endregion

        #region School Management System
        //public DbSet<TblStudentDetails> StudentDetails { get; set; }

        public DbSet<TblParentsLogin> TblParentsLogin { get; set; }

        public DbSet<TblWardDetails> TblWardDetails { get; set; }

        public DbSet<TblSysSchoolAcademicBatches> SysSchoolAcademicBatches { get; set; }

        public DbSet<TblSysSchoolAcademicsSubects> SysSchoolAcademicsSubects { get; set; }

        public DbSet<TblSysSchoolSectionsSection> SchoolSectionsSection { get; set; }

        public DbSet<TblSysSchoolAcedemicClassGrade> SchoolAcedemicClassGrade { get; set; }

        public DbSet<TblSysSchoolSemister> SchoolSemister { get; set; }

        public DbSet<TblSysSchoolPETCategory> SysSchoolPETCategory { get; set; }

        public DbSet<TblSysSchoolNationality> SysSchoolNationality { get; set; }

        public DbSet<TblSysSchoolStuLeaveType> SysSchoolStuLeaveType { get; set; }

        public DbSet<TblSysSchoolPayTypes> SysSchoolPayTypes { get; set; }

        public DbSet<TblSysSchoolReligion> SysSchoolReligion { get; set; }

        public DbSet<TblSysSchoolLanguages> SysSchoolLanguages { get; set; }

        public DbSet<TblSysSchoolGender> SysSchoolGender { get; set; }

        public DbSet<TblSysSchoolFeeTerms> SysSchoolFeeTerms { get; set; }
        public DbSet<TblSysSchoolFeeType> SysSchoolFeeType { get; set; }

        public DbSet<TblSysSchoolBranches> SchoolBranches { get; set; }

        public DbSet<TblSysSchoolFeeStructureHeader> SchoolFeeStructureHeader { get; set; }

        public DbSet<TblSysSchoolFeeStructureDetails> SchoolFeeStructureDetails { get; set; }

        public DbSet<TblSysSchoolGradeSectionMapping> SchoolGradeSectionMapping { get; set; }

        public DbSet<TblSysSchoolGradeSubjectMapping> SchoolGradeSubjectMapping { get; set; }

        //web student Registration
        public DbSet<TblWebStudentRegistration> WebStudentRegistration { get; set; }

        public DbSet<TblDefSchoolStudentMaster> DefSchoolStudentMaster { get; set; }

        public DbSet<TblDefStudentFeeHeader> DefStudentFeeHeader { get; set; }

        public DbSet<TblDefStudentFeeDetails> DefStudentFeeDetails { get; set; }

        public DbSet<TblDefStudentAddress> DefStudentAddress { get; set; }
        public DbSet<TblDefStudentGuardiansSiblings> DefStudentGuardiansSiblings { get; set; }
        public DbSet<TblDefStudentPrevEducation> DefStudentPrevEducation { get; set; }
        public DbSet<TblDefStudentNotices> DefStudentNotices { get; set; }
        public DbSet<TblDefStudentNoticesReasonCode> DefStudentNoticesReasonCode { get; set; }
        public DbSet<TblDefStudentAcademics> DefStudentAcademics { get; set; }

        public DbSet<TblSysSchoolBranchesAuthority> SysSchoolBranchesAuthority { get; set; }


        //mobile app Messages
        public DbSet<TblSchoolMessages> SchoolMessages { get; set; }
        public DbSet<TblDefStudentAttendance> StudentAttendance { get; set; }

        public DbSet<TblDefStudentApplyLeave> StudentApplyLeave { get; set; }

        public DbSet<TblSysSchoolHolidayCalanderStudent> StudentHolidayClaender { get; set; }

        public DbSet<TblParentAddRequest> ParentAddRequest { get; set; }

        public DbSet<TblTranFeeTransaction> FeeTransaction { get; set; }

        public DbSet<TblStudentHomeWork> StudentHomeWork { get; set; }

        public DbSet<TblSysSchooScheduleEvents> SchooScheduleEvents { get; set; }

        public DbSet<TblLessonPlanHeader> LessonPlanHeader { get; set; }

        public DbSet<TblSysSchoolNews> SysSchoolNews { get; set; }

        public DbSet<TblSysSchoolNewsMediaLib> SysSchoolNewsMedia { get; set; }

        public DbSet<TblDefSchoolTeacherMaster> DefSchoolTeacherMaster { get; set; }
        public DbSet<TblDefSchoolTeacherLanguages> DefSchoolTeacherLanguages { get; set; }
        public DbSet<TblDefSchoolTeacherQualification> DefSchoolTeacherQualification { get; set; }
        public DbSet<TblDefSchoolTeacherSubjectsMapping> DefSchoolTeacherSubjectsMapping { get; set; }
        public DbSet<TblDefSchoolTeacherClassMapping> DefSchoolTeacherClassMapping { get; set; }
        public DbSet<TblLessonPlanDetails> LessonPlanDetails { get; set; }
        public DbSet<TblDefSchoolSubjectExamsGrade> SchoolSubjectExamsGrade { get; set; }
        public DbSet<TblDefSchoolExaminationManagementHeader> SchoolExaminationManagementHeader { get; set; }
        public DbSet<TblDefSchoolExaminationManagementDetails> SchoolExaminationManagementDetails { get; set; }
        public DbSet<TblDefSchoolStudentResultHeader> SchoolStudentResultHeader { get; set; }
        public DbSet<TblDefSchoolStudentResultDetails> SchoolStudentResultDetails { get; set; }
        public DbSet<TblSysSchoolExaminationTypes> SchoolExaminationTypes { get; set; }
        public DbSet<TblSysSchoolSchedule> SchoolSchedule { get; set; }
        public DbSet<TblStudentAttnRegHeader> StudentAttnRegHeader { get; set; }
        public DbSet<TblStudentAttnRegDetails> StudentAttnRegDetails { get; set; }

        //public DbSet<TblLessonPlanDetails> LessonPlanDetails { get; set; }


        public DbSet<TblSysSchoolPushNotificationParent> PushNotificationParent { get; set; }

        public DbSet<TblParentMyGallery> ParentMyGallery { get; set; }

        public DbSet<TblSysSchoolNotifications> SchoolNotifications { get; set; }
        public DbSet<TblSysSchoolNotificationFilters> SchoolNotificationFilters { get; set; }

        public DbSet<TblSysNotificaticationTemplate> NotificaticationTemplates { get; set; }

        #endregion

        #region Fleet Management
        // public DbSet<TblParentsLogin> TblParentsLogin { get; set; }

        public DbSet<TblVehicleCompanyMaster> VehicleCompanyMaster { get; set; }

        public DbSet<TblVehicleTypeMaster> VehicleTypeMaster { get; set; }

        public DbSet<TblVehicleBrandMaster> VehicleBrandMaster { get; set; }

        public DbSet<TblDriverMaster> DriverMaster { get; set; }

        public DbSet<TblRouteMaster> RouteMaster { get; set; }

        public DbSet<TblRoutePlanHeader> RoutePlanHeader { get; set; }

        public DbSet<TblRoutePlanDetails> RoutePlanDetails { get; set; }

        public DbSet<TblVehicleMaster> VehicleMaster { get; set; }

        public DbSet<TblAssignDrivers> AssignDrivers { get; set; }

        public DbSet<TblAssignRoutes> AssignRoutes { get; set; }

        public DbSet<TblServiceCode> ServiceCode { get; set; }

        public DbSet<TblServiceProvider> ServiceProvider { get; set; }

        public DbSet<TblVehicleFuelEntry> VehicleFuelEntry { get; set; }
        public DbSet<TblSchoolTranInvoice> TblSchoolTranInvoice { get; set; }
        public DbSet<TblSchoolTranInvoiceItem> TblSchoolTranInvoiceItem { get; set; }

        #endregion

        #region HRM Administration

        //Set up
        public DbSet<TblHRMSysPosition> Positions { get; set; }
        public DbSet<TblHRMSysAddressType> AddressTypes { get; set; }
        public DbSet<TblHRMSysReligion> Religions { get; set; }
        public DbSet<TblHRMSysBank> Banks { get; set; }
        public DbSet<TblHRMSysBankBranch> BankBranches { get; set; }
        public DbSet<TblHRMSysNationality> Nationalities { get; set; }
        public DbSet<TblHRMSysEmployeeType> EmployeeTypes { get; set; }
        public DbSet<TblHRMSysBloodGroup> BloodGroups { get; set; }
        public DbSet<TblHRMSysGender> Genders { get; set; }
        public DbSet<TblHRMSysMaritalStatus> MaritalStatuses { get; set; }
        public DbSet<TblHRMSysGroup> Groups { get; set; }
        public DbSet<TblHRMSysSubGroup> SubGroups { get; set; }
        public DbSet<TblHRMSysLanguage> Languages { get; set; }
        public DbSet<TblHRMSysTitle> Titles { get; set; }
        public DbSet<TblHRMSysDependentType> DependentTypes { get; set; }
        public DbSet<TblHRMSysDegreeType> DegreeTypes { get; set; }
        public DbSet<TblHRMSysQualification> Qualifications { get; set; }
        public DbSet<TblHRMSysCourseType> CourseTypes { get; set; }
        public DbSet<TblHRMSysInsuranceType> InsuranceTypes { get; set; }
        public DbSet<TblHRMSysInsuranceClass> InsuranceClasses { get; set; }
        public DbSet<TblHRMSysInsuranceProvider> InsuranceProviders { get; set; }
        public DbSet<TblHRMSysVisaType> VisaTypes { get; set; }
        public DbSet<TblHRMSysDocumentType> DocumentTypes { get; set; }
        public DbSet<TblHRMSysShift> Shifts { get; set; }
        public DbSet<TblHRMSysDepartment> Departments { get; set; }
        public DbSet<TblHRMSysDivision> Divisions { get; set; }
        public DbSet<TblHRMSysEmployeeStatus> EmployeeStatuses { get; set; }
        public DbSet<TblHRMSysHoliday> Holidays { get; set; }
        public DbSet<TblHRMSysHolidayCalendar> HolidayCalendars { get; set; }
        public DbSet<TblHRMSysHolidayCalendarMapping> HolidayCalendarMappings { get; set; }
        public DbSet<TblHRMSysEmployeeWeeklyOff> EmployeeWeeklyOffs { get; set; }
        public DbSet<TblHRMSysGrade> Grades { get; set; }
        public DbSet<TblHRMSysLeaveType> LeaveTypes { get; set; }
        public DbSet<TblHRMSysLeaveTemplate> LeaveTemplates { get; set; }
        public DbSet<TblHRMSysLeaveTemplateMapping> LeaveTemplateMappings { get; set; }
        public DbSet<TblHRMSysVacationPolicy> VacationPolicies { get; set; }
        public DbSet<TblHRMSysFlightClass> FlightClasses { get; set; }

        //Transactions
        public DbSet<TblHRMTrnPersonalInformation> PersonalInformation { get; set; }
        public DbSet<TblHRMTrnEmployeeLanguageInfo> EmployeeLanguages { get; set; }
        public DbSet<TblHRMTrnEmployeeDependentInfo> EmployeeDependents { get; set; }
        public DbSet<TblHRMTrnEmployeeAddress> EmployeeAddress { get; set; }
        public DbSet<TblHRMTrnEmployeeQualificationInfo> EmployeeQualifications { get; set; }
        public DbSet<TblHRMTrnEmployeeInsuranceInfo> EmployeeInsurances { get; set; }
        public DbSet<TblHRMTrnEmployeeVisaInfo> EmployeeVisas { get; set; }
        public DbSet<TblHRMTrnEmployeeDocumentInfo> EmployeeDocuments { get; set; }
        public DbSet<TblHRMTrnEmployeeShiftInfo> EmployeeShifts { get; set; }
        public DbSet<TblHRMTrnEmployeeContractInfo> EmployeeContracts { get; set; }
        public DbSet<TblHRMTrnEmployeeControls> EmployeeControls { get; set; }
        public DbSet<TblHRMTrnEmployeeLeaveInformation> EmployeeLeaveInformations { get; set; }
        public DbSet<TblHRMTrnEmployeeContactInfo> EmployeeContactInformation { get; set; }


        #region ServiceRequest
        public DbSet<TblHRMDefServiceRequestApprovalAuthorityMatrix> ServiceRequestApprovalAuthorityMatrix { get; set; }
        public DbSet<TblHRMSysServiceRequestApprovalAuthority> ServiceRequestApprovalAuthorities { get; set; }
        public DbSet<TblHRMSysServiceRequestType> ServiceRequestTypes { get; set; }
        public DbSet<TblHRMTrnEmployeeServiceRequest> EmployeeServiceRequests { get; set; }
        public DbSet<TblHRMTrnEmployeeServiceRequestAudit> EmployeeServiceRequestAudits { get; set; }
        public DbSet<TblHRMTrnEmployeeServiceRequestDocumentDetails> EmployeeServiceRequestDocumentDetails { get; set; }
        public DbSet<TblHRMTrnEmployeeVacationServiceRequestLeaveDetails> EmployeeVacationServiceRequestLeaveDetails { get; set; }
        public DbSet<TblHRMTrnEmployeeVacationDateLog> EmployeeVacationDateLogs { get; set; }
        public DbSet<TblHRMTrnEmployeeExitReEntryInfo> EmployeeExitReEntryInfos { get; set; }
        public DbSet<TblHRMTrnEmployeeReportingBackInfo> EmployeeReportingBackInfos { get; set; }

        #endregion

        #endregion

        #region Time and Attendance
        //Setup
        public DbSet<TblTNASysPayrollGroup> PayrollGroups { get; set; }

        //Transactions
        public DbSet<TblTNATrnEmployeeRoaster> EmployeeRoasters { get; set; }
        public DbSet<TblTNATrnEmployeeAttendance> TNAEmployeeAttendance { get; set; }
        public DbSet<TblTNATrnConsolidatedEmployeeAttendance> ConsolidatedEmployeeAttendance { get; set; }

        #endregion

        #region Payroll
        //Setup
        public DbSet<TblPRLSysPayrollPeriod> PayrollPeriods { get; set; }
        public DbSet<TblPRLSysPayrollComponent> PayrollComponents { get; set; }

        //Transactions
        public DbSet<TblPRLTrnPayrollPackage> PayrollPackages { get; set; }
        public DbSet<TblPRLTrnPayrollPackageComponent> PayrollPackageComponents { get; set; }
        public DbSet<TblPRLTrnEmployeePayrollStructure> EmployeePayrollStructure { get; set; }
        #endregion

        #region ErpMobile    //by Samba
        public DbSet<ErpSysEmployeeLogin> ErpSysEmployeeLogin { get; set; }
        public DbSet<TblMobMgtAttendanceImages> MobMgtAttendanceImages { get; set; }
        public DbSet<TblMobMgtGeoLocationLogs> MobMgtGeoLocationLogs { get; set; }
        public DbSet<TblErpSysPushNotification> ErpSysPushNotifications { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Cycle Restrict


            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            #endregion

            #region FinanceMgt
            modelBuilder.Entity<TblFinDefAccountCategory>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblFinDefAccountlPaycodes>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblFinDefAccountSubCategory>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblFinDefMainAccounts>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblFinSysAccountType>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSndDefVendorMaster>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            #endregion

            #region SystemSetup
            modelBuilder.Entity<TblErpSysCityCode>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblErpSysCompanyBranch>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblErpSysCountryCode>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblErpSysMenuOption>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblErpSysStateCode>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblErpSysSystemTax>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblErpSysTransactionCode>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            #endregion

            #region InventoryMgt
            //    modelBuilder.Entity<TblIMTransactionDetails>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            // modelBuilder.Entity<TblIMTransactionHeader>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            #endregion

            #region InventorySetup
            modelBuilder.Entity<TblInvDefCategory>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblInvDefClass>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblInvDefDistributionGroup>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblInventoryDefDistributionGroup>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblInvDefSubCategory>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblInvDefSubClass>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblInvDefUOM>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblInvDefWarehouse>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblInvDefWarehouseTest>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblErpInvItemMaster>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            //modelBuilder.Entity<TblErpInvItemInventory>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            //modelBuilder.Entity<TblErpInvItemsBarcode>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblErpInvItemtype>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblErpInvItemtracking>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            #endregion

            #region PurchaseSetup
            modelBuilder.Entity<TblPopDefVendorCategory>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblPopDefVendorPOTermsCode>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblPopDefVendorShipment>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            #endregion

            #region SalesSetup
            modelBuilder.Entity<TblSndDefCustomerCategory>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSndDefSalesShipment>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSndDefSalesTermsCode>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            #endregion

            #region operationsMgt

            modelBuilder.Entity<TblSndDefCustomerMaster>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSndDefUnitMaster>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSndDefSiteMaster>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSndDefServiceMaster>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSndDefServiceUnitMap>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            //modelBuilder.Entity<TblSndDefServiceEnquiries>().Property(e => e.EnquiryID).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSndDefSurveyFormElement>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSndDefServiceEnquiryHeader>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSndDefSurveyFormHead>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            //modelBuilder.Entity<TblSndDefSurveyFormElementsMapp>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSndDefSurveyor>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            modelBuilder.Entity<TblOpReasonCode>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            modelBuilder.Entity<TblSndDefServiceUnitMap>()
       .HasKey(nameof(TblSndDefServiceUnitMap.ServiceCode), nameof(TblSndDefServiceUnitMap.UnitCode));

            /*phase-2*/

            // modelBuilder.Entity<HRM_DEF_EmployeeShift>().Property(e => e.ID).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            //modelBuilder.Entity<HRM_DEF_EmployeeShiftMaster>().Property(e => e.ShiftId).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            //modelBuilder.Entity<HRM_DEF_PayrollGroup>().Property(e => e.PayrollGroupID).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<OP_HRM_TEMP_Project>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            //modelBuilder.Entity<HRM_TRAN_Employee>().Property(e => e.EmployeeID).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            modelBuilder.Entity<TblOpSkillset>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblOpOperationExpenseHead>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblOpMaterialEquipment>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblOpLogisticsandvehicle>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);



            #endregion

            #region SchoolManagement

            modelBuilder.Entity<TblSysSchoolAcademicBatches>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSysSchoolAcademicsSubects>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSysSchoolSectionsSection>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSysSchoolAcedemicClassGrade>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSysSchoolSemister>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSysSchoolPETCategory>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSysSchoolNationality>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSysSchoolStuLeaveType>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSysSchoolPayTypes>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSysSchoolReligion>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSysSchoolLanguages>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSysSchoolGender>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSysSchoolFeeTerms>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSysSchoolFeeType>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSysSchoolFeeStructureHeader>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSysSchoolFeeStructureDetails>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblSysSchoolGradeSectionMapping>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblSysSchoolGradeSubjectMapping>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblSysSchoolBranches>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);

            //web student Registration
            modelBuilder.Entity<TblWebStudentRegistration>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblDefSchoolStudentMaster>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblDefStudentFeeHeader>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblDefStudentFeeDetails>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblDefStudentAddress>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblDefStudentGuardiansSiblings>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblDefStudentPrevEducation>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblDefStudentNotices>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblDefStudentNoticesReasonCode>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblDefStudentAcademics>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            //mobile app Messages
            modelBuilder.Entity<TblSchoolMessages>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblSysSchoolHolidayCalanderStudent>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblDefStudentAttendance>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblDefSchoolStudentMaster>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblParentAddRequest>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblTranFeeTransaction>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblStudentHomeWork>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblSysSchooScheduleEvents>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblLessonPlanHeader>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSysSchoolNews>().Property(e => e.NewId).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblSysSchoolNewsMediaLib>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblDefSchoolTeacherMaster>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);

            modelBuilder.Entity<TblDefSchoolTeacherLanguages>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblDefSchoolTeacherQualification>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblDefSchoolTeacherSubjectsMapping>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblDefSchoolTeacherClassMapping>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);

            modelBuilder.Entity<TblLessonPlanDetails>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblDefSchoolSubjectExamsGrade>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblDefSchoolExaminationManagementHeader>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblDefSchoolExaminationManagementDetails>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblDefSchoolStudentResultHeader>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblDefSchoolStudentResultDetails>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblSysSchoolExaminationTypes>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblSysSchoolSchedule>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblStudentAttnRegHeader>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblStudentAttnRegDetails>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);

            //modelBuilder.Entity<TblLessonPlanDetails>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<TblSysSchoolPushNotificationParent>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblParentMyGallery>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblSysSchoolBranchesAuthority>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblSysSchoolNotifications>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblSysSchoolNotificationFilters>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblSysNotificaticationTemplate>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);

            modelBuilder.Entity<TblSchoolTranInvoice>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblSchoolTranInvoiceItem>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);



            #endregion

            #region Fleet Management
            modelBuilder.Entity<TblVehicleCompanyMaster>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblVehicleTypeMaster>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblVehicleBrandMaster>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblDriverMaster>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblRouteMaster>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblRoutePlanHeader>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblRoutePlanDetails>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblVehicleMaster>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblAssignDrivers>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblVehicleFuelEntry>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblAssignRoutes>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblServiceCode>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblServiceProvider>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);


            #endregion

            #region HRM Administration

            //Set up
            modelBuilder.Entity<TblHRMSysPosition>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysAddressType>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysReligion>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysBank>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysBankBranch>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysNationality>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysEmployeeType>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysBloodGroup>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysGender>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysMaritalStatus>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysGroup>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysSubGroup>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysLanguage>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysTitle>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysDegreeType>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysQualification>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysCourseType>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysInsuranceType>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysInsuranceClass>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysInsuranceProvider>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysVisaType>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysDocumentType>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysShift>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysDepartment>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysDivision>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysEmployeeStatus>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysHoliday>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysHolidayCalendar>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysHolidayCalendarMapping>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysEmployeeWeeklyOff>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysGrade>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysLeaveType>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysLeaveTemplate>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysDependentType>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMSysVacationPolicy>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);

            //Transactions
            modelBuilder.Entity<TblHRMTrnPersonalInformation>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMTrnPersonalInformation>().Property(e => e.EmployeeNumber).HasComputedColumnSql("RIGHT('0000'+CONVERT([VARCHAR](5),[Id]),(5))");
            modelBuilder.Entity<TblHRMTrnEmployeeDependentInfo>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMTrnEmployeeAddress>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMTrnEmployeeQualificationInfo>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMTrnEmployeeInsuranceInfo>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMTrnEmployeeVisaInfo>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMTrnEmployeeDocumentInfo>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMTrnEmployeeShiftInfo>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMTrnEmployeeContractInfo>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMTrnEmployeeControls>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMTrnEmployeeLeaveInformation>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblHRMTrnEmployeeContactInfo>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);

            //ServiceRequest
            //modelBuilder.Entity<TblHRMTrnEmployeeServiceRequest>().Property(e => e.ServiceRequestRefNo).HasComputedColumnSql("('SRQ'+right(('0'+CONVERT([VARCHAR](5),[Id])+year(getdate())+replicate('0',(7)-len([Id])))+CONVERT([nvarchar],[Id],(0)),(15)))");

            #endregion

            #region Time and Attendance
            //Setup
            modelBuilder.Entity<TblTNASysPayrollGroup>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);

            //Transactions
            modelBuilder.Entity<TblTNATrnEmployeeRoaster>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblTNATrnEmployeeAttendance>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblTNATrnConsolidatedEmployeeAttendance>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            #endregion

            #region Payroll
            //Setup
            modelBuilder.Entity<TblPRLSysPayrollPeriod>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblPRLSysPayrollComponent>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);

            //Transactions
            modelBuilder.Entity<TblPRLTrnPayrollPackage>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblPRLTrnPayrollPackageComponent>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            modelBuilder.Entity<TblPRLTrnEmployeePayrollStructure>().Property(e => e.Id).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);


            #endregion

            base.OnModelCreating(modelBuilder);
        }

    }









    public class DMCContext : DbContext
    {
        public DMCContext()
        {

        }

        public DMCContext(DbContextOptions<DMCContext> options)
            : base(options)
        {

        }

        public DbSet<HRM_TRAN_EmployeeTimeChart> EmployeePostedAttendance { get; set; }
        public DbSet<HRM_TRAN_Employee> HRM_TRAN_Employees { get; set; }
        public DbSet<HRM_DEF_EmployeeShiftMaster> HRM_DEF_EmployeeShiftMasters { get; set; }
        public DbSet<HRM_DEF_EmployeeOff> HRM_DEF_EmployeeOffs { get; set; }
        public DbSet<HRM_DEF_Site> HRM_DEF_Sites { get; set; }
        public DbSet<HRM_DEF_Project> HRM_DEF_Projects { get; set; }
        public DbSet<HRM_DEF_Branch> HRM_DEF_Branches { get; set; }
        public DbSet<HRM_DEF_Department> HRM_DEF_Departments { get; set; }
        public DbSet<HRM_TRAN_EmployeePrimarySites_Log> HRM_TRAN_EmployeePrimarySites_Logs { get; set; }
        public DbSet<HRM_DEF_HolidayMaster> HRM_DEF_Holidays { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);
        }

    }


    public class DMC2Context : DbContext
    {
        public DMC2Context()
        {

        }

        public DMC2Context(DbContextOptions<DMC2Context> options)
            : base(options)
        {

        }

        public DbSet<HRM_TRAN_EmployeeTimeChart> EmployeePostedAttendance { get; set; }
        public DbSet<HRM_TRAN_Employee> HRM_TRAN_Employees { get; set; }
        public DbSet<HRM_DEF_EmployeeShiftMaster> HRM_DEF_EmployeeShiftMasters { get; set; }
        public DbSet<HRM_DEF_EmployeeOff> HRM_DEF_EmployeeOffs { get; set; }
        public DbSet<HRM_DEF_Site> HRM_DEF_Sites { get; set; }
        public DbSet<HRM_DEF_Branch> HRM_DEF_Branches { get; set; }

        // public DbSet<HRM_DEF_Department> HRM_DEF_Departments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);
        }

    }
}
