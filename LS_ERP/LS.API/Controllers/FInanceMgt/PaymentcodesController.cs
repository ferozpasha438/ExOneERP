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
    public class PaymentcodesController : BaseController
    {
        public PaymentcodesController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("payCodelList")]
        public async Task<IActionResult> PayCodelList([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetAccountlPaycodesList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string payCode)
        {
            var obj = await Mediator.Send(new GetPayCode() { PayCode = payCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getPayCodeById/{id:int}")]
        public async Task<IActionResult> GetPayCodeById([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetPayCodeById() { PayCode = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSelectPaymentCodeList")]
        public async Task<IActionResult> GetSelectPaymentCodeList()
        {
            var obj = await Mediator.Send(new GetSelectPaymentCodeList() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("getSelectPaymentTypeList")]
        public IActionResult GetSelectPaymentTypeList()
        {
            var obj = EnumData.GetPayCodeTypeEnum();
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] FinDefBankPayCodeCheckLeavesDto input)
        {
            //await Task.Delay(3000);

            var payCode = await Mediator.Send(new CreateAccountBranchMapping() { Input = input, User = UserInfo() });
            if (payCode.Id > 0)
            {
                return Created($"get/{payCode.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = payCode.Message });
        }
    }
}
