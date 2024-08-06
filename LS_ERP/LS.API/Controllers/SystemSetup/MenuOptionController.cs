using CIN.Application;
using CIN.Application.SystemQuery;
using CIN.Application.SystemSetupDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using static CIN.Application.SystemQuery.GetSideMenuOptionListHandler;

namespace LS.API.Controllers
{
    public class MenuOptionController : BaseController
    {
        public MenuOptionController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            var obj = await Mediator.Send(new GetPermissionMenuOptionList() { User = UserInfo() });
            return Ok(obj);
            //return Ok("{'ADMINISTRATION': {'System' : ['test', 'One']}}");
            //var obj = await Mediator.Send(new GetMenuOptionList() { Id = id, User = UserInfo() });
            //return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getAllMenuOptionListForPermissions")]
        public async Task<IActionResult> GetAllMenuOptionListForPermissions()
        {
            var obj = await Mediator.Send(new GetAllMenuOptionListForPermissions() { User = UserInfo() });
            return Ok(obj);
        }
        
        [HttpGet("getNgLinks")]
        public async Task<IActionResult> GetNgLinks()
        {
            var obj = await Mediator.Send(new GetNgLinks() { User = UserInfo() });
            return Ok(obj);
        }

        [HttpGet("getUserWiseMenuCodes/{id:int}")]
        public async Task<IActionResult> GetUserWiseMenuCodes(int id)
        {
            var obj = await Mediator.Send(new GetUserWiseMenuCodes() { UserId = id, User = UserInfo() });
            return Ok(obj);
        }


        [HttpGet("getSideMenuOptionList")]
        public async Task<IActionResult> GetSideMenuOptionList()
        {
            var obj = await Mediator.Send(new GetSideMenuOptionList() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("getPermissionUsers")]
        public async Task<IActionResult> GetPermissionUsers()
        {
            var obj = await Mediator.Send(new GetPermissionUsers() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        
        [HttpGet("getAllUsersList")]
        public async Task<IActionResult> GetAllUsersList()
        {
            var obj = await Mediator.Send(new GetAllUsersList() { User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        
        [HttpGet("getUsersByBranchCode")]
        public async Task<IActionResult> GetUsersByBranchCode([FromQuery]string branchCode)
        {
            var obj = await Mediator.Send(new GetUsersByBranchCode() { BranchCode = branchCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("getUserMenuSubLink/{id:int}")]
        public async Task<IActionResult> GetUserMenuSubLink(int id)
        {
            var obj = await Mediator.Send(new GetUserMenuSubLink() { UserId = id });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] MenuItemFlatNodeListDto input)
        {
            //await Task.Delay(3000);

            var userId = await Mediator.Send(new CreateMenuOption() { MenuNodeList = input });
            if (userId > 0)
            {
                return Ok(new ApiMessageDto { Message = ApiMessageInfo.Success });
            }
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }


    }
}
