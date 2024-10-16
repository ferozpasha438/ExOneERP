﻿using AutoMapper;
using CIN.Domain.HumanResource.Setup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.HumanResource.SetUp.HRMSetUpDtos
{
    [AutoMap(typeof(TblHRMSysInsuranceClass))]
    public class TblHRMSysInsuranceClassDto : AutoGeneratedIdKeyAuditableEntityDto<int>
    {
        [Required]
        [StringLength(20)]
        public string InsuranceClassCode { get; set; }
        [Required]
        [StringLength(100)]
        public string InsuranceClassNameEn { get; set; }
        [Required]
        [StringLength(100)]
        public string InsuranceClassNameAr { get; set; }
    }
}
