using CIN.Domain.OpeartionsMgt;
using CIN.Domain.SystemSetup;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.GeneralLedger
{
    [Table("tblFinTrnJournalVoucherStatement")]
    [Index(nameof(TranNumber), Name = "IX_tblFinTrnJournalVoucherStatement_TranNumber", IsUnique = false)]
    public class TblFinTrnJournalVoucherStatement : PrimaryKey<int>
    {
        [ForeignKey(nameof(JournalVoucherId))]
        public TblFinTrnJournalVoucher JournalVoucher { get; set; }
        public int? JournalVoucherId { get; set; }        
        [ForeignKey(nameof(LoginId))]
        public TblErpSysLogin SysLogin { get; set; }
        public int LoginId { get; set; }     
        public DateTime? JvDate { get; set; }
        
        [StringLength(50)]
        [Required]
        public string TranNumber { get; set; }
                          
        [StringLength(150)]
        public string Remarks1 { get; set; }
        [StringLength(150)]
        public string Remarks2 { get; set; }
        public bool IsPosted { get; set; }
        public bool IsVoid { get; set; }
    }
}
