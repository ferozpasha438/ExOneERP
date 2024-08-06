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
    [AutoMap(typeof(TblOpShiftSiteMap))]
    public class TblOpShiftSiteMapDto
    {
        public int Id { get; set; }
        [StringLength(20)]
        public string SiteCode { get; set; }
        [StringLength(20)]
        public string ShiftCode { get; set; }
    }
    public class OpShiftSiteMapDto
    {
        [StringLength(20)]
        public string SiteCode { get; set; }
        public List<TblOpShiftSiteMapDto> ShiftsList { get; set; }
    }

}