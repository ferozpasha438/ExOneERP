using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Domain.SystemSetup
{
    [Table("tblErpSysCompany")]
    public class TblErpSysCompany : PrimaryKey<int>
    {
        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }


        [StringLength(100)]
        public string CompanyNameAr { get; set; }
        [Required]
        [StringLength(500)]
        public string CompanyAddress { get; set; }
        [StringLength(500)]
        public string CompanyAddressAr { get; set; }
        [Required]
        [StringLength(20)]
        public string Phone { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(50)]
        public string VATNumber { get; set; }
        [StringLength(12)]
        [Required]
        public string DateFormat { get; set; }
        [StringLength(15)]
        public string GeoLocLatitude { get; set; }
        [StringLength(15)]
        public string GeoLocLongitude { get; set; }
        [StringLength(100)]
        public string LogoURL { get; set; }
        [StringLength(1)]
        [Required]
        public char PriceDecimal { get; set; }
        [StringLength(1)]
        [Required]
        public char QuantityDecimal { get; set; }

        [StringLength(20)]
        public string City { get; set; }

        [StringLength(20)]
        public string State { get; set; }

        [StringLength(50)]
        public string Country { get; set; }
        [StringLength(20)]
        public string Mobile { get; set; }
        [StringLength(100)]
        public string Website { get; set; }
        [StringLength(200)]
        public string LogoImagePath { get; set; }

        [StringLength(50)]
        public string CrNumber { get; set; }
        [StringLength(80)]
        public string CcNumber { get; set; }
        [StringLength(200)]
        public string HeaderLogo { get; set; }
        [StringLength(200)]
        public string FooterLogo { get; set; }


        [Column(TypeName = "nvarchar(max)")]
        public string Csr { get; set; }

        [StringLength(256)]
        public string DevRequestId { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string DevCsid { get; set; }
        [StringLength(256)]
        public string DevSecret { get; set; }


        [StringLength(256)]
        public string ProdRequestId { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string ProdCsid { get; set; }
        [StringLength(256)]
        public string ProdSecret { get; set; }

    }
}
