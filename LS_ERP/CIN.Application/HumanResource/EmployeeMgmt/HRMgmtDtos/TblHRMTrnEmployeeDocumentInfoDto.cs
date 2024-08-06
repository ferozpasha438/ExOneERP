using AutoMapper;
using CIN.Domain.HumanResource.EmployeeMgt;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.EmployeeMgmt.HRMgmtDtos
{
    [AutoMap(typeof(TblHRMTrnEmployeeDocumentInfo))]
    public class TblHRMTrnEmployeeDocumentInfoDto : AuditableEntityDto<int>
    {
        //EmployeeID
        [Required]
        public int EmployeeID { get; set; }

        //Document Type
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

        //Document Type Name
        [StringLength(100)]
        public string DocumentTypeName { get; set; }

        //Uploaded documents
        public List<IFormFile> Files { get; set; }
    }
}
