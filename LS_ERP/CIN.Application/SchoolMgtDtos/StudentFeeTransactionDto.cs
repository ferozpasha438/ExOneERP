using AutoMapper;
using CIN.Domain.SchoolMgt;
using CIN.DB.One.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Application.SchoolMgtDtos
{
    [AutoMap(typeof(TblTranFeeTransaction))]
    public class TblTranFeeTransactionDto
    {
        public int Id { get; set; }
        public string AdmissionNumber { get; set; }
        public string ReceiptVoucher { get; set; }
        public DateTime FeeDate { get; set; }
        public string FeeTerm { get; set; }
        public string FeeStructCode { get; set; }
        public string TermCode { get; set; }
        public DateTime FeeDueDate { get; set; }
        public decimal TotFeeAmount { get; set; }
        public decimal DiscAmount { get; set; }
        public decimal NetFeeAmount { get; set; }
        public string DiscReason { get; set; }
        public bool IsPaid { get; set; }
        public DateTime PaidDate { get; set; }
        public string PaidTransNum { get; set; }
        public string PaidRemarks1 { get; set; }
        public string PaidRemarks2 { get; set; }
        public string InvNumber { get; set; }
        public string JvNumber { get; set; }
        public bool PaidOnline { get; set; }
        public bool PaidManual { get; set; }
        public string PayCode { get; set; }
        public string ReceivedByUser { get; set; }
        public string AcademicYear { get; set; }
    }

    [AutoMap(typeof(TblTranFeeTransaction))]
    public class TranFeeTransactionDto
    {
        public int Id { get; set; }
        public string AdmissionNumber { get; set; }
        public string ReceiptVoucher { get; set; }
        public DateTime FeeDate { get; set; }
        public string FeeTerm { get; set; }
        public string FeeStructCode { get; set; }
        public string TermCode { get; set; }
        public DateTime FeeDueDate { get; set; }
        public decimal TotFeeAmount { get; set; }
        public decimal DiscAmount { get; set; }
        public decimal NetFeeAmount { get; set; }
        public string DiscReason { get; set; }
        public bool IsPaid { get; set; }
        public DateTime PaidDate { get; set; }
        public string PaidTransNum { get; set; }
        public string PaidRemarks1 { get; set; }
        public string PaidRemarks2 { get; set; }
        public string InvNumber { get; set; }
        public string JvNumber { get; set; }
        public bool PaidOnline { get; set; }
        public bool PaidManual { get; set; }
        public string PayCode { get; set; }
        public string ReceivedByUser { get; set; }
        public string AcademicYear { get; set; }
        public long? TranInvoiceId { get; set; }
    }

    public class StuFeeTransactionDto
    {
        public string GradeCode { get; set; }
        public string SectionCode { get; set; }
        public string AdmissionNumber { get; set; }
        public string TermCode { get; set; }
        public string PayCode { get; set; }
        public string BranchCode { get; set; }
        public string Remarks { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? Discount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
    }

    //public class StuBulkFeeTransactionDto
    //{
    //    public string GradeCode { get; set; }
    //    public string SectionCode { get; set; }
    //    public string AdmissionNumber { get; set; }
    //    public string TermCode { get; set; }
    //    public string PayCode { get; set; }
    //    public string BranchCode { get; set; }
    //    public string Remarks { get; set; }
    //    public decimal? DiscountPercentage { get; set; }
    //    public decimal? Discount { get; set; }
    //    public decimal PaidAmount { get; set; }
    //    public decimal RemainingAmount { get; set; }
    //}

    public class StuBulkFeeTransactionDto
    {
        public string GradeCode { get; set; }
        public string SectionCode { get; set; }
        public string AdmissionNumber { get; set; }
        public string TermCode { get; set; }
        public List<string> TermCodes { get; set; } // Changed from string to List<string>
        public string PayCode { get; set; }
        public string BranchCode { get; set; }
        public string Remarks { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? Discount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
    }

    public class StudentFeePaidDto
    {
        public int Id { get; set; }
        public string FeeStructCode { get; set; }
        public string TermCode { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DiscAmount { get; set; }
        public bool IsPaid { get; set; }
        public bool IsCompletelyPaid { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }

    }

    public class OnlineFeeTransactionDto
    {
        public int Id { get; set; }
        public  string StudNum{ get; set; }
        public decimal Amount { get; set; }
        public string TermCode { get; set; }
        public string BranchCode { get; set; }
        public bool IsTaxApplicable { get; set; }
        public decimal VATAmount { get; set; }

    }


    
    public class PrintStuFeeTransactionDto
    {
        public DateTime VoucherDate { get; set; }
        public string VoucherNumber { get; set; }
        public string StudentName { get; set; }
        public string StudentName2 { get; set; }
        public string AdmissionNumber { get; set; }
        public Decimal TotalFeeAmount { get; set; }
        public List<TermFeeDTO> TermFeeDetails { get; set; }


        public decimal TotFeeAmount { get; set; }
        public decimal DiscAmount { get; set; }
        public decimal NetFeeAmount { get; set; }
        public string DiscReason { get; set; }
        public bool IsPaid { get; set; }
        public DateTime PaidDate { get; set; }
        public string PaidTransNum { get; set; }
        public string PaidRemarks1 { get; set; }
        public string PaidRemarks2 { get; set; }
        public string InvNumber { get; set; }
        public string JvNumber { get; set; }
        public bool PaidOnline { get; set; }
        public bool PaidManual { get; set; }
        public string PayCode { get; set; }
        public string ReceivedByUser { get; set; }
        public string AcademicYear { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }
    }
    public class TermFeeDTO
    {
        public string TermName { get; set; }
        public string TermName2 { get; set; }
        public Decimal FeeAmount { get; set; }
        public decimal TotFeeAmount { get; set; }
        public decimal DiscAmount { get; set; }
        public decimal NetFeeAmount { get; set; }
        public decimal TaxAmount { get; set; }
    }
}
