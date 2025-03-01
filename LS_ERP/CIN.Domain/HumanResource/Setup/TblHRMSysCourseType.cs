﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.Setup
{
    [Table("tblHRMSysCourseType")]
    public class TblHRMSysCourseType : AutoGeneratedIdKeyAuditableEntity<int>
    {
        [StringLength(20)]
        [Key]
        public string CourseTypeCode { get; set; }
        [Required]
        [StringLength(100)]
        public string CourseTypeNameEn { get; set; }
        [Required]
        [StringLength(100)]
        public string CourseTypeNameAr { get; set; }
    }
}
