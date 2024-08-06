using CIN.Application;
using CIN.Application.Common;
using CIN.Application.FinanceMgtDtos;
using CIN.Application.FinanceMgtQuery;
using CIN.Application.SystemSetupDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Controllers
{
    public class TaxesController : BaseController
    {
        public TaxesController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            //await Task.Delay(3000);
            var obj = await Mediator.Send(new GetFinTax() { Id = id, User = UserInfo() });
            return Ok(obj);
        }

        [HttpGet("getAllTaxList")]
        public async Task<IActionResult> GetAllTaxList()
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetAllTaxList() { User = UserInfo() });
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblErpSysSystemTaxDto input)
        {
            //await Task.Delay(3000);

            var tax = await Mediator.Send(new CreateFinTax() { Input = input, User = UserInfo() });
            if (tax.Id > 0)
            {
                return Created($"get/{tax.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = tax.Message });
        }
    }
}
