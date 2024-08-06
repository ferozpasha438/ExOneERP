using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIN.Domain;

namespace CIN.Domain.SchoolMgt
{

    [Table("tblTranFeeTransaction")]
    public class TblTranFeeTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }
    }

}
