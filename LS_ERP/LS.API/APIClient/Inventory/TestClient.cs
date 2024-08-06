using CIN.Application.InventoryDtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.API
{
    public partial class ApiClient
    {
        public async Task<APIMessageDto<List<TestDto>>> GetTestListAsync()
        {
            var requestUrl = CreateRequestUri(string.Format(CultureInfo.InvariantCulture, "WeatherForecast/getTestList"));
            var response = await GetAsync<List<TestDto>>(requestUrl, null);
            return response;
        }
    }
}
