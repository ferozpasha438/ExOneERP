using CIN.Domain.OpeartionsMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.OpeartionsMgt
{
    [Table("tblSndDefServiceEnquiryHeader")]
    public class TblSndDefServiceEnquiryHeader : AutoActiveGenerateIdAuditableKey<int>
    {
        [Key]
        [Required]
        [StringLength(20)]
        public string EnquiryNumber { get; set; }
        [ForeignKey(nameof(CustomerCode))]
        public TblSndDefCustomerMaster SysCustCode { get; set; }
        [Required]
        public string CustomerCode { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateOfEnquiry { get; set; }
        [Column(TypeName = "date")]
        public DateTime EstimateClosingDate { get; set; }
        [StringLength(200)]
        public string UserName { get; set; }
        [Column(TypeName = "decimal(17,3)")]
        public decimal TotalEstPrice { get; set; }
        [StringLength(500)]
        public string Remarks { get; set; }
        [StringLength(20)]
        public string StusEnquiryHead { get; set; }
        public string BranchCode { get; set; }
        public bool IsConvertedToProject { get; set; }
        public short? Version { get; set; } = Int16.Parse("0");

    }
}
