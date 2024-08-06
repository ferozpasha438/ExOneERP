using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CIN.Application
{
    public class CheckCINServerDto
    {
        [Required(ErrorMessage = "*")]
        public string CINNumber { get; set; }
    }
    public class CheckCINServerMetaDataDto : CheckCINServerDto
    {
        [Required(ErrorMessage = "*")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "*")]
        public string Password { get; set; }
    }
    public class CheckMobileLoginCINServerMetaDataDto : CheckCINServerMetaDataDto
    {
        [Required(ErrorMessage = "*")]
        public decimal SiteGeoLatitude { get; set; }
        [Required(ErrorMessage = "*")]
        public decimal SiteGeoLongitude { get; set; }
    }

    public class CheckMobileCINServerMetaDataDto : CheckMobileLoginCINServerMetaDataDto
    {
        public decimal SiteLocationNvMeter { get; set; }
        public decimal SiteLocationPvMeter { get; set; }
        public decimal SiteLocationExtraMeter { get; set; }
    }
    public class CINServerMetaDataDto : CheckCINServerDto
    {
        public long Id { get; set; }
        // public DateTime ValidDate { get; set; }
        public string APIEndpoint { get; set; }
        public string ModuleCodes { get; set; }
        public string DBConnectionString { get; set; }
        public string AdmUrl { get; set; }
        public string FinUrl { get; set; }
        public string OpmUrl { get; set; }
        public string InvUrl { get; set; }
        public string SndUrl { get; set; }
        public string PosUrl { get; set; }
        public string PopUrl { get; set; }
        public string SchUrl { get; set; }
        public string ScpUrl { get; set; }
        public string SctUrl { get; set; }
        public string HrmUrl { get; set; }
        public string HraUrl { get; set; }
        public string HrsUrl { get; set; }
        public string FltUrl { get; set; }
        public string MfgUrl { get; set; }
        public string CrmUrl { get; set; }
        public string UtlUrl { get; set; }
        //   public DateTime PaymentDate { get; set; }
        public bool IsActive { get; set; }


    }

    public class BaseCINLoginUserDTO //: CINServerMetaDataDto
    {
        public int UserId { get; set; }
        public long? EmployeeId { get; set; }
        public string ApiToken { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public string BranchCode { get; set; }
        public string DBConnectionString { get; set; }
        public string DBHRMConnectionString { get; set; }
        public string SiteCode { get; set; }
        public string TeacherCode { get; set; }
    }


    //public class MenuItemListDto
    //{
    //    public string MainModule { get; set; }
    //    public string SubModule { get; set; }
    //    public List<MenuItemDto> List { get; set; }
    //}
    //public class MenuItemDto
    //{
    //    public string Name { get; set; }
    //    public string Link { get; set; }
    //}
    public class MobileLoginInput : CheckCINServerMetaDataDto
    {
        public DateTime? TodayDate { get; set; }
    }
}
