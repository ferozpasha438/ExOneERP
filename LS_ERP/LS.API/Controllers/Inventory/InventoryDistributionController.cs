using CIN.Application;
using CIN.Application.Common;
using CIN.Application.InventoryDtos;
using CIN.Application.InventoryQuery;
using CIN.Application.SystemQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Controllers
{
    public class InventoryDistributionController : BaseController
    {
        public InventoryDistributionController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }



        #region Purchase Distribution



        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetInventoryDistributionList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblInvDefDistributionGroupDto input)
        {

            var invDist = await Mediator.Send(new CreateInventoryDistribution() { Input = input, User = UserInfo() });
            if (invDist.Id > 0)
            {
                if (input.Id > 0)
                    return NoContent();
                else
                    return Created($"get/{invDist.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = invDist.Message });
        }

        #endregion


        #region Inventory Distribution

        [HttpGet("getInventoryPoDistributionList")]
        public async Task<IActionResult> GetInventoryPoDistributionList([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetInventoryPoDistributionList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }


        [HttpPost("createInventoryPoDistribution")]
        public async Task<ActionResult> CreateInventoryPoDistribution([FromBody] TblInvDefDistributionGroupDto input)
        {

            var invDist = await Mediator.Send(new CreateInventoryPoDistribution() { Input = input, User = UserInfo() });
            if (invDist.Id > 0)
            {
                if (input.Id > 0)
                    return NoContent();
                else
                    return Created($"get/{invDist.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = invDist.Message });
        }

        #endregion
    }
}
