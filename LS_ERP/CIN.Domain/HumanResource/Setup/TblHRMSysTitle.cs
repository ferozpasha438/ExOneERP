﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.Setup
{
    [Table("tblHRMSysTitle")]
    public class TblHRMSysTitle : AutoGeneratedIdKeyAuditableEntity<int>
    {
        [StringLength(20)]
        [Key]
        public string TitleCode { get; set; }
        [Required]
        [StringLength(100)]
        public string TitleNameEn { get; set; }
        [Required]
        [StringLength(100)]
        public string TitleNameAr { get; set; }
    }
}
