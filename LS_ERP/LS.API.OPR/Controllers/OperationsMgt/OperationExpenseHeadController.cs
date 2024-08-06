using CIN.Application;
using CIN.Application.Common;
using CIN.Application.OperationsMgtDtos;
using CIN.Application.OperationsMgtQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.OPR.Controllers.OperationsMgt
{
    public class OperationExpenseHeadController : BaseController
    {

        public OperationExpenseHeadController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet("getOperationExpenseHeadsPagedList")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {

            var list = await Mediator.Send(new GetOperationExpenseHeadsPagedList() { Input = filter.Values(), User = UserInfo() });
            return Ok(list);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblOpOperationExpenseHeadDto dTO)
        {
            var id = await Mediator.Send(new CreateOperationExpenseHead() { OperationExpenseHeadDto = dTO, User = UserInfo() });
            if (id > 0)
            {
                return Created($"get/{id}", dTO);
            }
            else if (id == -1)
            {
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate(nameof(dTO.CostHead)) });
            }
           return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }

        [HttpGet("getOperationExpenseHeadById/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetOperationExpenseHeadById() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getOperationExpenseHeadByCode/{OperationExpenseHeadCode}")]
        public async Task<IActionResult> Get([FromRoute] string OperationExpenseHeadCode)
        {
            var obj = await Mediator.Send(new GetOperationExpenseHeadByCode() { OperationExpenseHeadCode = OperationExpenseHeadCode, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }
        [HttpGet("getOperationExpenseHeadsForResources")]
        public async Task<List<TblOpOperationExpenseHeadDto>> GetOperationExpenseHeadsForResources()
        {
            var obj = await Mediator.Send(new GetOperationExpenseHeadsForResources() { User = UserInfo() });
            return obj;
        }

        [HttpGet("getOperationExpenseHeadsForLogistics")]
        public async Task<List<TblOpOperationExpenseHeadDto>> GetOperationExpenseHeadsForLogistics()
        {
            var obj = await Mediator.Send(new GetOperationExpenseHeadsForLogistics() { User = UserInfo() });
            return obj;
        }
[HttpGet("getOperationExpenseHeadsForMaterialEquipment")]
        public async Task<List<TblOpOperationExpenseHeadDto>> GetOperationExpenseHeadsForMaterialEquipment()
        {
            var obj = await Mediator.Send(new GetOperationExpenseHeadsForMaterialEquipment() { User = UserInfo() });
            return obj;
        }
[HttpGet("getOperationExpenseHeadsForFinancialExpence")]
        public async Task<List<TblOpOperationExpenseHeadDto>> GetOperationExpenseHeadsForFinancialExpence()
        {
            var obj = await Mediator.Send(new GetOperationExpenseHeadsForFinancialExpence() { User = UserInfo() });
            return obj;
        }

        [HttpGet("getAutoSelectListForFinancialExpense")]
        public async Task<IActionResult> GetAutoSelectListForFinancialExpense(string search)
        {
            var items = await Mediator.Send(new GetAutoSelectListForFinancialExpense() { search = search, User = UserInfo() });
            return Ok(items);
        }

        [HttpGet("getSelectOperationExpenseHeadList")]
        public async Task<IActionResult> GetSelectOperationExpenseHeadList()
        {
            var item = await Mediator.Send(new GetSelectOperationExpenseHeadList() { User = UserInfo() });
            return Ok(item);
        }

        [HttpGet("getOperationExpenseHeadCodes")]
        public async Task<IActionResult> GetOperationExpenseHeadCodes()
        {
            var item = await Mediator.Send(new GetOperationExpenseHeadCodes() { User = UserInfo() });
            return Ok(item);
        }


        [HttpGet("isExistCode/{Code}")]
        public async Task<bool> IsExistCode([FromRoute] string Code)
        {
            var obj = await Mediator.Send(new GetOperationExpenseHeadByCode() { OperationExpenseHeadCode = Code, User = UserInfo() });
            return obj is not null ? true : false;
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var OperationExpenseHeadId = await Mediator.Send(new DeleteOperationExpenseHead() { Id = id, User = UserInfo() });
            if (OperationExpenseHeadId > 0)
                return NoContent();
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}
