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
    [AutoMap(typeof(TblOpSkillsetPlanForProject))]
    public class TblOpSkillsetPlanForProjectDto
    {
        public int Id { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string ProjectCode { get; set; }
        [StringLength(20)]
        public string SkillsetCode { get; set; }
        public short Quantity { get; set; }
    }
    public class OpSkillsetPlanForProjectDto
    {
        [StringLength(20)]
        public string ProjectCode { get; set; }
        public string SiteCode { get; set; }
        public List<TblOpSkillsetPlanForProjectDto> SkillsetList { get; set; }
    }

}