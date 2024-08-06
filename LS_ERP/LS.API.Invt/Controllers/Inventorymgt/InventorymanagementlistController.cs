using CIN.Application;
using CIN.Application.Common;
using CIN.Application.InventoryDtos;
using CIN.Application.InventoryQuery;
using CIN.Application.SystemQuery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Invt.Controllers.Inventory
{
    public class InventorymanagementlistController : BaseController
    {
        public InventorymanagementlistController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            //await Task.Delay(3000);
            var list = await Mediator.Send(new GetInventoryItemList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("GetSelectSubCategoryList")]
        public async Task<IActionResult> GetSelectSubCategoryList([FromQuery] string CategoryCode)
        {
            var obj = await Mediator.Send(new GetSelectSubCategoryList() { Code = CategoryCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("GetSelectClass")]
        public async Task<IActionResult> GetSelectClass([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetSelectClass() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetTaxSelectList")]
        public async Task<IActionResult> GetTaxSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetTaxList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetUOMSelectList")]
        public async Task<IActionResult> GetUOMSelectList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new GetUOMList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("getSelectSubClassList")]
        public async Task<IActionResult> getSelectSubClassList([FromQuery] string search)
        {
            var obj = await Mediator.Send(new getSelectSubClassList() { Input = search, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblErpInvItemMasterDto input)
        {
            //await Task.Delay(3000);

            var branch = await Mediator.Send(new CreateInventoryManagementList() { Input = input, User = UserInfo() });
            if (branch.Str != null)
            {
                if (input.Id > 0)
                    return NoContent();
                else
                    return branch.Str is not null ? Ok(branch.Str) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
                //return Created($"get/{branch.Item1}", input);
            }
            return BadRequest(new ApiMessageDto { Message = branch.Message });

            //if (branch.Id > 0)
            //{
            //    //input.ItemCode = branch.Str;
            //    if (input.Id > 0)
            //        return NoContent();
            //    else
            //        return Created($"get/{branch.Id}", input);
            //    //return branch.Str is not null ? Ok(branch.Str) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
            //}
            //if(branch.Message=="Failed")
            //{
            //    branch.Message = "Duplicate Records";
            //    return BadRequest(new ApiMessageDto { Message = branch.Message });
            //}
           
            //else
                return BadRequest(new ApiMessageDto { Message = branch.Message });


            //var branch = await Mediator.Send(new CreateInvtConfig() { Input = input, User = UserInfo() });
            //if (branch.InvconfigId > 0)
            //{
            //    if (input.Id > 0)
            //        return NoContent();
            //    else
            //        return Created($"get/{branch.InvconfigId}", input);
            //}
            //return BadRequest(new ApiMessageDto { Message = branch.msg });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetItemMaster() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var ClassId = await Mediator.Send(new DeleteItemMaster() { Id = id, User = UserInfo() });
            if (ClassId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
        #region UOMTab
        [HttpGet("GetUOMListByID")]
        public async Task<IActionResult> GetUOMListByID([FromQuery] string itemvalue)
        {
            var obj = await Mediator.Send(new GetUOMListByID() { Code = itemvalue, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpPost("CreateUomItem")]
        public async Task<ActionResult> CreateInventoryItem([FromBody] TblINVTblErpInvItemsUOMDto input)
        {
            var accBranch = await Mediator.Send(new CreateUOMItem() { Input = input, User = UserInfo() });
            if (accBranch.Id > 0)
            {
                return Created($"get/{accBranch.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = accBranch.Message });
        }
        #endregion 
        #region InventoryItemTab
        [HttpGet("GetInventoryListByID")]
        public async Task<IActionResult> GetInventoryListByID([FromQuery] string itemvalue)
        {
            var obj = await Mediator.Send(new GetInventoryListByID() { Code = itemvalue, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpPost("CreateInventoryItem")]
        public async Task<ActionResult> CreateInventoryItem([FromBody] TblInventoryItemsDto input)
        {
            var accBranch = await Mediator.Send(new CreateInventoryItem() { Input = input, User = UserInfo() });
            if (accBranch.Id > 0)
            {
                return Created($"get/{accBranch.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = accBranch.Message });
        }
        #endregion
        #region BarcodeTab
        [HttpPost("CreateBarcodeItem")]
        public async Task<ActionResult> CreateBarcodeItem([FromBody] TblBarcodeItemsDto input)
        {
            var accBranch = await Mediator.Send(new CreateBarcodeItem() { Input = input, User = UserInfo() });
            if (accBranch.Id > 0)
            {
                return Created($"get/{accBranch.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = accBranch.Message });
        }
        [HttpGet("GetBarcodeListByID")]
        public async Task<IActionResult> GetBarcodeListByID([FromQuery] string itemvalue)
        {
            var obj = await Mediator.Send(new GetBarcodeListByID() { Code = itemvalue, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("GetBarcode")]
        public async Task<IActionResult> GetBarcode([FromQuery] string Barcode)
        {
            var obj = await Mediator.Send(new GetBarcode() { Code = Barcode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        #endregion
        #region NotesTab
        [HttpPost("CreateNotesItem")]
        public async Task<ActionResult> CreateNotesItem([FromBody] TblNotesItemsDto input)
        {
            var accBranch = await Mediator.Send(new CreateNotesItem() { Input = input, User = UserInfo() });
            if (accBranch.Id > 0)
            {
                return Created($"get/{accBranch.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = accBranch.Message });
        }
        #endregion
        #region HistoryTab
        [HttpPost("CreateItemHistory")]
        public async Task<ActionResult> CreateItemHistory([FromBody] TblHistoryItemsDto input)
        {
            var accBranch = await Mediator.Send(new CreateItemHistory() { Input = input, User = UserInfo() });
            if (accBranch.Id > 0)
            {
                return Created($"get/{accBranch.Id}", input);
            }
            return BadRequest(new ApiMessageDto { Message = accBranch.Message });
        }
        #endregion
        #region  GenerateItemNumber
        [HttpGet("GenerateItemNumber")]
        public async Task<IActionResult> GenerateItemNumber([FromQuery] PaginationFilterDto filter)
        {
            var obj = await Mediator.Send(new GetItemGenerate() { Input = filter.Values(), User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        #endregion
        #region InventoryItem
        [HttpGet("GetInventoryItems")]
        public async Task<IActionResult> GetInventoryItems([FromQuery] string ItemCode, [FromQuery] string checkCode)
        {
            var obj = await Mediator.Send(new GetInventoryItem() { ItemCode = ItemCode, CheckCode = checkCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        #endregion
       
        [HttpGet("InventoryHistory")]
        public async Task<IActionResult> InventoryHistory([FromQuery] string ItemCodes)
        {
            var obj = await Mediator.Send(new InventoryHistory() { ItemCodes = ItemCodes, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
    }
}
