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
    [Table("tblOpPvReplaceResourceReq")]
    public class TblOpPvReplaceResourceReq
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [StringLength(20)]
        public string CustomerCode { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }

     

        [StringLength(20)]
        public string ResignedEmployeeNumber { get; set; }

          [StringLength(20)]
        public string ReplacedEmployeeNumber { get; set; }


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
