using CIN.Domain.HumanResource.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.EmployeeMgt
{
    [Table("tblHRMTrnEmployeeDocumentInfo")]
    public class TblHRMTrnEmployeeDocumentInfo : AuditableEntity<int>
    {
        //EmployeeID
        [ForeignKey(nameof(EmployeeID))]
        public TblHRMTrnPersonalInformation TrnPersonalInformation { get; set; }
        [Required]
        public int EmployeeID { get; set; }

        //Document Type
        [ForeignKey(nameof(DocumentTypeCode))]
        public TblHRMSysDocumentType SysDocumentType { get; set; }
        [Required]
        [StringLength(20)]
        public string DocumentTypeCode { get; set; }

        //Is Verified
        [Required]
        public bool IsVerified { get; set; }

        //Document Number
        [StringLength(30)]
        public string DocumentNumber { get; set; }

        //Name of the document.
        [StringLength(256)]
        public string Name { get; set; }

        //Name with Guid and file extension.
        [StringLength(80)]
        public string FileName { get; set; }
    }
}
