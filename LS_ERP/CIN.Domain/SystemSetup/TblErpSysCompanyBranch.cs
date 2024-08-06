using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CIN.Domain.SystemSetup
{
    [Table("tblErpSysCompanyBranches")]
    //[Index(nameof(BranchCode), Name = "IX_TblErpSysCompanyBranches_BranchCode", IsUnique = true)]
    public class TblErpSysCompanyBranch : AutoActiveGenerateIdKey<int>
    {
        [ForeignKey(nameof(CompanyId))]
        public TblErpSysCompany SysCompany { get; set; }
        public int CompanyId { get; set; }

         [ForeignKey(nameof(ZoneId))]
        public TblErpSysZoneSetting ZoneSetting { get; set; }
        public int? ZoneId { get; set; }


        [StringLength(20)]
        [Key]
        public string BranchCode { get; set; }

        [StringLength(150)]
        public string BankName { get; set; }

        [StringLength(150)]
        public string BankNameAr { get; set; }

        [StringLength(100)]
        public string BranchName { get; set; }

        [StringLength(500)]
        [Required]
        public string BranchAddress { get; set; }

        [StringLength(500)]        
        public string BranchAddressAr { get; set; }

        [StringLength(80)]
        public string AccountNumber { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }
        [StringLength(20)]
        public string Mobile { get; set; }

        [ForeignKey(nameof(City))]
        public TblErpSysCityCode SysCityCode { get; set; }
        [StringLength(20)]
        public string City { get; set; } 

        [StringLength(20)]
        public string State { get; set; } // ref state

        [StringLength(100)]
        public string AuthorityName { get; set; }
        [StringLength(15)]
        public string GeoLocLatitude { get; set; }
        [StringLength(15)]
        public string GeoLocLongitude { get; set; }
        [StringLength(512)]
        public string Remarks { get; set; }
        [StringLength(80)]
        public string Iban { get; set; }
        
    }
}
