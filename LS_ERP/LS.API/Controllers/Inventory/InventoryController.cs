using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SystemQuery;
using CIN.Application.SystemSetupDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LS.API.Controllers
{
    //public class InventoryController : InventoryBaseController
    //{
    //    public InventoryController(IOptions<AppSettingsJson> appSettings) : base(appSettings)
    //    {
    //        ClaimSettings.WebAPIURL = appSettings.Value.InventoryApi;
    //    }

    //    [HttpGet("getTestList")]
    //    public async Task<IActionResult> GetTestList()
    //    {
    //        var data = await FactoryInstance.GetTestListAsync();
    //        if (data.Data is not null)
    //            return Ok(data.Data);
    //        return BadRequest(data.Message);
    //    }

    //}
}
