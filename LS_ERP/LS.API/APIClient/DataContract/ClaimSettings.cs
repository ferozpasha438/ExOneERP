using LS.API;
using LS.API.HttpContext;
using System.Linq;

namespace LS
{
    public class ClaimSettings
    {
        public static string WebAPIURL { get; set; }
        public static string DbConnectionString
        {
            get
            {
                if (HasClaims())// && HttpContext.Current.User.Claims.FirstOrDefault(c => c.Type == CustomClaimType.DbConnectionString).Value != null)
                    return HttpContext.Current.User.Claims.FirstOrDefault(c => c.Type == CustomClaimType.DbConnectionString)?.Value ?? string.Empty;
                return string.Empty;
            }
        }
        public static string DbHRMConnectionString
        {
            get
            {
                if (HasClaims())// && HttpContext.Current.User.Claims.FirstOrDefault(c => c.Type == CustomClaimType.DbConnectionString).Value != null)
                    return HttpContext.Current.User.Claims.FirstOrDefault(c => c.Type == CustomClaimType.DbHRMConnectionString)?.Value ?? string.Empty;
                return string.Empty;
            }
        }
        public static string BranchId
        {
            get
            {
                if (HasClaims())// && HttpContext.Current.User.Claims.FirstOrDefault(c => c.Type == "branchid").Value != null)
                    return HttpContext.Current.User.Claims.FirstOrDefault(c => c.Type == "branchid")?.Value ?? string.Empty;
                return string.Empty;
            }
        }

        //public static string ApiEndPoint
        //{
        //    get
        //    {
        //        if (HasClaims())// && HttpContext.Current.User.Claims.FirstOrDefault(c => c.Type == CustomClaimType.ApiEndPoint).Value != null)
        //            return HttpContext.Current.User.Claims.FirstOrDefault(c => c.Type == CustomClaimType.ApiEndPoint)?.Value ?? string.Empty;
        //        return string.Empty;
        //    }
        //}
        private static bool HasClaims() => HttpContext.Current != null && HttpContext.Current.User.Claims.Count() != 0;

        public static bool IsLoging { get; set; }

        public static string Language
        {
            get
            {
                var userLangs = HttpContext.Current.Request.Headers["Accept-Language"].ToString();
                var lang = userLangs.Split(',').FirstOrDefault();

                if (string.IsNullOrEmpty(lang))
                {
                    lang = "en-US";
                }
                return lang;
            }
        }
    }
}
