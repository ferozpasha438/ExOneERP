﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Domain.HumanResource.Setup
{
    [Table("tblHRMSysNationality")]
    public class TblHRMSysNationality : AutoGeneratedIdKeyAuditableEntity<int>
    {
        [StringLength(20)]
        [Key]
        public string NationalityCode { get; set; }
        [Required]
        [StringLength(100)]
        public string NationalityNameEn { get; set; }
        [StringLength(100)]
        public string NationalityNameAr { get; set; }
    }
}
