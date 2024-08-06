using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SystemQuery;
using CIN.Application.SystemSetupDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Fin.Controllers
{
    public class CityController : BaseController
    {
        public CityController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        {
            var list = await Mediator.Send(new GetCityPagedList() { Input = filter, User = UserInfo() });
            return Ok(list);
        }

        //[HttpGet]
        //public async Task<IActionResult> Get([FromQuery] PaginationFilterDto filter)
        //{
        //    var list = await Mediator.Send(new GetCityList() { Input = filter, User = UserInfo() });
        //    return Ok(list);
        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var obj = await Mediator.Send(new GetCity() { Id = id, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("cityfilter")]
        public async Task<IActionResult> Cityfilter([FromQuery] string city)
        {
            var obj = await Mediator.Send(new GetSelectList() { City = city, User = UserInfo() });
            return obj is not null ? Ok(obj) : NotFound(new ApiMessageDto { Message = ApiMessageInfo.NotFound });
        }

        [HttpGet("getSelectList")]
        public async Task<IActionResult> GetSelectList()
        {
            var list = await Mediator.Send(new GetSelectList() { User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getCitiesSelectList")]
        public async Task<IActionResult> GetCitiesSelectList()
        {
            var list = await Mediator.Send(new GetSelectCityList() { User = UserInfo() });
            return Ok(list);
        }
        [HttpGet("getSelectStateList")]
        public async Task<IActionResult> GetSelectStateList()
        {
            var list = await Mediator.Send(new GetSelectStateList() { User = UserInfo() });
            return Ok(list);
        }


        [HttpGet("getStatebyCityId/{code}")]
        public async Task<IActionResult> GetStatebyCityId(string code)
        {
            var list = await Mediator.Send(new GetStatebyCityId() { Input = code, User = UserInfo() });
            return Ok(list);
        }
        [HttpGet("GetCountrybyStateCode/{code}")]
        public async Task<IActionResult> GetCountrybyStateCode(string code)
        {
            var list = await Mediator.Send(new GetStatebyCityId() { Input = code, User = UserInfo() });
            return Ok(list);
        }

        [HttpGet("getStateCountrybyCityCode/{cityCode}")]
        public async Task<IActionResult> GetStateCountrybyCityCode(string cityCode)
        {
            var list = await Mediator.Send(new GetStateCountrybyCityCode() { CityCode = cityCode, User = UserInfo() });
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TblErpSysCityCodeDto dTO)
        {
            var cityId = await Mediator.Send(new CreateCity() { CityDto = dTO, User = UserInfo() });
            if (cityId > 0)
                return Created($"get/{cityId}", dTO);
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var cityId = await Mediator.Send(new DeleteCity() { Id = id, User = UserInfo() });
            if (cityId > 0)
                return Ok(ApiMessageInfo.Success);
            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });
        }


    }
}
