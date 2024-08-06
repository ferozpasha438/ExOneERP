using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDto;
using CIN.Application.SchoolMgtQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.SM.Controllers.Admin_Setups
{
    //public class StudentDetailsController : BaseController
    //{
    //    private readonly IConfiguration _Config;
       
    //    public StudentDetailsController(IOptions<AppSettingsJson> appSettings, IConfiguration config) : base(appSettings)
    //    {
    //        _Config = config;
    //           //_cinDbContext = cinDbContext;
    //    }


    //    //[HttpGet("getStudentDetailsList")]
    //    //public async Task<IActionResult> Get()
    //    //{

    //    //    var list = await Mediator.Send(new GetStudentDetailsList() { User = UserInfo() });
    //    //    return Ok(list);
    //    //}
    //}
}
