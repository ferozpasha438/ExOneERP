using AutoMapper;
using CIN.Domain.OpeartionsMgt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.OperationsMgtDtos
{
    [AutoMap(typeof(TblSndDefSurveyFormDataEntry))]
    public class TblSndDefSurveyFormDataEntryDto
    {
        public int EnquiryID { get; set; }
        public string FormElementCode { get; set; }
        public long EntryID { get; set; }
        public string ElementEngName { get; set; }
        [StringLength(200)]
        public string ElementArbName { get; set; }
        [StringLength(20)]
        public string ElementType { get; set; }
        [StringLength(500)]
        public string ListValueString { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public string EntryValue { get; set; }
    }
}
