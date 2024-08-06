using CIN.Application;
using CIN.Application.Common;
using CIN.Application.InvoiceDtos;
using CIN.Application.InvoiceQuery;
using CIN.Application.SystemQuery;
using CIN.Application.SystemSetupDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Fin.Controllers
{
    public class VatController : BaseController
    {
        public VatController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getSelectVatList")]
        public async Task<IActionResult> GetSelectProductList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetSelectVatList() { Input = search, User = UserInfo() });
            return Ok(obj);
        }


        [HttpGet("getSelectRatingList")]
        public async Task<IActionResult> GetSelectRatingList()
        {
            var obj = await Mediator.Send(new GetSelectRatingList() { });
            return Ok(obj);
        }


    }
}
