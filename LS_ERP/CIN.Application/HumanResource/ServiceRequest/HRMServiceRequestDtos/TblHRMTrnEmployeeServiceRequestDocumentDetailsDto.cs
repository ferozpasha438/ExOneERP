using AutoMapper;
using CIN.Application;
using CIN.Domain.HumanResource.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.ServiceRequest.HRMServiceRequestDtos
{
    [AutoMap(typeof(TblHRMTrnEmployeeServiceRequestDocumentDetails))]
    public class TblHRMTrnEmployeeServiceRequestDocumentDetailsDto : PrimaryKeyDto<int>
    {
        //EmployeeServiceRequestID
      
        [Required]
        public int EmployeeServiceRequestID { get; set; }

        //Document Type Code
       
        [Required]
        [StringLength(20)]
        public string DocumentTypeCode { get; set; }

        //Name of the document.
        [StringLength(256)]
        public string Name { get; set; }

        //Name with Guid, fullpath and file extension.
        [StringLength(80)]
        public string FileName { get; set; }

        //Uploaded Date
        public DateTime UploadedDate { get; set; }

        //UserID
        public int UploadedBy { get; set; }

        //ServiceRequestProcessStageID(Increment by 1 at each step.)
        [Required]
        public int ServiceRequestProcessStageID { get; set; }
    }
}
