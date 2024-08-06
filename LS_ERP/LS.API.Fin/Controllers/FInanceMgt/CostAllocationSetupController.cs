using CIN.Application;
using CIN.Application.FinanceMgtQuery;
using CIN.Application.FinPurchaseMgtQuery;
using CIN.Application.OperationsMgtQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LS.API.Fin.Controllers.FInanceMgt
{
    public class CostAllocationSetupController : BaseController
    {
        public CostAllocationSetupController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getCostAllocationSetupSelectList")]
        public async Task<IActionResult> GetCostAllocationSetupSelectList()
        {
            var obj = await Mediator.Send(new GetCostAllocationSetupSelectList() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpGet("getCostSegmentCodeSelectList/{id}")]
        public async Task<IActionResult> GetCostSegmentCodeSelectList([FromRoute] int id, [FromQuery] string search)
        {

            var obj = await Mediator.Send(new GetSingleCostAllocationSetup() { Id = id, User = UserInfo() });
            var costType = obj.CostType.ToLower();

            var result = costType switch
            {
                "customer" => await Mediator.Send(new GetLanCustomersCustomList() { Search = search, IsPayment = null, User = UserInfo() }),
                "vendor" => await Mediator.Send(new GetLanVendorsCustomList() { Search = search, IsPayment = null, User = UserInfo() }),
                "department" => await Mediator.Send(new GetLanDepartmentCustomList() { Search = search, User = UserInfo() }),
                "employee" => await Mediator.Send(new GetLanEmployeeCustomList() { Search = search, User = UserInfo() }),
                _ => new List<LanCustomSelectListItem>()
            };

            return obj is not null ? Ok(result) : NotFound(new ApiMessageDto
            {
                Message = ApiMessageInfo.NotFound
            });
        }


    }
}
