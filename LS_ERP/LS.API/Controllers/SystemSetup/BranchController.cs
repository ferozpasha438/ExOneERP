using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SystemQuery;
using CIN.Application.SystemSetupDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Controllers
{
    public class BranchController : BaseController
    {
        public BranchController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {

        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetBranchList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetBranch() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("getSelectSysBranchListByComId/{id}")]
        public async Task<IActionResult> GetSelectSysBranchList([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetSelectSysBranchListByComId() { Input = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("getSelectSysBranchList")]
        public async Task<IActionResult> GetSelectSysBranchList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetSelectSysBranchList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("getBranchByBranchCode")]
        public async Task<IActionResult> GetBranchByBranchCode([FromQuery] string branchCode)
        {
            var obj = await Mediator.Send(new GetBranchByBranchCode() { Input = branchCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSelectBranchCodeList")]
        public async Task<IActionResult> GetSelectBranchCodeList()
        {
            var obj = await Mediator.Send(new GetSelectBranchCodeList() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("checkBranchCode")]
        public async Task<IActionResult> CheckBranchCode([FromQuery] string branchCode)
        {
            var obj = await Mediator.Send(new CheckBranchCode() { Input = branchCode, User = UserInfo() });
            return Ok(obj);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblErpSysCompanyBranchDto input)
        {
            //await Task.Delay(3000);

            var branch = await Mediator.Send(new CreateBranch() { Input = input, User = UserInfo() });
            if (branch.BranchId > 0)
            {
                if (input.Id > 0)
                    return NoContent();
                else
                    return Created($"get/{branch.BranchId}", input);
            }
            return BadRequest(new ApiMessageDto { Message = branch.Message });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var BranchId = await Mediator.Send(new DeleteBranch() { Id = id, User = UserInfo() });
            if (BranchId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }


    }
}
