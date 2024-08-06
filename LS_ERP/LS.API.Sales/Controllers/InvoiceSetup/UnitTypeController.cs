using CIN.Application;
using CIN.Application.Common;
using CIN.Application.InvoiceDtos;
using CIN.Application.InvoiceQuery;
using CIN.Application.SystemQuery;
using CIN.Application.SystemSetupDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LS.API.Sales.Controllers.InvoiceSetup
{
    public class UnitTypeController : BaseController
    {
        public UnitTypeController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var UnitTypeDetails = await Mediator.Send(new GetUnitTypeList() { Input = filter.Values(), User = UserInfo() });
            return Ok(UnitTypeDetails );
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var UnitTypeDetail = await Mediator.Send(new GetUnitType() { UnitTypeId = id, User = UserInfo() });
            return Ok(UnitTypeDetail ?? new UnitTypeDTO());
        }
        [HttpGet("GetForCompany")]
        public async Task<ActionResult> GetForCompany()
        {
            var UnitTypeDetail = await Mediator.Send(new GetUnitTypeCompnayList() { User = UserInfo() });
            return Ok(UnitTypeDetail );
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] UnitTypeDTO dTO)
        {

            dTO.CompanyId = CompanyId;
            long UnitTypeId = await Mediator.Send(new CreateUnitTypes() { UnitTypeDTO = dTO, User = UserInfo() });
            if (UnitTypeId > 0)
                return Created($"get/{UnitTypeId}", dTO);
            else if (UnitTypeId == -1)
                return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Duplicate("Name") });

            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }
    }
}
