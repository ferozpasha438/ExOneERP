using AutoMapper;
using CIN.Domain.OpeartionsMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIN.Application.OperationsMgtDtos
{
    [AutoMap(typeof(TblOpPvAddResourceEmployeeToResourceMap))]
    public class TblOpPvAddResourceEmployeeToResourceMapDto          // : AutoActiveGenerateIdAuditableKey<int>
    {
        public long MapId { get; set; }
        public long PvAddResReqId { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string SkillSet { get; set; }
        [StringLength(20)]
        public string EmployeeNumber { get; set; }

        [StringLength(20)]
        public string DefShift { get; set; }
        public short OffDay { get; set; }
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }



    }

    public class InputPvAddResourceEmployeeToResourceMap
    {
        public List<TblOpPvAddResourceEmployeeToResourceMapDto> MappingsList { get; set; }

    }
}