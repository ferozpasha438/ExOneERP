using CIN.Application;
using CIN.Application.InventoryDtos;
using CIN.Application.SystemQuery;
using CIN.DB;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.Invt.Controllers
{
    public class WeatherForecastController : BaseController
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private CINDBOneContext _context;

        public WeatherForecastController(IOptions<AppSettingsJson> appSettings, CINDBOneContext context) : base(appSettings)
        {
            _context = context;
        }

        [HttpGet("getTestList")]
        public async Task<IActionResult> GetTestList()
        {
            var list1 = await Mediator.Send(new GetPermissionUsers() { User = UserInfo() });
            var list = list1.Select(e => new TestDto { Id = 0, Email = e.Text, Name = e.Value }).ToList();
            return Ok(list);

            //return Ok(new List<TestDto> {
            //    new TestDto { Id = 1, Email = "1Email", Name = "1Name" },
            //    new TestDto { Id = 2, Email = "2Email", Name = "2Name" },
            //    new TestDto { Id = 3, Email = "3Email", Name = "3Name" },
            //});
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
}
