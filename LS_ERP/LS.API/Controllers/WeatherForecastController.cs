using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LS.API.Controllers
{

    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }


    ////[ApiController]
    ////[Route("[controller]")]
    ////public class WeatherForecastController : ApiControllerBase
    ////{
    ////    private readonly IStringLocalizer<ViewResources.Resource> _sharedLocalizer;
    ////    private static readonly string[] Summaries = new[]
    ////    {
    ////        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ////    };

    ////    private readonly ILogger<WeatherForecastController> _logger;
    ////    public WeatherForecastController(ILogger<WeatherForecastController> logger,
    ////        IStringLocalizer<ViewResources.Resource> sharedLocalizer)
    ////    {
    ////        _logger = logger;
    ////        _sharedLocalizer = sharedLocalizer;
    ////    }

    ////    [HttpGet]
    ////    public IEnumerable<object> Get()
    ////    //public IEnumerable<object> Get([FromQuery] TestClassDto input)
    ////    {
    ////        //if (ModelState.IsValid)
    ////        //{

    ////        //}

    ////        //var state = ModelState.ValidationState;
    ////        string Culture = HttpContext.Items["SelectedLng"]?.ToString() ?? "en-US";

    ////        var res = HttpContext.Request;
    ////        string AddAccount = _sharedLocalizer["CommonErrorMessage"];
    ////        //var list = await _context.Employees.ToListAsync();
    ////        var rng = new Random();
    ////        return Enumerable.Range(1, 25).Select(item => new
    ////        {
    ////            Date = DateTime.Now,
    ////            TemperatureC = rng.Next(-20, 55),
    ////            Summary = item.ToString() //Summaries[rng.Next(Summaries.Length)]
    ////        })
    ////        .ToArray();
    ////    }
    ////}


    ////public class TestClassDto
    ////{
    ////    //[Required(ErrorMessageResourceType = typeof(ViewResources.Resource), ErrorMessageResourceName = "Invoice_Validation_Required")]
    ////    [Required(ErrorMessageResourceType = typeof(ViewResources.Resource), ErrorMessageResourceName = "CommonErrorMessage")]
    ////    public string Name { get; set; }
    ////}

}
