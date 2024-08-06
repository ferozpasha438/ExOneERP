using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.GeneralLedger.Ledger
{
    [Table("tblFinTrnTrialBalance")]
    public class TblFinTrnTrialBalance : PrimaryKey<int>
    {
        [StringLength(50)]
        public string AcCode { get; set; }
        [StringLength(100)]
        public string AcDesc { get; set; }
        [StringLength(15)]
        public string Type { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? CrAmount { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? DrAmount { get; set; }
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? Balance { get; set; }      
        [Column(TypeName = "decimal(18, 3)")]
        public decimal? CrBalance { get; set; }

    }
}
