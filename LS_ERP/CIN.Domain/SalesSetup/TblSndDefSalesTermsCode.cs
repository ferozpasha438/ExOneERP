
namespace CIN.Domain.SalesSetup
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("tblSndDefSalesTermsCode")]
    public class TblSndDefSalesTermsCode : AutoActiveGenerateIdAuditableKey<int>
    {
        [StringLength(20)]
        [Key]
        public string SalesTermsCode { get; set; }
        [StringLength(50)]
        public string SalesTermsName { get; set; }
        [StringLength(50)]
        public string SalesTermsDesc { get; set; }
        public sbyte SalesTermsDueDays { get; set; }
        public sbyte SalesTermDiscDays { get; set; }
        
    }
}
