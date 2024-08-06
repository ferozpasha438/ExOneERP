using System.ComponentModel.DataAnnotations;

namespace CIN.Application
{

    public static class ApiMessageInfo
    {
        public const string Failed = "Failed.";
        public const string NotFound = "No Data Found.";
        public static string Duplicate(string title) => $"Duplicate {title}";
        public const string Success = "Successful.";
        public const string ModelState = "Input fileds are invalid";

        /// <summary>
        /// Setting The Status
        /// </summary>
        /// <param name="type">0: Failed, 1: Success, 2: NotFound,3: Modelstate Errors</param>
        /// <param name="id">Entity ID</param>
        /// <returns></returns>
        public static AppCtrollerDto DuplicateInfo(string field) => new AppCtrollerDto { Message = Duplicate(field), Id = 0 };
        /// <summary>
        /// ( type == 0 is failed )
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static AppCtrollerDto Status(short type, long id = 0) => new AppCtrollerDto { Message = GetAppControllerStatus(type), Id = id };
        public static AppCtrollerDto Status(string message, long id = 0) => new AppCtrollerDto { Message = message, Id = id };
        public static MobileCtrollerDto Status(short type, bool status, long id = 0) => new MobileCtrollerDto { Message = GetAppControllerStatus(type), Status = status, Id = id };
        public static AppCtrollerStringDto Statusstring(short type, string str = "") => new AppCtrollerStringDto { Message = GetAppControllerstringStatus(type), Str = str };
        public static ApiMessageDto APIError(this string message) => new() { Message = message };
        private static string GetAppControllerStatus(short statusType) => statusType switch
        {
            0 => Failed,
            1 => Success,
            2 => NotFound,
            3 => ModelState,
            _ => string.Empty
        };

        private static string GetAppControllerstringStatus(short statusType) => statusType switch
        {
            0 => Failed,
            1 => Success,
            2 => NotFound,
            3 => ModelState,
            _ => string.Empty
        };
    }




    public class ApiMessageDto
    {
        public string Message { get; set; }
        public short Type { get; set; }
    }

    public class AppCtrollerDto
    {
        public string Message { get; set; }
        public long Id { get; set; }
    }

    public enum LoginStatusType
    {
        LicenseExpired,
        ConcurrentUsers,
    }
    public class AppCtrollerStringDto
    {
        public string Message { get; set; }
        public long Id { get; set; }
        public string Str { get; set; }
    }


    #region Mobile API Dtos

    public class MobileLoginResponseDto
    {
        public int Id { get; set; }
        public string BranchCode { get; set; } = string.Empty;
        public string SiteCode { get; set; } = string.Empty;
        public string SiteName { get; set; } = string.Empty;
        public string SiteNameAR { get; set; } = string.Empty;
        public decimal? SiteGeoLatitude { get; set; }
        public decimal? SiteGeoLongitude { get; set; }
        public decimal? SiteGeoGain { get; set; }
        public decimal? InnerRadius { get; set; }
        public decimal? OuterRadius { get; set; }
        public long? EmployeeId { get; set; }
        public bool IsLoginAllow { get; set; }

    }
    public class MobileApiMessageDto
    {
        public string Message { get; set; }
        public bool Status { get; set; }
    }

    public class OnlinePaymentApiMessageDto
    {
        public string Message { get; set; }
        public bool Status { get; set; }
        public string TrnNumber { get; set; }

    }

    public class CheckCINMobileServerMetaDataDto : CheckCINServerDto
    {
        [Required(ErrorMessage = "*")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "*")]
        public string Password { get; set; }
    }

    public class CINMobileServerMetaDataDto : CINServerMetaDataDto
    {
        public string Token { get; set; }
        public string SiteName { get; set; }
        public string SiteNameAr { get; set; }
        public decimal? InnerRadius { get; set; }
        public decimal? OuterRadius { get; set; }
        public decimal? SiteGeoLatitude { get; set; }
        public decimal? SiteGeoLongitude { get; set; }
        public decimal? SiteGeoGain { get; set; }
        public bool IsSubscribed { get; set; }
    }

    public class MobileCtrollerDto
    {
        public string Message { get; set; }
        public long Id { get; set; }
        public bool Status { get; set; }
    }


    #endregion
}
