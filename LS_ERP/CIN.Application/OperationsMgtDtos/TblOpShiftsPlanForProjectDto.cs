using AutoMapper;
using CIN.Domain.OpeartionsMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.OperationsMgtDtos
{
    [AutoMap(typeof(TblOpShiftsPlanForProject))]
    public class TblOpShiftsPlanForProjectDto
    {
        public int Id { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }
        [StringLength(20)]
        public string ShiftCode { get; set; }
    }
    public class OpShiftsPlanForprojectDto
    {
        [StringLength(20)]
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
        public List<TblOpShiftSiteMapDto> ShiftsList { get; set; }
    }

}