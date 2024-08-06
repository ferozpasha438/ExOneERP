using AutoMapper;
using CIN.Domain.OpeartionsMgt;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Application.OperationsMgtDtos
{
    [AutoMap(typeof(TblOpEmployeeToResourceMap))]
    public class TblOpEmployeeToResourceMapDto          // : AutoActiveGenerateIdAuditableKey<int>
    {
        public int MapId { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string SkillSet { get; set; }
        [StringLength(20)]
        public string EmployeeNumber { get; set; }
        public long EmployeeID { get; set; }

        public bool isPrimarySite { get; set; }

    }

    
}