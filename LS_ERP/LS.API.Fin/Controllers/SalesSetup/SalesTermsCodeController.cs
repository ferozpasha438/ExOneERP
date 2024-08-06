using CIN.Application;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.SalesSetupQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Fin.Controllers
{
    public class SalesTermsCodeController : BaseController
    {
        public SalesTermsCodeController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getSelectSalesTermsCodeList")]
        public async Task<IActionResult> GetSelectSalesTermsCodeList(string search)
        {
            var item = await Mediator.Send(new CIN.Application.OperationsMgtQuery.GetSelectSalesTermsCodeList() { Input = search, User = UserInfo() });
            return Ok(item);
        }

        [HttpGet("getSalesTermsByTermsCode/{salesTermsCode}")]
        public async Task<TblSndDefSalesTermsCodeDto> Get([FromRoute] string salesTermsCode)
        {
            var obj = await Mediator.Send(new CIN.Application.OperationsMgtQuery.GetSalesTermsByTermsCode() { SalesTermsCode = salesTermsCode, User = UserInfo() });
            return obj;
        }


        [HttpGet("getCustomSelectSalesTermsCodeList")]
        public async Task<IActionResult> GetCustomSelectSalesTermsCodeList()
        {
            var obj = await Mediator.Send(new GetCustomSelectSalesTermsCodeList() { User = UserInfo() });
            return Ok(obj);
        }
    }
}
