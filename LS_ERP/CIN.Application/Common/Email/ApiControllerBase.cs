using Microsoft.Extensions.Options;

namespace CIN.Application.Common.Email
{
    public class ApiControllerBase
    {
        private IOptions<AppSettingsJson> appSettings;

        public ApiControllerBase(IOptions<AppSettingsJson> appSettings)
        {
            this.appSettings = appSettings;
        }
    }
}