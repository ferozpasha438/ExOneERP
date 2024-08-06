using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application.OperationsMgtDtos
{
   public class CityStateCountryMappingDto: AutoActiveGenerateIdKeyDto<int>
    {
        [StringLength(50)]
        public string CityCode { get; set; }
         [StringLength(100)]
        public string CityName { get; set; }
        [StringLength(100)]
        public string CityNameAr { get; set; }
        [StringLength(50)]
        public string StateCode { get; set; }
        [StringLength(100)]
        public string StateName { get; set; }
        [StringLength(50)]
        public string CountryCode { get; set; }
        [StringLength(100)]
        public string CountryName { get; set; }



    }
}
