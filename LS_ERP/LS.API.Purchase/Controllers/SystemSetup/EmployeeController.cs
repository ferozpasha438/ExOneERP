using CIN.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace LS.API.Purchase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ApiControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly CINDBOneContext _userDbContext;
        private readonly IStringLocalizer<ViewResources.Resource> _sharedLocalizer;
        public EmployeeController(ILogger<EmployeeController> logger, CINDBOneContext userDbContext,
            IStringLocalizer<ViewResources.Resource> sharedLocalizer)
        {
            _logger = logger;
            _userDbContext = userDbContext;
            _sharedLocalizer = sharedLocalizer;
        }

        //[HttpGet]
        //public async Task<ActionResult> Get()//[FromQuery] PaginationFilterDto filter)
        //{

        //    var Companies = await Mediator.Send(new GetEmployeeList() { });
        //    //if (Companies.Count > 0)
        //    return Ok(new APIPayload<List<CINEmployeeDto>> { Data = Companies });// { Page = filter.Page, Query = filter.Query, OrderBy = filter.OrderBy, PageCount = filter.PageCount });
        //    //return NotFound("No Data");
        //}

        //[HttpGet]
        //public async Task<IEnumerable<object>> Get()
        //{

        //    //var list = await _userDbContext.Employees.ToListAsync();
        //    var rng = new Random();
        //    return list.Select(item => new
        //    {
        //        Date = DateTime.Now,
        //        TemperatureC = rng.Next(-20, 55),
        //        Summary = $"{item.Name} ({item.Email})"//Summaries[rng.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}
    }
}
