//using System.Collections.Generic;
//using System.Globalization;
//using System.Threading.Tasks;

//namespace LS.API.APIClient
//{
//    public partial class ApiClient
//    {
//        public async Task<(APIMessage<List<BankDetailDTO>> Data, string Message)> GetBankListAsync()
//        {
//            var requestUrl = CreateRequestUri(string.Format(CultureInfo.InvariantCulture, "Bank"));
//            var response = await GetAsync<List<BankDetailDTO>>(requestUrl, null);
//            return (response.Data, response.Message);
//        }
//        public async Task<(APIMessage<BankDetailDTO> Data, string Message)> GetCompanyBankListAsync()
//        {
//            var requestUrl = CreateRequestUri(string.Format(CultureInfo.InvariantCulture, "Bank/GetForCompany"));
//            var response = await GetAsync<BankDetailDTO>(requestUrl, null);
//            return (response.Data, response.Message);
//        }
//        public async Task<(APIMessage<BankDetailDTO> Data, string Message)> GetBankDetailAsync(int accountId)
//        {
//            var requestUrl = CreateRequestUri(string.Format(CultureInfo.InvariantCulture, $"Bank/{accountId}"));
//            var response = await GetAsync<BankDetailDTO>(requestUrl, null);
//            return (response.Data, response.Message);
//        }
//        public async Task<(APIMessage<int?> Data, string Message)> CreateBankDetailAsync(BankDetailDTO input)
//        {
//            var requestUrl = CreateRequestUri(string.Format(CultureInfo.InvariantCulture, $"Bank"));
//            var response = await PostAsync<int?, BankDetailDTO>(requestUrl, input);
//            return (response.Data, response.Message);
//        }
//    }
//}
