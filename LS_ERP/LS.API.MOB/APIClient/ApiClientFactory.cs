using System;
using System.Threading;

namespace LS.API
{
    public class ApiClientFactory
    {
        private static Uri apiUri;

        private static Lazy<ApiClient> restClient = new Lazy<ApiClient>(
          () => new ApiClient(apiUri), LazyThreadSafetyMode.ExecutionAndPublication);

        static ApiClientFactory()
        {
            apiUri = new Uri(ClaimSettings.WebAPIURL);
        }
        public static ApiClient APILoginURL(string loginAPiURL)
        {
            apiUri = new Uri(loginAPiURL);
            return restClient.Value;
        }

        public static ApiClient Instance
        {
            get
            {
                return restClient.Value;
            }
        }
    }
}
