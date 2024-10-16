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
    [AutoMap(typeof(TblHRMSysQualification))]
    public class TblHRMSysQualificationDto : AutoGeneratedIdKeyAuditableEntityDto<int>
    {
        [StringLength(20)]
        [Required]
        public string QualificationCode { get; set; }
        [Required]
        [StringLength(100)]
        public string QualificationNameEn { get; set; }
        [StringLength(100)]
        public string QualificationNameAr { get; set; }
        [Required]
        public bool IsTechnicalQualification { get; set; }
        //Degree Type
        [StringLength(20)]
        [Required]
        public string DegreeTypeCode { get; set; }
        [StringLength(100)]
        public string DegreeTypeName { get; set; }
    }
}
