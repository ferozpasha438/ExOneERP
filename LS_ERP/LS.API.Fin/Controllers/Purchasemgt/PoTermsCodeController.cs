using CIN.Application;
using CIN.Application.FinPurchaseMgtQuery;
using CIN.Application.PurchaseSetupQuery;
using CIN.Application.SalesSetupQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Fin.Controllers
{
    public class PoTermsCodeController : BaseController
    {
        public PoTermsCodeController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        //[HttpGet("getSelectPoTermsCodeList")]
        //public async Task<IActionResult> GetSelectPoTermsCodeList()
        //{
        //    var obj = await Mediator.Send(new GetSelectPoTermsCodeListOne() { User = UserInfo() });
        //    return Ok(obj);
        //}

        [HttpGet("getSelectPoTermsCodeList")]
        public async Task<IActionResult> GetSelectPoTermsCodeList(string search)
        {
            var item = await Mediator.Send(new GetSelectPoTermsCodeList() { Input = search, User = UserInfo() });
            return Ok(item);
        }
        [HttpGet("getPoTermsByTermsCode/{salesTermsCode}")]
        public async Task<IActionResult> Get([FromRoute] string salesTermsCode)
        {
            var obj = await Mediator.Send(new GetPoTermsByTermsCode() { PoTermsCode = salesTermsCode, User = UserInfo() });
            return Ok(obj);
        }

        [HttpGet("getCustomSelectPoTermsCodeList")]
        public async Task<IActionResult> GetCustomSelectSalesTermsCodeList()
        {
            var obj = await Mediator.Send(new GetCustomSelectPoTermsCodeList() { User = UserInfo() });
            return Ok(obj);
        }
    }
}
