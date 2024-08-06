
namespace CIN.Domain.SalesSetup
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("tblSndDefCustomerCategory")]
    public class TblSndDefCustomerCategory : AutoActiveGenerateIdAuditableKey<int>
    {
        [StringLength(20)]
        [Key]
        public string CustCatCode { get; set; }
        [StringLength(50)]
        public string CustCatName { get; set; }
        [StringLength(50)]
        public string CustCatDesc { get; set; }
        [StringLength(3)]
        public string CatPrefix { get; set; }
        public int LastSeq { get; set; }

    }
}
