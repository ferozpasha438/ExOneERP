﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.Setup
{
    [Table("tblHRMSysInsuranceType")]
    public class TblHRMSysInsuranceType : AutoGeneratedIdKeyAuditableEntity<int>
    {
        [StringLength(20)]
        [Key]
        public string InsuranceTypeCode { get; set; }
        [Required]
        [StringLength(100)]
        public string InsuranceTypeNameEn { get; set; }
        [Required]
        [StringLength(100)]
        public string InsuranceTypeNameAr { get; set; }
    }
}
