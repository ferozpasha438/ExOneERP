using CIN.Application;
using CIN.Application.Common;
using CIN.Application.PurchaseSetupDtos;
using CIN.Application.PurchaseSetupQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;


namespace LS.API.Purchase.Controllers
{
    public class purchasetermsController : BaseController
    {
        public purchasetermsController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetpotermList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetPoterm() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblPopDefVendorPOTermsCodeDto input)
        {
            //await Task.Delay(3000);

            var accBranch = await Mediator.Send(new CreatePurchaseterms() { Input = input, User = UserInfo() });
            if (accBranch.Id > 0)
            {
                return Created($"get/{accBranch.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = accBranch.Message });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var BranchId = await Mediator.Send(new DeletePoterm() { Id = id, User = UserInfo() });
            if (BranchId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        #region GetPurchaseTerm
        [HttpGet("GetPurchaseTerm")]
        public async Task<IActionResult> GetPurchaseTerm([FromQuery] string CatCode)
        {
            var obj = await Mediator.Send(new GetPurchaseTerm() { PotermCode = CatCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        #endregion

        //[HttpPost]
        //public async Task<ActionResult> Create([FromBody] TblPopDefVendorPOTermsCodeDto input)
        //{
        //    //await Task.Delay(3000);

        //    var accBranch = await Mediator.Send(new CreatePurchaseterms() { Input = input, User = UserInfo() });
        //    if (accBranch.Id > 0)
        //    {
        //        return Created($"get/{accBranch.Id}", input);
        //    }
        //    return BadRequest(new ApiMessageDto { Message = accBranch.Message });
        //}
    }
}
