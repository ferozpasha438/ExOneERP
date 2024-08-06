using CIN.Domain.SystemSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.OpeartionsMgt
{
    [Table("tblOpPvTransferWithReplacementReq")]
    public class TblOpPvTransferWithReplacementReq
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [StringLength(20)]
        public string SrcCustomerCode { get; set; }
        [StringLength(20)]
        public string SrcSiteCode { get; set; }
        [StringLength(20)]
        public string SrcProjectCode { get; set; }
          [StringLength(20)]
        public string DestCustomerCode { get; set; }
        [StringLength(20)]
        public string DestSiteCode { get; set; }
        [StringLength(20)]
        public string DestProjectCode { get; set; }


        [StringLength(20)]
        public string SrcEmployeeNumber { get; set; }
        [StringLength(20)]
        public string DestEmployeeNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FromDate { get; set; }

        public bool IsApproved { get; set; }
      

        public bool IsMerged { get; set; }



        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? Modified { get; set; }

        public int ModifiedBy { get; set; }
        public bool IsActive { get; set; }

        public int ApprovedBy { get; set; }
        public string FileUrl { get; set; }
        public int? FileUploadBy { get; set; }

    }
    
}
